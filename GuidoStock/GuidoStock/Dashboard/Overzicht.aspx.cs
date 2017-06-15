using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GuidoStock.App_Code;
using GuidoStock.Code;
using GuidoStock.Scannen;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace GuidoStock.Dashboard
{
    public partial class Overzicht : System.Web.UI.Page
    {
        public List<ScannenModel> EventsEnOrders
        {
            get { return (List<ScannenModel>)ViewState["EventsEnOrders"]; }
            set { ViewState["EventsEnOrders"] = value; }
        }

        public List<App_Code.Stock> Stocks
        {
            get { return (List<App_Code.Stock>)ViewState["Stocks"]; }
            set { ViewState["Stocks"] = value; }
        }

        public Artikel NieuwsteArtikel
        {
            get { return (Artikel)ViewState["NieuwsteArtikel"]; }
            set { ViewState["NieuwsteArtikel"] = value; }
        }

        public Artikel WijzigdeArtikel
        {
            get { return (Artikel)ViewState["WijzigdeArtikel"]; }
            set { ViewState["WijzigdeArtikel"] = value; }
        }

        public string NieuwArtikelUser
        {
            get { return (string)ViewState["NieuwArtikelUser"]; }
            set { ViewState["NieuwArtikelUser"] = value; }
        }

        public string WArtikelUser
        {
            get { return (string)ViewState["WArtikelUser"]; }
            set { ViewState["WArtikelUser"] = value; }
        }

        public Evenement NieuwsteEvent
        {
            get { return (Evenement)ViewState["NieuwsteEvent"]; }
            set { ViewState["NieuwsteEvent"] = value; }
        }

        public Evenement WijzigdeEvent
        {
            get { return (Evenement)ViewState["WijzigdeEvent"]; }
            set { ViewState["WijzigdeEvent"] = value; }
        }

        public string NieuwEventUser
        {
            get { return (string)ViewState["NieuwEventUser"]; }
            set { ViewState["NieuwEventUser"] = value; }
        }

        public string WEventUser
        {
            get { return (string)ViewState["WEventUser"]; }
            set { ViewState["WEventUser"] = value; }
        }

        public Wijziging WijzigingNieuwArtikel
        {
            get { return (Wijziging)ViewState["WijzigingNieuwArtikel"]; }
            set { ViewState["WijzigingNieuwArtikel"] = value; }
        }

        public Wijziging WijzigingArtikel
        {
            get { return (Wijziging)ViewState["WijzigingArtikel"]; }
            set { ViewState["WijzigingArtikel"] = value; }
        }

        public List<Wijziging> Wijzigingen
        {
            get { return (List<Wijziging>)ViewState["Wijzigingen"]; }
            set { ViewState["Wijzigingen"] = value; }
        }

        public List<VermistArtikel> VermisteArtikelens
        {
            get { return (List<VermistArtikel>)ViewState["VermisteArtikelen"]; }
            set { ViewState["VermisteArtikelen"] = value; }
        }

        public Recht Recht
        {
            get { return (Recht)ViewState["Recht"]; }
            set { ViewState["Recht"] = value; }
        }

        public int VerwijderLijn
        {
            get { return (int)ViewState["VerwijderLijn"]; }
            set { ViewState["VerwijderLijn"] = value; }
        }
        DBClass db = new DBClass();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();

            //Timeline
            var komendeEvenementen = db.GetHuidigeEvenementen();
            var komendeOrders = db.GetHuidigeOrders();
            EventsEnOrders = komendeEvenementen.Select(ev => new ScannenModel(ev, 0)).ToList();
            EventsEnOrders.AddRange(komendeOrders.Select(o => new ScannenModel(o, 0)));
            EventsEnOrders = EventsEnOrders.OrderBy(x => x.Datum).Take(10).ToList();

            //Verval
            Stocks = db.GetAllStocks();
            Stocks = Stocks.OrderBy(x => x.Vervaldatum).Where(x => x.Vervaldatum.ToShortDateString() != DateTime.MaxValue.ToShortDateString()).Where(x => x.Aantal > 0).Take(4).ToList();

            // User role
            var currentUser = manager.FindByName(HttpContext.Current.User.Identity.Name);
            if (currentUser == null)
            {
                RedirectToLogin();
                return;
            }
            Recht = db.GetRechtenForUserAndModule(currentUser.Id, "Dashboard");

            //Wijzigingen
            var wijzigingen = db.GetAllWijzigingen();
            if (wijzigingen.Count == 0) return;
            // Events
            var has = wijzigingen.Any(x => x.Type == "NEvent");
            if (!has) goto foo1;
            var wijzigingNieuwEvent = wijzigingen.Where(w => w.Type == "NEvent")
                .OrderByDescending(w => w.Time)
                .FirstOrDefault();
            NieuwEventUser = manager.FindByIdAsync(wijzigingNieuwEvent.UserId).Result.Name;
            NieuwsteEvent = db.GetEvenement(wijzigingNieuwEvent.Id);
            foo1: var has2 = wijzigingen.Any(x => x.Type == "WEvent");
            if (!has2) goto foo2;
            var wijzigingEvent = wijzigingen.Where(w => w.Type == "WEvent")
                .OrderByDescending(w => w.Time)
                .FirstOrDefault();
            WEventUser = manager.FindByIdAsync(wijzigingEvent.UserId).Result.Name;
            WijzigdeEvent = db.GetEvenement(wijzigingEvent.Id);
            //Artikels
            foo2: var has3 = wijzigingen.Any(x => x.Type == "NArtikel");
            if (!has3) goto foo3;
            WijzigingNieuwArtikel = wijzigingen.Where(w => w.Type == "NArtikel")
                .OrderByDescending(w => w.Time)
                .FirstOrDefault();
            NieuwsteArtikel = db.GetArtikel(WijzigingNieuwArtikel.Id);
            NieuwArtikelUser = manager.FindByIdAsync(WijzigingNieuwArtikel.UserId).Result.Name;
            foo3: var has4 = wijzigingen.Any(x => x.Type == "WArtikel");
            if (!has4) goto foo4;
            WijzigingArtikel = wijzigingen.Where(w => w.Type == "WArtikel")
                .OrderByDescending(w => w.Time)
                .FirstOrDefault();
            WijzigdeArtikel = db.GetArtikel(WijzigingArtikel.Id);
            WArtikelUser = manager.FindByIdAsync(WijzigingArtikel.UserId).Result.Name;
            //Gebeurtenissen
            foo4: Wijzigingen = wijzigingen.OrderByDescending(w => w.Time).GroupBy(item => new { item.Id, item.UserId, item.Type }).Select(group => @group.First()).Take(4).ToList();
            // Vermiste artikelen
            VermisteArtikelens = db.GetVermisteArtikelen();
            VermisteArtikelens.ForEach(x => x.UserName = manager.FindByIdAsync(x.UserId).Result.Name);
            VermisteArtikelens.Where(x => x.IsEvent).ForEach(x => x.Evenement = db.GetEvenement(x.EventId));
            VermisteArtikelens.Where(x => !x.IsEvent).ForEach(x => x.Order = db.GetOrder(x.EventId));
            VermisteArtikelen.DataSource = VermisteArtikelens;
            VermisteArtikelen.DataBind();
        }

        protected void btnAdd_OnCommand(object sender, CommandEventArgs e)
        {
            VerwijderLijn = int.Parse(e.CommandArgument.ToString());
            if (Recht.Schrijven)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "showWarning", "showWarning();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "showError", "showError();", true);
            }

        }

        protected void hdnVerwijder_OnValueChanged(object sender, EventArgs e)
        {
            db.DeleteVermistArtikel(VerwijderLijn);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "showConfirm", "showConfirm();", true);
        }

        private void RedirectToLogin()
        {
            string page = Page.Request.FilePath;
            if (page != "/Account/Login")
                Response.Redirect("~/Account/Login");
        }
    }
}