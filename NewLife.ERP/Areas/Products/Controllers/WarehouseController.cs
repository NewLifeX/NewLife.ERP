using Erp.Data.Products;
using NewLife.Cube;
using NewLife.Web;

namespace NewLife.ERP.Areas.Products.Controllers;

[ProductsArea]
[Menu(68)]
public class WarehouseController : EntityController<Warehouse>
{
    static WarehouseController()
    {
        LogOnChange = true;

        {
            var df = ListFields.AddListField("Stock", "CreateUser");
            df.DisplayName = "库存";
            df.Url = "ProductStock?warehouseId={Id}";
        }
        {
            var df = ListFields.AddListField("History", "CreateUser");
            df.DisplayName = "库存历史";
            df.Url = "StockHistory?warehouseId={Id}";
        }
        {
            var df = ListFields.AddListField("Log", "CreateUser");
            df.DisplayName = "日志";
            df.Url = "/Admin/Log?category=仓库&linkId={Id}";
        }
    }

    protected override IEnumerable<Warehouse> Search(Pager p)
    {
        var id = p["Id"].ToInt(-1);
        if (id > 0)
        {
            var entity = Warehouse.FindById(id);
            if (entity != null) return new[] { entity };
        }

        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        return Warehouse.Search(start, end, p["Q"], p);
    }
}