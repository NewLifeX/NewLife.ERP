using Erp.Data.Models;
using Erp.Data.Sales;
using NewLife.Serialization;

namespace NewLife.ERP.Services;

public class SaleService
{
    private readonly StockService _stockService;

    public SaleService(StockService stockService) => _stockService = stockService;

    /// <summary>
    /// 销售出库
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public Int32 SetOut(SaleOrder order)
    {
        if (order == null) throw new ArgumentNullException(nameof(order));
        if (order.Status != OrderStatus.录入) throw new InvalidOperationException($"订单[{order}]的状态[{order.Status}]异常");
        //if (order.WarehouseId == 0) throw new Exception("未指定仓库");

        var list = SaleOrderLine.FindAllByOrderId(order.Id);
        foreach (var line in list)
        {
            if (line.WarehouseId == 0) throw new Exception($"[{line.ProductName}]未指定仓库");

            line.OccurTime = order.OccurTime;
            line.Update();

            _stockService.Out(new StockModel
            {
                ProductId = line.ProductId,
                WarehouseId = line.WarehouseId,
                Quantity = line.Quantity,

                OccurTime = order.OccurTime,
                OrderId = $"Sale-{order.Id}",
                OrderTitle = order.Title,
                Remark = order.ToJson(),
            });
        }

        order.Status = OrderStatus.出库;

        var hi = new SaleOrderHistory
        {
            OrderId = order.Id,
            Action = "销售出库",
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
    public Int32 CancelOut(SaleOrder order)
    {
        if (order == null) throw new ArgumentNullException(nameof(order));
        if (order.Status == OrderStatus.录入) throw new InvalidOperationException("订单未入库");
        //if (order.WarehouseId == 0) throw new Exception("未指定仓库");

        var list = SaleOrderLine.FindAllByOrderId(order.Id);
        foreach (var line in list)
        {
            if (line.WarehouseId == 0) throw new Exception($"[{line.ProductName}]未指定仓库");

            _stockService.In(new StockModel
            {
                ProductId = line.ProductId,
                WarehouseId = line.WarehouseId,
                Quantity = line.Quantity,

                OccurTime = order.OccurTime,
                OrderId = $"Sale-{order.Id}",
                OrderTitle = order.Title,
                Remark = order.ToJson(),
            });
        }

        order.Status = OrderStatus.录入;

        var hi = new SaleOrderHistory
        {
            OrderId = order.Id,
            Action = "取消出库",
            OccurTime = order.OccurTime,
            Remark = order.ToJson(),
        };
        hi.Insert();

        return order.Update();
    }
}