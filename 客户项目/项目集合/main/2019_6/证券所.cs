using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main._2019_6
{
    public partial class 证券所 : Form
    {
        public 证券所()
        {
            InitializeComponent();
        }

        private void 证券所_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        public void run()
        {
            
            try
            {


                string Url = "http://query.sse.com.cn//marketdata/tradedata/queryAllTradeOpenDate.do?jsonCallBack=jsonpCallback78551&token=QUERY&tradeDate=20190509";

                string html = method.gethtml(Url,"", "utf-8");

                textBox2.Text = html;

            }


            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }


        public void run1()
        {

            try
            {


                string Url = "http://www.szse.cn/api/report/ShowReport/data?SHOWTYPE=JSON&CATALOGID=1842_xxpl&TABKEY=tab1&txtStart=2019-06-05&txtEnd=2019-06-05";

                string html = method.GetUrl(Url, "utf-8");

                textBox2.Text = html;

            }


            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
        }
    }
}
