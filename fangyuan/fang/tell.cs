using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fang
{
    public partial class tell : Form
    {
        public tell()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        bool status = true;
        bool zanting = true;
        private void tell_Load(object sender, EventArgs e)
        {
            readData();
        }

        ArrayList tells = new ArrayList();


        public void insertData(string value)
        {

            try
            {
                string constr = "Host =116.62.62.62;Database=vip;Username=root;Password=zhoukaige";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO linshi (tell)VALUES('" + value + " ')", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

                //  MySqlDataReader reader = cmd.ExecuteReader();  //读取数据库数据信息，注册不需要读取，直接执行SQL语句即可

                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.

            }

            catch (System.Exception ex)
            {
                ex.ToString();
            }
        }


        public void readData()
        {



            string constr = "Host =116.62.62.62;Database=vip;Username=root;Password=zhoukaige";
            MySqlConnection mycon = new MySqlConnection(constr);
            mycon.Open();
            MySqlCommand cmd = new MySqlCommand("select * from linshi ", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'
            MySqlDataReader dr = cmd.ExecuteReader();
            //用dr的read函数，每执行一次，返回一个包含下一行数据的集合dr
            while (dr.Read())
            {
                //构建一个ListView的数据，存入数据库数据，以便添加到listView1的行数据中
                ListViewItem lt = new ListViewItem();
                //将数据库数据转变成ListView类型的一行数据
                // lt.Text = dr["id"].ToString();
                lt.Text = (listView1.Items.Count + 1).ToString();
                lt.SubItems.Add(dr["tell"].ToString());
                lt.SubItems.Add(dr["yucun"].ToString());
                //将lt数据添加到listView1控件中
                listView1.Items.Add(lt);
            }

            mycon.Close();
        }


        public void deleteData()
        {
            try
            {
                string constr = "Host =116.62.62.62;Database=vip;Username=root;Password=zhoukaige";
                MySqlConnection mycon = new MySqlConnection(constr);
                mycon.Open();
                MySqlCommand cmd = new MySqlCommand("truncate table linshi", mycon);         //SQL语句读取textbox的值'"+skinTextBox1.Text+"'

               

                int count = cmd.ExecuteNonQuery();  //count就是受影响的行数,如果count>0说明执行成功,如果=0说明没有成功.

            }

            catch (System.Exception ex)
            {
                ex.ToString();
            }


        }

        #region  主函数
        public void baidu()

        {

            
            try

            {

                for (int i = 0; i < 9999; i++)
                {




                    string path = System.Environment.CurrentDirectory; //获取当前程序运行文件夹


                    string html = method.GetUrl("http://num.10010.com/NumApp/NumberCenter/qryNum?callback=jsonp_queryMoreNums&provinceCode=11&cityCode=110&monthFeeLimit=0&groupKey=&searchCategory=3&net=01&amounts=210&codeTypeCode=&searchValue=&qryType=01&goodsNet=4&_=1545119980725", "utf-8");

                    string prttern = @"\d{11}";
                    string prttern1 = @"\d{11},([\s\S]*?),";

                    MatchCollection aurls = Regex.Matches(html, prttern);
                    MatchCollection aurls1 = Regex.Matches(html, prttern1);

                    while (this.zanting == false)
                    {
                        Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                    }

                    if (this.status == false)
                        return;


                    if (aurls.Count > 0 && aurls1.Count > 0)
                    {

                        for (int j = 0; j < aurls.Count; j++)
                        {

                            if (!tells.Contains(aurls[j].Groups[0].Value))
                            {
                                tells.Add(aurls[j].Groups[0].Value);
                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据

                                lv1.SubItems.Add(aurls[j].Groups[0].Value);
                                lv1.SubItems.Add(aurls1[j].Groups[1].Value);
                                if (listView1.Items.Count - 1 > 0)
                                {
                                    listView1.EnsureVisible(listView1.Items.Count - 1);
                                }
                                insertData(aurls[j].Groups[0].Value);
                            }


                        }

                    }
                }

                
            }

            catch (Exception ex)
            {
                MessageBox.Show( ex.ToString());
            }
        }

        #endregion

        private void skinButton2_Click(object sender, EventArgs e)
        {
            deleteData();
            for (int i = 0; i < 5; i++)
            {
                Thread thread = new Thread(new ThreadStart(baidu));
                thread.Start();
            }

        }

        private void skinButton6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void skinButton4_Click(object sender, EventArgs e)
        {

            // method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("列表为空!");
            }
            else
            {
                List<string> list = new List<string>();
                foreach (ListViewItem item in listView1.Items)
                {
                    string temp = item.SubItems[1].Text;
                    string temp1 = item.SubItems[2].Text;
                    list.Add(temp+"\t"+temp1);
                }
                Thread thexp = new Thread(() => export(list)) { IsBackground = true };
                thexp.Start();
            }

        }


        private void export(List<string> list)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory  + Guid.NewGuid().ToString() + ".txt";

            //删除txt文本
            string path1 = Environment.CurrentDirectory;
                        string pattern = "*.txt";
                        string[] strFileName = Directory.GetFiles(path1, pattern);
                         foreach (var item in strFileName)
                            {
                                 File.Delete(item);
                               
                            }
            //删除txt文本

            StringBuilder sb = new StringBuilder();
            foreach (string tel in list)
            {
                sb.AppendLine(tel);
            }
            System.IO.File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
            MessageBox.Show("导出完成！存放地址为程序文件夹");
           
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            this.zanting = false;
        }

        private void skinButton5_Click(object sender, EventArgs e)
        {
            this.zanting = true;
        }
    }
}
