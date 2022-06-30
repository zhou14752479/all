<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index627.aspx.cs" Inherits="ASPnetWeb应用程序空.index627" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">image627
<head><meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1,maximum-scale=1, user-scalable=no" />
    <meta http-equiv="Cache-Control" content="no-cache" /><meta http-equiv="Pragma" content="no-cache" /><title>
	缅甸佤邦娱乐
</title><link href="CSS/muwen.css" rel="stylesheet" />
    <script>
      
        function changci(changci) {
           
            window.location.href = "index627.aspx?changci=" + changci;
        }
   function getQueryString(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]);
        return null;
    }
        window.onload = function () {
            var changci = getQueryString("changci");
            document.getElementById("img1").src = "image627/" + changci + ".jpg";
            document.getElementById("img2").src = "image627/a" + changci + ".jpg";
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
			
            opacity:100;
        }
		#wenzi{
		
		}
		#wenzi p{
		color:yellow;
		}
    </style>
</head>
<body style="background-image: url('image627/background.jpg'); background-size: cover;background-position: center center;">
     
        <%-- <div class="center top10 bottom10">
                    <img class="img1" id="img1" src="image627/1.jpg" class="w100" />
                </div>       --%>

         
        <div id="wenzi" class="center body" style="margin-top:100px">
          
			
					
           <p class="font-18 top-10">昨日开奖：<%=Application["zuori"]%></p>
            <p class="font-18 top-10"><%=Application["changname"]%></p>
            <p class="font-18 top-5"><span id="weikai"><%=Application["a2"]%></span></p>
            <p class="top-5"><%=Application["a3"]%></p>
            <p><%=Application["a4"]%></p>
            <p><%=Application["a5"]%></p>
            <p><%=Application["a6"]%></p>
           <p><%=Application["a7"]%></p>

        </div>
          <%-- <div class="center top10 bottom10">
                    <img class="img1" id="img2" src="image627/1.jpg" class="w100" />
                </div>      --%>

           <div  style="width:100%; position:fixed;top: 300px;right: 0px;" class ="float:left">

             <div class="text-style top10"> <button style="width:15%;" onclick="changci(1)">第一场</button></div>
       <div class="text-style top10" onclick="changci(2)"> <button style="width:15%;">第二场</button></div>
           <div class="text-style top10" onclick="changci(3)"> <button style="width:15%;">第三场</button></div>
               </div>
        
    
</body>
</html>