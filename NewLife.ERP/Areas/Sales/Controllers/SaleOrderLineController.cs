using System.ComponentModel;
using Erp.Data.Models;
using Erp.Data.Sales;
using NewLife.Cube;
using NewLife.Cube.ViewModels;
using NewLife.Web;

namespace NewLife.ERP.Areas.Sales.Controllers;

[SalesArea]
[Menu(0, false)]
public class SaleOrderLineController : EntityController<SaleOrderLine>
{
    static SaleOrderLineController()
    {
        LogOnChange = true;

        ListFields.RemoveField("OrderTitle");
        ListFields.RemoveRemarkField();

        {
            var df = ListFields.GetField("CustomerName") as ListField;
            df.Url = "/Customers/Customer?Id={CustomerId}";
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
            df.GetValue = e => (e as SaleOrderLine).OccurTime.ToString("yyyy-MM-dd");
        }
    }

    protected override IEnumerable<SaleOrderLine> Search(Pager p)
    {
        var orderId = p["orderId"].ToInt(-1);
        var productId = p["productId"].ToInt(-1);

        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        var order = SaleOrder.FindById(orderId);
        if (order != null && order.Status != OrderStatus.录入中) PageSetting.EnableAdd = false;

        p.RetrieveState = true;

        //if (orderId > 0) ListFields.RemoveField("OrderTitle");

        return SaleOrderLine.Search(orderId, productId, start, end, p["Q"], p);
    }

    protected override Boolean Valid(SaleOrderLine entity, DataObjectMethodType type, Boolean post)
    {
        if (post)
        {
            if (entity.Order.Status != OrderStatus.录入中) throw new InvalidOperationException("该状态下订单禁止修改！");

            //// 新建时使用产品价格，但是后面可以修改为0价格
            //if (type == DataObjectMethodType.Insert)
            //{
            //    if (entity.Amount <= 0 && entity.Product != null) entity.Amount = entity.Quantity * entity.Product.Price;
            //}
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