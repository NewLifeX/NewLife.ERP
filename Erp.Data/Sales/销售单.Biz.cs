﻿using System.Runtime.Serialization;
using System.Xml.Serialization;
using Erp.Data.Customers;
using Erp.Data.Models;
using Erp.Data.Purchases;
using NewLife;
using NewLife.Data;
using XCode;
using XCode.Membership;

namespace Erp.Data.Sales;

public partial class SaleOrder : Entity<SaleOrder>
{
    #region 对象操作
    static SaleOrder()
    {
        // 累加字段，生成 Update xx Set Count=Count+1234 Where xxx
        //var df = Meta.Factory.AdditionalFields;
        //df.Add(nameof(CustomerId));

        // 过滤器 UserModule、TimeModule、IPModule
        Meta.Modules.Add<UserModule>();
        Meta.Modules.Add<TimeModule>();
        Meta.Modules.Add<IPModule>();
    }

    /// <summary>验证并修补数据，通过抛出异常的方式提示验证失败。</summary>
    /// <param name="isNew">是否插入</param>
    public override void Valid(Boolean isNew)
    {
        //// 如果没有脏数据，则不需要进行任何处理
        //if (!HasDirty) return;

        if (CustomerId <= 0) throw new ArgumentNullException(nameof(CustomerId), "客户不能为空");

        // 建议先调用基类方法，基类方法会做一些统一处理
        base.Valid(isNew);

        // 在新插入数据或者修改了指定字段时进行修正
        // 货币保留6位小数
        Amount = Math.Round(Amount, 6);

        if (Status <= 0) Status = OrderStatus.录入中;

        if (OccurTime.Year < 2000) OccurTime = DateTime.Now;
        if (Title.IsNullOrEmpty()) Title = $"[{OccurTime:yyMMdd}]{CustomerName}的订单";

        var customer = Customer;
        if (customer != null)
        {
            var flag = Dirtys[nameof(CustomerId)];
            if (!isNew && flag || Receiver.IsNullOrEmpty()) Receiver = customer.Contact;
            if (!isNew && flag || Phone.IsNullOrEmpty()) Phone = customer.Phone;
            if (!isNew && flag || Address.IsNullOrEmpty()) Address = customer.Address;
            if (!isNew && flag || AreaCode == 0) AreaCode = customer.AreaCode;
        }
    }
    #endregion

    #region 扩展属性
    /// <summary>客户</summary>
    [XmlIgnore, IgnoreDataMember]
    //[ScriptIgnore]
    public Customer Customer => Extends.Get(nameof(Customer), k => Customer.FindById(CustomerId));

    /// <summary>客户</summary>
    [Map(nameof(CustomerId), typeof(Customer), "Id")]
    public String CustomerName => Customer?.Name;

    /// <summary>地区</summary>
    [Map(nameof(AreaCode))]
    public String AreaName => Area.FindByID(AreaCode)?.Path;
    #endregion

    #region 扩展查询
    /// <summary>根据编号查找</summary>
    /// <param name="id">编号</param>
    /// <returns>实体对象</returns>
    public static SaleOrder FindById(Int32 id)
    {
        if (id <= 0) return null;

        // 实体缓存
        if (Meta.Session.Count < 1000) return Meta.Cache.Find(e => e.Id == id);

        // 单对象缓存
        return Meta.SingleCache[id];

        //return Find(_.Id == id);
    }

    /// <summary>根据客户查找</summary>
    /// <param name="customerId">客户</param>
    /// <returns>实体列表</returns>
    public static IList<SaleOrder> FindAllByCustomerId(Int32 customerId)
    {
        // 实体缓存
        if (Meta.Session.Count < 1000) return Meta.Cache.FindAll(e => e.CustomerId == customerId);

        return FindAll(_.CustomerId == customerId);
    }
    #endregion

    #region 高级查询
    /// <summary>高级查询</summary>
    /// <param name="customerId">客户</param>
    /// <param name="productId">产品</param>
    /// <param name="status">状态</param>
    /// <param name="start">更新时间开始</param>
    /// <param name="end">更新时间结束</param>
    /// <param name="key">关键字</param>
    /// <param name="page">分页参数信息。可携带统计和数据权限扩展查询等信息</param>
    /// <returns>实体列表</returns>
    public static IList<SaleOrder> Search(Int32 customerId, Int32 productId, OrderStatus status, DateTime start, DateTime end, String key, PageParameter page)
    {
        var exp = new WhereExpression();

        if (customerId >= 0) exp &= _.CustomerId == customerId;
        if (productId > 0) exp &= _.Id.In(SaleOrderLine.SearchSql(productId, start, end));
        if (status > 0) exp &= _.Status == status;
        exp &= _.OccurTime.Between(start, end);
        if (!key.IsNullOrEmpty()) exp &= _.Title.Contains(key) | _.ContractNo.Contains(key) | _.BillCode.Contains(key) | _.Receiver.Contains(key) | _.CreateUser.Contains(key) | _.CreateIP.Contains(key) | _.UpdateUser.Contains(key) | _.UpdateIP.Contains(key) | _.Remark.Contains(key);

        return FindAll(exp, page);
    }

    // Select Count(Id) as Id,Category From SaleOrder Where CreateTime>'2020-01-24 00:00:00' Group By Category Order By Id Desc limit 20
    //static readonly FieldCache<SaleOrder> _CategoryCache = new FieldCache<SaleOrder>(nameof(Category))
    //{
    //Where = _.CreateTime > DateTime.Today.AddDays(-30) & Expression.Empty
    //};

    ///// <summary>获取类别列表，字段缓存10分钟，分组统计数据最多的前20种，用于魔方前台下拉选择</summary>
    ///// <returns></returns>
    //public static IDictionary<String, String> GetCategoryList() => _CategoryCache.FindAllName();
    #endregion

    #region 业务操作
    /// <summary>修正关联数据</summary>
    public void Fix()
    {
        // 明细
        var list = SaleOrderLine.FindAllByOrderId(Id);
        if (list.Count > 0)
        {
            foreach (var item in list)
            {
                if (item.Fix(this))
                    item.Update();
            }

            Quantity = list.Sum(e => e.Quantity);
            Amount = list.Sum(e => e.Amount) + Freight;

            if (Title.IsNullOrEmpty() || Title.StartsWith("["))
            {
                var txt = $"[{OccurTime:yyMMdd}]" + list.OrderByDescending(e => e.Amount).Join("、", e => e.ProductName);
                Title = txt.Cut(25);
            }
        }
    }

    public void Clone(SaleOrder order)
    {
        CustomerId = order.CustomerId;
        Title = order.Title;
        Quantity = order.Quantity;
        Amount = order.Amount;
        Status = order.Status;
        OccurTime = order.OccurTime;
        ContractNo = order.ContractNo;
        Payment = order.Payment;
        Operator = order.Operator;
        Invoice = order.Invoice;
        //BillCode = order.BillCode;
        Freight = order.Freight;
        //Receiver = order.Receiver;
        //Phone = order.Phone;
        //Address = order.Address;
        Remark = order.Remark;
    }
    #endregion
}