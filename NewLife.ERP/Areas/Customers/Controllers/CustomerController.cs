using Erp.Data.Customers;
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
    }

    protected override IEnumerable<Customer> Search(Pager p)
    {
        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        return Customer.Search(start, end, p["Q"], p);
    }
}