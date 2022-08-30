using Erp.Data.Purchases;
using NewLife.Cube;
using NewLife.Web;

namespace NewLife.ERP.Areas.Purchases.Controllers;

[PurchasesArea]
[Menu(0, false)]
public class PurchaseOrderHistoryController : ReadOnlyEntityController<PurchaseOrderHistory>
{
    protected override IEnumerable<PurchaseOrderHistory> Search(Pager p)
    {
        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        return PurchaseOrderHistory.Search(start, end, p["Q"], p);
    }
}