using Application.Interfaces;
using Application.Requests;
using Application.Responses;
using Domain.Entities;
using Domain.Enums;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class MovieService(IApplicationDbContext context)
{
    private const string MovieNotFoundMessage = "Movie '{0}' was not found.";

    public async Task<Result<PagedResponse<MovieResponse>>> GetAsync(
        int page,
        int size,
        string? title = null,
        MovieGenre? genre = null,
        string? actorName = null,
        CancellationToken cancellationToken = default)
    {
        var query = context.Movies
            .Include(m => m.Actors)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(title))
            query = query.Where(m => EF.Functions.Like(m.Title, $"%{title}%"));

        if (genre is not null)
            query = query.Where(m => m.Genre == genre);

        if (!string.IsNullOrWhiteSpace(actorName))
            query = query.Where(m => m.Actors.Any(a => EF.Functions.Like(a.Name, $"%{actorName}%")));

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(m => m.Title)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);

        return Result.Ok(new PagedResponse<MovieResponse>
        {
            Page = page,
            Size = size,
            TotalCount = totalCount,
            Items = items.Select(MovieResponse.Create)
        });
    }

    public async Task<Result<MovieResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var movie = await context.Movies
            .Include(m => m.Actors)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        if (movie is null)
            return Result.Fail(string.Format(MovieNotFoundMessage, id));

        return Result.Ok(MovieResponse.Create(movie));
    }

    public async Task<Result<MovieResponse>> AddAsync(PersistMovieRequest request, CancellationToken cancellationToken = default)
    {
        Movie movie;

        try
        {
            movie = new Movie(Guid.NewGuid(), request.Title, request.Genre);
        }
        catch (ArgumentException ex)
        {
            return Result.Fail(ex.Message);
        }

        await context.Movies.AddAsync(movie, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok(MovieResponse.Create(movie));
    }

    public async Task<Result<MovieResponse>> UpdateAsync(Guid id, PersistMovieRequest request, CancellationToken cancellationToken = default)
    {
        var movie = await context.Movies
            .Include(m => m.Actors)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        if (movie is null)
            return Result.Fail(string.Format(MovieNotFoundMessage, id));

        movie.Title = request.Title;
        movie.Genre = request.Genre;

        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok(MovieResponse.Create(movie));
    }

    public async Task<Result> RemoveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var movie = await context.Movies.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        if (movie is null)
            return Result.Fail(string.Format(MovieNotFoundMessage, id));

        context.Movies.Remove(movie);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    public async Task<Result> AddActorAsync(UpdateActorToMoviesRequest request, CancellationToken cancellationToken = default)
    {
        var movie = await context.Movies
            .Include(m => m.Actors)
            .FirstOrDefaultAsync(m => m.Id == request.MovieId, cancellationToken);

        if (movie is null)
            return Result.Fail(string.Format(MovieNotFoundMessage, request.MovieId));

        var actor = await context.Actors.FirstOrDefaultAsync(a => a.Id == request.ActorId, cancellationToken);

        if (actor is null)
            return Result.Fail($"Actor '{request.ActorId}' was not found.");

        if (movie.Actors.All(a => a.Id != actor.Id))
            movie.Actors.Add(actor);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    public async Task<Result> RemoveActorAsync(UpdateActorToMoviesRequest request, CancellationToken cancellationToken = default)
    {
        var movie = await context.Movies
            .Include(m => m.Actors)
            .FirstOrDefaultAsync(m => m.Id == request.MovieId, cancellationToken);

        if (movie is null)
            return Result.Fail(string.Format(MovieNotFoundMessage, request.MovieId));

        var actor = movie.Actors.FirstOrDefault(a => a.Id == request.ActorId);

        if (actor is not null)
            movie.Actors.Remove(actor);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
