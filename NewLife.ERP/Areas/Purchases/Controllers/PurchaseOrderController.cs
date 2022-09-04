﻿using Erp.Data.Models;
using System.ComponentModel;
using Erp.Data.Purchases;
using Microsoft.AspNetCore.Mvc;
using NewLife.Cube;
using NewLife.ERP.Services;
using NewLife.Web;
using XCode.Membership;
using XCode;
using NewLife.Cube.ViewModels;

namespace NewLife.ERP.Areas.Purchases.Controllers;

[PurchasesArea]
[Menu(70)]
public class PurchaseOrderController : EntityController<PurchaseOrder>
{
    private readonly PurchaseService _purchaseService;

    static PurchaseOrderController()
    {
        LogOnChange = true;

        ListFields.RemoveField("ContractNo", "BillCode");
        ListFields.RemoveCreateField();
        ListFields.RemoveRemarkField();

        {
            var df = ListFields.GetField("SupplierName") as ListField;
            df.Url = "/Purchases/Supplier?Id={SupplierId}";
        }
        {
            var df = ListFields.GetField("WarehouseName") as ListField;
            df.Url = "/Products/Warehouse?Id={WarehouseId}";
        }
        {
            var df = ListFields.AddListField("Lines", "OccurTime");
            df.DisplayName = "订单明细";
            df.Url = "/Purchases/PurchaseOrderLine?orderId={Id}";
        }
        {
            var df = ListFields.AddListField("History", "OccurTime");
            df.DisplayName = "历史";
            df.Url = "/Purchases/PurchaseOrderHistory?orderId={Id}";
        }

        AddFormFields.RemoveField("Status");
        AddFormFields.RemoveField("Quantity", "Price");

        EditFormFields.RemoveField("Status");
        EditFormFields.RemoveField("Quantity", "Price");
    }

    public PurchaseOrderController(PurchaseService purchaseService)
    {
        _purchaseService = purchaseService;
    }

    protected override IEnumerable<PurchaseOrder> Search(Pager p)
    {
        var id = p["Id"].ToInt(-1);
        if (id > 0)
        {
            var entity = PurchaseOrder.FindById(id);
            if (entity != null) return new[] { entity };
        }

        var supplierId = p["supplierId"].ToInt(-1);
        var warehouseId = p["warehouseId"].ToInt(-1);

        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        p.RetrieveState = true;

        return PurchaseOrder.Search(supplierId, warehouseId, start, end, p["Q"], p);
    }

    static String[] _protects = new[] { "SupplierId", "WarehouseId", "OccurTime" };
    protected override Boolean Valid(PurchaseOrder entity, DataObjectMethodType type, Boolean post)
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
                case DataObjectMethodType.Insert:
                    if (entity.Receiver.IsNullOrEmpty()) entity.Receiver = ManageProvider.User + "";
                    break;
            }
        }

        return base.Valid(entity, type, post);
    }

    protected override Int32 OnUpdate(PurchaseOrder entity)
    {
        entity.Fix();

        return base.OnUpdate(entity);
    }

    /// <summary>批量入库</summary>
    /// <returns></returns>
    [EntityAuthorize(PermissionFlags.Update)]
    public ActionResult SetIn()
    {
        var count = 0;
        var ids = GetRequest("keys").SplitAsInt();
        if (ids.Length > 0)
        {
            using var tran = PurchaseOrder.Meta.CreateTrans();

            foreach (var id in ids)
            {
                var entity = PurchaseOrder.FindById(id);
                if (entity != null)
                    count += _purchaseService.SetIn(entity);
            }

            tran.Commit();
        }

        return JsonRefresh($"共处理[{count}]个订单");
    }

    /// <summary>批量取消入库</summary>
    /// <returns></returns>
    [EntityAuthorize(PermissionFlags.Update)]
    public ActionResult CancelIn()
    {
        var count = 0;
        var ids = GetRequest("keys").SplitAsInt();
        if (ids.Length > 0)
        {
            using var tran = PurchaseOrder.Meta.CreateTrans();

            foreach (var id in ids)
            {
                var entity = PurchaseOrder.FindById(id);
                if (entity != null)
                    count += _purchaseService.CancelIn(entity);
            }

            tran.Commit();
        }

        return JsonRefresh($"共处理[{count}]个订单");
    }
}