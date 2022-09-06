using Erp.Data.Sales;
using NewLife.Cube;
using NewLife.Cube.Extensions;
using NewLife.Web;

namespace NewLife.ERP.Areas.Sales.Controllers;

[SalesArea]
[Menu(0, false)]
public class SaleOrderHistoryController : ReadOnlyEntityController<SaleOrderHistory>
{
    static SaleOrderHistoryController()
    {
        ListFields.TraceUrl("TraceId");
    }

    protected override IEnumerable<SaleOrderHistory> Search(Pager p)
    {
        var orderId = p["orderId"].ToInt(-1);
        //var productId = p["productId"].ToInt(-1);

        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        return SaleOrderHistory.Search(orderId, start, end, p["Q"], p);
    }
}