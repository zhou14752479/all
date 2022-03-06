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

namespace 浙江企业基础信息查询
{
    public partial class 查询2 : Form
    {
        public 查询2()
        {
            InitializeComponent();
        }
       
       
        public string GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalMilliseconds);
            return a.ToString();
        }
        public void run()
        {
            try
            {
                for (int a = 0; a < dt.Rows.Count; a++)
                {
                    if (DateTime.Now > Convert.ToDateTime("2022-04-05"))
                    {
                        MessageBox.Show("{\"msg\":\"非法请求\"}");
                        return;
                    }

                    DataRow dr = dt.Rows[a];
                    string uid = dr[0].ToString();
                    string timestr = GetTimeStamp();
                    string sign = method.GetMD5("rkkpcxxcxjtapp" + timestr);
                    string zj_ggsjpt_sign = method.GetMD5("ada72850-2b2e-11e7-985b-008cfaeb3d74" + "995e00df72f14bbcb7833a9ca063adef" + timestr);
                    string url = "http://app.gjzwfw.gov.cn/jimps/link.do?param=%7B%22from%22%3A%222%22%2C%22key%22%3A%223b8d18a7d9b4482caf1cbc39b4404d06%22%2C%22requestTime%22%3A%22"+timestr+"%22%2C%22sign%22%3A%22"+sign+"%22%2C%22zj_ggsjpt_app_key%22%3A%22ada72850-2b2e-11e7-985b-008cfaeb3d74%22%2C%22zj_ggsjpt_sign%22%3A%22"+ zj_ggsjpt_sign + "%22%2C%22zj_ggsjpt_time%22%3A%22"+timestr+"%22%2C%22gmsfhm%22%3A%22"+uid+"%22%2C%22additional%22%3A%22%22%7D";
                    label3.Text = "正在查询：" + uid;
                  
                    
                    string html = method.GetUrl(url,"utf-8");
                   
                  

                    MatchCollection dwxxmc = Regex.Matches(html, @"""dwxxmc"":""([\s\S]*?)""");
                    MatchCollection xywcqk = Regex.Matches(html, @"""xywcqk"":""([\s\S]*?)""");
                    MatchCollection gmsfhm = Regex.Matches(html, @"""gmsfhm"":""([\s\S]*?)""");
                    MatchCollection sfsz = Regex.Matches(html, @"""sfsz"":""([\s\S]*?)""");
                    MatchCollection hkdjd = Regex.Matches(html, @"""hkdjd"":""([\s\S]*?)""");
                    MatchCollection mz = Regex.Matches(html, @"""mz"":""([\s\S]*?)""");
                    MatchCollection xm = Regex.Matches(html, @"""xm"":""([\s\S]*?)""");
                    MatchCollection hyzk = Regex.Matches(html, @"""hyzk"":""([\s\S]*?)""");
                    MatchCollection jnsyzk = Regex.Matches(html, @"""jnsyzk"":""([\s\S]*?)""");


                    for (int i = 0; i < dwxxmc.Count; i++)
                    {
                        if (jiami == true)
                        {
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(method.Base64Encode(Encoding.GetEncoding("utf-8"), dwxxmc[i].Groups[1].Value));
                            lv1.SubItems.Add(method.Base64Encode(Encoding.GetEncoding("utf-8"), xywcqk[i].Groups[1].Value));
                            lv1.SubItems.Add(method.Base64Encode(Encoding.GetEncoding("utf-8"), gmsfhm[i].Groups[1].Value));
                            lv1.SubItems.Add(method.Base64Encode(Encoding.GetEncoding("utf-8"), sfsz[i].Groups[1].Value));
                            lv1.SubItems.Add(method.Base64Encode(Encoding.GetEncoding("utf-8"), hkdjd[i].Groups[1].Value));
                            lv1.SubItems.Add(method.Base64Encode(Encoding.GetEncoding("utf-8"), mz[i].Groups[1].Value));
                            lv1.SubItems.Add(method.Base64Encode(Encoding.GetEncoding("utf-8"), xm[i].Groups[1].Value));
                            lv1.SubItems.Add(method.Base64Encode(Encoding.GetEncoding("utf-8"), hyzk[i].Groups[1].Value));
                            lv1.SubItems.Add(method.Base64Encode(Encoding.GetEncoding("utf-8"), jnsyzk[i].Groups[1].Value));
                        }
                        if (jiami == false)
                        {
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(dwxxmc[i].Groups[1].Value);
                            lv1.SubItems.Add(xywcqk[i].Groups[1].Value);
                            lv1.SubItems.Add(gmsfhm[i].Groups[1].Value);
                            lv1.SubItems.Add(sfsz[i].Groups[1].Value);
                            lv1.SubItems.Add(hkdjd[i].Groups[1].Value);
                            lv1.SubItems.Add(mz[i].Groups[1].Value);
                            lv1.SubItems.Add(xm[i].Groups[1].Value);
                            lv1.SubItems.Add(hyzk[i].Groups[1].Value);
                            lv1.SubItems.Add(jnsyzk[i].Groups[1].Value);
                        }
                        Thread.Sleep(1000);

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

                   
                   
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        bool jiami = false;
        bool zanting = true;
        bool status = false;
        Thread thread;
        DataTable dt;
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "24791475")
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
                        string value = listView1.Items[i].SubItems[j].Text;
                        if (jiami == false)
                        {
                            listView1.Items[i].SubItems[j].Text = method.Base64Encode(Encoding.GetEncoding("utf-8"), value) ;



                        }
                        else
                        {
                            listView1.Items[i].SubItems[j].Text = method.Base64Decode(Encoding.GetEncoding("utf-8"), value);
                        }

                    }
                    catch (Exception ex)
                    {

                        continue;
                    }
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

        private void 查询2_Load(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }
}
