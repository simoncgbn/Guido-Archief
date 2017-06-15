using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GuidoStock.App_Code;
using GuidoStock.Code;
using GuidoStock.Models;

namespace GuidoStock.Order
{
    public partial class Checklist : System.Web.UI.Page
    {
        private readonly static DBClass _db = new DBClass();

        private List<Artikel> Stocklijst
        {
            get { return (List<Artikel>) ViewState["Stocklijst"]; }
            set { ViewState["Stocklijst"] = value; }
        }

        private Code.Order Order
        {
            get { return (Code.Order) ViewState["Order"]; }
            set { ViewState["Order"] = value; }
        }

        private List<AvailableModel> AvailableModels
        {
            get { return (List<AvailableModel>)ViewState["AvailableModels"]; }
            set { ViewState["AvailableModels"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            Stocklijst = _db.GetArtikelen();
            Stocklijst = Stocklijst.Where(a => (a.Aantal > 0 && a.IsVerwijderd == false)).ToList();

            if (Request.QueryString["id"] == null) Response.Redirect("Overzicht.aspx");
            var id = 0;
            if (!int.TryParse(Request.QueryString["Id"], out id)) Response.Redirect("Overzicht.aspx");

            if ((Order = _db.GetOrder(id)) == null)
            {
                Order = new Code.Order();
            }
            Order.OrderLijnen = _db.GetOrderLijnenByOrderId(Order.Id);
            ParseStockLijst();
            var array = Order.OrderLijnen.Select(a => a.Artikel.Id);

            Stocklijst.RemoveAll(a => array.Contains(a.Id));
            ChecklistGridView.DataSource = Order.OrderLijnen;
            ChecklistGridView.DataBind();
            StocklijstGridView.DataSource = Stocklijst;
            StocklijstGridView.DataBind();
        }

        protected void StocklijstGridView_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Add":
                    VoegArtikelToe(Convert.ToInt32(e.CommandArgument));
                    break;
                default:
                    break;
            }
        }

        private void VoegArtikelToe(int artikelId)
        {
            var index = Order.OrderLijnen.FindIndex(a => a.Artikel.Id == artikelId);
            if (index != -1) return;
            Order.OrderLijnen.Insert(0, new OrderLijn(Stocklijst.Find(a => a.Id == artikelId), Order, 1));
            ChecklistGridView.DataSource = Order.OrderLijnen;
            ChecklistGridView.DataBind();
            // Remove from stocklijst
            Stocklijst.RemoveAll(a => a.Id == artikelId);
            StocklijstGridView.DataSource = Stocklijst;
            StocklijstGridView.DataBind();
        }

        protected void ChecklistGridView_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            var txt = e.Row.FindControl("txtAantal") as TextBox;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("$(document).ready(function() {");
            sb.AppendLine("$(\"#" + txt.ClientID + "\").TouchSpin({");
            sb.AppendLine("verticalbuttons: true, max: " + ((OrderLijn)e.Row.DataItem).Artikel.AvailableAantal);
            sb.AppendLine("});");
            sb.AppendLine("});");
            ScriptManager.RegisterStartupScript(updatePanel, updatePanel.GetType(), txt.ClientID, sb.ToString(), true);
        }

        protected void ChecklistGridView_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Remove":
                    VerwijderArtikel(Convert.ToInt32(e.CommandArgument));
                    break;
                default:
                    break;
            }
        }

        private void VerwijderArtikel(int artikelId)
        {
            // Remove from evenementLijn
            Order.OrderLijnen.RemoveAll(a => a.Artikel.Id == artikelId);
            ChecklistGridView.DataSource = Order.OrderLijnen;
            ChecklistGridView.DataBind();
            // Add to stocklijst
            var artikel = _db.GetArtikel(artikelId);
            Stocklijst.Insert(0, artikel);
            QuickParse(artikel);
            StocklijstGridView.DataSource = Stocklijst;
            StocklijstGridView.DataBind();
        }

        protected void txtAantal_OnTextChanged(object sender, EventArgs e)
        {
            var txt = (TextBox)sender;
            var aantal = Convert.ToInt32(txt.Text);
            var id = Convert.ToInt32(txt.Attributes["CommandArgument"]);
            Order.OrderLijnen.Find(a => a.Artikel.Id == id).Aantal = aantal;
        }

        protected void OnClick(object sender, EventArgs e)
        {
            Response.Redirect("Overzicht.aspx?id=" + Order.Id);
        }

        protected void BtnNieuw_OnClick(object sender, EventArgs e)
        {
            if (Order == null)
            {
                // Er moet altijd een order zijn
                Response.Redirect("Overzicht.aspx");
                return;
            }
            if (Order.Id == -1)
                Order.Id = _db.AddOrder(Order);
            else
                _db.UpdateOrder(Order);

            var orderLijn = CheckForConflictingChanges();
            if (orderLijn == null)
            {
                bool result = _db.UpdateOrderLijnenByOrder(Order.OrderLijnen, Order.Id);
                // TODO :: Hier moet eig iets getoond worden indien result niet true is. Moeilijk te testen.
                if (result)
                {
                    ScriptManager.RegisterStartupScript(updatePanel, updatePanel.GetType(), "showSuccess", "showSuccesModal(" + Order.Id + ");", true);
                }
            }
            else
            {
                Order.OrderLijnen.Find(a => a.Artikel.Id == orderLijn.Artikel.Id).Artikel.AvailableAantal = orderLijn.Artikel.AvailableAantal;
                ChecklistGridView.DataSource = Order.OrderLijnen;
                ChecklistGridView.DataBind();
                string query = "openModal('" + orderLijn.Artikel.Naam + "'," + orderLijn.Artikel.AvailableAantal + ");";
                ScriptManager.RegisterStartupScript(updatePanel, updatePanel.GetType(), "showError", query, true);
            }
        }

        private OrderLijn CheckForConflictingChanges()
        {
            // Fetch all overlapping evenementLijnen
            AvailableModels = _db.GetAvailableAantallenVoorOrder(Order);
            // Check if nothing conflicts
            foreach (OrderLijn t in Order.OrderLijnen)
            {
                if (AvailableModels.FindIndex(a => a.ArtikelId == t.Artikel.Id) != -1)
                {
                    var aantal = 0;
                    AvailableModels.Where(a => a.ArtikelId == t.Artikel.Id).ToList().ForEach(d =>
                    {
                        if (d.LijnAantal > d.AantalVervalt) aantal += d.LijnAantal;
                        else aantal += d.AantalVervalt + d.LijnAantal;
                    });
                    // aantal overlappingen + aantal dat moet toegevoegd worden > aantal beschikbaar in totaal
                    if ((aantal + t.Aantal) <= t.Artikel.Aantal) continue;
                    // Conflict, return true
                    t.Artikel.AvailableAantal = t.Artikel.Aantal - aantal;
                    return t;
                }
            }
            // No conflicts, return false
            return null;
        }

        private void ParseStockLijst()
        {

            AvailableModels = _db.GetAvailableAantallenVoorOrder(Order);
            AvailableModels.ForEach(model =>
            {
                var index = -1;
                if ((index = Stocklijst.FindIndex(a => a.Id == model.ArtikelId)) != -1)
                {
                    if (model.LijnAantal > model.AantalVervalt) Stocklijst[index].AvailableAantal -= model.LijnAantal;
                    else Stocklijst[index].AvailableAantal -= model.AantalVervalt + model.LijnAantal;
                }
                index = -1;
                if ((index = Order.OrderLijnen.FindIndex(a => a.Artikel.Id == model.ArtikelId)) != -1)
                {
                    Order.OrderLijnen[index].Artikel.AvailableAantal -= model.LijnAantal > model.AantalVervalt ? model.LijnAantal : model.AantalVervalt + model.LijnAantal;
                }
            });
        }

        private void QuickParse(Artikel artikel)
        {
            var index = -1;
            if ((index = Stocklijst.FindIndex(a => a.Id == artikel.Id)) != -1)
            {
                var model = AvailableModels.Find(a => a.ArtikelId == artikel.Id);
                Stocklijst[index].AvailableAantal -= model.LijnAantal > model.AantalVervalt
                    ? model.LijnAantal
                    : model.AantalVervalt + model.LijnAantal;
            }
            else if ((index = Order.OrderLijnen.FindIndex(a => a.Artikel.Id == artikel.Id)) != -1)
            {
                var model = AvailableModels.Find(a => a.ArtikelId == artikel.Id);
                Order.OrderLijnen[index].Artikel.AvailableAantal -= model.LijnAantal > model.AantalVervalt
                    ? model.LijnAantal
                    : model.AantalVervalt + model.LijnAantal;
            }
        }
    }
}