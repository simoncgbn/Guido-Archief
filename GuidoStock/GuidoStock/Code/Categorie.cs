using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GuidoStock.App_Code {
    [Serializable]
    public class Categorie
    {
        #region{Fields}
        private int _Id;
        private string _Naam;
        private string _Omschrijving;
        #endregion

        #region{Constructors}

        public Categorie()
        {

        }

        public Categorie(DataRow row)
        {
            _Id = Convert.ToInt32(row["Id"]);
            _Naam = row["Naam"].ToString();
            _Omschrijving = row["Omschrijving"].ToString();
        }

        public Categorie(int id, string naam, string omschrijving)
        {
            _Id = id;
            _Naam = naam;
            _Omschrijving = omschrijving;
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

        public string Omschrijving
        {
            get { return _Omschrijving; }
            set { _Omschrijving = value; }
        }
        #endregion
    }
}