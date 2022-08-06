namespace DelegasiAPI.Models
{
    public class ErrorResponse
    {
        public ErrorResponse()
        {
        }

        public ErrorResponse(List<string> errors, object? attachment = null)
        {
            Message = string.Join(", ", errors);
            Errors = errors;
            Attachment = attachment;
        }

        public ErrorResponse(string message, List<string> errors, object? attachment = null)
        {
            Message = message;
            Errors = errors;
            Attachment = attachment;
        }

        public ErrorResponse(string message, string error, object? attachment = null)
        {
            Message = message;
            Errors = new List<string> { error };
            Attachment = attachment;
        }

        public ErrorResponse(string message, object? attachment = null)
        {
            Message = message;
            Errors = new List<string> { message };
            Attachment = attachment;
        }

        public string Message { get; set; } = "";
        public List<string> Errors { get; set; } = new List<string>();
        public object? Attachment { get; set; }
    }
}
