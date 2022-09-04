namespace Erp.Data.Models;

/// <summary>
/// 库存模型
/// </summary>
public class StockModel
{
    public Int32 ProductId { get; set; }

    public Int32 WarehouseId { get; set; }

    public Int32 Quantity { get; set; }

    public String OrderId { get; set; }

    public String OrderTitle { get; set; }

    public String Remark { get; set; }
}