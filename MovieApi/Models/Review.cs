using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MovieApi.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string ReviewerName { get; set; } = null!;
        public string Comment { get; set; } = null!;
        [Range(1, 5)]
        public int Rating { get; set; }
        public int MovieId { get; set; } // Foreign key. 1:N - Movie:Review

        [JsonIgnore] // Prevents circular reference recursion
        public Movie Movie { get; set; } = null!; // Navigation property
    }
}
