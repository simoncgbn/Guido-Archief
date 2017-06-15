using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GuidoStock.Code
{
    [Serializable]
    public class Locatie
    {
        private int _Id;
        private string _Straat;
        private string _Huisnummer;
        private string _Postcode;
        private string _Land;
        private string _Plaats;
        private string _Zaal;

        public Locatie()
        {

        }

        public Locatie(DataRow row)
        {
            _Id = Convert.ToInt32(row["LocatieId"]);
            _Straat = row["Straat"].ToString();
            _Huisnummer = row["Huisnummer"].ToString();
            _Postcode = row["Postcode"].ToString();
            _Land = row["Land"].ToString();
            _Plaats = row["Plaats"].ToString();
            _Zaal = row["Zaal"].ToString();
        }

        public Locatie(string straat, string huisnummer, string postcode, string land, string plaats, string zaal)
        {
            _Straat = straat;
            _Huisnummer = huisnummer;
            _Postcode = postcode;
            _Land = land;
            _Plaats = plaats;
            _Zaal = zaal;

        }

        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public string Straat
        {
            get { return _Straat; }
            set { _Straat = value; }
        }
  
        public string Huisnummer 
        {
            get { return _Huisnummer; }
            set { _Huisnummer = value; }
        }

        public string Postcode
        {
            get { return _Postcode; }
            set { _Postcode = value; }
        }

        public string Land
        {
            get { return _Land; }
            set { _Land = value; }
        }

        public string Plaats
        {
            get { return _Plaats; }
            set { _Plaats = value; }
        }

        public string Zaal
        {
            get { return _Zaal; }
            set { _Zaal = value; }
        }


        protected bool Equals(Locatie other)
        {
            return string.Equals(_Straat, other._Straat) && string.Equals(_Postcode, other._Postcode) && string.Equals(_Land, other._Land) && string.Equals(_Plaats, other._Plaats) && string.Equals(_Zaal, other._Zaal);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Locatie)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (_Straat != null ? _Straat.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_Postcode != null ? _Postcode.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_Land != null ? _Land.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_Plaats != null ? _Plaats.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_Zaal != null ? _Zaal.GetHashCode() : 0);
                return hashCode;
            }
        }

    }
}