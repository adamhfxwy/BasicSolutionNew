
using Solution.Core.BackServer.BackService;
using Solution.Core.BackServer.Quartz;
using Solution.Core.CommonHelper;
using Solution.Core.Infrastructure;
using Solution.Core.Infrastructure.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MobileAPI;
using MobileAPI.Filters;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
     .AddNewtonsoftJson(options =>
     {
         options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
         //options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
         //options.SerializerSettings.ContractResolver = new DefaultContractResolver();
     });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddCors(options =>
//{
//    options.AddDefaultPolicy(builder =>
//    {
//        builder.AllowAnyOrigin()
//               .AllowAnyMethod()
//               .AllowAnyHeader();
//    });
//});
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme//
    {
        Description = "token!",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme{
                Reference =new OpenApiReference{
                    Type = ReferenceType.SecurityScheme,
                    Id ="Bearer"
                }
            },new string[]{ }
        }
    });
    options.SwaggerDoc("Identity", new OpenApiInfo
    {
        Version = "Identity",
        Title = "员工、权限相关",
        Description = "员工、权限相关"
    });
    options.SwaggerDoc("Common", new OpenApiInfo
    {
        Version = "Common",
        Title = "公共接口",
        Description = "公共接口"
    });
    options.SwaggerDoc("DeviceMaintain", new OpenApiInfo
    {
        Version = "DeviceMaintain",
        Title = "设备相关",
        Description = "设备相关"
    });//SmartLighting
    options.SwaggerDoc("Patrol", new OpenApiInfo
    {
        Version = "Patrol",
        Title = "巡检",
        Description = "巡检"
    });
    options.DocInclusionPredicate((docName, apiDes) =>
    {
        if (!apiDes.TryGetMethodInfo(out MethodInfo method))
            return false;
        /*使用ApiExplorerSettingsAttribute里面的GroupName进行特性标识
         * DeclaringType只能获取controller上的特性
         * 我们这里是想以action的特性为主
         * */
        var version = method.DeclaringType.GetCustomAttributes(true).OfType<ApiExplorerSettingsAttribute>().Select(m => m.GroupName);
        if (docName == "v1" && !version.Any())
            return true;
        //这里获取action的特性
        var actionVersion = method.GetCustomAttributes(true).OfType<ApiExplorerSettingsAttribute>().Select(m => m.GroupName);
        if (actionVersion.Any())
            return actionVersion.Any(v => v == docName);
        return version.Any(v => v == docName);
    });
    options.EnableAnnotations();
    string basePath = AppDomain.CurrentDomain.BaseDirectory;
    DirectoryInfo d = new DirectoryInfo(basePath);
    FileInfo[] files = d.GetFiles("*.xml");
    var xmls = files.Select(a => Path.Combine(basePath, a.FullName)).ToList();
    foreach (var item in xmls)
    {
        options.IncludeXmlComments(item, true);
    }
});
var assemblies = ReflectionHelper.GetAllReferencedAssemblies();
builder.Services.RunModuleInitializers(assemblies);
builder.Services.AddDbContext<NpgSqlContext>(
            opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("NpgSqlConnection")));
var jwtParam = new JwtParam();
builder.Configuration.Bind("JWT", jwtParam);
//Configuration.GetSection("JwtParam");
builder.Services.AddSingleton(jwtParam);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(opt =>
 {
     opt.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuerSigningKey = jwtParam.ValidateIssuerSigningKey,
         IssuerSigningKey =
             new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtParam.ValidIssuerSigningKey)),
         ValidateIssuer = jwtParam.ValidateIssuer,
         ValidIssuer = jwtParam.ValidIssuer,
         ValidateAudience = jwtParam.ValidateAudience,
         ValidAudience = jwtParam.ValidAudience,
         ValidateLifetime = jwtParam.ValidateLifetime,
         ClockSkew = TimeSpan.FromMinutes(jwtParam.ValidLifetime)
     };
 });
builder.Configuration.AddJsonFile("appsettings.json", true, true);
builder.Configuration.AddCommandLine(args);

builder.Services.Configure<MvcOptions>(options =>
{
    options.Filters.Add<TransactionScopeFilter>();
    options.Filters.Add<MyExceptionFilter>();
    options.Filters.Add<MyActionFilter>();
    options.Filters.Add<AuthorilizeFilter>();
    options.Filters.Add<OperationLogFilter>();
});


builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IMemoryCacheHelper, MemoryCacheHelper>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//builder.Services.AddHostedService<BackService>();

//Quartz开始
builder.Services.AddSingleton<IJobFactory, SingletonJobFactory>();
builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

// Add our job

builder.Services.AddJob(builder.Configuration);

builder.Services.AddHostedService<QuartzHostedService>();
//Quartz结束
builder.Services.AddAutoMapper(typeof(MappingProfile));

// 注册 OperationLogApplication
//builder.Services.AddScoped<OperationLogEventService>();
//builder.Services.AddScoped<AuthorilizeEventService>();
// 在这里订阅事件
//OperationLogFilter.LogActionEvent += builder.Services.BuildServiceProvider().GetRequiredService<OperationLogEventService>().AddLog;
//AuthorilizeFilter.AuthorilizFuncEvent += builder.Services.BuildServiceProvider().GetRequiredService<AuthorilizeEventService>().GetPermissionByRoleIdAsync;
var app = builder.Build();
// 使用作用域进行事件订阅
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;

//    var operationLogEventService = services.GetRequiredService<OperationLogEventService>();
//    // var authorilizeEventService = services.GetRequiredService<AuthorilizeEventService>();

//    OperationLogFilter.LogActionEvent += operationLogEventService.AddLog;
//}
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseRouting();
//app.UseCors();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/Identity/swagger.json", "员工、权限相关");
    c.SwaggerEndpoint("/swagger/Common/swagger.json", "公共接口");
    c.SwaggerEndpoint("/swagger/DeviceMaintain/swagger.json", "设备相关");
    c.SwaggerEndpoint("/swagger/Patrol/swagger.json", "巡检");
});
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
