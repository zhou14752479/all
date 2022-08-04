        <%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="ASP足球网站.index" %>


        <!DOCTYPE html>
        <html>
        <head>
        <meta charset="utf-8" />
        <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/>
        <meta name="viewport" content="width=device-width,initial-scale=1.0,maximum-scale=1.0,user-scalable=0"/>
        <meta name="robots" content="all"/>
        <meta name="referrer" content="always"/>
        <meta name="renderer" content="webkit"/>
        <meta http-equiv="Cache-Control" content="no-transform"/>
        <meta http-equiv="Cache-Control" content="no-siteapp"/>
        <meta name="format-detection" content="telephone=no"/>
        <meta name="applicable-device" content="mobile"/>
        <title>足球数据查询</title>
      
        <link rel="stylesheet" href="static/css/common.css"/>
        <link rel="stylesheet" type="text/css" media="screen" href="static/css/index.css"/>


        </head>
        <body>
        <div class="wrapper">
        <div class="header">
        <div class="mod-head mod-head-hide">

        <div class="hd">
        <div class="logo">
        <a class="subtitle" href="">足球数据查询</a>
        </div>
        <div class="menu" data-name="足球胜平负"></div>

                        <div class="nav-bar">
<span></span>
<span></span>
<span></span>
</div>
</div>
<div class="bd">
    <input  id="getdta" type="button"  onclick="getdata()" name="mobile" style="width:50%;height:30px;margin:20px; background:#2095f2;color:#fff;border:medium none" value="点击查询">
    <div class="module mod-panel">
    <table style="box-sizing:border-box;">
        <thead>
        <tr>
        <th><strong>比赛时间</strong></th>
        <th><strong>主队</strong></th>
        <th><strong>客队</strong></th>
        <th><strong>赔率</strong></th>
        <th><strong>胜率</strong></th>
        <th><strong>买入</strong></th>
        </tr>
        </thead>
        <tbody id="tableList">


        </tbody>
        </table>
      
        </div>
</div>


     


        <div class="mark"></div>
        </div>
        </div><div class="container">
        <div class="mod-menu">
        <div class="bd">
        <a class="prev" href="javascript:;" rel="nofollow"></a>
        <a class="next" href="javascript:;" rel="nofollow"></a>
        <ul>
        <li><a href="/index.aspx?type=1">足球胜平负</a></li>
        <li><a href="/lanqiu.aspx?type=2">篮球让分</a></li>
        <li><a href="/lanqiu.aspx?type=3">篮球大小分</a></li>
        </ul>
        </div>
        </div>
        <div class="module mod-panel">
        <div class="hd">
        <h3>澳客网比赛</h3>
        </div>
        <div class="bd">
        <div class="panel">
        <table>
        <thead>
        <tr>
        <th><strong>序号</strong></th>
        <th><strong>比赛时间</strong></th>
        <th><strong>主队</strong></th>
        <th><strong>客队</strong></th>
        <th><strong>操作</strong></th>
      
        </tr>
        </thead>
        <tbody>


        <%=Application["okooo_data"]%>

        </tbody>
        </table>
        </div>
        </div>
        </div>
        </div>

          
          


<div style="border:1px solid #000;padding:10px;text-align:center">
            
                <form method="post" runat="server" id="saveReportForm" >
                    <div><span id="time"><%=matchtime%></span></div>
                    <div><span id="zhu"><%=zhu%></span>&nbsp;VS&nbsp;<span id="ke"><%=ke%></span></div>
   赔率&nbsp;<input  id="peilv1" type="text" name="peilv1" style="width:25%;height:25px;" placeholder="胜" value=<%=sheng%> >&nbsp;
    <input  id="peilv2" type="text" name="peilv2" style="width:25%;height:25px;" placeholder="平" value=<%=ping%> >&nbsp;
    <input  id="peilv3" type="text" name="peilv3" style="width:25%;height:25px;" placeholder="负" value=<%=fu%> >
   <br />
   初始&nbsp;<input  id="chushenglv1" type="text" name="mobile" style="width:25%;height:25px;" placeholder="胜" value=<%=chusheng_bili%>>&nbsp;
    <input  id="chushenglv2" type="text" name="mobile" style="width:25%;height:25px;" placeholder="平" value=<%=chuping_bili%>>&nbsp;
    <input  id="chushenglv3" type="text" name="mobile" style="width:25%;height:25px;" placeholder="负" value=<%=chufu_bili%>>

                <br />
   变化&nbsp;<input  id="shenglv1" type="text" name="mobile" style="width:25%;height:25px;" placeholder="胜" value=<%=sheng_bili%>>&nbsp;
    <input  id="shenglv2" type="text" name="mobile" style="width:25%;height:25px;" placeholder="平" value=<%=ping_bili%>>&nbsp;
    <input  id="shenglv3" type="text" name="mobile" style="width:25%;height:25px;" placeholder="负" value=<%=fu_bili%>>
                <br />

   买入&nbsp;<input  id="mai1" type="text" name="mobile" style="width:25%;height:25px;" placeholder="1000" value="1000" >&nbsp;
    <input id="mai2" type="text" name="mobile" style="width:25%;height:25px;" placeholder="1000" value="1000">&nbsp;
    <input id="mai3" type="text" name="mobile" style="width:25%;height:25px;" placeholder="1000" value="1000">

<button type="button" onclick="buy()" style="width:100%;height:30px;margin-top:10px; background:#2095f2;color:#fff;border:medium none;" >保存</button>
    </form>   
    </div>
 
            <div class="module mod-panel">
        <div class="hd">
        <h3>查询结果</h3>
        </div>
        <div class="bd">
        <div class="panel">
        <table>
        <thead>
        <tr>
        <th><strong>比赛时间</strong></th>
        <th><strong>比赛名</strong></th>
        <th><strong>主队</strong></th>
        <th><strong>客队</strong></th>
        <th><strong>比分</strong></th>
        <th><strong>胜</strong></th>
        <th><strong>平</strong></th>
        <th><strong>负</strong></th>
        <th><strong>赛果</strong></th>
        </tr>
        </thead>
        <tbody>


        <%=result%>

        </tbody>
        </table>
        </div>
        </div>
        </div>
        </div>
        <div class="mod-sidebar">
        <a class="gotop" href="javascript:;" rel="nofollow"><img src="//cache.ip138.com/m/image/public/gotop.png" width="24" height="24" alt="返回顶部"></a>
        </div>
        
     <script type="text/javascript"src="static/js/jquery.js"></script>

         
            <script type="text/javascript">
                function getPar(par) {
                    //获取当前URL
                    var local_url = document.location.href;
                    //获取要取得的get参数位置
                    var get = local_url.indexOf(par + "=");
                    if (get == -1) {
                        return false;
                    }
                    //截取字符串
                    var get_par = local_url.slice(par.length + get + 1);
                    //判断截取后的字符串是否还有其他get参数
                    var nextPar = get_par.indexOf("&");
                    if (nextPar != -1) {
                        get_par = get_par.slice(0, nextPar);
                    }
                    return get_par;
                }
                window.onload = function(){
                    if (getPar("type") == "1") {
                        document.getElementsByTagName("h3")[0].innerText = "足球胜平负";
                    }
                    if (getPar("type") == "2") {
                        document.getElementsByTagName("h3")[0].innerText = "篮球让分";
                    }
                    if (getPar("type") == "3") {
                        document.getElementsByTagName("h3")[0].innerText = "篮球大小分";
                    }
                }
                
                var bar = document.getElementsByClassName("nav-bar")[0];
                bar.onclick = function () {
                    var aa = document.getElementsByClassName("mod-head")[0];
                    var bb = document.getElementsByClassName("bd")[0];
                    if (aa.className == "mod-head mod-head-hide") {
                        bb.style.width = '100%';
                        
                        aa.className = "mod-head mod-head-hide mod-head-show";
                    }
                    else {
                        bb.style.width = '200px';
                        aa.className = "mod-head mod-head-hide";
                    }
                    
                }
                
              
                function buy() {

                    var time = document.getElementById("time").innerText;
                    var zhu = document.getElementById("zhu").innerText;
                    var ke = document.getElementById("ke").innerText;


                    var peilv1 = document.getElementById("peilv1").value;
                    var peilv2 = document.getElementById("peilv2").value;
                    var peilv3 = document.getElementById("peilv3").value;

                    var shenglv1 = document.getElementById("shenglv1").value;
                    var shenglv2 = document.getElementById("shenglv2").value;
                    var shenglv3 = document.getElementById("shenglv3").value;

                    var mai1 = document.getElementById("mai1").value;
                    var mai2 = document.getElementById("mai2").value;
                    var mai3 = document.getElementById("mai3").value;

                    $.ajax({
                        url: "savedata.aspx?type=savedata",
                        type: "post",
                        dataType: "json",
                        data: { "time": time, "zhu": zhu, "ke": ke, "peilv1": peilv1, "peilv2": peilv2, "peilv3": peilv3, "shenglv1": shenglv1, "shenglv2": shenglv2, "shenglv3": shenglv3, "mai1": mai1, "mai2": mai2, "mai3": mai3 },
                        success: function (data) {
                            alert("保存成功");
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {

                            // 状态码

                            console.log(XMLHttpRequest.status);

                            // 状态

                            console.log(XMLHttpRequest.readyState);

                            // 错误信息

                            console.log(textStatus);
                        }

                        })
                }


                function getdata() {


                    $.ajax({
                        url: "savedata.aspx?type=getdata",
                        type: "post",
                        dataType: "json",
                        data: {  },
                        success: function (data) {
                            var content = data.data;
                            
                            if (!content)
                                return;
                            var datas = "";
                            
                            for (var i = 0; i < content.length; i++) {
                                var obj = content[i];
                               
                                datas = datas + "<td><span>" + obj["time"] + "</span></td>";
                                datas = datas + "<td><span>" + obj["zhu"] + "</span></td>";
                                datas = datas + "<td><span>" + obj["ke"] + "</span></td>";
                                datas = datas + "<td><span>" + obj["peilv1"] + "<br/>" + obj["peilv2"] + "<br/>" + obj["peilv3"] + "<br/>" + "</span></td>";
                                datas = datas + "<td><span>" + obj["shenglv1"] + "<br/>" + obj["shenglv2"] + "<br/>" + obj["shenglv3"] + "<br/>"+ "</span></td>";
                                datas = datas + "<td><span>" + obj["mai1"] + "<br/>" + obj["mai2"] + "<br/>" + obj["mai3"] + "<br/>" + "</span></td>";
                              
                                datas = "<tr>" + datas + "</tr>";
                            }
                            
                            //console.log(datas);
                            $("#tableList").html(datas);
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {

                            // 状态码

                            console.log(XMLHttpRequest.status);

                            // 状态

                            console.log(XMLHttpRequest.readyState);

                            // 错误信息

                            console.log(textStatus);
                        }

                    })
                }


            </script>


        </body>
        </html>