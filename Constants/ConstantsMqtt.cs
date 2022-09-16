namespace Constants
{
    public static class ConstantsMqttClient
    {
        private const string MqttClient= "MqttClient";
        public const string Endpoint = $"{MqttClient}:Endpoint";
        public const string Port = $"{MqttClient}:Port";
        public const string ClientId = $"{MqttClient}:ClientId";
        public const string Password = $"{MqttClient}:Password";
        public const string SubTopic = $"{MqttClient}:SubTopic";
        
    }
    public static class ConstantsMqttServer
    {
        private const string MqttServer = "MqttServer";
        public const string Endpoint = $"{MqttServer}:Endpoint";
        public const string Port = $"{MqttServer}:Port";
        public const string UserName = $"{MqttServer}:UserName";
        public const string Password = $"{MqttServer}:Password";
        public const string Topic = $"{MqttServer}:Topic";
        public const string CallBackTopic = $"{MqttServer}:CallBackTopic";
        public const string EventDownTopic = $"{MqttServer}:EventDownTopic";
    }
}