using Base.Core.Domains;

namespace Base.Infrastructure.Files.MiniIo.Infrastructure;

public static class FileHelper
{
    public static byte[]? ReadBytes(Stream? fileStream)
    {
        if (fileStream == null)
        {
            return null;
        }

        byte[] bytes = new byte[fileStream.Length];

        fileStream.Read(buffer: bytes, offset: 0, count: (int)fileStream.Length);

        fileStream.Seek(offset: 0, origin: SeekOrigin.Begin);

        return bytes;
    }

    public static void ValidateFileInformation
        (this Stream stream, string path, string fileName, string contentType,
        IValidateFileInformationService validateFileInformationService)
    {
        var bytes = ReadBytes(fileStream: stream);

        var extension =
            GetValidExtension(path: path, fileName: fileName);

        var isExtensionValid =
            validateFileInformationService.IsExtensionValid
            (bytes: bytes, fileExtension: extension);

        if (isExtensionValid == false)
        {
            throw new BusinessException
                (message: "پسوند فایل ارسال شده، با ماهیت فایل همخوانی ندارد");
        }

        var isMimeTypeValid =
            validateFileInformationService.IsMimeTypeValid
            (bytes: bytes, fileMimeType: contentType);

        if (isMimeTypeValid == false)
        {
            throw new BusinessException
                (message: "Mime-Type فایل ارسال شده، با ماهیت فایل همخوانی ندارد");
        }
    }

    private static string GetValidExtension(string path, string fileName)
    {
        var extension =
            Path.GetExtension(path: path);

        if (string.IsNullOrWhiteSpace(extension))
        {
            extension = fileName.Contains('.') ?
                fileName.Substring(fileName.LastIndexOf('.')) : string.Empty;
        }

        if (string.IsNullOrWhiteSpace(extension))
        {
            throw new BusinessException
                (message: "مسیر فایل ارسال شده، صحیح نمی باشد");
        }

        return extension;
    }
}
