using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 主程序202401
{
    public partial class 添加账号 : Form
    {
        public 添加账号()
        {
            InitializeComponent();
        }
        static long GetTimestamp(DateTime date)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)(date.ToUniversalTime() - epoch).TotalSeconds;
        }

     
        /// <summary>
        /// 获取账号的广告ID
        /// </summary>
        /// <returns></returns>
        public string getad_id(string cookie)
        {


            try
            {




                string url = "https://business.oceanengine.com/bp/api/promotion/promotion_bentham/get_standard_account_list";
                DateTime now = DateTime.Now;
                DateTime midnight = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);

                DateTime now1 = DateTime.Now.AddDays(1);
                DateTime midnight1 = new DateTime(now1.Year, now1.Month, now1.Day, 0, 0, 0);

                string today = GetTimestamp(midnight).ToString();
                string today1 = GetTimestamp(midnight1).ToString();


                string postdata = "{\"startTime\":\"" + today + "\",\"endTime\":\"" + today1 + "\",\"cascadeMetrics\":[\"advertiser_name\",\"advertiser_id\",\"advertiser_followed\"],\"fields\":[\"stat_cost\",\"show_cnt\",\"click_cnt\",\"video_oto_pay_order_stat_amount\",\"dy_like\",\"dy_comment\",\"dy_share\",\"dy_collect\"],\"orderField\":\"stat_cost\",\"orderType\":1,\"offset\":1,\"limit\":10,\"accountType\":10168,\"filter\":{\"pricingCategory\":[2],\"advertiser\":{},\"group\":{},\"campaign\":{}},\"platformVersion\":\"2\"}";


                string html = method.PostUrl(url, postdata, cookie, "utf-8", "application/json", "");

                
                string advertiser_id = Regex.Match(html, @"""advertiser_id"":""([\s\S]*?)""").Groups[1].Value;
                return advertiser_id;


            }
            catch (Exception ex)
            {
                return "";
            }


        }



        /// <summary>
        /// 获取账号的广告ID
        /// </summary>
        /// <returns></returns>
        public static string get_Moduleid(string cookie,string adid,string frameid)
        {


            try
            {




                string url = "https://localads.chengzijianzhan.cn/api/lamp/pc/v2/statistics/data/pageMetrics?advid="+adid+"&frameId="+frameid;
             

                string html = method.GetUrlWithCookie(url,cookie, "utf-8");


                string ModuleId = Regex.Matches(html, @"""ModuleId"":""([\s\S]*?)""")[1].Groups[1].Value;
                return ModuleId;


            }
            catch (Exception ex)
            {
                return "";
            }


        }



        public void readTxt()
        {
            listView1.Items.Clear();    
            StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\cookie.txt", method.EncodingType.GetTxtType(AppDomain.CurrentDomain.BaseDirectory + "\\cookie.txt"));
            //一次性读取完 
            string texts = sr.ReadToEnd();

            sr.Close();  //只关闭流
            sr.Dispose();
            

            string[] text = texts.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);

            for (int a = 0; a < text.Length; a++)
            {


                string[] aaa = text[a].Split(new string[] { "###" }, StringSplitOptions.None);
                string beizhu = aaa[0].Trim();
                string ad_id = aaa[1].Trim();
               


                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                lv1.SubItems.Add(beizhu);
                lv1.SubItems.Add(ad_id);

            }

        }


        public void deleteTxT(string key)
        {
            try
            {

                // 读取文件所有行
                string[] lines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\cookie.txt");

                // 过滤不包含关键字的行（区分大小写）
                var filteredLines = lines.Where(line => !line.Contains(key)).ToArray();

                // 写回原文件（覆盖）
                File.WriteAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\cookie.txt", filteredLines);


            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("错误：文件未找到！");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if(textBox1.Text=="" || textBox2.Text =="")
            {
                MessageBox.Show("值为空");
                return;
            }


            string name = textBox1.Text.Trim();
            string cookie = textBox2.Text.Trim();
            string ad_id = getad_id(cookie);

            
            if(ad_id=="")
            {
                MessageBox.Show("cookie无效或不存在广告账户");
                return;
            }

            deleteTxT(name);



            FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\cookie.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
            sw.WriteLine(name+"###"+ ad_id + "###"+cookie);
            sw.Close();
            fs1.Close();
            sw.Dispose();

            MessageBox.Show("添加成功");
            readTxt();
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            deleteTxT(textBox1.Text.Trim());
        }

        private void 添加账号_Load(object sender, EventArgs e)
        {
            readTxt();
        }

        private void 删除账号ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
               
                string beizhu = listView1.SelectedItems[0].SubItems[1].Text;

                deleteTxT(beizhu);
                MessageBox.Show("删除成功");

            }
            else
            {
                MessageBox.Show("请选择一行数据");
            }

        }
    }
}
