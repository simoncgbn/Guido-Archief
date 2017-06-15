using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GuidoStock.Code
{
    [Serializable]
    public class Taak
    {
        private int _Id;
        private string _Naam;

        public Taak()
        {

        }

        public Taak(DataRow row)
        {
            _Id = Convert.ToInt32(row["TaakId"]);
            _Naam = row["TaakNaam"].ToString();
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
    }
}