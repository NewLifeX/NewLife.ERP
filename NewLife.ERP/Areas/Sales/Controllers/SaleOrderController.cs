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
        ListFields.RemoveField("ContractNo", "BillCode");
        ListFields.RemoveCreateField();
        ListFields.RemoveRemarkField();

        {
            var df = ListFields.AddListField("Lines", "OccurTime");
            df.DisplayName = "订单明细";
            df.Url = "SaleOrderLine?orderId={Id}";
        }
        {
            var df = ListFields.AddListField("History", "OccurTime");
            df.DisplayName = "历史";
            df.Url = "SaleOrderHistory?orderId={Id}";
        }
    }

    protected override IEnumerable<SaleOrder> Search(Pager p)
    {
        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        p.RetrieveState = true;

        return SaleOrder.Search(start, end, p["Q"], p);
    }
}