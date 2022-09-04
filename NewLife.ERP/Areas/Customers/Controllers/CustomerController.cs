using Erp.Data.Customers;
using Erp.Data.Purchases;
using NewLife.Cube;
using NewLife.Web;

namespace NewLife.ERP.Areas.Customers.Controllers;

[CustomersArea]
[Menu(80)]
public class CustomerController : EntityController<Customer>
{
    static CustomerController()
    {
        LogOnChange = true;

        ListFields.RemoveRemarkField();

        {
            var df = ListFields.AddListField("Order", "CreateUser");
            df.DisplayName = "销售单";
            df.Url = "/Sales/SaleOrder?customerId={Id}";
        }
        {
            var df = ListFields.AddListField("Log", "CreateUser");
            df.DisplayName = "日志";
            df.Url = "/Admin/Log?category=供应商&linkId={Id}";
        }
    }

    protected override IEnumerable<Customer> Search(Pager p)
    {
        var id = p["Id"].ToInt(-1);
        if (id > 0)
        {
            var entity = Customer.FindById(id);
            if (entity != null) return new[] { entity };
        }

        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        return Customer.Search(start, end, p["Q"], p);
    }
}