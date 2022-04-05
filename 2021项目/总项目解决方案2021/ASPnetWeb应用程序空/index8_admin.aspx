<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index8_admin.aspx.cs" Inherits="ASPnetWeb应用程序空.index8_admin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head><meta http-equiv="Content-Type" content="text/html; charset=utf-8" /><meta name="viewport" content="width=device-width, initial-scale=1,maximum-scale=1, user-scalable=no" /><meta http-equiv="Cache-Control" content="no-cache" /><meta http-equiv="Pragma" content="no-cache" /><title>
	缅甸佤邦娱乐
</title><link href="CSS/muwen.css" rel="stylesheet" />
    <script>
         function goto(changci) {
            window.location.href = "index8_admin.aspx?changci=" + changci;
        }
        function resetdata() {
            window.location.href = "Index.aspx?action=reset&_=" + Math.random();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
         <div class="w50 center">
            <div class="title">缅甸佤邦娱乐</div>
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
        </div>
         <div class="w50 center">
             <div class="border">
          <div class="w80 center">
                    <div class="text-style top10" style="text-align:center">
        <button type="button" class="button button-block top10 bottom10" onclick="goto(1);">设置第一场</button>
         <button type="button" class="button button-block top10 bottom10" onclick="goto(2);">设置第二场</button>
         <button type="button" class="button button-block top10 bottom10" onclick="goto(3);">设置第三场</button>
                        </div>
              </div>
        </div>
              </div>
          <div class="w50 center">
            <div class="title"><%=Application["changname"]%></div>
            <div class="border">
                
                <div class="w80 center">
                     <div class="text-style top10">
                        <input type="text" name="changname"   value="<%=Application["changname"]%>" placeholder="场次" />
                    </div>
                    <div class="text-style top10">
                        <input type="text" name="a2"   value="<%=Application["a2"]%>" placeholder="【未开】" />
                    </div>
                    <div class="text-style top10">
                        <input type="text" name="a3" value="<%=Application["a3"]%>" placeholder="第一句" />  
                    </div>
                     <div class="text-style top10">
                        <input type="text" name="a4" value="<%=Application["a4"]%>" placeholder="第二句" />  
                    </div>
                      <div class="text-style top10">
                        <input type="text" name="a5" value="<%=Application["a5"]%>" placeholder="宝钢：不" />  
                    </div>
                    <div class="text-style top10">
                        <input type="text" name="a6" value="<%=Application["a6"]%>" placeholder="提示" />
                       
                    </div>
                     <div class="text-style top10"> <button>保存</button></div>
                    
                </div>
            </div>
        </div>



    </form>
    
       
    
</body>
</html>
