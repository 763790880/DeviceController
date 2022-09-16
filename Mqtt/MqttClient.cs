using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using System.Text;

namespace Mqtt
{
    public class MqttClient: IMqttClientinterface
    {
        public readonly IMqttClient _mqttClient;
        private readonly MqttClientOptions _options;
        private readonly ILogger<MqttService> _logger;
        private readonly IMqttClientModel _mqttClientModel;
        public event Func<MqttApplicationMessageReceivedEventArgs, Task> MessageReceived;
        public event Func<MqttClientConnectedEventArgs, Task> Connected;
        public event Func<MqttClientDisconnectedEventArgs, Task> Disconnected;

        private int _retry = 0;
        public MqttClient(ILogger<MqttService> logger, IMqttClientModel mqttClientModel)
        {
            _logger = logger;
            if (_mqttClient == null)
                _mqttClient = new MqttFactory().CreateMqttClient();
            MqttClientOptionsBuilder optionsBuilder = new MqttClientOptionsBuilder();
            optionsBuilder.WithTcpServer(mqttClientModel.Endpoint, mqttClientModel.Port);
            optionsBuilder.WithCredentials(mqttClientModel.ClientId, mqttClientModel.Password);
            optionsBuilder.WithClientId(mqttClientModel.ClientId);
            //optionsBuilder.WithCleanSession(true);
            _options = optionsBuilder.Build();
            _mqttClientModel = mqttClientModel;
        }
        public async Task Build()
        {
            
            _mqttClient.ApplicationMessageReceivedAsync += MessageReceived;
            _mqttClient.ConnectedAsync += Connected?? MqttClient_ClientConnected;
            _mqttClient.DisconnectedAsync += Disconnected?? MqttClient_ClientDisconnected;
        }
        public async Task Connect()
        {
            try
            {
                await _mqttClient.ConnectAsync(_options);
                if (_mqttClient.IsConnected)
                    _retry = 0;
            }
            catch (Exception ex)
            {

                throw;
            }
      
        }
        public bool IsConnected => _mqttClient?.IsConnected==true;
        public async Task Sub(string topic )
        {
            try
            {
                await _mqttClient.SubscribeAsync(new MqttClientSubscribeOptions() { TopicFilters = new List<MQTTnet.Packets.MqttTopicFilter>() { new MQTTnet.Packets.MqttTopicFilter() { Topic = topic } } });
            }
            catch (Exception ex)
            {

                throw;
            }
         }
        public async Task Pub(string topic,string message)
        {
            try
            {
                var mqttMsg = new MqttApplicationMessage();
                mqttMsg.Topic = topic;
                mqttMsg.Payload = Encoding.UTF8.GetBytes(message);
                mqttMsg.QualityOfServiceLevel = MqttQualityOfServiceLevel.AtLeastOnce;
                await _mqttClient.PublishAsync(mqttMsg);
                Console.WriteLine("上报EMQX成功！");
                Console.WriteLine($"消息体格式是：{message}");
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private Task MqttClient_ClientConnected(MqttClientConnectedEventArgs e)
        {
            Console.WriteLine($"链接成功");
            return Task.CompletedTask;
        }

        private async Task MqttClient_ClientDisconnected(MqttClientDisconnectedEventArgs e)
        {
            if (_retry > 3)
            {
                Console.WriteLine("断链重试超3次");
            }
            else
            {
                //await Connect();
                Console.WriteLine("断线重连成功");
            }
            _retry++;
        }

    }
}
