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

namespace 电信查询
{
    public partial class 电信查询 : Form
    {
        public 电信查询()
        {
            InitializeComponent();
        }

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
                WebHeaderCollection headers = request.Headers;
                headers.Add("X-CSRF-TOKEN:35ed0d90-5ecf-4d61-ba3f-a2db1fdfaba5");
                //headers.Add("sec-fetch-site:same-origin");
                //headers.Add("sec-fetch-user:?1");
                //headers.Add("upgrade-insecure-requests: 1");
                //添加头部
                //request.ContentType = "application/x-www-form-urlencoded";
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请先导入数据");
                return;
            }
            status = true;

            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
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

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        /// <summary>
        /// 获取宽带
        /// </summary>
        /// <param name="custid"></param>
        /// <returns></returns>
        public string getPackage(string custid)
        {
            string url = "http://135.224.117.8/portal-web/cust/CustViewModuleController/qryPackageOffer.do";
            string postdata = "{\"blockIds\":[\"saleProductsInfo\",\"saleProductsMobileInfo\",\"saleProductsBroadbandInfo\",\"saleProductsFixedPhoneInfo\",\"saleProductsITVInfo\",\"saleProductsOtheInfo\",\"saleProductsInvalidOffer\"],\"custId\":\""+custid+"\",\"lanId\":\"1010\",\"searchT\":\"-1\"}";

            string html = PostUrlDefault(url,postdata,cookie);
            html = Regex.Match(html, @"""saleProductsFixedPhoneInfo""([\s\S]*?)saleProductsBroadbandInfo").Groups[1].Value;
            return html;
}

        /// <summary>
        /// 获取更多合同
        /// </summary>
        /// <param name="custid"></param>
        /// <returns></returns>
        public string getPackage2(string custid)
        {
            string url = "http://135.224.117.8/portal-web/cust/CustViewModuleController/qryPackageOffer.do";
            string postdata = "{\"blockIds\":[\"saleProductsInfo\"],\"custId\":\""+custid+"\",\"lanId\":\"1010\",\"searchT\":\"-1\",\"pageIndex\":1,\"pageSize\":30}";

            string html = PostUrlDefault(url, postdata, cookie);
            
            return html;
        }

        string cookie = "bss3_portal_index=1; ZSMART_LOCALE=zh; bss3_loginIp=135.224.81.252; loginIp=135.224.81.252; bss3_launchAppCode=GL-1507801713775; launchAppCode=GL-1507801713775; bss3_lanId=1010; lanId=1010; bss3_sysUserId=300000022075; sysUserId=300000022075; bss3_sysUserCode=XIATIAN; sysUserCode=XIATIAN; bss3_orgId=6501041292995; orgId=6501041292995; bss3_orgPath=65650000.65650100.65010021.98039372.6501041292995; orgPath=65650000.65650100.65010021.98039372.6501041292995; bss3_salesFisrtClass=\"\"; salesFisrtClass=\"\"; CLOUD_APP_NAME=crm_xjproductportalweb; CLOUD_APP_ID=27; SESSION=ebb88f25-487e-4878-9db0-d40260d5c0ea; showDoubleTopheader=y";
        
        public void run()
        {
            List<string> htlist = new List<string>();
            string[] text = textBox4.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            foreach (string ht in text)
            {
                if(ht.Trim()!="")
                {
                    htlist.Add(ht.Trim()); 
                }
            }





          
            try
            {
                for (int a = 0; a < dt.Rows.Count; a++)
                {
                   

                    DataRow dr = dt.Rows[a];
                    string uid = dr[0].ToString();
                    label4.Text = "正在查询第"+a+"个"+uid;
                    if (status == false)
                        return;
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                    string url = "http://135.224.117.8/portal-web/cust/ProdCustViewModuleController/qryCustInfo.do";
                    string postdata = "{\"lanId\":\"1010\",\"searchType\":\"-1\",\"searchValue\":\""+uid+"\",\"pageIndex\":\"1\",\"pageSize\":\"10\"}";
                    string html = PostUrlDefault(url,postdata,cookie);
                    textBox2.Text = html;
                    string custId = Regex.Match(html, @"""custId"":""([\s\S]*?)""").Groups[1].Value;
                    string custAddress = Regex.Match(html, @"""custAddress"":""([\s\S]*?)""").Groups[1].Value;
                    string contactName = Regex.Match(html, @"""contactName"":""([\s\S]*?)""").Groups[1].Value;

                    string ahtml = getPackage(custId);
                     MatchCollection kdzhanghao = Regex.Matches(ahtml, @"""accNbr"":""([\s\S]*?)""");
                    MatchCollection kdaddr = Regex.Matches(ahtml, @"""addressDesc"":""([\s\S]*?)""");


                    string bhtml = getPackage2(custId);

                    MatchCollection father= Regex.Matches(bhtml, @"""actionOfferRule""([\s\S]*?)mainFlag");
                    MatchCollection date = Regex.Matches(bhtml, @"""statusDate""([\s\S]*?)children");
                    MatchCollection ht = Regex.Matches(bhtml, @"""children""([\s\S]*?)actionOfferRule");


                    

                    textBox3.Text = bhtml;
                    for (int j= 0; j < kdzhanghao.Count; j++)
                    {
                    

                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(uid);
                        lv1.SubItems.Add(custId);
                        lv1.SubItems.Add(contactName);
                        lv1.SubItems.Add(kdzhanghao[j].Groups[1].Value);
                        lv1.SubItems.Add(kdaddr[j].Groups[1].Value);
                        lv1.SubItems.Add("");
                        lv1.SubItems.Add("");
                        lv1.SubItems.Add("");
                        lv1.SubItems.Add("");
                    }
                    for (int j = 0; j < father.Count; j++)
                    {
                      
                        string htname = Regex.Match(father[j].Groups[1].Value, @"""offerName"":""([\s\S]*?)""").Groups[1].Value;
                        string hthao = Regex.Match(ht[j].Groups[1].Value, @"""acctCd"":""([\s\S]*?)""").Groups[1].Value;
                        string httime1 = Regex.Match(date[j].Groups[1].Value, @"""effDate"":""([\s\S]*?)""").Groups[1].Value;
                        string httime2 = Regex.Match(date[j].Groups[1].Value, @"""expDate"":""([\s\S]*?)""").Groups[1].Value;
                        if (htlist.Contains(htname))
                        {
                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(uid);
                            lv1.SubItems.Add(custId);
                            lv1.SubItems.Add(contactName);
                            lv1.SubItems.Add("");
                            lv1.SubItems.Add("");
                            lv1.SubItems.Add(htname);
                            lv1.SubItems.Add(hthao);
                            lv1.SubItems.Add(httime1);
                            lv1.SubItems.Add(httime2);
                        }
                        else
                        {
                            label4.Text = "正在查询" + uid+" 合同名称不符合";
                        }
                    }

                  
                    if (listView1.Items.Count > 2)
                    {
                        this.listView1.Items[this.listView1.Items.Count - 1].EnsureVisible();
                    }


                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }



        }

        private void 电信查询_Load(object sender, EventArgs e)
        {

        }

        private void 电信查询_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
