namespace Base.Infrastructure.Files.MiniIo.Images;

public interface IImageLossLessCompressor
{
    public Stream Compression(Stream file, string contentType);
}