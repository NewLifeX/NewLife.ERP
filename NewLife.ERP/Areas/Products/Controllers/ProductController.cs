﻿using System.ComponentModel;
using Erp.Data.Models;
using Erp.Data.Products;
using Microsoft.AspNetCore.Mvc;
using NewLife.Cube;
using NewLife.Cube.ViewModels;
using NewLife.Data;
using NewLife.Web;
using XCode;

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

    public ActionResult Search(Int32 roleId = 0, Int32 departmentId = 0, String key = null)
    {
        var exp = new WhereExpression();
        if (roleId > 0) exp &= _.RoleID == roleId;
        if (departmentId > 0) exp &= _.DepartmentID == departmentId;
        exp &= _.Enable == true;
        if (!key.IsNullOrEmpty()) exp &= _.Code.StartsWith(key) | _.Name.StartsWith(key) | _.DisplayName.StartsWith(key) | _.Mobile.StartsWith(key);

        var page = new PageParameter { PageSize = 20 };

        // 默认排序
        if (page.Sort.IsNullOrEmpty()) page.Sort = _.Name;

        var list = XCode.Membership.User.FindAll(exp, page);

        return Json(0, null, list.Select(e => new
        {
            e.ID,
            e.Code,
            e.Name,
            e.DisplayName,
            //e.DepartmentID,
            DepartmentName = e.Department?.ToString(),
        }).ToArray());
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
}