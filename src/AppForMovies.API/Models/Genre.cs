namespace AppForMovies.API.Models
{
    //Title is unique for each instance of Movie
    [Index(nameof(Name), IsUnique = true)]
    public class Genre
    {
        public Genre()
        {
        }

        public Genre(string name)
        {
            Name = name;
        }

        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "Title name cannot be longer than 50 characters.", MinimumLength = 4)]
        public string Name { get; set; }

    }
}
