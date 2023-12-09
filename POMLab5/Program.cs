using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

IWebDriver driver = new ChromeDriver();
driver.Url = "https://www.globalsqa.com/angularJs-protractor/SearchFilter/";
Thread.Sleep(5000);
driver.Quit();

