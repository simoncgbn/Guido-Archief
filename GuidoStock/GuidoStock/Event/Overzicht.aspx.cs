using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GuidoStock.App_Code;
using GuidoStock.Code;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace GuidoStock.Event
{
    public partial class Overzicht : System.Web.UI.Page
    {
        protected Evenement Evenement
        {
            get { return (Evenement)ViewState["Evenement"]; }
            set { ViewState["Evenement"] = value; }
        }

        private List<Evenement> Evenementen
        {
            get { return (List<Evenement>) ViewState["Evenementen"]; }
            set { ViewState["Evenementen"] = value; }
        }

        protected string FilterType
        {
            get { return (string)ViewState["FilterType"]; }
            set { ViewState["FilterType"] = value; }
        }

        DBClass db = new DBClass();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string type = Request.QueryString["type"];
                
                if (type == "0" || type == null || type == "komende")
                {
                    FilterType = "komende";
                    Evenementen = db.GetHuidigeEvenementen();
                }
                else if (type == "alles")
                {
                    FilterType = "alles";
                    Evenementen = db.GetEvenementen();
                }
                else if (type == "verlopen")
                {
                    FilterType = "verlopen";
                    Evenementen = db.GetVerlopenEvenementen();
                }
                if (Evenementen.Count > 0)
                {
                    EvenementenGridView.DataSource = Evenementen;
                    EvenementenGridView.DataBind();
                    Evenement = db.GetEvenement(Evenementen[0].Id);
                    EvenementenGridView.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
                else
                {
                    FilterType = "alles";
                    Evenementen = db.GetEvenementen();
                    EvenementenGridView.DataSource = Evenementen;
                    EvenementenGridView.DataBind();
                    if (Evenementen.Count > 0)
                        EvenementenGridView.HeaderRow.TableSection = TableRowSection.TableHeader;
                }

                string id = Request.QueryString["id"];
                

                if (!string.IsNullOrEmpty(id))
                {
                    int n;
                    bool parsed = int.TryParse(id, out n);
                    bool exist = false;

                    if (parsed)
                    {
                        foreach (var ev in Evenementen.Where(ev => ev.Id == int.Parse(id)))
                        {
                            exist = true;
                        }

                        if (exist)
                        {
                            Evenement = db.GetEvenement(int.Parse(id));
                        }
                    }
                }
            }
        }

        protected void EvenementenGridView_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect("Overzicht.aspx?id=" + EvenementenGridView.SelectedDataKey.Value+"&type="+(FilterType != null ? FilterType : "0"));
        }

        protected void EvenementenGridView_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(EvenementenGridView,
                    "select$" + e.Row.RowIndex);
            }
        }

        protected void checklistButton_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("ChecklistOverzicht.aspx?id=" + Evenement.Id);
        }

        protected void komende_OnServerClick(object sender, EventArgs e)
        {
            FilterType = "komende";
            Evenementen = db.GetHuidigeEvenementen();
            if (Evenementen.Count > 0)
            {
                EvenementenGridView.DataSource = Evenementen;
                EvenementenGridView.DataBind();
            }
            else
            {
                EvenementenGridView.DataSource = null;
                EvenementenGridView.DataBind();
            }
        }

        protected void alles_OnServerClick(object sender, EventArgs e)
        {
            FilterType = "alles";
            Evenementen = db.GetEvenementen();
            if (Evenementen.Count > 0)
            {
                EvenementenGridView.DataSource = Evenementen;
                EvenementenGridView.DataBind();
            }
            else
            {
                EvenementenGridView.DataSource = null;
                EvenementenGridView.DataBind();
            }
        }

        protected void verlopen_OnServerClick(object sender, EventArgs e)
        {
            FilterType = "verlopen";
            Evenementen = db.GetVerlopenEvenementen();
            if (Evenementen.Count > 0)
            {
                EvenementenGridView.DataSource = Evenementen;
                EvenementenGridView.DataBind();
            }
            else
            {
                EvenementenGridView.DataSource = null;
                EvenementenGridView.DataBind();
            }
        }

        protected void btnEditStock_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("AddEvent.aspx?id=" + Evenement.Id);
        }
    }
}