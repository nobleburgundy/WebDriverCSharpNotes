using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;  // for debug.writeline
using System.Threading;
using Microsoft.VisualStudio.TestTools.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils = ImperialUITests.Utilities;
using OpenQA.Selenium;   // needed for iwebdriver
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System.Drawing;


// quick template for any .cs file to create individual tests in
// each test is [TestMethod]
// non-working [ClassInitialize] & its clean-up function listed here, but might be depreciated
// 


namespace ImperialUITests
{
    [TestClass]
    public class AdminTests
    {
        public IWebDriver driver = new InternetExplorerDriver(internetExplorerDriverServerDirectory: "C:\\");
        public InternetExplorerOptions opts = new InternetExplorerOptions();

        [ClassInitialize]
        public static void Adm_ClassInit(TestContext mycontext)
        {
            Debug.WriteLine("class initialized"); // this never runs
            
        }


        [TestInitialize]
        public void TestInit()
        {
            Debug.WriteLine("  Test init is happening here");
            
        }

        //[TestCategory("Admin Tests")]

        [TestMethod]
        [TestProperty("Description","Any admin test here")]
        public void Admintest1()
        {
            Debug.WriteLine("       junkmethod1 written");

        }


        [TestMethod]
        [TestProperty("Description", "Any admin test here2")]
        public void Admintest2()
        {
            Debug.WriteLine("     junkmethod2 written");

        }


        // cleanup event
        [TestCleanup]
        public void TestCleanup()
        {
            Debug.WriteLine("  Test cleanup");
        }

        
        // runs very last(cant get to run)
        [ClassCleanup]
        static public void ClassCleanup()
        {
            Debug.WriteLine("class cleanup");  
        }

    }
}
