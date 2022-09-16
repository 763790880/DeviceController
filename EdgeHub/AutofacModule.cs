using Autofac;
using Mqtt;

namespace EdgeHub
{
    public class AutofacModule
    {
        public static void RegisterServices(HostBuilderContext context, ContainerBuilder containerBuilder)
        {
            RegisterAssemblyModules(containerBuilder);
        }
        private static void RegisterAssemblyModules(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyModules(
                typeof(AutofacModule).Assembly,
                typeof(MqttModule).Assembly
                );
        }
    }
}
