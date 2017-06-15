using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuidoStock.Code
{
    public class Country
    {
        private string _Code;
        private string _Naam;

        public Country()
        {

        }

        public Country(string code, string naam)
        {
            _Code = code;
            _Naam = naam;
        }

        public string Code
        {
            get { return _Code; }
            set { _Code = value; }
        }

        public string Naam
        {
            get { return _Naam; }
            set { _Naam = value; }
        }

    }
}