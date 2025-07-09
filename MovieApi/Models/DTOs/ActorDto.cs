namespace MovieApi.Models.DTOs
{
    public class ActorDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public int BirthYear { get; set; }
        public IEnumerable<MovieDto> Movies { get; set; } = Enumerable.Empty<MovieDto>();
    }
}
