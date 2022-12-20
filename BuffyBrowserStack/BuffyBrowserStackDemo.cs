using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections;
using System.Linq;
using System.Net;
using WebDriverManager.DriverConfigs.Impl;

namespace SeleniumLearning
{
    public class BuffyBrowserStackDemo
    {

        IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Url = "https://rahulshettyacademy.com/loginpagePractise/";
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(8));
        }


        [Test]
        public void E2E()
        {
            String[] expectedProducts = { "iphone X", "Blackberry" };

            String username = getName(driver);
            String password = getPassword(driver);
            //LoginAndAddtoCart lc = new LoginAndAddtoCart();


            //driver.Url = "https://rahulshettyacademy.com/#/index";
            //TestContext.Progress.WriteLine(driver.Title);
            //TestContext.Progress.WriteLine(driver.Url);
            //driver.Navigate().GoToUrl("https://rahulshettyacademy.com/loginpagePractise/");

            driver.FindElement(By.Id("username")).SendKeys(username);
            //driver.FindElement(By.Id("username")).SendKeys("test");
            driver.FindElement(By.Id("password")).SendKeys(password);

            //Static dropdown trick
            IWebElement dropdown = driver.FindElement(By.CssSelector("select[class='form-control']"));
            SelectElement s = new SelectElement(dropdown);
            s.SelectByText("Teacher");

            //Radio button trick 1 - If any new radio button gets added than this may not work
            /*            IList<IWebElement> rdos = driver.FindElements(By.CssSelector("input[type='radio']"));
                        rdos[1].Click();*/

            //Radio button trick 2
            IList<IWebElement> rdos = driver.FindElements(By.CssSelector("input[type='radio']"));

            foreach (IWebElement rd in rdos)
            {
                if (rd.GetAttribute("value").Equals("user"))
                {
                    rd.Click();
                    break;
                }
            }
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(8));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("okayBtn")));
            driver.FindElement(By.Id("okayBtn")).Click();

            Boolean result = driver.FindElement(By.Id("usertype")).Selected;

            //Assert.That(result, Is.True);

            driver.FindElement(By.CssSelector("div[class='form-group']:nth-child(6) label span:nth-child(1)")).Click();
            driver.FindElement(By.CssSelector(".btn")).Click();

            //wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TextToBePresentInElementValue(driver.FindElement(By.CssSelector(".btn")), "Sign In"));

            //String errorMessage = driver.FindElement(By.CssSelector(".alert-danger")).Text;
            //TestContext.Progress.WriteLine(errorMessage);

            IWebElement link = driver.FindElement(By.LinkText("Free Access to InterviewQues/ResumeAssistance/Material"));
            //link.SendKeys(Keys.Tab+username);
            String hrefAttribute = link.GetAttribute("href");
            String expectedURL = "https://rahulshettyacademy.com/documents-request";

            Assert.AreEqual(expectedURL, hrefAttribute);

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.PartialLinkText("Checkout ")));

            //Select Products
            IList<IWebElement> products = driver.FindElements(By.TagName("app-card"));

            foreach (IWebElement product in products)
            {
                product.FindElement(By.CssSelector(".card-title a"));
                if (expectedProducts.Contains(product.FindElement(By.CssSelector(".card-title a")).Text))
                {
                    product.FindElement(By.CssSelector(".card-footer button")).Click();
                }
            }

            //Checkout button
            driver.FindElement(By.PartialLinkText("Checkout ")).Click();

        }

        public String getName(IWebDriver driver)
        {
            driver.Url = "https://rahulshettyacademy.com/loginpagePractise/";
            String usernameText = driver.FindElement(By.XPath("//div[@class='form-group']/p")).Text;
            String[] usernameText1 = usernameText.Split(' ');
            String usernameText2 = usernameText1[2].Trim();
            //TestContext.Progress.WriteLine(usernameText2);
            return usernameText2;
        }

        public String getPassword(IWebDriver driver)
        {
            driver.Url = "https://rahulshettyacademy.com/loginpagePractise/";
            String passwordText = driver.FindElement(By.XPath("//div[@class='form-group']/p")).Text;
            String[] passwordText1 = passwordText.Split(' ');
            String passwordText2 = passwordText1[6].Split(")")[0].Trim();
            //TestContext.Progress.WriteLine(passwordText2);
            return passwordText2;
        }
    }
}