using Constants;
using Gpio;
using Mqtt;
using Mqtt.Model;
using MQTTnet.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlLiteHelper;
using System.Data.SQLite;
using System.Device.Gpio;
using System.Text;

namespace GpioMapper
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMqttClientinterface _mqttClientinterface;
        private readonly IMqttClientModel _mqttClientModel;
        private readonly IGpioHelper _gpioHellper;
        private readonly ISQLiteHelper _sQLiteHelper;
        private JObject _json;

        public Worker(ILogger<Worker> logger, IMqttClientinterface mqttClientinterface, IMqttClientModel mqttClientModel, ISQLiteHelper sQLiteHelper)
        {
            _logger = logger;
            _mqttClientinterface = mqttClientinterface;
            _mqttClientModel = mqttClientModel;
            //_gpioHellper = gpioHellper;
            _sQLiteHelper = sQLiteHelper;
        }
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("初始化数据");
            ReadJson();
            _logger.LogInformation("链接Mqtt服务端");
            await _mqttClientinterface.Connect();
            _mqttClientinterface.MessageReceived += EventMsg;
            await _mqttClientinterface.Build();
            Console.WriteLine("开始订阅");
            await _mqttClientinterface.Sub(IotDeviceAPI.IotEventDown);
            _logger.LogInformation("开启监听");
            await DeviceMonitor();
            #region test
            //await Task.Run(() =>
            //{
            //    int val = 0;
            //    while (true)
            //    {
            //        Console.WriteLine("发送了一个消息");
            //        _mqttClientinterface.Pub("abc", $"我是测试消息{val}");
            //        Thread.Sleep(15000);
            //        val++;
            //    }

            //});
            #endregion
            Console.WriteLine("启动成功！");
        }
        private async Task EventMsg(MqttApplicationMessageReceivedEventArgs e)
        {
            _logger.LogInformation($"收到EdgeHub发送的命令");
            if (e.ApplicationMessage.Topic.Equals(IotDeviceAPI.IotEventDown))
            {
                await IotEventDown(e);
            }
        }
        public async Task IotEventDown(MqttApplicationMessageReceivedEventArgs e)
        {
            var eventPayload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            var body = JsonConvert.DeserializeObject<EventInput>(eventPayload);
            try
            {
                _logger.LogInformation($"Topic:{IotDeviceAPI.IotEventDown}开始执行命令");
                #region 命令转换成指定pin值
                SQLiteParameter[] parameters = {
                    new SQLiteParameter("@order",body?.Event)
                };
                var model = _sQLiteHelper.ExecuteDataTable<GpioPinModel>("SELECT  gp.Pin ,gp.Level from GpioOrder go2 inner join GpioPin gp on go2 .Pin ==gp.Id where go2 .Name ==@order", parameters);
                var h=model.Where(f => f.Level).Select(f => f.Pin).ToList();
                var l = model.Where(f => !f.Level).Select(f => f.Pin).ToList();
                //var pins = _json.SelectToken($"Event.{body?.Event}");
                //var disPins = JsonConvert.DeserializeObject<Dictionary<string, int[]>>(pins.ToString());
                //var h = new List<int>();
                //var l = new List<int>();
                //foreach (var item in disPins)
                //{
                //    if (item.Key == "1")
                //        h.AddRange(item.Value.ToList());
                //    else if (item.Key == "0")
                //        l.AddRange(item.Value.ToList());
                //}
                #endregion
                Console.WriteLine($"高电压：{h.Count}");
                Console.WriteLine($"低电压：{l.Count}");
                _gpioHellper.PinTransition(h, l);
                var eventoutput = JsonConvert.SerializeObject(new EventOutput() { Id = body.MessageId, Code = "succeed", Detail = "你今天造富了吗？" });
                await _mqttClientinterface.Pub(IotDeviceAPI.IotEventUp, eventoutput);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"命令执行失败");
                var eventoutput = JsonConvert.SerializeObject(new EventOutput() { Id = body.MessageId, Code = "failed", Detail = "今天无法造富了！" });
                await _mqttClientinterface.Pub(IotDeviceAPI.IotEventUp, eventoutput);
            }
        }
        public async Task DeviceMonitor()
        {
            //物模型配置监听那些pin   默认已经得到  20
            try
            {
                var pins = _json.SelectToken($"MonitoredPin");
                var monitorpins = JsonConvert.DeserializeObject<int[]>(pins.ToString());
                monitorpins = new int[] { 20 };
                _gpioHellper.OpenPin(monitorpins, PinMode.InputPullDown);
                Task.Factory.StartNew(() =>
                {
                    _gpioHellper.MonitorPin(monitorpins, TurnColour);
                });
                Console.WriteLine("监听成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"监听失败");
            }


        }
        public void TurnColour(object sender, PinValueChangedEventArgs pinValueChangedEventArgs)
        {
            Console.WriteLine("按钮被触发");
            var mesup = "{\"version\": \"1\", \"id\": \"" + Guid.NewGuid().ToString() + "\", \"dataType\": \"\", \"time\": \"" + DateTime.UtcNow.ToString("s") + "\", \"resource\": \"halo-iothub\", \"deviceId\": \"03fb0687-e254-01cd-7cd5-371a7da86967\", \"detail\": {\"resourceId\": \"03fb0687-e254-01cd-7cd5-371a7da86967\", \"thingName\": \"Led\", \"schema\": \"2022-08-24\", \"data\": [{\"ts\": " + GenerateTS() + ", \"ns\": \"SystemMetrics\", \"metrics\": [{\"name\": \"CpuUsage\", \"value\": 15, \"unit\": \"Percent\"}, {\"name\": \"SystemMemUsage\", \"value\": 10201.0, \"unit\": \"Megabytes\"}]}]}}";
            var msg = new MessageUp() { Id = Guid.NewGuid().ToString(), Timestamp = DateTime.Now, Detail = mesup };
            _mqttClientinterface.Pub(IotDeviceAPI.IotMessagesSend, JsonConvert.SerializeObject(msg)).Wait();
        }
        public static long GenerateTS()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1);
            long time = (long)ts.TotalMilliseconds;
            return time;
        }
        public string ReadJson()
        {
            var path = Directory.GetCurrentDirectory();
            using StreamReader r = new StreamReader($"{path}/initializeconfig.json");
            string jsonString = r.ReadToEnd();
            _json = JObject.Parse(jsonString);
            return jsonString;
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("停止工作");
            return Task.CompletedTask;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}