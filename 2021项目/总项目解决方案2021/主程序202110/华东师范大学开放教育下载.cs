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
    public partial class 华东师范大学开放教育下载 : Form
    {
        public 华东师范大学开放教育下载()
        {
            InitializeComponent();
        }
        //string yuming = "2021fjxj";
        string yuming = "2020scxj";
        string path = AppDomain.CurrentDomain.BaseDirectory+"//file//";
        string cookie = "UM_distinctid=18069d2dad78fe-0e58b93388c42e-6b3e555b-384000-18069d2dad814ca; Authorization=eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiJnYW9xaW1lbmc0MjkiLCJhdWRpZW5jZSI6IndlYiIsImF1dGgiOlsiUk9MRV9URUFDSEVSX1VTRVIiXSwiY3JlYXRlZCI6MTY1NjU1ODY4OTMwMiwic3ViamVjdElkIjoiMmM5ODI4Y2M4MDQxOGI1YTAxODA0NGJiYzU0ZTE1ODciLCJzdWJJZCI6IjJjOTgyOGQzODA5ODNjZGYwMTgwOWMzNmRhNzk1YTdlIiwiYXJlYUNvZGUiOiI1MTA4MDIiLCJjbGFzc0lkIjoiMmM5ODI4Y2M4MDUwYTk3OTAxODA1ZjAwNzNhNDFkNzgiLCJtYW5hZ2VyQ29kZSI6bnVsbCwic2Nob29sSWQiOiIyYzk4MjhkMzdmMmI2MDNkMDE3ZjJmOWZmZTI0NDc2YiIsInRlbmFudElkIjoxLCJleHAiOjE2NTcxNjM0ODksInByb2plY3RJZCI6IjExNTUifQ.VsfqZhQJrAQ_pIGs8ZAUawaChKQDq2D9uSfLcW9Q_DVzg2AWYuw77G7-_UK7BMSDc1xKrkC_bpxCIHAOR6yHvQ; Authorization_token=%7B%22name%22%3A%22%E9%AB%98%E5%90%AF%E8%90%8C%22%2C%22avatar%22%3A%22https%3A%2F%2Fgw.alipayobjects.com%2Fzos%2Frmsportal%2FBiazfanxmamNRoxxVxka.png%22%2C%22userid%22%3A%222c9828d380983cdf01809c36da795a7e%22%2C%22areaCode%22%3A%22510802%22%2C%22areaName%22%3Anull%2C%22tenantId%22%3Anull%2C%22managerCode%22%3Anull%2C%22notifyCount%22%3A%220%22%2C%22proSubName%22%3Anull%2C%22impersonate%22%3Afalse%2C%22originIdentityUserId%22%3Anull%2C%22areaLevel%22%3Anull%2C%22tenantType%22%3Anull%2C%22schoolId%22%3A%222c9828d37f2b603d017f2f9ffe24476b%22%2C%22schoolName%22%3Anull%2C%22projectId%22%3A%221155%22%2C%22projectName%22%3A%222021%E5%9B%9B%E5%B7%9D%E5%9B%BD%E5%9F%B9%E5%AD%A6%E7%A7%91%E9%AA%A8%E5%B9%B2%E6%95%99%E5%B8%88%E4%BF%A1%E6%81%AF%E5%8C%96%E6%95%99%E5%AD%A6%E5%88%9B%E6%96%B0%E8%83%BD%E5%8A%9B%E6%8F%90%E5%8D%87%E5%9F%B9%E8%AE%AD%EF%BC%88%E5%AD%A6%E5%89%8D%EF%BC%89%22%2C%22classId%22%3A%222c9828cc8050a97901805f0073a41d78%22%2C%22subjectId%22%3A%222c9828cc80418b5a018044bbc54e1587%22%2C%22studentId%22%3A%222c9828d38074ed3c01809c36da9917fe%22%2C%22identityCode%22%3Anull%2C%22projectType%22%3A%22INFOMATIONPROJECT%22%2C%22roles%22%3A%5B%22ROLE_TEACHER_USER%22%5D%2C%22projectStartTime%22%3A%222022-04-19%22%2C%22projectEndTime%22%3A%222022-09-30%22%2C%22planningState%22%3Anull%2C%22testPaperState%22%3A%220%22%2C%22testFlag%22%3A%220%22%2C%22groupType%22%3A%22MEMBER%22%2C%22groupId%22%3A%222c9828d3809e9bde0180bd92a073647b%22%2C%22educationType%22%3A%22PreSchool%22%2C%22token%22%3A%22eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiJnYW9xaW1lbmc0MjkiLCJhdWRpZW5jZSI6IndlYiIsImF1dGgiOlsiUk9MRV9URUFDSEVSX1VTRVIiXSwiY3JlYXRlZCI6MTY1NjU1ODY4OTMwMiwic3ViamVjdElkIjoiMmM5ODI4Y2M4MDQxOGI1YTAxODA0NGJiYzU0ZTE1ODciLCJzdWJJZCI6IjJjOTgyOGQzODA5ODNjZGYwMTgwOWMzNmRhNzk1YTdlIiwiYXJlYUNvZGUiOiI1MTA4MDIiLCJjbGFzc0lkIjoiMmM5ODI4Y2M4MDUwYTk3OTAxODA1ZjAwNzNhNDFkNzgiLCJtYW5hZ2VyQ29kZSI6bnVsbCwic2Nob29sSWQiOiIyYzk4MjhkMzdmMmI2MDNkMDE3ZjJmOWZmZTI0NDc2YiIsInRlbmFudElkIjoxLCJleHAiOjE2NTcxNjM0ODksInByb2plY3RJZCI6IjExNTUifQ.VsfqZhQJrAQ_pIGs8ZAUawaChKQDq2D9uSfLcW9Q_DVzg2AWYuw77G7-_UK7BMSDc1xKrkC_bpxCIHAOR6yHvQ%22%7D";
        public void run()
        {
            string zimu = "";
            try
            {

              
                string aurl = "http://" + yuming + ".yanxiuonline.com/api/v1/onlineLearning/proInfo/pageQuerySub?defaultCurrent=1&pageSize=100&proId=1155";
                string ahtml = method.GetUrlWithCookie(aurl, cookie, "utf-8");
                textBox1.Text = ahtml;
                MatchCollection ids = Regex.Matches(ahtml, @"""id"" : ""([\s\S]*?)""");
                MatchCollection subNames = Regex.Matches(ahtml, @"""subName"" : ""([\s\S]*?)""");
                //遍历学科
                for (int i = 0; i < ids.Count; i++)
                {
                    //判断是否已下载
                    StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "//data.txt", Encoding.GetEncoding("utf-8"));
                    //一次性读取完 
                    string value = sr.ReadToEnd();
                    sr.Close();
                    sr.Dispose();
                    if (value.Contains(subNames[i].Groups[1].Value))
                    {
                        continue;
                    }

                    FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\data.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                    sw.WriteLine(subNames[i].Groups[1].Value);
                    sw.Close();
                    fs1.Close();
                    sw.Dispose();
                    //判断是否已下载



                    textBox1.Text += "正在下载："+ subNames[i].Groups[1].Value+"\r\n";
                    string burl = "http://"+yuming+".yanxiuonline.com/api/v1/read/activity/segmentParticipations/querySegmentParticipationListV2?defaultCurrent=1&pageSize=9999&type=2&businessIds=" + ids[i].Groups[1].Value;



                    string sPath = path + subNames[i].Groups[1].Value + "//";
                    //if (!Directory.Exists(sPath))
                    //{
                    //    Directory.CreateDirectory(sPath); //创建文件夹
                    //}
                    string bhtml = method.GetUrlWithCookie(burl, cookie, "utf-8");
                    MatchCollection userids = Regex.Matches(bhtml, @"""userId"" : ""([\s\S]*?)""");
                    MatchCollection stageIds = Regex.Matches(bhtml, @"""stageId"" : ""([\s\S]*?)""");
                    MatchCollection userNames = Regex.Matches(bhtml, @"""userName"" : ""([\s\S]*?)""");
                    if (userids.Count==0)
                    {
                        continue;
                    }
                    for (int j = 0; j < userids.Count; j++)
                    {
                        


                        string url = " http://" + yuming + ".yanxiuonline.com/api/v1/activity/segmentParticipations/" + userids[j].Groups[1].Value + "/abilityTaskInfoForLook?stageId=" + stageIds[j].Groups[1].Value;
                        string html = method.GetUrlWithCookie(url, cookie, "utf-8");


                      
                        string score = Regex.Match(html, @"""score"" : ""([\s\S]*?)""").Groups[1].Value;
                        MatchCollection fileNames = Regex.Matches(html, @"""fileName"" : ""([\s\S]*?)""");
                        MatchCollection fileUrls = Regex.Matches(html, @"""fileUrl"" : ""([\s\S]*?)""");

                        try
                        {
                            if (Convert.ToDouble(score) > 84)
                            {
                                for (int a = 0; a < fileNames.Count; a++)
                                {


                                  string  zimu1 = Regex.Match(fileNames[a].Groups[1].Value, @"[A-Z]\d{1,5}").Groups[0].Value;
                                    if(zimu1!="" &&zimu1!="P4")
                                    {
                                        zimu = zimu1;
                                    }
                                    string lastpath = sPath + zimu + "//" + userNames[j].Groups[1].Value + "//";
                                    if (!Directory.Exists(lastpath))
                                    {
                                        Directory.CreateDirectory(lastpath); //创建文件夹
                                    }
                                    method.downloadFile(fileUrls[a].Groups[1].Value, lastpath, fileNames[a].Groups[1].Value, cookie);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                          
                            continue;
                        }
                       

                    }

                }
            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

            MessageBox.Show("完成");

        }

        Thread thread;

        private void button1_Click(object sender, EventArgs e)
        {


            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void 华东师范大学开放教育下载_Load(object sender, EventArgs e)
        {

        }

        private void 华东师范大学开放教育下载_FormClosing(object sender, FormClosingEventArgs e)
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
