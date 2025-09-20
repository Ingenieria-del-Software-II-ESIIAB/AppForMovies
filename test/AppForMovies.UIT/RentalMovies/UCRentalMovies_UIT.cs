using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForMovies.UIT.RentalMovies {
    public class UCRentalMovies_UIT: UC_UIT {

        public UCRentalMovies_UIT(ITestOutputHelper output) :base(output) { }

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
            _driver.Navigate()
                    .GoToUrl(_URI + "Identity/Account/Login");
            // _driver.FindElement(By.Id("Input_Email"))
            //     .SendKeys("elena.navarro@uclm.es");
            _driver.FindElement(By.Id("Input_Email"))
                .SendKeys("elena@uclm.es");

            _driver.FindElement(By.Id("Input_Password"))
                .SendKeys("Password1234%");

            _driver.FindElement(By.Id("login-submit"))
                .Click();
        }



    }
}
