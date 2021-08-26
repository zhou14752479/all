using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

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
                string type = comboBox1.Text;
            
                string pinpai = textBox1.Text.Trim();
                string cate1 = textBox2.Text.Trim();
                string cate2 = textBox3.Text.Trim();
                string sq_starttime = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                string sq_endtime = dateTimePicker2.Value.ToString("yyyy-MM-dd");

                
                string sql = "SELECT id,type,name,pinpai,cate1,cate2,sq_starttime,sq_endtime,yjsq_starttime,is_yuanjian,is_shouhou,is_shangbiao,shangbiao_endtime from datas  where ";
                if (comboBox1.Text == "全部授权")
                {
                    sql = sql + ("type like '_%' AND");
                }
                else
                {
                    sql = sql + ("type like '" + type + "' AND ");
                }

                if (textBox1.Text == "")
                {
                    sql = sql + (" pinpai like '_%' AND");
                }
                else
                {
                    sql = sql + ("pinpai like '" + pinpai+ "' AND ");
                }

                if (textBox2.Text == "")
                {
                    sql = sql + (" cate1 like '_%' AND ");
                }
                else
                {
                    sql = sql + ("cate1 like '" + cate1 + "' AND ");
                }

                if (textBox3.Text == "")
                {
                    sql = sql + (" cate2 like '_%' AND ");
                }
                else
                {
                    sql = sql + ("cate2 like '" + cate2+ "' AND ");
                }


                sql = sql + ("sq_starttime >= '" + sq_starttime + "' AND ");
                sql = sql + ("sq_starttime <= '" + sq_endtime + "' ");

               
                DataTable dt = fc.getdata(sql);
               fc.ShowDataInListView(dt,listView1);

            }
            catch (Exception ex)
            {

               MessageBox.Show(ex.ToString());
            }

        }

        #region 下载文件
        public void downloadfile(string colname)
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
                    string id = listView1.CheckedItems[i].SubItems[0].Text;
                    string name = listView1.CheckedItems[i].SubItems[2].Text;
                    string base64 = fc.getziduan(id, colname);
                   
                    if (base64 != "")
                    {
                        fc.Base64ToImage(base64,path + "//" + name + ".jpg");
                        label7.Text = DateTime.Now.ToString()+"：正在下载："+name;
                        
                        // img.Save(path + "//" + name + ".jpg");
                    }
                }

                label7.Text = DateTime.Now.ToString() + "：全部下载完成";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion
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

            downloadfile("img_shouquan");
        }

        private void 导出选定售后函ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            downloadfile("img_shouhou");
        }

        private void 清空数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
