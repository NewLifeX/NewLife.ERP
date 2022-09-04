﻿using Erp.Data.Models;
using Erp.Data.Purchases;
using System.ComponentModel;
using Erp.Data.Sales;
using Microsoft.AspNetCore.Mvc;
using NewLife.Cube;
using NewLife.ERP.Services;
using NewLife.Web;
using XCode.Membership;
using XCode;

namespace NewLife.ERP.Areas.Sales.Controllers;

[SalesArea]
[Menu(70)]
public class SaleOrderController : EntityController<SaleOrder>
{
    private readonly SaleService _saleService;

    static SaleOrderController()
    {
        LogOnChange = true;

        ListFields.RemoveField("ContractNo", "BillCode");
        ListFields.RemoveCreateField();
        ListFields.RemoveRemarkField();

        {
            var df = ListFields.AddListField("Lines", "OccurTime");
            df.DisplayName = "订单明细";
            df.Url = "SaleOrderLine?orderId={Id}";
        }
        {
            var df = ListFields.AddListField("History", "OccurTime");
            df.DisplayName = "历史";
            df.Url = "SaleOrderHistory?orderId={Id}";
        }

        AddFormFields.RemoveField("Status");
        AddFormFields.RemoveField("Title", "Quantity", "Price");

        EditFormFields.RemoveField("Status");
        EditFormFields.RemoveField("Title", "Quantity", "Price");
    }

    public SaleOrderController(SaleService saleService)
    {
        _saleService = saleService;
    }

    protected override IEnumerable<SaleOrder> Search(Pager p)
    {
        var id = p["Id"].ToInt(-1);
        if (id > 0)
        {
            var entity = SaleOrder.FindById(id);
            if (entity != null) return new[] { entity };
        }

        var customerId = p["customerId"].ToInt(-1);

        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        p.RetrieveState = true;

        return SaleOrder.Search(customerId, start, end, p["Q"], p);
    }

    static String[] _protects = new[] { "CustomerId", "OccurTime" };
    protected override Boolean Valid(SaleOrder entity, DataObjectMethodType type, Boolean post)
    {
        if (post)
        {
            switch (type)
            {
                case DataObjectMethodType.Update:
                    var order = entity as IEntity;
                    if (order.Dirtys.Any(d => d.EqualIgnoreCase(_protects)))
                    {
                        if (entity.Status != OrderStatus.录入) throw new InvalidOperationException("该状态下订单禁止修改！");
                    }
                    break;
                case DataObjectMethodType.Delete:
                    if (entity.Status != OrderStatus.录入) throw new InvalidOperationException("该状态下订单删除修改！");
                    break;
            }
        }

        return base.Valid(entity, type, post);
    }

    protected override Int32 OnUpdate(SaleOrder entity)
    {
        entity.Fix();

        return base.OnUpdate(entity);
    }

    /// <summary>批量出库</summary>
    /// <returns></returns>
    [EntityAuthorize(PermissionFlags.Update)]
    public ActionResult SetOut()
    {
        var count = 0;
        var ids = GetRequest("keys").SplitAsInt();
        if (ids.Length > 0)
        {
            using var tran = SaleOrder.Meta.CreateTrans();

            foreach (var id in ids)
            {
                var entity = SaleOrder.FindById(id);
                if (entity != null)
                    count += _saleService.SetOut(entity);
            }

            tran.Commit();
        }

        return JsonRefresh($"共处理[{count}]个订单");
    }

    /// <summary>批量取消出库</summary>
    /// <returns></returns>
    [EntityAuthorize(PermissionFlags.Update)]
    public ActionResult CancelOut()
    {
        var count = 0;
        var ids = GetRequest("keys").SplitAsInt();
        if (ids.Length > 0)
        {
            using var tran = SaleOrder.Meta.CreateTrans();

            foreach (var id in ids)
            {
                var entity = SaleOrder.FindById(id);
                if (entity != null)
                    count += _saleService.CancelOut(entity);
            }

            tran.Commit();
        }

        return JsonRefresh($"共处理[{count}]个订单");
    }
}