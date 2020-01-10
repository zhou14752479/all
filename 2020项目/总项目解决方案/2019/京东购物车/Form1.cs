using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 京东购物车
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36";

                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string content = reader.ReadToEnd();

                reader.Close();
                response.Close();
                return content;

            }
            catch (System.Exception ex)
            {
                ex.ToString();

            }
            return "";
        }
        #endregion
        bool zanting = true;
        public static string COOKIE= "__jdu=156160947285147253463; shshshfpb=jMIbgd9wbtSimsYA8lg7WDw%3D%3D; shshshfpa=0e3caa30-b4bf-a1c6-66b7-2bf518aaf940-1561609475; __jdv=76161171|direct|-|none|-|1571818595878; areaId=12; ipLoc-djd=12-933-3407-0; user-key=7987c80f-e94f-42f6-b44f-be6e474e96fb; TrackID=1mQ4i_yJwQYlG7eianrBoEIGuSQmUZglgybIXSA4jkcPYcimyI4j4sysRN4eVmPLeRPpGjaDqMFSs6t9IBa6MfyiN_fbzK2keKtrpYIGXhSE; pinId=wBSTpSnGmOea2wq-tGfAabV9-x-f3wj7; pin=jd_7a668be69efce; unick=%E5%91%A8%E5%87%AF%E6%AD%8C926; ceshi3.com=103; _tp=Di7eJDNFrmd1yz1kW%2F%2BsJm9I0i6a1fTtvQSGK2Uil64%3D; _pst=jd_7a668be69efce; cart-main=xx; __jda=122270672.156160947285147253463.1561609473.1568710992.1571818596.10; __jdc=122270672; 3AB9D23F7A4B3C9B=AKQZ7O3A4AP2RJFVQZCCBVUZNBY5VIBNRY4UKATNHVLFYR6LHPDYIHX3KC4A6TRJXJYGPQGURXXHDJAMCVRFFRUAPY; cd=0; cn=20; __jdb=122270672.9.156160947285147253463|10.1571818596; shshshfp=72fe982229299654e71c36e0c207f057; shshshsID=3c062315c22679f38a98df25a9265dfb_6_1571819272709; thor=ED9083D596B1BF34B43E3F2B584554DCE2AAFEFE994B3AF7C0B27B21E7A068263F6F7421926242D070F5F335BD3089D46946DE7FE5F68D8A5E3C2230754FE7915B0C33A859509442E7CF6A66E9C1717CD19545051DF8236EFBBE105453730076165F4B9DEB16516402205CEB7A0BE8D9E3727AF2D72B4B60EE45A419D2127560B5CB95DA3477CA470A1762DAB9C4EFB527ECC14680FB8DCD57D93A4DA7DCB6A6";
        #region  京东购物车价格
        public void run()
        {
            try
            {
                string html = method.GetUrlWithCookie("https://cart.jd.com/cart.action",COOKIE, "utf-8");
                Match uid = Regex.Match(html, @"id=""allSkuIds"" value=""([\s\S]*?)""");
                string[] uids = uid.Groups[1].Value.Split(new string[] { "," }, StringSplitOptions.None);
                if (uids.Length == 0)
                {
                    MessageBox.Show("获取购物车宝贝失败");
                    return;
                }
                for (int i = 0; i < uids.Length; i++)
                {

                    string JDurl = "https://c0.3.cn/stock?skuId=" + uids[i] + "&area=12_933_3407_0&venderId=1000000140&buyNum=1&choseSuitSkuIds=&cat=670,671,1105&ch=1&callback=jQuery8851179";
                   
                    string JDhtml = GetUrl(JDurl, "gb2312");
                    textBox1.Text = JDurl;
                    Match jdprice = Regex.Match(JDhtml, @"""p"":""([\s\S]*?)""");

                    ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                    listViewItem.SubItems.Add(uids[i]);
                    listViewItem.SubItems.Add(jdprice.Groups[1].Value);

                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                    Thread.Sleep(1000);


                }

            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }





        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
