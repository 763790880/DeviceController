namespace Mqtt
{
    public interface IMqttClientModel
    {
        string Endpoint { get; set; }
        int Port { get; set; }
        string ClientId { get; set; }
        string Password { get; set; }
    }
}
