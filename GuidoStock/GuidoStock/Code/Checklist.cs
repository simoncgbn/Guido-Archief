using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GuidoStock.App_Code;
using WebGrease.Css.Extensions;

namespace GuidoStock.Code
{
    [Serializable]
    public class Checklist
    {
        private List<ChecklistLijn> _ChecklistLijnen;
        private List<Lijn> _Lijnen; 
        private dynamic _Model;
        private List<Tuple<App_Code.Stock, int>> _AlteredStocks; 

        public Checklist()
        {

        }

        public Checklist(string type, int id)
        {
            _AlteredStocks = new List<Tuple<App_Code.Stock, int>>();
            var db = new DBClass();
            if (type.ToLower().Equals("true"))
                _Model = db.GetEvenement(id);
            else
                _Model = db.GetOrder(id);
            _ChecklistLijnen = db.GetChecklistlijnen(_Model);
            _Lijnen = db.GetLijnen(_Model);
            if (_ChecklistLijnen.Count == 0)
                GenerateChecklist();
            SplitUpStocks(db);
            //AddOrUpdateAltered(db);
        }

        public Checklist(dynamic model)
        {
            _AlteredStocks = new List<Tuple<App_Code.Stock, int>>();
            var db = new DBClass();
            _Model = model;
            _ChecklistLijnen = db.GetChecklistlijnen(_Model);
            _Lijnen = db.GetLijnen(_Model);
            if (_ChecklistLijnen.Count == 0)
                GenerateChecklist();
            SplitUpStocks(db);
            //AddOrUpdateAltered(db);
        }

        public List<ChecklistLijn> ChecklistLijnen
        {
            get { return _ChecklistLijnen.Where(lijn => lijn.Aantal != 0).OrderBy(lijn => lijn.Stock.ArtikelLocatie.Code).ThenBy(w => w.Stock.Artikel.Naam).ToList(); }
            set { _ChecklistLijnen = value; }
        }

        public dynamic Model
        {
            get { return _Model; }
            set { _Model = value; }
        }

        public List<Lijn> Lijnen
        {
            
            get { return _Lijnen; }
            set { _Lijnen = value; }
        }

        private void GenerateChecklist()
        {
            var db = new DBClass();
            var unavailables = db.GetUnavailableLijnen();
            var stocks = db.GetAllStocks();
            // Trek unavailable aantallen af van totaal aantallen
            unavailables.ForEach(tup => stocks.Find(s => s.Id == tup.Item1).Aantal -= tup.Item2);

            _Lijnen.ForEach(lijn =>
            {
                var totAantal = 0;
                var lijnStocks = GetStocksPerLijn(lijn, stocks).Where(s => s.Aantal != 0).Where(s => s.Vervaldatum > DateTime.Now).OrderBy(s => s.Vervaldatum).ThenByDescending(s => (lijn.Aantal % s.Unit.Aantal == 0) && (s.Aantal >= lijn.Aantal)).ThenByDescending(s => s.Unit.Aantal).ThenBy(s => s.Aantal).ToList();
                while (totAantal < lijn.Aantal)
                {
                    var stock = lijnStocks.Find(s => s.Unit.Aantal <= lijn.Aantal - totAantal && s.Vervaldatum == lijnStocks[0].Vervaldatum) ?? lijnStocks[0];
                    //var stock = lijnStocks[0];
                    // Check of er nog kleinere zijn
                    var aantal = Math.Min(lijn.Aantal - totAantal, stock.Unit.Aantal);
                    stock.TempAantal -= aantal;
                    totAantal += aantal;
                    var index = _ChecklistLijnen.FindIndex(
                        c => c.Stock.Id == stock.Id && c.Stock.Unit.Id == stock.Unit.Id);
                    if (index == -1)
                    {
                        var checklistLijn = new ChecklistLijn(stock, Model, aantal);
                        _ChecklistLijnen.Add(checklistLijn);
                    }
                    else
                    {
                        _ChecklistLijnen[index].Aantal += aantal;
                    }
                    if (stock.TempAantal != 0) continue;
                    lijnStocks.Remove(stock);
                }
            });
        }

        private List<App_Code.Stock> GetStocksPerLijn(Lijn lijn, List<App_Code.Stock> stocks) => stocks.Where(s => s.Artikel.Id == lijn.Artikel.Id).ToList();

        private void SplitUpStocks(DBClass db)
        {
            while (_ChecklistLijnen.Count(x => x.Aantal%x.Stock.Unit.Aantal != 0) != 0)
            {
                var list = new List<ChecklistLijn>();
                _ChecklistLijnen.Where(x => x.Aantal % x.Stock.Unit.Aantal != 0).ForEach(lijn =>
                {
                    // Stock moet gesplitst worden
                    double test = (double)lijn.Aantal / lijn.Stock.Unit.Aantal;
                    var toppedAantal = (int)Math.Ceiling(test) * lijn.Stock.Unit.Aantal;
                    var lowerAantal = (int) Math.Floor(test)*lijn.Stock.Unit.Aantal;
                    var origi = lijn.Aantal - lowerAantal;
                    var difference = toppedAantal - lowerAantal;
                    lijn.Aantal = lowerAantal;
                    var child = lijn.Stock.Unit.ChildUnitId;
                    // TODO :: remove toppedAantal van lijn.Stock   (???)
                    var stockNieuw = new App_Code.Stock(lijn.Stock);
                    stockNieuw.Unit = lijn.Stock.Unit;
                    //_AlteredStocks.Add(new Tuple<App_Code.Stock, int>(stockNieuw, -difference));
                    AddOrUpdateStock(lijn.Stock, -difference, db, stockNieuw.Unit);
                    // var stock = stocks.Find(x => x.Id == lijn.Stock.Id);
                    // db.UpdateStock(stock.Id, stock.Vervaldatum, stock.Aantal - toppedAantal, stock.ArtikelLocatie.Id, stock.Artikel.Id, stock.Unit.Id);
                    var totAantal = 0;
                    while (totAantal < difference)
                    {
                        if (child == 0) continue;
                        var unit = db.GetUnitById(child);
                        child = unit.ChildUnitId;
                        var aantal = (toppedAantal - totAantal) / unit.Aantal;
                        if (aantal == 0) continue;
                        AddOrUpdateStock(lijn.Stock, difference, db, unit);
                        totAantal += aantal * unit.Aantal;
                        var stock = new App_Code.Stock(lijn.Stock);
                        var check = new ChecklistLijn(stock, Model, (aantal * unit.Aantal) > origi ? origi : (aantal * unit.Aantal));
                        check.Stock.Unit = unit;
                        list.Add(check);
                    }
                });
                _ChecklistLijnen.RemoveAll(x => x.Aantal % x.Stock.Unit.Aantal != 0);
                _ChecklistLijnen.AddRange(list);
            }     
        }

        private void AddOrUpdateStock(App_Code.Stock stock, int aantal, DBClass db, Unit unit)
        {
            var stocks = db.GetAllStocks();
            var check =
                    stocks.Find(x => x.ArtikelLocatie.Id == stock.ArtikelLocatie.Id && x.UnitId == unit.Id && x.Vervaldatum.ToShortDateString().Equals(stock.Vervaldatum.ToShortDateString()));
            if (check != null)
            {
                //db.UpdateStock(check.Id, check.Vervaldatum, check.Aantal + aantal, check.ArtikelLocatie.Id, check.Artikel.Id, check.UnitId);
                _AlteredStocks.Add(new Tuple<App_Code.Stock, int>(check, aantal));
            }
            else
            {
                // Add Stock
                //db.AddStock(stock.Vervaldatum, aantal, stock.ArtikelLocatie.Id, stock.Artikel.Id, unit.Id);
                var newStock = new App_Code.Stock(stock);
                newStock.Id = 0;
                newStock.Unit = unit;
                _AlteredStocks.Add(new Tuple<App_Code.Stock, int>(newStock, aantal));
            }
        }

        public void AddOrUpdateAltered(DBClass db)
        {
            var list =_AlteredStocks.GroupBy(x => new { x.Item1.UnitID2, x.Item1.Id}).Select(c1 => new Tuple<App_Code.Stock, int>(c1.First().Item1, c1.Sum(c => c.Item2))).ToList();
            foreach (var tuple in list)
            {
                if (tuple.Item1.Id == 0)
                    db.AddStock(tuple.Item1.Vervaldatum, tuple.Item2, tuple.Item1.ArtikelLocatie.Id,
                        tuple.Item1.Artikel.Id, tuple.Item1.Unit.Id);
                else
                {
                    db.UpdateStock(tuple.Item1.Id, tuple.Item1.Vervaldatum, tuple.Item1.Aantal + tuple.Item2, tuple.Item1.ArtikelLocatie.Id,
                        tuple.Item1.Artikel.Id, tuple.Item1.Unit.Id);
                }
            }
        }
    }
}