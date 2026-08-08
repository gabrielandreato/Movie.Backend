namespace Application.Requests;

public class UpdateActorToMoviesRequest
{
    public Guid MovieId { get; set; }
    public Guid ActorId { get; set; }
}
