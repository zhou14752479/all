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

namespace 京东价格对比
{
    public partial class 京东价格对比 : Form
    {
        public 京东价格对比()
        {
            InitializeComponent();
        }

        private void 京东价格对比_Load(object sender, EventArgs e)
        {

        }

        string inportFilename = "";

        function fc = new function();
        Thread thread;
        private void 导入商品ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            // openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.Filter = "excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                inportFilename = openFileDialog1.FileName;
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(inportexcel);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }

            }

        }


        public void inportexcel()
        {
            try
            {
                if (inportFilename == "")
                {
                    MessageBox.Show("请先选择表格文件");
                    return;
                }

                DataTable dt = method.ExcelToDataTable(inportFilename,true);

                int max = dt.Rows.Count;
                progressBar1.Value = 0;  //清空进度条
                progressBar1.Maximum = max-1;  //清空进度条
                for (int i = 0; i < max; i++)
                {
                    progressBar1.Value = i;
                    log_label.Text=("导入进度：" +((i/(max-1))*100).ToString() + "%");

                    string itemid = dt.Rows[i][0].ToString().Trim();
                    string skuid = dt.Rows[i][1].ToString().Trim();
                    string name = dt.Rows[i][2].ToString().Trim();
                    string cate1 = dt.Rows[i][3].ToString().Trim();
                    string cate2 = dt.Rows[i][4].ToString().Trim();
                    string cate3 = dt.Rows[i][5].ToString().Trim();
                    string skuprice = dt.Rows[i][6].ToString().Trim();
                    string jdskuurl = dt.Rows[i][7].ToString().Trim();

                    string sql = "INSERT INTO datas(itemid,skuid,name,cate1,cate2,cate3,skuprice,jdskuurl)VALUES('" + @itemid + "'," +
                        "'" + skuid + "'," +
                        "'" + name + "'," +
                        "'" + cate1 + "'," +
                        "'" + cate2 + "'," +
                        "'" + cate3 + "'," +
                        "'" + skuprice+ "'," +
                        "'" + jdskuurl+ "')";
                    fc.insertdata(sql);
                }
                log_label.Text = "导入成功";
                getall();
            }
            catch (Exception ex)
            {
               
                log_label.Text = ex.ToString();
            }
        }

        public void getall()
        {
            try
            {
                string sql = "select * from datas";
                DataTable dt =fc.chaxundata(sql);
                method.ShowDataInListView(dt,listView1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

                log_label.Text=ex.ToString();
            }
        }


        public void updateinfo()
        {
            try
            {
                string sql = "select * from datas";
                DataTable dt = fc.chaxundata(sql);
                int max = dt.Rows.Count;
                progressBar1.Value = 0;  //清空进度条
                progressBar1.Maximum = max - 1;  //清空进度条
             
                for (int i = 0; i < max; i++)
                {
                    progressBar1.Value = i;
                    log_label.Text = ("更新进度：" + ((i / (max - 1)) * 100).ToString() + "%");

                    string id = dt.Rows[i][0].ToString().Trim();
                    string jdskuurl = dt.Rows[i][8].ToString().Trim();
                    string oldxinghao = dt.Rows[i][9].ToString().Trim();

                    //型号为空，代表JD参数为空，需要填入JD抓取信息
                    if (oldxinghao == "")
                    {
                        string url = "https://p.3.cn/prices/mgets?skuIds=J_23192213069";
                        string html = function.GetUrl(url);

                        string xinghao = "1";
                        string guige = "2";
                        string danwei = "3";
                        string price = "4";
                        string status = "5";
                        string sql2 = "UPDATE datas set xinghao='" + xinghao + "',guige='" + @guige + "',danwei='" + @danwei + "',price='" + @price + "',status='" + status + "' where id='" + id + "' ";
                        fc.insertdata(sql2);
                    }

                    //型号不为空，则只需要更新价格和状态，原有参数已抓取
                    else
                    {
                        string url = "https://p.3.cn/prices/mgets?skuIds=J_23192213069";
                        string html = function.GetUrl(url);
                        string price = "4";
                        string status = "5";
                        string sql2 = "UPDATE datas set price='" + @price + "',status='" + status + "' where id='" + id + "' ";
                        fc.insertdata(sql2);
                    }
                }
               
            }
            catch (Exception ex)
            {

                log_label.Text = ex.ToString();
            }

        }

        private void 刷新数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(getall);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(updateinfo);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
    }
}
