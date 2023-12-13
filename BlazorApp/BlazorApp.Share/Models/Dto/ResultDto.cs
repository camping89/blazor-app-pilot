namespace BlazorApp.Share.Models.Dto;

public class ResultDto<T> where T : class
{
    public T? Data { get; set; }
    public bool IsError { get; set; }
    public Dictionary<string, List<string>>? ErrorDetails { get; set; } 
}