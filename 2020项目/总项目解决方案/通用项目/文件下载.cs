using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 通用项目
{
    public partial class 文件下载 : Form
    {
        public 文件下载()
        {
            InitializeComponent();
        }
        #region 去掉路径中非法字符
        public string removeValid(string illegal)
        {
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            foreach (char c in invalid)
            {
                illegal = illegal.Replace(c.ToString(), "");
            }
            return illegal;
        }
       

        string path = AppDomain.CurrentDomain.BaseDirectory;
        #endregion
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {

            try
            {

                for (int i = 1; i <56; i++)
                {
                    


                    string url = "https://wenku.baidu.com/lottery/interface/2020wuhanfightmain?sourceType=1&grade=0&subject=0&bookVersion=0&pn="+i+"&rn=16&tokenCheck=1";
                    string html = method.GetUrl(url, "gb2312");

                    MatchCollection IDS = Regex.Matches(html, @"""strID"":""([\s\S]*?)""");
                    foreach (Match ID in IDS)
                    {

                        string aurl = "https://wenku.baidu.com/lottery/browse/2020wuhanfight?id=" + ID.Groups[1].Value;
                        string ahtml = method.GetUrl(aurl, "gb2312");
                        Match title = Regex.Match(ahtml, @"<div class=""c2 f28 font-weight pkg-title"">([\s\S]*?)</div>");
                        Match downurl = Regex.Match(ahtml, @"""down_url"":""([\s\S]*?)""");
                        string downUrl = downurl.Groups[1].Value.Replace("\\","");

                        method.downloadFile(downUrl, path + "下载文件\\", removeValid(title.Groups[1].Value) + "." +".zip","");

                        textBox1.Text = title.Groups[1].Value;


                        Thread.Sleep(1000);





                    }

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }


        }

        private void 文件下载_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
