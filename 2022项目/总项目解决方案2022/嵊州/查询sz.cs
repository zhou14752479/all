using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;
namespace 嵊州
{
    public partial class 查询sz : Form
    {
        public 查询sz()
        {
            InitializeComponent();
        }
        bool zanting = true;
        bool status = false;
        Thread thread;
        DataTable dt;
        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            // openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.Filter = "excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox1.Text = openFileDialog1.FileName;
                dt = method.ExcelToDataTable(textBox1.Text, true);

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请先导入表格");
                return;
            }

            if (status == true)
            {
                status = false;

            }
            else
            {
                status = true;
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(run);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {

                zanting = true;
            }
            else
            {
                zanting = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private delegate string Encrypt(string key, string data);

        public string encrypt(string key,string data)
        {

            string result = webBrowser1.Document.InvokeScript("encryptByDES", new object[] {data,key }).ToString();
            return result;
        }
        public string decrypt(string key, string data)
        {

            string result = webBrowser1.Document.InvokeScript("decryptByDES", new object[] { data,key }).ToString();
            return result;
        }

        /// <summary>
        /// 获取时间戳毫秒
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalMilliseconds);
            return a.ToString();
        }

        public void run()
        {
            string gregegedrgerheh = gdsgdgdgdgdstgfeewrwerw3r23r32rvxsvdsv.rgebgdgdvsdfsdvsdfsdvdsbgdsrt435b515sdfsdf();
            string token = gregegedrgerheh.Split(new string[] { "," }, StringSplitOptions.None)[0];
            string expiretime = gregegedrgerheh.Split(new string[] { "," }, StringSplitOptions.None)[1];

            if (DateTime.Now > Convert.ToDateTime(expiretime))
            {

                return;
            }

            for (int a = 0; a < dt.Rows.Count; a++)
            {



                string time = GetTimeStamp();
                string key = time.Substring(time.Length-8,8);
                DataRow dr = dt.Rows[a];
                string card = dr[0].ToString();

                //请求加密
                Encrypt aa = new Encrypt(encrypt);
                string data = "{\"funCode\":\"9113\",\"cardId\":\""+card+"\"}";
                IAsyncResult iar = BeginInvoke(aa, new object[] { key,data });
                string crypt = EndInvoke(iar).ToString();
               
                string url = "https://cthyqfk.szhealth.net/prod-api/data/wechat/invoker";

                string postdata = "{\"requestTime\":\""+ time + "\",\"paramString\":\""+crypt+"\",\"funCode\":\"getUserCreate\"}";
                string html = method.PostUrl(url, postdata,"","utf-8", "application/json", "");
                
                string resultData = Regex.Match(html, @"""resultData"":""([\s\S]*?)""").Groups[1].Value;
                string responseTime = Regex.Match(html, @"""responseTime"":""([\s\S]*?)""").Groups[1].Value;


                //返回解密
                string key2= responseTime.Substring(time.Length - 8, 8);
                Encrypt bb = new Encrypt(decrypt);
                IAsyncResult iar2 = BeginInvoke(bb, new object[] {key2, resultData });
                string decrytdata= EndInvoke(iar2).ToString();

                
                string patName = Regex.Match(decrytdata, @"""patName"":""([\s\S]*?)""").Groups[1].Value;
                string idCard = Regex.Match(decrytdata, @"""idCard"":""([\s\S]*?)""").Groups[1].Value;
                string phone = Regex.Match(decrytdata, @"""phone"":""([\s\S]*?)""").Groups[1].Value;

                if (jiami == true)
                {
                    patName = method.Base64Encode(Encoding.GetEncoding("utf-8"), patName);
                    idCard = method.Base64Encode(Encoding.GetEncoding("utf-8"), idCard);
                    phone = method.Base64Encode(Encoding.GetEncoding("utf-8"), phone);


                }

                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据 
                lv1.SubItems.Add(patName);
                lv1.SubItems.Add(idCard);
                lv1.SubItems.Add(phone);

                if (radioButton1.Checked == true)
                {
                    Thread.Sleep(1500);
                }
                if (radioButton2.Checked == true)
                {
                    Thread.Sleep(3000);
                }


                while (zanting == false)
                {
                    Application.DoEvents();//等待本次加载完毕才执行下次循环.
                }
                if (status == false)
                    return;
            }



        }


        bool jiami = true;
        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
        string path = AppDomain.CurrentDomain.BaseDirectory;
        private void 查询sz_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate(path + "index/des.html"); //按照姓名找回 执行加密RSA JS方法
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "14752479")
            {
                MessageBox.Show("密码错误");
                return;
            }


            zanting = false;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                for (int j = 1; j < listView1.Columns.Count; j++)
                {
                    try
                    {

                        if (jiami == false)
                        {
                            if (j != 0)
                            {
                                listView1.Items[i].SubItems[j].Text = method.Base64Encode(Encoding.GetEncoding("utf-8"), listView1.Items[i].SubItems[j].Text);
                            }

                        }
                        else
                        {
                            if (j != 0)
                            {
                                listView1.Items[i].SubItems[j].Text = method.Base64Decode(Encoding.GetEncoding("utf-8"), listView1.Items[i].SubItems[j].Text);
                            }
                        }

                    }
                    catch (Exception ex)
                    {

                        continue;
                    }
                }
            }


            zanting = true;

            if (jiami == false)
            {
                jiami = true;
            }
            else
            {
                jiami = false;
            }
        }




    }
}
