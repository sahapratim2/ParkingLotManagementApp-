using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using Bogus;
using Microsoft.Extensions.Configuration;
using ParkingManagementApp.Common.DataConstants;

namespace ParkingManagementApp.Tests
{
    public class HomeViewFrontendTest : IDisposable
    {

        private readonly IWebDriver _driver;

        private string _baseUrl = "http://localhost:7001"; // Have To Replae With App's URL

        private readonly Faker _faker;

        private readonly IConfiguration _configuration ;

        public HomeViewFrontendTest()
        {

           var configurationBuilder = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json")
          .Build();

            _configuration = configurationBuilder;

            _baseUrl = _configuration[AppSettingConstants.APPSETTINGS_CONSTANTS.API_URL];

            _driver = new ChromeDriver();

            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            _faker = new Faker();

        }

        [Fact]
        public void CheckIn_Button_Should_Return_Success_Message()
        {
            _driver.Navigate().GoToUrl(_baseUrl);

            IWebElement input = _driver.FindElement(By.Id("tagNumber"));

            IWebElement target = _driver.FindElement(By.Id("btnCheckIn"));

            IWebElement message = _driver.FindElement(By.Id("successMessage"));

            string randomTagNumber = _faker.Random.AlphaNumeric(6);

            input.SendKeys(randomTagNumber);

            target.Click();

            Thread.Sleep(1000);

            Assert.Equal("Successfully reserved a parking lot.", message.Text);
        }

        [Fact]
        public void CheckIn_Button_Should_Return_Car_Already_Park_Message()
        {
            _driver.Navigate().GoToUrl(_baseUrl);

            IWebElement input = _driver.FindElement(By.Id("tagNumber"));

            IWebElement target = _driver.FindElement(By.Id("btnCheckIn"));

            IWebElement message = _driver.FindElement(By.Id("alertMessage"));

            string randomTagNumber = _faker.Random.AlphaNumeric(8);

            input.SendKeys(randomTagNumber);

            target.Click();

            Thread.Sleep(1000);

            IWebElement closeButton = _driver.FindElement(By.CssSelector("button[data-bs-dismiss='modal']"));

            closeButton.Click();

            input.SendKeys(randomTagNumber);

            target.Click();// Trying to Park Same Car

            Thread.Sleep(1000);

            Assert.Equal("Car already in the parking lot.", message.Text);
        }

        [Fact]
        public void CheckOut_Button_Should_Return_Payment_Message()
        {
            _driver.Navigate().GoToUrl(_baseUrl);

            IWebElement input = _driver.FindElement(By.Id("tagNumber"));

            IWebElement checkIn = _driver.FindElement(By.Id("btnCheckIn"));

            IWebElement checkOut = _driver.FindElement(By.Id("btnCheckOut"));

            IWebElement message = _driver.FindElement(By.Id("successMessage"));

            string randomTagNumber = _faker.Random.AlphaNumeric(8);

            input.SendKeys(randomTagNumber);

            checkIn.Click();// Check In

            Thread.Sleep(1000);

            IWebElement closeButton = _driver.FindElement(By.CssSelector("button[data-bs-dismiss='modal']"));

            closeButton.Click();

            input.SendKeys(randomTagNumber);

            checkOut.Click();// Check Out

            Thread.Sleep(1000);

            Assert.Contains("Your have to pay", message.Text);
        }

        [Fact]
        public void CheckOut_Button_Should_Return_Car_Not_Resgistered_Message()
        {
            _driver.Navigate().GoToUrl(_baseUrl);

            IWebElement input = _driver.FindElement(By.Id("tagNumber"));

            IWebElement target = _driver.FindElement(By.Id("btnCheckOut"));

            IWebElement message = _driver.FindElement(By.Id("alertMessage"));

            string randomTagNumber = _faker.Random.AlphaNumeric(8);

            input.SendKeys(randomTagNumber);

            target.Click();

            Thread.Sleep(1000);

            Assert.Equal("The car is not registered in the parking lot.", message.Text);
        }

        [Fact]
        public void Stats_Button_Should_Return_Table_Data_From_Modal()
        {
            _driver.Navigate().GoToUrl(_baseUrl);

            IWebElement statsButton = _driver.FindElement(By.Id("btnShowStats"));

            statsButton.Click();

            Thread.Sleep(1000);

            IWebElement table = _driver.FindElement(By.Id("tblParkingStats"));

            Assert.False(string.IsNullOrEmpty(table.GetAttribute("innerHTML")));
        }

        public void Dispose()
        {
            _driver.Quit();
            GC.SuppressFinalize(this);
        }
    }
}
