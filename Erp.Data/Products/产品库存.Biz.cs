using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using NewLife;
using NewLife.Data;
using XCode;
using XCode.Membership;

namespace Erp.Data.Products;

public partial class ProductStock : Entity<ProductStock>
{
    #region 对象操作
    static ProductStock()
    {
        // 累加字段，生成 Update xx Set Count=Count+1234 Where xxx
        var df = Meta.Factory.AdditionalFields;
        df.Add(nameof(Quantity));

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
            if (!Dirtys[nameof(UpdateUserID)]) UpdateUserID = user.ID;
        }*/
        //if (isNew && !Dirtys[nameof(CreateTime)]) CreateTime = DateTime.Now;
        //if (!Dirtys[nameof(UpdateTime)]) UpdateTime = DateTime.Now;
        //if (isNew && !Dirtys[nameof(CreateIP)]) CreateIP = ManageProvider.UserHost;
        //if (!Dirtys[nameof(UpdateIP)]) UpdateIP = ManageProvider.UserHost;

        // 检查唯一索引
        // CheckExist(isNew, nameof(ProductId), nameof(WarehouseId));
    }

    /// <summary>首次连接数据库时初始化数据，仅用于实体类重载，用户不应该调用该方法</summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected override void InitData()
    {
        // InitData一般用于当数据表没有数据时添加一些默认数据，该实体类的任何第一次数据库操作都会触发该方法，默认异步调用
        if (Meta.Session.Count > 0) return;

        var entity = new ProductStock
        {
            ProductId = 1,
            //UnitName = "挂耳",
            WarehouseId = 1,
            Quantity = 40,
        };
        entity.Insert();

        //entity = new ProductStock
        //{
        //    ProductId = 1,
        //    //UnitName = "侧方",
        //    WarehouseId = 1,
        //    Quantity = 50,
        //};
        //entity.Insert();

        entity = new ProductStock
        {
            ProductId = 2,
            WarehouseId = 1,
            Quantity = 60,
        };
        entity.Insert();
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
    public static ProductStock FindById(Int32 id)
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
    /// <returns>实体对象</returns>
    public static ProductStock FindByProductIdAndWarehouseId(Int32 productId, Int32 warehouseId)
    {
        // 实体缓存
        if (Meta.Session.Count < 1000) return Meta.Cache.Find(e => e.ProductId == productId && e.WarehouseId == warehouseId);

        return Find(_.ProductId == productId & _.WarehouseId == warehouseId);
    }

    public static IList<ProductStock> FindAllByProduct(Int32 productId)
    {
        if (productId <= 0) return new List<ProductStock>();

        if (Meta.Session.Count < 1000) return Meta.Cache.FindAll(e => e.ProductId == productId);

        return FindAll(_.ProductId == productId);
    }
    #endregion

    #region 高级查询
    /// <summary>高级查询</summary>
    /// <param name="productId">产品</param>
    /// <param name="warehouseId">仓库</param>
    /// <param name="start">更新时间开始</param>
    /// <param name="end">更新时间结束</param>
    /// <param name="key">关键字</param>
    /// <param name="page">分页参数信息。可携带统计和数据权限扩展查询等信息</param>
    /// <returns>实体列表</returns>
    public static IList<ProductStock> Search(Int32 productId, Int32 warehouseId, DateTime start, DateTime end, String key, PageParameter page)
    {
        var exp = new WhereExpression();

        if (productId >= 0) exp &= _.ProductId == productId;
        if (warehouseId >= 0) exp &= _.WarehouseId == warehouseId;
        exp &= _.UpdateTime.Between(start, end);
        if (!key.IsNullOrEmpty()) exp &= _.CreateUser.Contains(key) | _.CreateIP.Contains(key) | _.UpdateUser.Contains(key) | _.UpdateIP.Contains(key) | _.Remark.Contains(key);

        return FindAll(exp, page);
    }

    // Select Count(Id) as Id,Category From ProductStock Where CreateTime>'2020-01-24 00:00:00' Group By Category Order By Id Desc limit 20
    //static readonly FieldCache<ProductStock> _CategoryCache = new FieldCache<ProductStock>(nameof(Category))
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