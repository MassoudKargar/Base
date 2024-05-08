using Base.Extensions.DependencyInjection;
using Base.Utilities.SerilogRegistration.Extensions;
using Base.Utilities.SerilogRegistration.Sample;
using Base.Utilities.SerilogRegistration.Sample.SampleEnrichers;
SerilogExtensions.RunWithSerilogExceptionHandling(() =>
{
    var builder = WebApplication.CreateBuilder(args);
    var app = builder.AddBaseSerilog(c =>
    {
        c.ApplicationName = "SerilogRegistration";
        c.ServiceName = "SampleService";
        c.ServiceVersion = "1.0";
        c.ServiceId= Guid.NewGuid().ToString();
    },typeof(Sample01Enricher),typeof(Sample02Enricher)).ConfigureServices().ConfigurePipeline();
    app.Run();
});
