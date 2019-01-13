using System;
using System.Collections;
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

namespace _58.程序界面
{
    public partial class caiji_58 : Form
    {
        public caiji_58()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false; // 设置线程之间可以操作
        }


        bool zanting = true;
        #region  58二手房
        public void ershoufang()
        {



            try
            {
                string[] headers = { "标题", "联系人", "电话", "地区", "小区", "面积", "网址" };

                setDatagridview(skinDataGridView1, 7, headers);

                ArrayList citys = Method.get58CityNames();

                foreach (string city in citys)
                {

                for (int i = 1; i <= 1; i++)
                {
                    String Url = "http://" + city + ".58.com/ershoufang/0/pn" + i + "/";

                    string html = Method.GetUrl(Url);


                        MatchCollection TitleMatchs = Regex.Matches(html, @"https://[a-z]+.58.com/[a-z]+/[0-9]+x.shtml", RegexOptions.IgnoreCase | RegexOptions.Multiline);


                        ArrayList lists = new ArrayList();

                        foreach (Match NextMatch in TitleMatchs)
                        {

                            if (!lists.Contains(NextMatch.Groups[0].Value))
                            {
                                lists.Add(NextMatch.Groups[0].Value);
                            }


                        }
                        if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                        break;


                    string tm1 = DateTime.Now.ToString();                                                                            //获取系统时间
                    textBox1.Text = tm1 + "-->正在采集第" + i + "页";

                        foreach (string list in lists)
                        {

                            int index = this.skinDataGridView1.Rows.Add();

                            // str = str.Substring(str.Length - i) 从右边开始取i个字符

                            string Url2 = "http://m.58.com/" + city + "/ershoufang/" + list.Substring(list.Length - 21);                       //获取二手房手机端的网址

                            string strhtml = Method.GetUrl(list);                                                                                //定义的GetRul方法 返回 reader.ReadToEnd()

                            string strhtml2 = Method.GetUrl(Url2);                                                                               //请求手机端网址

                            string title = @"<h1 class=""c_333 f20"">([\s\S]*?)</h1>";  //标题
                            string Rxg = @"<h2 class=""agent-title"">([\s\S]*?)</h2>";  //手机端正则匹配联系人
                            string Rxg1 = @"<p class='phone-num'>([\s\S]*?)<";   //电话
                            string Rxg2 = @"<li class=""address-info"">([\s\S]*?) -";//手机端地区
                            string Rxg3 = @"小区：([\s\S]*?)</h2>";//手机端小区
                            string Rxg4 = @"面积</p>([\s\S]*?)</p>"; //手机端面积去除标签



                            Match titles = Regex.Match(strhtml, title);
                            Match contacts = Regex.Match(strhtml2, Rxg);                                                        //手机端正则匹配联系人
                            Match tell = Regex.Match(strhtml, Rxg1);
                            Match area = Regex.Match(strhtml2, Rxg2);

                            Match xiaoqu = Regex.Match(strhtml2, Rxg3);
                            Match mianji = Regex.Match(strhtml2, Rxg4);


                            this.skinDataGridView1.Rows[index].Cells[0].Value = titles.Groups[1].Value;

                            this.skinDataGridView1.Rows[index].Cells[1].Value = contacts.Groups[1].Value;

                            this.skinDataGridView1.Rows[index].Cells[2].Value = tell.Groups[1].Value;


                            this.skinDataGridView1.Rows[index].Cells[3].Value = city + "" + area.Groups[1].Value;


                            this.skinDataGridView1.Rows[index].Cells[4].Value = xiaoqu.Groups[1].Value;


                            string temp = Regex.Replace(mianji.Groups[1].Value, "<[^>]*>", "");
                            this.skinDataGridView1.Rows[index].Cells[5].Value = temp.Trim();

                            this.skinDataGridView1.Rows[index].Cells[6].Value = list;

                            this.skinDataGridView1.CurrentCell = this.skinDataGridView1.Rows[index].Cells[0];                         //让datagridview滚动到当前行

                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            Application.DoEvents();
                            System.Threading.Thread.Sleep(1000);   //内容获取间隔，可变量


                        }
                    }
                   

                }
            }



            catch (System.Exception ex)
            {

                ex.ToString();
            }

        }
        #region 设置表格格式

        public void setDatagridview(DataGridView dgv, int count, string[] headers)
        {

            dgv.ColumnCount = count;


            for (int i = 0; i < count; i++)
            {
                dgv.Columns[i].HeaderText = headers[i];

            }
        }

        #endregion

        #endregion

        #region  生意转让、商铺出租、商铺出售
        public void shangpu(object item)
        {
            string[] headers = { "标题", "联系人", "电话", "地区", "网址" };

            setDatagridview(skinDataGridView1, 5, headers);

            try
            {

                ArrayList citys = Method.get58CityNames();

                foreach (string city in citys)
                {


                    for (int i = 1; i <= 1; i++)
                    {
                        String Url = "http://" + city + ".58.com/" + item.ToString() + "/0/pn" + i + "/";
                        string html = Method.GetUrl(Url);


                        MatchCollection TitleMatchs = Regex.Matches(html, @"https://[a-z]+.58.com/[a-z]+/[0-9]+x.shtml", RegexOptions.IgnoreCase | RegexOptions.Multiline);



                        ArrayList lists = new ArrayList();

                       
                        foreach (Match NextMatch in TitleMatchs)
                        {
                            if (!lists.Contains(NextMatch.Groups[0].Value))
                            {
                                lists.Add(NextMatch.Groups[0].Value);
                            }

                        }
                        if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                            break;

                        string tm1 = DateTime.Now.ToString();  //获取系统时间

                        textBox1.Text = tm1 + "-->正在采集第" + i + "页";



                        foreach (string list in lists)
                        {
                            int index = this.skinDataGridView1.Rows.Add();
                            String Url1 = list;
                            string strhtml = Method.GetUrl(Url1);  //定义的GetRul方法 返回 reader.ReadToEnd()


                            string title = @"<h1 class=""c_000 f20"">([\s\S]*?)</h1>";
                            string Rxg = @"<span class='f14 c_333 jjrsay'>([\s\S]*?)</span>";
                            string Rxg1 = @"<p class='phone-num'>([\s\S]*?)</p>";
                            string Rxg2 = @"详细地址:</span>([\s\S]*?)</span>";



                            Match titles = Regex.Match(strhtml, title);
                            Match contacts = Regex.Match(strhtml, Rxg);
                            Match tell = Regex.Match(strhtml, Rxg1);
                            Match region = Regex.Match(strhtml, Rxg2);



                            this.skinDataGridView1.Rows[index].Cells[0].Value = titles.Groups[1].Value.Trim();

                            this.skinDataGridView1.Rows[index].Cells[1].Value = contacts.Groups[1].Value.Trim();
                            this.skinDataGridView1.Rows[index].Cells[2].Value = tell.Groups[1].Value;

                            string temp = Regex.Replace(region.Groups[1].Value, "<[^>]*>", "");
                            this.skinDataGridView1.Rows[index].Cells[3].Value = temp.Trim().Replace(" ", "").Replace("&nbsp;", "");

                            this.skinDataGridView1.Rows[index].Cells[4].Value = list;


                            this.skinDataGridView1.CurrentCell = this.skinDataGridView1.Rows[index].Cells[0];  //让datagridview滚动到当前行
                            while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            Application.DoEvents();
                            Thread.Sleep(1000);   //内容获取间隔，可变量

                        }
                        

                    }
                }
            }

            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }


        }


        #endregion
        private void button2_Click(object sender, EventArgs e)
        {
            Method.DgvToTable(this.skinDataGridView1);
            Method.DataTableToExcel(Method.DgvToTable(this.skinDataGridView1), "Sheet1", true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                Thread thread = new Thread(new ThreadStart(ershoufang));
                Control.CheckForIllegalCrossThreadCalls = false;
                thread.Start();
            }

            else if (radioButton2.Checked == true)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(shangpu));
                string o = "shangpucs";
                thread.Start((object)o);
                
            }

            else if (radioButton3.Checked == true)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(shangpu));
                string o = "shangpucz";
                thread.Start((object)o);

            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.zanting = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.zanting = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            skinDataGridView1.Rows.Clear();
        }
    }
}
