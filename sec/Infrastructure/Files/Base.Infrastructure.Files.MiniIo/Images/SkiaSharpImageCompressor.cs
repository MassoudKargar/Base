using SkiaSharp;

namespace Base.Infrastructure.Files.MiniIo.Images;
public class SkiaSharpImageCompressor : IImageCompressor
{
    public Stream Compression(Stream file, string contentType, int compressQuality)
    {
        if (compressQuality < 0 || compressQuality > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(compressQuality));
        }

        var result = new MemoryStream();

        using (var bitmap = SKBitmap.Decode(file))
        {
            var imageFormat = ImageHelper.DetectFormat(contentType);

            using (var surface = SKSurface.Create(new SKImageInfo(bitmap.Width, bitmap.Height, bitmap.ColorType, bitmap.AlphaType)))
            using (var paint = new SKPaint() { FilterQuality = SKFilterQuality.Medium })
            {
                var canvas = surface.Canvas;
                canvas.DrawBitmap(bitmap, 0, 0, paint);
                canvas.Flush();

                switch (imageFormat)
                {
                    case OutputFormat.Jpeg:
                        surface.Snapshot()
                            .Encode(SKEncodedImageFormat.Jpeg, compressQuality)
                            .SaveTo(result);
                        break;
                    case OutputFormat.Png:
                        surface.Snapshot()
                            .Encode(SKEncodedImageFormat.Png, compressQuality)
                            .SaveTo(result);
                        break;
                    case OutputFormat.Gif:
                        surface.Snapshot()
                            .Encode(SKEncodedImageFormat.Gif, compressQuality)
                            .SaveTo(result);
                        break;
                    case OutputFormat.WebP:
                        surface.Snapshot()
                            .Encode(SKEncodedImageFormat.Webp, compressQuality)
                            .SaveTo(result);
                        break;
                    case OutputFormat.Bmp:
                        surface.Snapshot()
                          .Encode(SKEncodedImageFormat.Bmp, compressQuality)
                          .SaveTo(result);
                        break;
                    default:
                        throw new InvalidOperationException($"Format not supported:{contentType}");
                }

            }
        }
        result.Position = 0;
        return result;
    }
}
