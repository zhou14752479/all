using System;
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


namespace 思忆美团
{
    public partial class 选择分类 : Form
    {
        public 选择分类()
        {
            InitializeComponent();
        }

        functions fc = new functions();
        

        int x = 0;
        int y = 0;
        private void 选择分类_Load(object sender, EventArgs e)
        {
            panel2.Controls.Clear();
            try
            {
                StreamReader sr = new StreamReader(fc.path + "cates.json", functions.EncodingType.GetTxtType(fc.path + "cates.json"));
                //一次性读取完 
                string html = sr.ReadToEnd();

                MatchCollection cates = Regex.Matches(html, @"""id"":([\s\S]*?),""name"":""([\s\S]*?)""");

                for (int i = 0; i < cates.Count; i++)
                {
                    LinkLabel link = new LinkLabel();
                    // link.Name = "MyButton";
                    link.Text = string.Format(cates[i].Groups[2].Value.Trim());
                    link.Size = new Size(80, 30);
                    link.Location = new Point(x, y);
                    link.Font = new Font("FontName", 12);
                    link.LinkClicked += new LinkLabelLinkClickedEventHandler(link_LinkClicked);
                    panel2.Controls.Add(link);

                    x = x + 80;
                    if (x > 900)
                    {
                        y = y + 30;
                        x = 0;
                    }

                    if (!functions.catedic.ContainsKey(cates[i].Groups[2].Value.Trim()))
                    {
                                              
                       functions.catedic.Add(cates[i].Groups[2].Value.Trim(), cates[i].Groups[1].Value.Trim());
                    }
                }
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存


            }

            catch (System.Exception ex)
            {
                ex.ToString();
            }
          
        }



        private void link_LinkClicked(object sender, EventArgs e)
        {
          if(!textBox1.Text.Contains(((LinkLabel)sender).Text))
            {
                textBox1.Text += ((LinkLabel)sender).Text+",";
            }
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (Control c in panel2.Controls)//遍历groupBox1内的所有控件
            {
                if (c is LinkLabel)//只遍历CheckBox控件
                {
                   textBox1.Text += ((LinkLabel)c).Text+",";
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            functions.catename_selected = textBox1.Text.Trim();
            this.Hide();
        }
    }
}
