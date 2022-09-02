using Erp.Data.Models;
using Erp.Data.Products;
using NewLife.Cube;
using NewLife.Cube.Extensions;
using NewLife.Web;

namespace NewLife.ERP.Areas.Products.Controllers;

[ProductsArea]
[Menu(0, false)]
public class StockHistoryController : ReadOnlyEntityController<StockHistory>
{
    static StockHistoryController()
    {
        ListFields.TraceUrl("TraceId");
    }

    protected override IEnumerable<StockHistory> Search(Pager p)
    {
        var productId = p["productId"].ToInt(-1);
        var warehouseId = p["warehouseId"].ToInt(-1);
        var operation = (StockOperations)p["operation"].ToInt(-1);

        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        return StockHistory.Search(productId, warehouseId, operation, start, end, p["Q"], p);
    }
}