using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models.DTOs
{
    public class MovieManipulationDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = null!;

        [Required]
        [Range(1878, 2100)]
        public int Year { get; set; }

        [Required]
        [StringLength(50)]
        public string Genre { get; set; } = null!;

        [Required]
        [Range(1, 55000)]
        public int DurationInMinutes { get; set; }

        [Required]
        [StringLength(2000)]
        public string Synopsis { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Language { get; set; } = null!;

        [Range(0, int.MaxValue)]
        public int Budget { get; set; }

        [Required]
        public string ActorFirstName { get; set; } = null!;

        [Required]
        public string ActorLastName { get; set; } = null!;

        [Required]
        public int ActorBirthYear { get; set; }

        [Required]
        public string ReviewReviewerName { get; set; } = null!;

        [Required]
        public string ReviewComment { get; set; } = null!;

        [Required]
        public string ReviewRating { get; set; } = null!;
    }
}
