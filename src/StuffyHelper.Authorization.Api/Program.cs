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

app.UseSwagger("Stuffy Authorization Service API V1");
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandling();
app.MapControllers();

app.Services.AddAuthDatabaseMigration();
app.Run();