using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace main
{
   
    public partial class mainForm : Form
    {
       
        /// <summary>
        /// 获取用户名
        /// </summary>
        /// <param name="username"></param>
        public  void getUsername(string username)
        {
            userStatus.Text = username;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            userPointStatus.Text = method.getPoints(userStatus.Text).ToString();
            textBox1.Text = pubVariables.exs;

        }

        pubVariables pub = new pubVariables();
        aFunction af = new aFunction();


        public mainForm()
        {
            InitializeComponent(); 
        }

        /// <summary>
        /// 阻止datagridview复制数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void skinDataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
                e.Handled = true;
            if (e.Control && e.KeyCode == Keys.A)
                e.Handled = true;
            if (e.Control && e.KeyCode == Keys.X)
                e.Handled = true;
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
           
            userPointStatus.Text = method.getPoints(userStatus.Text).ToString();
         
        }
  

        

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            listView1.MultiSelect = false;
            //鼠标右键
            if (e.Button == MouseButtons.Right)
            {
                //选中列表中数据才显示 空白处不显示
                pubVariables.item = listView1.SelectedItems[0].Text; //获取选中文件名
                Point p = new Point(e.X, e.Y);
                contextMenuStrip1.Show(listView1, p);  //在listview2的p处显示contextmenueStrip1
            }
        }
        /// <summary>
        /// 右击编辑事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void 编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (treeView1.SelectedNode != null)
            {

                infosForm iform = new infosForm();

                pubVariables.item = treeView1.SelectedNode.Text; //获取选中文件名
                iform.Show();
                iform.Text = pubVariables.item;


                if (pubVariables.item.Contains("美团") || pubVariables.item.Contains("马可波罗") || pubVariables.item.Contains("搜了网"))
                {
                    method.getMeituanCityName(iform.comboBox1);


                }
                else if (pubVariables.item.Contains("58同城"))
                {
                    method.get58CityName(iform.comboBox1);
                    iform.radioButton1.Visible = true;
                    iform.radioButton2.Visible = true;

                }

                else if (pubVariables.item.Contains("慧聪网"))
                {
                    method.getHccityName(iform.comboBox1);

                }
                else if (pubVariables.item.Contains("黄页88企业采集"))
                {
                    method.gethy88CityName(iform.comboBox1);
                    method.gethy88itemName(iform.comboBox2);

                }
                else if (pubVariables.item.Contains("赶集网本地服务"))
                {
                    method.ganjicityName(iform.comboBox1);
                    method.ganjiItemName(iform.comboBox2);
                    iform.comboBox2.Visible = true;
                }
                else if (pubVariables.item.Contains("赶集个人二手车"))
                {
                    method.ganjicityName(iform.comboBox1);
                }
                else if (pubVariables.item.Contains("顺企网"))
                {
                    method.gethy88CityName(iform.comboBox1);
                    method.getshunqiItemName(iform.comboBox2);
                }
                else
                {
                    method.get58CityName(iform.comboBox1);
                }

            }

        }
        
        /// <summary>
        /// 鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {

                infosForm iform = new infosForm();
            
                pubVariables.item = treeView1.SelectedNode.Text; //获取选中文件名
                iform.Show();
                iform.Text = pubVariables.item;
               

                if (pubVariables.item.Contains("美团") || pubVariables.item.Contains("马可波罗") || pubVariables.item.Contains("搜了网"))
                {
                    method.getMeituanCityName(iform.comboBox1);


                }
                else if (pubVariables.item.Contains("58同城"))
                {
                    method.get58CityName(iform.comboBox1);
                    iform.radioButton1.Visible = true;
                    iform.radioButton2.Visible = true;

                }

                else if (pubVariables.item.Contains("慧聪网"))
                {
                    method.getHccityName(iform.comboBox1);

                }
                else if (pubVariables.item.Contains("黄页88企业采集"))
                {
                    method.gethy88CityName(iform.comboBox1);
                    method.gethy88itemName(iform.comboBox2);

                }
                else if (pubVariables.item.Contains("赶集网本地服务"))
                {
                    method.ganjicityName(iform.comboBox1);
                    method.ganjiItemName(iform.comboBox2);
                    iform.comboBox2.Visible = true;
                }
                else if (pubVariables.item.Contains("赶集个人二手车"))
                {
                    method.ganjicityName(iform.comboBox1);
                }
                else if (pubVariables.item.Contains("顺企网"))
                {
                    method.gethy88CityName(iform.comboBox1);
                    method.getshunqiItemName(iform.comboBox2);
                }
                else
                {
                    method.get58CityName(iform.comboBox1);
                }

            }
         

        }

        

        private void 开始ToolStripMenuItem_Click(object sender, EventArgs e)
        {          
            ListViewItem lv1 = listView1.Items.Add(pubVariables.item); //第二行 第一行加载时候以后实例化
            lv1.SubItems.Add("采集中");
            lv1.ImageIndex = 1;
            lv1.SubItems.Add(DateTime.Now.ToString());
            lv1.SubItems.Add(pubVariables.citys);
            lv1.SubItems.Add(pubVariables.keywords);



            dataView dv = new dataView();
            
            dv.TopLevel = false;
            TabPage page = new TabPage(); //建立新的tabpage
            page.Text = pubVariables.item;
            page.Name = pubVariables.item;
            page.Controls.Add(dv);
            tabControl2.TabPages.Add(page);
            dv.Show();
            dv.Dock = DockStyle.Fill;
            

            rundelegate rd = new rundelegate(dv.run);
            rd();
          
            

        }


        private void 暂停ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            af.tm.Stop();
            
        }

        private void 继续ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            af.tm.Start();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            开始ToolStripMenuItem_Click(this, e);
        }

       

        private void 导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string Nowpath = System.Environment.CurrentDirectory; //获取当前程序运行文件夹
            if (Directory.Exists(Nowpath + "\\data\\" + pubVariables.item))
            {
                this.tabControl3.SelectedIndex = 1;
                this.tabControl3.TabPages[1].Text = pubVariables.item + "数据";               
                string path = Nowpath + "\\data\\" + pubVariables.item;
                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\Content.db");

                SQLiteDataAdapter sda = new SQLiteDataAdapter("Select * From result ", mycon);
                DataSet Ds = new DataSet();

                sda.Fill(Ds, "T_Class");
                this.skinDataGridView1.DataSource = Ds.Tables[0];


                int i = Convert.ToInt32(userPointStatus.Text) - this.skinDataGridView1.Rows.Count;
                if (i >= 0)
                {
                    MessageBox.Show("本次导出共需要消耗" + this.skinDataGridView1.Rows.Count + "积分");
                    method.DataTableToExcel(method.DgvToTable(this.skinDataGridView1), "Sheet1", true);

                    method.decreasePoints(userStatus.Text, Convert.ToInt32(userPointStatus.Text), this.skinDataGridView1.Rows.Count);

                }
                else
                {
                    MessageBox.Show("您的积分不足，请充值！");
                }


            }
            else
            {
                MessageBox.Show("不存在该任务数据！");
            }


            
        }

    

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            暂停ToolStripMenuItem_Click(this, e);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            继续ToolStripMenuItem_Click(this, e);
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            导出ToolStripMenuItem_Click(this, e);
        }

        private void treeView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right&& treeView1.SelectedNode !=null)
            {
                //选中列表中数据才显示 空白处不显示
                pubVariables.item = treeView1.SelectedNode.Text; //获取选中文件名
                Point p = new Point(e.X, e.Y);
                contextMenuStrip1.Show(treeView1, p);  //在控件的p处显示contextmenueStrip1

                
            }
        }



        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://wpa.qq.com/msgrd?v=3&uin=852266010&site=qq&menu=yes");

        }


        private void toolStripButton6_Click(object sender, EventArgs e)
        {
           
            
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            pubVariables.status = false;
        }

        private void 清空ToolStripMenuItem_Click(object sender, EventArgs e)
        {
               
            //this.skinDataGridView1.Rows.Clear();
            //this.listView1.Items.Clear();

            string Nowpath = System.Environment.CurrentDirectory; //获取当前程序运行文件夹
            if (Directory.Exists(Nowpath + "\\data\\" + pubVariables.item))
            {
                method.clearTable(pubVariables.item);
                this.tabControl3.SelectedIndex = 1;
                this.tabControl3.TabPages[1].Text = pubVariables.item + "数据";
                string path = Nowpath + "\\data\\" + pubVariables.item;
                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\Content.db");

                SQLiteDataAdapter sda = new SQLiteDataAdapter("Select * From result ", mycon);
                DataSet Ds = new DataSet();

                sda.Fill(Ds, "T_Class");
                this.skinDataGridView1.DataSource = Ds.Tables[0];


            }
            else
            {
                MessageBox.Show("不存在该任务数据！");
            }

        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            清空ToolStripMenuItem_Click(this, e);
            //this.listView1.Items.Clear();
        }

       

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://acaiji.com/buy.aspx");
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            userPointStatus.Text = method.getPoints(userStatus.Text).ToString();
            
        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

     

        private void 用户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void 编辑该任务数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Nowpath = System.Environment.CurrentDirectory; //获取当前程序运行文件夹
            if (Directory.Exists(Nowpath + "\\data\\" + pubVariables.item))
            {
                this.tabControl3.SelectedIndex = 1;
                this.tabControl3.TabPages[1].Text = pubVariables.item + "数据";
                string path = Nowpath + "\\data\\" + pubVariables.item;
                SQLiteConnection mycon = new SQLiteConnection("Data Source=" + path + "\\Content.db");

                SQLiteDataAdapter sda = new SQLiteDataAdapter("Select * From result ", mycon);
                DataSet Ds = new DataSet();

                sda.Fill(Ds, "T_Class");
                this.skinDataGridView1.DataSource = Ds.Tables[0];
                int count = Convert.ToInt32(this.skinDataGridView1.Rows.Count) - 1;
                toolStripLabel1.Text = "总共 " + count+" 条数据";

            }
            else {
                MessageBox.Show("不存在该任务数据！");
            }

        }
        //导出表格
        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(userPointStatus.Text) - this.skinDataGridView1.Rows.Count;
            if (i >= 0)
            {
                MessageBox.Show("本次导出共需要消耗" + this.skinDataGridView1.Rows.Count + "积分");
                method.DataTableToExcel(method.DgvToTable(this.skinDataGridView1), "Sheet1", true);

                method.decreasePoints(userStatus.Text, Convert.ToInt32(userPointStatus.Text), this.skinDataGridView1.Rows.Count);

            }
            else
            {
                MessageBox.Show("您的积分不足，请充值！");
            }

        }
        //去除空白号码
        private void toolStripButton6_Click_1(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i <= skinDataGridView1.Rows.Count; i++)
                {
                    string value = skinDataGridView1.Rows[i].Cells[2].Value.ToString();

                    if (value=="")                //Contains()包含, indexof()返回字符在字符串中首次出现的位置，若没有返回 -1
                    {
                        skinDataGridView1.Rows.RemoveAt(i);
                    }
                }
            }

            catch (System.Exception ex)
            {
                 pubVariables.exs= ex.ToString();
            }
        }
        //去除固话
        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i <= skinDataGridView1.Rows.Count; i++)
                {
                    string value = skinDataGridView1.Rows[i].Cells[2].Value.ToString();

                    if (value.Contains("-"))                //Contains()包含, indexof()返回字符在字符串中首次出现的位置，若没有返回 -1
                    {
                        skinDataGridView1.Rows.RemoveAt(i);
                    }
                }
            }

            catch (System.Exception ex)
            {
                pubVariables.exs = ex.ToString();
            }
        }

        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(userPointStatus.Text) - this.skinDataGridView1.Rows.Count;
            if (i >=0)
            {
                MessageBox.Show("本次导出共需要消耗" + this.skinDataGridView1.Rows.Count + "积分");
                method.dataGridViewToCSV(this.skinDataGridView1);

                method.decreasePoints(userStatus.Text, Convert.ToInt32(userPointStatus.Text), this.skinDataGridView1.Rows.Count);

            }
            else
            {
                MessageBox.Show("您的积分不足，请充值！");
            }
            
        }
    }
}
