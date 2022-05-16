using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 主程序selenium
{
    public class function
    {
        public static IWebDriver getdriver(bool headless,bool nopic)
        {
            //关闭cmd窗口
            ChromeDriverService driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;


            ChromeOptions options = new ChromeOptions();
            options.BinaryLocation = "Chrome/Application/chrome.exe";
            //禁用图片
            if(nopic)
            {
                options.AddUserProfilePreference("profile.default_content_setting_values.images", 2);
            }
            
            if (headless)
            {
                //options.AddArgument("window-size=1920,1080");
                options.AddArgument("--headless");

            }
            options.AddArgument("--disable-gpu");

            return new ChromeDriver(driverService, options);
        }
    }
}
