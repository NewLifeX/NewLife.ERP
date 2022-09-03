using Erp.Data.Purchases;
using NewLife.Cube;
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
            var df = ListFields.AddListField("Order", "CreateUser");
            df.DisplayName = "采购单";
            df.Url = "PurchaseOrder?supplierId={Id}";
        }
        {
            var df = ListFields.AddListField("Log", "CreateUser");
            df.DisplayName = "日志";
            df.Url = "/Admin/Log?category=供应商&linkId={Id}";
        }
    }

    protected override IEnumerable<Supplier> Search(Pager p)
    {
        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        return Supplier.Search(start, end, p["Q"], p);
    }
}