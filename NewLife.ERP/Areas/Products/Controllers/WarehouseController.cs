using Erp.Data.Products;
using NewLife.Cube;
using NewLife.Web;

namespace NewLife.ERP.Areas.Products.Controllers;

[ProductsArea]
[Menu(68)]
public class WarehouseController : EntityController<Warehouse>
{
    static WarehouseController() => LogOnChange = true;

    protected override IEnumerable<Warehouse> Search(Pager p)
    {
        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        return Warehouse.Search(start, end, p["Q"], p);
    }
}