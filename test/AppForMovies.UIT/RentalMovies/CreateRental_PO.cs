using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForMovies.UIT.RentalMovies {
    public class CreateRental_PO:PageObject {

        private By _nameSurnameBy = By.Id("NameSurname");
        private IWebElement _nameSurname() => _driver.FindElement(_nameSurnameBy);
        private IWebElement _deliveryAddress() => _driver.FindElement(By.Id("DeliveryAddress"));
        private IWebElement _paymentMethod() => _driver.FindElement(By.Id("PaymentMethod"));




        public CreateRental_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output) {
        }

        public void FillInRentalInfo(string nameSurname, string deliveryAddress, string paymentMethod) {
            WaitForBeingVisible(_nameSurnameBy);
            _nameSurname().SendKeys(nameSurname);
            _deliveryAddress().SendKeys(deliveryAddress);

            //create select element object 
            SelectElement selectElement = new SelectElement(_paymentMethod());

            //select Action from the dropdown menu
            selectElement.SelectByText(paymentMethod);
        }

        public void FillInRentalDescription(string rentalDescription, int movieId) {
            _driver.FindElement(By.Id("description_" + movieId)).SendKeys(rentalDescription);
        }


        public void PressRentYourMovies() {
            _driver.FindElement(By.Id("Submit")).Click();
        }



        public void PressModifyMovies() {
            _driver.FindElement(By.Id("ModifyMovies")).Click();
        }

        public bool CheckListOfRentalItems(List<string[]> expectedRentalItems) {
            return CheckBodyTable(expectedRentalItems, By.Id("TableOfRentalItems"));
        }

        public bool CheckValidationError(string expectedError) {
            return _driver.PageSource.Contains(expectedError);
        }

    }
}
