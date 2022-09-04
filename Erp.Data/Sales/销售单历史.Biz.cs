using System.Runtime.Serialization;
using System.Xml.Serialization;
using NewLife;
using NewLife.Data;
using XCode;
using XCode.Membership;

namespace Erp.Data.Sales
{
    public partial class SaleOrderHistory : Entity<SaleOrderHistory>
    {
        #region 对象操作
        static SaleOrderHistory()
        {
            Meta.Table.DataTable.InsertOnly = true;

            // 累加字段，生成 Update xx Set Count=Count+1234 Where xxx
            //var df = Meta.Factory.AdditionalFields;
            //df.Add(nameof(OrderId));

            // 过滤器 UserModule、TimeModule、IPModule
            Meta.Modules.Add<UserModule>();
            Meta.Modules.Add<TimeModule>();
            Meta.Modules.Add<IPModule>();
            Meta.Modules.Add<TraceModule>();
        }

        /// <summary>验证并修补数据，通过抛出异常的方式提示验证失败。</summary>
        /// <param name="isNew">是否插入</param>
        public override void Valid(Boolean isNew)
        {
            // 如果没有脏数据，则不需要进行任何处理
            if (!HasDirty) return;

            var len = _.Remark.Length;
            if (len > 0 && !Remark.IsNullOrEmpty() && Remark.Length > len) Remark = Remark[..len];

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
        /// <summary>订单</summary>
        [XmlIgnore, IgnoreDataMember]
        //[ScriptIgnore]
        public SaleOrder Order => Extends.Get(nameof(Order), k => SaleOrder.FindById(OrderId));

        /// <summary>订单</summary>
        [Map(nameof(OrderId), typeof(SaleOrder), "Id")]
        public String OrderTitle => Order?.Title;
        #endregion

        #region 扩展查询
        /// <summary>根据编号查找</summary>
        /// <param name="id">编号</param>
        /// <returns>实体对象</returns>
        public static SaleOrderHistory FindById(Int32 id)
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
        public static IList<SaleOrderHistory> FindAllByOrderId(Int32 orderId)
        {
            // 实体缓存
            if (Meta.Session.Count < 1000) return Meta.Cache.FindAll(e => e.OrderId == orderId);

            return FindAll(_.OrderId == orderId);
        }
        #endregion

        #region 高级查询
        /// <summary>高级查询</summary>
        /// <param name="orderId">订单</param>
        /// <param name="start">创建时间开始</param>
        /// <param name="end">创建时间结束</param>
        /// <param name="key">关键字</param>
        /// <param name="page">分页参数信息。可携带统计和数据权限扩展查询等信息</param>
        /// <returns>实体列表</returns>
        public static IList<SaleOrderHistory> Search(Int32 orderId, DateTime start, DateTime end, String key, PageParameter page)
        {
            var exp = new WhereExpression();

            if (orderId >= 0) exp &= _.OrderId == orderId;
            exp &= _.CreateTime.Between(start, end);
            if (!key.IsNullOrEmpty()) exp &= _.Action.Contains(key) | _.Remark.Contains(key) | _.TraceId.Contains(key) | _.CreateUser.Contains(key) | _.CreateIP.Contains(key);

            return FindAll(exp, page);
        }

        // Select Count(Id) as Id,Category From SaleOrderHistory Where CreateTime>'2020-01-24 00:00:00' Group By Category Order By Id Desc limit 20
        //static readonly FieldCache<SaleOrderHistory> _CategoryCache = new FieldCache<SaleOrderHistory>(nameof(Category))
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
}