using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序1
{
    public partial class 收取5173 : Form
    {
        public 收取5173()
        {
            InitializeComponent();
        }

      
        string conn = "";

        #region 绑定数据
        public void getdata()
        {
            
            MySqlDataAdapter sda = new MySqlDataAdapter("Select infos from data ORDER BY date DESC", conn);
            DataSet Ds = new DataSet();
            sda.Fill(Ds, "T_Class");

            this.dataGridView1.DataSource = Ds.Tables["T_Class"];
           
        }

        #endregion

       

        public void clearData()
        {
            MySqlConnection mycon = new MySqlConnection(conn);
            mycon.Open();



            MySqlCommand cmd = new MySqlCommand("delete from data", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


            int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
            if (count > 0)
            {


                mycon.Close();

            }
            else
            {

                mycon.Close();
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            getdata();
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)//得到总行数并在之内循环  
            {

                dataGridView1.Rows[i].Cells[0].Value = System.Web.HttpUtility.UrlDecode(dataGridView1.Rows[i].Cells[0].Value.ToString());//定位到相同的单元格  

            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            clearData();
        }

        private void 收取5173_Load(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            StreamReader sr = new StreamReader(path + "ip.txt", Encoding.Default);
            //一次性读取完 
            string ip = sr.ReadToEnd().Trim();
            

            conn = "Host =" + ip + ";Database=jiagejiankong;Username=jiagejiankong;Password=rsFFARtWZ27jWPhz";
        }
    }
}
