using StuffyHelper.ApiGateway.Registration;
using StuffyHelper.Common.Configurators;
using StuffyHelper.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCustomConfiguration(builder.Configuration, builder.Environment.EnvironmentName);
builder.Services.AddControllers();
builder.Services.AddSwagger();

builder.Services.AddStuffyAuthorization(builder.Configuration);
builder.Services.AddApi(builder.Configuration);
builder.Services.AddServices(builder.Configuration);
builder.Services.AddClients(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger(c =>
{
    c.RouteTemplate = "api/swagger/{documentname}/swagger.json";
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "Stuffy helper API Gateway V1");
    c.RoutePrefix = "api/swagger";
});

app.UseApi();
app.MapControllers();

app.Run();