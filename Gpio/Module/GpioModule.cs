using Autofac;

namespace Gpio
{
    public class GpioModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<GpioHelper>()
                .As<IGpioHelper>()
                .SingleInstance();
        }
    }
}
