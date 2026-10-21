namespace Base.Infrastructure.Files.MiniIo.Images;
public static class ImageHelper
{
    public static bool IsImage(string contentType)
    {
        return DetectFormat(contentType).HasValue;
    }

    public static OutputFormat? DetectFormat(string contentType)
    {
        switch (contentType)
        {
            case "image/jpeg":
                return OutputFormat.Jpeg;
            case "image/png":
                return OutputFormat.Png;
            case "image/gif":
                return OutputFormat.Gif;
            case "image/bmp":
                return OutputFormat.Bmp;
            case "image/webp":
                return OutputFormat.WebP;
            default:
                break;
        }

        return null;
    }
}
