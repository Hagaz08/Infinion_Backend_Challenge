namespace Infinion.Core.DTOs
{
    public class Error
    {
        public string Message { get; }
        public string Code { get; }
        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        
    }
}
