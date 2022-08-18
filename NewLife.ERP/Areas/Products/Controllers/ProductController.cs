using Erp.Data.Products;
using NewLife.Cube;
using NewLife.Web;

namespace NewLife.ERP.Areas.Products.Controllers;

[ProductsArea]
[Menu(75)]
public class ProductController : EntityController<Product>
{
    static ProductController() => LogOnChange = true;

    protected override IEnumerable<Product> Search(Pager p)
    {
        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        return Product.Search(start, end, p["Q"], p);
    }
}