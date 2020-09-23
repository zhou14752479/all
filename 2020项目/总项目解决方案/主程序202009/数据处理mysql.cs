using MySql.Data.MySqlClient;
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

namespace 主程序202009
{
    public partial class 数据处理mysql : Form
    {
        public 数据处理mysql()
        {
            InitializeComponent();
        }
        string conn = "";
        private void 数据处理mysql_FormClosing(object sender, FormClosingEventArgs e)
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
            }

        }

        private void 数据处理mysql_Load(object sender, EventArgs e)
        {
           
            foreach (Control ctr in groupBox1.Controls)
            {

                if (ctr is TextBox)
                {

                    string path = AppDomain.CurrentDomain.BaseDirectory + "data\\";
                    if (File.Exists(path + ctr.Name + ".txt"))
                    {

                        StreamReader sr1 = new StreamReader(path + ctr.Name + ".txt", Encoding.GetEncoding("utf-8"));
                        //一次性读取完 
                        string texts1 = sr1.ReadToEnd();
                        ctr.Text = texts1;
                        sr1.Close();
                    }
                }
            }
        }

        bool zanting = true;


        public string insert(string a,string b,string c,string d)
        {
            try
            {
                this.conn = "Host =" + textBox1.Text.Replace("\r\n", "").Trim() + ";Database=" + textBox2.Text.Replace("\r\n", "").Trim() + ";Username=" + textBox5.Text.Replace("\r\n", "").Trim() + ";Password=" + textBox6.Text.Replace("\r\n", "").Trim();
                MySqlConnection mycon = new MySqlConnection(conn);
                mycon.Open();



                MySqlCommand cmd = new MySqlCommand("INSERT INTO " + textBox4.Text.Replace("\r\n", "").Trim() + " (标题,内容,链接,关键词)VALUES('" + a + " ', '" + b + " ', '" + c + " ', '" + d + " ')", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {
                    mycon.Close();
                    return ("插入成功！");



                }
                else
                {
                    mycon.Close();
                    return ("插入失败！");
                }
            }
            catch (Exception)
            {
               
                return ""  ;
            }
           
        }

        public string chuli(string neirong)

        {
            neirong = neirong.Replace("1)","").Replace("2)", "").Replace("3)", "").Replace("4)", "").Replace("5)", "").Replace("6)", "").Replace("7)", "").Replace("8)", "").Replace("9)", "").Replace("10)", "").Replace("11)", "").Replace("12)", "");
            return neirong;
        }
        public void run()
        {
            int num = 0;
            this.conn = "Host =" + textBox1.Text.Replace("\r\n", "").Trim() + ";Database=" + textBox2.Text.Replace("\r\n", "").Trim() + ";Username=" + textBox5.Text.Replace("\r\n", "").Trim() + ";Password=" + textBox6.Text.Replace("\r\n", "").Trim();
          
            string sql = "Select * From " + textBox3.Text.Replace("\r\n", "").Trim() + "";
            
            MySqlDataAdapter sda = new MySqlDataAdapter(sql, conn);
            DataSet Ds = new DataSet();
            sda.Fill(Ds, "T_Class");
          
            foreach (DataRow myRow in Ds.Tables["T_Class"].Rows)
            {
                num = num + 1;
                label7.Text = "共"+ Ds.Tables["T_Class"].Rows.Count+"条，已处理："+num;
                string biaoti = myRow[1].ToString();
                string neirongs = myRow[2].ToString();
                string link = myRow[3].ToString();
                string keys = myRow[4].ToString();

                string[] neirong = neirongs.Split(new string[] { "</p> (" }, StringSplitOptions.None);
             
                if (neirong.Length ==1)
                {
                    neirongs = neirongs.Replace("(1)", "");
                    textBox7.Text += insert(biaoti,neirongs,link,keys) + "\r\n";
                }
                else
                {
                    for (int i = 0; i < neirong.Length; i++)
                    {
                        if (i == 0)
                        {
                            neirong[i] = neirong[i].Replace("(1)","") + "</p>";
                        }
                        else if (i == neirong.Length - 1)
                        {
                            neirong[i] = chuli(neirong[i]);
                        }
                        else
                        {
                            neirong[i] = chuli(neirong[i]);
                        }

                        textBox7.Text +=DateTime.Now.ToString()+"："+ insert(biaoti, neirong[i], link, keys) + "\r\n";
                    }
                }
                while (this.zanting == false)
                {
                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                }
            }
            MessageBox.Show("处理完成");
           
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }
    }
}
