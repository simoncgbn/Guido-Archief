 using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using GuidoStock.App_Code;

namespace GuidoStock.Code
{
    [Serializable]
    public class EvenementLijn
    {
        private Evenement _Evenement;
        private Artikel _Artikel;
        private int _Aantal;

        public EvenementLijn()
        {
            
        }

        public EvenementLijn(Evenement evenement, Artikel artikel, int aantal)
        {
            _Evenement = evenement;
            _Artikel = artikel;
            _Aantal = aantal;
        }

        public EvenementLijn(DataRow row)
        {
            _Artikel = new Artikel(row,0,"1");
            if (row.Table.Columns.Contains("ArtikelId"))
            {
                _Artikel.Id = Convert.ToInt32(row["ArtikelId"]);
            }
            _Aantal = Convert.ToInt32(row["EvenementLijnAantal"]);
            if (row.Table.Columns.Contains("ArtikelLocatieNaam"))
            {
                var stock = new App_Code.Stock(row, _Artikel);
                _Artikel.Stocks.Add(stock);
            }       
        }

        public Evenement Evenement
        {
            get { return _Evenement; }
            set { _Evenement = value; }
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
    }
}