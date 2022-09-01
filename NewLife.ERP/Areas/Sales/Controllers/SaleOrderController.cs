using Erp.Data.Sales;
using NewLife.Cube;
using NewLife.Web;

namespace NewLife.ERP.Areas.Sales.Controllers;

[SalesArea]
[Menu(70)]
public class SaleOrderController : EntityController<SaleOrder>
{
    static SaleOrderController()
    {
        {
            var df = ListFields.AddListField("Lines", "CreateUser");
            df.DisplayName = "订单明细";
            df.Url = "SaleOrderLine?orderId={Id}";
        }
        {
            var df = ListFields.AddListField("History", "CreateUser");
            df.DisplayName = "历史";
            df.Url = "SaleOrderHistory?orderId={Id}";
        }
    }

    protected override IEnumerable<SaleOrder> Search(Pager p)
    {
        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        return SaleOrder.Search(start, end, p["Q"], p);
    }
}