using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 管理软件
{
    public partial class 客户端 : Form
    {
        public 客户端()
        {
            InitializeComponent();
        }
        #region 获取公网IP
        public static string GetIP()
        {
            using (var webClient = new WebClient())
            {
                try
                {
                    webClient.Credentials = CredentialCache.DefaultCredentials;
                    byte[] pageDate = webClient.DownloadData("http://pv.sohu.com/cityjson?ie=utf-8");
                    String ip = Encoding.UTF8.GetString(pageDate);
                    webClient.Dispose();

                    Match rebool = Regex.Match(ip, @"\d{2,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
                    return rebool.Value;
                }
                catch (Exception e)
                {
                    return e.ToString();
                }

            }
        }

        #endregion

        #region 获取所有连接
        public ArrayList geturls(string str)
        {
            ArrayList list = new ArrayList();
            try
            {
                string constr = "Host =47.99.68.92;Database=links;Username=root;Password=zhoukaige00.@*.";
                
                MySqlDataAdapter da = new MySqlDataAdapter(str, constr);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(dr[0].ToString().Trim());
                }
                
            }
            catch (MySqlException ee)
            {
                MessageBox.Show(ee.Message.ToString());
            }
            return list;

        }
        #endregion


        #region 获取ip
        public ArrayList getips()
        {
            ArrayList list = new ArrayList();
            try
            {
                string constr = "Host =47.99.68.92;Database=links;Username=root;Password=zhoukaige00.@*.";
                string str = "SELECT ip from ips";
                MySqlDataAdapter da = new MySqlDataAdapter(str, constr);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(dr[0].ToString().Trim());
                }

            }
            catch (MySqlException ee)
            {
                MessageBox.Show(ee.Message.ToString());
            }
            return list;

        }
        #endregion
        private void 客户端_Load(object sender, EventArgs e)
        {
            label2.Text = GetIP();
            ArrayList ips = getips();
            if (!ips.Contains(label2.Text))
            {
                MessageBox.Show("ip未添加");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        public void run(string str,int value)

        {
            try
            {
               
                ArrayList urls= geturls(str);
                foreach (string url in urls)
                {
                    for (int i = 0; i < value; i++)
                    {
                        System.Diagnostics.Process.Start(textBox2.Text + url);
                    }
                   

                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str = "SELECT url from aaa";
            run(str,5);
        }
    }
}
