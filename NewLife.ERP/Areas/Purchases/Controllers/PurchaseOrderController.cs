using Erp.Data.Purchases;
using Microsoft.AspNetCore.Mvc;
using NewLife.Cube;
using NewLife.ERP.Services;
using NewLife.Web;
using XCode.Membership;

namespace NewLife.ERP.Areas.Purchases.Controllers;

[PurchasesArea]
[Menu(70)]
public class PurchaseOrderController : EntityController<PurchaseOrder>
{
    private readonly PurchaseService _purchaseService;

    static PurchaseOrderController()
    {
        LogOnChange = true;

        ListFields.RemoveField("ContractNo", "BillCode");
        ListFields.RemoveCreateField();
        ListFields.RemoveRemarkField();

        {
            var df = ListFields.AddListField("Lines", "OccurTime");
            df.DisplayName = "订单明细";
            df.Url = "PurchaseOrderLine?orderId={Id}";
        }
        {
            var df = ListFields.AddListField("History", "OccurTime");
            df.DisplayName = "历史";
            df.Url = "PurchaseOrderHistory?orderId={Id}";
        }

        AddFormFields.RemoveField("Status");
        EditFormFields.RemoveField("Status");
    }

    public PurchaseOrderController(PurchaseService purchaseService)
    {
        _purchaseService = purchaseService;
    }

    protected override IEnumerable<PurchaseOrder> Search(Pager p)
    {
        var supplierId = p["supplierId"].ToInt(-1);
        var warehouseId = p["warehouseId"].ToInt(-1);

        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        p.RetrieveState = true;

        return PurchaseOrder.Search(supplierId, warehouseId, start, end, p["Q"], p);
    }

    protected override Int32 OnUpdate(PurchaseOrder entity)
    {
        entity.Fix();

        return base.OnUpdate(entity);
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
            using var tran = PurchaseOrder.Meta.CreateTrans();

            foreach (var id in ids)
            {
                var entity = PurchaseOrder.FindById(id);
                if (entity != null)
                    count += _purchaseService.SetIn(entity);
            }

            tran.Commit();
        }

        return JsonRefresh($"共处理[{count}]个订单");
    }

    /// <summary>批量取消入库</summary>
    /// <returns></returns>
    [EntityAuthorize(PermissionFlags.Update)]
    public ActionResult CancelIn()
    {
        var count = 0;
        var ids = GetRequest("keys").SplitAsInt();
        if (ids.Length > 0)
        {
            using var tran = PurchaseOrder.Meta.CreateTrans();

            foreach (var id in ids)
            {
                var entity = PurchaseOrder.FindById(id);
                if (entity != null)
                    count += _purchaseService.CancelIn(entity);
            }

            tran.Commit();
        }

        return JsonRefresh($"共处理[{count}]个订单");
    }
}