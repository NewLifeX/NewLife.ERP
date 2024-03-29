﻿using System.ComponentModel;
using NewLife;
using NewLife.Data;
using NewLife.Log;
using XCode;
using XCode.Membership;

namespace Erp.Data.Customers;

/// <summary>客户。客户中心的一切资源，以客户为中心</summary>
public partial class Customer : Entity<Customer>
{
    #region 对象操作
    static Customer()
    {
        // 累加字段，生成 Update xx Set Count=Count+1234 Where xxx
        //var df = Meta.Factory.AdditionalFields;
        //df.Add(nameof(CreateUserID));

        // 过滤器 UserModule、TimeModule、IPModule
        Meta.Modules.Add<UserModule>();
        Meta.Modules.Add<TimeModule>();
        Meta.Modules.Add<IPModule>();

        // 单对象缓存
        var sc = Meta.SingleCache;
        sc.FindSlaveKeyMethod = k => Find(_.Name == k);
        sc.GetSlaveKeyMethod = e => e.Name;
    }

    /// <summary>验证并修补数据，通过抛出异常的方式提示验证失败。</summary>
    /// <param name="isNew">是否插入</param>
    public override void Valid(Boolean isNew)
    {
        // 如果没有脏数据，则不需要进行任何处理
        if (!HasDirty) return;

        // 这里验证参数范围，建议抛出参数异常，指定参数名，前端用户界面可以捕获参数异常并聚焦到对应的参数输入框
        if (Name.IsNullOrEmpty()) throw new ArgumentNullException(nameof(Name), "名称不能为空！");

        // 建议先调用基类方法，基类方法会做一些统一处理
        base.Valid(isNew);

        var ts = Tags?.Split(",", "，", ";", "；");
        if (ts != null && ts.Length > 0) Tags = $",{ts.Join(",")},";

        if (PinYin.IsNullOrEmpty() || Dirtys[nameof(Name)]) PinYin = NewLife.Common.PinYin.GetFirst(Name);
        if (PinYin2.IsNullOrEmpty() || Dirtys[nameof(Name)] || Dirtys[nameof(FullName)]) PinYin2 = NewLife.Common.PinYin.Get(FullName ?? Name);
    }

    /// <summary>首次连接数据库时初始化数据，仅用于实体类重载，用户不应该调用该方法</summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected override void InitData()
    {
        // InitData一般用于当数据表没有数据时添加一些默认数据，该实体类的任何第一次数据库操作都会触发该方法，默认异步调用
        if (Meta.Session.Count > 0) return;

        if (XTrace.Debug) XTrace.WriteLine("开始初始化Customer[客户]数据……");

        var entity = new Customer
        {
            Name = "默认客户",
            Enable = true,
        };
        entity.Insert();

        if (XTrace.Debug) XTrace.WriteLine("完成初始化Customer[客户]数据！");
    }
    #endregion

    #region 扩展属性
    /// <summary>地区</summary>
    [Map(nameof(AreaCode))]
    public String AreaName => Area.FindByID(AreaCode)?.Path;
    #endregion

    #region 扩展查询
    /// <summary>根据编号查找</summary>
    /// <param name="id">编号</param>
    /// <returns>实体对象</returns>
    public static Customer FindById(Int32 id)
    {
        if (id <= 0) return null;

        // 实体缓存
        if (Meta.Session.Count < 1000) return Meta.Cache.Find(e => e.Id == id);

        // 单对象缓存
        return Meta.SingleCache[id];

        //return Find(_.Id == id);
    }

    /// <summary>根据名称查找</summary>
    /// <param name="name">名称</param>
    /// <returns>实体对象</returns>
    public static Customer FindByName(String name)
    {
        // 实体缓存
        if (Meta.Session.Count < 1000) return Meta.Cache.Find(e => e.Name.EqualIgnoreCase(name));

        // 单对象缓存
        //return Meta.SingleCache.GetItemWithSlaveKey(name) as Customer;

        return Find(_.Name == name);
    }
    #endregion

    #region 高级查询
    /// <summary>高级查询</summary>
    /// <param name="name">名称</param>
    /// <param name="areaCode">地区</param>
    /// <param name="start">更新时间开始</param>
    /// <param name="end">更新时间结束</param>
    /// <param name="key">关键字</param>
    /// <param name="page">分页参数信息。可携带统计和数据权限扩展查询等信息</param>
    /// <returns>实体列表</returns>
    public static IList<Customer> Search(String name, Int32 areaCode, DateTime start, DateTime end, String key, PageParameter page)
    {
        var exp = new WhereExpression();

        if (!name.IsNullOrEmpty()) exp &= _.Name == name;
        if (areaCode > 0)
        {
            if (areaCode > 999999)
                exp &= _.AreaCode == areaCode;
            else
            {
                var endCode = areaCode + 1;
                if (areaCode % 10000 == 0)
                    endCode = areaCode + 10000;
                else if (areaCode % 100 == 0)
                    endCode = areaCode + 100;
                exp &= _.AreaCode >= areaCode & _.AreaCode < endCode;
            }
        }
        exp &= _.UpdateTime.Between(start, end);
        if (!key.IsNullOrEmpty()) exp &= _.Name.Contains(key) | _.FullName.Contains(key) | _.Tags.Contains($",{key},") | _.PinYin.Contains(key) | _.PinYin2.Contains(key) | _.Contact.Contains(key) | _.Phone.Contains(key) | _.Address.Contains(key) | _.Remark.Contains(key);

        return FindAll(exp, page);
    }

    // Select Count(Id) as Id,Category From Customer Where CreateTime>'2020-01-24 00:00:00' Group By Category Order By Id Desc limit 20
    //static readonly FieldCache<Customer> _CategoryCache = new FieldCache<Customer>(nameof(Category))
    //{
    //Where = _.CreateTime > DateTime.Today.AddDays(-30) & Expression.Empty
    //};

    ///// <summary>获取类别列表，字段缓存10分钟，分组统计数据最多的前20种，用于魔方前台下拉选择</summary>
    ///// <returns></returns>
    //public static IDictionary<String, String> GetCategoryList() => _CategoryCache.FindAllName();
    #endregion

    #region 业务操作
    public void Fix()
    {

    }
    #endregion
}