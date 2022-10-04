using System.ComponentModel;
using Erp.Data.Models;
using Erp.Data.Purchases;
using Erp.Data.Sales;
using Microsoft.AspNetCore.Mvc;
using NewLife.Cube;
using NewLife.Cube.ViewModels;
using NewLife.ERP.Services;
using NewLife.Web;
using XCode;
using XCode.Membership;

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
            var df = ListFields.GetField("CustomerName") as ListField;
            //df.DisplayName = "客户";
            df.Url = "/Customers/Customer?Id={CustomerId}";
        }
        {
            var df = ListFields.AddListField("Lines", "OccurTime");
            df.DisplayName = "订单明细";
            df.Url = "/Sales/SaleOrderLine?orderId={Id}";
        }
        {
            var df = ListFields.AddListField("InvoicePage1", "OccurTime");
            df.DisplayName = "发货单";
            df.Url = "/Sales/SaleOrder/Invoice?Id={Id}&money=1";
            df.Target = "_blank";
        }
        {
            var df = ListFields.AddListField("InvoicePage2", "OccurTime");
            df.DisplayName = "发货单（无价格）";
            df.Url = "/Sales/SaleOrder/Invoice?Id={Id}&money=0";
            df.Target = "_blank";
        }
        {
            var df = ListFields.AddListField("History", "OccurTime");
            df.DisplayName = "历史";
            df.Url = "/Sales/SaleOrderHistory?orderId={Id}";
        }
        {
            var df = ListFields.AddListField("CloneOrder", "OccurTime");
            df.DisplayName = "克隆订单";
            df.HeaderTitle = df.Title = "克隆该订单，用于重复出货给不同客户";
            df.Url = "/Sales/SaleOrder/CloneOrder?Id={Id}";
        }
        {
            var df = ListFields.GetField("OccurTime") as ListField;
            df.GetValue = e => (e as SaleOrder).OccurTime.ToString("yyyy-MM-dd");
        }

        AddFormFields.RemoveField("Status");
        AddFormFields.RemoveField("Quantity", "Price");

        EditFormFields.RemoveField("Status");
        EditFormFields.RemoveField("Quantity", "Price");
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
        var productId = p["productId"].ToInt(-1);
        var status = (OrderStatus)p["status"].ToInt();

        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        p.RetrieveState = true;

        return SaleOrder.Search(customerId, productId, status, start, end, p["Q"], p);
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
                        if (entity.Status != OrderStatus.录入中) throw new InvalidOperationException("该状态下订单禁止修改！");
                    }
                    break;
                case DataObjectMethodType.Delete:
                    if (entity.Status != OrderStatus.录入中) throw new InvalidOperationException("该状态下订单删除修改！");
                    break;
            }
        }
        else
        {
            switch (type)
            {
                case DataObjectMethodType.Insert:
                    var customer = entity.Customer;
                    if (customer != null)
                    {
                        if (entity.Title.IsNullOrEmpty()) entity.Title = $"[{DateTime.Today:yyMMdd}]{entity.CustomerName}的订单";

                        if (entity.Receiver.IsNullOrEmpty()) entity.Receiver = customer.Contact;
                        if (entity.Phone.IsNullOrEmpty()) entity.Phone = customer.Phone;
                        if (entity.Address.IsNullOrEmpty()) entity.Address = customer.Address;
                    }
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
    [EntityAuthorize((PermissionFlags)16)]
    [DisplayName("出库")]
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
    [EntityAuthorize((PermissionFlags)32)]
    [DisplayName("取消出库")]
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

    /// <summary>克隆订单</summary>
    /// <returns></returns>
    [EntityAuthorize(PermissionFlags.Insert)]
    public ActionResult CloneOrder(Int32 id)
    {
        var order = SaleOrder.FindById(id);
        if (order == null) throw new ArgumentNullException(nameof(id));

        using var tran = SaleOrder.Meta.CreateTrans();

        var order2 = new SaleOrder();
        order2.Clone(order);
        order2.Status = OrderStatus.录入中;
        order2.Insert();

        foreach (var item in SaleOrderLine.FindAllByOrderId(order.Id))
        {
            var line = new SaleOrderLine();
            line.Clone(item);
            line.OrderId = order2.Id;
            line.Insert();
        }

        tran.Commit();

        return RedirectToAction("Edit", new { id = order2.Id });
    }

    /// <summary>批量修正数据</summary>
    /// <returns></returns>
    [EntityAuthorize(PermissionFlags.Update)]
    public ActionResult Fix()
    {
        var count = 0;
        var ids = GetRequest("keys").SplitAsInt();
        if (ids.Length > 0)
        {
            foreach (var id in ids)
            {
                var entity = SaleOrder.FindById(id);
                if (entity != null)
                {
                    entity.Fix();
                    count += entity.Update();
                }
            }
        }

        return JsonRefresh($"共处理[{count}]个订单");
    }

    /// <summary>发货单</summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [EntityAuthorize(PermissionFlags.Detail)]
    public ActionResult Invoice(Int32 id)
    {
        var entity = SaleOrder.FindById(id);

        var set = PageSetting;
        set.EnableNavbar = false;
        set.EnableFooter = false;

        return View(entity);
    }
}