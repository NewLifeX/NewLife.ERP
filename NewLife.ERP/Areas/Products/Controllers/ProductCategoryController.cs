using Erp.Data.Products;
using NewLife.Cube;
using NewLife.Web;

namespace NewLife.ERP.Areas.Products.Controllers;

[ProductsArea]
[Menu(10)]
public class ProductCategoryController : EntityTreeController<ProductCategory>
{
    //static ProductCategoryController() => LogOnChange = true;

    protected override IEnumerable<ProductCategory> Search(Pager p)
    {
        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        return ProductCategory.Search(start, end, p["Q"], p);
    }
}