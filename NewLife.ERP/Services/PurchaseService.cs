using Erp.Data.Models;
using Erp.Data.Purchases;

namespace NewLife.ERP.Services;

public class PurchaseService
{
    public Int32 SetIn(PurchaseOrder order)
    {
        if (order == null) throw new ArgumentNullException(nameof(order));
        if (order.Status != OrderStatus.录入) throw new InvalidOperationException($"订单[{order}]的状态[{order.Status}]异常");

        order.Status = OrderStatus.入库;

        return order.Update();
    }

    public Int32 CancelIn(PurchaseOrder order)
    {
        return 0;
    }
}
