namespace Base.Core.Domains.Exceptions;
/// <summary>
/// Errors related to invalid status in ValueObjects are sent by this class
/// </summary>
/// <param name="message">Error message or pattern</param>
/// <param name="parameters">Parameters to be placed in the message template, if any</param>
public class InvalidValueObjectStateException(string message, params string[] parameters)
    : DomainStateException(message, parameters);
