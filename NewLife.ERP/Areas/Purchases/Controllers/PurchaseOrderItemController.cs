using Erp.Data.Purchases;
using NewLife.Cube;
using NewLife.Web;

namespace NewLife.ERP.Areas.Purchases.Controllers;

[PurchasesArea]
[Menu(0, false)]
public class PurchaseOrderItemController : EntityController<PurchaseOrderItem>
{
    protected override IEnumerable<PurchaseOrderItem> Search(Pager p)
    {
        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        return PurchaseOrderItem.Search(start, end, p["Q"], p);
    }
}