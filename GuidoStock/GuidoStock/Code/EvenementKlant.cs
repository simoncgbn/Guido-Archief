using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GuidoStock.Code
{
    [Serializable]
    public class EvenementKlant
    {
        private int _Id;
        private string _Organisatie;
        private string _ContactNaam;
        private string _ContactTel;
        private string _ContactEmail;

        public EvenementKlant()
        {

        }

        public EvenementKlant(DataRow row)
        {
            _Id = Convert.ToInt32(row["Id"]);
            _Organisatie = row["Organisatie"].ToString();
            _ContactNaam = row["ContactNaam"].ToString();
            _ContactTel = row["ContactTel"].ToString();
            _ContactEmail = row["ContactEmail"].ToString();
            if (row.Table.Columns.Contains("EvenementKlantId"))
                _Id = Convert.ToInt32(row["EvenementKlantId"]);
        }


        public EvenementKlant(DataRow row,  int nul)
        {
            _Id = Convert.ToInt32(row["Id"]);
            _Organisatie = row["Organisatie"].ToString();
            
        }

        public EvenementKlant(string organisatie,string contactnaam,string contacttel,string contactemail)
        {
            _Organisatie = organisatie;
            _ContactNaam = contactnaam;
            _ContactTel = contacttel;
            _ContactEmail = contactemail;
        }


        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public string Organisatie
        {
            get { return _Organisatie; }
            set { _Organisatie = value; }
        }

        public string ContactNaam
        {
            get { return _ContactNaam; }
            set { _ContactNaam = value; }
        }

        public string ContactTel
        {
            get { return _ContactTel;}
            set { _ContactTel = value; }
        }

        public string ContactEmail
        {
            get { return _ContactEmail; }
            set { _ContactEmail = value; }
        }


        private bool Equals(EvenementKlant other)
        {
            return string.Equals(_Organisatie, other._Organisatie) && string.Equals(_ContactNaam, other._ContactNaam);
        }

    

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EvenementKlant)obj);
        }

       


    }
}