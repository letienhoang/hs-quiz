using System.Net.Mime;
using System.Text.Json;
using Examination.API;
using Examination.Application.Commands.V1.StartExam;
using Examination.Application.Mapping;
using Examination.Domain.AggregateModels.ExamAggregate;
using Examination.Domain.AggregateModels.ExamResultAggregate;
using Examination.Domain.AggregateModels.UserAggregate;
using Examination.Infrastructure.Repositoty;
using Examination.Infrastructure.SeedWork;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var appName = typeof(ExamLogger).Namespace;
var user = builder.Configuration.GetValue<string>("DatabaseSettings:User");
var password = builder.Configuration.GetValue<string>("DatabaseSettings:Password");
var server = builder.Configuration.GetValue<string>("DatabaseSettings:Server");
var databaseName = builder.Configuration.GetValue<string>("DatabaseSettings:DatabaseName");
var mongodbConnectionString = "mongodb://" + user + ":" + password + "@" + server + "/" + databaseName + "?authSource=admin";

// Create Serilog logger
var configuration = ExamLogger.GetConfiguration();
Log.Logger = ExamLogger.CreateSerilogLogger(configuration);
builder.Host.UseSerilog();

Log.Information("Starting application ({ApplicationContext}) ...", appName);

// Add services to the container.
// Version API
builder.Services.AddApiVersioning(options => {
    options.ReportApiVersions = true;
});
builder.Services.AddVersionedApiExplorer(options => {
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddSingleton<IMongoClient>(c => {
    return new MongoClient(mongodbConnectionString);
});

builder.Services.AddScoped(c => c.GetService<IMongoClient>()!.StartSession());
builder.Services.AddAutoMapper(cfg => { cfg.AddProfile(new MappingProfile()); });
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(StartExamCommandHandler).Assembly);
});

builder.Services.AddCors(options =>{
    options.AddPolicy("CorsPolicy", builder => builder.SetIsOriginAllowed((host) => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});

builder.Services.Configure<ExamSettings>(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new() { Title = "Examination.API V1", Version = "v1" });
    c.SwaggerDoc("v2", new() { Title = "Examination.API V2", Version = "v2" });
});
builder.Services.AddControllers();

// Health checks
builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy())
    .AddMongoDb(clientFactory: sp => sp.GetRequiredService<IMongoClient>(), databaseNameFactory: sp => databaseName!, name: "MongoDB", failureStatus: HealthStatus.Unhealthy);

builder.Services.AddHealthChecksUI(opt => {
    opt.SetEvaluationTimeInSeconds(15);
    opt.MaximumHistoryEntriesPerEndpoint(60);
    opt.SetApiMaxActiveRequests(1);
    opt.AddHealthCheckEndpoint("Exam API", "/hc");
})
.AddInMemoryStorage();

builder.Services.AddTransient<IExamRepository, ExamRepository>();
builder.Services.AddTransient<IExamResultRepository, ExamResultRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();

var app = builder.Build();

Log.Information("The application built successfully ({ApplicationContext})", appName);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Examination.API v1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "Examination.API v2");
    });

    app.Use(async (context, next) =>
    {
        if (context.Request.Path == "/")
        {
            context.Response.Redirect("/swagger");
        }
        else
        {
            await next();
        }
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthorization();

// Health checks
app.MapHealthChecks("/hc", new HealthCheckOptions(){
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.UseHealthChecksUI(config => {
    config.UIPath = "/hc-ui";
    config.ApiPath = "/hc-api";
});
app.MapHealthChecks("/liveness", new HealthCheckOptions{
    Predicate = r => r.Name.Contains("self")
});
app.MapHealthChecks("/hc-details", new HealthCheckOptions {
    ResponseWriter = async (context, report) =>
    {
        var result = JsonSerializer.Serialize(new{
            status = report.Status.ToString(),
            monitors = report.Entries.Select(e => new { key = e.Key, value = Enum.GetName(typeof(HealthStatus), e.Value.Status) })
        });
        context.Response.ContentType = MediaTypeNames.Application.Json;
        await context.Response.WriteAsync(result);
    }
});

app.MapControllers();

Log.Information("Starting the application ({ApplicationContext}) ...", appName);
try
{
    app.Run();
    Log.Information("The application started successfully ({ApplicationContext})", appName);
}
catch (Exception ex) 
{
    Log.Fatal(ex, "The application failed to start correctly ({ApplicationContext})", appName);
}
finally
{
    Log.CloseAndFlush();
}