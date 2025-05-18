using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Runtime.InteropServices;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Core;

namespace 旺旺发消息
{
    public partial class 旺旺发消息 : Form
    {
        public 旺旺发消息()
        {
            InitializeComponent();
        }


        Thread thread;
        bool status = true;


        StringBuilder sb=new StringBuilder();   

        private void 旺旺发消息_Load(object sender, EventArgs e)
        {
            //webView21.Source = new Uri("https://air.1688.com/app/ocms-fusion-components-1688/def_cbu_web_im_core/index.html?touid=cnalichn媚裳服装厂&siteid=cnalichn&status=1&portalId=&gid=&offerId=893587953407&itemsId=&spmid=a261y.7663282.0#/");

          
        }

        // 辅助方法：转义 JavaScript 字符串
        private string EscapeJsString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            return value.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\n", "\\n").Replace("\r", "\\r");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            }
          
        }



        public void inputTxt(WebView2 webview)
        {


           

            string className = "edit";
            string valueToInput = textBox1.Text;

            if (string.IsNullOrEmpty(className))
            {
                MessageBox.Show("请输入要查找的 Class 名称");
                return;
            }

            try
            {
                // 使用 JavaScript 查找指定 class 的 contenteditable 元素并赋值
                string script = $@"
                    (function() {{
                        var elements = document.querySelectorAll('[contenteditable=""true""].{className}');
                        if (elements.length > 0) {{
                            // 为第一个找到的元素设置内容
                            elements[0].textContent = '{EscapeJsString(valueToInput)}';
                            
                            // 触发输入事件，使页面感知到内容变化
                            var event = new Event('input', {{ bubbles: true }});
                            elements[0].dispatchEvent(event);
                            
                            // 触发焦点事件，模拟用户操作
                            elements[0].focus();
                            
                            return true;
                        }}
                        return false;
                    }})();
                ";

                webview.Invoke(new Action(async () =>
                {
                    await webview.CoreWebView2.ExecuteScriptAsync(script);
                }));
            }


            catch (Exception ex)
            {
                MessageBox.Show($"执行脚本时出错: {ex.Message}");
            }
        }

        public void senMsg(WebView2 webview)
        {

           

            string buttonText = "发送";

            if (string.IsNullOrEmpty(buttonText))
            {
                MessageBox.Show("请输入按钮文本");
                return;
            }

            try
            {
                string script = "";

                script = $@"
                            (function() {{
                                // 使用 XPath 查找包含指定文本的 span 元素
                                var xpath = '//span[contains(text(), ""{EscapeJsString(buttonText)}"")]';
                                var result = document.evaluate(xpath, document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null);
                                var element = result.singleNodeValue;
                                
                                if (element) {{
                                    // 模拟鼠标点击事件
                                    var clickEvent = new MouseEvent('click', {{
                                        bubbles: true,
                                        cancelable: true,
                                        view: window
                                    }});
                                    element.dispatchEvent(clickEvent);
                                    return true;
                                }}
                                return false;
                            }})();
                        ";

               
                webview.Invoke(new Action(async () =>
                {
                   await webview.CoreWebView2.ExecuteScriptAsync(script);
                }));

            }
            catch (Exception ex)
            {
                MessageBox.Show($"执行脚本时出错: {ex.Message}");
            }
        }


        List<WebView2> WebView2list = new List<WebView2>();

        public async void createWeb()
        {

            WebView2 webView = new WebView2();
            webView.Dock = DockStyle.Fill;


            TabPage newTab = new TabPage($"新增账号 {tabControl1.TabCount + 1}");
            tabControl1.TabPages.Add(newTab);
            tabControl1.SelectedTab = newTab;


            // 添加到窗体
            newTab.Controls.Add(webView);


            // 生成唯一用户数据目录
            string userDataFolder = Path.Combine(
                Path.GetTempPath(),
                $"WebView2Data_{Guid.NewGuid()}"
            );

            // 创建独立环境
            var env = await CoreWebView2Environment.CreateAsync(
                userDataFolder: userDataFolder
            );

            // 配置WebView2选项
            var options = new CoreWebView2EnvironmentOptions
            {
                // 可选：禁用首次运行提示
                AllowSingleSignOnUsingOSPrimaryAccount = false
            };

            // 初始化WebView2控件
            await webView.EnsureCoreWebView2Async(env);


            WebView2list.Add(webView);  
            webView.Source = new Uri("https://air.1688.com/app/ocms-fusion-components-1688/def_cbu_web_im_core/index.html?touid=cnalichn媚裳服装厂&siteid=cnalichn&status=1&portalId=&gid=&offerId=893587953407&itemsId=&spmid=a261y.7663282.0#/");


        }
        string wangwangs = "";

      


        public void run()
        {
            if(WebView2list.Count==0)
            {
                MessageBox.Show("请添加账号");
                return;

            }

            string[] text = wangwangs.Split(new string[] { "\r\n" }, StringSplitOptions.None);

           
            if (text.Length<=1)
            {
                MessageBox.Show("请导入旺旺");
                return;

            }
            for (int i = 0; i < text.Length; i++)
            {
                for (int a = 0; a< WebView2list.Count; a++)
                {

                    string wangwang = text[i].Trim();
                    sb.AppendLine(wangwang+"  "+"成功");

                    WebView2list[a].Invoke(new Action(async () =>
                    {

                         WebView2list[a].CoreWebView2.Navigate("https://air.1688.com/app/ocms-fusion-components-1688/def_cbu_web_im_core/index.html?touid=cnalichn" + wangwang + "&siteid=cnalichn&status=1&portalId=&gid=&offerId=893587953407&itemsId=&spmid=a261y.7663282.0#/");
                    }));

                    Thread.Sleep(2000);
                    inputTxt(WebView2list[a]);

                    Thread.Sleep(1000);
                    senMsg(WebView2list[a]);

                    Thread.Sleep(Convert.ToInt32(textBox3.Text) * 1000);
                    if (status == false)
                        return;
                }
            }

        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName, function.EncodingType.GetTxtType(openFileDialog1.FileName));
                //一次性读取完 
                wangwangs = sr.ReadToEnd();
                string[] text = wangwangs.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                MessageBox.Show("共导入"+text.Length.ToString());
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            createWeb();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "文本文件|*.txt";
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveDialog.FileName, sb.ToString());
                    MessageBox.Show("导出完成");
                }

               
            }
        }
    }
}
