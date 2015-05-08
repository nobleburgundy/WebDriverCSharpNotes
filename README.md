#WEBDRIVER/C# TEST AUTOMATION NOTES
####Ongoing notes on using WebDriver with C# for automation

##INSTALLATION

**Firefox** just works - no need to install anything. **Chrome** requires the installation of the chromedriver.exe (not exact name) into location specified in PATH var. **IE** same thing as chrome with IEdriver.exe file. Use only the 32-bit IE driver.


1. Right-click on the solution with your test and choose **Manage NuGet Packages**.
2. Type "WebDriver" in the search bar.
3. Install the WebDriver and WebDriver Support Classes packages.
4. Once the packages are installed, add the following **Using** statements at the beginning of your test file.
```c#
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
```

##AUTOMATION CODE EXAMPLES
#####CREATING THE DRIVER OBJECT & OPEN TO URL
```c#
string vetURL = "http://ntxxxx:xxxx/";
IWebDriver driver = new FirefoxDriver();
driver.Url = vetURL;
```
	
#####SETTING IMPLICIT WAIT (timeout to wait for elements to appear)
```c#
driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(20));
```

#####DELAY/SLEEP
```c#
using System.Threading;//required

Thread.Sleep(5000);
```

#####LOGIN EXAMPLE
```c#
string userName = "xxxxxx@mailinator.com"; string password = "xxxxxx1*";
driver.FindElement(By.Id("UserName")).SendKeys(userName);
driver.FindElement(By.Id("Password")).SendKeys(password);
driver.FindElement(By.Id("loginSubmit")).Click();
```
	
#####USING ASSERT TO REPORT FAILURE
```c#
if (driver.FindElement(By.Id("addCartItemItemNum")).Displayed != true)
{
	Assert.Fail("Unable to verify the Quick Order Portlet exists.");
}
```
	
#####GETTING RADIO BUTTON VALUE
```c#
string radioVal = driver.FindElement(By.ClassName("non")).GetAttribute("value");
if (radioVal != "GR")
{
	Assert.Fail("The radio value not set to 'Ground' as expected. Radio val = " + radioVal);
}
```
	
#####GET TEXT FIELD VALUE
```c#
string specialInstructionsValue = driver.FindElement(By.Id("ShippingNote")).Text;
if (specialInstructionsFieldValue.Length > 0)
{
	Assert.Fail("Special Instructions field populated by default.");
	driver.FindElement(By.Id("ShippingNote")).Clear();
}
```
	
#####CYCLE THROUGH A NUMBER OF ELEMENTS & LOOK FOR TEXT
```c#
IReadOnlyCollection<IWebElement> textAreas = driver.FindElements(By.ClassName("contentAreaText"));
bool noneFound = false;
foreach(IWebElement textArea in textAreas)
{
	string text = textArea.Text;
	if (text.IndexOf("None") > -1)
	{
		noneFound = true;
		break;
	}
}
if (noneFound != true)
{
	Assert.Fail("Unable to verify the Additional Charges is set to 'None'.");
}
```

Use **IList** to cycle through elements using the index...
```c#
IList<IWebElement> reviewLinks = driver.FindElements(By.LinkText("Review"));
for (i = 1; i < reviewLinks.Count; i++)
{
    if (reviewLinks[i].Text.Lenght == 9)
    {
    	orderFound = true;
    }
}
```

#####ITERATE THROUGH A TABLE
```c#
IWebElement table = driver.FindElement(By.ID("orderTable"));
ReadOnlyCollection<IWebElement> allRows = table.FindElements(By.TagName("tr"));

foreach (IWebElement row in allRows)
{
    ReadOnlyCollection<IWebElement> cells = row.FindElements(By.TagName("td"));
    
    foreach (IWebElement cell in cells)
    {
        Console.WriteLIne("cell text = " + cell.Text);
    }
}
```

#####FIND AN ELEMENT BY ATTRIBUTE & ATTRIBUTE'S VALUE
```c#
// use By.CSSSelector
// There are numerous ways to grab 1 or more elements via this method
// http://www.w3schools.com/cssref/css_selectors.asp
IWebElement e = driver.FindElement(By.CssSelector("[alt=linkedIn]")).Click();
```

#####SELECT OPTION IN DROP DOWN LIST
```c#
IWebElement CCDropDown = driver.FindElement(By.Id("CreditCardModel_CardType"));
SelectElement select = new SelectElement(CCDropDown);
select.SelectByText("Visa");
```

#####RIGHT CLICK ON ELEMENT
```c#
using OpenQA.Selenium.Interactions;


IWebDriver driver = new InternetExplorerDriver(internetExplorerDriverServerDirectory: "C:\\");
driver.Navigate().GoToUrl("http://google.com");
IWebElement gsearch = driver.FindElement(By.Id("gbqfq"));
Actions builder = new Actions(driver);
OpenQA.Selenium.Interactions.IAction rightClick = builder.ContextClick(gsearch).Build();
rightClick.Perform();
```

#####XPATH
Using XPATH is a powerful and easy way to find elements...
```c#
HTML button without ID or text
<button class="big class blue">
	<span>Ok</span>
</button>
//You can easily find this element by...
driver.FindElement(By.XPath("//span[text()='Ok']")).Click();
```
Action											| XPath
------------- 										| -------------
Find all elements with tag input							| //input
Find all input tag element having attribute type = ‘hidden’				| //input[@type='hidden']
Find all input tag element having attribute type = ‘hidden’ and name attribute = ‘ren’	| //input[@type='hidden'][@name='ren']
Find all input tag element with attribute type containing ‘hid’				| //input[contains(@type,'hid')]
Find all input tag element with attribute type starting with ‘hid’			| //input[starts-with(@type,'hid')]
Find all elements having innertext = ‘password’						| //*[text()='Password']
Find all td elements having innertext = ‘password’					| //td[text()='Password']
Find all next siblings of td tag having innertext = ‘gender’				| //td[text()='Gender']//following-sibling::*
Find all elements in the 2nd next sibling of td tag having innertext = ‘gender’		| //td[text()='Gender']//following-sibling::*[2]//*	
Find input elements in the 2nd next sibling of td tag having innertext = ‘gender’	| //td[text()='Gender']//following-sibling::*[2]//input
Find the td which contains font element containing the text ‘12’			| //td[font[contains(text(),'12')]]

#####WRITING INFO TO LOG
```c#
Using System.Diagnostics;

Debug.WriteLine("Hello, World!");
```


#####TRACING FAILURES
On the stack trace of a run(in the bottom sub-panel of the Test Explorer panel) you will see numerous trace statements. You only want to pay attention to the bottom-most trace line. Hovering over it with your mouse will show you the line the error occured on(like when it can't find the element). Clicking on the bottom-most line will take you to the line where the test failed. You do not need to run in debug mode for this. 


```c#
Using System.Diagnostics;
Assert.Inconclusive("I am an inconclusive message");
```
Note that this will end the test after the statement. Warning icon will show instead of pass or fail. 

#####WORKING WITH EXCEL
The following example grabs all the rows from all the sheets of a workbook and puts the data into a dictionary.
```c#
using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Excel = Microsoft.Office.Interop.Excel;

namespace SmokeTests.New_Functionality
{
    [TestClass]
    public class Create_301_Redirects_from_Spreadsheet_from_BU
    {
        IWebDriver driver = new InternetExplorerDriver();

        [TestMethod]
        public void Create301RedirectsFromSpreadsheet()
        {
            Debug.WriteLine("Starting 'Create301Redirects...' method");

            string excelFileName = "Copy of PBI-3212-PDCOM_KWredirects-5-Dec-2014-Cason.xlsx";
            string excelFilePath = "\\\\nt124072\\SharedServices\\Testing\\Automation\\" + excelFileName;

            if (File.Exists(excelFilePath)){
                Debug.WriteLine("The file EXISTS at " + excelFilePath);
            }
            else
            {
                Assert.Fail("The file does not exist at path '" + excelFilePath + "'. Terminating.");
            }

            Excel.Application xlApp;
            Excel.Workbook xlWorkbook;
            Excel.Worksheet xlWorksheet;
            Excel.Range range;

            int rowCount = 0;
            int colCount = 0;
            string str;
            StringBuilder rowString = new StringBuilder();

            xlApp = new Excel.Application();
            xlWorkbook = xlApp.Workbooks.Open(excelFilePath);
            xlWorksheet = xlWorkbook.Worksheets.get_Item(1);

            //create dictionary with all values from all sheets
            Dictionary<string, string> allRowsDictionary = new Dictionary<string, string>();

            for (int i = 1; i < xlWorkbook.Worksheets.Count; i++ )
            {
                xlWorksheet = xlWorkbook.Worksheets.get_Item(i);
                range = xlWorksheet.UsedRange;

                //key = cell name (ie row name col name)
                for (rowCount = 1; rowCount <= range.Rows.Count; rowCount++)
                {
                    for (colCount = 1; colCount <= range.Columns.Count; colCount++)
                    {
                        str = (string)(range.Cells[rowCount, colCount] as Excel.Range).Value2;
                        rowString.Append("#" + str);
                    }
                    allRowsDictionary.Add("sheet " + i + ":row " + rowCount, rowString.ToString());
                    rowString.Clear();
                }
            }

            xlWorkbook.Close(true, null, null);
            xlApp.Quit();

            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorksheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkbook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);

            string row = "";
            string searchTerm = "";
            string url = "http://nt124181.pdental.com:5108/";
            //string rowValues[];

            foreach (KeyValuePair<string, string> entry in allRowsDictionary)
            {
                row = entry.Value;
                //don't include the header row
                if (row.IndexOf("Keyword Redirect") == -1)
                {
                    string[] rowValues = row.Split('#');
                    searchTerm = rowValues[2];
                    //only perform the search if there is > 0 search terms
                    if (searchTerm.Length > 1)
                    {
                        //see if there are multiple search terms
                        string[] searchTerms = searchTerm.Split(';');
                        string expectedURL = rowValues[rowValues.GetUpperBound(0)];
                        //to avoid enviroment changes, just verify the last 10 characters are in the actual URL
                        string expectedURLSubString = expectedURL.Substring(expectedURL.Length - 10);

                        //| Step 1 | Navigate to the Patterson Dental homepage.QA:  http://nt124181.pdental.com:5108/       | User is navigated to the Patterson Dental homepage. 
                        driver.Url = url;

                        foreach (string s in searchTerms)
                        {
                            //| Step 2 | Select a search term from column B.                                                    | Selected.                                           
                            //handled in next step

                            //| Step 3| Enter the value into the search bar near the top right, and click Search.              | User is navigated to the search results page.       
                            driver.FindElement(By.Name("q")).SendKeys(s);
                            driver.FindElement(By.Id("searchSubmit")).Click();
                            Thread.Sleep(1000);

                            //| Step 4 | Verify that the URL matches the one in column C corresponding to the search term in B. | URL matches.                                        
                            string pageURL = driver.Url;
                            if (pageURL.IndexOf(expectedURLSubString) > -1)
                            {
                                Debug.WriteLine("STEP PASS - pageURLSubString == expectedURLSubString");
                            }
                            else
                            {
                                Debug.WriteLine("FAIL - unable to verify the expected url string '" + expectedURLSubString + "' NOT found in the pageURL(" + pageURL + ").");
                            }
                        }
                    }
                }
            }
            //TEST END
            driver.Close();
        }     
    }
}
```
###PAGE OBJECT MODEL
We will be using the Page Object Model (POM) to organize the automation code and tests.  The objective is to separate the details of the site implementation from the actual test. 

#####Important points

+ The public methods represent the services that the page offers
+ Try not to expose the internals of the page
+ Tests, not the PageObjects, should be responsible for making assertions about the state of a page
+ Return "this" if no navigation takes page, return the new page object if navigation to other page.
```c#
public HomePage setPasswordField(string password)
{
	driver.FindElement(PasswordField).SendKeys(password);
	return this;
}
```

+ If navigating to a new page, return a new object of the navigated to page...
```c#
public HomePage login(string userName, string password)
{
	setUserNameField(userName);
	setPasswordField(password);
	clickLoginButton();
	return new HomePage(driver);
}
```

###PROJECT STRUCTURE

+ Solution - Dental, Veterinary, PMI, Medical, etc...
	+ Project - Smoke Tests, Regression
		+ Test Class File - on class file per folder???
			+ Test Methods
				+ Test1() - should match Test Case name
				+ Test2()
				
	
	
###CREATING LIBRARIES

You will want to create function libraries that share the "driver" object(which is just IWebDriver) so all WebDriver commands go to the same driver. This can be done by setting your library's "driver" to your test class' [TestInitialize] section, which will always run. So here's a quick template:


```c#
    [TestClass]
    public class Regression_Ordering
    {
        IWebDriver driver = new InternetExplorerDriver(internetExplorerDriverServerDirectory: "C:\\");   // prototype

	[TestInitialize]
	public void MyTestInit()
	{
	    Debug.WriteLine(" ** [TestInitialize] MyTestInit Invoked ** ");
	    Utils.Gen2.driver = driver;  // send this to general lib
	}
```        

Now here's what the libary class(Gen2) looks like:

```c#
    public static class Gen2
    {
        public static IWebDriver driver = null; // set in your test class
        
        public static void QuickMethodTest()
        {
            Debug.WriteLine("this is quickmethodtest");
            driver.FindElement(By.Id("myelement")).Click();
        }        

```  

So using a library function int your test would look like this:


```c#
        [TestMethod]
        public void TestPrototype()
        {
            Utils.Gen2.QuickMethodTest();
        }

```  	


## USING SHARED PARAMETERS

First, you need to incorporate TestContext which is easy. This is done at the class declaration at each .cs file. Just put it after the public IWebDriver declaration
```c#
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }
```

Then we do the header, and the rest of the code. For "2581", replace it with a test case that uses a shared parameter table, otherwise it says it's empty. If there's 20 rows in the shared parameter table, 20 tests will be run(one for each data row)

```c#
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.TestCase", "http://our.server/tfs;CoreProject", "2581", DataAccessMethod.Sequential)]  // works!

        [TestMethod]
        public void ConnectTFS()
        {
            TestContext tc = TestContext;
            string column1 = TestContext.DataRow[0].ToString(); // read parameter by column index
            //string column2 = TestContext.DataRow["MyVal"].ToString(); //read parameter by column name

            Debug.WriteLine(column1.ToString());
        }

```


##SYNTAX, RULES, AND BEST PRACTICES

#####Variables that reference fields should match the label of the field on the screen.
```c#
IWebElement SignInButton = driver.FindElement(By.Id("SignInButton"));

NOT

IWebElement SIbtn = driver.FindElement(By.Id("SignInButton"));//or similar
```
#####Be verbose with variable names.
For example, don't shorten "admin" to "adm".
#####Do not abbreviate fields or parameters. Spell them out to remove ambiguity.
#####Use inner parenthesis in expressions.
Most of the time it doesn’t make any difference at all to the compiled code. It is however easier to read when a piece of code has something like:
```c#
if((x==Y) && ((y != 10)||(y < 299)) && (DateTime.Now.Year > 2013))
{
	...
}
```
#####Never check in commented out code.
With version control, there isn't any need to keep in old code that doesn't work for reference.

#####Naming Conventions - TODO: this is still in progress and not actually decided on yet.
+ Variables - lowerCamelCase. Example: itemNumber
+ Fields - UpperCamelCase. Example: SpecialInstructionsField
+ Objects - UpperCamelCase. Example: ItemTable
+ Method Names - UpperCamelCase. Example: GetTableValues()
+ Class Names - UpperCamelCase. Example: Test
+ Test Method Names - UpperCamelCase and should match actual test name. Example: ???
+ Test Class Name - UpperCamelCase and should match the test folder.

#####If you're doing a LOT of string concantenating, use StringBuilder instead of string concatenation...
```c#
//StringBuilder is part of System.Text
using System.Text;

StringBuilder concatenatedString = new StringBuilder();
foreach (string rowName in giantList)
{
	allText = allText.Append(rowName);
}
allText.Replace(" ", ",");
```

#####Use .Contains() instead of .IndexOf when determining if text exists in a certain context. .IndexOf should be used when getting the index of text within a text, but not to just determine if it exists.
```c#
if (divText.Contains(expectedText)
{
	test.recordStep("Step Passed");
}
else
{
	test.failTest("Expected text '" + expectedText + "' does not exist.");
}
//IndexOf example
int textPosition = divText.IndexOf("ending in");
```

##HELPFUL SQL QUERIES
###VET
Get active customer with balance
```sql
SELECT TOP(1) U.UserName FROM custrep.dbo.CUSTOMER C
INNER JOIN custrep.dbo.APPLICATION_BRANCH A
ON C.branch_number = A.branch_number
INNER JOIN custrep.dbo.ADDRESS AD
ON C.customer_id = AD.customer_id
INNER JOIN pvweb.dbo.Profile P
ON C.persistent_customer_number = P.CustomerNumber
INNER JOIN pvweb.dbo.[User] U
ON P.UserId = U.UserId
WHERE C.amount_past_due > 1
AND C.customer_status_code = 'A'
```
Get postal code of specific customer and use of variables
```sql
DECLARE @CustomerNumber nvarchar(9);
SET @CustomerNumber = '100146075';
SELECT DISTINCT TOP(1) AD.postal_code FROM custrep.dbo.CUSTOMER C
INNER JOIN custrep.dbo.APPLICATION_BRANCH A
ON C.branch_number = A.branch_number
INNER JOIN custrep.dbo.ADDRESS AD
ON C.customer_id = AD.customer_id
INNER JOIN pvweb.dbo.Profile P
ON C.persistent_customer_number = P.CustomerNumber
INNER JOIN pvweb.dbo.[User] U
ON P.UserId = U.UserId
WHERE P.CustomerNumber = @CustomerNumber
```
