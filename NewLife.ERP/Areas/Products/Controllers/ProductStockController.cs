using Erp.Data.Products;
using NewLife.Cube;
using NewLife.Web;

namespace NewLife.ERP.Areas.Products.Controllers;

[ProductsArea]
[Menu(0, false)]
public class ProductStockController : EntityController<ProductStock>
{
    static ProductStockController() => LogOnChange = true;

    protected override IEnumerable<ProductStock> Search(Pager p)
    {
        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        return ProductStock.Search(start, end, p["Q"], p);
    }
}