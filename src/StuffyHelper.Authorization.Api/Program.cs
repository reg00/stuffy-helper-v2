using StuffyHelper.Authorization.Api.Registration;
using StuffyHelper.Common.Configurators;
using StuffyHelper.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCustomConfiguration(builder.Configuration, builder.Environment.EnvironmentName);
builder.Services.AddControllers();
builder.Services.AddSwagger();

builder.Services.AddStuffyAuthorization(builder.Configuration);
builder.Services.AddAuthServices(builder.Configuration);
builder.Services.AddClients(builder.Configuration);

var app = builder.Build();

app.UseSwagger(c =>
{
    c.RouteTemplate = "api/swagger/{documentname}/swagger.json";
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "Stuffy Authorization Service API V1");
    c.RoutePrefix = "swagger";
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();