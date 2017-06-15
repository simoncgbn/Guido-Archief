using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GuidoStock.Code
{
    [Serializable]
    public class EvenementLocatie
    {
        private Evenement _Evenement;
        private Locatie _Locatie;
        private EvenementKlant _EvenementKlant;
        private DateTime _BeginTijd;
        private DateTime _EindTijd;
        private int _VerwachteOpkomst = -1;
        private int _Target = -1;

        public EvenementLocatie()
        {

        }

        public EvenementLocatie(DataRow row, Evenement evenement)
        {
            _BeginTijd = Convert.ToDateTime(row["BeginTijd"]);
            _EindTijd = Convert.ToDateTime(row["EindTijd"]);
            if (!(row["VerwachteOpkomst"] is DBNull))
            {
                _VerwachteOpkomst = Convert.ToInt32(row["VerwachteOpkomst"]);
            }
            if (!(row["Target"] is DBNull))
            {
                _Target = Convert.ToInt32(row["Target"]);
            }

            _Locatie = new Locatie(row);
            _EvenementKlant = new EvenementKlant(row);
            _Evenement = evenement;
        }

        public EvenementLocatie(DataRow row)
        {
            _BeginTijd = Convert.ToDateTime(row["BeginTijd"]);
            _EindTijd = Convert.ToDateTime(row["EindTijd"]);
            if (!(row["VerwachteOpkomst"] is DBNull))
            {
                _VerwachteOpkomst = Convert.ToInt32(row["VerwachteOpkomst"]);
            }
            if (!(row["Target"] is DBNull))
            {
                _Target = Convert.ToInt32(row["Target"]);
            }

            _Locatie = new Locatie(row);
            _EvenementKlant = new EvenementKlant(row);
        }

        public EvenementLocatie(DateTime beginTijd, DateTime eindTijd)
        {
            _BeginTijd = beginTijd;
            _EindTijd = eindTijd;
        }

        public EvenementLocatie(DateTime beginTijd, DateTime eindTijd, string plaats)
        {
            _BeginTijd = beginTijd;
            _EindTijd = eindTijd;
            _Locatie = new Locatie();
            _Locatie.Plaats = plaats;
        }

        public Evenement Evenement
        {
            get { return _Evenement; }
            set { _Evenement = value; }
        }

        public Locatie Locatie
        {
            get { return _Locatie; }
            set { _Locatie = value; }
        }

        public EvenementKlant EvenementKlant
        {
            get { return _EvenementKlant; }
            set { _EvenementKlant = value; }
        }

        public DateTime BeginTijd
        {
            get { return _BeginTijd; }
            set { _BeginTijd = value; }
        }

        public DateTime EindTijd
        {
            get { return _EindTijd; }
            set { _EindTijd = value; }
        }

        public int VerwachteOpkomst
        {
            get { return _VerwachteOpkomst; }
            set { _VerwachteOpkomst = value; }
        }

        public int Target
        {
            get { return _Target; }
            set { _Target = value; }
        }

        public int TempIndex { get; set; }
    }
}