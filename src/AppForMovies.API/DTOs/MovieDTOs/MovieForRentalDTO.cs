using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForMovies.Shared.MovieDTOs
{
    public class MovieForRentalDTO
    {
        public MovieForRentalDTO()
        {
        }

        public MovieForRentalDTO(int id, string title, string genre, DateTime releaseDate, double priceForRenting)
        {
            Id = id;
            Title = title;
            Genre = genre;
            ReleaseDate = releaseDate;
            PriceForRenting = priceForRenting;
        }

        public MovieForRentalDTO(int id, string title, string genre, DateTime releaseDate, double priceForRenting, DateTime? lastRental) : this(id, title, genre, releaseDate, priceForRenting)
        {
            LastRental = lastRental;
        }

        public int Id { get; set; }

        [StringLength(50,ErrorMessage = "Title must have a maximun length of 50 characters")]
        public string Title { get; set; }

        [StringLength(50, ErrorMessage = "Genre must have a maximun length of 50 characters", MinimumLength = 4)]
        public string Genre { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }


        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(1, float.MaxValue, ErrorMessage = "Minimum price is 1 ")]
        [Display(Name = "Price For Renting")]
        public double PriceForRenting
        {
            get; set;
        }
        public DateTime? LastRental {  get; set; }

        public override bool Equals(object? obj)
        {
            return obj is MovieForRentalDTO dTO &&
                   Id == dTO.Id &&
                   Title == dTO.Title &&
                   Genre == dTO.Genre &&
                   ReleaseDate == dTO.ReleaseDate &&
                   PriceForRenting == dTO.PriceForRenting &&
                   LastRental == dTO.LastRental;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Title, Genre, ReleaseDate, PriceForRenting, LastRental);
        }
    }
}
