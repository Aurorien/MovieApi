using System.Text.Json.Serialization;

namespace MovieApi.Models
{
    public class MovieDetails
    {
        public int Id { get; set; }
        public string Synopsis { get; set; } = null!;
        public string Language { get; set; } = null!;
        public int Budget { get; set; }
        public int MovieId { get; set; } // Foreign key

        [JsonIgnore] // Prevents circular reference recursion
        public Movie Movie { get; set; } = null!; // Navigation property
    }
}
