using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForMovies.UIT.RentalMovies {
    public class CreateRental_PO:PageObject {

        private By _nameSurnameBy = By.Id("NameSurname");
        private IWebElement FindNameSurname() => _driver.FindElement(_nameSurnameBy);
        private IWebElement FindDeliveryAddress() => _driver.FindElement(By.Id("DeliveryAddress"));
        private IWebElement FindPaymentMethod() => _driver.FindElement(By.Id("PaymentMethod"));

        private IWebElement FindValidationError() => _driver.FindElement(By.Id("ValidationError"));




        public CreateRental_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output) {
        }

        public void FillInRentalInfo(string nameSurname, string deliveryAddress, string paymentMethod) {
            WaitForBeingVisible(_nameSurnameBy);
            FindNameSurname().SendKeys(nameSurname);
            FindDeliveryAddress().SendKeys(deliveryAddress);

            //create select element object 
            SelectElement selectElement = new SelectElement(FindPaymentMethod());

            //select Action from the dropdown menu
            selectElement.SelectByText(paymentMethod);
        }

        public void FillInRentalDescription(string rentalDescription, int movieId) {
            _driver.FindElement(By.Id("description_" + movieId)).SendKeys(rentalDescription);
        }


        public void PressRentYourMovies() {
            WaitForBeingClickable(By.Id("Submit"));
            _driver.FindElement(By.Id("Submit")).Click();
        }



        public void PressModifyMovies() {
            
            WaitForBeingClickable(By.Id("ModifyMovies"));
            _driver.FindElement(By.Id("ModifyMovies")).Click();
        }

        public bool CheckListOfRentalItems(List<string[]> expectedRentalItems) {
            return CheckBodyTable(expectedRentalItems, By.Id("TableOfRentalItems"));
        }

        public string ReturnValidationError() {
            return FindValidationError().Text;

        }

    }
}
