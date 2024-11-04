using StuffyHelper.Common.Configurators;
using StuffyHelper.Common.Extensions;
using StuffyHelper.EmailService.Core.Registration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCustomConfiguration(builder.Configuration, builder.Environment.EnvironmentName);
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwagger();

builder.Services.AddEmailService(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
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