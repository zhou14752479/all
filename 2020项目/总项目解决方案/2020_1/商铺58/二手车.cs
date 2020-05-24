using System;
using System.Collections;
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

namespace 商铺58
{
    public partial class 二手车 : Form
    {
        private bool zanting;

        public 二手车()
        {
            InitializeComponent();
        }


        ArrayList tels = new ArrayList();

        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string COOKIE = "id58=Ch8BCFzCmzIkdB4WK5XDAg==; 58tj_uuid=a741f941-0d76-4ad4-aca7-5009322f93ed; _ga=GA1.2.339422915.1556257586; als=0; wmda_uuid=70c7fd7fd7b308b2e6a6206def584118; gr_user_id=18275427-2ddc-4696-8411-a40f183ffda9; xxzl_deviceid=bstJ2gEkRsCFq7pCuBr4SAESHKWXUAyqCxRFcdNKImCFNgUsbEWr9c0wqpe3Lbdy; xxzl_smartid=ec64668edf0cea7ecc1cc570a6df6ee4; Hm_lvt_5a7a7bfd6e7dfd9438b9023d5a6a4a96=1559876130; cookieuid1=e87rjVz52Aq4OEPbBQziAg==; mcity=suqian; mcityName=%E5%AE%BF%E8%BF%81; nearCity=%5B%7B%22cityName%22%3A%22%E4%B8%8A%E6%B5%B7%22%2C%22city%22%3A%22sh%22%7D%2C%7B%22cityName%22%3A%22%E5%AE%BF%E8%BF%81%22%2C%22city%22%3A%22suqian%22%7D%5D; __utma=253535702.339422915.1556257586.1564796955.1564796955.1; __utmz=253535702.1564796955.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none); Hm_lvt_295da9254bbc2518107d846e1641908e=1564883078; Hm_lvt_e2d6b2d0ec536275bb1e37b421085803=1565490373; city=suqian; 58home=suqian; wmda_visited_projects=%3B6333604277682%3B1409632296065%3B1732038237441%3B1731916484865%3B2385390625025%3B1732030748417%3B1444510081921%3B6289197098934; __xsptplus8=8.1.1569640714.1569640719.2%234%7C%7C%7C%7C%7C%23%23v9ujfcpY5FwFv_20ewcr1QcMNVIf0l9x%23; Hm_lvt_3bb04d7a4ca3846dcc66a99c3e861511=1569641042; Hm_lvt_e15962162366a86a6229038443847be7=1569641042; xxzl_sid=\"6BxAzb-Sze-LZI-PcD-1aBGNWEeQ\"; xxzl_token=\"OjUrnPxK9PpKSH + MmdW9cafl6rKCJfN0XQsJprWodFu3t4845j386itwD8y0dYuMin35brBb//eSODvMgkQULA==\"; new_uv=30; utm_source=; spm=; init_refer=; new_session=0; xxzl_cid=6924f4ad6a79459fa6729679775b5c15; xzuid=f4c51223-1774-4118-ab41-2a45f9d60044; ppStore_fingerprint=4EB12C202E83C6B4F5E0862F611827369C8C45E085D33781%EF%BC%BF1573896390005";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36";

                request.AllowAutoRedirect = true;
                request.Headers.Add("Cookie", COOKIE);
                request.KeepAlive = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.Timeout = 5000;
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string content = reader.ReadToEnd();

                reader.Close();
                response.Close();
                return content;

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
            return "";
        }
        #endregion
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {

            string cityid = textBox1.Text;
            try
            {
                string url = "https://www.hx2car.com/mobile/filteDataNew.json?areaCode="+cityid+"&pageSize=9999&currPage=1&order=&priceInterval=&year=&carType=&standards=&dayInterval=&colors=&gears=&is4s=&pifa=&mileage=&carKinds=&bodType=&factory=&country=&motor=&devicetoken=mini_UFQAjkRSAck&newCar=&appmobile=17606117606&apptoken=450ce158c523820bd204a841c542c716&editionFlag=1&serial=";



                string html = GetUrl(url);

                MatchCollection ids = Regex.Matches(html, @"\{""id"":1([\s\S]*?),");
                MatchCollection titles = Regex.Matches(html, @"""seriesBrandCarStyle"":""([\s\S]*?)""");
                MatchCollection names = Regex.Matches(html, @"""loginName"":""([\s\S]*?)""");
               


                for (int i = 0; i < ids.Count; i++)
                {


                    string URL = "https://www.hx2car.com/mobile/carDetail.json?id=1" + ids[i].Groups[1].Value;

                    string ahtml = GetUrl(URL);
                    Match tel = Regex.Match(ahtml, @"""mobile"":""([\s\S]*?)""");
                    if (!tels.Contains(tel.Groups[1].Value))
                    {
                        if (checkBox1.Checked == true)
                        {
                            tels.Add(tel.Groups[1].Value);
                        }




                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据   
                        lv1.SubItems.Add(titles[i].Groups[1].Value);
                        lv1.SubItems.Add(names[i].Groups[1].Value);
                        lv1.SubItems.Add(tel.Groups[1].Value);





                        while (zanting == false)
                        {
                            Application.DoEvents();//等待本次加载完毕才执行下次循环.
                        }
                        if (status == false)
                            return;

                        Thread.Sleep(100);
                    }
                }


            }


            catch (Exception)
            {

                throw;
            }
        }


        bool status = true;
        private void 二手车_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = GetUrl("http://www.acaiji.com/index/index/vip.html");

            if (html.Contains(@"58shangjia"))
            {
                status = true;
                button1.Enabled = false;
                zanting = true;
                Thread thread1 = new Thread(new ThreadStart(run));
                thread1.Start();
                Control.CheckForIllegalCrossThreadCalls = false;

            }

            else
            {
                MessageBox.Show("验证失败");
                return;
            }


            #endregion
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
