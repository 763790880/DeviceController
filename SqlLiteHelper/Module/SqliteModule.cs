using Autofac;

namespace SqlLiteHelper
{
    public class SqliteModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<SQLiteHelper>()
                .As<ISQLiteHelper>()
                .SingleInstance();
        }
    }
}
