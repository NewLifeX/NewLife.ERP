using System;
using System.ComponentModel;
using NewLife;
using NewLife.Cube;

namespace NewLife.ERP.Areas.Customers
{
    [DisplayName("客户中心")]
    public class CustomersArea : AreaBase
    {
        public CustomersArea() : base(nameof(CustomersArea).TrimEnd("Area")) { }

        static CustomersArea() => RegisterArea<CustomersArea>();
    }
}