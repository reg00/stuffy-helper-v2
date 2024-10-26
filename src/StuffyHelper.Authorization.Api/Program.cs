using StuffyHelper.Authorization.Api.Registration;
using StuffyHelper.Common.Configurators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCustomConfiguration(builder.Environment.EnvironmentName);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthServices(builder.Configuration);
builder.Services.AddClients(builder.Configuration);

var app = builder.Build();

app.UseSwagger(c =>
{
    c.RouteTemplate = "api/swagger/{documentname}/swagger.json";
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "Stuffy Email Service API V1");
    c.RoutePrefix = "swagger";
});

app.MapControllers();
app.Run();