using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Core.Domain.Exceptions;
public class InvalidEntityStateException : DomainStateException
{
    /// <summary>
    /// Errors related to incorrect status in Entities are sent by this class
    /// </summary>
    /// <param name="message">Error message or pattern</param>
    /// <param name="parameters">Parameters to be placed in the message template, if any</param>
    public InvalidEntityStateException(string message, params string[] parameters) : base(message, parameters)
    {
    }
}