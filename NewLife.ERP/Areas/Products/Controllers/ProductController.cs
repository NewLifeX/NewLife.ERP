using System.ComponentModel;
using Erp.Data.Models;
using Erp.Data.Products;
using Erp.Data.Purchases;
using Microsoft.AspNetCore.Mvc;
using NewLife.Cube;
using NewLife.Cube.ViewModels;
using NewLife.ERP.Services;
using NewLife.Web;
using XCode.Membership;

namespace NewLife.ERP.Areas.Products.Controllers;

[ProductsArea]
[Menu(75)]
public class ProductController : EntityController<Product>
{
    static ProductController()
    {
        LogOnChange = true;

        ListFields.RemoveCreateField();
        ListFields.RemoveUpdateField();
        ListFields.RemoveRemarkField();
        ListFields.RemoveField("Image", "Specification");

        //{
        //    var df = ListFields.GetField("Units") as ListField;
        //    df.DisplayName = "{Units}";
        //    df.Title = "管理产品SKU单元";
        //    df.Url = "ProductUnit?productId={Id}";
        //}
        {
            var df = ListFields.AddListField("Stock", "Weight");
            df.DisplayName = "库存";
            df.Url = "ProductStock?productId={Id}";
        }
        {
            var df = ListFields.AddListField("History", "Weight");
            df.DisplayName = "库存历史";
            df.Url = "StockHistory?productId={Id}";
        }
        {
            var df = ListFields.AddListField("Purchase", "Weight");
            df.DisplayName = "采购记录";
            df.Url = "/Purchases/PurchaseOrderLine?productId={Id}";
        }
        {
            var df = ListFields.AddListField("Sale", "Weight");
            df.DisplayName = "销售记录";
            df.Url = "/Sales/SaleOrderLine?productId={Id}";
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

        p.RetrieveState = true;

        return Product.Search(code, categoryId, kind, enable, start, end, p["Q"], p);
    }

    protected override Int32 OnUpdate(Product entity)
    {
        entity.Fix();

        var rs = base.OnUpdate(entity);

        var cat = entity.Category;
        if (cat != null)
        {
            cat.Fix();
            cat.Update();
        }

        return rs;
    }

    protected override Int32 OnInsert(Product entity)
    {
        var rs = base.OnInsert(entity);

        var cat = entity.Category;
        if (cat != null)
        {
            cat.Fix();
            cat.Update();
        }

        return rs;
    }

    protected override Int32 OnDelete(Product entity)
    {
        var rs = base.OnDelete(entity);

        var cat = entity.Category;
        if (cat != null)
        {
            cat.Fix();
            cat.Update();
        }

        return rs;
    }

    /// <summary>更新库存</summary>
    /// <returns></returns>
    [EntityAuthorize(PermissionFlags.Update)]
    public ActionResult RefreshStock()
    {
        var count = 0;
        var ids = GetRequest("keys").SplitAsInt();
        if (ids.Length > 0)
        {
            foreach (var id in ids)
            {
                var entity = Product.FindById(id);
                if (entity != null)
                {
                    entity.Fix();

                    count += entity.Update();
                }
            }
        }

        return JsonRefresh($"共更新[{count}]个");
    }
}