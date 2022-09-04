using Erp.Data.Models;
using Erp.Data.Purchases;
using NewLife.Log;
using NewLife.Serialization;

namespace NewLife.ERP.Services;

public class PurchaseService
{
    private readonly StockService _stockService;
    private readonly ITracer _tracer;

    public PurchaseService(StockService stockService,ITracer tracer)
    {
        _stockService = stockService;
        _tracer = tracer;
    }

    /// <summary>
    /// 采购入库
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public Int32 SetIn(PurchaseOrder order)
    {
        if (order == null) throw new ArgumentNullException(nameof(order));
        if (order.Status != OrderStatus.录入) throw new InvalidOperationException($"订单[{order}]的状态[{order.Status}]异常");
        if (order.WarehouseId == 0) throw new Exception("未指定仓库");

        using var span = _tracer?.NewSpan("erp:Purchase:SetIn", order);

        var list = PurchaseOrderLine.FindAllByOrderId(order.Id);
        foreach (var line in list)
        {
            line.OccurTime = order.OccurTime;
            line.Update();

            _stockService.In(new StockModel
            {
                ProductId = line.ProductId,
                WarehouseId = order.WarehouseId,
                Quantity = line.Quantity,

                OccurTime = order.OccurTime,
                OrderId = $"Purchase-{order.Id}",
                OrderTitle = order.Title,
                Remark = order.ToJson(),
            });
        }

        order.Status = OrderStatus.入库;

        var hi = new PurchaseOrderHistory
        {
            OrderId = order.Id,
            Action = "采购入库",
            OccurTime= order.OccurTime,
            Remark = order.ToJson(),
        };
        hi.Insert();

        return order.Update();
    }

    /// <summary>
    /// 取消采购入库
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="Exception"></exception>
    public Int32 CancelIn(PurchaseOrder order)
    {
        if (order == null) throw new ArgumentNullException(nameof(order));
        if (order.Status == OrderStatus.录入) throw new InvalidOperationException("订单未入库");
        if (order.WarehouseId == 0) throw new Exception("未指定仓库");

        using var span = _tracer?.NewSpan("erp:Purchase:CancelIn", order);

        var list = PurchaseOrderLine.FindAllByOrderId(order.Id);
        foreach (var line in list)
        {
            _stockService.Out(new StockModel
            {
                ProductId = line.ProductId,
                WarehouseId = order.WarehouseId,
                Quantity = line.Quantity,

                OccurTime = order.OccurTime,
                OrderId = $"Purchase-{order.Id}",
                OrderTitle = order.Title,
                Remark = order.ToJson(),
            });
        }

        order.Status = OrderStatus.录入;

        var hi = new PurchaseOrderHistory
        {
            OrderId = order.Id,
            Action = "取消入库",
            OccurTime= order.OccurTime,
            Remark = order.ToJson(),
        };
        hi.Insert();

        return order.Update();
    }
}