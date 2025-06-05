using StuffyHelper.Api.Registration;
using StuffyHelper.Common.Configurations;
using StuffyHelper.Common.Configurators;
using StuffyHelper.Common.Extensions;
using StuffyHelper.Core.Registration;
using StuffyHelper.EntityFrameworkCore.Features.Schema;
using StuffyHelper.EntityFrameworkCore.Registration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCustomConfiguration(builder.Configuration, builder.Environment.EnvironmentName);
builder.Services.AddControllers();
builder.Services.AddSwagger();

builder.Services.AddStuffyAuthorization(builder.Configuration);
builder.Services.AddApi(builder.Configuration);
builder.Services.AddServices(builder.Configuration);
builder.Services.AddClients(builder.Configuration);

builder.Services.AddDbStore<StuffyHelperContext>(builder.Configuration.GetSection(StuffyConfiguration.DefaultSection));
builder.Services.AddEfDbServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger(c =>
{
    c.RouteTemplate = "api/swagger/{documentname}/swagger.json";
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "Stuffy helper API V1");
    c.RoutePrefix = "api/swagger";
});

app.UseExceptionHandling();
app.UseApi();
app.MapControllers();

app.Run();