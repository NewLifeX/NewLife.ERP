using System.Runtime.Serialization;
using System.Xml.Serialization;
using Erp.Data.Models;
using Erp.Data.Products;
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
        // 如果没有脏数据，则不需要进行任何处理
        if (!HasDirty) return;

        // 建议先调用基类方法，基类方法会做一些统一处理
        base.Valid(isNew);

        // 在新插入数据或者修改了指定字段时进行修正
        // 货币保留6位小数
        Price = Math.Round(Price, 6);

        if (Status <= 0) Status = OrderStatus.录入;

        if (OccurTime.Year < 2000) OccurTime = DateTime.Now;
    }
    #endregion

    #region 扩展属性
    /// <summary>仓库</summary>
    [XmlIgnore, IgnoreDataMember]
    //[ScriptIgnore]
    public Warehouse Warehouse => Extends.Get(nameof(Warehouse), k => Warehouse.FindById(WarehouseId));

    /// <summary>仓库</summary>
    [Map(nameof(WarehouseId), typeof(Warehouse), "Id")]
    public String WarehouseName => Warehouse?.Name;
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
    /// <param name="start">更新时间开始</param>
    /// <param name="end">更新时间结束</param>
    /// <param name="key">关键字</param>
    /// <param name="page">分页参数信息。可携带统计和数据权限扩展查询等信息</param>
    /// <returns>实体列表</returns>
    public static IList<SaleOrder> Search(Int32 customerId, DateTime start, DateTime end, String key, PageParameter page)
    {
        var exp = new WhereExpression();

        if (customerId >= 0) exp &= _.CustomerId == customerId;
        exp &= _.UpdateTime.Between(start, end);
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
            Quantity = list.Sum(e => e.Quantity);
            Price = list.Sum(e => e.Quantity * e.Price);

            if (Title.IsNullOrEmpty())
            {
                var txt = list.Join("、", e => e.ProductName);
                Title = txt.Cut(50);
            }
        }
    }
    #endregion
}