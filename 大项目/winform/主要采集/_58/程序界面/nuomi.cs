using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using CCWin;
using System.Collections;

namespace _58
{
    public partial class nuomi : Form
    {
        public nuomi()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
          

            

        }

        private void skinTreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            label6.Text = e.Node.Name;
            label14.Text = e.Node.Text;
            return;
        }

        
        
        #region 采集
        public void run(int item)
        {
            
            string city = label6.Text;
            if (city == "")
            {
                MessageBox.Show("请选择城市！");
                return;
            }

            for (int i = 1; i <100; i++)
            {
                String Url = "https://" + city + ".nuomi.com/"+item+"-page" + i;
                
                
                string html = Method.GetUrl(Url);  //定义的GetRul方法 返回 reader.ReadToEnd()

                
                
                MatchCollection TitleMatchs = Regex.Matches(html, @"<li class=""shop-infoo-list-item clearfix"">([\s\S]*?)<a href=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                ArrayList lists = new ArrayList();
                foreach (Match NextMatch in TitleMatchs)
                {

                    lists.Add("https:" + NextMatch.Groups[2].Value);


                }

                if (lists.Count ==0)
                    break;

                foreach (string list in lists)
                {

                    int index = this.skinDataGridView1.Rows.Add();
                    string strhtml = Method.GetUrl(list);  //定义的GetRul方法 返回 reader.ReadToEnd()

                    string Rxg = @"name:'([\s\S]*?)'";
                    string Rxg1 = @"address:'([\s\S]*?)'";
                    string Rxg2 = @"phone:'([\s\S]*?)'";



                    Match name = Regex.Match(strhtml, Rxg);
                    Match addr = Regex.Match(strhtml, Rxg1);

                    Match tel = Regex.Match(strhtml, Rxg2);

                    this.skinDataGridView1.Rows[index].Cells[0].Value = name.Groups[1].Value;
                    this.skinDataGridView1.Rows[index].Cells[2].Value = addr.Groups[1].Value;
                    this.skinDataGridView1.Rows[index].Cells[1].Value = tel.Groups[1].Value;
                    this.skinDataGridView1.CurrentCell = this.skinDataGridView1.Rows[i].Cells[0];  //让datagridview滚动到当前行



                    Application.DoEvents();
                    System.Threading.Thread.Sleep(800);

                }
            }
        }

        #endregion

        #region  不同分类酒店采集
        public void hotel()
        {
           
            string city = label6.Text;
            if (city == "")
            {
                MessageBox.Show("请选择城市！");
                return;
            }
            for (int i = 1; i < 100; i++)
            {
                String Url = "https://t.nuomi.com/" + city + "-page" + i;


                string html = Method.GetUrl(Url);  //定义的GetRul方法 返回 reader.ReadToEnd()



                MatchCollection TitleMatchs = Regex.Matches(html, @"<li class=""shop-infoo-list-item clearfix"">([\s\S]*?)<a href=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);

               
                ArrayList lists = new ArrayList();
                foreach (Match NextMatch in TitleMatchs)
                {

                    lists.Add("https:" + NextMatch.Groups[2].Value);


                }

                if (lists.Count == 0)
                    break;

                foreach (string list in lists)
                {



                    String Url1 = list;
                    string strhtml = Method.GetUrl(Url1);  //定义的GetRul方法 返回 reader.ReadToEnd()



                    string Rxg = @"name:'([\s\S]*?)'";
                    string Rxg1 = @"address:'([\s\S]*?)'";
                    string Rxg2 = @"phone:'([\s\S]*?)'";


                    MatchCollection name = Regex.Matches(strhtml, Rxg);
                    MatchCollection add = Regex.Matches(strhtml, Rxg1);

                    MatchCollection tel = Regex.Matches(strhtml, Rxg2);

                    foreach (Match NextMatch in name)
                    {

                        this.skinDataGridView1.Rows[i].Cells[1].Value = NextMatch.Groups[1].Value;


                        this.skinDataGridView1.Columns[0].FillWeight = 20;   //设置列宽
                        this.skinDataGridView1.Columns[3].FillWeight = 40;

                        this.skinDataGridView1.CurrentCell = this.skinDataGridView1.Rows[i].Cells[0];  //让datagridview滚动到当前行

                    }
                    foreach (Match NextMatch in add)
                    {
                        this.skinDataGridView1.Rows[i].Cells[2].Value = NextMatch.Groups[1].Value;

                    }

                    foreach (Match NextMatch in tel)
                    {
                        this.skinDataGridView1.Rows[i].Cells[3].Value = NextMatch.Groups[1].Value;

                    }




                    Application.DoEvents();
                    System.Threading.Thread.Sleep(800);
                }
             
            }
        }

        #endregion
        private void skinButton18_Click(object sender, EventArgs e)
        {
            label8.Text = skinButton18.Text;
        }

        private void skinButton15_Click(object sender, EventArgs e)
        {
            label8.Text = skinButton15.Text;
        }

        private void skinButton16_Click(object sender, EventArgs e)
        {
            label8.Text = skinButton16.Text;
        }

        private void skinButton17_Click(object sender, EventArgs e)
        {
            label8.Text = skinButton17.Text;
        }

        private void skinButton20_Click(object sender, EventArgs e)
        {
            label8.Text = skinButton20.Text;
        }

        private void skinButton21_Click(object sender, EventArgs e)
        {
            label8.Text = skinButton21.Text;
        }

        private void skinButton19_Click(object sender, EventArgs e)
        {
            label8.Text = skinButton19.Text;
        }

        private void skinButton14_Click(object sender, EventArgs e)
        {
            label8.Text = skinButton14.Text;
        }

        private void skinButton13_Click(object sender, EventArgs e)
        {
            label8.Text = skinButton13.Text;
        }

       

        private void skinButton1_Click(object sender, EventArgs e)
        {
            if (label15.Text == "测试用户" || label15.Text == "")
            {
                MessageBox.Show("请注册账号登陆！");
                return;
            }

            skinButton3.Text = "停止采集";


          

            if (label8.Text == "美食类")
            {
                run(326);
            }
            else if (label8.Text == "休闲娱乐")
            {
               run(320);
            }
            else if (label8.Text == "丽人")
            {
                run(955);
            }
            else if (label8.Text == "生活服务")
            {
                run(316);
            }
            else if (label8.Text == "酒店")
            {
                hotel();
            }
            else if (label8.Text == "运动健身")
            {
                run(952);
            }
            else if (label8.Text == "瑜伽")
            {
                run(2159);
            }
            else if (label8.Text == "结婚")
            {
                run(565);
            }
            else if (label8.Text == "培训")
            {
                run(375);
            }

            else
            {
                MessageBox.Show("请选择采集模板！");
            }

        }

        private void skinTreeView1_AfterSelect_1(object sender, TreeViewEventArgs e)
        {
            label6.Text = e.Node.Name;
            label14.Text = e.Node.Text;
            return;
        }


        private void skinButton2_Click(object sender, EventArgs e)
        {
            Method.DataTableToExcel(Method.DgvToTable(this.skinDataGridView1), "Sheet1", true);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("IEXPLORE.EXE", "http://wpa.qq.com/msgrd?v=3&uin=852266010&site=qq&menu=yes");
        }

        private void skinButton22_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("IEXPLORE.EXE", "http://wpa.qq.com/msgrd?v=3&uin=852266010&site=qq&menu=yes");
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            skinButton3.Text = "已停止";
        }

        private void nuomi_Enter(object sender, EventArgs e)
        {

        }

        private void nuomi_MouseEnter(object sender, EventArgs e)
        {
            label15.Text = Method.User;
        }

      

        private void 登陆账号ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login lg = new Login();
            lg.Show();
        }
    }
}
