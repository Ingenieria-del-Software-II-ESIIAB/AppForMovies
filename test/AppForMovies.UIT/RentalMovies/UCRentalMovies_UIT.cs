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

        private void Precondition_perform_login() {
            Perform_login("elena@uclm.es", "Password1234%");
        }


        [Theory]
        [InlineData(movieTitle1, movieGenre1, movieReleaseDate1, moviePriceForRenting1, "last of", "")]
        [InlineData(movieTitle2, movieGenre2, movieReleaseDate2, moviePriceForRenting2, "", movieGenre2)]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_4_5_AF1_filteringbyTitleandGenre(string title, string genre,
            string releasedate, string price, string filterTitle, string filterGenre) {
            //Arrange
            var listmovies = new ListMoviesForRental_PO(_driver, _output);


            var from = DateTime.Today.AddDays(2);
            var to = DateTime.Today.AddDays(3);
            var expectedMovies = new List<string[]> { new string[] { title, genre, releasedate, price }, };
            //Act
            Precondition_perform_login();
            listmovies.WaitForBeingVisibleIgnoringExeptionTypes(By.Id("CreateRenting"));
            _driver.FindElement(By.Id("CreateRenting")).Click();

            listmovies.FilterMovies(filterTitle, filterGenre, from, to);

            //Assert            
            Assert.True(listmovies.CheckListOfMovies(expectedMovies));

        }



    }
}
