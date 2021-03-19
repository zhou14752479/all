using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 思忆美团
{
    class functions
    {
        string path = AppDomain.CurrentDomain.BaseDirectory;

        #region  读取分类
        public Dictionary<string, string> catedic = new Dictionary<string, string>();
        public void Getcates(ComboBox cob)
        {

            try
            {
                StreamReader sr = new StreamReader(path + "system\\cates.json", myDLL.method.EncodingType.GetTxtType(path + "system//cates.json"));
                //一次性读取完 
                string texts = sr.ReadToEnd();
                string html = Regex.Match(texts, @"cateData([\s\S]*?)\}\}\}").Groups[1].Value;
               
                MatchCollection cates = Regex.Matches(html, @"""id"":([\s\S]*?),""name"":""([\s\S]*?)""");
                for (int i = 0; i < cates.Count; i++)
                {
                    cob.Items.Add(cates[i].Groups[2].Value);
                    catedic.Add(cates[i].Groups[2].Value, cates[i].Groups[1].Value);
                }
                sr.Close();  //只关闭流
                sr.Dispose();   //销毁流内存
            }

            catch (System.Exception ex)
            {
                 ex.ToString();
            }

        }



        #endregion

        #region  获取城市ID

        public string GetcityId(string city)
        {

            try
            {
                string url = "https://apimobile.meituan.com/group/v1/area/search/" + System.Web.HttpUtility.UrlEncode(city);
                string html =  myDLL.method.GetUrl(url,"utf-8");
                Match cityId = Regex.Match(html, @"""cityId"":([\s\S]*?),");

                return cityId.Groups[1].Value;
            }

            catch (System.Exception ex)
            {
                return ex.ToString();
            }

        }



        #endregion

        #region 获取区域
        public ArrayList getareas(string city)
        {
            string Url = "https://" + city + ".meituan.com/meishi/";

            string html = myDLL.method.GetUrl(Url,"utf-8");  //定义的GetRul方法 返回 reader.ReadToEnd()

            MatchCollection areas = Regex.Matches(html, @"""subAreas"":\[\{""id"":([\s\S]*?),");
            ArrayList lists = new ArrayList();

            foreach (Match item in areas)
            {
                lists.Add(item.Groups[1].Value);
            }

            return lists;
        }

        #endregion
    }
}
