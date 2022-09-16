using MQTTnet.Client;

namespace Mqtt
{
    public interface IMqttClientinterface
    {
        event Func<MqttApplicationMessageReceivedEventArgs, Task> MessageReceived;
        event Func<MqttClientConnectedEventArgs, Task> Connected;
        event Func<MqttClientDisconnectedEventArgs, Task> Disconnected;
        Task Build();
        Task Connect();
        Task Sub(string topic);
        Task Pub(string topic, string message);

    }
}
