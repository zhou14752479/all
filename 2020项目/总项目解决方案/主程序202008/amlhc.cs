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
using helper;

namespace 主程序202008
{
    public partial class amlhc : Form
    {
        public amlhc()
        {
            InitializeComponent();
            dic.Add(1, "鼠");
            dic.Add(2, "猪");
            dic.Add(3, "狗");
            dic.Add(4, "鸡");
            dic.Add(5, "猴");
            dic.Add(6, "羊");
            dic.Add(7, "马");
            dic.Add(8, "蛇");
            dic.Add(9, "龙");
            dic.Add(10, "兔");
            dic.Add(11, "虎");
           



            for (int i= 12; i<=49; i++)
            {
            
                    switch (i % 12)
                    {
                        case 0:
                            dic.Add(i,"牛");
                            break;
                    }
                    switch (i % 12)
                    {
                        case 1:
                            dic.Add(i, "鼠");
                            break;
                    }
                    switch (i % 12)
                    {
                        case 2:
                            dic.Add(i, "猪");
                            break;
                    }
                    switch (i % 12)
                    {
                        case 3:
                            dic.Add(i, "狗");
                            break;
                    }
                    switch (i % 12)
                    {
                        case 4:
                            dic.Add(i, "鸡");
                            break;
                    }
                    switch (i % 12)
                    {
                        case 5:
                            dic.Add(i, "猴");
                            break;
                    }
                    switch (i % 12)
                    {
                        case 6:
                            dic.Add(i, "羊");
                            break;
                    }
                    switch (i % 12)
                    {
                        case 7:
                            dic.Add(i, "马");
                            break;
                    }
                    switch (i % 12)
                    {
                        case 8:
                            dic.Add(i, "蛇");
                            break;
                    }
                    switch (i % 12)
                    {
                        case 9:
                            dic.Add(i, "龙");
                            break;
                    }
                    switch (i % 12)
                    {
                        case 10:
                            dic.Add(i, "兔");
                            break;
                    }
                    switch (i % 12)
                    {
                        case 11:
                            dic.Add(i, "虎");
                            break;
                    }

                

            }
        }
        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl(string url, string postData)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.ContentType = "application/json";

                WebHeaderCollection headers = request.Headers;
                

                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", "");

                request.Referer = "";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string html = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return html;
            }
            catch (WebException ex)
            {

                return ex.ToString();
            }


        }

        #endregion


        string[] shengxiaos = { "鼠" ,"牛 " ,"虎" ,"兔" ,"龙" ,"蛇" ,"马" ,"羊" ,"猴" ,"鸡" ,"狗" ,"猪 " };
        Dictionary<int, string> dic = new Dictionary<int, string>();
      

        ArrayList lists = new ArrayList();


        public void duqu()
        {
            StreamReader sr = new StreamReader(path + "data.txt", Encoding.GetEncoding("utf-8"));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);


            ListViewItem lv1 = listView1.Items.Add("平码1");
            ListViewItem lv2 =  listView1.Items.Add("平码2");
            ListViewItem lv3 = listView1.Items.Add("平码3");
            ListViewItem lv4 = listView1.Items.Add("平码4");
            ListViewItem lv5 = listView1.Items.Add("平码5");
            ListViewItem lv6 = listView1.Items.Add("平码6");

           

            listView1.Columns.Add("组名");
            for (int i = 0; i < text.Length; i++)
            {
                listView1.Columns.Add( (i + 1).ToString());
                listView1.Columns[i].Width = 40;
                lv1.SubItems.Add(text[i].Substring(0, 1));
                lv2.SubItems.Add(text[i].Substring(1, 1));
                lv3.SubItems.Add(text[i].Substring(2, 1));
                lv4.SubItems.Add(text[i].Substring(3, 1));
                lv5.SubItems.Add(text[i].Substring(4, 1));
                lv6.SubItems.Add(text[i].Substring(5, 1));

                label1.Text = "正在载入第"+(i+1)+"组";

                while (this.zanting == false)
                {
                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                }

                if (i == text.Length - 1)
                {
                    MessageBox.Show("载入完成");
                }


            }
            
        }
        int c = 5;
        private void 查看ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            ListViewItem lv1 = listView1.Items.Add("  ");
            for (int i = 0; i < listView1.Columns.Count; i++)
            {
                lv1.SubItems.Add(" 1");
            }
            c = c + 1;


                int a = this.dataGridView1.CurrentRow.Index;
            int j = this.dataGridView1.CurrentCell.ColumnIndex;
            if (this.dataGridView1.CurrentRow.Cells[j].Value != null)
            {

                string value = this.dataGridView1.CurrentRow.Cells[j].Value.ToString();
                value = Regex.Replace(value, @"\d{1,}", "").Trim();

                if (value != "")
                {

                    for (j = 1; j < listView1.Columns.Count; j++)
                    {
                        for (int i = 0; i < 6; i++)
                        {

                        
                            
                            if (listView1.Items[i].SubItems[j].Text.Trim() == value)
                            {
                                listView1.Items[c].UseItemStyleForSubItems = false;
                                listView1.Items[c].SubItems[j].Text = value+"√";
                                listView1.Items[c].SubItems[j].ForeColor = Color.Red;
                                break;

                            }
                            else
                            {
                                listView1.Items[c].SubItems[j].Text = value + "×";
                                

                            }

                        }
                    }


                }
            }

        }

        /// <summary>
        /// 获取时间戳  秒
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalSeconds);
            return a.ToString();
        }

        int index;
        string path = AppDomain.CurrentDomain.BaseDirectory+"\\data\\";
        private void amlhc_Load(object sender, EventArgs e)
        {

            Thread thread = new Thread(new ThreadStart(duqu));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;





            foreach (Control ctr in groupBox1.Controls)
            {

                if (ctr is TextBox)
                {

                    string path = AppDomain.CurrentDomain.BaseDirectory + "data\\";
                    if (File.Exists(path + ctr.Name + ".txt"))
                    {

                        StreamReader sr11 = new StreamReader(path + ctr.Name + ".txt", Encoding.GetEncoding("utf-8"));
                        //一次性读取完 
                        string texts11 = sr11.ReadToEnd();
                        ctr.Text = texts11;
                        sr11.Close();
                    }
                }
            }





           


            StreamReader sr1 = new StreamReader(path + "luru.txt", Encoding.GetEncoding("utf-8"));
            //一次性读取完 
            string texts1 = sr1.ReadToEnd();

            string[] text1 = texts1.Split(new string[] { "\r\n" }, StringSplitOptions.None);


            for (int i = 0; i < text1.Length; i++)
            {
                if (text1[i].Trim() == "")
                {
                    sr1.Close();
                    return;
                }

                string[] value = text1[i].Split(new string[] { "#" }, StringSplitOptions.None);


                this.index = this.dataGridView1.Rows.Add();

                dataGridView1.Rows[index].Cells[0].Value = value[0];
                dataGridView1.Rows[index].Cells[1].Value = value[1];
                dataGridView1.Rows[index].Cells[2].Value = value[2];
                dataGridView1.Rows[index].Cells[3].Value = value[3];
                dataGridView1.Rows[index].Cells[4].Value = value[4];
                dataGridView1.Rows[index].Cells[5].Value = value[5];
                dataGridView1.Rows[index].Cells[6].Value = value[6];
                dataGridView1.Rows[index].Cells[7].Value = value[7];
                dataGridView1.Rows[index].Cells[8].Value = value[8];
                dataGridView1.Rows[index].Cells[9].Value = value[9];

            }
            sr1.Close();


        }
        bool zanting = true;
        private void button1_Click(object sender, EventArgs e)
        {
            zanting = false;



        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

      

       

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string a1 = textBox1.Text.Trim();
            string a2 = textBox2.Text.Trim() + dic[Convert.ToInt32(textBox2.Text.Trim())];
            string a3 = textBox3.Text.Trim() + dic[Convert.ToInt32(textBox3.Text.Trim())];
            string a4 = textBox4.Text.Trim() + dic[Convert.ToInt32(textBox4.Text.Trim())];
            string a5 = textBox5.Text.Trim() + dic[Convert.ToInt32(textBox5.Text.Trim())];
            string a6 = textBox6.Text.Trim() + dic[Convert.ToInt32(textBox6.Text.Trim())];
            string a7 = textBox7.Text.Trim() + dic[Convert.ToInt32(textBox7.Text.Trim())];
            string a8 = textBox8.Text.Trim() + dic[Convert.ToInt32(textBox8.Text.Trim())];
            string a9 = textBox9.Text.Trim();
            string a10 = textBox10.Text.Trim();


            this.index = this.dataGridView1.Rows.Add();

            dataGridView1.Rows[index].Cells[0].Value = a1;
            dataGridView1.Rows[index].Cells[1].Value = a2;
            dataGridView1.Rows[index].Cells[2].Value = a3;
            dataGridView1.Rows[index].Cells[3].Value = a4;
            dataGridView1.Rows[index].Cells[4].Value = a5;
            dataGridView1.Rows[index].Cells[5].Value = a6;
            dataGridView1.Rows[index].Cells[6].Value = a7;
            dataGridView1.Rows[index].Cells[7].Value = a8;
            dataGridView1.Rows[index].Cells[8].Value = a9;
            dataGridView1.Rows[index].Cells[9].Value = a10;


           
           

        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

       
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {


            


        }
       
      

        private void amlhc_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("确认退出吗？", "退出询问"
          , MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result != DialogResult.OK)
            {
                e.Cancel = true;//告诉窗体关闭这个任务取消

            }
            else
            {
                foreach (Control ctr in groupBox1.Controls)
                {
                    if (ctr is TextBox)
                    {


                        string path = AppDomain.CurrentDomain.BaseDirectory + "data\\";
                        FileStream fs1 = new FileStream(path + ctr.Name + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                        StreamWriter sw = new StreamWriter(fs1);
                        sw.WriteLine(ctr.Text);
                        sw.Close();
                        fs1.Close();

                    }
                }
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (dataGridView1.Rows[i].Cells[0].Value != null )
                    {
                        string a1 = dataGridView1.Rows[i].Cells[0].Value.ToString();
                        string a2 = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        string a3 = dataGridView1.Rows[i].Cells[2].Value.ToString();
                        string a4 = dataGridView1.Rows[i].Cells[3].Value.ToString();
                        string a5 = dataGridView1.Rows[i].Cells[4].Value.ToString();
                        string a6 = dataGridView1.Rows[i].Cells[5].Value.ToString();
                        string a7 = dataGridView1.Rows[i].Cells[6].Value.ToString();
                        string a8 = dataGridView1.Rows[i].Cells[7].Value.ToString();
                        string a9 = dataGridView1.Rows[i].Cells[8].Value.ToString();
                        string a10 = dataGridView1.Rows[i].Cells[9].Value.ToString();

                        sb.Append(a1 + "#" + a2 + "#" + a3 + "#" + a4 + "#" + a5 + "#" + a6 + "#" + a7 + "#" + a8 + "#" + a9 + "#" + a10+"\r\n");
                    }
                }


                FileStream fs2 = new FileStream(path + "luru.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                StreamWriter sw2 = new StreamWriter(fs2);
                sw2.WriteLine(sb.ToString());
                sw2.Close();
                fs2.Close();




                Environment.Exit(0);

            }
        }
    }
}
