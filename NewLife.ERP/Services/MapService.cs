using NewLife.Data;
using NewLife.Map;

namespace NewLife.ERP.Services;

public class MapService
{
    private readonly IMap _map;

    public MapService(IMap map)
    {
        _map = map;
    }

    public GeoAddress Parse(String address)
    {
        if (address.IsNullOrEmpty()) return null;

        var addr = _map.GetGeoAsync(address, null, null, true).Result;

        return addr;
    }
}