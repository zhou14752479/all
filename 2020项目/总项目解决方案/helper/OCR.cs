using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace helper
{
   public  class OCR
    {
        public string Shibie(string textUser,string textPass,Image image)

        {
           
                MemoryStream ms = new MemoryStream();
                Bitmap bmp = new Bitmap(image);
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] photo_byte = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(photo_byte, 0, Convert.ToInt32(ms.Length));
                bmp.Dispose();
                string returnMess = Dc.RecByte_A(photo_byte, photo_byte.Length, textUser, textPass, "0");//第5个参数为软件id,缺省为0
                if (returnMess.Equals("Error:No Money!"))
                {
                    MessageBox.Show("点数不足", "友情提示");
                return "";
            }
                else if (returnMess.Equals("Error:No Reg!"))
                {
                    MessageBox.Show("账户密码错误", "友情提示");
                return "";
            }
                else if (returnMess.Equals("Error:Put Fail!"))
                {
                    MessageBox.Show("上传失败，图片过大或图片不完整", "友情提示");
                return "";
            }
                else if (returnMess.Equals("Error:TimeOut!"))
                {
                    MessageBox.Show("识别超时", "友情提示");
                return "";
            }
                else if (returnMess.Equals("Error:empty picture!"))
                {
                    MessageBox.Show("上传无效验证码", "友情提示");
                return "";
            }
                else if (returnMess.Equals("Error:Account or Software Bind!"))
                {
                    MessageBox.Show("账户或IP被冻结", "友情提示");
                return "";
            }
                else if (returnMess.Equals("Error:Software Frozen!"))
                {
                    MessageBox.Show("软件被冻结", "友情提示");
                return "";
            }
                else if (returnMess.Equals("Error:Account or IP Frozen!"))
                {
                    MessageBox.Show("当前IP不在绑定IP列表中或IP被冻结", "友情提示");
                return "";
            }
                else if (returnMess.Equals("Error:Parameter!"))
                {
                    MessageBox.Show("参数错误,请检查各参数", "友情提示");
                return "";
            }
                else if (returnMess.IndexOf("|") > -1)
                {
                    return returnMess.Split('|')[0];
                    //textBox6.Text = returnMess.Split('|')[2];
                    //MessageBox.Show("识别成功", "友情提示");
                }
                else
                {
                    MessageBox.Show(returnMess, "其他错误");
                     return "";
                }

            
        }


    }
}
