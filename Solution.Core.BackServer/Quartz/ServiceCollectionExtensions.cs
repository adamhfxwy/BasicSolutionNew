
namespace Solution.Core.BackServer.Quartz
{
    public static class ServiceCollectionExtensions
    {
        public static void AddJob(this IServiceCollection services,IConfiguration config)
        {
            //巡检temp表定时重置
            //string changeChargingTemplateStatusConf = config["Quartz:PatrolTempJob"];
            //if (!string.IsNullOrEmpty(changeChargingTemplateStatusConf))
            //{
            //    services.AddSingleton(new JobSchedule(
            //   jobType: typeof(PatrolTempJob), cronExpression: changeChargingTemplateStatusConf)); //每天零晨0点执行一次
            //}


        }
    }
}
