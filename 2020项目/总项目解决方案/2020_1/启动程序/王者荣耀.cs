using System;
using System.Collections;
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
using helper;

namespace 启动程序
{
    public partial class 王者荣耀 : Form
    {
        public 王者荣耀()
        {
            InitializeComponent();
        }

        string pifus;
        private void 王者荣耀_Load(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;

            StreamReader sr = new StreamReader(path + "pifu.txt", Encoding.GetEncoding("utf-8"));

            pifus = sr.ReadToEnd();

            webBrowser1.Navigate("https://user.qzone.qq.com/");
            webBrowser1.ScriptErrorsSuppressed = true;
            method.SetWebBrowserFeatures(method.IeVersion.IE10);
        }
        string cookie;


        public string getRoleId()
        {

            try
            {
                string url = "https://yxzj.aci.game.qq.com/main?game=yxzj&area=1&partition=1226&callback=158701422552319029&sCloudApiName=ams.gameattr.role&iAmsActivityId=https%3A%2F%2Fpvp.qq.com%2Fweb201605%2Fartdetail.shtml";
                string html = method.GetUrlWithCookie(url, cookie, "gb2312");
               
                Match role = Regex.Match(html, @"&openid=([\s\S]*?)&");
                return role.Groups[1].Value;
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }
        }


        public void run()
        {
            try
            {



                string url = "https://mapps.game.qq.com/yxzj/web201605/GetHeroSkin.php?appid=1104466820&area="+ textBox2.Text.Trim()+ "&partition="+textBox3.Text.Trim()+"&roleid="+getRoleId()+"&r=0.4968435418279442";
                string html = method.GetUrlWithCookie(url, cookie, "gb2312");
               
                 Match match = Regex.Match(html, @"""HeroSkinStr"":\{([\s\S]*?)}");
                string[] texts = match.Groups[1].Value.Split(new string[] { "," }, StringSplitOptions.None);

                for (int i = 0; i < texts.Length; i++)  //循环拥有的英雄ID：皮肤ID
                {
                    string[] values = texts[i].Split(new string[] { ":" }, StringSplitOptions.None);
                    char[] strCharArr = values[1].Replace("\"", "").ToCharArray();
                    ArrayList lists = new ArrayList();
                  
                    for (int a = strCharArr.Length-1; a>=0; a--)
                    {
                        if (strCharArr[a].ToString() == "1")
                        {
                            lists.Add(strCharArr.Length - 1-a);
                        }
                       
                    }

                  

                    MatchCollection HeroIds = Regex.Matches(pifus, @"""iHeroId"":""([\s\S]*?)""");
                    MatchCollection Heronames = Regex.Matches(pifus, @"""szHeroTitle"":""([\s\S]*?)""");
                    MatchCollection pifunames = Regex.Matches(pifus, @"""szTitle"":""([\s\S]*?)""");

                    ArrayList alists = new ArrayList();
                    string hero = "";
                    for (int j = 0; j < HeroIds.Count; j++)
                    {
                      
                        if (values[0].Replace("\"", "") == HeroIds[j].Groups[1].Value)
                        {
                            alists.Add(pifunames[j].Groups[1].Value+" ");
                            hero = Heronames[j].Groups[1].Value;

                        }                

                    }

               


                    StringBuilder sb = new StringBuilder();
                    for (int z = 0; z < lists.Count; z++)
                    {
                        try
                        {
                            int zhi = Convert.ToInt32(lists[z]);


                            if (z != 0)
                            {
                                sb.Append(alists[zhi]);
                            }

                        }
                        catch 
                        {
                            sb.Append("！");
                            continue;
                        }
                     
                           
                      

                       
                    }






                    textBox1.Text +=hero+" " + sb.ToString()+" ";
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
             cookie = method.GetCookies("https://pvp.qq.com/web201605/artdetail.shtml");

            run();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }
    }
}
