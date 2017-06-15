using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GuidoStock.Code
{
    [Serializable]
    public class EvenementTransport
    {
        // TODO :: Refactor al die private properties naar nen deftige naam
        private Evenement _Evenement;
        private Transport _Transport;
        private Gebruiker _ChauffeurHeen;
        private Gebruiker _ChauffeurTerug;
        private DateTime _Vertrek;

        public EvenementTransport()
        {
            
        }

        public EvenementTransport(DataRow row, Evenement evenement)
        {
            _Evenement = evenement;
            _Transport = new Transport(Convert.ToInt32(row["Id"]), row["Naam"].ToString(), Convert.ToInt32(row["MaxGewicht"]));
            _ChauffeurHeen = new Gebruiker(Convert.ToInt32(row["TerugId"]), row["TerugVoornaam"].ToString(), row["TerugAchternaam"].ToString(),
                                           row["TerugTel"].ToString());
            _ChauffeurTerug = new Gebruiker(Convert.ToInt32(row["HeenId"]), row["HeenVoornaam"].ToString(), row["HeenAchternaam"].ToString(),
                                            row["HeenTel"].ToString());
            _Vertrek = Convert.ToDateTime(row["Vertrek"]);
        }

        public Evenement Evenement
        {
            get { return _Evenement; }
            set { _Evenement = value; }
        }

        public Transport Transport
        {
            get { return _Transport; }
            set { _Transport = value; }
        }

        public Gebruiker ChauffeurHeen
        {
            get { return _ChauffeurHeen; }
            set { _ChauffeurHeen = value; }
        }

        public Gebruiker ChauffeurTerug
        {
            get { return _ChauffeurTerug; }
            set { _ChauffeurTerug = value; }
        }

        public DateTime Vertrek
        {
            get { return _Vertrek; }
            set { _Vertrek = value; }
        }
    }
}