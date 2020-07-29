using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序202008
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string constr = "Host =111.229.244.97;Database=fang;Username=root;Password=root";
        #region  插入数据

        public void insertData()
        {

            try
            {

                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                string a1 = "";
                string a2 = "";

                MySqlCommand cmd = new MySqlCommand("INSERT INTO vip (username,password,register_t,ip,mac)VALUES('" + a1 + " ', '" + a2 + " ')", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'
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
                mycon.Close();
            }
        }
            #endregion


            private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
