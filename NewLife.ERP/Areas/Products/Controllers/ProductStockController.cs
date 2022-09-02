using Erp.Data.Models;
using Erp.Data.Products;
using Microsoft.AspNetCore.Mvc;
using NewLife.Cube;
using NewLife.Cube.Extensions;
using NewLife.ERP.Areas.Products.Models;
using NewLife.ERP.Services;
using NewLife.Web;
using XCode.Membership;

namespace NewLife.ERP.Areas.Products.Controllers;

[ProductsArea]
[Menu(0, false)]
public class ProductStockController : EntityController<ProductStock>
{
    private readonly StockService _stockService;

    static ProductStockController()
    {
        LogOnChange = true;

        {
            var df = ListFields.AddListField("Move", null, "Quantity");
            df.DisplayName = "移库";
            df.Url = "ProductStock/Move?id={Id}";
        }
    }

    public ProductStockController(StockService stockService) => _stockService = stockService;

    protected override IEnumerable<ProductStock> Search(Pager p)
    {
        var id = p["id"].ToInt(-1);
        if (id > 0)
        {
            var entity = ProductStock.FindById(id);
            if (entity != null) return new[] { entity };
        }

        var productId = p["productId"].ToInt(-1);
        //var unitName = p["unitName"];
        var warehouseId = p["warehouseId"].ToInt(-1);

        var start = p["dtStart"].ToDateTime();
        var end = p["dtEnd"].ToDateTime();

        return ProductStock.Search(productId, warehouseId, start, end, p["Q"], p);
    }

    /// <summary>移库</summary>
    /// <returns></returns>
    [EntityAuthorize(PermissionFlags.Update)]
    public ActionResult Move(Int32 id) => View("Move", new MoveStockModel { Id = id });

    /// <summary>移库</summary>
    /// <returns></returns>
    [EntityAuthorize(PermissionFlags.Update)]
    public ActionResult MoveSave(MoveStockModel model)
    {
        try
        {
            var ps = ProductStock.FindById(model.Id);

            var stockModel = new StockModel
            {
                ProductId = ps.ProductId,
                WarehouseId = ps.WarehouseId,
                Quantity = ps.Quantity,
            };

            _stockService.Move(stockModel, model.WarehouseId);

            return RedirectToAction("Index", new { productId = ps.ProductId });
        }
        catch (Exception ex)
        {
            if (ex is ArgumentException aex)
                ModelState.AddModelError(aex.ParamName, aex.Message);
            else
                ModelState.AddModelError("", ex.Message);

            ViewBag.StatusMessage = "移库失败！" + ex.Message;

            WriteLog("Move", false, ex.Message);

            return View("Move", model);
        }
    }
}