using Autofac;
using Gpio;
using Mqtt;
using SqlLiteHelper;

namespace GpioMapper
{
    public class AutofacModule
    {
        public static void RegisterServices(HostBuilderContext context, ContainerBuilder containerBuilder)
        {
            RegisterAssemblyModules(containerBuilder);
        }
        private static void RegisterAssemblyModules(ContainerBuilder containerBuilder)
        {
            //ABB.Ability.Cloud.DCS.PlatformEventProcessor.Core.Modules
            containerBuilder.RegisterAssemblyModules(
                typeof(AutofacModule).Assembly,
                typeof(GpioModule).Assembly,
                typeof(SqliteModule).Assembly,
                typeof(MqttModule).Assembly
                );
        }
    }
}
