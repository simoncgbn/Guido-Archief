using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using GuidoStock.App_Code;
using GuidoStock.Code;

namespace GuidoStock.Scannen
{
    public partial class Event : System.Web.UI.Page
    {

        private new List<Evenement> Events
        {
            get { return (List<Evenement>)ViewState["Events"]; }
            set { ViewState["Events"] = value; }
        }

        public List<Evenement> LopendeEvenements
        {
            get { return (List<Evenement>)ViewState["LopendeEvenements"]; }
            set { ViewState["LopendeEvenements"] = value; }
        }

        public List<Evenement> KomendEvenements
        {
            get { return (List<Evenement>)ViewState["KomendEvenements"]; }
            set { ViewState["KomendEvenements"] = value; }
        }
        protected Evenement Evenement
        {
            get { return (Evenement)ViewState["Evenement"]; }
            set { ViewState["Evenement"] = value; }
        }

        public List<Code.Order> Orders
        {
            get { return (List<Code.Order>)ViewState["Orders"]; }
            set { ViewState["Orders"] = value; }
        }

        public List<Code.Order> KomendeOrders
        {
            get { return (List<Code.Order>)ViewState["KomendeOrders"]; }
            set { ViewState["KomendeOrders"] = value; }
        }

        public List<Code.Order> LopendeOrders
        {
            get { return (List<Code.Order>)ViewState["LopendeOrders"]; }
            set { ViewState["LopendeOrders"] = value; }
        }

        public List<ScannenModel> LopendeScannen
        {
            get { return (List<ScannenModel>)ViewState["LopendeScannen"]; }
            set { ViewState["LopendeScannen"] = value; }
        }

        public List<ScannenModel> KomendeScannen
        {
            get { return (List<ScannenModel>)ViewState["KomendeScannen"]; }
            set { ViewState["KomendeScannen"] = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Evenement = new Evenement();
                DBClass db = new DBClass();

                Events = db.GetEvenementen();
                KomendEvenements = db.GetKomendeEvenementen();
                KomendEvenements = KomendEvenements.OrderBy(x => x.Datum).ToList();

                LopendeEvenements = db.GetLopendeEvenementen();
                LopendeEvenements = LopendeEvenements.OrderBy(x => x.Datum).ToList();

                for (int i = LopendeEvenements.Count - 1; i >= 0; i--)
                {
                    if (LopendeEvenements[i].EventComplete > DateTime.MinValue)
                        LopendeEvenements.RemoveAt(i);
                }

                LopendeOrders = new List<Code.Order>();
                KomendeOrders = db.GetKomendeOrders();
                Orders = db.GetOrders();
                foreach (var order in Orders)
                {
                    if (order.OrderComplete == DateTime.MinValue)
                    {
                        if (order.ChecklistComplete != DateTime.MinValue)
                        {
                            LopendeOrders.Add(order);
                        }
                    }
                }
                LopendeScannen = new List<ScannenModel>();
                KomendeScannen = new List<ScannenModel>();
                foreach (var evnt in LopendeEvenements)
                {
                    ScannenModel mdl = new ScannenModel(evnt);
                    LopendeScannen.Add(mdl);
                }

                foreach (var order in LopendeOrders)
                {
                    ScannenModel mdl = new ScannenModel(order);
                    LopendeScannen.Add(mdl);
                }
                var checklistLijnen = db.GetEvenementChecklistLijnen();
                var orderlijnen = db.GetOrderChecklistLijnen();
                foreach (var evnt in KomendEvenements)
                {
                    ScannenModel mdl = new ScannenModel(evnt);
                    bool has = checklistLijnen.Any(x => x.Evenement.Id == mdl.Id);
                    if (has) mdl.IsOnHold = true;
                    KomendeScannen.Add(mdl);
                }

                foreach (var order in KomendeOrders)
                {
                    ScannenModel mdl = new ScannenModel(order);
                    bool has = orderlijnen.Any(x => x.Order.Id == mdl.Id);
                    if (has) mdl.IsOnHold = true;
                    KomendeScannen.Add(mdl);
                }

                LopendeScannen = LopendeScannen.OrderBy(l => l.Datum).ToList();
                KomendeScannen = KomendeScannen.OrderBy(l => l.Datum).ToList();

            }
        }


        protected void EventSelect_OnValueChanged(object sender, EventArgs e)
        {

            
                string value = EventSelect.Value;
                string id = value.Split('¬')[0];
                string type = value.Split('¬')[1];
                string isevent = value.Split('¬')[2];

            if (type == "Event")
            {
                Response.Redirect("EventScannen.aspx?id=" + id + "&isevent=" + isevent);

            }else if (type == "EventLopend")
            {
                Response.Redirect("EventScannenLopend.aspx?id=" + id + "&isevent=" + isevent);

            }



        }

       

        
    }
}