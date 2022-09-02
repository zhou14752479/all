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
    public partial class 查询6 : Form
    {
        public 查询6()
        {
            InitializeComponent();
        }

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
        bool jiami = true;
        public void run()
        {
            for (int a = 0; a < dt.Rows.Count; a++)
            {
             
               

                string time = 查询2.GetTimeStamp();
                DataRow dr = dt.Rows[a];
                string name= dr[0].ToString();
                string card = dr[1].ToString();
                string gregegedrgerheh = gdsgdgdgdgdstgfeewrwerw3r23r32rvxsvdsv.rgebgdgdvsdfsdvsdfsdvdsbgdsrt435b515sdfsdf("shebao", time);
                string sign = gregegedrgerheh.Split(new string[] { "," }, StringSplitOptions.None)[0];
                string ggsjpt_sign = gregegedrgerheh.Split(new string[] { "," }, StringSplitOptions.None)[1];

                string expiretime = gregegedrgerheh.Split(new string[] { "," }, StringSplitOptions.None)[2];

                if (DateTime.Now > Convert.ToDateTime(expiretime))
                {
                    MessageBox.Show("{\"msg\":\"非法请求\"}");
                    return;
                }

                //string url = "http://app.gjzwfw.gov.cn/jmopen/interfaces/wxTransferPort.do?callback=jQuery18309492701749972507_" + time + "&requestUrl=http%3A%2F%2Fapp.gjzwfw.gov.cn%2Fjimps%2Flink.do&datas=dhzkh%22param%22%3A%22dhzkh%5C%22from%5C%22%3A%5C%221%5C%22%2C%5C%22key%5C%22%3A%5C%2291da7d51a42542219852bb3df4399d03%5C%22%2C%5C%22requestTime%5C%22%3A%5C%22" + time + "%5C%22%2C%5C%22sign%5C%22%3A%5C%22" + sign + "%5C%22%2C%5C%22zj_ggsjpt_app_key%5C%22%3A%5C%22ada72850-2b2e-11e7-985b-008cfaeb3d74%5C%22%2C%5C%22zj_ggsjpt_sign%5C%22%3A%5C%22" + ggsjpt_sign + "%5C%22%2C%5C%22zj_ggsjpt_time%5C%22%3A%5C%22" + time + "%5C%22%2C%5C%22name%5C%22%3A%5C%22"+name+"%5C%22%2C%5C%22cardId%5C%22%3A%5C%22"+card+"%5C%22%2C%5C%22additional%5C%22%3A%5C%22%5C%22dhykh%22dhykh&heads=&_=" + time;


                string url = "http://app.gjzwfw.gov.cn/jimps/link.do?param=%7B%22from%22%3A%221%22%2C%22key%22%3A%2291da7d51a42542219852bb3df4399d03%22%2C%22requestTime%22%3A%22" + time + "%22%2C%22sign%22%3A%22" + sign + "%22%2C%22zj_ggsjpt_app_key%22%3A%22ada72850-2b2e-11e7-985b-008cfaeb3d74%22%2C%22zj_ggsjpt_sign%22%3A%22" + ggsjpt_sign + "%22%2C%22zj_ggsjpt_time%22%3A%22" + time + "%22%2C%22cardId%22%3A%22" + card + "%22%2C%22name%22%3A%22" + name + "%22%2C%22additional%22%3A%22%22%7D";


                //url = url.Replace("%22:", "%22%3A");

                string html = 查询2.GetUrl(url, "utf-8");
                MessageBox.Show(html);
                string com = Regex.Match(html, @"""companyName"":""([\s\S]*?)""").Groups[1].Value;

                string aa = Regex.Match(html, @"""personelNo"":""([\s\S]*?)""").Groups[1].Value;
                string bb = Regex.Match(html, @"""insuranceType"":""([\s\S]*?)""").Groups[1].Value;
                string cc = Regex.Match(html, @"""addr"":""([\s\S]*?)""").Groups[1].Value;
                string dd = Regex.Match(html, @"""telNo"":""([\s\S]*?)""").Groups[1].Value;
                string ee = Regex.Match(html, @"""tong_time"":""([\s\S]*?)""").Groups[1].Value;

                if (jiami == true)
                {
                   name = method.Base64Encode(Encoding.GetEncoding("utf-8"), name);
                    card = method.Base64Encode(Encoding.GetEncoding("utf-8"), card);
                   com = method.Base64Encode(Encoding.GetEncoding("utf-8"), com);
                    aa = method.Base64Encode(Encoding.GetEncoding("utf-8"), aa);
                   bb = method.Base64Encode(Encoding.GetEncoding("utf-8"), bb);
                    cc = method.Base64Encode(Encoding.GetEncoding("utf-8"), cc);
                    dd = method.Base64Encode(Encoding.GetEncoding("utf-8"), dd);
                    ee = method.Base64Encode(Encoding.GetEncoding("utf-8"), ee);

                }


                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count).ToString()); //使用Listview展示数据 
                lv1.SubItems.Add(name);
                lv1.SubItems.Add(card);
                lv1.SubItems.Add(com);
                lv1.SubItems.Add(aa);
                lv1.SubItems.Add(bb);
                lv1.SubItems.Add(cc);
                lv1.SubItems.Add(dd);
                lv1.SubItems.Add(ee);

                Thread.Sleep(2000);
                while (zanting == false)
                {
                    Application.DoEvents();//等待本次加载完毕才执行下次循环.
                }
                if (status == false)
                    return;
            }



        }

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

        private void button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void 查询社保_FormClosing(object sender, FormClosingEventArgs e)
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "14752479")
            {
                MessageBox.Show("密码错误");
                return;
            }


            zanting = false;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                for (int j = 0; j < listView1.Columns.Count; j++)
                {
                    try
                    {

                        if (jiami == false)
                        {
                            if (j != 0)
                            {
                                listView1.Items[i].SubItems[j].Text = method.Base64Encode(Encoding.GetEncoding("utf-8"), listView1.Items[i].SubItems[j].Text);
                            }

                        }
                        else
                        {
                            if (j != 0)
                            {
                                listView1.Items[i].SubItems[j].Text = method.Base64Decode(Encoding.GetEncoding("utf-8"), listView1.Items[i].SubItems[j].Text);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.ToString());
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

        private void 查询6_Load(object sender, EventArgs e)
        {

        }
    }
}
