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
    public partial class TransportControl : System.Web.UI.UserControl
    {

        private readonly DBClass _db = new DBClass();
        public List<Gebruiker> _gebruikersList = new List<Gebruiker>();

        // public EvenementTransport EvenementTransport { get; set; }
        public EvenementTransport Test
        {
            get { return (EvenementTransport) ViewState["Test"]; }
            set { ViewState["Test"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            // EvenementTransport = new EvenementTransport() {ChauffeurHeen = new Gebruiker(), ChauffeurTerug = new Gebruiker(), Evenement = new Evenement()};
            _gebruikersList = _db.GetGebruikers();
            ddlChauffeurHeen.DataSource = _gebruikersList;
            ddlChauffeurHeen.DataBind();

            ddlChauffeurTerug.DataSource = _gebruikersList;
            ddlChauffeurTerug.DataBind();

            ddlTransportNaam.DataSource = _db.getTransporten();
            ddlTransportNaam.DataBind(); 
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            // UpdateModel();
            UpdateUI();
        }

        protected void hdnTransport_OnValueChanged(object sender, EventArgs e)
        {
            var transport = hdnTransport.Value;
            var test = 0;
            Test.Transport.Id = !int.TryParse(hdnTransport.Value, out test) ? int.MaxValue : test;
            if (Regex.IsMatch(transport, @"^\d+$"))
            {
                // Test.Transport.Id = Convert.ToInt32(transport);
                return;
            }
            var transportList = new List<Transport>();
            var tran = new Transport
            {
                Id = int.MaxValue,
                Naam = hdnTransport.Value
            };

            transportList.Add(tran);

            transportList.AddRange(_db.getTransporten());

            ddlTransportNaam.DataSource = transportList;
            ddlTransportNaam.DataBind();
        }

        public void UpdateModel()
        {
            var test = 0;
            if (!string.IsNullOrEmpty(hdnTransport.Value))
            {
                Test.Transport.Id = !int.TryParse(hdnTransport.Value, out test) ? int.MaxValue : test;
                Test.Transport.Naam = ddlTransportNaam.SelectedItem.Text;
            }
            Test.ChauffeurHeen.Id = Convert.ToInt32(ddlChauffeurHeen.SelectedValue);
            Test.ChauffeurTerug.Id = Convert.ToInt32(ddlChauffeurTerug.SelectedValue);
            // Test.Vertrek = Convert.ToDateTime(VertrekTransport.Text);
            DateTime date;
            if (DateTime.TryParse(VertrekTransport.Text, out date))
            {
                Test.Vertrek = date;
            }
        }

        private void UpdateUI()
        {
            _gebruikersList = _db.GetGebruikers();
            ddlChauffeurHeen.DataSource = _gebruikersList;
            ddlChauffeurHeen.DataBind();

            ddlChauffeurTerug.DataSource = _gebruikersList;
            ddlChauffeurTerug.DataBind();
            ddlTransportNaam.DataSource = _db.getTransporten();
            ddlTransportNaam.DataBind();
            switch (Test.Transport.Id)
            {
                case 0:
                    return;
                case int.MaxValue:
                    if (hdnTransport.Value == null || hdnTransport.Value.Equals(""))
                        hdnTransport.Value = Test.Transport.Naam;
                    hdnTransport_OnValueChanged(hdnTransport, null);
                    break;
                default:
                    ddlTransportNaam.SelectedValue = Test.Transport.Id.ToString();
                    break;
            }
            if (Test.ChauffeurTerug.Id != 0)
            {
                ddlChauffeurTerug.SelectedValue = Test.ChauffeurTerug.Id.ToString();
            }
            if (Test.ChauffeurHeen.Id != 0)
            {
                ddlChauffeurHeen.SelectedValue = Test.ChauffeurHeen.Id.ToString();
            }
            if (Test.Vertrek != DateTime.MinValue)
            {
                VertrekTransport.Text = Test.Vertrek.ToString("yyyy-MM-dd H:mm");
            }
        }

        protected void ddlChauffeurHeen_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl = (sender as DropDownList);
            var test = ddl.SelectedValue;
            Test.ChauffeurHeen.Id = Convert.ToInt32(test);
            Test.ChauffeurHeen.Naam = ddl.SelectedItem.Text;
        }

        protected void ddlChauffeurTerug_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl = (sender as DropDownList);
            var test = ddl.SelectedValue;
            Test.ChauffeurTerug.Id = Convert.ToInt32(test);
            Test.ChauffeurTerug.Naam = ddl.SelectedItem.Text;
        }
    }
}