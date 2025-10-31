using AppForMovies.API.Controllers;
using AppForMovies.API.DTOs.RentalDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForMovies.UT.RentalsController_test {
    public class PostRentals_test:AppForMovies4SqliteUT {
        private const string _userName = "elena.navarro@uclm.es";
        private const string _customerNameSurname = "Elena Navarro";
        private const string _deliveryAddress = "Avda. España s/n, Albacete 02071";

        private const string _movie1Title = "The lord of the rings";
        private const string _movie1Genre = "Sci - Fi";
        private const string _movie2Title = "The man in the high castle";
        private const string _movie2Genre = "Drama";

        public PostRentals_test() {

            var genres = new List<Genre>() {
                new Genre(_movie1Genre),
                new Genre(_movie2Genre),
            };

            var movies = new List<Movie>(){
                new Movie(_movie1Title, genres[0],new DateTime(2011, 10, 20),10.0m, 5,1.0,1),
                new Movie(_movie2Title, genres[1],new DateTime(2015, 01, 01),10.0m,0, 4.0,15),
            };

            ApplicationUser user = new ApplicationUser("1", "Elena", "Navarro Martínez", _userName);

            var rental = new Rental(_userName, _customerNameSurname,
                   user, _deliveryAddress,
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
            var rentalNoITem = new RentalForCreateDTO(_userName, _customerNameSurname,
                _deliveryAddress, PaymentMethodTypes.CreditCard,
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(5), new List<RentalItemDTO>());

            var rentalItems = new List<RentalItemDTO>() { new RentalItemDTO(2, _movie2Title, _movie2Genre, 4.0, "My favourite movie") };

            var rentalFromBeforeToday = new RentalForCreateDTO(_userName, _customerNameSurname,
                _deliveryAddress, PaymentMethodTypes.CreditCard,
                DateTime.Today, DateTime.Today.AddDays(5), rentalItems);

            var rentalToBeforeFrom = new RentalForCreateDTO(_userName, _customerNameSurname,
                _deliveryAddress, PaymentMethodTypes.CreditCard,
                DateTime.Today.AddDays(5), DateTime.Today.AddDays(2), rentalItems);

            var RentalApplicationUser = new RentalForCreateDTO("victor.lopez@uclm.es", _customerNameSurname,
                _deliveryAddress, PaymentMethodTypes.CreditCard,
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(4), rentalItems);

            var rentalMovieNotAvailable = new RentalForCreateDTO(_userName, _customerNameSurname,
                _deliveryAddress, PaymentMethodTypes.CreditCard,
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(5),
                new List<RentalItemDTO>() { new RentalItemDTO(1, _movie1Title, _movie1Genre, 1.0) });


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

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreateRental_Success_test() {
            // Arrange
            var mock = new Mock<ILogger<RentalsController>>();
            ILogger<RentalsController> logger = mock.Object;

            var controller = new RentalsController(_context, logger);

            DateTime to = DateTime.Today.AddDays(6);
            DateTime from = DateTime.Today.AddDays(7);

            var rentalDTO = new RentalForCreateDTO(_userName, _customerNameSurname,
                _deliveryAddress, PaymentMethodTypes.CreditCard,
                to, from, new List<RentalItemDTO>()
                { new RentalItemDTO(2, _movie1Title, _movie1Genre, 1.0) });

            var expectedrentalDetailDTO = new RentalDetailDTO(2, DateTime.Now,
                _userName, _customerNameSurname,
                _deliveryAddress, PaymentMethodTypes.CreditCard,
                to, from, new List<RentalItemDTO>()
                { new RentalItemDTO(2, _movie1Title, _movie1Genre, 1.0) });

            // Act
            var result = await controller.CreateRental(rentalDTO);

            //Assert
            //we check that the response type is BadRequest and obtain the error returned
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualRentalDetailDTO = Assert.IsType<RentalDetailDTO>(createdResult.Value);

            Assert.Equal(expectedrentalDetailDTO, actualRentalDetailDTO);

        }

    }
}

