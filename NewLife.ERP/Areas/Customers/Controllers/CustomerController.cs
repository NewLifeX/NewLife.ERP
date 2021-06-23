using Erp.Data.Customers;
using NewLife.Cube;
using NewLife.Web;
using System;
using System.Collections.Generic;

namespace NewLife.ERP.Areas.Customers.Controllers
{
    [CustomersArea]
    public class CustomerController : EntityController<Customer>
    {
        static CustomerController() => MenuOrder = 80;

        protected override IEnumerable<Customer> Search(Pager p)
        {
            var start = p["dtStart"].ToDateTime();
            var end = p["dtEnd"].ToDateTime();

            return Customer.Search(start, end, p["Q"], p);
        }
    }
}