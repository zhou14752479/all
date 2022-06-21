using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序202203
{
    public partial class 题目格式 : Form
    {
        public 题目格式()
        {
            InitializeComponent();
        }

        int count = 18;
        private void button1_Click(object sender, EventArgs e)
        {
            count=count+1;  
               StringBuilder sb = new StringBuilder();
            sb.AppendLine("<div class=\"question-each\">");

            sb.AppendLine("<!-- 标题 -->");
            sb.AppendLine("<div class=\"question-name\">"+textBox1.Text.Replace(" ", "&nbsp;").Trim()+"</div>");
            sb.AppendLine("<!-- 选项 -->");
            sb.AppendLine("<div class=\"question-option\">");


            string[] text = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            string A = "";
            string B = "";
            string C = "";
            string D = "";
            foreach (string item in text)
            {
                if(item.Trim()!="")
                {
                    if(A=="")
                    {
                        A = item.Trim().Replace(" ", "&nbsp;");
                    }
                    else if(B == "")
                    {
                        B = item.Trim().Replace(" ", "&nbsp;");
                    }
                    else if (C== "")
                    {
                        C = item.Trim().Replace(" ", "&nbsp;");
                    }
                    else if (D == "")
                    {
                        D= item.Trim().Replace(" ", "&nbsp;");
                    }
                }
            }
            sb.AppendLine("<label><input type=\"radio\" value=\"A\" name=\"single["+count+"]\" required>"+A+"</label>");
            sb.AppendLine("<label><input type=\"radio\" value=\"B\" name=\"single[" + count + "]\" required>" + B + "</label>");
            sb.AppendLine("<label><input type=\"radio\" value=\"C\" name=\"single[" + count + "]\" required>" + C + "</label>");
            sb.AppendLine("<label><input type=\"radio\" value=\"D\" name=\"single[" + count + "]\" required>" + D + "</label>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");

            richTextBox1.Text+=sb.ToString()+"\r\n";
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox1.Text = textBox1.Text + "<br/>";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HtmlConvertToPdf("https://www.toutiao.com/", "D:\\a.pdf");
        }


        public static bool HtmlConvertToPdf(string htmlPath, string savePath)
        {
            bool flag = false;
            CheckFilePath(savePath);

            ///这个路径为程序集的目录，因为我把应用程序 wkhtmltopdf.exe 放在了程序集同一个目录下
            string exePath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "wkhtmltopdf.exe";
            if (!File.Exists(exePath))
            {
                throw new Exception("No application wkhtmltopdf.exe was found.");
            }

            try
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                processStartInfo.FileName = exePath;
                processStartInfo.WorkingDirectory = Path.GetDirectoryName(exePath);
                processStartInfo.UseShellExecute = false;
                processStartInfo.CreateNoWindow = true;
                processStartInfo.RedirectStandardInput = true;
                processStartInfo.RedirectStandardOutput = true;
                processStartInfo.RedirectStandardError = true;
                processStartInfo.Arguments = GetArguments(htmlPath, savePath);



                Process process = new Process();
                process.StartInfo = processStartInfo;
                process.Start();
                process.WaitForExit(60000);

                ///用于查看是否返回错误信息
                StreamReader srone = process.StandardError;
                StreamReader srtwo = process.StandardOutput;
                string ss1 = srone.ReadToEnd();
                string ss2 = srtwo.ReadToEnd();
                srone.Close();
                srone.Dispose();
                srtwo.Close();
                srtwo.Dispose();

                process.Close();
                process.Dispose();

                flag = true;
            }
            catch
            {
                flag = false;
            }
            return flag;
        }


        private static void CheckFilePath(string savePath)
        {
            string ext = string.Empty;
            string path = string.Empty;
            string fileName = string.Empty;

            ext = Path.GetExtension(savePath);
            if (string.IsNullOrEmpty(ext) || ext.ToLower() != ".pdf")
            {
                throw new Exception("Extension error:This method is used to generate PDF files.");
            }

            fileName = Path.GetFileName(savePath);
            if (string.IsNullOrEmpty(fileName))
            {
                throw new Exception("File name is empty.");
            }

            try
            {
                path = savePath.Substring(0, savePath.IndexOf(fileName));
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch
            {
                throw new Exception("The file path does not exist.");
            }
        }

        
        // 在main函数直接调用HtmlConvertToPdf(htmlPath, savePath);



        private static string GetArguments(string htmlPath, string savePath)
        {
            if (string.IsNullOrEmpty(htmlPath))
            {
                throw new Exception("HTML local path or network address can not be empty.");
            }

            if (string.IsNullOrEmpty(savePath))
            {
                throw new Exception("The path saved by the PDF document can not be empty.");
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" --page-height 100 ");        //页面高度100mm
            stringBuilder.Append(" --page-width 100 ");         //页面宽度100mm
            stringBuilder.Append(" --header-center 我是页眉 ");  //设置居中显示页眉
            stringBuilder.Append(" --header-line ");         //页眉和内容之间显示一条直线
            stringBuilder.Append(" --footer-center \"Page [page] of [topage]\" ");    //设置居中显示页脚
            stringBuilder.Append(" --footer-line ");       //页脚和内容之间显示一条直线
            stringBuilder.Append(" " + htmlPath + " ");       //本地 HTML 的文件路径或网页 HTML 的URL地址
            stringBuilder.Append(" " + savePath + " ");       //生成的 PDF 文档的保存路径
            return stringBuilder.ToString();
        }


    }
}
