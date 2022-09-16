using MQTTnet.Server;

namespace Mqtt
{
    public interface IMqttService
    {
        Task Start();
        Task Stop();
        Task Bulid(Func<InterceptingPublishEventArgs, Task> func);
        Task<bool> IsStart();
        Task SendMessageClientAll( string msg,string topic= null);
    }
}
