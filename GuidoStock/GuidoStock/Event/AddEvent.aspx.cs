using GuidoStock.App_Code;
using GuidoStock.Code;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GuidoStock.Controls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace GuidoStock.Event
{
    public partial class AddEvent : System.Web.UI.Page
    {
        private readonly DBClass _db = new DBClass();

        List<TextBox> beginData;
        List<TextBox> eindData;
        List<ListBox> medewerkers;

        public Evenement Evenement
        {
            get { return (Evenement) ViewState["Evenement"]; }
            set { ViewState["Evenement"] = value; }
        }

        private List<Gebruiker> GebruikersList
        {
            get { return (List<Gebruiker>)ViewState["GebruikersList"]; }
            set { ViewState["GebruikersList"] = value; }
        } 

        protected void Page_Load(object sender, EventArgs e)
        {
            beginData = new List<TextBox>() { BegintijdLaden, BegintijdOpbouw, BegintijdActie, BegintijdAfbouw };
            eindData = new List<TextBox>() { EindtijdLaden, EindtijdOpbouw, EindtijdActie, EindtijdAfbouw };
            medewerkers = new List<ListBox>() { ddlMedewerkersLaden, ddlMedewerkersOpbouw, ddlMedewerkersActie, ddlMedewerkersAfbouw };
            if (IsPostBack) return;

            Evenement = new Evenement();

            if (Request.QueryString["id"] != null)
            {
                var id = 0;
                if (int.TryParse(Request.QueryString["id"], out id)) Evenement = _db.GetEvenement(id);
            }

            if (Evenement == null)
            {
                Evenement = new Evenement
                {
                    EvenementLocaties = new List<EvenementLocatie>(),
                    EvenementTransporten = new List<EvenementTransport>()
                    {
                        new EvenementTransport()
                        {
                            ChauffeurHeen = new Gebruiker(),
                            ChauffeurTerug = new Gebruiker(),
                            Evenement = new Evenement(),
                            Transport = new Transport()
                        }
                    },
                    EvenementTeamLeden = new List<EvenementTeamLid>() {new EvenementTeamLid()}
                };
            }

            transportRepeater.DataSource = Evenement.EvenementTransporten;
            transportRepeater.DataBind();
            teamlidRepeater.DataSource = Evenement.EvenementTeamLeden;
            teamlidRepeater.DataBind();
            organisatieRepeater.DataSource = Evenement.EvenementLocaties;
            organisatieRepeater.DataBind();
            GebruikersList = _db.GetGebruikers();
            ddlOpdrachtgever.DataSource = GebruikersList;
            ddlOpdrachtgever.DataBind();

            ddlCoordinator.DataSource = GebruikersList;
            ddlCoordinator.DataBind();

            ddlFieldManager.DataSource = GebruikersList;
            ddlFieldManager.DataBind();

            ddlMedewerkersLaden.DataSource = GebruikersList;
            ddlMedewerkersLaden.DataBind();

            ddlMedewerkersOpbouw.DataSource = GebruikersList;
            ddlMedewerkersOpbouw.DataBind();

            ddlMedewerkersAfbouw.DataSource = GebruikersList;
            ddlMedewerkersAfbouw.DataBind();

            ddlMedewerkersActie.DataSource = GebruikersList;
            ddlMedewerkersActie.DataBind();
            if (Evenement.Id != 0)
                UpdateUI();
        }

        //protected void changeValGroup_ValueChanged(object sender, EventArgs e)
        //{
        //    var id = changeValGroup.Value;
        //    switch (id)
        //    {
        //        case "1":
        //            btnStep1.ValidationGroup = "group1";
        //            btnStep2.ValidationGroup = "group1";
        //            btnStep3.ValidationGroup = "group1";
        //            break;
        //        case "2":
        //            btnStep1.ValidationGroup = "group2";
        //            btnStep2.ValidationGroup = "group2";
        //            btnStep3.ValidationGroup = "group2";
        //            break;
        //        case "3":
        //            btnStep1.ValidationGroup = "group3";
        //            btnStep2.ValidationGroup = "group3";
        //            btnStep3.ValidationGroup = "group3";
        //            break;
        //    }
        //}

        protected void SaveEvent_Click(object sender, EventArgs e)
        {
            var a = 0;
            foreach (var control in from RepeaterItem item in transportRepeater.Items select item.FindControl("TransportControl") as TransportControl)
            {
                control.UpdateModel();
                Evenement.EvenementTransporten[a++] = control.Test;
            }

            var c = 0;
            foreach (var control in from RepeaterItem item in teamlidRepeater.Items select item.FindControl("TeamlidControl") as TeamlidControl)
            {
                control.UpdateModel();
                Evenement.EvenementTeamLeden[c++] = control.TeamLid;
            }
            // LOCATIE
            foreach (var evenementLocatie in Evenement.EvenementLocaties)
            {
                if (evenementLocatie.Locatie.Id == int.MaxValue)
                {
                    evenementLocatie.Locatie.Id = _db.AddLocatie(evenementLocatie.Locatie.Straat, evenementLocatie.Locatie.Postcode,
                        evenementLocatie.Locatie.Land, evenementLocatie.Locatie.Plaats, evenementLocatie.Locatie.Zaal);
                }

                var blnEqualLocatie = _db.GetLocaties().Any(loc => loc.Equals(evenementLocatie.Locatie));
                if (!blnEqualLocatie)
                {
                    _db.UpdateLocatie(evenementLocatie.Locatie.Id, evenementLocatie.Locatie.Straat, evenementLocatie.Locatie.Postcode, evenementLocatie.Locatie.Land, evenementLocatie.Locatie.Plaats, evenementLocatie.Locatie.Zaal);
                }

                if (evenementLocatie.EvenementKlant.Id == int.MaxValue)
                {
                    evenementLocatie.EvenementKlant.Id = _db.AddEvenementKlant(evenementLocatie.EvenementKlant.Organisatie,
                        evenementLocatie.EvenementKlant.ContactNaam, evenementLocatie.EvenementKlant.ContactTel, "");
                }

                var blnEqualKlant = false;
                var blnEqualContact = false;

                foreach (var k in _db.GetEvenementKlanten())
                {
                    if (k.ContactTel == evenementLocatie.EvenementKlant.ContactTel)
                        blnEqualContact = true;
                    if (k.Equals(evenementLocatie.EvenementKlant))
                        blnEqualKlant = true;
                }

                if (!blnEqualContact && !blnEqualKlant)
                {
                    evenementLocatie.EvenementKlant.Id = _db.AddEvenementKlant(evenementLocatie.EvenementKlant.Organisatie, evenementLocatie.EvenementKlant.ContactNaam, evenementLocatie.EvenementKlant.ContactTel, "");
                }
                else if (!blnEqualKlant)
                {
                    evenementLocatie.EvenementKlant.Id = _db.AddEvenementKlant(evenementLocatie.EvenementKlant.Organisatie, evenementLocatie.EvenementKlant.ContactNaam, evenementLocatie.EvenementKlant.ContactTel, "");
                }
                else if (!blnEqualContact)
                {
                    _db.UpdateEvenementKlant(evenementLocatie.EvenementKlant.Organisatie, evenementLocatie.EvenementKlant.ContactNaam, evenementLocatie.EvenementKlant.ContactTel, "");
                }
            }
            // EVENT
            bool exist = false;
            var trnsprtdt = Evenement.EvenementTransporten.Count != 0 ? Evenement.EvenementTransporten.Select(d => d.Vertrek).Min() : DateTime.MaxValue;
            var opmerking = !string.IsNullOrEmpty(Opmerking.Text) ? Opmerking.Text : "null";
            int eventId;
            if (Evenement.Id != 0)
            {
                exist = true;
                eventId = Evenement.Id;
                _db.UpdateEvenement(NieuwEventNaam.Text, opmerking, trnsprtdt,
                int.Parse(ddlOpdrachtgever.SelectedValue), int.Parse(ddlCoordinator.SelectedValue),
                int.Parse(ddlFieldManager.SelectedValue), Evenement.Id);
            }
            else
            {
                exist = false;
                eventId = _db.AddEvenement(NieuwEventNaam.Text, opmerking, trnsprtdt,
                int.Parse(ddlOpdrachtgever.SelectedValue), int.Parse(ddlCoordinator.SelectedValue),
                int.Parse(ddlFieldManager.SelectedValue));
            }

            // EVENT LOCATIE
            foreach (var organisatie in Evenement.EvenementLocaties)
            {
                _db.AddEvenementLocatie(eventId, organisatie.Locatie.Id, organisatie.BeginTijd, organisatie.EindTijd, organisatie.VerwachteOpkomst , organisatie.Target, organisatie.EvenementKlant.Id);
            }

            //EVENEMENTTAAK
            var takenList = new List<EvenementTaak>();
            for (var i = 0; i< beginData.Count; i++)
            {
                if (!string.IsNullOrEmpty(beginData[i].Text) && !string.IsNullOrEmpty(medewerkers[i].Text))
                {
                    var dat = string.IsNullOrEmpty(eindData[i].Text) ? new DateTime() : DateTime.Parse(eindData[i].Text);
                    takenList.AddRange(medewerkers[i].GetSelectedIndices().Select(gebruiker => new EvenementTaak(DateTime.Parse(beginData[i].Text), dat, i + 1, eventId, int.Parse(medewerkers[i].Items[gebruiker].Value))));
                }

                var b = _db.UpdateEvenementTaak(takenList, eventId);
            }

            // EVENEMENTTRANSPORTEN
            if (Evenement.EvenementTransporten.Count != 0)
            {
                _db.UpdateEvenementTransporten(Evenement.EvenementTransporten, eventId);
            }

            // TEAMLEDEN
            _db.UpdateEvenementTeammleden(Evenement.EvenementTeamLeden, eventId);

            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var currentUser = manager.FindByName(HttpContext.Current.User.Identity.Name);
            _db.AddWijziging(eventId, DateTime.Now, currentUser.Id, exist ? "WEvent" : "NEvent");
            if (!exist)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertUser",
                    "BootstrapDialog.show({title: 'Geslaagd!', message: 'Het event is toegevoegd.', buttons: [{ label: 'Sluiten', action: function(){ window.location.href = 'Overzicht.aspx?id=" +
                    eventId + "';}}]}); ", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertUser",
                    "BootstrapDialog.show({title: 'Geslaagd!', message: 'Het event is gewijzigd.', buttons: [{ label: 'Sluiten', action: function(){ window.location.href = 'Overzicht.aspx?id=" +
                    eventId + "';}}]}); ", true);
            }

        }

        protected void hdnMedewerkersLaden_ValueChanged(object sender, EventArgs e)
        {
            var control = sender as Control;
            if (control == null) return;
            var id = control.ClientID;
            var lst = new ListBox();
            if (id == hdnMedewerkersLaden.ClientID) lst = ddlMedewerkersLaden;
            else if(id == hdnMedewerkersAfbouw.ClientID) lst = ddlMedewerkersAfbouw;
            else if (id == hdnMedewerkersActie.ClientID) lst = ddlMedewerkersActie;
            else if (id == hdnMedewerkersOpbouw.ClientID) lst = ddlMedewerkersOpbouw;


            var gebruikers = ((HiddenField) sender).Value;
            if (string.IsNullOrEmpty(gebruikers)) return;
            var gebruikersList = gebruikers.Split(',');
            foreach (var b in gebruikersList)
            {
                lst.Items.FindByValue(b).Selected = true;
            }
        }

        protected void kenmerkToevoegen_OnClick(object sender, EventArgs e)
        {
            organisatie1.UpdateModel();
            var locatie = organisatie1.LocatieModel;
            int index;
            if ((index = Evenement.EvenementLocaties.FindIndex(o => o.TempIndex == locatie.TempIndex)) != -1)
                Evenement.EvenementLocaties[index] = locatie;
            else
            {
                locatie.TempIndex = Evenement.EvenementLocaties.Count;
                Evenement.EvenementLocaties.Add(locatie);
            }
            
            organisatie1.LocatieModel = new EvenementLocatie() {EvenementKlant = new EvenementKlant(), Locatie = new Locatie(), TempIndex = -1};
            organisatie1.ResetUI();

            organisatieRepeater.DataSource = Evenement.EvenementLocaties;
            organisatieRepeater.DataBind();


        }

        protected void LinkButton3_OnClick(object sender, EventArgs e)
        {
            var i = 0;
            foreach (var control in from RepeaterItem item in transportRepeater.Items select item.FindControl("TransportControl") as TransportControl)
            {
                control.UpdateModel();
                Evenement.EvenementTransporten[i++] = control.Test;
            }
            Evenement.EvenementTransporten.Add(new EvenementTransport() { ChauffeurHeen = new Gebruiker(), ChauffeurTerug = new Gebruiker(), Evenement = new Evenement(), Transport = new Transport()});
            transportRepeater.DataSource = Evenement.EvenementTransporten;
            transportRepeater.DataBind();
        }

        protected void LinkButton7_OnClick(object sender, EventArgs e)
        {
            var i = 0;
            foreach ( var control in from RepeaterItem item in teamlidRepeater.Items select item.FindControl("TeamlidControl") as TeamlidControl)
            {
                control.UpdateModel();
                Evenement.EvenementTeamLeden[i++] = control.TeamLid;
            }
            Evenement.EvenementTeamLeden.Add(new EvenementTeamLid());
            teamlidRepeater.DataSource = Evenement.EvenementTeamLeden;
            teamlidRepeater.DataBind();
        }

        protected void OnCommand(object sender, CommandEventArgs e)
        {
            var tempIndex = int.Parse((string)e.CommandArgument);
            organisatie1.LocatieModel = Evenement.EvenementLocaties.Find(o => o.TempIndex == tempIndex);
            organisatie1.UpdateUI();
        }

        private void UpdateUI()
        {
            // Details
            NieuwEventNaam.Text = Evenement.Naam;
            Datum.Text = Evenement.Datum.ToString();
            Debug.WriteLine(ddlOpdrachtgever.SelectedIndex);
            ddlOpdrachtgever.SelectedValue = Evenement.Opdrachtgever.Id.ToString();
            ddlCoordinator.SelectedValue = Evenement.EventCoordinator.Id.ToString();
            ddlFieldManager.SelectedValue = Evenement.Fieldmanager.Id.ToString();

            #region{Planning}
            // Planning
            EvenementTaak laden, opbouw, actie, afbouw;
            if ((laden = Evenement.EvenementTaken.Find(t => t.Taak.Id == 1)) != null)
            {
                BegintijdLaden.Text = laden.Van.ToString("yyyy-MM-dd H:mm");
                EindtijdLaden.Text = laden.Tot.ToString("yyyy-MM-dd H:mm");
            }

            if ((opbouw = Evenement.EvenementTaken.Find(t => t.Taak.Id == 2)) != null)
            {
                BegintijdOpbouw.Text = opbouw.Van.ToString("yyyy-MM-dd H:mm");
                EindtijdOpbouw.Text = opbouw.Tot.ToString("yyyy-MM-dd H:mm");
            }

            if ((actie = Evenement.EvenementTaken.Find(t => t.Taak.Id == 3)) != null)
            {
                BegintijdActie.Text = actie.Van.ToString("yyyy-MM-dd H:mm");
                EindtijdActie.Text = actie.Tot.ToString("yyyy-MM-dd H:mm");
            }

            if ((afbouw = Evenement.EvenementTaken.Find(t => t.Taak.Id == 4)) != null)
            {
                BegintijdAfbouw.Text = afbouw.Van.ToString("yyyy-MM-dd H:mm");
                EindtijdAfbouw.Text = afbouw.Tot.ToString("yyyy-MM-dd H:mm");
            }
            Evenement.EvenementTaken.FindAll(t => t.Taak.Id == 1).ForEach(t =>
            {
                hdnMedewerkersLaden.Value += t.Gebruiker.Id.ToString() + ",";
            });
            if (hdnMedewerkersLaden.Value.Count() != 0)
                hdnMedewerkersLaden.Value = hdnMedewerkersLaden.Value.Remove(hdnMedewerkersLaden.Value.Count() - 1, 1);
            hdnMedewerkersLaden_ValueChanged(hdnMedewerkersLaden, null);
            Evenement.EvenementTaken.FindAll(t => t.Taak.Id == 2).ForEach(t =>
            {
                hdnMedewerkersOpbouw.Value += t.Gebruiker.Id.ToString() + ",";
            });
            if (hdnMedewerkersOpbouw.Value.Count() != 0)
                hdnMedewerkersOpbouw.Value = hdnMedewerkersOpbouw.Value.Remove(hdnMedewerkersOpbouw.Value.Count() - 1, 1);
            hdnMedewerkersLaden_ValueChanged(hdnMedewerkersOpbouw, null);
            Evenement.EvenementTaken.FindAll(t => t.Taak.Id == 3).ForEach(t =>
            {
                hdnMedewerkersActie.Value += t.Gebruiker.Id.ToString() + ",";
            });
            if (hdnMedewerkersActie.Value.Count() != 0)
                hdnMedewerkersActie.Value = hdnMedewerkersActie.Value.Remove(hdnMedewerkersActie.Value.Count() - 1, 1);
            hdnMedewerkersLaden_ValueChanged(hdnMedewerkersActie, null);
            Evenement.EvenementTaken.FindAll(t => t.Taak.Id == 4).ForEach(t =>
            {
                hdnMedewerkersAfbouw.Value += t.Gebruiker.Id.ToString() + ",";
            });
            if (hdnMedewerkersAfbouw.Value.Count() != 0)
                hdnMedewerkersAfbouw.Value = hdnMedewerkersAfbouw.Value.Remove(hdnMedewerkersAfbouw.Value.Count() - 1, 1);
            hdnMedewerkersLaden_ValueChanged(hdnMedewerkersAfbouw, null);
            Opmerking.Text = Evenement.Opmerking;
            #endregion


        }

      
        public bool IsGroupValid(string sValidationGroup)
        {
            Page.Validate(sValidationGroup);
            foreach (BaseValidator validator in Page.GetValidators(sValidationGroup))
            {
                if (!validator.IsValid)
                {
                    return false;
                }
            }
            return true;
        }
    }
}