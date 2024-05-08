using Base.Extensions.DependencyInjection;
using Base.Utilities.SoftwarePartDetector.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddBaseMicrosoftSerializer();

builder.Services.AddSoftwarePartDetector(builder.Configuration, "SoftwarePart");

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Services.GetService<SoftwarePartDetectorService>()?.Run();

app.Run();
