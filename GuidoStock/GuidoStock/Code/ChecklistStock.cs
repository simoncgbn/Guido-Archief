using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GuidoStock.Code
{
    [Serializable]
    public class ChecklistStock
    {
        private int _EventId;
        private bool _IsEvent;
        private int _StockId;
        private DateTime _Vervaldatum;
        private int _Aantal;
        private int _AantalWeg;
        private int _ArtikelLocatieId;
        private int _ArtikelId;
        private int _UnitId;

        public ChecklistStock(){}

        public ChecklistStock(int eventid, bool isevent, int stockid, DateTime vervaldatum, int aantal, int aantalweg, int artikellocatieid, int artikelid, int unitid)
        {
            _EventId = eventid;
            _IsEvent = isevent;
            _StockId = stockid;
            _Vervaldatum = vervaldatum;
            _Aantal = aantal;
            _AantalWeg = aantalweg;
            _ArtikelLocatieId = artikellocatieid;
            _ArtikelId = artikelid;
            _UnitId = unitid;
        }

        public ChecklistStock(Checklist checklist, ChecklistLijn lijn)
        {
            _EventId = checklist.Model.Id;
            _IsEvent = checklist.Model is Evenement;
            _StockId = lijn.Stock.Id;
            _Vervaldatum = lijn.Stock.Vervaldatum;
            _Aantal = 0;
            _AantalWeg = lijn.AantalWeg != 0 ? lijn.AantalWeg : 0;
            _ArtikelLocatieId = lijn.Stock.ArtikelLocatie.Id;
            _ArtikelId = lijn.Stock.Artikel.Id;
            _UnitId = lijn.Stock.UnitId;
        }

        public ChecklistStock(Checklist checklist, ChecklistLijn lijn, int nul)
        {
            _EventId = checklist.Model.Id;
            _IsEvent = checklist.Model is Evenement;
            _StockId = 0;
            _Vervaldatum = lijn.Stock.Vervaldatum;
            _Aantal = lijn.Aantal;
            _AantalWeg = 0;
            _ArtikelLocatieId = lijn.Stock.ArtikelLocatie.Id;
            _ArtikelId = lijn.Stock.Artikel.Id;
            _UnitId = lijn.Stock.Unit.Id;
        }

        public ChecklistStock(DataRow row)
        {
            _EventId = Convert.ToInt32(row["EventId"]);
            _IsEvent = Convert.ToBoolean(row["IsEvent"]);
            _StockId = Convert.ToInt32(row["StockId"]);
            if (!(row["Vervaldatum"] is DBNull))
            {
                _Vervaldatum = Convert.ToDateTime(row["Vervaldatum"]);
            }
            _Aantal = Convert.ToInt32(row["Aantal"]);
            _AantalWeg = Convert.ToInt32(row["AantalWeg"]);
            _ArtikelLocatieId = Convert.ToInt32(row["ArtikelLocatieId"]);
            _ArtikelId = Convert.ToInt32(row["ArtikelId"]);
            _UnitId = Convert.ToInt32(row["UnitId"]);
        }

        public int EventId
        {
            get { return _EventId; }
            set { _EventId = value; }
        }

        public bool IsEvent
        {
            get { return _IsEvent; }
            set { _IsEvent = value; }
        }

        public int StockId
        {
            get { return _StockId; }
            set { _StockId = value; }
        }

        public DateTime Vervaldatum
        {
            get { return _Vervaldatum; }
            set { _Vervaldatum = value; }
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

        public int ArtikelLocatieId
        {
            get { return _ArtikelLocatieId; }
            set { _ArtikelLocatieId = value; }
        }

        public int ArtikelId
        {
            get { return _ArtikelId; }
            set { _ArtikelId = value; }
        }

        public int UnitId
        {
            get { return _UnitId; }
            set { _UnitId = value; }
        }
    }
}