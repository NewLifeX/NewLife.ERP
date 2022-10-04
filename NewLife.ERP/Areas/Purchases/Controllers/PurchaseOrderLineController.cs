using System.ComponentModel;
using Erp.Data.Models;
using Erp.Data.Purchases;
using Erp.Data.Sales;
using NewLife.Cube;
using NewLife.Cube.ViewModels;
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

        {
            var df = ListFields.GetField("SupplierName") as ListField;
            df.Url = "/Purchases/Supplier?Id={SupplierId}";
        }
        {
            var df = ListFields.GetField("WarehouseName") as ListField;
            df.Url = "/Products/Warehouse?Id={WarehouseId}";
        }
        {
            var df = ListFields.GetField("ProductName") as ListField;
            df.Url = "/Products/Product?Id={ProductId}";
        }
        {
            var df = ListFields.GetField("OccurTime") as ListField;
            df.GetValue = e => (e as PurchaseOrderLine).OccurTime.ToString("yyyy-MM-dd");
        }
    }

    protected override IEnumerable<PurchaseOrderLine> Search(Pager p)
    {
        var orderId = p["orderId"].ToInt(-1);
        var productId = p["productId"].ToInt(-1);

        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        var order = PurchaseOrder.FindById(orderId);
        if (order != null && order.Status != OrderStatus.录入中) PageSetting.EnableAdd = false;

        p.RetrieveState = true;

        return PurchaseOrderLine.Search(orderId, productId, start, end, p["Q"], p);
    }

    protected override Boolean Valid(PurchaseOrderLine entity, DataObjectMethodType type, Boolean post)
    {
        if (post)
        {
            if (entity.Order.Status != OrderStatus.录入中) throw new InvalidOperationException("该状态下订单禁止修改！");

            // 新建时使用产品价格，但是后面可以修改为0价格
            if (type == DataObjectMethodType.Insert)
            {
                if (entity.Price <= 0 && entity.Product != null) entity.Price = entity.Product.Price;
            }
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