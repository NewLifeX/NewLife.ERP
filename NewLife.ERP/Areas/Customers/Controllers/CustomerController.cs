using Erp.Data.Customers;
using Microsoft.AspNetCore.Mvc;
using NewLife.Cube;
using NewLife.Data;
using NewLife.Web;
using XCode;

namespace NewLife.ERP.Areas.Customers.Controllers;

[CustomersArea]
[Menu(80)]
public class CustomerController : EntityController<Customer>
{
    static CustomerController() => LogOnChange = true;

    protected override IEnumerable<Customer> Search(Pager p)
    {
        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        return Customer.Search(start, end, p["Q"], p);
    }

    public ActionResult Search(String key = null)
    {
        var page = new PageParameter { PageSize = 20 };

        // 默认排序
        if (page.Sort.IsNullOrEmpty()) page.Sort = Customer._.Name;

        var list = Customer.Search(DateTime.MinValue, DateTime.MinValue, key, page);

        return Json(0, null, list.Select(e => new
        {
            e.Id,
            e.Name,
            e.FullName,
            e.Contact,
        }).ToArray());
    }
}