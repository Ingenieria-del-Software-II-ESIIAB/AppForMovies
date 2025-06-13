namespace AppForMovies.API.Models
{
    [Index(nameof(Title), IsUnique = true)]
    public class Movie
    {
        public Movie()
        {
        }

        public Movie(string title, Genre genre, DateTime releaseDate, decimal priceForPurchase, int quantityForPurchase, double priceForRenting, int quantityForRenting)
        {
            Title = title;
            Genre = genre;
            ReleaseDate = releaseDate;
            PriceForPurchase = priceForPurchase;
            QuantityForPurchase = quantityForPurchase;
            PriceForRenting = priceForRenting;
            QuantityForRenting = quantityForRenting;
        }

        public int Id { get; set; }


        [StringLength(50, ErrorMessage = "Title name cannot be longer than 50 characters.")]
        public string Title { get; set; }


        public Genre Genre { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }


        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(0.5, float.MaxValue, ErrorMessage = "Minimum price is 0.5 ")]
        [Display(Name = "Price For Purchase")]
        [Precision(10, 2)]
        public decimal PriceForPurchase { get; set; }


        [Display(Name = "Quantity For Purchase")]
        [Range(0, int.MaxValue, ErrorMessage = "Minimum quantity for Purchase is 1")]
        public int QuantityForPurchase { get; set; }


        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(0.5, 100, ErrorMessage = "Minimum is 0.5 and maximum 100")]
        [Display(Name = "Price For Renting")]
        public double PriceForRenting { get; set; }



        [Display(Name = "Quantity For Renting")]
        [Range(1, int.MaxValue, ErrorMessage = "Minimum quantity for renting is 1")]
        public int QuantityForRenting { get; set; }

    }
}
