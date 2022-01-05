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

namespace 主程序202110
{
    public partial class 纪念币预约 : Form
    {
        public 纪念币预约()
        {
            InitializeComponent();
        }

        private void 纪念币预约_Load(object sender, EventArgs e)
        {
        }


        public void run()
        {
           
                try
                {
                
                

                    string card = "";
                    string phone = "";
                    string url = "https://eapply.abchina.com/coin/coin/CoinSubmitRequest?issueid=I105&orgsbatch=1";


                    //orglevel银行分级网点
                    //cardvalue0 数量
                    //cardid0  币ID
                    string postdata = "name=%E5%91%A8%E5%93%A5&cardtype=0&identNo=321321199903032222&mobile=17606117606&piccode=&phoneCaptchaNo=&orglevel1=509999&orglevel2=501000&orglevel3=501740&orglevel4=5017405&cardvalue0=10&cardid0=201916&coindate=2021-12-25&issueid=I105&orgsbatch=1&extraparam=param";
                    string html = method.PostUrlDefault(url, postdata, "");
                    string yuyue = "查询失败";
                    string xingming = "";
                    string name = "";
                    string shuliang = "";
                    string wangdian = "";
                    string shoujihao = "";
                    string addr = "";
                    Thread.Sleep(2000);
                    MessageBox.Show("非预约期");
                    ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(card);
                    lv1.SubItems.Add(phone);
                    lv1.SubItems.Add(yuyue);
                    lv1.SubItems.Add(name);
                    lv1.SubItems.Add(shuliang);
                    lv1.SubItems.Add(wangdian);
                    lv1.SubItems.Add(xingming);
                    lv1.SubItems.Add(shoujihao);
                    lv1.SubItems.Add(addr);
                    Thread.Sleep(1000);

                while (this.zanting == false)
                {
                    Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                }
            }
                catch (Exception ex)
                {

                    //continue;
                }
            
          
        }


        Thread thread;
        bool zanting = true;
        private void button1_Click(object sender, EventArgs e)
        {
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
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
                    string[] value = text[i].Split(new string[] { textBox1.Text.Trim() }, StringSplitOptions.None);
                    if (value.Length > 1)
                    {
                        ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                        lv1.SubItems.Add(value[0]);
                        lv1.SubItems.Add(value[1]);
                    }
                }
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox3.Text=="东明县支行")
            {
                comboBox4.Items.Clear();
                comboBox4.Items.Add("东城分理处");
                comboBox4.Items.Add("东明县支行营业部");
                comboBox4.Items.Add("城关分理处");
            }

            if (comboBox3.Text == "单县支行")
            {
                comboBox4.Items.Clear();
                comboBox4.Items.Add("开发区支行");
                comboBox4.Items.Add("城关分理处");
                comboBox4.Items.Add("郭村分理处");
                comboBox4.Items.Add("黄岗分理处");
                comboBox4.Items.Add("蔡堂分理处");
                comboBox4.Items.Add("单县支行营业部");
            }

            if (comboBox3.Text == "曹县支行")
            {
                comboBox4.Items.Clear();
                comboBox4.Items.Add("磐石支行");
                comboBox4.Items.Add("北城分理处");
                comboBox4.Items.Add("南城分理处");
                comboBox4.Items.Add("清江路分理处");   
                comboBox4.Items.Add("曹县支行营业部");
            }
            if (comboBox3.Text == "郓城县支行")
            {
                comboBox4.Items.Clear();
                comboBox4.Items.Add("水浒支行");
                comboBox4.Items.Add("金河支行");
                comboBox4.Items.Add("南城分理处");
                comboBox4.Items.Add("武安分理处");
                comboBox4.Items.Add("潘渡分理处");
                comboBox4.Items.Add("黄安路分理处");
                comboBox4.Items.Add("郓城县支行营业部");
            }
            if (comboBox3.Text == "开发区支行")
            {
                comboBox4.Items.Clear();
                comboBox4.Items.Add("岳程支行");
                comboBox4.Items.Add("和平支行");
                comboBox4.Items.Add("花城支行");
                comboBox4.Items.Add("桂陵路分理处");
                comboBox4.Items.Add("市中分理处");
                comboBox4.Items.Add("双河分理处");
                comboBox4.Items.Add("火车站分理处");
                comboBox4.Items.Add("开发区支行营业部");
            }
            if (comboBox3.Text == "鄄城县支行")
            {
                comboBox4.Items.Clear();
                comboBox4.Items.Add("城区分理处");
                comboBox4.Items.Add("鄄城县支行营业部");
            }
            if (comboBox3.Text == "牡丹支行")
            {
                comboBox4.Items.Clear();
                comboBox4.Items.Add("师专路分理处");
                comboBox4.Items.Add("牡丹支行营业部");

                comboBox4.Items.Add("国花支行");
                comboBox4.Items.Add("东方分理处");
                comboBox4.Items.Add("曹州路支行");
                comboBox4.Items.Add("解放北街分理处");
                comboBox4.Items.Add("高新区支行");
            }
            if (comboBox3.Text == "成武县支行")
            {
                comboBox4.Items.Clear();
                comboBox4.Items.Add("文亭分理处");
                comboBox4.Items.Add("成武县支行营业部");
            }
            if (comboBox3.Text == "巨野县支行")
            {
                comboBox4.Items.Clear();
                comboBox4.Items.Add("银星分理处");
                comboBox4.Items.Add("永丰支行");
                comboBox4.Items.Add("巨野县支行营业部");
            }

            if (comboBox3.Text == "定陶区支行")
            {
                comboBox4.Items.Clear();
                comboBox4.Items.Add("东城分理处");
               ;
                comboBox4.Items.Add("定陶支行营业部");
            }
            if (comboBox3.Text == "菏泽分行营业部")
            {
                comboBox4.Items.Clear();
                
                comboBox4.Items.Add("菏泽分行营业部营业室");
            }
            if (comboBox3.Text == "市中支行")
            {
                comboBox4.Items.Clear();

                comboBox4.Items.Add("市中支行营业部");
            }

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
