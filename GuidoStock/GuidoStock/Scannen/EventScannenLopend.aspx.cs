using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using ClosedXML.Excel;
using GuidoStock.App_Code;
using GuidoStock.Code;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace GuidoStock.Scannen
{
    public partial class EventScannenLopend : Page
    {
        private readonly DBClass db = new DBClass();

        public Checklist Checklist
        {
            get { return (Checklist)ViewState["Checklist"]; }
            set { ViewState["Checklist"] = value; }
        }

        public Checklist MergedChecklist
        {
            get { return (Checklist)ViewState["MergedChecklist"]; }
            set { ViewState["MergedChecklist"] = value; }
        }

        public List<ChecklistLijn> SelectedStocks
        {
            get { return (List<ChecklistLijn>)ViewState["SelectedStocks"]; }
            set { ViewState["SelectedStocks"] = value; }
        }

        public List<ChecklistStock> ChecklistStocks
        {
            get { return (List<ChecklistStock>)ViewState["ChecklistStocks"]; }
            set { ViewState["ChecklistStocks"] = value; }
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

            int modelId;
            if (string.IsNullOrEmpty(id)) return;
            if (!int.TryParse(id, out modelId)) return;
            if (string.IsNullOrEmpty(isEvent)) return;
            Checklist = new Checklist(isEvent, modelId);
            // Merge lijnen
            MergedChecklist = new Checklist(isEvent, modelId);
            MergedChecklist.ChecklistLijnen = (from ol in MergedChecklist.ChecklistLijnen
                group ol by new {ol.Stock.Artikel.Id}
                into grp
                select new ChecklistLijn
                {
                    Model = grp.Select(x => x.Model).FirstOrDefault(),
                    Stock = grp.Select(x => x.Stock).FirstOrDefault(x => { x.Unit.Aantal = 1;
                        return true;
                    }),
                    Aantal = grp.Sum(x => x.Aantal),
                    AantalWeg = grp.Sum(x => x.AantalWeg),
                    ChecklistComplete = grp.Select(x => x.ChecklistComplete).FirstOrDefault()
                }
            ).ToList();

            ChecklistStocks = db.GetChecklistStocksByEventId(Checklist.Model.Id);
            foreach (var s in ChecklistStocks)
            {
                if (s.StockId == 0)
                {
                    foreach (var ln in MergedChecklist.ChecklistLijnen)
                    {
                        if (ln.Stock.Artikel.Id == s.ArtikelId && MergedChecklist.Model.Id == s.EventId && MergedChecklist.Model is Evenement == s.IsEvent)
                        {
                            ln.AantalWeg -= s.Aantal;
                        }
                    }
                } 
            }
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
                .Where(x => x.Stock.Artikel.Id == unit.ArtikelId && (IsAncestor(x.Stock.Unit, unit) || x.Stock.Unit.Id == unit.Id))
                .OrderByDescending(x => x.Stock.Unit.Id == unit.Id) // Gescande stock eerst
                .ThenBy(x => x.Stock.Unit.Aantal) // Daarna op volgorde van grootte
                .ToList();
            if (SelectedStocks.Count == 0)
            {
                // Artikel niet in checklist
                DisplayBarcodeError();
                return;
            }
            // Get alle locaties
            var artkLocaties = new List<ArtikelLocatie>();
            foreach (var x in SelectedStocks)
            {
                artkLocaties.Add(x.Stock.ArtikelLocatie);
            }
            artkLocaties = artkLocaties.GroupBy(x => x.Id).Select(y => y.First()).ToList();
            var otherLocaties = db.GetArtikelLocaties().Where(x => artkLocaties.All(y => y.Id != x.Id)).ToList();
            artkLocaties.AddRange(otherLocaties);
            
            // Stocks geselecteerd -> locatie opvullen
            Locatie.DataSource = artkLocaties;
            Locatie.DataBind();
            SelectedLijn = MergedChecklist.ChecklistLijnen.FirstOrDefault(x => x.Stock.Artikel.Id == unit.ArtikelId);
            Aantal.Text = (SelectedLijn.AantalWeg / unit.Aantal).ToString(); 
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "fillLocatie", "fillLocatie(" + Aantal.Text + ");", true);
            PrepareUI();
        }

        protected void hdnLocatie_OnValueChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "fillLocatie", "fillLocatie(" + (SelectedLijn.AantalWeg / unit.Aantal) + ");", true);
        }

        private void BindGridView()
        {
            // Uitgecheckte lijnen niet weergeven
            MergedChecklist.ChecklistLijnen = MergedChecklist.ChecklistLijnen.Where(x => x.AantalWeg != 0).ToList();
            Checklist.ChecklistLijnen = Checklist.ChecklistLijnen.Where(x => x.AantalWeg != 0).ToList();
            
            // Check of alles is ingescand
            if (MergedChecklist.ChecklistLijnen.Count == 0)
            {
                // Update setcomplete
                var t = SetEventComplete();
                AlterStock();
                UpdateLog();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "showSucces", t + "(" + Checklist.Model.Id + ");", true);
                return;
            }
            // Id toevoegen
            for (var i = 0; i < MergedChecklist.ChecklistLijnen.Count; i++)
            {
                MergedChecklist.ChecklistLijnen[i].Id = i;
            }
            ChecklistGridView.DataSource = MergedChecklist.ChecklistLijnen;
            ChecklistGridView.DataBind();
        }


        protected void CheckUit_OnClick(object sender, EventArgs e)
        {
            // Nieuw aantal ophalen
            int nieuwAantal = int.Parse(Aantal.Text);
            nieuwAantal *= unit.Aantal;
            var bAantal = SelectedLijn.AantalWeg - nieuwAantal;

            // Add ChecklistStock
            SelectedLijn.Stock.Id = 0;
            SelectedLijn.Stock.Vervaldatum = Vervaldatum.Text != ""
                ? DateTime.Parse(Vervaldatum.Text)
                : DateTime.MaxValue;
            SelectedLijn.Stock.ArtikelLocatie.Id = int.Parse(Locatie.Text);
            SelectedLijn.Stock.ArtikelLocatieId = int.Parse(Locatie.Text);
            SelectedLijn.Stock.Unit.Id = unit.Id;
            SelectedLijn.Stock.Artikel.Id = unit.ArtikelId;
            SelectedLijn.Aantal = nieuwAantal;
            var c = new ChecklistStock(MergedChecklist,SelectedLijn,0);
            if (!SelectedLijn.Stock.Artikel.IsHerbruikbaar)
            {
                db.AddChecklistStock(c);
            }
            else
            {
                // Artikel is verbruiksgoed
                // Controleer of de stock al bestaat
                var stocks = db.GetAllStocks();
                var sStock = stocks
                    .FirstOrDefault(x => x.Artikel.IsHerbruikbaar == true &&
                                               x.Artikel.Id == SelectedLijn.Stock.Artikel.Id
                                               && x.Unit.Id == SelectedLijn.Stock.Unit.Id &&
                                               x.ArtikelLocatie.Id == SelectedLijn.Stock.ArtikelLocatie.Id
                                               && x.Vervaldatum.ToShortDateString() ==
                                               SelectedLijn.Stock.Vervaldatum.ToShortDateString());
                if (sStock != null)
                {
                    // Update stock
                    db.UpdateStock(sStock.Id, sStock.Vervaldatum, sStock.Aantal + nieuwAantal, sStock.ArtikelLocatie.Id,
                        sStock.Artikel.Id, sStock.Unit.Id);
                }
                else
                {
                    // Add stock
                    db.AddStock(SelectedLijn.Stock.Vervaldatum, nieuwAantal, SelectedLijn.Stock.ArtikelLocatie.Id,
                        SelectedLijn.Stock.Artikel.Id, SelectedLijn.Stock.Unit.Id);
                }
            }
            // Beschadiging
            if (Beschadigd.Checked)
            {
                var beschadigdAantal = int.Parse(hdnArtikelBeschadigdAantal.Value);
                var beschadigdOmschrijving = hdnArtikelBeschadigdOmschrijving.Value;
                db.AddArtikelMetaTag("Beschadiging",
                    beschadigdAantal + " beschadigd - " + beschadigdOmschrijving, SelectedLijn.Stock.Artikel.Id);
            }

            // Update aantal
            MergedChecklist.ChecklistLijnen
                .FirstOrDefault(x => x.Stock.Artikel.Id == SelectedLijn.Stock.Artikel.Id)
                .AantalWeg -= nieuwAantal;
            SelectedLijn.AantalWeg = bAantal;
            BindGridView();
            ResetUI();

        }

        protected void GaDoor_OnClick(object sender, EventArgs e)
        {
            // Controleer of overige verbruiksgoederen zijn
            var t = MergedChecklist.ChecklistLijnen.Where(x => x.Stock.Artikel.IsHerbruikbaar);
            if (t.Count() == MergedChecklist.ChecklistLijnen.Count)
            {
                var j = SetEventComplete();
                AlterStock();
                UpdateLog();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "showSuccesMsg", j+"(" + Checklist.Model.Id + ");", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "showWarning", "showWarning(" + Checklist.Model.Id + ");", true);
            }
        }

        private void PrepareUI()
        {
            CheckUit.Enabled = true;
            CheckUit.CssClass = CheckUit.CssClass.Replace("CheckUitDisabled", "test");
            Aantal.ReadOnly = false;
            Aantal.Enabled = true;
            Locatie.Enabled = true;
            Vervaldatum.Enabled = true;
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
            Vervaldatum.Enabled = false;
            Vervaldatum.Text = "";
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
            Vervaldatum.Enabled = false;
            Vervaldatum.Text = "";
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
            db.AddWijziging(Checklist.Model.Id, DateTime.Now, currentUser.Id, "WChecklist");
        }

        protected void AlterStock()
        {
            // Schrijf stocklijnen weg naar echte stock
            ChecklistStocks = db.GetChecklistStocksByEventId(MergedChecklist.Model.Id);
            foreach (var ln in ChecklistStocks)
            {
                var stocks = db.GetAllStocks();
                if (ln.StockId != 0)
                {
                    // Update oude stocks
                    var s = stocks.FirstOrDefault(x => x.Id == ln.StockId);
                    db.UpdateStock(s.Id,
                        s.Vervaldatum.ToShortDateString() != DateTime.MaxValue.ToShortDateString()
                            ? s.Vervaldatum
                            : DateTime.MaxValue, s.Aantal - ln.AantalWeg, s.ArtikelLocatie.Id, s.Artikel.Id, s.Unit.Id);
                }
                else
                {
                    // Controleer of stock al bestaat
                    var s = stocks
                        .FirstOrDefault(x => !x.Artikel.IsHerbruikbaar &&
                                             x.Artikel.Id == ln.ArtikelId
                                             && x.Unit.Id == ln.UnitId &&
                                             x.ArtikelLocatie.Id == ln.ArtikelLocatieId
                                             && x.Vervaldatum.ToShortDateString() ==
                                             ln.Vervaldatum.ToShortDateString());
                    if (s != null)
                    {
                        // Update stock 
                        db.UpdateStock(s.Id,
                            s.Vervaldatum.ToShortDateString() != DateTime.MaxValue.ToShortDateString()
                                ? s.Vervaldatum
                                : DateTime.MaxValue, s.Aantal + ln.Aantal, s.ArtikelLocatie.Id, s.Artikel.Id, s.Unit.Id);
                    }
                    else
                    {
                        // Add stock
                        db.AddStock(Vervaldatum.Text != "" ? DateTime.Parse(Vervaldatum.Text) : DateTime.MaxValue,
                            ln.Aantal, ln.ArtikelLocatieId, ln.ArtikelId, ln.UnitId);
                    }
                }
            }
        }

        protected void hdnGaDoorKeuze_OnValueChanged(object sender, EventArgs e)
        {
            if (hdnGaDoorKeuze.Value == "opslaan")
            {
               // Checklist voltooid
               // Niet teruggekeerde artikelen opslaan
                foreach (var ln in MergedChecklist.ChecklistLijnen)
                {
                    if (ln.Stock.Artikel.IsHerbruikbaar) continue;
                    var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    var currentUser = manager.FindByName(HttpContext.Current.User.Identity.Name);
                    if (MergedChecklist.Model is Evenement && MergedChecklist.Model.FieldmanagerId != 0)
                    {
                        var gebruiker = db.GetGebruikerById(MergedChecklist.Model.FieldmanagerId);
                        currentUser = manager.FindByNameAsync(gebruiker.Email).Result;
                    }
                    if (MergedChecklist.Model is Evenement && MergedChecklist.Model.EventCoordinatorId != 0)
                    {
                        var gebruiker = db.GetGebruikerById(MergedChecklist.Model.EventCoordinatorId);
                        currentUser = manager.FindByNameAsync(gebruiker.Email).Result;
                    }
                    db.AddVermistArtikel(new VermistArtikel(MergedChecklist.Model.Id, ln.Model is Evenement, ln.Stock.Artikel.Id, ln.AantalWeg, currentUser.Id));
                }
                // Overige lijnen die geen verbruiksgoederen zijn opslaan
                AlterStock();
                var t = SetEventComplete();
                UpdateLog();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "showSuccesMsg", t + "(" + Checklist.Model.Id + ");", true);
            }
            hdnGaDoorKeuze.Value = "";
        }

        protected string SetEventComplete()
        {
            var j = "";
            if (MergedChecklist.Model is Evenement)
            {
                db.UpdateEventComplete(MergedChecklist.Model.Id);
                j = "showSuccesMsg";
            }
            else
            {
                db.UpdateOrderComplete(MergedChecklist.Model.Id);
                j = "showSuccesMsg2";
            }
            return j;
        }

    }
            
    }
