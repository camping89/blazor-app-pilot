namespace BlazorApp.Share.ValueObjects;

public class ResultDto<T> where T : class
{
    public T? Payload { get; set; }
    public bool IsError { get; set; }
    public Dictionary<string, List<string>>? ErrorDetails { get; set; } 
}