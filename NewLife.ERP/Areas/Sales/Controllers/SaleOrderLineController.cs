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
        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        return SaleOrderLine.Search(start, end, p["Q"], p);
    }
}