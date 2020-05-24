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

namespace 权限管理
{
    public partial class 主界面 : Form
    {
        public 主界面()
        {
            InitializeComponent();
        }

        public static string qid;
        public static string username;

        string constr = "Host =111.229.244.97;Database=qun;Username=root;Password=root";

        #region  image转base64
        public static string ImageToBase64(Image image)
        {
            try
            {
                Bitmap bmp = new Bitmap(image);
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                return Convert.ToBase64String(arr);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region  base64转image
        public static Bitmap Base64ToImage(string strbase64)
        {
            try
            {
                byte[] arr = Convert.FromBase64String(strbase64);
                MemoryStream ms = new MemoryStream(arr);
                Bitmap bmp = new Bitmap(ms);
                ms.Close();
                
                return bmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        #endregion


        #region  添加账号

        public void addUsers(string username, string password,string uname,string qid)
        {

            try
            {

              
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO users (username,password,uname,qid)VALUES('" + username + " ', '" + password + " ', '" + uname + " ', '" + qid + " ')", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {
                    MessageBox.Show("添加账户成功");
                    mycon.Close();

                }

            }

            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region  添加账号

        public void deleteUsers(string username)
        {

            try
            {


                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE FROM users where username= '" + username + " '", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {
                    MessageBox.Show("添加账户成功");
                    mycon.Close();

                }

            }

            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region  添加二维码

        public void addImage(string name,string data)
        {

            try
            {

                
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO images (name,data,user,time)VALUES('" + name + " ','" + data + " ','未分发','" + DateTime.Now.ToString() + " ')", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {
                   
                    mycon.Close();

                }

            }

            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion


        #region 获取所有用户
        public void getusers(string sql)
        {
            
            MySqlDataAdapter sda = new MySqlDataAdapter(sql, constr);
            DataSet Ds = new DataSet();
            sda.Fill(Ds, "T_Class");

            this.dataGridView1.DataSource = Ds.Tables["T_Class"];
        }

        #endregion

        #region 获取所有二维码
        public void getpics(string sql,DataGridView dgv)
        {
           
            try
            {
                MySqlDataAdapter sda = new MySqlDataAdapter(sql, constr);
                DataSet Ds = new DataSet();
                sda.Fill(Ds, "T_Class");

                dgv.DataSource = Ds.Tables["T_Class"];
            }
            catch (Exception ex)
            {
                MessageBox.Show("该账户下无分配二维码");
            }
        }

        #endregion


        #region  分发二维码

        public void fenfaImage(string user,string id)
        {

            try
            {


                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("UPDATE images SET user='" + user + " ' where id= '" + id + " '", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {

                    mycon.Close();

                }

            }

            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region  标记二维码

        public void biaoji( string id,string status)
        {

            try
            {
                string sql = "UPDATE images SET status='" + status + " ' ,updatetime='" + DateTime.Now.ToShortDateString() + " ' where id= '" + id + " '";

                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand(sql, mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {

                    mycon.Close();

                }

            }

            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        private void 主界面_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            if (qid == "0")
            {
                tabPage2.Parent = null;
                tabPage3.Parent = null;
            }
            if (qid == "1")
            {
                tabPage1.Parent = null;
                tabPage3.Parent = null;
            }
            if (qid == "2")
            {
                tabPage1.Parent = null;
                tabPage2.Parent = null;
            }


        }

       
        private void Get_Folder()
        {
            string FilePath = textBox1.Text;
            if (Directory.Exists(FilePath))
            {
                foreach (string d in Directory.GetFileSystemEntries(FilePath))
                {
                    Image img = Image.FromFile(d);
                    if (File.Exists(d) && img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg) ||
                            img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Gif) ||
                            img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Bmp) ||
                            img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png))
                    {
                        listBox1.Items.Add(d.ToString());
                       
                    }
                }
            }
            else
            {
                MessageBox.Show("文件夹不存在!");
            }

            progressBar1.Maximum = listBox1.Items.Count-1;
            for (int i = 0; i < listBox1.Items.Count; i++)
            {

                Image image = Image.FromFile(listBox1.Items[i].ToString());
                string base64 = ImageToBase64(image);
                addImage(System.IO.Path.GetFileName(listBox1.Items[i].ToString()),base64);
                progressBar1.Value = i ;
                label8.Text = (i * 100 / progressBar1.Maximum).ToString() + "%";//显示百分比
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            Thread thread = new Thread(new ThreadStart(Get_Folder));
            thread.Start();
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择所在文件夹";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    MessageBox.Show(this, "文件夹路径不能为空", "提示");
                    return;
                }

                textBox1.Text = dialog.SelectedPath;
            }
        }

        private void 主界面_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("确认退出吗？", "退出询问"
           , MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result != DialogResult.OK)
            {
                e.Cancel = true;//告诉窗体关闭这个任务取消

            }
            else
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("请完善账户信息");
                return;
            }


            string username = textBox3.Text.Trim();
            string password = textBox4.Text.Trim();
            string uname = textBox5.Text.Trim();
            string qid = "1";
            if (comboBox1.Text == "部门")
            {
                qid = "1";
                uname = "admin";
            }
            if (comboBox1.Text == "员工")
            {
                qid = "2";
                uname = textBox5.Text;
            }

            addUsers(username,password,uname,qid);
            button11.PerformClick();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "部门")
            {
                label5.Visible = false;
                textBox5.Visible = false;
            }
            else
            {
                label5.Visible = true;
                textBox5.Visible = true;

            }
            
        }
        //部门增加用户
        private void button8_Click(object sender, EventArgs e)
        {
            string username = textBox9.Text.Trim();
            string password = textBox10.Text.Trim();
            string qid = "2";

            addUsers(username, password,username, qid);
        }
        //总管理获取用户
        private void button11_Click(object sender, EventArgs e)
        {
            string sql = "Select* From users ";
            getusers(sql);
        }
        //部门获取用户
        private void button12_Click(object sender, EventArgs e)
        {
            string sql = "Select* From users where uname="+username;
            getusers(sql);
        }

        //删除账户
        private void button5_Click(object sender, EventArgs e)
        {
            deleteUsers(textBox6.Text.ToString());
            button11.PerformClick();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            deleteUsers(textBox7.Text.ToString());
            button12.PerformClick();
        }

        //管理员分发二维码
        private void button3_Click(object sender, EventArgs e)
        {
            string user = textBox2.Text;
            foreach (DataGridViewRow dgvRow in dataGridView2.SelectedRows)
            {
                string id = dgvRow.Cells["id"].Value.ToString();
                fenfaImage(user,id);
            }
            button6.PerformClick();
           
            MessageBox.Show("分发成功");
            
        }
        //部门分发二维码
        private void button10_Click(object sender, EventArgs e)
        {
            string user = textBox11.Text;
            foreach (DataGridViewRow dgvRow in dataGridView4.SelectedRows)
            {
                string id = dgvRow.Cells["id"].Value.ToString();
                fenfaImage(user, id);
            }
            button9.PerformClick();
            MessageBox.Show("分发成功");
            
        }


        //管理员获取所有二维码
        
        private void button6_Click(object sender, EventArgs e)
        {
           
            string sql = "Select id,name,user,time From images ";
            getpics(sql,dataGridView2);
        }
        //部门获取二维码
        private void button9_Click(object sender, EventArgs e)
        {
           
            string sql = "Select  id ,name,user,time,data From images where user="+username;
           
            getpics(sql, dataGridView4);
        }
        //员工获取二维码
        private void button14_Click(object sender, EventArgs e)
        {
           
            string sql = "Select id, name,user,time,data From images where status=0 AND user=" + username;

            getpics(sql, dataGridView5);
        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void splitContainer3_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {
            string status = "0";

            if (radioButton1.Checked == true)
            {
                status = "0";
            }
            if (radioButton2.Checked == true)
            {
                status = "1";
            }
            if (radioButton3.Checked == true)
            {
                status = "2";
            }

            foreach (DataGridViewRow dgvRow in dataGridView5.SelectedRows)
            {
                string id = dgvRow.Cells["id"].Value.ToString();
                biaoji(id,status);
            }


            button13.Text = "已标记此二维码";
        }

     

        private void dataGridView5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int a = dataGridView5.CurrentRow.Index;
            string str = dataGridView5.Rows[a].Cells[4].Value.ToString();

            pictureBox1.Image = Base64ToImage(str);

            button13.Text = "标记二维码";
        }

       
    }
}
