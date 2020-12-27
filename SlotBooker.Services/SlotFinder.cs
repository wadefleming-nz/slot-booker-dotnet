using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using SlotBooker.Services.Models;
using SlotBooker.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SlotBooker.Services
{
    public class SlotFinder
    {
        private const int elementWaitTimeout = 10 * 1000;

        private const int waitForUserToLoginDelay = 60 * 1000;
        private const int waitForBookingConfirmationDelay = 3 * 60 * 1000;
        private const int retryDelay = 2 * 1000;
        private const int waitUntilClosingDelay = 2 * 60 * 1000;

        public FindSlotResult FindSlot(FindSlotParams findSlotParams)
        {
            var dateFormatters = findSlotParams.Dates.Select(d => new DateFormatter(d)).ToList();

            using (var driver = CreateUndetectableDriver())
            {
                PrepareLoginPage(driver, findSlotParams.Email, findSlotParams.Password);
                WaitForUserToLogin(driver);
                NavigateToManageFamilyRegistrationPage(driver);
                NavigateToHoldYourAccommodationPage(driver);
                SelectNoAccessibilityNeeds(driver);
                var dateBooked = RetryUntilSlotBooked(driver, dateFormatters);
                WaitForConfirmation(driver);

                Thread.Sleep(waitUntilClosingDelay);
                return new FindSlotResult { Succeeded = true, DateBooked = dateBooked };
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

            driver.FindElementById("gtm-acceptAllCookieButton").Click();
            driver.FindElementById("username").SendKeys(email);
            driver.FindElementById("password").SendKeys(password);
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

        private DateTime RetryUntilSlotBooked(ChromeDriver driver, List<DateFormatter> dateFormatters)
        {
            for (int attempt = 0; ; attempt++)
            {
                var result = BookAnyDate(driver, dateFormatters);
                if (result.Succeeded)
                {
                    return result.DateBooked.Value;
                }

                Thread.Sleep(retryDelay);            
                driver.Navigate().Refresh();
            }
        }

        private (bool Succeeded, DateTime? DateBooked) BookAnyDate(ChromeDriver driver, List<DateFormatter> dateFormatters)
        {
            foreach (DateFormatter dateFormatter in dateFormatters)
            {
                if (BookDate(driver, dateFormatter))
                    return (true, dateFormatter.Date);
            }

            return (false, null);
        }

        private bool BookDate(ChromeDriver driver, DateFormatter dateFormatter)
        {
            NavigateToMonth(driver, dateFormatter.MonthString);
            return BookDay(driver, dateFormatter.DateString);
        }

        private void NavigateToMonth(ChromeDriver driver, string month)
        {
            const string nextMonthButtonClass = "flatpickr-next-month";
            const string currMonthTextClass = "cur-month";

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(elementWaitTimeout));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName(nextMonthButtonClass)));
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName(currMonthTextClass)));

            var nextMonthButton = driver.FindElementByClassName(nextMonthButtonClass);
            var currentMonth = driver.FindElementByClassName(currMonthTextClass);

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
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(waitForBookingConfirmationDelay));
            wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath($"//*[contains(text(), '{successText}')]")));
        }
    }
}
