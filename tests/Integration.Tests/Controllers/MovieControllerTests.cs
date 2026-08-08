using System.Net;
using System.Net.Http.Json;
using Application.Responses;
using Domain.Entities;
using Domain.Enums;
using Infra.Data.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Tests.Controllers;

public class MovieControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public MovieControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_ReturnsPagedMovies_WhenMoviesExist()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var movie = new Movie(Guid.NewGuid(), "The Matrix", MovieGenre.SciFi);
        context.Movies.Add(movie);
        await context.SaveChangesAsync();

        var client = _factory.CreateClient();

        var response = await client.GetAsync("/movie");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<PagedResponse<MovieResponse>>();

        Assert.NotNull(result);
        Assert.Contains(result.Items, m => m.Title == "The Matrix");
    }
}
