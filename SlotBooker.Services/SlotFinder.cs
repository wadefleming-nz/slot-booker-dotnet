using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using SlotBooker.Services.Models;
using SlotBooker.Services.Utils;
using System;
using System.Linq;
using System.Threading;

namespace SlotBooker.Services
{
    public class SlotFinder
    {
        private const int waitForUserToLoginDelay = 60 * 1000;
        private const int waitForBookingConfirmationDelay = 3 * 60 * 1000;
        private const int retryDelay = 2 * 1000;
        private const int waitUntilClosingDelay = 5 * 60 * 1000;

        public void FindSlot(FindSlotParams findSlotParams)
        {
            var dateFormatter = new DateFormatter(findSlotParams.Date);

            using (var driver = CreateUndetectableDriver())
            {
                PrepareLoginPage(driver, findSlotParams.Email, findSlotParams.Password);
                WaitForUserToLogin(driver);
                NavigateToManageFamilyRegistrationPage(driver);
                NavigateToHoldYourAccommodationPage(driver);
                SelectNoAccessibilityNeeds(driver);
                RetryUntilSlotBooked(driver, dateFormatter);
                WaitForConfirmation(driver);
                TakeScreenshotOfConfirmation(driver);

                Thread.Sleep(waitUntilClosingDelay);
            }
        }

        private ChromeDriver CreateUndetectableDriver()
        {
            var options = new ChromeOptions();

            // prevent automation detection by recaptcha
            options.AddArgument("--disable-blink-features=AutomationControlled");

            return new ChromeDriver(options);
        }

        private void PrepareLoginPage(ChromeDriver driver, string email, string password)
        {
            driver.Url = "https://allocation.miq.govt.nz/portal/login";

            driver.FindElementByCssSelector("#gtm-acceptAllCookieButton").Click();
            driver.FindElementByCssSelector("#username").SendKeys(email);
            driver.FindElementByCssSelector("#password").SendKeys(password);
        }

        private void WaitForUserToLogin(ChromeDriver driver)
        {
            // wait for manual solve of recaptcha and login
            var selectBookingText = "Select the booking to manage";
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(waitForUserToLoginDelay));
            wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath($"//*[contains(text(), '{selectBookingText}')]")));
        }

        private void NavigateToManageFamilyRegistrationPage(ChromeDriver driver)
        {
            var viewButton = driver.FindElementByCssSelector("[aria-label=\"View Passengers\' details\"]");
            driver.ScrollToAndClick(viewButton);
        }

        private void NavigateToHoldYourAccommodationPage(ChromeDriver driver)
        {
            var secureButton = driver.FindElementByCssSelector("[aria-label='Secure your allocation']");
            driver.ScrollToAndClick(secureButton);
        }

        private void SelectNoAccessibilityNeeds(ChromeDriver driver)
        {
            var noAccessibilityNeedsOption = driver.FindElementByCssSelector("[for='form_rooms_0_accessibilityRequirement_1']");
            driver.ScrollToAndClick(noAccessibilityNeedsOption);
        }

        private bool RetryUntilSlotBooked(ChromeDriver driver, DateFormatter dateFormatter)
        {
            for (int attempt = 0; ; attempt++)
            {
                if (BookDate(driver, dateFormatter))
                    return true;

                Thread.Sleep(retryDelay);            
                driver.Navigate().Refresh();
            }
        }

        private bool BookDate(ChromeDriver driver, DateFormatter dateFormatter)
        {
            NavigateToMonth(driver, dateFormatter.MonthString);
            return BookDay(driver, dateFormatter.DateString);
        }

        private void NavigateToMonth(ChromeDriver driver, string month)
        {
            var nextMonthButton = driver.FindElementByClassName("flatpickr-next-month");
            var currentMonth = driver.FindElementByClassName("cur-month");

            driver.ScrollTo(nextMonthButton);      // must be in view to be clickable  

            while (currentMonth.Text != month)
            {
                nextMonthButton.Click();
            }
        }

        private bool BookDay(ChromeDriver driver, string date)
        {
            var dayElement = driver
                .FindElementsByCssSelector($":not(.flatpickr-disabled)[aria-label='{date}']")
                .FirstOrDefault();

            if (dayElement != null)
            {
                dayElement.Click();
                driver.FindElementById("form_next").Click();

                return true;
            }

            return false;
        }

        private void WaitForConfirmation(ChromeDriver driver)
        {
            var successText = "Managed isolation allocation is held pending flight confirmation";
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(waitForBookingConfirmationDelay));
            wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath($"//*[contains(text(), '{successText}')]")));
        }

        private void TakeScreenshotOfConfirmation(ChromeDriver driver)
        {
            Thread.Sleep(1 * 1000);     //wait for in built auto scroll to relevant section

            var screenshot = driver.GetScreenshot();
            screenshot.SaveAsFile(@"C:\temp\miq\miq-booked.png", ScreenshotImageFormat.Png);
        }
    }
}
