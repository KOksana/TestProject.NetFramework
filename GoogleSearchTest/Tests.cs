﻿using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;

namespace GoogleSearchTest
{
    public class Tests
    {
        private IWebDriver _driver;
        private const string URL = "https://www.google.com/";

        private const string SEARCH_FIELD_XPATH = "//input[@class = 'gLFyf gsfi']";
        private const string SEARCH_FIELD_CSS = "input[class = 'gLFyf gsfi']";

        private const string SEARCH_BUTTON_XPATH = "//div[@class = 'tfB0Bf']//input[@class = 'gNO89b']";
        private const string SEARCH_BUTTON_CSS = "div[class = 'tfB0Bf'] input[class = 'gNO89b']";

        private const string WIKI_RESULT_XPATH = "//h3[text()='XPath - Wikipedia']/../..";
        private const string WIKI_RESULT_CSS = "div[class = 'r'] a[href $= 'wiki/XPath']";

        private const string CONTEXT_LIST_XPATH = "//div[@id= 'toc']/ul//a";
        private const string CONTEXT_LIST_CSS = "div[id= 'toc']>ul a";

        [OneTimeSetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddArguments("--lang=en-GB");

            _driver = new ChromeDriver(options);
            _driver.Manage().Window.Maximize();
        }

        [Test]
        public void Test1()
        {
            _driver.Navigate().GoToUrl(URL);

            var wait = new WebDriverWait(_driver, new TimeSpan(0, 0, 5));
            wait.Until(drv => drv.FindElement(By.XPath(SEARCH_FIELD_XPATH)));

            _driver.FindElement(By.CssSelector(SEARCH_FIELD_CSS)).SendKeys("xpath");

            wait.Until(d => _driver.FindElement(By.CssSelector(SEARCH_BUTTON_CSS)).Displayed);

            var searchButton = _driver.FindElement(By.CssSelector(SEARCH_BUTTON_CSS));
            var actions = new Actions(_driver);
            actions.MoveToElement(searchButton).Build().Perform();

            _driver.FindElement(By.XPath(SEARCH_BUTTON_XPATH)).Click();

            wait.Until(drv => drv.FindElement(By.XPath(WIKI_RESULT_XPATH)));

            _driver.FindElements(By.CssSelector(WIKI_RESULT_CSS))[0].Click();

            wait.Until(drv => drv.FindElement(By.ClassName("toctitle")));

            var allContextElements = _driver.FindElements(By.CssSelector(CONTEXT_LIST_CSS));

            var contextListLength = allContextElements.Count;
            Assert.IsTrue(contextListLength == 37, $"Number of elements in context should be 36, but it is {contextListLength}");
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _driver.Quit();
        }
    }
}
