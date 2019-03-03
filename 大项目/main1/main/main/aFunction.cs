using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main
{
    class aFunction
    {


        pubVariables pub = new pubVariables();

        public System.Windows.Forms.Timer tm = new System.Windows.Forms.Timer();
        

        AutoResetEvent autoEvent = new AutoResetEvent(false);

        public  void tm_Tick(object sender, EventArgs e)
        {
            autoEvent.Set(); //通知阻塞的线程继续执行
        }

      



        /// <summary>
        /// 58二手房
        /// </summary>
        /// <param name="dgv1"></param>
        public void ershoufang(object dgv1)
        {
            
            DataGridView dgv = (DataGridView)dgv1;

            try
            {
                string[] headers = { "标题", "联系人", "电话", "地区", "小区", "面积", "价格", "网址" };

                method.setDatagridview(dgv, 8, headers);

                
               method.CreateTable("58同城房源", headers); //创建数据表

                string[] citys = pubVariables.citys.Split(new string[] { "," }, StringSplitOptions.None);

                foreach (string cityname in citys)
                {
                    if (cityname == "")
                    {
                        MessageBox.Show("请选择城市！");
                        return;
                    }

                    string city = method.Get58pinyin(cityname);

                    for (int i = 1; i < 71; i++)
                    {
                        String Url = "http://" + city + ".58.com/ershoufang/"+pubVariables.fangFrom+"/pn" + i + "/";

                        string html = method.GetUrl(Url);

                        
                       MatchCollection TitleMatchs = Regex.Matches(html, @"<li logr=""([\s\S]*?)_([\s\S]*?)_([\s\S]*?)_([\s\S]*?)_", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        ArrayList lists = new ArrayList();
                        foreach (Match NextMatch in TitleMatchs)
                        {
                            lists.Add("http://" + city + ".58.com/ershoufang/" + NextMatch.Groups[4].Value + "x.shtml");

                        }

                        if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                            break;





                        foreach (string list in lists)

                        {
                            

                            if (pubVariables.status == false)

                                return;
                           
                            //  autoEvent.WaitOne();  //阻塞当前线程，等待通知以继续执行


                            pub.index= dgv.Rows.Add();
                           
                            String Url1 = list;

                            string Url2 = "http://m.58.com/" + city + "/ershoufang/" + Url1.Substring(Url1.Length - 21);                       //获取二手房手机端的网址

                            string strhtml = method.GetUrl(Url1);                                                                                //定义的GetRul方法 返回 reader.ReadToEnd()

                            string strhtml2 = method.GetUrl(Url2);                                                                               //请求手机端网址

                            string title = @"<h1 class=""c_333 f20"">([\s\S]*?)</h1>";  //标题
                            string Rxg = @"<h2 class=""agent-title"">([\s\S]*?)</h2>";  //手机端正则匹配联系人
                            string Rxg1 = @"<p class='phone-num'>([\s\S]*?)<";   //电话
                            string Rxg2 = @"<li class=""address-info"">([\s\S]*?) -";//手机端地区
                            string Rxg3 = @"小区：([\s\S]*?)</h2>";//手机端小区
                            string Rxg4 = @"面积</p>([\s\S]*?)</p>"; //手机端面积去除标签
                            string Rxg5 = @"售价</p>([\s\S]*?)</p>"; //手机端售价去除标签



                            Match titles = Regex.Match(strhtml, title);
                            Match contacts = Regex.Match(strhtml2, Rxg);                                                        //手机端正则匹配联系人
                            Match tell = Regex.Match(strhtml, Rxg1);
                            Match area = Regex.Match(strhtml2, Rxg2);

                            Match xiaoqu = Regex.Match(strhtml2, Rxg3);
                            Match mianji = Regex.Match(strhtml2, Rxg4);
                            Match price = Regex.Match(strhtml2, Rxg5);

                           
                            dgv.Rows[pub.index].Cells[0].Value = titles.Groups[1].Value;

                            dgv.Rows[pub.index].Cells[1].Value = contacts.Groups[1].Value;

                            dgv.Rows[pub.index].Cells[2].Value = tell.Groups[1].Value;


                            dgv.Rows[pub.index].Cells[3].Value = area.Groups[1].Value;


                            dgv.Rows[pub.index].Cells[4].Value = xiaoqu.Groups[1].Value;


                            string temp = Regex.Replace(mianji.Groups[1].Value, "<[^>]*>", "");
                            dgv.Rows[pub.index].Cells[5].Value = temp.Trim();

                            string temp1 = Regex.Replace(price.Groups[1].Value, "<[^>]*>", "");
                            dgv.Rows[pub.index].Cells[6].Value = temp1.Trim();

                            dgv.Rows[pub.index].Cells[7].Value = list;

                            dgv.CurrentCell = dgv.Rows[pub.index].Cells[0];                         //让datagridview滚动到当前行


                            //存入数据库开始
                            string[] values = { dgv.Rows[pub.index].Cells[0].Value.ToString(),
                                dgv.Rows[pub.index].Cells[1].Value.ToString(),
                                dgv.Rows[pub.index].Cells[2].Value.ToString(),
                                dgv.Rows[pub.index].Cells[3].Value.ToString(),
                                dgv.Rows[pub.index].Cells[4].Value.ToString(),
                                dgv.Rows[pub.index].Cells[5].Value.ToString(),
                                dgv.Rows[pub.index].Cells[6].Value.ToString(),
                                dgv.Rows[pub.index].Cells[7].Value.ToString(),


                            };

                            string sql = "INSERT INTO result VALUES( '" + values[0] + "','" + values[1] + "','" + values[2] + "','" + values[3] + "','" + values[4] + "','" + values[5] + "','" + values[6] + "','" + values[7] + "')";

                            method.insertData("58同城房源", sql);
                            //存入数据库结束



                            Application.DoEvents();
                            System.Threading.Thread.Sleep(Convert.ToInt32(1000));   //内容获取间隔，可变量                     


                            
                        }


                    }
                }
            }


            catch (System.Exception ex)
            {

                pubVariables.exs=ex.ToString();

            }

        }



        /// <summary>
        /// 生意转让
        /// </summary>
        /// <param name="dgv1"></param>
        public void shengyizr(object dgv1)
        {
                            
            DataGridView dgv = (DataGridView)dgv1;
           

            string[] headers = { "标题", "联系人", "电话", "地区", "网址" };

            method.setDatagridview(dgv, 5, headers);

            method.CreateTable("58同城生意转让", headers); //创建数据表

            string[] citys = pubVariables.citys.Split(new string[] { "," }, StringSplitOptions.None);
            try
            {

                foreach (string cityname in citys)
                {
                    string city = method.Get58pinyin(cityname);

                    for (int i = 1; i <= 70; i++)
                    {
                        String Url = "http://" + city + ".58.com/shengyizr/" + pubVariables.fangFrom + "/pn" + i + "/";
                        string html = method.GetUrl(Url);
                        MatchCollection TitleMatchs = Regex.Matches(html, @"<li logr=""([\s\S]*?)_([\s\S]*?)_([\s\S]*?)_([\s\S]*?)_", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        ArrayList lists = new ArrayList();
                        foreach (Match NextMatch in TitleMatchs)
                        {

                            lists.Add("https://" + city + ".58.com/shangpu/" + NextMatch.Groups[4].Value + "x.shtml");

                        }
                        if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                            break;


                        foreach (string list in lists)

                        {
                            if (pubVariables.status == false)
                                return;

                            //autoEvent.WaitOne();  //阻塞当前线程，等待通知以继续执行
                            pub.index = dgv.Rows.Add();
                            String Url1 = list;
                            string strhtml = method.GetUrl(Url1);  //定义的GetRul方法 返回 reader.ReadToEnd()


                            string title = @"<h1 class=""c_000 f20"">([\s\S]*?)</h1>";
                            string Rxg = @"<span class='f14 c_333 jjrsay'>([\s\S]*?)</span>";
                            string Rxg1 = @"<p class='phone-num'>([\s\S]*?)</p>";
                            string Rxg2 = @"详细地址:</span>([\s\S]*?)</span>";



                            Match titles = Regex.Match(strhtml, title);
                            Match contacts = Regex.Match(strhtml, Rxg);
                            Match tell = Regex.Match(strhtml, Rxg1);
                            Match region = Regex.Match(strhtml, Rxg2);



                            dgv.Rows[pub.index].Cells[0].Value = titles.Groups[1].Value.Trim();

                            dgv.Rows[pub.index].Cells[1].Value = contacts.Groups[1].Value.Trim();
                            dgv.Rows[pub.index].Cells[2].Value = tell.Groups[1].Value;

                            string temp = Regex.Replace(region.Groups[1].Value, "<[^>]*>", "");
                            dgv.Rows[pub.index].Cells[3].Value = temp.Trim().Replace(" ", "").Replace("&nbsp;", "");

                            dgv.Rows[pub.index].Cells[4].Value = list;

                            dgv.CurrentCell = dgv.Rows[pub.index].Cells[0];  //让datagridview滚动到当前行


                            //存入数据库开始
                            string[] values = { dgv.Rows[pub.index].Cells[0].Value.ToString(),
                                dgv.Rows[pub.index].Cells[1].Value.ToString(),
                                dgv.Rows[pub.index].Cells[2].Value.ToString(),
                                dgv.Rows[pub.index].Cells[3].Value.ToString(),
                                dgv.Rows[pub.index].Cells[4].Value.ToString()


                            };

                            string sql = "INSERT INTO result VALUES( '" + values[0] + "','" + values[1] + "','" + values[2] + "','" + values[3] + "','" + values[4] + "')";

                            method.insertData("58同城生意转让", sql);
                            
                            //存入数据库结束

                            //ListViewItem lv1 = listView1.Items.Add(titles.Groups[1].Value.Trim()); //使用Listview展示数据
                            //lv1.SubItems.Add(contacts.Groups[1].Value);
                            //lv1.SubItems.Add(list);


                            Application.DoEvents();
                            Thread.Sleep(Convert.ToInt32(1000));   //内容获取间隔，可变量

                        }

                    }

                }
            }

            catch (System.Exception ex)
            {
                pubVariables.exs=ex.ToString();
             
            }


        }



        /// <summary>
        /// 商铺出租
        /// </summary>
        /// <param name="dgv1"></param>
        public void shangpucz(object dgv1)
        {
            DataGridView dgv = (DataGridView)dgv1;

            string[] headers = { "标题", "联系人", "电话", "地区", "网址" };

            method.setDatagridview(dgv, 5, headers);
            method.CreateTable("58同城商铺出租", headers); //创建数据表

            string[] citys = pubVariables.citys.Split(new string[] { "," }, StringSplitOptions.None);
            try
            {

                foreach (string cityname in citys)
                {
                    string city = method.Get58pinyin(cityname);

                    for (int i = 1; i <= 70; i++)
                    {
                        String Url = "http://" + city + ".58.com/shangpucz/" + pubVariables.fangFrom + "/pn" + i + "/";
                        string html = method.GetUrl(Url);
                        MatchCollection TitleMatchs = Regex.Matches(html, @"<li logr=""([\s\S]*?)_([\s\S]*?)_([\s\S]*?)_([\s\S]*?)_", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        ArrayList lists = new ArrayList();
                        foreach (Match NextMatch in TitleMatchs)
                        {

                            lists.Add("https://" + city + ".58.com/shangpu/" + NextMatch.Groups[4].Value + "x.shtml");

                        }
                        if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                            break;


                        foreach (string list in lists)

                        {
                            if (pubVariables.status == false)
                                return;

                            autoEvent.WaitOne();  //阻塞当前线程，等待通知以继续执行

                            pub.index = dgv.Rows.Add();
                            String Url1 = list;
                            string strhtml = method.GetUrl(Url1);  //定义的GetRul方法 返回 reader.ReadToEnd()


                            string title = @"<h1 class=""c_000 f20"">([\s\S]*?)</h1>";
                            string Rxg = @"<span class='f14 c_333 jjrsay'>([\s\S]*?)</span>";
                            string Rxg1 = @"<p class='phone-num'>([\s\S]*?)</p>";
                            string Rxg2 = @"详细地址:</span>([\s\S]*?)</span>";



                            Match titles = Regex.Match(strhtml, title);
                            Match contacts = Regex.Match(strhtml, Rxg);
                            Match tell = Regex.Match(strhtml, Rxg1);
                            Match region = Regex.Match(strhtml, Rxg2);



                            dgv.Rows[pub.index].Cells[0].Value = titles.Groups[1].Value.Trim();

                            dgv.Rows[pub.index].Cells[1].Value = contacts.Groups[1].Value.Trim();
                            dgv.Rows[pub.index].Cells[2].Value = tell.Groups[1].Value;

                            string temp = Regex.Replace(region.Groups[1].Value, "<[^>]*>", "");
                            dgv.Rows[pub.index].Cells[3].Value = temp.Trim().Replace(" ", "").Replace("&nbsp;", "");

                            dgv.Rows[pub.index].Cells[4].Value = list;

                            dgv.CurrentCell = dgv.Rows[pub.index].Cells[0];  //让datagridview滚动到当前行


                            //存入数据库开始
                            string[] values = { dgv.Rows[pub.index].Cells[0].Value.ToString(),
                                dgv.Rows[pub.index].Cells[1].Value.ToString(),
                                dgv.Rows[pub.index].Cells[2].Value.ToString(),
                                dgv.Rows[pub.index].Cells[3].Value.ToString(),
                                dgv.Rows[pub.index].Cells[4].Value.ToString()


                            };

                            string sql = "INSERT INTO result VALUES( '" + values[0] + "','" + values[1] + "','" + values[2] + "','" + values[3] + "','" + values[4] + "')";

                            method.insertData("58同城商铺出租", sql);
                            //存入数据库结束


                            Application.DoEvents();
                            Thread.Sleep(Convert.ToInt32(1000));   //内容获取间隔，可变量

                        }

                    }

                }
            }

            catch (System.Exception ex)
            {

                pubVariables.exs=ex.ToString();
            }


        }


        /// <summary>
        ///  商铺出售
        /// </summary>
        /// <param name="dgv1"></param>
        public void shangpucs(object dgv1)
        {
            DataGridView dgv = (DataGridView)dgv1;

            string[] headers = { "标题", "联系人", "电话", "地区", "网址" };

            method.setDatagridview(dgv, 5, headers);
            method.CreateTable("58同城商铺出售", headers); //创建数据表
            string[] citys = pubVariables.citys.Split(new string[] { "," }, StringSplitOptions.None);
            try
            {

                foreach (string cityname in citys)
                {
                    string city = method.Get58pinyin(cityname);

                    for (int i = 1; i <= 70; i++)
                    {
                        String Url = "http://" + city + ".58.com/shangpucs/" + pubVariables.fangFrom + "/pn" + i + "/";
                        string html = method.GetUrl(Url);
                        MatchCollection TitleMatchs = Regex.Matches(html, @"<li logr=""([\s\S]*?)_([\s\S]*?)_([\s\S]*?)_([\s\S]*?)_", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        ArrayList lists = new ArrayList();
                        foreach (Match NextMatch in TitleMatchs)
                        {

                            lists.Add("https://" + city + ".58.com/shangpu/" + NextMatch.Groups[4].Value + "x.shtml");

                        }
                        if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                            break;


                        foreach (string list in lists)

                        {
                            if (pubVariables.status == false)
                                return;
                            autoEvent.WaitOne();  //阻塞当前线程，等待通知以继续执行
                            pub.index = dgv.Rows.Add();
                            String Url1 = list;
                            string strhtml = method.GetUrl(Url1);  //定义的GetRul方法 返回 reader.ReadToEnd()


                            string title = @"<h1 class=""c_000 f20"">([\s\S]*?)</h1>";
                            string Rxg = @"<span class='f14 c_333 jjrsay'>([\s\S]*?)</span>";
                            string Rxg1 = @"<p class='phone-num'>([\s\S]*?)</p>";
                            string Rxg2 = @"详细地址:</span>([\s\S]*?)</span>";



                            Match titles = Regex.Match(strhtml, title);
                            Match contacts = Regex.Match(strhtml, Rxg);
                            Match tell = Regex.Match(strhtml, Rxg1);
                            Match region = Regex.Match(strhtml, Rxg2);



                            dgv.Rows[pub.index].Cells[0].Value = titles.Groups[1].Value.Trim();

                            dgv.Rows[pub.index].Cells[1].Value = contacts.Groups[1].Value.Trim();
                            dgv.Rows[pub.index].Cells[2].Value = tell.Groups[1].Value;

                            string temp = Regex.Replace(region.Groups[1].Value, "<[^>]*>", "");
                            dgv.Rows[pub.index].Cells[3].Value = temp.Trim().Replace(" ", "").Replace("&nbsp;", "");

                            dgv.Rows[pub.index].Cells[4].Value = list;

                            dgv.CurrentCell = dgv.Rows[pub.index].Cells[0];  //让datagridview滚动到当前行



                            //存入数据库开始
                            string[] values = { dgv.Rows[pub.index].Cells[0].Value.ToString(),
                                dgv.Rows[pub.index].Cells[1].Value.ToString(),
                                dgv.Rows[pub.index].Cells[2].Value.ToString(),
                                dgv.Rows[pub.index].Cells[3].Value.ToString(),
                                dgv.Rows[pub.index].Cells[4].Value.ToString()


                            };

                            string sql = "INSERT INTO result VALUES( '" + values[0] + "','" + values[1] + "','" + values[2] + "','" + values[3] + "','" + values[4] + "')";

                            method.insertData("58同城商铺出售", sql);
                            //存入数据库结束

                            Application.DoEvents();
                            Thread.Sleep(Convert.ToInt32(1000));   //内容获取间隔，可变量

                        }

                    }

                }
            }

            catch (System.Exception ex)
            {

                pubVariables.exs=ex.ToString();
            }


        }


        /// <summary>
        /// 58出租
        /// </summary>
        /// <param name="dgv1"></param>
        public void chuzu(object dgv1)
        {

            DataGridView dgv = (DataGridView)dgv1;

            string[] headers = { "标题", "联系人", "电话", "地区", "网址" };

            method.setDatagridview(dgv, 5, headers);

            method.CreateTable(pubVariables.item, headers); //创建数据表

            string[] citys = pubVariables.citys.Split(new string[] { "," }, StringSplitOptions.None);
            try
            {

                foreach (string cityname in citys)
                {
                    string city = method.Get58pinyin(cityname);

                    for (int i = 1; i <= 70; i++)
                    {
                        String Url = "http://" + city + ".58.com/chuzu/" + pubVariables.fangFrom + "/pn" + i + "/";
                        string html = method.GetUrl(Url);
                        MatchCollection TitleMatchs = Regex.Matches(html, @"<li logr=""([\s\S]*?)_([\s\S]*?)_([\s\S]*?)_([\s\S]*?)_", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        ArrayList lists = new ArrayList();
                        foreach (Match NextMatch in TitleMatchs)
                        {

                            lists.Add("https://" + city + ".58.com/shangpu/" + NextMatch.Groups[4].Value + "x.shtml");

                        }
                        if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                            break;


                        foreach (string list in lists)

                        {
                            if (pubVariables.status == false)
                                return;

                            //autoEvent.WaitOne();  //阻塞当前线程，等待通知以继续执行
                            pub.index = dgv.Rows.Add();
                            String Url1 = list;
                            string strhtml = method.GetUrl(Url1);  //定义的GetRul方法 返回 reader.ReadToEnd()


                            string title = @"<h1 class=""c_000 f20"">([\s\S]*?)</h1>";
                            string Rxg = @"<span class='f14 c_333 jjrsay'>([\s\S]*?)</span>";
                            string Rxg1 = @"<p class='phone-num'>([\s\S]*?)</p>";
                            string Rxg2 = @"详细地址:</span>([\s\S]*?)</span>";



                            Match titles = Regex.Match(strhtml, title);
                            Match contacts = Regex.Match(strhtml, Rxg);
                            Match tell = Regex.Match(strhtml, Rxg1);
                            Match region = Regex.Match(strhtml, Rxg2);



                            dgv.Rows[pub.index].Cells[0].Value = titles.Groups[1].Value.Trim();

                            dgv.Rows[pub.index].Cells[1].Value = contacts.Groups[1].Value.Trim();
                            dgv.Rows[pub.index].Cells[2].Value = tell.Groups[1].Value;

                            string temp = Regex.Replace(region.Groups[1].Value, "<[^>]*>", "");
                            dgv.Rows[pub.index].Cells[3].Value = temp.Trim().Replace(" ", "").Replace("&nbsp;", "");

                            dgv.Rows[pub.index].Cells[4].Value = list;

                            dgv.CurrentCell = dgv.Rows[pub.index].Cells[0];  //让datagridview滚动到当前行


                            //存入数据库开始
                            string[] values = { dgv.Rows[pub.index].Cells[0].Value.ToString(),
                                dgv.Rows[pub.index].Cells[1].Value.ToString(),
                                dgv.Rows[pub.index].Cells[2].Value.ToString(),
                                dgv.Rows[pub.index].Cells[3].Value.ToString(),
                                dgv.Rows[pub.index].Cells[4].Value.ToString()


                            };

                            string sql = "INSERT INTO result VALUES( '" + values[0] + "','" + values[1] + "','" + values[2] + "','" + values[3] + "','" + values[4] + "')";

                            method.insertData(pubVariables.item, sql);
                            //存入数据库结束

                            //ListViewItem lv1 = listView1.Items.Add(titles.Groups[1].Value.Trim()); //使用Listview展示数据
                            //lv1.SubItems.Add(contacts.Groups[1].Value);
                            //lv1.SubItems.Add(list);


                            Application.DoEvents();
                            Thread.Sleep(Convert.ToInt32(1000));   //内容获取间隔，可变量

                        }

                    }

                }
            }

            catch (System.Exception ex)
            {

                pubVariables.exs = ex.ToString();

                MessageBox.Show(ex.ToString());
            }


        }


        /// <summary>
        /// 58厂房
        /// </summary>
        /// <param name="dgv1"></param>
        public void changfang(object dgv1)
        {

            DataGridView dgv = (DataGridView)dgv1;


            string[] headers = { "标题", "联系人", "电话", "地区", "网址" };

            method.setDatagridview(dgv, 5, headers);

            method.CreateTable("58同城厂房", headers); //创建数据表

            string[] citys = pubVariables.citys.Split(new string[] { "," }, StringSplitOptions.None);
            try
            {

                foreach (string cityname in citys)
                {
                    string city = method.Get58pinyin(cityname);

                    for (int i = 1; i <= 70; i++)
                    {
                        String Url = "http://" + city + ".58.com/changfang/" + pubVariables.fangFrom + "/pn" + i + "/";
                        string html = method.GetUrl(Url);
                        MatchCollection TitleMatchs = Regex.Matches(html, @"<li logr=""([\s\S]*?)_([\s\S]*?)_([\s\S]*?)_([\s\S]*?)_", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        ArrayList lists = new ArrayList();
                        foreach (Match NextMatch in TitleMatchs)
                        {

                            lists.Add("https://" + city + ".58.com/fangchan/" + NextMatch.Groups[4].Value + "x.shtml");

                        }
                        if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                            break;


                        foreach (string list in lists)

                        {
                            if (pubVariables.status == false)
                                return;

                            //autoEvent.WaitOne();  //阻塞当前线程，等待通知以继续执行
                            pub.index = dgv.Rows.Add();
                            String Url1 = list;
                            string strhtml = method.GetUrl(Url1);  //定义的GetRul方法 返回 reader.ReadToEnd()


                            string title = @"<h1 class=""c_000 f20"">([\s\S]*?)</h1>";
                            string Rxg = @"<span class='f14 c_333 jjrsay'>([\s\S]*?)</span>";
                            string Rxg1 = @"<p class='phone-num'>([\s\S]*?)</p>";
                            string Rxg2 = @"详细地址:</span>([\s\S]*?)</span>";



                            Match titles = Regex.Match(strhtml, title);
                            Match contacts = Regex.Match(strhtml, Rxg);
                            Match tell = Regex.Match(strhtml, Rxg1);
                            Match region = Regex.Match(strhtml, Rxg2);



                            dgv.Rows[pub.index].Cells[0].Value = titles.Groups[1].Value.Trim();

                            dgv.Rows[pub.index].Cells[1].Value = contacts.Groups[1].Value.Trim();
                            dgv.Rows[pub.index].Cells[2].Value = tell.Groups[1].Value;

                            string temp = Regex.Replace(region.Groups[1].Value, "<[^>]*>", "");
                            dgv.Rows[pub.index].Cells[3].Value = temp.Trim().Replace(" ", "").Replace("&nbsp;", "");

                            dgv.Rows[pub.index].Cells[4].Value = list;

                            dgv.CurrentCell = dgv.Rows[pub.index].Cells[0];  //让datagridview滚动到当前行


                            //存入数据库开始
                            string[] values = { dgv.Rows[pub.index].Cells[0].Value.ToString(),
                                dgv.Rows[pub.index].Cells[1].Value.ToString(),
                                dgv.Rows[pub.index].Cells[2].Value.ToString(),
                                dgv.Rows[pub.index].Cells[3].Value.ToString(),
                                dgv.Rows[pub.index].Cells[4].Value.ToString()


                            };

                            string sql = "INSERT INTO result VALUES( '" + values[0] + "','" + values[1] + "','" + values[2] + "','" + values[3] + "','" + values[4] + "')";

                            method.insertData("58同城厂房", sql);
                            //存入数据库结束

                            //ListViewItem lv1 = listView1.Items.Add(titles.Groups[1].Value.Trim()); //使用Listview展示数据
                            //lv1.SubItems.Add(contacts.Groups[1].Value);
                            //lv1.SubItems.Add(list);


                            Application.DoEvents();
                            Thread.Sleep(Convert.ToInt32(1000));   //内容获取间隔，可变量

                        }

                    }

                }
            }

            catch (System.Exception ex)
            {

                pubVariables.exs = ex.ToString();

                MessageBox.Show(ex.ToString());
            }


        }


        /// <summary>
        /// 58车库
        /// </summary>
        /// <param name="dgv1"></param>
        public void cheku(object dgv1)
        {

            DataGridView dgv = (DataGridView)dgv1;


            string[] headers = { "标题", "联系人", "电话", "地区", "网址" };

            method.setDatagridview(dgv, 5, headers);

            method.CreateTable("58同城车库", headers); //创建数据表

            string[] citys = pubVariables.citys.Split(new string[] { "," }, StringSplitOptions.None);
            try
            {

                foreach (string cityname in citys)
                {
                    string city = method.Get58pinyin(cityname);

                    for (int i = 1; i <= 70; i++)
                    {
                        String Url = "http://" + city + ".58.com/cheku/" + pubVariables.fangFrom + "/pn" + i + "/";
                        string html = method.GetUrl(Url);
                        MatchCollection TitleMatchs = Regex.Matches(html, @"<li logr=""([\s\S]*?)_([\s\S]*?)_([\s\S]*?)_([\s\S]*?)_", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        ArrayList lists = new ArrayList();
                        foreach (Match NextMatch in TitleMatchs)
                        {

                            lists.Add("https://" + city + ".58.com/fangchan/" + NextMatch.Groups[4].Value + "x.shtml");

                        }
                        if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                            break;


                        foreach (string list in lists)

                        {
                            if (pubVariables.status == false)
                                return;

                            //autoEvent.WaitOne();  //阻塞当前线程，等待通知以继续执行
                            pub.index = dgv.Rows.Add();
                            String Url1 = list;
                            string strhtml = method.GetUrl(Url1);  //定义的GetRul方法 返回 reader.ReadToEnd()


                            string title = @"<h1 class=""c_000 f20"">([\s\S]*?)</h1>";
                            string Rxg = @"<span class='f14 c_333 jjrsay'>([\s\S]*?)</span>";
                            string Rxg1 = @"<p class='phone-num'>([\s\S]*?)</p>";
                            string Rxg2 = @"详细地址:</span>([\s\S]*?)</span>";



                            Match titles = Regex.Match(strhtml, title);
                            Match contacts = Regex.Match(strhtml, Rxg);
                            Match tell = Regex.Match(strhtml, Rxg1);
                            Match region = Regex.Match(strhtml, Rxg2);



                            dgv.Rows[pub.index].Cells[0].Value = titles.Groups[1].Value.Trim();

                            dgv.Rows[pub.index].Cells[1].Value = contacts.Groups[1].Value.Trim();
                            dgv.Rows[pub.index].Cells[2].Value = tell.Groups[1].Value;

                            string temp = Regex.Replace(region.Groups[1].Value, "<[^>]*>", "");
                            dgv.Rows[pub.index].Cells[3].Value = temp.Trim().Replace(" ", "").Replace("&nbsp;", "");

                            dgv.Rows[pub.index].Cells[4].Value = list;

                            dgv.CurrentCell = dgv.Rows[pub.index].Cells[0];  //让datagridview滚动到当前行


                            //存入数据库开始
                            string[] values = { dgv.Rows[pub.index].Cells[0].Value.ToString(),
                                dgv.Rows[pub.index].Cells[1].Value.ToString(),
                                dgv.Rows[pub.index].Cells[2].Value.ToString(),
                                dgv.Rows[pub.index].Cells[3].Value.ToString(),
                                dgv.Rows[pub.index].Cells[4].Value.ToString()


                            };

                            string sql = "INSERT INTO result VALUES( '" + values[0] + "','" + values[1] + "','" + values[2] + "','" + values[3] + "','" + values[4] + "')";

                            method.insertData("58同城车库", sql);
                            //存入数据库结束

                            //ListViewItem lv1 = listView1.Items.Add(titles.Groups[1].Value.Trim()); //使用Listview展示数据
                            //lv1.SubItems.Add(contacts.Groups[1].Value);
                            //lv1.SubItems.Add(list);


                            Application.DoEvents();
                            Thread.Sleep(Convert.ToInt32(1000));   //内容获取间隔，可变量

                        }

                    }

                }
            }

            catch (System.Exception ex)
            {

                pubVariables.exs = ex.ToString();

                MessageBox.Show(ex.ToString());
            }


        }

        /// <summary>
        /// 58写字楼
        /// </summary>
        /// <param name="dgv1"></param>
        public void xiezilou(object dgv1)
        {

            DataGridView dgv = (DataGridView)dgv1;


            string[] headers = { "标题", "联系人", "电话", "地区", "网址" };

            method.setDatagridview(dgv, 5, headers);

            method.CreateTable("58同城写字楼", headers); //创建数据表

            string[] citys = pubVariables.citys.Split(new string[] { "," }, StringSplitOptions.None);
            try
            {

                foreach (string cityname in citys)
                {
                    string city = method.Get58pinyin(cityname);

                    for (int i = 1; i <= 70; i++)
                    {
                        String Url = "http://" + city + ".58.com/zhaozu/" + pubVariables.fangFrom + "/pn" + i + "/";
                        string html = method.GetUrl(Url);
                        MatchCollection TitleMatchs = Regex.Matches(html, @"<li logr=""([\s\S]*?)_([\s\S]*?)_([\s\S]*?)_([\s\S]*?)_", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        ArrayList lists = new ArrayList();
                        foreach (Match NextMatch in TitleMatchs)
                        {

                            lists.Add("https://" + city + ".58.com/fangchan/" + NextMatch.Groups[4].Value + "x.shtml");

                        }
                        if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                            break;


                        foreach (string list in lists)

                        {
                            if (pubVariables.status == false)
                                return;

                            //autoEvent.WaitOne();  //阻塞当前线程，等待通知以继续执行
                            pub.index = dgv.Rows.Add();
                            String Url1 = list;
                            string strhtml = method.GetUrl(Url1);  //定义的GetRul方法 返回 reader.ReadToEnd()


                            string title = @"<h1 class=""c_000 f20"">([\s\S]*?)</h1>";
                            string Rxg = @"<span class='f14 c_333 jjrsay'>([\s\S]*?)</span>";
                            string Rxg1 = @"<p class='phone-num'>([\s\S]*?)</p>";
                            string Rxg2 = @"详细地址:</span>([\s\S]*?)</span>";



                            Match titles = Regex.Match(strhtml, title);
                            Match contacts = Regex.Match(strhtml, Rxg);
                            Match tell = Regex.Match(strhtml, Rxg1);
                            Match region = Regex.Match(strhtml, Rxg2);



                            dgv.Rows[pub.index].Cells[0].Value = titles.Groups[1].Value.Trim();

                            dgv.Rows[pub.index].Cells[1].Value = contacts.Groups[1].Value.Trim();
                            dgv.Rows[pub.index].Cells[2].Value = tell.Groups[1].Value;

                            string temp = Regex.Replace(region.Groups[1].Value, "<[^>]*>", "");
                            dgv.Rows[pub.index].Cells[3].Value = temp.Trim().Replace(" ", "").Replace("&nbsp;", "");

                            dgv.Rows[pub.index].Cells[4].Value = list;

                            dgv.CurrentCell = dgv.Rows[pub.index].Cells[0];  //让datagridview滚动到当前行


                            //存入数据库开始
                            string[] values = { dgv.Rows[pub.index].Cells[0].Value.ToString(),
                                dgv.Rows[pub.index].Cells[1].Value.ToString(),
                                dgv.Rows[pub.index].Cells[2].Value.ToString(),
                                dgv.Rows[pub.index].Cells[3].Value.ToString(),
                                dgv.Rows[pub.index].Cells[4].Value.ToString()


                            };

                            string sql = "INSERT INTO result VALUES( '" + values[0] + "','" + values[1] + "','" + values[2] + "','" + values[3] + "','" + values[4] + "')";

                            method.insertData("58同城写字楼", sql);
                            //存入数据库结束

                            //ListViewItem lv1 = listView1.Items.Add(titles.Groups[1].Value.Trim()); //使用Listview展示数据
                            //lv1.SubItems.Add(contacts.Groups[1].Value);
                            //lv1.SubItems.Add(list);


                            Application.DoEvents();
                            Thread.Sleep(Convert.ToInt32(1000));   //内容获取间隔，可变量

                        }

                    }

                }
            }

            catch (System.Exception ex)
            {

                pubVariables.exs = ex.ToString();

                MessageBox.Show(ex.ToString());
            }


        }

        /// <summary>
        /// 58土地
        /// </summary>
        /// <param name="dgv1"></param>
        public void tudi(object dgv1)
        {

            DataGridView dgv = (DataGridView)dgv1;


            string[] headers = { "标题", "联系人", "电话", "地区", "网址" };

            method.setDatagridview(dgv, 5, headers);

            method.CreateTable("58同城土地", headers); //创建数据表

            string[] citys = pubVariables.citys.Split(new string[] { "," }, StringSplitOptions.None);
            try
            {

                foreach (string cityname in citys)
                {
                    string city = method.Get58pinyin(cityname);

                    for (int i = 1; i <= 70; i++)
                    {
                        String Url = "http://" + city + ".58.com/tudi/" + pubVariables.fangFrom + "/pn" + i + "/";
                        string html = method.GetUrl(Url);
                        MatchCollection TitleMatchs = Regex.Matches(html, @"<li logr=""([\s\S]*?)_([\s\S]*?)_([\s\S]*?)_([\s\S]*?)_", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                        ArrayList lists = new ArrayList();
                        foreach (Match NextMatch in TitleMatchs)
                        {

                            lists.Add("https://" + city + ".58.com/fangchan/" + NextMatch.Groups[4].Value + "x.shtml");

                        }
                        if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                            break;


                        foreach (string list in lists)

                        {
                            if (pubVariables.status == false)
                                return;

                            //autoEvent.WaitOne();  //阻塞当前线程，等待通知以继续执行
                            pub.index = dgv.Rows.Add();
                            String Url1 = list;
                            string strhtml = method.GetUrl(Url1);  //定义的GetRul方法 返回 reader.ReadToEnd()


                            string title = @"<h1 class=""c_000 f20"">([\s\S]*?)</h1>";
                            string Rxg = @"<span class='f14 c_333 jjrsay'>([\s\S]*?)</span>";
                            string Rxg1 = @"<p class='phone-num'>([\s\S]*?)</p>";
                            string Rxg2 = @"详细地址:</span>([\s\S]*?)</span>";



                            Match titles = Regex.Match(strhtml, title);
                            Match contacts = Regex.Match(strhtml, Rxg);
                            Match tell = Regex.Match(strhtml, Rxg1);
                            Match region = Regex.Match(strhtml, Rxg2);



                            dgv.Rows[pub.index].Cells[0].Value = titles.Groups[1].Value.Trim();

                            dgv.Rows[pub.index].Cells[1].Value = contacts.Groups[1].Value.Trim();
                            dgv.Rows[pub.index].Cells[2].Value = tell.Groups[1].Value;

                            string temp = Regex.Replace(region.Groups[1].Value, "<[^>]*>", "");
                            dgv.Rows[pub.index].Cells[3].Value = temp.Trim().Replace(" ", "").Replace("&nbsp;", "");

                            dgv.Rows[pub.index].Cells[4].Value = list;

                            dgv.CurrentCell = dgv.Rows[pub.index].Cells[0];  //让datagridview滚动到当前行


                            //存入数据库开始
                            string[] values = { dgv.Rows[pub.index].Cells[0].Value.ToString(),
                                dgv.Rows[pub.index].Cells[1].Value.ToString(),
                                dgv.Rows[pub.index].Cells[2].Value.ToString(),
                                dgv.Rows[pub.index].Cells[3].Value.ToString(),
                                dgv.Rows[pub.index].Cells[4].Value.ToString()


                            };

                            string sql = "INSERT INTO result VALUES( '" + values[0] + "','" + values[1] + "','" + values[2] + "','" + values[3] + "','" + values[4] + "')";

                            method.insertData("58同城土地", sql);
                            //存入数据库结束

                            //ListViewItem lv1 = listView1.Items.Add(titles.Groups[1].Value.Trim()); //使用Listview展示数据
                            //lv1.SubItems.Add(contacts.Groups[1].Value);
                            //lv1.SubItems.Add(list);


                            Application.DoEvents();
                            Thread.Sleep(Convert.ToInt32(1000));   //内容获取间隔，可变量

                        }

                    }

                }
            }

            catch (System.Exception ex)
            {

                pubVariables.exs = ex.ToString();

                MessageBox.Show(ex.ToString());
            }


        }

        /// <summary>
        /// 美团美食
        /// </summary>
        /// <param name="dgv1"></param>
        public void meituan_meishi(object dgv1)
        {
            DataGridView dgv = (DataGridView)dgv1;
            string[] headers = { "商家", "地址", "电话", "地区", "网址" };

            method.setDatagridview(dgv, 5, headers);
            method.CreateTable("美团美食分类商家", headers); //创建数据表
            try
            {
                string[] citys = pubVariables.citys.Split(new string[] { "," }, StringSplitOptions.None);

                string[] keywords = pubVariables.keywords.Split(new string[] { "," }, StringSplitOptions.None);



                foreach (string city in citys)

                {

                    foreach (string keyword in keywords)

                    {


                        for (int i = 1; i <= 50; i++)

                        {


                            String Url = "http://i.meituan.com/s/" + method.GetMtpinyin(city) + "-" + keyword + "?p=" + i;
                            string strhtml = method.GetMtUrl(Url, pub.mtcookie);  //定义的GetRul方法 返回 reader.ReadToEnd()


                            string Rxg = @"data-href=""//i.meituan.com/poi/([\s\S]*?)"">";



                            MatchCollection all = Regex.Matches(strhtml, Rxg);

                            ArrayList lists = new ArrayList();
                            foreach (Match NextMatch in all)
                            {


                                lists.Add("http://www.meituan.com/meishi/" + NextMatch.Groups[1].Value + "/");



                            }

                            if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                break;

                            string Rxg0 = @"<title>([\s\S]*?)_";
                            string Rxg1 = @"""phone"":""([\s\S]*?)""";
                            string Rxg2 = @"""address"":""([\s\S]*?)""";

                            foreach (string list in lists)

                            {

                                if (pubVariables.status == false)
                                    return;
                                autoEvent.WaitOne();  //阻塞当前线程，等待通知以继续执行

                                string strhtml1 = method.GetMtUrl(list, pub.mtcookie);  //定义的GetRul方法 返回 reader.ReadToEnd()



                                Match name = Regex.Match(strhtml1, Rxg0);
                                Match tell = Regex.Match(strhtml1, Rxg1);
                                Match addr = Regex.Match(strhtml1, Rxg2);


                                pub.index = dgv.Rows.Add();    //利用dgv.Rows.Add()事件为DataGridView控件增加新的行，该函数返回添加新行的索引号，即新行的行号，然后可以通过该索引号操作该行的各个单元格，如dgv.Rows[pub.index].Cells[0].Value = "1"。这是很常用也是很简单的方法。


                                dgv.Rows[pub.index].Cells[0].Value = name.Groups[1].Value;

                                dgv.Rows[pub.index].Cells[1].Value = addr.Groups[1].Value;
                                dgv.Rows[pub.index].Cells[2].Value = tell.Groups[1].Value;


                                dgv.Rows[pub.index].Cells[3].Value = city;
                                dgv.Rows[pub.index].Cells[4].Value = list;


                                dgv.CurrentCell = dgv.Rows[pub.index].Cells[0];  //让datagridview滚动到当前行


                                //存入数据库开始
                                string[] values = { dgv.Rows[pub.index].Cells[0].Value.ToString(),
                                dgv.Rows[pub.index].Cells[1].Value.ToString(),
                                dgv.Rows[pub.index].Cells[2].Value.ToString(),
                                dgv.Rows[pub.index].Cells[3].Value.ToString(),
                                dgv.Rows[pub.index].Cells[4].Value.ToString()


                            };

                                string sql = "INSERT INTO result VALUES( '" + values[0] + "','" + values[1] + "','" + values[2] + "','" + values[3] + "','" + values[4] + "')";

                                method.insertData("美团美食分类商家", sql);
                                //存入数据库结束
                                Application.DoEvents();
                                Thread.Sleep(1000);


                            }


                        }

                    }





                }

            }

            catch (System.Exception ex)
            {
                pubVariables.exs=ex.ToString();
            }
        }



        /// <summary>
        /// 美团其他分类
        /// </summary>
        /// <param name="dgv1"></param>
        public void meituan_others(object dgv1)
        {
            DataGridView dgv = (DataGridView)dgv1;
            string[] headers = { "商家", "地址", "电话", "地区", "网址" };

            method.setDatagridview(dgv, 5, headers);
            method.CreateTable("美团其他分类商家", headers); //创建数据表
            try
            {
                string[] citys = pubVariables.citys.Split(new string[] { "," }, StringSplitOptions.None);



                string[] keywords = pubVariables.keywords.Split(new string[] { "," }, StringSplitOptions.None);



                foreach (string city in citys)

                {

                    foreach (string keyword in keywords)

                    {


                        for (int i = 1; i <= 50; i++)

                        {


                            String Url = "http://i.meituan.com/s/" + method.GetMtpinyin(city) + "-" + keyword + "?p=" + i;

                            string strhtml = method.GetUrl(Url);  //定义的GetRul方法 返回 reader.ReadToEnd()

                            string Rxg = @"data-href=""//i.meituan.com/poi/([\s\S]*?)"">";

                            MatchCollection all = Regex.Matches(strhtml, Rxg);

                            ArrayList lists = new ArrayList();
                            foreach (Match NextMatch in all)
                            {


                                lists.Add("http://i.meituan.com/poi/" + NextMatch.Groups[1].Value);


                            }

                            if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                break;

                            string Rxg0 = @"<h1 class=""dealcard-brand"">([\s\S]*?)</h1>";
                            string Rxg1 = @"data-tele=""([\s\S]*?)""";
                            string Rxg2 = @"addr:([\s\S]*?)&";

                            foreach (string list in lists)

                            {
                                if (pubVariables.status == false)
                                    return;
                                autoEvent.WaitOne();  //阻塞当前线程，等待通知以继续执行

                                String Url1 = list;
                                string strhtml1 = method.GetUrl(Url1);  //定义的GetRul方法 返回 reader.ReadToEnd()

                                Match name = Regex.Match(strhtml1, Rxg0);
                                Match tell = Regex.Match(strhtml1, Rxg1);
                                Match addr = Regex.Match(strhtml1, Rxg2);

                                pub.index = dgv.Rows.Add();    //利用dgv.Rows.Add()事件为DataGridView控件增加新的行，该函数返回添加新行的索引号，即新行的行号，然后可以通过该索引号操作该行的各个单元格，如dgv.Rows[pub.index].Cells[0].Value = "1"。这是很常用也是很简单的方法。

                                dgv.Rows[pub.index].Cells[0].Value = name.Groups[1].Value;
                                dgv.Rows[pub.index].Cells[1].Value = addr.Groups[1].Value;
                                dgv.Rows[pub.index].Cells[2].Value = tell.Groups[1].Value;

                                dgv.Rows[pub.index].Cells[3].Value = city;
                                dgv.Rows[pub.index].Cells[4].Value = list;


                                dgv.CurrentCell = dgv.Rows[pub.index].Cells[0];  //让datagridview滚动到当前行

                                //存入数据库开始
                                string[] values = { dgv.Rows[pub.index].Cells[0].Value.ToString(),
                                dgv.Rows[pub.index].Cells[1].Value.ToString(),
                                dgv.Rows[pub.index].Cells[2].Value.ToString(),
                                dgv.Rows[pub.index].Cells[3].Value.ToString(),
                                dgv.Rows[pub.index].Cells[4].Value.ToString()


                            };

                                string sql = "INSERT INTO result VALUES( '" + values[0] + "','" + values[1] + "','" + values[2] + "','" + values[3] + "','" + values[4] + "')";

                                method.insertData("美团其他分类商家", sql);
                                //存入数据库结束
                                Application.DoEvents();
                                Thread.Sleep(1000);


                            }


                        }

                    }





                }

            }

            catch (System.Exception ex)
            {
                pubVariables.exs=ex.ToString();
            }
        }


        /// <summary>
        /// 慧聪网
        /// </summary>
        /// <param name="dgv1"></param>
        public void huicong(object dgv1)
        {
            DataGridView dgv = (DataGridView)dgv1;
            try
            {
                string[] headers = { "企业名称", "联系人", "电话", "手机", "地址" };

                method.setDatagridview(dgv, 5, headers);
                method.CreateTable("慧聪网企业采集", headers); //创建数据表


                string[] keywords = pubVariables.keywords.Split(new string[] { "," }, StringSplitOptions.None);
                string[] citys = pubVariables.citys.Split(new string[] { "," }, StringSplitOptions.None);

                foreach (string city in citys)

                {
                    string citycode = System.Web.HttpUtility.UrlEncode(city, System.Text.Encoding.GetEncoding("gb2312"));

                    foreach (string keyword in keywords)

                    {

                        string keywordtogb2312 = System.Web.HttpUtility.UrlEncode(keyword, System.Text.Encoding.GetEncoding("gb2312"));

                        for (int i = 1; i <= 100; i++)


                        {



                            String Url = "https://s.hc360.com/?w=" + keywordtogb2312 + "&mc=enterprise&ee=" + i + "&z=" + citycode;
                            string strhtml = method.GetUrl(Url);  //定义的GetRul方法 返回 reader.ReadToEnd()

                            string Rxg = @"<h3><a data-exposurelog=""([\s\S]*?)"" href=""([\s\S]*?)""";



                            MatchCollection all = Regex.Matches(strhtml, Rxg);

                            ArrayList lists = new ArrayList();
                            foreach (Match NextMatch in all)
                            {

                                lists.Add(NextMatch.Groups[2].Value);


                            }
                            if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                break;



                            foreach (string list in lists)
                            {
                                if (pubVariables.status == false)
                                    return;
                                autoEvent.WaitOne();  //阻塞当前线程，等待通知以继续执行
                                pub.index = dgv.Rows.Add();


                                string strhtml1 = method.GetUrl(list);  //定义的GetRul方法 返回 reader.ReadToEnd()



                                string Rxg0 = @"infoname=""([\s\S]*?)""";
                                string Rxg1 = @"联系人</span><span>：([\s\S]*?)</span>";
                                string Rxg2 = @"电话</span><span>：([\s\S]*?)</span>";
                                string Rxg3 = @"手机</span><span>：([\s\S]*?)</span>";

                                string Rxg4 = @"<p>地址：([\s\S]*?)&";



                                Match name = Regex.Match(strhtml1, Rxg0);

                                Match contacts = Regex.Match(strhtml1, Rxg1);


                                Match phone = Regex.Match(strhtml1, Rxg2);

                                Match tell = Regex.Match(strhtml1, Rxg3);
                                Match addr = Regex.Match(strhtml1, Rxg4);
                                dgv.Rows[pub.index].Cells[0].Value = name.Groups[1].Value;

                                string temp = Regex.Replace(contacts.Groups[1].Value, "<[^>]*>", "");   //去除所有标签去除所有标签
                                dgv.Rows[pub.index].Cells[1].Value = temp;

                                dgv.Rows[pub.index].Cells[2].Value = phone.Groups[1].Value;

                                dgv.Rows[pub.index].Cells[3].Value = tell.Groups[1].Value;

                                dgv.Rows[pub.index].Cells[4].Value = addr.Groups[1].Value.Trim();

                                dgv.CurrentCell = dgv.Rows[pub.index].Cells[0];  //让datagridview滚动到当前行


                                //存入数据库开始
                                string[] values = { dgv.Rows[pub.index].Cells[0].Value.ToString(),
                                dgv.Rows[pub.index].Cells[1].Value.ToString(),
                                dgv.Rows[pub.index].Cells[2].Value.ToString(),
                                dgv.Rows[pub.index].Cells[3].Value.ToString(),
                                dgv.Rows[pub.index].Cells[4].Value.ToString()


                            };

                                string sql = "INSERT INTO result VALUES( '" + values[0] + "','" + values[1] + "','" + values[2] + "','" + values[3] + "','" + values[4] + "')";

                                method.insertData("慧聪网企业采集", sql);
                                //存入数据库结束
                                Application.DoEvents();
                                System.Threading.Thread.Sleep(1000);



                            }

                        }

                    }

                }
            }




            catch (System.Exception ex)
            {
                pubVariables.exs=ex.ToString();
            }
        }



        /// <summary>
        /// 阿里巴巴
        /// </summary>
        /// <param name="dgv1"></param>
        public void alibaba(object dgv1)
        {
            DataGridView dgv = (DataGridView)dgv1;

            try
            {

                string[] headers = { "企业名称", "联系人", "电话", "地区", "旺旺", "经营模式", "网址" };

                method.setDatagridview(dgv, 7, headers);
                method.CreateTable("阿里巴巴供应商企业采集", headers); //创建数据表
                string[] keywords = pubVariables.keywords.Split(new string[] { "," }, StringSplitOptions.None);
                string[] citys = pubVariables.citys.Split(new string[] { "," }, StringSplitOptions.None);

                foreach (string city in citys)

                {
                    string city1 = System.Web.HttpUtility.UrlEncode(city, System.Text.Encoding.GetEncoding("GB2312"));

                    foreach (string keyword in keywords)
                    {

                        string keyword1 = System.Web.HttpUtility.UrlEncode(keyword, System.Text.Encoding.GetEncoding("GB2312"));

                        for (int i = 1; i < 100; i++)
                        {

                            String Url = "https://s.1688.com/company/company_search.htm?keywords=" + keyword1 + "&city=" + city1 + "&n=y&filt=y&pageSize=30&offset=3&beginPage=" + i;


                            string html = method.GetAliUrl(Url, pub.aliCookie);


                            MatchCollection TitleMatchs = Regex.Matches(html, @"offer-stat=""com"" title=""([\s\S]*?)"" target=""_blank"" href=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);


                            ArrayList lists = new ArrayList();
                            foreach (Match NextMatch in TitleMatchs)
                            {

                                lists.Add(NextMatch.Groups[2].Value);

                            }


                            if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                break;


                            foreach (string list in lists)
                            {

                                if (pubVariables.status == false)
                                    return;
                                autoEvent.WaitOne();  //阻塞当前线程，等待通知以继续执行

                                String Url1 = list;
                                string strhtml = method.GetAliUrl(Url1, pub.aliCookie);  //定义的GetRul方法 返回 reader.ReadToEnd()

                                string Rxg = @"compname"">([\s\S]*?)</a>";
                                string Rxg1 = @"data-no=""([\s\S]*?)""";
                                string Rxg2 = @"class=""membername"" target=""_blank"">([\s\S]*?)    ";

                                string Rxg3 = @"所在地区：</label>([\s\S]*?)</span>";
                                string Rxg4 = @"data-encodeid=""([\s\S]*?)""";
                                string Rxg5 = @"经营模式：</label>([\s\S]*?)</span>";





                                Match name = Regex.Match(strhtml, Rxg);
                                Match tell = Regex.Match(strhtml, Rxg1);
                                Match lxr = Regex.Match(strhtml, Rxg2);
                                Match area = Regex.Match(strhtml, Rxg3);
                                Match wangwang = Regex.Match(strhtml, Rxg4);
                                Match moshi = Regex.Match(strhtml, Rxg5);


                                pub.index = dgv.Rows.Add();

                                dgv.Rows[pub.index].Cells[0].Value = name.Groups[1].Value;
                                dgv.CurrentCell = dgv.Rows[pub.index].Cells[0];  //让datagridview滚动到当前行
                                dgv.Rows[pub.index].Cells[2].Value = tell.Groups[1].Value;
                                dgv.Rows[pub.index].Cells[1].Value = lxr.Groups[1].Value.Replace("</a>&nbsp;", "").Replace("&nbsp;", "");

                                string temp = Regex.Replace(area.Groups[1].Value, "<[^>]*>", "");   //去除所有标签去除所有标签去除所有标签去除所有标签去除所有标签去除所有标签去除所有标签

                                string temp1 = Regex.Replace(temp, "\\s+", "");     //去掉空格去掉空格去掉空格去掉空格去掉空格去掉空格去掉空格去掉空格去掉空格
                                dgv.Rows[pub.index].Cells[3].Value = temp1;



                                dgv.Rows[pub.index].Cells[4].Value = wangwang.Groups[1].Value;


                                string temp2 = Regex.Replace(moshi.Groups[1].Value, "<[^>]*>", "");   //去除所有标签去除所有标签去除所有标签去除所有标签去除所有标签去除所有标签去除所有标签

                                string temp3 = Regex.Replace(temp2, "\\s+", "");     //去掉空格去掉空格去掉空格去掉空格去掉空格去掉空格去掉空格去掉空格去掉空格

                                dgv.Rows[pub.index].Cells[5].Value = temp3;


                                dgv.Rows[pub.index].Cells[6].Value = Url1;


                                //存入数据库开始
                                string[] values = { dgv.Rows[pub.index].Cells[0].Value.ToString(),
                                dgv.Rows[pub.index].Cells[1].Value.ToString(),
                                dgv.Rows[pub.index].Cells[2].Value.ToString(),
                                dgv.Rows[pub.index].Cells[3].Value.ToString(),
                                dgv.Rows[pub.index].Cells[4].Value.ToString(),
                                dgv.Rows[pub.index].Cells[5].Value.ToString(),
                                dgv.Rows[pub.index].Cells[6].Value.ToString(),
                                


                            };

                                string sql = "INSERT INTO result VALUES( '" + values[0] + "','" + values[1] + "','" + values[2] + "','" + values[3] + "','" + values[4] + "','" + values[5] + "','" + values[6] + "')";

                                method.insertData("阿里巴巴供应商企业采集", sql);
                                //存入数据库结束

                                Application.DoEvents();
                                System.Threading.Thread.Sleep(1000);   //内容获取间隔，可变量

                            }

                        }

                    }
                }
            }


            catch (System.Exception ex)
            {

                pubVariables.exs=ex.ToString();
            }

        }



        /// <summary>
        /// 58同城招聘企业搜索采集
        /// </summary>
        /// <param name="dgv1"></param>
        public void company_58(object dgv1)
        {
            DataGridView dgv = (DataGridView)dgv1;
            string[] headers = { "企业名称", "联系人", "电话", "地址", "职位" };

            method.setDatagridview(dgv, 5, headers);
            method.CreateTable("58同城招聘企业", headers); //创建数据表
            try
            {

                string[] citys = pubVariables.citys.Split(new string[] { "," }, StringSplitOptions.None);

                string[] keywords = pubVariables.keywords.Split(new string[] { "," }, StringSplitOptions.None);





                foreach (string city in citys)
                {
                    string citypinyin = method.Get58pinyin(city);
                    foreach (string keyword1 in keywords)
                    {
                        string keyword = System.Web.HttpUtility.UrlEncode(keyword1, System.Text.Encoding.GetEncoding("utf-8"));

                        for (int i = 1; i < 71; i++)

                        {

                            String Url = "http://" + citypinyin + ".58.com/job/pn" + i + "/?key=" + keyword + "&final=1&jump=1";

                            string html = method.GetUrl(Url);


                            MatchCollection TitleMatchs = Regex.Matches(html, @"<a href=""http://qy.58.com([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);


                            ArrayList lists = new ArrayList();

                            foreach (Match NextMatch in TitleMatchs)

                            {
                                lists.Add("http://qy.m.58.com/m_detail/" + NextMatch.Groups[1].Value);

                            }
                            if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                break;

                            foreach (string list in lists)

                            {

                                if (pubVariables.status == false)
                                    return;
                                autoEvent.WaitOne();  //阻塞当前线程，等待通知以继续执行
                                pub.index = dgv.Rows.Add();

                                String Url1 = list;
                                string strhtml = method.GetUrl(Url1);  //定义的GetRul方法 返回 reader.ReadToEnd()


                                string rxg = @"<h1>([\s\S]*?)</h1>";    //公司
                                string Rxg = @"<a href=""tel:([\s\S]*?)""";                                    //电话
                                string Rxg1 = @"</span><span>([\s\S]*?)</span>";                                    //联系人
                                string Rxg2 = @"<dt>公司地址：</dt>([\s\S]*?)</dd>";
                                string Rxg3 = @"<div class=""retTit""><strong>([\s\S]*?)</strong>";




                                Match company = Regex.Match(strhtml, rxg);
                                Match tel = Regex.Match(strhtml, Rxg);
                                Match contacts = Regex.Match(strhtml, Rxg1);
                                Match addr = Regex.Match(strhtml, Rxg2);
                                Match job = Regex.Match(strhtml, Rxg3);



                                dgv.Rows[pub.index].Cells[0].Value = company.Groups[1].Value;
                                dgv.CurrentCell = dgv.Rows[pub.index].Cells[0];  //让datagridview滚动到当前行

                                dgv.Rows[pub.index].Cells[1].Value = contacts.Groups[1].Value;
                                dgv.Rows[pub.index].Cells[2].Value = tel.Groups[1].Value;

                                string temp = Regex.Replace(addr.Groups[1].Value, "<[^>]*>", "");   //去除所有标签去除所有标签                         
                                dgv.Rows[pub.index].Cells[3].Value = temp;
                                dgv.Rows[pub.index].Cells[4].Value = job.Groups[1].Value;



                                dgv.CurrentCell = dgv.Rows[pub.index].Cells[0];  //让datagridview滚动到当前行

                                //存入数据库开始
                                string[] values = { dgv.Rows[pub.index].Cells[0].Value.ToString(),
                                dgv.Rows[pub.index].Cells[1].Value.ToString(),
                                dgv.Rows[pub.index].Cells[2].Value.ToString(),
                                dgv.Rows[pub.index].Cells[3].Value.ToString(),
                                dgv.Rows[pub.index].Cells[4].Value.ToString()


                                                 };

                                string sql = "INSERT INTO result VALUES( '" + values[0] + "','" + values[1] + "','" + values[2] + "','" + values[3] + "','" + values[4] + "')";

                                method.insertData("58同城招聘企业", sql);
                                //存入数据库结束
                                Application.DoEvents();
                                System.Threading.Thread.Sleep(1000);

                            }
                        }
                    }

                }

            }
            catch (System.Exception ex)
            {
                pubVariables.exs=ex.ToString();
            }

        }


        /// <summary>
        /// 黄页88
        /// </summary>
        /// <param name="dgv1"></param>
        public void hy88(object dgv1)
        {
            DataGridView dgv = (DataGridView)dgv1;
            string[] headers = { "企业名称", "联系人", "电话", "介绍" };

            method.setDatagridview(dgv, 4, headers);
            method.CreateTable("黄页88企业采集", headers); //创建数据表
            try
            {

                string[] citys = pubVariables.citys.Split(new string[] { "," }, StringSplitOptions.None);
                string[] keywords = pubVariables.keywords.Split(new string[] { "," }, StringSplitOptions.None);


               

 

                foreach (string city1 in citys)
                {
                    string city = method.Gethy88pinyin(city1);

                    foreach (string keyword in keywords)
                    {

                        string item = method.gethy88itemCode(keyword);

                        if (item == "")
                        {
                            MessageBox.Show("请选择分类！");
                            return;
                        }
                        for (int i = 1; i <= 50; i++)

                        {


                        String Url = "http://b2b.huangye88.com/" + city + "/" + item + "/pn" + i + "/";

                       
                        string html = method.GetUrl(Url);

                        MatchCollection TitleMatchs = Regex.Matches(html, @"<h4><a href=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);


                        ArrayList lists = new ArrayList();

                        foreach (Match NextMatch in TitleMatchs)

                        {
                            lists.Add(NextMatch.Groups[1].Value);

                        }

                        if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                            break;

                        foreach (string list in lists)

                        {
                            if (pubVariables.status == false)
                                return;
                            autoEvent.WaitOne();  //阻塞当前线程，等待通知以继续执行
                            pub.index = dgv.Rows.Add();
                            string strhtml = method.GetUrl(list);  //定义的GetRul方法 返回 reader.ReadToEnd()


                            string rxg = @"<dl class=""bottom"">([\s\S]*?)名称：([\s\S]*?)<";            //公司
                            string Rxg = @"<dl class=""bottom"">([\s\S]*?)手机：([\s\S]*?)<";           //电话
                            string Rxg1 = @"联系人：([\s\S]*?)rel=""nofollow"">([\s\S]*?)</a>";          //联系人
                            string Rxg2 = @"<meta name=""Description"" content=""([\s\S]*?)""";          //介绍



                            Match company = Regex.Match(strhtml, rxg);
                            Match tel = Regex.Match(strhtml, Rxg);
                            Match contacts = Regex.Match(strhtml, Rxg1);
                            Match introduction = Regex.Match(strhtml, Rxg2);

                            dgv.Rows[pub.index].Cells[0].Value = company.Groups[2].Value;


                            dgv.CurrentCell = dgv.Rows[pub.index].Cells[0];  //让datagridview滚动到当前行

                            dgv.Rows[pub.index].Cells[1].Value = contacts.Groups[2].Value;
                            dgv.Rows[pub.index].Cells[2].Value = tel.Groups[2].Value;


                            dgv.Rows[pub.index].Cells[3].Value = introduction.Groups[1].Value;

                            //存入数据库开始
                            string[] values = { dgv.Rows[pub.index].Cells[0].Value.ToString(),
                                dgv.Rows[pub.index].Cells[1].Value.ToString(),
                                dgv.Rows[pub.index].Cells[2].Value.ToString(),
                                dgv.Rows[pub.index].Cells[3].Value.ToString(),
                                

                                                 };

                            string sql = "INSERT INTO result VALUES( '" + values[0] + "','" + values[1] + "','" + values[2] + "','" + values[3] + "')";

                            method.insertData("黄页88企业采集", sql);
                            //存入数据库结束

                            Application.DoEvents();
                            System.Threading.Thread.Sleep(Convert.ToInt32(500));   //内容获取间隔，可变量


                        }

                    }

                }

            }
         }
            catch (System.Exception ex)
            {
                pubVariables.exs=ex.ToString();
            }

        }


        /// <summary>
        /// 百度地图采集
        /// </summary>
        /// <param name="dgv1"></param>
        public void baiduMap(object dgv1)

        {
            DataGridView dgv = (DataGridView)dgv1;
            string[] headers = { "企业名称", "地址", "电话", "关键词", "区域" };

            method.setDatagridview(dgv, 5, headers);
            method.CreateTable("百度地图", headers); //创建数据表
            string[] citys = pubVariables.citys.Split(new string[] { "," }, StringSplitOptions.None);

            string[] keywords = pubVariables.keywords.Split(new string[] { "," }, StringSplitOptions.None);
            try

            {


                int pages = 100;


                foreach (string city in citys)

                {
                    int cityid = method.getcityId(city + "市");  //获取 citycode;

                    foreach (string keyword in keywords)

                    {

                        for (int i = 0; i <= pages; i++)

                        {


                            String Url = "https://map.baidu.com/?newmap=1&reqflag=pcmap&biz=1&from=webmap&da_par=direct&pcevaname=pc4.1&qt=con&from=webmap&c=" + cityid + "&wd=" + keyword + "&wd2=&pn=" + i + "&nn=" + i + "0&db=0&sug=0&addr=0&&da_src=pcmappg.poi.page&on_gel=1&src=7&gr=3&l=13&tn=B_NORMAL_MAP&u_loc=13167420,3999298&ie=utf-8";

                            string html = method.GetUrl(Url);

                            //var obj = JsonConvert.DeserializeObject(html);json转html对象

                            //string html2 = obj.ToString(); html对象转字符串           

                            MatchCollection TitleMatchs = Regex.Matches(html, @"""primary_uid"":""([\s\S]*?)""", RegexOptions.IgnoreCase);

                            ArrayList lists = new ArrayList();

                            foreach (Match NextMatch in TitleMatchs)
                            {


                                lists.Add(NextMatch.Groups[1].Value);

                            }
                            if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                break;



                            foreach (string uid in lists)

                            {
                                if (pubVariables.status == false)
                                    return;
                                autoEvent.WaitOne();  //阻塞当前线程，等待通知以继续执行
                                pub.index = dgv.Rows.Add();

                                string Url1 = "http://map.baidu.com/detail?qt=ninf&uid=" + uid + "&detail=cater";
                                string strhtml = method.GetUrl(Url1);


                                string rxg = @"<title>([\s\S]*?)</title>";
                                string Rxg = @"""phone"":""([\s\S]*?)""";
                                string Rxg1 = @"<i class=""icon sp-adress""></i>([\s\S]*?)</p>";




                                Match title = Regex.Match(strhtml, rxg);
                                Match tell = Regex.Match(strhtml, Rxg);
                                Match address = Regex.Match(strhtml, Rxg1);


                                dgv.Rows[pub.index].Cells[0].Value = title.Groups[1].Value;
                                dgv.CurrentCell = dgv.Rows[pub.index].Cells[0];
                                dgv.Rows[pub.index].Cells[1].Value = address.Groups[1].Value.Trim();
                                dgv.Rows[pub.index].Cells[2].Value = tell.Groups[1].Value;

                                dgv.Rows[pub.index].Cells[3].Value = keyword.Trim();
                                dgv.Rows[pub.index].Cells[4].Value = city;

                                //存入数据库开始
                                string[] values = { dgv.Rows[pub.index].Cells[0].Value.ToString(),
                                dgv.Rows[pub.index].Cells[1].Value.ToString(),
                                dgv.Rows[pub.index].Cells[2].Value.ToString(),
                                dgv.Rows[pub.index].Cells[3].Value.ToString(),
                                dgv.Rows[pub.index].Cells[4].Value.ToString()


                                                 };

                                string sql = "INSERT INTO result VALUES( '" + values[0] + "','" + values[1] + "','" + values[2] + "','" + values[3] + "','" + values[4] + "')";

                                method.insertData("百度地图", sql);
                                //存入数据库结束

                                Application.DoEvents();
                                Thread.Sleep(100);   //内容获取间隔，可变量
                            }

                        }


                    }
                }
            }

            catch (System.Exception ex)
            {

                pubVariables.exs=ex.ToString();
            }

        }



        /// <summary>
        /// 腾讯地图采集
        /// </summary>
        /// <param name="dgv1"></param>
        public void txMap(object dgv1)

        {
            DataGridView dgv = (DataGridView)dgv1;
            string[] headers = { "企业名称", "地址", "电话", "关键词", "区域" };

            method.setDatagridview(dgv, 5, headers);
            method.CreateTable("腾讯地图", headers); //创建数据表
            try

            {
                string[] citys = pubVariables.citys.Split(new string[] { "," }, StringSplitOptions.None);

                string[] keywords = pubVariables.keywords.Split(new string[] { "," }, StringSplitOptions.None);

                int pages = 250;

                foreach (string city in citys)

                {

                    string cityutf8 = System.Web.HttpUtility.UrlEncode(city, System.Text.Encoding.GetEncoding("utf-8"));
                    foreach (string keyword in keywords)

                    {
                        string keywordutf8 = System.Web.HttpUtility.UrlEncode(keyword, System.Text.Encoding.GetEncoding("utf-8"));
                        for (int i = 0; i <= pages; i++)


                        {

                            String Url = "http://map.qq.com/m/place/result/city=" + cityutf8 + "&word=" + keywordutf8 + "&bound=&page=" + i + "&cpos=&mode=list";


                            string html = method.GetUrl(Url);


                            MatchCollection TitleMatchs = Regex.Matches(html, @"poid=([\s\S]*?)""", RegexOptions.IgnoreCase);

                            ArrayList lists = new ArrayList();

                            foreach (Match NextMatch in TitleMatchs)
                            {


                                lists.Add(NextMatch.Groups[1].Value);

                            }
                            if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                break;

                            string tm1 = DateTime.Now.ToString();  //获取系统时间



                            foreach (string poid in lists)

                            {
                                pub.index = dgv.Rows.Add();

                                string Url1 = "http://map.qq.com/m/detail/poi/poid=" + poid;
                                string strhtml = method.GetUrl(Url1);


                                string title = @"<div class=""poiDetailTitle "">([\s\S]*?)</div>";
                                string Rxg = @"<a href=""tel:([\s\S]*?)""";
                                string Rxg1 = @"span class=""poiDetailAddrTxt"">([\s\S]*?)</span>";



                                Match titles = Regex.Match(strhtml, title);
                                Match tell = Regex.Match(strhtml, Rxg);
                                Match address = Regex.Match(strhtml, Rxg1);


                                if (tell.Groups[1].Value.Trim() != "")

                                {
                                    if (pubVariables.status == false)
                                        return;
                                    autoEvent.WaitOne();  //阻塞当前线程，等待通知以继续执行

                                    dgv.Rows[pub.index].Cells[0].Value = titles.Groups[1].Value;
                                    dgv.CurrentCell = dgv.Rows[pub.index].Cells[0];

                                    dgv.Rows[pub.index].Cells[1].Value = address.Groups[1].Value;

                                    dgv.Rows[pub.index].Cells[2].Value += tell.Groups[1].Value.Trim() + ",";

                                    dgv.Rows[pub.index].Cells[3].Value = keyword;
                                    dgv.Rows[pub.index].Cells[4].Value = city;
                                }
                                //存入数据库开始
                                string[] values = {

                                dgv.Rows[pub.index].Cells[0].Value.ToString(),
                                dgv.Rows[pub.index].Cells[1].Value.ToString(),
                                dgv.Rows[pub.index].Cells[2].Value.ToString(),
                                dgv.Rows[pub.index].Cells[3].Value.ToString(),
                                dgv.Rows[pub.index].Cells[4].Value.ToString()


                                                 };

                                string sql = "INSERT INTO result VALUES( '" + values[0] + "','" + values[1] + "','" + values[2] + "','" + values[3] + "','" + values[4] + "')";

                                method.insertData("腾讯地图", sql);
                                //存入数据库结束

                                Application.DoEvents();
                                Thread.Sleep(500);
                            }

                        }


                    }
                }
            }

            catch (System.Exception ex)
            {

                pubVariables.exs=ex.ToString();

            }

        }



        /// <summary>
        /// 搜狗地图采集
        /// </summary>
        /// <param name="dgv1"></param>
        public void sougouMap(object dgv1)

        {
            DataGridView dgv = (DataGridView)dgv1;
            string cookie = "CXID=D44C63D34623066DA36D11B4B82C488C; SUV=1518337255419884; SMAPUVID=1518337255419884; SUV=1801190926013760; IPLOC=CN3213; sct=1; SNUID=D67B2A4373761425DCE0226F733E2FA7; ad=zMGYjlllll2zYIpclllllVr1fv6lllllGq6poyllll9llllljZlll5@@@@@@@@@@; SUID=A10659313565860A5A6291B800040AA1; wP_w=544ebe2c0329~HXcgwyvcH_c_5BDcHXULrZvNyX9XwbmSJXPsNNDN3XNBbbDJbdNhb; activecity=%u5BBF%u8FC1%2C13168867%2C3999623%2C12; ho_co=";
            string url = "http://map.sogou.com/EngineV6/search/json";

            string charset = "gb2312";
            try

            {
                string[] headers = { "企业名称", "地址", "电话", "关键词", "区域" };

                method.setDatagridview(dgv, 5, headers);
                method.CreateTable("搜狗地图", headers); //创建数据表
                string[] citys = pubVariables.citys.Split(new string[] { "," }, StringSplitOptions.None);

                string[] keywords = pubVariables.keywords.Split(new string[] { "," }, StringSplitOptions.None);



                int pages = 200;

                foreach (string city in citys)

                {
                    //搜索的城市和关键词都需要两次url编码

                    string city1 = System.Web.HttpUtility.UrlEncode(city, System.Text.Encoding.GetEncoding("utf-8"));
                    string cityutf8 = System.Web.HttpUtility.UrlEncode(city1, System.Text.Encoding.GetEncoding("utf-8"));
                    foreach (string keyword in keywords)

                    {
                        string keyword1 = System.Web.HttpUtility.UrlEncode(keyword, System.Text.Encoding.GetEncoding("utf-8"));
                        string keywordutf8 = System.Web.HttpUtility.UrlEncode(keyword1, System.Text.Encoding.GetEncoding("utf-8"));

                        for (int i = 1; i <= pages; i++)
                        {

                            string postData = "what=keyword%3A" + keywordutf8 + "&range=bound%3A00000000.5%2C0000000.5%2C99999999.5%2C9999999.5%3A0&othercityflag=1&appid=1361&thiscity=" + cityutf8 + "&lastcity=" + cityutf8 + "&userdata=3&encrypt=1&pageinfo=" + i + "%2C10&locationsort=0&version=7.0&ad=0&level=12&exact=1&type=&attr=&order=&submittime=0&resultTypes=poi&sort=0&reqid=1526008949358471&cb=parent.IFMS.search";

                            string html = method.PostUrl(url, postData, cookie, charset);


                            MatchCollection names = Regex.Matches(html, @"""poidesc"":""([\s\S]*?)""", RegexOptions.IgnoreCase);
                            MatchCollection address = Regex.Matches(html, @"""address"":""([\s\S]*?)""", RegexOptions.IgnoreCase);
                            MatchCollection phones = Regex.Matches(html, @"""phone"":""([\s\S]*?)""", RegexOptions.IgnoreCase);

                            for (int j = 0; j < names.Count; j++)
                            {
                                if (pubVariables.status == false)
                                    return;
                                autoEvent.WaitOne();  //阻塞当前线程，等待通知以继续执行
                                pub.index = dgv.Rows.Add();
                                dgv.Rows[pub.index].Cells[0].Value = names[j].Groups[1].Value;
                                dgv.Rows[pub.index].Cells[1].Value = address[j].Groups[1].Value;
                                dgv.Rows[pub.index].Cells[2].Value = phones[j].Groups[1].Value;
                                dgv.Rows[pub.index].Cells[3].Value = keyword;
                                dgv.Rows[pub.index].Cells[4].Value = city;
                                dgv.CurrentCell = dgv.Rows[pub.index].Cells[0];
                            }
                            //存入数据库开始
                            string[] values = { dgv.Rows[pub.index].Cells[0].Value.ToString(),
                                dgv.Rows[pub.index].Cells[1].Value.ToString(),
                                dgv.Rows[pub.index].Cells[2].Value.ToString(),
                                dgv.Rows[pub.index].Cells[3].Value.ToString(),
                                dgv.Rows[pub.index].Cells[4].Value.ToString()


                                                 };

                            string sql = "INSERT INTO result VALUES( '" + values[0] + "','" + values[1] + "','" + values[2] + "','" + values[3] + "','" + values[4] + "')";

                            method.insertData("搜狗地图", sql);
                            //存入数据库结束
                            Application.DoEvents();
                            Thread.Sleep(800);   //内容获取间隔，可变量


                        }


                    }
                }
            }

            catch (System.Exception ex)
            {
                pubVariables.exs=ex.ToString();
            }

        }



        /// <summary>
        /// 360地图采集
        /// </summary>
        /// <param name="dgv1"></param>
        public void map360(object dgv1)

        {
            DataGridView dgv = (DataGridView)dgv1;
            try

            {
                string[] headers = { "企业名称", "地址", "电话", "关键词", "区域" };

                method.setDatagridview(dgv, 5, headers);
                method.CreateTable("360地图", headers); //创建数据表
                string[] citys = pubVariables.citys.Split(new string[] { "," }, StringSplitOptions.None);

                string[] keywords = pubVariables.keywords.Split(new string[] { "," }, StringSplitOptions.None);




                int pages = 100;

                foreach (string city in citys)

                {
                    string cityutf8 = System.Web.HttpUtility.UrlEncode(city, System.Text.Encoding.GetEncoding("utf-8"));
                    foreach (string keyword in keywords)

                    {
                        string keywordutf8 = System.Web.HttpUtility.UrlEncode(keyword, System.Text.Encoding.GetEncoding("utf-8"));
                        for (int i = 1; i <= pages; i++)



                        {


                            String Url = "https://ditu.so.com/app/pit?jsoncallback=jQuery18308131636402501483_1525852464213&keyword=" + keywordutf8 + "&cityname=" + cityutf8 + "&batch=" + i + "%2c" + (i + 1) + "%2c" + (i + 2) + "%2c" + (i + 3) + "%2c" + (i + 4) + "&number=10";



                            string html = method.GetUrl(Url);


                            MatchCollection names = Regex.Matches(html, @"""name"":""([\s\S]*?)""", RegexOptions.IgnoreCase);
                            MatchCollection address = Regex.Matches(html, @"""address"":""([\s\S]*?)""", RegexOptions.IgnoreCase);
                            MatchCollection tels = Regex.Matches(html, @"""tel"":""([\s\S]*?)""", RegexOptions.IgnoreCase);

                            for (int j = 0; j < names.Count; j++)
                            {
                                if (pubVariables.status == false)
                                    return;
                                autoEvent.WaitOne();  //阻塞当前线程，等待通知以继续执行
                                pub.index = dgv.Rows.Add();
                                dgv.Rows[pub.index].Cells[0].Value = method.UnicodeToString(names[j].Groups[1].Value);
                                dgv.Rows[pub.index].Cells[1].Value = method.UnicodeToString(address[j].Groups[1].Value);
                                dgv.Rows[pub.index].Cells[2].Value = tels[j].Groups[1].Value;
                                dgv.Rows[pub.index].Cells[3].Value = keyword;
                                dgv.Rows[pub.index].Cells[4].Value = city;
                                dgv.CurrentCell = dgv.Rows[pub.index].Cells[0];
                            }
                            //存入数据库开始
                            string[] values = { dgv.Rows[pub.index].Cells[0].Value.ToString(),
                                dgv.Rows[pub.index].Cells[1].Value.ToString(),
                                dgv.Rows[pub.index].Cells[2].Value.ToString(),
                                dgv.Rows[pub.index].Cells[3].Value.ToString(),
                                dgv.Rows[pub.index].Cells[4].Value.ToString()


                                                 };

                            string sql = "INSERT INTO result VALUES( '" + values[0] + "','" + values[1] + "','" + values[2] + "','" + values[3] + "','" + values[4] + "')";

                            method.insertData("360地图", sql);
                            //存入数据库结束
                            Application.DoEvents();
                            Thread.Sleep(500);   //内容获取间隔，可变量


                        }


                    }
                }
            }

            catch (System.Exception ex)
            {

                pubVariables.exs=ex.ToString();
            }

        }



        /// <summary>
        /// 赶集本地服务
        /// </summary>
        /// <param name="dgv1"></param>
        public void ganji_bendi(object dgv1)
        {
            DataGridView dgv = (DataGridView)dgv1;
            string[] headers = { "企业名称", "行业", "电话", "地区" };

            method.setDatagridview(dgv, 4, headers);
            method.CreateTable("赶集网本地服务", headers); //创建数据表
            try
            {

                string[] citys = pubVariables.citys.Split(new string[] { "," }, StringSplitOptions.None);

                string[] keywords = pubVariables.keywords.Split(new string[] { "," }, StringSplitOptions.None);





                foreach (string city in citys)
                {
                    string citypinyin = method.ganjiCityPinyin(city);

                    foreach (string keyword in keywords)
                    {
                        string item = method.ganjiitempyin(keyword);

                        for (int i = 1; i < 70; i++)

                        {

                            String Url = "http://" + citypinyin + ".ganji.com/" + item + "/o" + i + "/";



                            string html = method.GetUrl(Url);

                            MatchCollection TitleMatchs = Regex.Matches(html, @"<!-- 商家网站 -->([\s\S]*?)click"">([\s\S]*?)</a>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                            MatchCollection names = Regex.Matches(html, @"<!-- 商家网站 -->([\s\S]*?)click"">([\s\S]*?)</a>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                            MatchCollection tells = Regex.Matches(html, @"data-phone=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);



                            for (int j = 0; j < names.Count; j++)
                            {
                                if (pubVariables.status == false)
                                    return;
                                autoEvent.WaitOne();  //阻塞当前线程，等待通知以继续执行
                                pub.index = dgv.Rows.Add();
                                dgv.Rows[pub.index].Cells[0].Value = names[j].Groups[2].Value;
                                dgv.Rows[pub.index].Cells[2].Value = tells[j].Groups[1].Value;
                                dgv.Rows[pub.index].Cells[1].Value = keyword;
                                dgv.Rows[pub.index].Cells[3].Value = city;
                                dgv.CurrentCell = dgv.Rows[pub.index].Cells[0];

                                //ListViewItem lv1 = listView1.Items.Add(names[j].Groups[2].Value);
                                //lv1.SubItems.Add(tells[j].Groups[1].Value);
                                //listView1.EnsureVisible(listView1.Items.Count - 1);  //滚动到指定位置


                                //存入数据库开始
                                string[] values = { dgv.Rows[pub.index].Cells[0].Value.ToString(),
                                dgv.Rows[pub.index].Cells[1].Value.ToString(),
                                dgv.Rows[pub.index].Cells[2].Value.ToString(),
                                dgv.Rows[pub.index].Cells[3].Value.ToString(),



                                                 };

                                string sql = "INSERT INTO result VALUES( '" + values[0] + "','" + values[1] + "','" + values[2] + "','" + values[3] + "')";

                                method.insertData("赶集网本地服务", sql);
                                //存入数据库结束

                            }


                            Application.DoEvents();
                            System.Threading.Thread.Sleep(1000);


                        }
                    }

                }

            }
            catch (System.Exception ex)
            {
                pubVariables.exs=ex.ToString();
            }

        }

        /// <summary>
        /// 赶集二手车
        /// </summary>
        /// <param name="dgv1"></param>
        public void ganji_ershouche(object dgv1)
        {
            DataGridView dgv = (DataGridView)dgv1;
            string[] headers = { "标题", "联系人", "电话", "地区" };

            method.setDatagridview(dgv, 4, headers);
            method.CreateTable("赶集网二手车", headers); //创建数据表
            try
            {

                string[] citys = pubVariables.citys.Split(new string[] { "," }, StringSplitOptions.None);


                foreach (string city in citys)
                {
                    string citypinyin = method.ganjiCityPinyin(city);



                    for (int i = 1; i < 70; i++)

                    {

                        String Url = "http://" + citypinyin + ".ganji.com/ershouche/a1o" + i + "/";

                        

                        string strhtml = method.GetUrl(Url);

                        MatchCollection TitleMatchs = Regex.Matches(strhtml, @"param=""href==([\s\S]*?)""");

                        ArrayList lists = new ArrayList();
                        foreach (Match match in TitleMatchs)
                        {
                            lists.Add(match.Groups[1].Value);
                        }

                        if (lists.Count == 0)
                                               
                            break;
                            
                        
                            
                        if (pubVariables.status == false)
                            return ;
                        // autoEvent.WaitOne();  //阻塞当前线程，等待通知以继续执行

                        string rxg1 = @"【([\s\S]*?)】";
                        string rxg2 = @"联系人：</label>([\s\S]*?)<";
                        string rxg3 = @"puid"":""([\s\S]*?)""";
                        string rxg4 = @"hash__ = \'([\s\S]*?)\'";

                        foreach(string list  in lists)
                        {
                            string html = method.GetUrl(list);
                        Match title = Regex.Match(html,rxg1);
                        Match lxr = Regex.Match(html, rxg2);
                        Match puid= Regex.Match(html, rxg3);
                        Match hash = Regex.Match(html, rxg4);


                        string url = "https://3g.ganji.com/ajax.php?dir=vehicle&module=get_phone400";
                        string postData = "puid="+puid.Groups[1].Value+"&key=";
                        string cookie = "ganji_xuuid=21af848e-22d1-4ef7-9885-c05d134a4690.1518531546343; ganji_uuid=9034648845650537098354; xxzl_deviceid=bahgtO60k4qAIekBP4XZl8FSWpJICUrI3lF8G3eFs9%2F7B79jjEHxUeOIliyeXXLL; 58uuid=938cde4b-4ffb-4556-a243-dfdc5a8f4202; als=0; __utmganji_v20110909=0xf1d75f82a6ea539b7abae047e63b0eb; gr_user_id=6311a343-9202-421d-b43a-a6d3249f2808; lg=1; ganji_fang_fzp_pc=1; WantedListPageScreenType=1920; gj_footprint=%5B%5B%22%5Cu4e8c%5Cu624b%5Cu623f%5Cu51fa%5Cu552e%22%2C%22http%3A%5C%2F%5C%2Fbj.ganji.com%5C%2Ffang5%5C%2F%22%5D%2C%5B%22%5Cu9500%5Cu552e%22%2C%22%5C%2Fzpshichangyingxiao%5C%2F%22%5D%2C%5B%22%5Cu79df%5Cu623f%22%2C%22http%3A%5C%2F%5C%2Fsh.ganji.com%5C%2Ffang1%5C%2F%22%5D%5D; ershoufangABTest=A; citydomain=sh; vehicle_list_view_type=1; ErshoucheDetailPageScreenType=1920; cityDomain=sh; UM_distinctid=1657b4f951318b-085ce1b3490508-762e6d31-f2e18-1657b4f9514435; ershouche_visit2=%5B%7B%22m%22%3A%221207%22%7D%5D; _gl_tracker=%7B%22ca_source%22%3A%22-%22%2C%22ca_name%22%3A%22-%22%2C%22ca_kw%22%3A%22-%22%2C%22ca_id%22%3A%22-%22%2C%22ca_s%22%3A%22self%22%2C%22ca_n%22%3A%22-%22%2C%22ca_i%22%3A%22-%22%2C%22sid%22%3A35542025155%7D; GANJISESSID=2t6llcbnev9nc7fg051ff6gp6j; Hm_lvt_8dba7bd668299d5dabbd8190f14e4d34=1535372038,1535421145; __utma=32156897.1817983647.1535355034.1535372041.1535421146.3; __utmc=32156897; __utmz=32156897.1535421146.3.3.utmcsr=sh.ganji.com|utmccn=(referral)|utmcmd=referral|utmcct=/; init_refer=http%253A%252F%252Fsh.ganji.com%252Fershouche%252Fa1%252F; new_uv=5; new_session=0; Hm_lpvt_8dba7bd668299d5dabbd8190f14e4d34=1535423129; ganji_login_act=1535423129113; __utmb=32156897.12.10.1535421146; mobversionbeta=3g; ershouche_post_browse_history=3623172797%2C3527293639%2C2914874652; ershoucheDetailHistory=3623172797-1215-161942; _wap__utmganji_wap_caInfo_V2=%7B%22ca_name%22%3A%22-%22%2C%22ca_source%22%3A%22-%22%2C%22ca_id%22%3A%22-%22%2C%22ca_kw%22%3A%22-%22%7D; _wap__utmganji_wap_newCaInfo_V2=%7B%22ca_n%22%3A%22-%22%2C%22ca_s%22%3A%22self%22%2C%22ca_i%22%3A%22-%22%7D; Hm_lvt_d486038d25d7a009c28de3dca11595e2=1535372074,1535423658; Hm_lpvt_d486038d25d7a009c28de3dca11595e2=1535423658; gr_session_id_b500fd00659c602c=76ed6d6d-47c2-42e9-9a83-2c5f76f99e04; gr_session_id_b500fd00659c602c_76ed6d6d-47c2-42e9-9a83-2c5f76f99e04=true; xzfzqtoken=lmLfjBoXmDei3fNdCQZrg9btT3Q0IbLE%2FNFA4Kg517LdZQkCMoR%2Bc4t7aI0xL3jpin35brBb%2F%2FeSODvMgkQULA%3D%3D";

                        string html2 = method.PostUrl(url, postData, cookie, "utf-8");
                            
                            
                        Match tell = Regex.Match(html2, @"phone:([\s\S]*?)""");


                        pub.index = dgv.Rows.Add();
                        dgv.Rows[pub.index].Cells[0].Value = title.Groups[1].Value;
                        dgv.Rows[pub.index].Cells[1].Value = lxr.Groups[1].Value.ToString().Replace("  ","").Trim();
                        dgv.Rows[pub.index].Cells[2].Value = tell.Groups[1].Value;
                        dgv.Rows[pub.index].Cells[3].Value = city;
                        dgv.CurrentCell = dgv.Rows[pub.index].Cells[0];


                        Application.DoEvents();
                        System.Threading.Thread.Sleep(1000);
                            
                        }
                    }

                }


            }
                 
            catch (System.Exception ex)
            {
                pubVariables.exs= ex.ToString();
            }

        }

        /// <summary>
        /// 马可波罗网
        /// </summary>
        /// <param name="dgv1"></param>
        public void makeboluo(object dgv1)
        {
            DataGridView dgv = (DataGridView)dgv1;
            try
            {
                string[] headers = { "企业名称", "联系人", "电话一", "电话二","地址","概述" };

                method.setDatagridview(dgv, 6, headers);

                method.CreateTable("马可波罗网采集", headers); //创建数据表

                string[] keywords = pubVariables.keywords.Split(new string[] { "," }, StringSplitOptions.None);
                string[] citys = pubVariables.citys.Split(new string[] { "," }, StringSplitOptions.None);

                foreach (string city in citys)

                {
                    

                    foreach (string keyword in keywords)

                    {

                        string keywordutf8 = System.Web.HttpUtility.UrlEncode(keyword, System.Text.Encoding.GetEncoding("utf-8"));

                        for (int i = 1; i < 100; i++)


                        {


                            string cookie = "Hm_lvt_7e7577ecbf4c96abade7fbcaa1d3b519=1535528938; _vid=C81ED5B0B3A0000125AD12BE586119CB; PHPSESSID=445f83599f8fb9fe4566edfd95dd57b0; hty_keyword=%7C%25u4E0D%25u9508%25u94A2%7C; __utma=162808035.639071119.1535529148.1535529148.1535529148.1; __utmc=162808035; __utmz=162808035.1535529148.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none); __utmt=1; __utmb=162808035.1.10.1535529148; search_key_cookie=%E4%B8%8D%E9%94%88%E9%92%A2; Hm_lpvt_7e7577ecbf4c96abade7fbcaa1d3b519=1535529581";
                            String Url = "http://caigou.makepolo.com/scw.php?pg=" + i + "&q="+keywordutf8+"&search_flag=q1&ae="+ method.GetMtpinyin(city);
                            
                            string strhtml = method.GetAliUrl(Url,cookie);  //定义的GetRul方法 返回 reader.ReadToEnd()

                            string Rxg = @"<div class=""h_com_btn""><a href=""([\s\S]*?)""";


                            MatchCollection all = Regex.Matches(strhtml, Rxg);

                            ArrayList lists = new ArrayList();
                            foreach (Match NextMatch in all)
                            {

                                lists.Add(NextMatch.Groups[1].Value);


                            }
                            if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                break;



                            foreach (string list in lists)
                            {
                                if (pubVariables.status == false)
                                    return;
                                autoEvent.WaitOne();  //阻塞当前线程，等待通知以继续执行
                                pub.index = dgv.Rows.Add();


                                string strhtml1 = method.GetUrl(list);  //定义的GetRul方法 返回 reader.ReadToEnd()



                                string Rxg0 = @"name=""corp_name"" value=""([\s\S]*?)""";
                                string Rxg1 = @"<li>联系人：([\s\S]*?)</li>";
                                string Rxg2 = @"<span class=""num_l"">([\s\S]*?)</span>";
                                string Rxg3 = @"<span class=""num_r"">([\s\S]*?)</span>";
                                string Rxg4 = @"<li>公司地址：([\s\S]*?)</li>";
                                string Rxg5 = @"<title>([\s\S]*?)</title>";



                                Match name = Regex.Match(strhtml1, Rxg0);

                                Match contacts = Regex.Match(strhtml1, Rxg1);


                                Match phone = Regex.Match(strhtml1, Rxg2);

                                Match tell = Regex.Match(strhtml1, Rxg3);
                                Match addr = Regex.Match(strhtml1, Rxg4);
                                Match title = Regex.Match(strhtml1, Rxg5);

                                dgv.Rows[pub.index].Cells[0].Value = name.Groups[1].Value;

                               
                                dgv.Rows[pub.index].Cells[1].Value = contacts.Groups[1].Value;

                                dgv.Rows[pub.index].Cells[2].Value = phone.Groups[1].Value;

                                dgv.Rows[pub.index].Cells[3].Value = tell.Groups[1].Value;

                                dgv.Rows[pub.index].Cells[4].Value = addr.Groups[1].Value.Trim();
                                dgv.Rows[pub.index].Cells[5].Value = title.Groups[1].Value.Trim();

                                dgv.CurrentCell = dgv.Rows[pub.index].Cells[0];  //让datagridview滚动到当前行

                                //存入数据库开始
                                string[] values = { dgv.Rows[pub.index].Cells[0].Value.ToString(),
                                dgv.Rows[pub.index].Cells[1].Value.ToString(),
                                dgv.Rows[pub.index].Cells[2].Value.ToString(),
                                dgv.Rows[pub.index].Cells[3].Value.ToString(),
                                dgv.Rows[pub.index].Cells[4].Value.ToString(),
                                dgv.Rows[pub.index].Cells[5].Value.ToString()


                                                 };

                                string sql = "INSERT INTO result VALUES( '" + values[0] + "','" + values[1] + "','" + values[2] + "','" + values[3] + "','" + values[4] + "','" + values[5] + "')";

                                method.insertData("马可波罗网采集", sql);
                                //存入数据库结束
                                Application.DoEvents();
                                System.Threading.Thread.Sleep(1000);



                            }

                        }

                    }

                }
            }




            catch (System.Exception ex)
            {
                pubVariables.exs=ex.ToString();
            }
        }


        /// <summary>
        /// 51搜了网
        /// </summary>
        /// <param name="dgv1"></param>
        public void sole51(object dgv1)
        {
            DataGridView dgv = (DataGridView)dgv1;
            try
            {
                string[] headers = { "企业名称", "联系人", "电话一", "电话二", "地址", "概述" };

                method.setDatagridview(dgv, 6, headers);


                method.CreateTable("51搜了网采集", headers); //创建数据表
                string[] keywords = pubVariables.keywords.Split(new string[] { "," }, StringSplitOptions.None);
                string[] citys = pubVariables.citys.Split(new string[] { "," }, StringSplitOptions.None);

                foreach (string city in citys)

                {


                    foreach (string keyword in keywords)

                    {

                        string keywordutf8 = System.Web.HttpUtility.UrlEncode(keyword, System.Text.Encoding.GetEncoding("utf-8"));

                        for (int i = 1; i <51; i++)


                        {
                    
                            String Url = "http://www.51sole.com/shenzhen-textile/p" + i+"/";

                            string strhtml = method.GetUrl(Url);  //定义的GetRul方法 返回 reader.ReadToEnd()

                            string Rxg = @"<li><span class=""fl""><a href=""([\s\S]*?)""";


                            MatchCollection all = Regex.Matches(strhtml, Rxg);

                            ArrayList lists = new ArrayList();
                            foreach (Match NextMatch in all)
                            {

                                lists.Add(NextMatch.Groups[1].Value);


                            }
                            if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                break;



                            foreach (string list in lists)
                            {
                                if (pubVariables.status == false)
                                    return;
                                autoEvent.WaitOne();  //阻塞当前线程，等待通知以继续执行
                                pub.index = dgv.Rows.Add();


                                string strhtml1 = method.GetUrl(list);  //定义的GetRul方法 返回 reader.ReadToEnd()



                                string Rxg0 = @"<li><b>([\s\S]*?)</b>";
                                string Rxg1 = @"联系人：</i><span>([\s\S]*?)</span>";
                                string Rxg2 = @"电话：</i><span>([\s\S]*?)</span>";
                                string Rxg3 = @"手机：</i><span>([\s\S]*?)</span>";
                                string Rxg4 = @"地址：</i><span>([\s\S]*?)</span>";
                                string Rxg5 = @"description""  content=""([\s\S]*?)""";



                                Match name = Regex.Match(strhtml1, Rxg0);

                                Match contacts = Regex.Match(strhtml1, Rxg1);


                                Match phone = Regex.Match(strhtml1, Rxg2);

                                Match tell = Regex.Match(strhtml1, Rxg3);
                                Match addr = Regex.Match(strhtml1, Rxg4);
                                Match title = Regex.Match(strhtml1, Rxg5);

                                dgv.Rows[pub.index].Cells[0].Value = name.Groups[1].Value.Trim();


                                dgv.Rows[pub.index].Cells[1].Value = contacts.Groups[1].Value.Trim();

                                dgv.Rows[pub.index].Cells[2].Value = phone.Groups[1].Value.Trim();

                                dgv.Rows[pub.index].Cells[3].Value = tell.Groups[1].Value.Trim();

                                dgv.Rows[pub.index].Cells[4].Value = addr.Groups[1].Value.Trim();
                                dgv.Rows[pub.index].Cells[5].Value = title.Groups[1].Value.Trim();

                                dgv.CurrentCell = dgv.Rows[pub.index].Cells[0];  //让datagridview滚动到当前行

                                //存入数据库开始
                                string[] values = { dgv.Rows[pub.index].Cells[0].Value.ToString(),
                                dgv.Rows[pub.index].Cells[1].Value.ToString(),
                                dgv.Rows[pub.index].Cells[2].Value.ToString(),
                                dgv.Rows[pub.index].Cells[3].Value.ToString(),
                                dgv.Rows[pub.index].Cells[4].Value.ToString(),
                                dgv.Rows[pub.index].Cells[5].Value.ToString()


                                                 };

                                string sql = "INSERT INTO result VALUES( '" + values[0] + "','" + values[1] + "','" + values[2] + "','" + values[3] + "','" + values[4] + "','" + values[5] + "')";

                                method.insertData("51搜了网采集", sql);
                                //存入数据库结束

                                Application.DoEvents();
                                System.Threading.Thread.Sleep(1000);



                            }

                        }

                    }

                }
            }




            catch (System.Exception ex)
            {
                pubVariables.exs=ex.ToString();
            }
        }


        /// <summary>
        /// 物友网
        /// </summary>
        /// <param name="dgv1"></param>
        public void wuyou(object dgv1)
        {
            DataGridView dgv = (DataGridView)dgv1;
            try
            {
                string[] headers = { "企业名称", "公司类型","成立时间","公司规模","注册资本", "联系人", "电话", "手机","地址" };

                method.setDatagridview(dgv, 9, headers);

                method.CreateTable("物友网企业采集", headers); //创建数据表

                string[] keywords = pubVariables.keywords.Split(new string[] { "," }, StringSplitOptions.None);
                string[] citys = pubVariables.citys.Split(new string[] { "," }, StringSplitOptions.None);

                foreach (string city in citys)

                {


                    foreach (string keyword in keywords)

                    {
                     

                        for (int i = 1; i < 51; i++)


                        {

                            String Url = "http://qiye.56ye.net/list-9142-2.html";

                            string strhtml = method.GetUrl(Url);  //定义的GetRul方法 返回 reader.ReadToEnd()

                            string Rxg = @"<td width=""1""><div><a href=""([\s\S]*?)""";


                            MatchCollection all = Regex.Matches(strhtml, Rxg);

                            ArrayList lists = new ArrayList();
                            foreach (Match NextMatch in all)
                            {

                                lists.Add(NextMatch.Groups[1].Value);


                            }
                            if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                break;



                            foreach (string list in lists)
                            {
                                if (pubVariables.status == false)
                                    return;
                                autoEvent.WaitOne();  //阻塞当前线程，等待通知以继续执行
                                pub.index = dgv.Rows.Add();


                                string strhtml1 = method.GetUrl(list);  //定义的GetRul方法 返回 reader.ReadToEnd()



                                string Rxg0 = @"<span class=""si-name"">([\s\S]*?)</span>";
                                string Rxg1 = @"<li>公司类型：([\s\S]*?)</li>";
                                string Rxg2 = @"<li>成立时间：([\s\S]*?)</li>";
                                string Rxg3 = @"<li>公司规模：([\s\S]*?)</li>";
                                string Rxg4 = @"<li>注册资本：([\s\S]*?)</li>";
                                string Rxg5 = @"<li>联系人：([\s\S]*?) ";
                                string Rxg6 = @">电话：([\s\S]*?)</li>";
                                string Rxg7 = @">手机：([\s\S]*?)</li>";
                                string Rxg8 = @">地址：([\s\S]*?)</li>";



                                Match name = Regex.Match(strhtml1, Rxg0);
                                Match type = Regex.Match(strhtml1, Rxg1);
                                Match time = Regex.Match(strhtml1, Rxg2);
                                Match guimo = Regex.Match(strhtml1, Rxg3);
                                Match ziben = Regex.Match(strhtml1, Rxg4);

                                Match contacts = Regex.Match(strhtml1, Rxg5);
                                Match phone = Regex.Match(strhtml1, Rxg6);

                                Match tell = Regex.Match(strhtml1, Rxg7);
                                Match addr = Regex.Match(strhtml1, Rxg8);
                                

                                dgv.Rows[pub.index].Cells[0].Value = name.Groups[1].Value.Trim();
                                dgv.Rows[pub.index].Cells[1].Value = type.Groups[1].Value.Trim();
                                dgv.Rows[pub.index].Cells[2].Value = time.Groups[1].Value.Trim();
                                dgv.Rows[pub.index].Cells[3].Value = guimo.Groups[1].Value.Trim();

                                dgv.Rows[pub.index].Cells[4].Value = ziben.Groups[1].Value.Trim();

                                dgv.Rows[pub.index].Cells[5].Value = contacts.Groups[1].Value.Trim();

                                dgv.Rows[pub.index].Cells[6].Value = phone.Groups[1].Value.Trim();

                                dgv.Rows[pub.index].Cells[7].Value = tell.Groups[1].Value.Trim();
                                dgv.Rows[pub.index].Cells[8].Value = addr.Groups[1].Value.Trim();

                                dgv.CurrentCell = dgv.Rows[pub.index].Cells[0];  //让datagridview滚动到当前行


                                //存入数据库开始
                                string[] values = { dgv.Rows[pub.index].Cells[0].Value.ToString(),
                                dgv.Rows[pub.index].Cells[1].Value.ToString(),
                                dgv.Rows[pub.index].Cells[2].Value.ToString(),
                                dgv.Rows[pub.index].Cells[3].Value.ToString(),
                                dgv.Rows[pub.index].Cells[4].Value.ToString(),
                                dgv.Rows[pub.index].Cells[5].Value.ToString(),
                                dgv.Rows[pub.index].Cells[6].Value.ToString(),
                                dgv.Rows[pub.index].Cells[7].Value.ToString(),
                                dgv.Rows[pub.index].Cells[8].Value.ToString(),


                                                 };

                                string sql = "INSERT INTO result VALUES( '" + values[0] + "','" + values[1] + "','" + values[2] + "','" + values[3] + "','" + values[4] + "','" + values[5] + "','" + values[6] + "','" + values[7] + "','" + values[8] + "')";

                                method.insertData("物友网企业采集", sql);
                                //存入数据库结束
                                Application.DoEvents();
                                System.Threading.Thread.Sleep(1000);



                            }

                        }

                    }

                }
            }




            catch (System.Exception ex)
            {
                pubVariables.exs = ex.ToString();
            }
        }



        /// <summary>
        /// 顺企网
        /// </summary>
        /// <param name="dgv1"></param>
        public void shunqi(object dgv1)
        {
            DataGridView dgv = (DataGridView)dgv1;
            string[] headers = { "企业名称", "联系人", "电话","手机", "地址" };

            method.setDatagridview(dgv, 5, headers);
            method.CreateTable("顺企网企业采集", headers); //创建数据表
            try
            {

                string[] citys = pubVariables.citys.Split(new string[] { "," }, StringSplitOptions.None);
                string[] keywords = pubVariables.keywords.Split(new string[] { "," }, StringSplitOptions.None);






                foreach (string city1 in citys)
                {
                    string city = method.Gethy88pinyin(city1);

                    foreach (string keyword in keywords)
                    {

                        string item = method.getshunqiItemId(keyword);

                        if (item == "")
                        {
                            MessageBox.Show("请选择分类！");
                            return;
                        }
                        for (int i = 1; i <21; i++)

                        {


                            String Url = "http://www.11467.com/"+city+"/search/"+item+"-"+i+".htm";


                            string html = method.GetUrl(Url);

                            MatchCollection TitleMatchs = Regex.Matches(html, @"<h4><a href=""([\s\S]*?)""", RegexOptions.IgnoreCase | RegexOptions.Multiline);


                            ArrayList lists = new ArrayList();

                            foreach (Match NextMatch in TitleMatchs)

                            {
                                lists.Add("http:"+NextMatch.Groups[1].Value);
                                
                            }

                            if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                                break;

                            foreach (string list in lists)

                            {
                                if (pubVariables.status == false)
                                    return;
                                autoEvent.WaitOne();  //阻塞当前线程，等待通知以继续执行
                                pub.index = dgv.Rows.Add();
                                string strhtml = method.GetUrl(list);  //定义的GetRul方法 返回 reader.ReadToEnd()


                                string rxg = @"<title>([\s\S]*?)</title>";            //公司
                                string Rxg = @"固定电话：</dt><dd>([\s\S]*?)</dd>";           //电话
                                string rxg1 = @"手机：</dt><dd>([\s\S]*?)</dd>";
                                string Rxg1 = @"经理：</dt><dd>([\s\S]*?)</dd>";          //联系人
                                string Rxg2 = @"公司地址：</dt><dd>([\s\S]*?)</dd>";          



                                Match company = Regex.Match(strhtml, rxg);
                                Match tel = Regex.Match(strhtml, Rxg);
                                Match phone = Regex.Match(strhtml, rxg1);
                                Match contacts = Regex.Match(strhtml, Rxg1);
                                Match introduction = Regex.Match(strhtml, Rxg2);

                                dgv.Rows[pub.index].Cells[0].Value = company.Groups[1].Value;


                                dgv.CurrentCell = dgv.Rows[pub.index].Cells[0];  //让datagridview滚动到当前行

                                dgv.Rows[pub.index].Cells[1].Value = contacts.Groups[1].Value;
                                dgv.Rows[pub.index].Cells[2].Value = tel.Groups[1].Value;
                                dgv.Rows[pub.index].Cells[3].Value = phone.Groups[1].Value;


                                dgv.Rows[pub.index].Cells[4].Value = introduction.Groups[1].Value;

                                //存入数据库开始
                                string[] values = { dgv.Rows[pub.index].Cells[0].Value.ToString(),
                                dgv.Rows[pub.index].Cells[1].Value.ToString(),
                                dgv.Rows[pub.index].Cells[2].Value.ToString(),
                                dgv.Rows[pub.index].Cells[3].Value.ToString(),
                                dgv.Rows[pub.index].Cells[4].Value.ToString(),


                                                 };

                                string sql = "INSERT INTO result VALUES( '" + values[0] + "','" + values[1] + "','" + values[2] + "','" + values[3] + "','" + values[4] + "')";

                                method.insertData("顺企网企业采集", sql);
                                //存入数据库结束

                                Application.DoEvents();
                                System.Threading.Thread.Sleep(Convert.ToInt32(2000));   //内容获取间隔，可变量


                            }

                        }

                    }

                }
            }
            catch (System.Exception ex)
            {
                pubVariables.exs = ex.ToString();
            }

        }





    }
}
