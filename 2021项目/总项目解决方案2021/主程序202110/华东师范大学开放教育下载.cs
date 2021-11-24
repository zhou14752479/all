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
        string path = AppDomain.CurrentDomain.BaseDirectory+"//file//";
        string cookie = "UM_distinctid=17d3222f64c3af-0f0b106dbe2449-4343363-1fa400-17d3222f64de2f; p_h5_u=D3BC037F-82EB-4169-BAAF-3124E9278BC9; Authorization=eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiIxODc1MDcwOTAyNiIsImF1ZGllbmNlIjoid2ViIiwiYXV0aCI6WyJST0xFX1RFQUNIRVJfVVNFUiJdLCJjcmVhdGVkIjoxNjM3MjM3MjE1MjcyLCJzdWJqZWN0SWQiOiIyYzk4MjhjYzc2ODhjMjcyMDE3Njg5ZTM1OTY3MmEwYiIsInN1YklkIjoiMmM5ODI4Y2U3OWU5Y2I3ZDAxNzlmNGRiMWQwNzNkZTkiLCJhcmVhQ29kZSI6IjM1MDEwMiIsImNsYXNzSWQiOiIyYzk4MjhjYzc5ZjQzOTI0MDE3OWY0YmRlYzg1NDlkZSIsIm1hbmFnZXJDb2RlIjoiMzUwMTAyIiwic2Nob29sSWQiOiIyYzk4MjhjZTc5ZTljYjdkMDE3OWY0YmRlYzhhMGZhOSIsInRlbmFudElkIjoxLCJleHAiOjE2Mzc4NDIwMTUsInByb2plY3RJZCI6IjJjOTgyOGNjNzY2YjI1YjYwMTc2NmVkZjBkYzM1OWIzIn0.iqKXyYnAD32kHs8CgeKsLJHLpKrEV2iMw0LnKJZRBxJuLhZfVnntr_k5UjQLiW4o5M6l-KA09w1dAZ9l6O90_Q; Authorization_token=%7B%22name%22%3A%22%E9%BB%84%E6%B8%85%22%2C%22avatar%22%3A%22https%3A%2F%2Fgw.alipayobjects.com%2Fzos%2Frmsportal%2FBiazfanxmamNRoxxVxka.png%22%2C%22userid%22%3A%222c9828ce79e9cb7d0179f4db1d073de9%22%2C%22areaCode%22%3A%22350102%22%2C%22areaName%22%3Anull%2C%22tenantId%22%3Anull%2C%22managerCode%22%3A%22350102%22%2C%22notifyCount%22%3A%220%22%2C%22proSubName%22%3Anull%2C%22impersonate%22%3Afalse%2C%22originIdentityUserId%22%3Anull%2C%22areaLevel%22%3Anull%2C%22tenantType%22%3Anull%2C%22schoolId%22%3A%222c9828ce79e9cb7d0179f4bdec8a0fa9%22%2C%22schoolName%22%3Anull%2C%22projectId%22%3A%222c9828cc766b25b601766edf0dc359b3%22%2C%22projectName%22%3A%222021%E5%B9%B4%E7%A6%8F%E5%BB%BA%E9%BC%93%E6%A5%BC%E5%8C%BA%E4%B8%AD%E5%B0%8F%E5%AD%A6%E6%95%99%E5%B8%88%E4%BF%A1%E6%81%AF%E6%8A%80%E6%9C%AF%E5%BA%94%E7%94%A8%E8%83%BD%E5%8A%9B%E6%8F%90%E5%8D%87%E5%B7%A5%E7%A8%8B2.0%E5%9F%B9%E8%AE%AD%22%2C%22classId%22%3A%222c9828cc79f439240179f4bdec8549de%22%2C%22subjectId%22%3A%222c9828cc7688c272017689e359672a0b%22%2C%22studentId%22%3A%222c9828cc79f439240179f4db1d0f7a2c%22%2C%22identityCode%22%3A%22350124199107081400%22%2C%22projectType%22%3A%22INFOMATIONPROJECT%22%2C%22roles%22%3A%5B%22ROLE_TEACHER_USER%22%5D%2C%22projectStartTime%22%3A%222021-06-15%22%2C%22projectEndTime%22%3A%222022-05-31%22%2C%22planningState%22%3Anull%2C%22testPaperState%22%3A%220%22%2C%22testFlag%22%3A%222%22%2C%22groupType%22%3A%22MEMBER%22%2C%22groupId%22%3A%222c9828cc7a9a5a75017aa3cb8cef256f%22%2C%22token%22%3A%22eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiIxODc1MDcwOTAyNiIsImF1ZGllbmNlIjoid2ViIiwiYXV0aCI6WyJST0xFX1RFQUNIRVJfVVNFUiJdLCJjcmVhdGVkIjoxNjM3MjM3MjE1MjcyLCJzdWJqZWN0SWQiOiIyYzk4MjhjYzc2ODhjMjcyMDE3Njg5ZTM1OTY3MmEwYiIsInN1YklkIjoiMmM5ODI4Y2U3OWU5Y2I3ZDAxNzlmNGRiMWQwNzNkZTkiLCJhcmVhQ29kZSI6IjM1MDEwMiIsImNsYXNzSWQiOiIyYzk4MjhjYzc5ZjQzOTI0MDE3OWY0YmRlYzg1NDlkZSIsIm1hbmFnZXJDb2RlIjoiMzUwMTAyIiwic2Nob29sSWQiOiIyYzk4MjhjZTc5ZTljYjdkMDE3OWY0YmRlYzhhMGZhOSIsInRlbmFudElkIjoxLCJleHAiOjE2Mzc4NDIwMTUsInByb2plY3RJZCI6IjJjOTgyOGNjNzY2YjI1YjYwMTc2NmVkZjBkYzM1OWIzIn0.iqKXyYnAD32kHs8CgeKsLJHLpKrEV2iMw0LnKJZRBxJuLhZfVnntr_k5UjQLiW4o5M6l-KA09w1dAZ9l6O90_Q%22%7D; CNZZDATA1277803829=1729544469-1637216530-http%253A%252F%252F2021fjxj.yanxiuonline.com%252F%7C1637228288";
        public void run()
        {
            try
            {
                string aurl = "http://2021fjxj.yanxiuonline.com/api/v1/onlineLearning/proInfo/pageQuerySub?defaultCurrent=1&pageSize=100&proId=2c9828cc766b25b601766edf0dc359b3";
                string ahtml = method.GetUrlWithCookie(aurl, cookie, "utf-8");
                MatchCollection proids = Regex.Matches(ahtml, @"""id"" : ""([\s\S]*?)""");
                MatchCollection subNames = Regex.Matches(ahtml, @"""subName"" : ""([\s\S]*?)""");
                //遍历学科
                for (int i = 4; i < proids.Count; i++)
                {
                    StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "//data.txt", Encoding.GetEncoding("utf-8"));
                    //一次性读取完 
                    string value = sr.ReadToEnd();
                    sr.Close();
                    sr.Dispose();
                    if(value.Contains(subNames[i].Groups[1].Value))
                    {
                        continue;
                    }

                    FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\data.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                    sw.WriteLine(subNames[i].Groups[1].Value);
                    sw.Close();
                    fs1.Close();
                    sw.Dispose();

                    textBox1.Text = "正在下载："+ subNames[i].Groups[1].Value;
                    string burl = "http://2021fjxj.yanxiuonline.com/api/v1/read/activity/segmentParticipations/querySegmentParticipationListV2?defaultCurrent=1&pageSize=9999&type=2&businessIds=" + proids[i].Groups[1].Value;
                    string sPath = path + subNames[i].Groups[1].Value + "//";
                    if (!Directory.Exists(sPath))
                    {
                        Directory.CreateDirectory(sPath); //创建文件夹
                    }
                    string bhtml = method.GetUrlWithCookie(burl, cookie, "utf-8");
                    MatchCollection userids = Regex.Matches(bhtml, @"""userId"" : ""([\s\S]*?)""");
                    MatchCollection stageIds = Regex.Matches(bhtml, @"""stageId"" : ""([\s\S]*?)""");

               
                    for (int j = 0; j < userids.Count; j++)
                    {
                        string url = " http://2021fjxj.yanxiuonline.com/api/v1/activity/segmentParticipations/" + userids[j].Groups[1].Value + "/abilityTaskInfoForLook?stageId=" + stageIds[j].Groups[1].Value;
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
                                    method.downloadFile(fileUrls[a].Groups[1].Value, sPath, fileNames[a].Groups[1].Value, cookie);
                                }
                            }
                        }
                        catch (Exception)
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
