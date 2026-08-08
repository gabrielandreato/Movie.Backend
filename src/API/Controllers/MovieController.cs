using Application.Requests;
using Application.Responses;
using Application.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("movie")]
public class MovieController(MovieService movieService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResponse<MovieResponse>>> Get(
        [FromQuery] int page = 1,
        [FromQuery] int size = 10,
        [FromQuery] string? title = null,
        [FromQuery] MovieGenre? genre = null,
        [FromQuery] string? actorName = null,
        CancellationToken cancellationToken = default)
    {
        var result = await movieService.GetAsync(page, size, title, genre, actorName, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors.Select(e => e.Message));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MovieResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await movieService.GetByIdAsync(id, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Errors.Select(e => e.Message));
    }

    [HttpPost]
    public async Task<ActionResult<MovieResponse>> Add([FromBody] PersistMovieRequest request, CancellationToken cancellationToken)
    {
        var result = await movieService.AddAsync(request, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
            : BadRequest(result.Errors.Select(e => e.Message));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<MovieResponse>> Update(Guid id, [FromBody] PersistMovieRequest request, CancellationToken cancellationToken)
    {
        var result = await movieService.UpdateAsync(id, request, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Errors.Select(e => e.Message));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Remove(Guid id, CancellationToken cancellationToken)
    {
        var result = await movieService.RemoveAsync(id, cancellationToken);

        return result.IsSuccess ? NoContent() : NotFound(result.Errors.Select(e => e.Message));
    }

    [HttpPost("{id:guid}:add-actor")]
    public async Task<IActionResult> AddActor(Guid id, [FromQuery] Guid actorId, CancellationToken cancellationToken)
    {
        var request = new UpdateActorToMoviesRequest { MovieId = id, ActorId = actorId };
        var result = await movieService.AddActorAsync(request, cancellationToken);

        return result.IsSuccess ? NoContent() : NotFound(result.Errors.Select(e => e.Message));
    }

    [HttpPost("{id:guid}:remove-actor")]
    public async Task<IActionResult> RemoveActor(Guid id, [FromQuery] Guid actorId, CancellationToken cancellationToken)
    {
        var request = new UpdateActorToMoviesRequest { MovieId = id, ActorId = actorId };
        var result = await movieService.RemoveActorAsync(request, cancellationToken);

        return result.IsSuccess ? NoContent() : NotFound(result.Errors.Select(e => e.Message));
    }
}
