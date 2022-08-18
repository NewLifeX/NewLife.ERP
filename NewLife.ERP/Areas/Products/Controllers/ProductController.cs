﻿using Erp.Data.Models;
using Erp.Data.Products;
using NewLife.Cube;
using NewLife.Web;

namespace NewLife.ERP.Areas.Products.Controllers;

[ProductsArea]
[Menu(75)]
public class ProductController : EntityController<Product>
{
    static ProductController()
    {
        LogOnChange = true;

        {
            var df = ListFields.AddListField("Stock", "CreateUser");
            df.DisplayName = "库存";
            df.Url = "ProductStock?productId={Id}";
        }
        {
            var df = ListFields.AddListField("History", "CreateUser");
            df.DisplayName = "库存历史";
            df.Url = "StockHistory?productId={Id}";
        }
        {
            var df = ListFields.AddListField("Log", "CreateUser");
            df.DisplayName = "日志";
            df.Url = "/Admin/Log?category=产品&linkId={Id}";
        }
    }

    protected override IEnumerable<Product> Search(Pager p)
    {
        var id = p["Id"].ToInt(-1);
        if (id > 0)
        {
            var entity = Product.FindById(id);
            if (entity != null) return new[] { entity };
        }

        var code = p["code"];
        var categoryId = p["categoryId"].ToInt(-1);
        var kind = (ProductKinds)p["kind"].ToInt(-1);
        var enable = p["enable"]?.ToBoolean();

        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        return Product.Search(code, categoryId, kind, enable, start, end, p["Q"], p);
    }
}