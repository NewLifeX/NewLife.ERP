using Erp.Data.Purchases;
using System.ComponentModel;
using Erp.Data.Sales;
using NewLife.Cube;
using NewLife.Web;
using Erp.Data.Models;

namespace NewLife.ERP.Areas.Sales.Controllers;

[SalesArea]
[Menu(0, false)]
public class SaleOrderLineController : EntityController<SaleOrderLine>
{
    static SaleOrderLineController()
    {
        LogOnChange = true;

        ListFields.RemoveRemarkField();
    }

    protected override IEnumerable<SaleOrderLine> Search(Pager p)
    {
        var orderId = p["orderId"].ToInt(-1);
        var productId = p["productId"].ToInt(-1);

        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        p.RetrieveState = true;

        return SaleOrderLine.Search(orderId, productId, start, end, p["Q"], p);
    }

    protected override Boolean Valid(SaleOrderLine entity, DataObjectMethodType type, Boolean post)
    {
        if (post)
        {
            if (entity.Order.Status != OrderStatus.录入) throw new InvalidOperationException("该状态下订单禁止修改！");

            if (entity.Price <= 0 && entity.Product != null) entity.Price = entity.Product.Price;
        }

        return base.Valid(entity, type, post);
    }

    protected override Int32 OnInsert(SaleOrderLine entity)
    {
        var order = entity.Order;
        if (order != null) entity.OccurTime = order.OccurTime;

        var rs = base.OnInsert(entity);

        if (order != null)
        {
            order.Fix();
            order.Update();
        }

        return rs;
    }

    protected override Int32 OnUpdate(SaleOrderLine entity)
    {
        var order = entity.Order;
        if (order != null) entity.OccurTime = order.OccurTime;

        var rs = base.OnUpdate(entity);

        if (order != null)
        {
            order.Fix();
            order.Update();
        }

        return rs;
    }
}