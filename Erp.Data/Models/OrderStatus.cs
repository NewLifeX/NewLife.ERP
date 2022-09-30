namespace Erp.Data.Models;

/// <summary>
/// 订单状态
/// </summary>
public enum OrderStatus
{
    录入中 = 1,

    已入库 = 10,

    已出库 = 20,

    取消 = 99,
}