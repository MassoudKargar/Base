namespace Base.Utilities.SoftwarePartDetector.Publishers;

public interface ISoftwarePartPublisher
{
    Task PublishAsync(SoftwarePart softwarePart);
}