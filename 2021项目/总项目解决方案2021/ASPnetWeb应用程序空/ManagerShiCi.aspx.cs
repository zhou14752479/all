using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASPnetWeb应用程序空
{
    public partial class admin : System.Web.UI.Page
    {
        /// <summary>
        /// 读取本地图片
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Bitmap ReadImageFile(string path)
        {
            if (!File.Exists(path))
            {
              
                return null;//文件不存在
            }
            FileStream fs = File.OpenRead(path); //OpenRead
            int filelength = 0;
            filelength = (int)fs.Length; //获得文件长度 
            Byte[] image = new Byte[filelength]; //建立一个字节数组 
            fs.Read(image, 0, filelength); //按字节流读取 
            System.Drawing.Image result = System.Drawing.Image.FromStream(fs);
            fs.Close();
            fs.Dispose();
            Bitmap bit = new Bitmap(result);
            return bit;
        }

        string[] shiciarr = {
            "树坚不怕风吹动，节操棱棱还自持",
"冰霜历尽心不移，况复阳和景渐宜",
"闲花野草尚葳蕤，风吹柏枝将何为？",
"金山杳在沧溟中，雪崖冰柱浮仙宫",
"乾坤扶持自今古，日月仿佛悬西东",
"我泛灵槎出尘世，搜索异境窥神功",
"一朝登临重叹息，四时想象何其雄",
"卷帘夜阁挂北斗，大鲸驾浪吹长空",
"舟摧岸断岂足数，往往霹雳搥蛟龙",
"寒蟾八月荡瑶海，秋光上下磨青铜",
"鸟飞不尽暮天碧，渔歌忽断芦花风",
"蓬莱久闻未曾往，壮观绝致遥应同",
"潮生潮落夜还晓，物与数会谁能穷",
"白云南来入长望，又起归兴随征鸿",
"亭亭画舸系春潭，直到行人酒半酣",
"不管烟波与风雨，载将离恨过江南",
"山围故国周遭在，潮打空城寂寞回",
"淮水东边旧时月，夜深还过女墙来",
"朱雀桥边野草花，乌衣巷口夕阳斜",
"旧时王谢堂前燕，飞入寻常百姓家",
"台城六代竞豪华，结绮临春事最奢",
"万户千门成野草，只缘一曲后庭花",
"生公说法鬼神听，身后空堂夜不扃",
"高坐寂寥尘漠漠，一方明月可中庭",
"南朝词臣北朝客，归来唯见秦淮碧",
"池台竹树三亩馀，至今人道江家宅",
"三万里河东入海，五千仞岳上摩天",
"遗民泪尽胡尘里，南望王师又一年",
"剑外忽传收蓟北，初闻涕泪满衣裳",
"却看妻子愁何在，漫卷诗书喜欲狂",
"白日放歌须纵酒，青春作伴好还乡",
"即从巴峡穿巫峡，便下襄阳向洛阳",
"皇家贵主好神仙，别业初开云汉边",
"山出尽如鸣凤岭，池成不让饮龙川",
"妆楼翠幌教春住，舞阁金铺借日悬",
"敬从乘舆来此地，称觞献寿乐钧天",
"望门投止思张俭，忍死须臾待杜根",
"我自横刀向天笑，去留肝胆两昆仑",
"望门投趾怜张俭，直谏陈书愧杜根",
"手掷欧刀仰天笑，留将公罪后人论",
"欲扫柴门迎远客，青苔黄叶满贫家",
"越女新妆出镜心，自知明艳更沉吟",
"齐纨未是人间贵，一曲菱歌敌万金",
"齐纨未足时人贵，一曲菱歌敌万金",
"别离已久犹为郡，闲向春风倒酒瓶",
"送客特过沙口堰，看花多上水心亭",
"晓来江气连城白，雨后山光满郭青",
"到此诗情应更远，醉中高咏有谁听",
"孟冬十郡良家子，血作陈陶泽中水",
"野旷天清无战声，四万义军同日死",
"群胡归来血洗箭，仍唱胡歌饮都市",
"都人回面向北啼，日夜更望官军至",
"桥西一曲水通村，岸阁浮萍绿有痕",
"家住石湖人不到，藕花多处别开门",
"少年上人号怀素，草书天下称独步",
"墨池飞出北溟鱼，笔锋杀尽中山兔",
"八月九月天气凉，酒徒词客满高堂",
"笺麻素绢排数箱，宣州石砚墨色光",
"吾师醉后倚绳床，须臾扫尽数千张",
"飘风骤雨惊飒飒，落花飞雪何茫茫",
"起来向壁不停手，一行数字大如斗",
"怳怳如闻神鬼惊，时时只见龙蛇走",
"左盘右蹙如惊电，状同楚汉相攻战",
"湖南七郡凡几家，家家屏障书题遍",
"张颠老死不足数，我师此义不师古",
"古来万事贵天生，公孙大娘浑脱舞",
"宦情羁思共凄凄，春半如秋意转迷",
"山城过雨百花尽，榕叶满庭莺乱啼",
"风恬日暖荡春光，戏蝶游蜂乱入房",
"数枝门柳低衣桁，一片山花落笔床",
"梁园日暮乱飞鸦，极目萧条三两家",
"庭树不知人去尽，春来还发旧时花",
"江涵秋影雁初飞，与客携壶上翠微",
"尘世难逢开口笑，菊花须插满头归",
"但将酩酊酬佳节，不用登临恨落晖",
"古往今来只如此，牛山何必独霑衣",
"金陵夜寂凉风发，独上高楼望吴越",
"白云映水摇空城，白露垂珠滴秋月",
"下沉吟久不归，古来相接眼中稀",
"解道澄江净如练，令人长忆谢玄晖",
"胜日寻芳泗水滨，无边光景一时新",
"等闲识得东风面，万紫千红总是春",
"十里长街市井连，月明桥上看神仙",
"人生只合扬州死，禅智山光好墓田",
"初月如弓未上弦，分明挂在碧霄边",
"时人莫道蛾眉小，三五团圆照满天",
"千秋佳节名空在，承露丝囊世已无",
"唯有紫苔偏称意，年年因雨上金铺",
"山光物态弄春晖，莫为轻阴便拟归",
"纵使晴明无雨色，入云深处亦沾衣",
"客中多病废登临，闻说南台试一寻",
"九轨徐行怒涛上，千艘横系大江心",
"寺楼钟鼓催昏晓，墟落云烟自古今",
"白发未除豪气在，醉吹横笛坐榕阴",
"寒雨连江夜入吴，平明送客楚山孤",
"洛阳亲友如相问，一片冰心在玉壶",
"闺中少妇不知愁，春日凝妆上翠楼",
"忽见陌头杨柳色，悔教夫婿觅封侯",
"昨夜风开露井桃，未央前殿月轮高",
"平阳歌舞新承宠，帘外春寒赐锦袍",
"独在异乡为异客，每逢佳节倍思亲",
"遥知兄弟登高处，遍插茱萸少一人",
"葡萄美酒夜光杯，欲饮琵琶马上催",
"醉卧沙场君莫笑，古来征战几人回？",
"泪湿罗巾梦不成，夜深前殿按歌声",
"红颜未老恩先断，斜倚薰笼坐到明",
"寂寂花时闭院门，美人相并立琼轩",
"含情欲说宫中事，鹦鹉前头不敢言",
"故人西辞黄鹤楼，烟花三月下扬州",
"孤帆远影碧空尽，唯见长江天际流",
"朝辞白帝彩云间，千里江陵一日还",
"两岸猿声啼不住，轻舟已过万重山",
"洞房昨夜停红烛，待晓堂前拜舅姑",
"妆罢低声问夫婿，画眉深浅入时无",
"故园东望路漫漫，双袖龙钟泪不干",
"马上相逢无纸笔，凭君传语报平安",
"回乐烽前沙似雪，受降城外月如霜",
"不知何处吹芦管，一夜征人尽望乡",
"宣室求贤访逐臣，贾生才调更无伦",
"可怜夜半虚前席，不问苍生问鬼神",
"乘兴南游不戒严，九重谁省谏书函",
"春风举国裁宫锦，半作障泥半作帆",
"瑶池阿母绮窗开，黄竹歌声动地哀",
"八骏日行三万里，穆王何事不重来",
"云母屏风烛影深，长河渐落晓星沉",
"嫦娥应悔偷灵药，碧海青天夜夜心",
"嵩云秦树久离居，双鲤迢迢一纸书",
"休问梁园旧宾客，茂陵秋雨病相如",
"为有云屏无限娇，凤城寒尽怕春宵",
"无端嫁得金龟婿，辜负香衾事早朝",
"岐王宅里寻常见，崔九堂前几度闻",
"正是江南好风景，落花时节又逢君",
"青山隐隐水迢迢，秋尽江南草未凋",
"二十四桥明月夜，玉人何处教吹箫？",
"落魄江湖载酒行，楚腰纤细掌中轻",
"十年一觉扬州梦，赢得青楼薄幸名",
"清时有味是无能，闲爱孤云静爱僧",
"欲把一麾江海去，乐游原上望昭陵",
"折戟沉沙铁未销，自将磨洗认前朝",
"东风不与周郎便，铜雀春深锁二乔",
"烟笼寒水月笼沙，夜泊秦淮近酒家",
"商女不知亡国恨，隔江犹唱后庭花",
"岁岁金河复玉关，朝朝马策与刀环",
"三春白雪归青冢，万里黄河绕黑山",
"谁谓伤心画不成，画人心逐世人情",
"君看六幅南朝事，老木寒云满故城",
"独怜幽草涧边生，上有黄鹂深树鸣",
"春潮带雨晚来急，野渡无人舟自横",
"隐隐飞桥隔野烟，石矶西畔问渔船",
"桃花尽日随流水，洞在清溪何处边",
"别梦依依到谢家，小廊回合曲阑斜",
"多情只有春庭月，犹为离人照落花",
"金陵津渡小山楼，一宿行人自可愁",
"潮落夜江斜月里，两三星火是瓜洲",
"日光斜照集灵台，红树花迎晓露开",
"昨夜上皇新授箓，太真含笑入帘来",
"虢国夫人承主恩，平明骑马入宫门",
"却嫌脂粉污颜色，淡扫蛾眉朝至尊",
"月落乌啼霜满天，江枫渔火对愁眠",
"姑苏城外寒山寺，夜半钟声到客船",

        };


        string[] chengyuarr =
        {
    "穿针引线",
"无忧无虑",
"无地自容",
"三位一体",
"落叶归根",
"相见恨晚",
"惊天动地",
"滔滔不绝",
"相濡以沫",
"长生不死",
"原来如此",
"女娲补天",
"三皇五帝",
"万箭穿心",
"水木清华",
"窈窕淑女",
"破釜沉舟",
"阳春白雪",
"杯弓蛇影",
"闻鸡起舞",
"四面楚歌",
"登堂入室",
"张灯结彩",
"而立之年",
"饮鸩止渴",
"杏雨梨云",
"龙凤呈祥",
"勇往直前",
"左道旁门",
"莫衷一是",
"马踏飞燕",
"掩耳盗铃",
"大江东去",
"凿壁偷光",
"色厉内荏",
"花容月貌",
"越俎代庖",
"鳞次栉比",
"美轮美奂",
"缘木求鱼",
"再接再厉",
"马到成功",
"红颜知己",
"赤子之心",
"迫在眉睫",
"风流韵事",
"相形见绌",
"九五之尊",
"随心所欲",
"干将莫邪",
"相得益彰",
"借刀杀人",
"浪迹天涯",
"刚愎自用",
"镜花水月",
"黔驴技穷",
"肝胆相照",
"多多益善",
"叱咤风云",
"杞人忧天",
"作茧自缚",
"一飞冲天",
"殊途同归",
"风卷残云",
"因果报应",
"无可厚非",
"赶尽杀绝",
"天长地久",
"飞龙在天",
"桃之夭夭",
"南柯一梦",
"口是心非",
"江山如画",
"风华正茂",
"一帆风顺",
"一叶知秋",
"阳春白雪",
"杯弓蛇影",
"闻鸡起舞",
"四面楚歌",
"登堂入室",
"张灯结彩",
"而立之年",
"饮鸩止渴",
"杏雨梨云",
"龙凤呈祥",
"勇往直前",
"左道旁门",
"莫衷一是",
"马踏飞燕",
"掩耳盗铃",
"大江东去",
"凿壁偷光",
"色厉内荏",
"花容月貌",
"越俎代庖",
"鳞次栉比",
"美轮美奂",
"缘木求鱼",
"再接再厉",
"马到成功",
"红颜知己",
"赤子之心",
"迫在眉睫",
"风流韵事",
"相形见绌",
"诸子百家",
"鬼迷心窍",
"星火燎原",
"画地为牢",
"岁寒三友",
"花花世界",
"纸醉金迷",
"狐假虎威",
"纵横捭阖",
"沧海桑田",
"不求甚解",
"暴殄天物",
"吃喝玩乐",
"乐不思蜀",
"身不由己",
"小家碧玉",
"文不加点",
"天马行空",
"人来人往",
"千方百计",
"天高地厚",
"万人空巷",
"争分夺秒",
"如火如荼",
"大智若愚",
"斗转星移",
"七情六欲",
"大禹治水",
"空穴来风",
"孟母三迁",
"草船借箭",
"铁石心肠",
"望其项背",
"头晕目眩",
"大浪淘沙",
"纵横天下",
"有问必答",
"无为而治",
"釜底抽薪",
"吹毛求疵",
"好事多磨",
"空谷幽兰",
"悬梁刺股",
"白手起家",
"完璧归赵",
"忍俊不禁",
"沐猴而冠",
"白云苍狗",
"贼眉鼠眼",
"围魏救赵",
"烟雨蒙蒙",
"炙手可热",
"尸位素餐",
"草木皆兵",
"宁缺毋滥",
"回光返照",
"露水夫妻",
"讳莫如深",
"贻笑大方",
"紫气东来",
"万马奔腾",
"一诺千金",
"老马识途",
"五花大绑",
"捉襟见肘",
"瓜田李下",
"水漫金山",
"苦心孤诣",
"可见一斑",
"五湖四海",
"虚怀若谷",
"欲擒故纵",
"风声鹤唳",
"毛遂自荐",
"蛛丝马迹",
"中庸之道",
"迷途知返",
"自由自在",
"龙飞凤舞",
"树大根深",
"雨过天晴",
"乘风破浪",
"筚路蓝缕",
"朝三暮四",
"患得患失",
"君子好逑",
"鞭长莫及",
"竭泽而渔",
"飞黄腾达",
"囊萤映雪",
"飞蛾扑火",
"自怨自艾",
"风驰电掣",
"白马非马",
"退避三舍",
"三山五岳",
"称心如意",
"望梅止渴",
"茕茕孑立",
"振聋发聩",
"运筹帷幄",
"逃之夭夭",
"杯水车薪",
"有的放矢",
"矫枉过正",
"睚眦必报",
"姗姗来迟",
"一鸣惊人",
"孜孜不倦",
"一马平川",
"入木三分",
"沆瀣一气",
"天伦之乐",
"兄弟阋墙",
"藕断丝连",
"心猿意马",
"想入非非",
"盲人摸象",
"眉飞色舞",
"三教九流",
"高楼大厦",
"锲而不舍",
"过犹不及",
"狗尾续貂",
"斗酒学士",
"高山仰止",
"形影不离",
"小心翼翼",
"返璞归真",
"见贤思齐",
"按图索骥",
"枪林弹雨",
"桀骜不驯",
"遇人不淑",
"道貌岸然",
"名扬四海",
"虚与委蛇",
"门可罗雀",
"水落石出",
"不卑不亢",
"无法无天",
"拔苗助长",
"大快朵颐",



        };
        /// <summary>
        /// 在现有画布上添加文字
        /// </summary>
        /// <param name="g">画布对象</param>
        /// <param name="txt">需要添加的文字</param>
        /// <param name="fontName">字体名称</param>
        /// <param name="fontSize">字体大小</param>
        /// <param name="fontStyle">字体风格，是否加粗等</param>
        /// <param name="x">绘画位置X</param>
        /// <param name="y">绘画位置Y</param>
        /// <param name="red">文字颜色R</param>
        /// <param name="green">文字颜色G</param>
        /// <param name="blue">文字颜色B</param>
        public void drawText(string txt, string fontName, int fontSize, FontStyle fontStyle, float x, float y, int red, int green, int blue,string oldfile, string newfile)
        {
            try
            {
               
                System.Drawing.Image image_sc = ReadImageFile(AppDomain.CurrentDomain.SetupInformation.ApplicationBase+"/result/" + oldfile + ".jpg");
                Bitmap nbmp = new Bitmap(image_sc);
                Graphics g = Graphics.FromImage(nbmp);
                //文字绘制
                Font font = new Font(fontName, fontSize, fontStyle, GraphicsUnit.Pixel);
                Brush brush = new SolidBrush(Color.FromArgb(red, green, blue));
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                g.DrawString(txt, font, brush, x, y, StringFormat.GenericDefault);
               
                 nbmp.Save(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/result/" + newfile + ".jpg");
            }
            catch (Exception ex)
            {

                Response.Write(ex.ToString());
            }

          
        }
        //if (HttpContext.Current.Request.RequestType == "POST")
        //   // string s = Request["KeyValue"];
        //    //if (!string.IsNullOrWhiteSpace(s))
        //    //{
        //    //    string result = GetPatientInfo(s);
        //    //    Response.Write(result);//ajax请求必须response回去，不然前台接收不到
        //    //    Response.End();//response必须end，不然会一直返回页面，数据丢失
        //    //}


     public static string value1 = "";
        public static string value2 = "";
        public static string value3 = "";
        public static string sxsh_value = "";

            public void shengcheng(string chang)
        {
            Random rd = new Random();
            string shici = shiciarr[rd.Next(0, 155)].Replace("，", "\n    ");
            string zi = shici.Substring(3, 1);

            Random rd2 = new Random();
            string chengyu = "提示:" + chengyuarr[rd2.Next(0, 248)] + "," + chengyuarr[rd.Next(0, 248)];


            string value = "    " + shici + "\n\n" + "       宝钢:" + zi + "\n\n" + chengyu + "\n\n" + "     第"+chang+"场:【未开】";

            if(chang=="一")
            {
                value1 = value;
            }
            if (chang == "二")
            {
                value2 = value;
            }
            if (chang == "三")
            {
                value3 = value;
            }


            drawText(value, "宋体", 19, FontStyle.Bold, 40, 100, 0, 0, 0, "场", "第"+chang+"场");

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //生成前端数据
           if( Request.QueryString["action"] == "create")
            {

                shengcheng("一");
                shengcheng("二");
                shengcheng("三");


                Random rd = new Random();
                string shici = shiciarr[rd.Next(0, 155)];

                Random rd2 = new Random();
                string chengyu = "提示:" + chengyuarr[rd2.Next(0, 248)] + "," + chengyuarr[rd.Next(0, 248)];

                string newshici = "";

                char[] arr = shici.ToArray();
                foreach (var item in arr)
                {
                    newshici += item + " ";
                }


               string zi1 = shici.Substring(3, 1);
                string zi2 = shici.Substring(12, 1);


                string value = "\n  " + newshici.Replace("，", "\n ") + "\n" + "         (" + zi1 + zi2 + ")\n" + chengyu + "\n" + "\n    生肖守护:【未开】";
                sxsh_value = value;
                drawText(value, "宋体", 80, FontStyle.Bold, 40, 100, 0, 0, 0, "生肖", "生肖守护");

            }


            //生成前端结果
            if (Request.QueryString["action"] == "createresult")
            {
                if (Request.QueryString["c1"] != "")
                {
                    string c1 = Request.QueryString["c1"];
                    string value = value1.Replace("未开", c1);
                    drawText(value, "宋体", 19, FontStyle.Bold, 40, 100, 0, 0, 0, "场", "第一场");
                }

                if (Request.QueryString["c2"] != "")
                {          
                    string c2 = Request.QueryString["c2"];
                    string value = value2.Replace("未开", c2);
                    drawText(value, "宋体", 19, FontStyle.Bold, 40, 100, 0, 0, 0, "场", "第二场");

                }

                if (Request.QueryString["c3"] != "")
                {
                    string c3 = Request.QueryString["c3"];
                    string value = value3.Replace("未开", c3);
                    drawText(value, "宋体", 19, FontStyle.Bold, 40, 100, 0, 0, 0, "场", "第三场");

                }
                if (Request.QueryString["sxsh"] != "")
                {
                    string sxsh = Request.QueryString["sxsh"];
                    string value = sxsh_value.Replace("未开", sxsh);
                    drawText(value, "宋体", 80, FontStyle.Bold, 40, 100, 0, 0, 0, "生肖", "生肖守护");

                }

            }
               

            }





            
    }
}