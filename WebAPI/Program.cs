
using Solution.Core.Infrastructure.Utils;

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
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme //
    {
        Description = "token!",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
    options.SwaggerDoc("Identity", new OpenApiInfo
    {
        Version = "Identity",
        Title = "身份验证",
        Description = "身份验证"
    });
    options.SwaggerDoc("Common", new OpenApiInfo
    {
        Version = "Common",
        Title = "公共接口",
        Description = "公共接口"
    });
    options.SwaggerDoc("CustomerMgt", new OpenApiInfo
    {
        Version = "CustomerMgt",
        Title = "客户管理",
        Description = "客户管理"
    }); //SmartLighting
    options.DocInclusionPredicate((docName, apiDes) =>
    {
        if (!apiDes.TryGetMethodInfo(out MethodInfo method))
            return false;

        var version = method.DeclaringType.GetCustomAttributes(true).OfType<ApiExplorerSettingsAttribute>()
            .Select(m => m.GroupName);
        if (docName == "v1" && !version.Any())
            return true;
        var actionVersion = method.GetCustomAttributes(true).OfType<ApiExplorerSettingsAttribute>()
            .Select(m => m.GroupName);
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
    opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection")));
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

//Quartz��ʼ
builder.Services.AddSingleton<IJobFactory, SingletonJobFactory>();
builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

// Add our job
builder.Services.AddJob(builder.Configuration);
builder.Services.AddHostedService<QuartzHostedService>();
//Quartz����
builder.Services.AddAutoMapper(typeof(MappingProfile));

// ע�� OperationLogApplication
//builder.Services.AddScoped<OperationLogEventService>();
//builder.Services.AddScoped<AuthorilizeEventService>();
// �����ﶩ���¼�
//OperationLogFilter.LogActionEvent += builder.Services.BuildServiceProvider().GetRequiredService<OperationLogEventService>().AddLog;
//AuthorilizeFilter.AuthorilizFuncEvent += builder.Services.BuildServiceProvider().GetRequiredService<AuthorilizeEventService>().GetPermissionByRoleIdAsync;
// ʹ������������¼�����

var app = builder.Build();
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;

//    var operationLogEventService = services.GetRequiredService<OperationLogEventService>();
//    var authorilizeEventService = services.GetRequiredService<AuthorilizeEventService>();

//    OperationLogFilter.LogActionEvent += operationLogEventService.AddLog;
//}
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseRouting();
app.UseCors();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/Identity/swagger.json", "身份验证");
    c.SwaggerEndpoint("/swagger/Common/swagger.json", "公共接口");
    c.SwaggerEndpoint("/swagger/CustomerMgt/swagger.json", "客户管理");
});
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();