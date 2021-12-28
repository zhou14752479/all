<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index6.aspx.cs" Inherits="ASPnetWeb应用程序空.index6" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head><meta http-equiv="Content-Type" content="text/html; charset=utf-8" /><meta name="viewport" content="width=device-width, initial-scale=1,maximum-scale=1, user-scalable=no" /><meta http-equiv="Cache-Control" content="no-cache" /><meta http-equiv="Pragma" content="no-cache" /><title>
	百胜后台数据
</title><link href="CSS/muwen.css" rel="stylesheet" />
    <script>
        function resetdata() {
            window.location.href = "Index6.aspx?action=reset&_=" + Math.random();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
         <div class="w50 center">
            <div class="title">百胜后台数据</div>
            <div class="border">
                
                <div class="w80 center">
                    <div class="text-style top10">
                        <input type="text" name="diyichang"   value="<%=Application["diyichang"]%>" placeholder="输入第一场" />
                        <button>保存</button>
                    </div>
                    <div class="text-style top10">
                        <input type="text" name="dierchang" value="<%=Application["dierchang"]%>" placeholder="输入第二场" />
                        <button>保存</button>
                    </div>
                    <div class="text-style top10">
                        <input type="text" name="disanchang" value="<%=Application["disanchang"]%>" placeholder="输入第三场" />
                        <button>保存</button>
                    </div>
                    
                    <button type="button" class="button button-block top10 bottom10" onclick="resetdata();">重置</button>
                </div>
            </div>
            <div class="title top10">生成结果</div>
            <div class="border">
                <div class="center top10 bottom10">
                    <img src="image6/result.jpg" class="w100" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
