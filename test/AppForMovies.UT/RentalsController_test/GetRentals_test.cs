using AppForMovies.API.Controllers;
using AppForMovies.API.DTOs.RentalDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForMovies.UT.RentalsController_test
{
    public class GetRentals_test : AppForMovies4SqliteUT
    {
        public GetRentals_test()
        {

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

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetRental_NotFound_test()
        {
            // Arrange
            var mock = new Mock<ILogger<RentalsController>>();
            ILogger<RentalsController> logger = mock.Object;

            var controller = new RentalsController(_context, logger);

            // Act
            var result = await controller.GetRental(0);

            //Assert
            //we check that the response type is OK and obtain the list of movies
            Assert.IsType<NotFoundResult>(result);

        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetRental_Found_test()
        {
            // Arrange
            var mock = new Mock<ILogger<RentalsController>>();
            ILogger<RentalsController> logger = mock.Object;
            var controller = new RentalsController(_context, logger);


            var expectedRental = new RentalDetailDTO(1, DateTime.Now, "elena.navarro@uclm.es", "Elena Navarro",
                        "Avda. España s/n, Albacete 02071", PaymentMethodTypes.CreditCard,
                        DateTime.Today.AddDays(2), DateTime.Today.AddDays(5),
                        new List<RentalItemDTO>());
            expectedRental.RentalItems.Add(new RentalItemDTO(1, "The lord of the rings", "Sci - Fi", 1.0, "My favourite movie"));

            // Act 
            var result = await controller.GetRental(1);

            //Assert
            //we check that the response type is OK and obtain the rental
            var okResult = Assert.IsType<OkObjectResult>(result);
            var rentalDTOActual = Assert.IsType<RentalDetailDTO>(okResult.Value);
            var eq = expectedRental.Equals(rentalDTOActual);
            //we check that the expected and actual are the same
            Assert.Equal(expectedRental, rentalDTOActual);

        }
    }
}
