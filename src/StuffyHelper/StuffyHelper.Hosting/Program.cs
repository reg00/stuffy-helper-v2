using StuffyHelper.Api.Registration;
using StuffyHelper.Authorization.Core.Registration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEckApi(builder.Configuration);

//builder.Services.AddDbStore<EckContext>(builder.Configuration);
//builder.Services.AddEfDbServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger(c =>
{
    c.RouteTemplate = "api/swagger/{documentname}/swagger.json";
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "ECK Web API V1");
    c.RoutePrefix = "api/swagger";
});

app.UseExceptionHandling();

app.UseEckApi();

app.SeedUserData();
//app.SeedEckData();

app.Run();
