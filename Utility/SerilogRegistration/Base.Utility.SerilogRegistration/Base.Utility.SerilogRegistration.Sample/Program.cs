using Base.Extensions.DependencyInjection;
using Base.Utilities.SerilogRegistration.Extensions;
using Base.Utility.SerilogRegistration.Sample;
using Base.Utility.SerilogRegistration.Sample.SampleEnrichers;
SerilogExtensions.RunWithSerilogExceptionHandling(() =>
{
    var builder = WebApplication.CreateBuilder(args);
    var app = builder.AddBaseSerilogWithElastic(
        builder.Configuration["ElasticConfiguration:Uri"],
        c =>
    {
        c.ApplicationName = "SerilogRegistration";
        c.ServiceName = "SampleService";
        c.ServiceVersion = "1.0";
        c.ServiceId = Guid.NewGuid().ToString();
    }).ConfigureServices().ConfigurePipeline();
    app.Run();
});
