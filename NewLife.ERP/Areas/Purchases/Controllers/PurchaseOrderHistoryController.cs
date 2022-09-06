using Erp.Data.Purchases;
using NewLife.Cube;
using NewLife.Cube.Extensions;
using NewLife.Web;

namespace NewLife.ERP.Areas.Purchases.Controllers;

[PurchasesArea]
[Menu(0, false)]
public class PurchaseOrderHistoryController : ReadOnlyEntityController<PurchaseOrderHistory>
{
    static PurchaseOrderHistoryController()
    {
        ListFields.TraceUrl("TraceId");
    }

    protected override IEnumerable<PurchaseOrderHistory> Search(Pager p)
    {
        var orderId = p["orderId"].ToInt(-1);
        //var productId = p["productId"].ToInt(-1);

        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        return PurchaseOrderHistory.Search(orderId, start, end, p["Q"], p);
    }
}