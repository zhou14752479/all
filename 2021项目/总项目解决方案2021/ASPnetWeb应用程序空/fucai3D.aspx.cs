using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASPnetWeb应用程序空
{
    public partial class fucai3D : System.Web.UI.Page
    {
        #region 获取txt编码
        //调用：EncodingType.GetTxtType(textBox1.Text)
        public class EncodingType
        {
            /// <summary> 
            /// 给定文件的路径，读取文件的二进制数据，判断文件的编码类型 
            /// </summary> 
            /// <param name=“FILE_NAME“>文件路径</param> 
            /// <returns>文件的编码类型</returns> 
            public static System.Text.Encoding GetTxtType(string FILE_NAME)
            {
                FileStream fs = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read);
                Encoding r = GetType(fs);
                fs.Close();
                return r;
            }

            /// <summary> 
            /// 通过给定的文件流，判断文件的编码类型 
            /// </summary> 
            /// <param name=“fs“>文件流</param> 
            /// <returns>文件的编码类型</returns> 
            public static System.Text.Encoding GetType(FileStream fs)
            {
                byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
                byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
                byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM 
                Encoding reVal = Encoding.Default;

                BinaryReader r = new BinaryReader(fs, System.Text.Encoding.Default);
                int i;
                int.TryParse(fs.Length.ToString(), out i);
                byte[] ss = r.ReadBytes(i);
                if (IsUTF8Bytes(ss) || (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
                {
                    reVal = Encoding.UTF8;
                }
                else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
                {
                    reVal = Encoding.BigEndianUnicode;
                }
                else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
                {
                    reVal = Encoding.Unicode;
                }
                r.Close();
                return reVal;

            }

            /// <summary> 
            /// 判断是否是不带 BOM 的 UTF8 格式 
            /// </summary> 
            /// <param name=“data“></param> 
            /// <returns></returns> 
            private static bool IsUTF8Bytes(byte[] data)
            {
                int charByteCounter = 1; //计算当前正分析的字符应还有的字节数 
                byte curByte; //当前分析的字节. 
                for (int i = 0; i < data.Length; i++)
                {
                    curByte = data[i];
                    if (charByteCounter == 1)
                    {
                        if (curByte >= 0x80)
                        {
                            //判断当前 
                            while (((curByte <<= 1) & 0x80) != 0)
                            {
                                charByteCounter++;
                            }
                            //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X 
                            if (charByteCounter == 1 || charByteCounter > 6)
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        //若是UTF-8 此时第一位必须为1 
                        if ((curByte & 0xC0) != 0x80)
                        {
                            return false;
                        }
                        charByteCounter--;
                    }
                }
                if (charByteCounter > 1)
                {
                    throw new Exception("非预期的byte格式");
                }
                return true;
            }
        }

        #endregion
        #region GET请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url">网址</param>
        /// <returns></returns>
        public static string GetUrl(string Url, string charset)
        {
            string html = "";
            string COOKIE = "";
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);  //创建一个链接
                request.Proxy = null;//防止代理抓包
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36";
                request.Referer = Url;
                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("sec-fetch-mode:navigate");
                request.Headers.Add("Cookie", COOKIE);
                request.Headers.Add("Accept-Encoding", "gzip");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                request.KeepAlive = true;
                request.Accept = "*/*";
                request.Timeout = 5000;
                // request.Accept = "application/json, text/javascript, */*; q=0.01"; //返回中文问号参考
                if (response.Headers["Content-Encoding"] == "gzip")
                {

                    GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);//解压缩
                    StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
                    html = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO
                    html = reader.ReadToEnd();
                    reader.Close();
                }

                response.Close();
                return html;



            }
            catch (System.Exception ex)
            {
                return ex.ToString();

            }



        }
        #endregion
        #region POST请求
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送的数据包</param>
        /// <param name="COOKIE">cookie</param>
        /// <param name="charset">编码格式</param>
        /// <returns></returns>
        public static string PostUrl(string url, string postData, string COOKIE, string charset)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //获取不到加上这一条
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "Post";
                 request.ContentType = "application/x-www-form-urlencoded";

                //添加头部
                //WebHeaderCollection headers = request.Headers;
                //headers.Add("appid:orders");
                //headers.Add("x-nike-visitid:5");
                //headers.Add("x-nike-visitorid:d03393ee-e42c-463e-9235-3ca0491475b4");
                //添加头部
               // request.ContentType = "application/json";
                request.ContentLength = postData.Length;
                //request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                request.AllowAutoRedirect = false;
                request.KeepAlive = true;

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                request.Headers.Add("Cookie", COOKIE);

                request.Referer = "https://web.duanmatong.cn/";
                StreamWriter sw = new StreamWriter(request.GetRequestStream());
                sw.Write(postData);
                sw.Flush();

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;  //获取反馈
                response.GetResponseHeader("Set-Cookie");
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset)); //reader.ReadToEnd() 表示取得网页的源码流 需要引用 using  IO

                string html = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return html;
            }
            catch (WebException ex)
            {

                return ex.ToString();
            }


        }

        #endregion

        string[] aaa =
        {
            "000",
"001",
"002",
"003",
"004",
"005",
"006",
"007",
"008",
"009",
"010",
"011",
"012",
"013",
"014",
"015",
"016",
"017",
"018",
"019",
"020",
"021",
"022",
"023",
"024",
"025",
"026",
"027",
"028",
"029",
"030",
"031",
"032",
"033",
"034",
"035",
"036",
"037",
"038",
"039",
"040",
"041",
"042",
"043",
"044",
"045",
"046",
"047",
"048",
"049",
"050",
"051",
"052",
"053",
"054",
"055",
"056",
"057",
"058",
"059",
"060",
"061",
"062",
"063",
"064",
"065",
"066",
"067",
"068",
"069",
"070",
"071",
"072",
"073",
"074",
"075",
"076",
"077",
"078",
"079",
"080",
"081",
"082",
"083",
"084",
"085",
"086",
"087",
"088",
"089",
"090",
"091",
"092",
"093",
"094",
"095",
"096",
"097",
"098",
"099",
"100",
"101",
"102",
"103",
"104",
"105",
"106",
"107",
"108",
"109",
"110",
"111",
"112",
"113",
"114",
"115",
"116",
"117",
"118",
"119",
"120",
"121",
"122",
"123",
"124",
"125",
"126",
"127",
"128",
"129",
"130",
"131",
"132",
"133",
"134",
"135",
"136",
"137",
"138",
"139",
"140",
"141",
"142",
"143",
"144",
"145",
"146",
"147",
"148",
"149",
"150",
"151",
"152",
"153",
"154",
"155",
"156",
"157",
"158",
"159",
"160",
"161",
"162",
"163",
"164",
"165",
"166",
"167",
"168",
"169",
"170",
"171",
"172",
"173",
"174",
"175",
"176",
"177",
"178",
"179",
"180",
"181",
"182",
"183",
"184",
"185",
"186",
"187",
"188",
"189",
"190",
"191",
"192",
"193",
"194",
"195",
"196",
"197",
"198",
"199",
"200",
"201",
"202",
"203",
"204",
"205",
"206",
"207",
"208",
"209",
"210",
"211",
"212",
"213",
"214",
"215",
"216",
"217",
"218",
"219",
"220",
"221",
"222",
"223",
"224",
"225",
"226",
"227",
"228",
"229",
"230",
"231",
"232",
"233",
"234",
"235",
"236",
"237",
"238",
"239",
"240",
"241",
"242",
"243",
"244",
"245",
"246",
"247",
"248",
"249",
"250",
"251",
"252",
"253",
"254",
"255",
"256",
"257",
"258",
"259",
"260",
"261",
"262",
"263",
"264",
"265",
"266",
"267",
"268",
"269",
"270",
"271",
"272",
"273",
"274",
"275",
"276",
"277",
"278",
"279",
"280",
"281",
"282",
"283",
"284",
"285",
"286",
"287",
"288",
"289",
"290",
"291",
"292",
"293",
"294",
"295",
"296",
"297",
"298",
"299",
"300",
"301",
"302",
"303",
"304",
"305",
"306",
"307",
"308",
"309",
"310",
"311",
"312",
"313",
"314",
"315",
"316",
"317",
"318",
"319",
"320",
"321",
"322",
"323",
"324",
"325",
"326",
"327",
"328",
"329",
"330",
"331",
"332",
"333",
"334",
"335",
"336",
"337",
"338",
"339",
"340",
"341",
"342",
"343",
"344",
"345",
"346",
"347",
"348",
"349",
"350",
"351",
"352",
"353",
"354",
"355",
"356",
"357",
"358",
"359",
"360",
"361",
"362",
"363",
"364",
"365",
"366",
"367",
"368",
"369",
"370",
"371",
"372",
"373",
"374",
"375",
"376",
"377",
"378",
"379",
"380",
"381",
"382",
"383",
"384",
"385",
"386",
"387",
"388",
"389",
"390",
"391",
"392",
"393",
"394",
"395",
"396",
"397",
"398",
"399",
"400",
"401",
"402",
"403",
"404",
"405",
"406",
"407",
"408",
"409",
"410",
"411",
"412",
"413",
"414",
"415",
"416",
"417",
"418",
"419",
"420",
"421",
"422",
"423",
"424",
"425",
"426",
"427",
"428",
"429",
"430",
"431",
"432",
"433",
"434",
"435",
"436",
"437",
"438",
"439",
"440",
"441",
"442",
"443",
"444",
"445",
"446",
"447",
"448",
"449",
"450",
"451",
"452",
"453",
"454",
"455",
"456",
"457",
"458",
"459",
"460",
"461",
"462",
"463",
"464",
"465",
"466",
"467",
"468",
"469",
"470",
"471",
"472",
"473",
"474",
"475",
"476",
"477",
"478",
"479",
"480",
"481",
"482",
"483",
"484",
"485",
"486",
"487",
"488",
"489",
"490",
"491",
"492",
"493",
"494",
"495",
"496",
"497",
"498",
"499",
"500",
"501",
"502",
"503",
"504",
"505",
"506",
"507",
"508",
"509",
"510",
"511",
"512",
"513",
"514",
"515",
"516",
"517",
"518",
"519",
"520",
"521",
"522",
"523",
"524",
"525",
"526",
"527",
"528",
"529",
"530",
"531",
"532",
"533",
"534",
"535",
"536",
"537",
"538",
"539",
"540",
"541",
"542",
"543",
"544",
"545",
"546",
"547",
"548",
"549",
"550",
"551",
"552",
"553",
"554",
"555",
"556",
"557",
"558",
"559",
"560",
"561",
"562",
"563",
"564",
"565",
"566",
"567",
"568",
"569",
"570",
"571",
"572",
"573",
"574",
"575",
"576",
"577",
"578",
"579",
"580",
"581",
"582",
"583",
"584",
"585",
"586",
"587",
"588",
"589",
"590",
"591",
"592",
"593",
"594",
"595",
"596",
"597",
"598",
"599",
"600",
"601",
"602",
"603",
"604",
"605",
"606",
"607",
"608",
"609",
"610",
"611",
"612",
"613",
"614",
"615",
"616",
"617",
"618",
"619",
"620",
"621",
"622",
"623",
"624",
"625",
"626",
"627",
"628",
"629",
"630",
"631",
"632",
"633",
"634",
"635",
"636",
"637",
"638",
"639",
"640",
"641",
"642",
"643",
"644",
"645",
"646",
"647",
"648",
"649",
"650",
"651",
"652",
"653",
"654",
"655",
"656",
"657",
"658",
"659",
"660",
"661",
"662",
"663",
"664",
"665",
"666",
"667",
"668",
"669",
"670",
"671",
"672",
"673",
"674",
"675",
"676",
"677",
"678",
"679",
"680",
"681",
"682",
"683",
"684",
"685",
"686",
"687",
"688",
"689",
"690",
"691",
"692",
"693",
"694",
"695",
"696",
"697",
"698",
"699",
"700",
"701",
"702",
"703",
"704",
"705",
"706",
"707",
"708",
"709",
"710",
"711",
"712",
"713",
"714",
"715",
"716",
"717",
"718",
"719",
"720",
"721",
"722",
"723",
"724",
"725",
"726",
"727",
"728",
"729",
"730",
"731",
"732",
"733",
"734",
"735",
"736",
"737",
"738",
"739",
"740",
"741",
"742",
"743",
"744",
"745",
"746",
"747",
"748",
"749",
"750",
"751",
"752",
"753",
"754",
"755",
"756",
"757",
"758",
"759",
"760",
"761",
"762",
"763",
"764",
"765",
"766",
"767",
"768",
"769",
"770",
"771",
"772",
"773",
"774",
"775",
"776",
"777",
"778",
"779",
"780",
"781",
"782",
"783",
"784",
"785",
"786",
"787",
"788",
"789",
"790",
"791",
"792",
"793",
"794",
"795",
"796",
"797",
"798",
"799",
"800",
"801",
"802",
"803",
"804",
"805",
"806",
"807",
"808",
"809",
"810",
"811",
"812",
"813",
"814",
"815",
"816",
"817",
"818",
"819",
"820",
"821",
"822",
"823",
"824",
"825",
"826",
"827",
"828",
"829",
"830",
"831",
"832",
"833",
"834",
"835",
"836",
"837",
"838",
"839",
"840",
"841",
"842",
"843",
"844",
"845",
"846",
"847",
"848",
"849",
"850",
"851",
"852",
"853",
"854",
"855",
"856",
"857",
"858",
"859",
"860",
"861",
"862",
"863",
"864",
"865",
"866",
"867",
"868",
"869",
"870",
"871",
"872",
"873",
"874",
"875",
"876",
"877",
"878",
"879",
"880",
"881",
"882",
"883",
"884",
"885",
"886",
"887",
"888",
"889",
"890",
"891",
"892",
"893",
"894",
"895",
"896",
"897",
"898",
"899",
"900",
"901",
"902",
"903",
"904",
"905",
"906",
"907",
"908",
"909",
"910",
"911",
"912",
"913",
"914",
"915",
"916",
"917",
"918",
"919",
"920",
"921",
"922",
"923",
"924",
"925",
"926",
"927",
"928",
"929",
"930",
"931",
"932",
"933",
"934",
"935",
"936",
"937",
"938",
"939",
"940",
"941",
"942",
"943",
"944",
"945",
"946",
"947",
"948",
"949",
"950",
"951",
"952",
"953",
"954",
"955",
"956",
"957",
"958",
"959",
"960",
"961",
"962",
"963",
"964",
"965",
"966",
"967",
"968",
"969",
"970",
"971",
"972",
"973",
"974",
"975",
"976",
"977",
"978",
"979",
"980",
"981",
"982",
"983",
"984",
"985",
"986",
"987",
"988",
"989",
"990",
"991",
"992",
"993",
"994",
"995",
"996",
"997",
"998",
"999"
        };
       

        public void getdata()
        {
            string date = Request.QueryString["values"];

            StringBuilder sb = new StringBuilder();
            int count = 0;
            string[] dates = date.Split(new string[] { "*" }, StringSplitOptions.None);


            List<string> list = new List<string>();
            for (int i = Convert.ToInt32(dates[0].Substring(0,4)); i <= Convert.ToInt32(dates[1].Substring(0, 4)); i++)
            {
                string url = "https://kaijiang.78500.cn/3dshijihao/";
                string postdata = "startqi=&endqi=&year="+i+"&action=years";
                string html = PostUrl(url, postdata, "", "gb2312");
                MatchCollection values = Regex.Matches(html, @"<td><font color=""blue"">([\s\S]*?)</font>");
                MatchCollection values2 = Regex.Matches(html, @"<td class=""red"">([\s\S]*?)</a>");

              
                int jiezhicount = values.Count;
                if (i == Convert.ToInt32(dates[1].Substring(0, 4)))
                {
                    jiezhicount = Convert.ToInt32(dates[1].Substring(4, 3));
                }
                if (Convert.ToInt32(dates[0].Substring(0, 4)) != Convert.ToInt32(dates[1].Substring(0, 4))) //前后不是同一年
                {
                    for (int j = 0; j < jiezhicount; j++)
                    {
                        int fanxiang = values.Count- j-1;
                        list.Add(values[fanxiang].Groups[1].Value.Replace(",", ""));
                        list.Add(Regex.Replace(values2[fanxiang].Groups[1].Value, "<[^>]+>", "").Replace(" ", "").Trim());

                    }
                }
                if (Convert.ToInt32(dates[0].Substring(0, 4)) == Convert.ToInt32(dates[1].Substring(0, 4)))  //同一年内
                {
                    for (int j = Convert.ToInt32(dates[0].Substring(4, 3))-1; j < jiezhicount; j++)
                    {
                        // Response.Write(j.ToString() + "\r\n");

                        int fanxiang = values.Count - 1 - j;

                        list.Add(values[fanxiang].Groups[1].Value.Replace(",", ""));
                        list.Add(Regex.Replace(values2[fanxiang].Groups[1].Value, "<[^>]+>", "").Replace(" ", "").Trim());

                    }

                }
            }
           



            string[] bbb = list.ToArray();
            string[] arrAdd = aaa.Except(bbb).ToArray();
            foreach (var item in arrAdd)
            {
                count = count + 1;
                sb.Append("\"" + item + "\",");
            }

            string value = sb.ToString();
            if (sb.ToString().Length>1)
            {
                value = sb.ToString().Remove(sb.ToString().Length - 1, 1);

            }
            string result = "{\"list\":[" + value + "],\"count\":" + count + "}";
            Response.Write(result);

        }


        public void getdata2()
        {

            string date = Request.QueryString["values"];

            StringBuilder sb = new StringBuilder();
            int count = 0;
            string[] dates = date.Split(new string[] { "*" }, StringSplitOptions.None);

            string url = "https://www.8200.cn/kjh/3d/";
            string html = GetUrl(url, "utf-8");
          
            string qihao= Regex.Match(html, @"<span class=""current_qi value"">([\s\S]*?)</span>").Groups[1].Value;
           
            string ahtml = GetUrl("https://www.8200.cn/kjh/3d/"+ qihao+".htm", "utf-8");

            MatchCollection values = Regex.Matches(ahtml, @"<span class=""ball"">([\s\S]*?)</span>");
            MatchCollection values2 = Regex.Matches(ahtml, @"<b class=""mr-10"">([\s\S]*?)</b>");
            string kaijiang = values[0].Groups[1].Value.Trim() + values[1].Groups[1].Value.Trim() + values[2].Groups[1].Value.Trim();
            string kai = qihao + "," + kaijiang + "," + values2[0].Groups[1].Value.Trim() + "," + values2[1].Groups[1].Value.Trim();

            List<string> list = new List<string>();
           

            StreamReader sr = new StreamReader(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/fucai3d.txt", EncodingType.GetTxtType(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/fucai3d.txt"));
            //一次性读取完 
            string texts = sr.ReadToEnd().Trim();
            string[] text = texts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            string a1 = text[text.Length - 1];
            if(a1.Trim()=="")
            {
                a1 = text[text.Length - 2];
            }
            sr.Close();  //只关闭流
            sr.Dispose();   //销毁流内存



            if (a1.Split(new string[] { "," }, StringSplitOptions.None)[0] != qihao )
            {
                FileStream fs1 = new FileStream(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/fucai3d.txt", FileMode.Append, FileAccess.Write);//创建写入文件 
                StreamWriter sw = new StreamWriter(fs1, Encoding.GetEncoding("UTF-8"));
                sw.WriteLine("\r\n"+kai);
                sw.Close();
                fs1.Close();
                sw.Dispose();
            }


            StreamReader sr2 = new StreamReader(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/fucai3d.txt", EncodingType.GetTxtType(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/fucai3d.txt"));
            //一次性读取完 
            string texts2 = sr2.ReadToEnd().Trim();
            string[] text2 = texts2.Split(new string[] { "\r\n" }, StringSplitOptions.None);
           
            foreach (var item in text2)
            {
                if (item.Trim() != "")
                {
                    string aqihao = item.Split(new string[] { "," }, StringSplitOptions.None)[0];
                    string kaij = item.Split(new string[] { "," }, StringSplitOptions.None)[1];
                    string kaiji = item.Split(new string[] { "," }, StringSplitOptions.None)[2];
                    string shiji = item.Split(new string[] { "," }, StringSplitOptions.None)[3];
                    if (Convert.ToInt32(aqihao) >= Convert.ToInt32(dates[0]) && Convert.ToInt32(aqihao) <= Convert.ToInt32(dates[1]))
                    {

                        list.Add(kaij);
                        list.Add(kaiji);
                        list.Add(shiji);
                    }
                }

            }
            sr2.Close();  //只关闭流
            sr2.Dispose();   //销毁流内存


            string[] bbb = list.ToArray();
            string[] arrAdd = aaa.Except(bbb).ToArray();
            foreach (var item in arrAdd)
            {
                count = count + 1;
                sb.Append("\"" + item + "\",");
            }

            string value = sb.ToString();
            if (sb.ToString().Length > 1)
            {
                value = sb.ToString().Remove(sb.ToString().Length - 1, 1);

            }
            string result = "{\"list\":[" + value + "],\"count\":" + count + "}";
            Response.Write(result);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           
            getdata2();

        }
    }
}