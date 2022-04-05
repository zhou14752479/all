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
using myDLL;
using Spire.Xls;

namespace 浙江企业基础信息查询
{
    public partial class 浙江企业基础信息查询 : Form
    {
        public 浙江企业基础信息查询()
        {
            InitializeComponent();
        }
        #region  listView导出CSV
        /// <summary>
        /// 导出CSV
        /// </summary>
        /// <param name="listView"></param>
        /// <param name="includeHidden"></param>
        /// 
        public static void ListViewToCSV(ListView listView, bool includeHidden)
        {
            //make header string
            SaveFileDialog sfd = new SaveFileDialog();
            //sfd.Filter = "xlsx|*.xls|xlsx|*.xlsx";

            //sfd.Title = "Excel文件导出";
            string filePath = "";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                filePath = sfd.FileName + ".csv";
            }
            StringBuilder result = new StringBuilder();
            WriteCSVRow(result, listView.Columns.Count, i => includeHidden || listView.Columns[i].Width > 0, i => listView.Columns[i].Text);

            //export data rows
            foreach (ListViewItem listItem in listView.Items)
                WriteCSVRow(result, listView.Columns.Count, i => includeHidden || listView.Columns[i].Width > 0, i => listItem.SubItems[i].Text);

            File.WriteAllText(filePath, result.ToString(), Encoding.GetEncoding("utf-8"));
            MessageBox.Show("导出成功");
        }


        private static void WriteCSVRow(StringBuilder result, int itemsCount, Func<int, bool> isColumnNeeded, Func<int, string> columnValue)
        {
            bool isFirstTime = true;
            for (int i = 0; i < itemsCount; i++)
            {
                try
                {

                    if (!isColumnNeeded(i))
                        continue;

                    if (!isFirstTime)
                        result.Append(",");
                    isFirstTime = false;

                    result.Append(String.Format("{0}", columnValue(i)));
                }
                catch
                {
                    continue;
                }
            }

            result.AppendLine();
        }

        #endregion

        /// <summary>
        /// 获取时间戳毫秒
        /// </summary>
        /// <returns></returns>
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
                int cuowucishu = 0;

                for (int a = 0; a< dt.Rows.Count; a++)
                {
                    if(DateTime.Now>Convert.ToDateTime("2022-05-02"))
                    {
                        MessageBox.Show("{\"msg\":\"非法请求\"}");
                        return;
                    }
                    
                    DataRow dr = dt.Rows[a];
                    string uid = dr[0].ToString();
                    string url = "http://app.gjzwfw.gov.cn/jimps/link.do";
                    label3.Text = "正在查询：" + uid;
                    string timestr = GetTimeStamp();
             
                    string gregegedrgerheh = gdsgdgdgdgdstgfeewrwerw3r23r32rvxsvdsv.rgebgdgdvsdfsdvsdfsdvdsbgdsrt435b515sdfsdf("1", timestr);
                    string sign = gregegedrgerheh.Split(new string[] { "," }, StringSplitOptions.None)[0];
                    string zj_ggsjpt_sign = gregegedrgerheh.Split(new string[] { "," }, StringSplitOptions.None)[1];

                    string postdata = "param=%7B%22from%22%3A%222%22%2C%22key%22%3A%22b4842fe0fadc44398d674c786a583f8e%22%2C%22requestTime%22%3A%22" + timestr + "%22%2C%22sign%22%3A%22" + sign + "%22%2C%22zj_ggsjpt_app_key%22%3A%22ada72850-2b2e-11e7-985b-008cfaeb3d74%22%2C%22zj_ggsjpt_sign%22%3A%22" + zj_ggsjpt_sign + "%22%2C%22zj_ggsjpt_time%22%3A%22" + timestr + "%22%2C%22uniscId%22%3A%22"+uid+"%22%2C%22companyName%22%3A%22%22%2C%22registerNo%22%3A%22%22%2C%22entType%22%3A%22E%22%2C%22additional%22%3A%22%22%7D";
                    string html = method.PostUrlDefault(url, postdata, "");
                    //MessageBox.Show(html);
                    //textBox2.Text = html;
                    string company = Regex.Match(html, @"""companyName"":""([\s\S]*?)""").Groups[1].Value;

                    string legalPerson = Regex.Match(html, @"""legalPerson"":""([\s\S]*?)""").Groups[1].Value;
                    string legalPersonPaperNo = Regex.Match(html, @"""legalPersonPaperNo"":""([\s\S]*?)""").Groups[1].Value;
                    string positionName = Regex.Match(html, @"""positionName"":""([\s\S]*?)""").Groups[1].Value;
                    string legalPersonpositionName = legalPerson + positionName;

                    string financeInfo = Regex.Match(html, @"financeInfo([\s\S]*?)\]").Groups[1].Value;
                    string liaisonInfo = Regex.Match(html, @"liaisonInfo([\s\S]*?)\]").Groups[1].Value;

                    if(company=="")
                    {
                        cuowucishu = cuowucishu + 1;
                        if(cuowucishu <= 3)
                        {
                            a = a - 1;
                        }
                       else
                        {
                            cuowucishu = 0;
                        }
                        continue;
                    }

                    MatchCollection names = Regex.Matches(html, @"shareholderName='([\s\S]*?)'");
                    MatchCollection cards = Regex.Matches(html, @"paperNo='([\s\S]*?)'");
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
                    string notel = "null";
                    if (jiami == true)
                    {
                        legalPersonpositionName = method.Base64Encode(Encoding.GetEncoding("utf-8"), legalPersonpositionName);
                        legalPersonPaperNo = method.Base64Encode(Encoding.GetEncoding("utf-8"), legalPersonPaperNo);

                        notel = method.Base64Encode(Encoding.GetEncoding("utf-8"), notel);
                        uid = method.Base64Encode(Encoding.GetEncoding("utf-8"), uid);
                        company = method.Base64Encode(Encoding.GetEncoding("utf-8"), company);
                      
                    }
                    StringBuilder sb = new StringBuilder();
                    sb.Append(uid + "," + company + ",");
                    sb.Append(legalPersonpositionName + "," + legalPersonPaperNo+ ","+notel+",");
                    for (int i = 0; i < names.Count; i++)
                    {
                        string name = names[i].Groups[1].Value;
                        string card = cards[i].Groups[1].Value;
                        if (jiami == true)
                        {
                          
                            name = method.Base64Encode(Encoding.GetEncoding("utf-8"), names[i].Groups[1].Value);
                            card = method.Base64Encode(Encoding.GetEncoding("utf-8"), cards[i].Groups[1].Value);
                        }

                        sb.Append(name+","+card+","+ notel+",");
                      
                    }
                    string aname = Regex.Match(financeInfo, @"nAME"":""([\s\S]*?)""").Groups[1].Value;
                    string acard = Regex.Match(financeInfo, @"cERNO"":""([\s\S]*?)""").Groups[1].Value;
                    string atel = Regex.Match(financeInfo, @"mOBTEL"":""([\s\S]*?)""").Groups[1].Value;
                    string bname = Regex.Match(liaisonInfo, @"nAME"":""([\s\S]*?)""").Groups[1].Value;
                    string bcard = Regex.Match(liaisonInfo, @"cERNO"":""([\s\S]*?)""").Groups[1].Value;
                    string btel = Regex.Match(liaisonInfo, @"mOBTEL"":""([\s\S]*?)""").Groups[1].Value;



                    if (jiami == true)
                    {
                        company = method.Base64Encode(Encoding.GetEncoding("utf-8"), company);
                        aname = method.Base64Encode(Encoding.GetEncoding("utf-8"), aname);
                        acard = method.Base64Encode(Encoding.GetEncoding("utf-8"), acard);
                        atel = method.Base64Encode(Encoding.GetEncoding("utf-8"), atel);
                        bname = method.Base64Encode(Encoding.GetEncoding("utf-8"), bname);
                        bcard = method.Base64Encode(Encoding.GetEncoding("utf-8"), bcard);
                        btel = method.Base64Encode(Encoding.GetEncoding("utf-8"), btel);
                    }
                    sb.Append(aname + "," + acard + "," + atel + "," + bname + "," + bcard + "," + btel);

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(sb.ToString());

                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        public void runbeifen()
        {
            try
            {
                for (int a = 0; a < dt.Rows.Count; a++)
                {


                    DataRow dr = dt.Rows[a];
                    string uid = dr[0].ToString();
                    string url = "http://app.gjzwfw.gov.cn/jimps/link.do";
                    label3.Text = "正在查询：" + uid;
                    string timestr = method.GetTimeStamp() + "000";
                    string sign = method.GetMD5("qyjbxxcxzj" + timestr);
                    string zj_ggsjpt_sign = method.GetMD5("ada72850-2b2e-11e7-985b-008cfaeb3d74" + "995e00df72f14bbcb7833a9ca063adef" + timestr);
                    string postdata = "param=%7B%22from%22%3A%222%22%2C%22key%22%3A%22b4842fe0fadc44398d674c786a583f8e%22%2C%22requestTime%22%3A%22" + timestr + "%22%2C%22sign%22%3A%22" + sign + "%22%2C%22zj_ggsjpt_app_key%22%3A%22ada72850-2b2e-11e7-985b-008cfaeb3d74%22%2C%22zj_ggsjpt_sign%22%3A%22" + zj_ggsjpt_sign + "%22%2C%22zj_ggsjpt_time%22%3A%22" + timestr + "%22%2C%22uniscId%22%3A%22" + uid + "%22%2C%22companyName%22%3A%22%22%2C%22registerNo%22%3A%22%22%2C%22entType%22%3A%22E%22%2C%22additional%22%3A%22%22%7D";
                    string html = method.PostUrlDefault(url, postdata, "");
                    //MessageBox.Show(html);
                    string company = Regex.Match(html, @"""companyName"":""([\s\S]*?)""").Groups[1].Value;

                    string financeInfo = Regex.Match(html, @"financeInfo([\s\S]*?)\]").Groups[1].Value;
                    string liaisonInfo = Regex.Match(html, @"liaisonInfo([\s\S]*?)\]").Groups[1].Value;



                    MatchCollection names = Regex.Matches(html, @"shareholderName='([\s\S]*?)'");
                    MatchCollection cards = Regex.Matches(html, @"paperNo='([\s\S]*?)'");

                    for (int i = 0; i < names.Count; i++)
                    {
                        if (jiami == true)
                        {
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(uid);
                            lv1.SubItems.Add(method.Base64Encode(Encoding.GetEncoding("utf-8"), company));
                            lv1.SubItems.Add(method.Base64Encode(Encoding.GetEncoding("utf-8"), names[i].Groups[1].Value));
                            lv1.SubItems.Add(method.Base64Encode(Encoding.GetEncoding("utf-8"), cards[i].Groups[1].Value));
                        }
                        if (jiami == false)
                        {
                            ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(uid);
                            lv1.SubItems.Add(company);
                            lv1.SubItems.Add(names[i].Groups[1].Value);
                            lv1.SubItems.Add(cards[i].Groups[1].Value);
                        }


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

                    string aname = Regex.Match(financeInfo, @"nAME"":""([\s\S]*?)""").Groups[1].Value;
                    string acard = Regex.Match(financeInfo, @"cERNO"":""([\s\S]*?)""").Groups[1].Value;
                    string atel = Regex.Match(financeInfo, @"mOBTEL"":""([\s\S]*?)""").Groups[1].Value;

                    string bname = Regex.Match(liaisonInfo, @"nAME"":""([\s\S]*?)""").Groups[1].Value;
                    string bcard = Regex.Match(liaisonInfo, @"cERNO"":""([\s\S]*?)""").Groups[1].Value;
                    string btel = Regex.Match(liaisonInfo, @"mOBTEL"":""([\s\S]*?)""").Groups[1].Value;



                    if (jiami == true)
                    {
                        company = method.Base64Encode(Encoding.GetEncoding("utf-8"), company);
                        aname = method.Base64Encode(Encoding.GetEncoding("utf-8"), aname);
                        acard = method.Base64Encode(Encoding.GetEncoding("utf-8"), acard);
                        atel = method.Base64Encode(Encoding.GetEncoding("utf-8"), atel);
                        bname = method.Base64Encode(Encoding.GetEncoding("utf-8"), bname);
                        bcard = method.Base64Encode(Encoding.GetEncoding("utf-8"), bcard);
                        btel = method.Base64Encode(Encoding.GetEncoding("utf-8"), btel);
                    }



                    if (aname != "")
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(uid);
                        lv1.SubItems.Add(company);
                        lv1.SubItems.Add(aname);
                        lv1.SubItems.Add(acard);
                        lv1.SubItems.Add(atel);
                    }




                    if (bname != "")
                    {
                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(uid);
                        lv1.SubItems.Add(company);
                        lv1.SubItems.Add(bname);
                        lv1.SubItems.Add(bcard);
                        lv1.SubItems.Add(btel);
                    }
                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        bool zanting = true;
        bool status = false;
        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text=="")
            {
                MessageBox.Show("请先导入表格");
                return;
            }

            if(status==true)
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
            method.DataTableToExcelName(listViewToDataTable(this.listView1), "sample.xlsx", true);
            excel_fenlie();
            // ListViewToCSV(listView1,true);
        }

       public void excel_fenlie()
        {
            //创建Workbook，加载Excel测试文档
            Workbook book = new Workbook();
            book.LoadFromFile("sample.xlsx");
            //获取第一个工作表
            Worksheet sheet = book.Worksheets[0];

            //遍历数据（从第2行到最后一行）
            string[] splitText = null;
            string text = null;
            for (int i = 1; i < sheet.LastRow; i++)
            {
                text = sheet.Range[i + 1, 1].Text;
                //分割按逗号作为分隔符的数据列
                splitText = text.Split(',');
                //保存被分割的数据到数组，数组项写入列
                for (int j = 0; j < splitText.Length; j++)
                {
                    sheet.Range[i + 1, 1 + j + 1].Text = splitText[j];
                }
            }
            //保存并打开文档
            string time = GetTimeStamp();
            book.SaveToFile("结果"+ time + ".xlsx", ExcelVersion.Version2010);
            File.Delete("sample.xlsx");
            MessageBox.Show("导出成功文件位于软件根目录："+ time+".xlsx");
        }


        #region listview转datable
        /// <summary>
        /// listview转datable
        /// </summary>
        /// <param name="lv"></param>
        /// <returns></returns>
        public static DataTable listViewToDataTable(ListView lv)
        {
            int i, j;
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Clear();
            dt.Columns.Clear();
            //lv.Columns.Count
            //生成DataTable列头
            for (i = 1; i < lv.Columns.Count; i++)
            {
                dt.Columns.Add(lv.Columns[i].Text.Trim(), typeof(String));
            }
            //每行内容
            for (i = 0; i < lv.Items.Count; i++)
            {
                dr = dt.NewRow();
                dr[0] = lv.Items[i].SubItems[1].Text.Trim();
                dt.Rows.Add(dr);
            }

            return dt;
        }
        #endregion
        private void button5_Click(object sender, EventArgs e)
        {
            
            listView1.Items.Clear();
        }


        bool jiami = true;
        private void button3_Click(object sender, EventArgs e)
        {
            if(textBox3.Text!="14752479")
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
                        
                        if(jiami==false)
                        {
                            if(j==1)
                            {
                                
                                string[] text = listView1.Items[i].SubItems[j].Text.Split(new string[] { "," }, StringSplitOptions.None);
                                listView1.Items[i].SubItems[j].Text = "";
                                foreach (var item in text)
                                {
                                    listView1.Items[i].SubItems[j].Text += method.Base64Encode(Encoding.GetEncoding("utf-8"), item) + ",";
                                }
                            }
                            
                        }
                        else
                        {
                            if (j == 1)
                            {
                                string[] text = listView1.Items[i].SubItems[j].Text.Split(new string[] { "," }, StringSplitOptions.None);
                                listView1.Items[i].SubItems[j].Text = "";
                                foreach (var item in text)
                                {
                                   
                                    listView1.Items[i].SubItems[j].Text += method.Base64Decode(Encoding.GetEncoding("utf-8"), item)+",";
                                }
                            }
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        
                        continue;
                    }
                }
            }


            zanting = true;

            if(jiami==false)
            {
                jiami = true;
            }
            else 
            {
                jiami = false;
            }
        }

        private void 浙江企业基础信息查询_Load(object sender, EventArgs e)
        {
            #region 通用检测


            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"2DZkG"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            #endregion
        }
    }
}
