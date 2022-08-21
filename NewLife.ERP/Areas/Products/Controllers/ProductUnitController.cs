using Erp.Data.Products;
using NewLife.Cube;
using NewLife.Web;

namespace NewLife.ERP.Areas.Products.Controllers;

[ProductsArea]
[Menu(0, false)]
public class ProductUnitController : EntityController<ProductUnit>
{
    static ProductUnitController()
    {
        LogOnChange = true;

        ListFields.RemoveCreateField();
        ListFields.RemoveUpdateField();
        ListFields.RemoveRemarkField();
        ListFields.RemoveField("Image", "Specification");
    }

    protected override IEnumerable<ProductUnit> Search(Pager p)
    {
        var productId = p["productId"].ToInt(-1);

        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        return ProductUnit.Search(productId, null, start, end, p["Q"], p);
    }

    protected override Int32 OnInsert(ProductUnit entity)
    {
        var rs = base.OnInsert(entity);

        var prd = entity.Product;
        if (prd != null)
        {
            prd.Fix();
            prd.Update();
        }

        return rs;
    }

    protected override Int32 OnUpdate(ProductUnit entity)
    {
        var rs = base.OnUpdate(entity);

        var prd = entity.Product;
        if (prd != null)
        {
            prd.Fix();
            prd.Update();
        }

        return rs;
    }
}