using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GuidoStock.App_Code;
using GuidoStock.Code;

namespace GuidoStock.Controls
{
    public partial class Organisatie : System.Web.UI.UserControl
    {
        private string OrganisatieOud
        {
            get { return (string) ViewState["OrganisatieOud"]; }
            set { ViewState["OrganisatieOud"] = value; }
        }

        public EvenementLocatie LocatieModel
        {
            get { return (EvenementLocatie)ViewState["EvenementLocatie"]; }
            set { ViewState["EvenementLocatie"] = value; }
        }

        private List<EvenementKlant> KlantenLijst
        {
            get { return (List<EvenementKlant>) ViewState["KlantenLijst"]; }
            set { ViewState["KlantenLijst"] = value; }
        }
 
        private readonly DBClass _db = new DBClass();
        private List<Locatie> LocatieList = new List<Locatie>();

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                if (KlantenLijst == null)
                {
                    KlantenLijst = new List<EvenementKlant>();
                }
                var klantLeeg = new EvenementKlant
                {
                    Organisatie = "Selecteer een organisatie",
                    Id = 0
                };
                KlantenLijst.Add(klantLeeg);
                KlantenLijst.AddRange(_db.GetEvenementKlantenDistinct());
                ddlOrganisatieNaam.DataSource = KlantenLijst;
                LocatieModel = new EvenementLocatie { EvenementKlant = new EvenementKlant(), Locatie = new Locatie(), TempIndex = -1};
                LocatieList = _db.GetLocaties();
                // Org = new EvenementKlant();
                
                ddlCountries.DataSource = IsoCountries();
                ddlCountries.DataBind();

                var klantenList = new List<EvenementKlant>();
                
                
                ddlOrganisatieNaam.DataBind();

                var contactenlist = new List<EvenementKlant>();
                var contactleeg = new EvenementKlant
                {
                    ContactNaam = "Voeg een contact toe",
                    Id = 0
                };
                contactenlist.Add(contactleeg);

                ddlContactpersoon.DataSource = contactenlist;
                ddlContactpersoon.DataBind();
            }
        }

        private static List<Country> IsoCountries()
        {

            var belgium = new Country("BE", "België");
            var nederland = new Country("NL", "Nederland");
            var frankrijk = new Country("FR", "Frankrijk");
            var duitsland = new Country("DE", "Duitsland");

            var countries = new List<Country> { belgium, nederland, frankrijk, duitsland };

            return countries;
        }

        protected void hdnOrganisatie_ValueChanged(object sender, EventArgs e)
        {
            var nieuOrgan = hdnOrganisatie.Value;
            OrganisatieOud = hdnOrganisatie2.Value;
            LocatieModel.EvenementKlant.Organisatie = OrganisatieOud;
            if (!Regex.IsMatch(nieuOrgan, @"^\d+$"))
            {
                var klantenList = new List<EvenementKlant>();             
                LocatieModel.EvenementKlant.Id = int.MaxValue;
                LocatieModel.EvenementKlant.Organisatie = OrganisatieOud;

                klantenList.Add(LocatieModel.EvenementKlant);

                klantenList.AddRange(_db.GetEvenementKlantenDistinct());

                ddlOrganisatieNaam.DataSource = klantenList;
                ddlOrganisatieNaam.DataBind();


                ddlContactpersoon.Items.Clear();
                var list = new List<EvenementKlant>() {new EvenementKlant() {Id = int.MaxValue, ContactNaam = "Voeg een contact toe"} };
                ddlContactpersoon.DataSource = list;
                ddlContactpersoon.DataBind();
                telContact.Text = "";

                Plaats.Text = "";
                Adres.Text = "";
                Postcode.Text = "";
                ddlCountries.SelectedIndex = 0;

                var allLocaties = new List<Locatie>();

                var locatieLeeg = new Locatie
                {
                    Id = 0,
                    Zaal = "Selecteer een zaal"
                };

                allLocaties.Add(locatieLeeg);

                allLocaties.AddRange(_db.GetLocaties());
                ddlZaal.DataSource = allLocaties;
                ddlZaal.DataBind();

            }
            else
            {
                if (nieuOrgan != "0" && nieuOrgan != int.MaxValue.ToString())
                {
                    var contactpersonen = _db.GetEvenementKlantenByOrganisatie(OrganisatieOud);
                    var klantenList = new List<EvenementKlant>();
                    var klantLeeg = new EvenementKlant
                    {
                        ContactNaam = "Selecteer een naam",
                        Id = 0
                    };
                    klantenList.Add(klantLeeg);
                    klantenList.AddRange(contactpersonen);
                    ddlContactpersoon.DataSource = klantenList;
                    ddlContactpersoon.DataBind();
                    GetLocaties(OrganisatieOud);
                }
                else
                {
                    var contactenlist = new List<EvenementKlant>();
                    var contactleeg = new EvenementKlant
                    {
                        ContactNaam = "Voeg een contact toe",
                        Id = 0
                    };
                    contactenlist.Add(contactleeg);

                    ddlContactpersoon.DataSource = contactenlist;
                    ddlContactpersoon.DataBind();
                    telContact.Text = "";
                    ddlZaal.Items.Clear();
                    Plaats.Text = "";
                    Adres.Text = "";
                    Postcode.Text = "";
                    ddlCountries.SelectedIndex = 0;
                }
            }
        }

        protected void hdnContactpersoon_ValueChanged(object sender, EventArgs e)
        {
            var nieuwContact = hdnContactpersoon.Value;
            LocatieModel.EvenementKlant.ContactNaam = hdnContactpersoon2.Value;
            // LocatieModel.EvenementKlant.ContactTel = nieuwContact;
            switch (nieuwContact)
            {
                case "NieuweContactpersoon":
                    var klantenList = new List<EvenementKlant> {LocatieModel.EvenementKlant};
                    ddlContactpersoon.DataSource = klantenList;
                    ddlContactpersoon.DataBind();
                    telContact.Text = "";
                    break;
                case "0":
                    telContact.Text = "";
                    break;
                default:
                    telContact.Text = nieuwContact;
                    break;
            }
        }

        protected void hdnZaal_ValueChanged(object sender, EventArgs e)
        {
            var zaal = hdnZaal.Value;
            LocatieModel.Locatie.Zaal = ddlZaal.Text;
            if (!Regex.IsMatch(zaal, @"^\d+$"))
            {
                var locatieList = new List<Locatie>();

                var loc = new Locatie
                {
                    Id = int.MaxValue,
                    Zaal = zaal
                };

                locatieList.Add(loc);

                locatieList.AddRange(LocatieList);

                ddlZaal.DataSource = locatieList;
                ddlZaal.DataBind();

                Plaats.Text = "";
                Adres.Text = "";
                Postcode.Text = "";
                ddlCountries.SelectedIndex = 0;
            }
            else
            {
                if (zaal == "0")
                {
                    Plaats.Text = "";
                    Adres.Text = "";
                    Postcode.Text = "";
                    ddlCountries.SelectedIndex = 0;
                }
                else
                {
                    var allLocaties = FillLocaties(OrganisatieOud, 0);
                    foreach (var loc in allLocaties.Where(loc => loc.Id == int.Parse(zaal)))
                    {
                        Plaats.Text = loc.Plaats;
                        Adres.Text = loc.Straat;
                        Postcode.Text = loc.Postcode;
                        ddlCountries.DataSource = IsoCountries();
                        ddlCountries.DataBind();
                        ddlCountries.Items.FindByText(loc.Land).Selected = true;
                    }
                }
            }
        }

        private void GetLocaties(string organisatie)
        {
            var allLocaties = FillLocaties(organisatie, 1);
            ddlZaal.DataSource = allLocaties;
            ddlZaal.DataBind();
        }

        private List<Locatie> FillLocaties(string organisatie, int count)
        {
            var locaties = _db.GetLocaties();
            var organisatieLocaties = _db.GetLocatiesByOrganisatie(organisatie);
            var allLocaties = new List<Locatie>();

            if (count == 1)
            {
                var locatieLeeg = new Locatie
                {
                    Id = 0,
                    Zaal = "Selecteer een zaal"
                };

                allLocaties.Add(locatieLeeg);
            }

            var dupesList = organisatieLocaties.GroupBy(elem => elem.Zaal).Select(group => @group.First());

            var collection = dupesList as IList<Locatie> ?? dupesList.ToList();
            allLocaties.AddRange(collection);

            foreach (var loc in locaties)
            {
                var blnCheck = false;
                foreach (var locatie in collection.Where(locatie => loc.Zaal == locatie.Zaal))
                {
                    blnCheck = true;
                }
                if (!blnCheck)
                {
                    allLocaties.Add(loc);
                }
            }
            return allLocaties;
        }

        public void UpdateModel()
        {
            //LocatieModel.EvenementKlant.Id = hdnOrganisatie.Value == "NieuweContactpersoon" ? int.MaxValue : Convert.ToInt32(hdnOrganisatie.Value);
            var test = 0;
            LocatieModel.EvenementKlant.Id = !int.TryParse(hdnOrganisatie.Value, out test) ? int.MaxValue : test;
            LocatieModel.EvenementKlant.Organisatie = hdnOrganisatie2.Value;
            LocatieModel.EvenementKlant.ContactTel = telContact.Text;
            LocatieModel.EvenementKlant.ContactNaam = hdnContactpersoon2.Value;
            var test2 = 0;
            LocatieModel.EvenementKlant.Id = !int.TryParse(hdnZaal.Value, out test2) ? int.MaxValue : test;
            if (!string.IsNullOrEmpty(ddlZaal.SelectedValue))
            {
                LocatieModel.Locatie.Id = Convert.ToInt32(ddlZaal.SelectedValue);
                LocatieModel.Locatie.Zaal = ddlZaal.SelectedItem.Text;
            }
            LocatieModel.Locatie.Straat = Adres.Text;
            LocatieModel.Locatie.Plaats = Plaats.Text;
            LocatieModel.Locatie.Postcode = Postcode.Text;
            if (!string.IsNullOrEmpty(ddlCountries.SelectedValue))
                LocatieModel.Locatie.Land = ddlCountries.SelectedItem.Text;
            if (!string.IsNullOrEmpty(Begintijd.Text))
                LocatieModel.BeginTijd = Convert.ToDateTime(Begintijd.Text);
            if (!string.IsNullOrEmpty(Eindtijd.Text))
                LocatieModel.EindTijd = Convert.ToDateTime(Eindtijd.Text);
            if (!string.IsNullOrEmpty(Opkomst.Text))
                LocatieModel.VerwachteOpkomst = Convert.ToInt32(Opkomst.Text);
            if (!string.IsNullOrEmpty(Target.Text))
                LocatieModel.Target = Convert.ToInt32(Target.Text);
        }

        public void UpdateUI()
        {
            if (LocatieModel.EvenementKlant.Id == int.MaxValue)
            {
                hdnOrganisatie.Value = "NieuweContactpersoon";
                hdnOrganisatie2.Value = LocatieModel.EvenementKlant.Organisatie;
                hdnOrganisatie_ValueChanged(hdnOrganisatie, null);
                hdnContactpersoon.Value = "NieuweContactpersoon";
                hdnContactpersoon2.Value = LocatieModel.EvenementKlant.ContactNaam;
                hdnContactpersoon_ValueChanged(hdnContactpersoon, null);
                telContact.Text = LocatieModel.EvenementKlant.ContactTel;
                hdnZaal.Value = LocatieModel.Locatie.Zaal;
                hdnZaal_ValueChanged(hdnZaal, null);
            }
            else
            {
                hdnOrganisatie.Value = LocatieModel.EvenementKlant.Id.ToString();
                hdnOrganisatie2.Value = LocatieModel.EvenementKlant.Organisatie;
                ddlOrganisatieNaam.SelectedValue = LocatieModel.EvenementKlant.Id.ToString();
                hdnOrganisatie_ValueChanged(hdnOrganisatie, null);
                hdnContactpersoon.Value = LocatieModel.EvenementKlant.ContactTel;
                hdnContactpersoon2.Value = LocatieModel.EvenementKlant.ContactNaam;
                ddlContactpersoon.SelectedValue = LocatieModel.EvenementKlant.ContactTel;
                hdnContactpersoon_ValueChanged(hdnContactpersoon, null);
                telContact.Text = LocatieModel.EvenementKlant.ContactTel;
                hdnZaal.Value = LocatieModel.Locatie.Id.ToString();
                ddlZaal.SelectedValue = LocatieModel.Locatie.Id.ToString();
                hdnZaal_ValueChanged(hdnZaal, null);
            }

            Adres.Text = LocatieModel.Locatie.Straat;
            Plaats.Text = LocatieModel.Locatie.Plaats;
            Postcode.Text = LocatieModel.Locatie.Postcode;
            Begintijd.Text = LocatieModel.BeginTijd.ToString("yyyy-MM-dd H:mm");
            Eindtijd.Text = LocatieModel.EindTijd.ToString("yyyy-MM-dd H:mm");
            Opkomst.Text = LocatieModel.VerwachteOpkomst == -1 ? "": LocatieModel.VerwachteOpkomst.ToString();
            Target.Text = LocatieModel.Target == -1 ? "" : LocatieModel.Target.ToString();
        }

        public void ResetUI()
        {
            ddlZaal.DataSource = new List<Locatie>();
            ddlZaal.DataBind();
            telContact.Text = "";
            Adres.Text = "";
            Plaats.Text = "";
            Postcode.Text = "";
            Opkomst.Text = "";
            Target.Text = "";
            Begintijd.Text = DateTime.Now.ToString("yyyy-MM-dd H:mm");
            Eindtijd.Text = DateTime.Now.ToString("yyyy-MM-dd H:mm");

            KlantenLijst = new List<EvenementKlant>();
            var klantLeeg = new EvenementKlant
            {
                Organisatie = "Selecteer een organisatie",
                Id = 0
            };
            KlantenLijst.Add(klantLeeg);
            KlantenLijst.AddRange(_db.GetEvenementKlantenDistinct());
            ddlOrganisatieNaam.DataSource = KlantenLijst;

            LocatieList = _db.GetLocaties();

            ddlOrganisatieNaam.DataBind();

            var contactenlist = new List<EvenementKlant>();
            var contactleeg = new EvenementKlant
            {
                ContactNaam = "Voeg een contact toe",
                Id = 0
            };
            contactenlist.Add(contactleeg);

            ddlContactpersoon.DataSource = contactenlist;
            ddlContactpersoon.DataBind();
        }
    }
}