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
    /// <summary>采购单。采购订单</summary>
    [Serializable]
    [DataObject]
    [Description("采购单。采购订单")]
    [BindIndex("IX_PurchaseOrder_SupplierId_OccurTime", false, "SupplierId,OccurTime")]
    [BindIndex("IX_PurchaseOrder_WarehouseId_OccurTime", false, "WarehouseId,OccurTime")]
    [BindTable("PurchaseOrder", Description = "采购单。采购订单", ConnName = "Erp", DbType = DatabaseType.None)]
    public partial class PurchaseOrder
    {
        #region 属性
        private Int32 _Id;
        /// <summary>编号</summary>
        [DisplayName("编号")]
        [Description("编号")]
        [DataObjectField(true, true, false, 0)]
        [BindColumn("Id", "编号", "")]
        public Int32 Id { get => _Id; set { if (OnPropertyChanging("Id", value)) { _Id = value; OnPropertyChanged("Id"); } } }

        private Int32 _SupplierId;
        /// <summary>供应商</summary>
        [DisplayName("供应商")]
        [Description("供应商")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("SupplierId", "供应商", "")]
        public Int32 SupplierId { get => _SupplierId; set { if (OnPropertyChanging("SupplierId", value)) { _SupplierId = value; OnPropertyChanged("SupplierId"); } } }

        private String _Title;
        /// <summary>标题。概要描述信息</summary>
        [DisplayName("标题")]
        [Description("标题。概要描述信息")]
        [DataObjectField(false, false, true, 200)]
        [BindColumn("Title", "标题。概要描述信息", "", Master = true)]
        public String Title { get => _Title; set { if (OnPropertyChanging("Title", value)) { _Title = value; OnPropertyChanged("Title"); } } }

        private Int32 _Quantity;
        /// <summary>数量。总件数</summary>
        [DisplayName("数量")]
        [Description("数量。总件数")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("Quantity", "数量。总件数", "")]
        public Int32 Quantity { get => _Quantity; set { if (OnPropertyChanging("Quantity", value)) { _Quantity = value; OnPropertyChanged("Quantity"); } } }

        private Decimal _Price;
        /// <summary>价值。产品总价加上运费，已废弃</summary>
        [DisplayName("价值")]
        [Description("价值。产品总价加上运费，已废弃")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("Price", "价值。产品总价加上运费，已废弃", "")]
        public Decimal Price { get => _Price; set { if (OnPropertyChanging("Price", value)) { _Price = value; OnPropertyChanged("Price"); } } }

        private Decimal _Amount;
        /// <summary>金额。实际总价，含税和运费</summary>
        [DisplayName("金额")]
        [Description("金额。实际总价，含税和运费")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("Amount", "金额。实际总价，含税和运费", "")]
        public Decimal Amount { get => _Amount; set { if (OnPropertyChanging("Amount", value)) { _Amount = value; OnPropertyChanged("Amount"); } } }

        private Int32 _WarehouseId;
        /// <summary>仓库。进入的仓库</summary>
        [DisplayName("仓库")]
        [Description("仓库。进入的仓库")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("WarehouseId", "仓库。进入的仓库", "")]
        public Int32 WarehouseId { get => _WarehouseId; set { if (OnPropertyChanging("WarehouseId", value)) { _WarehouseId = value; OnPropertyChanged("WarehouseId"); } } }

        private Erp.Data.Models.OrderStatus _Status;
        /// <summary>状态</summary>
        [DisplayName("状态")]
        [Description("状态")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("Status", "状态", "")]
        public Erp.Data.Models.OrderStatus Status { get => _Status; set { if (OnPropertyChanging("Status", value)) { _Status = value; OnPropertyChanged("Status"); } } }

        private DateTime _OccurTime;
        /// <summary>发生时间</summary>
        [DisplayName("发生时间")]
        [Description("发生时间")]
        [DataObjectField(false, false, true, 0)]
        [BindColumn("OccurTime", "发生时间", "")]
        public DateTime OccurTime { get => _OccurTime; set { if (OnPropertyChanging("OccurTime", value)) { _OccurTime = value; OnPropertyChanged("OccurTime"); } } }

        private String _ContractNo;
        /// <summary>合同编号</summary>
        [DisplayName("合同编号")]
        [Description("合同编号")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn("ContractNo", "合同编号", "")]
        public String ContractNo { get => _ContractNo; set { if (OnPropertyChanging("ContractNo", value)) { _ContractNo = value; OnPropertyChanged("ContractNo"); } } }

        private String _Payment;
        /// <summary>付款方式</summary>
        [DisplayName("付款方式")]
        [Description("付款方式")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn("Payment", "付款方式", "")]
        public String Payment { get => _Payment; set { if (OnPropertyChanging("Payment", value)) { _Payment = value; OnPropertyChanged("Payment"); } } }

        private String _BillCode;
        /// <summary>快递单号。发货的快递单号，多个逗号隔开</summary>
        [DisplayName("快递单号")]
        [Description("快递单号。发货的快递单号，多个逗号隔开")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn("BillCode", "快递单号。发货的快递单号，多个逗号隔开", "")]
        public String BillCode { get => _BillCode; set { if (OnPropertyChanging("BillCode", value)) { _BillCode = value; OnPropertyChanged("BillCode"); } } }

        private String _Receiver;
        /// <summary>收件人</summary>
        [DisplayName("收件人")]
        [Description("收件人")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn("Receiver", "收件人", "")]
        public String Receiver { get => _Receiver; set { if (OnPropertyChanging("Receiver", value)) { _Receiver = value; OnPropertyChanged("Receiver"); } } }

        private Decimal _Freight;
        /// <summary>运费。快递费</summary>
        [DisplayName("运费")]
        [Description("运费。快递费")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("Freight", "运费。快递费", "")]
        public Decimal Freight { get => _Freight; set { if (OnPropertyChanging("Freight", value)) { _Freight = value; OnPropertyChanged("Freight"); } } }

        private Boolean _Invoice;
        /// <summary>发票。已开票</summary>
        [DisplayName("发票")]
        [Description("发票。已开票")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("Invoice", "发票。已开票", "")]
        public Boolean Invoice { get => _Invoice; set { if (OnPropertyChanging("Invoice", value)) { _Invoice = value; OnPropertyChanged("Invoice"); } } }

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
                    case "SupplierId": return _SupplierId;
                    case "Title": return _Title;
                    case "Quantity": return _Quantity;
                    case "Price": return _Price;
                    case "Amount": return _Amount;
                    case "WarehouseId": return _WarehouseId;
                    case "Status": return _Status;
                    case "OccurTime": return _OccurTime;
                    case "ContractNo": return _ContractNo;
                    case "Payment": return _Payment;
                    case "BillCode": return _BillCode;
                    case "Receiver": return _Receiver;
                    case "Freight": return _Freight;
                    case "Invoice": return _Invoice;
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
                    case "SupplierId": _SupplierId = value.ToInt(); break;
                    case "Title": _Title = Convert.ToString(value); break;
                    case "Quantity": _Quantity = value.ToInt(); break;
                    case "Price": _Price = Convert.ToDecimal(value); break;
                    case "Amount": _Amount = Convert.ToDecimal(value); break;
                    case "WarehouseId": _WarehouseId = value.ToInt(); break;
                    case "Status": _Status = (Erp.Data.Models.OrderStatus)value.ToInt(); break;
                    case "OccurTime": _OccurTime = value.ToDateTime(); break;
                    case "ContractNo": _ContractNo = Convert.ToString(value); break;
                    case "Payment": _Payment = Convert.ToString(value); break;
                    case "BillCode": _BillCode = Convert.ToString(value); break;
                    case "Receiver": _Receiver = Convert.ToString(value); break;
                    case "Freight": _Freight = Convert.ToDecimal(value); break;
                    case "Invoice": _Invoice = value.ToBoolean(); break;
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
        /// <summary>取得采购单字段信息的快捷方式</summary>
        public partial class _
        {
            /// <summary>编号</summary>
            public static readonly Field Id = FindByName("Id");

            /// <summary>供应商</summary>
            public static readonly Field SupplierId = FindByName("SupplierId");

            /// <summary>标题。概要描述信息</summary>
            public static readonly Field Title = FindByName("Title");

            /// <summary>数量。总件数</summary>
            public static readonly Field Quantity = FindByName("Quantity");

            /// <summary>价值。产品总价加上运费，已废弃</summary>
            public static readonly Field Price = FindByName("Price");

            /// <summary>金额。实际总价，含税和运费</summary>
            public static readonly Field Amount = FindByName("Amount");

            /// <summary>仓库。进入的仓库</summary>
            public static readonly Field WarehouseId = FindByName("WarehouseId");

            /// <summary>状态</summary>
            public static readonly Field Status = FindByName("Status");

            /// <summary>发生时间</summary>
            public static readonly Field OccurTime = FindByName("OccurTime");

            /// <summary>合同编号</summary>
            public static readonly Field ContractNo = FindByName("ContractNo");

            /// <summary>付款方式</summary>
            public static readonly Field Payment = FindByName("Payment");

            /// <summary>快递单号。发货的快递单号，多个逗号隔开</summary>
            public static readonly Field BillCode = FindByName("BillCode");

            /// <summary>收件人</summary>
            public static readonly Field Receiver = FindByName("Receiver");

            /// <summary>运费。快递费</summary>
            public static readonly Field Freight = FindByName("Freight");

            /// <summary>发票。已开票</summary>
            public static readonly Field Invoice = FindByName("Invoice");

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

        /// <summary>取得采购单字段名称的快捷方式</summary>
        public partial class __
        {
            /// <summary>编号</summary>
            public const String Id = "Id";

            /// <summary>供应商</summary>
            public const String SupplierId = "SupplierId";

            /// <summary>标题。概要描述信息</summary>
            public const String Title = "Title";

            /// <summary>数量。总件数</summary>
            public const String Quantity = "Quantity";

            /// <summary>价值。产品总价加上运费，已废弃</summary>
            public const String Price = "Price";

            /// <summary>金额。实际总价，含税和运费</summary>
            public const String Amount = "Amount";

            /// <summary>仓库。进入的仓库</summary>
            public const String WarehouseId = "WarehouseId";

            /// <summary>状态</summary>
            public const String Status = "Status";

            /// <summary>发生时间</summary>
            public const String OccurTime = "OccurTime";

            /// <summary>合同编号</summary>
            public const String ContractNo = "ContractNo";

            /// <summary>付款方式</summary>
            public const String Payment = "Payment";

            /// <summary>快递单号。发货的快递单号，多个逗号隔开</summary>
            public const String BillCode = "BillCode";

            /// <summary>收件人</summary>
            public const String Receiver = "Receiver";

            /// <summary>运费。快递费</summary>
            public const String Freight = "Freight";

            /// <summary>发票。已开票</summary>
            public const String Invoice = "Invoice";

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