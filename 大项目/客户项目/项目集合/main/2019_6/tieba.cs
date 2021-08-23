//using Aliyun.Acs.Core;
//using Aliyun.Acs.Core.Http;
//using Aliyun.Acs.Core.Profile;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main._2019_6
{
    public partial class tieba : Form
    {
        public tieba()
        {
            InitializeComponent();
        }

        private void Tieba_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            timer1.Start();
            timer1.Interval = Convert.ToInt32(textBox4.Text) * 1000 * 60;
        }
        bool zanting = true;
        int index { get; set; }
        #region 主程序
        public void run()
        {

            try
            {
                string[] keywords = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                foreach (string keyword in keywords)
                {
                    string key= System.Web.HttpUtility.UrlEncode(keyword, Encoding.GetEncoding("GB2312"));

                    string Url = "http://tieba.baidu.com/f?kw="+key;

                    string html = method.GetUrl(Url, "utf-8");
                    if (html == null)
                        break;
                    MatchCollection ids = Regex.Matches(html, @"data-tid='([\s\S]*?)'", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    ArrayList lists = new ArrayList();

                    foreach (Match id in ids)
                    {
                        lists.Add("http://tieba.baidu.com/p/" + id.Groups[1].Value);
                    }
                   
                    foreach (string list in lists)

                    {
                        
                        string strhtml = method.GetUrl(list, "utf-8");
                        string[] Keys = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                       
                       
                        foreach (string KEY in Keys)
                        {
                            label7.Text = list;
                         

                           
                            if (strhtml.Trim().Contains(KEY.Trim()))
                            {
                                this.index = this.dataGridView1.Rows.Add();
                                this.dataGridView1.Rows[index].Cells[0].Value = index.ToString();
                                this.dataGridView1.Rows[index].Cells[1].Value = list;
                                this.dataGridView1.Rows[index].Cells[2].Value = "是";
                               
                                //sendMsg(textBox3.Text.Trim());
                                
                            }
                           
                        }

                       
                           
                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
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

        
        //public  void sendMsg(string mobile)
        //{
            
        //    IClientProfile profile = DefaultProfile.GetProfile("default", "LTAIKq0r0wXeCOO7", "U2IDaGVEH3cl9Xsv9RKnodi5BA0ZML");
        //    DefaultAcsClient client = new DefaultAcsClient(profile);
        //    CommonRequest request = new CommonRequest();
        //    request.Method = MethodType.POST;
        //    request.Domain = "dysmsapi.aliyuncs.com";
        //    request.Version = "2017-05-25";
        //    request.Action = "SendSms";
        //    // request.Protocol = ProtocolType.HTTP;
        //    request.AddQueryParameters("SignName", "舆情宝");
        //    request.AddQueryParameters("PhoneNumbers", mobile);
        //    request.AddQueryParameters("TemplateCode", "SMS_167963792");
        //    request.AddQueryParameters("TemplateParam", "{'code':'8888',}");
        //    try
        //    {
        //        CommonResponse response = client.GetCommonResponse(request);
        //        Console.WriteLine(System.Text.Encoding.Default.GetString(response.HttpResponse.Content));
               
        //    }
        //    catch (ServerException ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //    label7.Text = "已成功发送消息！";
        //}
        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            label7.Text = "正在运行....";

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
           
            //sendMsg(textBox3.Text.Trim());
            
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            run();
        }
    }
}
