using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 电信套餐查询
{
    public partial class 电信套餐查询 : Form
    {
        public 电信套餐查询()
        {
            InitializeComponent();
        }

        private void 电信套餐查询_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("http://bss30.sntele.cn:8002/portal/index.jsp?param=F4A45485EC022429806765F41F5BDC20572029A25E9EC45DC60E29DB50C0FEF030CB59401EE0EE5B1252E08FD987C88C75F22E926D10A31D9574AF92595E3A840807A0D1E8B9873363045C74831C9D5C17FFCDC854840523EED6FA54D360618EB430C4419EF1B6D8#");
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
            tabControl1.SelectedIndex = 1;
        }
        bool zanting = true;
        string cookie = "JSESSIONID=xv7crorFA5NQ9Mucms1lVpX2Glrjg8YJlhwBGCag-g71Wrd6tnIk!-1701530529";
        Thread thread;
        bool status = true;

        public string creatRequestId()
        {
            try
            {

                string url = "http://bss30.sntele.cn:8002/SmartBssWeb/integrateQuery/creatRequestId.s";
                string postdata = "{\"latnId\":\"290\"}";
                string html = method.PostUrl(url, postdata, cookie, "utf-8", "application/json;", "");
                string requestId = Regex.Match(html, @"requestId"":""([\s\S]*?)""").Groups[1].Value;

                return requestId;


            }
            catch (Exception)
            {

                throw;
            }
        }
        public string creatCUSTID(string mobile)
        {
            try
            {
                //string aurl = "http://bss30.sntele.cn:8002/SmartBssWeb/integrateQuery/QryCertiNbrInfo.s";
                //string apostdata = "{\"latnId\":\"290\",\"certiNbr\":\"" + mobile + "\"}";
                //method.PostUrl(aurl, apostdata, cookie, "utf-8", "application/json;", "");

                //string url = "http://bss30.sntele.cn:8002/SmartBssWeb/integrateQuery/creatRequestId.s";
                //string postdata = "{\"latnId\":\"290\"}";
                //string html = method.PostUrl(url, postdata, cookie, "utf-8", "application/json;", "");
                //string requestId = Regex.Match(html, @"requestId"":""([\s\S]*?)""").Groups[1].Value;


                string url = "http://bss30.sntele.cn:8002/SmartBssWeb/integrateQuery/qryCustList.s";
                string postdata = "{\"latnId\":\"290\",\"custId\":\""+mobile+"\",\"conditionType\":5,\"channelId\":\"55346759\",\"staffId\":\"1100551588\",\"IfPriHide\":\"1\"}";
                string html = method.PostUrl(url, postdata, cookie, "utf-8", "application/json;", "");
                string CUSTID = Regex.Match(html, @"""CUSTID"":([\s\S]*?),").Groups[1].Value;



                return CUSTID;


            }
            catch (Exception)
            {

                throw;
            }
        }

        public void qryCustList()
        {
            cookie = method.GetCookies("http://bss30.sntele.cn:8002/SmartBssWeb/integrateQuery/qryCustList.s");
            if (cookie == "")
            {
                MessageBox.Show("请先登录");
                return;
            }
            try
            {

                label2.Text =DateTime.Now.ToLongTimeString()+ "：开始查询....";
                StreamReader sr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int a = 0; a < text.Length; a++)
                {
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    if (status == false)
                        return;
                    label2.Text = DateTime.Now.ToLongTimeString() + "：开始查询...."+text[a];
                    if (text[a].Trim() == "")
                    {
                        continue;
                    }
                    string mobile = text[a].Trim();
                    string requestId = creatRequestId();
                    string custid = creatCUSTID(mobile);
                    string url = "http://bss30.sntele.cn:8002/SmartBssWeb/integrateQuery/QueryIMInfo.s";
                    string postdata = "{\"methedType\":\"orderedPromote\",\"latnId\":\"290\",\"custId\":\"" + custid + "\",\"requestId\":\"" + requestId + "\"}";

                    string html = method.PostUrl(url, postdata, cookie, "utf-8", "application/json;", "");
                    MatchCollection ACCNBR = Regex.Matches(html, @"""ACCNBR"":""([\s\S]*?)""");
                    MatchCollection MAINOFFERNAME = Regex.Matches(html, @"""MAINOFFERNAME"":""([\s\S]*?)""");
                   // MatchCollection PROMOTETYPENAMEZD = Regex.Matches(html, @"""PROMOTETYPENAMEZD"":""([\s\S]*?)""");
                    MatchCollection PROMOTENAME = Regex.Matches(html, @"""PROMOTENAME"":""([\s\S]*?)""");
                    MatchCollection EFFDATE = Regex.Matches(html, @"""EFFDATE"":""([\s\S]*?)""");
                    MatchCollection EXPDATE = Regex.Matches(html, @"""EXPDATE"":""([\s\S]*?)""");

                    if (ACCNBR.Count == 0)
                        continue;

                    StringBuilder zhusb = new StringBuilder();
                    StringBuilder cusb = new StringBuilder();
                    StringBuilder timesb1 = new StringBuilder();
                    StringBuilder timesb2 = new StringBuilder();

                    for (int i = 0; i < ACCNBR.Count; i++)
                    {
                        try
                        {
                            zhusb.Append(MAINOFFERNAME[i].Groups[1].Value+"\n");

                            if (PROMOTENAME[i].Groups[1].Value.Contains("套餐") || PROMOTENAME[i].Groups[1].Value.Contains("翼支付") || PROMOTENAME[i].Groups[1].Value.Contains("可视门铃橙分期") || PROMOTENAME[i].Groups[1].Value.Contains("智家分期合约") || PROMOTENAME[i].Groups[1].Value.Contains("陕西云手机促销") || PROMOTENAME[i].Groups[1].Value.Contains("微信分免押租用"))
                            {
                                cusb.Append(PROMOTENAME[i].Groups[1].Value+"\n");
                                timesb1.Append(EFFDATE[i].Groups[1].Value+"\n");
                                timesb2.Append(EXPDATE[i].Groups[1].Value + "\n");

                                //ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                                //lv1.SubItems.Add(ACCNBR[i].Groups[1].Value);
                                //lv1.SubItems.Add(MAINOFFERNAME[i].Groups[1].Value);
                                //lv1.SubItems.Add(PROMOTENAME[i].Groups[1].Value);
                                //lv1.SubItems.Add(EFFDATE[i].Groups[1].Value);
                                //lv1.SubItems.Add(EXPDATE[i].Groups[1].Value);                    
                                
                            }
                            
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                            continue;
                        }
                    }
                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(mobile);
                    lv1.SubItems.Add(zhusb.ToString());
                    lv1.SubItems.Add(cusb.ToString());
                    lv1.SubItems.Add(timesb1.ToString());
                    lv1.SubItems.Add(timesb2.ToString());

                    Thread.Sleep(100);
                }
                label2.Text = DateTime.Now.ToLongTimeString() + "：查询结束";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString()); ;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {

          
            if (textBox1.Text == "")
            {
                MessageBox.Show("请先导入txt文本");
                return;
            }


            #region 通用检测

            string html = method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"IepkOu"))
            {

                return;
            }


            #endregion

            status = true;

            
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(qryCustList);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
               
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
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

        private void button6_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (DateTime.Now > Convert.ToDateTime("2021-12-01"))
            {
                return;
            }
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void 电信套餐查询_FormClosing(object sender, FormClosingEventArgs e)
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

     
    }
}
