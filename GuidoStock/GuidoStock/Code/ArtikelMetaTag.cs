using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GuidoStock.Code
{
    [Serializable]
    public class ArtikelMetaTag
    {
        #region{Fields}
        private int _Id;
        private string _Naam;
        private string _Waarde;
        private int _ArtikelId;
        #endregion

        #region{Constructors}

        public ArtikelMetaTag()
        {

        }

        public ArtikelMetaTag(DataRow row)
        {
            _Id = Convert.ToInt32(row["Id"]);
            _Naam = row["Naam"].ToString();
            _Waarde = row["Waarde"].ToString();
            _ArtikelId = Convert.ToInt32(row["ArtikelId"]);
        }

        public ArtikelMetaTag(int id, string naam, string waarde, int artikelid)
        {
            _Id = id;
            _Naam = naam;
            _Waarde = waarde;
            _ArtikelId = artikelid;
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

        public string Waarde
        {
            get { return _Waarde; }
            set { _Waarde = value; }
        }

        public int ArtikelId
        {
            get { return _ArtikelId; }
            set { _ArtikelId = value; }
        }

        #endregion


    }
}