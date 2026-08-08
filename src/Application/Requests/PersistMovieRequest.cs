using Domain.Enums;

namespace Application.Requests;

public class PersistMovieRequest
{
    public string Title { get; set; } = string.Empty;
    public MovieGenre Genre { get; set; }
}
