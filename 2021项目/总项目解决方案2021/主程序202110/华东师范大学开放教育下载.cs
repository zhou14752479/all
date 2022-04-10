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
        string yuming = "nnxj";
        string path = AppDomain.CurrentDomain.BaseDirectory+"//file//";
        string cookie = "UM_distinctid=17ff2a458ccb32-06ca5429dd241a-f791539-1fa400-17ff2a458cdb75; Authorization=eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiIxODc3NTIwMTk0MiIsImF1ZGllbmNlIjoid2ViIiwiYXV0aCI6WyJST0xFX1RFQUNIRVJfVVNFUiJdLCJjcmVhdGVkIjoxNjQ5NTA3NDQ0MTc2LCJzdWJqZWN0SWQiOiIyYzk4MjhjYzdhYTRlMmFhMDE3YWE3ZGQyOWFmMTQ0ZSIsInN1YklkIjoiMmM5ODI4Y2U3YWNkMDU1NDAxN2FjZDllODc1NjUwZjYiLCJhcmVhQ29kZSI6IjQ1MDEyNiIsImNsYXNzSWQiOiIyYzk4MjhjYzdhYmY3NGMyMDE3YWM4NTRiNmM1NjRhOSIsIm1hbmFnZXJDb2RlIjoiNDUwMTI2Iiwic2Nob29sSWQiOiIyYzk4MjhjZTczMGUwNGJkMDE3MzEzYTIzZDliMDI1OCIsInRlbmFudElkIjoxLCJleHAiOjE2NTAxMTIyNDQsInByb2plY3RJZCI6IjJjOTgyOGNjN2FhNGUyYWEwMTdhYTdkYzQ2NTkxM2Y3In0.6YIR9bq2Zwcm7HuKhnncn9p-rEmIeFjfmg0pXnOKnWPEZVrhKFjcEUqVUhtc0s1cOHIahdEWUeh4Oqv7PW21Xg; Authorization_token=%7B%22name%22%3A%22%E9%9F%A6%E7%87%95%E6%B8%85%22%2C%22avatar%22%3A%22https%3A%2F%2Fgw.alipayobjects.com%2Fzos%2Frmsportal%2FBiazfanxmamNRoxxVxka.png%22%2C%22userid%22%3A%222c9828ce7acd0554017acd9e875650f6%22%2C%22areaCode%22%3A%22450126%22%2C%22areaName%22%3Anull%2C%22tenantId%22%3Anull%2C%22managerCode%22%3A%22450126%22%2C%22notifyCount%22%3A%220%22%2C%22proSubName%22%3Anull%2C%22impersonate%22%3Afalse%2C%22originIdentityUserId%22%3Anull%2C%22areaLevel%22%3Anull%2C%22tenantType%22%3Anull%2C%22schoolId%22%3A%222c9828ce730e04bd017313a23d9b0258%22%2C%22schoolName%22%3Anull%2C%22projectId%22%3A%222c9828cc7aa4e2aa017aa7dc465913f7%22%2C%22projectName%22%3A%22%E5%8C%BA%E5%8E%BF%E5%AD%A6%E6%A0%A1-2021%E5%B9%B4%E5%8D%97%E5%AE%81%E5%B8%82%E4%B8%AD%E5%B0%8F%E5%AD%A6%E6%95%99%E5%B8%88%E4%BF%A1%E6%81%AF%E6%8A%80%E6%9C%AF%E5%BA%94%E7%94%A8%E8%83%BD%E5%8A%9B%E6%8F%90%E5%8D%87%E5%B7%A5%E7%A8%8B2.0%22%2C%22classId%22%3A%222c9828cc7abf74c2017ac854b6c564a9%22%2C%22subjectId%22%3A%222c9828cc7aa4e2aa017aa7dd29af144e%22%2C%22studentId%22%3A%222c9828cc7abf74c2017acd9e875f167c%22%2C%22identityCode%22%3A%2245212319970819192X%22%2C%22projectType%22%3A%22INFOMATIONPROJECT%22%2C%22roles%22%3A%5B%22ROLE_TEACHER_USER%22%5D%2C%22projectStartTime%22%3A%222021-07-15%22%2C%22projectEndTime%22%3A%222022-03-31%22%2C%22planningState%22%3Anull%2C%22testPaperState%22%3A%221%22%2C%22testFlag%22%3A%221%22%2C%22groupType%22%3A%22MEMBER%22%2C%22groupId%22%3A%222c9828cc7bc65845017bc7e2e5ae13f3%22%2C%22educationType%22%3Anull%2C%22token%22%3A%22eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiIxODc3NTIwMTk0MiIsImF1ZGllbmNlIjoid2ViIiwiYXV0aCI6WyJST0xFX1RFQUNIRVJfVVNFUiJdLCJjcmVhdGVkIjoxNjQ5NTA3NDQ0MTc2LCJzdWJqZWN0SWQiOiIyYzk4MjhjYzdhYTRlMmFhMDE3YWE3ZGQyOWFmMTQ0ZSIsInN1YklkIjoiMmM5ODI4Y2U3YWNkMDU1NDAxN2FjZDllODc1NjUwZjYiLCJhcmVhQ29kZSI6IjQ1MDEyNiIsImNsYXNzSWQiOiIyYzk4MjhjYzdhYmY3NGMyMDE3YWM4NTRiNmM1NjRhOSIsIm1hbmFnZXJDb2RlIjoiNDUwMTI2Iiwic2Nob29sSWQiOiIyYzk4MjhjZTczMGUwNGJkMDE3MzEzYTIzZDliMDI1OCIsInRlbmFudElkIjoxLCJleHAiOjE2NTAxMTIyNDQsInByb2plY3RJZCI6IjJjOTgyOGNjN2FhNGUyYWEwMTdhYTdkYzQ2NTkxM2Y3In0.6YIR9bq2Zwcm7HuKhnncn9p-rEmIeFjfmg0pXnOKnWPEZVrhKFjcEUqVUhtc0s1cOHIahdEWUeh4Oqv7PW21Xg%22%7D; CNZZDATA1277803829=1163994958-1649139303-http%253A%252F%252Fnnxj.yanxiuonline.com%252F%7C1649502380";
        

        public void run()
        {
            string zimu = "";
            try
            {

                //string aurl = "http://2021fjxj.yanxiuonline.com/api/v1/onlineLearning/proInfo/pageQuerySub?defaultCurrent=1&pageSize=100&proId=2c9828cc766b25b601766edf0dc359b3";//福建
                string aurl = "http://" + yuming + ".yanxiuonline.com/api/v1/onlineLearning/proInfo/pageQuerySub?defaultCurrent=1&pageSize=100&proId=2c9828cc7aa4e2aa017aa7dc465913f7";
                string ahtml = method.GetUrlWithCookie(aurl, cookie, "utf-8");
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
                            if (Convert.ToDouble(score) >= 80)
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
