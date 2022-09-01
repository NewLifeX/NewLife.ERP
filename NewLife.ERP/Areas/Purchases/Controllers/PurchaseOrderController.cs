using Erp.Data.Purchases;
using NewLife.Cube;
using NewLife.Web;

namespace NewLife.ERP.Areas.Purchases.Controllers;

[PurchasesArea]
[Menu(70)]
public class PurchaseOrderController : EntityController<PurchaseOrder>
{
    static PurchaseOrderController()
    {
        {
            var df = ListFields.AddListField("Lines", "CreateUser");
            df.DisplayName = "订单明细";
            df.Url = "PurchaseOrderLine?orderId={Id}";
        }
        {
            var df = ListFields.AddListField("History", "CreateUser");
            df.DisplayName = "历史";
            df.Url = "PurchaseOrderHistory?orderId={Id}";
        }
    }

    protected override IEnumerable<PurchaseOrder> Search(Pager p)
    {
        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        return PurchaseOrder.Search(start, end, p["Q"], p);
    }
}