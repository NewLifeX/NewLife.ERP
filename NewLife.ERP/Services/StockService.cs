using Erp.Data.Models;
using Erp.Data.Products;
using NewLife.Log;

namespace NewLife.ERP.Services;

/// <summary>
/// 库存服务
/// </summary>
public class StockService
{
    private readonly ITracer _tracer;

    public StockService(ITracer tracer)
    {
        _tracer = tracer;
    }

    /// <summary>
    /// 入库操作
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public StockHistory In(StockModel model)
    {
        if (model == null) throw new ArgumentNullException(nameof(model));
        if (model.ProductId == 0) throw new ArgumentOutOfRangeException(nameof(model));
        if (model.WarehouseId == 0) throw new ArgumentOutOfRangeException(nameof(model));

        using var span = _tracer?.NewSpan("erp:Stock:In", model);

        // 开启事务保护
        using var tran = ProductStock.Meta.CreateTrans();

        var ps = ProductStock.FindByProductIdAndWarehouseId(model.ProductId, model.WarehouseId);
        ps ??= new ProductStock { ProductId = model.ProductId, WarehouseId = model.WarehouseId };

        var qty = Math.Abs(model.Quantity);

        var hi = new StockHistory
        {
            ProductId = model.ProductId,
            WarehouseId = model.WarehouseId,
            Quantity = qty,

            Operation = StockOperations.入库,
            OccurTime = model.OccurTime,

            OrderId = model.OrderId,
            OrderTitle = model.OrderTitle,

            Remark = model.Remark,
        };

        // 修改库存
        hi.OldQuantity = ps.Quantity;
        ps.Quantity += qty;
        hi.NewQuantity = ps.Quantity;

        ps.Save();
        hi.Insert();

        // 修正产品关联数据
        var product = ps.Product;
        if (product != null)
        {
            product.Fix();
            product.Update();
        }

        tran.Commit();

        return hi;
    }

    /// <summary>
    /// 出库操作
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public StockHistory Out(StockModel model)
    {
        if (model == null) throw new ArgumentNullException(nameof(model));
        if (model.ProductId == 0) throw new ArgumentOutOfRangeException(nameof(model));
        if (model.WarehouseId == 0) throw new ArgumentOutOfRangeException(nameof(model));

        using var span = _tracer?.NewSpan("erp:Stock:Out", model);

        // 开启事务保护
        using var tran = ProductStock.Meta.CreateTrans();

        var ps = ProductStock.FindByProductIdAndWarehouseId(model.ProductId, model.WarehouseId);
        if (ps == null) return null;

        var qty = Math.Abs(model.Quantity);
        if (ps.Quantity < qty) throw new Exception($"[{ps.ProductName}]在[{ps.WarehouseName}]仓库的库存不足");

        var hi = new StockHistory
        {
            ProductId = model.ProductId,
            WarehouseId = model.WarehouseId,
            Quantity = -qty,

            Operation = StockOperations.出库,
            OccurTime = model.OccurTime,

            OrderId = model.OrderId,
            OrderTitle = model.OrderTitle,

            Remark = model.Remark,
        };

        // 修改库存
        hi.OldQuantity = ps.Quantity;
        ps.Quantity -= qty;
        hi.NewQuantity = ps.Quantity;

        ps.Update();
        hi.Insert();

        // 修正产品关联数据
        var product = ps.Product;
        if (product != null)
        {
            product.Fix();
            product.Update();
        }

        tran.Commit();

        return hi;
    }

    /// <summary>
    /// 移库操作
    /// </summary>
    /// <param name="model"></param>
    /// <param name="destWarehourseId"></param>
    /// <returns></returns>
    public StockHistory Move(StockModel model, Int32 destWarehourseId)
    {
        if (model == null) throw new ArgumentNullException(nameof(model));
        if (model.ProductId == 0) throw new ArgumentOutOfRangeException(nameof(model));
        if (model.WarehouseId == 0) throw new ArgumentOutOfRangeException(nameof(model));
        if (destWarehourseId == 0 || destWarehourseId == model.WarehouseId) throw new ArgumentOutOfRangeException(nameof(destWarehourseId));

        using var span = _tracer?.NewSpan("erp:Stock:Move", model);

        // 开启事务保护
        using var tran = ProductStock.Meta.CreateTrans();

        var ps = ProductStock.FindByProductIdAndWarehouseId(model.ProductId, model.WarehouseId);
        if (ps == null) return null;

        var ps2 = ProductStock.FindByProductIdAndWarehouseId(model.ProductId, destWarehourseId);
        ps2 ??= new ProductStock { ProductId = model.ProductId, WarehouseId = destWarehourseId };

        var qty = Math.Abs(model.Quantity);
        if (ps.Quantity < qty) throw new Exception($"[{ps.ProductName}]在[{ps.WarehouseName}]仓库的库存不足");

        var hi = new StockHistory
        {
            ProductId = model.ProductId,
            WarehouseId = model.WarehouseId,
            Quantity = -qty,

            Operation = StockOperations.移库,
            OccurTime = model.OccurTime,

            OrderId = model.OrderId,
            OrderTitle = model.OrderTitle,

            Remark = model.Remark,
        };

        var hi2 = new StockHistory
        {
            ProductId = model.ProductId,
            WarehouseId = destWarehourseId,
            Quantity = qty,

            Operation = StockOperations.移库,
            OccurTime = model.OccurTime,

            OrderId = model.OrderId,
            OrderTitle = model.OrderTitle,
        };

        // 修改库存
        hi.OldQuantity = ps.Quantity;
        ps.Quantity -= qty;
        hi.NewQuantity = ps.Quantity;

        hi2.OldQuantity = ps2.Quantity;
        ps2.Quantity += qty;
        hi2.NewQuantity = ps2.Quantity;

        ps.Update();
        hi.Insert();

        ps2.Save();
        hi2.Insert();

        // 移库不需要修正产品关联数据

        tran.Commit();

        return hi;
    }
}