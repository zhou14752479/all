using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fang.临时软件
{
    public partial class baidu : Form
    {
        public baidu()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        int index { get; set; }

        private void baidu_Load(object sender, EventArgs e)
        {
            ArrayList list = getKeywords();
            label2.Text = list.Count.ToString();
        }

        bool status = true;

        #region 获取关键词
        public static ArrayList getKeywords()
        {
            ArrayList list = new ArrayList();
            try
            {
                string constr = "Host =localhost;Database=baidu;Username=root;Password=root";
                string str = "SELECT keyword from clt_keywords ";
                MySqlDataAdapter da = new MySqlDataAdapter(str, constr);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(dr[0].ToString().Trim());
                }
            }
            catch (MySqlException ex)
            {
                 ex.ToString();
            }

            return list;
        }

        #endregion

        #region 获取网址
        public static ArrayList getWebSites()
        {
            ArrayList list = new ArrayList();
            try
            {
                string constr = "Host =localhost;Database=baidu;Username=root;Password=root";
                string str = "SELECT catdir from clt_website ";
                MySqlDataAdapter da = new MySqlDataAdapter(str, constr);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(dr[0].ToString().Trim());
                }
            }
            catch (MySqlException ex)
            {
                ex.ToString();
            }

            return list;
        }

        #endregion


        ArrayList finishes = new ArrayList();
        #region 百度关键词查询
        public void Run()
        {
            
            try
            {
                ArrayList keywords = getKeywords();
                ArrayList urls = getWebSites();

                foreach (string keyword in keywords)
                {
                    if (!finishes.Contains(keyword))
                    {
                        finishes.Add(keyword);

                        foreach (string url in urls)
                    {

                            for (int i = 0; i < 3; i++)
                            {

                                String Url = "https://www.baidu.com/s?wd=" + keyword + "&pn=" + i + "0";

                                string html = method.GetUrl(Url, "utf-8");


                                MatchCollection Matchs = Regex.Matches(html, @"text-decoration:none;"">([\s\S]*?)&", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                                for (int j = 0; j < Matchs.Count; j++)
                                {

                                    if (Matchs[j].Groups[1].Value.ToString().Contains(url) && i == 0)
                                    {
                                        this.index = this.skinDataGridView1.Rows.Add();
                                        this.skinDataGridView1.Rows[index].Cells[0].Value = keyword;
                                        this.skinDataGridView1.Rows[index].Cells[1].Value = url;
                                        this.skinDataGridView1.Rows[index].Cells[2].Value = (j + 1).ToString();
                                        this.skinDataGridView1.CurrentCell = this.skinDataGridView1.Rows[index].Cells[0];  //让datagridview滚动到当前行
                                        
                                    }


                                    if (Matchs[j].Groups[1].Value.ToString().Contains(url) && i > 0)

                                    {
                                        this.skinDataGridView1.Rows[index].Cells[2].Value = this.skinDataGridView1.Rows[index].Cells[2].Value + "," + (j + 1).ToString();
                                        

                                    }

                                    if (this.status == false)
                                        return;

                                }
                            }
                           
                        }

                    }

                }
            }


            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        #endregion

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(Run));
            thread.Start();

            Thread thread1 = new Thread(new ThreadStart(Run));
            thread1.Start();

            Thread thread2 = new Thread(new ThreadStart(Run));
            thread2.Start();

            Thread thread3 = new Thread(new ThreadStart(Run));
            thread3.Start();

            Thread thread4 = new Thread(new ThreadStart(Run));
            thread4.Start();

            Thread thread5 = new Thread(new ThreadStart(Run));
            thread5.Start();

            Thread thread6 = new Thread(new ThreadStart(Run));
            thread6.Start();

            Thread thread7 = new Thread(new ThreadStart(Run));
            thread7.Start();

            Thread thread8 = new Thread(new ThreadStart(Run));
            thread8.Start();
        }
    }
}
