namespace AppForMovies.API.Models
{
    public class RentalItem
    {
        public RentalItem()
        {
        }

        public RentalItem(Movie movie, Rental rent, string? description = null)
        {
            Movie = movie;
            MovieId = movie.Id;
            Rent = rent;
            RentalId = rent.Id;
            PriceForRenting = movie.PriceForRenting;
            Description = description;
        }

        public Movie Movie { get; set; }=new Movie();

        public int MovieId { get; set; }


        public Rental Rent { get; set; }=new Rental();

        public int RentalId { get; set; }

        [StringLength(100, ErrorMessage = "Title name cannot be longer than 50 characters.")]
        public string? Description { get; set; }
        public double PriceForRenting { get; set; }
    }   
}
