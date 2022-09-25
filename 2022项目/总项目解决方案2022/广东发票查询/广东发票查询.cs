using System;
using System.Collections;
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

namespace 广东发票查询
{
    public partial class 广东发票查询 : Form
    {
        public 广东发票查询()
        {
            InitializeComponent();
        }


       

        private void 广东发票查询_Load(object sender, EventArgs e)
        {
            #region 通用检测


            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"dfkil"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            #endregion
            doSendMsg += Change;
            doSendMsg += SendMsgHander;//下载过程处理事件


            
        }


       






        private void SendMsgHander(DownMsg msg)
        {

            this.Invoke((MethodInvoker)delegate ()
            {
                string gmfmc = Regex.Match(msg.status, @"""gmfmc"":""([\s\S]*?)""").Groups[1].Value.Trim();
                string nsrmc = Regex.Match(msg.status, @"""nsrmc"":""([\s\S]*?)""").Groups[1].Value.Trim();
                listView1.Items[msg.Id].SubItems[2].Text = gmfmc;
                
                if(checkBox1.Checked==true)
                {
                    listView1.Items[msg.Id].SubItems[3].Text = nsrmc;
                }
               


                this.listView1.Items[msg.Id].EnsureVisible();
                Application.DoEvents();
            });

        }

        public delegate void dlgSendMsg(DownMsg msg);
        public event dlgSendMsg doSendMsg;


        public class DownMsg
        {
            public int Id;
            public string Tag;
            public string status;

        }

        private void Change(DownMsg msg)
        {
            //按下停止键
            if (status == false)
                return;

            if (msg.Tag == "end")
            {
                StartDown(1);
            }
        }


        List<Thread> list = new List<Thread>();

        bool status = true;
        public void run()
        {
            listView1.Items.Clear();
           ;
            if (textBox1.Text == "")
            {

                MessageBox.Show("请导入数据");
                return;

            }

            StreamReader sr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存


            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] != "")
                {

                    ListViewItem item = listView1.Items.Add(new ListViewItem(new string[] { (listView1.Items.Count + 1).ToString(), text[i], "准备中", "" }));
                    int id = item.Index;
                    AddDown(id, text[i]);
                }
            }
            StartDown(Convert.ToInt32(numericUpDown1.Value));//开始线程
        }

        public void AddDown(int id, string uid)
        {
            Thread tsk = new Thread(() =>
            {
                download(id, uid);
            });
            list.Add(tsk);
        }

        public void StartDown(int StartNum)
        {

            for (int i2 = 0; i2 < StartNum; i2++)
            {
                lock (list)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].ThreadState == System.Threading.ThreadState.Unstarted || list[i].ThreadState == ThreadState.Suspended)
                        {
                            list[i].Start();
                            break;
                        }
                    }
                }
            }

        }




        private void download(int id, string uid)
        {
            DownMsg msg = new DownMsg();
            try
            {

                msg.Id = id;
                try
                {



                    string[] text = uid.Split(new string[] { "," }, StringSplitOptions.None);

                    if(text.Length<5)
                    {
                        msg.status = "格式错误";
                        msg.Tag = "end";
                        doSendMsg(msg);
                        return;
                    }
                    string url = "https://etax.guangdong.chinatax.gov.cn/bsfwtweb/service/fpcy_getNsrFpcy";
                    string postdata = "sfqdfp=N&fpdm="+text[2].Trim()+ "&fphm=" + text[3].Trim() + "&nsrsbh=" + text[4].Trim() + "&hjjexx=888&spfsbh=&spfmc=&spje=&se=&kprq=&validCodeId=fpcyCode&FPLX=DZDZK";
                    string html = method.PostUrlDefault(url,postdata,"");
                    
                    msg.status = html;
                    msg.Tag = "end";
                    doSendMsg(msg);

                }
                catch (Exception ex)
                {
                    
                    msg.Tag = "end";
                    msg.status = ex.ToString();
                }


            }
            catch (Exception ex)
            {
              
                msg.Tag = "end";
                msg.status = ex.Message;
            }


        }


        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            status = true;

            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
            //run();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            // openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.Filter = "TXT文本文件|*.txt";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox1.Text = openFileDialog1.FileName;



            }
        }

        private void 广东发票查询_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();    
        }

        private void button4_Click(object sender, EventArgs e)
        {
           
            ListviewToTxt(listView1,1);
        }


        #region  listview导出文本TXT
        public  void ListviewToTxt(ListView listview, int i)
        {

            if (listview.Items.Count == 0)
            {
                MessageBox.Show("列表为空!");
            }

            List<string> list = new List<string>();

           


            foreach (ListViewItem item in listview.Items)
            {
                if (item.SubItems[i].Text.Trim() != "")
                {
                    if(checkBox1.Checked)
                    {
                        list.Add(item.SubItems[i].Text+ item.SubItems[i+1].Text+","+ item.SubItems[i+2].Text);
                    }
                    else
                    {
                        list.Add(item.SubItems[i].Text + item.SubItems[i + 1].Text);
                    }
                   
                }


            }
            SaveFileDialog sfd = new SaveFileDialog();

            // string path = AppDomain.CurrentDomain.BaseDirectory + "导出_" + Guid.NewGuid().ToString() + ".txt";
            string path = "";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                path = sfd.FileName + ".txt";
            }

            string[] Mystrings = list.ToArray();

            for (int a= 0; a < Mystrings.Count(); a++) //每个字符串都要参与比较
            {
                for (int j = 1; j < Mystrings.Count(); j++) //字符串长度较长的排在前面
                {
                    if (Mystrings[j - 1].Length < Mystrings[j].Length)
                    {
                        string temp = Mystrings[j - 1];
                        Mystrings[j - 1] = Mystrings[j];
                        Mystrings[j] = temp;
                    }
                }
            }


            StringBuilder sb = new StringBuilder();
            foreach (string tel in Mystrings)
            {
                sb.AppendLine(tel);
            }
            System.IO.File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
            MessageBox.Show("文件导出成功!文件地址:" + path);


        }






        #endregion




        


    private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            status = true;
            run();
        }
    }
}
