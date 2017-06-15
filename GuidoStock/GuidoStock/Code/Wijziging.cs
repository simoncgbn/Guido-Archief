using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GuidoStock.Code
{
    [Serializable]
    public class Wijziging
    {
        private int _Id;
        private DateTime _Time;
        private string _UserId;
        private string _Type;

        public Wijziging(){}

        public Wijziging(int id, DateTime time, string userid, string type)
        {
            _Id = id;
            _Time = time;
            _UserId = userid;
            _Type = type;
        }

        public Wijziging(DataRow row)
        {
            _Id = Convert.ToInt32(row["Id"]);
            _Time = Convert.ToDateTime(row["TimeChange"]);
            _UserId = row["UserId"].ToString();
            _Type = row["TypeChange"].ToString();
        }

        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public DateTime Time
        {
            get { return _Time; }
            set { _Time = value; }
        }

        public string UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
    }
}