using Application.Requests;
using Application.Services;
using Application.Tests.Helpers;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.Tests.Services;

public class MovieServiceTests
{
    private static async Task<(TestApplicationDbContext Context, MovieService Service)> CreateSutAsync(
        params Movie[] movies)
    {
        var context = TestApplicationDbContext.Create();

        if (movies.Length > 0)
        {
            await context.Movies.AddRangeAsync(movies);
            await context.SaveChangesAsync();
        }

        return (context, new MovieService(context));
    }

    [Fact]
    public async Task GetAsync_ReturnsPagedResults()
    {
        var actor = new Actor(Guid.NewGuid(), "Keanu Reeves");
        var movies = Enumerable.Range(1, 5)
            .Select(i => new Movie(Guid.NewGuid(), $"Movie {i}", MovieGenre.Action, new List<Actor> { actor }))
            .ToArray();
        var (_, sut) = await CreateSutAsync(movies);

        var result = await sut.GetAsync(page: 2, size: 2);

        Assert.True(result.IsSuccess);
        Assert.Equal(5, result.Value.TotalCount);
        Assert.Equal(2, result.Value.Page);
        Assert.Equal(2, result.Value.Size);
        Assert.Equal(2, result.Value.Items.Count());
        Assert.Equal("Movie 3", result.Value.Items.First().Title);
    }

    [Fact]
    public async Task GetAsync_FiltersByTitleGenreAndActorName()
    {
        var reeves = new Actor(Guid.NewGuid(), "Keanu Reeves");
        var moss = new Actor(Guid.NewGuid(), "Carrie-Anne Moss");
        var matrix = new Movie(Guid.NewGuid(), "The Matrix", MovieGenre.SciFi, new List<Actor> { reeves, moss });
        var shrek = new Movie(Guid.NewGuid(), "Shrek", MovieGenre.Animation);
        var (_, sut) = await CreateSutAsync(matrix, shrek);

        var byTitle = await sut.GetAsync(1, 10, title: "Matrix");
        var byGenre = await sut.GetAsync(1, 10, genre: MovieGenre.Animation);
        var byActor = await sut.GetAsync(1, 10, actorName: "Keanu");

        Assert.Single(byTitle.Value.Items);
        Assert.Equal("The Matrix", byTitle.Value.Items.Single().Title);
        Assert.Single(byGenre.Value.Items);
        Assert.Equal("Shrek", byGenre.Value.Items.Single().Title);
        Assert.Single(byActor.Value.Items);
        Assert.Equal("The Matrix", byActor.Value.Items.Single().Title);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingMovie_ReturnsMovieWithActors()
    {
        var actor = new Actor(Guid.NewGuid(), "Keanu Reeves");
        var movie = new Movie(Guid.NewGuid(), "The Matrix", MovieGenre.SciFi, new List<Actor> { actor });
        var (_, sut) = await CreateSutAsync(movie);

        var result = await sut.GetByIdAsync(movie.Id);

        Assert.True(result.IsSuccess);
        Assert.Equal(movie.Id, result.Value.Id);
        Assert.Single(result.Value.Actors);
    }

    [Fact]
    public async Task GetByIdAsync_UnknownMovie_ReturnsFailure()
    {
        var (_, sut) = await CreateSutAsync();
        var id = Guid.NewGuid();

        var result = await sut.GetByIdAsync(id);

        Assert.True(result.IsFailed);
        Assert.Contains(result.Errors, e => e.Message.Contains(id.ToString()));
    }

    [Fact]
    public async Task AddAsync_AnimationWithoutActors_PersistsMovie()
    {
        var (context, sut) = await CreateSutAsync();
        var request = new PersistMovieRequest { Title = "Shrek", Genre = MovieGenre.Animation };

        var result = await sut.AddAsync(request);

        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value.Id);
        Assert.Equal("Shrek", result.Value.Title);
        Assert.Equal(MovieGenre.Animation.ToString(), result.Value.Genre);
        Assert.Equal(1, await context.Movies.CountAsync());
    }

    [Fact]
    public async Task UpdateAsync_ExistingMovie_UpdatesTitleAndGenre()
    {
        var movie = new Movie(Guid.NewGuid(), "Shrek", MovieGenre.Animation);
        var (context, sut) = await CreateSutAsync(movie);
        var request = new PersistMovieRequest { Title = "Shrek 2", Genre = MovieGenre.Comedy };

        var result = await sut.UpdateAsync(movie.Id, request);

        Assert.True(result.IsSuccess);
        Assert.Equal("Shrek 2", result.Value.Title);
        Assert.Equal(MovieGenre.Comedy.ToString(), result.Value.Genre);
        var persisted = await context.Movies.FindAsync(movie.Id);
        Assert.Equal("Shrek 2", persisted!.Title);
    }

    [Fact]
    public async Task UpdateAsync_UnknownMovie_ReturnsFailure()
    {
        var (_, sut) = await CreateSutAsync();
        var id = Guid.NewGuid();
        var request = new PersistMovieRequest { Title = "Shrek 2", Genre = MovieGenre.Comedy };

        var result = await sut.UpdateAsync(id, request);

        Assert.True(result.IsFailed);
        Assert.Contains(result.Errors, e => e.Message.Contains(id.ToString()));
    }

    [Fact]
    public async Task RemoveAsync_ExistingMovie_RemovesFromContext()
    {
        var movie = new Movie(Guid.NewGuid(), "Shrek", MovieGenre.Animation);
        var (context, sut) = await CreateSutAsync(movie);

        var result = await sut.RemoveAsync(movie.Id);

        Assert.True(result.IsSuccess);
        Assert.Equal(0, await context.Movies.CountAsync());
    }

    [Fact]
    public async Task RemoveAsync_UnknownMovie_ReturnsFailure()
    {
        var (_, sut) = await CreateSutAsync();

        var result = await sut.RemoveAsync(Guid.NewGuid());

        Assert.True(result.IsFailed);
    }

    [Fact]
    public async Task AddActorAsync_AssociatesActorWithMovie()
    {
        var movie = new Movie(Guid.NewGuid(), "Shrek", MovieGenre.Animation);
        var actor = new Actor(Guid.NewGuid(), "Mike Myers");
        var (context, sut) = await CreateSutAsync(movie);
        context.Actors.Add(actor);
        await context.SaveChangesAsync();

        var result = await sut.AddActorAsync(new UpdateActorToMoviesRequest { MovieId = movie.Id, ActorId = actor.Id });

        Assert.True(result.IsSuccess);
        var persisted = await context.Movies.Include(m => m.Actors).FirstAsync(m => m.Id == movie.Id);
        Assert.Single(persisted.Actors);
        Assert.Equal(actor.Id, persisted.Actors.Single().Id);
    }

    [Fact]
    public async Task AddActorAsync_ActorAlreadyAssociated_DoesNotDuplicate()
    {
        var actor = new Actor(Guid.NewGuid(), "Mike Myers");
        var movie = new Movie(Guid.NewGuid(), "Shrek", MovieGenre.Animation, new List<Actor> { actor });
        var (context, sut) = await CreateSutAsync(movie);

        var result = await sut.AddActorAsync(new UpdateActorToMoviesRequest { MovieId = movie.Id, ActorId = actor.Id });

        Assert.True(result.IsSuccess);
        var persisted = await context.Movies.Include(m => m.Actors).FirstAsync(m => m.Id == movie.Id);
        Assert.Single(persisted.Actors);
    }

    [Fact]
    public async Task AddActorAsync_UnknownMovie_ReturnsFailure()
    {
        var actor = new Actor(Guid.NewGuid(), "Mike Myers");
        var (context, sut) = await CreateSutAsync();
        context.Actors.Add(actor);
        await context.SaveChangesAsync();

        var request = new UpdateActorToMoviesRequest { MovieId = Guid.NewGuid(), ActorId = actor.Id };

        var result = await sut.AddActorAsync(request);

        Assert.True(result.IsFailed);
    }

    [Fact]
    public async Task AddActorAsync_UnknownActor_ReturnsFailure()
    {
        var movie = new Movie(Guid.NewGuid(), "Shrek", MovieGenre.Animation);
        var (_, sut) = await CreateSutAsync(movie);

        var request = new UpdateActorToMoviesRequest { MovieId = movie.Id, ActorId = Guid.NewGuid() };

        var result = await sut.AddActorAsync(request);

        Assert.True(result.IsFailed);
    }

    [Fact]
    public async Task RemoveActorAsync_RemovesAssociation()
    {
        var actor = new Actor(Guid.NewGuid(), "Mike Myers");
        var movie = new Movie(Guid.NewGuid(), "Shrek", MovieGenre.Animation, new List<Actor> { actor });
        var (context, sut) = await CreateSutAsync(movie);

        var result = await sut.RemoveActorAsync(new UpdateActorToMoviesRequest { MovieId = movie.Id, ActorId = actor.Id });

        Assert.True(result.IsSuccess);
        var persisted = await context.Movies.Include(m => m.Actors).FirstAsync(m => m.Id == movie.Id);
        Assert.Empty(persisted.Actors);
    }

    [Fact]
    public async Task RemoveActorAsync_ActorNotAssociated_IsNoOpSuccess()
    {
        var movie = new Movie(Guid.NewGuid(), "Shrek", MovieGenre.Animation);
        var (_, sut) = await CreateSutAsync(movie);

        var result = await sut.RemoveActorAsync(new UpdateActorToMoviesRequest { MovieId = movie.Id, ActorId = Guid.NewGuid() });

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task RemoveActorAsync_UnknownMovie_ReturnsFailure()
    {
        var (_, sut) = await CreateSutAsync();

        var request = new UpdateActorToMoviesRequest { MovieId = Guid.NewGuid(), ActorId = Guid.NewGuid() };

        var result = await sut.RemoveActorAsync(request);

        Assert.True(result.IsFailed);
    }
}
