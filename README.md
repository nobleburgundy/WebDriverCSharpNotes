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
string vetURL = "http://nt124181:5503/";
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
string userName = "pos@mailinator.com"; string password = "Pattnt1*";
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

####SELECT OPTION IN DROP DOWN LIST
```c#
IWebElement e = driver.FindElement(By.Id("CreditCardModel_CardType"));
SelectElement select = new SelectElement(e);
select.SelectByText("Visa");
```

####WRITING INFO TO LOG
```c#
Using System.Diagnostics;

Debug.WriteLine("Hello, World!");
```
