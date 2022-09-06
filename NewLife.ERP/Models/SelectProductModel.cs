namespace NewLife.ERP.Models;

/// <summary>选择产品控件所使用的模型</summary>
public class SelectProductModel
{
    /// <summary>控件</summary>
    public String Id { get; set; }

    /// <summary>分类</summary>
    public Int32 CategoryId { get; set; }

    /// <summary>产品Id</summary>
    public Int32 ProductId { get; set; }
}