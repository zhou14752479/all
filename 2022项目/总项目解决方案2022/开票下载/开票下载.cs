﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using myDLL;

namespace 开票下载
{
    public partial class 开票下载 : Form
    {
        public 开票下载()
        {
            InitializeComponent();
        }


        public string shenqingkai(string hao,string jine)
        {
            string url = "http://dzfp.sdshsy.com:18086/FPGLXT_WX/bill/applyKp.action";
            string postdata = "clientname=%E4%B8%AA%E4%BA%BA&clienttax=&clientaddress=&clientbank=&clientmail=&number="+hao+"&taxmoney="+jine;
            string html = method.PostUrlDefault(url,postdata,"");
            return html;
        }


        public string downshishi(string hao,string jine)
        {
            try
            {
                Thread.Sleep(10000);
                string url = "http://dzfp.sdshsy.com:18086/FPGLXT_WX/bill/getBill.action?number=" + hao + "&taxmoney=" + jine + "&code=J3A2";
                string html = method.GetUrl(url, "utf-8");
                string downurl = Regex.Match(html, @"btn_primary"" href=""([\s\S]*?)""").Groups[1].Value;
               while(downurl=="")
                {
                    Thread.Sleep(2000);
                    html = method.GetUrl(url, "utf-8");
                    downurl = Regex.Match(html, @"btn_primary"" href=""([\s\S]*?)""").Groups[1].Value;
                    if(downurl!="")
                    {
                        break;
                    }
                }
                if (Convert.ToDouble(jine) >= Convert.ToDouble(textBox3.Text))
                {
                    string downhtml = method.GetUrl(downurl, "utf-8");
                    string dlj = Regex.Match(downhtml, @"dlj"" value=""([\s\S]*?)""").Groups[1].Value;
                    string signatureString = Regex.Match(downhtml, @"signatureString"" value=""([\s\S]*?)""").Groups[1].Value;

                    string trueDownUrl = "https://dlj.51fapiao.cn/dlj/v7/downloadFile/" + dlj + "?signatureString=" + signatureString;
                    method.downloadFile(trueDownUrl, path, hao + ".pdf", "");

                    return "下载发票成功";
                }
               
                return "下载发票成功";
            }
            catch (Exception)
            {

                return "异常";
            }
        }

        string path = AppDomain.CurrentDomain.BaseDirectory+"//download//";
        public void run()
        {
            try
            {
                string[] text1 = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string[] text2 = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i <text1.Length; i++)
                {
                    for (int j = 0;j < text2.Length; j++)
                    {
                        if(text1[i]=="")
                        {
                            continue;
                        }
                        if (text2[j] == "")
                        {
                            continue;
                        }
                        Thread.Sleep(500);
                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(text1[i]);
                        lv1.SubItems.Add(text2[j]);
                        string url = "http://dzfp.sdshsy.com:18086/FPGLXT_WX/bill/getBill.action?number="+text1[i]+"&taxmoney="+text2[j]+"&code=J3A2";
                        string html = method.GetUrl(url,"utf-8");
                        string downurl = Regex.Match(html, @"btn_primary"" href=""([\s\S]*?)""").Groups[1].Value;
                       
                        if(downurl!="")
                        {
                            //金额满足
                            if (Convert.ToDouble(text2[j]) >= Convert.ToDouble(textBox3.Text))
                            {
                                string downhtml = method.GetUrl(downurl, "utf-8");
                                string dlj = Regex.Match(downhtml, @"dlj"" value=""([\s\S]*?)""").Groups[1].Value;
                                string signatureString = Regex.Match(downhtml, @"signatureString"" value=""([\s\S]*?)""").Groups[1].Value;

                                string trueDownUrl = "https://dlj.51fapiao.cn/dlj/v7/downloadFile/" + dlj + "?signatureString=" + signatureString;
                                method.downloadFile(trueDownUrl, path, text1[i] + ".pdf", "");

                                lv1.SubItems.Add("下载发票成功");
                                break;
                            }
                            else
                            {
                                lv1.SubItems.Add("金额不符合");
                                //金额不满足，跳到下一个流水号
                                break;
                            }
                        }
                        if(html.Contains("申请开票"))
                        {
                            string ahtml = shenqingkai(text1[i],text2[j]);
                            if(ahtml.Contains("正在等待开票"))
                            {
                                //金额满足
                                if (Convert.ToDouble(text2[j]) >= Convert.ToDouble(textBox3.Text))
                                {
                                    downshishi(text1[i], text2[j]);
                                    lv1.SubItems.Add("开票成功");
                                }
                                else
                                {
                                    lv1.SubItems.Add("金额不符合");
                                }
                            }
                            break;
                        }
                        if (html.Contains("该单据不存在"))
                        {
                            lv1.SubItems.Add("该单据不存在");
                            continue;
                        }

                    }
                    if (status == false)
                        return;

                }
                MessageBox.Show("完成");
                //http://dzfp.sdshsy.com:18086/FPGLXT_WX/bill/getBill.action?number=2022030625073096&taxmoney=3.3&code=J3A2

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }


        /// <summary>
        /// 模式2 只筛选正确金额  不开票
        /// </summary>
        public void run2()
        {
            try
            {
                string[] text1 = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                string[] text2 = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < text1.Length; i++)
                {
                    for (int j = 0; j < text2.Length; j++)
                    {
                        if (text1[i] == "")
                        {
                            continue;
                        }
                        if (text2[j] == "")
                        {
                            continue;
                        }
                        Thread.Sleep(500);
                      
                        string url = "http://dzfp.sdshsy.com:18086/FPGLXT_WX/bill/getBill.action?number=" + text1[i] + "&taxmoney=" + text2[j] + "&code=J3A2";
                        string html = method.GetUrl(url, "utf-8");
                        string downurl = Regex.Match(html, @"btn_primary"" href=""([\s\S]*?)""").Groups[1].Value;

                        if (downurl != "" || html.Contains("申请开票"))
                        {
                            FileStream fs1 = new FileStream(path + "流水号.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                            StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                            sw.WriteLine(text1[i]);
                            sw.Close();
                            fs1.Close();
                            sw.Dispose();

                            FileStream fs2= new FileStream(path + "金额.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                            StreamWriter sw2 = new StreamWriter(fs2, Encoding.GetEncoding("UTF-8"));
                            sw2.WriteLine(text2[i]);
                            sw2.Close();
                            fs2.Close();
                            sw2.Dispose();


                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(text1[i]);
                            lv1.SubItems.Add(text2[j]);
                            lv1.SubItems.Add("匹配");
                            break;

                        }
                       else 
                        {
                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(text1[i]);
                            lv1.SubItems.Add(text2[j]);
                            lv1.SubItems.Add("不匹配");
                        }
                       

                    }
                    if (status == false)
                        return;

                }
                MessageBox.Show("完成");
                //http://dzfp.sdshsy.com:18086/FPGLXT_WX/bill/getBill.action?number=2022030625073096&taxmoney=3.3&code=J3A2

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        bool status = true;
        private void 开票下载_Load(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName, method.EncodingType.GetTxtType(openFileDialog1.FileName));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < text.Length; i++)
                {
                    textBox1.Text += text[i]+"\r\n";
                }
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName, method.EncodingType.GetTxtType(openFileDialog1.FileName));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < text.Length; i++)
                {
                    textBox2.Text += text[i] + "\r\n";
                }
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
            }
        }


        Thread thread;
        private void button3_Click(object sender, EventArgs e)
        {
            if (DateTime.Now > Convert.ToDateTime("2024-12-10"))
            {
                return;
            }

            status = true;
            if(textBox1.Text=="" || textBox2.Text=="")
            {
                MessageBox.Show("请导入文本");
                return;
            }

            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            status = false;
            //method.downloadFile("https://dlj.51fapiao.cn/dlj/v7/downloadFile/942473b6c384501e74d2cde730904bb835c27e?signatureString=fcae62ac231e43b8892d3188d0b2d861", path,  "11.pdf", "");
        }

        public void down2()
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                string hao = listView1.Items[i].SubItems[1].Text;
                string jine = listView1.Items[i].SubItems[2].Text;
                string status = listView1.Items[i].SubItems[3].Text;
                if(status== "申请开票中")
                {
                    string url = "http://dzfp.sdshsy.com:18086/FPGLXT_WX/bill/getBill.action?number=" + hao + "&taxmoney=" + jine + "&code=J3A2";
                    string html = method.GetUrl(url, "utf-8");
                    string downurl = Regex.Match(html, @"btn_primary"" href=""([\s\S]*?)""").Groups[1].Value;
                    if (downurl != "")
                    {
                        if (Convert.ToDouble(jine) >=Convert.ToDouble(textBox3.Text))
                        {
                            string downhtml = method.GetUrl(downurl, "utf-8");
                            string dlj = Regex.Match(downhtml, @"dlj"" value=""([\s\S]*?)""").Groups[1].Value;
                            string signatureString = Regex.Match(downhtml, @"signatureString"" value=""([\s\S]*?)""").Groups[1].Value;

                            string trueDownUrl = "https://dlj.51fapiao.cn/dlj/v7/downloadFile/" + dlj + "?signatureString=" + signatureString;
                            method.downloadFile(trueDownUrl, path, hao + ".pdf", "");

                            listView1.Items[i].SubItems[3].Text = ("下载发票成功");
                        }
                    }
                    else
                    {
                        listView1.Items[i].SubItems[3].Text = ("申请开票中");
                    }
                }
            }
            MessageBox.Show("完成");
        }
        private void button5_Click(object sender, EventArgs e)
        {
            
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(down2);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void 开票下载_FormClosing(object sender, FormClosingEventArgs e)
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

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listView1.Items.Clear();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (DateTime.Now > Convert.ToDateTime("2024-12-10"))
            {
                return;
            }

            status = true;
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("请导入文本");
                return;
            }

            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run2);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }
    }
}
