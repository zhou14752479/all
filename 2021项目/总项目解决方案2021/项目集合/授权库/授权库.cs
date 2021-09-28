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
using myDLL;
using MySql.Data.MySqlClient;

namespace 授权库
{
    public partial class 授权库 : Form
    {
        public 授权库()
        {
            InitializeComponent();
        }
        function fc = new function();
        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            新增 add = new 新增();
            //add.MdiParent = this;
            //add.Dock = DockStyle.Fill;
            add.Show();

        }

        public void chaxun()
        {
            try
            {
                label7.Text = DateTime.Now.ToLongTimeString()+"：开始查询...";
                string type = comboBox1.Text;
            
                string pinpai = textBox1.Text.Trim();
                string cate1 = textBox2.Text.Trim();
                string cate2 = textBox3.Text.Trim();
              
                string sq_endtime = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                string yjsq_endtime = dateTimePicker2.Value.ToString("yyyy-MM-dd");
                string shangbiao_endtime= dateTimePicker3.Value.ToString("yyyy-MM-dd");
              
                string sql = "SELECT uid,type,name,pinpai,cate1,cate2,sq_starttime,sq_endtime,yjsq_starttime,is_yuanjian,is_shouhou,is_shangbiao,shangbiao_endtime,beizhu from datas  where ";
                if (comboBox1.Text == "全部授权")
                {
                    sql = sql + ("type like '_%' AND ");
                }
                else
                {
                    sql = sql + ("type like '" + type + "' AND ");
                }

                if (textBox1.Text == "")
                {
                    sql = sql + (" pinpai like '_%' AND ");
                }
                else
                {
                    sql = sql + (" pinpai like '" + pinpai+ "' AND ");
                }

                if (textBox2.Text == "")
                {
                    sql = sql + (" cate1 like '_%' AND ");
                }
                else
                {
                    sql = sql + (" cate1 like '" + cate1 + "' AND ");
                }

                if (textBox3.Text == "")
                {
                    sql = sql + (" cate2 like '_%' AND ");
                }
                else
                {
                    sql = sql + (" cate2 like '" + cate2+ "' AND ");
                }

                if (checkBox1.Checked == true)
                {
                    sql = sql + (" sq_endtime >= '" + sq_endtime + "' AND ");
                  
                }


                if (checkBox2.Checked == true)
                {
                    sql = sql + (" yjsq_starttime >= '" + yjsq_endtime + "' AND ");

                }

                if (checkBox3.Checked == true)
                {
                    sql = sql + (" shangbiao_endtime >= '" + shangbiao_endtime + "' ");

                }
                if (sql.Substring(sql.Length-4,4).Contains("AND"))
                {
                    sql = sql.Remove(sql.Length - 4, 4);
                }


                DataTable dt = fc.getdata(sql);
                label11.Text = dt.Rows.Count.ToString();
               fc.ShowDataInListView(dt,listView1);
                
                label7.Text = DateTime.Now.ToLongTimeString() + "：查询结束，授权到期小于一个月已标红";

                try
                {
                    daotiTime();
                }
                catch (Exception)
                {

                    label7.Text="到期时间监控失败，请检查时间格式";
                }
            }
            catch (Exception ex)
            {

               MessageBox.Show(ex.ToString());
            }

        }

        #region 下载文件
        public void downloadfile()
        {
            if (listView1.CheckedItems.Count == 0)
            {
                MessageBox.Show("未选中任何数据");
                return;
            }


            try
            {
                string path = "";
                System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
                dialog.Description = "请选择所在文件夹";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (string.IsNullOrEmpty(dialog.SelectedPath))
                    {
                        MessageBox.Show(this, "文件夹路径不能为空", "提示");
                        return;
                    }

                    path = dialog.SelectedPath;
                }
               
                for (int i = 0; i < listView1.CheckedItems.Count; i++)
                {
                    

                    string uid = listView1.CheckedItems[i].SubItems[0].Text;
                    string name = listView1.CheckedItems[i].SubItems[2].Text;
                    string pinpai = listView1.CheckedItems[i].SubItems[3].Text;
                    string sPath = path + "//" + pinpai+"-"+name+"-"+uid + "//";
                    if (!Directory.Exists(sPath))
                    {
                        Directory.CreateDirectory(sPath); //创建文件夹
                    }
                    

                    fc.getfile(uid, sPath);
                }

                label7.Text = DateTime.Now.ToString() + "：全部下载完成";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion

        public void daotiTime()
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                string time = listView1.Items[i].SubItems[7].Text.Trim();
                string time2 = listView1.Items[i].SubItems[8].Text.Trim();
                string time3 = listView1.Items[i].SubItems[12].Text.Trim();
                listView1.Items[i].UseItemStyleForSubItems = false;
                if (time != "")
                {
                    if (Convert.ToDateTime(time) <= DateTime.Now.AddDays(30))
                    {
                        listView1.Items[i].SubItems[7].BackColor = Color.Red;
                    }
                }

                if (time2 != "")
                {
                    if (Convert.ToDateTime(time2) <= DateTime.Now.AddDays(30))
                    {
                        listView1.Items[i].SubItems[8].BackColor = Color.Red;
                    }
                }

                if (time3 != "")
                {
                    if (Convert.ToDateTime(time3) <= DateTime.Now.AddDays(30))
                    {
                        listView1.Items[i].SubItems[12].BackColor = Color.Yellow;
                        //listView1.Items[i].Font = new Font(label1.Font.Name, 9, FontStyle.Bold );
                    }
                }
            }

        }
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
          
           


            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"YZaj3w"))
            {

                return;
            }



            #endregion
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(chaxun);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i<listView1.Items.Count ; i++)
            {
                listView1.Items[i].Checked = true;
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].Checked = false;
            }
        }

        private void 导出数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void 授权库_Load(object sender, EventArgs e)
        {
            
        }

        private void 导出选定授权ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            downloadfile();
        }

       

      
        


        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            viewdata("查看数据");
        }

        public void viewdata(string title)
        {
            try
            {
                if (this.listView1.SelectedItems.Count == 0)
                    return;

                string uid = listView1.SelectedItems[0].SubItems[0].Text;
             
                新增 add = new 新增();
                add.Text = title;

                MySqlConnection mycon = new MySqlConnection(function.constr);
                mycon.Open();

                string sql = "select * from datas where uid=" + uid;
                textBox1.Text = sql;
                MySqlCommand cmd = new MySqlCommand(sql, mycon);         //SQL语句读取textbox的值'"+textBox1.Text+"'


                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源
                reader.Read();
               
                    add.uid = reader["uid"].ToString().Trim();

                    add.type = reader["type"].ToString().Trim();
                    add.name = reader["name"].ToString().Trim();
                    add.pinpai = reader["pinpai"].ToString().Trim();
                    add.cate1 = reader["cate1"].ToString().Trim();
                    add.cate2 = reader["cate2"].ToString().Trim();


                    add.sq_starttime = reader["sq_starttime"].ToString().Trim();
                    add.sq_endtime = reader["sq_endtime"].ToString().Trim();
                    add.yjsq_starttime = reader["yjsq_starttime"].ToString().Trim();


                    add.is_yuanjian = reader["is_yuanjian"].ToString().Trim();
                    add.is_shouhou = reader["is_shouhou"].ToString().Trim();
                    add.is_shangbiao = reader["is_shangbiao"].ToString().Trim();
                    add.shangbiao_endtime = reader["shangbiao_endtime"].ToString().Trim();
                    add.beizhu = reader["beizhu"].ToString().Trim();
                    mycon.Close();
                    reader.Close();


                    add.Show();
                
            }
            catch (Exception ex)
            {

               MessageBox.Show(ex.ToString());
            }
        }

        private void 查看详细ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            viewdata("查看数据");
        }

        private void 修改数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            viewdata("修改数据");
        }

        private void 下载此条文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            downloadfile();
        }

        private void 删除数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
                return;

            string uid = listView1.SelectedItems[0].SubItems[0].Text;
            string sql = "delete from datas where uid='" + uid + "'  ";
            fc.SQL(sql);

            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(chaxun);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }


        }
    }
}
