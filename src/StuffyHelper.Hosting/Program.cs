using StuffyHelper.Api.Registration;
using StuffyHelper.Authorization.Core.Registration;
using StuffyHelper.Core.Registration;
using StuffyHelper.EntityFrameworkCore.Features.Schema;
using StuffyHelper.EntityFrameworkCore.Registration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApi(builder.Configuration);
builder.Services.AddServices(builder.Configuration);

builder.Services.AddDbStore<StuffyHelperContext>(builder.Configuration);
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

app.SeedUserData();

app.Run();
