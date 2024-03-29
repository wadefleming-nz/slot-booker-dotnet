﻿using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;

namespace SlotBooker.Services.Utils
{
    public static class RemoteWebDriverExtensions
    {
        public static void ScrollTo(this RemoteWebDriver driver, IWebElement element)
        {
            const int scrollWait = 1 * 1000;

            int elementPosition = element.Location.Y;
            string javascript = $"window.scroll(0, { elementPosition })";
            driver.ExecuteScript(javascript);
            Thread.Sleep(scrollWait);
        }

        public static void ScrollToAndClick(this RemoteWebDriver driver, IWebElement element)
        {
            ScrollTo(driver, element);
            element.Click();
        }

        public static IWebElement WaitFor(this RemoteWebDriver driver, By locator)
        {
            TimeSpan defaultTimeout = TimeSpan.FromSeconds(10);
            WebDriverWait wait = new WebDriverWait(driver, defaultTimeout);
            return wait.Until(ExpectedConditions.ElementIsVisible(locator));
        }
    }
}
