using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            StreamReader sr = new StreamReader(openFileDialog1.FileName, method.EncodingType.GetTxtType(textBox1.Text));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存
            for (int i = 0; i < text.Length; i++)
            {
                try
                {
                    string[] value = text[i].Split(new string[] { "#" }, StringSplitOptions.None);

                    string card = value[0].Trim();
                    string phone = value[1].Trim();
                    string url = "https://eapply.abchina.com/coin/coin/CoinSubmitRequest?issueid=I105&orgsbatch=1";

                    //orglevel银行分级网点
                    //cardvalue0 数量
                    //cardid0  币ID
                    string postdata = "name=%E5%91%A8%E5%93%A5&cardtype=0&identNo=321321199903032222&mobile=17606117606&piccode=&phoneCaptchaNo=&orglevel1=509999&orglevel2=501000&orglevel3=501740&orglevel4=5017405&cardvalue0=10&cardid0=201916&coindate=2021-12-24&issueid=I105&orgsbatch=1&extraparam=param";
                    string html = method.PostUrlDefault(url, postdata, "");
                    string yuyue = "查询失败";
                    string xingming = "";
                    string name = "";
                    string shuliang = "";
                    string wangdian = "";
                    string shoujihao = "";
                    string addr = "";

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
                }
                catch (Exception ex)
                {

                    continue;
                }
            }
            MessageBox.Show("完成");
        }
    






}
}
