using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GuidoStock.App_Code;

namespace GuidoStock.Test
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "loadJs", "jsLoad()", true);

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DBClass db = new DBClass();

                ddlNieuwMerknaam.DataSource = db.GetMerken();
                ddlNieuwMerknaam.DataBind();


            }

        }
    }
}