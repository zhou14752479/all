using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 基鹿工具箱
{
    public partial class 生意参谋指数导入 : Form
    {
        public 生意参谋指数导入()
        {
            InitializeComponent();
        }

        private void 生意参谋指数导入_Load(object sender, EventArgs e)
        {
           
                StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\gongshi.a", Util.EncodingType.GetTxtType(AppDomain.CurrentDomain.BaseDirectory + "\\gongshi.a"));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
               if(text.Length>1)
                {
                    textBox1.Text = text[0].Trim();
                    textBox2.Text = text[1].Trim();
                }
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
            

        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            string jyzs = textBox1.Text.Replace(" ", "").Replace("（", "(").Replace("）", ")");
            string zfzs = textBox2.Text.Replace(" ", "").Replace("（", "(").Replace("）", ")");

            FileStream fs1 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\gongshi.a", FileMode.Create, FileAccess.Write);//创建写入文件 
            StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
            sw.WriteLine( jyzs+ "\r\n"+zfzs);
            sw.Close();
            fs1.Close();
            sw.Dispose();

            string sql = "update gs set jyzs= '" + jyzs + " ',zfzs='" + zfzs + " '";
            Util.SQL(sql);
            MessageBox.Show("导入成功");

        }

        
    }
}
