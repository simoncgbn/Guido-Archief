using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GuidoStock.App_Code;
using GuidoStock.Stock;
using GuidoStock.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using GuidoStock.Code;

namespace GuidoStock.Beheer
{
    public partial class Overzicht : System.Web.UI.Page
    {
        DBClass db = new DBClass();

        public List<ArtikelLocatie> artikelLocatieList
        {
            get { return (List<ArtikelLocatie>)ViewState["artikelLocatieList"]; }
            set { ViewState["artikelLocatieList"] = value; }
        }

        public List<App_Code.Stock> stocksList
        {
            get { return (List<App_Code.Stock>)ViewState["stocksList"]; }
            set { ViewState["stocksList"] = value; }
        }

        public List<Transport> transportList
        {
            get { return (List<Transport>)ViewState["transportList"]; }
            set { ViewState["transportList"] = value; }
        }

        public List<Gebruiker> Gebruikers
        {
            get { return (List<Gebruiker>) ViewState["Gebruikers"]; }
            set { ViewState["Gebruikers"] = value; }
        } 

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                var id = Request.QueryString["id"];
                var oud = Request.QueryString["oud"];
                var nieuw = Request.QueryString["nieuw"];
                var gewicht = Request.QueryString["gewicht"];
                var verwijder = Request.QueryString["v"];
                var deleteGebruiker = Request.QueryString["deleteGebruiker"];

                if (id != null && oud != null && nieuw != null)
                {
                    if (id == "stock")
                    {
                        if (!string.IsNullOrEmpty(oud))
                        {
                            db.UpdateArtikelLocatieByCode(oud, nieuw);
                        }
                        ScriptManager.RegisterStartupScript(this, GetType(), "setActive",
                        "setActive('stock')", true);
                    }
                    else if (id == "transport" && gewicht != null)
                    {
                        if (!string.IsNullOrEmpty(oud))
                        {
                            int t;
                            if (int.TryParse(gewicht, out t))
                            {
                                db.UpdateTransport(oud, nieuw, t);
                            }
                            
                        }

                        ScriptManager.RegisterStartupScript(this, GetType(), "setActive",
                        "setActive('transport')", true);
                    }
                }
                else if (id != null && oud != null && verwijder != null)
                {
                    if (id == "transport" && verwijder == "true")
                    {
                        db.DeleteTransport(oud);
                    }
                    ScriptManager.RegisterStartupScript(this, GetType(), "setActive",
                        "setActive('transport')", true);
                }

                if (deleteGebruiker != null)
                {
                    int gebruikerId;
                    if (int.TryParse(deleteGebruiker, out gebruikerId))
                    {
                        VerwijderGebruiker(gebruikerId);
                    }
                }

                // set gebruikers active

                ScriptManager.RegisterStartupScript(this, GetType(), "setActive",
                        "setActive('gebruikers')", true);

                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                Gebruikers = db.GetGebruikers();
                var rechten = db.GetRechten();
                var list2 = Gebruikers.Select(gebruiker => manager.FindByName(gebruiker.Email)).Select(user => new UserModel(user, manager.GetRoles(user.Id)[0])).ToList();
                RechtenGridView.DataSource = rechten;
                RechtenGridView.DataBind();
                GebruikersGridView.DataSource = list2;
                GebruikersGridView.DataBind();
                ddlGebruiker.DataSource = list2;
                ddlGebruiker.DataBind();

                artikelLocatieList = new List<ArtikelLocatie>();
                stocksList = new List<App_Code.Stock>();
                FillArtikelLocaties();

                transportList = new List<Transport>();
                transportList = db.getTransporten();
                TransportengridView.DataSource = transportList;
                TransportengridView.DataBind();
                ddlModule.DataSource = db.GetModules();
                ddlModule.DataBind();

                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());
                ddlRol.DataSource = roleManager.Roles.ToList();
                ddlRol.DataBind();
                ddlRecht.DataSource = roleManager.Roles.ToList();
                ddlRecht.DataBind();
                ddlRemoveRecht.DataSource = roleManager.Roles.ToList();
                ddlRemoveRecht.DataBind();

                var recht = new List<string>() {"Geen", "Lezen", "Lezen & Schrijven"};
                ddlRechten.DataSource = recht;
                ddlRechten.DataBind();
            }

        }

        private void VerwijderGebruiker(int gebruikerId)
        {
            db.DeleteGebruiker(gebruikerId);
            Gebruikers = db.GetGebruikers();
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var list2 = Gebruikers.Select(gebruiker => manager.FindByName(gebruiker.Email)).Select(user => new UserModel(user, manager.GetRoles(user.Id)[0])).ToList();
            GebruikersGridView.DataSource = list2;
            GebruikersGridView.DataBind();
            ScriptManager.RegisterStartupScript(this, GetType(), "setActive",
                        "setActive('gebruikers')", true);
        }

        protected void FillArtikelLocaties()
        {
            artikelLocatieList = db.GetArtikelLocaties();
            stocksList = db.GetAllStocks();
            foreach (var loc in artikelLocatieList)
            {
                loc.AantalArtikelen = 0;
                loc.AantalStocks = 0;
                foreach (var stk in stocksList.Where(stk => stk.ArtikelLocatie.Id == loc.Id))
                {
                    loc.AantalArtikelen += stk.Aantal;
                    loc.AantalStocks += 1;
                }
            }
            StockGridView.DataSource = artikelLocatieList;
            StockGridView.DataBind();
        }

        protected void VoegToe_Click(object sender, EventArgs e)
        {
            var nieuweCode = txtCode.Text;
            bool blnExists = artikelLocatieList.Any(loc => loc.Code == nieuweCode);

            if (blnExists)
            {
                // Error
                ScriptManager.RegisterStartupScript(this, GetType(), "error",
                        "error('stock')", true);
            }
            else
            {
                // Add locatie
                db.AddArtikelLocatie(nieuweCode, nieuweCode);
            }

            FillArtikelLocaties();
            txtCode.Text = "";

            ScriptManager.RegisterStartupScript(this, GetType(), "setActive",
                        "setActive('stock')", true);
        }

        protected void VoegTransportToe_Click(object sender, EventArgs e)
        {
            var nieuweNaam = txtTransport.Text;
            var nieuwGewicht = txtGewicht.Text;
            bool blnExists = transportList.Any(trans => trans.Naam == nieuweNaam);

            if (blnExists)
            {
                // Error
                ScriptManager.RegisterStartupScript(this, GetType(), "error",
                        "error('transportnaam')", true);
            }
            else
            {
                int t;
                if (int.TryParse(nieuwGewicht, out t))
                {
                    db.AddTransport(nieuweNaam, t);
                }
            }


            var transporten = db.getTransporten();
            TransportengridView.DataSource = transporten;
            TransportengridView.DataBind();
            txtTransport.Text = "";
            txtGewicht.Text = "";

            ScriptManager.RegisterStartupScript(this, GetType(), "setActive",
                        "setActive('transport')", true);
        }

       

        protected void btnBarcode_OnClick(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            string url = btn.CommandArgument;
            ScriptManager.RegisterStartupScript(this, GetType(), "navigateTo",
                "navigateTo('"+url+"')", true);
        }

        protected void btnEdit_OnClick(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            string code = btn.CommandArgument;
            ScriptManager.RegisterStartupScript(this, GetType(), "editLocatie",
                "editLocatie('" + code + "')", true);

            ScriptManager.RegisterStartupScript(this, GetType(), "setActive",
                "setActive('stock')", true);

        }

        protected void btnDeleteTransport_OnClick(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            string naam = btn.CommandArgument;
            ScriptManager.RegisterStartupScript(this, GetType(), "deleteTransport",
                "deleteTransport('" + naam + "')", true);

            ScriptManager.RegisterStartupScript(this, GetType(), "setActive",
                "setActive('transport')", true);
        }

        protected void btnEditTransport_OnClick(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            string naam = btn.CommandArgument;
            int maxGewicht = transportList.Where(b => b.Naam == naam).Select(a => a.MaxGewicht).FirstOrDefault();
            ScriptManager.RegisterStartupScript(this, GetType(), "editTransport",
                "editTransport('"+naam+"','"+maxGewicht+"')", true);

            ScriptManager.RegisterStartupScript(this, GetType(), "setActive",
                "setActive('transport')", true);
        }

        protected void OnClick(object sender, EventArgs e)
        {
            var rolId = ddlRol.SelectedValue;
            var moduleId = ddlModule.SelectedValue;
            var recht = ddlRechten.SelectedValue;
            bool lezen, schrijven;
            if (recht.Equals("Geen"))
            {
                lezen = false;
                schrijven = false;
            } else if (recht.Equals("Lezen"))
            {
                lezen = true;
                schrijven = false;
            }
            else
            {
                lezen = true;
                schrijven = true;
            }
            db.UpdateOrInsertRecht(rolId, Convert.ToInt32(moduleId), lezen, schrijven);
            RechtenManager.Instance.UpdateRechten();
            RechtenGridView.DataSource = db.GetRechten();
            RechtenGridView.DataBind();
            ScriptManager.RegisterStartupScript(this, GetType(), "setActive",
                "setActive('rechten')", true);
        }

        protected void btnAdd_OnCommand(object sender, CommandEventArgs e)
        {
            var email = e.CommandArgument;
            var gebruiker = Gebruikers.Find(g => g.Email.Equals(email));
            if (gebruiker != null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "deleteGebruiker",
                "deleteGebruiker('" + gebruiker.Naam + "', " + gebruiker.Id + ")", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "setActive",
                        "setActive('gebruikers')", true);
            }
        }

        protected void btnChangeRole_OnClick(object sender, EventArgs e)
        {
            var userID = ddlGebruiker.SelectedValue;
            var roleID = ddlRecht.SelectedItem.Text;
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = manager.FindById(userID);
            manager.RemoveFromRoles(userID, manager.GetRoles(userID).ToArray());
            manager.AddToRole(userID, roleID);
            manager.Update(user);
            Gebruikers = db.GetGebruikers();
            var list2 = Gebruikers.Select(gebruiker => manager.FindByName(gebruiker.Email)).Select(user2 => new UserModel(user2, manager.GetRoles(user2.Id)[0])).ToList();
            GebruikersGridView.DataSource = list2;
            GebruikersGridView.DataBind();
            ScriptManager.RegisterStartupScript(this, GetType(), "setActive",
                        "setActive('gebruikers')", true);
        }

        protected void btnAddRole_OnClick(object sender, EventArgs e)
        {
            var name = txtRolNaam.Text;
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());
            roleManager.Create(new IdentityRole {Name = name});

            ddlRol.DataSource = roleManager.Roles.ToList();
            ddlRol.DataBind();
            ddlRecht.DataSource = roleManager.Roles.ToList();
            ddlRecht.DataBind();
            ddlRemoveRecht.DataSource = roleManager.Roles.ToList();
            ddlRemoveRecht.DataBind();

            ScriptManager.RegisterStartupScript(this, GetType(), "setActive",
                        "setActive('gebruikers')", true);
            txtRolNaam.Text = "";
        }

        protected void btnRemoveRole_OnClick(object sender, EventArgs e)
        {
            var roleID = ddlRemoveRecht.SelectedItem.Text;
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());
            var role = roleManager.Roles.ToList().Find(r => r.Name == roleID);
            if (role != null)
            {
                roleManager.Delete(role);
                db.DeleteRecht(role.Id);
            }
            ddlRol.DataSource = roleManager.Roles.ToList();
            ddlRol.DataBind();
            ddlRecht.DataSource = roleManager.Roles.ToList();
            ddlRecht.DataBind();
            ddlRemoveRecht.DataSource = roleManager.Roles.ToList();
            ddlRemoveRecht.DataBind();
            RechtenGridView.DataSource = db.GetRechten();
            RechtenGridView.DataBind();

            ScriptManager.RegisterStartupScript(this, GetType(), "setActive",
                        "setActive('gebruikers')", true);
        }
    }
}