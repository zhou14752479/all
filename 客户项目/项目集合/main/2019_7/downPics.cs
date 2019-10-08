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

namespace main._2019_7
{
    public partial class downPics : Form
    {
        public downPics()
        {
            InitializeComponent();
        }

        private void DownPics_Load(object sender, EventArgs e)
        {

        }
        public void taobao()
        {
            string html = method.GetUrl(textBox1.Text, "utf-8");
            MatchCollection zhupics = Regex.Matches(html, @"<a href=""#""><img data-src=""([\s\S]*?)jpg");  //主图
            MatchCollection skupics = Regex.Matches(html, @"<a href=""javascript:;"" style=""background:url\(([\s\S]*?)\)");  //SKU图
            MatchCollection pics = Regex.Matches(html, @"descnew([\s\S]*?)'");  //详情图来源网址
            MatchCollection vedios = Regex.Matches(html, @"<a href=""#""><img src=""([\s\S]*?)jpg");  //视频

        }

        public void tianmao()
        {
            string html = method.GetUrl(textBox1.Text,"utf-8");
            MatchCollection zhupics = Regex.Matches(html, @"<a href=""#""><img src=""([\s\S]*?)jpg");  //主图
            MatchCollection skupics = Regex.Matches(html, @"<a href=""#"" style=""background:url\(([\s\S]*?)\)");  //SKU图
            MatchCollection pics = Regex.Matches(html, @"httpsDescUrl"":""([\s\S]*?)""");  //详情图来源网址
            MatchCollection vedios = Regex.Matches(html, @"imgVedioUrl"":""([\s\S]*?)""");  //视频

        }

        public void jd()
        {
            string html = method.GetUrl(textBox1.Text, "utf-8");
            MatchCollection zhupics = Regex.Matches(html, @"imageList: \[([\s\S]*?)\]");  //主图
            MatchCollection skupics = Regex.Matches(html, @"<img data-img=""1"" src=""([\s\S]*?)""");  //SKU图
            MatchCollection pics = Regex.Matches(html, @"background-image:url\(([\s\S]*?)\)");  //详情图
            MatchCollection vedios = Regex.Matches(html, @"imgVedioUrl"":""([\s\S]*?)""");  //视频

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择所在文件夹";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    MessageBox.Show(this, "文件夹路径不能为空", "提示");
                    return;
                }

                textBox2.Text = dialog.SelectedPath;
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            method.downloadFile(textBox1.Text,textBox2.Text,"1.jpg");
        }

        private void TextBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
