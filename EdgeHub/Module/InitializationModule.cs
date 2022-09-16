using Autofac;

namespace EdgeHub
{
    public class InitializationModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<InitializationData>()
                .As<IInitializationData>()
                .SingleInstance();
        }
    }
}
