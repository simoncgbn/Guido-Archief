using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GuidoStock.App_Code {
    [Serializable]
    public class Merk
    {
        #region{Fields}
        private int _Id;
        private string _Naam;
        #endregion

        #region{Constructors}
        public Merk()
        {

        }

        public Merk(DataRow row)
        {
            _Id = Convert.ToInt32(row["Id"]);
            _Naam = row["Naam"].ToString();
        }

        public Merk(int id, string naam)
        {
            _Id = id;
            _Naam = naam;
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
        #endregion

    }
}