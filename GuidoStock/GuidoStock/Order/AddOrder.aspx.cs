using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml;
using GuidoStock.App_Code;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace GuidoStock.Order
{
    public partial class AddOrder : System.Web.UI.Page
    {
        public Code.Order Order
        {
            get { return (Code.Order) ViewState["Order"]; }
            set { ViewState["Order"] = value; }
        }

        private static readonly DBClass _db = new DBClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                chkIsVerhuur.Checked = true;
                if (Request.QueryString["id"] != null)
                {
                    var id = 0;
                    if (!int.TryParse(Request.QueryString["id"], out id)) Response.Redirect("Overzicht.aspx");
                    Order = _db.GetOrder(id) ?? new Code.Order();
                }
                else
                {
                    Order = new Code.Order();
                }
                UpdateUI();
            }

        }

        protected void LinkButton1_OnClick(object sender, EventArgs e)
        {
            Order.IsVerhuur = chkIsVerhuur.Checked;
            Order.BeginTijd = Convert.ToDateTime(txtBeginTijd.Text);
            Order.Naam = txtNaam.Text;
            Order.ContactNaam = txtNaamContactpersoon.Text;
            Order.Tel = txtTel.Text;
            Order.EindTijd = Order.IsVerhuur ? Convert.ToDateTime(txtEindTijd.Text) : DateTime.MaxValue;
            bool exist = false;
            if (Order.Id == -1)
            {
                Order.Id = _db.AddOrder(Order);
                exist = false;
            }
            else
            {
                _db.UpdateOrder(Order);
                exist = true;
            }
               

            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var currentUser = manager.FindByName(HttpContext.Current.User.Identity.Name);

            _db.AddWijziging(Order.Id, DateTime.Now, currentUser.Id, exist ? "WOrder" : "NOrder");
            if (!exist)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertUser", "BootstrapDialog.show({title: 'Geslaagd!', message: 'Het order is toegevoegd.', buttons: [{ label: 'Sluiten', action: function(){ window.location.href = 'Overzicht.aspx';}}]}); ", true);

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertUser", "BootstrapDialog.show({title: 'Geslaagd!', message: 'Het order is gewijzigd.', buttons: [{ label: 'Sluiten', action: function(){ window.location.href = 'Overzicht.aspx';}}]}); ", true);

            }
        }

        private void UpdateUI()
        {

            chkIsVerhuur.Checked = Order.IsVerhuur;
            txtBeginTijd.Text = Order.BeginTijd == DateTime.MinValue ? "" :  Order.BeginTijd.ToString("yyyy-MM-dd hh:mm");
            txtEindTijd.Text = Order.EindTijd == DateTime.MinValue ? "" : Order.EindTijd.ToString("yyyy-MM-dd hh:mm");
            txtNaam.Text = Order.Naam;
            txtNaamContactpersoon.Text = Order.ContactNaam;
            txtTel.Text = Order.Tel;
            RequiredFieldValidator2.ValidationGroup = Order.IsVerhuur ? "group1" : "group2";
        }

        protected void hdnToggle_OnValueChanged(object sender, EventArgs e)
        {
            var value = bool.Parse(hdnToggle.Value);
            Order.IsVerhuur = value;
            if (value)
            {
                RequiredFieldValidator2.ValidationGroup = "group1";
            }
            else
            {
                RequiredFieldValidator2.ValidationGroup = "group2";
            }
            
        }
    }
}