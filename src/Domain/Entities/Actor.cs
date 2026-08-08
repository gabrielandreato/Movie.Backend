namespace Domain.Entities;

public class Actor
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Movie> Movies { get; set; } = new List<Movie>();

    private Actor()
    {
    }

    public Actor(Guid id, string name, ICollection<Movie>? movies = null)
    {
        Id = id;
        Name = name;
        Movies = movies ?? new List<Movie>();
    }
}
