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
using myDLL;

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
        public void chaxun(TextBox t1, TextBox t2, TextBox t3, TextBox t4, TextBox t5, ComboBox comb1, TextBox t9, TextBox t8, TextBox t7, TextBox t13, TextBox xianshi)
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


                if (t1.Text != "")
                {
                    sql = sql + (" data1 like '" + t1.Text.Trim() + "' and");
                }
                if (t2.Text != "")
                {
                    sql = sql + (" data2 like '" + t2.Text.Trim() + "' and");
                }
                if (t3.Text != "")
                {
                    sql = sql + (" data3 like '" + t3.Text.Trim() + "' and");
                }




                if (t9.Text != "")
                {
                    sql = sql + (" data4 like '" + t9.Text.Trim() + "' and");
                }
                if (t8.Text != "")
                {
                    sql = sql + (" data5 like '" + t8.Text.Trim() + "' and");
                }
                if (t7.Text != "")
                {
                    sql = sql + (" data6 like '" + t7.Text.Trim() + "' and");
                }



                if (t5.Text != "")
                {
                    sql = sql + (" rangqiudaxiaoqiu like '" + t5.Text.Trim() + "%' and");
                }
                //if (t6.Text != "")
                //{
                //    sql = sql + (" rangqiudaxiaoqiu like '%" + t6.Text.Trim() + "' and");
                //}


                if (comb1.Text != "")
                {
                    sql = sql + (" gongsi like '" + comb1.Text.Trim() + "' and");
                }

                if (sql.Substring(sql.Length - 3, 3) == "and")
                {
                    sql = sql.Substring(0, sql.Length - 3);
                }

               
                DataTable dt = fc.chaxundata(sql);


                //计算

                for (int i = 0; i < dt.Rows.Count - 1; i++)//如果DataGridView中有空的数据，则提示数据输入不完整并退出添加，不包括标题行
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

                     
                        string sjpshaixuan1 = t4.Text;
                        string sjpshaixuan2= t13.Text;
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

                xianshi.Text = xianshi.Text + l1 + " " + l2 + " " + l3 + "   ";

               
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.ToString());


            }
        }

        public string getminmaxvalue(int hang, int sort)
        {
            double a = Convert.ToDouble(textBox1.Text);
            double b = Convert.ToDouble(textBox3.Text);

            if (hang == 1)
            {


            }
            if (hang == 2)
            {
                a = Convert.ToDouble(textBox9.Text);
                b = Convert.ToDouble(textBox7.Text);

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

        public string getshishidata(string company)
        {
            string vvv = "";
            try
            {

             



                label48.Text = "";
                string id = Regex.Match(linkLabel1.Text.Trim(), @"\(([\s\S]*?)\)").Groups[1].Value;

                string data = function.getshishidata(id, company);
                // MessageBox.Show(data);
                string[] text = data.Split(new string[] { "," }, StringSplitOptions.None);
                if (text.Length > 6)
                {
                    string s1 = "平";
                    string j1 = "平";
                    string p1 = "平";

                    string s2 = "平";
                    string j2 = "平";
                    string p2 = "平";



                    textBox1.Text = text[1].Replace("\"", "");
                    textBox2.Text = text[2].Replace("\"", "");
                    textBox3.Text = text[3].Replace("\"", "");
                    textBox9.Text = text[4].Replace("\"", "");
                    textBox8.Text = text[5].Replace("\"", "");
                    textBox7.Text = text[6].Replace("\"", "");

                    vvv = text[1].Replace("\"", "") + "|"+ text[2].Replace("\"", "") + "|" +text[3].Replace("\"", "");

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




                    if (Convert.ToDouble(text[5]) - Convert.ToDouble(text[4]) == 0)
                    {
                        label48.Text = "特殊数据";

                    }



                    textBox4.Text = s1 + j1 + p1;
                    textBox13.Text = s2 + j2 + p2;
                }
                return vvv;
            }
            catch (Exception ex)
            {
                return vvv;
                // MessageBox.Show(ex.Message);
            }
        }
     
        public void sancirun(TextBox t1, TextBox t2, TextBox t3, TextBox t4, TextBox t5,  TextBox t9, TextBox t8, TextBox t7, TextBox t13, TextBox xianshi)
        {
            chaxun(t1, t2, t3, t4, t5, comboBox1, t9, t8, t7, t13, xianshi);

          label32.Text=  getshishidata(comboBox2.Text.Trim());
            chaxun(t1, t2, t3, t4, t5, comboBox2,  t9, t8, t7, t13, xianshi);

            label33.Text = getshishidata(comboBox3.Text.Trim());
            chaxun(t1, t2, t3, t4, t5, comboBox3,  t9, t8, t7, t13, xianshi);

            label34.Text = getshishidata(comboBox4.Text.Trim());
            chaxun(t1, t2, t3, t4, t5, comboBox4, t9, t8, t7, t13, xianshi);

            label35.Text = getshishidata(comboBox5.Text.Trim());
            chaxun(t1, t2, t3, t4, t5, comboBox5, t9, t8, t7, t13, xianshi);

            label31.Text = getshishidata(comboBox1.Text.Trim());

        }
        TextBox t = new TextBox();
        private void button1_Click(object sender, EventArgs e)
        {
            rule1_txtbox.Text = "";
            t.Text = "";
            if (getminmaxvalue(1, 1) == textBox1.Text.Trim())
            {
                sancirun(textBox1, t, t, textBox4, textBox5, t, t, t, t, rule1_txtbox);
            }
            if (getminmaxvalue(1, 1) == textBox3.Text.Trim())
            {
                sancirun(t, t, textBox3, textBox4, textBox5, t, t, t, t, rule1_txtbox);

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            rule2_txtbox.Text = "";
            t.Text = "";
            if (getminmaxvalue(2, 1) == textBox9.Text.Trim())
            {
                sancirun(t, t, t,t , textBox5, textBox9, t, t, textBox13, rule2_txtbox);
            }
            if (getminmaxvalue(2, 1) == textBox7.Text.Trim())
            {
                sancirun(t, t, t, t, textBox5, t, t, textBox7, textBox13, rule2_txtbox);
            }
          

        }

        private void button6_Click(object sender, EventArgs e)
        {
            rule3_txtbox.Text = "";
            t.Text = "";
            if (getminmaxvalue(2, 1) == textBox9.Text.Trim())
            {
                sancirun(t, t, t, textBox4, textBox5, textBox9, t, t, textBox13, rule3_txtbox);
            }
            if (getminmaxvalue(2, 1) == textBox7.Text.Trim())
            {
                sancirun(t, t, t, textBox4, textBox5, t, t, textBox7, textBox13, rule3_txtbox);
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            rule4_txtbox.Text = "";
            t.Text = "";
            if (getminmaxvalue(2, 1) == textBox9.Text.Trim() && getminmaxvalue(1, 1) == textBox1.Text.Trim())
            {
                sancirun(textBox1, t, t, textBox4, textBox5, textBox9, t, t, textBox13, rule4_txtbox);
            }
            if (getminmaxvalue(2, 1) == textBox7.Text.Trim() && getminmaxvalue(1, 1) == textBox1.Text.Trim())
            {
                sancirun(textBox1, t, t, textBox4, textBox5, t, t, textBox7, textBox13, rule4_txtbox);
            }
            if (getminmaxvalue(2, 1) == textBox9.Text.Trim() && getminmaxvalue(1, 1) == textBox3.Text.Trim())
            {
                sancirun(t, t, textBox3, textBox4, textBox5, textBox9, t, t, textBox13, rule4_txtbox);
            }
            if (getminmaxvalue(2, 1) == textBox7.Text.Trim() && getminmaxvalue(1, 1) == textBox3.Text.Trim())
            {
                sancirun(t, t, textBox3, textBox4, textBox5, t, t, textBox7, textBox13, rule4_txtbox);
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            rule5_txtbox.Text = "";
            t.Text = "";
            if (getminmaxvalue(2, 1) == textBox9.Text.Trim())
            {
                sancirun(t, textBox2, t, textBox4, textBox5, textBox9, t, t, textBox13, rule5_txtbox);
            }
            if (getminmaxvalue(2, 1) == textBox7.Text.Trim())
            {
                sancirun(t, textBox2, t, textBox4, textBox5, t, t, textBox7, textBox13, rule5_txtbox);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            rule6_txtbox.Text = "";
            t.Text = "";
            if (getminmaxvalue(2, 1) == textBox9.Text.Trim() && getminmaxvalue(1, 3) == textBox1.Text.Trim())
            {
                sancirun(textBox1, t, t, textBox4, textBox5, textBox9, t, t, textBox13, rule6_txtbox);
            }
            if (getminmaxvalue(2, 1) == textBox7.Text.Trim() && getminmaxvalue(1, 3) == textBox1.Text.Trim())
            {
                sancirun(textBox1, t, t, textBox4, textBox5, t, t, textBox7, textBox13, rule6_txtbox);
            }
            if (getminmaxvalue(2, 1) == textBox9.Text.Trim() && getminmaxvalue(1,3) == textBox3.Text.Trim())
            {
                sancirun(t, t, textBox3, textBox4, textBox5, textBox9, t, t, textBox13, rule6_txtbox);
            }
            if (getminmaxvalue(2, 1) == textBox7.Text.Trim() && getminmaxvalue(1, 3) == textBox3.Text.Trim())
            {
                sancirun(t, t, textBox3, textBox4, textBox5, t, t, textBox7, textBox13, rule6_txtbox);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            rule7_txtbox.Text = "";
            t.Text = "";
            if (getminmaxvalue(2, 1) == textBox9.Text.Trim() && getminmaxvalue(1, 1) == textBox1.Text.Trim())
            {
                sancirun(textBox1, t, t, t, textBox5, textBox9, t, t, textBox13, rule7_txtbox);
            }
            if (getminmaxvalue(2, 1) == textBox7.Text.Trim() && getminmaxvalue(1, 1) == textBox1.Text.Trim())
            {
                sancirun(textBox1, t, t, t, textBox5, t, t, textBox7, textBox13, rule7_txtbox);
            }
            if (getminmaxvalue(2, 1) == textBox9.Text.Trim() && getminmaxvalue(1, 1) == textBox3.Text.Trim())
            {
                sancirun(t, t, textBox3, t, textBox5, textBox9, t, t, textBox13, rule7_txtbox);
            }
            if (getminmaxvalue(2, 1) == textBox7.Text.Trim() && getminmaxvalue(1, 1) == textBox3.Text.Trim())
            {
                sancirun(t, t, textBox3, t, textBox5, t, t, textBox7, textBox13, rule7_txtbox);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            rule8_txtbox.Text = "";
            t.Text = "";
            if (getminmaxvalue(2, 1) == textBox9.Text.Trim())
            {
                sancirun(t, textBox2, t, t, textBox5, textBox9, t, t, textBox13, rule8_txtbox);
            }
            if (getminmaxvalue(2, 1) == textBox7.Text.Trim())
            {
                sancirun(t, textBox2, t, t, textBox5, t, t, textBox7, textBox13, rule8_txtbox);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            rule9_txtbox.Text = "";
            t.Text = "";
            if (getminmaxvalue(2, 1) == textBox9.Text.Trim() && getminmaxvalue(1, 3) == textBox1.Text.Trim())
            {
                sancirun(textBox1, t, t, t, textBox5, textBox9, t, t, textBox13, rule9_txtbox);
            }
            if (getminmaxvalue(2, 1) == textBox7.Text.Trim() && getminmaxvalue(1, 3) == textBox1.Text.Trim())
            {
                sancirun(textBox1, t, t, t, textBox5, t, t, textBox7, textBox13, rule9_txtbox);
            }
            if (getminmaxvalue(2, 1) == textBox9.Text.Trim() && getminmaxvalue(1, 3) == textBox3.Text.Trim())
            {
                sancirun(t, t, textBox3, t, textBox5, textBox9, t, t, textBox13, rule9_txtbox);
            }
            if (getminmaxvalue(2, 1) == textBox7.Text.Trim() && getminmaxvalue(1, 3) == textBox3.Text.Trim())
            {
                sancirun(t, t, textBox3, t, textBox5, t, t, textBox7, textBox13, rule9_txtbox);
            }
        }










        private void button16_Click(object sender, EventArgs e)
        {
            rule1_txtbox_2.Text = "";
            t.Text = "";
            sancirun(t, textBox2, t, textBox4, textBox5, t, t, t, t, rule1_txtbox_2);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            rule2_txtbox_2.Text = "";
            t.Text = "";
            sancirun(t, t, t, t, textBox5, t, textBox8, t, textBox13, rule2_txtbox_2);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            rule3_txtbox_2.Text = "";
            t.Text = "";
            sancirun(t, t, t, textBox4, textBox5, t, textBox8, t, textBox13, rule3_txtbox_2);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            rule4_txtbox_2.Text = "";
            t.Text = "";
            if (getminmaxvalue(1, 1) == textBox1.Text.Trim())
            {
                sancirun(textBox1, t, t, textBox4, textBox5, t, textBox8, t, textBox13, rule4_txtbox_2);
            }
            if (getminmaxvalue(1, 1) == textBox3.Text.Trim())
            {
                sancirun(t, t, textBox3, textBox4, textBox5, t, textBox8, t, textBox13, rule4_txtbox_2);

            }
            
        }

        private void button12_Click(object sender, EventArgs e)
        {
            rule5_txtbox_2.Text = "";
            t.Text = "";
            sancirun(t, textBox2, t, textBox4, textBox5, t, textBox8, t, textBox13, rule5_txtbox_2);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            rule6_txtbox_2.Text = "";
            t.Text = "";
            if (getminmaxvalue(1, 3) == textBox1.Text.Trim())
            {
                sancirun(textBox1, t, t, textBox4, textBox5, t, textBox8, t, textBox13, rule6_txtbox_2);
            }
            if (getminmaxvalue(1, 3) == textBox3.Text.Trim())
            {
                sancirun(t, t, textBox3, textBox4, textBox5, t, textBox8, t, textBox13, rule6_txtbox_2);

            }
        }

        private void button26_Click(object sender, EventArgs e)
        {
            rule7_txtbox_2.Text = "";
            t.Text = "";
            if (getminmaxvalue(1, 1) == textBox1.Text.Trim())
            {
                sancirun(textBox1, t, t, t, textBox5, t, textBox8, t, textBox13, rule7_txtbox_2);
            }
            if (getminmaxvalue(1, 1) == textBox3.Text.Trim())
            {
                sancirun(t, t, textBox3, t, textBox5, t, textBox8, t, textBox13, rule7_txtbox_2);

            }
        }

        private void button27_Click(object sender, EventArgs e)
        {
            rule8_txtbox_2.Text = "";
            t.Text = "";
            sancirun(t, textBox2, t, t, textBox5, t, textBox8, t, textBox13, rule8_txtbox_2);
        }

        private void button28_Click(object sender, EventArgs e)
        {
            rule9_txtbox_2.Text = "";
            t.Text = "";
            if (getminmaxvalue(1, 3) == textBox1.Text.Trim())
            {
                sancirun(textBox1, t, t, t, textBox5, t, textBox8, t, textBox13, rule9_txtbox_2);
            }
            if (getminmaxvalue(1, 3) == textBox3.Text.Trim())
            {
                sancirun(t, t, textBox3, t, textBox5, t, textBox8, t, textBox13, rule9_txtbox_2);

            }
        }





        private void button25_Click(object sender, EventArgs e)
        {
            rule1_txtbox_3.Text = "";
            t.Text = "";
            if (getminmaxvalue(1, 3) == textBox1.Text.Trim())
            {
                sancirun(textBox1, t, t, textBox4, textBox5, t, t, t, t, rule1_txtbox_3);
            }
            if (getminmaxvalue(1, 3) == textBox3.Text.Trim())
            {
                sancirun(t, t, textBox3, textBox4, textBox5, t, t, t, t, rule1_txtbox_3);

            }
        }

        private void button24_Click(object sender, EventArgs e)
        {
            rule2_txtbox_3.Text = "";
            t.Text = "";
            if (getminmaxvalue(2, 3) == textBox9.Text.Trim())
            {
                sancirun(t, t, t, t, textBox5, textBox9, t, t, textBox13, rule2_txtbox_3);
            }
            if (getminmaxvalue(2, 3) == textBox7.Text.Trim())
            {
                sancirun(t, t, t, t, textBox5, t, t, textBox7, textBox13, rule2_txtbox_3);

            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            rule3_txtbox_3.Text = "";
            t.Text = "";
            if (getminmaxvalue(2, 3) == textBox9.Text.Trim())
            {
                sancirun(t, t, t, textBox4, textBox5, textBox9, t, t, textBox13, rule3_txtbox_3);
            }
            if (getminmaxvalue(2, 3) == textBox7.Text.Trim())
            {
                sancirun(t, t, t, textBox4, textBox5, t, t, textBox7, textBox13, rule3_txtbox_3);

            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            rule4_txtbox_3.Text = "";
            t.Text = "";
            if (getminmaxvalue(2, 3) == textBox9.Text.Trim() && getminmaxvalue(1, 1) == textBox1.Text.Trim())
            {
                sancirun(textBox1, t, t, textBox4, textBox5, textBox9, t, t, textBox13, rule4_txtbox_3);
            }
            if (getminmaxvalue(2, 3) == textBox7.Text.Trim() && getminmaxvalue(1, 1) == textBox1.Text.Trim())
            {
                sancirun(textBox1, t, t, textBox4, textBox5, t, t, textBox7, textBox13, rule4_txtbox_3);

            }

            if (getminmaxvalue(2, 3) == textBox9.Text.Trim() && getminmaxvalue(1, 1) == textBox3.Text.Trim())
            {
                sancirun(t, t, textBox3, textBox4, textBox5, textBox9, t, t, textBox13, rule4_txtbox_3);
            }
            if (getminmaxvalue(2, 3) == textBox7.Text.Trim() && getminmaxvalue(1, 1) == textBox3.Text.Trim())
            {
                sancirun(t, t, textBox3, textBox4, textBox5, t, t, textBox7, textBox13, rule4_txtbox_3);

            }

        }

        private void button21_Click(object sender, EventArgs e)
        {
            rule5_txtbox_3.Text = "";
            t.Text = "";
            if (getminmaxvalue(2, 3) == textBox9.Text.Trim())
            {
                sancirun(t, textBox2, t, textBox4, textBox5, textBox9, t, t, textBox13, rule5_txtbox_3);
            }
            if (getminmaxvalue(2, 3) == textBox7.Text.Trim())
            {
                sancirun(t, textBox2, t, textBox4, textBox5, t, t, textBox7, textBox13, rule5_txtbox_3);

            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            rule6_txtbox_3.Text = "";
            t.Text = "";
            if (getminmaxvalue(2, 3) == textBox9.Text.Trim() && getminmaxvalue(1, 3) == textBox1.Text.Trim())
            {
                sancirun(textBox1, t, t, textBox4, textBox5, textBox9, t, t, textBox13, rule6_txtbox_3);
            }
            if (getminmaxvalue(2, 3) == textBox7.Text.Trim() && getminmaxvalue(1, 3) == textBox1.Text.Trim())
            {
                sancirun(textBox1, t, t, textBox4, textBox5, t, t, textBox7, textBox13, rule6_txtbox_3);

            }

            if (getminmaxvalue(2, 3) == textBox9.Text.Trim() && getminmaxvalue(1, 3) == textBox3.Text.Trim())
            {
                sancirun(t, t, textBox3, textBox4, textBox5, textBox9, t, t, textBox13, rule6_txtbox_3);
            }
            if (getminmaxvalue(2, 3) == textBox7.Text.Trim() && getminmaxvalue(1, 3) == textBox3.Text.Trim())
            {
                sancirun(t, t, textBox3, textBox4, textBox5, t, t, textBox7, textBox13, rule6_txtbox_3);

            }
        }

        private void button29_Click(object sender, EventArgs e)
        {
            rule7_txtbox_3.Text = "";
            t.Text = "";
            if (getminmaxvalue(2, 3) == textBox9.Text.Trim() && getminmaxvalue(1, 1) == textBox1.Text.Trim())
            {
                sancirun(textBox1, t, t, t, textBox5, textBox9, t, t, textBox13, rule7_txtbox_3);
            }
            if (getminmaxvalue(2, 3) == textBox7.Text.Trim() && getminmaxvalue(1, 1) == textBox1.Text.Trim())
            {
                sancirun(textBox1, t, t, t, textBox5, t, t, textBox7, textBox13, rule7_txtbox_3);

            }

            if (getminmaxvalue(2, 3) == textBox9.Text.Trim() && getminmaxvalue(1, 1) == textBox3.Text.Trim())
            {
                sancirun(t, t, textBox3, t, textBox5, textBox9, t, t, textBox13, rule7_txtbox_3);
            }
            if (getminmaxvalue(2, 3) == textBox7.Text.Trim() && getminmaxvalue(1, 1) == textBox3.Text.Trim())
            {
                sancirun(t, t, textBox3, t, textBox5, t, t, textBox7, textBox13, rule7_txtbox_3);

            }
        }

        private void button30_Click(object sender, EventArgs e)
        {
            rule8_txtbox_3.Text = "";
            t.Text = "";
            if (getminmaxvalue(2, 3) == textBox9.Text.Trim())
            {
                sancirun(t, textBox2, t, t, textBox5, textBox9, t, t, textBox13, rule8_txtbox_3);
            }
            if (getminmaxvalue(2, 3) == textBox7.Text.Trim())
            {
                sancirun(t, textBox2, t, t, textBox5, t, t, textBox7, textBox13, rule8_txtbox_3);

            }
        }

        private void button31_Click(object sender, EventArgs e)
        {
            rule9_txtbox_3.Text = "";
            t.Text = "";
            if (getminmaxvalue(2, 3) == textBox9.Text.Trim() && getminmaxvalue(1, 3) == textBox1.Text.Trim())
            {
                sancirun(textBox1, t, t, t, textBox5, textBox9, t, t, textBox13, rule9_txtbox_3);
            }
            if (getminmaxvalue(2, 3) == textBox7.Text.Trim() && getminmaxvalue(1, 3) == textBox1.Text.Trim())
            {
                sancirun(textBox1, t, t, t, textBox5, t, t, textBox7, textBox13, rule9_txtbox_3);

            }

            if (getminmaxvalue(2, 3) == textBox9.Text.Trim() && getminmaxvalue(1, 3) == textBox3.Text.Trim())
            {
                sancirun(t, t, textBox3, t, textBox5, textBox9, t, t, textBox13, rule9_txtbox_3);
            }
            if (getminmaxvalue(2, 3) == textBox7.Text.Trim() && getminmaxvalue(1, 3) == textBox3.Text.Trim())
            {
                sancirun(t, t, textBox3, t, textBox5, t, t, textBox7, textBox13, rule9_txtbox_3);

            }
        }





        private void button46_Click(object sender, EventArgs e)
        {
            rule1_txtbox_4.Text = "";
            t.Text = "";
            if (getminmaxvalue(2, 3) == textBox9.Text.Trim())
            {
                sancirun(t, t, t, t, textBox5, textBox9, t, t, t, rule1_txtbox_4);
            }
            if (getminmaxvalue(2, 3) == textBox7.Text.Trim())
            {
                sancirun(t, t, t, t, textBox5, t, t, textBox7, t, rule1_txtbox_4);

            }
        }

        private void button45_Click(object sender, EventArgs e)
        {
            rule2_txtbox_4.Text = "";
            t.Text = "";
            if (getminmaxvalue(2, 3) == textBox9.Text.Trim())
            {
                sancirun(t, t, t, textBox4, textBox5, textBox9, t, t, t, rule2_txtbox_4);
            }
            if (getminmaxvalue(2, 3) == textBox7.Text.Trim())
            {
                sancirun(t, t, t, textBox4, textBox5, t, t, textBox7, t, rule2_txtbox_4);

            }
        }

        private void button44_Click(object sender, EventArgs e)
        {
            rule3_txtbox_4.Text = "";
            t.Text = "";
            if (getminmaxvalue(2, 3) == textBox9.Text.Trim() && getminmaxvalue(1, 3) == textBox1.Text.Trim())
            {
                sancirun(textBox1, t, t, textBox4, textBox5, textBox9, t, t, t, rule3_txtbox_4);
            }
            if (getminmaxvalue(2, 3) == textBox7.Text.Trim() && getminmaxvalue(1, 3) == textBox1.Text.Trim())
            {
                sancirun(textBox1, t, t, textBox4, textBox5, t, t, textBox7, t, rule3_txtbox_4);

            }
            if (getminmaxvalue(2, 3) == textBox9.Text.Trim() && getminmaxvalue(1, 3) == textBox3.Text.Trim())
            {
                sancirun(t, t, textBox3, textBox4, textBox5, textBox9, t, t, t, rule3_txtbox_4);
            }
            if (getminmaxvalue(2, 3) == textBox7.Text.Trim() && getminmaxvalue(1, 3) == textBox3.Text.Trim())
            {
                sancirun(t, t, textBox3, textBox4, textBox5, t, t, textBox7, t, rule3_txtbox_4);

            }

        }

        private void button43_Click(object sender, EventArgs e)
        {
            rule4_txtbox_4.Text = "";
            t.Text = "";
            if (getminmaxvalue(2, 3) == textBox9.Text.Trim())
            {
                sancirun(t, textBox2, t, textBox4, textBox5, textBox9, t, t, t, rule4_txtbox_4);
            }
            if (getminmaxvalue(2, 3) == textBox7.Text.Trim())
            {
                sancirun(t, textBox2, t, textBox4, textBox5, t, t, textBox7, t, rule4_txtbox_4);

            }
        }

        private void button42_Click(object sender, EventArgs e)
        {
            rule5_txtbox_4.Text = "";
            t.Text = "";
            sancirun(t, t, t, textBox4, textBox5, t, textBox8, t, t, rule5_txtbox_4);
        }

        private void button41_Click(object sender, EventArgs e)
        {
            rule6_txtbox_4.Text = "";
            t.Text = "";
            if (getminmaxvalue(1, 3) == textBox1.Text.Trim())
            {
                sancirun(textBox1, t, t, textBox4, textBox5, t, textBox8, t, t, rule6_txtbox_4);

            }
            if (getminmaxvalue(1, 3) == textBox3.Text.Trim())
            {
                sancirun(t, t, textBox3, textBox4, textBox5, t, textBox8, t, t, rule6_txtbox_4);

            }
        }





        private void button40_Click(object sender, EventArgs e)
        {
            rule1_txtbox_5.Text = "";
            t.Text = "";
            if (getminmaxvalue(1, 1) == textBox1.Text.Trim())
            {
                sancirun(textBox1, textBox2, t, textBox4, textBox5, t, t, t, t, rule1_txtbox_5);
            }
            if (getminmaxvalue(1, 1) == textBox3.Text.Trim())
            {
                sancirun(t, textBox2, textBox3, textBox4, textBox5, t, t, t, t, rule1_txtbox_5);

            }
        }

        private void button39_Click(object sender, EventArgs e)
        {
            rule2_txtbox_5.Text = "";
            t.Text = "";
            if (getminmaxvalue(1, 3) == textBox1.Text.Trim())
            {
                sancirun(textBox1, textBox2, t, textBox4, textBox5, t, t, t, t, rule2_txtbox_5);
            }
            if (getminmaxvalue(1, 3) == textBox3.Text.Trim())
            {
                sancirun(t, textBox2, textBox3, textBox4, textBox5, t, t, t, t, rule2_txtbox_5);

            }
        }

        private void button38_Click(object sender, EventArgs e)
        {
            rule3_txtbox_5.Text = "";
            t.Text = "";
            sancirun(textBox1, t, textBox3, textBox4, textBox5, t, t, t, t, rule3_txtbox_5);
        }

        private void button37_Click(object sender, EventArgs e)
        {
            rule4_txtbox_5.Text = "";
            t.Text = "";
            if (getminmaxvalue(1, 1) == textBox1.Text.Trim())
            {
                sancirun(textBox1, textBox2, t, textBox4, textBox5, t, t, t, textBox13, rule4_txtbox_5);
            }
            if (getminmaxvalue(1, 1) == textBox3.Text.Trim())
            {
                sancirun(t, textBox2, textBox3, textBox4, textBox5, t, t, t, textBox13, rule4_txtbox_5);

            }
        }

        private void button36_Click(object sender, EventArgs e)
        {
            rule5_txtbox_5.Text = "";
            t.Text = "";
            if (getminmaxvalue(1, 3) == textBox1.Text.Trim())
            {
                sancirun(textBox1, textBox2, t, textBox4, textBox5, t, t, t, textBox13, rule5_txtbox_5);
            }
            if (getminmaxvalue(1, 3) == textBox3.Text.Trim())
            {
                sancirun(t, textBox2, textBox3, textBox4, textBox5, t, t, t, textBox13, rule5_txtbox_5);

            }
        }

        private void button35_Click(object sender, EventArgs e)
        {
            rule6_txtbox_5.Text = "";
            t.Text = "";
            sancirun(textBox1, t, textBox3, textBox4, textBox5, t, t, t, textBox13, rule6_txtbox_5);
        }




        private void button34_Click(object sender, EventArgs e)
        {
            rule1_txtbox_6.Text = "";
            t.Text = "";
            if (getminmaxvalue(1, 1) == textBox1.Text.Trim())
            {
                sancirun(textBox1, t, t, textBox4, textBox5, t, t, t, textBox13, rule1_txtbox_6);
            }
            if (getminmaxvalue(1, 1) == textBox3.Text.Trim())
            {
                sancirun(t, t, textBox3, textBox4, textBox5, t, t, t, textBox13, rule1_txtbox_6);

            }
        }

        private void button33_Click(object sender, EventArgs e)
        {
            rule2_txtbox_6.Text = "";
            t.Text = "";
            sancirun(t, textBox2, t, textBox4, textBox5, t, t, t, textBox13, rule2_txtbox_6);
        }

        private void button32_Click(object sender, EventArgs e)
        {
            rule3_txtbox_6.Text = "";
            t.Text = "";
            if (getminmaxvalue(1, 3) == textBox1.Text.Trim())
            {
                sancirun(textBox1, t, t, textBox4, textBox5, t, t, t, textBox13, rule3_txtbox_6);
            }
            if (getminmaxvalue(1, 3) == textBox3.Text.Trim())
            {
                sancirun(t, t, textBox3, textBox4, textBox5, t, t, t, textBox13, rule3_txtbox_6);

            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            rule4_txtbox_6.Text = "";
            t.Text = "";
            sancirun(t, t, t, textBox4, textBox5, t, t, t, textBox13, rule4_txtbox_6);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            rule5_txtbox_6.Text = "";
            t.Text = "";
            sancirun(textBox1, textBox2, textBox3, textBox4, textBox5, t, t, t, t, rule5_txtbox_6);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            rule6_txtbox_6.Text = "";
            t.Text = "";
            sancirun(textBox1, textBox2, textBox3, t, textBox5, t, t, t, t, rule6_txtbox_6);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            getshishidata(comboBox1.Text.Trim());
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
            webBrowser1.Navigate("https://bf.titan007.com/football/Over_20231127.htm");
        }

        private void button47_Click(object sender, EventArgs e)
        {
           Thread thread = new Thread(allbutton);
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        public void allbutton()
        {
            foreach (Control item in groupBox2.Controls)
            {
                if (item is Button)
                {
                    if (item.Text != "全部运算")
                    {
                        Button button = (Button)item;
                        button.PerformClick();
                    }

                }

            }
        }


    }

}
