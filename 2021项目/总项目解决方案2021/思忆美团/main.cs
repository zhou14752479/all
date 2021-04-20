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
using myDLL;

namespace 思忆美团
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
        }

        Point mPoint = new Point();
        functions fc = new functions();
       
        private void main_Load(object sender, EventArgs e)
        {
            ProvinceCity.ProvinceCity.BindProvince(skinComboBox1);
           fc.Getcates(skinComboBox3);
          
            fc.jiqima = fc.getjiqima();
            vip_Item1.Text = "账号权限：  "+functions.vip;
          
        }



        #region  主程序
        public void run(object o)

        {
           
          

            string[] values = o.ToString().Split(new string[] { "#" }, StringSplitOptions.None);
            string[] citys = values[0].Trim().Split(new string[] { "," }, StringSplitOptions.None);
            string[] cates = values[1].Trim().Split(new string[] { "," }, StringSplitOptions.None);

            foreach (string city in citys)
            {
                
                if (city == "")
                    continue;
                string citychuli = city.Replace("市", "").Replace("土家族苗族自治州", "");
                string cityId = fc.GetcityId((citychuli));
                if (cityId == "")
                    continue;
                foreach (string cateName in cates)
                {
                    if (cateName == "")
                        continue;
                    string cateid = "";
                    try
                    {
                       cateid = fc.catedic[cateName];

                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show("类目输入有误") ;
                        return;
                    }
                   
                    try
                    {

                        for (int i = 0; i < 1001; i = i + 100)

                        {
                            string timestamp = fc.GetTimeStamp();
                           
                            string Url = "http://www.acaiji.com:8080/api/mt/mt_getdata.html?userid=2&cityid=" + cityId + "&cateid=" + cateid + "&page="+i+"&code="+fc.jiqima+"&timestamp=" + timestamp + "&sign=" + fc.getsign(timestamp); ;
                           
                            string html = method.GetUrl(Url, "utf-8");  //定义的GetRul方法 返回 reader.ReadToEnd()

                            if (html.Contains("siyifalse"))
                            {
                                MessageBox.Show(html);
                                return;
                            }
                            MatchCollection names = Regex.Matches(html, @"""shopName"":""([\s\S]*?)""");

                            MatchCollection address = Regex.Matches(html, @"""address"":""([\s\S]*?)""");
                            MatchCollection phone = Regex.Matches(html, @"""phone"":""([\s\S]*?)""");
                     
                            MatchCollection cate = Regex.Matches(html, @"mainCategoryName"":""([\s\S]*?)""");
                     
                            MatchCollection shangquan = Regex.Matches(html, @"""areaName"":""([\s\S]*?)""");


                            if (names.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                break;

                            for (int j = 0; j < names.Count; j++)
                            {

                                ListViewItem listViewItem = listView2.Items.Add((listView2.Items.Count + 1).ToString());
                                listViewItem.SubItems.Add(names[j].Groups[1].Value);
                                listViewItem.SubItems.Add(address[j].Groups[1].Value);
                                listViewItem.SubItems.Add(phone[j].Groups[1].Value);
                           

                                listViewItem.SubItems.Add(cate[j].Groups[1].Value);
                              
                                listViewItem.SubItems.Add(shangquan[j].Groups[1].Value);
                                listViewItem.SubItems.Add(city);
                                while (zanting == false)
                                {
                                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                                }
                                if (status == false)
                                    return;
                            }




                            Thread.Sleep(200);
                        }
                        Application.DoEvents();
                        Thread.Sleep(1000);




                    }
                    catch (System.Exception ex)
                    {
                        ex.ToString();
                    }
                }
            }


        }



        #endregion

  
       

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;//窗口正常显示
            this.ShowInTaskbar = true;//在任务栏中显示该窗口
        }

        private void 显示软件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;//窗口正常显示
            this.ShowInTaskbar = true;//在任务栏中显示该窗口
        }

        private void 开通会员ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 退出软件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否退出？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {

            }
            else
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        private void 隐藏软件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            notifyIcon1.Visible = true;//该控件可见
            this.ShowInTaskbar = false;//在任务栏中显示该窗口
        }

       

   

        private void pauseContinue_btn_Click(object sender, EventArgs e)
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

        private void stop_btn_Click(object sender, EventArgs e)
        {
            status = false;

        }
        Thread thread;
        bool zanting = true;
        bool status = true;
   

       
    

        private void skinMenuStrip1_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint.X = e.X;
            mPoint.Y = e.Y;
        }

        private void skinMenuStrip1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point myPosittion = MousePosition;
                myPosittion.Offset(-mPoint.X, -mPoint.Y);
                Location = myPosittion;
            }
        }

        private void close_label_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (MessageBox.Show("是否退出？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {

            }
            else
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        private void mini_label_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void skinComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProvinceCity.ProvinceCity.BindCity(skinComboBox1, skinComboBox2);
            
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            if (skinTextBox1.Text.Contains(skinComboBox2.Text))
            {
                MessageBox.Show(skinComboBox2.Text + "：请勿重复添加", "重复添加错误");
                return;
            }

            skinTextBox1.Text += skinComboBox2.Text + ",";
            string[] text = skinTextBox1.Text.Split(new string[] { "," }, StringSplitOptions.None);
            label3.Text = "已选城市：" + (text.Length - 1);
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
          
            for (int i = 0; i < skinComboBox2.Items.Count; i++)
            {
                skinTextBox1.Text += skinComboBox2.Items[i] + ",";
            }
          
            string[] text = skinTextBox1.Text.Split(new string[] { "," }, StringSplitOptions.None);
            label3.Text = "已选城市：" + (text.Length - 1);
        }

        private void skinButton4_Click(object sender, EventArgs e)
        {
            if (skinTextBox2.Text.Contains(skinComboBox3.Text))
            {
                MessageBox.Show(skinComboBox3.Text + "：请勿重复添加", "重复添加错误");
                return;
            }
            skinTextBox2.Text += skinComboBox3.Text+",";
            string[] text = skinTextBox2.Text.Split(new string[] { "," }, StringSplitOptions.None);
            label4.Text = "已选类目：" + (text.Length-1);
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            skinTextBox2.Text = "";
            for (int i = 0; i < skinComboBox3.Items.Count; i++)
            {
                skinTextBox2.Text += skinComboBox3.Items[i] + ",";
            }
            string[] text = skinTextBox2.Text.Split(new string[] { "," }, StringSplitOptions.None);
            label4.Text = "已选类目：" + (text.Length - 1);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            skinTextBox1.Text = "";
            label3.Text = "已选城市：0";
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            skinTextBox2.Text = "";
            label4.Text = "已选类目：0" ;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (skinTextBox1.Text != "" && skinTextBox2.Text != "")
            {

                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                string taskname = DateTime.Now.ToString("yyyyMMddHHMMss");
                lv1.SubItems.Add(taskname);
                lv1.SubItems.Add(skinTextBox1.Text);
                lv1.SubItems.Add(skinTextBox2.Text);

                fc.IniWriteValue("values", "taskname", taskname);
                fc.IniWriteValue("values", "city", skinTextBox1.Text);
                fc.IniWriteValue("values", "cate", skinTextBox2.Text);

            }
            else
            {
                if (skinTextBox1.Text == "")
                {
                    MessageBox.Show("请选择地区", "未选择参数");
                }

                if (skinTextBox2.Text == "")
                {
                    MessageBox.Show("请选择类目", "未选择参数");
                }

            }
        }

        private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            notifyIcon1.Visible = true;//该控件可见
            this.ShowInTaskbar = false;//在任务栏中显示该窗口
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (listView1.CheckedItems.Count==0)
            {
                MessageBox.Show("未选择任务");
                return;
            }

            status = true;
            for (int i = 0; i < listView1.CheckedItems.Count; i++)
            {
                if (thread == null || !thread.IsAlive)
                {
                    thread = new Thread(new ParameterizedThreadStart(run));
                    string o = listView1.CheckedItems[i].SubItems[2].Text+"#"+ listView1.CheckedItems[i].SubItems[3].Text;
                    thread.Start((object)o);
                    Control.CheckForIllegalCrossThreadCalls = false;
                }
            }

        }

        private void skinButton5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView2), "Sheet1", true);
        }

        private void skinButton6_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
        }

        private void qq_label_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://wpa.qq.com/msgrd?v=3&uin=852266010&site=qq&menu=yes");
        }

       

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            zanting = false;
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            zanting = true;
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            status = false;
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (listView1.CheckedItems.Count == 0)
            {
                MessageBox.Show("未选择任务");
                return;
            }

            for (int i = 0; i < listView1.CheckedItems.Count; i++)
            {
                listView1.Items.RemoveAt(i);

            }
        }

        private void skinButton7_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < skinComboBox1.Items.Count; i++)
            {
                skinComboBox1.Text= skinComboBox1.Items[i].ToString();
                ProvinceCity.ProvinceCity.BindCity(skinComboBox1, skinComboBox2);
                for (int j = 0; j < skinComboBox2.Items.Count; j++)
                {
                    if (!skinComboBox2.Items[j].ToString().Contains("区"))
                    {
                        skinTextBox1.Text += skinComboBox2.Items[j].ToString()+",";
                    }
                    
                }
            }
        }

        private void skinButton9_Click(object sender, EventArgs e)
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

        private void skinButton8_Click(object sender, EventArgs e)
        {
            status = false;
        }
    }
}
