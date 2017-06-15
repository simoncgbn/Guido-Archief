﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using GuidoStock.App_Code;

namespace GuidoStock.Code
{
    [Serializable]
    public class OrderLijn
    {
        private Artikel _Artikel;
        private Order _Order;
        private int _Aantal;

        public OrderLijn() {}

        public OrderLijn(Artikel artikel, Order order, int aantal)
        {
            _Artikel = artikel;
            _Order = order;
            _Aantal = aantal;
        }

        public OrderLijn(DataRow row)
        {
            _Artikel = new Artikel(row);
            if (row.Table.Columns.Contains("ArtikelId"))
            {
                _Artikel.Id = Convert.ToInt32(row["ArtikelId"]);
            }
            _Aantal = Convert.ToInt32(row["OrderLijnAantal"]);
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

        public Order Order
        {
            get { return _Order; }
            set { _Order = value; }
        }
    }
}