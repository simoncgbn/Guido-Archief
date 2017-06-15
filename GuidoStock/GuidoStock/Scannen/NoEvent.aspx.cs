using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using GuidoStock.App_Code;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace GuidoStock.Scannen
{
    public partial class NoEvent : System.Web.UI.Page
    {
        private readonly DBClass db = new DBClass();

        public string Type
        {
            get { return (string)ViewState["Type"]; }
            set { ViewState["Type"] = value; }
        }

        public Code.Unit Unit
        {
            get { return (Code.Unit)ViewState["unit"]; }
            set { ViewState["unit"] = value; }
        }

        private List<App_Code.Stock> selectedStocks
        {
            get { return (List<App_Code.Stock>)ViewState["selectedStocks"]; }
            set { ViewState["selectedStocks"] = value; }
        }

        private List<Code.Unit> units
        {
            get { return (List<Code.Unit>)ViewState["units"]; }
            set { ViewState["units"] = value; }
        }

        private List<ArtikelLocatie> ArtikelLocaties
        {
            get { return (List<ArtikelLocatie>) ViewState["ArtikelLocatie"]; }
            set { ViewState["ArtikelLocatie"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            Type = "";
            var id = Request.QueryString["id"];
            if (!string.IsNullOrEmpty(id))
            {
                Type = id;
            }

            if (Type == "")
            {
                Response.Redirect("Overzicht.aspx");
            }

            if (!IsPostBack)
            {
                units = db.GetAllUnits();
            }
            selectedStocks = db.GetAllStocks();
        }

        public bool IsInkomend => Type == "inkomend";

        private void FillLocaties(Code.Unit unit)
        {           
            if (IsInkomend)
            {
                // Get alle locaties van dit artikel en toon deze locaties waar artikel reeds voorkomt eerst
                ArtikelLocaties = db.GetArtikelLocatiesForArtikel(unit.ArtikelId);
                var locaties = db.GetArtikelLocaties().OrderBy(x => ArtikelLocaties.FindIndex(a => a.Id == x.Id) == -1).ToList();
                Locatie.DataSource = locaties;
            }
            else
            {
                // Get alle locaties van dit artikel waar unitAantal >= ingescande unit.Aantal
                selectedStocks.ForEach(s => s.Unit = units.Find(a => a.Id == s.UnitId));
                var stocks = selectedStocks.Where(a => a.Artikel.Id == unit.ArtikelId && (IsAncestor(a.Unit, Unit) || a.Unit.Id == Unit.Id));
                var enumerable = stocks as IList<App_Code.Stock> ?? stocks.ToList();
                enumerable.ForEach(stock =>
                {
                    stock.LocatieVervaldatum =
                        $"[{stock.Unit.NaamEnkelvoud}] {stock.ArtikelLocatie.Code}";
                });
                foreach (var stock in enumerable.Where(stock => stock.Vervaldatum.ToShortDateString() != DateTime.MaxValue.ToShortDateString()))
                {
                    // Zet vervalDatum naast locatie wnr stock vervaldatum heeft
                    stock.LocatieVervaldatum =
                        $"[{stock.Unit.NaamEnkelvoud}] {stock.ArtikelLocatie.Code} - {stock.Vervaldatum.ToShortDateString()}";
                }
                var locaties = enumerable.ToList();
                Locatie.DataSource = locaties;
            }
            // Als type inkomend is dan bestaat locaties uit ArtikelLocaties, als uitgaand dan bestaat locaties uit stocks
            Locatie.DataBind();
            Locatie.Enabled = true;
        }

        protected void CheckUit_OnClick(object sender, EventArgs e)
        {
            var aantal = Convert.ToInt32(Aantal.Text);
            var vervalDatum = DateTime.MaxValue;
            if (!string.IsNullOrEmpty(Vervaldatum.Text))
            {
                vervalDatum = Convert.ToDateTime(Vervaldatum.Text);
            }

            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var currentUser = manager.FindByName(HttpContext.Current.User.Identity.Name);
            if (IsInkomend)
            {
                // Inkomende stock
                int locatieId = int.Parse(Locatie.SelectedValue);
                // Bestaat er al een stock dat overeenkomt met deze unit, locatie en vervaldatum
                var stock =
                    selectedStocks.Find(x => x.ArtikelLocatie.Id == locatieId && x.UnitId == Unit.Id && x.Vervaldatum.ToShortDateString().Equals(vervalDatum.ToShortDateString()));
                if (stock != null)
                {
                    // Zoja, update deze stock
                    db.UpdateStock(stock.Id, vervalDatum, stock.Aantal + (aantal * Unit.Aantal), stock.ArtikelLocatie.Id, stock.Artikel.Id, Unit.Id);
                }
                else
                {
                    // Neen, maak nieuwe stock aan
                    db.AddStock(vervalDatum, (aantal * Unit.Aantal), locatieId, Unit.ArtikelId, Unit.Id);
                }
                

                db.AddWijziging(Unit.ArtikelId, DateTime.Now, currentUser.Id, "PArtikel");

            }
            else
            {
                // Uitgaand
                if (Unit == null) return;
                int stockId = int.Parse(Locatie.SelectedValue);
                selectedStocks.ForEach(s => s.Unit = units.Find(a => a.Id == s.UnitId));
                var stock = selectedStocks.Find(x => x.Id == stockId);
                // Bereken het totale aantal dat moet uitgescand worden
                var totaalAantal = aantal*Unit.Aantal;
                if (totaalAantal % stock.Unit.Aantal != 0)
                {
                    // Stock moet gesplitst worden
                    double test = (double)totaalAantal/stock.Unit.Aantal;
                    var toppedAantal = (int)Math.Ceiling(test)*stock.Unit.Aantal;
                    var splitAantal = toppedAantal - totaalAantal;
                    // TODO :: Update huidige stock met aangepast aantal (hoger dan) - toppedAantal
                    db.UpdateStock(stock.Id, stock.Vervaldatum, stock.Aantal - toppedAantal, stock.ArtikelLocatie.Id, stock.Artikel.Id, stock.UnitId);
                    // TODO :: Add of update nieuwe stock met overschot - splitAantal
                    stock.Unit = Unit;
                    AddOrUpdateStock(stock, splitAantal);
                }
                else
                {
                    // TODO :: update huidige stock met aantal - totaalAantal
                    db.UpdateStock(stock.Id, stock.Vervaldatum, stock.Aantal - aantal, stock.ArtikelLocatie.Id, stock.Artikel.Id, stock.Unit.Id);
                }
                db.AddWijziging(stock.Artikel.Id, DateTime.Now, currentUser.Id, "MArtikel");
            }
            ResetUI();
        }

        private void AddOrUpdateStock(App_Code.Stock stock, int aantal)
        {
            var check =
                    selectedStocks.Find(x => x.ArtikelLocatie.Id == stock.ArtikelLocatie.Id && x.UnitId == stock.Unit.Id && x.Vervaldatum.ToShortDateString().Equals(stock.Vervaldatum.ToShortDateString()));
            if (check != null)
            {
                db.UpdateStock(check.Id, check.Vervaldatum, check.Aantal + aantal, check.ArtikelLocatie.Id, check.Artikel.Id, check.UnitId);
            }
            else
            {
                // Add Stock
                db.AddStock(stock.Vervaldatum, aantal, stock.ArtikelLocatie.Id, stock.Artikel.Id, stock.Unit.Id);
            }
        }

        protected void hdnBarcode_OnValueChanged(object sender, EventArgs e)
        {
            var currentBarcode = hdnBarcode.Value;
            if ((Unit = units.Find(u => u.Barcode == currentBarcode)) != null)
            {
                FillLocaties(Unit);
                PrepareUI();
            }
            else
            {
                DisplayBarcodeError();
            }
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
            if (IsInkomend)
                Vervaldatum.Text = "";
        }

        private void PrepareUI()
        {
            CheckUit.Enabled = true;
            CheckUit.CssClass = CheckUit.CssClass.Replace("CheckUitDisabled", "test");
            Aantal.ReadOnly = false;
            Aantal.Enabled = true;
            ErrorBarcode.Attributes.Add("style", "display:none");
            if (!IsInkomend) hdnLocatie_OnValueChanged(hdnLocatie, null);
        }

        private void DisplayBarcodeError()
        {
            Aantal.Text = "";
            Locatie.Items.Clear();
            Vervaldatum.Text = "";
            CheckUit.Enabled = false;
            Locatie.Enabled = false;
            Aantal.ReadOnly = true;
            Aantal.Enabled = false;
            ErrorBarcode.Attributes.Add("style", "display:inline-block");
            CheckUit.CssClass = CheckUit.CssClass.Replace("test", "CheckUitDisabled");
            hdnBarcode.Value = "";
        }

        private bool IsAncestor(Code.Unit unit, Code.Unit target)
        {
            while (true)
            {
                if (unit.ChildUnitId == 0) return false;
                if (unit.ChildUnitId == target.Id)
                    return true;
                unit = units.Find(u => u.Id == unit.ChildUnitId);
            }
        }

        protected void hdnLocatie_OnValueChanged(object sender, EventArgs e)
        {
            if (IsInkomend) return;
            var selectedLocatieId = Convert.ToInt32(Locatie.SelectedValue);
            var stock = selectedStocks.Find(x => x.Id == selectedLocatieId);
            var aantal = stock.Aantal / Unit.Aantal;
            Aantal.Text = aantal.ToString();
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "fillLocatie2", "fillLocatie(" + aantal + ");", true);
        }
    }
}