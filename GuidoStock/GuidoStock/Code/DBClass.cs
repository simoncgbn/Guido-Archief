using GuidoStock.Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using GuidoStock.Controls;
using GuidoStock.Models;

namespace GuidoStock.App_Code
{
    public class DBClass
    {
        private readonly String cs =
            System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public DBClass()
        {
        }

        public List<Merk> GetMerken()
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetMerken", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Merk");
            List<Merk> list = new List<Merk>();
            for (int i = 0; i < ds.Tables["Merk"].Rows.Count; i++)
            {
                list.Add(new Merk(ds.Tables["Merk"].Rows[i]));
            }
            myConnection.Close();
            return list;
        }

        public List<Categorie> GetCategorien()
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetCategorien", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Categorie");
            List<Categorie> list = new List<Categorie>();
            for (int i = 0; i < ds.Tables["Categorie"].Rows.Count; i++)
            {
                list.Add(new Categorie(ds.Tables["Categorie"].Rows[i]));
            }
            myConnection.Close();
            return list;
        }

        public List<ArtikelMetaTag> GetMetaTags()
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetArtikelMetaTags", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            myCommand.Fill(ds, "ArtikelMetaTag");
            List<ArtikelMetaTag> list = new List<ArtikelMetaTag>();
            for (int i = 0; i < ds.Tables["ArtikelMetaTag"].Rows.Count; i++)
            {
                list.Add(new ArtikelMetaTag(ds.Tables["ArtikelMetaTag"].Rows[i]));
            }
            myConnection.Close();
            return list;
        }

        public List<ArtikelLocatie> GetArtikelLocaties()
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetArtikelLocaties", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            myCommand.Fill(ds, "ArtikelLocatie");
            List<ArtikelLocatie> list = new List<ArtikelLocatie>();
            for (int i = 0; i < ds.Tables["ArtikelLocatie"].Rows.Count; i++)
            {
                list.Add(new ArtikelLocatie(ds.Tables["ArtikelLocatie"].Rows[i]));
            }
            myConnection.Close();
            return list;
        }

        public List<Artikel> GetArtikelen()
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetEntireArtikelen", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Artikel");
            List<Artikel> list = new List<Artikel>();
            Dictionary<int, List<DataRow>> dict = GetStocks();
            Dictionary<int, List<DataRow>> metaTags = GetArtikelMetaTagRows();
            Dictionary<int, List<DataRow>> units = GetUnits();
            for (int i = 0; i < ds.Tables["Artikel"].Rows.Count; i++)
            {
                if (dict.ContainsKey(Convert.ToInt32(ds.Tables["Artikel"].Rows[i]["Id"])))
                {
                    list.Add(new Artikel(ds.Tables["Artikel"].Rows[i], dict[Convert.ToInt32(ds.Tables["Artikel"].Rows[i]["Id"])]));
                }
                else
                {
                    list.Add(new Artikel(ds.Tables["Artikel"].Rows[i]));
                }
                //TODO :: Schrijf hier doc bij want dit is vrij omslachtig.
                if (metaTags.ContainsKey(Convert.ToInt32(ds.Tables["Artikel"].Rows[i]["Id"])))
                {
                    list[i].ConvertArtikelMetaTags(metaTags[Convert.ToInt32(ds.Tables["Artikel"].Rows[i]["Id"])]);
                }

                if (units.ContainsKey(Convert.ToInt32(ds.Tables["Artikel"].Rows[i]["Id"])))
                {
                    list[i].ConvertUnits(units[Convert.ToInt32(ds.Tables["Artikel"].Rows[i]["Id"])]);
                }
            }
            myConnection.Close();
            return list;
        }

        public List<Artikel> GetHuidigeArtikelen()
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetHuidigeStocks", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Artikel");
            List<Artikel> list = new List<Artikel>();
            Dictionary<int, List<DataRow>> dict = GetStocks();
            Dictionary<int, List<DataRow>> metaTags = GetArtikelMetaTagRows();
            Dictionary<int, List<DataRow>> units = GetUnits();
            for (int i = 0; i < ds.Tables["Artikel"].Rows.Count; i++)
            {
                if (dict.ContainsKey(Convert.ToInt32(ds.Tables["Artikel"].Rows[i]["Id"])))
                {
                    list.Add(new Artikel(ds.Tables["Artikel"].Rows[i], dict[Convert.ToInt32(ds.Tables["Artikel"].Rows[i]["Id"])]));
                }
                else
                {
                    list.Add(new Artikel(ds.Tables["Artikel"].Rows[i]));
                }
                //TODO :: Schrijf hier doc bij want dit is vrij omslachtig.
                if (metaTags.ContainsKey(Convert.ToInt32(ds.Tables["Artikel"].Rows[i]["Id"])))
                {
                    list[i].ConvertArtikelMetaTags(metaTags[Convert.ToInt32(ds.Tables["Artikel"].Rows[i]["Id"])]);
                }

                if (units.ContainsKey(Convert.ToInt32(ds.Tables["Artikel"].Rows[i]["Id"])))
                {
                    list[i].ConvertUnits(units[Convert.ToInt32(ds.Tables["Artikel"].Rows[i]["Id"])]);
                }
            }
            myConnection.Close();
            return list;
        }

        public List<Artikel> GetVerlopenArtikelen()
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetVerlopenStocks", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Artikel");
            List<Artikel> list = new List<Artikel>();
            Dictionary<int, List<DataRow>> dict = GetStocks();
            Dictionary<int, List<DataRow>> metaTags = GetArtikelMetaTagRows();
            Dictionary<int, List<DataRow>> units = GetUnits();
            for (int i = 0; i < ds.Tables["Artikel"].Rows.Count; i++)
            {
                if (dict.ContainsKey(Convert.ToInt32(ds.Tables["Artikel"].Rows[i]["Id"])))
                {
                    list.Add(new Artikel(ds.Tables["Artikel"].Rows[i], dict[Convert.ToInt32(ds.Tables["Artikel"].Rows[i]["Id"])]));
                }
                else
                {
                    list.Add(new Artikel(ds.Tables["Artikel"].Rows[i]));
                }
                //TODO :: Schrijf hier doc bij want dit is vrij omslachtig.
                if (metaTags.ContainsKey(Convert.ToInt32(ds.Tables["Artikel"].Rows[i]["Id"])))
                {
                    list[i].ConvertArtikelMetaTags(metaTags[Convert.ToInt32(ds.Tables["Artikel"].Rows[i]["Id"])]);
                }

                if (units.ContainsKey(Convert.ToInt32(ds.Tables["Artikel"].Rows[i]["Id"])))
                {
                    list[i].ConvertUnits(units[Convert.ToInt32(ds.Tables["Artikel"].Rows[i]["Id"])]);
                }
            }
            myConnection.Close();
            return list;
        }

        public List<Module> GetModules()
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetModules", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Module");
            List<Module> result = new List<Module>();

            for (var i = 0; i < ds.Tables["Module"].Rows.Count; i++)
            {
                result.Add(new Module(ds.Tables["Module"].Rows[i]));
            }

            myConnection.Close();
            return result;
        }

        public List<Evenement> GetEvenementen()
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetEvenementen", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Evenement");
            List<Evenement> result = new List<Evenement>();

            for (int i = 0; i < ds.Tables["Evenement"].Rows.Count; i++)
            {
                var row = ds.Tables["Evenement"].Rows[i];
                int artikelId = Convert.ToInt32(row["EvenementId"].ToString());
                int index = result.FindIndex(a => a.Id == artikelId);
                if (index >= 0)
                {
                    result[index].EvenementLocaties.Add(new EvenementLocatie(Convert.ToDateTime(row["BeginTijd"]),
                        Convert.ToDateTime(row["EindTijd"])));
                }
                else
                {
                    result.Add(new Evenement(row));
                }
            }

            myConnection.Close();
            return result;
        }

        public List<Code.Order> GetOrders()
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetOrders", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Order");
            List<Code.Order> result = new List<Code.Order>();

            for (int i = 0; i < ds.Tables["Order"].Rows.Count; i++)
            {
                result.Add(new Code.Order(ds.Tables["Order"].Rows[i]));
            }

            myConnection.Close();
            return result;
        }

        public List<Code.Order> GetHuidigeOrders()
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetHuidigeOrders", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Order");
            List<Code.Order> result = new List<Code.Order>();

            for (int i = 0; i < ds.Tables["Order"].Rows.Count; i++)
            {
                result.Add(new Code.Order(ds.Tables["Order"].Rows[i]));
            }

            myConnection.Close();
            return result;
        }

        public List<Code.Order> GetKomendeOrders()
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetKomendeOrders", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Order");
            List<Code.Order> result = new List<Code.Order>();

            for (int i = 0; i < ds.Tables["Order"].Rows.Count; i++)
            {
                result.Add(new Code.Order(ds.Tables["Order"].Rows[i]));
            }

            myConnection.Close();
            return result;
        }

        public List<Code.Order> GetVerlopenOrders()
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetVerlopenOrders", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Order");
            List<Code.Order> result = new List<Code.Order>();

            for (int i = 0; i < ds.Tables["Order"].Rows.Count; i++)
            {
                result.Add(new Code.Order(ds.Tables["Order"].Rows[i]));
            }

            myConnection.Close();
            return result;
        }

        public Code.Order GetOrder(int id)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetOrderById", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pId = new SqlParameter("@Id", SqlDbType.Int);
            pId.Value = id;
            myCommand.SelectCommand.Parameters.Add(pId);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Order");
            if (ds.Tables["Order"].Rows.Count == 0)
            {
                return null;
            }
            var order = new Code.Order(ds.Tables["Order"].Rows[0]);
            myConnection.Close();
            return order;
        }


        public Gebruiker GetGebruikerByEmail(string email)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetGebruikerByEmail", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pEmail = new SqlParameter("@Email", SqlDbType.VarChar, 255);
            pEmail.Value = email;
            myCommand.SelectCommand.Parameters.Add(pEmail);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Gebruiker");

            if (ds.Tables["Gebruiker"].Rows.Count == 0)
                return null;
            var gebruiker = new Gebruiker(ds.Tables["Gebruiker"].Rows[0], 0);
            myConnection.Close();
            return gebruiker;
        }

        public Gebruiker GetGebruikerById(int id)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetGebruikerById", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pId = new SqlParameter("@Id", SqlDbType.VarChar, 255);
            pId.Value = id;
            myCommand.SelectCommand.Parameters.Add(pId);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Gebruiker");

            if (ds.Tables["Gebruiker"].Rows.Count == 0)
                return null;
            var gebruiker = new Gebruiker(ds.Tables["Gebruiker"].Rows[0], "0");
            myConnection.Close();
            return gebruiker;
        }

        public List<Evenement> GetHuidigeEvenementen()
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetHuidigeEvenementen", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Evenement");
            List<Evenement> result = new List<Evenement>();

            for (int i = 0; i < ds.Tables["Evenement"].Rows.Count; i++)
            {
                var row = ds.Tables["Evenement"].Rows[i];
                int artikelId = Convert.ToInt32(row["EvenementId"].ToString());
                int index = result.FindIndex(a => a.Id == artikelId);
                if (index >= 0)
                {
                    result[index].EvenementLocaties.Add(new EvenementLocatie(Convert.ToDateTime(row["BeginTijd"]),
                        Convert.ToDateTime(row["EindTijd"]), row["Plaats"].ToString()));
                }
                else
                {
                    result.Add(new Evenement(row, true));
                }
            }

            myConnection.Close();
            return result;
        }

        public List<Evenement> GetVerlopenEvenementen()
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetVerlopenEvenementen", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Evenement");
            List<Evenement> result = new List<Evenement>();

            for (int i = 0; i < ds.Tables["Evenement"].Rows.Count; i++)
            {
                var row = ds.Tables["Evenement"].Rows[i];
                int artikelId = Convert.ToInt32(row["EvenementId"].ToString());
                int index = result.FindIndex(a => a.Id == artikelId);
                if (index >= 0)
                {
                    result[index].EvenementLocaties.Add(new EvenementLocatie(Convert.ToDateTime(row["BeginTijd"]),
                        Convert.ToDateTime(row["EindTijd"])));
                }
                else
                {
                    result.Add(new Evenement(row));
                }
            }

            myConnection.Close();
            return result;
        }

        public int AddArtikelLocatie(string code, string barcode)
        {

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspAddArtikelLocatie", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pCode = new SqlParameter("@Code", SqlDbType.VarChar, 50);
            pCode.Value = code;
            myCommand.Parameters.Add(pCode);

            SqlParameter pBarcode = new SqlParameter("@Barcode", SqlDbType.VarChar, 20);
            pBarcode.Value = barcode;
            myCommand.Parameters.Add(pBarcode);

            int i = 0;
            myConnection.Open();
            i = (int)myCommand.ExecuteScalar();
            myConnection.Close();

            return i;

        }


        public void UpdateOrInsertRecht(string rolId, int moduleId, bool lezen, bool schrijven)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspUpdateOrInsertRecht", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pRolId = new SqlParameter("@RolId", SqlDbType.VarChar, 50);
            pRolId.Value = rolId;
            myCommand.Parameters.Add(pRolId);

            SqlParameter pModuleId = new SqlParameter("@ModuleId", SqlDbType.Int);
            pModuleId.Value = moduleId;
            myCommand.Parameters.Add(pModuleId);

            SqlParameter pLezen = new SqlParameter("@Lezen", SqlDbType.Bit);
            pLezen.Value = lezen;
            myCommand.Parameters.Add(pLezen);

            SqlParameter pSchrijven = new SqlParameter("@Schrijven", SqlDbType.Bit);
            pSchrijven.Value = schrijven;
            myCommand.Parameters.Add(pSchrijven);

            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();
        }

        public int AddOrder(Code.Order order)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspAddOrder", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pNaam = new SqlParameter("@Naam", SqlDbType.NVarChar, 100);
            pNaam.Value = order.Naam;
            myCommand.Parameters.Add(pNaam);

            SqlParameter pBeginTijd = new SqlParameter("@BeginTijd", SqlDbType.DateTime);
            pBeginTijd.Value = order.BeginTijd;
            myCommand.Parameters.Add(pBeginTijd);

            if (order.EindTijd != DateTime.MaxValue)
            {
                SqlParameter pEindTijd = new SqlParameter("@EindTijd", SqlDbType.DateTime);
                pEindTijd.Value = order.EindTijd;
                myCommand.Parameters.Add(pEindTijd);
            }

            SqlParameter pTel = new SqlParameter("@Tel", SqlDbType.NVarChar);
            pTel.Value = order.Tel;
            myCommand.Parameters.Add(pTel);

            SqlParameter pContactNaam = new SqlParameter("@ContactNaam", SqlDbType.NVarChar);
            pContactNaam.Value = order.ContactNaam;
            myCommand.Parameters.Add(pContactNaam); 

            SqlParameter pIsVerhuur = new SqlParameter("@IsVerhuur", SqlDbType.Bit);
            pIsVerhuur.Value = order.IsVerhuur;
            myCommand.Parameters.Add(pIsVerhuur);

            var i = 0;
            myConnection.Open();
            i = (int) myCommand.ExecuteScalar();
            myConnection.Close();
            return i;
        }

        public int AddRecht(string rolId, int moduleId, bool schrijven, bool lezen)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspAddRecht", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pRolId = new SqlParameter("@RolId", SqlDbType.NVarChar, 128);
            pRolId.Value = rolId;
            myCommand.Parameters.Add(pRolId);

            SqlParameter pModuleId = new SqlParameter("@ModuleId", SqlDbType.Int);
            pModuleId.Value = moduleId;
            myCommand.Parameters.Add(pModuleId);

            SqlParameter pSchrijven = new SqlParameter("@Schijven", SqlDbType.Bit);
            pSchrijven.Value = schrijven;
            myCommand.Parameters.Add(pSchrijven);

            SqlParameter pLezen = new SqlParameter("@Lezen", SqlDbType.Bit);
            pLezen.Value = lezen;
            myCommand.Parameters.Add(pLezen);

            int i = 0;
            myConnection.Open();
            i = (int) myCommand.ExecuteScalar();
            myConnection.Close();

            return i;
        }

        public void InsertEvenementChecklistLijnen(List<EvenementChecklistLijn> lijnen, int id)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("EvenementId");
            dataTable.Columns.Add("StockId");
            dataTable.Columns.Add("Aantal");
            dataTable.Columns.Add("AantalWeg");
            dataTable.Columns.Add("Unit");
            foreach (var lijn in lijnen)
            {
                var row = dataTable.NewRow();
                row["EvenementId"] = id;
                row["StockId"] = lijn.Stock.Id;
                row["Aantal"] = lijn.Aantal;
                row["AantalWeg"] = lijn.AantalWeg;
                row["Unit"] = lijn.Unit != null ? (object) lijn.Unit.Id : null;
                dataTable.Rows.Add(row);
            }

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspInsertEvenementChecklistLijnen", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.AddWithValue("@tblChecklistLijnen", dataTable);
            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();
        }

        public void InsertOrderChecklistLijnen(List<OrderChecklistLijn> lijnen, int id)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("OrderId");
            dataTable.Columns.Add("StockId");
            dataTable.Columns.Add("Aantal");
            dataTable.Columns.Add("AantalWeg");
            dataTable.Columns.Add("Unit");
            foreach (var lijn in lijnen)
            {
                var row = dataTable.NewRow();
                row["OrderId"] = id;
                row["StockId"] = lijn.Stock.Id;
                row["Aantal"] = lijn.Aantal;
                row["AantalWeg"] = lijn.AantalWeg;
                row["Unit"] = lijn.Unit != null ? (object)lijn.Unit.Id : null;
                dataTable.Rows.Add(row);
            }

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspInsertOrderChecklistLijnen", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.AddWithValue("@tblChecklistLijnen", dataTable);
            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();
        }

        public bool UpdateOrderLijnenByOrder(List<OrderLijn> orderLijnen, int id)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("OrderId");
            dataTable.Columns.Add("ArtikelId");
            dataTable.Columns.Add("Aantal");
            foreach (var orderLijn in orderLijnen)
            {
                var row = dataTable.NewRow();
                row["OrderId"] = id;
                row["ArtikelId"] = orderLijn.Artikel.Id;
                row["Aantal"] = orderLijn.Aantal;
                dataTable.Rows.Add(row);
            }

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspInsertOrUpdateOrderLijnen", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pId = new SqlParameter("@OrderId", SqlDbType.Int);
            pId.Value = id;
            myCommand.Parameters.Add(pId);

            myCommand.Parameters.AddWithValue("@tblOrderLijn", dataTable);

            bool b = false;
            myConnection.Open();
            b = Convert.ToBoolean(myCommand.ExecuteScalar());
            myConnection.Close();
            return b;
        }

        public bool UpdateEvenementLijnenByEvenement(List<EvenementLijn> evenementLijnen, int Id)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("EvenementId");
            dataTable.Columns.Add("ArtikelId");
            dataTable.Columns.Add("Aantal");
            foreach (var evenementLijn in evenementLijnen)
            {
                var row = dataTable.NewRow();
                row["EvenementId"] = evenementLijn.Evenement.Id;
                row["ArtikelId"] = evenementLijn.Artikel.Id;
                row["Aantal"] = evenementLijn.Aantal;
                dataTable.Rows.Add(row);
            }

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspInserOrUpdateEvenementLijnen", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pId = new SqlParameter("@EvenementId", SqlDbType.Int);
            pId.Value = Id;
            myCommand.Parameters.Add(pId);

            myCommand.Parameters.AddWithValue("@tblEvenementLijn", dataTable);

            bool b = false;
            myConnection.Open();
            b = Convert.ToBoolean(myCommand.ExecuteScalar());
            myConnection.Close();

            return b;
        }

        public int AddStock(DateTime vervaldatum, int aantal, int artikellocatieid, int artikelid, int unitId)
        {

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspAddStock", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            if (vervaldatum != DateTime.MinValue)
            {
                if (vervaldatum.ToShortDateString() != DateTime.MaxValue.ToShortDateString())
                {
                    SqlParameter pVervaldatum = new SqlParameter("@Vervaldatum", SqlDbType.DateTime);
                    pVervaldatum.Value = vervaldatum;
                    myCommand.Parameters.Add(pVervaldatum);
                }
            }


            SqlParameter pAantal = new SqlParameter("@Aantal", SqlDbType.Int);
            pAantal.Value = aantal;
            myCommand.Parameters.Add(pAantal);

            SqlParameter pArtikelLocatieId = new SqlParameter("@ArtikelLocatieId", SqlDbType.Int);
            pArtikelLocatieId.Value = artikellocatieid;
            myCommand.Parameters.Add(pArtikelLocatieId);

            SqlParameter pArtikelId = new SqlParameter("@ArtikelId", SqlDbType.Int);
            pArtikelId.Value = artikelid;
            myCommand.Parameters.Add(pArtikelId);

            SqlParameter pUnitId = new SqlParameter("@UnitId", SqlDbType.Int);
            pUnitId.Value = unitId;
            myCommand.Parameters.Add(pUnitId);

            int i = 0;
            myConnection.Open();
            i = (int)myCommand.ExecuteScalar();
            myConnection.Close();

            return i;

        }

        public int AddCategorie(string naam)
        {

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspAddCategorie", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pNaam = new SqlParameter("@Naam", SqlDbType.VarChar, 50);
            pNaam.Value = naam;
            myCommand.Parameters.Add(pNaam);



            int i = 0;
            myConnection.Open();
            i = (int)myCommand.ExecuteScalar();
            myConnection.Close();

            return i;

        }

        public int AddMerk(string naam)
        {

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspAddMerk", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pNaam = new SqlParameter("@Naam", SqlDbType.VarChar, 50);
            pNaam.Value = naam;
            myCommand.Parameters.Add(pNaam);


            int i = 0;
            myConnection.Open();
            i = (int)myCommand.ExecuteScalar();
            myConnection.Close();

            return i;

        }

        public int AddArtikel(string naam, string barcode, float gewicht, float naturaprijs, float verhuurprijs, bool isherbruikbaar, int merkid, int categorieid, int stopcontact)
        {

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspAddArtikel", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pNaam = new SqlParameter("@Naam", SqlDbType.VarChar, 50);
            pNaam.Value = naam;
            myCommand.Parameters.Add(pNaam);

            SqlParameter pOmschrijving = new SqlParameter("@Omschrijving", SqlDbType.VarChar, 255);
            pOmschrijving.Value = "";
            myCommand.Parameters.Add(pOmschrijving);

            SqlParameter pBarcode = new SqlParameter("@Barcode", SqlDbType.VarChar, 50);
            pBarcode.Value = barcode;
            myCommand.Parameters.Add(pBarcode);

            if (gewicht != -1)
            {
                SqlParameter pGewicht = new SqlParameter("@Gewicht", SqlDbType.Float);
                pGewicht.Value = gewicht;
                myCommand.Parameters.Add(pGewicht);
            }

            if (naturaprijs != -1)
            {
                SqlParameter pNaturaPrijs = new SqlParameter("@NaturaPrijs", SqlDbType.Float);
                pNaturaPrijs.Value = naturaprijs;
                myCommand.Parameters.Add(pNaturaPrijs);
            }

            if (verhuurprijs != -1)
            {
                SqlParameter pVerhuurPrijs = new SqlParameter("@VerhuurPrijs", SqlDbType.Float);
                pVerhuurPrijs.Value = verhuurprijs;
                myCommand.Parameters.Add(pVerhuurPrijs);
            }

            if (stopcontact != -1)
            {
                SqlParameter pStopcontact = new SqlParameter("@Stopcontact", SqlDbType.Int);
                pStopcontact.Value = stopcontact;
                myCommand.Parameters.Add(pStopcontact);
            }


            SqlParameter pIsHerbruikbaar = new SqlParameter("@IsHerbruikbaar", SqlDbType.Bit);
            pIsHerbruikbaar.Value = isherbruikbaar;
            myCommand.Parameters.Add(pIsHerbruikbaar);

            if (merkid != -1)
            {
                SqlParameter pMerkId = new SqlParameter("@Merkid", SqlDbType.Int);
                pMerkId.Value = merkid;
                myCommand.Parameters.Add(pMerkId);
            }



            SqlParameter pCategorieId = new SqlParameter("@CategorieId", SqlDbType.Int);
            pCategorieId.Value = categorieid;
            myCommand.Parameters.Add(pCategorieId);


            int i = 0;
            myConnection.Open();
            i = (int)myCommand.ExecuteScalar();
            myConnection.Close();

            return i;

        }

        public int AddUnit(string naam, int aantal, float prijs, string barcode, int artikelid, int childunitid)
        {

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspAddUnit", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pNaam = new SqlParameter("@NaamEnkelvoud", SqlDbType.VarChar, 30);
            pNaam.Value = naam;
            myCommand.Parameters.Add(pNaam);

            SqlParameter pNaamM = new SqlParameter("@NaamMeervoud", SqlDbType.VarChar, 30);
            pNaamM.Value = naam;
            myCommand.Parameters.Add(pNaamM);

            SqlParameter pAantal = new SqlParameter("@Aantal", SqlDbType.Int);
            pAantal.Value = aantal;
            myCommand.Parameters.Add(pAantal);

            if (prijs != -1)
            {
                SqlParameter pPrijs = new SqlParameter("@Prijs", SqlDbType.Float);
                pPrijs.Value = prijs;
                myCommand.Parameters.Add(pPrijs);
            }

            SqlParameter pBarcode = new SqlParameter("@Barcode", SqlDbType.VarChar, 20);
            pBarcode.Value = barcode;
            myCommand.Parameters.Add(pBarcode);

            SqlParameter pArtikelId = new SqlParameter("@ArtikelId", SqlDbType.Int);
            pArtikelId.Value = artikelid;
            myCommand.Parameters.Add(pArtikelId);

            SqlParameter pUnitId = new SqlParameter("@ChildUnitId", SqlDbType.Int);
            pUnitId.Value = childunitid;
            myCommand.Parameters.Add(pUnitId);

            int i = 0;
            myConnection.Open();
            i = (int)myCommand.ExecuteScalar();
            myConnection.Close();

            return i;

        }

        public int AddArtikelMetaTag(string naam, string waarde, int artikelid)
        {

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspAddArtikelMetaTag", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pNaam = new SqlParameter("@Naam", SqlDbType.VarChar, 50);
            pNaam.Value = naam;
            myCommand.Parameters.Add(pNaam);

            SqlParameter pWaarde = new SqlParameter("@Waarde", SqlDbType.VarChar, 100);
            pWaarde.Value = waarde;
            myCommand.Parameters.Add(pWaarde);

            SqlParameter pArtikelId = new SqlParameter("@ArtikelId", SqlDbType.Int);
            pArtikelId.Value = artikelid;
            myCommand.Parameters.Add(pArtikelId);

            int i = 0;
            myConnection.Open();
            i = (int)myCommand.ExecuteScalar();
            myConnection.Close();

            return i;

        }

        public Dictionary<int, List<DataRow>> GetArtikelMetaTagRows()
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetArtikelMetaTags", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            myCommand.Fill(ds, "ArtikelMetaTag");
            Dictionary<int, List<DataRow>> dict = new Dictionary<int, List<DataRow>>();
            for (int i = 0; i < ds.Tables["ArtikelMetaTag"].Rows.Count; i++)
            {
                if (dict.ContainsKey(Convert.ToInt32(ds.Tables["ArtikelMetaTag"].Rows[i]["ArtikelId"])))
                {
                    dict[Convert.ToInt32(ds.Tables["ArtikelMetaTag"].Rows[i]["ArtikelId"])].Add(ds.Tables["ArtikelMetaTag"].Rows[i]);
                }
                else
                {
                    dict.Add(Convert.ToInt32(ds.Tables["ArtikelMetaTag"].Rows[i]["ArtikelId"]), new List<DataRow>() { ds.Tables["ArtikelMetaTag"].Rows[i] });
                }
            }
            myConnection.Close();
            return dict;
        }


        public Dictionary<int, List<DataRow>> GetStocks()
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetStocks", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Stock");
            Dictionary<int, List<DataRow>> dict = new Dictionary<int, List<DataRow>>();
            for (int i = 0; i < ds.Tables["Stock"].Rows.Count; i++)
            {
                if (dict.ContainsKey(Convert.ToInt32(ds.Tables["Stock"].Rows[i]["ArtikelId"])))
                {
                    dict[Convert.ToInt32(ds.Tables["Stock"].Rows[i]["ArtikelId"])].Add(ds.Tables["Stock"].Rows[i]);
                }
                else
                {
                    dict.Add(Convert.ToInt32(ds.Tables["Stock"].Rows[i]["ArtikelId"]), new List<DataRow>() { ds.Tables["Stock"].Rows[i] });
                }
            }
            myConnection.Close();
            return dict;
        }

        public Dictionary<int, List<DataRow>> GetUnits()
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetUnits", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Unit");
            Dictionary<int, List<DataRow>> dict = new Dictionary<int, List<DataRow>>();
            for (int i = 0; i < ds.Tables["Unit"].Rows.Count; i++)
            {
                if (dict.ContainsKey(Convert.ToInt32(ds.Tables["Unit"].Rows[i]["ArtikelId"])))
                {
                    dict[Convert.ToInt32(ds.Tables["Unit"].Rows[i]["ArtikelId"])].Add(ds.Tables["Unit"].Rows[i]);
                }
                else
                {
                    dict.Add(Convert.ToInt32(ds.Tables["Unit"].Rows[i]["ArtikelId"]), new List<DataRow>() { ds.Tables["Unit"].Rows[i] });
                }
            }

            myConnection.Close();
            return dict;
        }

        public List<Unit> GetAllUnits()
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetAllUnits", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Unit");
            List<Unit> list = new List<Unit>();

            for (int i = 0; i < ds.Tables["Unit"].Rows.Count; i++)
            {
                list.Add(new Unit(ds.Tables["Unit"].Rows[i],0));
            }

            myConnection.Close();
            return list;
        }

        public Artikel GetArtikel(int Id)
        {
            if (Id == 0)
            {
                return new Artikel();
            }
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetArtikelById", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter artikelId = new SqlParameter("@Id", SqlDbType.Int);
            artikelId.Value = Id;
            myCommand.SelectCommand.Parameters.Add(artikelId);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Artikel");
            List<DataRow> stocks = GetStocksById(Id);
            List<DataRow> metatags = GetArtikelMetaTagsForArtikel(Id);
            List<Unit> units = GetUnitsForArtikel(Id);
            Artikel artikel = new Artikel(ds.Tables["Artikel"].Rows[0], stocks);
            artikel.ConvertArtikelMetaTags(metatags);
            artikel.Units = units;
            myConnection.Close();
            return artikel;
        }


        public Unit GetUnitById(int id)
        {
            if (id == 0) return null;
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetUnitById", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pId = new SqlParameter("@Id", SqlDbType.Int);
            pId.Value = id;
            myCommand.SelectCommand.Parameters.Add(pId);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Unit");

            if (ds.Tables["Unit"].Rows.Count == 0)
                return null;

            Unit unit = new Unit(ds.Tables["Unit"].Rows[0]);
            myConnection.Close();
            return unit;

        }

        public Artikel GetArtikelByBarcode(string barcode)
        {

            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetArtikelByBarcode", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter artikelId = new SqlParameter("@Barcode", SqlDbType.VarChar);
            artikelId.Value = barcode;
            myCommand.SelectCommand.Parameters.Add(artikelId);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Artikel");
            Artikel artikel = new Artikel(ds.Tables["Artikel"].Rows[0], "type");
            myConnection.Close();
            return artikel;
        }

        public Evenement GetEvenement(int Id)
        {
            if (Id == 0)
            {
                return null;
            }
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetEvenementById", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter evenementId = new SqlParameter("@Id", SqlDbType.Int);
            evenementId.Value = Id;
            myCommand.SelectCommand.Parameters.Add(evenementId);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Evenement");
            if (ds.Tables["Evenement"].Rows.Count == 0)
            {
                return null;
            }
            var evenement = new Evenement(ds.Tables["Evenement"]);

            myConnection.Close();
            return evenement;
        }

        public List<EvenementLijn> GetOverlappingEvenementLijnen(int id)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetStocksWithOverlappingDates", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter evenementId = new SqlParameter("@Id", SqlDbType.Int);
            evenementId.Value = id;
            myCommand.SelectCommand.Parameters.Add(evenementId);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "EvenementLijn");

            List<EvenementLijn> evenementLijnen = new List<EvenementLijn>();
            for (int i = 0; i < ds.Tables["EvenementLijn"].Rows.Count; i++)
            {
                evenementLijnen.Add(new EvenementLijn(ds.Tables["EvenementLijn"].Rows[i]));
            }

            myConnection.Close();
            return evenementLijnen;
        }

        public List<AvailableModel> GetAvailableAantallenVoorEvenement(Evenement evenement)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetAvailableAantalPerArtikelEvenement", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter evenementId = new SqlParameter("@Id", SqlDbType.Int);
            evenementId.Value = evenement.Id;
            myCommand.SelectCommand.Parameters.Add(evenementId);

            SqlParameter beginTijd = new SqlParameter("@BeginTijd", SqlDbType.DateTime);
            beginTijd.Value = evenement.BeginTijd;
            myCommand.SelectCommand.Parameters.Add(beginTijd);

            SqlParameter eindTijd = new SqlParameter("@EindTijd", SqlDbType.DateTime);
            eindTijd.Value = evenement.EindTijd;
            myCommand.SelectCommand.Parameters.Add(eindTijd);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "AvailableModel");

            List<AvailableModel> result = new List<AvailableModel>();
            for (int i = 0; i < ds.Tables["AvailableModel"].Rows.Count; i++)
            {
                result.Add(new AvailableModel(ds.Tables["AvailableModel"].Rows[i]));
            }

            myConnection.Close();
            return result;
        }

        public List<AvailableModel> GetAvailableAantallenVoorOrder(Code.Order order)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetAvailableAantalPerArtikelOrder", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter evenementId = new SqlParameter("@Id", SqlDbType.Int);
            evenementId.Value = order.Id;
            myCommand.SelectCommand.Parameters.Add(evenementId);

            SqlParameter beginTijd = new SqlParameter("@BeginTijd", SqlDbType.DateTime);
            beginTijd.Value = order.BeginTijd;
            myCommand.SelectCommand.Parameters.Add(beginTijd);

            
            SqlParameter eindTijd = new SqlParameter("@EindTijd", SqlDbType.DateTime);
            eindTijd.Value = order.EindTijd;
            myCommand.SelectCommand.Parameters.Add(eindTijd);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "AvailableModel");

            List<AvailableModel> result = new List<AvailableModel>();
            for (int i = 0; i < ds.Tables["AvailableModel"].Rows.Count; i++)
            {
                result.Add(new AvailableModel(ds.Tables["AvailableModel"].Rows[i]));
            }

            myConnection.Close();
            return result;
        } 

        public List<OrderLijn> GetOverlappingOrderLijnen(int id)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetOrdersWithOverlappingDates", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter orderId = new SqlParameter("@Id", SqlDbType.Int);
            orderId.Value = id;
            myCommand.SelectCommand.Parameters.Add(orderId);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "OrderLijn");

            List<OrderLijn> orderLijnen = new List<OrderLijn>();
            for (int i = 0; i < ds.Tables["OrderLijn"].Rows.Count; i++)
            {
                orderLijnen.Add(new OrderLijn(ds.Tables["OrderLijn"].Rows[i]));
            }

            myConnection.Close();
            return orderLijnen;
        } 

        public List<EvenementLijn> GetVerbruikbareLijnenKomendeEvenementen(int id)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetVerbruikbareLijnenKomendeEvenementen", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter evenementId = new SqlParameter("@Id", SqlDbType.Int);
            evenementId.Value = id;
            myCommand.SelectCommand.Parameters.Add(evenementId);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "EvenementLijn");

            List<EvenementLijn> evenementLijnen = new List<EvenementLijn>();
            for (int i = 0; i < ds.Tables["EvenementLijn"].Rows.Count; i++)
            {
                evenementLijnen.Add(new EvenementLijn(ds.Tables["EvenementLijn"].Rows[i]));
            }

            myConnection.Close();
            return evenementLijnen;
        }


        public List<OrderLijn> GetOrderLijnenByOrderId(int id)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetOrderLijnenByOrderId", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter orderId = new SqlParameter("@Id", SqlDbType.Int);
            orderId.Value = id;
            myCommand.SelectCommand.Parameters.Add(orderId);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "OrderLijn");

            Dictionary<string, OrderLijn> dict = new Dictionary<string, OrderLijn>();

            for (int i = 0; i < ds.Tables["OrderLijn"].Rows.Count; i++)
            {
                string key = ds.Tables["OrderLijn"].Rows[i]["OrderId"] + "-" + ds.Tables["OrderLijn"].Rows[i]["ArtikelId"];
                if (!dict.ContainsKey(key))
                    dict.Add(key, new OrderLijn(ds.Tables["OrderLijn"].Rows[i]));
                else
                    dict[key].Artikel.Stocks.Add(new Stock(ds.Tables["OrderLijn"].Rows[i], dict[key].Artikel));
            }

            myConnection.Close();
            return dict.Select(e => e.Value).ToList();
        }

        public List<EvenementLijn> GetEvenementLijnen(int id)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetEvenementLijnenByEvenementId", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter evenementId = new SqlParameter("@Id", SqlDbType.Int);
            evenementId.Value = id;
            myCommand.SelectCommand.Parameters.Add(evenementId);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "EvenementLijn");

            Dictionary<string, EvenementLijn> dict = new Dictionary<string, EvenementLijn>();

            for (int i = 0; i < ds.Tables["EvenementLijn"].Rows.Count; i++)
            {
                string key = ds.Tables["EvenementLijn"].Rows[i]["EvenementId"] + "-" + ds.Tables["EvenementLijn"].Rows[i]["ArtikelId"];
                if (!dict.ContainsKey(key))
                {
                    dict.Add(key, new EvenementLijn(ds.Tables["EvenementLijn"].Rows[i]));
                }
                else
                {
                    dict[key].Artikel.Stocks.Add(new Stock(ds.Tables["EvenementLijn"].Rows[i], dict[key].Artikel));
                }
            }
            
            myConnection.Close();
            return dict.Select(e => e.Value).ToList();
        }

        public List<Lijn> GetLijnen(dynamic model)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            var procedure = model is Evenement ? "uspGetLijnenEvenement" : "uspGetLijnenOrder";
            SqlDataAdapter myCommand = new SqlDataAdapter(procedure, myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pId = new SqlParameter("@Id", SqlDbType.Int);
            pId.Value = model.Id;
            myCommand.SelectCommand.Parameters.Add(pId);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Lijn");
            Dictionary<string, Lijn> dict = new Dictionary<string, Lijn>();

            for (int i = 0; i < ds.Tables["Lijn"].Rows.Count; i++)
            {
                string key = ds.Tables["Lijn"].Rows[i]["LijnId"] + "-" + ds.Tables["Lijn"].Rows[i]["ArtikelId"];
                if (!dict.ContainsKey(key))
                {
                    dict.Add(key, new Lijn(ds.Tables["Lijn"].Rows[i], model));
                }
                else
                {
                    dict[key].Artikel.Stocks.Add(new Stock(ds.Tables["Lijn"].Rows[i], dict[key].Artikel));
                }
            }

            myConnection.Close();
            return dict.Select(e => e.Value).ToList();
        }

        public List<Tuple<int, int>> GetUnavailableLijnen()
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetUnavailableLijnen", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Lijn");
            List<Tuple<int, int>> result = new List<Tuple<int, int>>();

            for (int i = 0; i < ds.Tables["Lijn"].Rows.Count; i++)
            {
                var row = ds.Tables["Lijn"].Rows[i];
                result.Add(new Tuple<int, int>(Convert.ToInt32(row["StockId"]), Convert.ToInt32(row["AantalWeg"])));
            }
            myConnection.Close();
            return result;
        }

        public List<Recht> GetRechten()
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetRechten", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Recht");
            List<Recht> list = new List<Recht>();

            for (int i = 0; i < ds.Tables["Recht"].Rows.Count; i++)
            {
                list.Add(new Recht(ds.Tables["Recht"].Rows[i]));
            }

            myConnection.Close();
            return list;
        }

        public List<Recht> GetRechtenForRol(string Id)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetRechtenForRol", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter rolId = new SqlParameter("@Id", SqlDbType.NVarChar);
            rolId.Value = Id;
            myCommand.SelectCommand.Parameters.Add(rolId);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Recht");
            List<Recht> list = new List<Recht>();

            for (int i = 0; i < ds.Tables["Recht"].Rows.Count; i++)
            {
                list.Add(new Recht(ds.Tables["Recht"].Rows[i]));
            }

            myConnection.Close();
            return list;
        }

        public Recht GetRechtenForRolAndModule(string RolId, int ModuleId)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetRechtenForModuleAndRol", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter rolId = new SqlParameter("@RolId", SqlDbType.NVarChar);
            rolId.Value = RolId;
            myCommand.SelectCommand.Parameters.Add(rolId);

            SqlParameter moduleId = new SqlParameter("@ModuleId", SqlDbType.Int);
            moduleId.Value = ModuleId;
            myCommand.SelectCommand.Parameters.Add(moduleId);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Recht");
            Recht recht = null;
            if (ds.Tables["Recht"].Rows.Count > 0)
            {
                recht = new Recht(ds.Tables["Recht"].Rows[0]);
            }

            myConnection.Close();
            return recht;
        }

        public Recht GetRechtenForUserAndModule(string UserId, string ModuleNaam)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetRechtenForUserAndModule", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter userId = new SqlParameter("@UserId", SqlDbType.NVarChar);
            userId.Value = UserId;
            myCommand.SelectCommand.Parameters.Add(userId);

            SqlParameter moduleNaam = new SqlParameter("@ModuleNaam", SqlDbType.NVarChar);
            moduleNaam.Value = ModuleNaam;
            myCommand.SelectCommand.Parameters.Add(moduleNaam);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Recht");
            Recht recht = null;

            if (ds.Tables["Recht"].Rows.Count > 0)
            {
                recht = new Recht(ds.Tables["Recht"].Rows[0]);
            }

            myConnection.Close();
            return recht;
        }

        public List<DataRow> GetStocksById(int Id)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetStocksById", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter artikelId = new SqlParameter("@Id", SqlDbType.Int);
            artikelId.Value = Id;
            myCommand.SelectCommand.Parameters.Add(artikelId);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Stock");
            List<DataRow> list = new List<DataRow>();

            for (int i = 0; i < ds.Tables["Stock"].Rows.Count; i++)
            {
                list.Add(ds.Tables["Stock"].Rows[i]);
            }

            myConnection.Close();
            return list;
        }

        public List<DataRow> GetArtikelMetaTagsForArtikel(int Id)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetArtikelMetaTagsForArtikel", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter artikelId = new SqlParameter("@ArtikelId", SqlDbType.Int);
            artikelId.Value = Id;
            myCommand.SelectCommand.Parameters.Add(artikelId);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "ArtikelMetaTag");
            List<DataRow> list = new List<DataRow>();

            for (int i = 0; i < ds.Tables["ArtikelMetaTag"].Rows.Count; i++)
            {
                list.Add(ds.Tables["ArtikelMetaTag"].Rows[i]);
            }

            myConnection.Close();
            return list;
        }

        /*public List<Unit> GetUnits()
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetUnits", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;
            
            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Unit");
            List<Unit> units = new List<Unit>();
            for (int i = 0; i < ds.Tables["Unit"].Rows.Count; i++)
            {
                units.Add(new Unit(ds.Tables["Unit"].Rows[i]));
            }

            myConnection.Close();
            return units;
        }*/

        public List<Unit> GetUnitsForArtikel(int Id)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetUnitsForArtikel", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter artikelId = new SqlParameter("@ArtikelId", SqlDbType.Int);
            artikelId.Value = Id;
            myCommand.SelectCommand.Parameters.Add(artikelId);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Unit");
            List<Unit> units = new List<Unit>();
            for (int i = 0; i < ds.Tables["Unit"].Rows.Count; i++)
            {
                units.Add(new Unit(ds.Tables["Unit"].Rows[i]));
            }

            myConnection.Close();
            return units;
        }

        public void UpdateOrder(Code.Order order)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspUpdateOrder", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pId = new SqlParameter("@Id", SqlDbType.Int);
            pId.Value = order.Id;
            myCommand.Parameters.Add(pId);

            SqlParameter pNaam = new SqlParameter("@Naam", SqlDbType.NVarChar);
            pNaam.Value = order.Naam;
            myCommand.Parameters.Add(pNaam);

            SqlParameter pBeginTijd = new SqlParameter("@BeginTijd", SqlDbType.DateTime);
            pBeginTijd.Value = order.BeginTijd;
            myCommand.Parameters.Add(pBeginTijd);

            if (order.EindTijd != DateTime.MaxValue)
            {
                SqlParameter pEindTijd = new SqlParameter("@EindTijd", SqlDbType.DateTime);
                pEindTijd.Value = order.EindTijd;
                myCommand.Parameters.Add(pEindTijd);
            }
           

            SqlParameter pTel = new SqlParameter("@Tel", SqlDbType.NVarChar);
            pTel.Value = order.Tel;
            myCommand.Parameters.Add(pTel);

            SqlParameter pContactNaam = new SqlParameter("@ContactNaam", SqlDbType.NVarChar);
            pContactNaam.Value = order.ContactNaam;
            myCommand.Parameters.Add(pContactNaam);

            SqlParameter pIsVerhuur = new SqlParameter("@IsVerhuur", SqlDbType.Bit);
            pIsVerhuur.Value = order.IsVerhuur;
            myCommand.Parameters.Add(pIsVerhuur);

            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();
        }
            
        public void UpdateStock(int id, DateTime vervaldatum, int aantal, int artikellocatieid, int artikelid, int unitId)
        {

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspUpdateStock", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pId = new SqlParameter("@Id", SqlDbType.Int);
            pId.Value = id;
            myCommand.Parameters.Add(pId);

            if (vervaldatum != DateTime.MinValue )
            {
                if (vervaldatum.ToShortDateString() != DateTime.MaxValue.ToShortDateString())
                {
                    SqlParameter pVervaldatum = new SqlParameter("@Vervaldatum", SqlDbType.DateTime);
                    pVervaldatum.Value = vervaldatum;
                    myCommand.Parameters.Add(pVervaldatum);
                }
            }


            SqlParameter pAantal = new SqlParameter("@Aantal", SqlDbType.Int);
            pAantal.Value = aantal;
            myCommand.Parameters.Add(pAantal);

            SqlParameter pArtikelLocatieId = new SqlParameter("@ArtikelLocatieId", SqlDbType.Int);
            pArtikelLocatieId.Value = artikellocatieid;
            myCommand.Parameters.Add(pArtikelLocatieId);

            SqlParameter pArtikelId = new SqlParameter("@ArtikelId", SqlDbType.Int);
            pArtikelId.Value = artikelid;
            myCommand.Parameters.Add(pArtikelId);

            SqlParameter pUnitId = new SqlParameter("@UnitId", SqlDbType.Int);
            pUnitId.Value = unitId;
            myCommand.Parameters.Add(pUnitId);

            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();


        }





        public void UpdateArtikel(int id, string naam, string barcode, float gewicht, float naturaprijs, float verhuurprijs, bool isherbruikbaar, int merkid, int categorieid, int stopcontact)
        {

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspUpdateArtikel", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pId = new SqlParameter("@Id", SqlDbType.Int);
            pId.Value = id;
            myCommand.Parameters.Add(pId);

            SqlParameter pNaam = new SqlParameter("@Naam", SqlDbType.VarChar, 100);
            pNaam.Value = naam;
            myCommand.Parameters.Add(pNaam);

            SqlParameter pOmschrijving = new SqlParameter("@Omschrijving", SqlDbType.VarChar, 255);
            pOmschrijving.Value = "";
            myCommand.Parameters.Add(pOmschrijving);

            SqlParameter pBarcode = new SqlParameter("@Barcode", SqlDbType.VarChar, 50);
            pBarcode.Value = barcode;
            myCommand.Parameters.Add(pBarcode);

            if (gewicht != -1)
            {
                SqlParameter pGewicht = new SqlParameter("@Gewicht", SqlDbType.Float);
                pGewicht.Value = gewicht;
                myCommand.Parameters.Add(pGewicht);
            }

            if (naturaprijs != -1)
            {
                SqlParameter pNaturaPrijs = new SqlParameter("@NaturaPrijs", SqlDbType.Float);
                pNaturaPrijs.Value = naturaprijs;
                myCommand.Parameters.Add(pNaturaPrijs);
            }

            if (verhuurprijs != -1)
            {
                SqlParameter pVerhuurPrijs = new SqlParameter("@VerhuurPrijs", SqlDbType.Float);
                pVerhuurPrijs.Value = verhuurprijs;
                myCommand.Parameters.Add(pVerhuurPrijs);
            }

            if (stopcontact != -1)
            {
                SqlParameter pStopcontact = new SqlParameter("@Stopcontact", SqlDbType.Int);
                pStopcontact.Value = stopcontact;
                myCommand.Parameters.Add(pStopcontact);
            }


            SqlParameter pIsHerbruikbaar = new SqlParameter("@IsHerbruikbaar", SqlDbType.Bit);
            pIsHerbruikbaar.Value = isherbruikbaar;
            myCommand.Parameters.Add(pIsHerbruikbaar);

            if (merkid != -1)
            {
                SqlParameter pMerkId = new SqlParameter("@Merkid", SqlDbType.Int);
                pMerkId.Value = merkid;
                myCommand.Parameters.Add(pMerkId);
            }



            SqlParameter pCategorieId = new SqlParameter("@CategorieId", SqlDbType.Int);
            pCategorieId.Value = categorieid;
            myCommand.Parameters.Add(pCategorieId);


            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();


        }

        public void UpdateUnit(int id, string naam, int aantal, float prijs, string barcode, int artikelid, int childunit)
        {

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspUpdateUnit", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pId = new SqlParameter("@Id", SqlDbType.Int);
            pId.Value = id;
            myCommand.Parameters.Add(pId);

            SqlParameter pNaam = new SqlParameter("@NaamEnkelvoud", SqlDbType.VarChar, 30);
            pNaam.Value = naam;
            myCommand.Parameters.Add(pNaam);

            SqlParameter pNaamM = new SqlParameter("@NaamMeervoud", SqlDbType.VarChar, 30);
            pNaamM.Value = naam;
            myCommand.Parameters.Add(pNaamM);

            SqlParameter pAantal = new SqlParameter("@Aantal", SqlDbType.Int);
            pAantal.Value = aantal;
            myCommand.Parameters.Add(pAantal);

            if (prijs != -1)
            {
                SqlParameter pPrijs = new SqlParameter("@Prijs", SqlDbType.Float);
                pPrijs.Value = prijs;
                myCommand.Parameters.Add(pPrijs);
            }

            SqlParameter pBarcode = new SqlParameter("@Barcode", SqlDbType.VarChar, 20);
            pBarcode.Value = barcode;
            myCommand.Parameters.Add(pBarcode);

            SqlParameter pArtikelId = new SqlParameter("@ArtikelId", SqlDbType.Int);
            pArtikelId.Value = artikelid;
            myCommand.Parameters.Add(pArtikelId);

            SqlParameter pChildUnit = new SqlParameter("@ChildUnit", SqlDbType.Int);
            pChildUnit.Value = childunit;
            myCommand.Parameters.Add(pChildUnit);

            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();


        }

        public void UpdateArtikelMetaTag(int id, string naam, string waarde, int artikelid)
        {

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspAddArtikelMetaTag", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pId = new SqlParameter("@Id", SqlDbType.Int);
            pId.Value = id;
            myCommand.Parameters.Add(pId);

            SqlParameter pNaam = new SqlParameter("@Naam", SqlDbType.VarChar, 50);
            pNaam.Value = naam;
            myCommand.Parameters.Add(pNaam);

            SqlParameter pWaarde = new SqlParameter("@Waarde", SqlDbType.VarChar, 100);
            pWaarde.Value = waarde;
            myCommand.Parameters.Add(pWaarde);

            SqlParameter pArtikelId = new SqlParameter("@ArtikelId", SqlDbType.Int);
            pArtikelId.Value = artikelid;
            myCommand.Parameters.Add(pArtikelId);

            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();


        }

        public void DeleteStock(int id)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspDeleteStock", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pId = new SqlParameter("@Id", SqlDbType.Int);
            pId.Value = id;
            myCommand.Parameters.Add(pId);

            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();
        }

        public void DeleteRecht(string id)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspRemoveRechtenWithRole", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pId = new SqlParameter("@RolId", SqlDbType.VarChar);
            pId.Value = id;
            myCommand.Parameters.Add(pId);

            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();
        }

        public void DeleteArtikel(int id)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspDeleteArtikel", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pId = new SqlParameter("@Id", SqlDbType.Int);
            pId.Value = id;
            myCommand.Parameters.Add(pId);

            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();
        }

        public void DeleteGebruiker(int id)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspDeleteGebruiker", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pId = new SqlParameter("@Id", SqlDbType.Int);
            pId.Value = id;
            myCommand.Parameters.Add(pId);

            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();
        }

        public void DeleteArtikelMetaTag(int id)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspDeleteStock", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pId = new SqlParameter("@Id", SqlDbType.Int);
            pId.Value = id;
            myCommand.Parameters.Add(pId);

            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();
        }

        public void DeleteUnit(int id)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspDeleteUnit", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pId = new SqlParameter("@Id", SqlDbType.Int);
            pId.Value = id;
            myCommand.Parameters.Add(pId);

            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();
        }

        public List<Gebruiker> GetGebruikers()
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetGebruikers", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Gebruiker");
            List<Gebruiker> list = new List<Gebruiker>();
            for (int i = 0; i < ds.Tables["Gebruiker"].Rows.Count; i++)
            {
                list.Add(new Gebruiker(ds.Tables["Gebruiker"].Rows[i]));
            }
            myConnection.Close();
            return list;
        }

        public List<EvenementKlant> GetEvenementKlantenDistinct()
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetEvenementKlantenDistinct", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            myCommand.Fill(ds, "EvenementKlant");
            List<EvenementKlant> list = new List<EvenementKlant>();
            for (int i = 0; i < ds.Tables["EvenementKlant"].Rows.Count; i++)
            {
                list.Add(new EvenementKlant(ds.Tables["EvenementKlant"].Rows[i], 0));
            }
            myConnection.Close();
            return list;
        }

        public List<EvenementKlant> GetEvenementKlanten()
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetEvenementKlanten", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            myCommand.Fill(ds, "EvenementKlant");
            List<EvenementKlant> list = new List<EvenementKlant>();
            for (int i = 0; i < ds.Tables["EvenementKlant"].Rows.Count; i++)
            {
                list.Add(new EvenementKlant(ds.Tables["EvenementKlant"].Rows[i]));
            }
            myConnection.Close();
            return list;
        }

        public EvenementLocatie GetEvenementLocatieById(int Id)
        {
            if (Id == 0)
            {
                return new EvenementLocatie();
            }
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetEvenementLocatieById", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter klantId = new SqlParameter("@Id", SqlDbType.Int);
            klantId.Value = Id;
            myCommand.SelectCommand.Parameters.Add(klantId);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "EvenementLocatie");
            var evenementLocatie = new EvenementLocatie(ds.Tables["EvenementLocatie"].Rows[0]);

            myConnection.Close();
            return evenementLocatie;
        }

        public List<EvenementKlant> GetEvenementKlantenByOrganisatie(string organisatie)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetEvenementKlantByOrganisatie", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter Organisatie = new SqlParameter("@Organisatie", SqlDbType.VarChar, 50);
            Organisatie.Value = organisatie;
            myCommand.SelectCommand.Parameters.Add(Organisatie);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "EvenementKlant");
            List<EvenementKlant> list = new List<EvenementKlant>();
            for (int i = 0; i < ds.Tables["EvenementKlant"].Rows.Count; i++)
            {
                list.Add(new EvenementKlant(ds.Tables["EvenementKlant"].Rows[i]));
            }
            myConnection.Close();
            return list;
        }

        public List<Locatie> GetLocaties()
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetLocaties", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Locatie");
            List<Locatie> list = new List<Locatie>();
            for (int i = 0; i < ds.Tables["Locatie"].Rows.Count; i++)
            {
                list.Add(new Locatie(ds.Tables["Locatie"].Rows[i]));
            }
            myConnection.Close();
            return list;
        }

        public List<Locatie> GetLocatiesByOrganisatie(string organisatie)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetLocatiesByOrganisatie", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter Organisatie = new SqlParameter("@Organisatie", SqlDbType.VarChar, 50);
            Organisatie.Value = organisatie;
            myCommand.SelectCommand.Parameters.Add(Organisatie);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Locatie");
            List<Locatie> list = new List<Locatie>();
            for (int i = 0; i < ds.Tables["Locatie"].Rows.Count; i++)
            {
                list.Add(new Locatie(ds.Tables["Locatie"].Rows[i]));
            }
            myConnection.Close();
            return list;
        }

        public List<EvenementTransport> GetEvenementTransportenByEvenementId(Evenement evenement)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetTransportenByEvenementId", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pId = new SqlParameter("@Id", SqlDbType.Int);
            pId.Value = evenement.Id;
            myCommand.SelectCommand.Parameters.Add(pId);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "EvenementTransport");
            List<EvenementTransport> list = new List<EvenementTransport>();
            for (int i = 0; i < ds.Tables["EvenementTransport"].Rows.Count; i++)
            {
                list.Add(new EvenementTransport(ds.Tables["EvenementTransport"].Rows[i], evenement));
            }

            myConnection.Close();
            return list;
        } 

        public List<Transport> getTransporten()
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetTransporten", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Transport");
            List<Transport> list = new List<Transport>();
            for (int i = 0; i < ds.Tables["Transport"].Rows.Count; i++)
            {
                list.Add(new Transport(ds.Tables["Transport"].Rows[i]));
            }
            myConnection.Close();
            return list;
        }

        public int AddLocatie(string adres, string postcode, string land, string plaats, string zaal)
        {

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspAddLocatie", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pStraat = new SqlParameter("@Straat", SqlDbType.VarChar, 50);
            pStraat.Value = adres;
            myCommand.Parameters.Add(pStraat);

            SqlParameter pPostcode = new SqlParameter("@Postcode", SqlDbType.VarChar, 10);
            pPostcode.Value = postcode;
            myCommand.Parameters.Add(pPostcode);

            SqlParameter pLand = new SqlParameter("@Land", SqlDbType.VarChar, 40);
            pLand.Value = land;
            myCommand.Parameters.Add(pLand);

            SqlParameter pPlaats = new SqlParameter("@Plaats", SqlDbType.VarChar, 40);
            pPlaats.Value = plaats;
            myCommand.Parameters.Add(pPlaats);

            SqlParameter pZaal = new SqlParameter("@Zaal", SqlDbType.VarChar, 50);
            pZaal.Value = zaal;
            myCommand.Parameters.Add(pZaal);


            int i = 0;
            myConnection.Open();
            i = (int)myCommand.ExecuteScalar();
            myConnection.Close();

            return i;

        }

        public int AddEvenementKlant(string organisatie, string contactnaam, string contacttel, string contactemail)
        {

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspAddEvenementKlant", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pOrganisatie = new SqlParameter("@Organisatie", SqlDbType.VarChar, 50);
            pOrganisatie.Value = organisatie;
            myCommand.Parameters.Add(pOrganisatie);

            SqlParameter pContactNaam = new SqlParameter("@ContactNaam", SqlDbType.VarChar, 50);
            pContactNaam.Value = contactnaam;
            myCommand.Parameters.Add(pContactNaam);

            SqlParameter pContactTel = new SqlParameter("@ContactTel", SqlDbType.VarChar, 50);
            pContactTel.Value = contacttel;
            myCommand.Parameters.Add(pContactTel);

            //SqlParameter pContactEmail = new SqlParameter("@ContactEmail", SqlDbType.VarChar, 255);
            //pContactEmail.Value = contactemail;
            //myCommand.Parameters.Add(pContactEmail);

            int i = 0;
            myConnection.Open();
            i = (int)myCommand.ExecuteScalar();
            myConnection.Close();

            return i;

        }

        public int AddGebruiker(string voornaam, string achternaam, string tel, string email)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspAddGebruiker", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pVoornaam = new SqlParameter("@Voornaam", SqlDbType.VarChar, 50);
            pVoornaam.Value = voornaam;
            myCommand.Parameters.Add(pVoornaam);

            SqlParameter pAchternaam = new SqlParameter("@Achternaam", SqlDbType.VarChar, 50);
            pAchternaam.Value = achternaam;
            myCommand.Parameters.Add(pAchternaam);

            SqlParameter pTel = new SqlParameter("@Tel", SqlDbType.VarChar, 20);
            pTel.Value = tel;
            myCommand.Parameters.Add(pTel);

            SqlParameter pEmail = new SqlParameter("@Email", SqlDbType.VarChar, 255);
            pEmail.Value = email;
            myCommand.Parameters.Add(pEmail);

            int i = 0;
            myConnection.Open();
            i = (int)myCommand.ExecuteScalar();
            myConnection.Close();

            return i;
        }

        public void UpdateEvenement(string naam, string opmerking, DateTime vertrektransport, int opdrachtgeverid,
            int eventcoordinatorid, int fieldmanagerid, int id)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspUpdateEvenement", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pNaam = new SqlParameter("@Naam", SqlDbType.VarChar, 100);
            pNaam.Value = naam;
            myCommand.Parameters.Add(pNaam);

            if (opmerking != "null")
            {
                SqlParameter pOpmerking = new SqlParameter("@Opmerking", SqlDbType.VarChar, -1);
                pOpmerking.Value = opmerking;
                myCommand.Parameters.Add(pOpmerking);
            }

            if (vertrektransport != DateTime.MinValue)
            {
                if (vertrektransport != DateTime.MaxValue)
                {
                    SqlParameter pvertrekTransport = new SqlParameter("@VertrekTransport", SqlDbType.DateTime);
                    pvertrekTransport.Value = vertrektransport;
                    myCommand.Parameters.Add(pvertrekTransport);
                }
            }

            SqlParameter pOpdrachtGeverId = new SqlParameter("@OpdrachtGeverId", SqlDbType.Int);
            pOpdrachtGeverId.Value = opdrachtgeverid;
            myCommand.Parameters.Add(pOpdrachtGeverId);

            SqlParameter pEventCoordinatorId = new SqlParameter("@EventCoordinatorId", SqlDbType.Int);
            pEventCoordinatorId.Value = eventcoordinatorid;
            myCommand.Parameters.Add(pEventCoordinatorId);

            SqlParameter pFieldManagerId = new SqlParameter("@FieldManagerId", SqlDbType.Int);
            pFieldManagerId.Value = fieldmanagerid;
            myCommand.Parameters.Add(pFieldManagerId);

            SqlParameter pId = new SqlParameter("@Id", SqlDbType.Int);
            pId.Value = id;
            myCommand.Parameters.Add(pId);

            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();
        }

        public int AddEvenement(string naam, string opmerking, DateTime vertrektransport, int opdrachtgeverid, int eventcoordinatorid, int fieldmanagerid)
        {

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspAddEvenement", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pNaam = new SqlParameter("@Naam", SqlDbType.VarChar, 100);
            pNaam.Value = naam;
            myCommand.Parameters.Add(pNaam);

            if (opmerking != "null")
            {
                SqlParameter pOpmerking = new SqlParameter("@Opmerking", SqlDbType.VarChar, -1);
                pOpmerking.Value = opmerking;
                myCommand.Parameters.Add(pOpmerking);
            }

            if(vertrektransport != DateTime.MinValue)
            {
                if (vertrektransport != DateTime.MaxValue)
                {
                    SqlParameter pvertrekTransport = new SqlParameter("@VertrekTransport", SqlDbType.DateTime);
                    pvertrekTransport.Value = vertrektransport;
                    myCommand.Parameters.Add(pvertrekTransport);
                }
            }

            SqlParameter pOpdrachtGeverId = new SqlParameter("@OpdrachtGeverId", SqlDbType.Int);
            pOpdrachtGeverId.Value = opdrachtgeverid;
            myCommand.Parameters.Add(pOpdrachtGeverId);

            SqlParameter pEventCoordinatorId = new SqlParameter("@EventCoordinatorId", SqlDbType.Int);
            pEventCoordinatorId.Value = eventcoordinatorid;
            myCommand.Parameters.Add(pEventCoordinatorId);

            SqlParameter pFieldManagerId = new SqlParameter("@FieldManagerId", SqlDbType.Int);
            pFieldManagerId.Value = fieldmanagerid;
            myCommand.Parameters.Add(pFieldManagerId);

            int i = 0;
            myConnection.Open();
            i = (int)myCommand.ExecuteScalar();
            myConnection.Close();

            return i;

        }

        public void AddEvenementLocatie(int evenementid, int locatieid, DateTime begintijd, DateTime eindtijd, int verwachteopkomst, int target, int evenementklantid)
        {

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspAddEvenementLocatie", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pEvenementId = new SqlParameter("@EvenementId", SqlDbType.Int);
            pEvenementId.Value = evenementid;
            myCommand.Parameters.Add(pEvenementId);

            SqlParameter pLocatieId = new SqlParameter("@LocatieId", SqlDbType.Int);
            pLocatieId.Value = locatieid;
            myCommand.Parameters.Add(pLocatieId);

            
            SqlParameter pBeginTijd = new SqlParameter("@BeginTijd", SqlDbType.DateTime);
            pBeginTijd.Value = begintijd;
            myCommand.Parameters.Add(pBeginTijd);

            SqlParameter pEindTijd = new SqlParameter("@EindTijd", SqlDbType.DateTime);
            pEindTijd.Value = eindtijd;
            myCommand.Parameters.Add(pEindTijd);

            if(verwachteopkomst != -1)
            {
                SqlParameter pVerwachteOpkomst = new SqlParameter("@VerwachteOpkomst", SqlDbType.Int);
                pVerwachteOpkomst.Value = verwachteopkomst;
                myCommand.Parameters.Add(pVerwachteOpkomst);
            }

            if(target != -1)
            {
                SqlParameter pTarget = new SqlParameter("@Target", SqlDbType.Int);
                pTarget.Value = target;
                myCommand.Parameters.Add(pTarget);
            }

            SqlParameter pEvenementKlantId = new SqlParameter("@EvenementKlantId", SqlDbType.Int);
            pEvenementKlantId.Value = evenementklantid;
            myCommand.Parameters.Add(pEvenementKlantId);

            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();


        }


        public int AddTaak(string naam)
        {

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspAddTaak ", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pNaam = new SqlParameter("@Naam", SqlDbType.VarChar, 100);
            pNaam.Value = naam;
            myCommand.Parameters.Add(pNaam);

            int i = 0;
            myConnection.Open();
            i = (int)myCommand.ExecuteScalar();
            myConnection.Close();

            return i;

        }

        public void AddEvenementTaak(DateTime van, DateTime tot, int evenementid, int gebruikerid, int taakid)
        {

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspAddEvenementTaak", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pVan = new SqlParameter("@Van", SqlDbType.DateTime);
            pVan.Value = van;
            myCommand.Parameters.Add(pVan);

            SqlParameter pTot = new SqlParameter("@Tot", SqlDbType.DateTime);
            pTot.Value = tot;
            myCommand.Parameters.Add(pTot);

            SqlParameter pEvenementId = new SqlParameter("@EvenementId", SqlDbType.Int);
            pEvenementId.Value = evenementid;
            myCommand.Parameters.Add(pEvenementId);

            SqlParameter pGebruikerId = new SqlParameter("@GebruikerId", SqlDbType.Int);
            pGebruikerId.Value = gebruikerid;
            myCommand.Parameters.Add(pGebruikerId);

            SqlParameter pTaakId = new SqlParameter("@TaakId", SqlDbType.Int);
            pTaakId.Value = taakid;
            myCommand.Parameters.Add(pTaakId);


            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();


        }

        public void UpdateEvenementTeammleden(List<EvenementTeamLid> evenementTeamleden, int id)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Id");
            dataTable.Columns.Add("Naam");
            dataTable.Columns.Add("Functie");
            dataTable.Columns.Add("Tel");
            dataTable.Columns.Add("Email");
            dataTable.Columns.Add("EvenementId");

            foreach (var teamlid in evenementTeamleden)
            {
                var row = dataTable.NewRow();
                row["Id"] = 0;
                row["Naam"] = teamlid.Naam;
                row["Functie"] = teamlid.Functie;
                row["Tel"] = teamlid.Tel;
                row["Email"] = teamlid.Email;
                row["EvenementId"] = id;
                dataTable.Rows.Add(row);
            }

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspInsertOrUpdateEvenementTeamleden", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pEvenementId = new SqlParameter("@EvenementId", SqlDbType.Int);
            pEvenementId.Value = id;
            myCommand.Parameters.Add(pEvenementId);

            myCommand.Parameters.AddWithValue("@tblEvenementTeamleden", dataTable);
            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();
        }

        public bool UpdateEvenementTransporten(List<EvenementTransport> evenementTransporten, int id)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("EvenementId");
            dataTable.Columns.Add("TransportId");
            dataTable.Columns.Add("ChauffeurHeen");
            dataTable.Columns.Add("ChauffeurTerug");
            dataTable.Columns.Add("Vertrek", Type.GetType("System.DateTime"));

            foreach (var transport in evenementTransporten)
            {
                var row = dataTable.NewRow();
                row["EvenementId"] = id;
                row["TransportId"] = transport.Transport.Id;
                row["ChauffeurHeen"] = transport.ChauffeurHeen.Id;
                row["ChauffeurTerug"] = transport.ChauffeurTerug.Id;
                row["Vertrek"] = transport.Vertrek;
                dataTable.Rows.Add(row);
            }

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspInsertOrUpdateEvenementTransporten", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter evenementId = new SqlParameter("@EvenementId", SqlDbType.Int);
            evenementId.Value = id;
            myCommand.Parameters.Add(evenementId);

            myCommand.Parameters.AddWithValue("@tblEvenementTransporten", dataTable);

            bool b = false;
            myConnection.Open();
            b = Convert.ToBoolean(myCommand.ExecuteScalar());
            myConnection.Close();

            return b;
        }

        public bool UpdateEvenementTaak(List<EvenementTaak> evenementTaken, int Id)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Van", Type.GetType("System.DateTime"));
            dataTable.Columns.Add("Tot", Type.GetType("System.DateTime"));
            dataTable.Columns.Add("EvenementId");
            dataTable.Columns.Add("GebruikerId");
            dataTable.Columns.Add("TaakId");


            foreach (var evenementTaak in evenementTaken)
            {
                var row = dataTable.NewRow();
                row["Van"] = evenementTaak.Van;
                row["Tot"] = evenementTaak.Tot;
                row["EvenementId"] = evenementTaak.Evenement.Id;
                row["GebruikerId"] = evenementTaak.Gebruiker.Id;
                row["TaakId"] = evenementTaak.Taak.Id;
                dataTable.Rows.Add(row);
            }

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspUpdateEvenementTaken", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pId = new SqlParameter("@EvenementId", SqlDbType.Int);
            pId.Value = Id;
            myCommand.Parameters.Add(pId);

            myCommand.Parameters.AddWithValue("@tblEvenementTaak", dataTable);

            bool b = false;
            myConnection.Open();
            b = Convert.ToBoolean(myCommand.ExecuteScalar());
            myConnection.Close();

            return b;
        }

        public void AddEvenementTransport(int evenementid, int transportid, int chauffeurheen, int chauffeurterug, DateTime vertrek)
        {

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspAddEvenementTransport ", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pEvenementId = new SqlParameter("@EvenementId", SqlDbType.Int);
            pEvenementId.Value = evenementid;
            myCommand.Parameters.Add(pEvenementId);

            SqlParameter pTransportId = new SqlParameter("@TransportId", SqlDbType.Int);
            pTransportId.Value = transportid;
            myCommand.Parameters.Add(pTransportId);

            if(chauffeurheen != 0)
            {
                SqlParameter pChauffeurHeen = new SqlParameter("@ChauffeurHeen", SqlDbType.Int);
                pChauffeurHeen.Value = chauffeurheen;
                myCommand.Parameters.Add(pChauffeurHeen);
            }

            if (chauffeurterug != 0)
            {
                SqlParameter pChauffeurTerug = new SqlParameter("@ChauffeurTerug", SqlDbType.Int);
                pChauffeurTerug.Value = chauffeurterug;
                myCommand.Parameters.Add(pChauffeurTerug);
            }

            if(vertrek != DateTime.MinValue)
            {
                SqlParameter pVertrek = new SqlParameter("@Vertrek", SqlDbType.DateTime);
                pVertrek.Value = vertrek;
                myCommand.Parameters.Add(pVertrek);
            }

            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();

           

        }

        public int AddEvenementTeamlid(string naam, string functie, string tel, int evenementid)
        {

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspAddEvenementTeamlid", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pNaam = new SqlParameter("@Naam", SqlDbType.VarChar,50);
            pNaam.Value = naam;
            myCommand.Parameters.Add(pNaam);

            SqlParameter pFunctie = new SqlParameter("@Functie", SqlDbType.VarChar, 50);
            pFunctie.Value = functie;
            myCommand.Parameters.Add(pFunctie);

            SqlParameter pTel = new SqlParameter("@Tel", SqlDbType.VarChar, 20);
            pTel.Value = tel;
            myCommand.Parameters.Add(pTel);

            //SqlParameter pEmail = new SqlParameter("@Email", SqlDbType.VarChar, 255);
            //pEmail.Value = email;
            //myCommand.Parameters.Add(pEmail);

            SqlParameter pEvenementId = new SqlParameter("@EvenementId", SqlDbType.Int);
            pEvenementId.Value = evenementid;
            myCommand.Parameters.Add(pEvenementId);


            int i = 0;
            myConnection.Open();
            i = (int)myCommand.ExecuteScalar();
            myConnection.Close();

            return i;

        }

        public void UpdateLocatie(int id, string adres, string postcode, string land, string plaats, string zaal)
        {

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspUpdateLocatie", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pId = new SqlParameter("@Id", SqlDbType.Int);
            pId.Value = id;
            myCommand.Parameters.Add(pId);

            SqlParameter pStraat = new SqlParameter("@Straat", SqlDbType.VarChar, 50);
            pStraat.Value = adres;
            myCommand.Parameters.Add(pStraat);

            SqlParameter pPostcode = new SqlParameter("@Postcode", SqlDbType.VarChar, 10);
            pPostcode.Value = postcode;
            myCommand.Parameters.Add(pPostcode);

            SqlParameter pLand = new SqlParameter("@Land", SqlDbType.VarChar, 40);
            pLand.Value = land;
            myCommand.Parameters.Add(pLand);

            SqlParameter pPlaats = new SqlParameter("@Plaats", SqlDbType.VarChar, 40);
            pPlaats.Value = plaats;
            myCommand.Parameters.Add(pPlaats);

            SqlParameter pZaal = new SqlParameter("@Zaal", SqlDbType.VarChar, 50);
            pZaal.Value = zaal;
            myCommand.Parameters.Add(pZaal);

            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();


        }

        public void UpdateEvenementKlant(string organisatie, string contactnaam, string contacttel, string contactemail)
        {

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspUpdateEvenementKlant", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

           

            SqlParameter pOrganisatie = new SqlParameter("@Organisatie", SqlDbType.VarChar, 50);
            pOrganisatie.Value = organisatie;
            myCommand.Parameters.Add(pOrganisatie);

            SqlParameter pContactNaam = new SqlParameter("@ContactNaam", SqlDbType.VarChar, 50);
            pContactNaam.Value = contactnaam;
            myCommand.Parameters.Add(pContactNaam);

            SqlParameter pContactTel = new SqlParameter("@ContactTel", SqlDbType.VarChar, 50);
            pContactTel.Value = contacttel;
            myCommand.Parameters.Add(pContactTel);

            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();


        }

        public List<Evenement> GetLopendeEvenementen()
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetLopendeEvenementen", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Evenement");
            List<Evenement> result = new List<Evenement>();

            for (int i = 0; i < ds.Tables["Evenement"].Rows.Count; i++)
            {
                var row = ds.Tables["Evenement"].Rows[i];
                int artikelId = Convert.ToInt32(row["EvenementId"].ToString());
                int index = result.FindIndex(a => a.Id == artikelId);
                if (index >= 0)
                {
                    result[index].EvenementLocaties.Add(new EvenementLocatie(Convert.ToDateTime(row["BeginTijd"]),
                        Convert.ToDateTime(row["EindTijd"]), row["Plaats"].ToString()));
                }
                else
                {
                    result.Add(new Evenement(row, 0, "eventcomplete"));
                }
            }

            myConnection.Close();
            return result;
        }

        public List<Evenement> GetKomendeEvenementen()
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetKomendeEvenementen", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Evenement");
            List<Evenement> result = new List<Evenement>();

            for (int i = 0; i < ds.Tables["Evenement"].Rows.Count; i++)
            {
                var row = ds.Tables["Evenement"].Rows[i];
                int artikelId = Convert.ToInt32(row["EvenementId"].ToString());
                int index = result.FindIndex(a => a.Id == artikelId);
                if (index >= 0)
                {
                    result[index].EvenementLocaties.Add(new EvenementLocatie(Convert.ToDateTime(row["BeginTijd"]),
                        Convert.ToDateTime(row["EindTijd"]), row["Plaats"].ToString()));
                }
                else
                {
                    result.Add(new Evenement(row,0));
                }
            }

            myConnection.Close();
            return result;
        }

        public List<EvenementChecklistLijn> GetEvenementChecklistLijnen()
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetEvenementChecklistLijn", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            myCommand.Fill(ds, "EvenementChecklistLijn");
            List<EvenementChecklistLijn> list = new List<EvenementChecklistLijn>();
            for (int i = 0; i < ds.Tables["EvenementChecklistLijn"].Rows.Count; i++)
            {
                list.Add(new EvenementChecklistLijn(ds.Tables["EvenementChecklistLijn"].Rows[i],0));
            }
            myConnection.Close();
            return list;
        }
        public List<EvenementChecklistLijn> GetEvenementChecklistLijnenByEvenementId(int evenementid)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetEvenementChecklistLijnByEvenementId", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pEvenementId = new SqlParameter("@EvenementId", SqlDbType.Int);
            pEvenementId.Value = evenementid;
            myCommand.SelectCommand.Parameters.Add(pEvenementId);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "EvenementChecklistLijn");
            List<EvenementChecklistLijn> list = new List<EvenementChecklistLijn>();
            for (int i = 0; i < ds.Tables["EvenementChecklistLijn"].Rows.Count; i++)
            {
                list.Add(new EvenementChecklistLijn(ds.Tables["EvenementChecklistLijn"].Rows[i]));
            }
            myConnection.Close();
            return list;
        }

        public List<ChecklistLijn> GetChecklistlijnen(dynamic model)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand;
            SqlParameter pEvenementId;
            if (model is Evenement)
            {
                myCommand = new SqlDataAdapter("uspGetEvenementChecklistLijnByEvenementId", myConnection);
                pEvenementId = new SqlParameter("@EvenementId", SqlDbType.Int);
            }
            else
            {
                myCommand = new SqlDataAdapter("uspGetOrderChecklistLijnByOrderId", myConnection);
                pEvenementId = new SqlParameter("@OrderId", SqlDbType.Int);
            }
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            pEvenementId.Value = model.Id;
            myCommand.SelectCommand.Parameters.Add(pEvenementId);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "ChecklistLijn");
            List<ChecklistLijn> list = new List<ChecklistLijn>();
            for (int i = 0; i < ds.Tables["ChecklistLijn"].Rows.Count; i++)
            {
                list.Add(new ChecklistLijn(ds.Tables["ChecklistLijn"].Rows[i], model));
            }
            myConnection.Close();
            return list;
        }

        public void AddChecklistLijnen(List<ChecklistLijn> lijnen)
        {
            DataTable dataTable = new DataTable();
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand;
            if (lijnen[0].Model is Evenement)
            {
                myCommand = new SqlDataAdapter("uspInsertEvenementChecklistLijnen", myConnection);
                dataTable.Columns.Add("EvenementId");
            }
            else
            {
                myCommand = new SqlDataAdapter("uspInsertOrderChecklistLijnen", myConnection);
                dataTable.Columns.Add("OrderId");
            } 
            dataTable.Columns.Add("StockId");
            dataTable.Columns.Add("Aantal");
            dataTable.Columns.Add("AantalWeg");
            dataTable.Columns.Add("Unit");
            foreach (var lijn in lijnen)
            {
                var row = dataTable.NewRow();
                if (lijnen[0].Model is Evenement)
                    row["EvenementId"] = lijn.Model.Id;
                else
                    row["OrderId"] = lijn.Model.Id;
                row["StockId"] = lijn.Stock.Id;
                row["Aantal"] = lijn.Aantal;
                row["AantalWeg"] = lijn.AantalWeg;
                row["Unit"] = lijn.Stock.Unit.Id;
                dataTable.Rows.Add(row);
            }

            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;
            myCommand.SelectCommand.Parameters.AddWithValue("@tblChecklistLijnen", dataTable);
            myConnection.Open();
            myCommand.SelectCommand.ExecuteScalar();
            myConnection.Close();
        }

        public void UpdateChecklistLijn(dynamic model, int stockid, int aantalweg, int unitid)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand;
            SqlParameter pEvenementId;
            if (model is Evenement)
            {
                myCommand = new SqlDataAdapter("uspUpdateEvenementChecklistLijn", myConnection);
                pEvenementId = new SqlParameter("@EvenementId", SqlDbType.Int);
            }
            else
            {
                myCommand = new SqlDataAdapter("uspUpdateOrderChecklistLijn", myConnection);
                pEvenementId = new SqlParameter("@OrderId", SqlDbType.Int);
            }
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            pEvenementId.Value = model.Id;
            myCommand.SelectCommand.Parameters.Add(pEvenementId);

            SqlParameter pStockId = new SqlParameter("@StockId", SqlDbType.Int) {Value = stockid};
            myCommand.SelectCommand.Parameters.Add(pStockId);

            SqlParameter pAantalWeg = new SqlParameter("@AantalWeg", SqlDbType.Int) { Value = aantalweg };
            myCommand.SelectCommand.Parameters.Add(pAantalWeg);

            SqlParameter pUnit = new SqlParameter("@Unit", SqlDbType.Int) { Value = unitid };
            myCommand.SelectCommand.Parameters.Add(pUnit);

            myConnection.Open();
            myCommand.SelectCommand.ExecuteScalar();
            myConnection.Close();
        }

        public void UpdateChecklistLijnAantal(dynamic model, int stockid, int aantal, int unitid)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand;
            SqlParameter pEvenementId;
            if (model is Evenement)
            {
                myCommand = new SqlDataAdapter("uspUpdateEvenementChecklistLijnAantal", myConnection);
                pEvenementId = new SqlParameter("@EvenementId", SqlDbType.Int);
            }
            else
            {
                myCommand = new SqlDataAdapter("uspUpdateOrderChecklistLijnAantal", myConnection);
                pEvenementId = new SqlParameter("@OrderId", SqlDbType.Int);
            }
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            pEvenementId.Value = model.Id;
            myCommand.SelectCommand.Parameters.Add(pEvenementId);

            SqlParameter pStockId = new SqlParameter("@StockId", SqlDbType.Int) { Value = stockid };
            myCommand.SelectCommand.Parameters.Add(pStockId);

            SqlParameter pAantal = new SqlParameter("@Aantal", SqlDbType.Int) { Value = aantal };
            myCommand.SelectCommand.Parameters.Add(pAantal);

            SqlParameter pUnit = new SqlParameter("@Unit", SqlDbType.Int) { Value = unitid };
            myCommand.SelectCommand.Parameters.Add(pUnit);

            myConnection.Open();
            myCommand.SelectCommand.ExecuteScalar();
            myConnection.Close();
        }

        public List<ArtikelLocatie> GetArtikelLocatiesForArtikel(int Id)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetArtikelLocatiesForArtikel", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pArtikelId = new SqlParameter("@ArtikelId", SqlDbType.Int);
            pArtikelId.Value = Id;
            myCommand.SelectCommand.Parameters.Add(pArtikelId);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "ArtikelLocatie");
            List<ArtikelLocatie> list = new List<ArtikelLocatie>();
            for (int i = 0; i < ds.Tables["ArtikelLocatie"].Rows.Count; i++)
            {
                list.Add(new ArtikelLocatie(ds.Tables["ArtikelLocatie"].Rows[i]));
            }
            myConnection.Close();
            return list;
        }

        public List<Stock> GetStocksForArtikel(int Id)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetStocksForArtikel", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pArtikelId = new SqlParameter("@ArtikelId", SqlDbType.Int);
            pArtikelId.Value = Id;
            myCommand.SelectCommand.Parameters.Add(pArtikelId);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Stock");
            List<Stock> list = new List<Stock>();
            for (int i = 0; i < ds.Tables["Stock"].Rows.Count; i++)
            {
                list.Add(new Stock(ds.Tables["Stock"].Rows[i], 0));
            }
            myConnection.Close();
            return list;
        }

        public List<Stock> GetStocksForArtikelByBarcode(string barcode)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetStocksForArtikelByBarcode", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pArtikelId = new SqlParameter("@Barcode", SqlDbType.NVarChar);
            pArtikelId.Value = barcode;
            myCommand.SelectCommand.Parameters.Add(pArtikelId);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Stock");
            List<Stock> list = new List<Stock>();
            for (int i = 0; i < ds.Tables["Stock"].Rows.Count; i++)
            {
                list.Add(new Stock(ds.Tables["Stock"].Rows[i], 0, "type"));
            }
            myConnection.Close();
            return list;
        }
        

        public List<Stock> GetExpiringStocks(DateTime eindtijd)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetStocksWithExpiringDates", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pEindtijd = new SqlParameter("@Eindtijd", SqlDbType.DateTime);
            pEindtijd.Value = eindtijd;
            myCommand.SelectCommand.Parameters.Add(pEindtijd);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Stock");
            List<Stock> list = new List<Stock>();
            for (int i = 0; i < ds.Tables["Stock"].Rows.Count; i++)
            {
                list.Add(new Stock(ds.Tables["Stock"].Rows[i]));
            }
            myConnection.Close();
            return list;
        }

        public void UpdateEvenementChecklistLijn(int stockid, int evenementid, int aantalweg, int unit)
        {

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspUpdateEvenementChecklistLijn", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pStockId = new SqlParameter("@StockId", SqlDbType.Int);
            pStockId.Value = stockid;
            myCommand.Parameters.Add(pStockId);

            SqlParameter pEvenementId = new SqlParameter("@EvenementId", SqlDbType.Int);
            pEvenementId.Value = evenementid;
            myCommand.Parameters.Add(pEvenementId);

            SqlParameter pAantalWeg = new SqlParameter("@AantalWeg", SqlDbType.Int);
            pAantalWeg.Value = aantalweg;
            myCommand.Parameters.Add(pAantalWeg);

            SqlParameter pUnit = new SqlParameter("@Unit", SqlDbType.Int);
            pUnit.Value = unit;
            myCommand.Parameters.Add(pUnit);

            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();


        }

        public EvenementChecklistLijn GetEvenementChecklistLijnByEvenementIdAndStockId(int evenementid, int stockid, int unit)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("getEvenementChecklistLijnByEvenementIdAndStockId",
                myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pEvenementId = new SqlParameter("@EvenementId", SqlDbType.Int);
            pEvenementId.Value = evenementid;
            myCommand.SelectCommand.Parameters.Add(pEvenementId);

            SqlParameter pStockId = new SqlParameter("@StockId", SqlDbType.Int);
            pStockId.Value = stockid;
            myCommand.SelectCommand.Parameters.Add(pStockId);

            SqlParameter pUnit = new SqlParameter("@Unit", SqlDbType.Int);
            pUnit.Value = unit;
            myCommand.SelectCommand.Parameters.Add(pUnit);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "EvenementChecklistLijn");
            EvenementChecklistLijn ecl = null;

            if (ds.Tables["EvenementChecklistLijn"].Rows.Count > 0)
            {
                ecl = new EvenementChecklistLijn(ds.Tables["EvenementChecklistLijn"].Rows[0],0);
            }

            myConnection.Close();
            return ecl;
        }

        public void UpdateEvenementChecklistComplete(int evenementid)
        {

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("UpdateEvenementChecklistComplete", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pEvenementId = new SqlParameter("@EvenementId", SqlDbType.Int);
            pEvenementId.Value = evenementid;
            myCommand.Parameters.Add(pEvenementId);

            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();


        }

        public void UpdateOrderChecklistComplete(int orderid)
        {

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspUpdateOrderChecklistComplete", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pOrderId = new SqlParameter("@OrderId", SqlDbType.Int);
            pOrderId.Value = orderid;
            myCommand.Parameters.Add(pOrderId);

            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();


        }

        public void UpdateEventComplete(int evenementid)
        {

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspUpdateEventComplete", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pEvenementId = new SqlParameter("@EvenementId", SqlDbType.Int);
            pEvenementId.Value = evenementid;
            myCommand.Parameters.Add(pEvenementId);

            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();

            
        }

        public void UpdateOrderComplete(int orderid)
        {

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspUpdateOrderComplete", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pOrderId = new SqlParameter("@OrderId", SqlDbType.Int);
            pOrderId.Value = orderid;
            myCommand.Parameters.Add(pOrderId);

            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();


        }

        public List<Stock> GetAllStocks()
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetAllStocks", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Stock");
            List<Stock> list = new List<Stock>();
            for (int i = 0; i < ds.Tables["Stock"].Rows.Count; i++)
            {
                list.Add(new Stock(ds.Tables["Stock"].Rows[i], "type"));
            }
            myConnection.Close();
            return list;
        }


        public void DeleteEvenementChecklistLijn(int evenementid, int stockid, int unitid)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspDeleteEvenementChecklistLijn", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pEvenementId = new SqlParameter("@EvenementId", SqlDbType.Int);
            pEvenementId.Value = evenementid;
            myCommand.Parameters.Add(pEvenementId);

            SqlParameter pStockId = new SqlParameter("@StockId", SqlDbType.Int);
            pStockId.Value = stockid;
            myCommand.Parameters.Add(pStockId);

            SqlParameter pUnit = new SqlParameter("@Unit", SqlDbType.Int);
            pUnit.Value = unitid;
            myCommand.Parameters.Add(pUnit);

            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();
        }

        public void UpdateTransport(string naamoud, string naamnieuw, int maxgewicht)
        {

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspUpdateTransport", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pNaamOud = new SqlParameter("@NaamOud", SqlDbType.VarChar);
            pNaamOud.Value = naamoud;
            myCommand.Parameters.Add(pNaamOud);

            SqlParameter pNaamNieuw = new SqlParameter("@NaamNieuw", SqlDbType.VarChar);
            pNaamNieuw.Value = naamnieuw;
            myCommand.Parameters.Add(pNaamNieuw);

            SqlParameter pMaxGewicht = new SqlParameter("@MaxGewicht", SqlDbType.Int);
            pMaxGewicht.Value = maxgewicht;
            myCommand.Parameters.Add(pMaxGewicht);

            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();


        }

        public void UpdateArtikelLocatieByCode(string naamoud, string naamnieuw)
        {

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspUpdateArtikelLocatieByCode", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pNaamOud = new SqlParameter("@NaamOud", SqlDbType.VarChar);
            pNaamOud.Value = naamoud;
            myCommand.Parameters.Add(pNaamOud);

            SqlParameter pNaamNieuw = new SqlParameter("@NaamNieuw", SqlDbType.VarChar);
            pNaamNieuw.Value = naamnieuw;
            myCommand.Parameters.Add(pNaamNieuw);

            

            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();


        }

        public void DeleteTransport(string naam)
        {

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspDeleteTransport", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pNaam = new SqlParameter("@Naam", SqlDbType.VarChar);
            pNaam.Value = naam;
            myCommand.Parameters.Add(pNaam);

     

            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();


        }

        public void AddTransport(string naam, int maxgewicht)
        {

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspAddTransport", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pNaam = new SqlParameter("@Naam", SqlDbType.VarChar);
            pNaam.Value = naam;
            myCommand.Parameters.Add(pNaam);

            SqlParameter pMaxGewicht = new SqlParameter("@MaxGewicht", SqlDbType.Int);
            pMaxGewicht.Value = maxgewicht;
            myCommand.Parameters.Add(pMaxGewicht);

            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();


        }

        public List<OrderChecklistLijn> GetOrderChecklistLijnen()
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetOrderChecklistLijn", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            myCommand.Fill(ds, "OrderChecklistLijn");
            List<OrderChecklistLijn> list = new List<OrderChecklistLijn>();
            for (int i = 0; i < ds.Tables["OrderChecklistLijn"].Rows.Count; i++)
            {
                list.Add(new OrderChecklistLijn(ds.Tables["OrderChecklistLijn"].Rows[i],0));
            }
            myConnection.Close();
            return list;
        }
        public List<OrderChecklistLijn> GetOrderChecklistLijnenByOrderId(int orderid)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetOrderChecklistLijnByOrderId", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pOrderId = new SqlParameter("@OrderId", SqlDbType.Int);
            pOrderId.Value = orderid;
            myCommand.SelectCommand.Parameters.Add(pOrderId);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "OrderChecklistLijn");
            List<OrderChecklistLijn> list = new List<OrderChecklistLijn>();
            for (int i = 0; i < ds.Tables["OrderChecklistLijn"].Rows.Count; i++)
            {
                list.Add(new OrderChecklistLijn(ds.Tables["OrderChecklistLijn"].Rows[i]));
            }
            myConnection.Close();
            return list;
        }

        public void UpdateOrderChecklistLijn(int stockid, int orderid, int aantalweg, int unit)
        {

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspUpdateOrderChecklistLijn", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pStockId = new SqlParameter("@StockId", SqlDbType.Int);
            pStockId.Value = stockid;
            myCommand.Parameters.Add(pStockId);

            SqlParameter pOrderId = new SqlParameter("@OrderId", SqlDbType.Int);
            pOrderId.Value = orderid;
            myCommand.Parameters.Add(pOrderId);

            SqlParameter pAantalWeg = new SqlParameter("@AantalWeg", SqlDbType.Int);
            pAantalWeg.Value = aantalweg;
            myCommand.Parameters.Add(pAantalWeg);

            SqlParameter pUnit = new SqlParameter("@Unit", SqlDbType.Int);
            pUnit.Value = unit;
            myCommand.Parameters.Add(pUnit);

            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();


        }

        public OrderChecklistLijn GetOrderChecklistLijnByPrimaryKeys(int orderid, int stockid, int unit)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetOrderChecklistLijnByPrimarykeys",
                myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pOrderId = new SqlParameter("@OrderId", SqlDbType.Int);
            pOrderId.Value = orderid;
            myCommand.SelectCommand.Parameters.Add(pOrderId);

            SqlParameter pStockId = new SqlParameter("@StockId", SqlDbType.Int);
            pStockId.Value = stockid;
            myCommand.SelectCommand.Parameters.Add(pStockId);

            SqlParameter pUnit = new SqlParameter("@Unit", SqlDbType.Int);
            pUnit.Value = unit;
            myCommand.SelectCommand.Parameters.Add(pUnit);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "OrderChecklistLijn");
            OrderChecklistLijn ecl = null;

            if (ds.Tables["OrderChecklistLijn"].Rows.Count > 0)
            {
                ecl = new OrderChecklistLijn(ds.Tables["OrderChecklistLijn"].Rows[0], 0);
            }

            myConnection.Close();
            return ecl;
        }

        //public void DeleteEvenementChecklistLijn(int evenementid, int stockid, int unitid)
        //{
        //    SqlConnection myConnection = new SqlConnection(cs);
        //    SqlCommand myCommand = new SqlCommand("uspDeleteEvenementChecklistLijn", myConnection);
        //    myCommand.CommandType = CommandType.StoredProcedure;

        //    SqlParameter pEvenementId = new SqlParameter("@EvenementId", SqlDbType.Int);
        //    pEvenementId.Value = evenementid;
        //    myCommand.Parameters.Add(pEvenementId);

        //    SqlParameter pStockId = new SqlParameter("@StockId", SqlDbType.Int);
        //    pStockId.Value = stockid;
        //    myCommand.Parameters.Add(pStockId);

        //    SqlParameter pUnit = new SqlParameter("@Unit", SqlDbType.Int);
        //    pUnit.Value = unitid;
        //    myCommand.Parameters.Add(pUnit);

        //    myConnection.Open();
        //    myCommand.ExecuteScalar();
        //    myConnection.Close();
        //}

        public void AddWijziging(int id, DateTime time, string userid, string type)
        {

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspInsertIntoWijzigingen", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pId = new SqlParameter("@Id", SqlDbType.Int);
            pId.Value = id;
            myCommand.Parameters.Add(pId);

            SqlParameter pTime = new SqlParameter("@Time", SqlDbType.DateTime);
            pTime.Value = time;
            myCommand.Parameters.Add(pTime);

            SqlParameter pUserId = new SqlParameter("@UserId", SqlDbType.VarChar, 100);
            pUserId.Value = userid;
            myCommand.Parameters.Add(pUserId);

            SqlParameter pType = new SqlParameter("@Type", SqlDbType.VarChar, 50);
            pType.Value = type;
            myCommand.Parameters.Add(pType);

            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();
        }

        public List<Wijziging> GetAllWijzigingen()
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetAllWijzigingen", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "Wijziging");
            List<Wijziging> list = new List<Wijziging>();

            for (int i = 0; i < ds.Tables["Wijziging"].Rows.Count; i++)
            {
                list.Add(new Wijziging(ds.Tables["Wijziging"].Rows[i]));
            }

            myConnection.Close();
            return list;
        }

        public void InsertChecklistStocks(List<ChecklistStock> lijnen, int id)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("EventId", typeof(int));
            dataTable.Columns.Add("IsEvent", typeof(bool));
            dataTable.Columns.Add("StockId", typeof(int));
            dataTable.Columns.Add("Vervaldatum", typeof(DateTime));
            dataTable.Columns.Add("Aantal", typeof(int));
            dataTable.Columns.Add("AantalWeg", typeof(int));
            dataTable.Columns.Add("ArtikelLocatieId", typeof(int));
            dataTable.Columns.Add("ArtikelId", typeof(int));
            dataTable.Columns.Add("UnitId", typeof(int));
            foreach (var lijn in lijnen)
            {
                var row = dataTable.NewRow();
                row["EventId"] = id;
                row["IsEvent"] = lijn.IsEvent;
                row["StockId"] = lijn.StockId;
                row["Vervaldatum"] = lijn.Vervaldatum.ToShortDateString() != DateTime.MaxValue.ToShortDateString()
                    ? (object) lijn.Vervaldatum
                    : DBNull.Value;
                row["Aantal"] = lijn.Aantal;
                row["AantalWeg"] = lijn.AantalWeg;
                row["ArtikelLocatieId"] = lijn.ArtikelLocatieId;
                row["ArtikelId"] = lijn.ArtikelId;
                row["UnitId"] = lijn.UnitId;
                dataTable.Rows.Add(row);
            }

            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspInsertChecklistStockLijnen", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            myCommand.Parameters.AddWithValue("@tblStockLijnen", dataTable);
            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();
        }

        public void UpdateChecklistStocks(ChecklistStock checkliststock, int id)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspUpdateChecklistStockAantalWeg", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pEventId = new SqlParameter("@EventId", SqlDbType.Int);
            pEventId.Value = id;
            myCommand.Parameters.Add(pEventId);

            SqlParameter pIsEvent = new SqlParameter("@IsEvent", SqlDbType.Bit);
            pIsEvent.Value = checkliststock.IsEvent;
            myCommand.Parameters.Add(pIsEvent);

            SqlParameter pStockId = new SqlParameter("@StockId", SqlDbType.Int);
            pStockId.Value = checkliststock.StockId;
            myCommand.Parameters.Add(pStockId);

            SqlParameter pAantalWeg = new SqlParameter("@AantalWeg", SqlDbType.Int);
            pAantalWeg.Value = checkliststock.AantalWeg;
            myCommand.Parameters.Add(pAantalWeg);

            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();
        }

        public void UpdateChecklistStocksAantal(ChecklistStock checkliststock, int id)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspUpdateChecklistStockAantal", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pEventId = new SqlParameter("@EventId", SqlDbType.Int);
            pEventId.Value = id;
            myCommand.Parameters.Add(pEventId);

            SqlParameter pIsEvent = new SqlParameter("@IsEvent", SqlDbType.Bit);
            pIsEvent.Value = checkliststock.IsEvent;
            myCommand.Parameters.Add(pIsEvent);

            SqlParameter pStockId = new SqlParameter("@StockId", SqlDbType.Int);
            pStockId.Value = checkliststock.StockId;
            myCommand.Parameters.Add(pStockId);

            SqlParameter pAantal = new SqlParameter("@Aantal", SqlDbType.Int);
            pAantal.Value = checkliststock.Aantal;
            myCommand.Parameters.Add(pAantal);

            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();
        }

        public List<ChecklistStock> GetChecklistStocksByEventId(int id)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetChecklistStocksByEventId", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pId = new SqlParameter("@EventId", SqlDbType.Int);
            pId.Value = id;
            myCommand.SelectCommand.Parameters.Add(pId);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "ChecklistStock");
            List<ChecklistStock> list = new List<ChecklistStock>();

            for (int i = 0; i < ds.Tables["ChecklistStock"].Rows.Count; i++)
            {
                list.Add(new ChecklistStock(ds.Tables["ChecklistStock"].Rows[i]));
            }

            myConnection.Close();
            return list;
        }

        public void AddChecklistStock(ChecklistStock checkliststock)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspAddChecklistStock", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pEventId = new SqlParameter("@EventId", SqlDbType.Int);
            pEventId.Value = checkliststock.EventId;
            myCommand.Parameters.Add(pEventId);

            SqlParameter pIsEvent = new SqlParameter("@IsEvent", SqlDbType.Bit);
            pIsEvent.Value = checkliststock.IsEvent;
            myCommand.Parameters.Add(pIsEvent);

            SqlParameter pStockId = new SqlParameter("@StockId", SqlDbType.Int);
            pStockId.Value = checkliststock.StockId;
            myCommand.Parameters.Add(pStockId);

            SqlParameter pVervaldatum = new SqlParameter("@Vervaldatum", SqlDbType.DateTime);
            pVervaldatum.Value = checkliststock.Vervaldatum;
            myCommand.Parameters.Add(pVervaldatum);

            SqlParameter pAantal = new SqlParameter("@Aantal", SqlDbType.Int);
            pAantal.Value = checkliststock.Aantal;
            myCommand.Parameters.Add(pAantal);

            SqlParameter pAantalWeg = new SqlParameter("@AantalWeg", SqlDbType.Int);
            pAantalWeg.Value = checkliststock.AantalWeg;
            myCommand.Parameters.Add(pAantalWeg);

            SqlParameter pArtikelLocatieId = new SqlParameter("@ArtikelLocatieId", SqlDbType.Int);
            pArtikelLocatieId.Value = checkliststock.ArtikelLocatieId;
            myCommand.Parameters.Add(pArtikelLocatieId);

            SqlParameter pArtikelId = new SqlParameter("@ArtikelId", SqlDbType.Int);
            pArtikelId.Value = checkliststock.ArtikelId;
            myCommand.Parameters.Add(pArtikelId);

            SqlParameter pUnitId = new SqlParameter("@UnitId", SqlDbType.Int);
            pUnitId.Value = checkliststock.UnitId;
            myCommand.Parameters.Add(pUnitId);

            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();
        }

        public void AddVermistArtikel(VermistArtikel artikel)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspAddVermistArtikel", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pEventId = new SqlParameter("@EventId", SqlDbType.Int);
            pEventId.Value = artikel.EventId;
            myCommand.Parameters.Add(pEventId);

            SqlParameter pIsEvent = new SqlParameter("@IsEvent", SqlDbType.Bit);
            pIsEvent.Value = artikel.IsEvent;
            myCommand.Parameters.Add(pIsEvent);

            SqlParameter pAantal = new SqlParameter("@Aantal", SqlDbType.Int);
            pAantal.Value = artikel.Aantal;
            myCommand.Parameters.Add(pAantal);

            SqlParameter pArtikelId = new SqlParameter("@ArtikelId", SqlDbType.Int);
            pArtikelId.Value = artikel.ArtikelId;
            myCommand.Parameters.Add(pArtikelId);

            SqlParameter pUserId = new SqlParameter("@UserId", SqlDbType.VarChar, 100);
            pUserId.Value = artikel.UserId;
            myCommand.Parameters.Add(pUserId);

            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();
        }

        public List<VermistArtikel> GetVermisteArtikelen()
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetVermisteArtikelen", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "VermistArtikel");
            List<VermistArtikel> list = new List<VermistArtikel>();

            for (int i = 0; i < ds.Tables["VermistArtikel"].Rows.Count; i++)
            {
                list.Add(new VermistArtikel(ds.Tables["VermistArtikel"].Rows[i]));
            }

            myConnection.Close();
            return list;
        }

        public void DeleteVermistArtikel(int id)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspDeleteVermistArtikel", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pId = new SqlParameter("@Id", SqlDbType.Int);
            pId.Value = id;
            myCommand.Parameters.Add(pId);

            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();
        }

        public void AddArtikelVerantwoordelijke(ArtikelVerantwoordelijke av)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspAddArtikelVerantwoordelijke", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pArtikelId = new SqlParameter("@ArtikelId", SqlDbType.Int);
            pArtikelId.Value = av.Artikelid;
            myCommand.Parameters.Add(pArtikelId);

            SqlParameter pUserId = new SqlParameter("@UserId", SqlDbType.Int);
            pUserId.Value = av.UserId;
            myCommand.Parameters.Add(pUserId);

            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();
        }

        public List<ArtikelVerantwoordelijke> GetArtikelVerantwoordelijkeById(int artikelid)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetArtikelVerantwoordelijkeById", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pId = new SqlParameter("@ArtikelId", SqlDbType.Int);
            pId.Value = artikelid;
            myCommand.SelectCommand.Parameters.Add(pId);

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "ArtikelVerantwoordelijke");
            List<ArtikelVerantwoordelijke> list = new List<ArtikelVerantwoordelijke>();

            for (int i = 0; i < ds.Tables["ArtikelVerantwoordelijke"].Rows.Count; i++)
            {
                list.Add(new ArtikelVerantwoordelijke(ds.Tables["ArtikelVerantwoordelijke"].Rows[i]));
            }

            myConnection.Close();
            return list;
        }

        public List<ArtikelVerantwoordelijke> GetArtikelVerantwoordelijken()
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlDataAdapter myCommand = new SqlDataAdapter("uspGetArtikelVerantwoordelijken", myConnection);
            myCommand.SelectCommand.CommandType = CommandType.StoredProcedure;

            DataSet ds = new DataSet();
            myCommand.Fill(ds, "ArtikelVerantwoordelijke");
            List<ArtikelVerantwoordelijke> list = new List<ArtikelVerantwoordelijke>();

            for (int i = 0; i < ds.Tables["ArtikelVerantwoordelijke"].Rows.Count; i++)
            {
                list.Add(new ArtikelVerantwoordelijke(ds.Tables["ArtikelVerantwoordelijke"].Rows[i]));
            }

            myConnection.Close();
            return list;
        }
        

        public void DeleteArtikelVerantwoordelijke(ArtikelVerantwoordelijke av)
        {
            SqlConnection myConnection = new SqlConnection(cs);
            SqlCommand myCommand = new SqlCommand("uspDeleteArtikelVerantwoordelijke", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter pId = new SqlParameter("@ArtikelId", SqlDbType.Int);
            pId.Value = av.Artikelid;
            myCommand.Parameters.Add(pId);

            SqlParameter pUserId = new SqlParameter("@UserId", SqlDbType.Int);
            pUserId.Value = av.UserId;
            myCommand.Parameters.Add(pUserId);

            myConnection.Open();
            myCommand.ExecuteScalar();
            myConnection.Close();
        }
    }
}