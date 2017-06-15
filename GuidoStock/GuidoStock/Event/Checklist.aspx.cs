using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using GuidoStock.App_Code;
using GuidoStock.Code;
using GuidoStock.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Net;
using System.Net.Mail;


namespace GuidoStock.Event
{
    public partial class Checklist : System.Web.UI.Page
    {
        private static DBClass _db = new DBClass();

        private List<Artikel> Stocklijst
        {
            get { return (List<Artikel>) ViewState["Stocklijst"]; }
            set { ViewState["Stocklijst"] = value; }
        }

        private Evenement Evenement
        {
            get { return (Evenement) ViewState["Evenement"]; }
            set { ViewState["Evenement"] = value; }
        }

        private List<EvenementLijn> EvenementLijnen
        {
            get { return (List<EvenementLijn>) ViewState["EvenementLijnen"]; }
            set { ViewState["EvenementLijnen"] = value; }
        }

        private List<AvailableModel> AvailableModels
        {
            get { return (List<AvailableModel>) ViewState["AvailableModels"]; }
            set { ViewState["AvailableModels"] = value; }
        }

        public int TotaalGewicht
        {
            get { return (int)ViewState["TotaalGewicht"]; }
            set { ViewState["TotaalGewicht"] = value; }
        }

        public int MaxGewicht
        {
            get { return (int)ViewState["MaxGewicht"]; }
            set { ViewState["MaxGewicht"] = value; }
        }

        public int Stopcontacten
        {
            get { return (int)ViewState["Stopcontacten"]; }
            set { ViewState["Stopcontacten"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                TotaalGewicht = 0;
                MaxGewicht = 0;
                Stopcontacten = 0;
                Stocklijst = _db.GetArtikelen();
                Stocklijst = Stocklijst.Where(a => (a.Aantal > 0 && a.IsVerwijderd == false)).ToList();
                string checklistId = Request.QueryString["id"];
                if (!string.IsNullOrEmpty(checklistId))
                {
                    int id;
                    bool parsed = int.TryParse(checklistId, out id);
                    if (parsed)
                    {
                        // Get evenement
                        Evenement = _db.GetEvenement(id);
                        if (Evenement == null)
                        {
                            // Er moet altijd een evenement zijn
                            Response.Redirect("Overzicht.aspx");
                            return;
                        }
                        // Try to fetch all items in checklist event. If no items -> new checklist
                        EvenementLijnen = _db.GetEvenementLijnen(id);
                        EvenementLijnen.ForEach(a => a.Evenement = Evenement);
                        ParseStockLijst();
                        ChecklistGridView.DataSource = EvenementLijnen;
                        ChecklistGridView.DataBind();
                        var array = EvenementLijnen.Select(a => a.Artikel.Id);

                        Stocklijst.RemoveAll(a => array.Contains(a.Id));
                        if (Stocklijst.Count > 0)
                        {
                            StocklijstGridView.DataSource = Stocklijst;
                            StocklijstGridView.DataBind();
                        }

                        EvenementId.Value = Evenement.Id.ToString();
                        GetEvenementTransport();
                        // TODO -> Stopcontact
                        float tempGewicht = 0.0f;
                        foreach (var lijn in EvenementLijnen)
                        {
                            tempGewicht += lijn.Artikel.Gewicht * lijn.Aantal;
                            Stopcontacten += lijn.Artikel.AantalStopcontacten * lijn.Aantal;
                        }
                        TotaalGewicht = (int)Math.Ceiling(tempGewicht);
                        if (TotaalGewicht > MaxGewicht)
                        {
                            ScriptManager.RegisterStartupScript(updatePanel, updatePanel.GetType(), "showWeightWarning",
                                "weightWarning();", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(updatePanel, updatePanel.GetType(), "DontShowWeightWarning", "dontWeightWarning();", true);
                        }

                    }
                }
                else
                {
                    Response.Redirect("Overzicht.aspx");
                }
            }
            


        }

        protected override void OnLoadComplete(EventArgs e)
        {
            if (TotaalGewicht > MaxGewicht)
            {
                ScriptManager.RegisterStartupScript(updatePanel, updatePanel.GetType(), "showWeightWarning",
                    "weightWarning();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(updatePanel, updatePanel.GetType(), "DontShowWeightWarning", "dontWeightWarning();", true);
            }
        }

        protected void GetEvenementTransport()
        {
            var transporten = _db.GetEvenementTransportenByEvenementId(Evenement);
            foreach (var trans in transporten)
            {
                MaxGewicht += trans.Transport.MaxGewicht;
            }
        }

        protected void OnClick(object sender, EventArgs e)
        {
            if (Evenement == null)
            {
                // Er moet altijd een evenement zijn
                Response.Redirect("Overzicht.aspx");
                return;
            }
            var evenementLijn = CheckForConflictingChanges();
            if (evenementLijn == null)
            {
                bool result = _db.UpdateEvenementLijnenByEvenement(EvenementLijnen, Evenement.Id);
                // TODO :: Hier moet eig iets getoond worden indien result niet true is. Moeilijk te testen.
                if (result)
                {
                    CheckArtikelVerantwoordelijke();
                    var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    var currentUser = manager.FindByName(HttpContext.Current.User.Identity.Name);
                    _db.AddWijziging(Evenement.Id, DateTime.Now, currentUser.Id, "NChecklist");

                    var huidige = _db.GetHuidigeEvenementen();
                    foreach (var evnt in huidige)
                    {
                        if (Evenement.Id == evnt.Id)
                        {
                            ScriptManager.RegisterStartupScript(updatePanel, updatePanel.GetType(), "showSuccess", "showSuccesModal(" + Evenement.Id + ",'komende');", true);
                            goto foo;
                        }
                    }
                    var verlopen = _db.GetVerlopenEvenementen();
                    foreach (var evnt in verlopen)
                    {
                        if (Evenement.Id == evnt.Id)
                        {
                            ScriptManager.RegisterStartupScript(updatePanel, updatePanel.GetType(), "showSuccess", "showSuccesModal(" + Evenement.Id + ",'verlopen');", true);
                            goto foo;
                        }
                    }
                    ScriptManager.RegisterStartupScript(updatePanel, updatePanel.GetType(), "showSuccess", "showSuccesModal(" + Evenement.Id + ",'alles');", true);


                    foo: Debug.WriteLine("check");
                    
                }
            }
            else
            {
                EvenementLijnen.Find(a => a.Artikel.Id == evenementLijn.Artikel.Id).Artikel.AvailableAantal = evenementLijn.Artikel.AvailableAantal;
                ChecklistGridView.DataSource = EvenementLijnen;
                ChecklistGridView.DataBind();
                string query = "openModal('" + evenementLijn.Artikel.Naam + "'," + evenementLijn.Artikel.AvailableAantal + ");";
                ScriptManager.RegisterStartupScript(updatePanel, updatePanel.GetType(), "showError", query, true);
            }
        }

        protected void CheckArtikelVerantwoordelijke()
        {
            var av = _db.GetArtikelVerantwoordelijken();
            foreach (var ln in EvenementLijnen)
            {
                foreach (var a in av)
                {
                    if (ln.Artikel.Id == a.Artikelid)
                    {
                        // Verstuur mail naar a.UserId
                        SendMail(a);
                    }
                }
            }

        }

        protected void SendMail(ArtikelVerantwoordelijke a)
        {
            var to = _db.GetGebruikerById(a.UserId);
            var artk = _db.GetArtikel(a.Artikelid);
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var currentUser = manager.FindByName(HttpContext.Current.User.Identity.Name);
            MailMessage msg = new MailMessage();
            msg.To.Add(new MailAddress(to.Email, to.Voornaam + " " + to.Achternaam));
            msg.From = new MailAddress("Admin@Guido.be", "Guido Stock");
            msg.Subject = ""+artk.Naam+" wordt gebruikt.";
            msg.Body = "<p>Een artikel waarvoor u verantwoordelijk bent, is zojuist in een checklist geplaatst. </p>" +
                       "<b>Artikel: </b> <a href='stock.guido.be/Stock/Overzicht.aspx?id=" + a.Artikelid +
                       "&type=alles'>" + artk.Naam + "</a><br />" +
                       "<b>Evenement: </b> <a href='stock.guido.be/Event/Overzicht.aspx?id=" +
                       EvenementLijnen[0].Evenement.Id + "&type=alles'>" + EvenementLijnen[0].Evenement.Naam + "</a><br />" +
                       "<b>Opgenomen door: </b>" + currentUser.FirstName + " " + currentUser.SurName;
            msg.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.Port = 25; 
            client.Host = "smtp.telenet.be";
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            try
            {
                client.Send(msg);
            }
            catch (Exception ex)
            {
            }
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

        private void VoegArtikelToe(int artikelId)
        {
            var index = EvenementLijnen.FindIndex(a => a.Artikel.Id == artikelId);
            if (index != -1) return;
            EvenementLijnen.Insert(0, new EvenementLijn(Evenement,
                       Stocklijst.Find(a => a.Id == artikelId), 1));
            ChecklistGridView.DataSource = EvenementLijnen;
            ChecklistGridView.DataBind();
            // Remove from stocklijst
            Stocklijst.RemoveAll(a => a.Id == artikelId);
            StocklijstGridView.DataSource = Stocklijst;
            StocklijstGridView.DataBind();
            // TODO -> Max gewicht / stopcontact
            TotaalGewicht = 0;
            Stopcontacten = 0;
            float tempGewicht = 0.0f;
            foreach (var lijn in EvenementLijnen)
            {
                tempGewicht += lijn.Artikel.Gewicht * lijn.Aantal;
                Stopcontacten += lijn.Artikel.AantalStopcontacten * lijn.Aantal;
            }
            TotaalGewicht = (int)Math.Ceiling(tempGewicht);
        }

        private void VerwijderArtikel(int artikelId)
        {
            // Remove from evenementLijn
            EvenementLijnen.RemoveAll(a => a.Artikel.Id == artikelId);
            ChecklistGridView.DataSource = EvenementLijnen;
            ChecklistGridView.DataBind();
            // Add to stocklijst
            var artikel = _db.GetArtikel(artikelId);
            Stocklijst.Insert(0, artikel);
            QuickParse(artikel);
            StocklijstGridView.DataSource = Stocklijst;
            StocklijstGridView.DataBind();
            // TODO -> Max gewicht / stopcontact
            TotaalGewicht = 0;
            Stopcontacten = 0;
            float tempGewicht = 0.0f;
            foreach (var lijn in EvenementLijnen)
            {
                tempGewicht += lijn.Artikel.Gewicht * lijn.Aantal;
                Stopcontacten += lijn.Artikel.AantalStopcontacten * lijn.Aantal;
            }
            TotaalGewicht = (int)Math.Ceiling(tempGewicht);
        }

        private void ParseStockLijst()
        {
            AvailableModels = _db.GetAvailableAantallenVoorEvenement(Evenement);
            AvailableModels.ForEach(model =>
            {
                var index = -1;
                if ((index = Stocklijst.FindIndex(a => a.Id == model.ArtikelId)) != -1)
                {
                    Stocklijst[index].AvailableAantal -= model.LijnAantal > model.AantalVervalt ? model.LijnAantal : model.AantalVervalt + model.LijnAantal;
                }
                index = -1;
                if ((index = EvenementLijnen.FindIndex(a => a.Artikel.Id == model.ArtikelId)) != -1)
                {
                    EvenementLijnen[index].Artikel.AvailableAantal -= model.LijnAantal > model.AantalVervalt ? model.LijnAantal : model.AantalVervalt + model.LijnAantal;
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
            } else if ((index = EvenementLijnen.FindIndex(a => a.Artikel.Id == artikel.Id)) != -1)
            {
                var model = AvailableModels.Find(a => a.ArtikelId == artikel.Id);
                EvenementLijnen[index].Artikel.AvailableAantal -= model.LijnAantal > model.AantalVervalt
                    ? model.LijnAantal
                    : model.AantalVervalt + model.LijnAantal;
            }
                
        }

        private EvenementLijn CheckForConflictingChanges()
        {
            // Fetch all overlapping evenementLijnen
            AvailableModels = _db.GetAvailableAantallenVoorEvenement(Evenement);
            // Check if nothing conflicts
            foreach (EvenementLijn t in EvenementLijnen)
            {
                if (AvailableModels.FindIndex(a => a.ArtikelId == t.Artikel.Id) != -1)
                {
                    var aantal = 0;
                    AvailableModels.Where(a => a.ArtikelId == t.Artikel.Id).ToList().ForEach(d =>
                    {
                        aantal += d.LijnAantal > d.AantalVervalt
                    ? d.LijnAantal
                    : d.AantalVervalt + d.LijnAantal;
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

        protected void ChecklistGridView_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            var txt = e.Row.FindControl("txtAantal") as TextBox;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("$(document).ready(function() {");
            sb.AppendLine("$(\"#" + txt.ClientID + "\").TouchSpin({");
            sb.AppendLine("verticalbuttons: true, max: " + ((EvenementLijn)e.Row.DataItem).Artikel.AvailableAantal);
            sb.AppendLine("});");
            sb.AppendLine("});");
            ScriptManager.RegisterStartupScript(updatePanel, updatePanel.GetType(), txt.ClientID, sb.ToString(), true);
        }

        protected void Annuleren_OnClick(object sender, EventArgs e)
        {
            var huidige = _db.GetHuidigeEvenementen();
            foreach (var evnt in huidige)
            {
                if (Evenement.Id == evnt.Id)
                {
                    Response.Redirect("Overzicht.aspx?id=" + Evenement.Id + "&type=komende");
                    goto foo;
                }
            }
            var verlopen = _db.GetVerlopenEvenementen();
            foreach (var evnt in verlopen)
            {
                if (Evenement.Id == evnt.Id)
                {
                    Response.Redirect("Overzicht.aspx?id=" + Evenement.Id + "&type=verlopen");
                    goto foo;
                }
            }
            Response.Redirect("Overzicht.aspx?id=" + Evenement.Id);
           
            foo: Debug.WriteLine("check");
            
        }

        protected void hdnAantal_OnValueChanged(object sender, EventArgs e)
        {
            var hdn = (HiddenField) sender;
            string hdn2 = hdn.Value;
            int aantal = int.Parse(hdn2.Split('¬')[0]);
            int id = int.Parse(hdn2.Split('¬')[1]);
            EvenementLijnen.Find(a => a.Artikel.Id == id).Aantal = aantal;
            ChecklistGridView.DataSource = EvenementLijnen;
            ChecklistGridView.DataBind();
            // TODO -> Max gewicht / stopcontact
            TotaalGewicht = 0;
            Stopcontacten = 0;
            float tempGewicht = 0.0f;
            foreach (var lijn in EvenementLijnen)
            {
                tempGewicht += lijn.Artikel.Gewicht * lijn.Aantal;
                Stopcontacten += lijn.Artikel.AantalStopcontacten * lijn.Aantal;
            }
            TotaalGewicht = (int) Math.Ceiling(tempGewicht);
        }
    }
}