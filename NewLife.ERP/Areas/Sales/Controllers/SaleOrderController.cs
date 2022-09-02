using Erp.Data.Purchases;
using Erp.Data.Sales;
using Microsoft.AspNetCore.Mvc;
using NewLife.Cube;
using NewLife.ERP.Services;
using NewLife.Web;
using XCode.Membership;

namespace NewLife.ERP.Areas.Sales.Controllers;

[SalesArea]
[Menu(70)]
public class SaleOrderController : EntityController<SaleOrder>
{
    private readonly SaleService _saleService;

    static SaleOrderController()
    {
        LogOnChange = true;

        ListFields.RemoveField("ContractNo", "BillCode");
        ListFields.RemoveCreateField();
        ListFields.RemoveRemarkField();

        {
            var df = ListFields.AddListField("Lines", "OccurTime");
            df.DisplayName = "订单明细";
            df.Url = "SaleOrderLine?orderId={Id}";
        }
        {
            var df = ListFields.AddListField("History", "OccurTime");
            df.DisplayName = "历史";
            df.Url = "SaleOrderHistory?orderId={Id}";
        }
    }

    public SaleOrderController(SaleService saleService)
    {
        _saleService = saleService;
    }

    protected override IEnumerable<SaleOrder> Search(Pager p)
    {
        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        p.RetrieveState = true;

        return SaleOrder.Search(start, end, p["Q"], p);
    }

    protected override Int32 OnUpdate(SaleOrder entity)
    {
        entity.Fix();

        return base.OnUpdate(entity);
    }

    /// <summary>批量出库</summary>
    /// <returns></returns>
    [EntityAuthorize(PermissionFlags.Update)]
    public ActionResult SetOut()
    {
        var count = 0;
        var ids = GetRequest("keys").SplitAsInt();
        if (ids.Length > 0)
        {
            using var tran = SaleOrder.Meta.CreateTrans();

            foreach (var id in ids)
            {
                var entity = SaleOrder.FindById(id);
                if (entity != null)
                    count += _saleService.SetOut(entity);
            }

            tran.Commit();
        }

        return JsonRefresh($"共处理[{count}]个订单");
    }

    /// <summary>批量取消出库</summary>
    /// <returns></returns>
    [EntityAuthorize(PermissionFlags.Update)]
    public ActionResult CancelOut()
    {
        var count = 0;
        var ids = GetRequest("keys").SplitAsInt();
        if (ids.Length > 0)
        {
            using var tran = SaleOrder.Meta.CreateTrans();

            foreach (var id in ids)
            {
                var entity = SaleOrder.FindById(id);
                if (entity != null)
                    count += _saleService.CancelOut(entity);
            }

            tran.Commit();
        }

        return JsonRefresh($"共处理[{count}]个订单");
    }
}