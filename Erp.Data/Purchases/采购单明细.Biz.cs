using System.Runtime.Serialization;
using System.Xml.Serialization;
using Erp.Data.Products;
using NewLife;
using NewLife.Data;
using XCode;
using XCode.DataAccessLayer;
using XCode.Membership;

namespace Erp.Data.Purchases;

public partial class PurchaseOrderLine : Entity<PurchaseOrderLine>
{
    #region 对象操作
    static PurchaseOrderLine()
    {
        // 累加字段，生成 Update xx Set Count=Count+1234 Where xxx
        //var df = Meta.Factory.AdditionalFields;
        //df.Add(nameof(OrderId));

        // 过滤器 UserModule、TimeModule、IPModule
        Meta.Modules.Add<UserModule>();
        Meta.Modules.Add<TimeModule>();
        Meta.Modules.Add<IPModule>();

        Meta.Factory.SelectStat = _.Quantity.Sum() & "Sum(Quantity*Price) as Price";
    }

    /// <summary>验证并修补数据，通过抛出异常的方式提示验证失败。</summary>
    /// <param name="isNew">是否插入</param>
    public override void Valid(Boolean isNew)
    {
        // 如果没有脏数据，则不需要进行任何处理
        if (!HasDirty) return;

        if (ProductId <= 0) throw new ArgumentNullException(nameof(ProductId), "产品不能为空");
        if (Quantity <= 0) throw new ArgumentNullException(nameof(Quantity), "数量不能为空");

        // 建议先调用基类方法，基类方法会做一些统一处理
        base.Valid(isNew);

        // 在新插入数据或者修改了指定字段时进行修正
        // 货币保留6位小数
        Price = Math.Round(Price, 6);

        var order = Order;
        if (order != null)
        {
            if (OccurTime.Year < 2000) OccurTime = order.OccurTime;
        }
    }
    #endregion

    #region 扩展属性
    /// <summary>订单</summary>
    [XmlIgnore, IgnoreDataMember]
    //[ScriptIgnore]
    public PurchaseOrder Order => Extends.Get(nameof(Order), k => PurchaseOrder.FindById(OrderId));

    /// <summary>订单</summary>
    [Map(nameof(OrderId), typeof(PurchaseOrder), "Id")]
    public String OrderTitle => Order?.Title;

    /// <summary>产品</summary>
    [XmlIgnore, IgnoreDataMember]
    //[ScriptIgnore]
    public Product Product => Extends.Get(nameof(Product), k => Product.FindById(ProductId));

    /// <summary>产品</summary>
    [Map(nameof(ProductId), typeof(Product), "Id")]
    public String ProductName => Product?.Name;
    #endregion

    #region 扩展查询
    /// <summary>根据编号查找</summary>
    /// <param name="id">编号</param>
    /// <returns>实体对象</returns>
    public static PurchaseOrderLine FindById(Int32 id)
    {
        if (id <= 0) return null;

        // 实体缓存
        if (Meta.Session.Count < 1000) return Meta.Cache.Find(e => e.Id == id);

        // 单对象缓存
        return Meta.SingleCache[id];

        //return Find(_.Id == id);
    }

    /// <summary>根据订单查找</summary>
    /// <param name="orderId">订单</param>
    /// <returns>实体列表</returns>
    public static IList<PurchaseOrderLine> FindAllByOrderId(Int32 orderId)
    {
        // 实体缓存
        if (Meta.Session.Count < 1000) return Meta.Cache.FindAll(e => e.OrderId == orderId);

        return FindAll(_.OrderId == orderId);
    }

    /// <summary>根据产品查找</summary>
    /// <param name="productId">产品</param>
    /// <returns>实体列表</returns>
    public static IList<PurchaseOrderLine> FindAllByProductId(Int32 productId)
    {
        // 实体缓存
        if (Meta.Session.Count < 1000) return Meta.Cache.FindAll(e => e.ProductId == productId);

        return FindAll(_.ProductId == productId);
    }
    #endregion

    #region 高级查询
    /// <summary>高级查询</summary>
    /// <param name="orderId">订单</param>
    /// <param name="productId">产品</param>
    /// <param name="start">更新时间开始</param>
    /// <param name="end">更新时间结束</param>
    /// <param name="key">关键字</param>
    /// <param name="page">分页参数信息。可携带统计和数据权限扩展查询等信息</param>
    /// <returns>实体列表</returns>
    public static IList<PurchaseOrderLine> Search(Int32 orderId, Int32 productId, DateTime start, DateTime end, String key, PageParameter page)
    {
        var exp = new WhereExpression();

        if (orderId >= 0) exp &= _.OrderId == orderId;
        if (productId >= 0) exp &= _.ProductId == productId;
        exp &= _.OccurTime.Between(start, end);
        if (!key.IsNullOrEmpty()) exp &= _.CreateUser.Contains(key) | _.CreateIP.Contains(key) | _.UpdateUser.Contains(key) | _.UpdateIP.Contains(key) | _.Remark.Contains(key);

        return FindAll(exp, page);
    }

    public static SelectBuilder SearchSql(Int32 productId, DateTime start, DateTime end)
    {
        var exp = new WhereExpression();

        if (productId >= 0) exp &= _.ProductId == productId;
        exp &= _.OccurTime.Between(start, end);

        return FindSQL(exp, null, _.OrderId);
    }

    // Select Count(Id) as Id,Category From PurchaseOrderItem Where CreateTime>'2020-01-24 00:00:00' Group By Category Order By Id Desc limit 20
    //static readonly FieldCache<PurchaseOrderItem> _CategoryCache = new FieldCache<PurchaseOrderItem>(nameof(Category))
    //{
    //Where = _.CreateTime > DateTime.Today.AddDays(-30) & Expression.Empty
    //};

    ///// <summary>获取类别列表，字段缓存10分钟，分组统计数据最多的前20种，用于魔方前台下拉选择</summary>
    ///// <returns></returns>
    //public static IDictionary<String, String> GetCategoryList() => _CategoryCache.FindAllName();
    #endregion

    #region 业务操作
    #endregion
}