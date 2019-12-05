using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zhaopin_58
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {

            try
            {
                string mac = method.GetMacAddress();
                DateTime time = DateTime.Now;

                string constr = "Host =47.99.68.92;Database=acaiji;Username=root;Password=zhoukaige00.@*.";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                MySqlCommand cmd = new MySqlCommand("select * from kami where mac='" + mac.Trim() + "'  ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源
                    
                if (reader.Read())
                {
                    DateTime datatime = Convert.ToDateTime(reader["time"].ToString().Trim());
                    if (time<datatime)
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new meituan());
                    }
                    else
                    {
                        MessageBox.Show("您的软件已经过期！");

                    }

                }
                else
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new yanzheng());

                }

                mycon.Close();
                reader.Close();




            }

            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }



        }
    }
}
