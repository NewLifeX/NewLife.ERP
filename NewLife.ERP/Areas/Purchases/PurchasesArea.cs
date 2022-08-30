using System;
using System.ComponentModel;
using NewLife.Cube;

namespace NewLife.ERP.Areas.Purchases;

[DisplayName("采购中心")]
public class PurchasesArea : AreaBase
{
    public PurchasesArea() : base(nameof(PurchasesArea).TrimEnd("Area")) { }

    static PurchasesArea() => RegisterArea<PurchasesArea>();
}