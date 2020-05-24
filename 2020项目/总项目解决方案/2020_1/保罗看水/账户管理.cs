using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 保罗看水
{
    public partial class 账户管理 : Form
    {
        public 账户管理()
        {
            InitializeComponent();
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
        #region  获取32位MD5加密
        public string GetMD5(string txt)
        {
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.Default.GetBytes(txt);
                //开始加密
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        #endregion
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetDataObject(textBox1.Text.Trim());
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox1.Text = GetMD5(GetTimeStamp());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请先生成激活码");
                return;
            }
            addUsers();
        }
        #region  添加账号

        public void addUsers()
        {

            try
            {
                string constr = "Host =111.229.244.97;Database=kanshui;Username=root;Password=root";

                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO users (zhucema,mac,expiretime)VALUES('" + textBox1.Text.Trim() + " ', '1', '" + DateTime.Now.AddDays(Convert.ToInt32(comboBox1.Text)) + " ')", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {
                    MessageBox.Show("添加成功");
                    mycon.Close();
                    getusers();
                }

            }

            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region 获取所有用户
        public void getusers()
        {
            string constr = "Host =111.229.244.97;Database=kanshui;Username=root;Password=root";
            MySqlDataAdapter sda = new MySqlDataAdapter("Select* From users ", constr);
            DataSet Ds = new DataSet();
            sda.Fill(Ds, "T_Class");

            this.dataGridView1.DataSource = Ds.Tables["T_Class"];
        }

        #endregion
        private void 账户管理_Load(object sender, EventArgs e)
        {

        }
    }
}
