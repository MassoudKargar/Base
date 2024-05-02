namespace Base.Core.Domains.Exceptions;
/// <summary>
/// Domain layer errors related to Entities and ValueObjects are sent to higher layers with the help of Extension
/// Considering that both Entity and ValueObject send the error in the same way, an Exception class has been designed and implemented.
/// In order to be able to recognize the difference between the error and its place of occurrence in the higher layers, the MicroType template is used.
/// </summary>
public abstract class DomainStateException(string message, params string[] parameters) : Exception(message)
{
    /// <summary>
    /// List of error parameters
    /// If there is a parameter, send the message as a template and the values of the parameters are placed in a special place in the template.
    /// </summary>
    public string[] Parameters { get; } = parameters;

    public override string ToString()
    {
        if (Parameters?.Length < 1)
        {
            return Message;
        }

        var result = Message;

        if (Parameters == null) return result;
        for (var i = Parameters.Length - 1; i >= 0; i--)
        {
            var placeHolder = $"{{{i}}}";
            result = result.Replace(placeHolder, Parameters[i]);
        }
        return result;
    }
}