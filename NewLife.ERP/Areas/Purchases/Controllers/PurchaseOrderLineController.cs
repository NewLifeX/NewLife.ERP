using Erp.Data.Purchases;
using NewLife.Cube;
using NewLife.Web;

namespace NewLife.ERP.Areas.Purchases.Controllers;

[PurchasesArea]
[Menu(0, false)]
public class PurchaseOrderLineController : EntityController<PurchaseOrderLine>
{
    protected override IEnumerable<PurchaseOrderLine> Search(Pager p)
    {
        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        return PurchaseOrderLine.Search(start, end, p["Q"], p);
    }

    protected override Int32 OnInsert(PurchaseOrderLine entity)
    {
        var rs = base.OnInsert(entity);

        var order = entity.Order;
        if(order != null)
        {
            order.Fix();
            order.Update();
        }

        return rs;
    }

    protected override Int32 OnUpdate(PurchaseOrderLine entity)
    {
        var rs = base.OnUpdate(entity);

        var order = entity.Order;
        if (order != null)
        {
            order.Fix();
            order.Update();
        }

        return rs;
    }
}