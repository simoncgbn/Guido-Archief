using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GuidoStock.Code
{
    [Serializable]
    public class Gebruiker
    {
        #region{Fields}
        private int _Id;
        private string _Voornaam;
        private string _Achternaam;
        private string _Tel;
        private string _Email;
        private string _Naam;
        private bool _IsVerwijderd;
        #endregion


        #region{Constructors}
        public Gebruiker()
        {

        }

        public Gebruiker(DataRow row)
        {
            _Id = Convert.ToInt32(row["GebruikerId"]);
            _Voornaam = row["Voornaam"].ToString();
            _Achternaam = row["Achternaam"].ToString();
            _Tel = row["Tel"].ToString();
            _Email = row["Email"].ToString();
            _Naam = row["Naam"].ToString();
        }

        public Gebruiker(DataRow row, string nul)
        {
            _Id = Convert.ToInt32(row["GebruikerId"]);
            _Voornaam = row["Voornaam"].ToString();
            _Achternaam = row["Achternaam"].ToString();
            _Tel = row["Tel"].ToString();
            _Email = row["Email"].ToString();
        }

        public Gebruiker(DataRow row, int nul)
        {
            _Id = Convert.ToInt32(row["GebruikerId"]);
            _Voornaam = row["Voornaam"].ToString();
            _Achternaam = row["Achternaam"].ToString();
            _Tel = row["Tel"].ToString();
            _Email = row["Email"].ToString();
            _Naam = row["Naam"].ToString();
            _IsVerwijderd = Convert.ToBoolean(row["IsVerwijderd"]);
        }

        public Gebruiker(int id, string voornaam, string achternaam, string tel, string email, string naam)
        {
            _Id = id;
            _Voornaam = voornaam;
            _Achternaam = achternaam;
            _Tel = tel;
            _Email = email;
            _Naam = naam;
        }

        public Gebruiker(int id, string voornaam, string achternaam, string tel)
        {
            _Id = id;
            _Voornaam = voornaam;
            _Achternaam = achternaam;
            _Tel = tel;
            _Naam = voornaam + " " + achternaam;
        }

        public Gebruiker(Object id, Object voornaam, Object achternaam, Object tel, Object email)
        {
            if (!(id is DBNull))
                _Id = Convert.ToInt32(id);
            if (!(voornaam is DBNull))
                _Voornaam = voornaam.ToString();
            if (!(achternaam is DBNull))
                _Achternaam = achternaam.ToString();
            if (!(tel is DBNull))
                _Tel = tel.ToString();
            if (!(email is DBNull))
                _Email = email.ToString();
            if (!(voornaam is DBNull) && !(achternaam is DBNull))
                _Naam = voornaam + " " + achternaam;
        }
        #endregion

        #region{Properties}
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public string Voornaam
        {
            get { return _Voornaam ?? "Geen"; }
            set { _Voornaam = value; }
        }

        public string Achternaam
        {
            get { return _Achternaam ?? "Geen"; }
            set { _Achternaam = value; }
        }

        public string Tel
        {
            get { return _Tel ?? "Geen"; }
            set { _Tel = value; }
        }

        public string Email
        {
            get { return _Email ?? "Geen"; }
            set { _Email = value; }
        }

        public string Naam
        {
            get { return _Naam ?? "Geen"; }
            set { _Naam = value; }
        }

        public bool IsVerwijderd => _IsVerwijderd;

        #endregion



    }
}