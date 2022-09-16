// See https://aka.ms/new-console-template for more information


using DevicePOC;
using System.Device.Gpio;
using Newtonsoft.Json.Linq;
using System.Linq;
using Newtonsoft.Json;

Console.WriteLine("开始");
///三色灯引脚
var ledPin = new int[] { 13, 19, 26 };
//按钮引脚
var butPin = new int[] { 20 };
// 获取 GPIO 控制器
//using GpioController ledController = new GpioController(PinNumberingScheme.Logical);
//var _gpio = new GPIO(ledController);
//_gpio.OpenPin(ledPin, PinMode.Output);
//_gpio.OpenPin(butPin, PinMode.InputPullDown);

//Task.Run(() => {
//    _gpio.MonitorPin(butPin, _gpio.TurnColour);
//});

//Console.ReadLine();
//ledController.Dispose();

#region
try
{
    JObject json = JObject.Parse("{\"red\":{\"1\":[13],\"0\":[19,26]}}");
    var jpins = json.SelectToken("red");
    var dics=JsonConvert.DeserializeObject<Dictionary<string, int[]>>(jpins.ToString());
    Console.WriteLine(dics.ToString());
}
catch (Exception ex)
{

    throw;
}

#endregion
