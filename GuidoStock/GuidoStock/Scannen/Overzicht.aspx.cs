using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GuidoStock.Scannen
{
    public partial class Overzicht : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnScanEvent_OnClick(object sender, EventArgs e)
        {
            var id = (sender as Control).ClientID;

            switch (id)
            {
                case "MainContent_btnScanInkomend":
                    Response.Redirect("NoEvent.aspx?id=inkomend");
                    break;
                case "MainContent_btnScanUitgaand":
                    Response.Redirect("NoEvent.aspx?id=uitgaand");
                    break;
                case "MainContent_btnScanEvent":
                    Response.Redirect("Event.aspx");
                    break;

            }

        }
    }
}