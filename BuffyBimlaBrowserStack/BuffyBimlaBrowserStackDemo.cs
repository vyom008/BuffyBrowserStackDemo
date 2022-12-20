using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Support.UI;
using System.Collections;
using System.Linq;
using System.Net;
using WebDriverManager.DriverConfigs.Impl;

namespace SeleniumLearning
{
    public class BuffyBimlaBrowserStackDemo
    {

        IWebDriver driver;
        //Backup
/*        [SetUp]
        public void Setup()
        {
            new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Url = "https://rahulshettyacademy.com/loginpagePractise/";
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(8));
        }*/

        [SetUp]
        public void Setup()
        {

            string USERNAME = "vyombuffy_UANlyY";
            string AUTOMATE_KEY = "tsA6D7xTEXWJq5U3Uiah";

            OpenQA.Selenium.Chrome.ChromeOptions capability = new OpenQA.Selenium.Chrome.ChromeOptions();

            ChromeOptions capabilities = new ChromeOptions();
            capabilities.BrowserVersion = "latest";
            Dictionary<string, object> browserstackOptions = new Dictionary<string, object>();
            browserstackOptions.Add("userName", USERNAME);
            browserstackOptions.Add("accessKey", AUTOMATE_KEY);
            //Windows
            browserstackOptions.Add("os", "Windows");
            browserstackOptions.Add("osVersion", "11");
            //Apple
/*            browserstackOptions.Add("os", "OS X");
            browserstackOptions.Add("osVersion", "Catalina");*/

            //Common
            browserstackOptions.Add("projectName", "Sample sandbox project");
            browserstackOptions.Add("buildName", "Build #1");
            browserstackOptions.Add("sessionName", "My First Test");
            capabilities.AddAdditionalOption("bstack:options", browserstackOptions);

            //Mobile Phone
            /*            SafariOptions capabilities = new SafariOptions();
                        Dictionary<string, object> browserstackOptions = new Dictionary<string, object>();
                        browserstackOptions.Add("userName", USERNAME);
                        browserstackOptions.Add("accessKey", AUTOMATE_KEY);
                        browserstackOptions.Add("osVersion", "14");
                        browserstackOptions.Add("deviceName", "iPhone 12");
                        browserstackOptions.Add("realMobile", "true");
                        capabilities.AddAdditionalOption("bstack:options", browserstackOptions);*/

            //Common
            driver = new RemoteWebDriver(
            new Uri("https://hub-cloud.browserstack.com/wd/hub/"), capabilities
            );

/*
            new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
            driver = new ChromeDriver();*/
            driver.Manage().Window.Maximize();
            driver.Url = "https://rahulshettyacademy.com/loginpagePractise/";
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(8));
        }


        [TearDown]
        public void Close()
        {
            driver.Close();
            driver.Quit();
        }
        [Test]
        public void E2E()
        {
            String[] expectedProducts = { "iphone X", "Blackberry" };

            String username = getName(driver);
            String password = getPassword(driver);

            driver.FindElement(By.Id("username")).SendKeys(username);
            //driver.FindElement(By.Id("username")).SendKeys("test");
            driver.FindElement(By.Id("password")).SendKeys(password);

            //Static dropdown trick
            IWebElement dropdown = driver.FindElement(By.CssSelector("select[class='form-control']"));
            SelectElement s = new SelectElement(dropdown);
            s.SelectByText("Teacher");

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

            driver.FindElement(By.CssSelector("div[class='form-group']:nth-child(6) label span:nth-child(1)")).Click();
            driver.FindElement(By.CssSelector(".btn")).Click();

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
            return usernameText2;
        }

        public String getPassword(IWebDriver driver)
        {
            driver.Url = "https://rahulshettyacademy.com/loginpagePractise/";
            String passwordText = driver.FindElement(By.XPath("//div[@class='form-group']/p")).Text;
            String[] passwordText1 = passwordText.Split(' ');
            String passwordText2 = passwordText1[6].Split(")")[0].Trim();
            return passwordText2;
        }
    }
}