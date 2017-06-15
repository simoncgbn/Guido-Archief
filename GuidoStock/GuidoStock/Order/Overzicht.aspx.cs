using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GuidoStock.App_Code;

namespace GuidoStock.Order
{
    public partial class Overzicht : System.Web.UI.Page
    {
        private static readonly DBClass _db = new DBClass();

        private List<Code.Order> Orders
        {
            get { return (List<Code.Order>) ViewState["Orders"]; }
            set { ViewState["Orders"] = value; }
        }

        protected Code.Order Order
        {
            get { return (Code.Order) ViewState["Order"]; }
            set { ViewState["Order"] = value; }
        }

        protected string FilterType
        {
            get { return (string)ViewState["FilterType"]; }
            set { ViewState["FilterType"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            string type = Request.QueryString["type"];

            if (type == "0" || type == null || type == "komende")
            {
                FilterType = "komende";
                Orders = _db.GetHuidigeOrders();
            }
            else if (type == "alles")
            {
                FilterType = "alles";
                Orders = _db.GetOrders();
            }
            else if (type == "verlopen")
            {
                FilterType = "verlopen";
                Orders = _db.GetVerlopenOrders();
            }

            if (Orders.Count == 0)
            {
                Orders = _db.GetOrders();
            }

            OrdersGridView.DataSource = Orders;
            OrdersGridView.DataBind();

            var id = Request.QueryString["id"];
            if (string.IsNullOrEmpty(id)) return;
            var n = 0;
            if (!int.TryParse(id, out n)) return;
            if ((Order = _db.GetOrder(n)) == null)
            {
                Order = new Code.Order();
            }
            else
            {
                Order.OrderLijnen = _db.GetOrderLijnenByOrderId(Order.Id);
                ChecklistGridView.DataSource = Order.OrderLijnen;
                ChecklistGridView.DataBind();
            }
        }

        protected void OrdersGridView_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect("Overzicht.aspx?id=" + OrdersGridView.SelectedDataKey.Value+"&type="+(FilterType != null ? FilterType : "0"));
        }

        protected void OrdersGridView_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(OrdersGridView,
                    "select$" + e.Row.RowIndex);
            }
        }

        protected void checklistButton_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("Checklist.aspx?id=" + Order.Id);
        }

        protected void komende_OnServerClick(object sender, EventArgs e)
        {
            FilterType = "komende";
            Orders = _db.GetHuidigeOrders();
            if (Orders.Count > 0)
            {
                OrdersGridView.DataSource = Orders;
                OrdersGridView.DataBind();
            }
            else
            {
                OrdersGridView.DataSource = null;
                OrdersGridView.DataBind();
            }
        }

        protected void alles_OnServerClick(object sender, EventArgs e)
        {
            FilterType = "alles";
            Orders = _db.GetOrders();
            if (Orders.Count > 0)
            {
                OrdersGridView.DataSource = Orders;
                OrdersGridView.DataBind();
            }
            else
            {
                OrdersGridView.DataSource = null;
                OrdersGridView.DataBind();
            }
        }

        protected void verlopen_OnServerClick(object sender, EventArgs e)
        {
            FilterType = "verlopen";
            Orders = _db.GetVerlopenOrders();
            if (Orders.Count > 0)
            {
                OrdersGridView.DataSource = Orders;
                OrdersGridView.DataBind();
            }
            else
            {
                OrdersGridView.DataSource = null;
                OrdersGridView.DataBind();
            }
        }

        protected void btnEditOrder_OnClick(object sender, EventArgs e)
        {
            var ordId = Order.Id;
            Response.Redirect("AddOrder.aspx?id=" + ordId + "&type=" + (FilterType != null ? FilterType : "0"));
        }
    }
}