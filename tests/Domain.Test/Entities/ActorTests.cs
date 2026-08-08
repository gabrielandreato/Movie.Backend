using Domain.Entities;
using Domain.Enums;

namespace Domain.Test.Entities;

public class ActorTests
{
    [Fact]
    public void NewActor_SetsIdAndName()
    {
        var id = Guid.NewGuid();

        var actor = new Actor(id, "Keanu Reeves");

        Assert.Equal(id, actor.Id);
        Assert.Equal("Keanu Reeves", actor.Name);
        Assert.Empty(actor.Movies);
    }

    [Fact]
    public void NewActor_WithMovies_SetsMovies()
    {
        var actor = new Actor(Guid.NewGuid(), "Keanu Reeves");
        var movie = new Movie(Guid.NewGuid(), "The Matrix", MovieGenre.SciFi, new List<Actor> { actor });
        actor.Movies.Add(movie);

        Assert.Single(actor.Movies);
        Assert.Contains(movie, actor.Movies);
    }

    [Fact]
    public void Movies_CanAddMovie()
    {
        var actor = new Actor(Guid.NewGuid(), "Keanu Reeves");
        var movie = new Movie(Guid.NewGuid(), "The Matrix", MovieGenre.SciFi, new List<Actor> { actor });

        actor.Movies.Add(movie);

        Assert.Single(actor.Movies);
        Assert.Contains(movie, actor.Movies);
    }

    [Fact]
    public void Movies_CanRemoveMovie()
    {
        var actor = new Actor(Guid.NewGuid(), "Keanu Reeves");
        var movie = new Movie(Guid.NewGuid(), "The Matrix", MovieGenre.SciFi, new List<Actor> { actor });
        actor.Movies.Add(movie);

        actor.Movies.Remove(movie);

        Assert.Empty(actor.Movies);
    }
}
