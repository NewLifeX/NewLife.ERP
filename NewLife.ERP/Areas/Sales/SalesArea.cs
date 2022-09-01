using System;
using System.ComponentModel;
using NewLife.Cube;

namespace NewLife.ERP.Areas.Sales;

[DisplayName("销售中心")]
public class SalesArea : AreaBase
{
    public SalesArea() : base(nameof(SalesArea).TrimEnd("Area")) { }

    static SalesArea() => RegisterArea<SalesArea>();
}