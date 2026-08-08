using Domain.Enums;

namespace Domain.Entities;

public class Movie
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public MovieGenre Genre { get; set; }
    public ICollection<Actor> Actors { get; set; } = new List<Actor>();

    private Movie()
    {
    }

    public Movie(Guid id, string title, MovieGenre genre, ICollection<Actor>? actors = null)
    {
        Id = id;
        Title = title;
        Genre = genre;
        Actors = actors ?? new List<Actor>();
    }
}
