using Base.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddBaseApiAuthentication(builder.Configuration, "ApiAuthentication");
builder.Services.AddBaseSwagger(builder.Configuration, "Swagger");

var app = builder.Build();

app.UseBaseSwagger();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapControllers().RequireAuthorization();

app.Run();