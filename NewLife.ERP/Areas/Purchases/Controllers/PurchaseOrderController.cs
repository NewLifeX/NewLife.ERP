using Erp.Data.Purchases;
using Microsoft.AspNetCore.Mvc;
using NewLife.Cube;
using NewLife.Web;
using XCode.Membership;

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

    /// <summary>批量入库</summary>
    /// <returns></returns>
    [EntityAuthorize(PermissionFlags.Update)]
    public ActionResult SetIn()
    {
        var count = 0;
        var ids = GetRequest("keys").SplitAsInt();
        if (ids.Length > 0)
        {
            foreach (var id in ids)
            {
                var entity = PurchaseOrder.FindById(id);
                if (entity != null)
                {
                }
            }
        }

        return JsonRefresh($"共刷新[{count}]个团队");
    }
}