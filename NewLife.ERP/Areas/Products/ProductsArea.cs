using System.ComponentModel;
using NewLife.Cube;

namespace NewLife.ERP.Areas.Products;

[DisplayName("产品中心")]
public class ProductsArea : AreaBase
{
    public ProductsArea() : base(nameof(ProductsArea).TrimEnd("Area")) { }

    static ProductsArea() => RegisterArea<ProductsArea>();
}