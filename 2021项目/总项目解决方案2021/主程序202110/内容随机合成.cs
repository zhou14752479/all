using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序202110
{
    public partial class 内容随机合成 : Form
    {
        public 内容随机合成()
        {
            InitializeComponent();
        }


        List<string> lists = new List<string>();
        private void button3_Click(object sender, EventArgs ex)
        {
            a = 0;
            b = 0;
            c = 0;
            d = 0;
            e = 0;
            richTextBox_result.Text = "";
        }


        int a = 0;
        int b = 0;
        int c = 0;
        int d = 0;
        int e = 0;

        bool zanting = true;
        public void run()
        {

            Random rd = new Random(Guid.NewGuid().GetHashCode());



            string value_a = richTextBox1.Lines[a].ToString().Trim();
            string value_b = richTextBox2.Lines[b].ToString().Trim();
            string value_c = richTextBox3.Lines[c].ToString().Trim();
            string value_d = richTextBox4.Lines[d].ToString().Trim();
            string value_e = richTextBox5.Lines[e].ToString().Trim();

           
            int suiji_f = rd.Next(0, richTextBox6.Lines.Length);
            string value_f = richTextBox6.Lines[suiji_f].ToString().Trim();

            int suiji_g = rd.Next(0, richTextBox7.Lines.Length);
            string value_g = richTextBox7.Lines[suiji_g].ToString().Trim();

            int suiji_h = rd.Next(0, richTextBox8.Lines.Length);
            string value_h = richTextBox8.Lines[suiji_h].ToString().Trim();

            int suiji_i = rd.Next(0, richTextBox9.Lines.Length);
            string value_i = richTextBox9.Lines[suiji_i].ToString().Trim();

            int suiji_j = rd.Next(0, richTextBox10.Lines.Length);
            string value_j = richTextBox10.Lines[suiji_j].ToString().Trim();

            richTextBox_result.Text = richTextBox_source.Text.Replace("内容栏1", value_a).Replace("内容栏2", value_b).Replace("内容栏3", value_c).Replace("内容栏4", value_d).Replace("内容栏5", value_e).Replace("内容栏6", value_f).Replace("内容栏7", value_g).Replace("内容栏8", value_h).Replace("内容栏9", value_i).Replace("内容栏十", value_j);


            if (a < richTextBox1.Lines.Length - 1)
            {
                a = a + 1;
            }
            else
            {
                a = 0;
            }
            if (b < richTextBox2.Lines.Length - 1)
            {
                b = b + 1;
            }
            else
            {
                b = 0;
            }
            if (c < richTextBox3.Lines.Length - 1)
            {
                c = c + 1;
            }
            else
            {
                c = 0;
            }

            if (d < richTextBox4.Lines.Length - 1)
            {
               d = d + 1;
            }
            else
            {
                d = 0;
            }
            if (e< richTextBox5.Lines.Length - 1)
            {
                e = e + 1;
            }
            else
            {
                e = 0;
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.Clipboard.SetText(richTextBox_result.Text); //复制

            }
            catch (Exception)
            {


            }
        }


        Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
            zanting = true;


            //int[] IntArr = new int[] { 1, 2, 3, 4, 5 ,6}; //整型数组
            //List<int[]> ListCombination = PermutationAndCombination<int>.GetCombination(IntArr, 3); //求全部的3-3组合
            //foreach (int[] arr in ListCombination)
            //{
            //    string value = "";
            //    foreach (int item in arr)
            //    {
            //        value = value + item.ToString();
            //    }
            //    MessageBox.Show(value);
            //}


            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }


        public class PermutationAndCombination<T>
        {
            /// <summary>
            /// 交换两个变量
            /// </summary>
            /// <param name="a">变量1</param>
            /// <param name="b">变量2</param>
            public static void Swap(ref T a, ref T b)
            {
                T temp = a;
                a = b;
                b = temp;
            }
            /// <summary>
            /// 递归算法求数组的组合(私有成员)
            /// </summary>
            /// <param name="list">返回的范型</param>
            /// <param name="t">所求数组</param>
            /// <param name="n">辅助变量</param>
            /// <param name="m">辅助变量</param>
            /// <param name="b">辅助数组</param>
            /// <param name="M">辅助变量M</param>
            private static void GetCombination(ref List<T[]> list, T[] t, int n, int m, int[] b, int M)
            {
                for (int i = n; i >= m; i--)
                {
                    b[m - 1] = i - 1;
                    if (m > 1)
                    {
                        GetCombination(ref list, t, i - 1, m - 1, b, M);
                    }
                    else
                    {
                        if (list == null)
                        {
                            list = new List<T[]>();
                        }
                        T[] temp = new T[M];
                        for (int j = 0; j < b.Length; j++)
                        {
                            temp[j] = t[b[j]];
                        }
                        list.Add(temp);
                    }
                }
            }
            /// <summary>
            /// 递归算法求排列(私有成员)
            /// </summary>
            /// <param name="list">返回的列表</param>
            /// <param name="t">所求数组</param>
            /// <param name="startIndex">起始标号</param>
            /// <param name="endIndex">结束标号</param>
            private static void GetPermutation(ref List<T[]> list, T[] t, int startIndex, int endIndex)
            {
                if (startIndex == endIndex)
                {
                    if (list == null)
                    {
                        list = new List<T[]>();
                    }
                    T[] temp = new T[t.Length];
                    t.CopyTo(temp, 0);
                    list.Add(temp);
                }
                else
                {
                    for (int i = startIndex; i <= endIndex; i++)
                    {
                        Swap(ref t[startIndex], ref t[i]);
                        GetPermutation(ref list, t, startIndex + 1, endIndex);
                        Swap(ref t[startIndex], ref t[i]);
                    }
                }
            }
            /// <summary>
            /// 求从起始标号到结束标号的排列，其余元素不变
            /// </summary>
            /// <param name="t">所求数组</param>
            /// <param name="startIndex">起始标号</param>
            /// <param name="endIndex">结束标号</param>
            /// <returns>从起始标号到结束标号排列的范型</returns>
            public static List<T[]> GetPermutation(T[] t, int startIndex, int endIndex)
            {
                if (startIndex < 0 || endIndex > t.Length - 1)
                {
                    return null;
                }
                List<T[]> list = new List<T[]>();
                GetPermutation(ref list, t, startIndex, endIndex);
                return list;
            }
            /// <summary>
            /// 返回数组所有元素的全排列
            /// </summary>
            /// <param name="t">所求数组</param>
            /// <returns>全排列的范型</returns>
            public static List<T[]> GetPermutation(T[] t)
            {
                return GetPermutation(t, 0, t.Length - 1);
            }
            /// <summary>
            /// 求数组中n个元素的排列
            /// </summary>
            /// <param name="t">所求数组</param>
            /// <param name="n">元素个数</param>
            /// <returns>数组中n个元素的排列</returns>
            public static List<T[]> GetPermutation(T[] t, int n)
            {
                if (n > t.Length)
                {
                    return null;
                }
                List<T[]> list = new List<T[]>();
                List<T[]> c = GetCombination(t, n);
                for (int i = 0; i < c.Count; i++)
                {
                    List<T[]> l = new List<T[]>();
                    GetPermutation(ref l, c[i], 0, n - 1);
                    list.AddRange(l);
                }
                return list;
            }
            /// <summary>
            /// 求数组中n个元素的组合
            /// </summary>
            /// <param name="t">所求数组</param>
            /// <param name="n">元素个数</param>
            /// <returns>数组中n个元素的组合的范型</returns>
            public static List<T[]> GetCombination(T[] t, int n)
            {
                if (t.Length < n)
                {
                    return null;
                }
                int[] temp = new int[n];
                List<T[]> list = new List<T[]>();
                GetCombination(ref list, t, t.Length, n, temp, n);
                return list;
            }
        }


        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {
            string html = "";
            string COOKIE = "";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Proxy = null;//防止代理抓包
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;
                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    html = reader.ReadToEnd();
                    reader.Close();
                }

                response.Close();
                return html;



            }
            catch (System.Exception ex)
            {
                return ex.ToString();

            }



        }
        #endregion

        private void 内容随机合成_Load(object sender, EventArgs e)
        {
            #region 通用检测

            if (!GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"abc147258"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

            #endregion
            GetControls_value(this);




        }

        private void 内容随机合成_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("确认退出吗？", "退出询问"
          , MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result != DialogResult.OK)
            {
                e.Cancel = true;//告诉窗体关闭这个任务取消

            }
            else
            {

                GetControls(this);




            }


        }



        private void GetControls(Control fatherControl)
        {
            Control.ControlCollection sonControls = fatherControl.Controls;
            //遍历所有控件
            foreach (Control control in sonControls)
            {
                if (control is RichTextBox)
                {


                    string path = AppDomain.CurrentDomain.BaseDirectory + "value\\";
                    FileStream fs1 = new FileStream(path + control.Name + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1);
                    sw.WriteLine(control.Text.Trim());
                    sw.Close();
                    fs1.Close();

                }

                if (control.Controls != null)
                {
                    GetControls(control);
                }
            }
        }





        #region 组合获取
        /// <summary>
        /// 获得从n个不同元素中任意选取m个元素的组合的所有组合形式的列表
        /// </summary>
        /// <param name="elements">供组合选择的元素</param>
        /// <param name="m">组合中选取的元素个数</param>
        /// <returns>返回一个包含列表的列表，包含的每一个列表就是每一种组合可能</returns>
        public static List<List<string>> GetCombinationList(List<string> elements, int m)
        {
            List<List<string>> result = new List<List<string>>();//存放返回的列表
            List<List<string>> temp = null; //临时存放从下一级递归调用中返回的结果
            List<string> oneList = null; //存放每次选取的第一个元素构成的列表，当只需选取一个元素时，用来存放剩下的元素分别取其中一个构成的列表；
            string oneElment; //每次选取的元素
            List<string> source = new List<string>(elements); //将传递进来的元素列表拷贝出来进行处理，防止后续步骤修改原始列表，造成递归返回后原始列表被修改；
            int n = 0; //待处理的元素个数

            if (elements != null)
            {
                n = elements.Count;
            }
            if (n == m && m != 1)//n=m时只需将剩下的元素作为一个列表全部输出
            {
                result.Add(source);
                return result;
            }
            if (m == 1)  //只选取一个时，将列表中的元素依次列出
            {
                foreach (string el in source)
                {
                    oneList = new List<string>();
                    oneList.Add(el);
                    result.Add(oneList);
                    oneList = null;
                }
                return result;
            }

            for (int i = 0; i <= n - m; i++)
            {
                oneElment = source[0];
                source.RemoveAt(0);
                temp = GetCombinationList(source, m - 1);
                for (int j = 0; j < temp.Count; j++)
                {
                    oneList = new List<string>();
                    oneList.Add(oneElment);
                    oneList.AddRange(temp[j]);
                    result.Add(oneList);
                    oneList = null;
                }
            }


            return result;
        }


        #endregion




        private void GetControls_value(Control fatherControl)
        {
            Control.ControlCollection sonControls = fatherControl.Controls;
            //遍历所有控件
            foreach (Control control in sonControls)
            {
                if (control is RichTextBox)
                {

                    string path = AppDomain.CurrentDomain.BaseDirectory + "value\\";
                    if (File.Exists(path + control.Name + ".txt"))
                    {

                        StreamReader sr1 = new StreamReader(path + control.Name + ".txt", Encoding.GetEncoding("utf-8"));
                        //一次性读取完 
                        string texts1 = sr1.ReadToEnd();
                        control.Text = texts1.Trim();
                        sr1.Close();
                    }

                }

                if (control.Controls != null)
                {
                    GetControls_value(control);
                }
            }
        }

        private void richTextBox9_TextChanged(object sender, EventArgs e)
        {
            this.richTextBox_result.SelectionStart = this.richTextBox_result.Text.Length;
            this.richTextBox_result.SelectionLength = 0;
            this.richTextBox_result.ScrollToCaret();
        }
    }


}
