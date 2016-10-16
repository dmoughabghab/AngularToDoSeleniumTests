using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Protractor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeleniumToDo.NUnitTest
{

    public abstract class StandardTests
    {
        public IWebDriver driver;

        static Tuple<string, bool> WaitForNewPage(IWebDriver driver, IWebElement element, string url, int secondsForTimeout = 15)
        {

            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, secondsForTimeout));
            bool result = wait.Until(ExpectedConditions.StalenessOf(element));
            if (result)
            {
                result = wait.Until(ExpectedConditions.UrlMatches(url));

                if (result)
                {
                    return new Tuple<string, bool>("success | page change", true);
                }
                else
                    return new Tuple<string, bool>("failed | didn't reach target url", false);
            }
            else
                return new Tuple<string, bool>("failed | didn't leave page", false);

        }

        //The first step of your browser tests should be to begin at http://todomvc.com and browse to the AngularJS link.
        [Test]
        public void NavigateToAngularToDo()
        {
            var elementLink = driver.FindElement(By.CssSelector("a[href='examples/angularjs']"));

            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 15));
            wait.Until(ExpectedConditions.ElementToBeClickable(elementLink));

            elementLink.Click();
            var result = WaitForNewPage(driver, elementLink, "http://todomvc.com/examples/angularjs/#/");
            Assert.True(result.Item2, result.Item1);
        }
        [TearDown]
        public void TearDown()
        {
            driver.Close();
            driver.Dispose();
        }
    }


    public abstract class AngularTests
    {


        public void AddToDo(string todo)
        {


            NgWebElement todoForm = ngDriver.FindElement(By.Id("todo-form"));
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            NgWebElement todoInput = ngDriver.FindElement(By.Id("new-todo"));
            wait.Until(ExpectedConditions.ElementToBeClickable(todoInput));
            todoInput.SendKeys(todo);
            todoForm.Submit();
            ngDriver.WaitForAngular();


        }


        //I want to add a To-do item
        [Test]
        public void AddToDoTest()
        {
            AddToDo("AddToDo");
            NgWebElement toDoList = ngDriver.FindElement(By.Id("todo-list"));
            bool result = toDoList.FindElements(By.CssSelector("li")).Count == 1;

            Assert.True(result, "to-do was not added");
        }

        // I can add a second To-do
        [Test]
        public void AddSecondToDoTest()
        {
 
            AddToDo("FirstToDo");

            NgWebElement toDoList = ngDriver.FindElement(By.Id("todo-list"));
            bool result = toDoList.FindElements(By.CssSelector("li")).Count == 1;

            Assert.True(result, "First to-do was not added");

            AddToDo("SecondToDo");

            //toDoList = ngDriver.FindElement(By.Id("todo-list"));
            result = toDoList.FindElements(By.CssSelector("li")).Count == 2;

            Assert.True(result, "Second to-do was not added");

        }

        //I want to edit the content of an existing To-do item
        [Test]
        public void EditToDo()
        {
            AddToDo("ToDoToBeEdited");

            NgWebElement toDoList = ngDriver.FindElement(By.Id("todo-list"));

            Actions act = new Actions(ngDriver);
            NgWebElement item = toDoList.FindElement(By.CssSelector("li"));
            NgWebElement label = item.FindElement(By.CssSelector("label"));
            act.MoveToElement(label).DoubleClick().Perform();
            NgWebElement editInput = item.FindElement(NgBy.Model("todo.title"));
            editInput.SendKeys(Keys.Control + "a");
            editInput.SendKeys("ToDoThatHasBeenEdited");
            ngDriver.FindElement(By.Id("new-todo")).Click();

            Assert.AreEqual("ToDoThatHasBeenEdited", label.Text, "the todo wasn't modified");
        }

        //I can complete a To-do by clicking inside the circle UI to the left of the To-do
        [Test]
        public void CompleteToDo()
        {

                AddToDo("ToDoToBeCompleted");
                NgWebElement toDoList = ngDriver.FindElement(By.Id("todo-list"));

                Actions act = new Actions(ngDriver);
                NgWebElement item = toDoList.FindElement(By.CssSelector("li"));

                Assert.True(!item.GetAttribute("class").Contains("completed"), "item was already set to completed without being clicked");

                item.FindElement(NgBy.Model("todo.completed")).Click();

                Assert.True(item.GetAttribute("class").Contains("completed"), "item is still not to completed after it has been clicked");

        }


        //I can clear a single To-do item from the list completely by clicking the Close icon.
        [Test]
        public void DeleteToDo()
        {


                AddToDo("ToDoToBeDeleted");
                NgWebElement toDoList = ngDriver.FindElement(By.Id("todo-list"));
                Assert.AreEqual(1, toDoList.FindElements(By.CssSelector("li")).Count, "to-do item didn't add");

                NgWebElement item = toDoList.FindElement(By.CssSelector("li"));
                Actions act = new Actions(ngDriver);
                act.MoveToElement(item).Perform();

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button.destroy")));

                item.FindElement(By.CssSelector("button.destroy")).Click();

                Assert.Zero(toDoList.FindElements(By.CssSelector("li")).Count, "to-do item wasn't deleted");

        }

        //I can complete all active To-dos by clicking the down arrow at the top-left of the UI
        [Test]
        public void SetAllCompleted()
        {
            AddToDo("FirstToDoToBeSetToComplete");
            AddToDo("SecondToDoToBeSetToComplete");

            NgWebElement toDoList = ngDriver.FindElement(By.Id("todo-list"));

            int countAll = toDoList.FindElements(By.CssSelector("li")).Count;

            Assert.AreEqual(2, countAll, "the 2 test to-do's weren't added");

            int countAllCompletedPreClick = toDoList.FindElements(By.CssSelector("li.completed")).Count;

            Assert.AreEqual(0, countAllCompletedPreClick, "the to-do's began as completed");

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("toggle-all")));

            ngDriver.FindElement(By.Id("toggle-all")).Click();

            int countAllCompletedAfterClick = toDoList.FindElements(By.CssSelector("li.completed")).Count;

            Assert.AreEqual(2, countAllCompletedAfterClick, "the to-do's did not become completed");


        }


        //I can clear all completed To-do items from the list completely
        [Test]
        public void ClearCompleted()
        {

            AddToDo("ToDoToBeCleared");
            AddToDo("SecondToDoToNotBeCleared");


            NgWebElement toDoList = ngDriver.FindElement(By.Id("todo-list"));
            Assert.AreEqual(2, toDoList.FindElements(By.CssSelector("li")).Count, "to-do's weren't created");

            NgWebElement item = toDoList.FindElement(By.CssSelector("li"));

            WebDriverWait wait = new WebDriverWait(ngDriver, new TimeSpan(0, 0, 15));
            wait.Until(ExpectedConditions.ElementToBeClickable(item.FindElement(NgBy.Model("todo.completed"))));

            Assert.True(!item.GetAttribute("class").Contains("completed"), "item was already set to completed without being clicked");

            item.FindElement(NgBy.Model("todo.completed")).Click();

            Assert.True(item.GetAttribute("class").Contains("completed"), "item is still not to completed after it has been clicked");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("clear-completed")));

            ngDriver.FindElement(By.Id("clear-completed")).Click();

            Assert.AreEqual(1, toDoList.FindElements(By.CssSelector("li")).Count, "to-do wasn't deleted");

        }

        //I can filter the visible To-dos by Completed state
        [Test]
        public void FilterByCompleted()
        {

            AddToDo("ToDoToBeFiltered");
            AddToDo("SecondToDoToNotBeFiltered");

            NgWebElement toDoList = ngDriver.FindElement(By.Id("todo-list"));
            Assert.AreEqual(2, toDoList.FindElements(By.CssSelector("li")).Count, "to-do's weren't created");

            NgWebElement item = toDoList.FindElement(By.CssSelector("li"));

            Assert.True(!item.GetAttribute("class").Contains("completed"), "item was already set to completed without being clicked");


            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(item.FindElement(NgBy.Model("todo.completed"))));

            item.FindElement(NgBy.Model("todo.completed")).Click();

            Assert.True(item.GetAttribute("class").Contains("completed"), "item is still not to completed after it has been clicked");

            NgWebElement filters = ngDriver.FindElement(By.Id("filters"));
            filters.FindElement(By.CssSelector("a[href='#/completed']")).Click();


            //changing the filter reloaded the todo list from scratch causing a stale todo list element
            Assert.AreEqual(1, ngDriver.FindElement(By.Id("todo-list")).FindElements(By.CssSelector("li")).Count, "to-do wasn't filtered");

        }

        //I can re-activate a completed To-do by clicking inside the circle UI
        [Test]
        public void UncompleteToDo()
        {
            AddToDo("ToDoToBeSetBackToACtive");

            NgWebElement toDoList = ngDriver.FindElement(By.Id("todo-list"));
            Assert.AreEqual(1, toDoList.FindElements(By.CssSelector("li")).Count, "to-do wasn't created");
            NgWebElement item = toDoList.FindElement(By.CssSelector("li"));


            WebDriverWait wait = new WebDriverWait(ngDriver, new TimeSpan(0, 0, 15));
            wait.Until(ExpectedConditions.ElementToBeClickable(item.FindElement(NgBy.Model("todo.completed"))));

            item.FindElement(NgBy.Model("todo.completed")).Click();


            //issue was caused in edge attemted to fix with commented out code but basically the clicks whould happen too fast and the seond wouldnt be recognised
            Thread.Sleep(1000);

            Assert.True(item.GetAttribute("class").Contains("completed"),"to-do wasn't set to completed");
            wait.Until(ExpectedConditions.ElementToBeClickable(item.FindElement(NgBy.Model("todo.completed"))));

            item.FindElement(NgBy.Model("todo.completed")).Click();

            //ngDriver.WaitForAngular();

            Assert.True(!item.GetAttribute("class").Contains("completed"), "to-do wasn't set to active");
            

        }

        public IWebDriver driver;
        public NgWebDriver ngDriver;



        [TearDown]
        public void TearDown()
        {
            ngDriver.ExecuteScript("window.localStorage.clear();");
            driver.Close();
            driver.Dispose();
        }

    }
}
