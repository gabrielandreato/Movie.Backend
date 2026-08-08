namespace Application.Responses;

public class PagedResponse<T>
{
    public int Size { get; set; }
    public int Page { get; set; }
    public int TotalCount { get; set; }
    public IEnumerable<T> Items { get; set; } = new List<T>();
}
