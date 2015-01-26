# WebDriver/CSharpNotes
Ongoing notes on using WebDriver with C# for automation

Notes
-----

###INSTALLATION

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

###EXAMPLES
####CREATING THE DRIVER OBJECT & OPEN TO URL
```c#
string vetURL = "http://ntxxxx:xxxx/";
IWebDriver driver = new FirefoxDriver();
driver.Url = vetURL;
```
	
####SETTING IMPLICIT WAIT (timeout to wait for elements to appear)
```c#
driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(20));
```

####DELAY/SLEEP
```c#
using System.Threading;//required

Thread.Sleep(5000);
```

####LOGIN EXAMPLE
```c#
string userName = "xxxxxx@mailinator.com"; string password = "xxxxxx1*";
driver.FindElement(By.Id("UserName")).SendKeys(userName);
driver.FindElement(By.Id("Password")).SendKeys(password);
driver.FindElement(By.Id("loginSubmit")).Click();
```
	
####USING ASSERT TO REPORT FAILURE
```c#
if (driver.FindElement(By.Id("addCartItemItemNum")).Displayed != true)
{
	Assert.Fail("Unable to verify the Quick Order Portlet exists.");
}
```
	
####GETTING RADIO BUTTON VALUE
```c#
string radioVal = driver.FindElement(By.ClassName("non")).GetAttribute("value");
if (radioVal != "GR")
{
	Assert.Fail("The radio value not set to 'Ground' as expected. Radio val = " + radioVal);
}
```
	
####GET TEXT FIELD VALUE
```c#
string specialInstructionsValue = driver.FindElement(By.Id("ShippingNote")).Text;
if (specialInstructionsFieldValue.Length > 0)
{
	Assert.Fail("Special Instructions field populated by default.");
	driver.FindElement(By.Id("ShippingNote")).Clear();
}
```
	
####CYCLE THROUGH A NUMBER OF ELEMENTS & LOOK FOR TEXT
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

####ITERATE THROUGH A TABLE
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

####FIND AN ELEMENT BY ATTRIBUTE & ATTRIBUTE'S VALUE
```c#
// use By.CSSSelector
// There are numerous ways to grab 1 or more elements via this method
// http://www.w3schools.com/cssref/css_selectors.asp
IWebElement e = driver.FindElement(By.CssSelector("[alt=linkedIn]")).Click();
```

####SELECT OPTION IN DROP DOWN LIST
```c#
IWebElement CCDropDown = driver.FindElement(By.Id("CreditCardModel_CardType"));
SelectElement select = new SelectElement(CCDropDown);
select.SelectByText("Visa");
```

####WRITING INFO TO LOG
```c#
Using System.Diagnostics;

Debug.WriteLine("Hello, World!");
```

```c#
Using System.Diagnostics;
Assert.Inconclusive("I am an inconclusive message");
```
Note that this will end the test after the statement. Warning icon will show instead of pass or fail. 

####WORKING WITH EXCEL
The following example grabs all the rows from all the sheets of a workbook and puts the data into a dictionary.
```c#
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
}
```



#SYNTAX, RULES, AND BEST PRACTICES

####Variables that reference fields should match the label of the field on the screen.
```c#
IWebElement SignInButton = driver.FindElement(By.Id("SignInButton"));

NOT

IWebElement SIbtn = driver.FindElement(By.Id("SignInButton"));//or similar
```
####Be verbose with variable names.
For example, don't shorten "admin" to "adm".
####Do not abbreviate fields or parameters. Spell them out to remove ambiguity.
####Use inner parenthesis in expressions.
Most of the time it doesn’t make any difference at all to the compiled code. It is however easier to read when a piece of code has something like:
```c#
if((x==Y) && ((y != 10)||(y < 299)) && (DateTime.Now.Year > 2013))
{
	...
}
```
####Never check in commented out code.
With version control, there isn't any need to keep in old code that doesn't work for reference.
####Naming Conventions - TODO: this is still in progress and not actually decided on yet.
+ Variables - lowerCamelCase. Example: itemNumber
+ Fields - UpperCamelCase. Example: SpecialInstructionsField
+ Objects - UpperCamelCase. Example: ItemTable
+ Method Names - UpperCamelCase. Example: GetTableValues()
+ Class Names - UpperCamelCase. Example: Test
+ Test Method Names - UpperCamelCase and should match actual test name. Example: SmokeTest_OrderWithCreditCard()
+ Test Class Name - UpperCamelCase and should match the test folder. SmokeTests
