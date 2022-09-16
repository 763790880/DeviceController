using Autofac;

namespace Mqtt
{
    public class MqttModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<MqttService>()
                .As<IMqttService>()
                .SingleInstance();
            builder.RegisterType<MqttClient>()
                .As<IMqttClientinterface>()
                .SingleInstance();
            builder.RegisterType<MqttServerModel>()
             .As<IMqttServerModel>()
             .SingleInstance();
            builder.RegisterType<MqttClientModel>()
             .As<IMqttClientModel>()
             .SingleInstance();
            
        }
    }
}
