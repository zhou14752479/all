using Spire.Doc;
using Spire.Doc.Documents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序2025
{
    public partial class 文档处理 : Form
    {
        public 文档处理()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string inputFilePath = @"C:\Users\Administrator\Desktop\input.docx";   // 源文件路径
            string outputFilePath = @"C:\Users\Administrator\Desktop\output.docx"; // 处理后文件路径
            RemoveAllHeaders(inputFilePath, outputFilePath);
        }


        static void RemoveAllHeaders(string inputPath, string outputPath)
        {
            try
            {
                // 加载文档
                Document doc = new Document();
                doc.LoadFromFile(inputPath);

                // 遍历所有节
                foreach (Section section in doc.Sections)
                {
                    // 获取所有类型的页眉（主要页眉、首页页眉、偶数页页眉）
                    foreach (HeaderFooterType type in Enum.GetValues(typeof(HeaderFooterType)))
                    {
                        if (type.ToString().Contains("Header")) // 只处理页眉类型
                        {
                            HeaderFooter header = section.HeadersFooters[type];
                            if (header != null)
                            {
                                // 清除页眉内容
                                header.Paragraphs.Clear();
                            }
                        }
                    }
                }

                // 保存修改后的文档
                doc.SaveToFile(outputPath, FileFormat.Docx);
                Console.WriteLine("页眉已成功移除，文件保存至: " + outputPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"操作失败: {ex.Message}");
            }
        }



    }
}
