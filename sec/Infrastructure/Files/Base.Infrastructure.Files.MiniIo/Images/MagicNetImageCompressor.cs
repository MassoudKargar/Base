using ImageMagick;

using System.IO;

namespace Base.Infrastructure.Files.MiniIo.Images;

public class MagicNetImageCompressor(ILogger<MagicNetImageCompressor> logger) : IImageLossLessCompressor
{
    public Stream Compression(Stream file, string contentType)
    {
        var result = new MemoryStream();
        var optimizer = new ImageOptimizer();

        try
        {
            file.CopyTo(result);

            result.Position = 0;

            var lossLessResult = optimizer.LosslessCompress(result);

            if (lossLessResult)
            {
                result.Position = 0;
            }
            else
            {
                IsImage(result: result);
            }

            return result;
        }
        catch (Exception ex)
        {
            file.Seek(offset: 0, origin: SeekOrigin.Begin);

            logger.LogError(exception: ex, message: "در فرآیند فشرده سازی فایل، خطایی بوجود آمده است");
        }

        return file;
    }

    private static void IsImage(MemoryStream result)
    {
        try
        {
            result.Position = 0;

            var info = new MagickImageInfo(result);
        }
        finally
        {
            result.Position = 0;
        }
    }
}
