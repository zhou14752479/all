using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 淘宝访客
{
    public partial class 数据获取 : Form
    {
        public 数据获取()
        {
            InitializeComponent();
        }

        string sqlurl = "http://120.24.252.181/kehutaobao/do.php";

      
        public string getall()
        {
            string postdata = "method=getall";
            string html = method.PostUrlDefault(sqlurl, postdata, "");
            return html;
        }
       

        public void getalldata()
        {
            dataGridView1.Rows.Clear();
            string html = getall();

            MatchCollection shebei = Regex.Matches(html, @"""shebei"":""([\s\S]*?)""");
            MatchCollection shopname = Regex.Matches(html, @"""shopname"":""([\s\S]*?)""");
            MatchCollection shopurl = Regex.Matches(html, @"""shopurl"":""([\s\S]*?)""");
            MatchCollection item = Regex.Matches(html, @"""item"":""([\s\S]*?)""");
            MatchCollection itemurl = Regex.Matches(html, @"""itemurl"":""([\s\S]*?)""");
            MatchCollection shouhou = Regex.Matches(html, @"""shouhou"":""([\s\S]*?)""");
            MatchCollection z_fk = Regex.Matches(html, @"""z_fk"":""([\s\S]*?)""");
            MatchCollection z_all_fk = Regex.Matches(html, @"""z_all_fk"":""([\s\S]*?)""");
            MatchCollection s_fk = Regex.Matches(html, @"""s_fk"":""([\s\S]*?)""");
            MatchCollection s_all_fk = Regex.Matches(html, @"""s_all_fk"":""([\s\S]*?)""");
            MatchCollection keywords = Regex.Matches(html, @"""keywords"":""([\s\S]*?)""");



            for (int i = 0; i < shopname.Count; i++)
            {

                dataGridView1.Rows.Add(new object[] { shebei[i].Groups[1].Value, method.Unicode2String(shopname[i].Groups[1].Value), shopurl[i].Groups[1].Value, method.Unicode2String(item[i].Groups[1].Value), method.Unicode2String(itemurl[i].Groups[1].Value), shouhou[i].Groups[1].Value, z_fk[i].Groups[1].Value, z_all_fk[i].Groups[1].Value, s_fk[i].Groups[1].Value, s_all_fk[i].Groups[1].Value, method.Unicode2String(keywords[i].Groups[1].Value).Replace(@"\r\n", System.Environment.NewLine) });


            }



        }

        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            getalldata();
        }

        public void delall()
        {
            string postdata = "method=delall";
            string html = method.PostUrlDefault(sqlurl, postdata, "");
           
        }
        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.DgvToTable(dataGridView1), "Sheet1", true);
        }
        #region  程序关闭删除自身
        public static void TestForKillMyself()
        {
            string bat = @"@echo off
                           :tryagain
                           del %1
                           if exist %1 goto tryagain
                           del %0";
            File.WriteAllText("killme.bat", bat);//写bat文件
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "killme.bat";
            psi.Arguments = "\"" + Environment.GetCommandLineArgs()[0] + "\"";
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(psi);
        }


        #endregion
        private void 数据获取_Load(object sender, EventArgs e)
        {
            #region 通用检测


            if (!method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"uHtj"))
            {
                TestForKillMyself();
                System.Diagnostics.Process.GetCurrentProcess().Kill();

                return;
            }

            #endregion
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                delall();
                getalldata();
            }
            else
            {
               
            }
        }
    }
}
