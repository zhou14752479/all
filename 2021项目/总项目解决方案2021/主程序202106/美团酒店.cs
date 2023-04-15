using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 主程序202106
{
    public partial class 美团酒店 : Form
    {
        public 美团酒店()
        {
            InitializeComponent();
        }
        Thread thread;
        bool zanting = true;
        bool status = true;

        #region GET请求
        public static string meituan_GetUrl(string Url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                //request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.97 Safari/537.11";
                request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 13_6_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148 MicroMessenger/8.0.4(0x1800042c) NetType/4G Language/zh_CN";
                WebHeaderCollection headers = request.Headers;
                //headers.Add("uuid: E82ADB4FE4B6D0984D5B1BEA4EE9DE13A16B4B25F8A306260A976B724DF44576");
                headers.Add("mtgsig: {\"a1\":\"1.1\",\"a2\":1634620588257,\"a3\":\"35y5z0zvw6yx5z4418uyu2uyvwx65w0782x076xu37688988u426z26y\",\"a4\":\"bc19900376f1b535039019bc35b5f176e250e32ad7e5125c\",\"a5\":\"JAa73Rfx72tnXVeYw8MDLxPZTTTerl4Af4me1ULhxiBetB6AXdLwp2+LjByyFY4HvTVzwiTHo+Vs58iZE6nq4BmhF0KabSOZyOlt6Xc0hYGOghvfxeOmtWIibqgvO9QIKazOwEWESNgYcAcAsgCYccIJ0ilyWJqgc+qG5RPNTQAx8j2vzKgSEcAhtM/xXMqw4IjqrBHST/WVB9/VV7KQ4vJj1ox1p6Dt\",\"a6\":\"w1.00tlrgKaQ8nF6hTsiCnWypNRjsnhPgjiZt1klOT118rN8SWnNfMJ4xKgbXs6lejv0g1y1GwSzNFDnVx0kOF2VQu5Ki7qmfklvvTwvNHxM3oVmA0pjXjMGAP1+0Fn59TSCPUyqbs9iBKzi/lP75X+7fQ368GCr6dk9ubOsKNE+l1A8OTEBFEovMQxgHRLctnAIjrJKrB9+xNS6v6D77d28saJDOUx5NjRu7ojTyv+06R8IpZcZDIphyvQdruyN2m3UEO8iGKbUxkAB9HWk2hnq5yMM3kim+Ai/OV/raiKcm05hgl+dCoHbvrB3zky80QUHO2gyiTQwSGLHw0DFUANPKhsCFgVmK9sYG0MlOafqQj0fyuNcK/MNjwNGtuOb/8gQBNJ8O0hbV1yS+VUktM8WJqMvg+DSmFX4zUU0RVS6wSZEscUjJ8MroYbdkHET6yqeP5HN44eA2F9ZHa2FkRa6apP+XGoAg7XoaQj518PQjgCDwbHD0RwskPolSnJ31BdoOWlSLDXB3BVxlOvzpQtKnuqt077PyOmXS4mXCJkxOURnd9elQz7RTL1aTP14KvP5RaZmvU3PaGEwhB1DfLvP6CfAwurTGMSXEJ+zMSfc5KXl+CIlqPSZYGhoj9rKg/XJB89nMlNmgIqKwK8cFJ7J4hEC7AEtoRBB6ll3Zh8OBDqZ+CbuTFadF3U4ABbsmjQQfqgof3dGTZJQVpBwL593Zx8PzHDveZm9erO0okGu1Sg=\",\"a7\":\"wxde8ac0a21135c07d\",\"x0\":3,\"d1\":\"a202f030aa475f308799f76cefc1ad7b\"}");
                headers.Add("openIdCipher: AwQAAABJAgAAAAEAAAAyAAAAPLgC95WH3MyqngAoyM/hf1hEoKrGdo0pJ5DI44e1wGF9AT3PH7Wes03actC2n/GVnwfURonD78PewMUppAAAADhq+X5+N+Cjq/cZyyWQkbVlw1zTBRltsV8Tsu1RC6Eq82jKTGdFzlq8MpEWZIJ53XNCHlmCUGib7Q==");

                request.Referer = "https://servicewechat.com/wxde8ac0a21135c07d/328/page-frame.html";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈

                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string content = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return content;

            }
            catch (System.Exception ex)
            {
                ex.ToString();



            }
            return "";
        }
        #endregion
        #region  获取城市ID

        public string GetcityId(string city)
        {

            try
            {
                string url = "https://apimobile.meituan.com/group/v1/area/search/" + System.Web.HttpUtility.UrlEncode(city);
                string html = method.GetUrl(url,"utf-8");
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
        public ArrayList getAreaId(string cityid)
        {

            ArrayList lists = new ArrayList();
            Dictionary<string, string> dics = new Dictionary<string, string>(); ;
            string Url = "https://i.meituan.com/wrapapi/search/filters?riskLevel=71&optimusCode=10&ci=" + cityid;

            string html = method.GetUrl(Url, "utf-8");  //定义的GetRul方法 返回 reader.ReadToEnd()

            MatchCollection areas = Regex.Matches(html, @"{""id"":([\s\S]*?),([\s\S]*?)""name"":""([\s\S]*?)""");


            for (int i = 0; i < areas.Count; i++)
            {

                if (areas[i].Groups[3].Value.Contains("区") || areas[i].Groups[3].Value.Contains("县"))
                {
                    if (!areas[i].Groups[3].Value.Contains("小区") && !areas[i].Groups[3].Value.Contains("街区") && !areas[i].Groups[3].Value.Contains("商业区") && !areas[i].Groups[3].Value.Contains("城区") && !areas[i].Groups[3].Value.Contains("市区") && !areas[i].Groups[3].Value.Contains("地区") && !areas[i].Groups[3].Value.Contains("社区") && areas[i].Groups[3].Value.Length < 5)
                    {
                        if (!dics.ContainsKey(areas[i].Groups[3].Value))
                        {
                            lists.Add(areas[i].Groups[1].Value);

                        }
                    }
                }

            }

            return lists;
        }

        #endregion


        #region 获取区域新
        public ArrayList getareas2(string cityid)
        {
            ArrayList lists = new ArrayList();

            string Url = "https://m.dianping.com/mtbeauty/index/ajax/loadnavigation?token=gORmhG3WtAc9Pfr4vTbhivSxQk0AAAAADA4AAPrp_ewNUU2qGaRBE9FjidEQTVrC4_z5BShh7mlouJWGaKp4u3_FM5r8Gh5U2I2LrQ&cityid=" + cityid + "&cateid=22&categoryids=22&lat=33.96271&lng=118.24239&userid=&uuid=oJVP50IRqKIIshugSqrvYE3OHJKQ&utm_source=meituan-wxapp&utmmedium=&utmterm=&utmcontent=&versionname=&utmcampaign=&mock=0&openid=oJVP50IRqKIIshugSqrvYE3OHJKQ&mtlite=false";

            //string html = meituan_GetUrl(Url);  //定义的GetRul方法 返回 reader.ReadToEnd()
            string html = method.GetUrl(Url,"utf-8");
          
            MatchCollection areas = Regex.Matches(html, @"""name"":""([\s\S]*?)"",""id"":([\s\S]*?),");
            
           
            for (int i = 0; i < areas.Count; i++)
            {

                if (areas[i].Groups[1].Value.Contains("区") || areas[i].Groups[1].Value.Contains("县"))
                {
                    if (!areas[i].Groups[1].Value.Contains("小区") && !areas[i].Groups[1].Value.Contains("街区") && !areas[i].Groups[1].Value.Contains("商业区") && !areas[i].Groups[1].Value.Contains("城区") && !areas[i].Groups[1].Value.Contains("市区") && !areas[i].Groups[1].Value.Contains("地区") && !areas[i].Groups[1].Value.Contains("社区") && areas[i].Groups[1].Value.Length < 5)
                    {
                        if (!lists.Contains(areas[i].Groups[2].Value))
                        {
                            lists.Add(areas[i].Groups[2].Value);

                        }
                    }
                }

            }

            return lists;
        }

        List<string> finishes = new List<string>();
        #endregion


        #region 筛选
        public string shaixuan(string tel)
        {
            try
            {
                string haoma = tel;
                string[] tels = tel.Split(new string[] { "\\" }, StringSplitOptions.None);

                if (checkBox1.Checked == true)
                {
                    if (tels.Length == 0)
                    {
                        haoma = "";
                        return haoma;
                    }

                }
                if (checkBox2.Checked == true)
                {
                    if (tels.Length == 1)
                    {
                        if (!tel.Contains("-") && tels[0].Length > 10)
                        {
                            haoma = tel;
                            return haoma;
                        }
                        else
                        {
                            return "";
                        }

                    }

                    if (tels.Length == 2)
                    {
                        if (!tels[0].Contains("-") && tels[0].Length > 10)
                        {
                            haoma = tels[0];
                        }

                        else if (!tels[1].Contains("-") && tels[1].Length > 10)
                        {
                            haoma = tels[1];
                        }
                        else
                        {
                            haoma = "";
                        }
                    }
                }
                if (checkBox3.Checked == true)
                {
                    finishes.Add(haoma);
                }
                return haoma.Trim();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(tel+"   " +ex.ToString());
                return "";
            }


        }
        #endregion

        string cookie = "ci=184; cityname=%E5%AE%BF%E8%BF%81; i_extend=C085021465567978133866654328195751632659_g1E129793523704877745831744257526160635882_c0_e1803825002331044728_anull_o1_dhotelpoitagb_k1002_f41795978Gempty__xhotelhomepage__yselect__zday; isid=AgH1IiCU8AvX8PQ6Fs-pSGkdPEKCFy8JLEKaE1tm-VBydcY4Gbib7265dvdAHlTTrHxr2pn9JqWs_QAAAADjFwAAZ0B3V1ndoiC-VgbIV27k2SKRzvnwmD5TbCeoKlH-rRrfoXNxZ47NKCF7UiAYShAz; mt_c_token=AgH1IiCU8AvX8PQ6Fs-pSGkdPEKCFy8JLEKaE1tm-VBydcY4Gbib7265dvdAHlTTrHxr2pn9JqWs_QAAAADjFwAAZ0B3V1ndoiC-VgbIV27k2SKRzvnwmD5TbCeoKlH-rRrfoXNxZ47NKCF7UiAYShAz; userId=875973616;  ";

        string pccookie = "_lxsdk_cuid=182ca747cffc8-0a3cbbc1e88c3-26021a51-384000-182ca747cffc8; uuid=2e860293968b4878a7ee.1681535926.1.0.0; WEBDFPID=7u498x04w6785v56y66wx45150x7w4yw812164z956797958v9xu3y9z-1996895928928-1681535928609IWACIOWfd79fef3d01d5e9aadc18ccd4d0c95071377; JSESSIONID=node0hdytv6x9q6n41j9sbh78m8a37215798.node0; IJSESSIONID=node0hdytv6x9q6n41j9sbh78m8a37215798; iuuid=0A259ACAAC178C65CAE584A0A82A7C1890C8F5199417179483B306F17BC8F769; _lxsdk=0A259ACAAC178C65CAE584A0A82A7C1890C8F5199417179483B306F17BC8F769; mtcdn=K; userTicket=ldiXmFTMeKrVtdaVsJagdPGcHysgSSvyfSTgEYHO; _yoda_verify_resp=MUTQ%2BUhVbsjk94yv2hZBXFzkYW30FMTj0T8KZVLvSuzWGAvY%2F7xNBC1DQHX0hwB60WIKJrfUePqhk3ISLnVOKmrTqOEkWrSbnfgZqxojndvsTfi5rcdvlZVe5pP%2Blb%2FNvJNSvI0dorVByHTuvKdNM7oO5uCAqBdKrI50YWPrPIveL7g%2B8t9Abun0%2BAM9wBTxUwgqDHlZgXANgVOvkTutwoA4Wg9WPB4QXYW0DimL2MZtVkFZ%2BhBsKcDAjwA0C7mpbfL095DSpfUYCDJUDzxlW9l1c2F1a6h0eHW5ZYFjbZ0W5lBv4E6wy1KDpg7Ok%2Bd6zesLYx%2BXbBv2Vu8MWCUydSlMniaYSdIKHBG82X3mUtezdooza572eIbnOqBiExkq; _yoda_verify_rid=16dbb3c80f024046; u=875973616; n=Ffv936639060; lt=AgHSJRmKTBhhG3PrAN7_uaNHPEEw1p74oeQa-vhxzwI0OUe-Byrbol-4xT6VBVMbTav8mHRy-tCk6QAAAADjFwAAjk5R4uR0VGFzTJZpsPbSIbl-eib7lRzwX0U7vmTt2hGuwxxCbgS3gJHX9eLt4BgU; mt_c_token=AgHSJRmKTBhhG3PrAN7_uaNHPEEw1p74oeQa-vhxzwI0OUe-Byrbol-4xT6VBVMbTav8mHRy-tCk6QAAAADjFwAAjk5R4uR0VGFzTJZpsPbSIbl-eib7lRzwX0U7vmTt2hGuwxxCbgS3gJHX9eLt4BgU; token=AgHSJRmKTBhhG3PrAN7_uaNHPEEw1p74oeQa-vhxzwI0OUe-Byrbol-4xT6VBVMbTav8mHRy-tCk6QAAAADjFwAAjk5R4uR0VGFzTJZpsPbSIbl-eib7lRzwX0U7vmTt2hGuwxxCbgS3gJHX9eLt4BgU; token2=AgHSJRmKTBhhG3PrAN7_uaNHPEEw1p74oeQa-vhxzwI0OUe-Byrbol-4xT6VBVMbTav8mHRy-tCk6QAAAADjFwAAjk5R4uR0VGFzTJZpsPbSIbl-eib7lRzwX0U7vmTt2hGuwxxCbgS3gJHX9eLt4BgU; isid=AgHSJRmKTBhhG3PrAN7_uaNHPEEw1p74oeQa-vhxzwI0OUe-Byrbol-4xT6VBVMbTav8mHRy-tCk6QAAAADjFwAAjk5R4uR0VGFzTJZpsPbSIbl-eib7lRzwX0U7vmTt2hGuwxxCbgS3gJHX9eLt4BgU; oops=AgHSJRmKTBhhG3PrAN7_uaNHPEEw1p74oeQa-vhxzwI0OUe-Byrbol-4xT6VBVMbTav8mHRy-tCk6QAAAADjFwAAjk5R4uR0VGFzTJZpsPbSIbl-eib7lRzwX0U7vmTt2hGuwxxCbgS3gJHX9eLt4BgU; logintype=normal; ci=1; rvct=1; firstTime=1681536077868; unc=Ffv936639060; cityname=%E5%8C%97%E4%BA%AC; _lxsdk_s=187835a07f7-f33-b6e-700%7C%7C24";
        



        public string  gettel(string city, string name)
        {
            string url = "https://m.ctrip.com/restapi/soa2/26872/search";
            string postdata = "action=online&source=globalonline&keyword=" + System.Web.HttpUtility.UrlEncode(city+name);
            string html = method.PostUrlDefault(url,postdata,cookie);
           
            string aurl= Regex.Match(html, @"""resultPageUrl"":""([\s\S]*?)""").Groups[1].Value;
            string ahtml=method.GetUrlWithCookie(aurl, "SECKEY_ABVK=lK2zXq5qE7tBp5pjIqSiUJiUl60x12OpVY6ZvFFXqmc%3D; BMAP_SECKEY=lK2zXq5qE7tBp5pjIqSiUAFeFjLBBTkcffBfZ5g2r2RmnbXi_OVkRLQRmgGcLOS0smKmE5P8PEQce4lBdPpid2tDPsSIeoErKvhDMtt0QrfpwBYQXWO18aC7EwLgrIx2MVb5vMDR0_XvWgG4wn1ruR1Sbij-lLhPbBy-w1Xjru5DFtYiVVGaKNHVmTlrjG1_; QN1=0000a380306c4f85df20901f; QN300=s%3Dbaidu; QN99=4896; QunarGlobal=10.67.200.16_22579bf7_187834b6c84_-60e0|1681536485018; qunar-assist={%22version%22:%2220211215173359.925%22%2C%22show%22:false%2C%22audio%22:false%2C%22speed%22:%22middle%22%2C%22zomm%22:1%2C%22cursor%22:false%2C%22pointer%22:false%2C%22bigtext%22:false%2C%22overead%22:false%2C%22readscreen%22:false%2C%22theme%22:%22default%22}; QN205=s%3Dbaidu; QN277=s%3Dbaidu; csrfToken=gJ8gaYAHNDPAnaEr2RtMu6CH73iVv3pT; QN601=8fd47e2d976fb0f18e164e8d5507ec9a; QN48=00008d002f104f85df28acba; QN163=0; QN269=4C54C7B1DB4E11EDBA87FA163E028184; _i=VInJOmWb60YqCV-qYzkYfSOPaGxq; fid=1690477f-bf38-498e-9bbd-30902c2686f9; HN1=v176a9a8e264442deee187f1447f252702; HN2=quuqszqnnusqn; ctt_june=1654604625968##iK3wWSPOaUPwawPwa%3DXmasaAaRoTERkTW%3DfTEKjNEKWhXSgAa%3DGhWKiDaPiRiK3siK3saKjmaKPsWSPsaRX%3DVuPwaUvt; _vi=aN5hcn_EVeZmigckyMyC522qzLeFL4GrqXlWLvKVlELSP1dIb9Y8x_eI-MDJVujjWjgIcc8cOnoO2ooH36NKX1jSi5WyKYoP6NSx4YVE7m-Jzsx7lDqlb2TrjypkbiMOClExzR8G7jYO5_bbHFb3YQN6ZWd_sZ3Tlvr9XT6eBpAa; QN267=1278839183b22753fc; QN25=302675d3-1962-4c00-aea1-5bb71be0e56d-9f992f90; ctf_june=1654604625968##iK3waKtAauPwawPwasa%3DaSXAXsoDWRXNWs2%2Ba%3D3wWStwXsawEPanWKj8XK0hiK3siK3saKjmaKPsWstwVRvwaUPwaUvt; ariaDefaultTheme=undefined; cs_june=22eef72cd875acd234069f84e073a13d2c16bd6af4cd05506a0c558a33bc095af9a1f66e9926451c91843f4a1d4f158f4a83f9ad5f68ffa5cbeba30a2eac7b82b17c80df7eee7c02a9c1a6a5b97c1179a0528e2c65e4d228c2371a47a776dac65a737ae180251ef5be23400b098dd8ca; QN271=220dde0d-e579-4611-9797-da90461b2ca8; __qt=v1%7CVTJGc2RHVmtYMTl2KzZMNnFKcStCN280dDI3UDFpa3FCak0rOGtCNWZDcElBQndnNW83dTVPVmMrOGV0cEJWNjM3dmVWVEwxZkFrMXRPc09DbFlpMFdDZ3lqdWEvNitCMSt1Vmp6aVNKSjJrN3ZKMHlZMHJTRjJ1Tm50R051UFhDYnl2TjZZRGxuOEt6WlJiTXNFZWhMbyszbGd6TUdra01UNWU1VndIeXFJPQ%3D%3D%7C1681537851211%7CVTJGc2RHVmtYMStVYWV0bE4xUHVGWkl5Rys2UGp2d25VQSsyRkxWcnVGLytkS0hMUHAxd1BvbDAvSUpZY2dScFEzVEFwcHJIU3hJcnBGZEpCMGFEK0E9PQ%3D%3D%7CVTJGc2RHVmtYMTg0QjFnalBvT3dOQ29aaTl3ZVE4blNxUUl4RHhVRGkyRk5IRlNiWlpuL1NsLytpV1hlV0NRWml6TnlscVZwYzlxeGVRckNhM0E1U0MxVm5ES0JZVlBnTGUycTVuVHR2WHF0QkdMUkxnbGhyZ2ZkdFJ2VFBma1BRTS9EK3JINm1YY2ZaZ2cyaDhpWWVTdXZ1dUxUaTUrWUR3OGUrVWFIMVQ3Wlp5TE52c0NmcEl3RjNzczh3Q2JJVDlGTlR5bmZLZlRnT1VGMURvaW9qby90ZEZxVWdReFIrT0Qwa21xWnIxTzJRMnRuYTJnbTBFSitXamQzRGYzNUQ2TzU5Umo1cTBWR0ZZakFiOFZaOHZmRWtjVlltUS8weC9CaFhJTDFKTVd6M0t1RUI3NEVBbGFESW1qM2g4ZVZOVnhHMk1GRmlJV216a3d2NFB3YUIwVCswUTUxZXhwb1p0YW5yZDlYOHl6WFF3Z3E5ZFBZNFdxQ2NBZTFvMzllbitHRkk2UjZROStOZldFQVAvTWZQd3M5djQ4cFJrVWp5NCthVkJGcENSZTZRZVlCVXlxZGw1aTVlRHlWUTcxTUJIdlhScmd6NytNUTBBMGlrZS92eG9GWDd2b2tvUXhSalNQZGt2QU9neStDVkl0YjBqWk5YMnRPWUx5Z3pDak9OT3oyWTBHWHVWUCtmaU8xOXpKZ25KZCtUZVFjbFNHdzBxRTJ1cTVTUUtpUXJsUzAxbzF1UE9MTU0wNU9yWGpnbTgrRGJSbzdRUThFS3h6dFpaOXhBL0dZbldDRjk1cDZOSGVmVFp4Q21KeTFVdGFaN3R6bHRsRkhBY2ZGVTByYkJYemg4VjZzUWNuSWpOTFA4dlZGeGg4a1N1cCt6UW8vWkJMUkZoYnpCSmM9", "utf-8");
           // textBox1.Text = aurl;
            string tel = Regex.Match(ahtml, @"联系方式：([\s\S]*?)""").Groups[1].Value;
            return tel;
        }


        
        #region  主函数
        public void run()

        {

           
            try

            {
                label1.Text = "开始采集";

                string[] text = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (var item in text)
                {
                    string cityid = GetcityId(item.Replace("市", ""));
                    ArrayList areaIds = getareas2(cityid);

                    foreach (string areaId in areaIds)
                    {
                       

                        for (int i = 0; i < 1001; i = i + 20)
                        {
                            string url = "https://ihotel.meituan.com/hbsearch/HotelSearch?utm_medium=pc&version_name=999.9&cateId=20&attr_28=129&cityId=" + cityid + "&areaId=" + areaId + "&offset=" + i + "&limit=20&startDay=" + DateTime.Now.ToString("yyyyMMdd") + "&endDay=" + DateTime.Now.ToString("yyyyMMdd") + "&q=&sort=defaults";

                           
                            string html = method.GetUrlWithCookie(url, cookie,"utf-8");


                            MatchCollection matchs = Regex.Matches(html, @"""realPoiId"":([\s\S]*?),", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                            MatchCollection prices = Regex.Matches(html, @"""lowestPrice"":([\s\S]*?),");

                            ArrayList lists = new ArrayList();

                            foreach (Match NextMatch in matchs)
                            {
                                lists.Add(NextMatch.Groups[1].Value);

                            }
                            label1.Text = "获取到数量：" + lists.Count.ToString();
                            if (lists.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集
                            {
                                Thread.Sleep(1000);
                                if (i > 20)
                                {
                                    i = i - 20;
                                }
                                continue;
                            }

                            

                            for (int j = 0; j < lists.Count; j++)
                            {
                                string strurl = "https://hotel.meituan.com/" + lists[j] + "/";
                                string strhtml = method.GetUrlWithCookie(strurl, pccookie,"utf-8");
                                textBox1.Text = strurl+"-"+ strhtml;
                                Match titles = Regex.Match(strhtml, @"<title>([\s\S]*?)_");
                                Match addr = Regex.Match(strhtml, @"""addr"":""([\s\S]*?)""");
                                Match zhuangxiu = Regex.Match(strhtml, @"装修时间"",""attrValue"":""([\s\S]*?)""");
                                Match fangjian = Regex.Match(strhtml, @"客房总量"",""attrValue"":""([\s\S]*?)""");
                                Match phone = Regex.Match(strhtml, @"""phone"":""([\s\S]*?)""");

                                Match type = Regex.Match(strhtml, @"""hotelStar"":""([\s\S]*?)""");
                                Match city = Regex.Match(strhtml, @"""cityName"":""([\s\S]*?)""");


                                string newphone = shaixuan(phone.Groups[1].Value.Replace("u002F", " "));
                                if (newphone != "")
                                {
                                    if (!finishes.Contains(newphone))
                                    {

                                        ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                        lv1.SubItems.Add(titles.Groups[1].Value);

                                        lv1.SubItems.Add(addr.Groups[1].Value);
                                        lv1.SubItems.Add(zhuangxiu.Groups[1].Value);
                                        lv1.SubItems.Add(fangjian.Groups[1].Value);
                                        lv1.SubItems.Add(newphone);

                                        lv1.SubItems.Add(type.Groups[1].Value);
                                        lv1.SubItems.Add(comboBox2.Text);
                                        lv1.SubItems.Add(city.Groups[1].Value);
                                        lv1.SubItems.Add("无");
                                    }
                                }

                                Thread.Sleep(100);
                                if (listView1.Items.Count - 1 > 1)
                                {
                                    listView1.EnsureVisible(listView1.Items.Count - 1);
                                }
                                if (this.status == false)

                                {
                                    return;
                                }


                            }
                        }

                    }
                }
            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        #region  主函数新
        public void run_new()

        {


            try

            {
                label1.Text = "开始采集";

                string[] text = textBox2.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (var item in text)
                {
                    string cityid = GetcityId(item.Replace("市", ""));
                   

                        for (int i = 0; i < 501; i = i + 20)
                        {
                            //string url = "https://ihotel.meituan.com/hbsearch/HotelSearch?utm_medium=pc&version_name=999.9&cateId=20&attr_28=129&cityId=" + cityid + "&areaId=" + areaId + "&offset=" + i + "&limit=20&startDay=" + DateTime.Now.ToString("yyyyMMdd") + "&endDay=" + DateTime.Now.ToString("yyyyMMdd") + "&q=&sort=defaults";
                            string url = "https://ihotel.meituan.com/hbsearch/HotelSearch?utm_medium=touch&version_name=999.9&platformid=1&cateId=20&newcate=1&limit=100&offset="+i+"&cityId="+cityid+"&ci="+cityid+"&startendday="+ DateTime.Now.ToString("yyyyMMdd") + "~" + DateTime.Now.ToString("yyyyMMdd") + "&startDay=" + DateTime.Now.ToString("yyyyMMdd") + "&endDay=" + DateTime.Now.ToString("yyyyMMdd") + "&attr_28=129&sort=defaults&userid=875973616&uuid=45232915CCB472C4CC11CB7147D6EB0E9578DFBE34DD8DCCAD71E58D27C73E37";

                            string html = method.GetUrlWithCookie(url, cookie, "utf-8");
                           
                            MatchCollection cityNames = Regex.Matches(html, @"""cityName"":""([\s\S]*?)""");
                            MatchCollection prices = Regex.Matches(html, @"""lowestPrice"":([\s\S]*?)""");
                            MatchCollection names = Regex.Matches(html, @"hourRoomSpan([\s\S]*?)""name"":""([\s\S]*?)""");
                            MatchCollection addrs = Regex.Matches(html, @"""addr"":""([\s\S]*?)""");
                            MatchCollection poiExtendsInfosDescs = Regex.Matches(html, @"""poiExtendsInfosDesc"":\[([\s\S]*?)\]");


                            for (int j = 0; j < names.Count; j++)
                            {


                                ListViewItem lv1 = listView1.Items.Add((listView1.Items.Count + 1).ToString()); //使用Listview展示数据
                                lv1.SubItems.Add(names[j].Groups[2].Value);
                                lv1.SubItems.Add(addrs[j].Groups[1].Value);
                                lv1.SubItems.Add(poiExtendsInfosDescs[j].Groups[1].Value.Trim().Replace("\"",""));
                               
                                lv1.SubItems.Add(comboBox2.Text);
                                lv1.SubItems.Add(cityNames[j].Groups[1].Value);

                                string tel = gettel(cityNames[j].Groups[1].Value, names[j].Groups[2].Value);
                                lv1.SubItems.Add(tel);
                            if (this.status == false)

                            {
                                return;

                            }

                            if (listView1.Items.Count - 1 > 1)
                            {
                                listView1.EnsureVisible(listView1.Items.Count - 1);
                            }

                            while (zanting == false)
                            {
                                Application.DoEvents();//等待本次加载完毕才执行下次循环.
                            }
                        }

                          
                              

                            Thread.Sleep(1000);
                            }
                        }

                    
                
            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
            status = true;
            #region 通用检测

            if (!method.GetUrl("http://acaiji.com/index/index/vip.html", "utf-8").Contains(@"xlD23Y"))
            {
                return;
            }

            #endregion
            if (thread == null || !thread.IsAlive)
            {
                thread = new Thread(run_new);
                thread.Start();
                Control.CheckForIllegalCrossThreadCalls = false;
            }
        }

        private void 美团酒店_Load(object sender, EventArgs e)
        {
            ProvinceCity.ProvinceCity.BindProvince(comboBox2);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text.Contains("上海"))
            {
               comboBox3.Text = "上海";
                return;
            }
            if (comboBox2.Text.Contains("北京"))
            {
                comboBox3.Text = "北京";
                return;
            }
            if (comboBox2.Text.Contains("重庆"))
            {
                comboBox3.Text = "重庆";
                return;
            }
            if (comboBox2.Text.Contains("天津"))
            {
                comboBox3.Text = "天津";
                return;
            }
            ProvinceCity.ProvinceCity.BindCity(comboBox2, comboBox3);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (zanting == false)
            {
                zanting = true;
            }
            else
            {
                zanting = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(gettel("宿迁","恒力国际大酒店"));
            listView1.Items.Clear();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!textBox2.Text.Contains(comboBox3.Text))
            {
                textBox2.Text += comboBox3.Text + "\r\n";
            }
        }

        private void 美团酒店_FormClosing(object sender, FormClosingEventArgs e)
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
    }
}
