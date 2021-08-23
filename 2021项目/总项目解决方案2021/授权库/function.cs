using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 授权库
{
    class function
    {
        string constr = "Host =localhost;Database=shouquanku;Username=root;Password=root";

        #region  注册函数

        public void SQL(string sql)
        {

            try
            {

              
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();




                MySqlCommand cmd = new MySqlCommand(sql, mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'


                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.
                if (count > 0)
                {
                    MessageBox.Show("执行成功！");
                    mycon.Close();

                }
                else
                {
                    MessageBox.Show("执行失败！");
                }


            }

            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

           

        }
        #endregion


        #region 获取数据
        public DataTable getdata(string sql)
        {
           
            try
            {
               
             
                MySqlDataAdapter da = new MySqlDataAdapter(sql, constr);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                return dt;
            }
            catch (MySqlException ee)
            {
                MessageBox.Show(ee.Message.ToString());
            }
            return null;

        }
        #endregion

        #region  image转base64
        public string ImageToBase64(Image image)
        {
            try
            {
                Bitmap bmp = new Bitmap(image);
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                return Convert.ToBase64String(arr);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region  base64转image
        public  Bitmap Base64ToImage(string strbase64)
        {
            try
            {
                byte[] arr = Convert.FromBase64String(strbase64);
                MemoryStream ms = new MemoryStream(arr);
                Bitmap bmp = new Bitmap(ms);
                ms.Close();

                return bmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        #endregion

        #region 读取datatable到ListView
        public void ShowDataInListView(DataTable dt, ListView lst)
        {
           lst.Clear();
            //   lst.View=System.Windows.Forms.View.Details;
            lst.AllowColumnReorder = true;//用户可以调整列的位置
            lst.GridLines = true;

            int RowCount, ColCount, i, j;
            DataRow dr = null;

            if (dt == null) return;
            RowCount = dt.Rows.Count;
            ColCount = dt.Columns.Count;
            //添加列标题名
            for (i = 0; i < ColCount; i++)
            {
                lst.Columns.Add(dt.Columns[i].Caption.Trim(), lst.Width / ColCount, System.Windows.Forms.HorizontalAlignment.Left);
            }

            if (RowCount == 0) return;
            for (i = 0; i < RowCount; i++)
            {
                dr = dt.Rows[i];
                lst.Items.Add(dr[0].ToString().Trim());
                for (j = 1; j < ColCount; j++)
                {
                    lst.Columns[j].Width = 100;
                    lst.Items[i].SubItems.Add((string)dr[j].ToString().Trim());
                }
            }
        }

        #endregion
    }
}
