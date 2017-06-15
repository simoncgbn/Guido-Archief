using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using GuidoStock.App_Code;

namespace GuidoStock.Code
{
    [Serializable]
    public class Evenement
    {
        private int _Id;
        private string _Naam;
        private string _Opmerking;
        private DateTime _VertrekTransport = DateTime.MaxValue;
        private List<EvenementLocatie> _EvenementLocaties;
        private List<EvenementTaak> _EvenementTaken;
        private List<EvenementTeamLid> _EvenementTeamLeden;
        private List<EvenementTransport> _EvenementTransporten;
        private Gebruiker _Opdrachtgever;
        private Gebruiker _EventCoordinator;
        private Gebruiker _Fieldmanager;
        private DateTime _EventComplete;
        private string _CheckList;

        public Evenement()
        {
            _EvenementLocaties = new List<EvenementLocatie>();
            _EvenementTaken = new List<EvenementTaak>();
            _EvenementTeamLeden = new List<EvenementTeamLid>();
            _EvenementTransporten = new List<EvenementTransport>();
        }

        public Evenement(DataRow row)
        {
            _Id = Convert.ToInt32(row["EvenementId"]);
            _Naam = row["Naam"].ToString();
            _Opmerking = row["Opmerking"].ToString();
            if (!(row["VertrekTransport"] is DBNull))
            {
                _VertrekTransport = Convert.ToDateTime(row["VertrekTransport"]);
            }
            _EvenementLocaties = new List<EvenementLocatie>();
            _EvenementLocaties.Add(new EvenementLocatie(Convert.ToDateTime(row["BeginTijd"]),
                Convert.ToDateTime(row["EindTijd"])));
            _CheckList = row["CheckList"].ToString();
        }

        public Evenement(DataRow row, bool nul)
        {
            _Id = Convert.ToInt32(row["EvenementId"]);
            _Naam = row["Naam"].ToString();
            _Opmerking = row["Opmerking"].ToString();
            if (!(row["VertrekTransport"] is DBNull))
            {
                _VertrekTransport = Convert.ToDateTime(row["VertrekTransport"]);
            }
            _EvenementLocaties = new List<EvenementLocatie>();
            _EvenementLocaties.Add(new EvenementLocatie(Convert.ToDateTime(row["BeginTijd"]),
                Convert.ToDateTime(row["EindTijd"]), row["Plaats"].ToString()));
            _CheckList = row["CheckList"].ToString();
        }

        public Evenement(DataRow row, int nul)
        {
            _Id = Convert.ToInt32(row["EvenementId"]);
            _Naam = row["Naam"].ToString();
            _Opmerking = row["Opmerking"].ToString();
            if (!(row["VertrekTransport"] is DBNull))
            {
                _VertrekTransport = Convert.ToDateTime(row["VertrekTransport"]);
            }
            _EvenementLocaties = new List<EvenementLocatie>();
            _EvenementLocaties.Add(new EvenementLocatie(Convert.ToDateTime(row["BeginTijd"]),
                Convert.ToDateTime(row["EindTijd"]), row["Plaats"].ToString()));
        }

        public Evenement(DataRow row, int nul, string complete)
        {
            _Id = Convert.ToInt32(row["EvenementId"]);
            _Naam = row["Naam"].ToString();
            _Opmerking = row["Opmerking"].ToString();
            if (!(row["VertrekTransport"] is DBNull))
            {
                _VertrekTransport = Convert.ToDateTime(row["VertrekTransport"]);
            }
            _EvenementLocaties = new List<EvenementLocatie>();
            _EvenementLocaties.Add(new EvenementLocatie(Convert.ToDateTime(row["BeginTijd"]),
                Convert.ToDateTime(row["EindTijd"]), row["Plaats"].ToString()));
            if (!(row["EventComplete"] is DBNull))
            {
                _EventComplete = Convert.ToDateTime(row["EventComplete"]);
            }
        }

        public Evenement(DataRow row, int nul, int nuls)
        {
            _Id = Convert.ToInt32(row["EvenementId"]);
            _Naam = row["EvenementNaam"].ToString();
        }

        public Evenement(DataRow row, int nul, int nuls, int twee)
        {
            _Id = Convert.ToInt32(row["EvenementId"]);
        }

        public Evenement(DataTable table)
        {
            var row = table.Rows[0];
            _Id = Convert.ToInt32(row["Id"]);
            _Naam = row["Naam"].ToString();
            _Opmerking = row["Opmerking"].ToString();
            if (!(row["VertrekTransport"] is DBNull))
            {
                _VertrekTransport = Convert.ToDateTime(row["VertrekTransport"]);
            }
            Dictionary<string, EvenementLocatie> dict = new Dictionary<string, EvenementLocatie>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (!(dict.ContainsKey(table.Rows[i]["Id"] + "-" + table.Rows[i]["LocatieId"].ToString())))
                {
                    dict.Add(table.Rows[i]["Id"] + "-" + table.Rows[i]["LocatieId"].ToString(),
                        new EvenementLocatie(table.Rows[i], this));
                }
            }
            _EvenementLocaties = dict.Select(locatie => locatie.Value).ToList();
            Dictionary<int, EvenementTaak> taken = new Dictionary<int, EvenementTaak>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (!(table.Rows[i]["EvenementTaakId"] is DBNull))
                {
                    if (!(taken.ContainsKey(Convert.ToInt32(table.Rows[i]["EvenementTaakId"]))))
                    {
                        taken.Add(Convert.ToInt32(table.Rows[i]["EvenementTaakId"]),
                            new EvenementTaak(table.Rows[i], this));
                    }
                }
            }
            _EvenementTaken = taken.Select(taak => taak.Value).ToList();
            Dictionary<int, EvenementTeamLid> teamleden = new Dictionary<int, EvenementTeamLid>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (!(table.Rows[i]["EvenementTeamlidId"] is DBNull))
                {
                    if (!(teamleden.ContainsKey(Convert.ToInt32(table.Rows[i]["EvenementTeamlidId"]))))
                    {
                        teamleden.Add(Convert.ToInt32(table.Rows[i]["EvenementTeamlidId"]),
                            new EvenementTeamLid(table.Rows[i], this));
                    }
                }
            }
            _EvenementTeamLeden = teamleden.Select(teamlid => teamlid.Value).ToList();
            _Opdrachtgever = new Gebruiker(row["OpdrachtgeverId"], row["OpdrachtgeverVoornaam"],
                row["OpdrachtgeverAchternaam"],
                row["OpdrachtgeverTel"], row["OpdrachtgeverEmail"]);
            _EventCoordinator = new Gebruiker(row["CoordinatorId"], row["CoordinatorVoornaam"],
                row["CoordinatorAchternaam"],
                row["CoordinatorTel"], row["CoordinatorEmail"]);
            _Fieldmanager = new Gebruiker(row["FieldmanagerId"], row["FieldmanagerVoornaam"],
                row["FieldmanagerAchternaam"],
                row["FieldmanagerTel"], row["FieldmanagerEmail"]);
            _EvenementTransporten = new DBClass().GetEvenementTransportenByEvenementId(this);
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

        public string Opmerking
        {
            get { return _Opmerking; }
            set { _Opmerking = value; }
        }

        public DateTime VertrekTransport
        {
            get { return _VertrekTransport; }
            set { _VertrekTransport = value; }
        }

        public List<EvenementLocatie> EvenementLocaties
        {
            get { return _EvenementLocaties; }
            set { _EvenementLocaties = value; }
        }

        public List<EvenementTaak> EvenementTaken
        {
            get { return _EvenementTaken; }
            set { _EvenementTaken = value; }
        }

        public List<EvenementTeamLid> EvenementTeamLeden
        {
            get { return _EvenementTeamLeden; }
            set { _EvenementTeamLeden = value; }
        }

        public Gebruiker EventCoordinator
        {
            get { return _EventCoordinator; }
            set { _EventCoordinator = value; }
        }

        public Gebruiker Opdrachtgever
        {
            get { return _Opdrachtgever; }
            set { _Opdrachtgever = value; }
        }

        public Gebruiker Fieldmanager
        {
            get { return _Fieldmanager; }
            set { _Fieldmanager = value; }
        }

        public DateTime EventComplete
        {
            get { return _EventComplete; }
            set { _EventComplete = value; }
        }

        public List<EvenementTransport> EvenementTransporten
        {
            get { return _EvenementTransporten; }
            set { _EvenementTransporten = value; }
        }

        public string CheckList
        {
            get { return _CheckList; }
            set { _CheckList = value; }
        }

        public Dictionary<int, List<EvenementTaak>> ParsedEvenementTaken
        {
            get
            {
                var dict = new Dictionary<int, List<EvenementTaak>>();
                foreach (var taak in _EvenementTaken)
                {
                    if (dict.ContainsKey(taak.Taak.Id))
                    {
                        dict[taak.Taak.Id].Add(taak);
                    }
                    else
                    {
                        dict[taak.Taak.Id] = new List<EvenementTaak>() {taak};
                    }
                }
                return dict;
            }
        }

        public DateTime Datum => (from d in EvenementLocaties select d.BeginTijd).Min();

        public DateTime BeginTijd => (from d in EvenementLocaties select d.BeginTijd).Min();

        public DateTime EindTijd => (from d in EvenementLocaties select d.EindTijd).Max();
        public string Subtext => _EvenementLocaties[0].Locatie.Plaats;
    }
}