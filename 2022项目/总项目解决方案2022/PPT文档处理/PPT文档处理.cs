using Spire.Presentation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PPT文档处理
{
    public partial class PPT文档处理 : Form
    {
        public PPT文档处理()
        {
            InitializeComponent();
        }


        #region PPT

        public string getppt(string file)
        {

            //实例化一个Presentation对象
            Presentation ppt = new Presentation();

            //加载PPT文档
            ppt.LoadFromFile(file, Spire.Presentation.FileFormat.Pptx2010);

            //实例化一个StringBuilder 对象
            StringBuilder sb = new StringBuilder();

            //提取PPT所有页面的文本
            for (int i = 0; i < ppt.Slides.Count; i++)
            {
                for (int j = 0; j < ppt.Slides[i].Shapes.Count; j++)
                {
                    if (ppt.Slides[i].Shapes[j] is IAutoShape)
                    {
                        IAutoShape shape = ppt.Slides[i].Shapes[j] as IAutoShape;
                        if (shape.TextFrame != null)
                        {
                            foreach (TextParagraph tp in shape.TextFrame.Paragraphs)
                            {
                                sb.Append(tp.Text + Environment.NewLine);
                            }
                        }

                    }
                }
            }

       
            return sb.ToString();

        }


        #endregion



        public void replace()
        {
            string filename = "test.pptx";
            string content = getppt(filename);
           content = content.Replace(" ", "").Replace("\r", "").Replace("\n", "").Trim();
            richTextBox1.Text = content;    

            //创建一个Dictionary 实例并添加一个item
            Dictionary<string, string> TagValues = new Dictionary<string, string>();
            //TagValues.Add("浅谈河道治理技术", "测试888");
            //加载PowerPoint示例文档
            Presentation presentation = new Presentation();
            presentation.LoadFromFile(filename, FileFormat.Pptx2007);

            char[] chars = content.ToCharArray();   
            for (int i = 0; i < 100; i=i+10)
            {
                StringBuilder sb = new StringBuilder();
                for (int j = i; j < i+10; j++)
                {
                    if(j<chars.Length)
                    {
                        sb.Append(chars[j]);
                    }
                    
                }

                string value = sb.ToString().Substring(sb.ToString().Length - 3, 3);
                if (!TagValues.ContainsKey(value))
                {
                    
                    TagValues.Add(value, value + value.Substring(value.Length - 1, 1));
                }
            }




            ReplaceTags(presentation.Slides[1], TagValues);
            //保存文档
            presentation.SaveToFile("Result.pptx", FileFormat.Pptx2010);
            System.Diagnostics.Process.Start("Result.pptx");
        }

        public void ReplaceTags(Spire.Presentation.ISlide pSlide, Dictionary<string, string> TagValues)
        {
            foreach (IShape curShape in pSlide.Shapes)
            {
                if (curShape is IAutoShape)
                {
                    foreach (TextParagraph tp in (curShape as IAutoShape).TextFrame.Paragraphs)
                    {
                        foreach (var curKey in TagValues.Keys)
                        {
                            if (tp.Text.Contains(curKey))
                            {
                               
                                tp.Text = tp.Text.Replace(curKey, TagValues[curKey]);
                            }
                        }
                    }
                   


                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // replace();

            Presentation presentation = new Presentation();
            presentation.LoadFromFile("test.pptx");

            //获取第一个Shape
            IAutoShape shape = presentation.Slides[0].Shapes[0] as IAutoShape;
            TextParagraph par = shape.TextFrame.Paragraphs[0];

            //获取段落里文本的字体
           
            MessageBox.Show(par.Text);

        }


       



    }
}
