using Domain.Entities;
using Domain.Enums;

namespace Domain.Test.Entities;

public class MovieTests
{
    [Fact]
    public void NewMovie_SetsIdTitleAndGenre()
    {
        var id = Guid.NewGuid();
        var actor = new Actor(Guid.NewGuid(), "Keanu Reeves");

        var movie = new Movie(id, "The Matrix", MovieGenre.SciFi, new List<Actor> { actor });

        Assert.Equal(id, movie.Id);
        Assert.Equal("The Matrix", movie.Title);
        Assert.Equal(MovieGenre.SciFi, movie.Genre);
        Assert.Single(movie.Actors);
        Assert.Contains(actor, movie.Actors);
    }

    [Fact]
    public void NewMovie_AnimationWithoutActors_IsAllowed()
    {
        var movie = new Movie(Guid.NewGuid(), "Shrek", MovieGenre.Animation);

        Assert.Empty(movie.Actors);
    }

    [Fact]
    public void NewMovie_NonAnimationWithoutActors_IsAllowed()
    {
        var movie = new Movie(Guid.NewGuid(), "The Matrix", MovieGenre.SciFi);

        Assert.Empty(movie.Actors);
    }

    [Fact]
    public void Actors_CanAddActor()
    {
        var actor = new Actor(Guid.NewGuid(), "Keanu Reeves");
        var movie = new Movie(Guid.NewGuid(), "The Matrix", MovieGenre.SciFi, new List<Actor> { actor });
        var newActor = new Actor(Guid.NewGuid(), "Carrie-Anne Moss");

        movie.Actors.Add(newActor);

        Assert.Equal(2, movie.Actors.Count);
        Assert.Contains(newActor, movie.Actors);
    }

    [Fact]
    public void Actors_CanRemoveActor()
    {
        var actor = new Actor(Guid.NewGuid(), "Keanu Reeves");
        var movie = new Movie(Guid.NewGuid(), "The Matrix", MovieGenre.SciFi, new List<Actor> { actor });

        movie.Actors.Remove(actor);

        Assert.Empty(movie.Actors);
    }
}
