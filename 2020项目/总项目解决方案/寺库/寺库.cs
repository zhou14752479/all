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
using helper;
namespace 寺库
{
    public partial class 寺库 : Form
    {
        public 寺库()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 主程序
        /// </summary>
        public void run()
        {
            try
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("请输入网址");
                    return;


                }
                string[] texts = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                foreach (string text in texts)
                {

                    Match pid = Regex.Match(text, @"\d{6,}");

                    string url = "https://las.secoo.com/api/product/detail_new?upk=&productId=" + pid.Groups[0].Value + "&size=2&c_platform_type=1&c_app_ver=7.5.1&callback=jsonp1";
                    string html = method.GetUrl(url, "utf-8");

                    Match title = Regex.Match(html, @"发现了仅售([\s\S]*?)的([\s\S]*?)""");
                    MatchCollection pics = Regex.Matches(html, @"""info"":""([\s\S]*?)""");
                    Match a1 = Regex.Match(html, @"""nowPriceNum"":""([\s\S]*?)""");
                    Match a2 = Regex.Match(html, @"商品编号"",""value"":""([\s\S]*?)""");
                    Match a3 = Regex.Match(html, @"是否带锁"",""value"":""([\s\S]*?)""");
                    Match a4 = Regex.Match(html, @"内衬材质"",""value"":""([\s\S]*?)""");
                    Match a5 = Regex.Match(html, @"提拎部件类型"",""value"":""([\s\S]*?)""");
                    Match a6 = Regex.Match(html, @"适用性别"",""value"":""([\s\S]*?)""");
                    Match a7 = Regex.Match(html, @"图案"",""value"":""([\s\S]*?)""");
                    Match a8 = Regex.Match(html, @"商品大小"",""value"":""([\s\S]*?)""");
                    Match a9 = Regex.Match(html, @"箱包尺寸"",""value"":""([\s\S]*?)""");
                    Match a10 = Regex.Match(html, @"材质"",""value"":""([\s\S]*?)""");
                    Match a11 = Regex.Match(html, @"尺寸"",""value"":""([\s\S]*?)""");


















                    string path = textBox2.Text + "\\" + title.Groups[2].Value.Replace("\\", "").Replace("/", "") + "\\";
                    if (false == System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);

                    }


                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据         
                    lv1.SubItems.Add(title.Groups[2].Value);
                    lv1.SubItems.Add(a1.Groups[1].Value);
                    lv1.SubItems.Add(a2.Groups[1].Value);
                    lv1.SubItems.Add(a3.Groups[1].Value);
                    lv1.SubItems.Add(a4.Groups[1].Value);
                    lv1.SubItems.Add(a5.Groups[1].Value);
                    lv1.SubItems.Add(a6.Groups[1].Value);
                    lv1.SubItems.Add(a7.Groups[1].Value);
                    lv1.SubItems.Add(a8.Groups[1].Value);
                    lv1.SubItems.Add(a9.Groups[1].Value);
                    lv1.SubItems.Add(a10.Groups[1].Value);
                    lv1.SubItems.Add(a11.Groups[1].Value);


                    for (int i = 0; i < pics.Count; i++)
                    {
                        string pic = pics[i].Groups[1].Value.Replace("\\", "");
                        method.downloadFile(pic, path, i + ".jpg");
                    }

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void 寺库_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                MessageBox.Show("请选择保存图片文件夹");
                return;
            }
            #region 通用验证

            bool value = false;
            string html = method.GetUrl("http://acaiji.com/success/ip.php", "utf-8");
            string localip = method.GetIP();
            MatchCollection ips = Regex.Matches(html, @"<td style='color:red;'>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (Match ip in ips)
            {
                if (ip.Groups[1].Value.Trim() == "10.10.10.10")
                {
                    value = true;
                    break;
                }

            }
            if (value == true)
            {
                Thread thread = new Thread(new ThreadStart(run));
                thread.Start();
               
                Control.CheckForIllegalCrossThreadCalls = false;
              


            }
            else
            {
                MessageBox.Show("IP不符");

            }
            #endregion
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath;
                textBox2.Text = foldPath;
               
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            web w1 = new web("https://jinlaike.net/wi_wxapp_home.php?version_id=3184");
            w1.Show();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
