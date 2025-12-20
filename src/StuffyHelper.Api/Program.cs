using StuffyHelper.Api.Registration;
using StuffyHelper.Common.Configurations;
using StuffyHelper.Common.Configurators;
using StuffyHelper.Common.Extensions;
using StuffyHelper.Data.Registration;
using StuffyHelper.Data.Storage;

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

app.UseSwagger("Stuffy helper API V1");
app.UseExceptionHandling();
app.UseApi();
app.MapControllers();

app.Run();