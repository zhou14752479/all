﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManagerShiCi.aspx.cs" Inherits="ASPnetWeb应用程序空.admin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="CSS/muwen.css" rel="stylesheet" />
    <title></title>
    <script>
        function create() {
            window.location.href = "ManagerShiCi.aspx?action=create";
        }
        function createresult() {
            var c1 = document.getElementById('c1');
            var c2 = document.getElementById('c2');
            var c3 = document.getElementById('c3');
            var sxsh = document.getElementById('sxsh');
            var shici = document.getElementById('shici');
             var chengyu = document.getElementById('chengyu');
            window.location.href = "ManagerShiCi.aspx?action=createresult&c1=" + c1.value + "&c2=" + c2.value + "&c3=" + c3.value + "&sxsh=" + sxsh.value + "&shici=" + shici.value+"&chengyu=" + chengyu.value;
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
            <div class="center w50">
            <div class="top10 title">生成数据</div>
            <div class="border">
                <div class="w90 center">
                    <button type="button" class="button button-block top10" onclick="create();">(2) 生成前端数据</button>
                    <div class="top10 bottom10">
                        <div>
                            <input type="text" id="c1" class="border w100 top10" placeholder="第一场结果" value="" />
                            <input type="text" id="c2" class="border w100 top10" placeholder="第二场结果" value="" />
                            <input type="text" id="c3" class="border w100 top10" placeholder="第三场结果" value="" />
                            <input type="text" id="sxsh" class="border w100 top10" placeholder="生肖守护结果" value="" />
                        </div>
                        <button type="button" class="button w100 top10" onclick="createresult();">(3) 生成前端结果</button>
                    </div>
                </div>
            </div>
            <div class="top10 title">
                数据编辑
            </div>
            <div class="border">
                
                <div class="top10 text-style w90 center">
                   <input type="text" id="chengyu" class="border w100 top10" placeholder="输入成语" value="" />
                    <input type="text" id="shici" name="content" placeholder="输入诗词" />
                  <%--  <button type="submit" class="button">(1) 保存</button>--%>
                </div>
                <div class="bottom10">
                    <p>生财有道 极乐世界,情不自禁,愚公移山,魑魅魍魉,龙生九子,精卫填海,海市蜃楼,高山流水,卧薪尝胆,壮志凌云,金枝玉叶,四海一家,

穿针引线,

无忧无虑,

无地自容,

三位一体,

落叶归根,

相见恨晚,

惊天动地,

滔滔不绝,

相濡以沫,

长生不死,

原来如此,

女娲补天,

三皇五帝,

万箭穿心,

水木清华,

窈窕淑女,

破釜沉舟,
                        阳春白雪,

杯弓蛇影,

闻鸡起舞,

四面楚歌,

登堂入室,

张灯结彩,

而立之年,

饮鸩止渴,

杏雨梨云,

龙凤呈祥,

勇往直前,

左道旁门,

莫衷一是,

马踏飞燕,

掩耳盗铃,

大江东去,

凿壁偷光,

色厉内荏,

花容月貌,

越俎代庖,

鳞次栉比,

美轮美奂,

缘木求鱼,

再接再厉,

马到成功,

红颜知己,

赤子之心,

迫在眉睫,

风流韵事,

相形见绌,绘声绘色,

九五之尊,

随心所欲,

干将莫邪,

相得益彰,

借刀杀人,

浪迹天涯,

刚愎自用,

镜花水月,

黔驴技穷,

肝胆相照,

多多益善,

叱咤风云,

杞人忧天,

作茧自缚,

一飞冲天,

殊途同归,

风卷残云,

因果报应,

无可厚非,

赶尽杀绝,

天长地久,

飞龙在天,

桃之夭夭,

南柯一梦,

口是心非,

江山如画,

风华正茂,

一帆风顺,

一叶知秋
                    </p>
                    <p>树坚不怕风吹动，节操棱棱还自持</p>
                    
                    <p>冰霜历尽心不移，况复阳和景渐宜。</p>
                    
                    <p>闲花野草尚葳蕤，风吹柏枝将何为？</p>
                    
                    <p>金山杳在沧溟中，雪崖冰柱浮仙宫。</p>
                    
                    <p>乾坤扶持自今古，日月仿佛悬西东。</p>
                    
                    <p>我泛灵槎出尘世，搜索异境窥神功。</p>
                    
                    <p>一朝登临重叹息，四时想象何其雄！</p>
                    
                    <p>卷帘夜阁挂北斗，大鲸驾浪吹长空。</p>
                    
                    <p>舟摧岸断岂足数，往往霹雳搥蛟龙。</p>
                    
                    <p>寒蟾八月荡瑶海，秋光上下磨青铜。</p>
                    
                    <p>鸟飞不尽暮天碧，渔歌忽断芦花风。</p>
                    
                    <p>蓬莱久闻未曾往，壮观绝致遥应同。</p>
                    
                    <p>潮生潮落夜还晓，物与数会谁能穷</p>
                    
                    <p>白云南来入长望，又起归兴随征鸿。</p>
                    
                    <p>亭亭画舸系春潭，直到行人酒半酣</p>
                    
                    <p>。不管烟波与风雨，载将离恨过江南。</p>
                    
                    <p>世乱同南去，时清独北还。</p>
                    
                    <p>他乡生白发，旧国见青山</p>
                    
                    <p>晓月过残垒，繁星宿故关</p>
                    
                    <p>寒禽与衰草，处处伴愁颜。</p>
                    
                    <p>山围故国周遭在，潮打空城寂寞回。</p>
                    
                    <p>淮水东边旧时月，夜深还过女墙来。</p>
                    
                    <p>朱雀桥边野草花，乌衣巷口夕阳斜。</p>
                    
                    <p>旧时王谢堂前燕，飞入寻常百姓家。</p>
                    
                    <p>台城六代竞豪华，结绮临春事最奢。</p>
                    
                    <p>万户千门成野草，只缘一曲后庭花。</p>
                    
                    <p>生公说法鬼神听，身后空堂夜不扃。</p>
                    
                    <p>高坐寂寥尘漠漠，一方明月可中庭。</p>
                    
                    <p>南朝词臣北朝客，归来唯见秦淮碧。</p>
                    
                    <p>池台竹树三亩馀，至今人道江家宅。</p>
                    
                    <p>三万里河东入海，五千仞岳上摩天。</p>
                    
                    <p>遗民泪尽胡尘里，南望王师又一年。</p>
                    
                    <p>剑外忽传收蓟北，初闻涕泪满衣裳。</p>
                    
                    <p>却看妻子愁何在，漫卷诗书喜欲狂。</p>
                    
                    <p>白日放歌须纵酒，青春作伴好还乡。</p>
                    
                    <p>即从巴峡穿巫峡，便下襄阳向洛阳。</p>
                    
                    <p>皇家贵主好神仙，别业初开云汉边。</p>
                    
                    <p>山出尽如鸣凤岭，池成不让饮龙川。</p>
                    
                    <p>妆楼翠幌教春住，舞阁金铺借日悬。</p>
                    
                    <p>敬从乘舆来此地，称觞献寿乐钧天。</p>
                    
                    <p>望门投止思张俭，忍死须臾待杜根。</p>
                    
                    <p>我自横刀向天笑，去留肝胆两昆仑。</p>
                    
                    <p>望门投趾怜张俭，直谏陈书愧杜根。</p>
                    
                    <p>手掷欧刀仰天笑，留将公罪后人论。</p>
                    
                    <p>两地俱秋夕，相望共星河。</p>
                    
                    <p>高梧一叶下，空斋归思多。</p>
                    
                    <p>方用忧人瘼，况自抱微痾。</p>
                    
                    <p>无将别来近，颜鬓已蹉跎。</p>
                    
                    <p>孤舟相访至天涯，万转云山路更赊。</p>
                    
                    <p>欲扫柴门迎远客，青苔黄叶满贫家。</p>
                    
                    <p>越女新妆出镜心，自知明艳更沉吟。</p>
                    
                    <p>齐纨未是人间贵，一曲菱歌敌万金。</p>
                    
                    <p>齐纨未足时人贵，一曲菱歌敌万金。</p>
                    
                    <p>平旦驱驷马，旷然出五盘。</p>
                    
                    <p>江回两崖斗，日隐群峰攒。</p>
                    
                    <p>苍翠烟景曙，森沉云树寒。</p>
                    
                    <p>松疏露孤驿，花密藏回滩。</p>
                    
                    <p>栈道谿雨滑，畬田原草干。</p>
                    
                    <p>此行为知己，不觉蜀道难</p>
                    
                    <p>别离已久犹为郡，闲向春风倒酒瓶。</p>
                    
                    <p>送客特过沙口堰，看花多上水心亭。</p>
                    
                    <p>晓来江气连城白，雨后山光满郭青。</p>
                    
                    <p>到此诗情应更远，醉中高咏有谁听。</p>
                    
                    <p>沉沉更鼓急，渐渐人声绝。</p>
                    
                    <p>吹灯窗更明，月照一天雪。</p>
                    
                    <p>孟冬十郡良家子，血作陈陶泽中水。</p>
                    
                    <p>野旷天清无战声，四万义军同日死。</p>
                    
                    <p>群胡归来血洗箭，仍唱胡歌饮都市。</p>
                    
                    <p>都人回面向北啼，日夜更望官军至。</p>
                    
                    <p>桥西一曲水通村，岸阁浮萍绿有痕。</p>
                    
                    <p>家住石湖人不到，藕花多处别开门。</p>
                    
                    <p>少年上人号怀素，草书天下称独步。</p>
                    
                    <p>墨池飞出北溟鱼，笔锋杀尽中山兔。</p>
                    
                    <p>八月九月天气凉，酒徒词客满高堂。</p>
                    
                    <p>笺麻素绢排数箱，宣州石砚墨色光。</p>
                    
                    <p>吾师醉后倚绳床，须臾扫尽数千张</p>
                    
                    <p>飘风骤雨惊飒飒，落花飞雪何茫茫</p>
                    
                    <p>起来向壁不停手，一行数字大如斗</p>
                    
                    <p>怳怳如闻神鬼惊，时时只见龙蛇走。</p>
                    
                    <p>左盘右蹙如惊电，状同楚汉相攻战。</p>
                    
                    <p>湖南七郡凡几家，家家屏障书题遍。</p>
                    
                    <p>张颠老死不足数，我师此义不师古。</p>
                    
                    <p>古来万事贵天生，何必要公孙大娘浑脱舞。</p>
                    
                    <p>恃爱如欲进，含羞未肯前。</p>
                    
                    <p>朱口发艳歌，玉指弄娇弦。</p>
                    
                    <p>宦情羁思共凄凄，春半如秋意转迷。</p>
                    
                    <p>山城过雨百花尽，榕叶满庭莺乱啼。</p>
                    
                    <p>浮香绕曲岸，圆影覆华池。</p>
                    
                    <p>常恐秋风早，飘零君不知。</p>
                    
                    <p>风恬日暖荡春光，戏蝶游蜂乱入房。</p>
                    
                    <p>数枝门柳低衣桁，一片山花落笔床。</p>
                    
                    <p>梁园日暮乱飞鸦，极目萧条三两家。</p>
                    
                    <p>庭树不知人去尽，春来还发旧时花。</p>
                    
                    <p>江涵秋影雁初飞，与客携壶上翠微。</p>
                    
                    <p>尘世难逢开口笑，菊花须插满头归。</p>
                    
                    <p>但将酩酊酬佳节，不用登临恨落晖。</p>
                    
                    <p>古往今来只如此，牛山何必独霑衣</p>
                    
                    <p>孟冬寒气至，北风何惨栗。</p>
                    
                    <p>愁多知夜长，仰观众星列。</p>
                    
                    <p>三五明月满，四五蟾兔缺。</p>
                    
                    <p>客从远方来，遗我一书札。</p>
                    
                    <p>上言长相思，下言久离别。</p>
                    
                    <p>置书怀袖中，三岁字不灭。</p>
                    
                    <p>一心抱区区，惧君不识察。</p>
                    
                    <p>蓼虫避葵堇，习苦不言非。</p>
                    
                    <p>小人自龌龊，安知旷士怀。</p>
                    
                    <p>鸡鸣洛城里，禁门平旦开。</p>
                    
                    <p>冠盖纵横至，车骑四方来。</p>
                    
                    <p>素带曳长飙，华缨结远埃。</p>
                    
                    <p>日中安能止，钟鸣犹未归。</p>
                    
                    <p>夷世不可逢，贤君信爱才。</p>
                    
                    <p>明虑自天断，不受外嫌猜。</p>
                    
                    <p>一言分珪爵，片善辞草莱。</p>
                    
                    <p>岂伊白璧赐，将起黄金台。</p>
                    
                    <p>今君有何疾，临路独迟回。</p>
                    
                    <p>明月沉珠浦，秋风濯锦川。</p>
                    
                    <p>楼台临绝岸，洲渚亘长天。</p>
                    
                    <p>旅泊成千里，栖遑共百年。</p>
                    
                    <p>穷途唯有泪，还望独潸然。</p>
                    
                    <p>金陵夜寂凉风发，独上高楼望吴越。</p>
                    
                    <p>白云映水摇空城，白露垂珠滴秋月。</p>
                    
                    <p>下沉吟久不归，古来相接眼中稀。</p>
                    
                    <p>解道澄江净如练，令人长忆谢玄晖。</p>
                    
                    <p>陶公有逸兴，不与常人俱。</p>
                    
                    <p>筑台像半月，回向高城隅。</p>
                    
                    <p>置酒望白云，商飙起寒梧。</p>
                    
                    <p>秋山入远海，桑柘罗平芜。</p>
                    
                    <p>水色渌且明，令人思镜湖。</p>
                    
                    <p>终当过江去，爱此暂踟蹰。</p>
                    
                    <p>胜日寻芳泗水滨，无边光景一时新。</p>
                    
                    <p>等闲识得东风面，万紫千红总是春。</p>
                    
                    <p>十里长街市井连，月明桥上看神仙。</p>
                    
                    <p>人生只合扬州死，禅智山光好墓田。</p>
                    
                    <p>初月如弓未上弦，分明挂在碧霄边。</p>
                    
                    <p>时人莫道蛾眉小，三五团圆照满天。</p>
                    
                    <p>君到姑苏见，人家尽枕河。</p>
                    
                    <p>古宫闲地少，水港小桥多</p>
                    
                    <p>遥知未眠月，乡思在渔歌。</p>
                    
                    <p>千秋佳节名空在，承露丝囊世已无。</p>
                    
                    <p>唯有紫苔偏称意，年年因雨上金铺。</p>
                    
                    <p>山光物态弄春晖，莫为轻阴便拟归。</p>
                    
                    <p>纵使晴明无雨色，入云深处亦沾衣。</p>
                    
                    <p>客中多病废登临，闻说南台试一寻。</p>
                    
                    <p>九轨徐行怒涛上，千艘横系大江心。</p>
                    
                    <p>寺楼钟鼓催昏晓，墟落云烟自古今。</p>
                    
                    <p>白发未除豪气在，醉吹横笛坐榕阴。</p>
                    
                    <p>雨在时时黑，春归处处青</p>
                    
                    <p>山深失小寺，湖尽得孤亭</p>
                    
                    <p>春着湖烟腻，晴摇野水光。</p>
                    
                    <p>草青仍过雨，山紫更斜阳。</p>
                    
                    <p>凉风度秋海，吹我乡思飞。</p>
                    
                    <p>连山去无际，流水何时归。</p>
                    
                    <p>目极浮云色，心断明月晖。</p>
                    
                    <p>芳草歇柔艳，白露催寒衣。</p>
                    
                    <p>梦长银汉落，觉罢天星稀。</p>
                    
                    <p>含悲想旧国，泣下谁能挥。</p>
                    
                    <p>晋家南渡日，此地旧长安。</p>
                    
                    <p>地即帝王宅，山为龙虎盘。</p>
                    
                    <p>金陵空壮观，天堑净波澜。</p>
                    
                    <p>悲时俗之迫阨兮，愿轻举而远游。</p>
                    
                    <p>质菲薄而无因兮，焉讬乘而上浮？</p>
                    
                    <p>遭沈浊而污秽兮，独郁结其谁语！</p>
                    
                    <p>夜耿耿而不寐兮，魂营营而至曙。</p>
                    
                    <p>惟天地之无穷兮，哀人生之长勤。</p>
                    
                    <p>往者余弗及兮，来者吾不闻。</p>
                    
                    <p>步徙倚而遥思兮，怊惝怳而乖怀。</p>
                    
                    <p>意荒忽而流荡兮，心愁悽而增悲。</p>
                    
                    <p>神倏忽而不反兮，形枯槁而独留。</p>
                    
                    <p>内惟省以操端兮，求正气之所由。</p>
                    
                    <p>漠虚静以恬愉兮，澹无为而自得。</p>
                    
                    <p>闻赤松之清尘兮，愿承风乎遗则。</p>
                    
                    <p>贵真人之休德兮，美往世之登仙；</p>
                    
                    <p>与化去而不见兮，名声著而日延。</p>
                    
                    <p>奇傅说之讬辰星兮，羡韩众之得一。</p>
                    
                    <p>形穆穆以浸远兮，离人群而遁逸。</p>
                    
                    <p>因气变而遂曾举兮，忽神奔而鬼怪。</p>
                    
                    <p>时仿佛以遥见兮，精晈晈以往来。</p>
                    
                    <p>超氛埃而淑邮兮，终不反其故都。</p>
                    
                    <p>免众患而不惧兮，世莫知其所如。</p>
                    
                    <p>恐天时之代序兮，耀灵晔而西征。</p>
                    
                    <p>微霜降而下沦兮，悼芳草之先蘦。</p>
                    
                    <p>聊仿佯而逍遥兮，永历年而无成。</p>
                    
                    <p>谁可与玩斯遗芳兮？长向风而舒情。</p>
                    
                    <p>高阳邈以远兮，余将焉所程？</p>
                    
                    <p>春秋忽其不淹兮，奚久留此故居。</p>
                    
                    <p>轩辕不可攀援兮，吾将从王乔而娱戏。</p>
                    
                    <p>餐六气而饮沆瀣兮，漱正阳而含朝霞。</p>
                    
                    <p>保神明之清澄兮，精气入而麤秽除。</p>
                    
                    <p>顺凯风以从游兮，至南巢而壹息。</p>
                    
                    <p>见王子而宿之兮，审壹气之和德。</p>
                    
                    <p>吸飞泉之微液兮，怀琬琰之华英。</p>
                    
                    <p>玉色頩以脕颜兮，精醇粹而始壮。</p>
                    
                    <p>质销铄以汋约兮，神要眇以淫放。</p>
                    
                    <p>嘉南州之炎德兮，丽桂树之冬荣；</p>
                    
                    <p>山萧条而无兽兮，野寂漠其无人</p>
                    
                    <p>载营魄而登霞兮，掩浮云而上征。</p>
                    
                    <p>命天阍其开关兮，排阊阖而望予。</p>
                    
                    <p>召丰隆使先导兮，问太微之所居。</p>
                    
                    <p>集重阳入帝宫兮，造旬始而观清都。</p>
                    
                    <p>朝发轫于太仪兮，夕始临乎于微闾。</p>
                    
                    <p>屯余车之万乘兮，纷容与而并驰。</p>
                    
                    <p>驾八龙之婉婉兮，载云旗之逶蛇。</p>
                    
                    <p>建雄虹之采旄兮，五色杂而炫耀。</p>
                    
                    <p>服偃蹇以低昂兮，骖连蜷以骄骜。</p>
                    
                    <p>骑胶葛以杂乱兮，斑漫衍而方行。</p>
                    
                    <p>撰余辔而正策兮，吾将过乎句芒。</p>
                    
                    <p>历太皓以右转兮，前飞廉以启路。</p>
                    
                    <p>阳杲杲其未光兮，凌天地以径度。</p>
                    
                    <p>风伯为余先驱兮，氛埃辟而清凉。</p>
                    
                    <p>凤凰翼其承旂兮，遇蓐收乎西皇。</p>
                    
                    <p>揽慧星以为旍兮，举斗柄以为麾。</p>
                    
                    <p>叛陆离其上下兮，游惊雾之流波。</p>
                    
                    <p>时暧曃其曭莽兮，召玄武而奔属。</p>
                    
                    <p>后文昌使掌行兮，选署众神以并轂。</p>
                    
                    <p>路漫漫其修远兮，徐弭节而高厉。</p>
                    
                    <p>左雨师使径侍兮，右雷公以为卫。</p>
                    
                    <p>欲度世以忘归兮，意姿睢以抯挢。</p>
                    
                    <p>内欣欣而自美兮，聊媮娱以淫乐。</p>
                    
                    <p>涉青云以汎滥游兮，忽临睨夫旧乡。</p>
                    
                    <p>仆夫怀余心悲兮，边马顾而不行。</p>
                    
                    <p>思旧故以想象兮，长太息而掩涕。</p>
                    
                    <p>汜容与而遐举兮，聊抑志而自弭。</p>
                    
                    <p>指炎神而直驰兮，吾将往乎南疑。</p>
                    
                    <p>览方外之荒忽兮，沛罔瀁而自浮。</p>
                    
                    <p>祝融戒而跸御兮，腾告鸾鸟迎宓妃。</p>
                    
                    <p>张咸池奏承云兮，二女御九韶歌。</p>
                    
                    <p>音乐博衍无终极兮，焉乃逝以徘徊。</p>
                    
                    <p>舒并节以驰骛兮，逴绝垠乎寒门。</p>
                    
                    <p>轶迅风于清源兮，从颛顼乎增冰。</p>
                    
                    <p>历玄冥以邪径兮，乘间维以反顾。</p>
                    
                    <p>召黔赢而见之兮，为余先乎平路。</p>
                    
                    <p>下峥嵘而无地兮，上寥廓而无天。</p>
                    
                    <p>寒雨连江夜入吴，平明送客楚山孤。</p>
                    
                    <p>洛阳亲友如相问，一片冰心在玉壶。</p>
                    
                    <p>闺中少妇不知愁，春日凝妆上翠楼</p>
                    
                    <p>忽见陌头杨柳色，悔教夫婿觅封侯</p>
                    
                    <p>昨夜风开露井桃，未央前殿月轮高。</p>
                    
                    <p>平阳歌舞新承宠，帘外春寒赐锦袍。</p>
                    
                    <p>独在异乡为异客，每逢佳节倍思亲。</p>
                    
                    <p>遥知兄弟登高处，遍插茱萸少一人。</p>
                    
                    <p>葡萄美酒夜光杯，欲饮琵琶马上催。</p>
                    
                    <p>醉卧沙场君莫笑，古来征战几人回？</p>
                    
                    <p>泪湿罗巾梦不成，夜深前殿按歌声。</p>
                    
                    <p>红颜未老恩先断，斜倚薰笼坐到明。</p>
                    
                    <p>寂寂花时闭院门，美人相并立琼轩。</p>
                    
                    <p>含情欲说宫中事，鹦鹉前头不敢言。</p>
                    
                    <p>故人西辞黄鹤楼，烟花三月下扬州。</p>
                    
                    <p>孤帆远影碧空尽，唯见长江天际流</p>
                    
                    <p>朝辞白帝彩云间，千里江陵一日还。</p>
                    
                    <p>两岸猿声啼不住，轻舟已过万重山</p>
                    
                    <p>洞房昨夜停红烛，待晓堂前拜舅姑。</p>
                    
                    <p>妆罢低声问夫婿，画眉深浅入时无。</p>
                    
                    <p>故园东望路漫漫，双袖龙钟泪不干。</p>
                    
                    <p>马上相逢无纸笔，凭君传语报平安。</p>
                    
                    <p>回乐烽前沙似雪，受降城外月如霜。</p>
                    
                    <p>不知何处吹芦管，一夜征人尽望乡。</p>
                    
                    <p>宣室求贤访逐臣，贾生才调更无伦。</p>
                    
                    <p>可怜夜半虚前席，不问苍生问鬼神。</p>
                    
                    <p>乘兴南游不戒严，九重谁省谏书函。</p>
                    
                    <p>春风举国裁宫锦，半作障泥半作帆。</p>
                    
                    <p>瑶池阿母绮窗开，黄竹歌声动地哀。</p>
                    
                    <p>八骏日行三万里，穆王何事不重来。</p>
                    
                    <p>云母屏风烛影深，长河渐落晓星沉。</p>
                    
                    <p>嫦娥应悔偷灵药，碧海青天夜夜心。</p>
                    
                    <p>嵩云秦树久离居，双鲤迢迢一纸书。</p>
                    
                    <p>休问梁园旧宾客，茂陵秋雨病相如。</p>
                    
                    <p>为有云屏无限娇，凤城寒尽怕春宵。</p>
                    
                    <p>无端嫁得金龟婿，辜负香衾事早朝。</p>
                    
                    <p>岐王宅里寻常见，崔九堂前几度闻。</p>
                    
                    <p>正是江南好风景，落花时节又逢君。</p>
                    
                    <p>青山隐隐水迢迢，秋尽江南草未凋。</p>
                    
                    <p>二十四桥明月夜，玉人何处教吹箫？</p>
                    
                    <p>落魄江湖载酒行，楚腰纤细掌中轻</p>
                    
                    <p>十年一觉扬州梦，赢得青楼薄幸名。</p>
                    
                    <p>清时有味是无能，闲爱孤云静爱僧。</p>
                    
                    <p>欲把一麾江海去，乐游原上望昭陵。</p>
                    
                    <p>折戟沉沙铁未销，自将磨洗认前朝。</p>
                    
                    <p>东风不与周郎便，铜雀春深锁二乔。</p>
                    
                    <p>烟笼寒水月笼沙，夜泊秦淮近酒家。</p>
                    
                    <p>商女不知亡国恨，隔江犹唱后庭花。</p>
                    
                    <p>岁岁金河复玉关，朝朝马策与刀环。</p>
                    
                    <p>三春白雪归青冢，万里黄河绕黑山。</p>
                    
                    <p>谁谓伤心画不成，画人心逐世人情。</p>
                    
                    <p>君看六幅南朝事，老木寒云满故城。</p>
                    
                    <p>独怜幽草涧边生，上有黄鹂深树鸣。</p>
                    
                    <p>春潮带雨晚来急，野渡无人舟自横。</p>
                    
                    <p>隐隐飞桥隔野烟，石矶西畔问渔船。</p>
                    
                    <p>桃花尽日随流水，洞在清溪何处边。</p>
                    
                    <p>别梦依依到谢家，小廊回合曲阑斜。</p>
                    
                    <p>多情只有春庭月，犹为离人照落花。</p>
                    
                    <p>金陵津渡小山楼，一宿行人自可愁。</p>
                    
                    <p>潮落夜江斜月里，两三星火是瓜洲。</p>
                    
                    <p>日光斜照集灵台，红树花迎晓露开。</p>
                    
                    <p>昨夜上皇新授箓，太真含笑入帘来。</p>
                    
                    <p>虢国夫人承主恩，平明骑马入宫门。</p>
                    
                    <p>却嫌脂粉污颜色，淡扫蛾眉朝至尊。</p>
                    
                    <p>月落乌啼霜满天，江枫渔火对愁眠。</p>
                    
                    <p>姑苏城外寒山寺，夜半钟声到客船。</p>
                    
                </div>
            </div>
        </div>


    </form>
</body>
</html>
