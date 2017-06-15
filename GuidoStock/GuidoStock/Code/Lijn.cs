using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using GuidoStock.App_Code;

namespace GuidoStock.Code
{
    [Serializable]
    public class Lijn
    {
        private Artikel _Artikel;
        private dynamic _Model;
        private int _Aantal;

        public Lijn(DataRow row, dynamic model)
        {
            _Artikel = new Artikel(row);
            if (row.Table.Columns.Contains("ArtikelId"))
            {
                _Artikel.Id = Convert.ToInt32(row["ArtikelId"]);
            }
            _Aantal = Convert.ToInt32(row["LijnAantal"]);
            _Model = model;
            if (row.Table.Columns.Contains("ArtikelLocatieNaam"))
            {
                var stock = new App_Code.Stock(row, _Artikel);
                _Artikel.Stocks.Add(stock);
            }
        }

        public Artikel Artikel
        {
            get { return _Artikel; }
            set { _Artikel = value; }
        }

        public int Aantal
        {
            get { return _Aantal; }
            set { _Aantal = value; }
        }

        public dynamic Model
        {
            get { return _Model; }
            set { _Model = value; }
        }
    }
}