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
    }
}
