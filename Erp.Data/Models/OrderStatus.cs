using System;
using System.Collections.Generic;
using System.Text;

namespace Erp.Data.Models;

/// <summary>
/// 订单状态
/// </summary>
public enum OrderStatus
{
    录入 = 1,

    入库 = 10,

    出库 = 20,

    取消 = 99,
}