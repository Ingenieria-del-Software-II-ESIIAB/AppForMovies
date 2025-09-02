
namespace AppForMovies.API.DTOs.RentalDTOs
{
    public class RentalItemDTO
    {
        public RentalItemDTO(int movieID, string title, string genre, double priceForRenting, string description = "")
        {
            MovieID = movieID;
            Title = title;
            PriceForRenting = priceForRenting;
            Genre = genre;
            Description = description;
        }

        public int MovieID { get; set; }


        public string Title { get; set; }


        public double PriceForRenting { get; set; }

        public string? Description { get; set; }

        public string Genre { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is RentalItemDTO dTO &&
                   MovieID == dTO.MovieID &&
                   Title == dTO.Title &&
                   PriceForRenting == dTO.PriceForRenting &&
                   Description == dTO.Description &&
                   Genre == dTO.Genre;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MovieID, Title, PriceForRenting, Description, Genre);
        }
    }
}
