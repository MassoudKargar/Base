namespace Base.Infrastructure.Files.MiniIo.Images;

public interface IImageValidator
{
    public bool IsValid(Stream file, string contentType);
}
