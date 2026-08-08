using Domain.Entities;

namespace Application.Responses;

public class MovieResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public IEnumerable<ActorResponse> Actors { get; set; } = [];

    public static MovieResponse Create(Movie movie) => new()
    {
        Id = movie.Id,
        Title = movie.Title,
        Genre = movie.Genre.ToString(),
        Actors = movie.Actors.Select(a => new ActorResponse { Id = a.Id, Name = a.Name })
    };
}
