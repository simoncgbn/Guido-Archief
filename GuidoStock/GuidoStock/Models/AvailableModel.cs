using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GuidoStock.Models
{
    [Serializable]
    public class AvailableModel
    {
        private int _ArtikelId;
        private int _StockAantal;
        private bool _IsHerbruikbaar;
        private int _LijnAantal;
        private int _AantalVervalt;

        public AvailableModel() { }

        public AvailableModel(DataRow row)
        {
            _ArtikelId = Convert.ToInt32(row["ArtikelId"]);
            _StockAantal = Convert.ToInt32(row["StockAantal"]);
            _IsHerbruikbaar = Convert.ToBoolean(row["IsHerbruikbaar"]);
            _LijnAantal = Convert.ToInt32(row["LijnAantal"]);
            _AantalVervalt = Convert.ToInt32(row["AantalVervalt"]);
        }

        public int ArtikelId
        {
            get { return _ArtikelId; }
            set { _ArtikelId = value; }
        }

        public int StockAantal
        {
            get { return _StockAantal; }
            set { _StockAantal = value; }
        }

        public bool IsHerbruikbaar
        {
            get { return _IsHerbruikbaar; }
            set { _IsHerbruikbaar = value; }
        }

        public int LijnAantal
        {
            get { return _LijnAantal; }
            set { _LijnAantal = value; }
        }

        public int AantalVervalt
        {
            get { return _AantalVervalt; }
            set { _AantalVervalt = value; }
        }
    }
}