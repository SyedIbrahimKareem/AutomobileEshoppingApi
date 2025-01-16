using EShoppingAutoMobiles;

var builder = WebApplication.CreateBuilder(args);
var startup = new Startup(builder.Configuration);
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