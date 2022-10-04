using System.Runtime.Serialization;
using System.Xml.Serialization;
using Erp.Data.Models;
using Erp.Data.Products;
using NewLife;
using NewLife.Data;
using XCode;
using XCode.Membership;

namespace Erp.Data.Purchases;

public partial class PurchaseOrder : Entity<PurchaseOrder>
{
    #region 对象操作
    static PurchaseOrder()
    {
        // 累加字段，生成 Update xx Set Count=Count+1234 Where xxx
        //var df = Meta.Factory.AdditionalFields;
        //df.Add(nameof(SupplierId));

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

        if (SupplierId <= 0) throw new ArgumentNullException(nameof(SupplierId), "供应商不能为空");

        // 建议先调用基类方法，基类方法会做一些统一处理
        base.Valid(isNew);

        // 在新插入数据或者修改了指定字段时进行修正
        // 货币保留6位小数
        Amount = Math.Round(Amount, 6);

        if (Status <= 0) Status = OrderStatus.录入中;

        if (OccurTime.Year < 2000) OccurTime = DateTime.Now;
        if (Title.IsNullOrEmpty()) Title = $"[{OccurTime:yyMMdd}]{SupplierName}的订单";
        if (isNew && Receiver.IsNullOrEmpty()) Receiver = CreateUser;
    }
    #endregion

    #region 扩展属性
    /// <summary>供应商</summary>
    [XmlIgnore, IgnoreDataMember]
    //[ScriptIgnore]
    public Supplier Supplier => Extends.Get(nameof(Supplier), k => Supplier.FindById(SupplierId));

    /// <summary>供应商</summary>
    [Map(nameof(SupplierId), typeof(Supplier), "Id")]
    public String SupplierName => Supplier?.Name;

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
    public static PurchaseOrder FindById(Int32 id)
    {
        if (id <= 0) return null;

        // 实体缓存
        if (Meta.Session.Count < 1000) return Meta.Cache.Find(e => e.Id == id);

        // 单对象缓存
        return Meta.SingleCache[id];

        //return Find(_.Id == id);
    }

    /// <summary>根据供应商、仓库查找</summary>
    /// <param name="supplierId">供应商</param>
    /// <param name="warehouseId">仓库</param>
    /// <returns>实体列表</returns>
    public static IList<PurchaseOrder> FindAllBySupplierIdAndWarehouseId(Int32 supplierId, Int32 warehouseId)
    {
        // 实体缓存
        if (Meta.Session.Count < 1000) return Meta.Cache.FindAll(e => e.SupplierId == supplierId && e.WarehouseId == warehouseId);

        return FindAll(_.SupplierId == supplierId & _.WarehouseId == warehouseId);
    }

    /// <summary>根据仓库查找</summary>
    /// <param name="warehouseId">仓库</param>
    /// <returns>实体列表</returns>
    public static IList<PurchaseOrder> FindAllByWarehouseId(Int32 warehouseId)
    {
        // 实体缓存
        if (Meta.Session.Count < 1000) return Meta.Cache.FindAll(e => e.WarehouseId == warehouseId);

        return FindAll(_.WarehouseId == warehouseId);
    }
    #endregion

    #region 高级查询
    /// <summary>高级查询</summary>
    /// <param name="supplierId">供应商</param>
    /// <param name="warehouseId">仓库。进入的仓库</param>
    /// <param name="productId">产品</param>
    /// <param name="status">状态</param>
    /// <param name="start">更新时间开始</param>
    /// <param name="end">更新时间结束</param>
    /// <param name="key">关键字</param>
    /// <param name="page">分页参数信息。可携带统计和数据权限扩展查询等信息</param>
    /// <returns>实体列表</returns>
    public static IList<PurchaseOrder> Search(Int32 supplierId, Int32 warehouseId, Int32 productId, OrderStatus status, DateTime start, DateTime end, String key, PageParameter page)
    {
        var exp = new WhereExpression();

        if (supplierId >= 0) exp &= _.SupplierId == supplierId;
        if (warehouseId >= 0) exp &= _.WarehouseId == warehouseId;
        if (productId > 0) exp &= _.Id.In(PurchaseOrderLine.SearchSql(productId, start, end));
        if (status > 0) exp &= _.Status == status;
        exp &= _.OccurTime.Between(start, end);
        if (!key.IsNullOrEmpty()) exp &= _.Title.Contains(key) | _.ContractNo.Contains(key) | _.BillCode.Contains(key) | _.Receiver.Contains(key) | _.CreateUser.Contains(key) | _.CreateIP.Contains(key) | _.UpdateUser.Contains(key) | _.UpdateIP.Contains(key) | _.Remark.Contains(key);

        return FindAll(exp, page);
    }

    // Select Count(Id) as Id,Category From PurchaseOrder Where CreateTime>'2020-01-24 00:00:00' Group By Category Order By Id Desc limit 20
    //static readonly FieldCache<PurchaseOrder> _CategoryCache = new FieldCache<PurchaseOrder>(nameof(Category))
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
        var list = PurchaseOrderLine.FindAllByOrderId(Id);
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
    #endregion
}