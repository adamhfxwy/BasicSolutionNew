

namespace Solution.Core.Infrastructure.Utils
{
    class ModuleInitializer : IModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集
            var types = assembly.GetTypes(); // 获取程序集中所有的类型

            foreach (var type in types)
            {
                if (type.IsClass && !type.IsAbstract && typeof(IServiceSupport).IsAssignableFrom(type))
                {
                    var interfaces = type.GetInterfaces().Where(i =>
                         i != typeof(IRepository<>) &&
                         (!i.IsGenericType || !i.GetGenericTypeDefinition().IsAssignableFrom(typeof(IRepository<>))));


                    foreach (var @interface in interfaces)
                    {
                        // 注册服务
                        services.AddScoped(@interface, type);
                        break;
                    }
                }
            }
            //services.AddScoped(typeof(IRepository<>), typeof(MySqlRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(NpgSqlRepository<>));
        }
    }
}
