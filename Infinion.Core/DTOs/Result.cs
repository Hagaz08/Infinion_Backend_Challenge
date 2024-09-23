namespace Infinion.Core.DTOs
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; } = false;
        public string Message { get; set; } = "";
        public T? Content { get; set; }
        public IEnumerable<Error>? Error { get; set; } 
        
    }
    public class Result
    {
        public bool IsSuccess { get; set; } = false;
        public string Message { get; set; } = "";
        public IEnumerable<Error>? Error { get; set; }

    }
}
