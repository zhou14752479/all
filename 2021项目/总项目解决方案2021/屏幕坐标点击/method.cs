using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;
namespace 屏幕坐标点击
{
    class method
    {
        public void dianji(int num,string p)
        {
            for (int i = 0; i < num; i++)
            {
                string[] text = p.Split(new string[] { "#" }, StringSplitOptions.None);
                for (int j = 0; j < text.Length; j++)
                {
                    string[] position = text[j].Split(new string[] { "," }, StringSplitOptions.None);
                    if (position.Length > 1)
                    {

                        // MouseFlag.MouseLefClickEvent(9, 1059, 0);
                        int positionx = Convert.ToInt32(position[0]);
                        int positiony = Convert.ToInt32(position[1]);
                        MouseFlag.MouseLefClickEvent(positionx, positiony, 0);
                        Thread.Sleep(2000);
                    }
                }
            }

        }

        public void run(int num)
        {

            try
            {
                dianji(num, "954,338#956,590");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
           
        }

    }
}
