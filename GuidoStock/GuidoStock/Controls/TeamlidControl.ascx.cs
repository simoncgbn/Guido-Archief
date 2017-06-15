using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GuidoStock.Code;

namespace GuidoStock.Controls
{
    public partial class TeamlidControl : System.Web.UI.UserControl
    {

        public EvenementTeamLid TeamLid
        {
            get { return (EvenementTeamLid) ViewState["TeamLid"]; }
            set { ViewState["TeamLid"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            // UpdateModel();
            UpdateUI();
        }

        private void UpdateUI()
        {
            TeamlidNaam.Text = TeamLid.Naam;
            TeamlidFunctie.Text = TeamLid.Functie;
            TeamlidTel.Text = TeamLid.Tel;
        }

        public void UpdateModel()
        {
            TeamLid.Naam = TeamlidNaam.Text;
            TeamLid.Functie = TeamlidFunctie.Text;
            TeamLid.Tel = TeamlidTel.Text;
        }
    }
}