using Erp.Data.Customers;
using Erp.Data.Sales;
using Microsoft.AspNetCore.Mvc;
using NewLife.Cube;
using NewLife.Data;
using NewLife.ERP.Services;
using NewLife.Web;
using XCode.Membership;

namespace NewLife.ERP.Areas.Customers.Controllers;

[CustomersArea]
[Menu(80)]
public class CustomerController : EntityController<Customer>
{
    private readonly CustomerService _customerService;

    static CustomerController()
    {
        LogOnChange = true;

        ListFields.RemoveField("PinYin", "PinYin2");
        ListFields.RemoveCreateField();
        ListFields.RemoveRemarkField();

        {
            var df = ListFields.AddListField("Order", "UpdateUser");
            df.DisplayName = "销售单";
            df.Url = "/Sales/SaleOrder?customerId={Id}";
        }
        {
            var df = ListFields.AddListField("Log", "UpdateUser");
            df.DisplayName = "日志";
            df.Url = "/Admin/Log?category=供应商&linkId={Id}";
        }
    }

    public CustomerController(CustomerService customerService)
    {
        _customerService = customerService;
    }

    protected override IEnumerable<Customer> Search(Pager p)
    {
        var id = p["Id"].ToInt(-1);
        if (id > 0)
        {
            var entity = Customer.FindById(id);
            if (entity != null) return new[] { entity };
        }

        var rids = p["areaId"].SplitAsInt("/");
        var areaCode = (rids == null || rids.Length == 0) ? 0 : rids[^1];

        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        return Customer.Search(null, areaCode, start, end, p["Q"], p);
    }

    public ActionResult Search(String key = null)
    {
        var page = new PageParameter { PageSize = 20 };
        var list = Customer.Search(DateTime.MinValue, DateTime.MinValue, key, page);

        return Json(0, null, list.Select(e => new
        {
            e.Id,
            e.Name,
            e.FullName,
            e.Contact,
        }).ToArray());
    }

    /// <summary>批量修正数据</summary>
    /// <returns></returns>
    [EntityAuthorize(PermissionFlags.Update)]
    public ActionResult Fix()
    {
        var count = 0;
        var ids = GetRequest("keys").SplitAsInt();
        if (ids.Length > 0)
        {
            foreach (var id in ids)
            {
                var entity = Customer.FindById(id);
                if (entity != null)
                {
                    //entity.Fix();
                    //count += entity.Update();
                    count += _customerService.Fix(entity);
                }
            }
        }

        return JsonRefresh($"共处理[{count}]个");
    }
}