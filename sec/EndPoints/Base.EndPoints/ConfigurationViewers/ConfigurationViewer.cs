using Microsoft.VisualBasic;

using System.Text.Json;
using System.Text.RegularExpressions;
namespace Base.EndPoints.ConfigurationViewers;

public static partial class ConfigurationViewer
{
    private static Regex passwordRegex = new Regex("(?<=password=).+?(?=(;|'|\"|$))", RegexOptions.IgnoreCase);
    private static Regex passwordInUrlRegex = new Regex("(?<=:\\/\\/[^:]*?:)[^@\\s]*(?=@)", RegexOptions.IgnoreCase);
    private static string MaskValue = "******";


    public static string GetConfigView(this IConfigurationRoot root, ConfigurationViewerType? viewType = null)
    {
        var result = new Dictionary<string, object>();
        switch (viewType)
        {
            case ConfigurationViewerType.All:
            case null:
                GetChildObject(root, root.GetChildren(), result, debugMode: false, new string[0]);
                break;
            case ConfigurationViewerType.Normal:
                GetChildObject(root, root.GetChildren(), result, debugMode: false, IgnoreProviders);
                break;
            case ConfigurationViewerType.Providers:
                root.GetAllProvider(result);
                break;
            case ConfigurationViewerType.Debug:
                GetChildObject(root, root.GetChildren(), result, debugMode: true, IgnoreProviders);
                break;
        }

        return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
    }

    private static void GetAllProvider(this IConfigurationRoot root, Dictionary<string, object> all)
    {
        var counter = 0;
        foreach (IConfigurationProvider provider in root.Providers)
        {
            counter++;
            var providerName = counter.ToString("00") + ") " + provider.ToString() ?? provider.GetType().Name;

            var ProviderValues = new Dictionary<string, object>();

            GetChildObject(provider, null, ProviderValues);

            all[providerName] = ProviderValues;
        }
    }

    private static bool GetChildObject(IConfigurationProvider provider, string rootKey, Dictionary<string, object> ObjectDic)
    {
        var keys = provider.GetChildKeys(Enumerable.Empty<string>(), rootKey);

        foreach (var key in keys)
        {
            string surrogateKey = key;
            if (rootKey != null)
            {
                surrogateKey = rootKey + ":" + key;
            }
            var childObject = new Dictionary<string, object>();

            var isValue = GetChildObject(provider, surrogateKey, childObject);

            if (isValue)
            {
                provider.TryGet(surrogateKey, out var value);
                if (IsSensitivePropery(key))
                {
                    ObjectDic[key] = MaskValue;
                }
                else if (IsKeyPropery(key))
                {
                    ObjectDic[key] = MaskKey(value);
                }
                else
                {
                    ObjectDic[key] = MaskPassword(value);
                }
            }
            else
            {
                ObjectDic[key] = childObject;
            }

        }



        return !keys.Any();
    }

    private static void GetChildObject(IConfigurationRoot root, IEnumerable<IConfigurationSection> children,
        Dictionary<string, object> ObjectDic, bool debugMode, string[] ignoreProviders)
    {
        foreach (var child in children)
        {
            (string? Value, IConfigurationProvider? Provider) valueAndProvider = GetValueAndProvider(root, child.Path);

            var childObject = new Dictionary<string, object>();

            var isValue = valueAndProvider.Provider != null;

            GetChildObject(root, child.GetChildren(), childObject, debugMode, ignoreProviders);

            if (isValue)
            {
                if (!ignoreProviders.Contains(valueAndProvider.Provider?.GetType().Name))
                {
                    if (IsSensitivePropery(child.Key))
                    {
                        ObjectDic[child.Key] = MaskValue;
                    }
                    else if (IsKeyPropery(child.Key))
                    {
                        ObjectDic[child.Key] = MaskKey(valueAndProvider.Value);
                    }
                    else
                    {
                        ObjectDic[child.Key] = MaskPassword(valueAndProvider.Value);
                    }

                    ObjectDic[child.Key] += debugMode
                       ? $" [{valueAndProvider.Provider}]"
                       : string.Empty;
                }
            }
            else
            {
                if (childObject.Any())
                {
                    ObjectDic[child.Key] = ConvertToOrginalType(childObject);
                }
            }
        }
    }

    private static string MaskKey(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        switch (value.Length)
        {
            case <= 3:
                return ChangeCharacters(value, 1, value.Length);
            case <= 7:
                return ChangeCharacters(value, 2, value.Length - 1);
            case <= 10:
                return ChangeCharacters(value, 3, value.Length - 3);
            default:
                return ChangeCharacters(value, 3, value.Length - 5);
        }
    }

    private static string ChangeCharacters(string value, int from, int to, char newChar = '*')
    {
        from -= 1; to -= 1;
        var len = to - from;
        var newValue = "";
        for (int i = 0; i < len; i++)
        {
            newValue += newChar;
        }

        return value.Remove(from, len).Insert(from, newValue);
    }

    private static bool IsKeyPropery(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var keyNames = new[] { "key", "accesskey", "secretkey", "xapikey", "apikey" };

        if (keyNames.Contains(value.ToLower()))
            return true;

        return false;
    }

    private static bool IsSensitivePropery(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        value = value.ToLower().Trim();
        var sensitiveNames = new[] { "pwd", "password", "pass" };

        if (sensitiveNames.Contains(value))
            return true;

        var containsNames = new[] { "pwd", "password" };

        foreach (var name in containsNames)
        {
            if (value.Contains(name))
                return true;
        }

        return false;
    }

    private static string MaskPassword(string? value)
    {
        if (value is null)
            return string.Empty;

        value = passwordRegex.Replace(value, MaskValue);

        value = passwordInUrlRegex.Replace(value, MaskValue);

        return value;
    }

    private static object ConvertToOrginalType(Dictionary<string, object> childObject)
    {
        var isAllKeyNumber = childObject.Keys.All(Information.IsNumeric);

        if (!isAllKeyNumber)
            return childObject;

        return childObject.Select(c => c.Value).ToArray();

    }

    private static (string? Value, IConfigurationProvider? Provider) GetValueAndProvider(
         IConfigurationRoot root,
         string key)
    {
        foreach (IConfigurationProvider provider in root.Providers.Reverse())
        {
            if (provider.TryGet(key, out string? value))
            {
                return (value, provider);
            }
        }

        return (null, null);
    }

    private static HashSet<string> GetFullKeyNames(this IConfigurationProvider provider, string rootKey, HashSet<string> initialKeys)
    {
        foreach (var key in provider.GetChildKeys(Enumerable.Empty<string>(), rootKey))
        {
            string surrogateKey = key;
            if (rootKey != null)
            {
                surrogateKey = rootKey + ":" + key;
            }

            provider.GetFullKeyNames(surrogateKey, initialKeys);

            if (!initialKeys.Any(k => k.StartsWith(surrogateKey)))
            {
                initialKeys.Add(surrogateKey);
            }
        }

        return initialKeys;
    }

    private static string[] IgnoreProviders = new[] {
        "EnvironmentVariablesConfigurationProvider",
        "MemoryConfigurationProvider",
        "ChainedConfigurationProvider"
    };
}
