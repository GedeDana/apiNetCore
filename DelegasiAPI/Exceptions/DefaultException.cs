using DelegasiAPI.Models;

namespace DelegasiAPI.Exceptions
{
    public class DefaultException: Exception
    {
        public DefaultException(string message, List<string> errors, object? attachment = null) : base(message)
        {
            Error = new ErrorResponse(message, errors, attachment);
        }

        public DefaultException(string message, string error, object? attachment = null) : base(message)
        {
            Error = new ErrorResponse(message, error, attachment);
        }

        public DefaultException(string message, object? attachment = null) : base(message)
        {
            Error = new ErrorResponse(message, attachment);
        }

        public DefaultException(ErrorResponse errors) : base(errors.Message)
        {
            Error = errors;
        }

        public ErrorResponse Error { get; private set; }
    }
}
