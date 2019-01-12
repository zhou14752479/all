using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fang
{
    public partial class Form8 : Form
    {
        bool status = true;
        bool zanting= true;
        int times=10;
        string yuming = "com";


        public Form8()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url)
        {
            try
            {

                            
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.81 Safari/537.36";
                request.AllowAutoRedirect = false;
                request.KeepAlive = false;
                request.Timeout = 1000;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  

                StreamReader reader = new StreamReader(response.GetResponseStream(),Encoding.GetEncoding("utf-8")); 

                string content = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return content;

            }
            catch (System.Exception ex)
            {
                ex.ToString();

            }
            return "";
        }
        #endregion


        /// <summary>
        /// 获取所有值
        /// </summary>
        /// <returns></returns>
        public ArrayList getListviewValue()

        {
            ArrayList values = new ArrayList(); 
            
            for (int i = 0; i < listView2.Items.Count; i++)
            {
                ListViewItem item = listView2.Items[i];
                for (int j = 0; j < item.SubItems.Count; j++)
                {
                   values.Add( item.SubItems[j].Text);
                    
                }


            }

            return values;

        }
        /// <summary>
        /// 获取第二列
        /// </summary>
        /// <returns></returns>
        public ArrayList getListviewValue1()

        {
            ArrayList values = new ArrayList();

            for (int i = 0; i < listView2.Items.Count; i++)
            {
                ListViewItem item = listView2.Items[i];
                
                 values.Add(item.SubItems[1].Text);
             

            }

            return values;

        }


        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public ArrayList splitStr(string a)
        {
            
            ArrayList list = new ArrayList();
            for (int i = 0; i < a.Length; i++)
            {
                list.Add(a.Substring(i, 1));

                
            }

            return list;
        }
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string CreateDirectory(string item)
        {

            string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

            if (!Directory.Exists(path + "\\data\\" + item))
            {
                Directory.CreateDirectory(path + "\\data\\" + item);
            }
            return path + "\\data\\" + item;
        }

        ArrayList finishes = new ArrayList();

        #region  域名筛选关键字
        public void run()

        {

            ArrayList lists= getListviewValue1();
            try

            {
                string start = "";
                string end = "";
                string zimu =null;
                string zimu1 = null;


                string[] array = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

                StringBuilder sb = new StringBuilder();
                foreach (char each in textBox2.Text)
                {
                    if (array.Contains(each.ToString().ToUpper()))
                    {
                        sb.Append(each);
                        
                    }
                }

                

                if (sb.ToString() != null && sb.ToString() !="")
                {
                    start = textBox2.Text.Replace(sb.ToString().ToLower(), "");
                    end = textBox3.Text.Replace(sb.ToString().ToLower(), "");
                }

                else
                {
                    start = textBox2.Text.Trim();
                    end = textBox3.Text.Trim();

                }

               
                if (array.Contains(textBox2.Text.Substring(0, 1).ToUpper()))
                {
                    zimu = sb.ToString();
                }

                else if (array.Contains(textBox2.Text.Substring(textBox2.Text.Length - 1, 1).ToUpper()))
                {
                    zimu1 = sb.ToString();
                }
                else
                {
                    zimu = sb.ToString();
                }
               //MessageBox.Show(start + "#" + end + "#" + zimu + "#" + zimu1);

                for (long i = Convert.ToInt64(start.Trim()); i < Convert.ToInt64(end.Trim()); i++)
                {

                    string url = "http://www."+ zimu+ i +zimu1+"." + this.yuming;

                    if (!finishes.Contains(url))
                    {
                        finishes.Add(url);

                        if (zimu != null)
                        {
                            toolStripLabel5.Text = zimu.ToLower() + i.ToString();
                        }

                        else if (zimu1 != null)
                        {
                            toolStripLabel5.Text = i.ToString()+zimu1.ToLower() ;

                        }
                        string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

                       
                        string html = GetUrl(url);

                        Match title = Regex.Match(html, @"<title>([\s\S]*?)</title>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        for (int j = 0; j < lists.Count; j++)
                        {


                            if (lists[j].ToString() != "")
                            {


                                while (this.zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }

                                if (this.status == false)
                                    return;


                                    if(html.Contains(lists[j].ToString()))
                                    {
                                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                                        lv1.SubItems.Add(url);
                                        lv1.SubItems.Add(title.Groups[1].Value.Trim());
                                        lv1.SubItems.Add(lists[j].ToString());                                 
                                        break;

                                    }
                            
                            }

                            if (listView1.Items.Count - 1 > 0)
                            {
                                listView1.EnsureVisible(listView1.Items.Count - 1);
                            }
                           

                        }
                    }
                }
            }

            catch (Exception ex)
            {
             ex.ToString();
            }
        }

        #endregion

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            this.times= trackBar1.Value;
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            this.zanting = false;
        }

        private void skinButton5_Click(object sender, EventArgs e)
        {
            this.zanting = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.yuming=comboBox1.SelectedItem.ToString();
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            this.status = true;
            
            if (listView2.Items.Count<1)
            {

                MessageBox.Show("请先添加关键词");
                return;
            }
            if (textBox2.Text == "" || textBox3.Text == "")
            {

                MessageBox.Show("请设置域名段，左小右大");
                return;
            }
            //Task t1 = Task.Factory.StartNew(delegate { run(); });
            //Task t2 = Task.Factory.StartNew(delegate { run(); });
            //Task t3 = Task.Factory.StartNew(delegate { run(); });
            //Task t4 = Task.Factory.StartNew(delegate { run(); });
            //Task t5 = Task.Factory.StartNew(delegate { run(); });
            //Task t6 = Task.Factory.StartNew(delegate { run(); });

            //Task.WaitAll(t1, t2, t3, t4, t5, t6);

            //Thread thread = new Thread(new ThreadStart(run));          
            //thread.Start();



            for (int i = 0; i <60; i++)
            {
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
            }


            timer1.Start();
        }
     
        private void skinButton1_Click(object sender, EventArgs e)
        {
           

            ListViewItem lv2 = listView2.Items.Add(listView2.Items.Count.ToString());
            lv2.SubItems.Add(textBox1.Text);
            string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

            StreamWriter sw = File.AppendText(path + "//keywords.txt");
            sw.WriteLine(textBox1.Text);
     
            sw.Flush();
            sw.Close();
            textBox1.Text = "";

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripLabel7.Text = (Convert.ToInt32(toolStripLabel7.Text)+1).ToString();
        }

        private void skinButton4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
                 
            
        }

        private void skinButton6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹
            File.WriteAllText(path + "//datas.txt", string.Empty);
        }

        private void Form8_Load(object sender, EventArgs e)
        {

            string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹
            if (File.Exists(path + "//keywords.txt"))
            {

                StreamReader sr = new StreamReader(path + "//keywords.txt");
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length; i++)
                {
                    ListViewItem item = listView2.Items.Add(listView2.Items.Count.ToString());

                    item.SubItems.Add(text[i]);


                }
                sr.Close();
            }





            if (File.Exists(path + "//datas.txt"))
            {

                StreamReader sr = new StreamReader(path + "//datas.txt");
                //一次性读取完 
                string datas = sr.ReadToEnd();
                string[] data = datas.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < data.Length-1; i++)
                {
                    string[] each = data[i].Split(new string[] { "#" }, StringSplitOptions.None);

                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString());
                    lv1.SubItems.Add(each[0]);
                    lv1.SubItems.Add(each[1]);
                    lv1.SubItems.Add(each[2]);

                }
                sr.Close();
            }



            if (File.Exists(path + "//id1.txt"))
            {

                StreamReader sr1 = new StreamReader(path + "\\id1.txt");
                textBox2.Text = sr1.ReadToEnd();

                sr1.Close();
            }

            if (File.Exists(path + "//id2.txt"))
            {
                StreamReader sr2 = new StreamReader(path + "\\id2.txt");
                textBox3.Text = sr2.ReadToEnd();
                sr2.Close();

            }
        }

        private void 删除改项ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // MessageBox.Show(this.listView2.SelectedItems[0].SubItems[1].Text);   //选中行，第二列，如果是SubItems[0]就是第一列

            this.listView2.Items.Remove(this.listView2.SelectedItems[0]);

            string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

            File.WriteAllText(path + "//keywords.txt", string.Empty);
            for (int i = 0; i < listView2.Items.Count; i++)
            {
                StreamWriter sw = File.AppendText(path + "//keywords.txt");
                sw.WriteLine(listView2.Items[i].SubItems[1].Text);

                sw.Flush();
                sw.Close();
            }


        }

        private void 删除所有ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.listView2.Items.Clear();
            string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹
        
            File.WriteAllText(path + "//keywords.txt", string.Empty);
                        
        }

        private void skinButton7_Click(object sender, EventArgs e)
        {
            this.status = false;
        }

        private void Form8_FormClosed(object sender, FormClosedEventArgs e)
        {
            string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹

            FileStream fs1 = new FileStream(path + "//id1.txt", FileMode.Create, FileAccess.Write);//在当前程序运行文件夹内创建文件 
            StreamWriter sw = new StreamWriter(fs1);
            sw.WriteLine(toolStripLabel5.Text.Trim());//开始写入值
            sw.Close();
            fs1.Close();



            

            FileStream fs2 = new FileStream(path + "//id2.txt", FileMode.Create, FileAccess.Write);//在当前程序运行文件夹内创建文件 
            StreamWriter sw2 = new StreamWriter(fs2);
            sw2.WriteLine(textBox3.Text.Trim());//开始写入值
            sw2.Close();
            fs2.Close();



            for (int i = 0; i < listView1.Items.Count; i++)
            {
                StreamWriter sw3= File.AppendText(path + "//datas.txt");
                sw3.WriteLine(listView1.Items[i].SubItems[1].Text+"#"+ listView1.Items[i].SubItems[2].Text+"#"+ listView1.Items[i].SubItems[3].Text+"\r");

                sw3.Flush();
                sw3.Close();
            }

        }
    }
}
