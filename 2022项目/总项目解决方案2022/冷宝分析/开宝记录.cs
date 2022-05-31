using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 冷宝分析
{
    public partial class 开宝记录 : Form
    {
        public 开宝记录()
        {
            InitializeComponent();
        }
       
        public void xieruDatagridview(string date,string chang,string anima)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if(listView1.Items[i].SubItems[0].Text==date)
                {
                    switch(chang)
                    {
                        case "第一场":
                            listView1.Items[i].SubItems[1].Text = anima;
                            break;
                        case "第二场":
                            listView1.Items[i].SubItems[2].Text = anima;
                            break;
                        case "第三场":
                            listView1.Items[i].SubItems[3].Text = anima;
                            break;
                        case "第四场":
                            listView1.Items[i].SubItems[4].Text = anima;
                            break;
                        case "第五场":
                            listView1.Items[i].SubItems[5].Text = anima;
                            break;

                    }
                }

            }
        }

        private void 开宝记录_Load(object sender, EventArgs e)
        {
            string colorname = function.colorname;

            this.BackColor = Color.FromName(colorname);
            LblTitle.BackColor = Color.FromName(colorname); 
            listView1.BackColor = Color.FromName(colorname);    

            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(1, 23);
            listView1.SmallImageList = imgList;



            LblTitle.Text = DateTime.Now.ToString(function.softname+"截止MM月份开宝记录");
            DateTime dtNow = DateTime.Now; 
            int days = DateTime.DaysInMonth(dtNow.Year, dtNow.Month);
            for (int i = 1; i < days+1; i++)
            {
                string day = i.ToString();
                if (i < 10)
                {
                    day = "0" + day;
                }
                ListViewItem lv1 = listView1.Items.Add(DateTime.Now.ToString("MM月") + day + "日"); //使用Listview展示数据

               
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
                lv1.SubItems.Add("");
            }


         
            if (function.ExistINIFile(function.inipath))
            {


               
                List<string> sessionlist = function.ReadSessions(function.inipath);
                foreach (string session in sessionlist)
                {
                    List<string> keylist = function.ReadKeys(session, function.inipath);
                    foreach (string key in keylist)
                    {
                        string anima = function.IniReadValue(session, key, function.inipath);
                        xieruDatagridview(session,key,anima);
                    }
                }



            }


        }
    }
}
