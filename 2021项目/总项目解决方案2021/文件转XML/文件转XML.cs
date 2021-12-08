using Spire.Xls;
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
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace 文件转XML
{

    /// <summary>
    // 每个文件格式都可以看作是数据流，
    //流格式在C#里通过file类打开然后通过\r分割为相应字符。
    //然后重组为xml格式
   
    　/// </summary>
    partial class 文件转XML : System.Windows.Forms.Form
    {
        public 文件转XML()
        {
            InitializeComponent();
        }

        private void 文件转XML_Load(object sender, EventArgs e)
        {
            //当前code时间校验已去除
        }


        public string txtContent;
        //打开txt文件
        private void toolStripOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                // fileDialog.Filter = "文本文件(*.txt)|*.txt";
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    String fileName = fileDialog.FileName;
                    textBox1.Text = fileName;
                    if (!String.IsNullOrEmpty(fileName))
                    {
                        using (StreamReader st = new StreamReader(fileName, System.Text.Encoding.GetEncoding("utf-8")))
                        {
                            txtContent = st.ReadToEnd();
                            //读取txt文件到txtTXT文本框

                            st.Close();
                        }
                    }
                }
            }
        }

        public string txtXML;
        //将txt文件内容转换成xml格式内容
        private void toolStripConvert()
        {
            try
            {
                //将txt内容分解为行数组
                String[] lines = txtContent.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                String[] heads = null;
                if (lines != null && lines.Length > 0)
                {
                    //读取第一行数据，该行数据为xml文件的节点描述数据
                    heads = lines[0].Split(new string[] { "/t" }, StringSplitOptions.None);
                    //MessageBox.Show(heads.Length.ToString() + " " + heads[0]);
                }
                //
                StringBuilder sb = new StringBuilder();
                sb.Append("<?xml version=\"1.0\" encoding=\"gbk\"?>").Append(Environment.NewLine).Append("<dataRoot>").Append(Environment.NewLine);
                //生成xml节点
                for (int i = 1; i < lines.Length; i++)
                {
                    if (lines[i] == null || lines[i].Trim().Length < 1)
                        continue;
                    String[] info = lines[i].Split(new string[] { "/t" }, StringSplitOptions.None);
                    sb.Append(createNode(heads, info));
                }
                sb.Append("</dataRoot>");
                this.txtXML = sb.ToString();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        //产生xml节点
        private String createNode(String[] head, String[] info)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<ROW>").Append(Environment.NewLine);
            for (int i = 0; i < head.Length; i++)
            {
                sb.Append("<CELL>" + info[i] + "</CELL>").Append(Environment.NewLine);
            }
            sb.Append("</ROW>").Append(Environment.NewLine);
            return sb.ToString();
        }

        //将txtXML文本框内容另存为xml文件
        private void toolStripSaveas()
        {
            try
            {
                String fileName = "";
                using (SaveFileDialog fileDialog = new SaveFileDialog())
                {
                    fileDialog.Filter = "XML数据文件(*.xml)|*.xml";
                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        fileName = fileDialog.FileName;
                        if (!String.IsNullOrEmpty(fileName))
                        {
                            FileStream fs = new FileStream(fileName, FileMode.Create);
                            //获得字节数组
                            byte[] data = System.Text.Encoding.GetEncoding("GBK").GetBytes(this.txtXML);
                            //开始写入
                            fs.Write(data, 0, data.Length);
                            //清空缓冲区、关闭流
                            fs.Flush();
                            fs.Close();
                        }
                    }
                }
                MessageBox.Show(String.Format("文件成功保存到{0}", fileName));
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }







      



        //private void button1_Click(object sender, System.EventArgs e)
        //        {
        //            saveFileDialog1.Filter = "xml 文件|*.xml";//设置打开对话框的文件过滤条件
        //            saveFileDialog1.Title = "保存成 xml 文件";//设置打开对话框的标题
        //            saveFileDialog1.FileName = "";
        //            saveFileDialog1.ShowDialog();//打开对话框
        //            if (saveFileDialog1.FileName != "")//检测用户是否输入了保存文件名
        //            {
        //            Workbook workbook = new Workbook();
        //            workbook.LoadFromFile(strFileName);
        //            workbook.SaveAsXml(saveFileDialog1.FileName);

        //                MessageBox.Show("保存成功");
        //            }
        //        }




        //string strFileName;

        private void button4_Click(object sender, EventArgs e)
        {
            if(textBox1.Text=="")
            {
                MessageBox.Show("请先选择文件");
                return;
            }
            toolStripConvert();
            toolStripSaveas();
        }

        //private void button3_Click(object sender, System.EventArgs e)
        //    {

        //      //  openFileDialog1.Filter = "Office Documents(*.doc, *.xls, *.ppt)|*.doc;*.xls;*.ppt";
        //        openFileDialog1.FilterIndex = 1;
        //        openFileDialog1.FileName = "";
        //        openFileDialog1.ShowDialog();
        //        strFileName = openFileDialog1.FileName;
        //        if (strFileName.Length != 0)
        //        {

        //            MessageBox.Show("加载成功");
        //        }
        //    }


        
    }
}

