using MimeDetective;

namespace Base.Infrastructure.Files.MiniIo;

public interface IValidateFileInformationService
{
    bool IsMimeTypeValid(byte[]? bytes, string? fileMimeType);
    bool IsExtensionValid(byte[]? bytes, string? fileExtension);
}

internal class ValidateFileInformationService : IValidateFileInformationService
{
    private readonly static IContentInspector _inspector;

    static ValidateFileInformationService()
    {
        _inspector = new ContentInspectorBuilder()
        {
            Definitions = new MimeDetective.Definitions.ExhaustiveBuilder()
            {
                UsageType = MimeDetective.Definitions.Licensing.UsageType.PersonalNonCommercial
            }.Build()
        }.Build();
    }

    private readonly ILogger<ValidateFileInformationService> _logger;

    public ValidateFileInformationService(ILogger<ValidateFileInformationService> logger)
    {
        _logger = logger;
    }

    public bool IsMimeTypeValid(byte[]? bytes, string? fileMimeType)
    {
        if (bytes == null || bytes.Length <= 0)
        {
            return false;
        }

        var result = _inspector.Inspect(bytes);

        if (result != null && result.Any() && result[0].Definition.File.MimeType != null)
        {
            var foundedMimeType = result[0].Definition.File.MimeType;

            if (fileMimeType?.ToLower() == foundedMimeType?.ToLower())
            {
                return true;
            }

            return false;
        }

        return false;
    }

    public bool IsExtensionValid(byte[]? bytes, string? fileExtension)
    {
        if (bytes == null || bytes.Length <= 0)
        {
            return false;
        }

        var result = _inspector.Inspect(bytes);

        var isValid = false;

        if (result != null && result.Any() &&
            result[0].Definition.File.Extensions != null && result[0].Definition.File.Extensions.Any())
        {
            var foundedExtensions = result[0].Definition.File.Extensions;

            if (foundedExtensions != null && foundedExtensions.Any())
            {
                for (var index = 0; index <= foundedExtensions.Length - 1; index++)
                {
                    var foundedExtension = foundedExtensions[index];

                    if (foundedExtension.StartsWith('.') == false)
                    {
                        foundedExtension = $".{foundedExtension}";
                    }

                    if (fileExtension?.ToLower() == foundedExtension?.ToLower())
                    {
                        isValid = true;

                        break;
                    }
                }
            }
        }

        return isValid;
    }
}
