using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Test.Selenium
{
    public class TestClass
    {
        // Requires at minimum Chrome Beta 104 it appears (https://www.androidpolice.com/chrome-104/).  
        // It will give an error about version number on earlier ones. 
        // In a VS Code ( >= version 1.68.1) Terminal, do a "dotnet clean," followed by a "dotnet build" 
        //  then you should see the options to "Run Test" and "Debug Test" above FirstTest Method below. 
        private const String google = "http://www.google.com";
        private const String relativePath = @"..\..\..\..\Matt\searchTerms.txt";

        // searchTerms will be driven by input from the text file. 
        private List<String> searchTerms = new List<string>();

        [SetUp]
        public void TestSetup()
        {
            var currentDir = Directory.GetCurrentDirectory();
            if (File.Exists(relativePath)) 
            {
                searchTerms = File.ReadAllLines(relativePath).ToList();
            }
        }

        // Options to Debug and/or Run Test appear in VS Code above the FirstTest method below after building this project. 
        [Test]
        public void FirstTest()
        {
            Parallel.ForEach(searchTerms, searchTerm =>
            {
                    // Create a new instance of Chrome for each search to perform all searches asynchronously and in parallel. 
                    ChromeDriver driver = new ChromeDriver();
                    driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
                    driver.Navigate().GoToUrl(google);

                    // The name "q" doesn't seem nearly specific enough, i.e. not an ID - thanks Google - and this is a potential risk. 
                    var elementName = "q";
                    var googleSearchBox = driver?.FindElement(By.Name(elementName)); 
                    googleSearchBox?.SendKeys(searchTerm);
                    googleSearchBox?.SendKeys(Keys.Enter);
                    driver?.Close();
                    driver?.Quit();
            });
        }

        [TearDown]
        public void TestTearDown()
        {
            //driver?.Quit(); // Moved this into the main Parallel.ForEach for earlier cleanup. 
        }
    }
}
