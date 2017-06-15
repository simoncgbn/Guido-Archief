using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using GuidoStock.Code;

namespace GuidoStock.App_Code {
    [Serializable]
    public class Stock
    {
        #region{Fields}
        private int _Id;
        private DateTime _Vervaldatum = DateTime.MaxValue;
        private int _Aantal;
        private Artikel _Artikel;
        private ArtikelLocatie _ArtikelLocatie;
        private string _LocatieVervaldatum;
        private int _ArtikelLocatieId;
        private int _AantalWeg;
        private string _ArtikelLocatieCode;
        private int _ArtikelId;
        private int _UnitId;
        private int _UnitAantal;
        private Unit _Unit;
        private int _TempAantal = -1;

        #endregion

        #region{Constructors}
        public Stock()
        {

        }

        public Stock(DataRow row, Artikel Artikel)
        {
            _Id = Convert.ToInt32(row["Id"]);
            if (!(row["VervalDatum"] is DBNull))
            {
                _Vervaldatum = Convert.ToDateTime(row["Vervaldatum"]);
            }
            _Aantal = Convert.ToInt32(row["Aantal"]);
            _Artikel = Artikel;
            _ArtikelLocatie = new ArtikelLocatie(Convert.ToInt32(row["ArtikelLocatieId"]), row["ArtikelLocatieNaam"].ToString(), row["ArtikelLocatieCode"].ToString(), row["ArtikelLocatieBarcode"].ToString());
        }

        public Stock(DataRow row, Artikel artikel, int nul)
        {
            _Id = Convert.ToInt32(row["Id"]);
            if (!(row["VervalDatum"] is DBNull))
            {
                _Vervaldatum = Convert.ToDateTime(row["Vervaldatum"]);
            }
            _Aantal = Convert.ToInt32(row["Aantal"]);
            _Artikel = new Artikel(Convert.ToInt32(row["ArtikelId"]), row["ArtikelNaam"].ToString(), Convert.ToInt32(row["MerkId"]), row["MerkNaam"].ToString(),row["ArtikelBarcode"].ToString(),Convert.ToBoolean(row["isHerbruikbaar"]));
            _ArtikelLocatie = new ArtikelLocatie(Convert.ToInt32(row["ArtikelLocatieId"]), row["ArtikelLocatieNaam"].ToString(), row["ArtikelLocatieCode"].ToString(), row["ArtikelLocatieBarcode"].ToString());
            _AantalWeg = Convert.ToInt32(row["AantalWeg"]);
        }

        public Stock(DataRow row, Artikel artikel, int nul, string n)
        {
            _Id = Convert.ToInt32(row["Id"]);
            if (!(row["VervalDatum"] is DBNull))
            {
                _Vervaldatum = Convert.ToDateTime(row["Vervaldatum"]);
            }
            _Aantal = Convert.ToInt32(row["Aantal"]);
            _Artikel = new Artikel(Convert.ToInt32(row["ArtikelId"]), row["ArtikelNaam"].ToString(), Convert.ToInt32(row["MerkId"]), row["MerkNaam"].ToString(), row["ArtikelBarcode"].ToString(), Convert.ToBoolean(row["isHerbruikbaar"]));
            _ArtikelLocatie = new ArtikelLocatie(Convert.ToInt32(row["ArtikelLocatieId"]), row["ArtikelLocatieNaam"].ToString(), row["ArtikelLocatieCode"].ToString(), row["ArtikelLocatieBarcode"].ToString());
            _AantalWeg = Convert.ToInt32(row["AantalWeg"]);
            _Unit = new Unit(row,0,1);
        }

        public Stock(DataRow row)
        {
            _Id = Convert.ToInt32(row["Id"]);
            if (!(row["VervalDatum"] is DBNull))
            {
                _Vervaldatum = Convert.ToDateTime(row["Vervaldatum"]);
            }
            _Aantal = Convert.ToInt32(row["Aantal"]);
            _Artikel = new Artikel() {Id = Convert.ToInt32(row["ArtikelId"])};
            _ArtikelLocatie = new ArtikelLocatie(Convert.ToInt32(row["ArtikelLocatieId"]), row["ArtikelLocatieNaam"].ToString(), row["ArtikelLocatieCode"].ToString(), row["ArtikelLocatieBarcode"].ToString());
        }

        public Stock(DataRow row, string type)
        {
            _Id = Convert.ToInt32(row["Id"]);
            if (!(row["VervalDatum"] is DBNull))
            {
                _Vervaldatum = Convert.ToDateTime(row["Vervaldatum"]);
            }
            _Aantal = Convert.ToInt32(row["Aantal"]);
            _Artikel = new Artikel() { Id = Convert.ToInt32(row["ArtikelId"]), IsHerbruikbaar = Convert.ToBoolean(row["IsHerbruikbaar"]),  Barcode = row["ArtikelBarcode"].ToString(), Naam = row["Naam"].ToString(), Merk = new Merk() {Naam = row["MerkNaam"].ToString()} };
            _ArtikelLocatie = new ArtikelLocatie(Convert.ToInt32(row["ArtikelLocatieId"]), row["ArtikelLocatieNaam"].ToString(), row["ArtikelLocatieCode"].ToString(), row["ArtikelLocatieBarcode"].ToString());
            _LocatieVervaldatum = row["ArtikelLocatieCode"].ToString();
            if (!(row["UnitId"] is DBNull))
            {
                _UnitId = Convert.ToInt32(row["UnitId"]);
            }
            if (!(row["UnitAantal"] is DBNull))
            {
                _UnitAantal = Convert.ToInt32(row["UnitAantal"]);
            }
            Unit unit = new Unit()
            {
                Id = _UnitId,
                Aantal = _UnitAantal,
                Barcode = row["UnitBarcode"].ToString(),
                NaamEnkelvoud = row["UnitNaamEnkelvoud"].ToString(),
                ChildUnitId = Convert.ToInt32(row["UnitChild"])
            };
            _Unit = unit;
        }

        public Stock(DataRow row, int nul)
        {
            _Id = Convert.ToInt32(row["StockId"]);
            if (!(row["VervalDatum"] is DBNull))
            {
                _Vervaldatum = Convert.ToDateTime(row["Vervaldatum"]);
                _LocatieVervaldatum = row["ArtikelLocatieCode"] + " - " + Convert.ToDateTime(row["Vervaldatum"]).ToString("dd/MM/yyyy");
            }
            else
            {
                _LocatieVervaldatum = row["ArtikelLocatieCode"].ToString();
            }
            _Aantal = Convert.ToInt32(row["Aantal"]);
            _ArtikelLocatie = new ArtikelLocatie(Convert.ToInt32(row["ArtikelLocatieId"]), row["ArtikelLocatieNaam"].ToString(), row["ArtikelLocatieCode"].ToString(), row["ArtikelLocatieBarcode"].ToString());
            _ArtikelLocatieId = Convert.ToInt32(row["ArtikelLocatieId"]);
            _ArtikelLocatieCode = row["ArtikelLocatieCode"].ToString();
        }

        public Stock(DataRow row, int nul, string type)
        {
            _Id = Convert.ToInt32(row["StockId"]);
            if (!(row["VervalDatum"] is DBNull))
            {
                _Vervaldatum = Convert.ToDateTime(row["Vervaldatum"]);
                _LocatieVervaldatum = row["ArtikelLocatieCode"] + " - " + Convert.ToDateTime(row["Vervaldatum"]).ToString("dd/MM/yyyy");
            }
            else
            {
                _LocatieVervaldatum = row["ArtikelLocatieCode"].ToString();
            }
            _Aantal = Convert.ToInt32(row["Aantal"]);
            _ArtikelLocatie = new ArtikelLocatie(Convert.ToInt32(row["ArtikelLocatieId"]), row["ArtikelLocatieNaam"].ToString(), row["ArtikelLocatieCode"].ToString(), row["ArtikelLocatieBarcode"].ToString());
            _ArtikelLocatieId = Convert.ToInt32(row["ArtikelLocatieId"]);
            _ArtikelLocatieCode = row["ArtikelLocatieCode"].ToString();
            _ArtikelId = Convert.ToInt32(row["ArtikelId"]);
        }

        public Stock(Stock stock)
        {
            _Id = stock.Id;
            _Aantal = stock.Aantal;
            _Artikel = stock.Artikel;
            _ArtikelLocatie = stock.ArtikelLocatie;
            _ArtikelLocatieId = stock.ArtikelLocatieId;
            _ArtikelLocatieCode = stock.ArtikelLocatieCode;
            _LocatieVervaldatum = stock.LocatieVervaldatum;
            _Vervaldatum = stock.Vervaldatum;
            DisplayNaam = stock.Artikel.Naam;
        }


        #endregion

        #region{Properties}

        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
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

        public Artikel Artikel
        {
            get { return _Artikel; }
            set { _Artikel = value; }
        }

        public ArtikelLocatie ArtikelLocatie
        {
            get { return _ArtikelLocatie; }
            set { _ArtikelLocatie = value; }
        }

        public string LocatieVervaldatum
        {
            get { return _LocatieVervaldatum; }
            set { _LocatieVervaldatum = value; }
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

        public int AantalWeg
        {
            get { return _AantalWeg; }
            set { _AantalWeg = value; }
        }

        public string ArtikelLocatieCode
        {
            get { return _ArtikelLocatieCode; }
            set { _ArtikelLocatieCode = value; }
        }

        public string DisplayNaam { get; set; }

        public string IsVervaldatum => _Vervaldatum != DateTime.MaxValue ? _Vervaldatum.ToString("dd MMMM yyyy") : "";

        public int UnitId => _UnitId;

        public int UnitAantal => _UnitAantal;

        public string VervalDatumString => Vervaldatum.Equals(DateTime.MaxValue) ? "" : Vervaldatum.ToString("dd/MM/yyyy");

        public Unit Unit
        {
            get { return _Unit; }
            set { _Unit = value; }
        }

        public string DisplayNaam2
        {
            get
            {
                if (Artikel.Naam == Unit.NaamEnkelvoud)
                    return Artikel.Naam;
                if(Unit.Aantal != 1)
                    return Unit.NaamEnkelvoud + " " + Artikel.Naam;
                return Artikel.Naam;
            }
        }

        public int TempAantal
        {
            get{ return _TempAantal != -1 ? _TempAantal : Aantal; }
            set { _TempAantal = value; }
        }

        public string FilterString => _Id + "-" + _Unit.Id;

        public int UnitID2 => _Unit.Id;

        #endregion


    }
}