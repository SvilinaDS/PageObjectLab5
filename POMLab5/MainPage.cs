using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace POMLab5
{
    public class MainPage
    {
        private readonly IWebDriver driver;

        public MainPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public IWebElement byPayee => driver.FindElement(By.Id("input1"));
        public IWebElement byAccount => driver.FindElement(By.Id("input2"));
        public IWebElement byType => driver.FindElement(By.Id("input3"));
        public IWebElement byExPayees => driver.FindElement(By.Id("input4"));
        public IWebElement Table => driver.FindElement(By.ClassName("table"));


        public void SelectOptionInAccount(string option)
        {
            var selectInput2 = new SelectElement(byAccount);
            selectInput2.SelectByText(option);
        }

        public void SelectOptionInType(string option)
        {
            var selectInput3 = new SelectElement(byType);
            selectInput3.SelectByText(option);
        }

        public void SendKeysToByPayee(string input)
        {
            byPayee.SendKeys(input);
        }

        public void SendKeysTobyExPayees(string input)
        {
            byExPayees.SendKeys(input);
        }
        public int GetRowCount()
        {
            var rows = Table.FindElements(By.TagName("tr"));
            return rows.Count - 1;
        }
        private int GetColumnIndex(string columnName)
        {
            var headers = Table.FindElements(By.TagName("th"));
            for (int i = 0; i < headers.Count; i++)
            {
                if (headers[i].Text.Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }
            throw new NotFoundException($"Column with name '{columnName}' not found.");
        }
        public string GetCellText(int rowNumber, string columnName)
        {
            var columnIndex = GetColumnIndex(columnName);
            var cell = Table.FindElement(By.XPath($"//tr[{rowNumber + 1}]/td[{columnIndex + 1}]"));
            return cell.Text;
        }

        public bool IsTableDisplayed()
        {
            return Table.Displayed;
        }

    }
}

