using AppForMovies.API.Controllers;
using AppForMovies.API.DTOs.RentalDTOs;
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

        public static IEnumerable<object[]> TestCasesFor_CreatePurchase() {
            var rentalNoITem = new RentalForCreateDTO("elena@uclm.es", "Elena Navarro",
                "Avda. España s/n, Albacete", PaymentMethodTypes.CreditCard,
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(5), new List<RentalItemDTO>());

            var rentalItems = new List<RentalItemDTO>() { new RentalItemDTO(2, "The man in the high castle", "Drama", 4.0, "My favourite movie") };

            var rentalFromBeforeToday = new RentalForCreateDTO("elena@uclm.es", "Elena Navarro",
                "Avda. España s/n, Albacete", PaymentMethodTypes.CreditCard,
                DateTime.Today, DateTime.Today.AddDays(5), rentalItems);

            var rentalToBeforeFrom = new RentalForCreateDTO("elena@uclm.es", "Elena Navarro",
                "Avda. España s/n, Albacete", PaymentMethodTypes.CreditCard,
                DateTime.Today.AddDays(5), DateTime.Today.AddDays(2), rentalItems);

            var RentalApplicationUser = new RentalForCreateDTO("victor.lopez@uclm.es", "Elena Navarro",
                "Avda. España s/n, Albacete", PaymentMethodTypes.CreditCard,
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(4), rentalItems);

            var rentalMovieNotAvailable = new RentalForCreateDTO("elena@uclm.es", "Elena Navarro",
                "Avda. España s/n, Albacete", PaymentMethodTypes.CreditCard,
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(5),
                new List<RentalItemDTO>() { new RentalItemDTO(1, "The lord of the rings", "Sci - Fi", 1.0) });


            var allTests = new List<object[]>
            {             //input for createpurchase - Error expected
                new object[] { rentalNoITem, "Error! You must include at least one movie to be rented",  },
                new object[] { rentalFromBeforeToday, "Error! Your rental date must start later than today", },
                new object[] { rentalToBeforeFrom, "Error! Your rental must end later than it starts", },
                new object[] { RentalApplicationUser, "Error! UserName is not registered", },
                new object[] { rentalMovieNotAvailable, "Error! Movie titled 'The lord of the rings' is not available for being rented from", },
            };

            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreatePurchase))]
        public async Task CreateRental_Error_test(RentalForCreateDTO rentalDTO, string errorExpected) {
            // Arrange
            var mock = new Mock<ILogger<RentalsController>>();
            ILogger<RentalsController> logger = mock.Object;

            var controller = new RentalsController(_context, logger);

            // Act
            var result = await controller.CreateRental(rentalDTO);

            //Assert
            //we check that the response type is BadRequest and obtain the error returned
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            var errorActual = problemDetails.Errors.First().Value[0];

            //we check that the expected error message and actual are the same
            Assert.StartsWith(errorExpected, errorActual);

        }

    }
}

