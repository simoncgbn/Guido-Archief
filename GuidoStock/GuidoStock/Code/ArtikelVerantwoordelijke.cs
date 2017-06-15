using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GuidoStock.Code
{
    public class ArtikelVerantwoordelijke
    {
        private int _ArtikelId;
        private int _UserId;

        public ArtikelVerantwoordelijke() { }

        public ArtikelVerantwoordelijke(int artikelid, int userid)
        {
            _ArtikelId = artikelid;
            _UserId = userid;
        }

        public ArtikelVerantwoordelijke(DataRow row)
        {
            _ArtikelId = Convert.ToInt32(row["ArtikelId"]);
            _UserId = Convert.ToInt32(row["UserId"]);
        }

        public int Artikelid
        {
            get{ return _ArtikelId;}
            set { _ArtikelId = value; }
        }

        public int UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }
    }
}