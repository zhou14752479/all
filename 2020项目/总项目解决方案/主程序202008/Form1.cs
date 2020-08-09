using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace 主程序202008
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        bool zanting = true;
        string constr = "Host =111.229.244.97;Database=fang;Username=root;Password=root";

        public void chaxun()
        {

            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();
            MySqlCommand cmd = new MySqlCommand("select * from datas", mycon);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())//调用while语句，读取SqlDataReader
            {

                string room = sdr[3].ToString().Replace("房", "").Trim();
                room = room == "" ? "0" : room.Trim();

                string jiage = sdr[4].ToString().Replace("元", "").Trim();
                jiage = jiage == "" ? "0" : jiage.Trim();


                string louceng = sdr[5].ToString().Replace("楼", "").Trim();
                louceng = louceng == "" ? "0" : louceng.Trim();

                string mianji = sdr[6].ToString().Replace("平", "").Trim();
                mianji = mianji == "" ? "0" : mianji.Trim();

                try
                {

                    if (Convert.ToInt32(textBox1.Text) <= Convert.ToInt32(room) && Convert.ToInt32(room) <= Convert.ToInt32(textBox2.Text))

                    {
                        if (Convert.ToInt32(textBox4.Text) <= Convert.ToInt32(jiage) && Convert.ToInt32(jiage) <= Convert.ToInt32(textBox3.Text))

                        {

                            if (Convert.ToInt32(textBox6.Text) <= Convert.ToInt32(louceng) && Convert.ToInt32(louceng) <= Convert.ToInt32(textBox5.Text))

                            {
                                if (Convert.ToInt32(textBox10.Text) <= Convert.ToInt32(mianji) && Convert.ToInt32(mianji) <= Convert.ToInt32(textBox9.Text))

                                {
                                    ListViewItem lv1 = listView1.Items.Add(sdr[0].ToString());

                                    lv1.SubItems.Add(sdr[1].ToString()); //数据库第二列
                                    lv1.SubItems.Add(sdr[2].ToString());
                                    lv1.SubItems.Add(sdr[3].ToString());
                                    lv1.SubItems.Add(sdr[4].ToString());
                                    lv1.SubItems.Add(sdr[5].ToString());
                                    lv1.SubItems.Add(sdr[6].ToString());
                                    lv1.SubItems.Add(sdr[7].ToString());
                                    lv1.SubItems.Add(sdr[8].ToString());
                                    lv1.SubItems.Add(sdr[9].ToString());
                                    lv1.SubItems.Add("  ");
                                    while (this.zanting == false)
                                    {
                                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                    }
                                }
                            }
                        }
                    }


                }
                catch 
                {

                    ;
                }

            }
            mycon.Close(); //释放连接

        }



        #region  插入数据

        public void insertData(string sql)
        {

            try
            {

                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                

                MySqlCommand cmd = new MySqlCommand(sql, mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'
                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {
                    //成功
                    mycon.Close();
                }
                else
                {
                    //失败
                    mycon.Close();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }
        #endregion

        #region  更新标签

        public void updateData(string sql)
        {

            try
            {

                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();



                MySqlCommand cmd = new MySqlCommand(sql, mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'
                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {
                    //成功
                    mycon.Close();
                }
                else
                {
                    //失败
                    mycon.Close();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }
        #endregion


        private void Form1_Load(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }
        OpenFileDialog Ofile = new OpenFileDialog();

        DataSet ds = new DataSet();
        private void button1_Click(object sender, EventArgs e)
        {


            dataGridView1.Columns.Clear();


            this.ds.Tables.Clear();
            this.Ofile.FileName = "";
            this.dataGridView1.DataSource = "";
            this.Ofile.ShowDialog();
            string fileName = this.Ofile.FileName;

            
            if (fileName.Trim().EndsWith("xlsm")|| fileName.Trim().EndsWith("xls"))//判断所要的?展名?型；
            {
                if (fileName != null && fileName != "")
                {
                    

                    string connectionString = " Provider = Microsoft.Jet.OLEDB.4.0 ; Data Source = " + fileName + "; Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";
                    OleDbConnection oleDbConnection = new OleDbConnection(connectionString);
                    oleDbConnection.Open();
                    DataTable oleDbSchemaTable = oleDbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[]
                    {
                    null,
                    null,
                    null,
                    "TABLE"
                    });
                    string str = oleDbSchemaTable.Rows[0]["TABLE_NAME"].ToString();
                    string selectCommandText = "select * from [" + str + "]";
                    OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(selectCommandText, oleDbConnection);
                    oleDbDataAdapter.Fill(this.ds, "temp");
                    oleDbConnection.Close();
                    this.dataGridView1.DataSource = this.ds.Tables[0];


                }

            }
        }


        public void inputData()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                try
                {



                    string v1 = dataGridView1.Rows[i].Cells[0].Value.ToString().Trim();
                    string v2 = dataGridView1.Rows[i].Cells[1].Value.ToString().Trim();
                    string v3 = dataGridView1.Rows[i].Cells[2].Value.ToString().Trim();
                    string v4 = dataGridView1.Rows[i].Cells[3].Value.ToString().Trim();
                    string v5 = dataGridView1.Rows[i].Cells[4].Value.ToString().Trim();
                    string v6 = dataGridView1.Rows[i].Cells[5].Value.ToString().Trim();
                    string v7 = dataGridView1.Rows[i].Cells[6].Value.ToString().Trim();
                    string v8 = dataGridView1.Rows[i].Cells[7].Value.ToString().Trim();
                    string v9 = dataGridView1.Rows[i].Cells[8].Value.ToString().Trim();

                    string sql = "INSERT INTO datas (title,neirong,room,price,louceng,mianji,tel,lxr,Wx)VALUES('" + v1 + " ', '" + v2 + " ', '" + v3 + " ', '" + v4 + " ', '" + v5 + " ', '" + v6 + " ', '" + v7 + " ', '" + v8 + " ', '" + v9 + " ')";
                    insertData(sql);
                    label1.Text = "正在导入： " + v1;
                }
                catch (Exception)
                {

                    continue;
                }
            }
            MessageBox.Show("导入完成");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(inputData));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }


       
        private void button3_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(chaxun));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            string str = Interaction.InputBox("可以一次添加多个标签，用英文逗号隔开", "添加标签", "输入标签", -1, -1);
            foreach (ListViewItem lv in listView1.Items)
            {
                if (lv.Checked)
                {
                    lv.SubItems[10].ForeColor = Color.Red;
                    lv.SubItems[10].Text=str;
                    string sql = "update datas set tags = '" + str.Trim() + " ' WHERE id= '"+lv.SubItems[0].Text+"'";
                    updateData(sql);

                }
            }


        }

        private void button5_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            zanting = true;
        }
    }
}
