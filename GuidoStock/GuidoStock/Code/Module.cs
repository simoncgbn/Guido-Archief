using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GuidoStock.Code
{
    public class Module
    {
        private int _Id;
        private string _Naam;

        public Module(DataRow row)
        {
            _Id = Convert.ToInt32(row["Id"]);
            _Naam = row["Naam"].ToString();
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