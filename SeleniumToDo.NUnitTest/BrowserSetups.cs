using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using Protractor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumToDo.NUnitTest
{

    [TestFixture]
    class ChromeSetup : StandardTests
    {
        [SetUp]
        public void SetUp()
        {

            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("http://todomvc.com");

        }
    }

    [TestFixture]
    class FirefoxSetup : StandardTests
    {
        [SetUp]
        public void SetUp()
        {

            driver = new FirefoxDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("http://todomvc.com");

        }
    }

    [TestFixture]

    class EdgeSetup : StandardTests
    {
        [SetUp]
        public void SetUp()
        {

            driver = new EdgeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("http://todomvc.com");

        }
    }

    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    class ChromeAngularSetup : AngularTests
    {
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("http://todomvc.com/examples/angularjs/#/");
            driver.Manage().Window.Maximize();
            ngDriver = new NgWebDriver(driver);
            ngDriver.Manage().Timeouts().SetScriptTimeout(new TimeSpan(0, 0, 15));

            ngDriver.WaitForAngular();
            WebDriverWait wait = new WebDriverWait(ngDriver, new TimeSpan(0, 0, 15));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("new-todo")));
            ngDriver.ExecuteScript("localStorage.clear();");
        }
    }

    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    class FirefoxAngularSetup : AngularTests
    {
        [SetUp]
        public void Setup()
        {
            driver = new FirefoxDriver();
            driver.Navigate().GoToUrl("http://todomvc.com/examples/angularjs/#/");
            driver.Manage().Window.Maximize();

            ngDriver = new NgWebDriver(driver);
            ngDriver.Manage().Timeouts().SetScriptTimeout(new TimeSpan(0, 0, 15));

            ngDriver.WaitForAngular();
            WebDriverWait wait = new WebDriverWait(ngDriver, new TimeSpan(0, 0, 15));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("new-todo")));
            ngDriver.ExecuteScript("localStorage.clear();");
        }
    }


    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    class EdgeAngularSetup : AngularTests
    {
        [SetUp]
        public void Setup()
        {
            driver = new EdgeDriver();
            driver.Navigate().GoToUrl("http://todomvc.com/examples/angularjs/#/");
            driver.Manage().Window.Maximize();
            ngDriver = new NgWebDriver(driver);
            ngDriver.Manage().Timeouts().SetScriptTimeout(new TimeSpan(0, 0, 15));
            ngDriver.WaitForAngular();
            WebDriverWait wait = new WebDriverWait(ngDriver, new TimeSpan(0, 0, 15));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("new-todo")));
            ngDriver.ExecuteScript("window.localStorage.clear();");
        }
    }
}
