namespace MovieApi.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int Year { get; set; }
        public string Genre { get; set; } = null!;
        public int Duration { get; set; }
        public MovieDetails MovieDetails { get; set; } = null!; // Navigation property
        public ICollection<Review> Reviews { get; set; } = new List<Review>(); // 1:N - Movie:Review
        public ICollection<Actor> Actors { get; set; } = new List<Actor>(); // N:M - Movie:Actor
    }
}
