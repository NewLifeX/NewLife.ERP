using Erp.Data.Products;
using NewLife.Cube;
using NewLife.Web;

namespace NewLife.ERP.Areas.Products.Controllers;

[ProductsArea]
[Menu(0, false)]
public class StockHistoryController : ReadOnlyEntityController<StockHistory>
{
    protected override IEnumerable<StockHistory> Search(Pager p)
    {
        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        return StockHistory.Search(start, end, p["Q"], p);
    }
}