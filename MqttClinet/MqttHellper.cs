using MQTTnet;
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttClinetServer
{
    public class MqttHellper
    {
        public readonly IMqttClient _mqttClient;
        public readonly MqttClientOptions _options;
        public MqttHellper()
        {
            if (_mqttClient == null)
                _mqttClient = new MqttFactory().CreateMqttClient();
            MqttClientOptionsBuilder optionsBuilder = new MqttClientOptionsBuilder();
            optionsBuilder.WithTcpServer("127.0.0.1", 8001);
            optionsBuilder.WithCredentials("yufei", "123456");
            _options = optionsBuilder.Build();

        }

        public void OpenConment()
        {
            _mqttClient.ConnectAsync(_options);
        }
        public void OpenTheSubscription()
        {
 
        }
    }

}
