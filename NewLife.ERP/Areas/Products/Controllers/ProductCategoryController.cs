using Erp.Data.Products;
using NewLife.Cube;
using NewLife.Cube.ViewModels;
using NewLife.Web;

namespace NewLife.ERP.Areas.Products.Controllers;

[ProductsArea]
[Menu(10)]
public class ProductCategoryController : EntityTreeController<ProductCategory>
{
    static ProductCategoryController()
    {
        LogOnChange = true;

        ListFields.RemoveRemarkField();

        {
            var df = ListFields.GetField("Products") as ListField;
            df.DisplayName = "{Products}";
            df.Title = "管理产品";
            df.Url = "Product?categoryId={Id}";
        }
    }

    protected override IEnumerable<ProductCategory> Search(Pager p)
    {
        var id = p["Id"].ToInt(-1);
        if (id > 0)
        {
            var entity = ProductCategory.FindById(id);
            if (entity != null) return new[] { entity };
        }

        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        return ProductCategory.Search(start, end, p["Q"], p);
    }

    protected override Int32 OnUpdate(ProductCategory entity)
    {
        entity.Fix();
        return base.OnUpdate(entity);

    }
}