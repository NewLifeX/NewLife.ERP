using Erp.Data.Sales;
using NewLife.Cube;
using NewLife.Web;

namespace NewLife.ERP.Areas.Sales.Controllers;

[SalesArea]
[Menu(0, false)]
public class SaleOrderHistoryController : ReadOnlyEntityController<SaleOrderHistory>
{
    protected override IEnumerable<SaleOrderHistory> Search(Pager p)
    {
        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        return SaleOrderHistory.Search(start, end, p["Q"], p);
    }
}