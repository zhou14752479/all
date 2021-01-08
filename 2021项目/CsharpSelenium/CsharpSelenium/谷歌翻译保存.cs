using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CsharpSelenium
{
    public partial class 谷歌翻译保存 : Form
    {
        public 谷歌翻译保存()
        {
            InitializeComponent();
        }
        public void run()
        {
                    
            ChromeOptions options = new ChromeOptions();
            //  options.AddArgument("--user-agent=" + "some safari agent");
            //options.AddArgument("--lang=en");
            options.AddArgument("--save-page-as-mhtml");


            //            var prefs = new Dictionary<string, object> {

            //    { "download.default_directory", @"C:\code" },

            //    { "download.prompt_for_download", false }

            //};
            var prefs = "{\"translate_whitelists\": {\"en\":\"zh - CN\"}, \"translate\":{\"enabled\":\"true\"}}";
            options.AddUserProfilePreference("printing.print_preview_sticky_settings.appState", prefs);
            
              //options.AddUserProfilePreference("prefs", prefs);



            IWebDriver driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl("https://www.baidu.com/s?ie=UTF-8&wd=4311012688009");  //driver.Url = "http://www.baidu.com"是一样的
            Thread.Sleep(1000);

            //driver.Quit();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            run();
        }
    }
}
