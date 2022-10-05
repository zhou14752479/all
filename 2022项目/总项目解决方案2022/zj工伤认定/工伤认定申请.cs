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

namespace zj工伤认定
{
    public partial class 工伤认定申请 : Form
    {
        public 工伤认定申请()
        {
            InitializeComponent();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

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

        bool zanting = true;
        bool status = false;
        Thread thread;
        DataTable dt;


        private void button1_Click(object sender, EventArgs e)
        {
            cookie = textBox2.Text.Trim();
            
            if (textBox1.Text == "")
            {
                MessageBox.Show("请先导入表格");
                return;
            }

            if (status == true)
            {
                status = false;
                label2.Text = "已停止";
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
        public static string rgebgdgdvsdfsdvsdfsdvdsbgdsrt435b515sdfsdf(string aefesfsgvsg, string ewrwrwer324234)
        {
            string sfsfewfwefwr234vsdadacszc = method.GetUrl("http://www.acaiji.com/zhejiang/zhejiang.php?type=" + aefesfsgvsg + "&time=" + ewrwrwer324234, "utf-8");
            return sfsfewfwefwr234vsdadacszc.Trim();
        }
        string cookie = "cna=nHxyG3AA8QwCAf////+GB2NJ; _bl_uid=8OlL372dvz4pp99mauqLmdb6ztbh; ZJZWFWSESSIONID=a7ae43b2-a79e-4c9d-96e9-0c5d26640b9b; amlbcookie=01; PROXY_URL=\"https://esso.zjzwfw.gov.cn/opensso/oauth2c/OAuthProxy.jsp\"; zjzwfwloginhx=13394581; ORIG_URL=\"https://esso.zjzwfw.gov.cn/opensso/UI/Login?goto=https://esso.zjzwfw.gov.cn/opensso/spsaehandler/metaAlias/sp?spappurl=\"; URL_FOR_REG=https%3A%2F%2Fesso.zjzwfw.gov.cn%2Fopensso%2FUI%2FLogin%3Fgoto%3Dhttps%3A%2F%2Fesso.zjzwfw.gov.cn%2Fopensso%2Fspsaehandler%2FmetaAlias%2Fsp%3Fspappurl%3D; iPlanetDirectoryPro=AQIC5wM2LY4SfczwREkoBmEhsKglBw__26-7viZvssjIHNM.*AAJTSQACMDIAAlNLABQtNTYyOTE4MjIxNDMyMzUxMjEyMAACUzEAAjAx*; ssxmod_itna=eqGEAKY5BIPAoxlR=+qG=H8SQDuDGq27eGQ8FDl=CexA5D8D6DQeGTrRt+Iep5eD6YPiNItCh035tW/dgfbQbEmimFDU4i8DCuTodGxem=D5xGoDPxDeDA7qGaDb4DrRPqGpc2Lss1OD3qDwDB=DmqG2DlNDA4DjFqx4Fwt6QeiexGtNAkGxFqDMD7tD/SDbveDB3ZW4nwttZeGWbEQ4oeGuDG6Ppwdex0PyBut2mce5bEe5+0fbDgfal7DibAoNi2weK7vPUim3DEmPbB+xFA+n6AODGfG3DkDxD===; ssxmod_itna2=eqGEAKY5BIPAoxlR=+qG=H8SQDuDGq27eGQ8D6hFbBpD40yi+d035ztcqpleH0DnRh8xc=7h0bP5jdg0+d2Ym7iNtP4339GP3zPE0dQyxQB+6hMbF1cIT6R/gBBfTl6j0x=hfzs2wIqNBUfUvF3YEejtunRYUUdA2lTvuRTRnFQihfl6vClYuiMrcKPQuCwdeZWE8YAKNliXQE8iuBwcBzcNHXmA1sb5Gk171Xlf+rLBVFqUxCejv+QS8OWAraWNS9Q24suQ5z6BdevLPKkyKaIdi83OeN=L3dh8wAkDUw6Y6GLmfBVilEzuGF1xkH+GPL5Zb=vAKAhiyhzC2xH2xCM4yxsext5i37G+G2qCloz7bYgb5xphOHwDdAgDlqY=OhjlK7gabgFQloV+dlAKhS8c7bZjKDja4=tgdAVeFBlA5LialhNGD0BDyuxOMD7udTrxrgLoo5QxeiQot8Dzlbf16C2qXSQmWYhAYmM7kkGrfeOLdaEe36T34PD7QsCxiW4OmuFnm7zeFmBPV5FBhpDg1iNDjKDewD4D; zh_choose_undefined=s; session=9f340693f9d1437bb6ccae9b2554e6dd; clientUserType=legal; REG_URL=https%3A%2F%2Fesso.zjzwfw.gov.cn%2Fopensso%2FUI%2FLogin%3Fgoto%3Dhttps%3A%2F%2Fesso.zjzwfw.gov.cn%2Fopensso%2Fspsaehandler%2FmetaAlias%2Fsp%3Fspappurl%3Dhttps%3A%2F%2Fportal.zjzwfw.gov.cn%2Fportal%2Flegal%2Flogin%3F1%3D1%26goto%3Dhttps%253A%252F%252Frecept.zjzwfw.gov.cn%252Fonline%252Faccept%2523%252Faccept%252Fform%253FmatterType%253DpowerDirectory%2526matterId%253D101201980%2526syncUserType%253Dtrue%2526useDetailNew%253Dtrue%2526endpoint%253Donline%2526sequenceNo%253Db8f7ddfe6dab4ca08fc99a49fff38f93%2526viewId%253DForm%2526config%253D%25257B%252522viewId%252522%25253A%252522Form%252522%25257D; SAEORGURL=\"\"; SAEORGPOSTPARAMS=\"\"; C_zj_gsid=1f4371319c024acaaf743302ae21814b-gsid-; C_zj_platform=h5; C_zj_accountType=legal; SERVERID=e529133a5b74b3caa0c97e3b5a97be14|1664195273|1664195268";
        public void run()
        {
            string gregegedrgerheh = rgebgdgdvsdfsdvsdfsdvdsbgdsrt435b515sdfsdf("1", "112233");
            string sign = gregegedrgerheh.Split(new string[] { "," }, StringSplitOptions.None)[0];
            string zj_ggsjpt_sign = gregegedrgerheh.Split(new string[] { "," }, StringSplitOptions.None)[1];
            string expiretime = gregegedrgerheh.Split(new string[] { "," }, StringSplitOptions.None)[2];

            if (DateTime.Now > Convert.ToDateTime(expiretime))
            {
               
                return;
            }

            try
            {
                for (int a = 0; a < dt.Rows.Count; a++)
                {
                  

                    DataRow dr = dt.Rows[a];
                    string uid = dr[0].ToString();
                    string timestr = method.GetTimeStamp();

                  


                    string url = "https://recept.zjzwfw.gov.cn/form/commonAsyncDatashare";

                    label2.Text = "正在查询：" + uid;
                    string postdata = "{\"key_certNumber\":\"self.ZJHM\",\"entityCode\":\"gov.metadatacenter.zwwInterface.query\",\"scriptId\":\"1031400475\",\"zwwtype\":\"/gt/injuryCogApply/person/newNetwork/queryInjuryCogInfo\",\"p1\":\"330000\",\"p2\":\""+ uid + "\",\"bizCode\":\""+ uid + "\",\"desensitizeConfig\":\"contact.NAME = NAME; applicant.LXDH = LXDH\",\"requestId\":\"queryInjuryCogInfo\",\"sequenceNo\":\"b8f7ddfe6dab4ca08fc99a49fff38f93\",\"matterId\":\"101201980\",\"matterType\":\"powerDirectory\"}";
                    
                    string html = method.PostUrl(url,postdata,cookie, "utf-8", "application/json", "");

                    string name = Regex.Match(html, @"NAME\\"":\\""([\s\S]*?)\\""").Groups[1].Value;
                    string company = Regex.Match(html, @"""DWMC"":""([\s\S]*?)""").Groups[1].Value;
                    string addr = Regex.Match(html, @"LXDZ\\"":\\""([\s\S]*?)\\""").Groups[1].Value;
                    string tel = Regex.Match(html, @"LXDH\\"":\\""([\s\S]*?)\\""").Groups[1].Value;
                    string companyAddr = Regex.Match(html, @"""QYDZ"":""([\s\S]*?)""").Groups[1].Value;
                   

                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(uid);
                    lv1.SubItems.Add(name);
                    lv1.SubItems.Add(tel);

                    lv1.SubItems.Add(addr);
                    lv1.SubItems.Add(company);
                    lv1.SubItems.Add(companyAddr);

                    Thread.Sleep(100);
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
                MessageBox.Show("完成");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
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

        private void 工伤认定申请_FormClosing(object sender, FormClosingEventArgs e)
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
    }
}
