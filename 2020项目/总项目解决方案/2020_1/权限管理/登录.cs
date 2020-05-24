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

namespace 权限管理
{
    public partial class 登录 : Form
    {
        public 登录()
        {
            InitializeComponent();
        }

      

        #region  登陆函数

        public void login(string user,string pass)
        {

            try
            {
                string constr = "Host =111.229.244.97;Database=qun;Username=root;Password=root";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("select * from users where username='" + user + "'  ", mycon);         //SQL语句读取textbox的值'"+textBox1.Text+"'


                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

                if (reader.Read())
                {
                    string password = reader["password"].ToString().Trim();
                   string  qid= reader["qid"].ToString().Trim();
                    string username = reader["username"].ToString().Trim();
                    if (pass.Trim() == password)
                    {
                        主界面.qid = qid;
                        主界面.username = username;
                        主界面 zhu = new 主界面();
                        zhu.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("密码错误");
                    }
                }
                else
                {
                    MessageBox.Show("账户不存在");
                }

                mycon.Close();
                reader.Close();
                


            }

            catch (System.Exception ex)
            {
               MessageBox.Show( ex.ToString());
            }


        }

        #endregion
        private void 登录_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            login(textBox1.Text,textBox2.Text);
        }
    }
}
