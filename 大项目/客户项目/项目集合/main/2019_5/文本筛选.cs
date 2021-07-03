using Microsoft.Win32;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main._2019_5
{
    public partial class 文本筛选 : Form
    {
        public 文本筛选()
        {
            InitializeComponent();
        }

        string strpath = "";
        string heipath = "";

        /// <summary>
        /// 根据字节数截取字符串
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="LimitSize"></param>
        /// <returns></returns>
        private string GetSubStringByLength(string Text, int LimitSize)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(Text);
            int nByte = 0;                       //  表示当前的字节数
            int i = 0;                              //  要截取的字节数
            for (; i < bytes.GetLength(0) && nByte < LimitSize; i++)
            {
                if (i % 2 == 0)                 //  偶数位置，如0、2、4等，为UCS2编码中两个字节的第一个字节
                {
                    nByte++;                    //  在UCS2第一个字节时n加1
                }
                else
                {
                    if (bytes[i] > 0)               //  当UCS2编码的第二个字节大于0时，该UCS2字符为汉字，一个汉字算两个字节
                    {
                        nByte++;
                    }
                }
            }

            //  如果i为奇数时，处理成偶数
            if (i % 2 == 1)
            {
                if (bytes[i] > 0)                               //  该UCS2字符是汉字时，去掉这个截一半的汉字
                    i = i - 1;
                else
                    i = i + 1;                                  //  该UCS2字符是字母或数字，则保留该字符
            }
            LimitSize = i;

            return Encoding.Unicode.GetString(bytes, 0, i);
        }

        private void button3_Click(object sender, EventArgs e)
        {


            

            #region   读取注册码信息才能运行软件！

            RegistryKey rsg = Registry.CurrentUser.OpenSubKey("zhucema"); //true表可修改                
            if (rsg != null && rsg.GetValue("mac") != null)  //如果值不为空
            {
                Thread thread = new Thread(new ThreadStart(run));
                Control.CheckForIllegalCrossThreadCalls = false;
                thread.Start();

            }

            else
            {
                MessageBox.Show("请注册软件！");
                register lg = new register();
                lg.Show();
            }

            #endregion
        }



        public void run()
        {

            if (heipath == "")
            {
                MessageBox.Show("请添加过滤词");
                return;
            }

            if (strpath == "")
            {
                MessageBox.Show("请添加原文件");
                return;
            }

            string path = AppDomain.CurrentDomain.BaseDirectory;
            //if (openFileDialog1.ShowDialog() == DialogResult.OK)
            //{
            StreamReader sr = new StreamReader(strpath, Encoding.Default);
            //一次性读取完 
            string texts = sr.ReadToEnd();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);



            string[] text1 = File.ReadAllLines(heipath, Encoding.GetEncoding("gb2312")); //黑名单


            ArrayList lists = new ArrayList();

            FileStream fs1 = new FileStream(path + "新文档.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs1);
            for (int i = 0; i < text.Length; i++)
            {

                if (System.Text.Encoding.Default.GetByteCount(text[i].ToString()) > 50)
                {

                   // lists.Add(GetSubStringByLength(text[i].ToString(), 2 * Convert.ToInt32(textBox1.Text)));
                    lists.Add(text[i].ToString());
                }
            }

            for (int i = 0; i < lists.Count; i++)
            {
                if (!lists[i].ToString().Contains(" "))
                {
                    for (int j = 0; j < text1.Length; j++)
                    {
                        if (lists[i].ToString().Contains(text1[j]) && text1[j].ToString() != "")
                        {


                            lists[i] = lists[i].ToString().Replace(text1[j], "").Trim();

                        }

                    }
                    sw.WriteLine(lists[i]);
                }
            }




            sw.Close();
            
            fs1.Close();
            MessageBox.Show("生成成功");
        }



        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                strpath = openFileDialog1.FileName;
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                heipath = openFileDialog1.FileName;
            }

            
        }

        private void 文本筛选_Load(object sender, EventArgs e)
        {

        }
    }
}



