namespace EventManagement.TestAutomation.TestCore.Base
{
    using System;
    using System.Drawing;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Firefox;
    using OpenQA.Selenium.IE;
    using OpenQA.Selenium.Safari;
    using OpenQA.Selenium.Support.UI;

    using Protractor;

    public static class Driver
    {
        private static WebDriverWait browserWait;
        private static IWebDriver browser;
        private static NgWebDriver ngDriver;

        public static IWebElement selElement { get; set; }
        public static NgWebElement prElement { get; set; }

        public static IWebDriver Browser
        {
            get
            {
                return browser;
            }
            private set
            {
                browser = value;
            }
        }

        public static NgWebDriver NgDriver
        {
            get
            {
                return ngDriver;
            }
            private set
            {
                ngDriver = value;
            }
        }

        public static WebDriverWait BrowserWait
        {
            get
            {
                return browserWait;
            }
            private set
            {
                browserWait = value;
            }
        }
        public static void StartBrowser(BrowserTypes browserType = BrowserTypes.Firefox, int defaultTimeOut = 30)
        {
            switch (browserType)
            {
                case BrowserTypes.Firefox:
                    Driver.Browser = new FirefoxDriver();
                    break;
                case BrowserTypes.InternetExplorer:
                    Driver.Browser = new InternetExplorerDriver();
                    break;
                case BrowserTypes.Chrome:
                    Driver.Browser = new ChromeDriver();
                    break;
                case BrowserTypes.Safari:
                    Driver.Browser = new SafariDriver();
                    break;
                case BrowserTypes.ProtractorIE:
                    Driver.Browser = new InternetExplorerDriver();
                    var ieWebSt = (Driver.Browser as InternetExplorerDriver).HasWebStorage;
                    Driver.NgDriver = new NgWebDriver(browser);
                    break;
                case BrowserTypes.ProtractorChrome:
                    Driver.Browser = new ChromeDriver();
                    Driver.Browser.Manage().Window.Maximize();
                    Driver.NgDriver = new NgWebDriver(browser);
                    break;
                default:
                    break;
            }
            //BrowserWait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(defaultTimeOut));
        }

        public static void SetMobileDevice(MobileDevicesTypes mobileType = MobileDevicesTypes.iPhone5)
        {
            switch (mobileType)
            {
                case MobileDevicesTypes.iPhone5:
                    Driver.Browser.Manage().Window.Size = new Size(320, 568);
                    break;
                case MobileDevicesTypes.iPhone6:
                    Driver.Browser.Manage().Window.Size = new Size(375, 667);
                    break;
                case MobileDevicesTypes.iPhone6Plus:
                    Driver.Browser.Manage().Window.Size = new Size(414, 736);
                    break;
                case MobileDevicesTypes.iPadAir:
                    Driver.Browser.Manage().Window.Size = new Size(768, 1024);
                    break;
                case MobileDevicesTypes.iPadPro:
                    Driver.Browser.Manage().Window.Size = new Size(768, 1366);
                    break;
                default:
                    break;
            }
        }
        public static void StopBrowser()
        {
            Browser.Quit();
            Browser = null;
            BrowserWait = null;
        }
    }
}
