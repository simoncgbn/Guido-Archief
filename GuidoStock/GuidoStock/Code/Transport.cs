using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GuidoStock.Code
{
    [Serializable]
    public class Transport
    {
        private int _Id;
        private string _Naam;
        private int _MaxGewicht;

        public Transport() { }

        public Transport(int id, string naam, int maxGewicht)
        {
            _Id = id;
            _Naam = naam;
            _MaxGewicht = maxGewicht;
        }

        public Transport(DataRow row)
        {
            _Id = Convert.ToInt32(row["TransportId"]);
            _Naam = row["Naam"].ToString();
            _MaxGewicht = Convert.ToInt32(row["MaxGewicht"]);
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

        public int MaxGewicht
        {
            get { return _MaxGewicht; }
            set { _MaxGewicht = value; }
        }
    }
}