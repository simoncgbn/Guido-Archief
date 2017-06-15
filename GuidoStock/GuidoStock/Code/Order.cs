using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DocumentFormat.OpenXml.Spreadsheet;
using GuidoStock.App_Code;

namespace GuidoStock.Code
{
    [Serializable]
    public class Order
    {
        private int _Id;
        private string _Naam;
        private DateTime _BeginTijd;
        private DateTime _EindTijd;
        private string _Tel;
        private List<OrderLijn> _OrderLijnen;
        private DateTime _ChecklistComplete;
        private bool _IsVerhuur;
        private string _ContactNaam;
        private DateTime _OrderComplete;
        private string _CheckList;

        public Order()
        {
            _Id = -1;
            _OrderLijnen = new List<OrderLijn>();
        }

        public Order(DataRow row)
        {
            _Id = Convert.ToInt32(row["Id"]);
            _Naam = row["Naam"].ToString();
            _BeginTijd = Convert.ToDateTime(row["BeginTijd"]);
            if (!(row["EindTijd"] is DBNull)) { 
                _EindTijd = Convert.ToDateTime(row["EindTijd"]);
            }

        _Tel = row["Tel"].ToString();
            _OrderLijnen = new List<OrderLijn>();
            _IsVerhuur = Convert.ToBoolean(row["IsVerhuur"]);
            if (!(row["ChecklistComplete"] is DBNull))
                _ChecklistComplete = Convert.ToDateTime(row["ChecklistComplete"]);
            _ContactNaam = row["ContactNaam"].ToString();
            if (!(row["OrderComplete"] is DBNull))
            {
                _OrderComplete = Convert.ToDateTime(row["OrderComplete"]);
            }
            if (!(row["CheckList"] is DBNull))
            {
                _CheckList = row["CheckList"].ToString();
            }
                
        }

        public Order(DataRow row, int nul)
        {
            _Id = Convert.ToInt32(row["Id"]);
            _Naam = row["OrderNaam"].ToString();
            if (!(row["ChecklistComplete"] is DBNull))
                _ChecklistComplete = Convert.ToDateTime(row["ChecklistComplete"]);
        }

        public Order(DataRow row, int nul, int een)
        {
            _Id = Convert.ToInt32(row["Id"]);
        }

        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public string Naam
        {
            get { return _Naam; }
            set { _Naam = value; }
        }

        public DateTime BeginTijd
        {
            get { return _BeginTijd; }
            set { _BeginTijd = value; }
        }

        public DateTime EindTijd
        {
            get { return _EindTijd; }
            set { _EindTijd = value; }
        }

        public string Tel
        {
            get { return _Tel; }
            set { _Tel = value; }
        }

        public List<OrderLijn> OrderLijnen
        {
            get { return _OrderLijnen; }
            set { _OrderLijnen = value; }
        }

        public DateTime ChecklistComplete
        {
            get { return _ChecklistComplete; }
            set { _ChecklistComplete = value; }
        }

        public bool IsVerhuur
        {
            get { return _IsVerhuur; }
            set { _IsVerhuur = value; }
        }

        public string ContactNaam
        {
            get { return _ContactNaam; }
            set { _ContactNaam = value; }
        }

        public DateTime OrderComplete
        {
            get { return _OrderComplete; }
            set { _OrderComplete = value; }
        }

        public string CheckList
        {
            get { return _CheckList; }
            set { _CheckList = value; }
        }

        public DateTime Datum => _BeginTijd;

        public string Subtext => _ContactNaam;
    }
}