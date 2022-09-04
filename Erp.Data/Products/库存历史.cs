using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace Erp.Data.Products
{
    /// <summary>库存历史。对仓库中产品进行出入库管理</summary>
    [Serializable]
    [DataObject]
    [Description("库存历史。对仓库中产品进行出入库管理")]
    [BindIndex("IX_StockHistory_WarehouseId_ProductId", false, "WarehouseId,ProductId")]
    [BindIndex("IX_StockHistory_ProductId_Operation", false, "ProductId,Operation")]
    [BindIndex("IX_StockHistory_WarehouseId_Operation", false, "WarehouseId,Operation")]
    [BindIndex("IX_StockHistory_Operation", false, "Operation")]
    [BindTable("StockHistory", Description = "库存历史。对仓库中产品进行出入库管理", ConnName = "Erp", DbType = DatabaseType.None)]
    public partial class StockHistory
    {
        #region 属性
        private Int64 _Id;
        /// <summary>编号</summary>
        [DisplayName("编号")]
        [Description("编号")]
        [DataObjectField(true, false, false, 0)]
        [BindColumn("Id", "编号", "")]
        public Int64 Id { get => _Id; set { if (OnPropertyChanging("Id", value)) { _Id = value; OnPropertyChanged("Id"); } } }

        private Int32 _ProductId;
        /// <summary>产品</summary>
        [DisplayName("产品")]
        [Description("产品")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("ProductId", "产品", "")]
        public Int32 ProductId { get => _ProductId; set { if (OnPropertyChanging("ProductId", value)) { _ProductId = value; OnPropertyChanged("ProductId"); } } }

        private Int32 _WarehouseId;
        /// <summary>仓库</summary>
        [DisplayName("仓库")]
        [Description("仓库")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("WarehouseId", "仓库", "")]
        public Int32 WarehouseId { get => _WarehouseId; set { if (OnPropertyChanging("WarehouseId", value)) { _WarehouseId = value; OnPropertyChanged("WarehouseId"); } } }

        private Erp.Data.Models.StockOperations _Operation;
        /// <summary>操作。出库、入库、移库、盘点、报废等</summary>
        [DisplayName("操作")]
        [Description("操作。出库、入库、移库、盘点、报废等")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("Operation", "操作。出库、入库、移库、盘点、报废等", "")]
        public Erp.Data.Models.StockOperations Operation { get => _Operation; set { if (OnPropertyChanging("Operation", value)) { _Operation = value; OnPropertyChanged("Operation"); } } }

        private Int32 _Quantity;
        /// <summary>数量。本次操作涉及产品数量，可能是负数</summary>
        [DisplayName("数量")]
        [Description("数量。本次操作涉及产品数量，可能是负数")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("Quantity", "数量。本次操作涉及产品数量，可能是负数", "")]
        public Int32 Quantity { get => _Quantity; set { if (OnPropertyChanging("Quantity", value)) { _Quantity = value; OnPropertyChanged("Quantity"); } } }

        private Int32 _OldQuantity;
        /// <summary>原数量。操作前数量</summary>
        [DisplayName("原数量")]
        [Description("原数量。操作前数量")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("OldQuantity", "原数量。操作前数量", "")]
        public Int32 OldQuantity { get => _OldQuantity; set { if (OnPropertyChanging("OldQuantity", value)) { _OldQuantity = value; OnPropertyChanged("OldQuantity"); } } }

        private Int32 _NewQuantity;
        /// <summary>新数量。操作后数量</summary>
        [DisplayName("新数量")]
        [Description("新数量。操作后数量")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("NewQuantity", "新数量。操作后数量", "")]
        public Int32 NewQuantity { get => _NewQuantity; set { if (OnPropertyChanging("NewQuantity", value)) { _NewQuantity = value; OnPropertyChanged("NewQuantity"); } } }

        private DateTime _OccurTime;
        /// <summary>发生时间。实际出入库时间，不同于数据录入时间</summary>
        [DisplayName("发生时间")]
        [Description("发生时间。实际出入库时间，不同于数据录入时间")]
        [DataObjectField(false, false, true, 0)]
        [BindColumn("OccurTime", "发生时间。实际出入库时间，不同于数据录入时间", "")]
        public DateTime OccurTime { get => _OccurTime; set { if (OnPropertyChanging("OccurTime", value)) { _OccurTime = value; OnPropertyChanged("OccurTime"); } } }

        private String _OrderId;
        /// <summary>关联订单。</summary>
        [DisplayName("关联订单")]
        [Description("关联订单。")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn("OrderId", "关联订单。", "")]
        public String OrderId { get => _OrderId; set { if (OnPropertyChanging("OrderId", value)) { _OrderId = value; OnPropertyChanged("OrderId"); } } }

        private String _OrderTitle;
        /// <summary>订单标题</summary>
        [DisplayName("订单标题")]
        [Description("订单标题")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn("OrderTitle", "订单标题", "")]
        public String OrderTitle { get => _OrderTitle; set { if (OnPropertyChanging("OrderTitle", value)) { _OrderTitle = value; OnPropertyChanged("OrderTitle"); } } }

        private String _TraceId;
        /// <summary>性能追踪。用于APM性能追踪定位，还原该事件的调用链</summary>
        [DisplayName("性能追踪")]
        [Description("性能追踪。用于APM性能追踪定位，还原该事件的调用链")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn("TraceId", "性能追踪。用于APM性能追踪定位，还原该事件的调用链", "")]
        public String TraceId { get => _TraceId; set { if (OnPropertyChanging("TraceId", value)) { _TraceId = value; OnPropertyChanged("TraceId"); } } }

        private String _CreateUser;
        /// <summary>创建者</summary>
        [Category("扩展")]
        [DisplayName("创建者")]
        [Description("创建者")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn("CreateUser", "创建者", "")]
        public String CreateUser { get => _CreateUser; set { if (OnPropertyChanging("CreateUser", value)) { _CreateUser = value; OnPropertyChanged("CreateUser"); } } }

        private Int32 _CreateUserID;
        /// <summary>创建人</summary>
        [Category("扩展")]
        [DisplayName("创建人")]
        [Description("创建人")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("CreateUserID", "创建人", "")]
        public Int32 CreateUserID { get => _CreateUserID; set { if (OnPropertyChanging("CreateUserID", value)) { _CreateUserID = value; OnPropertyChanged("CreateUserID"); } } }

        private String _CreateIP;
        /// <summary>创建地址</summary>
        [Category("扩展")]
        [DisplayName("创建地址")]
        [Description("创建地址")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn("CreateIP", "创建地址", "")]
        public String CreateIP { get => _CreateIP; set { if (OnPropertyChanging("CreateIP", value)) { _CreateIP = value; OnPropertyChanged("CreateIP"); } } }

        private DateTime _CreateTime;
        /// <summary>创建时间</summary>
        [Category("扩展")]
        [DisplayName("创建时间")]
        [Description("创建时间")]
        [DataObjectField(false, false, true, 0)]
        [BindColumn("CreateTime", "创建时间", "")]
        public DateTime CreateTime { get => _CreateTime; set { if (OnPropertyChanging("CreateTime", value)) { _CreateTime = value; OnPropertyChanged("CreateTime"); } } }

        private String _Remark;
        /// <summary>备注</summary>
        [Category("扩展")]
        [DisplayName("备注")]
        [Description("备注")]
        [DataObjectField(false, false, true, 500)]
        [BindColumn("Remark", "备注", "")]
        public String Remark { get => _Remark; set { if (OnPropertyChanging("Remark", value)) { _Remark = value; OnPropertyChanged("Remark"); } } }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        public override Object this[String name]
        {
            get
            {
                switch (name)
                {
                    case "Id": return _Id;
                    case "ProductId": return _ProductId;
                    case "WarehouseId": return _WarehouseId;
                    case "Operation": return _Operation;
                    case "Quantity": return _Quantity;
                    case "OldQuantity": return _OldQuantity;
                    case "NewQuantity": return _NewQuantity;
                    case "OccurTime": return _OccurTime;
                    case "OrderId": return _OrderId;
                    case "OrderTitle": return _OrderTitle;
                    case "TraceId": return _TraceId;
                    case "CreateUser": return _CreateUser;
                    case "CreateUserID": return _CreateUserID;
                    case "CreateIP": return _CreateIP;
                    case "CreateTime": return _CreateTime;
                    case "Remark": return _Remark;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case "Id": _Id = value.ToLong(); break;
                    case "ProductId": _ProductId = value.ToInt(); break;
                    case "WarehouseId": _WarehouseId = value.ToInt(); break;
                    case "Operation": _Operation = (Erp.Data.Models.StockOperations)value.ToInt(); break;
                    case "Quantity": _Quantity = value.ToInt(); break;
                    case "OldQuantity": _OldQuantity = value.ToInt(); break;
                    case "NewQuantity": _NewQuantity = value.ToInt(); break;
                    case "OccurTime": _OccurTime = value.ToDateTime(); break;
                    case "OrderId": _OrderId = Convert.ToString(value); break;
                    case "OrderTitle": _OrderTitle = Convert.ToString(value); break;
                    case "TraceId": _TraceId = Convert.ToString(value); break;
                    case "CreateUser": _CreateUser = Convert.ToString(value); break;
                    case "CreateUserID": _CreateUserID = value.ToInt(); break;
                    case "CreateIP": _CreateIP = Convert.ToString(value); break;
                    case "CreateTime": _CreateTime = value.ToDateTime(); break;
                    case "Remark": _Remark = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得库存历史字段信息的快捷方式</summary>
        public partial class _
        {
            /// <summary>编号</summary>
            public static readonly Field Id = FindByName("Id");

            /// <summary>产品</summary>
            public static readonly Field ProductId = FindByName("ProductId");

            /// <summary>仓库</summary>
            public static readonly Field WarehouseId = FindByName("WarehouseId");

            /// <summary>操作。出库、入库、移库、盘点、报废等</summary>
            public static readonly Field Operation = FindByName("Operation");

            /// <summary>数量。本次操作涉及产品数量，可能是负数</summary>
            public static readonly Field Quantity = FindByName("Quantity");

            /// <summary>原数量。操作前数量</summary>
            public static readonly Field OldQuantity = FindByName("OldQuantity");

            /// <summary>新数量。操作后数量</summary>
            public static readonly Field NewQuantity = FindByName("NewQuantity");

            /// <summary>发生时间。实际出入库时间，不同于数据录入时间</summary>
            public static readonly Field OccurTime = FindByName("OccurTime");

            /// <summary>关联订单。</summary>
            public static readonly Field OrderId = FindByName("OrderId");

            /// <summary>订单标题</summary>
            public static readonly Field OrderTitle = FindByName("OrderTitle");

            /// <summary>性能追踪。用于APM性能追踪定位，还原该事件的调用链</summary>
            public static readonly Field TraceId = FindByName("TraceId");

            /// <summary>创建者</summary>
            public static readonly Field CreateUser = FindByName("CreateUser");

            /// <summary>创建人</summary>
            public static readonly Field CreateUserID = FindByName("CreateUserID");

            /// <summary>创建地址</summary>
            public static readonly Field CreateIP = FindByName("CreateIP");

            /// <summary>创建时间</summary>
            public static readonly Field CreateTime = FindByName("CreateTime");

            /// <summary>备注</summary>
            public static readonly Field Remark = FindByName("Remark");

            static Field FindByName(String name) => Meta.Table.FindByName(name);
        }

        /// <summary>取得库存历史字段名称的快捷方式</summary>
        public partial class __
        {
            /// <summary>编号</summary>
            public const String Id = "Id";

            /// <summary>产品</summary>
            public const String ProductId = "ProductId";

            /// <summary>仓库</summary>
            public const String WarehouseId = "WarehouseId";

            /// <summary>操作。出库、入库、移库、盘点、报废等</summary>
            public const String Operation = "Operation";

            /// <summary>数量。本次操作涉及产品数量，可能是负数</summary>
            public const String Quantity = "Quantity";

            /// <summary>原数量。操作前数量</summary>
            public const String OldQuantity = "OldQuantity";

            /// <summary>新数量。操作后数量</summary>
            public const String NewQuantity = "NewQuantity";

            /// <summary>发生时间。实际出入库时间，不同于数据录入时间</summary>
            public const String OccurTime = "OccurTime";

            /// <summary>关联订单。</summary>
            public const String OrderId = "OrderId";

            /// <summary>订单标题</summary>
            public const String OrderTitle = "OrderTitle";

            /// <summary>性能追踪。用于APM性能追踪定位，还原该事件的调用链</summary>
            public const String TraceId = "TraceId";

            /// <summary>创建者</summary>
            public const String CreateUser = "CreateUser";

            /// <summary>创建人</summary>
            public const String CreateUserID = "CreateUserID";

            /// <summary>创建地址</summary>
            public const String CreateIP = "CreateIP";

            /// <summary>创建时间</summary>
            public const String CreateTime = "CreateTime";

            /// <summary>备注</summary>
            public const String Remark = "Remark";
        }
        #endregion
    }
}