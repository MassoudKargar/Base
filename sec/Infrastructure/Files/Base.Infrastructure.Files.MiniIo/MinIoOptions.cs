namespace Base.Infrastructure.Files.MiniIo;

public class MinIoOptions
{
    public string Endpoint { get; set; }

    public string AccessKey { get; set; }

    public string SecretKey { get; set; }

    public bool ImageCompression { get; set; }

    public int ImageCompressionLevel { get; set; }

    public bool LosslessImageCompression { get; set; }

    public bool SSL { get; set; }

    public bool MimeDetective { get; set; }
}