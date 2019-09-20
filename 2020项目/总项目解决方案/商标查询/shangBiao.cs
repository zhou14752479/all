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

namespace 商标查询
{
    public partial class shangBiao : Form
    {
        public shangBiao()
        {
            InitializeComponent();
        }

        bool zanting = true;
        #region  主程序
        public void run()
        {
            try
            
{
            //    StreamReader sr = new StreamReader(textBox1.Text, Encoding.Default);
            //    //一次性读取完 
            //    string texts = sr.ReadToEnd();
            //    string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            //    for (int a = 0; a < text.Length - 1; a++)
            //    {

                    string url = "https://api.ipr.kuaifawu.com/xcx/tmsearch/index";

                    string postdata = "{" +
                        "\"openid\" : \"o3GDw0LyLEu85v0bQkk1Z8TpURxc\"," +
                        "\"cls\" : \"\"," +
                        "\"mode\" : \"3\"," +
                        "\"appdate\" : \"\"," +
                        "\"key\" : \"37380438\"," +
                        "\"page\" : 1," +
                        "\"state\" : \"\"," +
                        "}";
                      

                        string html =method.PostUrl(url,postdata,"","utf-8");

                    textBox2.Text = html;
                        for (int j = 0; j < 10; j++)
                        {
                           

                          
                               
                                    //ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据      
                                    //lv1.SubItems.Add(Names[j].Groups[1].Value.Trim());
                                    //lv1.SubItems.Add(prices[j].Groups[1].Value.Trim());
                                    //lv1.SubItems.Add(comments[j].Groups[1].Value.Trim());
                                    //lv1.SubItems.Add(catids[j].Groups[1].Value.Trim());
                                    //lv1.SubItems.Add("https://item.jd.com/" + uids[j].Groups[1].Value + ".html");
                                    //while (this.zanting == false)
                                    //{
                                    //    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                    //}
                            
                       

                    }

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion
        private void Button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
