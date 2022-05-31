using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 冷宝分析
{
    public partial class 场次管理 : Form
    {
        public 场次管理()
        {
            InitializeComponent();
        }

        private void 场次管理_Load(object sender, EventArgs e)
        {
            ArrayList animals = function.getanimals();
            comboBox1.DataSource = animals;
            comboBox4.DataSource = animals;
        }


        private void BtnAddZiDingYiChangCi_Click(object sender, EventArgs e)
        {

            string animal = comboBox1.Text.ToString();
            string changci = comboBox2.Text.ToString();
            function.IniWriteValue(DatChangCiTime.Value.ToString("MM月dd日"), changci, animal, function.inipath);


            if (function.ExistINIFile(function.iniCountpath))
            {

                List<string> keylist = function.ReadKeys("场次统计", function.iniCountpath);
                foreach (string key in keylist)
                {

                    string value = function.IniReadValue("场次统计", key, function.iniCountpath);
                    value = (Convert.ToInt32(value) + 1).ToString();
                    function.IniWriteValue("场次统计", key, value, function.iniCountpath);
                }

                if (DatChangCiTime.Value.ToString("yyyy年MM月dd日") == DateTime.Now.ToString("yyyy年MM月dd日"))
                {
                    string weikai = function.IniReadValue("场次统计", animal, function.iniCountpath);
                    function.IniWriteValue("场次统计", animal, "0", function.iniCountpath);
                }
            }
            else
            {


                foreach (string key in comboBox1.Items)
                {
                    function.IniWriteValue("场次统计", key, "1", function.iniCountpath);
                }
                if (DatChangCiTime.Value.ToString("yyyy年MM月dd日") == DateTime.Now.ToString("yyyy年MM月dd日"))
                {
                    function.IniWriteValue("场次统计", animal, "0", function.iniCountpath);
                }
            }





            //更新天数
            if (function.ExistINIFile(function.iniDaypath))
            {
                if (!function.haveAddDayList.Contains(DatChangCiTime.Value.ToString("MM月dd日")))
                {
                    function.haveAddDayList.Add(DatChangCiTime.Value.ToString("MM月dd日"));
                    List<string> keylist = function.ReadKeys("天数统计", function.iniDaypath);
                    foreach (string key in keylist)
                    {

                        string value = function.IniReadValue("天数统计", key, function.iniDaypath);
                        value = (Convert.ToInt32(value) + 1).ToString();
                        function.IniWriteValue("天数统计", key, value, function.iniDaypath);
                    }
     
                }


                if (DatChangCiTime.Value.ToString("yyyy年MM月dd日") == DateTime.Now.ToString("yyyy年MM月dd日"))
                {
                    string weikai = function.IniReadValue("天数统计", animal, function.iniDaypath);
                    function.IniWriteValue("天数统计", animal, "0", function.iniDaypath);
                }
            }

            else
            {

                function.haveAddDayList.Add(DatChangCiTime.Value.ToString("MM月dd日"));
                foreach (string key in comboBox1.Items)
                {
                    function.IniWriteValue("天数统计", key, "1", function.iniDaypath);
                }
                if (DatChangCiTime.Value.ToString("yyyy年MM月dd日") == DateTime.Now.ToString("yyyy年MM月dd日"))
                {
                    function.IniWriteValue("天数统计", animal, "0", function.iniDaypath);
                }
            }


            MessageBox.Show("添加场次成功");



        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要清空吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                File.Delete(function.iniCountpath);
                File.Delete(function.iniDaypath);
            }
            else
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要清空吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                File.Delete(function.inipath);
            }
            else
            {

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string animal = comboBox4.Text.ToString();
            string changci = textBox1.Text;
            string tianshu = textBox2.Text;

            function.IniWriteValue("场次统计", animal, changci, function.iniCountpath);
            function.IniWriteValue("天数统计", animal, tianshu, function.iniDaypath);
            MessageBox.Show("添加场次成功");
        }
    }
}
