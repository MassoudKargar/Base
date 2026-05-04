namespace Base.Infrastructure.Files.MiniIo.Images;

public interface IImageCompressor
{
    public Stream Compression(Stream file, string contentType, int compressQuality);
}
