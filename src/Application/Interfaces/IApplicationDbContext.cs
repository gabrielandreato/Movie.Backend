using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Movie> Movies { get; }
    DbSet<Actor> Actors { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
