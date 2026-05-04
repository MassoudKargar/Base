namespace Base.EndPoints.Healthchecks;
public class HealthCheckPublisher : IHealthCheckPublisher
{
    public Task PublishAsync(HealthReport report, CancellationToken cancellationToken)
    {
        if (report.Status == HealthStatus.Healthy)
        {
            // ...
        }
        else
        {
            // Send email
        }

        return Task.CompletedTask;
    }
}