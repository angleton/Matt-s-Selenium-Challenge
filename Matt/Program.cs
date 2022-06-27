using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Matt
{
    public class TestClass
    {
        // Requires at minimum Chrome Beta 104 it appears (https://www.androidpolice.com/chrome-104/).  
        // It will give an error about version number on earlier ones. 
        // In a VS Code ( >= version 1.68.1) Terminal, do a "dotnet clean," followed by a "dotnet build" 
        //  then you should see the options to "Run Test" and "Debug Test" above FirstTest Method below. 
        private const string Google = "http://www.google.com";
        private const string RelativePath = @"..\..\..\..\Matt\searchTerms.txt";

        // searchTerms will be driven by input from the text file. 
        private List<string> _searchTerms = new();

        [SetUp]
        public void TestSetup()
        {
            if (File.Exists(RelativePath)) 
            {
                _searchTerms = File.ReadAllLines(RelativePath).ToList();
            }
        }

        public static string[] SearchTerms()
        {
            return !File.Exists(RelativePath) ? new[] {""} : File.ReadAllLines(RelativePath).ToArray();
        }

        // Options to Debug and/or Run Test appear in VS Code above the FirstTest method below after building this project. 
        [Test, TestCaseSource(nameof(SearchTerms))]
        [Parallelizable(scope: ParallelScope.All)]
        public void FirstTest(string searchTerm)
        {
            // Create a new instance of Chrome for each search to perform all searches asynchronously and in parallel. 
            var driver = new ChromeDriver();
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
            driver.Navigate().GoToUrl(Google);

            // The name "q" doesn't seem nearly specific enough, i.e. not an ID - thanks Google - and this is a potential risk. 
            const string ElementName = "q";
            var googleSearchBox = driver?.FindElement(By.Name(ElementName)); 
            googleSearchBox?.SendKeys(searchTerm);
            googleSearchBox?.SendKeys(Keys.Enter);
            driver?.Close();
            driver?.Quit();
        }

        [TearDown]
        public void TestTearDown()
        {
            //driver?.Quit(); // Moved this into the main Parallel.ForEach for earlier cleanup. 
        }
    }
}
