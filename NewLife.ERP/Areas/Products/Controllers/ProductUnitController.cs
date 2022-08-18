using Erp.Data.Products;
using NewLife.Cube;
using NewLife.Web;

namespace NewLife.ERP.Areas.Products.Controllers;

[ProductsArea]
[Menu(0, false)]
public class ProductUnitController : EntityController<ProductUnit>
{
    static ProductUnitController() => LogOnChange = true;

    protected override IEnumerable<ProductUnit> Search(Pager p)
    {
        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        return ProductUnit.Search(start, end, p["Q"], p);
    }
}