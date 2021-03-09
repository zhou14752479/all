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

namespace 主程序202101
{
    public partial class 快递查询 : Form
    {
        public 快递查询()
        {
            InitializeComponent();
        }
        Thread thread;
        bool zanting = true;
        bool status = true;
        string cookie = "BIDUPSID=A71B424CE88935CF0B85D5E3FE18C45D; PSTM=1589341639; BAIDUID=A71B424CE88935CFBE40D6274B822189:FG=1; H_WISE_SIDS=148078_146000_151099_151249_150685_152056_150076_147088_141748_150084_148867_150796_148713_150746_147279_152308_150043_152778_150984_151015_148524_151032_127969_146550_152495_149718_146653_151320_151951_146732_145788_152741_131423_152014_152114_147527_107316_151580_152275_149253_151220_152284_151871_144966_152272_152512_139883_152457_152739_149807_152247_147547_148868_151703_110085; MCITY=-%3A; __yjs_duid=1_4fc6e83f85878668ae372af8d23e9e461608860016280; BDORZ=B490B5EBF6F3CD402E515D22BCDA1598; BDSFRCVID_BFESS=dnCOJeC62CEi1rJrU2KvbQSQ1mqUgB7TH6aozIYcJ6McfWPCn2sKEG0P8U8g0KubuvGYogKKL2OTHmIF_2uxOjjg8UtVJeC6EG0Ptf8g0f5; H_BDCLCKID_SF_BFESS=tJCDVC0MJK_3fP36q6_Wq4tehHRJJUo9WDTm_DoTJqn-SRo43joH34Du0nLD26QztIbX-pPKKR7SHPcJ-jbl-f41-qji5CrO3mkjbn5yfn02OP5PL6OU5P4syPRrJfRnWIjAKfA-b4ncjRcTehoM3xI8LNj405OTbIFO0KJzJCFhhItRj5u-ePI0hq3Q-46KHJIOsJOOaCvDhhROy4oWK441DnPDLjQHQRTKKRolLMDaDqvoD-Jc3M04X-_OhRL8Qnnp2f_a3UQFJ-0xQft20b0EeMJu54PLyIOQQn7jWhk2ep72y5jvQlRX5q79atTMfNTJ-qcH0KQpsIJM5-DWbT8IjHCeJ6tftnFOVIvO5JQEHnKk5PjDMR-jqlOybTnbae79aJ5nJDoTsJIlKx6K3jDU5x5IQh_Da66P0h_5QpP-HJ7uyqrjy6KI-UjUQUJXtj7iKl0MLInlbb0xynoD5JK1WfnMBMnrteOnan6Y3fAKftnOM46JehL3346-35543bRTLnLy5KJtMDcnK4-XDTcXDH5P; BAIDUID_BFESS=98217108B6FC2A44C653E7CB7C1F6B13:FG=1; BDUSS=5hfn51Y0U1T08xeGFDTjdybXFyakVMM35iMVd4SkV2eXlrQklrYzZ3MmlnaUJnSVFBQUFBJCQAAAAAAQAAAAEAAABio5cbemhvdTE0NzUyNDc5AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAKL1-F-i9fhfaE; BDUSS_BFESS=5hfn51Y0U1T08xeGFDTjdybXFyakVMM35iMVd4SkV2eXlrQklrYzZ3MmlnaUJnSVFBQUFBJCQAAAAAAQAAAAEAAABio5cbemhvdTE0NzUyNDc5AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAKL1-F-i9fhfaE; delPer=0; PSINO=5; ZD_ENTRY=baidu; H_PS_PSSID=33423_33420_33256_31660_32971_33284_33287_33336_22157_33370; BA_HECTOR=8ha4ak0105848l21qg1fviktu0q";
        public void run()
        {

            if (textBox1.Text == "")
            {
                MessageBox.Show("请选择TXT");
                return;
            }
            StreamReader sr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
            //一次性读取完 
            string texts = sr.ReadToEnd();

            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] != "")
                {
                    label3.Text = text[i];
                    string url = "https://express.baidu.com/express/api/express?query_from_srcid=&isBaiduBoxApp=10002&isWisePc=10020&tokenV2=aLR1aRC6Lbzbj2X1kiudus40pR-AiVnbZSO9oQJjGg8lvVfitg8hgZPioopNwhPW&cb=jQuery110203853112159186509_1610241621009&appid=4001&com=&nu="+text[i].Trim()+"&vcode=&token=&qid=d8888c19008f197a&_=1610241621012";
                   
                    string html = method.GetUrlWithCookie(url,cookie,"utf-8");
                   
                    MatchCollection txts = Regex.Matches(html, @"""desc"":""([\s\S]*?)""");

                    for (int j= 0; j < txts.Count; j++)
                    {
                        Match zhandian = Regex.Match(method.Unicode2String(txts[j].Groups[1].Value), @"【([\s\S]*?)】");

                        MatchCollection haomas = Regex.Matches(method.Unicode2String(txts[j].Groups[1].Value), @"\d{7,}");
                        for (int a = 0; a < haomas.Count; a++)
                        {

                            ListViewItem lv1 = listView1.Items.Add(listView1.Items.Count.ToString()); //使用Listview展示数据
                            lv1.SubItems.Add(text[i]);
                            lv1.SubItems.Add(method.Unicode2String(zhandian.Groups[1].Value)+j);
                            lv1.SubItems.Add(haomas[a].Groups[0].Value);
                        }
                      

                       
                    }
                    Thread.Sleep(1000);
                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }
                    if (status == false)
                        return;

                }
            }



        }
        private void button3_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"qichachafapiao"))
            {
                MessageBox.Show("验证失败");
                return;
            }



            #endregion
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
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

        private void button6_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void 快递查询_Load(object sender, EventArgs e)
        {

        }

        private void 快递查询_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
