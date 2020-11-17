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
    public partial class 数据去重 : Form
    {
        public 数据去重()
        {
            InitializeComponent();
        }
        Thread thread;
        DateTime start;
        private void button1_Click(object sender, EventArgs e)
        { 
           

                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(run);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
                label8.Text =DateTime.Now.ToString()+ "：开始执行";
           
        }
        string conn = "";
        private void 数据去重_FormClosing(object sender, FormClosingEventArgs e)
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
                        if (ctr.Name != "textBox6")
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
        }

        private void 数据去重_Load(object sender, EventArgs e)
        {
            foreach (Control ctr in groupBox1.Controls)
            {

                if (ctr is TextBox)
                {

                    string path = AppDomain.CurrentDomain.BaseDirectory + "data\\";
                    if (File.Exists(path + ctr.Name + ".txt"))
                    {
                        if (ctr.Name != "textBox6")
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
        }


        public void runsql(string connstr,string sqltxt)
        {
            try
            {
                MySqlConnection CON = new MySqlConnection(connstr);
                MySqlCommand command = new MySqlCommand();
                CON.Open();
                command.CommandText = sqltxt;
                command.Connection = CON;
                command.CommandType = CommandType.Text;
                command.CommandTimeout = 99999999;
                int num = command.ExecuteNonQuery();

                CON.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
          
           


        }

        public void run()
        {
            string biaoName = textBox3.Text.Replace("\r\n", "").Trim();
            start = DateTime.Now;
            StringBuilder sb1 = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            string[] value = textBox6.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < value.Length; i++)
            {
                sb1.Append(value[i]+",");
                sb2.Append("a."+ value[i] + "=b."+ value[i] + " and ");
            }
            string ziduan1 = sb1.ToString().Trim().Remove(sb1.ToString().Trim().Length - 1, 1);
            string ziduan2 = sb2.ToString().Trim().Remove(sb2.ToString().Trim().Length - 3, 3);

            conn = "Host =" + textBox1.Text.Replace("\r\n", "").Trim() + ";Database=" + textBox2.Text.Replace("\r\n", "").Trim() + ";Username=" + textBox4.Text.Replace("\r\n", "").Trim() + ";Password=" + textBox5.Text.Replace("\r\n", "").Trim();

            //string sqltxt = "CREATE TABLE " + biaoName + " SELECT * FROM wangzhe WHERE 1=2";


            string sqltxt = "delete from " + biaoName + " where id in (select a.id from (select max(id) id from " + biaoName + " a where EXISTS(select 1 from " + biaoName + " b where " + ziduan2 + " group by " + ziduan1 + " HAVING count(1)>1 )group by " + ziduan1 + " ) a)";
            
            runsql(conn,sqltxt);
           


           
            DateTime end = DateTime.Now;
            TimeSpan dt = end - start;
            label8.Text = "执行结束共用时：" + dt;
                
        }







    }
}
