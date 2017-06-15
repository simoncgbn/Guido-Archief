using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using GuidoStock.App_Code;
using GuidoStock.Code;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Unit = GuidoStock.Code.Unit;

namespace GuidoStock.Scannen
{
    public partial class EventScannen : System.Web.UI.Page
    {
        DBClass db = new DBClass();

        public Checklist Checklist
        {
            get { return (Checklist) ViewState["Checklist"]; }
            set { ViewState["Checklist"] = value; }
        }

        public List<ChecklistLijn> SelectedStocks
        {
            get { return (List<ChecklistLijn>)ViewState["SelectedStocks"]; }
            set { ViewState["SelectedStocks"] = value; }
        }

        public ChecklistLijn SelectedLijn
        {
            get { return (ChecklistLijn)ViewState["SelectedLijn"]; }
            set { ViewState["SelectedLijn"] = value; }
        }

        private List<Code.Unit> Units
        {
            get { return (List<Code.Unit>)ViewState["Units"]; }
            set { ViewState["Units"] = value; }
        }
        private Code.Unit unit
        {
            get { return (Code.Unit)ViewState["unit"]; }
            set { ViewState["unit"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            Units = db.GetAllUnits();
          
            var id = Request.QueryString["id"];
            var isEvent = Request.QueryString["isevent"];
            var orderId = Request.QueryString["orderid"];

            if (orderId != null)
            {
                db.UpdateOrderComplete(Checklist.Model.Id);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertUser2", "showSuccesMsgOrder(" + Checklist.Model.Id + ")", true);
            }

            int modelId;
            if (string.IsNullOrEmpty(id)) return;
            if (!int.TryParse(id, out modelId)) return;
            if (string.IsNullOrEmpty(isEvent)) return;
            Checklist = new Checklist(isEvent, modelId);
            BindGridView();
        }

        protected void hdnBarcode_OnValueChanged(object sender, EventArgs e)
        {
            var barcode = hdnBarcode.Value;
            // Fetch artikel uit lijst
            unit = Units.FirstOrDefault(x => x.Barcode == barcode);
            if (unit == null)
            {
                // Foutieve barcode
                DisplayBarcodeError();
                return;
            }
            SelectedStocks = Checklist.ChecklistLijnen
                .Where(x => x.Stock.Artikel.Id == unit.ArtikelId && (IsAncestor(x.Stock.Unit, unit) || x.Stock.Unit.Id == unit.Id ))
                .OrderByDescending(x => x.Stock.Unit.Id == unit.Id) // Gescande stock eerst
                .ThenBy(x => x.Stock.Unit.Aantal) // Daarna op volgorde van grootte
                .ToList();
            if (SelectedStocks.Count == 0)
            {
                // Artikel niet in checklist
                DisplayBarcodeError();
                return;
            }
            // Locatievervaldatum opvullen
            foreach (var x in SelectedStocks)
            {
                if (x.Stock.Unit.Aantal != 1)
                {
                    if (x.Stock.Vervaldatum.ToShortDateString() != DateTime.MaxValue.ToShortDateString())
                    {
                        x.Stock.LocatieVervaldatum =
                            "[" + x.Stock.Unit.NaamEnkelvoud + "] " + x.Stock.ArtikelLocatie.Code + " - " +
                            x.Stock.Vervaldatum.ToShortDateString();
                    }
                    else
                    {
                        x.Stock.LocatieVervaldatum =
                            "[" + x.Stock.Unit.NaamEnkelvoud + "] " + x.Stock.ArtikelLocatie.Code;
                    }
                }
                else
                {
                    if (x.Stock.Vervaldatum.ToShortDateString() != DateTime.MaxValue.ToShortDateString())
                    {
                        x.Stock.LocatieVervaldatum =
                           x.Stock.ArtikelLocatie.Code + " - " +
                            x.Stock.Vervaldatum.ToShortDateString();
                    }
                    else
                    {
                        x.Stock.LocatieVervaldatum = x.Stock.ArtikelLocatie.Code;
                    }
                }
            }
            // Stocks geselecteerd -> locatie opvullen
            Locatie.DataSource = SelectedStocks;
            Locatie.DataBind();
            SelectedLijn = SelectedStocks[0];
            Aantal.Text = ((SelectedLijn.Aantal - SelectedLijn.AantalWeg) / (SelectedLijn.Stock.Unit.Aantal == 0 ? 1 : SelectedLijn.Stock.Unit.Aantal)).ToString();
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "fillLocatie", "fillLocatie(" + Aantal.Text + ");", true);
            PrepareUI();
        }

        protected void hdnLocatie_OnValueChanged(object sender, EventArgs e)
        {
            var locatie = hdnLocatie.Value;
            SelectedLijn = Checklist.ChecklistLijnen.FirstOrDefault(x => x.Id == int.Parse(locatie));
            Aantal.Text = ((SelectedLijn.Aantal - SelectedLijn.AantalWeg)/(SelectedLijn.Stock.Unit.Aantal == 0 ? 1 : SelectedLijn.Stock.Unit.Aantal) ).ToString();
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "fillLocatie2", "fillLocatie(" + Aantal.Text + ");", true);
        }

        protected void CheckUit_OnClick(object sender, EventArgs e)
        {
            // Nieuw aantal ophalen
            int nieuwAantal = int.Parse(Aantal.Text);
            Checklist.AddOrUpdateAltered(db);
            nieuwAantal *= SelectedLijn.Stock.Unit.Aantal;
            var test = nieuwAantal;
            nieuwAantal += SelectedLijn.AantalWeg;
            // Set aantal
            var stocks = db.GetAllStocks();
            Checklist.ChecklistLijnen.ForEach(lijn => lijn.Stock = stocks.Find(s => s.Unit.Id == lijn.Stock.Unit.Id && s.Vervaldatum == lijn.Stock.Vervaldatum && s.ArtikelLocatie.Id == lijn.Stock.ArtikelLocatie.Id));
            Checklist.ChecklistLijnen
                .FirstOrDefault(x => x.Id == SelectedLijn.Id)
                .AantalWeg = nieuwAantal;
            SelectedLijn.AantalWeg = nieuwAantal;
            // Add or Update geselecteerde lijn
            List<ChecklistLijn> t = db.GetChecklistlijnen(SelectedLijn.Model);
            if (t.Count == 0)
            {
                db.AddChecklistLijnen(Checklist.ChecklistLijnen);
                // Schrijf stocks weg naar stockscopy
                var checklistStocks = Checklist.ChecklistLijnen.Where(x => !x.Stock.Artikel.IsHerbruikbaar).Select(ln => new ChecklistStock(Checklist, ln)).ToList();
                if(checklistStocks.Count != 0) db.InsertChecklistStocks(checklistStocks, Checklist.Model.Id);
            }
            else
            {
                db.UpdateChecklistLijn(SelectedLijn.Model, SelectedLijn.Stock.Id, nieuwAantal, SelectedLijn.Stock.Unit.Id);
                // Update stockscopy
                if(!SelectedLijn.Stock.Artikel.IsHerbruikbaar) db.UpdateChecklistStocks(new ChecklistStock(Checklist, SelectedLijn), Checklist.Model.Id);
            }

            if (SelectedLijn.Stock.Artikel.IsHerbruikbaar)
            {
                db.UpdateStock(SelectedLijn.Stock.Id, SelectedLijn.Stock.Vervaldatum, SelectedLijn.Stock.Aantal - test, SelectedLijn.Stock.ArtikelLocatie.Id, SelectedLijn.Stock.Artikel.Id, SelectedLijn.Stock.Unit.Id);
            }

            BindGridView();
            ResetUI();
        }

        private void BindGridView()
        {
            // Uitgecheckte lijnen niet weergeven
            Checklist.ChecklistLijnen = Checklist.ChecklistLijnen.Where(x => x.Aantal != x.AantalWeg).ToList();
            // Check of alles is ingescand
            if (Checklist.ChecklistLijnen.Count == 0)
            {
                // Update setcomplete
                var t = "";
                if (Checklist.Model is Evenement)
                {
                    db.UpdateEvenementChecklistComplete(Checklist.Model.Id);
                    t = "showSuccesMsg";
                }
                else
                {
                    db.UpdateOrderChecklistComplete(Checklist.Model.Id);
                    t = "showSuccesMsgOrder";
                    if (!Checklist.Model.IsVerhuur)
                    {
                        db.UpdateOrderComplete(Checklist.Model.Id);
                    }
                }
                UpdateLog();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "showSucces", t+"("+Checklist.Model.Id+");", true);
                return;
            }
            // Id toevoegen
            for (var i = 0; i < Checklist.ChecklistLijnen.Count; i++)
            {
                Checklist.ChecklistLijnen[i].Id = i;
            }
            ChecklistGridView.DataSource = Checklist.ChecklistLijnen;
            ChecklistGridView.DataBind();
        }

        private void PrepareUI()
        {
            CheckUit.Enabled = true;
            CheckUit.CssClass = CheckUit.CssClass.Replace("CheckUitDisabled", "test");
            Aantal.ReadOnly = false;
            Aantal.Enabled = true;
            Locatie.Enabled = true;
            ErrorBarcode.Attributes.Add("style", "display:none");
        }

        private bool IsAncestor(Code.Unit unit, Code.Unit target)
        {
            while (true)
            {
                if (unit.ChildUnitId == 0) return false;
                if (unit.ChildUnitId == target.Id)
                    return true;
                unit = Units.Find(u => u.Id == unit.ChildUnitId);
            }
        }

        private void DisplayBarcodeError()
        {
            Aantal.Text = "";
            Locatie.Items.Clear();
            CheckUit.Enabled = false;
            Locatie.Enabled = false;
            Aantal.ReadOnly = true;
            Aantal.Enabled = false;
            ErrorBarcode.Attributes.Add("style", "display:inline-block");
            CheckUit.CssClass = CheckUit.CssClass.Replace("test", "CheckUitDisabled");
            hdnBarcode.Value = "";
        }
        private void ResetUI()
        {
            CheckUit.Enabled = false;
            CheckUit.CssClass = CheckUit.CssClass.Replace("test", "CheckUitDisabled");
            Aantal.ReadOnly = true;
            Aantal.Enabled = false;
            ErrorBarcode.Attributes.Add("style", "display:none");
            Locatie.DataSource = new List<Object>();
            Locatie.DataBind();
            Aantal.Text = "";
            Barcode.Text = "";
            hdnBarcode.Value = "";
        }

        protected void UpdateLog()
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var currentUser = manager.FindByName(HttpContext.Current.User.Identity.Name);
            db.AddWijziging(Checklist.Model.Id, DateTime.Now, currentUser.Id, "NChecklist");
        }

        protected void GaDoor_OnClick(object sender, EventArgs e)
        {
            List<ChecklistLijn> t = db.GetChecklistlijnen(Checklist.Model);
            if (t.Count == 0)
            {
                Response.Redirect("Event.aspx");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "showWarning", "showWarning(" + Checklist.Model.Id + ");", true);
            }
        }

        protected void hdnGaDoorKeuze_OnValueChanged(object sender, EventArgs e)
        {
            if (hdnGaDoorKeuze.Value == "opslaan")
            {
                // Pas checklist aan met nieuwe aantallen
                foreach (var ln in Checklist.ChecklistLijnen)
                {
                    ln.Aantal -= ln.Aantal - ln.AantalWeg;
                    db.UpdateChecklistLijnAantal(Checklist.Model, ln.Stock.Id, ln.Aantal, ln.Stock.Unit.Id);
                }
                // Update eventcomplete
                if (Checklist.Model is Evenement)
                {
                    db.UpdateEvenementChecklistComplete(Checklist.Model.Id);
                }
                else
                {
                    db.UpdateOrderChecklistComplete(Checklist.Model.Id);
                }
                UpdateLog();
            }
            hdnGaDoorKeuze.Value = "";
            Response.Redirect("Event.aspx");
        }
    }
}