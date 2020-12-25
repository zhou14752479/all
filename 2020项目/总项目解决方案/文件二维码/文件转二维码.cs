using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;


namespace 文件二维码
{
    public partial class 文件转二维码 : Form
    {
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


        public 文件转二维码()
        {
            InitializeComponent();
        }
        #region 获取Mac地址
        /// <summary>
        /// 获取Mac地址
        /// </summary>
        /// <returns></returns>
        public static string GetMacAddress()
        {
            try
            {
                string strMac = string.Empty;
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        strMac = mo["MacAddress"].ToString();
                    }
                }
                moc = null;
                mc = null;
                return strMac;
            }
            catch
            {
                return "unknown";
            }
        }

        #endregion
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <returns></returns>

        public Bitmap creatQcode(string path)
        {
            try
            {
                int width = 232; //图片宽度
                int height = 232;//图片长度
                BarcodeWriter barCodeWriter = new BarcodeWriter();
                barCodeWriter.Format = BarcodeFormat.QR_CODE; // 生成码的方式(这里设置的是二维码),有条形码\二维码\还有中间嵌入图片的二维码等
                barCodeWriter.Options.Hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");// 支持中文字符串
                barCodeWriter.Options.Hints.Add(EncodeHintType.ERROR_CORRECTION, ZXing.QrCode.Internal.ErrorCorrectionLevel.H);
                barCodeWriter.Options.Height = height;
                barCodeWriter.Options.Width = width;
                barCodeWriter.Options.Margin = 0; //设置的白边大小
                ZXing.Common.BitMatrix bm = barCodeWriter.Encode(path);  //DNS为要生成的二维码字符串
                Bitmap result = barCodeWriter.Write(bm);
                Bitmap Qcbmp = result.Clone(new Rectangle(Point.Empty, result.Size), PixelFormat.Format1bppIndexed);//位深度
                                                                                                                    // SaveImg(currentPath, Qcbmp); //图片存储自己写的函数
                                                                                                                    //Qcbmp=WhiteUp(Qcbmp,10);
                return Qcbmp;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                return null;
                    
              }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in ofd.FileNames)
                {
                    ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                    lv1.SubItems.Add(file);
                    lv1.SubItems.Add(" ");
                }
            }
        }
        
        


        string localpath = AppDomain.CurrentDomain.BaseDirectory+"images//";
        public void save(string name,Bitmap bitmap)
        {
            try
            {
                string path = localpath + name + ".jpg";
                bitmap.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }


        public void zhuanhuan()
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                string apath = listView1.Items[i].SubItems[1].Text;
                string filename = Path.GetFileNameWithoutExtension(apath);
                save(filename, creatQcode(apath));
                listView1.Items[i].SubItems[2].Text = "成功";
            }

        
        
        
        }

        Thread thread;
        private void button2_Click(object sender, EventArgs e)
        {
            if (IniReadValue("mac地址", "mac") != "1C:1B:0D:2E:AA:8E")
            {
                MessageBox.Show("此设备不允许访问");
                return;
            }



            if (thread == null || !thread.IsAlive)
            {
                IniWriteValue("设备记录", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), GetMacAddress());  //初始化读取并记录mac
                thread = new Thread(zhuanhuan);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }

        }

        private void 文件转二维码_Load(object sender, EventArgs e)
        {
            
        }
    }
}
