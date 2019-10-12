using System;
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

namespace main._2019_10
{
    public partial class 淘宝联盟 : Form
    {
        public 淘宝联盟()
        {
            InitializeComponent();
            登陆.COOKIE = "t=66b127d321b80e83458173989c887619; cna=8QJMFUu4DhACATFZv2JYDtwd; account-path-guide-s1=true; cookie2=124fbbb9b491ba0a4a8e55b68cc6b4f9; v=0; _tb_token_=e1e33be63538; alimamapwag=TW96aWxsYS81LjAgKFdpbmRvd3MgTlQgMTAuMDsgV2luNjQ7IHg2NCkgQXBwbGVXZWJLaXQvNTM3LjM2IChLSFRNTCwgbGlrZSBHZWNrbykgQ2hyb21lLzc0LjAuMzcyOS4xMDggU2FmYXJpLzUzNy4zNg%3D%3D; cookie32=2d392e569cc3a01381c91efc1d2578fd; alimamapw=TA8BWQAGBwAGAAQBOgkAAgUOV1NRUVcBAg8CU1MBUAoCVQJaB1NXBwEFUVNS; cookie31=MTI3MjA5NjM2LHprZzg1MjI2NjAxMCwxMDUyMzQ3NTQ4QGFsaW1hbWEuY29tLFRC; login=UtASsssmOIJ0bQ%3D%3D; rurl=aHR0cHM6Ly9wdWIuYWxpbWFtYS5jb20v; l=cB_ODS3eq4YS4UNzBOfNVQhfh87TuQdfGcVP2FyMGICP_YfDlHAAWZBEX1TkCnGVLsdkJ3oWYJ1uBj8gAy4EhGaMPgLQuv3P.; isg=BG1tKmYhzFItqqg9hqeMxvJMfAlL3queczU_BK9xNYQ0JonYdhnMbbC0ELpllblU";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            登陆 dl = new 登陆();
            dl.Show();
        }

        #region 获取推广位

        public void gett()
        {
            string html = method.GetUrlWithCookie("https://pub.alimama.com/common/adzone/newSelfAdzone2.json?tag=29&itemId=&blockId=", 登陆.COOKIE, "utf-8");
            MatchCollection  a1s = Regex.Matches(html, @"tagList"":\[\]\,""name"":""([\s\S]*?)"",""id"":([\s\S]*?)""");
            foreach (Match match in a1s)
            {
                comboBox1.Items.Add(match.Groups[1].Value+"-"+ match.Groups[2].Value);
            }
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.Text = comboBox1.Items[0].ToString();
            }
        }

        #endregion


        #region 获取佣金比例

        public string gety(string id)
        {
            string html = method.GetUrlWithCookie("https://pub.alimama.com/openapi/json2/1/gateway.unionpub/optimus.material.json?t=1570240748303&_data_=%7B\"floorId\"%3A\"20392\"%2C\"pageNum\"%3A0%2C\"pageSize\"%3A60%2C\"refpid\"%3A\"mm_127209636_0_0\"%2C\"variableMap\"%3A%7B\"fn\"%3A\"search\"%2C\"q\"%3A\"https%3A%2F%2Fdetail.tmall.com%2Fitem.htm%3Fid%3D"+id+"\"%2C\"_t\"%3A\"1570240747987\"%7D%7D", 登陆.COOKIE, "utf-8");
            Match a1 = Regex.Match(html, @"""calTkRate"":""([\s\S]*?)""");

            if (a1.Groups[1].Value == "")
            {
                return "未推广";
            }
            else
            {
                return (Convert.ToInt32(a1.Groups[1].Value) / 100).ToString();
            }
            
        }

        #endregion

        #region  淘宝联盟
        public void tb()
        {
            try
            {
                gett();
                Thread.Sleep(1000);
                string[] urls = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string url in urls)
                {
                    if (url != "")
                    {
                        Match uid = Regex.Match(url, @"\d{10,}");                //获取商品ID
                        Match tid = Regex.Match(comboBox1.Text, @"\d{10,}"); //获取推广位Id
                        string URL = "https://pub.alimama.com/openapi/param2/1/gateway.unionpub/shareitem.json?t=1570239508002&shareUserType=1&unionBizCode=union_pub&shareSceneCode=item_search&materialId=" + uid.Groups[0].Value + "&tkClickSceneCode=qtz_pub_search&siteId=913050312&adzoneId=" + tid.Groups[0].Value + "&bypage=1&extendMap=%7B%22qtzParam%22%3A%7B%22lensId%22%3A%22OPT%401570239087%400b1a25c0_0ea0_16d998b01ec_95dc%4001%22%7D%7D&materialType=1&needQueryQtz=true";

                      
                        string strhtml = method.GetUrlWithCookie(URL, 登陆.COOKIE, "utf-8");


                        Match a1 = Regex.Match(strhtml, @"shortLinkInfo([\s\S]*?)url"":""([\s\S]*?)""");
                        Match a2 = Regex.Match(strhtml, @"shortLinkInfo([\s\S]*?)""couponUrl"":""([\s\S]*?)""");



                        ListViewItem listViewItem = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                        if (gety(uid.Groups[0].Value) == "未推广")
                        {

                            listViewItem.SubItems.Add(url);
                            listViewItem.SubItems.Add("未推广");
                            listViewItem.SubItems.Add("未推广");
                            listViewItem.SubItems.Add("未推广");
                            continue;
                        }
                        listViewItem.SubItems.Add(url);
                        listViewItem.SubItems.Add(gety(uid.Groups[0].Value) + "%");

                        if (a1.Groups[2].Value == "")
                        {
                            listViewItem.SubItems.Add("无");
                        }
                        else
                        {
                            listViewItem.SubItems.Add(a1.Groups[2].Value);
                        }

                        if (a2.Groups[2].Value == "")
                        {
                            listViewItem.SubItems.Add("无");
                        }
                        else
                        {
                            listViewItem.SubItems.Add(a2.Groups[2].Value);
                        }

                       



                        Thread.Sleep(Convert.ToInt32(textBox2.Text));
                    }
                }
                //MessageBox.Show(登陆.COOKIE);
                //string html = method.GetUrlWithCookie("https://pub.alimama.com/manage/overview/index.htm",登陆.COOKIE,"utf-8");
                //textBox1.Text = html;


            }
            catch (Exception ex)
            {
              ex.ToString();
            }
        }


        #endregion

        private void button3_Click(object sender, EventArgs e)
        {

            #region 通用验证

            bool value = false;
            string html = method.GetUrl("http://acaiji.com/success/ip.php", "utf-8");
            string localip = method.GetIP();
            MatchCollection ips = Regex.Matches(html, @"<td style='color:red;'>([\s\S]*?)</td>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (Match ip in ips)
            {
                if (ip.Groups[1].Value.Trim() == "18.18.18.18")
                {
                    value = true;
                    break;
                }

            }
            if (value == true)
            {
                Thread thread = new Thread(new ThreadStart(tb));
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;



            }
            else
            {
                MessageBox.Show("IP不符");

            }
            #endregion

        }

        private void 打开佣金链接ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(this.listView1.SelectedItems[0].SubItems[3].Text);
        }

        private void 打开领券链接ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(this.listView1.SelectedItems[0].SubItems[4].Text);
        }

      

        private void 淘宝联盟_Load(object sender, EventArgs e)
        {
           
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            
        }

        private void 复制佣金链接ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(this.listView1.SelectedItems[0].SubItems[3].Text);
        }

 

        private void 复制领券链接ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(this.listView1.SelectedItems[0].SubItems[4].Text);
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listView1.Items.Clear();
        }

     
    }
}
