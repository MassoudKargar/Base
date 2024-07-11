var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddBaseSqlDistributedCache(options =>
{
    options.TableName = builder.Configuration["Cache:TableName"] ?? "";
    options.SchemaName = builder.Configuration["Cache:SchemaName"] ?? "";
    options.ConnectionString = builder.Configuration["Cache:ConnectionString"] ?? "";
    options.AutoCreateTable = true;
});
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
