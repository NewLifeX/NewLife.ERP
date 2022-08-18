using System.Runtime.Serialization;
using System.Xml.Serialization;
using Erp.Data.Models;
using NewLife;
using NewLife.Data;
using XCode;
using XCode.Membership;

namespace Erp.Data.Products;

public partial class StockHistory : Entity<StockHistory>
{
    #region 对象操作
    static StockHistory()
    {
        Meta.Table.DataTable.InsertOnly = true;

        // 累加字段，生成 Update xx Set Count=Count+1234 Where xxx
        //var df = Meta.Factory.AdditionalFields;
        //df.Add(nameof(ProductId));
        // 按天分表
        //Meta.ShardPolicy = new TimeShardPolicy(nameof(Id), Meta.Factory)
        //{
        //    TablePolicy = "{{0}}_{{1:yyyyMMdd}}",
        //    Step = TimeSpan.FromDays(1),
        //};

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
        // 处理当前已登录用户信息，可以由UserModule过滤器代劳
        /*var user = ManageProvider.User;
        if (user != null)
        {
            if (isNew && !Dirtys[nameof(CreateUserID)]) CreateUserID = user.ID;
        }*/
        //if (isNew && !Dirtys[nameof(CreateTime)]) CreateTime = DateTime.Now;
        //if (isNew && !Dirtys[nameof(CreateIP)]) CreateIP = ManageProvider.UserHost;
    }
    #endregion

    #region 扩展属性
    /// <summary>产品</summary>
    [XmlIgnore, IgnoreDataMember]
    //[ScriptIgnore]
    public Product Product => Extends.Get(nameof(Product), k => Product.FindById(ProductId));

    /// <summary>产品</summary>
    [Map(nameof(ProductId), typeof(Product), "Id")]
    public String ProductName => Product?.Name;

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
    public static StockHistory FindById(Int64 id)
    {
        if (id <= 0) return null;

        // 实体缓存
        if (Meta.Session.Count < 1000) return Meta.Cache.Find(e => e.Id == id);

        // 单对象缓存
        return Meta.SingleCache[id];

        //return Find(_.Id == id);
    }

    /// <summary>根据产品、仓库查找</summary>
    /// <param name="productId">产品</param>
    /// <param name="warehouseId">仓库</param>
    /// <returns>实体列表</returns>
    public static IList<StockHistory> FindAllByProductIdAndWarehouseId(Int32 productId, Int32 warehouseId)
    {
        // 实体缓存
        if (Meta.Session.Count < 1000) return Meta.Cache.FindAll(e => e.ProductId == productId && e.WarehouseId == warehouseId);

        return FindAll(_.ProductId == productId & _.WarehouseId == warehouseId);
    }

    /// <summary>根据仓库查找</summary>
    /// <param name="warehouseId">仓库</param>
    /// <returns>实体列表</returns>
    public static IList<StockHistory> FindAllByWarehouseId(Int32 warehouseId)
    {
        // 实体缓存
        if (Meta.Session.Count < 1000) return Meta.Cache.FindAll(e => e.WarehouseId == warehouseId);

        return FindAll(_.WarehouseId == warehouseId);
    }

    /// <summary>根据操作查找</summary>
    /// <param name="operation">操作</param>
    /// <returns>实体列表</returns>
    public static IList<StockHistory> FindAllByOperation(StockOperations operation)
    {
        // 实体缓存
        if (Meta.Session.Count < 1000) return Meta.Cache.FindAll(e => e.Operation == operation);

        return FindAll(_.Operation == operation);
    }
    #endregion

    #region 高级查询
    /// <summary>高级查询</summary>
    /// <param name="productId">产品</param>
    /// <param name="warehouseId">仓库</param>
    /// <param name="operation">操作。出库、入库、移库、盘点、报废等</param>
    /// <param name="start">创建时间开始</param>
    /// <param name="end">创建时间结束</param>
    /// <param name="key">关键字</param>
    /// <param name="page">分页参数信息。可携带统计和数据权限扩展查询等信息</param>
    /// <returns>实体列表</returns>
    public static IList<StockHistory> Search(Int32 productId, Int32 warehouseId, StockOperations operation, DateTime start, DateTime end, String key, PageParameter page)
    {
        var exp = new WhereExpression();

        if (productId >= 0) exp &= _.ProductId == productId;
        if (warehouseId >= 0) exp &= _.WarehouseId == warehouseId;
        if (operation >= 0) exp &= _.Operation == operation;
        exp &= _.CreateTime.Between(start, end);
        if (!key.IsNullOrEmpty()) exp &= _.OrderId.Contains(key) | _.OrderTitle.Contains(key) | _.CreateUser.Contains(key) | _.CreateIP.Contains(key) | _.Remark.Contains(key);

        return FindAll(exp, page);
    }

    // Select Count(Id) as Id,Category From StockHistory Where CreateTime>'2020-01-24 00:00:00' Group By Category Order By Id Desc limit 20
    //static readonly FieldCache<StockHistory> _CategoryCache = new FieldCache<StockHistory>(nameof(Category))
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