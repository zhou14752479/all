using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main
{



    public partial class dataView : Form
    {
        public int datagridCounts { get; set; }

        pubVariables pub = new pubVariables();
        aFunction af = new aFunction();

        public dataView()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false; // 设置线程之间可以操作

            af.tm.Interval = 1;       //aFunction类的暂停停止的计数器间隔1ms
            af.tm.Tick += new EventHandler(af.tm_Tick);
        }

        private void dataView_Load(object sender, EventArgs e)
        {

        }


        public void run()
        {
            if (pubVariables.citys == null || pubVariables.citys == "")
            {
                MessageBox.Show("请先设置采集区域");
                return;
            }

            af.tm.Start();
            pubVariables.status = true;


            switch (pubVariables.item)
            {



                case "58同城房源":
                    Thread thread = new Thread(new ParameterizedThreadStart(af.ershoufang));
                    DataGridView o = skinDataGridView1;
                    thread.Start((object)o);

                    break;
                case "58同城生意转让":

                    ParameterizedThreadStart s = new ParameterizedThreadStart(af.shengyizr);
                    Thread thread1 = new Thread(s);
                    DataGridView o1 = skinDataGridView1;
                    thread1.Start((object)o1);

                    break;
                case "58同城商铺出租":
                    Thread thread2 = new Thread(new ParameterizedThreadStart(af.shangpucz));
                    DataGridView o2 = skinDataGridView1;
                    thread2.Start((object)o2);

                    break;
                case "58同城商铺出售":
                    Thread thread3 = new Thread(new ParameterizedThreadStart(af.shangpucs));
                    DataGridView o3 = skinDataGridView1;
                    thread3.Start((object)o3);

                    break;
                case "美团美食分类商家":
                    Thread thread4 = new Thread(new ParameterizedThreadStart(af.meituan_meishi));
                    DataGridView o4 = skinDataGridView1;
                    thread4.Start((object)o4);

                    break;
                case "美团其他分类商家":
                    Thread thread5 = new Thread(new ParameterizedThreadStart(af.meituan_others));
                    DataGridView o5 = skinDataGridView1;
                    thread5.Start((object)o5);

                    break;
                case "慧聪网企业采集":
                    Thread thread6 = new Thread(new ParameterizedThreadStart(af.huicong));
                    DataGridView o6 = skinDataGridView1;
                    thread6.Start((object)o6);

                    break;
                case "阿里巴巴供应商企业采集":
                    Thread thread7 = new Thread(new ParameterizedThreadStart(af.alibaba));
                    DataGridView o7 = skinDataGridView1;
                    thread7.Start((object)o7);

                    break;
                case "58同城招聘企业":
                    Thread thread8 = new Thread(new ParameterizedThreadStart(af.company_58));
                    DataGridView o8 = skinDataGridView1;
                    thread8.Start((object)o8);

                    break;
                case "黄页88企业采集":
                    Thread thread9 = new Thread(new ParameterizedThreadStart(af.hy88));
                    DataGridView o9 = skinDataGridView1;
                    thread9.Start((object)o9);

                    break;
                case "百度地图":
                    Thread thread10 = new Thread(new ParameterizedThreadStart(af.baiduMap));
                    DataGridView o10 = skinDataGridView1;
                    thread10.Start((object)o10);

                    break;
                case "腾讯地图":
                    Thread thread11 = new Thread(new ParameterizedThreadStart(af.txMap));
                    DataGridView o11 = skinDataGridView1;
                    thread11.Start((object)o11);

                    break;
                case "搜狗地图":
                    Thread thread12 = new Thread(new ParameterizedThreadStart(af.sougouMap));
                    DataGridView o12 = skinDataGridView1;
                    thread12.Start((object)o12);

                    break;
                case "360地图":
                    Thread thread13 = new Thread(new ParameterizedThreadStart(af.map360));
                    DataGridView o13 = skinDataGridView1;
                    thread13.Start((object)o13);

                    break;
                case "赶集网本地服务":
                    Thread thread14 = new Thread(new ParameterizedThreadStart(af.ganji_bendi));
                    DataGridView o14 = skinDataGridView1;
                    thread14.Start((object)o14);

                    break;
                case "赶集网二手车":
                    Thread thread15 = new Thread(new ParameterizedThreadStart(af.ganji_ershouche));
                    DataGridView o15 = skinDataGridView1;
                    thread15.Start((object)o15);
                    break;
                case "马可波罗网采集":
                    Thread thread16 = new Thread(new ParameterizedThreadStart(af.makeboluo));
                    DataGridView o16 = skinDataGridView1;
                    thread16.Start((object)o16);
                    break;

                case "51搜了网采集":
                    Thread thread17 = new Thread(new ParameterizedThreadStart(af.sole51));
                    DataGridView o17 = skinDataGridView1;
                    thread17.Start((object)o17);
                    break;
                case "物友网企业采集":
                    Thread thread18 = new Thread(new ParameterizedThreadStart(af.wuyou));
                    DataGridView o18 = skinDataGridView1;
                    thread18.Start((object)o18);
                    break;
                case "58同城厂房":
                    Thread thread19 = new Thread(new ParameterizedThreadStart(af.changfang));
                    DataGridView o19 = skinDataGridView1;
                    thread19.Start((object)o19);
                    break;
                case "58同城写字楼":
                    Thread thread20 = new Thread(new ParameterizedThreadStart(af.xiezilou));
                    DataGridView o20 = skinDataGridView1;
                    thread20.Start((object)o20);
                    break;
                case "58同城车库":
                    Thread thread21 = new Thread(new ParameterizedThreadStart(af.cheku));
                    DataGridView o21 = skinDataGridView1;
                    thread21.Start((object)o21);
                    break;
                case "58同城土地":
                    Thread thread22 = new Thread(new ParameterizedThreadStart(af.tudi));
                    DataGridView o22 = skinDataGridView1;
                    thread22.Start((object)o22);
                    break;
                case "顺企网企业采集":
                    Thread thread23 = new Thread(new ParameterizedThreadStart(af.shunqi));
                    DataGridView o23 = skinDataGridView1;
                    thread23.Start((object)o23);
                    break;

            }

        }

        private void skinDataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            this.datagridCounts = this.skinDataGridView1.Rows.Count;
            
        }

        private void skinDataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
                e.Handled = true;
            if (e.Control && e.KeyCode == Keys.A)
                e.Handled = true;
            if (e.Control && e.KeyCode == Keys.X)
                e.Handled = true;
        }

        private void skinDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
