using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using System;

namespace ContosoUniversity.CodedUITest
{
    [TestClass]
    public class NavigationMenuTest
    {
        private string baseURL = "https://contosouniversityexample.azurewebsites.net/";
        private WebDriver driver;
        private string browser = string.Empty;

        public TestContext TestContext { get; set; }

        [TestMethod]
        [TestCategory("CodedUI")]
        [Priority(1)]
        public void MenuNavigate()
        {
            try
            {
                driver = new ChromeDriver(Environment.GetEnvironmentVariable("ChromeWebDriver"));

                driver.Manage().Window.Maximize();
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                driver.Navigate().GoToUrl(this.baseURL);

                //disable cookie alert
                driver.FindElement(By.Id("btn-cookie")).Click();

                driver.FindElement(By.Id("link-home")).Click();
                string resHome = driver.FindElement(By.Id("text-welcome")).Text;
                Assert.AreEqual("Welcome to Contoso University", resHome);

                driver.FindElement(By.Id("link-about")).Click();
                string resAbout = driver.FindElement(By.Id("title")).Text;
                Assert.AreEqual("About", resAbout);

                driver.FindElement(By.Id("link-departments")).Click();
                string resDep = driver.FindElement(By.Id("title")).Text;
                Assert.AreEqual("Departments", resDep);

                driver.FindElement(By.Id("link-courses")).Click();
                string resCou = driver.FindElement(By.Id("title")).Text;
                Assert.AreEqual("Courses", resCou);

                driver.FindElement(By.Id("link-instructors")).Click();
                string resIns = driver.FindElement(By.Id("title")).Text;
                Assert.AreEqual("Instructors", resIns);

                driver.FindElement(By.Id("link-students")).Click();
                string resStu = driver.FindElement(By.Id("title")).Text;
                Assert.AreEqual("Students", resStu);

                driver.FindElement(By.Id("link-contact")).Click();
                string resCon = driver.FindElement(By.Id("title")).Text;
                Assert.AreEqual("Contact", resCon);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
            finally
            {
                driver.Quit();
                driver.Dispose();
            }
        }


        [TestInitialize()]
        public void MyTestInitialize()
        {
            //if (this.TestContext.Properties["Url"] != null) //Set URL from a build
            //{
            //    this.baseURL = this.TestContext.Properties["Url"].ToString();
            //}   
        }

        [TestCleanup()]
        public void MyTestCleanup()
        {
            try
            {
                driver.Quit();
                driver.Dispose();
            }
            catch (Exception)
            {
            }
        }

    }
}