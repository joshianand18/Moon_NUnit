using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;

namespace MoonNUnitCore.Chrome
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class SmokeTest
    {
        public TestContext TestContext { get; set; }

        private readonly Dictionary<string, IWebDriver> driverStack = new Dictionary<string, IWebDriver>();

        private IWebDriver InitiateMoonWebDriver(string browserVersion)
        {
            Dictionary<dynamic, dynamic> moonOptions = new Dictionary<dynamic, dynamic>();
            string testName = TestContext.CurrentContext.Test.Name;
            ChromeOptions options = new ChromeOptions { };

            /*Browser Version*/
            options.BrowserVersion = browserVersion;

            /*Enabling VNC*/
            moonOptions.Add("enableVNC", true);

            /*Session Timeout*/
            moonOptions.Add("sessionTimeout", "2m");

            /*Test Name*/
            moonOptions.Add("name", testName);

            /*Enabling Video Recording*/
            moonOptions.Add("enableVideo", true);

            /*Root CA*/

            options.AddAdditionalCapability("moon:options", moonOptions, true);
            IWebDriver driver = new RemoteWebDriver(new Uri("http://10.146.250.134:4444/wd/hub"), options.ToCapabilities(), TimeSpan.FromSeconds(300));
            driverStack.Add(testName, driver);
            Console.WriteLine(((RemoteWebDriver)driver).SessionId);
            return driver;
        }

        [Test]
        [Category("Demo")]
        [TestCase("95.0")]
        [TestCase("94.0")]
        [TestCase("93.0")]
        [TestCase("92.0")]
        [TestCase("91.0")]

        public void Demo_Chrome_smokeTest_DellDotCom(string browserVersion)
        {
            DellDotComTests(InitiateMoonWebDriver(browserVersion));
        }

        public void DellDotComTests(IWebDriver driver)
        {
            driver.Manage().Window.Maximize();
            Console.WriteLine("Step1: **********Navigating to dell.com @ " + DateTime.Now.ToString("HH:mm:ss") + "***********\n");
            driver.Navigate().GoToUrl("https://www.dell.com/en-us");
            driver.Manage().Timeouts().PageLoad.Add(System.TimeSpan.FromSeconds(300));
            driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(300);

            Console.WriteLine("Step2: **********Clicking on Cart @ " + DateTime.Now.ToString("HH:mm:ss") + "***********\n");
            driver.FindElement(By.LinkText("Cart")).Click();
            driver.Manage().Timeouts().PageLoad.Add(System.TimeSpan.FromSeconds(300));
            driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(300);

            Console.WriteLine("Step3: **********Clicking on My Account @ " + DateTime.Now.ToString("HH:mm:ss") + "***********\n");
            driver.FindElement(By.LinkText("My Account")).Click();
            driver.Manage().Timeouts().PageLoad.Add(System.TimeSpan.FromSeconds(300));
            driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(300);
            Console.WriteLine("********* Step3 Assert statement begining: ************\n");
            Assert.IsTrue(driver.FindElement(By.LinkText("Forgot your password?")).Displayed);

        }

        [TearDown]
        public void Dispose()
        {
            IWebDriver driver = driverStack[TestContext.CurrentContext.Test.Name];
            driver?.Quit();
        }

    }
}
