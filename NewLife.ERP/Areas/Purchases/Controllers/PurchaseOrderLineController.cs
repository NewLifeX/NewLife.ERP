using System.ComponentModel;
using Erp.Data.Purchases;
using NewLife.Cube;
using NewLife.Web;

namespace NewLife.ERP.Areas.Purchases.Controllers;

[PurchasesArea]
[Menu(0, false)]
public class PurchaseOrderLineController : EntityController<PurchaseOrderLine>
{
    static PurchaseOrderLineController()
    {
        LogOnChange = true;

        ListFields.RemoveRemarkField();
    }

    protected override IEnumerable<PurchaseOrderLine> Search(Pager p)
    {
        var orderId = p["orderId"].ToInt(-1);
        var productId = p["productId"].ToInt(-1);

        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        p.RetrieveState = true;

        return PurchaseOrderLine.Search(orderId, productId, start, end, p["Q"], p);
    }

    protected override Boolean Valid(PurchaseOrderLine entity, DataObjectMethodType type, Boolean post)
    {
        if (post)
        {
            if (entity.Price <= 0 && entity.Product != null) entity.Price = entity.Product.Price;
        }

        return base.Valid(entity, type, post);
    }

    protected override Int32 OnInsert(PurchaseOrderLine entity)
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

    protected override Int32 OnUpdate(PurchaseOrderLine entity)
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