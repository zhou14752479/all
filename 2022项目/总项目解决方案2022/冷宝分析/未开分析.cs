using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 冷宝分析
{
    public partial class 未开分析 : Form
    {
        public 未开分析()
        {
            InitializeComponent();
        }

        public List<Animal> Animals_a
        {
            get;
            set;
        } = new List<Animal>();
        private void 未开分析_Load(object sender, EventArgs e)
        {
            string colorname = function.colorname;
            
            this.BackColor = Color.FromName(colorname);
            label1.BackColor = Color.FromName(colorname);
            label2.BackColor = Color.FromName(colorname);
            label3.BackColor = Color.FromName(colorname);
            label4.BackColor = Color.FromName(colorname);
            PanContent.BackColor = Color.FromName(colorname);



            label1.Text = string.Format("{0}截至{1:yyyy年M月d日}冷宝统计", function.softname, DateTime.Now);

           

            if (function.ExistINIFile(function.iniCountpath))
            {


                List<string> keylist = function.ReadKeys("场次统计", function.iniCountpath);
                List<string> daylist = function.ReadKeys("天数统计", function.iniDaypath);

                foreach (string key in keylist)
                {
                    
                    string value = function.IniReadValue("场次统计", key, function.iniCountpath);
                   Animal animal = new Animal();
                    animal.Name = key;
                    animal.WeiKaiChangShu = Convert.ToInt32(value);


                    string dayvalue = function.IniReadValue("天数统计", key, function.iniDaypath).Trim();
                    
                    if(dayvalue=="" || dayvalue==null)
                    {
                        dayvalue = "0";
                    }
                   
                    animal.WeiKaiTianShu = Convert.ToInt32(dayvalue);

                    Animals_a.Add(animal);
                    


                }   
            }


            Animals_a = (from i in Animals_a
                         orderby i.WeiKaiChangShu
                                    select i).ToList<Animal>();
            foreach (Animal animal in Animals_a)
            {
                UseLenBao useLenBao = new UseLenBao(animal);
                useLenBao.Dock = DockStyle.Top;
                useLenBao.Height = 20;
                useLenBao.Parent = this.PanContent;
            }


        }





    }
}
