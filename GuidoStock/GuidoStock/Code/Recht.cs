using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GuidoStock.Code
{
    [Serializable]
    public class Recht
    {
        private int _Id;
        private string _Rol;
        private string _Module;
        private bool _Schrijven;
        private bool _Lezen;

        public Recht()
        {

        }

        public Recht(DataRow row)
        {
            _Id = Convert.ToInt32(row["Id"]);
            _Rol = row["RolNaam"].ToString();
            _Module = row["ModuleNaam"].ToString();
            _Schrijven = Convert.ToBoolean(row["Schrijven"]);
            _Lezen = Convert.ToBoolean(row["Lezen"]);
        }

        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public string Rol
        {
            get { return _Rol; }
            set { _Rol = value; }
        }

        public string Module
        {
            get { return _Module; }
            set { _Module = value; }
        }

        public bool Schrijven
        {
            get { return _Schrijven; }
            set { _Schrijven = value; }
        }

        public bool Lezen
        {
            get { return _Lezen; }
            set { _Lezen = value; }
        }

        public string Rechten
        {
            get
            {
                if (Lezen && Schrijven)
                {
                    return "Lezen & schrijven";
                } else if (Lezen)
                {
                    return "Lezen";
                }
                else
                {
                    return "Geen";
                }
            }
        }
    }
}