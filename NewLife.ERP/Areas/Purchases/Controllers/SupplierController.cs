using Erp.Data.Purchases;
using NewLife.Cube;
using NewLife.Web;

namespace NewLife.ERP.Areas.Purchases.Controllers;

[PurchasesArea]
[Menu(80)]
public class SupplierController : EntityController<Supplier>
{
    static SupplierController() => LogOnChange = true;

    protected override IEnumerable<Supplier> Search(Pager p)
    {
        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        return Supplier.Search(start, end, p["Q"], p);
    }
}