using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using GuidoStock.App_Code;

namespace GuidoStock.Code
{
    [Serializable]
    public class OrderChecklistLijn
    {
        private Order _Order;
        private App_Code.Stock _Stock;
        private int _Aantal;
        private int _AantalWeg;
        private DateTime _ChecklistComplete;
        private Unit _Unit;


        public OrderChecklistLijn()
        {

        }

        public OrderChecklistLijn(Order order, App_Code.Stock stock, int aantal, int aantalweg)
        {
            _Order = order;
            _Stock = stock;
            _Aantal = aantal;
            _AantalWeg = aantalweg;
        }

        public OrderChecklistLijn(Order order, App_Code.Stock stock, int aantal, int aantalweg, Unit unit)
        {
            _Order = order;
            _Stock = stock;
            _Aantal = aantal;
            _AantalWeg = aantalweg;
            _Unit = unit;
        }

        public OrderChecklistLijn(DataRow row)
        {
            _Order = new Order(row,0,1);
            if (row.Table.Columns.Contains("OrderId"))
            {
                _Order.Id = Convert.ToInt32(row["OrderId"]);
            }
            Artikel Artikel = new Artikel();
            Artikel.Id = Convert.ToInt32(row["ArtikelId"]);
            _Stock = new App_Code.Stock(row, Artikel, 0);
            if (row.Table.Columns.Contains("StockId"))
            {
                _Stock.Id = Convert.ToInt32(row["StockId"]);
            }
            _Aantal = Convert.ToInt32(row["Aantal"]);
            _AantalWeg = Convert.ToInt32(row["AantalWeg"]);
            if (!string.IsNullOrEmpty(row["ChecklistComplete"].ToString()))
            {
                _ChecklistComplete = Convert.ToDateTime(row["ChecklistComplete"]);
            }
            if (!(row["UnitId"] is DBNull))
            {
                _Unit = new Unit(row, 0,1);
            }
        }

        public OrderChecklistLijn(DataRow row, int nul)
        {
            _Order = new Order();
            if (row.Table.Columns.Contains("OrderId"))
            {
                _Order.Id = Convert.ToInt32(row["OrderId"]);
            }

            _Stock = new App_Code.Stock();
            if (row.Table.Columns.Contains("StockId"))
            {
                _Stock.Id = Convert.ToInt32(row["StockId"]);
            }
            _Aantal = Convert.ToInt32(row["Aantal"]);
            _AantalWeg = Convert.ToInt32(row["AantalWeg"]);
            if (!string.IsNullOrEmpty(row["ChecklistComplete"].ToString()))
            {
                _ChecklistComplete = Convert.ToDateTime(row["ChecklistComplete"]);
            }
        }

        public Order Order
        {
            get { return _Order; }
            set { _Order = value; }
        }

        public App_Code.Stock Stock
        {
            get { return _Stock; }
            set { _Stock = value; }
        }


        public int Aantal
        {
            get { return _Aantal; }
            set { _Aantal = value; }
        }

        public int AantalWeg
        {
            get { return _AantalWeg; }
            set { _AantalWeg = value; }
        }

        public DateTime ChecklistComplete
        {
            get { return _ChecklistComplete; }
            set { _ChecklistComplete = value; }
        }
        public Unit Unit
        {
            get { return _Unit; }
            set { _Unit = value; }
        }

    }
}