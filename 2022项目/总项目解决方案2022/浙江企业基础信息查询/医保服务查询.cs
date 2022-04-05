using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 浙江企业基础信息查询
{
    public partial class 医保服务查询 : Form
    {
        #region POST默认请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrlDefault(string url, string postData, string COOKIE)
        {
            try
            {

                string charset = "utf-8";
                string html = "";
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                //ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;  //用于验证服务器证书
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                request.Proxy = null;//防止代理抓包
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                //headers.Add("sec-fetch-site:same-origin");
                //headers.Add("sec-fetch-user:?1");
                //headers.Add("upgrade-insecure-requests: 1");
                //添加头部
               // request.ContentType = "application/x-www-form-urlencoded";
                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
                request.ContentType = "application/json";
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                // request.ContentLength = postData.Length;
                request.Headers.Add("Accept-Encoding", "gzip");
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);

                request.Referer = url;
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");

                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    html = reader.ReadToEnd();
                    reader.Close();
                }


                response.Close();
                return html;
            }
            catch (WebException ex)
            {

                return ex.ToString();
            }


        }

        #endregion
        public 医保服务查询()
        {
            InitializeComponent();
        }
        bool jiami = true;
        bool zanting = true;
        bool status = false;
        Thread thread;
        DataTable dt;
        private void button6_Click(object sender, EventArgs e)
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
                textBox1.Text = openFileDialog1.FileName;
                dt = method.ExcelToDataTable(textBox1.Text, true);

            }
        }




        public string jiami_encode(string data)
        {
            string url = "http://www.acaiji.com:520";
            string postdata = "{\"type\":\"encode\",\"data\":\"{\\\"uscc\\\":\\\""+data+"\\\"}\"}";
            string html = method.PostUrlDefault(url,postdata,"");
            return html.ToUpper();

        }
        public string jiemi_decode(string data)
        {
            string url = "http://www.acaiji.com:520";
            string postdata = "{\"type\":\"decode\",\"data\":\"" + data + "\"}";
            string html = method.PostUrlDefault(url, postdata, "");
            return html.ToUpper();

        }

        int total;
        public void run()
        {

            try
            {
                for (int a = 0; a < dt.Rows.Count; a++)
                {
                    total = total + 1;
                    if (DateTime.Now > Convert.ToDateTime("2022-05-05"))
                    {
                        MessageBox.Show("{\"msg\":\"非法请求\"}");
                        return;
                    }

                    DataRow dr = dt.Rows[a];
                    string uid = dr[0].ToString();
                    if (uid == "")
                        continue;
                    uid = jiami_encode(uid);
                   
                    string url = "https://zhyb.ybj.zj.gov.cn/hsa-pss-cw-local/api/pss/web/cw/queryInsuEmpInfoByRegist";
                    string postdata = "{\"data\":{\"appId\":\"0c5efcc7a3d8409cbdc3657534da13f0\",\"version\":\"1.0.0\",\"encType\":\"SM4\",\"signType\":\"SM2\",\"timestamp\":1648956669005,\"encData\":\""+uid+"\",\"signData\":\"YStFqrooEaKIbDsPTxaXvCMLL0LCobpigyKFzwC4Uq2A6EA3zEEMBT+GVYVuwoj8xZcnFoq7urp5vJmfwIeDTw==\"}}";

                    string html =PostUrlDefault(url, postdata, "");

                    string encData = Regex.Match(html,@"""encData"":""([\s\S]*?)""").Groups[1].Value;
                    html = jiemi_decode(encData);
                    //textBox2.Text = encData;
                    label3.Text = "正在查询：" + uid;
                    string EMPADDR = Regex.Match(html, @"""EMPADDR"":""([\s\S]*?)""").Groups[1].Value;
                    string EMPNAME = Regex.Match(html, @"""EMPNAME"":""([\s\S]*?)""").Groups[1].Value;
                    //string EMPNO = Regex.Match(html, @"""EMPNO"":""([\s\S]*?)""").Groups[1].Value;
                    string INSUADMDVS = Regex.Match(html, @"""INSUADMDVS"":""([\s\S]*?)""").Groups[1].Value;
                    string LEGREPCERTNO = Regex.Match(html, @"""LEGREPCERTNO"":""([\s\S]*?)""").Groups[1].Value;
                    string LEGREPNAME = Regex.Match(html, @"""LEGREPNAME"":""([\s\S]*?)""").Groups[1].Value;
                    string LEGREPTEL = Regex.Match(html, @"""LEGREPTEL"":""([\s\S]*?)""").Groups[1].Value;

                    string input = dr[0].ToString();
                    if (jiami == true)
                    {
                        input = method.Base64Encode(Encoding.GetEncoding("utf-8"), dr[0].ToString());
                        EMPADDR = method.Base64Encode(Encoding.GetEncoding("utf-8"), EMPADDR);
                        EMPNAME = method.Base64Encode(Encoding.GetEncoding("utf-8"), EMPNAME);
                       // EMPNO = method.Base64Encode(Encoding.GetEncoding("utf-8"), EMPNO);
                        INSUADMDVS = method.Base64Encode(Encoding.GetEncoding("utf-8"), INSUADMDVS);
                        LEGREPCERTNO = method.Base64Encode(Encoding.GetEncoding("utf-8"), LEGREPCERTNO);
                        LEGREPNAME = method.Base64Encode(Encoding.GetEncoding("utf-8"), LEGREPNAME);
                        LEGREPTEL = method.Base64Encode(Encoding.GetEncoding("utf-8"), LEGREPTEL);

                    }

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(input);
                    lv1.SubItems.Add(EMPADDR);
                    lv1.SubItems.Add(EMPNAME);
                    //lv1.SubItems.Add(EMPNO);
                    lv1.SubItems.Add(INSUADMDVS);
                    lv1.SubItems.Add(LEGREPCERTNO);
                    lv1.SubItems.Add(LEGREPNAME);
                    lv1.SubItems.Add(LEGREPTEL);

                    Thread.Sleep(100);
                    if (listView1.Items.Count > 2)
                    {
                        this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                    }


                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    if (status == false)
                        return;

                }
                MessageBox.Show("完成");


            }
            catch (Exception ex)
            {


                MessageBox.Show(ex.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请先导入表格");
                return;
            }

            if (status == true)
            {
                status = false;
                label3.Text = "已停止";
            }
            else
            {
                status = true;
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(run);
                    thread.Start();
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {

                zanting = true;
            }
            else
            {
                zanting = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "14752479")
            {
                MessageBox.Show("密码错误");
                return;
            }


            zanting = false;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                for (int j = 1; j < listView1.Columns.Count; j++)
                {
                    try
                    {

                        if (jiami == false)
                        {
                            if (j != 0)
                            {
                                listView1.Items[i].SubItems[j].Text = method.Base64Encode(Encoding.GetEncoding("utf-8"), listView1.Items[i].SubItems[j].Text);
                            }

                        }
                        else
                        {
                            if (j != 0)
                            {
                                listView1.Items[i].SubItems[j].Text = method.Base64Decode(Encoding.GetEncoding("utf-8"), listView1.Items[i].SubItems[j].Text);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                       
                        continue;
                    }
                }
                zanting = true;

                if (jiami == false)
                {
                    jiami = true;
                }
                else
                {
                    jiami = false;
                }

            }
        }

        private void 医保服务查询_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }





    }
}
