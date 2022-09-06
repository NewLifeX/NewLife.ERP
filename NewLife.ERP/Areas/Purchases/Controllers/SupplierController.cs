using Erp.Data.Purchases;
using Microsoft.AspNetCore.Mvc;
using NewLife.Cube;
using NewLife.Data;
using NewLife.Web;

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

    public ActionResult Search(String key = null)
    {
        var page = new PageParameter { PageSize = 20 };
        var list = Supplier.Search(DateTime.MinValue, DateTime.MinValue, key, page);

        return Json(0, null, list.Select(e => new
        {
            e.Id,
            e.Name,
            e.FullName,
            e.Contact,
        }).ToArray());
    }
}