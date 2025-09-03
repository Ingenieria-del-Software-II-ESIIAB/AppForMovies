using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForMovies.UT.RentalsController_test {
    public class PostRentals_test:AppForMovies4SqliteUT {
        public PostRentals_test() {

            var genres = new List<Genre>() {
                new Genre("Sci - Fi"),
                new Genre("Drama"),
            };

            var movies = new List<Movie>(){
                new Movie("The lord of the rings", genres[0],new DateTime(2011, 10, 20),10.0m, 5,1.0,1),
                new Movie("The man in the high castle", genres[1],new DateTime(2015, 01, 01),10.0m,0, 4.0,15),
            };

            ApplicationUser user = new ApplicationUser("1", "Elena", "Navarro Martínez", "elena@uclm.es");

            var rental = new Rental("elena.navarro@uclm.es", "Elena Navarro",
                   user, "Avda. España s/n, Albacete 02071",
                    DateTime.Now, AppForMovies.API.Models.PaymentMethodTypes.CreditCard,
                    DateTime.Today.AddDays(2), DateTime.Today.AddDays(5),
                    new List<RentalItem>());
            rental.RentalItems.Add(new RentalItem(movies[0], rental, "My favourite movie"));

            _context.ApplicationUsers.Add(user);
            _context.AddRange(genres);
            _context.AddRange(movies);
            _context.Add(rental);
            _context.SaveChanges();
        }
    }
}
