using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GuidoStock.App_Code {
    [Serializable]
    public class ArtikelLocatie
    {
        #region{Fields}
        private int _Id;
        private string _Naam;
        private string _Code;
        private string _Barcode;
        private string _LocatieVervaldatum;
        #endregion

        #region{Constructors}
        public ArtikelLocatie()
        {

        }

        public ArtikelLocatie(DataRow row)
        {
            _Id = Convert.ToInt32(row["Id"]);
            _Naam = row["Naam"].ToString();
            _Code = row["Code"].ToString();
            _Barcode = row["Barcode"].ToString();
        }

        public ArtikelLocatie(string code)
        {
            _Code = code;
        }

        public ArtikelLocatie(int id, string naam, string code, string barcode)
        {
            _Id = id;
            _Naam = naam;
            _Code = code;
            _Barcode = barcode;
        }

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

        public string Code
        {
            get { return _Code; }
            set { _Code = value; }
        }

        public string Barcode
        {
            get { return _Barcode; }
            set { _Barcode = value; }
        }

        public int AantalStocks { get; set; }
        public int AantalArtikelen { get; set; }

        public string BarcodeImage
        {
            get { return "http://www.guido.be/barcode.aspx?code=" + _Code; }
        }

        public string LocatieVervalDatum
        {
            get { return string.IsNullOrEmpty(_LocatieVervaldatum) ? _Code : _LocatieVervaldatum; }
            set { _LocatieVervaldatum = value; }
        }

        #endregion
    }
}