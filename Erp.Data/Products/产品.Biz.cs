using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Erp.Data.Models;
using NewLife;
using NewLife.Data;
using XCode;
using XCode.Membership;

namespace Erp.Data.Products;

public partial class Product : Entity<Product>
{
    #region 对象操作
    static Product()
    {
        // 累加字段，生成 Update xx Set Count=Count+1234 Where xxx
        //var df = Meta.Factory.AdditionalFields;
        //df.Add(nameof(CategoryId));

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

        // 这里验证参数范围，建议抛出参数异常，指定参数名，前端用户界面可以捕获参数异常并聚焦到对应的参数输入框
        if (Code.IsNullOrEmpty()) throw new ArgumentNullException(nameof(Code), "编码不能为空！");
        if (Name.IsNullOrEmpty()) throw new ArgumentNullException(nameof(Name), "名称不能为空！");

        // 建议先调用基类方法，基类方法会做一些统一处理
        base.Valid(isNew);

        // 在新插入数据或者修改了指定字段时进行修正
        // 货币保留6位小数
        Price = Math.Round(Price, 6);
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
        // CheckExist(isNew, nameof(Code));
    }

    /// <summary>首次连接数据库时初始化数据，仅用于实体类重载，用户不应该调用该方法</summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected override void InitData()
    {
        // InitData一般用于当数据表没有数据时添加一些默认数据，该实体类的任何第一次数据库操作都会触发该方法，默认异步调用
        if (Meta.Session.Count > 0) return;

        var entity = new Product
        {
            Code = "A2",
            Name = "A2工业计算机",
            Title = "A2工业计算机 物联网 边缘网关",
            CategoryId = 1,
            Kind = ProductKinds.实物,
            Enable = true,
            Unit = "台",
            Quantity = 100,
            Price = 1500,
            //Units = 2,
        };
        entity.Insert();

        entity = new Product
        {
            Code = "A2-4G",
            Name = "A2工业计算机4G版",
            Title = "A2工业计算机 4G 物联网 边缘网关",
            CategoryId = 1,
            Kind = ProductKinds.实物,
            Enable = true,
            Unit = "台",
            Quantity = 100,
            Price = 1800,
        };
        entity.Insert();
    }
    #endregion

    #region 扩展属性
    /// <summary>类别</summary>
    [XmlIgnore, IgnoreDataMember]
    public ProductCategory Category => Extends.Get(nameof(Category), k => ProductCategory.FindById(CategoryId));

    /// <summary>类别</summary>
    [Map(nameof(CategoryId), typeof(ProductCategory), "Id")]
    public String CategoryName => Category?.Name;
    #endregion

    #region 扩展查询
    /// <summary>根据编号查找</summary>
    /// <param name="id">编号</param>
    /// <returns>实体对象</returns>
    public static Product FindById(Int32 id)
    {
        if (id <= 0) return null;

        // 实体缓存
        if (Meta.Session.Count < 1000) return Meta.Cache.Find(e => e.Id == id);

        // 单对象缓存
        return Meta.SingleCache[id];

        //return Find(_.Id == id);
    }

    /// <summary>根据编码查找</summary>
    /// <param name="code">编码</param>
    /// <returns>实体对象</returns>
    public static Product FindByCode(String code)
    {
        // 实体缓存
        if (Meta.Session.Count < 1000) return Meta.Cache.Find(e => e.Code.EqualIgnoreCase(code));

        return Find(_.Code == code);
    }

    public static Int32 FindCountByCategory(Int32 categoryId)
    {
        // 实体缓存
        if (Meta.Session.Count < 1000) return Meta.Cache.Entities.Count(e => e.CategoryId == categoryId);

        return (Int32)FindCount(_.CategoryId == categoryId);
    }
    #endregion

    #region 高级查询
    /// <summary>高级查询</summary>
    /// <param name="code">编码。全局唯一编码</param>
    /// <param name="start">更新时间开始</param>
    /// <param name="end">更新时间结束</param>
    /// <param name="key">关键字</param>
    /// <param name="page">分页参数信息。可携带统计和数据权限扩展查询等信息</param>
    /// <returns>实体列表</returns>
    public static IList<Product> Search(String code, Int32 categoryId, ProductKinds kind, Boolean? enable, DateTime start, DateTime end, String key, PageParameter page)
    {
        var exp = new WhereExpression();

        if (!code.IsNullOrEmpty()) exp &= _.Code == code;
        if (categoryId >= 0) exp &= _.CategoryId == categoryId;
        if (kind > 0) exp &= _.Kind == kind;
        if (enable != null) exp &= _.Enable == enable;

        exp &= _.UpdateTime.Between(start, end);
        if (!key.IsNullOrEmpty()) exp &= _.Code.Contains(key) | _.Name.Contains(key) | _.Title.Contains(key) | _.Unit.Contains(key) | _.Specification.Contains(key) | _.CreateUser.Contains(key) | _.CreateIP.Contains(key) | _.UpdateUser.Contains(key) | _.UpdateIP.Contains(key) | _.Remark.Contains(key);

        return FindAll(exp, page);
    }

    // Select Count(Id) as Id,Category From Product Where CreateTime>'2020-01-24 00:00:00' Group By Category Order By Id Desc limit 20
    //static readonly FieldCache<Product> _CategoryCache = new FieldCache<Product>(nameof(Category))
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
        // 库存
        var stocks = ProductStock.FindAllByProduct(Id);

        // SKU
        var us = ProductUnit.FindAllByProduct(Id);
        Units = us.Count;

        // 各SKU库存来自于仓库数量
        foreach (var unit in us)
        {
            var st = stocks.FirstOrDefault(s => s.UnitName == unit.Name);
            if (st != null)
            {
                unit.Quantity = st.Quantity;
                unit.Update();
            }
        }

        // 优先以仓库数量累加产品库存，因为部分库存可能未登记SKU
        if (stocks.Count > 0)
            Quantity = stocks.Sum(e => e.Quantity);
        else if (us.Count > 0)
            Quantity = us.Sum(e => e.Quantity);
    }
    #endregion
}