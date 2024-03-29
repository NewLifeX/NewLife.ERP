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
    /// <summary>产品。企业内采购、生产、库存、销售的物品，包括虚拟物品</summary>
    [Serializable]
    [DataObject]
    [Description("产品。企业内采购、生产、库存、销售的物品，包括虚拟物品")]
    [BindIndex("IU_Product_Code", true, "Code")]
    [BindIndex("IX_Product_CategoryId", false, "CategoryId")]
    [BindIndex("IX_Product_Kind", false, "Kind")]
    [BindTable("Product", Description = "产品。企业内采购、生产、库存、销售的物品，包括虚拟物品", ConnName = "Erp", DbType = DatabaseType.None)]
    public partial class Product
    {
        #region 属性
        private Int32 _Id;
        /// <summary>编号</summary>
        [DisplayName("编号")]
        [Description("编号")]
        [DataObjectField(true, true, false, 0)]
        [BindColumn("Id", "编号", "")]
        public Int32 Id { get => _Id; set { if (OnPropertyChanging("Id", value)) { _Id = value; OnPropertyChanged("Id"); } } }

        private String _Code;
        /// <summary>编码。全局唯一编码，外观或颜色不同的产品，可以使用不同编码</summary>
        [DisplayName("编码")]
        [Description("编码。全局唯一编码，外观或颜色不同的产品，可以使用不同编码")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn("Code", "编码。全局唯一编码，外观或颜色不同的产品，可以使用不同编码", "")]
        public String Code { get => _Code; set { if (OnPropertyChanging("Code", value)) { _Code = value; OnPropertyChanged("Code"); } } }

        private String _Name;
        /// <summary>名称。简短而准确的名字</summary>
        [DisplayName("名称")]
        [Description("名称。简短而准确的名字")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn("Name", "名称。简短而准确的名字", "", Master = true)]
        public String Name { get => _Name; set { if (OnPropertyChanging("Name", value)) { _Name = value; OnPropertyChanged("Name"); } } }

        private Int32 _CategoryId;
        /// <summary>类别</summary>
        [DisplayName("类别")]
        [Description("类别")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("CategoryId", "类别", "")]
        public Int32 CategoryId { get => _CategoryId; set { if (OnPropertyChanging("CategoryId", value)) { _CategoryId = value; OnPropertyChanged("CategoryId"); } } }

        private Erp.Data.Models.ProductKinds _Kind;
        /// <summary>种类。实物、虚拟、组合</summary>
        [DisplayName("种类")]
        [Description("种类。实物、虚拟、组合")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("Kind", "种类。实物、虚拟、组合", "")]
        public Erp.Data.Models.ProductKinds Kind { get => _Kind; set { if (OnPropertyChanging("Kind", value)) { _Kind = value; OnPropertyChanged("Kind"); } } }

        private String _Title;
        /// <summary>标题。概要描述信息</summary>
        [DisplayName("标题")]
        [Description("标题。概要描述信息")]
        [DataObjectField(false, false, true, 200)]
        [BindColumn("Title", "标题。概要描述信息", "")]
        public String Title { get => _Title; set { if (OnPropertyChanging("Title", value)) { _Title = value; OnPropertyChanged("Title"); } } }

        private Boolean _Enable;
        /// <summary>启用</summary>
        [DisplayName("启用")]
        [Description("启用")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("Enable", "启用", "")]
        public Boolean Enable { get => _Enable; set { if (OnPropertyChanging("Enable", value)) { _Enable = value; OnPropertyChanged("Enable"); } } }

        private Int32 _Quantity;
        /// <summary>数量。真实数量以各仓库库存量为准</summary>
        [DisplayName("数量")]
        [Description("数量。真实数量以各仓库库存量为准")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("Quantity", "数量。真实数量以各仓库库存量为准", "")]
        public Int32 Quantity { get => _Quantity; set { if (OnPropertyChanging("Quantity", value)) { _Quantity = value; OnPropertyChanged("Quantity"); } } }

        private String _Unit;
        /// <summary>单位</summary>
        [DisplayName("单位")]
        [Description("单位")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn("Unit", "单位", "")]
        public String Unit { get => _Unit; set { if (OnPropertyChanging("Unit", value)) { _Unit = value; OnPropertyChanged("Unit"); } } }

        private Decimal _Price;
        /// <summary>价格。销售参考价，用于评估库存价值，以及采购销售默认价格</summary>
        [DisplayName("价格")]
        [Description("价格。销售参考价，用于评估库存价值，以及采购销售默认价格")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("Price", "价格。销售参考价，用于评估库存价值，以及采购销售默认价格", "")]
        public Decimal Price { get => _Price; set { if (OnPropertyChanging("Price", value)) { _Price = value; OnPropertyChanged("Price"); } } }

        private Double _Weight;
        /// <summary>重量。单位kg</summary>
        [DisplayName("重量")]
        [Description("重量。单位kg")]
        [DataObjectField(false, false, false, 0)]
        [BindColumn("Weight", "重量。单位kg", "")]
        public Double Weight { get => _Weight; set { if (OnPropertyChanging("Weight", value)) { _Weight = value; OnPropertyChanged("Weight"); } } }

        private String _Dimension;
        /// <summary>尺寸。长宽高LWH，单位cm</summary>
        [DisplayName("尺寸")]
        [Description("尺寸。长宽高LWH，单位cm")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn("Dimension", "尺寸。长宽高LWH，单位cm", "")]
        public String Dimension { get => _Dimension; set { if (OnPropertyChanging("Dimension", value)) { _Dimension = value; OnPropertyChanged("Dimension"); } } }

        private String _Image;
        /// <summary>图片</summary>
        [DisplayName("图片")]
        [Description("图片")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn("Image", "图片", "", ItemType = "Image")]
        public String Image { get => _Image; set { if (OnPropertyChanging("Image", value)) { _Image = value; OnPropertyChanged("Image"); } } }

        private String _Specification;
        /// <summary>规格</summary>
        [DisplayName("规格")]
        [Description("规格")]
        [DataObjectField(false, false, true, 200)]
        [BindColumn("Specification", "规格", "")]
        public String Specification { get => _Specification; set { if (OnPropertyChanging("Specification", value)) { _Specification = value; OnPropertyChanged("Specification"); } } }

        private String _PinYin;
        /// <summary>拼音。仅用于快速搜索</summary>
        [DisplayName("拼音")]
        [Description("拼音。仅用于快速搜索")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn("PinYin", "拼音。仅用于快速搜索", "")]
        public String PinYin { get => _PinYin; set { if (OnPropertyChanging("PinYin", value)) { _PinYin = value; OnPropertyChanged("PinYin"); } } }

        private String _PinYin2;
        /// <summary>拼音2。仅用于快速搜索</summary>
        [DisplayName("拼音2")]
        [Description("拼音2。仅用于快速搜索")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn("PinYin2", "拼音2。仅用于快速搜索", "")]
        public String PinYin2 { get => _PinYin2; set { if (OnPropertyChanging("PinYin2", value)) { _PinYin2 = value; OnPropertyChanged("PinYin2"); } } }

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
                    case "Code": return _Code;
                    case "Name": return _Name;
                    case "CategoryId": return _CategoryId;
                    case "Kind": return _Kind;
                    case "Title": return _Title;
                    case "Enable": return _Enable;
                    case "Quantity": return _Quantity;
                    case "Unit": return _Unit;
                    case "Price": return _Price;
                    case "Weight": return _Weight;
                    case "Dimension": return _Dimension;
                    case "Image": return _Image;
                    case "Specification": return _Specification;
                    case "PinYin": return _PinYin;
                    case "PinYin2": return _PinYin2;
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
                    case "Code": _Code = Convert.ToString(value); break;
                    case "Name": _Name = Convert.ToString(value); break;
                    case "CategoryId": _CategoryId = value.ToInt(); break;
                    case "Kind": _Kind = (Erp.Data.Models.ProductKinds)value.ToInt(); break;
                    case "Title": _Title = Convert.ToString(value); break;
                    case "Enable": _Enable = value.ToBoolean(); break;
                    case "Quantity": _Quantity = value.ToInt(); break;
                    case "Unit": _Unit = Convert.ToString(value); break;
                    case "Price": _Price = Convert.ToDecimal(value); break;
                    case "Weight": _Weight = value.ToDouble(); break;
                    case "Dimension": _Dimension = Convert.ToString(value); break;
                    case "Image": _Image = Convert.ToString(value); break;
                    case "Specification": _Specification = Convert.ToString(value); break;
                    case "PinYin": _PinYin = Convert.ToString(value); break;
                    case "PinYin2": _PinYin2 = Convert.ToString(value); break;
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
        /// <summary>取得产品字段信息的快捷方式</summary>
        public partial class _
        {
            /// <summary>编号</summary>
            public static readonly Field Id = FindByName("Id");

            /// <summary>编码。全局唯一编码，外观或颜色不同的产品，可以使用不同编码</summary>
            public static readonly Field Code = FindByName("Code");

            /// <summary>名称。简短而准确的名字</summary>
            public static readonly Field Name = FindByName("Name");

            /// <summary>类别</summary>
            public static readonly Field CategoryId = FindByName("CategoryId");

            /// <summary>种类。实物、虚拟、组合</summary>
            public static readonly Field Kind = FindByName("Kind");

            /// <summary>标题。概要描述信息</summary>
            public static readonly Field Title = FindByName("Title");

            /// <summary>启用</summary>
            public static readonly Field Enable = FindByName("Enable");

            /// <summary>数量。真实数量以各仓库库存量为准</summary>
            public static readonly Field Quantity = FindByName("Quantity");

            /// <summary>单位</summary>
            public static readonly Field Unit = FindByName("Unit");

            /// <summary>价格。销售参考价，用于评估库存价值，以及采购销售默认价格</summary>
            public static readonly Field Price = FindByName("Price");

            /// <summary>重量。单位kg</summary>
            public static readonly Field Weight = FindByName("Weight");

            /// <summary>尺寸。长宽高LWH，单位cm</summary>
            public static readonly Field Dimension = FindByName("Dimension");

            /// <summary>图片</summary>
            public static readonly Field Image = FindByName("Image");

            /// <summary>规格</summary>
            public static readonly Field Specification = FindByName("Specification");

            /// <summary>拼音。仅用于快速搜索</summary>
            public static readonly Field PinYin = FindByName("PinYin");

            /// <summary>拼音2。仅用于快速搜索</summary>
            public static readonly Field PinYin2 = FindByName("PinYin2");

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

        /// <summary>取得产品字段名称的快捷方式</summary>
        public partial class __
        {
            /// <summary>编号</summary>
            public const String Id = "Id";

            /// <summary>编码。全局唯一编码，外观或颜色不同的产品，可以使用不同编码</summary>
            public const String Code = "Code";

            /// <summary>名称。简短而准确的名字</summary>
            public const String Name = "Name";

            /// <summary>类别</summary>
            public const String CategoryId = "CategoryId";

            /// <summary>种类。实物、虚拟、组合</summary>
            public const String Kind = "Kind";

            /// <summary>标题。概要描述信息</summary>
            public const String Title = "Title";

            /// <summary>启用</summary>
            public const String Enable = "Enable";

            /// <summary>数量。真实数量以各仓库库存量为准</summary>
            public const String Quantity = "Quantity";

            /// <summary>单位</summary>
            public const String Unit = "Unit";

            /// <summary>价格。销售参考价，用于评估库存价值，以及采购销售默认价格</summary>
            public const String Price = "Price";

            /// <summary>重量。单位kg</summary>
            public const String Weight = "Weight";

            /// <summary>尺寸。长宽高LWH，单位cm</summary>
            public const String Dimension = "Dimension";

            /// <summary>图片</summary>
            public const String Image = "Image";

            /// <summary>规格</summary>
            public const String Specification = "Specification";

            /// <summary>拼音。仅用于快速搜索</summary>
            public const String PinYin = "PinYin";

            /// <summary>拼音2。仅用于快速搜索</summary>
            public const String PinYin2 = "PinYin2";

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