using Bogus;
using Microsoft.EntityFrameworkCore;
using MovieApi.Models;
using System.Globalization;

namespace MovieApi.Data
{
    public class SeedData
    {
        private static Faker faker = new Faker();
        internal static async Task InitAsync(MovieApiContext context)
        {
            if (await context.Movie.AnyAsync()) return;

            var actors = GenerateActors(3);
            await context.AddRangeAsync(actors);

            var movies = GenerateMovies(20);
            await context.AddRangeAsync(movies);

            await context.SaveChangesAsync();

            AssignActorsToMovies(movies, actors);

            var reviews = GenerateReviews(movies, 50);
            await context.AddRangeAsync(reviews);

            await context.SaveChangesAsync();
        }


        private static void AssignActorsToMovies(IEnumerable<Movie> movies, List<Actor> actors)
        {
            foreach (var movie in movies)
            {
                // Randomly assigning 1-3 actors to each movie
                var numberOfActors = faker.Random.Int(1, Math.Min(3, actors.Count));
                var selectedActors = faker.PickRandom(actors, numberOfActors);

                foreach (var actor in selectedActors)
                {
                    movie.Actors.Add(actor);
                }
            }
        }


        private static IEnumerable<Movie> GenerateMovies(int numberOfMovies)
        {
            var movies = new List<Movie>();

            for (int i = 0; i < numberOfMovies; i++)
            {
                //var title = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(faker.Commerce.ProductName());
                var title = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(faker.Company.CompanyName());
                var year = faker.Random.Int(1878, DateTime.Now.Year);
                var genre = GetRandomMovieGenre();
                var durationInMinutes = faker.Random.Int(1, 55000);

                var movie = new Movie
                {
                    Title = title,
                    Year = year,
                    Genre = genre,
                    DurationInMinutes = durationInMinutes,
                    MovieDetails = new MovieDetails
                    {
                        Synopsis = faker.Lorem.Paragraph(5),
                        Language = GetRandomLanguage(),
                        Budget = faker.Random.Int(1, int.MaxValue)
                    }
                };

                movies.Add(movie);
            }

            return movies;
        }


        private static List<Actor> GenerateActors(int numberOfActors)
        {
            var actors = new List<Actor>();

            for (int i = 0; i < numberOfActors; i++)
            {
                var fName = faker.Name.FirstName();
                var lName = faker.Name.LastName();
                var birthYear = faker.Random.Int(1850, DateTime.Now.Year);

                var actor = new Actor
                {
                    FirstName = fName,
                    LastName = lName,
                    BirthYear = birthYear
                };

                actors.Add(actor);
            }

            return actors;
        }

        private static IEnumerable<Review> GenerateReviews(IEnumerable<Movie> movies, int totalReviews)
        {
            var reviews = new List<Review>();

            for (int i = 0; i < totalReviews; i++)
            {
                var movie = faker.PickRandom(movies);

                var review = new Review
                {
                    ReviewerName = faker.Name.FullName(),
                    Comment = faker.Lorem.Sentence(faker.Random.Int(5, 20)),
                    Rating = faker.Random.Int(1, 5),
                    MovieId = movie.Id
                };

                reviews.Add(review);
            }

            return reviews;
        }

        private static string GetRandomLanguage()
        {
            var cultures = new[]
            {
                "en-US", // English
                "es-ES", // Spanish
                "fr-FR", // French
                "de-DE", // German
                "it-IT", // Italian
                "ja-JP", // Japanese
                "pt-BR", // Portuguese
                "ru-RU", // Russian
                "zh-CN", // Chinese
                "ko-KR"  // Korean
            };

            return faker.PickRandom(cultures);
        }

        private static string GetRandomMovieGenre()
        {

            var movieGenres = new[]
            {
                "Action", "Adventure", "Comedy", "Drama", "Fantasy",
                "Horror", "Mystery", "Romance", "Sci-Fi", "Thriller",
                "Western", "Crime", "Documentary", "Animation", "Musical"
            };

            return faker.PickRandom(movieGenres);
        }

    }
}

