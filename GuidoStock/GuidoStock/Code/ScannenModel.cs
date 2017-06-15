using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuidoStock.Code
{
    [Serializable]
    public class ScannenModel
    {
        private int _Id;
        private DateTime _Datum;
        private string _Naam;
        private string _Subtext;
        private bool _isEvent;
        private bool _isOnHold;
        private bool _isOnHold2;

        public ScannenModel()
        {
            
        }

        public ScannenModel(Evenement evnt)
        {
            _Id = evnt.Id;
            _Datum = evnt.Datum;
            _Naam = evnt.Naam;
            _Subtext = evnt.Subtext;
            _isEvent = true;
        }

        public ScannenModel(Evenement evnt, int nul)
        {
            _Id = evnt.Id;
            _Datum = evnt.Datum;
            _Naam = evnt.Naam;
            _Subtext = evnt.EvenementLocaties[0].Locatie.Plaats + " - " + evnt.Datum.ToString("dd MMMM");
            _isEvent = true;
        }

        public ScannenModel(Order order)
        {
            _Id = order.Id;
            _Datum = order.Datum;
            _Naam = order.Naam;
            _Subtext = order.Subtext;
            _isEvent = false;
        }

        public ScannenModel(Order order, int nul)
        {
            _Id = order.Id;
            _Datum = order.Datum;
            _Naam = order.Naam;
            _Subtext = order.IsVerhuur ? "Verhuur" : "Natura" + " - " + order.Datum.ToString("dd MMMM");
            _isEvent = false;
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

        public DateTime Datum
        {
            get { return _Datum; }
            set { _Datum = value; }
        }

        public string Subtext
        {
            get { return _Subtext; }
            set { _Subtext = value; }
        }

        public bool IsEvent
        {
            get { return _isEvent; }
            set { _isEvent = value; }
        }

        public bool IsOnHold
        {
            get { return _isOnHold; }
            set { _isOnHold = value; }
        }

        public bool IsOnHold2
        {
            get { return _isOnHold2; }
            set { _isOnHold2 = value; }
        }
    }
}