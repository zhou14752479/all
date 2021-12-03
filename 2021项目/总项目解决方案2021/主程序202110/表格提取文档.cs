using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using myDLL;

namespace 主程序202110
{
    public partial class 表格提取文档 : Form
    {
        public 表格提取文档()
        {
            InitializeComponent();
        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        string inipath = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
        /// <summary> 
        /// 写入INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        /// <param name="Value">值</param> 
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.inipath);
        }

        /// <summary> 
        /// 读出INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(Section, Key, "", temp, 500, this.inipath);
            return temp.ToString();
        }

        /// <summary> 
        /// 验证文件是否存在 
        /// </summary> 
        /// <returns>布尔值</returns> 
        public bool ExistINIFile()
        {
            return File.Exists(inipath);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开excel文件";
            // openFileDialog1.Filter = "excel03文件(*.xls)|*.xls|excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.Filter = "excel07文件(*.xlsx)|*.xlsx";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox2.Text = openFileDialog1.FileName;



            }
        }
        Thread thread;
        public void run()
        {
            textBox1.Text = "";
            DataTable dt = method.ExcelToDataTable(textBox2.Text, false);
          
            for (int i = 5; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][2].ToString().Trim() != "")
                {
                   
                    string a1 = dt.Rows[i][5].ToString().Trim() == "" ? "0.00" : dt.Rows[i][5].ToString().Trim();
                    string a2 = dt.Rows[i][5].ToString().Trim() == "" ? "0.00" : dt.Rows[i][6].ToString().Trim();
                    string a3 = dt.Rows[i][5].ToString().Trim() == "" ? "0.00" : dt.Rows[i][7].ToString().Trim();






                
                    if(dt.Rows[i][12].ToString().Trim()=="")
                    {
                        textBox1.Text += "不显示不提取\r\n";
                    }
                    else
                    {
                        if (shaixuan(dt.Rows[i][12].ToString().Trim()))
                        {

                            string danwei = "m³";

                            if(dt.Rows[i][4].ToString().Trim()!="")
                            {
                                danwei = dt.Rows[i][4].ToString().Trim();
                            }
                            textBox1.Text += dt.Rows[i][2].ToString().Trim();
                            textBox1.Text += "：送审" + dt.Rows[3][5].ToString().Trim() + "为" + a1 + danwei+"，";
                            textBox1.Text += "送审" + dt.Rows[3][6].ToString().Trim() + "为" + a2 + "元/"+danwei+"，";
                            textBox1.Text += "送审" + dt.Rows[3][7].ToString().Trim() + "为" + a3 + "元；";

                            textBox1.Text += "审定" + dt.Rows[3][8].ToString().Trim() + "为" + dt.Rows[i][8].ToString().Trim() + danwei+"，";
                            textBox1.Text += "审定" + dt.Rows[3][10].ToString().Trim() + "为" + dt.Rows[i][10].ToString().Trim() + "元/"+danwei+"，";  //第9列为空
                            textBox1.Text += "审定" + dt.Rows[3][11].ToString().Trim() + "为" + dt.Rows[i][11].ToString().Trim() + "元；";

                            //textBox1.Text += "共" + dt.Rows[2][12].ToString().Trim() + "为" + dt.Rows[i][12].ToString().Trim() + "元；";
                            //textBox1.Text += "共" + dt.Rows[2][13].ToString().Trim() + "为" + dt.Rows[i][13].ToString().Trim() + "元\r\n";


                            string hejian = "核减";
                            if(!dt.Rows[i][12].ToString().Trim().Contains("-"))
                            {
                                hejian = "核增";
                            }
                            textBox1.Text += "共"+hejian+"综合合价" + dt.Rows[i][12].ToString().Trim().Replace("-","") + "元，";
                            textBox1.Text += hejian+"原因" + dt.Rows[i][13].ToString().Trim() + "；\r\n";
                        }
                    }
                   
                }
            }

        }

        public bool  shaixuan(string jine)
        {
            try
            {

                if (textBox3.Text == "")
                {
                    return true;
                }
                else
                {
                    if (Convert.ToDouble(jine) >= Convert.ToDouble(textBox3.Text) || (Convert.ToDouble(jine) * -1) >= Convert.ToDouble(textBox3.Text))
                    {
                        return true;
                    }


                }
                return false;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            jiance();
            if (textBox2.Text=="")
            {
                MessageBox.Show("请选择对比表");
                return;
            }

            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

        }

        public void ExportWord(string str)
        {
            System.Windows.Forms.SaveFileDialog objSave = new System.Windows.Forms.SaveFileDialog();
            objSave.Filter = "(*.doc)|*.doc|" + "(*.*)|*.*";//+ "(*.txt)|*.txt|"

            objSave.FileName = DateTime.Now.ToString("yyyyMMddHHmm") + ".doc";

            if (objSave.ShowDialog() == DialogResult.OK)
            {

                Microsoft.Office.Interop.Word.ApplicationClass MyWord = new Microsoft.Office.Interop.Word.ApplicationClass();
                Microsoft.Office.Interop.Word.Document MyDoc;

                Object Nothing = System.Reflection.Missing.Value;

                MyDoc = MyWord.Documents.Add(ref Nothing, ref Nothing, ref Nothing, ref Nothing);

                MyDoc.Paragraphs.Last.Range.Font.Name = "宋体";

                MyDoc.Paragraphs.Last.Range.Text = str;

                object MyFileName = objSave.FileName;
                //将WordDoc文档对象的内容保存为DOC文档 
                MyDoc.SaveAs(ref MyFileName, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);
                //关闭WordDoc文档对象 
                MyDoc.Close(ref Nothing, ref Nothing, ref Nothing);
                //关闭WordApp组件对象 
                MyWord.Quit(ref Nothing, ref Nothing, ref Nothing);
                MessageBox.Show("文件保存成功", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //this.txtDetail.SaveFile(objSave.FileName,RichTextBoxStreamType.PlainText);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            //ExportWord(textBox1.Text);
            System.Windows.Forms.SaveFileDialog objSave = new System.Windows.Forms.SaveFileDialog();
            objSave.Filter = "(*.doc)|*.doc|" + "(*.*)|*.*";//+ "(*.txt)|*.txt|"

            objSave.FileName = DateTime.Now.ToString("yyyyMMddHHmm") + ".doc";

            if (objSave.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(objSave.FileName, textBox1.Text, Encoding.UTF8);
            }

        }

        private void 表格提取文档_Load(object sender, EventArgs e)
        {
            #region 通用检测

            string html = method.GetUrl("http://www.acaiji.com/index/index/vip.html", "utf-8");

            if (!html.Contains(@"zp4YY"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            #endregion





        }


        #region 机器码
        public void jiance()
        {
            if (ExistINIFile())
            {
                string key = IniReadValue("values", "key");
                string secret = IniReadValue("values", "secret");
                string[] value = secret.Split(new string[] { "asd147" }, StringSplitOptions.None);


                if (Convert.ToInt32(value[1]) < Convert.ToInt32(method.GetTimeStamp()))
                {
                    MessageBox.Show("激活已过期");
                    string str = Interaction.InputBox("您的机器码如下，请复制机器码提供到后台，输入激活码然后激活！", "激活软件", method.GetMD5(method.GetMacAddress()), -1, -1);
                    string[] text = str.Split(new string[] { "asd" }, StringSplitOptions.None);

                    if (text[0] == method.GetMD5(method.GetMD5(method.GetMacAddress()) + "siyiruanjian"))
                    {
                        IniWriteValue("values", "key", method.GetMD5(method.GetMacAddress()));
                        IniWriteValue("values", "secret", str);
                        MessageBox.Show("激活成功");


                    }
                    else
                    {
                        MessageBox.Show("激活码错误");
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                        return;
                    }

                }


                else if (value[0] != method.GetMD5(method.GetMD5(method.GetMacAddress()) + "siyiruanjian") || key != method.GetMD5(method.GetMacAddress()))
                {

                    string str = Interaction.InputBox("您的机器码如下，请复制机器码提供到后台，输入激活码然后激活！", "激活软件", method.GetMD5(method.GetMacAddress()), -1, -1);
                    string[] text = str.Split(new string[] { "asd147" }, StringSplitOptions.None);

                    if (text[0] == method.GetMD5(method.GetMD5(method.GetMacAddress()) + "siyiruanjian"))
                    {
                        IniWriteValue("values", "key", method.GetMD5(method.GetMacAddress()));
                        IniWriteValue("values", "secret", str);
                        MessageBox.Show("激活成功");


                    }
                    else
                    {
                        MessageBox.Show("激活码错误");
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                        return;
                    }
                }


            }
            else
            {

                string str = Interaction.InputBox("您的机器码如下，请复制机器码提供到后台，输入激活码然后激活！", "激活软件", method.GetMD5(method.GetMacAddress()), -1, -1);
                string[] text = str.Split(new string[] { "asd147" }, StringSplitOptions.None);
                if (text[0] == method.GetMD5(method.GetMD5(method.GetMacAddress()) + "siyiruanjian"))
                {
                    IniWriteValue("values", "key", method.GetMD5(method.GetMacAddress()));
                    IniWriteValue("values", "secret", str);
                    MessageBox.Show("激活成功");


                }
                else
                {
                    MessageBox.Show("激活码错误");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    return;
                }
            }

        }

        #endregion




    }
}
