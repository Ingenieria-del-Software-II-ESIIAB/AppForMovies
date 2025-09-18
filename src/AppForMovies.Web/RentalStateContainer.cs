using AppForMovies.Web.API;

namespace AppForMovies.Web {
    public class RentalStateContainer {
        public RentalForCreateDTO Rental { get; private set; } = new RentalForCreateDTO() {
            RentalItems = new List<RentalItemDTO>()
        };

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();


        public void AddMovieToRental(MovieForRentalDTO movie) {
            if (!Rental.RentalItems.Any(ri => ri.MovieID == movie.Id))
                Rental.RentalItems.Add(new RentalItemDTO() {
                    MovieID = movie.Id,
                    Genre = movie.Genre,
                    Title = movie.Title,
                    PriceForRenting = movie.PriceForRenting,
                }
            );
            ComputeTotalPrice();
        }

        private void ComputeTotalPrice() {
            int numberOfDays = (Rental.RentalDateTo - Rental.RentalDateFrom).Days;
            Rental.TotalPrice = Rental.RentalItems.Sum(ri => ri.PriceForRenting * numberOfDays);
        }

        public void RemoveRentalItemToRent(RentalItemDTO item) {
            Rental.RentalItems.Remove(item);
            ComputeTotalPrice();
        }

        public void ClearRentingCart() {
            Rental.RentalItems.Clear();
            Rental.TotalPrice = 0;
        }

        public void RentalProcessed() {
            //we have finished the rental process so we create a new object without data
            Rental = new RentalForCreateDTO() {
                RentalItems = new List<RentalItemDTO>()
            };
        }

    }
}
