using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
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

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Navigate().GoToUrl("https://www.globalsqa.com/angularJs-protractor/SearchFilter/");
        }

        [Test]
        public void SearchByAccountAndPayeeTest()
        {
            var page = new MainPage(driver);

            Assert.IsTrue(page.IsTableDisplayed(), "Table is not displayed.");

            page.SelectOptionInAccount("Bank Savings");
            page.SendKeysToByPayee("Salary");

            Assert.AreEqual("Bank Savings", page.GetCellText(0, "Account"), "Incorrect Account value after filtering.");
            Assert.AreEqual("Salary", page.GetCellText(0, "Payee"), "Incorrect Payee value after filtering.");
        }

        [Test]
        public void SearchByAccountTest()
        {
            var page = new MainPage(driver);

            page.SelectOptionInAccount("Cash");

            Assert.AreEqual(3, page.GetRowCount(), "Incorrect number of rows after filtering.");

            Assert.AreEqual("Cash", page.GetCellText(0, "Account"), "Incorrect Account value after filtering.");
        }
        [Test]
        public void SearchByTypeTest()
        {
            var page = new MainPage(driver);

            page.SelectOptionInType("INCOME");

            Assert.AreEqual(1, page.GetRowCount(), "Incorrect number of rows after filtering.");

            Assert.AreEqual("INCOME", page.GetCellText(0, "Type"), "Incorrect Type value after filtering.");

        }

        [Test]
        public void SearchByCashAndIncomeTest()
        {
            var page = new MainPage(driver);

            page.SelectOptionInAccount("Cash");
            page.SelectOptionInType("INCOME");

            Assert.AreEqual(0, page.GetRowCount(), "Table should only display column header.");
        }

        [Test]
        public void SearchByCashAndExpenditureTest()
        {
            var page = new MainPage(driver);

            page.SelectOptionInAccount("Cash");
            page.SelectOptionInType("EXPENDITURE");

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

            Assert.AreEqual("HouseRent", page.GetCellText(0, "Payee"), "Incorrect Payee value after filtering.");
        }

        [Test]
        public void SearchByAccountAndExPayeesTest()
        {
            var page = new MainPage(driver);

            page.SelectOptionInAccount("Cash");

            page.byExPayees.SendKeys("HouseRent");

            Assert.AreEqual(1, page.GetRowCount(), "Incorrect number of rows after filtering.");

            Assert.AreEqual("Cash", page.GetCellText(0, "Account"), "Incorrect Account value after filtering.");
            Assert.AreEqual("HouseRent", page.GetCellText(0, "Payee"), "Incorrect Payee value after filtering.");
           
        }
        
        [Test]
        public void SearchByMatchingLetterExPayee()
        {
            var page = new MainPage(driver);

            page.SendKeysTobyExPayees("B");

            Assert.IsTrue(page.GetRowCount() > 0, "No rows found after filtering.");

            Assert.AreEqual("Bank Savings", page.GetCellText(0, "Account"), "Incorrect Account value after filtering.");
            Assert.AreEqual("EXPENDITURE", page.GetCellText(0, "Type"), "Incorrect Type value after filtering.");
            Assert.AreEqual("InternetBill", page.GetCellText(0, "Payee"), "Incorrect Payee value after filtering.");
            Assert.AreEqual("500", page.GetCellText(0, "Amount"), "Incorrect Amount value after filtering.");

            Assert.AreEqual("Cash", page.GetCellText(1, "Account"), "Incorrect Account value after filtering.");
            Assert.AreEqual("EXPENDITURE", page.GetCellText(1, "Type"), "Incorrect Type value after filtering.");
            Assert.AreEqual("InternetBill", page.GetCellText(1, "Payee"), "Incorrect Payee value after filtering.");
            Assert.AreEqual("1200", page.GetCellText(1, "Amount"), "Incorrect Amount value after filtering.");

            Assert.AreEqual("Cash", page.GetCellText(2, "Account"), "Incorrect Account value after filtering.");
            Assert.AreEqual("EXPENDITURE", page.GetCellText(2, "Type"), "Incorrect Type value after filtering.");
            Assert.AreEqual("PowerBill", page.GetCellText(2, "Payee"), "Incorrect Payee value after filtering.");
            Assert.AreEqual("200", page.GetCellText(2, "Amount"), "Incorrect Amount value after filtering.");

        }
        [Test]
        public void SearchWithNonexistentValueTest()
        {
            var page = new MainPage(driver);

            page.byPayee.SendKeys("Sample");
            Assert.AreEqual(0, page.GetRowCount(), "Table should only display column headers with nonexistent value.");

        }
        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }

    }
}