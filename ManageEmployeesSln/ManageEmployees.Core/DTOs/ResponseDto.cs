namespace ManageEmployees.Core.DTOs
{
    public class ResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public object Result { get; set; } = string.Empty;
        public int StatusCode { get; set; }
    }
}
