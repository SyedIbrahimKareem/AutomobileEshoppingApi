using EShoppingAutoMobiles;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var startup = new Startup(builder.Configuration);
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
startup.ConfigureServices(builder.Services); // calling ConfigureServices method
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
startup.Configure(app, builder.Environment);
app.UseMiddleware<ExceptionMiddleware>();
app.Run();