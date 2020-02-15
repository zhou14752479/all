using System;
using System.Collections;
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

namespace main._2019_4
{
    public partial class crm58 : Form
    {
        public crm58()
        {
            InitializeComponent();
        }

     
        /// <summary>
        /// 获取标签未用到
        /// </summary>
        /// <returns></returns>
        public  void getTags()
        {
            string html = method.GetUrlWithCookie("http://s.crm.58.com/OppTag/GetFrequentTags", textBox1.Text.Trim(), "utf-8");

            MatchCollection tags = Regex.Matches(html, @"tagName"":""([\s\S]*?)""");

            ArrayList lists = new ArrayList();
            foreach (Match tag in tags)
            {
                lists.Add(tag.Groups[1].Value);
            }
            
            
        }



        #region  crm58库

        public void run()
        {
            textBox2.Text += DateTime.Now.ToString() + "程序启动...." + "\r\n";
            try
            {
                //                string[] buIds = { "0089B90C-1029-E311-93F6-D4AE5278BCF7",
                //"0114228B-C1B5-4515-B3A3-4FA851D5A5E2",
                //"042D02F7-7EF5-4D76-8EC4-E2381D20FBAF",
                //"05196842-9396-E411-93FA-D4AE5278BCF7",
                //"0E7A67C4-4403-46E2-A5AF-3E871B40060E",
                //"0F4C84DD-9E72-427D-AC07-7152E29AD78C",
                //"1DE4F4C4-DF0F-43C0-AA52-2EA87FE6F8F9",
                //"2B8624FA-FF86-4A3C-9694-787433D3A1F5",
                //"2BBD985C-EC99-44DE-ABBE-DB76529168FE",
                //"2F53D58F-2B7F-46C3-A9B0-B8367750DC9A",
                //"335A1ACA-2586-412D-9C73-826E43555B38",
                //"33937C30-B3A4-4FED-84FB-611A205A4B1A",
                //"39244C59-9543-4354-BE3B-C3DA99435ACE",
                //"3E8B61BD-8D6C-4A3F-B3EE-7C9CF7D6EEB3",
                //"497BD758-6452-485E-A0BE-4C1A48125F29",
                //"4B4E7C88-CCED-4B10-91FA-9B9CE0971851",
                //"4CD0A1C0-DF33-467C-B447-5B16875CB957",
                //"5756E8E7-AF8D-406F-8041-41758A8F92E2",
                //"59B0BC45-921C-453B-B593-FA3E4743F3C3",
                //"62222DD6-979E-47EA-BB65-DF903AA26226",
                //"682A3977-C7D1-440A-8DE3-B22F7B0ADA3F",
                //"6CF2FEDB-2C61-4FAD-B46C-B5E72BB12AC7",
                //"703B76C8-4EEE-49D9-BD91-57EC83027BCE",
                //"76E98177-3A93-4737-B081-2828CFAEB3A8",
                //"78f915a0-4c56-4d89-b6bf-861537958036",
                //"792EF071-391D-43AA-8EC2-8CB0C6BAC42F",
                //"7A1B2124-4BB4-49CB-996C-E2A0F63CED42",
                //"7A67BA06-1029-E311-93F6-D4AE5278BCF7",
                //"7A9D810F-7719-4380-9ABE-545011849FD5",
                //"7D8A6FF2-499B-E411-93FA-D4AE5278BCF7",
                //"7DAA9130-39F7-4B6B-8883-F94F2AFEBD94",
                //"7E67BA06-1029-E311-93F6-D4AE5278BCF7",
                //"808D878C-D9BB-4B8C-8D5B-BAA44E7DA590",
                //"8667BA06-1029-E311-93F6-D4AE5278BCF7",
                //"8A67BA06-1029-E311-93F6-D4AE5278BCF7",
                //"966FC438-9396-E411-93FA-D4AE5278BCF7",
                //"976FB6BF-F41A-42C8-8B51-A69893417FF6",
                //"A5EBA771-99B9-4FC1-8CE7-50115BB2BDD5",
                //"A8893381-76EA-43F7-928F-9725DB3F6179",
                //"AD79E251-1FC7-4FFF-961B-7196DD0A04B4",
                //"B868E3C9-69BE-4B52-BE04-4B49277BEB1A",
                //"bdca95b8-a2cb-4e29-9aad-8319c930f009",
                //"CFA4BA57-A5AC-4442-9819-3C8985666C7F",
                //"D2383DD6-F9C2-4DF6-85AE-28C64D236576",
                //"D4785F57-CF8B-4F0B-8668-12BB6390E4F6",
                //"D8F441FF-799D-41AD-9791-D5E5583B0EDB",
                //};

                //foreach (string buId in buIds)
                //{
                string buId = textBox3.Text.Trim() ;

                    string Url = "http://crm.58.com/Release/GetTodayWillReleaseOppFromEs?buId=" + buId + "&userCity=%E5%8C%97%E4%BA%AC";

                    string html = method.GetUrlWithCookie(Url, textBox1.Text.Trim(), "utf-8");


                    if (!html.Contains("盾"))
                    {
                       
                        MatchCollection titles = Regex.Matches(html, @"CustomerName"":""([\s\S]*?)""");
                        MatchCollection owners = Regex.Matches(html, @"OwnerName"":""([\s\S]*?)""");
                        MatchCollection types = Regex.Matches(html, @"LibType"":""([\s\S]*?)""");
                        MatchCollection OppIds = Regex.Matches(html, @"OppId"":""([\s\S]*?)""");

                        for (int i = 0; i < titles.Count; i++)
                        {
                            if (types[i].Groups[1].Value.Contains("私有"))
                            {

                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(titles[i].Groups[1].Value);
                                lv1.SubItems.Add(owners[i].Groups[1].Value);
                                lv1.SubItems.Add(types[i].Groups[1].Value);
                                lv1.SubItems.Add(OppIds[i].Groups[1].Value);


                                listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                            }
                            else
                            {

                                textBox2.Text += titles[i].Groups[1].Value + "不是私有忽略" + "\r\n";
                                this.textBox2.SelectionStart = this.textBox2.Text.Length;
                                this.textBox2.SelectionLength = 0;
                                this.textBox2.ScrollToCaret();
                            }

                        }


                        Thread.Sleep(3000);   //内容获取间隔，可变量        
                    }

                    else 
                    {

                        MessageBox.Show("登录失效！");
                        return;
                    }



                //}

                //textBox2.Text += DateTime.Now.ToString() + "已完成获取释放预告，请添加到标签" + "\r\n";
            }







            catch (System.Exception ex)
            {

                textBox2.Text = ex.ToString();
            }

        }

        #endregion
        private void crm58_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            textBox2.Text = "";
            Thread thread = new Thread(new ThreadStart(run));
            Control.CheckForIllegalCrossThreadCalls = false;
            thread.Start();
        }

        private void 添加到标签ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string tagname= DateTime.Now.ToString("yyyy-MM-dd");
            ListViewItem lvi = listView1.SelectedItems[0];
            string url = "http://s.crm.58.com/oppTag/addTag";
            string postData = "oppid="+ lvi.SubItems[4].Text + "&source=%E4%BB%8A%E6%97%A5%E9%87%8A%E6%94%BE%E9%A2%84%E5%91%8A&tagName="+ tagname;
            
            
            textBox2.Text= method.PostUrl(url, postData, textBox1.Text.Trim(), "utf-8");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string status = "";
            foreach (ListViewItem item in listView1.Items)
            {
                string tagname = DateTime.Now.ToString("yyyy-MM-dd");
                
                string url = "http://s.crm.58.com/oppTag/addTag";
                string postData = "oppid=" + item.SubItems[4].Text + "&source=%E4%BB%8A%E6%97%A5%E9%87%8A%E6%94%BE%E9%A2%84%E5%91%8A&tagName=" + tagname;
                 status = method.PostUrl(url, postData, textBox1.Text.Trim(), "utf-8");

            }

            if (status != "")
            {
                MessageBox.Show("添加完成");
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Browser web = new Browser("http://crm.58.com/");
            web.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            textBox2.Text = "";
        }
    }
}
