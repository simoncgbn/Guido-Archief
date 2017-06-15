using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using GuidoStock.App_Code;
using ServiceStack;
using DataTable = DocumentFormat.OpenXml.Drawing.Charts.DataTable;

namespace GuidoStock.Stock {
    public partial class Overzicht : System.Web.UI.Page, IPostBackEventHandler
    {
        //:TODO :: Add pijltjes op Gridview om te sorteren
        private List<Artikel> Artikelen
        {
            get { return (List<Artikel>) ViewState["Artikelen"]; }
            set { ViewState["Artikelen"] = value; }
        }
        protected Artikel Artikel
        {
            get { return (Artikel) ViewState["Artikel"]; }
            set { ViewState["Artikel"] = value; }
        }

        protected string FilterType
        {
            get { return (string)ViewState["FilterType"]; }
            set { ViewState["FilterType"] = value; }
        }

        DBClass db = new DBClass();

        public Overzicht()
        {
        }

        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack)
            {
                if (HttpContext.Current.User.IsInRole("Admin"))
                {
                    btnRemove.Visible = false;
                }

                string type = Request.QueryString["type"];

                if (type == "0" || type == null || type == "huidige")
                {
                    FilterType = "huidige";
                    Artikelen = db.GetHuidigeArtikelen();
                }
                else if (type == "alles")
                {
                    FilterType = "alles";
                    Artikelen = db.GetArtikelen(); 
                }
                else if (type == "verlopen")
                {
                    FilterType = "verlopen";
                    Artikelen = db.GetVerlopenArtikelen();
                }
                Artikelen = Artikelen.Where(a => (a.IsVerwijderd == false)).ToList();
                Artikel = new Artikel();

                string id = Request.QueryString["id"];
                string showMessage = Request.QueryString["showMessage"];
                if (!string.IsNullOrEmpty(id))
                {
                    int n;
                    bool parsed = int.TryParse(id, out n);
                    bool exist = false;

                    if (parsed)
                    {
                        foreach (var art in Artikelen)
                        {
                            if (art.Id == int.Parse(id))
                            {
                                exist = true;
                            }
                        }

                        if (exist)
                        {
                            Artikel = db.GetArtikel(int.Parse(id));
                            shouldBeDetail.Value = "True";
                            Artikel.Stocks = Artikel.Stocks.OrderBy(s => s.Vervaldatum).Where(x => x.Aantal > 0).ToList();
                            Artikel.Stocks = (from ol in  Artikel.Stocks

                                group ol by new {ol.Vervaldatum, ol.ArtikelLocatie.Id}
                                into grp
                                select new App_Code.Stock
                                {
                                    Id = grp.Select(ex => ex.Id).FirstOrDefault(),
                                    Aantal = grp.Sum(ex => ex.Aantal),
                                    Artikel = grp.Select(ex => ex.Artikel).FirstOrDefault(),
                                    ArtikelLocatie = grp.Select(ex => ex.ArtikelLocatie).FirstOrDefault(),
                                    ArtikelLocatieId = grp.Select(ex => ex.ArtikelLocatieId).FirstOrDefault(),
                                    ArtikelLocatieCode = grp.Select(ex => ex.ArtikelLocatieCode).FirstOrDefault(),
                                    LocatieVervaldatum = grp.Select(ex => ex.LocatieVervaldatum).FirstOrDefault(),
                                    Vervaldatum = grp.Key.Vervaldatum

                                }
                            ).ToList();
                            Artikel.Units = Artikel.Units.Where(x => x.Aantal != 1).ToList();
                        }
                    }
                }

                if (!string.IsNullOrEmpty(showMessage))
                {
                    messageBox.Style["display"] = "block";
                }


                if (Artikelen.Count > 0)
                {
                    StockGridView.DataSource = Artikelen;
                }
                else
                {
                    FilterType = "alles";
                    Artikelen = db.GetArtikelen();
                    Artikelen = Artikelen.Where(a => (a.IsVerwijderd == false)).ToList();
                    StockGridView.DataSource = Artikelen;
                }
                StockGridView.DataBind();
            }
            if (Artikelen.Count > 0)
                StockGridView.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        protected void StockGridView_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(StockGridView,
                    "select$" + e.Row.RowIndex);
            }
        }

        protected void StockGridView_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            // Hier linken naar DetailView
            /*DBClass db = new DBClass();
            Artikel = db.GetArtikel(Convert.ToInt32(StockGridView.SelectedDataKey.Value));
            shouldBeDetail.Value = "True";*/
            Response.Redirect("Overzicht.aspx?id=" + StockGridView.SelectedDataKey.Value + "&type=" + (FilterType != null ? FilterType : "0"));
        }

        protected void btnEditStock_Click(object sender, EventArgs e)
        {
            var artId = Artikel.Id;
            Response.Redirect("AddArtikel.aspx?id=" + artId + "&type=" + (FilterType != null ? FilterType : "0"));
        }

        protected void ExportToExcel(object sender, EventArgs e)
        {
            var workbook = new XLWorkbook();

            var dataTable = GetTable();
            workbook.Worksheets.Add(dataTable);

            MemoryStream fs = new MemoryStream();
            workbook.SaveAs(fs, false);
            fs.Position = 0;

            string filename = $"stocklijst_{DateTime.Today.ToShortDateString()}.xlsx";
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename="+filename);
            Response.ContentType = "application/vnd.ms-excel";
            Response.BinaryWrite(fs.ToArray());
            Response.Flush();
            Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest();

        }

        protected void ExportToCSV(object sender, EventArgs e)
        {
            var dataTable = GetTable();
            string output = ToCSV(dataTable);

            string filename = $"stocklijst_{DateTime.Today.ToShortDateString()}.csv";
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + filename);
            Response.ContentType = "text/csv";
            Response.Write(output);
            Response.Flush();
            Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        private System.Data.DataTable GetTable()
        {
            DBClass db = new DBClass();
            List<Artikel> artikelen2 = db.GetArtikelen();
            System.Data.DataTable table = new System.Data.DataTable();
            table.TableName = "StockLijst";
            table.Columns.Add("Merk", typeof (string));
            table.Columns.Add("Artikel", typeof (string));
            table.Columns.Add("Categorie", typeof (string));
            table.Columns.Add("Locatie", typeof (string));
            table.Columns.Add("Vervaldatum", typeof (string));
            table.Columns.Add("Voorraad", typeof (int));

            foreach (var artikel2 in artikelen2)
            {
                foreach (var stock in artikel2.Stocks)
                {
                    table.Rows.Add(artikel2.Merk.Naam, artikel2.Naam, artikel2.Categorie.Naam, stock.ArtikelLocatie.Code,
                        stock.Vervaldatum == DateTime.MaxValue ? "" : stock.Vervaldatum.ToString("dd/MM/yyyy"), stock.Aantal);
                }
            }
            return table;
        }

        private static string ToCSV(System.Data.DataTable table)
        {
            var result = new StringBuilder();
            for (int i = 0; i < table.Columns.Count; i++)
            {
                result.Append(table.Columns[i].ColumnName);
                result.Append(i == table.Columns.Count - 1 ? "\n" : ",");
            }

            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    result.Append(row[i].ToString());
                    result.Append(i == table.Columns.Count - 1 ? "\n" : ",");
                }
            }

            return result.ToString();
        }

        protected void btnPrintBarcode_Click(object sender, EventArgs e)
        {
            string url = "http://www.guido.be/barcode.aspx?code=" + Artikel.Barcode;

        }

        public void RaisePostBackEvent(string eventArgument)
        {
            if (eventArgument.Equals("verwijder"))
            {
                DBClass db = new DBClass();
                db.DeleteArtikel(Artikel.Id);
                Response.Redirect("Overzicht.aspx?showMessage=true");
            }
        }

        protected void FilterAlles(object sender, EventArgs e)
        {
            FilterType = "alles";
            Artikelen = db.GetArtikelen();
            Artikelen = Artikelen.Where(a => (a.IsVerwijderd == false)).ToList();
            if (Artikelen.Count > 0)
            {
                StockGridView.DataSource = Artikelen;
                StockGridView.DataBind();
            }
            else
            {
                StockGridView.DataSource = null;
                StockGridView.DataBind();
            }
        }

        protected void FilterStock(object sender, EventArgs e)
        {
            FilterType = "huidige";
            Artikelen = db.GetHuidigeArtikelen();
            Artikelen = Artikelen.Where(a => (a.IsVerwijderd == false)).ToList();
            if (Artikelen.Count > 0)
            {
                StockGridView.DataSource = Artikelen;
                StockGridView.DataBind();
            }
            else
            {
                StockGridView.DataSource = null;
                StockGridView.DataBind();
            }
        }

        protected void FilterNoStock(object sender, EventArgs e)
        {
            FilterType = "verlopen";
            Artikelen = db.GetVerlopenArtikelen();
            Artikelen = Artikelen.Where(a => (a.IsVerwijderd == false)).ToList();
            if (Artikelen.Count > 0)
            {
                StockGridView.DataSource = Artikelen;
                StockGridView.DataBind();
            }
            else
            {
                StockGridView.DataSource = null;
                StockGridView.DataBind();
            }
        }
    }
}