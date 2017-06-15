using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GuidoStock.Code
{
    [Serializable]
    public class EvenementTeamLid
    {
        private int _Id;
        private string _Naam;
        private string _Functie;
        private string _Tel;
        private string _Email;
        private Evenement _Evenement;

        public EvenementTeamLid()
        {

        }

        public EvenementTeamLid(DataRow row, Evenement evenement)
        {
            _Evenement = evenement;
            _Id = Convert.ToInt32(row["EvenementTeamlidId"]);
            _Naam = row["EvenementTeamlidNaam"].ToString();
            _Functie = row["EvenementTeamlidFunctie"].ToString();
            _Tel = row["EvenementTeamlidTel"].ToString();
            _Email = row["EvenementTeamlidEmail"].ToString();
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

        public string Functie
        {
            get { return _Functie; }
            set { _Functie = value; }
        }

        public string Tel
        {
            get { return _Tel; }
            set { _Tel = value; }
        }

        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        public Evenement Evenement
        {
            get { return _Evenement; }
            set { _Evenement = value; }
        }
    }
}