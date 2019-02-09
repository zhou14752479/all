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

namespace fang.临时软件
{
    public partial class LOL : Form
    {
        public LOL()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void LOL_Load(object sender, EventArgs e)
        {

        }

        #region  主函数
        public void run()

        {
            string[] ids = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            try

            {


                        string Url = "http://na.op.gg/summoner/userName=Made%20in%20Jim";

                        string html = method.GetUrl(Url,"utf-8");

                    Match summonerId = Regex.Match(html, @"data-summoner-id=""([\s\S]*?)""");
                    Match lastinfo = Regex.Match(html, @"data-last-info=""([\s\S]*?)""");
                    MatchCollection userNames = Regex.Matches(html, @"userName=([\s\S]*?)\\");

                    int a = userNames.Count;

                MessageBox.Show(a.ToString());
                while (a!= 0)
                    { 
                        string url = "http://na.op.gg/summoner/matches/ajax/averageAndList/startInfo=" + lastinfo.Groups[1].Value.ToString() + "&summonerId=" + summonerId.Groups[1].Value.ToString();

                        string strhtml = method.GetUrl(Url, "utf-8");
                        Match lastinfo1 = Regex.Match(strhtml, @"lastInfo"":([\s\S]*?),");
                        MatchCollection userNames1 = Regex.Matches(strhtml, @"userName=([\s\S]*?)""");
                        for (int i = 0; i < userNames1.Count; i++)
                        {
                        textBox1.Text += userNames1[i].Groups[1].Value.ToString()+"\r\n";
                        }
                        MessageBox.Show(a.ToString());
                        a = userNames1.Count;
                    }
                        
                    }

                
            

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();

        }
    }
}
