namespace Mqtt
{
    public interface IMqttServerModel
    {
        string Endpoint { get; set; }
        int Port { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        string Topic { get; set; }
        
    }
}
