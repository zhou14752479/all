using MySql.Data.MySqlClient;
using O2S.Components.PDFRender4NET;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 授权库
{
    class function
    {
       public static string constr = "Host =8.134.91.137;Database=shouquanku;Username=root;Password=root";

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

        #region  base64转Bitmap并保存
        public void Base64ToImage(string strbase64,string filename)
        {
            try
            {
                byte[] arr = Convert.FromBase64String(strbase64);
                MemoryStream ms = new MemoryStream(arr);
                Bitmap bmp = new Bitmap(ms);
                ms.Close();

                //这里复制一份对图像进行保存，否则会出现“GDI+ 中发生一般性错误”的错误提示
                var bmpNew = new Bitmap(bmp);
                bmpNew.Save(filename);
                bmpNew.Dispose();
                bmp.Dispose();
               // return bmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //return null;
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
            //for (i = 0; i < ColCount; i++)
            //{
            //    lst.Columns.Add(dt.Columns[i].Caption.Trim(), lst.Width / ColCount, System.Windows.Forms.HorizontalAlignment.Left);
            //}
            lst.Columns.Add("uid" ,100, HorizontalAlignment.Center);
            lst.Columns.Add("类型", 100, HorizontalAlignment.Center);
            lst.Columns.Add("名称", 100, HorizontalAlignment.Center);
            lst.Columns.Add("品牌", 100, HorizontalAlignment.Center);
            lst.Columns.Add("类目一", 100, HorizontalAlignment.Center);
            lst.Columns.Add("类目二", 100, HorizontalAlignment.Center);
            lst.Columns.Add("授权开始时间", 100, HorizontalAlignment.Center);
            lst.Columns.Add("授权结束时间", 100, HorizontalAlignment.Center);
            lst.Columns.Add("一级授权截止时间", 100, HorizontalAlignment.Center);
            lst.Columns.Add("是否有原件", 100, HorizontalAlignment.Center);
            lst.Columns.Add("是否有售后承诺书", 100, HorizontalAlignment.Center);
            lst.Columns.Add("是否有商标", 100, HorizontalAlignment.Center);
            lst.Columns.Add("商标结束时间", 100, HorizontalAlignment.Center);
            lst.Columns.Add("备注", 100, HorizontalAlignment.Center);

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


        #region  查询某个字段

        public string getziduan(string id,string name)
        {

            try
            {
             
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();

                string sql = "select " + name + " from datas where id='" + id + "'  ";
                MySqlCommand cmd = new MySqlCommand(sql, mycon);         //SQL语句读取textbox的值'"+textBox1.Text+"'

              
                MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，这个方法不需要绑定资源

                reader.Read();

                string citypinyin = reader[name].ToString().Trim();
                mycon.Close();
                reader.Close();
                return citypinyin;


            }

            catch (System.Exception ex)
            {
                return ex.ToString();
            }


        }

        #endregion

        /// <summary>
        /// 获取时间戳  秒
        /// </summary>
        /// <returns></returns>
        public long GetTimeStamp()
        {
            TimeSpan tss = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long a = Convert.ToInt64(tss.TotalSeconds);
            return a;
        }



        public void getfile(string id,string dicpath)
        {
           
            string str = "select * from file where uid='" + id + "' ";
            MySqlConnection myconn = new MySqlConnection(constr);
            MySqlDataAdapter sda = new MySqlDataAdapter(str, constr);
            DataSet myds = new DataSet();
            myconn.Open();
            sda.Fill(myds);
            myconn.Close();
            DataTable dt = myds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                //Byte[] Files = (Byte[])myds.Tables[0].Rows[0]["filedata"];
                //BinaryWriter bw = new BinaryWriter(File.Open(filename, FileMode.OpenOrCreate));

                string name = dr["name"].ToString();
                Byte[] Files = (Byte[])dr["filedata"];
                BinaryWriter bw = new BinaryWriter(File.Open(dicpath+"//"+name, FileMode.OpenOrCreate));
                bw.Write(Files);
                bw.Close();
            }
           
            

        }


        public Dictionary<string,string> getfileinfos(string id)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string str = "select * from file where uid='" + id + "' ";
            MySqlConnection myconn = new MySqlConnection(constr);
            MySqlDataAdapter sda = new MySqlDataAdapter(str, constr);
            DataSet myds = new DataSet();
            myconn.Open();
            sda.Fill(myds);
            myconn.Close();
            DataTable dt = myds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                
                string uid = dr["uid"].ToString();
                string name = dr["name"].ToString();
                if(!dic.ContainsKey(name))
                {
                    dic.Add(name, uid);
                }
                
            }

            return dic;

        }


        public void insertfile(string uid,string filename)
        {
            string name = Path.GetFileName(filename);
            FileStream fs = new FileStream(filename, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            Byte[] byData = br.ReadBytes((int)fs.Length);
            fs.Close();
            
            MySqlConnection myconn = new MySqlConnection(constr);
            myconn.Open();
            string str = "insert into file (uid,name,filedata) values('" + uid + " ','" + name + " ',@file)";
            MySqlCommand mycomm = new MySqlCommand(str, myconn);
            mycomm.Parameters.Add("@file", MySqlDbType.Binary, byData.Length);
            mycomm.Parameters["@file"].Value = byData;
            mycomm.ExecuteNonQuery();
            myconn.Close();

        }

       
    }
}
