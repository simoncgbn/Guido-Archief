using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using GuidoStock.App_Code;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace GuidoStock.Code
{
    [Serializable]
    public class VermistArtikel
    {
        private int _Id;
        private int _EventId;
        private bool _IsEvent;
        private int _ArtikelId;
        private int _Aantal;
        private string _UserId;
        private string _UserName;
        private Artikel _Artikel;
        private Evenement _Evenement;
        private Order _Order;

        public VermistArtikel() { }

        public VermistArtikel(int eventid, bool isevent, int artikelid, int aantal, string userid)
        {
            _EventId = eventid;
            _IsEvent = isevent;
            _ArtikelId = artikelid;
            _Aantal = aantal;
            _UserId = userid;
        }

        public VermistArtikel(DataRow row)
        {
            _Id = Convert.ToInt32(row["Id"]);
            _EventId = Convert.ToInt32(row["EventId"]);
            _IsEvent = Convert.ToBoolean(row["IsEvent"]);
            _ArtikelId = Convert.ToInt32(row["ArtikelId"]);
            _Artikel = new Artikel
            {
                Naam = row["ArtikelNaam"].ToString(),
                Merk = new Merk
                {
                    Naam = row["MerkNaam"].ToString()
                }
            };
            _Aantal = Convert.ToInt32(row["Aantal"]);
            _UserId = row["UserId"].ToString();
        }

        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public int EventId
        {
            get { return _EventId; }
            set { _EventId = value; }
        }

        public bool IsEvent
        {
            get { return _IsEvent; }
            set { _IsEvent = value; }
        }

        public int ArtikelId
        {
            get { return _ArtikelId; }
            set { _ArtikelId = value; }
        }

        public int Aantal
        {
            get { return _Aantal; }
            set { _Aantal = value; }
        }

        public string UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }

        public Artikel Artikel
        {
            get { return _Artikel; }
            set { _Artikel = value; }
        }

        public Evenement Evenement
        {
            get { return _Evenement; }
            set { _Evenement = value; }
        }

        public Order Order
        {
            get { return _Order; }
            set { _Order = value; }
        }

        public string CombNaam => _IsEvent ? _Evenement.Naam : _Order.Naam;
        public DateTime CombDatum => _IsEvent ? _Evenement.EvenementLocaties[0].BeginTijd : _Order.BeginTijd;
    }
    }