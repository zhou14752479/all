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
using myDLL;

namespace 主程序
{
    public partial class ebay : Form
    {
        public ebay()
        {
            InitializeComponent();
        }
        bool zanting = true;
        public void run()
        {
            StreamReader streamReader = new StreamReader(this.textBox1.Text, Encoding.Default);
            string text = streamReader.ReadToEnd();
            string[] array = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            for (int i = 0; i < array.Length; i++)
            {
                try
                {
                    string url = "https://accounts.ebay.com/acctxs/init";
                    string postdata = "identifier=" + System.Web.HttpUtility.UrlEncode(array[i]) + "&srt=01000400000050d8d8d3ca7a9fbdb76f0e616c27df9c210951784fbf08456c8f44acb0373c637b0c2c1731660b4ea02b35f937a643cd045f9eae0fa7bed4e36139833942e98821587e63b0e5bc4e4afdc098959f444bd8&hbi=0&ru=&srcappid=&clientapptype=";
                    string cookie = "s=CgAD4ACBeZve3YmQxMTQ3MjIxNzAwYTE2ZmIwY2VhNDBiZmZjODBiOThthxIq; D_ZID=EE32E04F-A1CE-34C4-ADC4-855CD7A80638; D_ZUID=2D498867-AF9F-36C1-B43A-EA5A900B62DA; D_HID=30ECF726-4C2B-31D6-AF17-8F6EBF293D60; D_SID=218.93.192.122:qzBNEKcqyeC25XrC1S6HHqrQTUGas66EFxrxct1bIiI; ak_bmsc=8B6ED729B44D16541A1027823E759B4743942F9C652300003FA6655EF3B72B6D~plCnW+egimIyy85w/bvySEiLs6FdOU+YP647ZS7SfEMf2t+QaoIdQAf5jLTC+dKVcOS9fDgcJNdyAY8rgjMdqnDngzILjl5Jd/63NFiWD8JqLhC5Z2bmEexJ3umJkqbT0/1w7LdCZ55pms6TtSL2gc40EVZf81YbN1H5bMsbwnDRAZ1ZJ4DxoMQjTiNRwTk1X8hgtxHVV/Ty89wLJiTfU1qNsOOEGHVu+sZUsfJ2urEsw=; D_IID=E85317E3-9239-3F74-8ADD-215547869C54; D_UID=4F9BBA52-3FE9-3706-8DA7-C6CD74CB339E; bm_sv=EE357BCC25E6C7837D0C383EDFB1937A~puDhnUYz6WcVJHAe0v430EARRWzH+v4LMflVK6LOJXGacb0M3I0vp6u+QJtqoTWYMnR0xELgCGSZNzfb+c41bF/SKrZ1okyRm8DtvhBKKHjXx23oyII6eOBANXtqBiBM7/IOL5+DD9m7RKhE7IsnYw==; npii=btguid/bd1147221700a16fb0cea40bffc80b9862280db1^cguid/bd1153aa1700a9b19c52b1c3fdefd29f62280db1^; dp1=bbl/CN62280db1^; nonsession=BAQAAAXAiGIjaAAaAADMABmBG2jEyMjM4MDAAygAgYigNsWJkMTE0NzIyMTcwMGExNmZiMGNlYTQwYmZmYzgwYjk4AMsAAl5lrbkxN8qir4deAtrtBzP6xYzlq497TIXd; ebay=%5Esbf%3D%23000000%5Epsi%3DAEx1Ax%2FY*%5E; ds2=sotr/b9nKjzzzzzzz^";
                    string html = method.PostUrlDefault(url, postdata, cookie);
                    if (html.Contains("acctxs"))
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(array[i]);
                        lv1.SubItems.Add("正确");

                    }
                    else
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(array[i]);
                        lv1.SubItems.Add("错误");
                    }

                    while (zanting == false)
                    {
                        Application.DoEvents();//等待本次加载完毕才执行下次循环.
                    }

                }
                catch 
                {

                    continue;
                }
               

            }
        }

        private void Ebay_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (html.Contains(@"ebay"))
            {
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;

            }

            else
            {
                MessageBox.Show("验证失败");
                return;
            }


            #endregion
           
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
