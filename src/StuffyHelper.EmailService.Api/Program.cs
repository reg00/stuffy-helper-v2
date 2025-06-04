using StuffyHelper.Common.Configurators;
using StuffyHelper.Common.Extensions;
using StuffyHelper.EmailService.Core.Registration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCustomConfiguration(builder.Configuration, builder.Environment.EnvironmentName);
builder.Services.AddControllers();
builder.Services.AddSwagger();

builder.Services.AddStuffyAuthorization(builder.Configuration);
builder.Services.AddEmailService(builder.Configuration);

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();