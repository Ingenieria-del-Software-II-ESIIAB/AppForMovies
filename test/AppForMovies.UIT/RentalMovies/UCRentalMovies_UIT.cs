using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForMovies.UIT.RentalMovies {
    public class UCRentalMovies_UIT: UC_UIT {

        public UCRentalMovies_UIT(ITestOutputHelper output) :base(output) {
            Initial_step_opening_the_web_page();
            listmovies = new ListMoviesForRental_PO(_driver, _output);
        }

        private const int movieId1 = 1;
        private const string movieTitle1 = "The last of us";
        private const string movieGenre1 = "Sci - Fi";
        private const string moviePriceForRenting1 = "1";
        private const string movieReleaseDate1 = "15/03/2023";
        private const string movieRentalDescription1 = "Loved the computer game it is based on.";

        private const string movieTitle2 = "The man in the high castle";
        private const string movieGenre2 = "Drama";
        private const string moviePriceForRenting2 = "3";
        private const string movieReleaseDate2 = "15/01/2015";

        private ListMoviesForRental_PO listmovies;

        private void Precondition_perform_login() {
            Perform_login("elena@uclm.es", "Password1234%");
        }

        private void InitialStepsForRentalMovies_UIT() {
            Precondition_perform_login();
            listmovies.WaitForBeingVisibleIgnoringExeptionTypes(By.Id("CreateRenting"));
            _driver.FindElement(By.Id("CreateRenting")).Click();
        }


        [Theory]
        [InlineData(movieTitle1, movieGenre1, movieReleaseDate1, moviePriceForRenting1, "last of", "")]
        [InlineData(movieTitle2, movieGenre2, movieReleaseDate2, moviePriceForRenting2, "", movieGenre2)]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_4_5_AF1_filteringbyTitleandGenre(string title, string genre,
            string releasedate, string price, string filterTitle, string filterGenre) {
            //Arrange

            var from = DateTime.Today.AddDays(2);
            var to = DateTime.Today.AddDays(3);
            var expectedMovies = new List<string[]> { new string[] { title, genre, releasedate, price }, };
            //Act
            InitialStepsForRentalMovies_UIT();

            listmovies.FilterMovies(filterTitle, filterGenre, from, to);

            //Assert            
            Assert.True(listmovies.CheckListOfMovies(expectedMovies));

        }


        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_6_AF1_filteringbyDate() {
            //Arrange
            var from = DateTime.Today.AddDays(2);
            var to = DateTime.Today.AddDays(3);
            var expectedMovies = new List<string[]> { new string[] { movieTitle1, movieGenre1, movieReleaseDate1, moviePriceForRenting1 },
                                                       new string[] { movieTitle2, movieGenre2, movieReleaseDate2, moviePriceForRenting2 },};
            //Act
            InitialStepsForRentalMovies_UIT();

            listmovies.FilterMovies("", "", from, to);

            //Assert

            Assert.True(listmovies.CheckListOfMovies(expectedMovies));

        }

        public static IEnumerable<object[]> TestCasesFor_UC2_4_5_AF2_errorindates() {
            var allTests = new List<object[]> {
                new object[] { DateTime.Today.AddDays(-1), DateTime.Today.AddDays(2), "Your rental period must be later",  },
                //cannot be checked if datetime is before today, because the next condition is checked before
                new object[] { DateTime.Today.AddDays(-2), DateTime.Today.AddDays(-1), "Your rental period must be later", },
                new object[] { DateTime.Today.AddDays(7), DateTime.Today.AddDays(5), "Your rental must end after than its starts", },
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_UC2_4_5_AF2_errorindates))]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_7_8_9_AF2_errorindates(DateTime from, DateTime to, string error) {
            //Arrange


            //Act
            InitialStepsForRentalMovies_UIT();

            listmovies.FilterMovies("", "", from, to);

            //Assert

            //this message will be shown if assert fails
            Assert.True(listmovies.CheckMessageError(error), $"Error in the message box for test {from} - {to}");

        }


        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_10_AF3_ModifySelectedMovies() {
            //Arrange

            var from = DateTime.Today.AddDays(2);
            var to = DateTime.Today.AddDays(3);
            //Act
            InitialStepsForRentalMovies_UIT();

            listmovies.FilterMovies("", "", from, to);
            listmovies.SelectMovies(new List<string> { movieTitle1, movieTitle2 });
            listmovies.ModifyRentingCart(movieTitle2);


            //Assert            
            Assert.True(listmovies.CheckShoppingCart(moviePriceForRenting1));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_11_AF4_RentButtonNotAvailable() {
            //Arrange

            var from = DateTime.Today.AddDays(2);
            var to = DateTime.Today.AddDays(3);
            //Act
            InitialStepsForRentalMovies_UIT();

            listmovies.FilterMovies("", "", from, to);
            listmovies.SelectMovies(new List<string> { movieTitle1 });
            listmovies.ModifyRentingCart(movieTitle1);


            //Assert            
            Assert.True(listmovies.CheckRentMoviesDisabled(), "Rent button should be disabled");
        }


        [Theory]
        [InlineData("", "Calle de la Universidad 1, Albacete, 02006, España", "The CustomerNameSurname field is required")]
        [InlineData("Elena", "Calle de la Universidad 1, Albacete, 02006, España", "The field CustomerNameSurname must be a string with a minimum length of 10 and a maximum length of 50")]
        [InlineData("Elena Navarro", "", "The DeliveryAddress field is required")]
        [InlineData("Elena Navarro", "Calle", "The field DeliveryAddress must be a string with a minimum length of 10 and a maximum length of 50")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_12_13_14_15_AF5_testingErrorsMandatorydata(string nameSurname, string deliveryAddress,
            string expectedMessageError) {
            //Arrange

            var createrental = new CreateRental_PO(_driver, _output);

            var from = DateTime.Today.AddDays(2);
            var to = DateTime.Today.AddDays(3);
            //Act
            InitialStepsForRentalMovies_UIT();

            listmovies.FilterMovies("", "", from, to);
            listmovies.SelectMovies(new List<string> { movieTitle1 });
            listmovies.RentMovies();
            createrental.FillInRentalInfo(nameSurname, deliveryAddress, "CreditCard");
            createrental.PressRentYourMovies();

            //Assert
            //the expected error is shown in the view
            Assert.True(createrental.CheckValidationError(expectedMessageError), $"Expected error: {expectedMessageError}");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_16_AF6_ModifyRentalItems() {
            //Arrange

            var createrental = new CreateRental_PO(_driver, _output);

            var from = DateTime.Today.AddDays(2);
            var to = DateTime.Today.AddDays(3);
            //Act
            InitialStepsForRentalMovies_UIT();

            listmovies.FilterMovies("", "", from, to);
            listmovies.SelectMovies(new List<string> { movieTitle1, movieTitle2 });
            listmovies.RentMovies();
            createrental.PressModifyMovies();
            //we remove movietitle2 from the rentingcart
            listmovies.ModifyRentingCart(movieTitle2);
            listmovies.RentMovies();

            //Assert
            //the list of movies must change
            var expectedRentalItems = new List<string[]> { new string[] { movieTitle1, movieGenre1, moviePriceForRenting1 }, };
            Assert.True(createrental.CheckListOfRentalItems(expectedRentalItems));
        }

        [Fact(Skip = "First change the quantifyofrenting of the movies to 0 using script dbo.Movies.QuantityForRenting0")]
        //[Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_17_AF0_MoviesNotAvailableForRentalPeriod() {
            //Arrange
 
            var from = DateTime.Today.AddDays(2);
            var to = DateTime.Today.AddDays(3);
            var expectedMessage = "There are no movies available for being rented";
            //Act
            InitialStepsForRentalMovies_UIT();

            listmovies.FilterMovies("", "", from, to);

            //Assert
            //this message will be shown if assert fails
            Assert.True(listmovies.CheckMessageErrorNotAvaibleMovies(expectedMessage), $"Movie {movieTitle2} with genre {movieGenre2} does not exist");

        }

        [Theory]
        [InlineData("Elena Navarro", "Calle de la Universidad 1, Albacete, 02006, España", "CreditCard")]
        [InlineData("Elena Navarro", "Calle de la Universidad 1, Albacete, 02006, España", "PayPal")]
        [InlineData("Elena Navarro", "Calle de la Universidad 1, Albacete, 02006, España", "Cash")]

        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_1_2_3_BasicFlow(string nameSurname, string deliveryAddress, string paymentMethod) {
            //Arrange

            var createrental = new CreateRental_PO(_driver, _output);
            var detailRental = new DetailRental_PO(_driver, _output);

            var from = DateTime.Today.AddDays(1);
            var to = DateTime.Today.AddDays(2);



            //Act
            InitialStepsForRentalMovies_UIT();

            listmovies.FilterMovies("", "", from, to);
            listmovies.SelectMovies(new List<string> { movieTitle1 });
            listmovies.RentMovies();

            createrental.FillInRentalInfo(nameSurname, deliveryAddress, paymentMethod);
            createrental.FillInRentalDescription(movieRentalDescription1, movieId1);
            createrental.PressRentYourMovies();
            createrental.PressOkModalDialog();


            //Assert
            //the expected error is shown in the view
            Assert.True(detailRental.CheckRentalDetail(nameSurname,
                deliveryAddress, paymentMethod, DateTime.Now, from, to, moviePriceForRenting1 + " €"),
                "Error: detail rental is not as expected");

            var expectedRentalItems = new List<string[]>
                    { new string[] { movieTitle1, movieGenre1, moviePriceForRenting1+" €" , movieRentalDescription1}, };

            Assert.True(detailRental.CheckListOfMovies(expectedRentalItems),
                "Error: rental items are not as expected");

        }

    }
}
