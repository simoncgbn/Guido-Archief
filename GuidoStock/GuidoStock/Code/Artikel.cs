using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using GuidoStock.Code;

namespace GuidoStock.App_Code {
    [Serializable]
    public class Artikel {
        #region{Fields}
        private int _Id;
        private string _Naam;
        private string _Omschrijving;
        private string _Barcode;
        private float _Gewicht;
        private float _NaturaPrijs;
        private float _VerhuurPrijs;
        private bool _IsHerbruikbaar;
        private bool _IsVerwijderd;
        private int _AvailableAantal;
        private Categorie _Categorie;
        private Merk _Merk;
        private List<Stock> _Stocks;
        private List<ArtikelMetaTag> _ArtikelMetaTags;
        private List<Unit> _Units;
        private int _AantalStopcontacten;
        #endregion

        #region{Constructors}
        public Artikel() {
            _Stocks = new List<Stock>();
            _Units = new List<Unit>();
            _ArtikelMetaTags = new List<ArtikelMetaTag>();
            AvailableAantal = -1;


        }

        public Artikel(DataRow row)
        {
            _Id = Convert.ToInt32(row["Id"]);
            _Naam = row["Naam"].ToString();
            _Omschrijving = row["Omschrijving"].ToString();
            _Barcode = row["Barcode"].ToString();
            if (!(row["Gewicht"] is DBNull))
            {
                _Gewicht = Convert.ToInt32(row["Gewicht"]);
            }
            else
            {
                _Gewicht = -1;
            }
            if (!(row["NaturaPrijs"] is DBNull))
            {
                _NaturaPrijs = Convert.ToInt32(row["NaturaPrijs"]);
            }
            else
            {
                _NaturaPrijs = -1;
            }
            if (!(row["VerhuurPrijs"] is DBNull))
            {
                _VerhuurPrijs = Convert.ToInt32(row["VerhuurPrijs"]);
            }
            else
            {
                _VerhuurPrijs = -1;
            }
            _IsHerbruikbaar = Convert.ToBoolean(row["IsHerbruikbaar"]);
            _IsVerwijderd = Convert.ToBoolean(row["isVerwijderd"]);
            _Categorie = new Categorie(Convert.ToInt32(row["CategorieId"]), row["CategorieNaam"].ToString(), row["CategorieOmschrijving"].ToString());
            _Merk = new Merk(Convert.ToInt32(row["MerkId"]), row["MerkNaam"].ToString());
            _Stocks = new List<Stock>();
            AvailableAantal = -1;
            if (!(row["AantalStopcontacten"] is DBNull))
            {
                _AantalStopcontacten = Convert.ToInt32(row["AantalStopcontacten"]);
            }


        }

        public Artikel(DataRow row, int nul, string een)
        {
            _Id = Convert.ToInt32(row["Id"]);
            _Naam = row["Naam"].ToString();
            _Omschrijving = row["Omschrijving"].ToString();
            _Barcode = row["Barcode"].ToString();
            if (!(row["Gewicht"] is DBNull))
            {
                _Gewicht = Convert.ToInt32(row["Gewicht"]);
            }
            else
            {
                _Gewicht = -1;
            }
            if (!(row["NaturaPrijs"] is DBNull))
            {
                _NaturaPrijs = Convert.ToInt32(row["NaturaPrijs"]);
            }
            else
            {
                _NaturaPrijs = -1;
            }
            if (!(row["VerhuurPrijs"] is DBNull))
            {
                _VerhuurPrijs = Convert.ToInt32(row["VerhuurPrijs"]);
            }
            else
            {
                _VerhuurPrijs = -1;
            }
            _IsHerbruikbaar = Convert.ToBoolean(row["IsHerbruikbaar"]);
            _IsVerwijderd = Convert.ToBoolean(row["isVerwijderd"]);
            _Categorie = new Categorie(Convert.ToInt32(row["CategorieId"]), row["CategorieNaam"].ToString(), row["CategorieOmschrijving"].ToString());
            _Merk = new Merk(Convert.ToInt32(row["MerkId"]), row["MerkNaam"].ToString());
            _Stocks = new List<Stock>();
            AvailableAantal = -1;
            if (!(row["AantalStopcontacten"] is DBNull))
            {
                _AantalStopcontacten = Convert.ToInt32(row["AantalStopcontacten"]);
            }
        }

        public Artikel(DataRow row, int nul)
        {
            _Id = Convert.ToInt32(row["Id"]);
            _Naam = row["ArtikelNaam"].ToString();
            _Barcode = row["ArtikelBarcode"].ToString();
            _IsHerbruikbaar = Convert.ToBoolean(row["IsHerbruikbaar"]);
            _Categorie = new Categorie(Convert.ToInt32(row["CategorieId"]), row["CategorieNaam"].ToString(), row["CategorieOmschrijving"].ToString());
            _Merk = new Merk(Convert.ToInt32(row["MerkId"]), row["MerkNaam"].ToString());
            _Stocks = new List<Stock>();
            AvailableAantal = -1;
            if (!(row["AantalStopcontacten"] is DBNull))
            {
                _AantalStopcontacten = Convert.ToInt32(row["AantalStopcontacten"]);
            }
        }

        public Artikel(DataRow row, string type)
        {
            _Id = Convert.ToInt32(row["Id"]);
        }

        public Artikel(DataRow row, List<DataRow> stocks)
        {
            _Id = Convert.ToInt32(row["Id"]);
            _Naam = row["Naam"].ToString();
            _Omschrijving = row["Omschrijving"].ToString();
            _Barcode = row["Barcode"].ToString();
            if (!(row["Gewicht"] is DBNull)) {
                _Gewicht = Convert.ToSingle(row["Gewicht"]);
            }
            else
            {
                _Gewicht = -1;
            }
            if (!(row["NaturaPrijs"] is DBNull)) {
                _NaturaPrijs = Convert.ToSingle(row["NaturaPrijs"]);
            }
            else
            {
                _NaturaPrijs = -1;
            }
            if (!(row["VerhuurPrijs"] is DBNull)) {
                _VerhuurPrijs = Convert.ToSingle(row["VerhuurPrijs"]);
            }
            else
            {
                _VerhuurPrijs = -1;
            }
            _IsHerbruikbaar = Convert.ToBoolean(row["IsHerbruikbaar"]);
            _IsVerwijderd = Convert.ToBoolean(row["isVerwijderd"]);
            int id = Convert.ToInt32(row["CategorieId"]);
            string naam = row["CategorieNaam"].ToString();
            string omsch = row["CategorieOmschrijving"].ToString();
            _Categorie = new Categorie(id, naam, omsch);
            _Merk = new Merk(Convert.ToInt32(row["MerkId"]), row["MerkNaam"].ToString());
            _Stocks = new List<Stock>();
            // Add Stocks
            for (int i = 0; i < stocks.Count; i++)
            {
                _Stocks.Add(new Stock(stocks[i], this));
            }
            AvailableAantal = -1;
            if (!(row["AantalStopcontacten"] is DBNull))
            {
                _AantalStopcontacten = Convert.ToInt32(row["AantalStopcontacten"]);
            }
        }

        public Artikel(int artikelid, string artikelnaam, int merkid, string merknaam, string barcode, bool isherbruikbaar)
        {
            _Id = artikelid;
            _Naam = artikelnaam;
            _Merk = new Merk(merkid,merknaam);
            _Barcode = barcode;
            _IsHerbruikbaar = isherbruikbaar;
            AvailableAantal = -1;
        }

        //public Artikel(Artikel artikel)
        //{
        //    _Id = artikel.Id;
        //    _ArtikelMetaTags = artikel.ArtikelMetaTags;
        //    _Barcode = artikel.Barcode;
        //    _
        //}

        #endregion

        #region{Properties}
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

        public string Omschrijving
        {
            get { return _Omschrijving; }
            set { _Omschrijving = value; }
        }

        public string Barcode
        {
            get { return _Barcode; }
            set { _Barcode = value; }
        }

        public float Gewicht
        {
            get { return _Gewicht; }
            set { _Gewicht = value; }
        }

        public float Naturaprijs
        {
            get { return _NaturaPrijs; }
            set { _NaturaPrijs = value; }
        }

        public float Verhuurprijs
        {
            get { return _VerhuurPrijs; }
            set { _VerhuurPrijs = value; }
        }

        public bool IsHerbruikbaar
        {
            get { return _IsHerbruikbaar; }
            set { _IsHerbruikbaar = value; }
        }

        public Categorie Categorie
        {
            get { return _Categorie; }
            set { _Categorie = value; }
        }

        public Merk Merk
        {
            get { return _Merk; }
            set { _Merk = value; }
        }

        public int Aantal
        {
            get
            {
                return _Stocks.Sum(t => t.Aantal);
            }
        }

        public List<ArtikelMetaTag> ArtikelMetaTags
        {
            get { return _ArtikelMetaTags; }
            set { _ArtikelMetaTags = value; }
        }

        public int AantalStopcontacten
        {
            get { return _AantalStopcontacten; }
            set { _AantalStopcontacten = value; }
        }

        private DateTime Vervaldatum
        {
            get
            {
                var date = DateTime.MaxValue;
                foreach (var t in _Stocks.Where(t => t.Vervaldatum < date))
                {
                    date = t.Vervaldatum;
                }
                return date;
            }
        }

        public string VervalDatumString => Vervaldatum.Equals(DateTime.MaxValue) ? "" : Vervaldatum.ToString("dd/MM/yyyy");

        /*public List<Stock> Stocks
        {
            get { return _Stocks; }
            set { _Stocks = value; }
        }*/

        public string ArtikelLocatie
        {
            get
            {
                var codes = new HashSet<string>();
                foreach (var t in _Stocks)
                {
                    codes.Add(t.ArtikelLocatie.Code);
                }
                return codes.Count == 1 ? codes.First() : (codes.Count == 0 ? "Geen" : "Meerdere");
            }
        }

        public void ConvertArtikelMetaTags(List<DataRow> metatags)
        {
            ArtikelMetaTags = new List<ArtikelMetaTag>();
            foreach (DataRow metatag in metatags)
            {
                ArtikelMetaTags.Add(new ArtikelMetaTag(metatag));
            }
        }

        public void ConvertUnits(List<DataRow> units)
        {
            Units = new List<Unit>();
            foreach (DataRow unit in units)
            {
                Units.Add(new Unit(unit));
            }
        }

        public List<Stock> Stocks
        {
            get { return _Stocks; }
            set { _Stocks = value; }
        }

        public List<Unit> Units
        {
            get { return _Units; }
            set { _Units = value; }
        }

        public bool IsVerwijderd
        {
            get { return _IsVerwijderd; }
            set { _IsVerwijderd = value; }
        }

        public int AvailableAantal
        {
            get {
                return _AvailableAantal == -1 ? Aantal : _AvailableAantal;
            }

            set { _AvailableAantal = value; }
        }

        public string DisplayNaam { get; set; }

        #endregion
    }
}