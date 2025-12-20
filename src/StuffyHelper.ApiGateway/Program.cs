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

app.UseSwagger("Stuffy helper API Gateway V1");
app.UseApi();
app.UseExceptionHandling();
app.MapControllers();

app.Run();