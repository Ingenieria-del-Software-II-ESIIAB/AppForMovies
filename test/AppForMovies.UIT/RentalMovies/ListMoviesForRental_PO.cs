using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForMovies.UIT.RentalMovies {
    public class ListMoviesForRental_PO:PageObject {

        private By _movieTitleBy = By.Id("movieTitle");
        private By _movieGenreBy = By.Id("selectGenre");
        private By _fromBy = By.Id("fromDate");
        private By _toBy = By.Id("toDate");


        private By _ShowRentingCartBy = By.Id("showRentingCart");
        private By _searchMoviesBy = By.Id("searchMovies");
        private By _rentButtonBy = By.Id("Rent");

        private By _tableOfMoviesBy = By.Id("TableOfMovies");
        private By _modalBy = By.Id("DialogOKSaveDelete");

        private IWebElement _movieTitle() => _driver.FindElement(_movieTitleBy);
        //this code is a shortcut for:
        // private IWebElement _movieTitle() {return _driver.FindElement(By.Id("movieTitle"));}

        private IWebElement _movieGenre() => _driver.FindElement(_movieGenreBy);
        private IWebElement _showRentingCartButton() => _driver.FindElement(_ShowRentingCartBy);
        private IWebElement _searchMoviesButton() => _driver.FindElement(_searchMoviesBy);
        private IWebElement _rentButton() => _driver.FindElement(_rentButtonBy);


        public ListMoviesForRental_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output) {

        }


        public void FilterMovies(string titleFilter, string genreSelected,
            DateTime from, DateTime to) {
            WaitForBeingVisible(_movieTitleBy);


            _movieTitle().SendKeys(titleFilter);

            //if no genre is selected then all the genres are applicable
            if (genreSelected == "") genreSelected = "All";

            //create select element object 
            SelectElement selectElement = new SelectElement(_movieGenre());
            //select Action from the dropdown menu
            selectElement.SelectByText(genreSelected);

            InputDateInDatePicker(_fromBy, from);
            InputDateInDatePicker(_toBy, to);

            _searchMoviesButton().Click();
            //we wait for 2 seconds (2000 milliseconds) till the table is reloaded as we have to wait for the API service to be called
            System.Threading.Thread.Sleep(2000);

        }

        public void SelectMovies(List<string> movieTitles) {
            //we wait for till the movies are available to be selected 
            foreach (var movieTitle in movieTitles) {
                WaitForBeingVisible(By.Id($"movieToRent_{movieTitle}"));
                _driver.FindElement(By.Id($"movieToRent_{movieTitle}")).Click();
            }
        }

        public void RentMovies() {
            WaitForBeingClickable(_rentButtonBy);
            _rentButton().Click();
        }

        public void ModifyRentingCart(string title) {
            _showRentingCartButton().Click();
            WaitForBeingVisible(By.Id($"removeMovie_{title}"));
            _driver.FindElement(By.Id($"removeMovie_{title}")).Click();

        }

        public bool CheckListOfMovies(List<string[]> expectedMovies) {

            return CheckBodyTable(expectedMovies, _tableOfMoviesBy);
        }

        public bool CheckRentMoviesDisabled() {
            //we return true if the button is disabled
            return !(_rentButton().Enabled);
        }

        public bool CheckShoppingCart(string price) {
            //string texto = _showRentingCartButton().Text;
            //WaitForTextToBePresentInElement(_rentButtonBy, $"Renting Cart: {price} €" );
            return _showRentingCartButton().Text.Contains(price);
        }

        public bool CheckMessageErrorNotAvaibleMovies(string expectedError) {
            return _driver.PageSource.Contains(expectedError);

        }

        public bool CheckMessageError(string expectedError) {
            return CheckModalBodyText(expectedError, _modalBy);
        }

    }
}
