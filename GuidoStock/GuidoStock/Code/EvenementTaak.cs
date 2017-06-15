using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GuidoStock.Code
{
    [Serializable]
    public class EvenementTaak
    {
        private int _Id;
        private DateTime _Van;
        private DateTime _Tot;
        private Taak _Taak;
        private Gebruiker _Gebruiker;
        private Evenement _Evenement;

        public EvenementTaak()
        {
            
        }

        public EvenementTaak(DataRow row, Evenement evenement)
        {
            _Id = Convert.ToInt32(row["EvenementTaakId"]);
            _Van = Convert.ToDateTime(row["Van"]);
            _Tot = Convert.ToDateTime(row["Tot"]);
            _Taak = new Taak(row);
            _Evenement = evenement;
            _Gebruiker = new Gebruiker(row);
        }
        
        public EvenementTaak(DateTime van, DateTime tot, int taak, int evenement, int gebruiker)
        {
            _Van = van;
            _Tot = tot;
            _Taak = new Taak();
            _Taak.Id = taak;
            _Evenement = new Evenement();
            _Evenement.Id = evenement;
            _Gebruiker = new Gebruiker();
            _Gebruiker.Id = gebruiker;
        }


        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public DateTime Van
        {
            get { return _Van; }
            set { _Van = value; }
        }

        public DateTime Tot
        {
            get { return _Tot; }
            set { _Tot = value; }
        }

        public Taak Taak
        {
            get { return _Taak; }
            set { _Taak = value; }
        }

        public Evenement Evenement
        {
            get { return _Evenement; }
            set { _Evenement = value; }
        }

        public Gebruiker Gebruiker
        {
            get { return _Gebruiker; }
            set { _Gebruiker = value; }
        }

        public string VanUur => _Van.ToString("Humm");

        public string TotUur => _Tot.ToString("Humm");
    }
}