using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASPnetWeb应用程序空
{
    public partial class index3_admin : System.Web.UI.Page
    {
        //string c1 = Request["c1"];  //获取form表单 name
        //string c2 = Request.QueryString["c2"];  //获取浏览器url参数


        /// <summary>
        /// 读取本地图片
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Bitmap ReadImageFile(string path)
        {
            if (!File.Exists(path))
            {

                return null;//文件不存在
            }
            FileStream fs = File.OpenRead(path); //OpenRead
            int filelength = 0;
            filelength = (int)fs.Length; //获得文件长度 
            Byte[] image = new Byte[filelength]; //建立一个字节数组 
            fs.Read(image, 0, filelength); //按字节流读取 
            System.Drawing.Image result = System.Drawing.Image.FromStream(fs);
            fs.Close();
            fs.Dispose();
            Bitmap bit = new Bitmap(result);
            return bit;
        }
        /// <summary>
        /// 在现有画布上添加文字
        /// </summary>
        /// <param name="g">画布对象</param>
        /// <param name="txt">需要添加的文字</param>
        /// <param name="fontName">字体名称</param>
        /// <param name="fontSize">字体大小</param>
        /// <param name="fontStyle">字体风格，是否加粗等</param>
        /// <param name="x">绘画位置X</param>
        /// <param name="y">绘画位置Y</param>
        /// <param name="red">文字颜色R</param>
        /// <param name="green">文字颜色G</param>
        /// <param name="blue">文字颜色B</param>
        public void drawText(string txt, string fontName, int fontSize, FontStyle fontStyle, float x, float y, int red, int green, int blue, string oldfile, string newfile)
        {
            try
            {

                System.Drawing.Image image_sc = ReadImageFile(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/result3/" + oldfile + ".jpg");
                Bitmap nbmp = new Bitmap(image_sc);
                Graphics g = Graphics.FromImage(nbmp);
                //文字绘制
                Font font = new Font(fontName, fontSize, fontStyle, GraphicsUnit.Pixel);
                Brush brush = new SolidBrush(Color.FromArgb(red, green, blue));
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                g.DrawString(txt, font, brush, x, y, StringFormat.GenericDefault);

                nbmp.Save(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/result3/" + newfile + ".jpg");
            }
            catch (Exception ex)
            {

                Response.Write(ex.ToString());
            }


        }

       
        public static string value1 = "";
        public static string value2 = "";
        public static string value3 = "";
        public static string sxsh_value = "";


    
          public void shengcheng(string chang,string c)
        {
           
          string c1 = Request["c1"];
            string c2 = Request["c2"];
            string c3 = Request["c3"];
            string c4 = Request["c4"];
            string c5 = Request["c5"];
            string c6 = Request["c6"];
            string c7 = Request["c7"];
            string c8 = Request["c8"];
            string c9 = Request["c9"];
            string c10 = Request["c10"];
            string c11 = Request["c11"];
            string c12 = Request["c12"];

            //Session["c1"] = c1;
            //Session["c2"] = c2;
            //Session["c3"] = c3;
            //Session["c4"] = c4;
            //Session["c5"] = c5;
            //Session["c6"] = c6;
            //Session["c7"] = c7;
            //Session["c8"] = c8;
            //Session["c9"] = c9;
            //Session["c10"] = c10;
            //Session["c11"] = c11;
            //Session["c12"] = c12;
            if (chang== "一")
            {
                FileStream fs1 = new FileStream(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/index3_1.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                sw.WriteLine(c1 + "\r\n" + c2 + "\r\n" + c3 + "\r\n" + c4 + "\r\n" + c);
                sw.Close();
                fs1.Close();
                sw.Dispose();
            }

            if (chang == "二")
            {
                FileStream fs1 = new FileStream(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/index3_2.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                sw.WriteLine(c5 + "\r\n" + c6 + "\r\n" + c7 + "\r\n" + c8 + "\r\n" + c);
                sw.Close();
                fs1.Close();
                sw.Dispose();
            }

            if (chang == "三")
            {
                FileStream fs1 = new FileStream(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/index3_3.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                sw.WriteLine(c9 + "\r\n" + c10 + "\r\n" + c11 + "\r\n" + c12 + "\r\n" + c);
                sw.Close();
                fs1.Close();
                sw.Dispose();
            }

           



            Application["c1"] = c1;
            Application["c2"] = c2;
            Application["c3"] = c3;
            Application["c4"] = c4;
            Application["c5"] = c5;
            Application["c6"] = c6;
            Application["c7"] = c7;
            Application["c8"] = c8;
            Application["c9"] = c9;
            Application["c10"] = c10;
            Application["c11"] = c11;
            Application["c12"] = c12;


            if (chang == "一")
            {
                string value = "    " + c1 + "\n" + "    " + c2 + "\n" + "       宝钢:" + c3 + "\n\n" + "提示:" + c4 + "\n\n" + "     第一场:【"+c+"】";
                value1 = value;
                drawText(value, "宋体", 19, FontStyle.Bold, 40, 100, 0, 0, 0, "场", "第一场");
            }
            if (chang == "二")
            {
                string value = "    " + c5 + "\n" + "    " + c6 + "\n" + "       宝钢:" + c7+ "\n\n" + "提示:" + c8+ "\n\n" + "     第二场:【" + c + "】";
                value2 = value;
                drawText(value, "宋体", 19, FontStyle.Bold, 40, 100, 0, 0, 0, "场", "第二场");
               
            }
            if (chang == "三")
            {
                string value = "    " + c9+ "\n" + "    " + c10+ "\n" + "       宝钢:" + c11 + "\n\n" + "提示:" + c12 + "\n\n" + "     第三场:【" + c + "】";
                value3 = value;
                drawText(value, "宋体", 19, FontStyle.Bold, 40, 100, 0, 0, 0, "场", "第三场");
            }


          

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                StreamReader sr = new StreamReader(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/index3_1.txt", Encoding.GetEncoding("UTF-8"));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                Application["c1"] = text[0];
                Application["c2"] = text[1];
                Application["c3"] = text[2];
                Application["c4"] = text[3];
                Application["diyichang1"] = text[4];
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
            }
            catch (Exception ex)
            {

                Response.Write(ex.ToString());
            }

            try
            {
                StreamReader sr = new StreamReader(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/index3_2.txt", Encoding.GetEncoding("UTF-8"));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                Application["c5"] = text[0];
                Application["c6"] = text[1];
                Application["c7"] = text[2];
                Application["c8"] = text[3];
                Application["dierchang1"] = text[4];
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
            }
            catch (Exception ex)
            {

                ex.ToString();
            }


            try
            {
                StreamReader sr = new StreamReader(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/index3_3.txt", Encoding.GetEncoding("UTF-8"));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                Application["c9"] = text[0];
                Application["c10"] = text[1];
                Application["c11"] = text[2];
                Application["c12"] = text[3];
                Application["disanchang1"] = text[4];
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
            }
            catch (Exception ex)
            {

                ex.ToString();
            }

            try
            {
                StreamReader sr = new StreamReader(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/index3_sxsh.txt", Encoding.GetEncoding("UTF-8"));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                Application["c13"] = text[0];
                Application["c14"] = text[1];
                Application["c15"] = text[2];
                Application["c16"] = text[3];
                Application["shengxiao1"] = text[4];
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
            }
            catch (Exception ex)
            {

                ex.ToString();
            }

          

            //Session.Timeout = 99999;

            //生成前端结果
            if (HttpContext.Current.Request.RequestType == "POST")
            {
                if (Request.QueryString["diyichang"] != "")
                {
                    string c1 = Request["diyichang"];
                    Application["diyichang1"] = c1;
                    shengcheng("一",c1);
                  
                }

                if (Request.QueryString["dierchang"] != "")
                {
                    string c2 = Request["dierchang"];
                    Application["dierchang1"] = c2;
                    shengcheng("二", c2);

                }

                if (Request.QueryString["disanchang"] != "")
                {
                    string c3 = Request["disanchang"];
                    Application["disanchang1"] = c3;
                    shengcheng("三", c3);

                }
                if (Request.QueryString["shengxiao"] != "")
                {
                    string sxsh = Request["shengxiao"];
                    Application["shengxiao1"] = sxsh;

                    string c13 = Request["c13"];
                    string c14 = Request["c14"];
                    string c15 = Request["c15"];
                    string c16 = Request["c16"];

                    //Session["c13"] = c13;
                    //Session["c14"] = c14;
                    //Session["c15"] = c15;
                    //Session["c16"] = c16;

                    Application["c13"] = c13;
                    Application["c14"] = c14;
                    Application["c15"] = c15;
                    Application["c16"] = c16;
                    FileStream fs1 = new FileStream(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/index3_sxsh.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                    sw.WriteLine(c13 + "\r\n" + c14 + "\r\n" + c15 + "\r\n" + c16 + "\r\n" + sxsh);
                    sw.Close();
                    fs1.Close();
                    sw.Dispose();
                    string value = "\n    " + c13 + "\n    " + c14 + "\n      宝钢:" + c15 + "\n提示:" + c16 + "\n" + "\n    生肖守护:【"+sxsh+"】";
                    sxsh_value = value;
                    drawText(value, "宋体", 80, FontStyle.Bold, 40, 100, 0, 0, 0, "生肖", "生肖守护");

                }

            }


        }

    
    }
}