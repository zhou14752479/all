using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using myDLL;
using NPOI.SS.Formula.Functions;

namespace win007
{
    public partial class 软件6套9列 : Form
    {
        public 软件6套9列()
        {
            InitializeComponent();
        }



     
        private void 软件6套9列_Load(object sender, EventArgs e)
        {
          


            foreach (Control control in groupBox1.Controls)
            {
                if (control is TextBox)
                {
                    control.Text = "0.00 0.00 0.00  0.00 0.01 0.02  0.00 0.01 0.02  0.00 0.01 0.02  0.00 0.01 0.02";


                }

            }

            foreach (Control control in groupBox18.Controls)
            {
                if (control is TextBox)
                {
                    control.Text = "0.50 0.01 0.02  0.50 0.01 0.01  0.50 0.01 0.02  0.00 0.03 0.02  0.00 0.01 0.03";


                }

            }
            foreach (Control control in groupBox3.Controls)
            {
                if (control is TextBox)
                {
                    control.Text = "0.00 0.01 0.02  0.00 0.01 0.02  0.00 0.01 0.02  0.00 0.01 0.01  0.00 0.01 0.02";


                }

            }
            foreach (Control control in groupBox4.Controls)
            {
                if (control is TextBox)
                {
                    control.Text = "0.00 0.01 0.02  0.00 0.01 0.02  0.00 0.01 0.02  0.00 0.01 0.02  0.00 0.01 0.02";


                }

            }
            foreach (Control control in groupBox5.Controls)
            {
                if (control is TextBox)
                {
                    control.Text = "0.00 0.01 0.02  0.00 0.01 0.02  0.00 0.01 0.01  0.00 0.01 0.02  0.00 0.01 0.02";


                }

            }
            foreach (Control control in groupBox6.Controls)
            {
                if (control is TextBox)
                {
                    control.Text = "0.00 0.01 0.02  0.00 0.01 0.02  0.00 0.01 0.02  0.00 0.01 0.02  0.00 0.01 0.02";


                }

            }
            method.SetFeatures(11000);
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate("https://live.titan007.com/");
        }

        private void webBrowser1_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            string url = this.webBrowser1.StatusText;
            linkLabel1.Text = "点击填入数据：" + url;

          
        }





        function fc = new function();
        public string chaxun(string t1, string t2, string t3, string t4, string t5, ComboBox comb1, string t9, string t8, string t7, string t13, TextBox xianshi)
        {
            int zhusheng_bifen_count = 0;
            int heju_bifen_count = 0;
            int kesheng_bifen_count = 0;

            try
            {
                //if(t1.Text=="" &&t2.Text=="" && t3.Text=="")
                //{
                //    MessageBox.Show("请输入数值");
                //    return;
                //}



                string sql = "select * from datas where";


                if (t1 != "")
                {
                    sql = sql + (" data1 like '" + t1.Trim() + "' and");
                }
                if (t2 != "")
                {
                    sql = sql + (" data2 like '" + t2.Trim() + "' and");
                }
                if (t3 != "")
                {
                    sql = sql + (" data3 like '" + t3.Trim() + "' and");
                }




                if (t9 != "")
                {
                    sql = sql + (" data4 like '" + t9.Trim() + "' and");
                }
                if (t8 != "")
                {
                    sql = sql + (" data5 like '" + t8.Trim() + "' and");
                }
                if (t7 != "")
                {
                    sql = sql + (" data6 like '" + t7.Trim() + "' and");
                }


                //新数据库更新让球所在列
                //if (t5 != "")
                //{
                //    sql = sql + (" rangqiudaxiaoqiu like '" + t5.Trim() + "%' and");
                //}

                if (t5 != "")
                {
                    sql = sql + (" sjp2 like '" + t5.Trim() + "%' and");
                }



                if (comb1.Text != "")
                {
                    //sql = sql + (" gongsi like '" + comb1.Text.Trim() + "' and");
                    sql = sql + (" gongsi == '" + comb1.Text.Trim() + "' and");
                }

                if (sql.Substring(sql.Length - 3, 3) == "and")
                {
                    sql = sql.Substring(0, sql.Length - 3);
                }


                
               
                DataTable dt = fc.chaxundata(sql);

                
                //计算

                for (int i = 0; i < dt.Rows.Count; i++)//如果DataGridView中有空的数据，则提示数据输入不完整并退出添加，不包括标题行
                {
                    try
                    {
                        string value1 = dt.Rows[i]["data1"].ToString(); 
                        string value2 = dt.Rows[i]["data2"].ToString(); 
                        string value3 = dt.Rows[i]["data3"].ToString(); 

                        string value4 = dt.Rows[i]["data4"].ToString(); 
                        string value5 = dt.Rows[i]["data5"].ToString();
                        string value6 = dt.Rows[i]["data6"].ToString();

                        string value7 = dt.Rows[i]["data7"].ToString();
                        string value8 = dt.Rows[i]["data8"].ToString();
                        string value9 = dt.Rows[i]["data9"].ToString();

                        //rule1_txtbox.Text+= (dt.Rows[i]["bifen"].ToString()+"--");
                        double cha1 = Convert.ToDouble(value1) - Convert.ToDouble(value4);
                        double cha2 = Convert.ToDouble(value2) - Convert.ToDouble(value5);
                        double cha3 = Convert.ToDouble(value3) - Convert.ToDouble(value6);


                        double cha33 = Convert.ToDouble(value6) - Convert.ToDouble(value3);


                        double cha4 = Convert.ToDouble(value4) - Convert.ToDouble(value7);
                        double cha5 = Convert.ToDouble(value5) - Convert.ToDouble(value8);
                        double cha6 = Convert.ToDouble(value6) - Convert.ToDouble(value9);





                        string sjp1 = "平";
                        string sjp2 = "平";
                        string sjp3 = "平";


                        string sjp4 = "平";
                        string sjp5 = "平";
                        string sjp6 = "平";

                        if (cha1 > 0)
                        {
                            sjp1 = "升";
                        }
                        if (cha1 < 0)
                        {
                            sjp1 = "降";
                        }

                        if (cha2 > 0)
                        {
                            sjp2 = "升";
                        }
                        if (cha2 < 0)
                        {
                            sjp2 = "降";
                        }
                        if (cha3 > 0)
                        {
                            sjp3 = "升";
                        }
                        if (cha3 < 0)
                        {
                            sjp3 = "降";
                        }


                        if (cha4 > 0)
                        {
                            sjp4 = "升";
                           
                        }
                        if (cha5 > 0)
                        {
                            sjp5 = "升";
                            
                        }
                        if (cha6 > 0)
                        {
                            sjp6 = "升";
                           
                        }
                        if (cha4 < 0)
                        {
                            sjp4 = "降";
                           
                        }
                        if (cha5 < 0)
                        {
                            sjp5 = "降";
                           
                        }
                        if (cha6 < 0)
                        {
                            sjp6 = "降";
                           
                        }

                        string bifen= dt.Rows[i]["bifen"].ToString();
                        string sjp11 = sjp1 + sjp2 + sjp3;
                        string sjp22= sjp4 + sjp5+ sjp6;

                     
                        string sjpshaixuan1 = t4.Trim();
                        string sjpshaixuan2= t13.Trim();
                        if (sjp11.Contains(sjpshaixuan1)&& sjp22.Contains(sjpshaixuan2))
                        {
                            string[] bifens = bifen.Split(new string[] { "-" }, StringSplitOptions.None);
                            if (bifens.Length == 2)
                            {
                                if (Convert.ToInt32(bifens[0]) > Convert.ToInt32(bifens[1]))
                                {
                                    zhusheng_bifen_count = zhusheng_bifen_count + 1;
                                }
                                else if (Convert.ToInt32(bifens[0]) == Convert.ToInt32(bifens[1]))
                                {
                                    heju_bifen_count = heju_bifen_count + 1;
                                }
                                else if (Convert.ToInt32(bifens[0]) < Convert.ToInt32(bifens[1]))
                                {
                                    kesheng_bifen_count = kesheng_bifen_count + 1;
                                }
                            }
                        }


                    }
                    catch (Exception)
                    {

                        continue;
                    }

                }

                //MessageBox.Show(zhusheng_bifen_count +" "+ heju_bifen_count+" "+ kesheng_bifen_count);

                string l1 = Convert.ToDouble(Convert.ToDouble(zhusheng_bifen_count) / Convert.ToDouble((zhusheng_bifen_count + heju_bifen_count + kesheng_bifen_count))).ToString("F2").Replace("非数字", "0.00").Replace("NaN","0.00");
                string l2 = Convert.ToDouble(Convert.ToDouble(heju_bifen_count) / Convert.ToDouble((zhusheng_bifen_count + heju_bifen_count + kesheng_bifen_count))).ToString("F2").Replace("非数字", "0.00").Replace("NaN", "0.00");
                string l3 = Convert.ToDouble(Convert.ToDouble(kesheng_bifen_count) / Convert.ToDouble((zhusheng_bifen_count + heju_bifen_count + kesheng_bifen_count))).ToString("F2").Replace("非数字", "0.00").Replace("NaN", "0.00");

                //  xianshi.Text = xianshi.Text + l1 + " " + l2 + " " + l3 + "   ";

                string value = l1 + " " + l2 + " " + l3 + "   ";
                return value;


            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.ToString());

                return "";
            }
        }

        public string getminmaxvalue(int hang, int sort)
        {
            double a = Convert.ToDouble(dic[1]);
            double b = Convert.ToDouble(dic[3]);

            if (hang == 1)
            {


            }
            if (hang == 2)
            {
                a = Convert.ToDouble(dic[9]);
                b = Convert.ToDouble(dic[7]);

            }

            double min, max;


            if (a < b)
            {
                min = a;
                max = b;
            }

            else if (a > b)
            {
                min = b;
                max = a;
            }
            else
            {
                min = a;
                max = b;
            }


            if (sort == 1)
            {
                return min.ToString();
            }

            if (sort == 3)
            {
                return max.ToString();
            }

            return "";
        }

        public Dictionary<int, string> get_data(string company)
        {

            Dictionary<int, string> dic = new Dictionary<int, string>();
            string vvv = "";
            try
            {

                //label48.Text = "";
                string id = Regex.Match(linkLabel1.Text.Trim(), @"\(([\s\S]*?)\)").Groups[1].Value;
                if (id == "")
                {
                    id = Regex.Match(textBox6.Text.Trim(), @"\d{6,}").Groups[0].Value;
                }


                string data = function.getshishidata(id, company);


                string[] text = data.Split(new string[] { "," }, StringSplitOptions.None);
                if (text.Length > 3)
                {
                    string s1 = "平";
                    string j1 = "平";
                    string p1 = "平";

                    string s2 = "平";
                    string j2 = "平";
                    string p2 = "平";




                    dic.Add(1, text[1].Replace("\"", ""));
                    dic.Add(2, text[2].Replace("\"", ""));
                    dic.Add(3, text[3].Replace("\"", ""));

                    dic.Add(9, text[4].Replace("\"", ""));
                    dic.Add(8, text[5].Replace("\"", ""));
                    dic.Add(7, text[6].Replace("\"", ""));

                    vvv = text[1].Replace("\"", "") + "|" + text[2].Replace("\"", "") + "|" + text[3].Replace("\"", "");

                    s1 = Convert.ToDouble(text[1]) - Convert.ToDouble(text[4]) < 0 ? "降" : "升";
                    j1 = Convert.ToDouble(text[2]) - Convert.ToDouble(text[5]) < 0 ? "降" : "升";
                    p1 = Convert.ToDouble(text[3]) - Convert.ToDouble(text[6]) < 0 ? "降" : "升";

                    s1 = Convert.ToDouble(text[1]) - Convert.ToDouble(text[4]) == 0 ? "平" : s1;
                    j1 = Convert.ToDouble(text[2]) - Convert.ToDouble(text[5]) == 0 ? "平" : j1;
                    p1 = Convert.ToDouble(text[3]) - Convert.ToDouble(text[6]) == 0 ? "平" : p1;





                    s2 = Convert.ToDouble(text[4]) - Convert.ToDouble(text[7]) < 0 ? "降" : "升";
                    j2 = Convert.ToDouble(text[5]) - Convert.ToDouble(text[8]) < 0 ? "降" : "升";
                    p2 = Convert.ToDouble(text[6]) - Convert.ToDouble(text[9]) < 0 ? "降" : "升";

                    s2 = Convert.ToDouble(text[4]) - Convert.ToDouble(text[7]) == 0 ? "平" : s2;
                    j2 = Convert.ToDouble(text[5]) - Convert.ToDouble(text[8]) == 0 ? "平" : j2;
                    p2 = Convert.ToDouble(text[6]) - Convert.ToDouble(text[9]) == 0 ? "平" : p2;




                    //if (Convert.ToDouble(text[5]) - Convert.ToDouble(text[4]) == 0)
                    //{
                    //    label48.Text = "特殊数据";

                    //}

                    dic.Add(4, s1 + j1 + p1);
                    dic.Add(13, s2 + j2 + p2);
                    //textBox4.Text = s1 + j1 + p1;
                    //textBox13.Text = s2 + j2 + p2;

                    if (company == comboBox1.Text.Trim())
                    {
                        label31.Text = vvv;
                    }
                    if (company == comboBox2.Text.Trim())
                    {
                        label32.Text = vvv;
                    }
                    if (company == comboBox3.Text.Trim())
                    {
                        label33.Text = vvv;
                    }
                    if (company == comboBox4.Text.Trim())
                    {
                        label34.Text = vvv;
                    }
                    if (company == comboBox5.Text.Trim())
                    {
                        label35.Text = vvv;
                    }
                }
                return dic;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                return dic;

            }
        }



        Dictionary<int, string> dic1 = new Dictionary<int, string>();
        Dictionary<int, string> dic2 = new Dictionary<int, string>();
        Dictionary<int, string> dic3 = new Dictionary<int, string>();
        Dictionary<int, string> dic4 = new Dictionary<int, string>();
        Dictionary<int, string> dic5 = new Dictionary<int, string>();



        Dictionary<int, string> dic = new Dictionary<int, string>();
        ComboBox comb;
        public void sancirun(string t1, string t2, string t3, string t4, string t5, string t9, string t8, string t7, string t13, TextBox xianshi)
        {
            try
            {
                string value = "0.00 0.00 0.00  ";
            
             
                value = chaxun(t1, t2, t3, t4, t5, comb, t9, t8, t7, t13, xianshi);
             
              
                
                //label31.Text = getshishidata(comboBox1.Text.Trim());

                
                xianshi.Text =xianshi.Text+ value;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        string t = "";


        bool status1 = true;
        bool status2 = true;
        bool status3 = true;
        bool status4 = true;
        bool status5 = true;
        bool status6 = true;

        public void run()
        {
            new System.Threading.Thread((System.Threading.ThreadStart)delegate {


                #region  //算法一开始

                new System.Threading.Thread((System.Threading.ThreadStart)delegate {

                    if (getminmaxvalue(1, 1) == dic[1])
                    {
                        sancirun(dic[1], t, t, dic[4], textBox5.Text, t, t, t, t, rule1_txtbox);
                    }
                    else if (getminmaxvalue(1, 1) == dic[3])
                    {
                        sancirun(t, t, dic[3], dic[4], textBox5.Text, t, t, t, t, rule1_txtbox);

                    }



                    if (getminmaxvalue(2, 1) == dic[9])
                    {
                        sancirun(t, t, t, t, textBox5.Text, dic[9], t, t, dic[13], rule2_txtbox);
                    }
                    else if (getminmaxvalue(2, 1) == dic[7])
                    {
                        sancirun(t, t, t, t, textBox5.Text, t, t, dic[7], dic[13], rule2_txtbox);
                    }




                    if (getminmaxvalue(2, 1) == dic[9])
                    {
                        sancirun(t, t, t, dic[4], textBox5.Text, dic[9], t, t, dic[13], rule3_txtbox);
                    }
                    else if (getminmaxvalue(2, 1) == dic[7])
                    {
                        sancirun(t, t, t, dic[4], textBox5.Text, t, t, dic[7], dic[13], rule3_txtbox);
                    }



                }).Start();

               

             
                //算法一结束

                #endregion

                status1 = true;

            }).Start();


            

            new System.Threading.Thread((System.Threading.ThreadStart)delegate {

              
                #region    算法二开始



             

                sancirun(t, dic[2], t, dic[4], textBox5.Text, t, t, t, t, rule1_txtbox_2);

              

                sancirun(t, t, t, t, textBox5.Text, t, dic[8], t, dic[13], rule2_txtbox_2);

             

                sancirun(t, t, t, dic[4], textBox5.Text, t, dic[8], t, dic[13], rule3_txtbox_2);

               

             

                #endregion 算法二结束

                status2 = true;

            }).Start();


             new System.Threading.Thread((System.Threading.ThreadStart)delegate {

                #region 算法三开始
               

                if (getminmaxvalue(1, 3) == dic[1])
                {
                    sancirun(dic[1], t, t, dic[4], textBox5.Text, t, t, t, t, rule1_txtbox_3);
                }
                else if (getminmaxvalue(1, 3) == dic[3])
                {
                    sancirun(t, t, dic[3], dic[4], textBox5.Text, t, t, t, t, rule1_txtbox_3);

                }

               

                if (getminmaxvalue(2, 3) == dic[9])
                {
                    sancirun(t, t, t, t, textBox5.Text, dic[9], t, t, dic[13], rule2_txtbox_3);
                }
                else if (getminmaxvalue(2, 3) == dic[7])
                {
                    sancirun(t, t, t, t, textBox5.Text, t, t, dic[7], dic[13], rule2_txtbox_3);

                }

               

                if (getminmaxvalue(2, 3) == dic[9])
                {
                    sancirun(t, t, t, dic[4], textBox5.Text, dic[9], t, t, dic[13], rule3_txtbox_3);
                }
                else if (getminmaxvalue(2, 3) == dic[7])
                {
                    sancirun(t, t, t, dic[4], textBox5.Text, t, t, dic[7], dic[13], rule3_txtbox_3);

                }

               

              

                 //算法三结束
                 #endregion

                 status3 = true;

             }).Start();


            new System.Threading.Thread((System.Threading.ThreadStart)delegate {

                #region 算法四开始

                new System.Threading.Thread((System.Threading.ThreadStart)delegate {

                    if (getminmaxvalue(2, 3) == dic[9])
                    {
                        sancirun(t, t, t, t, textBox5.Text, dic[9], t, t, t, rule1_txtbox_4);
                    }
                    else if (getminmaxvalue(2, 3) == dic[7])
                    {
                        sancirun(t, t, t, t, textBox5.Text, t, t, dic[7], t, rule1_txtbox_4);

                    }



                    if (getminmaxvalue(2, 3) == dic[9])
                    {
                        sancirun(t, t, t, dic[4], textBox5.Text, dic[9], t, t, t, rule2_txtbox_4);
                    }
                    else if (getminmaxvalue(2, 3) == dic[7])
                    {
                        sancirun(t, t, t, dic[4], textBox5.Text, t, t, dic[7], t, rule2_txtbox_4);

                    }



                    if (getminmaxvalue(2, 3) == dic[9] && getminmaxvalue(1, 3) == dic[1])
                    {
                        sancirun(dic[1], t, t, dic[4], textBox5.Text, dic[9], t, t, t, rule3_txtbox_4);
                    }
                    else if (getminmaxvalue(2, 3) == dic[7] && getminmaxvalue(1, 3) == dic[1])
                    {
                        sancirun(dic[1], t, t, dic[4], textBox5.Text, t, t, dic[7], t, rule3_txtbox_4);

                    }
                    else if (getminmaxvalue(2, 3) == dic[9] && getminmaxvalue(1, 3) == dic[3])
                    {
                        sancirun(t, t, dic[3], dic[4], textBox5.Text, dic[9], t, t, t, rule3_txtbox_4);
                    }
                    else if (getminmaxvalue(2, 3) == dic[7] && getminmaxvalue(1, 3) == dic[3])
                    {
                        sancirun(t, t, dic[3], dic[4], textBox5.Text, t, t, dic[7], t, rule3_txtbox_4);

                    }



                }).Start();


              



               

                //算法四结束

                #endregion

                status4 = true;

            }).Start();

            new System.Threading.Thread((System.Threading.ThreadStart)delegate {

                #region 算法五开始
              
                if (getminmaxvalue(1, 1) == dic[1])
                {
                    sancirun(dic[1], dic[2], t, dic[4], textBox5.Text, t, t, t, t, rule1_txtbox_5);
                }
                else if (getminmaxvalue(1, 1) == dic[3])
                {
                    sancirun(t, dic[2], dic[3], dic[4], textBox5.Text, t, t, t, t, rule1_txtbox_5);

                }

               
                if (getminmaxvalue(1, 3) == dic[1])
                {
                    sancirun(dic[1], dic[2], t, dic[4], textBox5.Text, t, t, t, t, rule2_txtbox_5);
                }
                else if (getminmaxvalue(1, 3) == dic[3])
                {
                    sancirun(t, dic[2], dic[3], dic[4], textBox5.Text, t, t, t, t, rule2_txtbox_5);

                }

               
                sancirun(dic[1], t, dic[3], dic[4], textBox5.Text, t, t, t, t, rule3_txtbox_5);

               

             

                //算法五结束

                #endregion

                status5 = true;

            }).Start();


            new System.Threading.Thread((System.Threading.ThreadStart)delegate {

                #region 算法六开始
              
                if (getminmaxvalue(1, 1) == dic[1])
                {
                    sancirun(dic[1], t, t, dic[4], textBox5.Text, t, t, t, dic[13], rule1_txtbox_6);
                }
                else if (getminmaxvalue(1, 1) == dic[3])
                {
                    sancirun(t, t, dic[3], dic[4], textBox5.Text, t, t, t, dic[13], rule1_txtbox_6);

                }

             

                sancirun(t, dic[2], t, dic[4], textBox5.Text, t, t, t, dic[13], rule2_txtbox_6);

             

                if (getminmaxvalue(1, 3) == dic[1])
                {
                    sancirun(dic[1], t, t, dic[4], textBox5.Text, t, t, t, dic[13], rule3_txtbox_6);
                }
                else if (getminmaxvalue(1, 3) == dic[3])
                {
                    sancirun(t, t, dic[3], dic[4], textBox5.Text, t, t, t, dic[13], rule3_txtbox_6);

                }

              


                //算法六结束

                #endregion
                status6 = true;

            }).Start();


            
        }
    

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           
            //三行
            sanci(); 


            //凯利指数
            listView2.Items.Clear();
            ListViewItem lv1 = listView2.Items.Add((listView2.Items.Count + 1).ToString()); //使用Listview展示数据
            ListViewItem lv2 = listView2.Items.Add((listView2.Items.Count + 1).ToString()); //使用Listview展示数据
            ListViewItem lv3 = listView2.Items.Add((listView2.Items.Count + 1).ToString()); //使用Listview展示数据
            kailizhishu(lv1, lv2, lv3, comboBox1.Text);
            kailizhishu(lv1, lv2, lv3, comboBox2.Text);
            kailizhishu(lv1, lv2, lv3, comboBox3.Text);
            kailizhishu(lv1, lv2, lv3, comboBox4.Text);
            kailizhishu(lv1, lv2, lv3, comboBox5.Text);


            lv1.SubItems.Add("##");
            lv2.SubItems.Add("##");
            lv3.SubItems.Add("##");



          

                                      
            function.kaili_jisuan(listView2);//凯利指数计算




            string id = Regex.Match(linkLabel1.Text.Trim(), @"\(([\s\S]*?)\)").Groups[1].Value;
            if (id == "")
            {
                id = Regex.Match(textBox6.Text.Trim(), @"\d{6,}").Groups[0].Value;
            }

            string data = function.teshujiance(id, comboBox1);
            string[] text = data.Split(new string[] { "#" }, StringSplitOptions.None);
            textBox3.Text = text[0]; //特殊数据检测
            textBox8.Text = text[1];  //三行运算


            get_data(comboBox1.Text.Trim());
            textBox1.Text = function.getdata_yarang(id);//获取亚让


           

           


           
                                             
            textBox4.Text= function.chazhi_mu(id); //母大差
            
            textBox7.Text = function.chazhi_zi(id);//子大差

            textBox9.Text = function.chazhi_jituan(id);//集团差
        }

        private void 软件6套9列_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            webBrowser1.Navigate("https://bf.titan007.com/football/Over_"+DateTime.Now.AddDays(-2).ToString("yyyyMMdd")+".htm");
        }

        private void button47_Click(object sender, EventArgs e)
        {
            
            

            textBox2.Text = function.paixuStr;//获取公司赔率时间排序



            GetControls_value(groupBox1);
            GetControls_value(groupBox3);
            GetControls_value(groupBox4);
            GetControls_value(groupBox5);
            GetControls_value(groupBox6);
            GetControls_value(groupBox18);

          

            Thread  thread = new Thread(allbutton);
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
            





        }

        public void allbutton()
        {
            dic = get_data(comboBox1.Text.Trim());
            if (dic.Count == 0)
            {
                dic.Add(1, "0");
                dic.Add(2, "0");
                dic.Add(3, "0");
                dic.Add(9, "0");
                dic.Add(8, "0");
                dic.Add(7, "0");
                dic.Add(4, "0");
                dic.Add(13, "0");
            }
            comb = comboBox1;
            status1 = false;
            status2 = false;
            status3 = false;
            status4 = false;
            status5 = false;
            status6 = false;
            run();

//公司2
            while(true)
            {
                if (status1 && status2 && status3 && status4  && status5 && status6 )
                {
                   
                    dic = get_data(comboBox2.Text.Trim());
                    if (dic.Count == 0)
                    {
                        dic.Add(1, "0");
                        dic.Add(2, "0");
                        dic.Add(3, "0");
                        dic.Add(9, "0");
                        dic.Add(8, "0");
                        dic.Add(7, "0");
                        dic.Add(4, "0");
                        dic.Add(13, "0");
                    }
                    comb = comboBox2;
                    status1 = false;
                    status2 = false;
                    status3 = false;
                    status4 = false;
                    status5 = false;
                    status6 = false;
                    run();
                    break;
                }
            }

            //公司3
            while (true)
            {
                if (status1 && status2 && status3 && status4 && status5 && status6)
                {

                    dic = get_data(comboBox3.Text.Trim());
                    if (dic.Count == 0)
                    {
                        dic.Add(1, "0");
                        dic.Add(2, "0");
                        dic.Add(3, "0");
                        dic.Add(9, "0");
                        dic.Add(8, "0");
                        dic.Add(7, "0");
                        dic.Add(4, "0");
                        dic.Add(13, "0");
                    }
                    comb = comboBox3;
                    status1 = false;
                    status2 = false;
                    status3 = false;
                    status4 = false;
                    status5 = false;
                    status6 = false;
                    run();
                    break;
                }
            }
            //公司4

            while (true)
            {
                if (status1 && status2 && status3 && status4 && status5 && status6)
                {

                    dic = get_data(comboBox4.Text.Trim());
                    if (dic.Count == 0)
                    {
                        dic.Add(1, "0");
                        dic.Add(2, "0");
                        dic.Add(3, "0");
                        dic.Add(9, "0");
                        dic.Add(8, "0");
                        dic.Add(7, "0");
                        dic.Add(4, "0");
                        dic.Add(13, "0");
                    }
                    comb = comboBox4;
                    status1 = false;
                    status2 = false;
                    status3 = false;
                    status4 = false;
                    status5 = false;
                    status6 = false;
                    run();
                    break;
                }
            }

            //公司5
            while (true)
            {
                if (status1 && status2 && status3 && status4 && status5 && status6)
                {

                    dic = get_data(comboBox5.Text.Trim());
                    if (dic.Count == 0)
                    {
                        dic.Add(1, "0");
                        dic.Add(2, "0");
                        dic.Add(3, "0");
                        dic.Add(9, "0");
                        dic.Add(8, "0");
                        dic.Add(7, "0");
                        dic.Add(4, "0");
                        dic.Add(13, "0");
                    }
                    comb = comboBox5;
                    status1 = false;
                    status2 = false;
                    status3 = false;
                    status4 = false;
                    status5 = false;
                    status6 = false;
                    run();
                   
                    break;
                   
                }
            }

          
        }



        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            webBrowser1.Navigate("https://live.titan007.com/oldIndexall.aspx");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //rebutton();
        }


    









        private void GetControls_value(Control fatherControl)
        {
            Control.ControlCollection sonControls = fatherControl.Controls;
            //遍历所有控件
            foreach (Control control in sonControls)
            {
                if (control is TextBox)
                {
                    control.Text = "";
                    

                }

                if (control.Controls != null)
                {
                    GetControls_value(control);
                }
            }
        }

        static string FindCommonDigits(string str1, string str2, string str3)
        {
          

           StringBuilder sb = new StringBuilder();  
            HashSet<char> uniqueDigits = new HashSet<char>();

            // 遍历第一个数字的每个字符
            foreach (char digit in str1)
            {
                if (!uniqueDigits.Contains(digit))
                {
                    uniqueDigits.Add(digit);
                    if (str2.Contains(digit) && str3.Contains(digit))
                    {
                        sb.Append(digit);
                    }
                }
            }

            return sb.ToString();
        }

        public void  sanhang_jisuan(TextBox t1, TextBox t2, TextBox t3)
        {
            try
            {


                string[] text1 = t1.Text.Split(new string[] { "  " }, StringSplitOptions.None); //两个空格
                string[] text2 = t2.Text.Split(new string[] { "  " }, StringSplitOptions.None);
                string[] text3 = t3.Text.Split(new string[] { "  " }, StringSplitOptions.None);
              


                int count = 5;
            

                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
              
                for (int i = 0; i < count; i++)
                {
                    string tong = FindCommonDigits(function.Find3Max(text1[i]), function.Find3Max(text2[i]), function.Find3Max(text3[i]));
                    if (tong!="")
                    {
                        lv1.SubItems.Add(tong);
                    }
                   else
                    {
                        lv1.SubItems.Add("-");
                    }
                   
                }
               

                lv1.SubItems.Add("##");
            }
            catch (Exception ex)
            {

              ex.ToString();
            }
        }
        /// <summary>
        /// 获取三行数据223值
        /// </summary>
        public void sanci()
        {
            listView1.Items.Clear();
            sanhang_jisuan(rule1_txtbox, rule2_txtbox, rule3_txtbox);
            sanhang_jisuan(rule1_txtbox_2, rule2_txtbox_2, rule3_txtbox_2);
            sanhang_jisuan(rule1_txtbox_3, rule2_txtbox_3, rule3_txtbox_3);

            sanhang_jisuan(rule1_txtbox_4, rule2_txtbox_4, rule3_txtbox_4);
            sanhang_jisuan(rule1_txtbox_5, rule2_txtbox_5, rule3_txtbox_5);
            sanhang_jisuan(rule1_txtbox_6, rule2_txtbox_6, rule3_txtbox_6);
        }

      

       

      

        public void kailizhishu(ListViewItem lv1, ListViewItem lv2, ListViewItem lv3,string com)
        {
            try
            {
                 
                string matchid = Regex.Match(linkLabel1.Text.Trim(), @"\(([\s\S]*?)\)").Groups[1].Value.Trim();
                if (matchid == "")
                {
                    matchid = Regex.Match(textBox6.Text.Trim(), @"\d{6,}").Groups[0].Value;
                }

              

                string com1data = function.getshishi_kailidata(matchid, com);

               
                
                if(com1data!="")
                {
                    string[] text = com1data.Split(new string[] { "," }, StringSplitOptions.None);
                    string hang1 = "";
                    string hang2 = "";
                    string hang3 = "";

                    hang1=function.CompareAndLabel(Convert.ToDouble(text[0]), Convert.ToDouble(text[1]), Convert.ToDouble(text[2])).ToString();

                    if (text[3].Trim() != "")
                    {
                        hang2 = function.CompareAndLabel(Convert.ToDouble(text[3]), Convert.ToDouble(text[4]), Convert.ToDouble(text[5])).ToString();
                    }
                    if (text[6].Trim() != "")
                    {

                        hang3 = function.CompareAndLabel(Convert.ToDouble(text[6]), Convert.ToDouble(text[7]), Convert.ToDouble(text[8])).ToString();
                    }

                    lv1.SubItems.Add(hang1);
                    lv2.SubItems.Add(hang2);
                    lv3.SubItems.Add(hang3);

                }
                else
                {
                    lv1.SubItems.Add("");
                    lv2.SubItems.Add("");
                    lv3.SubItems.Add("");


                }
                


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

       

        string shuzi = "";

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string shuzhi = Interaction.InputBox("提示信息", "", "0", -1, -1);
                listView1.SelectedItems[0].SubItems[6].Text = shuzhi;   
            }
        }

        private void listView2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView2.SelectedItems.Count > 0)
            {
                string shuzhi = Interaction.InputBox("提示信息", "", "0", -1, -1);
                listView2.SelectedItems[0].SubItems[6].Text = shuzhi;
            }
        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }
    }

}
