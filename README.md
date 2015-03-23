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
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Firefox;

namespace Dental.CreateAndValidateUsers
{
    public class Test
    {
        public string testEnvironment = "UAT";
        public string consumerURL;
        public string adminURL;
        public int timeout = 5000;
    }

    [TestClass]
    public class UnitTest1 : Test
    {
        public TestContext TestContext { get; set; }
        public DataRow DataRow { get; set; }

        //http://tfs-training:8080/tfs/Patterson/WebDelivery/Veterinary
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.TestCase",
                    "http://tfs-training:8080/tfs/Patterson;WebDelivery",
                    "3024",
                    DataAccessMethod.Sequential)]
        public void ConnectTFS()
        {
            //not hooked up yet - data is in dictionary below until this can be figured out
            throw new NotImplementedException();
            string column1 = TestContext.DataRow[0].ToString(); // read parameter by column index
            string column2 = TestContext.DataRow["Column2"].ToString();
        }

        public static void Delay(int timeMilliseconds)
        {
            System.Threading.Thread.Sleep(timeMilliseconds);
        }

        [TestMethod]
        public void CreateTargetWebAccountsWithAccountNumbers()
        {
            Test test = new Test();
            IWebDriver driver = new FirefoxDriver();
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(30));

            Dictionary<string, string> userNameAccountNumberDictionary = new Dictionary<string, string>();
            userNameAccountNumberDictionary.Add("Legacy652user1", "652103128");
            userNameAccountNumberDictionary.Add("Legacy652user2", "652104320");
            userNameAccountNumberDictionary.Add("Legacy652user3", "652104335");
            userNameAccountNumberDictionary.Add("Legacy642user1", "642100205");
            userNameAccountNumberDictionary.Add("Legacy642user2", "642100207");
            userNameAccountNumberDictionary.Add("Legacy642user3", "642100564");
            userNameAccountNumberDictionary.Add("Legacy620user1", "620100231");
            userNameAccountNumberDictionary.Add("Legacy620user2", "620101957");
            userNameAccountNumberDictionary.Add("Legacy620user3", "620102072");
            userNameAccountNumberDictionary.Add("Legacy448user1", "448100073");
            userNameAccountNumberDictionary.Add("Legacy448user2", "448100758");
            userNameAccountNumberDictionary.Add("Legacy448user3", "448101554");
            userNameAccountNumberDictionary.Add("Legacy450user1", "450101075");
            userNameAccountNumberDictionary.Add("Legacy450user2", "450109862");
            userNameAccountNumberDictionary.Add("Legacy450user3", "450117585");
            
            //STEP 1
            //Access Patterson Dental ecommerce site. 
            //Navigate to PDSI Homepage
            //DEV trunk: http://nt124181.pdental.com:5104/
            //DEV: http://nt124181.pdental.com:5106/
            //QA: http://nt124181.pdental.com:5108/
            //UAT: http://uat.pattersondental.com
            string url = "http://nt124181.pdental.com:5108/";
            string accountSuccessText = "Thank you for creating an account with Patterson";
            string password = "Pattnt1*";
            string question = "who owns this acct";
            string answer = "Corp QA group";
            string lastName = "Linked";
            int successCount = 0;

            switch (test.testEnvironment)
            {
                case "DEV":
                    url = "http://nt124181.pdental.com:5106/";
                    break;
                case "DEV trunk":
                    url = "http://nt124181.pdental.com:5106/";
                    break;
                case "QA":
                    url = "http://nt124181.pdental.com:5108/";
                    break;
                case "UAT":
                    url = "http://uat.pattersondental.com";
                    break;
                default:
                    url = "http://nt124181.pdental.com:5108/";
                    break;
            }

            driver.Url = url;

            //loop each row in dictionary
            foreach (KeyValuePair<string, string> pair in userNameAccountNumberDictionary)
            {
                string userName = pair.Key + "@mailinator.com";
                string accountNumber = pair.Value;

                //STEP 2
                //Click the Register Now link in the Home page Sign In portlet.
                driver.FindElement(By.LinkText("Register Now")).Click();
                Delay(timeout);
                driver.FindElement(By.Id("NewCustomerDto_ReceiveNewsLetter")).Click();
                //STEP 3
                //Enter in the Username field @username
                driver.FindElement(By.Id("NewCustomerDto_EmailAddress")).SendKeys(userName);

                //STEP 4
                //Enter in the Confirm Username @username
                driver.FindElement(By.Id("NewCustomerDto_ConfirmEmailAddress")).SendKeys(userName);

                //STEP 5
                //Enter in the Password field @password
                driver.FindElement(By.Id("NewCustomerDto_Password")).SendKeys(password);

                //STEP 6
                //Enter in the Retype Password field @password
                driver.FindElement(By.Id("NewCustomerDto_ConfirmPassword")).SendKeys(password);

                //STEP 7
                //Enter in the Security Question field @question
                driver.FindElement(By.Id("NewCustomerDto_SecurityQuestion")).SendKeys(question);

                //STEP 8
                //Enter in the Security Answer field @answer
                driver.FindElement(By.Id("NewCustomerDto_SecurityAnswer")).SendKeys(answer);

                //STEP 9
                //Enter in the First Name field @username
                driver.FindElement(By.Id("NewCustomerDto_FirstName")).SendKeys(userName);

                //STEP 10
                //Enter in the Last Name field @lastname
                driver.FindElement(By.Id("NewCustomerDto_LastName")).SendKeys(lastName);

                //STEP 11
                //Enter in the Designator field DDS
                driver.FindElement(By.Id("NewCustomerDto_Designator")).SendKeys("DDS");

                //STEP 12
                //Select in the Job Title dropdown Specialist Dentist
                IWebElement CCDropDown = driver.FindElement(By.Id("NewCustomerDto_JobTitleId"));
                SelectElement select = new SelectElement(CCDropDown);
                select.SelectByText("Dentist");

                //STEP 13
                //Retain the radio button for I have a Patterson Account
                //VERIFY: text on screen

                //STEP 14
                //Enter in the Patterson Customer Number field @acctnumber
                driver.FindElement(By.Id("NewCustomerDto_BranchNumber")).SendKeys(accountNumber.Substring(0, 3));
                driver.FindElement(By.Id("NewCustomerDto_CustNumberBPart")).SendKeys(accountNumber.Substring(3));

                //STEP 15
                //Uncheck the box before I would like to recieve Patterson dental newsletter and promotions
                //VERIFY: text on screen
                //HANDLED AT THE TOP OF THE TEST DUE TO PAGE FORMAT ISSUE WHERE CHECKBOX IS NOT VISIBLE
                //AFTER THE PAGE SCROLLS

                //STEP 16
                //Click the Submit button.
                //VERIFY: "Thank you for creating an account with Patterson, Inc."
                //         "Our customer service department is working actively to validate new accounts."
                //         "Your local branch will be contacting you via phone or email as soon as possible to finalize the sign up process and you will be able to sign-in into the web site."
                driver.FindElement(By.Id("Submit")).Click();
                Delay(timeout);
                if (driver.FindElement(By.TagName("body")).Text.IndexOf(accountSuccessText) > -1)
                {
                    Debug.WriteLine("Account '" + userName + "' successfully created.");
                    successCount++;
                }
                else
                {
                    Debug.WriteLine("Account '" + userName + "' NOT created.");
                }
                //STEP 17
                //Click the Continue button - user is navigated back to the Home page.
                driver.FindElement(By.LinkText("Home")).Click();
                Delay(timeout);
            }
           
            //STEP 18 
            //Close
            Debug.WriteLine(successCount.ToString() + " out of " + userNameAccountNumberDictionary.Count.ToString() + " accounts created successfully in the " + testEnvironment + " test environment.");
            driver.Close();

            //now verify web accounts on the Admin site
        }

        [TestMethod]
        public void VerifyWebAcounts()
        {
            //IWebDriver driver = new InternetExplorerDriver();
            IWebDriver driver = new FirefoxDriver();
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(30));
            string userName = "pdwebadmin";
            string password = "password";
            string url = "";

            Dictionary<string, string> userNameAccountNumberDictionary = new Dictionary<string, string>();
            userNameAccountNumberDictionary.Add("Legacy652user1", "652103128");
            userNameAccountNumberDictionary.Add("Legacy652user2", "652104320");
            userNameAccountNumberDictionary.Add("Legacy652user3", "652104335");
            userNameAccountNumberDictionary.Add("Legacy642user1", "642100205");
            userNameAccountNumberDictionary.Add("Legacy642user2", "642100207");
            userNameAccountNumberDictionary.Add("Legacy642user3", "642100564");
            userNameAccountNumberDictionary.Add("Legacy620user1", "620100231");
            userNameAccountNumberDictionary.Add("Legacy620user2", "620101957");
            userNameAccountNumberDictionary.Add("Legacy620user3", "620102072");
            userNameAccountNumberDictionary.Add("Legacy448user1", "448100073");
            userNameAccountNumberDictionary.Add("Legacy448user2", "448100758");
            userNameAccountNumberDictionary.Add("Legacy448user3", "448101554");
            userNameAccountNumberDictionary.Add("Legacy450user1", "450101075");
            userNameAccountNumberDictionary.Add("Legacy450user2", "450109862");
            userNameAccountNumberDictionary.Add("Legacy450user3", "450117585");

            switch (testEnvironment)
            {
                case "DEV":
                    url = "???";
                    break;
                case "DEV trunk":
                    url = "???";
                    break;
                case "QA":
                    url = "http://nt124181:6102";
                    break;
                case "UAT":
                    url = "http://nt124181:6104";
                    break;
                default:
                    url = "http://nt124181:6102";
                    break;
            }

            //| Step 1          | Navigate to the old Patterson Dental Admin site QA - ttp://nt124181:6102UAT - ttp://nt124181:6104                                                                                                                                                                                                     
            driver.Url = url;
            //| Step 2          | Login to either environment with the following userUser - pdwebadmin Password - password
            driver.FindElement(By.Id("_ctl0_Username_Textbox")).SendKeys(userName);
            driver.FindElement(By.Id("_ctl0_Password_Textbox")).SendKeys(password);
            driver.FindElement(By.Id("_ctl0_Login_Button")).Click();

             //loop each row in dictionary
            foreach (KeyValuePair<string, string> pair in userNameAccountNumberDictionary)
            {
                string newUser = pair.Key + "@mailinator.com";
                string userPassword = "Pattnt1*";
                string accountNumber = pair.Value;

                

                //| Step 3          | Under User Management click on New Customer Login Requests
                driver.Navigate().GoToUrl(url);
                System.Threading.Thread.Sleep(5000);
                driver.FindElement(By.LinkText("New Customer Login Requests")).Click();               

                //| Step 4          | Copy username from Dictionary                                      
            

                //| Step 5          | On the Admin site click Edit > Find on This Page                                                              


                //| Step 6          | In the text box next to Find paste the username

                //| Step 7          | Once the user is located click the Pencil icon under Edit for that user     
                //first get the row of given user
            

                IWebElement table = driver.FindElement(By.Id("_ctl0_dgOrderCust"));
                ReadOnlyCollection<IWebElement> allRows = table.FindElements(By.TagName("tr"));
                int rowINT = 0;
                int colINT = 0;
                int userRow = 0;
                int userCol = 0;
                foreach (IWebElement row in allRows)
                {
                    rowINT++;
                    ReadOnlyCollection<IWebElement> cells = row.FindElements(By.TagName("td"));

                    foreach (IWebElement cell in cells)
                    {
                        colINT++;
                        if (cell.Text.IndexOf(newUser) > -1)
                        {
                            //this is the cell with the tempuser
                            userRow = rowINT;
                            userCol = colINT;
                            break;
                        }                   
                    }
                    if (userRow > 0)
                    {
                        break;
                    }
                }

                int rowCount = allRows.Count;
                int editIndex = userRow - 2; // -2 due to header row and zero based index
                if (editIndex < 0)
                {
                    editIndex = 0;
                }
                string userEditLinkID = "_ctl0_dgOrderCust_lnkEdit_" + editIndex.ToString();

                driver.FindElement(By.Id(userEditLinkID)).Click();

                //| Step 8          | Locate the Patterson Customer # and copy the number to the clipboard


                //| Step 9          | In the lower left of the page there is a Patterson Customer # text box. 
                                    //Paste the customer number from the clipboard into this box
                driver.FindElement(By.Id("_ctl0_txtPattersonCustNo")).SendKeys(accountNumber);

                //| Step 10         | Click the Validate button
                driver.FindElement(By.Id("_ctl0_btnValidate")).Click();
                Delay(timeout);

                //verify back on Validate New Customers page
                if (driver.Title.IndexOf("Validate New Customers") > -1)
                {
                    Debug.WriteLine("User '" + newUser + "' with account number '" + accountNumber + "' successfully validated.");
                }
                else
                {
                    Debug.WriteLine("User '" + newUser + "' NOT validated.");
                }
            }

        }

        [TestMethod]
        public void ConfirmConsumerLogin(string userName, string password)
        {
            IWebDriver driver = new FirefoxDriver();
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(30));
            string url = "";

            Test test = new Test();
            switch (test.testEnvironment)
            {
                case "DEV":
                    url = "http://nt124181.pdental.com:5106/";
                    break;
                case "DEV trunk":
                    url = "http://nt124181.pdental.com:5106/";
                    break;
                case "QA":
                    url = "http://nt124181.pdental.com:5108/";
                    break;
                case "UAT":
                    url = "http://uat.pattersondental.com";
                    break;
                default:
                    url = "http://nt124181.pdental.com:5108/";
                    break;
            }

            Dictionary<string, string> userNameAccountNumberDictionary = new Dictionary<string, string>();
            userNameAccountNumberDictionary.Add("Legacy652user1", "652103128");
            userNameAccountNumberDictionary.Add("Legacy652user2", "652104320");
            userNameAccountNumberDictionary.Add("Legacy652user3", "652104335");
            userNameAccountNumberDictionary.Add("Legacy642user1", "642100205");
            userNameAccountNumberDictionary.Add("Legacy642user2", "642100207");
            userNameAccountNumberDictionary.Add("Legacy642user3", "642100564");
            userNameAccountNumberDictionary.Add("Legacy620user1", "620100231");
            userNameAccountNumberDictionary.Add("Legacy620user2", "620101957");
            userNameAccountNumberDictionary.Add("Legacy620user3", "620102072");
            userNameAccountNumberDictionary.Add("Legacy448user1", "448100073");
            userNameAccountNumberDictionary.Add("Legacy448user2", "448100758");
            userNameAccountNumberDictionary.Add("Legacy448user3", "448101554");
            userNameAccountNumberDictionary.Add("Legacy450user1", "450101075");
            userNameAccountNumberDictionary.Add("Legacy450user2", "450109862");
            userNameAccountNumberDictionary.Add("Legacy450user3", "450117585");

            //verify users can login on consumer site
            driver.Url = url;

            foreach (KeyValuePair<string, string> pair in userNameAccountNumberDictionary)
            {
                string newUser = pair.Key + "@mailinator.com";
                string userPassword = "Pattnt1*";
                string accountNumber = pair.Value;

                string validateFailText = "Our customer service department is working actively to validate new accounts";

                driver.FindElement(By.Id("txtUserName")).SendKeys(userName);
                driver.FindElement(By.Id("txtPassword")).SendKeys(password);
                driver.FindElement(By.Id("loginSubmit")).Click();
                Delay(5000);

                if (driver.FindElement(By.TagName("body")).Text.IndexOf(validateFailText) > -1)
                {
                    Debug.WriteLine("The user '" + userName + "' was not able to login after validating new account request on the Admin site.");
                }
                else
                {
                    Debug.WriteLine("Login on consumer site successfull for '" + userName + "'.");
                }

                driver.Navigate().GoToUrl(url);
            }
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
