using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using POMLab5;
using SeleniumExtras.WaitHelpers;
using System;

namespace POMTests
{
    public class Tests
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.globalsqa.com/angularJs-protractor/SearchFilter/");
        }

        [Test]
        public void SearchByAccountAndPayeeTest()
        {
            var page = new MainPage(driver);

            page.SelectOptionInAccount("Bank Savings");
            page.SendKeysToByPayee("Salary");

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            wait.Until(driver => page.GetRowCount() > 0);

            Assert.IsTrue(page.GetRowCount() > 0, "No rows found after filtering.");

            Assert.AreEqual("Bank Savings", page.GetCellText(0, "Account"), "Incorrect Account value after filtering.");
            Assert.AreEqual("Salary", page.GetCellText(0, "Payee"), "Incorrect Payee value after filtering.");
        }

        [Test]
        public void SearchByAccountTest()
        {
            var page = new MainPage(driver);

            page.SelectOptionInAccount("Cash");

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(driver => page.GetRowCount() > 0);

            Assert.AreEqual(3, page.GetRowCount(), "Incorrect number of rows after filtering.");

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//tr[contains(@class, 'ng-scope')]/td[1]")));

            Assert.AreEqual("Cash", page.GetCellText(0, "Account"), "Incorrect Account value after filtering.");
        }
        [Test]
        public void SearchByTypeTest()
        {
            var page = new MainPage(driver);

            page.SelectOptionInType("INCOME");

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(driver => page.GetRowCount() > 0);

            Assert.AreEqual(1, page.GetRowCount(), "Incorrect number of rows after filtering.");

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//tr[contains(@class, 'ng-scope')]/td[1]")));

            Assert.AreEqual("INCOME", page.GetCellText(0, "Type"), "Incorrect Type value after filtering.");

        }

        [Test]
        public void SearchByCashAndIncomeTest()
        {
            var page = new MainPage(driver);

            page.SelectOptionInAccount("Cash");
            page.SelectOptionInType("INCOME");

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("table.table")));

            Assert.AreEqual(0, page.GetRowCount(), "Table should only display column header.");
        }

        [Test]
        public void SearchByCashAndExpenditureTest()
        {
            var page = new MainPage(driver);

            page.SelectOptionInAccount("Cash");
            page.SelectOptionInType("EXPENDITURE");

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            wait.Until(driver => page.GetRowCount() > 0);

            Assert.IsTrue(page.GetRowCount() > 0, "No rows found after filtering.");

            for (int i = 0; i < page.GetRowCount(); i++)
            {
                Assert.AreEqual("Cash", page.GetCellText(i, "Account"), $"Incorrect Account value in row {i + 1} after filtering.");
                Assert.AreEqual("EXPENDITURE", page.GetCellText(i, "Type"), $"Incorrect Type value in row {i + 1} after filtering.");
            }
        }
        [Test]
        public void SearchByPayeeTest()
        {
            var page = new MainPage(driver);

            page.byPayee.SendKeys("HouseRent");

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(driver => page.GetRowCount() > 0);

            Assert.AreEqual(1, page.GetRowCount(), "Incorrect number of rows after filtering.");

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//tr[contains(@class, 'ng-scope')]/td[1]")));

            Assert.AreEqual("HouseRent", page.GetCellText(0, "Payee"), "Incorrect Payee value after filtering.");
        }

        [Test]
        public void SearchByAccountAndExPayeesTest()
        {
            var page = new MainPage(driver);

            page.SelectOptionInAccount("Cash");

            page.byExPayees.SendKeys("HouseRent");

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(driver => page.GetRowCount() > 0);

            Assert.AreEqual(1, page.GetRowCount(), "Incorrect number of rows after filtering.");

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//tr[contains(@class, 'ng-scope')]/td[1]")));

            Assert.AreEqual("Cash", page.GetCellText(0, "Account"), "Incorrect Account value after filtering.");
            Assert.AreEqual("HouseRent", page.GetCellText(0, "Payee"), "Incorrect Payee value after filtering.");
           
        }
        
        [Test]
        public void SearchWithNonexistentValueTest()
        {
            var page = new MainPage(driver);

            page.byPayee.SendKeys("Sample");

            if (page.GetRowCount() > 0)
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//tr[contains(@class, 'ng-scope')]/td[1]")));

                Assert.AreEqual(1, page.GetRowCount(), "Table should only display column headers with nonexistent value.");
            }
        }
        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }

    }
}