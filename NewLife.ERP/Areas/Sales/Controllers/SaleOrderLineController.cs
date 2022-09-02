﻿using Erp.Data.Purchases;
using System.ComponentModel;
using Erp.Data.Sales;
using NewLife.Cube;
using NewLife.Web;

namespace NewLife.ERP.Areas.Sales.Controllers;

[SalesArea]
[Menu(0, false)]
public class SaleOrderLineController : EntityController<SaleOrderLine>
{
    protected override IEnumerable<SaleOrderLine> Search(Pager p)
    {
        var orderId = p["orderId"].ToInt(-1);
        var productId = p["productId"].ToInt(-1);

        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        p.RetrieveState = true;

        return SaleOrderLine.Search(orderId, productId, start, end, p["Q"], p);
    }

    protected override Boolean Valid(SaleOrderLine entity, DataObjectMethodType type, Boolean post)
    {
        if (post)
        {
            if (entity.Price <= 0 && entity.Product != null) entity.Price = entity.Product.Price;
        }

        return base.Valid(entity, type, post);
    }

    protected override Int32 OnInsert(SaleOrderLine entity)
    {
        var rs = base.OnInsert(entity);

        var order = entity.Order;
        if (order != null)
        {
            order.Fix();
            order.Update();
        }

        var prd = entity.Product;
        if (prd != null)
        {
            prd.Fix();
            prd.Update();
        }

        return rs;
    }

    protected override Int32 OnUpdate(SaleOrderLine entity)
    {
        var rs = base.OnUpdate(entity);

        var order = entity.Order;
        if (order != null)
        {
            order.Fix();
            order.Update();
        }

        var prd = entity.Product;
        if (prd != null)
        {
            prd.Fix();
            prd.Update();
        }

        return rs;
    }
}