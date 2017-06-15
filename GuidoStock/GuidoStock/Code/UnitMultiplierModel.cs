using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuidoStock.Code
{
    [Serializable]
    public class UnitMultiplierModel
    {
        private int _Id;
        private string _Naam;

        public UnitMultiplierModel()
        {
            
        }

        public UnitMultiplierModel(int id, string naam)
        {
            _Id = id;
            _Naam = naam;
            
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