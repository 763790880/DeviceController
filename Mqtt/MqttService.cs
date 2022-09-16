using Constants;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;
using System.Text;

namespace Mqtt
{
    public class MqttService: IMqttService
    {
        private readonly ILogger<MqttService> _logger;
        private MqttServer _mqttServer;
        private MqttServerOptions _serverOptions;
        private IMqttServerModel _mqttServerModel;
        public MqttService(ILogger<MqttService> logger, IMqttServerModel mqttServerModel)
        {
            _logger = logger;
            if (_mqttServer == null)
            {
                var optionsBuilder = new MqttServerOptionsBuilder()
                .WithDefaultEndpoint()
                .WithPersistentSessions()
                .WithDefaultEndpointPort(mqttServerModel.Port);
                _serverOptions = optionsBuilder.Build();
                _mqttServer = new MqttFactory().CreateMqttServer(_serverOptions);
            }
            _mqttServerModel = mqttServerModel;
        }
        private Task MqttServer_InterceptingPublishAsync(InterceptingPublishEventArgs arg)
        {
            Console.WriteLine($"我是{arg.ClientId}上报的消息：{Encoding.UTF8.GetString(arg.ApplicationMessage.Payload)}");
            return Task.CompletedTask;
        }

        public async Task Start()
        {
            try
            {
                 await _mqttServer.StartAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "服务端启动失败");
            }
        }
        public Task<bool> IsStart()
        {
            try
            {
                return Task.FromResult(_mqttServer.IsStarted);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "服务端启动失败");
                return Task.FromResult(false); ;
            }
        }
        public Task Stop()
        {
             _mqttServer.StopAsync();
            _logger.LogInformation("Mqtt服务已停止");
            return Task.CompletedTask;
        }
        public Task Bulid(Func<InterceptingPublishEventArgs, Task> func)
        {
            _mqttServer.ApplicationMessageNotConsumedAsync += MqttServer_ApplicationMessageReceived;
            _mqttServer.ClientConnectedAsync += MqttServer_ClientConnected;
            _mqttServer.ClientDisconnectedAsync += MqttServer_ClientDisconnected;
            _mqttServer.InterceptingPublishAsync += func;
            return Task.CompletedTask;
        }
        public Task SendMessageClientAll(string msg, string topic)
        {
            
            var mqttList=_mqttServer.GetClientsAsync().Result;
            foreach (var item in mqttList)
            {
                if (string.IsNullOrEmpty(topic))
                    topic = IotDeviceAPI.IotEventDown;
                var message = new MqttApplicationMessageBuilder().WithTopic(topic).WithPayload(msg)
                    .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce).WithRetainFlag(true).Build();
                // Now inject the new message at the broker.
                _mqttServer.InjectApplicationMessage(
                    new InjectedMqttApplicationMessage(message)
                    {
                        SenderClientId = item.Id
                        
                    }).Wait();
            }
            return Task.CompletedTask;
        }

        private Task MqttServer_ClientConnected(ClientConnectedEventArgs e)
        {
            Console.WriteLine($"客户端[{e.ClientId}]已连接，协议版本：{e.UserName}");
            Task.Delay(1000).Wait();
            
            return Task.CompletedTask;
        }

        private Task MqttServer_ClientDisconnected(ClientDisconnectedEventArgs e)
        {
            Console.WriteLine($"客户端[{e.ClientId}]已断开连接！");
            return Task.CompletedTask;
        }

        private Task MqttServer_ApplicationMessageReceived(ApplicationMessageNotConsumedEventArgs e)
        {
            Console.WriteLine($"客户端[{e.SenderId}]>> 主题：{e.ApplicationMessage.Topic} 负荷：{Encoding.UTF8.GetString(e.ApplicationMessage.Payload)} Qos：{e.ApplicationMessage.QualityOfServiceLevel} 保留：{e.ApplicationMessage.Retain}");
            return Task.CompletedTask;
        }
    }
}
