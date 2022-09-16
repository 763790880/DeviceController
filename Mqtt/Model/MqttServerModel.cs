using Constants;
using Microsoft.Extensions.Configuration;
using Utils;
namespace Mqtt
{
    public class MqttServerModel: IMqttServerModel
    {
        public string Endpoint { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Topic { get; set; }
        public string EventDownTopic { get; set; }
        public string CallBackTopic { get; set; }
        public MqttServerModel(IConfiguration configuration)
        {
            Endpoint = configuration[ConstantsMqttServer.Endpoint];
            Port = configuration[ConstantsMqttServer.Port].ToInt32();
            UserName = configuration[ConstantsMqttServer.UserName];
            Password = configuration[ConstantsMqttServer.Password];
            Topic = configuration[ConstantsMqttServer.Topic];
            EventDownTopic = configuration[ConstantsMqttServer.EventDownTopic];
            CallBackTopic = configuration[ConstantsMqttServer.CallBackTopic];
        }
    }
}
