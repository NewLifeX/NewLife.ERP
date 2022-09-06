using Erp.Data.Purchases;
using Microsoft.AspNetCore.Mvc;
using NewLife.Cube;
using NewLife.Data;
using NewLife.Web;
using XCode;

namespace NewLife.ERP.Areas.Purchases.Controllers;

[PurchasesArea]
[Menu(80)]
public class SupplierController : EntityController<Supplier>
{
    static SupplierController()
    {
        LogOnChange = true;

        ListFields.RemoveField("Phone", "Address");
        ListFields.RemoveCreateField();
        ListFields.RemoveRemarkField();

        {
            var df = ListFields.AddListField("Order", "UpdateUser");
            df.DisplayName = "采购单";
            df.Url = "/Purchases/PurchaseOrder?supplierId={Id}";
        }
        {
            var df = ListFields.AddListField("Log", "UpdateUser");
            df.DisplayName = "日志";
            df.Url = "/Admin/Log?category=供应商&linkId={Id}";
        }
    }

    protected override IEnumerable<Supplier> Search(Pager p)
    {
        var id = p["Id"].ToInt(-1);
        if (id > 0)
        {
            var entity = Supplier.FindById(id);
            if (entity != null) return new[] { entity };
        }

        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        return Supplier.Search(start, end, p["Q"], p);
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
}