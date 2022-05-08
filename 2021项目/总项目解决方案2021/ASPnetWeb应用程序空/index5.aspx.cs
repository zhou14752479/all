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
    public partial class index5 : System.Web.UI.Page
    {
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
        /// 指定位置添加图片
        /// </summary>
        /// <param name="changci"></param>
        /// <param name="value"></param>
        public void run(string changci, string value)
        {


            try
            {
                string month = DateTime.Now.AddHours(-1).Month.ToString();
                string day = DateTime.Now.AddHours(-1).Day.ToString();

                if(DateTime.Now.Hour==0 || DateTime.Now.Hour == 24)
                {
                    day = DateTime.Now.AddDays(-1).Day.ToString();
                }

                if (month.Length == 1)
                {
                    month = "0" + month;
                }
                if (day.Length == 1)
                {
                    day = "0" + day;
                }

                char[] monthchar = month.ToCharArray();



                char[] daychar = day.ToCharArray();


                System.Drawing.Image image_changci = ReadImageFile(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/场次/" + changci + ".png");



                System.Drawing.Image image_month = ReadImageFile(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/date/" + monthchar[0] + ".png");
                System.Drawing.Image image_month2 = ReadImageFile(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/date/" + monthchar[1] + ".png");
                System.Drawing.Image image_yue = ReadImageFile(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/date/月.png");
                System.Drawing.Image image_ri = ReadImageFile(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/date/日.png");
                System.Drawing.Image image_day = ReadImageFile(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/date/" + daychar[0] + ".png");
                System.Drawing.Image image_day2 = ReadImageFile(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/date/" + daychar[1] + ".png");







                Bitmap bitmap = new Bitmap(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/main5/" + value + ".jpg");
                Graphics device = Graphics.FromImage(bitmap);
                //如果picturebox1本身有内容，就先画到image上
                device.DrawImage(image_changci, 383, 83); //用你想要的位置画小图


                if (value != "宝石")
                {
                    device.DrawImage(image_month, 145, 75);
                    device.DrawImage(image_month2, 160, 75);

                    device.DrawImage(image_day, 205, 75);
                    device.DrawImage(image_day2, 220, 75);



                    device.DrawImage(image_yue, 125, 46);
                    device.DrawImage(image_ri, 195, 46);
                }







                //宝石图片单独做
                else
                {
                    device.DrawImage(image_month, 135, 75);
                    device.DrawImage(image_month2, 150, 75);

                    device.DrawImage(image_day, 195, 75);
                    device.DrawImage(image_day2, 210, 75);



                    device.DrawImage(image_yue, 95, 36);
                    device.DrawImage(image_ri, 165, 36);
                }

                bitmap.Save(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/image5/" + changci + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);


                image_month.Dispose();
                image_month2.Dispose();
                image_yue.Dispose();
                image_ri.Dispose();
                image_day.Dispose();
                image_day2.Dispose();
                image_changci.Dispose();
                bitmap.Dispose();
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());

            }

        }




        /// <summary>
        /// 上下合成图片
        /// </summary>
        static private void CombinImage2()
        {
            System.Drawing.Image img1 = System.Drawing.Image.FromFile(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/image5/1.jpg");
            Bitmap map1 = new Bitmap(img1);
            System.Drawing.Image img2 = System.Drawing.Image.FromFile(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/image5/2.jpg");
            Bitmap map2 = new Bitmap(img2);

            var width = Math.Max(img1.Width, img2.Width);
            var height = img1.Height + img2.Height + 10;
            // 初始化画布(最终的拼图画布)并设置宽高
            Bitmap bitMap = new Bitmap(width, height);
            // 初始化画板
            Graphics g1 = Graphics.FromImage(bitMap);
            // 将画布涂为白色(底部颜色可自行设置)
            g1.FillRectangle(Brushes.White, new Rectangle(0, 0, width, height));
            //在x=0，y=0处画上图一
            g1.DrawImage(map1, 0, 0, img1.Width, img1.Height);
            //在x=0，y在图一往下10像素处画上图二
            g1.DrawImage(map2, 0, img1.Height, img2.Width, img2.Height);

            map1.Dispose();
            map2.Dispose();
            img1.Dispose();
            img2.Dispose();

            System.Drawing.Image img = bitMap;
            //保存
            img.Save(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/image5/result.jpg"));
            img.Dispose();


        }


        /// <summary>
        /// 上下合成图片
        /// </summary>
        static private void CombinImage3()
        {
            System.Drawing.Image img1 = System.Drawing.Image.FromFile(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/image5/1.jpg");
            Bitmap map1 = new Bitmap(img1);
            System.Drawing.Image img2 = System.Drawing.Image.FromFile(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/image5/2.jpg");
            Bitmap map2 = new Bitmap(img2);
            System.Drawing.Image img3 = System.Drawing.Image.FromFile(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/image5/3.jpg");
            Bitmap map3 = new Bitmap(img3);
            var width = Math.Max(img1.Width, img2.Width);
            var height = img1.Height + img2.Height + img2.Height + 10;
            // 初始化画布(最终的拼图画布)并设置宽高
            Bitmap bitMap = new Bitmap(width, height);
            // 初始化画板
            Graphics g1 = Graphics.FromImage(bitMap);
            // 将画布涂为白色(底部颜色可自行设置)
            g1.FillRectangle(Brushes.White, new Rectangle(0, 0, width, height));
            //在x=0，y=0处画上图一
            g1.DrawImage(map1, 0, 0, img1.Width, img1.Height);
            //在x=0，y在图一往下10像素处画上图二
            g1.DrawImage(map2, 0, img1.Height, img2.Width, img2.Height);
            g1.DrawImage(map3, 0, img1.Height + img2.Height, img3.Width, img3.Height);
            map1.Dispose();
            map2.Dispose();
            map3.Dispose();
            img1.Dispose();
            img2.Dispose();
            img3.Dispose();


            System.Drawing.Image img = bitMap;
            //保存
            img.Save(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/image5/result.jpg"));
            img.Dispose();


        }


       
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                StreamReader sr = new StreamReader(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/index5.txt", Encoding.GetEncoding("UTF-8"));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                Application["diyichang"] = text[0];
                Application["dierchang"] = text[1];
                Application["disanchang"] = text[2];
                Application["shengxiao"] = text[3];
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
            }
            catch (Exception ex)
            {

                ex.ToString();
            }








            // Session.Timeout = 99999;
            if (Request["action"] == "reset")
            {
                if (File.Exists(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/image5/result.jpg"))
                {
                    File.Delete(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/image5/result.jpg");
                }
            }
            if (HttpContext.Current.Request.RequestType == "POST")
            {


                string diyichang = Request["diyichang"];
                string dierchang = Request["dierchang"];
                string disanchang = Request["disanchang"];
                string shengxiao = Request["shengxiao"];


                //Session["diyichang"] = diyichang;
                //Session["dierchang"] = dierchang;
                //Session["disanchang"] = disanchang;
                //Session["shengxiao"] = shengxiao;

                FileStream fs1 = new FileStream(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/index5.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                sw.WriteLine(diyichang + "\r\n" + dierchang + "\r\n" + disanchang + "\r\n" + shengxiao);
                sw.Close();
                fs1.Close();
                sw.Dispose();


                Application["diyichang"] = diyichang;
                Application["dierchang"] = dierchang;
                Application["disanchang"] = disanchang;
                Application["shengxiao"] = shengxiao;

                if (diyichang != "")
                {

                    run("1", diyichang);
                    if (File.Exists(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/image5/result.jpg"))
                    {
                        File.Delete(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/image5/result.jpg");
                    }
                    File.Copy(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/image5/1.jpg", AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/image5/result.jpg");
                }
                
                if (dierchang != "")
                {

                    run("2", dierchang);
                    CombinImage2();
                }
                if (disanchang != "")
                {

                    run("3", disanchang);
                    CombinImage3();
                }


            }

        }


    }
}