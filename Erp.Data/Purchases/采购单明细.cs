using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace Erp.Data.Purchases
{
    /// <summary>采购单明细。采购单包含的每一种产品</summary>
    [Serializable]
    [DataObject]
    [Description("采购单明细。采购单包含的每一种产品")]
    [BindIndex("IX_PurchaseOrderLine_OrderId", false, "OrderId")]
    [BindIndex("IX_PurchaseOrderLine_ProductId_OccurTime", false, "ProductId,OccurTime")]
    [BindIndex("IX_PurchaseOrderLine_SupplierId_OccurTime", false, "SupplierId,OccurTime")]
    [BindIndex("IX_PurchaseOrderLine_SupplierId_ProductId_OccurTime", false, "SupplierId,ProductId,OccurTime")]
    [BindIndex("IX_PurchaseOrderLine_WarehouseId_OccurTime", false, "WarehouseId,OccurTime")]
    [BindTable("PurchaseOrderLine", Description = "采购单明细。采购单包含的每一种产品", ConnName = "Erp", DbType = DatabaseType.None)]
    public partial class PurchaseOrderLine
    {
        #region 属性
        private Int32 _Id;
        /// <summary>编号</summary>
        [DisplayName("编号")]
        [Description("编号")]
        [DataObjectField(true, true, false, 0)]
        [BindColumn("Id", "编号", "")]
        public Int32 Id { get => _Id; set { if (OnPropertyChanging("Id", value)) { _Id = value; OnPropertyChanged("Id"); } } }

        private Int32 _OrderId;
        /// <summary>订单</summary>
        [DisplayName("订单")]
        [Description("订单")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("OrderId", "订单", "")]
        public Int32 OrderId { get => _OrderId; set { if (OnPropertyChanging("OrderId", value)) { _OrderId = value; OnPropertyChanged("OrderId"); } } }

        private Int32 _ProductId;
        /// <summary>产品</summary>
        [DisplayName("产品")]
        [Description("产品")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("ProductId", "产品", "")]
        public Int32 ProductId { get => _ProductId; set { if (OnPropertyChanging("ProductId", value)) { _ProductId = value; OnPropertyChanged("ProductId"); } } }

        private Int32 _Quantity;
        /// <summary>数量</summary>
        [DisplayName("数量")]
        [Description("数量")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("Quantity", "数量", "")]
        public Int32 Quantity { get => _Quantity; set { if (OnPropertyChanging("Quantity", value)) { _Quantity = value; OnPropertyChanged("Quantity"); } } }

        private Decimal _Price;
        /// <summary>价格。采购价，如果含税，加上去，可修改为0价格</summary>
        [DisplayName("价格")]
        [Description("价格。采购价，如果含税，加上去，可修改为0价格")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("Price", "价格。采购价，如果含税，加上去，可修改为0价格", "")]
        public Decimal Price { get => _Price; set { if (OnPropertyChanging("Price", value)) { _Price = value; OnPropertyChanged("Price"); } } }

        private Decimal _Amount;
        /// <summary>金额。实际总价，含税</summary>
        [DisplayName("金额")]
        [Description("金额。实际总价，含税")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("Amount", "金额。实际总价，含税", "")]
        public Decimal Amount { get => _Amount; set { if (OnPropertyChanging("Amount", value)) { _Amount = value; OnPropertyChanged("Amount"); } } }

        private Int32 _SupplierId;
        /// <summary>供应商</summary>
        [DisplayName("供应商")]
        [Description("供应商")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("SupplierId", "供应商", "")]
        public Int32 SupplierId { get => _SupplierId; set { if (OnPropertyChanging("SupplierId", value)) { _SupplierId = value; OnPropertyChanged("SupplierId"); } } }

        private Int32 _WarehouseId;
        /// <summary>仓库。进入的仓库</summary>
        [DisplayName("仓库")]
        [Description("仓库。进入的仓库")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("WarehouseId", "仓库。进入的仓库", "")]
        public Int32 WarehouseId { get => _WarehouseId; set { if (OnPropertyChanging("WarehouseId", value)) { _WarehouseId = value; OnPropertyChanged("WarehouseId"); } } }

        private DateTime _OccurTime;
        /// <summary>发生时间。来自订单</summary>
        [DisplayName("发生时间")]
        [Description("发生时间。来自订单")]
        [DataObjectField(false, false, true, 0)]
        [BindColumn("OccurTime", "发生时间。来自订单", "")]
        public DateTime OccurTime { get => _OccurTime; set { if (OnPropertyChanging("OccurTime", value)) { _OccurTime = value; OnPropertyChanged("OccurTime"); } } }

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

        private String _UpdateUser;
        /// <summary>更新者</summary>
        [Category("扩展")]
        [DisplayName("更新者")]
        [Description("更新者")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn("UpdateUser", "更新者", "")]
        public String UpdateUser { get => _UpdateUser; set { if (OnPropertyChanging("UpdateUser", value)) { _UpdateUser = value; OnPropertyChanged("UpdateUser"); } } }

        private Int32 _UpdateUserID;
        /// <summary>更新人</summary>
        [Category("扩展")]
        [DisplayName("更新人")]
        [Description("更新人")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("UpdateUserID", "更新人", "")]
        public Int32 UpdateUserID { get => _UpdateUserID; set { if (OnPropertyChanging("UpdateUserID", value)) { _UpdateUserID = value; OnPropertyChanged("UpdateUserID"); } } }

        private String _UpdateIP;
        /// <summary>更新地址</summary>
        [Category("扩展")]
        [DisplayName("更新地址")]
        [Description("更新地址")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn("UpdateIP", "更新地址", "")]
        public String UpdateIP { get => _UpdateIP; set { if (OnPropertyChanging("UpdateIP", value)) { _UpdateIP = value; OnPropertyChanged("UpdateIP"); } } }

        private DateTime _UpdateTime;
        /// <summary>更新时间</summary>
        [Category("扩展")]
        [DisplayName("更新时间")]
        [Description("更新时间")]
        [DataObjectField(false, false, true, 0)]
        [BindColumn("UpdateTime", "更新时间", "")]
        public DateTime UpdateTime { get => _UpdateTime; set { if (OnPropertyChanging("UpdateTime", value)) { _UpdateTime = value; OnPropertyChanged("UpdateTime"); } } }

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
                    case "OrderId": return _OrderId;
                    case "ProductId": return _ProductId;
                    case "Quantity": return _Quantity;
                    case "Price": return _Price;
                    case "Amount": return _Amount;
                    case "SupplierId": return _SupplierId;
                    case "WarehouseId": return _WarehouseId;
                    case "OccurTime": return _OccurTime;
                    case "CreateUser": return _CreateUser;
                    case "CreateUserID": return _CreateUserID;
                    case "CreateIP": return _CreateIP;
                    case "CreateTime": return _CreateTime;
                    case "UpdateUser": return _UpdateUser;
                    case "UpdateUserID": return _UpdateUserID;
                    case "UpdateIP": return _UpdateIP;
                    case "UpdateTime": return _UpdateTime;
                    case "Remark": return _Remark;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case "Id": _Id = value.ToInt(); break;
                    case "OrderId": _OrderId = value.ToInt(); break;
                    case "ProductId": _ProductId = value.ToInt(); break;
                    case "Quantity": _Quantity = value.ToInt(); break;
                    case "Price": _Price = Convert.ToDecimal(value); break;
                    case "Amount": _Amount = Convert.ToDecimal(value); break;
                    case "SupplierId": _SupplierId = value.ToInt(); break;
                    case "WarehouseId": _WarehouseId = value.ToInt(); break;
                    case "OccurTime": _OccurTime = value.ToDateTime(); break;
                    case "CreateUser": _CreateUser = Convert.ToString(value); break;
                    case "CreateUserID": _CreateUserID = value.ToInt(); break;
                    case "CreateIP": _CreateIP = Convert.ToString(value); break;
                    case "CreateTime": _CreateTime = value.ToDateTime(); break;
                    case "UpdateUser": _UpdateUser = Convert.ToString(value); break;
                    case "UpdateUserID": _UpdateUserID = value.ToInt(); break;
                    case "UpdateIP": _UpdateIP = Convert.ToString(value); break;
                    case "UpdateTime": _UpdateTime = value.ToDateTime(); break;
                    case "Remark": _Remark = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得采购单明细字段信息的快捷方式</summary>
        public partial class _
        {
            /// <summary>编号</summary>
            public static readonly Field Id = FindByName("Id");

            /// <summary>订单</summary>
            public static readonly Field OrderId = FindByName("OrderId");

            /// <summary>产品</summary>
            public static readonly Field ProductId = FindByName("ProductId");

            /// <summary>数量</summary>
            public static readonly Field Quantity = FindByName("Quantity");

            /// <summary>价格。采购价，如果含税，加上去，可修改为0价格</summary>
            public static readonly Field Price = FindByName("Price");

            /// <summary>金额。实际总价，含税</summary>
            public static readonly Field Amount = FindByName("Amount");

            /// <summary>供应商</summary>
            public static readonly Field SupplierId = FindByName("SupplierId");

            /// <summary>仓库。进入的仓库</summary>
            public static readonly Field WarehouseId = FindByName("WarehouseId");

            /// <summary>发生时间。来自订单</summary>
            public static readonly Field OccurTime = FindByName("OccurTime");

            /// <summary>创建者</summary>
            public static readonly Field CreateUser = FindByName("CreateUser");

            /// <summary>创建人</summary>
            public static readonly Field CreateUserID = FindByName("CreateUserID");

            /// <summary>创建地址</summary>
            public static readonly Field CreateIP = FindByName("CreateIP");

            /// <summary>创建时间</summary>
            public static readonly Field CreateTime = FindByName("CreateTime");

            /// <summary>更新者</summary>
            public static readonly Field UpdateUser = FindByName("UpdateUser");

            /// <summary>更新人</summary>
            public static readonly Field UpdateUserID = FindByName("UpdateUserID");

            /// <summary>更新地址</summary>
            public static readonly Field UpdateIP = FindByName("UpdateIP");

            /// <summary>更新时间</summary>
            public static readonly Field UpdateTime = FindByName("UpdateTime");

            /// <summary>备注</summary>
            public static readonly Field Remark = FindByName("Remark");

            static Field FindByName(String name) => Meta.Table.FindByName(name);
        }

        /// <summary>取得采购单明细字段名称的快捷方式</summary>
        public partial class __
        {
            /// <summary>编号</summary>
            public const String Id = "Id";

            /// <summary>订单</summary>
            public const String OrderId = "OrderId";

            /// <summary>产品</summary>
            public const String ProductId = "ProductId";

            /// <summary>数量</summary>
            public const String Quantity = "Quantity";

            /// <summary>价格。采购价，如果含税，加上去，可修改为0价格</summary>
            public const String Price = "Price";

            /// <summary>金额。实际总价，含税</summary>
            public const String Amount = "Amount";

            /// <summary>供应商</summary>
            public const String SupplierId = "SupplierId";

            /// <summary>仓库。进入的仓库</summary>
            public const String WarehouseId = "WarehouseId";

            /// <summary>发生时间。来自订单</summary>
            public const String OccurTime = "OccurTime";

            /// <summary>创建者</summary>
            public const String CreateUser = "CreateUser";

            /// <summary>创建人</summary>
            public const String CreateUserID = "CreateUserID";

            /// <summary>创建地址</summary>
            public const String CreateIP = "CreateIP";

            /// <summary>创建时间</summary>
            public const String CreateTime = "CreateTime";

            /// <summary>更新者</summary>
            public const String UpdateUser = "UpdateUser";

            /// <summary>更新人</summary>
            public const String UpdateUserID = "UpdateUserID";

            /// <summary>更新地址</summary>
            public const String UpdateIP = "UpdateIP";

            /// <summary>更新时间</summary>
            public const String UpdateTime = "UpdateTime";

            /// <summary>备注</summary>
            public const String Remark = "Remark";
        }
        #endregion
    }
}