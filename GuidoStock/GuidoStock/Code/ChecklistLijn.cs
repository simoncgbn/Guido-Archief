using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web;
using GuidoStock.App_Code;

namespace GuidoStock.Code
{
    [Serializable]
    public class ChecklistLijn
    {
        private dynamic _Model;
        private App_Code.Stock _Stock;
        private int _Aantal;
        private int _AantalWeg;
        private DateTime _ChecklistComplete;
        private int _Id;
        private string _LocatieVervaldatum;

        public ChecklistLijn()
        {
            
        }

        public ChecklistLijn(App_Code.Stock stock, dynamic model, int aantal)
        {
            _Stock = stock;
            _Model = model;
            _Aantal = aantal;
            _AantalWeg = 0;
            _ChecklistComplete = DateTime.MaxValue;
        }

        public ChecklistLijn(DataRow row, dynamic model)
        {
            _Model = model;
            Artikel artikel = new Artikel {Id = Convert.ToInt32(row["ArtikelId"])};
            _Stock = new App_Code.Stock(row, artikel, 0,"n");
            if (row.Table.Columns.Contains("StockId"))
            {
                _Stock.Id = Convert.ToInt32(row["StockId"]);
            }
            _Aantal = Convert.ToInt32(row["Aantal"]);
            _AantalWeg = Convert.ToInt32(row["AantalWeg"]);
            if (!string.IsNullOrEmpty(row["ChecklistComplete"].ToString()))
            {
                _ChecklistComplete = Convert.ToDateTime(row["ChecklistComplete"]);
            }
        }

        public dynamic Model
        {
            get { return _Model; }
            set { _Model = value; }
        }

        public App_Code.Stock Stock
        {
            get { return _Stock; }
            set { _Stock = value; }
        }

        public int Aantal
        {
            get { return _Aantal; }
            set { _Aantal = value; }
        }

        public int AantalWeg
        {
            get { return _AantalWeg; }
            set { _AantalWeg = value; }
        }

        public DateTime ChecklistComplete
        {
            get { return _ChecklistComplete; }
            set { _ChecklistComplete = value; }
        }

        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public string LocatieVervaldatum => _Stock.LocatieVervaldatum;
        public int DisplayAantal => _Aantal/_Stock.Unit.Aantal - _AantalWeg/_Stock.Unit.Aantal;
        public int DisplayAantal2 => _AantalWeg / _Stock.Unit.Aantal;
    }
}