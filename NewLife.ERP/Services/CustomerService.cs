using Erp.Data.Customers;

namespace NewLife.ERP.Services;

public class CustomerService
{
    private readonly MapService _mapService;

    public CustomerService(MapService mapService)
    {
        _mapService = mapService;
    }

    public Int32 Fix(Customer customer)
    {
        if (customer == null || customer.Address.IsNullOrEmpty()) return 0;

        var geo = _mapService.Parse(customer.Address);
        if (geo == null) return 0;

        customer.AreaCode = geo.Code;
        customer.Address2 = geo.Address;

        return customer.Update();
    }
}