var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddBaseNewtonSoftSerializer();
builder.Services.AddBaseRabbitMqMessageBus(c =>
{
    c.PerssistMessage = true;
    c.ExchangeName = "MiniBlogExchange";
    c.ServiceName = "SampleApplicationReceiver";
    c.Url = @"amqp://guest:guest@localhost:5672/";
});
builder.Services.AddBaseMessageInbox(c =>
{
    c.ApplicationName = "SampleApplicationReceiver";
    //c.ConnectionString = "Server=.;Initial Catalog=InboxDb;User Id=sa; Password=1qaz!QAZ;Encrypt=false";
});
builder.Services.AddBaseMessageInboxDalSql(c =>
{
    //c.TableName = "MessageInbox";
    c.SchemaName = "dbo";
    c.ConnectionString = "Server=.;Initial Catalog=InboxDb;User Id=sa; Password=1qaz!QAZ;Encrypt=false";
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.Services.ReceiveEventFromRabbitMqMessageBus(new KeyValuePair<string, string>("SampleApplciatoin", "PersonEvent"));
//app.Services.ReceiveEventFromRabbitMqMessageBus(new KeyValuePair<string, string>("SampleApplciatoin", "PersonEvent"));
app.Services.ReceiveEventFromRabbitMqMessageBus(new KeyValuePair<string, string>("MiniBlog", "BlogCreated"));

app.UseAuthorization();

app.MapControllers();

app.Run();
