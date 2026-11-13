namespace LibraryService.DTOs.Common
{
    public class ErrorResponse
    {
      public  int Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public Dictionary<string, string[]>? Errors { get; set; }
    }
}
