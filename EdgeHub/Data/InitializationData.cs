using Constants;
using Mqtt;
using MQTTnet.Client;
using MQTTnet.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace EdgeHub
{
    public class InitializationData : IInitializationData
    {
        private IMqttService _mqttService;
        private readonly IMqttClientinterface _mqttClientinterface;
        private readonly IMqttClientModel _mqttClientModel;
        private IMqttServerModel _mqttServerModel;
        private ILogger<InitializationData> _logger;
        public InitializationData(IMqttService mqttService, IMqttClientinterface mqttClientinterface, IMqttClientModel mqttClientModel, IMqttServerModel mqttServerModel, ILogger<InitializationData> logger)
        {
            _mqttService = mqttService;
            _mqttClientinterface = mqttClientinterface;
            _mqttClientModel = mqttClientModel;
            _mqttServerModel = mqttServerModel;
            _logger = logger;
        }
        public void Initialization()
        {
            //控制线程数
            //throw new NotImplementedException();
        }
        public void CretaClientService()
        {

        }
        public void CretaServerService()
        {
            //启动服务
            _mqttService.Start();
            //监听所有真实设备的callback（执行结果）
            _mqttService.Bulid(CallBack);

        }
        /// <summary>
        /// Client消息处理结果监听
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private async Task CallBack(InterceptingPublishEventArgs arg)
        {
            _logger.LogInformation($"收到{arg.ClientId}的执行结果");
            switch (arg.ApplicationMessage.Topic)
            {
                case IotDeviceAPI.IotEventUp:
                    _logger.LogInformation($"准备把Event执行结果通知到EMQX");
                    await _mqttClientinterface.Pub(String.Format(IotDeviceAPI.CommandsUp, _mqttClientModel.ClientId) + Guid.NewGuid().ToString(), Encoding.UTF8.GetString(arg.ApplicationMessage.Payload));
                    break;
                case IotDeviceAPI.IotMessagesSend:
                    _logger.LogInformation("准备消息上报MessageUp");
                    await _mqttClientinterface.Pub(String.Format(IotDeviceAPI.MessagesSend, _mqttClientModel.ClientId), JObject.Parse(Encoding.UTF8.GetString(arg.ApplicationMessage.Payload))?.SelectToken("Detail")?.ToString()??"");
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 创建EMQX链接
        /// </summary>
        public async Task CretaEmqxLink()
        {
            _mqttClientinterface.MessageReceived += ClientInform;
            await _mqttClientinterface.Build();
            await _mqttClientinterface.Connect();
            //开始监听服务端的topic
            await Event_Message_Down();
            Console.WriteLine("与EMQX链接成功！");
        }
        /// <summary>
        /// Emqx监听回调事件
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private async Task ClientInform(MqttApplicationMessageReceivedEventArgs e)
        {
            _logger.LogInformation($"收到EMQX消息：客户端：{e.ClientId}Topic:{e.ApplicationMessage.Topic}");
            var topic = e.ApplicationMessage.Topic;
            //需要加入数据校验
            //模拟消息body={MessageId:"123456","Event":"red"}
            if (topic.Equals(string.Format(IotDeviceAPI.EventDown, _mqttClientModel.ClientId)))
            {
                if (await _mqttService.IsStart())
                {
                    //数据逻辑处理
                    //收到IOT命令下发给所有的真实设备
                    _logger.LogInformation($"准备发送命令给设备");
                    await _mqttService.SendMessageClientAll(Encoding.UTF8.GetString(e.ApplicationMessage.Payload) ?? "");
                }
            }
        }
        public void MessageUp()
        {

        }
        /// <summary>
        /// 监听Event与Mesg
        /// </summary>
        /// <returns></returns>
        public async Task Event_Message_Down()
        {
            var topics = new string[] {
            IotDeviceAPI.EventDown,
            IotDeviceAPI.MessagesDown
            };
            foreach (var topic in topics)
            {
                await _mqttClientinterface.Sub(string.Format(topic, _mqttClientModel.ClientId));
            }
        }
        public void Text()
        {
            int val = 0;
            while (true)
            {
                Task.Delay(1000).Wait();
                _mqttService.SendMessageClientAll("abc", $"服务端终极命令开始变身！！！！！{val}");
                val++;
            }

        }
    }
}
