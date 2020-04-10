using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;

namespace 启动程序
{
    public partial class TxtoExcel : Form
    {
        public TxtoExcel()
        {
            InitializeComponent();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;

           
        }
      




        public void run()
        {
            try
            {
                ArrayList lists = new ArrayList();
                StreamReader sr = new StreamReader(textBox1.Text, Encoding.Default);
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i] != "")
                    {
                        lists.Add(text[i]);
                        
                    }

                }



                for (int i = 0; i < lists.Count; i++)
                {
                    try
                    {
                        string value = lists[(13 * i)].ToString().Replace("姓名", "").Trim();
                        string value1 = lists[(13 * i)+1].ToString().Replace("统一社会信用代码", "").Trim();
                        string value2 = lists[(13 * i)+2].ToString().Replace("险种类型", "").Trim();
                        string value3 = lists[(13 * i)+3].ToString().Replace("基准参保关系ID", "").Trim();
                        string value4 = lists[(13 * i)+4].ToString().Replace("证件类型", "").Trim();
                        string value5 = lists[(13 * i)+5].ToString().Replace("证件号码", "").Trim();
                        string value6 = lists[(13 * i)+6].ToString().Replace("开始日期", "").Trim();
                        string value7 = lists[(13 * i)+7].ToString().Replace("终止日期", "").Trim();
                        string value8 = lists[(13 * i)+8].ToString().Replace("单位编号", "").Trim();
                        string value9 = lists[(13 * i)+9].ToString().Replace("单位名称", "").Trim();
                        string value10 = lists[(13 * i)+10].ToString().Replace("行政区划代码", "").Trim();
                        string value11 = lists[(13 * i)+11].ToString().Replace("个人编号", "").Trim();
                        string value12 = lists[(13 * i)+12].ToString().Replace("社会保障号码", "").Trim();


                        ListViewItem lv1 = listView1.Items.Add(value); //使用Listview展示数据

                        lv1.SubItems.Add(value1);
                        lv1.SubItems.Add(value2);
                        lv1.SubItems.Add(value3);
                        lv1.SubItems.Add(value4);
                        lv1.SubItems.Add(value5);
                        lv1.SubItems.Add(value6);
                        lv1.SubItems.Add(value7);
                        lv1.SubItems.Add(value8);
                        lv1.SubItems.Add(value9);
                        lv1.SubItems.Add(value10);
                        lv1.SubItems.Add(value11);
                        lv1.SubItems.Add(value12);



                        if (listView1.Items.Count > 2)
                        {
                            listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置
                        }





                    }


                    catch
                    {

                        continue;
                    }



                }







            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            bool flag = this.openFileDialog1.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }
    }
}
