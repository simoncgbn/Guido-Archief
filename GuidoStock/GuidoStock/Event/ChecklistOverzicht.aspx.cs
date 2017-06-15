using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using GuidoStock.App_Code;
using GuidoStock.Code;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GuidoStock.Event
{
    public partial class ChecklistOverzicht : System.Web.UI.Page
    {
        private static DBClass db = new DBClass();

        private Evenement Evenement
        {
            get { return (Evenement)ViewState["Evenement"]; }
            set { ViewState["Evenement"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                DBClass db = new DBClass();

                string checklistId = Request.QueryString["id"];
                if (!string.IsNullOrEmpty(checklistId))
                {
                    int id;
                    if (int.TryParse(checklistId, out id))
                    {
                        Evenement = db.GetEvenement(id);
                        if (Evenement == null)
                        {
                            // Er moet altijd een evenement zijn
                            Response.Redirect("Overzicht.aspx");
                            return;
                        }
                        var evenementLijnen = db.GetEvenementLijnen(id);
                        if (evenementLijnen.Count == 0)
                        {
                            Response.Redirect("Checklist.aspx?id=" + Evenement.Id);
                        }
                        ChecklistGridView.DataSource = evenementLijnen;
                        ChecklistGridView.DataBind();
                    }
                }
                else
                {
                    Response.Redirect("Overzicht.aspx");
                    return;
                }      
            }
        }

        protected void OnClick(object sender, EventArgs e)
        {
            Response.Redirect("Checklist.aspx?id=" + Evenement.Id);
        }

        protected void btnVorige_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("Overzicht.aspx?id=" + Evenement.Id);
        }
    }
}