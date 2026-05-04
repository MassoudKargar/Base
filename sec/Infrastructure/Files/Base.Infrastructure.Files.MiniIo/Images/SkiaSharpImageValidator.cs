using ImageMagick;

namespace Base.Infrastructure.Files.MiniIo.Images;
public class SkiaSharpImageValidator : IImageValidator
{
    public bool IsValid(Stream file, string contentType)
    {
        try
        {
            file.Position = 0;
            var info = new MagickImageInfo(file);
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            file.Position = 0;
        }

        return true;
    }
}
