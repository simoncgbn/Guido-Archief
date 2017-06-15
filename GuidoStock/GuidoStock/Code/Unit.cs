using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GuidoStock.Code {
    [Serializable]
    public class Unit
    {
        private int _Id;
        private string _NaamEnkelvoud;
        private string _NaamMeervoud;
        private int _Aantal;
        private float _Prijs;
        private string _Barcode;
        private int _ArtikelId;
        private string _ChildUnitString;
        private int _ChildUnitId;

        public Unit()
        {

        }

        public Unit(string naam, int aantal, string barcode, int artikelid, string childunit)
        {
            _NaamEnkelvoud = naam;
            _NaamMeervoud = naam;
            _Aantal = aantal;
            _Barcode = barcode;
            _ArtikelId = artikelid;
            _ChildUnitString = childunit;
        }

        public Unit(string naam, int aantal, string barcode, int artikelid, string childunit, int childUnitId)
        {
            _NaamEnkelvoud = naam;
            _NaamMeervoud = naam;
            _Aantal = aantal;
            _Barcode = barcode;
            _ArtikelId = artikelid;
            _ChildUnitString = childunit;
            _ChildUnitId = childUnitId;
        }

        public Unit(DataRow row)
        {
            _Id = Convert.ToInt32(row["Id"]);
            _NaamEnkelvoud = row["NaamEnkelvoud"].ToString();
            _NaamMeervoud = row["NaamMeervoud"].ToString();
            _Aantal = Convert.ToInt32(row["Aantal"]);
            _Prijs = Convert.ToSingle(row["Prijs"]);
            _Barcode = row["Barcode"].ToString();
            _ChildUnitId = Convert.ToInt32(row["ChildUnit"]);
        }

        public Unit(DataRow row, int nul)
        {
            _Id = Convert.ToInt32(row["Id"]);
            _NaamEnkelvoud = row["NaamEnkelvoud"].ToString();
            _NaamMeervoud = row["NaamMeervoud"].ToString();
            _Aantal = Convert.ToInt32(row["Aantal"]);
            _Prijs = Convert.ToSingle(row["Prijs"]);
            _Barcode = row["Barcode"].ToString();
            _ArtikelId = Convert.ToInt32(row["ArtikelId"]);
            if (!(row["ChildUnit"] is DBNull))
                _ChildUnitId = Convert.ToInt32(row["ChildUnit"]);
        }

        public Unit(DataRow row, int nul, int een)
        {
            _Id = Convert.ToInt32(row["UnitId"]);
            _NaamEnkelvoud = row["NaamEnkelvoud"].ToString();
            _NaamMeervoud = row["NaamMeervoud"].ToString();
            _Aantal = Convert.ToInt32(row["UnitAantal"]);
            _Prijs = Convert.ToSingle(row["UnitPrijs"]);
            _Barcode = row["UnitBarcode"].ToString();
            _ArtikelId = Convert.ToInt32(row["UnitArtikelId"]);
            if (!(row["ChildUnit"] is DBNull))
                _ChildUnitId = Convert.ToInt32(row["ChildUnit"]);
        }

        public Unit(int id)
        {
            _Id = id;
        }



        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public string NaamEnkelvoud
        {
            get { return _NaamEnkelvoud;}
            set { _NaamEnkelvoud = value; }
        }

        public string NaamMeervoud
        {
            get { return _NaamMeervoud; }
            set { _NaamMeervoud = value; }
        }

        public int Aantal
        {
            get { return _Aantal; }
            set { _Aantal = value; }
        }

        public float Prijs
        {
            get { return _Prijs; }
            set { _Prijs = value; }
        }

        public string Barcode
        {
            get { return _Barcode; }
            set { _Barcode = value; }
        }

        public int ArtikelId
        {
            get { return _ArtikelId; }
            set { _ArtikelId = value; }
        }

        public string UnitChildString
        {
            get { return _ChildUnitString; }
            set { _ChildUnitString = value; }
        }

        public int ChildUnitId
        {
            get { return _ChildUnitId; }
            set { _ChildUnitId = value; }
        }
    }
}