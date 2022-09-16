using Constants;
using Microsoft.Extensions.Configuration;
using Utils;

namespace Mqtt
{
    public class MqttClientModel: IMqttClientModel
    {
        public string Endpoint { get; set; }
        public int Port { get; set; }
        public string ClientId { get; set; }
        public string Password { get; set; }

        public MqttClientModel(IConfiguration configuration)
        {
            Endpoint = configuration[ConstantsMqttClient.Endpoint];
            Port = configuration[ConstantsMqttClient.Port].ToInt32();
            ClientId = configuration[ConstantsMqttClient.ClientId];
            Password = configuration[ConstantsMqttClient.Password];
        }
    }
}
