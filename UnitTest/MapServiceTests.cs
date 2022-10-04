using NewLife.Configuration;
using NewLife.ERP.Services;
using NewLife.Log;
using NewLife.Yun;

namespace UnitTest;

public class MapServiceTests
{
    IMap _map;
    public MapServiceTests()
    {
        var config = new JsonConfigProvider { FileName = "appsettings.json" };
        //config.LoadAll();

        var map = new BaiduMap
        {
            Log = XTrace.Log,
            AppKey = config["MapKey"],
        };
        _map = map;
    }

    [Fact]
    public void Test1()
    {
        var ms = new MapService(_map);

        var address = "上海市古美路1068弄7号";
        var addr = ms.Parse(address);

        Assert.NotNull(addr);
        Assert.Equal("上海市徐汇区古美路1090号东兰小区内", addr.Address);
        Assert.Equal(310104, addr.Code);
        Assert.Equal(310104012, addr.Towncode);
    }
}