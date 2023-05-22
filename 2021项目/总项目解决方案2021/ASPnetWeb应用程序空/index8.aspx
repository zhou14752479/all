<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index8.aspx.cs" Inherits="ASPnetWeb应用程序空.index8" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head><meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1,maximum-scale=1, user-scalable=no" />
    <meta http-equiv="Cache-Control" content="no-cache" /><meta http-equiv="Pragma" content="no-cache" /><title>
	缅甸佤邦娱乐
</title><link href="CSS/muwen.css" rel="stylesheet" />
    <script>
      
        function changci(changci) {
           
            window.location.href = "index8.aspx?changci=" + changci;
        }
   function getQueryString(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]);
        return null;
    }
        window.onload = function () {
            var changci = getQueryString("changci");
            document.getElementById("img1").src = "image8/" + changci + ".jpg";

            var x1 = document.getElementById("weikai").innerText;
            if (x1 == "【未开】")
            {
                document.getElementById("img1").style.opacity = 0;
            }

}

        function resetdata() {
            window.location.href = "Index.aspx?action=reset&_=" + Math.random();
        }
    </script>
     <style>
        .bottom {
            position: fixed;
            bottom: 0px;
            height: 50px;
            background-color: white;
            width: 100%;
        }

            .bottom img {

                width: 20px;
                height: 20px;
            }

        .body {
            color: white;
            font-size: 25px;
           
        }

        .img1{
            position: relative; margin:0 auto;
            width:70%;
            height:70%;
			margin-top: 250px;
            margin-bottom:100px;
            opacity:100;
        }
		#wenzi{
		
		}
		#wenzi p{
		color:yellow;
		}
    </style>
</head>
<body style="background-image: url('image8/background.jpg'); background-size: cover;background-position: center center;">
     
       

       <form id="form1"  action="index8_admin.aspx">     
        <div id="wenzi" class="center body">
          
			
						<p style="margin-top: 100px;">克 伦 邦 字 花</p>
            <p class="top-20"><%=DateTime.Now.ToString("yyyy年MM月dd日")%></p>
            <p class="font-18 top-10"><%=Application["changname"]%></p>
            <p class="font-18 top-5"><span id="weikai"><%=Application["a2"]%></span></p>
            <p class="top-5"><%=Application["a3"]%></p>
            <p><%=Application["a4"]%></p>
            <p><%=Application["a5"]%></p>
            <p class="font-18" style="margin-bottom: 100px;">题目译文：<%=Application["a6"]%></p>
           

        </div>

             <div class="center top10 bottom10">
                    <img class="img1" id="img1" src="image8/1.jpg" class="w100" />
                </div>       


        <div class="flex-around bottom">
            <div class="center" onclick="changci(1);">
                <img src="image8/btn1.png" />
                <p>上场</p>
            </div>
            <div class="center" onclick="changci(2);">
                <img src="image8/btn2.png" />
                <p>中场</p>
            </div>
            <div class="center" onclick="changci(3);">
                <img src="image8/btn3.png" />
                <p>下场</p>
            </div>
        </div>
    </form>
</body>
</html>
