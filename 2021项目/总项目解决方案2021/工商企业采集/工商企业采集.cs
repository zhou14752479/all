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
using myDLL;

namespace 工商企业采集
{
    public partial class 工商企业采集 : Form
    {
        public 工商企业采集()
        {
            InitializeComponent();
        }

        public string gettel(string id)
        {
            StringBuilder sb = new StringBuilder();
            string url = "https://aiqicha.baidu.com/company_detail_"+ id;
            string html= method.GetUrl(url, "utf-8");
            MatchCollection phones = Regex.Matches(html, @"""phone"":""([\s\S]*?)""");
            foreach (Match item in phones)
            {
                if(item.Groups[1].Value.Length>5)
                {
                    sb.Append(item.Groups[1].Value + ",");
                }
                
            }

            if (sb.ToString().Length > 2)
            {
                return sb.ToString().Remove(sb.ToString().Length - 1, 1);
            }
            else
            {
                return sb.ToString();
            }


            
        }

        public void run()
        {

            try
            {
                for (int page= 1; page < 10001; page++)
                {

                    string url = "https://aiqicha.baidu.com/s/advanceFilterAjax?q=" + System.Web.HttpUtility.UrlEncode(textBox2.Text) + "&t=&p="+page+"&s=10&o=0&f=%7B%7D";
                    string html = method.GetUrl(url, "utf-8");
                    html = method.Unicode2String(html);
                    textBox1.Text = html;
                    MatchCollection uids = Regex.Matches(html, @"""pid"":""([\s\S]*?)""");
                    MatchCollection entName = Regex.Matches(html, @"""titleName"":""([\s\S]*?)""");
                    MatchCollection legalPerson = Regex.Matches(html, @"""legalPerson"":""([\s\S]*?)""");
                    MatchCollection regCap = Regex.Matches(html, @"""regCap"":""([\s\S]*?)""");
                    MatchCollection validityFrom = Regex.Matches(html, @"""validityFrom"":""([\s\S]*?)""");
                    MatchCollection domicile = Regex.Matches(html, @"""domicile"":""([\s\S]*?)""");
                    MatchCollection scope = Regex.Matches(html, @"""scope"":""([\s\S]*?)""");

                    for (int i = 0; i < uids.Count; i++)
                    {
                        label7.Text =DateTime.Now.ToLongTimeString()+ "正在提取："+ entName[i].Groups[1].Value;
                        string tel = gettel(uids[i].Groups[1].Value);
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString());
                        lv1.SubItems.Add(entName[i].Groups[1].Value);
                        lv1.SubItems.Add(legalPerson[i].Groups[1].Value);
                        lv1.SubItems.Add(regCap[i].Groups[1].Value);
                        lv1.SubItems.Add(validityFrom[i].Groups[1].Value);
                        lv1.SubItems.Add(domicile[i].Groups[1].Value);
                        lv1.SubItems.Add(scope[i].Groups[1].Value);
                        lv1.SubItems.Add(tel);
                        Thread.Sleep(1000);
                        while (this.zanting == false)
                        {
                            Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                        }
                        if (status == false)
                            return;
                    }
                }
            }
            catch (Exception ex)
            {

               label7.Text=ex.ToString();
            }
        }
        private void 工商企业采集_Load(object sender, EventArgs e)
        {

        }
        bool zanting = true;
        bool status = true;
        Thread thread;
        private void button7_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                MessageBox.Show("请输入关键字");
                return;
            }
            status = true;
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }


        public void creatVcf()

        {

            string text = method.GetTimeStamp() + ".vcf";
            if (File.Exists(text))
            {
                if (MessageBox.Show("“" + text + "”已经存在，是否删除它？", "确认", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return;
                }
                File.Delete(text);
            }
            UTF8Encoding encoding = new UTF8Encoding(false);
            StreamWriter streamWriter = new StreamWriter(text, false, encoding);
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                string name = listView1.Items[i].SubItems[1].Text.Trim();
                string tel = listView1.Items[i].SubItems[2].Text.Trim();
                if (name != "" && tel != "")
                {
                    streamWriter.WriteLine("BEGIN:VCARD");
                    streamWriter.WriteLine("VERSION:3.0");

                    streamWriter.WriteLine("N;CHARSET=UTF-8:" + name);
                    streamWriter.WriteLine("FN;CHARSET=UTF-8:" + name);

                    streamWriter.WriteLine("TEL;TYPE=CELL:" + tel);



                    streamWriter.WriteLine("END:VCARD");

                }
            }
            streamWriter.Flush();
            streamWriter.Close();
            MessageBox.Show("生成成功！文件名是：" + text);



        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                Thread thread= new Thread(creatVcf);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            status = false;
        }
    }
}
