using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Method;
using ClassLibrary1;
using System.IO;

namespace MeiT
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        

        #region 主方法
        public void run()
        {
            
            string[] citys = textBox1.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
            int page = Convert.ToInt32(textBox2.Text);

            foreach (string city in citys)

            {

                for (int i = 1; i <= page; i++)
                {
                    String Url = "http://"+city+".meituan.com/meishi/pn"+i+"/";
                    string html = method.meituan_GetUrl(Url);  //定义的GetRul方法 返回 reader.ReadToEnd()

                    MatchCollection TitleMatchs = Regex.Matches(html, @"{""poiId"":([\s\S]*?),", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    ArrayList lists = new ArrayList();
                    foreach (Match NextMatch in TitleMatchs)
                    {

                        lists.Add("http://www.meituan.com/meishi/" + NextMatch.Groups[1].Value + "/");

                    }
                  

                    Application.DoEvents();
                    Thread.Sleep(1001);

                    foreach (string list in lists)
                    {


                        

                        String Url1 = list;
                        string strhtml = method.meituan_GetUrl(Url1);  //定义的GetRul方法 返回 reader.ReadToEnd()

                        string Rxg = @"{""poiId"":([\s\S]*?),""name"":""([\s\S]*?)"",""avgScore"":([\s\S]*?),""address"":""([\s\S]*?)"",""phone"":""([\s\S]*?)"",";
                      
                        string Rxg1 = @"{""id"":""([\s\S]*?)"",""name"":""([\s\S]*?)"",""price"":([\s\S]*?),";

                        string rxg = @"""avgPrice"":([\s\S]*?),";

                        Match name = Regex.Match(strhtml, Rxg);
                        MatchCollection cai = Regex.Matches(strhtml, Rxg1);
                        Match price = Regex.Match(strhtml, rxg);




                        foreach (Match match in cai)
                        {
                            int index = this.dataGridView1.Rows.Add();
                            this.dataGridView1.Rows[index].Cells[0].Value = index;
                            this.dataGridView1.Rows[index].Cells[1].Value = name.Groups[2].Value;
                            this.dataGridView1.Rows[index].Cells[2].Value = name.Groups[5].Value;
                            this.dataGridView1.Rows[index].Cells[3].Value = name.Groups[4].Value;
                            this.dataGridView1.Rows[index].Cells[4].Value = price.Groups[1].Value;

                            this.dataGridView1.Rows[index].Cells[5].Value = match.Groups[2].Value;
                            this.dataGridView1.Rows[index].Cells[6].Value = match.Groups[3].Value;

                            this.dataGridView1.CurrentCell = this.dataGridView1.Rows[index].Cells[0];  //让datagridview滚动到当前行
                        }

                        if (button4.Text == "已停止")
                        {
                            return;
                        }




                        Application.DoEvents();
                        System.Threading.Thread.Sleep(2000);

                    }
                }

            }

         }

        #endregion



        #region 主方法2
        public void run2()
        {
            
            string[] citys = textBox1.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
            int page = Convert.ToInt32(textBox2.Text);

            foreach (string city in citys)

            {

                for (int i = 1; i <= page; i++)
                {
                    String Url = "http://" + city + ".meituan.com/meishi/pn" + i + "/";
                    string html = Cookie.Get(Url,textBox3.Text.Trim());  //定义的GetRul方法 返回 reader.ReadToEnd()

                    MatchCollection TitleMatchs = Regex.Matches(html, @"{""poiId"":([\s\S]*?),", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    ArrayList lists = new ArrayList();
                    foreach (Match NextMatch in TitleMatchs)
                    {

                        lists.Add("http://www.meituan.com/meishi/" + NextMatch.Groups[1].Value + "/");

                    }

                    if (button4.Text == "已停止")
                    
                        return;
                    
                    Application.DoEvents();
                    Thread.Sleep(1001);

                    foreach (string list in lists)
                    {

                        string strhtml = Cookie.Get(list,textBox3.Text.Trim());  //定义的GetRul方法 返回 reader.ReadToEnd()

                       

                        string Rxg = @"{""poiId"":([\s\S]*?),""name"":""([\s\S]*?)"",""avgScore"":([\s\S]*?),""address"":""([\s\S]*?)"",""phone"":""([\s\S]*?)"",";

                        string Rxg1 = @"g"",""title"":""([\s\S]*?)"",""soldNum"":([\s\S]*?),""price"":([\s\S]*?),";
                        string rxg = @"""avgPrice"":([\s\S]*?),";


                        Match name = Regex.Match(strhtml, Rxg);
                        MatchCollection cai = Regex.Matches(strhtml, Rxg1);
                        Match price = Regex.Match(strhtml, rxg);


                        foreach (Match match in cai)
                        {
                            int index = this.dataGridView2.Rows.Add();
                            this.dataGridView2.Rows[index].Cells[0].Value = index;
                            this.dataGridView2.Rows[index].Cells[1].Value = name.Groups[2].Value;
                            this.dataGridView2.Rows[index].Cells[2].Value = name.Groups[5].Value;
                            this.dataGridView2.Rows[index].Cells[2].Value = name.Groups[5].Value;
                            this.dataGridView2.Rows[index].Cells[3].Value = name.Groups[4].Value;
                            this.dataGridView2.Rows[index].Cells[4].Value = price.Groups[1].Value;

                            this.dataGridView2.Rows[index].Cells[5].Value = match.Groups[1].Value;
                            this.dataGridView2.Rows[index].Cells[6].Value = match.Groups[2].Value;
                            this.dataGridView2.Rows[index].Cells[7].Value = match.Groups[3].Value;

                            this.dataGridView2.CurrentCell = this.dataGridView2.Rows[index].Cells[0];  //让datagridview滚动到当前行
                        }


                        if (button4.Text == "已停止")
                        {
                            return;
                        }
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(2000);

                    }
                }

            }

        }

        #endregion

        #region   dataGridView导出CSV带进度条，导出分列
        public static void csv(DataGridView dgv,ProgressBar progressbar)
        {

            if (dgv.Rows.Count == 0)
            {
                MessageBox.Show("No data available!", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
                saveFileDialog.FilterIndex = 0;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.CreatePrompt = true;
                saveFileDialog.FileName = null;
                saveFileDialog.Title = "Save path of the file to be exported";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Stream myStream = saveFileDialog.OpenFile();
                    StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding(-0));
                    string strLine = "";
                    try
                    {
                        //Write in the headers of the columns.
                        for (int i = 0; i < dgv.ColumnCount; i++)
                        {
                            if (i > 0)
                                strLine += ",";
                            strLine += dgv.Columns[i].HeaderText;
                        }
                        strLine.Remove(strLine.Length - 1);
                        sw.WriteLine(strLine);
                        strLine = "";
                        //Write in the content of the columns.
                        for (int j = 0; j < dgv.Rows.Count; j++)
                        {
                            strLine = "";
                            for (int k = 0; k < dgv.Columns.Count; k++)
                            {
                                if (k > 0)
                                    strLine += ",";
                                if (dgv.Rows[j].Cells[k].Value == null)
                                    strLine += "";
                                else
                                {
                                    string m = dgv.Rows[j].Cells[k].Value.ToString().Trim();
                                    strLine += m.Replace(",", "，");
                                }
                            }
                            strLine.Remove(strLine.Length - 1);
                            sw.WriteLine(strLine);
                            //Update the Progess Bar.
                           progressbar.Value = 100 * (j + 1) / dgv.Rows.Count;
                        }
                        sw.Close();
                        myStream.Close();
                        MessageBox.Show("Data has been exported to：" + saveFileDialog.FileName.ToString(), "Exporting Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        progressbar.Value = 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Exporting Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }

        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                dataGridView2.Visible = false;
                Thread search_thread = new Thread(new ThreadStart(run));
                Control.CheckForIllegalCrossThreadCalls = false;
                search_thread.Start();
            }

            else if (radioButton2.Checked == true)
            {
                dataGridView1.Visible = false;
                Thread search_thread = new Thread(new ThreadStart(run2));
                Control.CheckForIllegalCrossThreadCalls = false;
                search_thread.Start();
            }

            else
            {
                MessageBox.Show("请选择采集分类！");
            }
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            csv(dataGridView1, progressBar1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            csv(dataGridView2, progressBar1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button4.Text = "已停止";
        }
    }
}
