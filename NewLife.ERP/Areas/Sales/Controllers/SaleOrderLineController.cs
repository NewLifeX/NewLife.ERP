using Erp.Data.Sales;
using NewLife.Cube;
using NewLife.Web;

namespace NewLife.ERP.Areas.Sales.Controllers;

[SalesArea]
[Menu(0, false)]
public class SaleOrderLineController : EntityController<SaleOrderLine>
{
    protected override IEnumerable<SaleOrderLine> Search(Pager p)
    {
        var orderId = p["orderId"].ToInt(-1);
        var productId = p["productId"].ToInt(-1);

        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        p.RetrieveState = true;

        return SaleOrderLine.Search(orderId, productId, start, end, p["Q"], p);
    }
}