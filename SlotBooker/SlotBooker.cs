using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Linq;
using System.Threading;

namespace SlotBooker
{
    public class SlotBooker
    {
        public void FindSlot()
        {
            string month = "January";

            var options = new ChromeOptions();

            // prevent automation detection by recaptcha
            options.AddArgument("--disable-blink-features=AutomationControlled");

            var driver = new ChromeDriver(options);

            driver.Url = "https://allocation.miq.govt.nz/portal/login";

            driver.FindElementByCssSelector("#gtm-acceptAllCookieButton").Click();
            driver.FindElementByCssSelector("#username").SendKeys("wadefleming@yahoo.com");
            driver.FindElementByCssSelector("#password").SendKeys("Dembava12345");

            // wait for manual solve of recaptcha
            var selectBookingText = "Select the booking to manage";
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath($"//*[contains(text(), '{selectBookingText}')]")));

            var viewButton = driver.FindElementByCssSelector("[aria-label=\"View Passengers\' details\"]");
            ScrollTo(driver, viewButton);
            Thread.Sleep(1 * 1000);
            viewButton.Click();

            var secureButton = driver.FindElementByCssSelector("[aria-label='Secure your allocation']");
            ScrollTo(driver, secureButton);
            Thread.Sleep(1 * 1000);
            secureButton.Click();

            // select No accessibility needs
            var accessibilityOption = driver.FindElementByCssSelector("[for='form_rooms_0_accessibilityRequirement_1']");
            ScrollTo(driver, accessibilityOption);
            Thread.Sleep(1 * 1000);
            accessibilityOption.Click();

            for (int retry = 0; ; retry++)
            {
                if (BookDate(driver, month, 999))
                    break;

                Thread.Sleep(2 * 1000);             // delay between retries
                driver.Navigate().Refresh();
            }

            Thread.Sleep(10 * 1000);

            driver.Close();
        }

        private bool BookDate(ChromeDriver driver, string month, int day)
        {
            NavigateToMonth(driver, month);
            return BookDay(driver, day);
        }

        private void NavigateToMonth(ChromeDriver driver, string month)
        {
            var nextMonthButton = driver.FindElementByClassName("flatpickr-next-month");
            var currentMonth = driver.FindElementByClassName("cur-month");

            ScrollTo(driver, nextMonthButton);      // must be in view to be clickable  
            Thread.Sleep(1 * 1000);

            while (currentMonth.Text != month)
            {
                nextMonthButton.Click();
            }
        }

        private bool BookDay(ChromeDriver driver, int day)
        {
            var dayElement = driver
                .FindElementsByCssSelector(":not(.flatpickr-disabled)[aria-label='January 19, 2021']")
                .FirstOrDefault();

            if (dayElement != null)
            {
                dayElement.Click();
                driver.FindElementById("form_next").Click();

                var successText = "Managed isolation allocation is held pending flight confirmation";
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(180));
                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath($"//*[contains(text(), '{successText}')]")));

                Thread.Sleep(1 * 1000);     //wait for in built auto scroll

                // take screenshot
                var screenshot = driver.GetScreenshot();
                screenshot.SaveAsFile(@"C:\temp\miq\miq-booked.png", ScreenshotImageFormat.Png);

                Thread.Sleep(5 * 1000);
                return true;
            }

            return false;
        }

        private void ScrollTo(ChromeDriver driver, IWebElement element)
        {
            int elementPosition = element.Location.Y;
            string javascript = $"window.scroll(0, { elementPosition })";
            driver.ExecuteScript(javascript);
        }
    }
}
