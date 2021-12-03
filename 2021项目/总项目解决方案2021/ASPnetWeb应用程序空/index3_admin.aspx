<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index3_admin.aspx.cs" Inherits="ASPnetWeb应用程序空.index3_admin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="CSS/muwen.css" rel="stylesheet" />
    <title></title>
    <script>
         function resetdata() {
            window.location.href = "Index3_admin.aspx?action=reset&_=" + Math.random();
        }
        function create() {
            var c1 = document.getElementById('c1');
            var c2 = document.getElementById('c2');
            var c3 = document.getElementById('c3');
             var c4 = document.getElementById('c4');
            var c5 = document.getElementById('c5');
            var c6 = document.getElementById('c6');
             var c7 = document.getElementById('c7');
            var c8 = document.getElementById('c8');
            var c9 = document.getElementById('c9');
             var c10 = document.getElementById('c10');
            var c11 = document.getElementById('c11');
            var c12 = document.getElementById('c12');
             var c13 = document.getElementById('c13');
            var c14 = document.getElementById('c14');
            var c15 = document.getElementById('c15');
              var c16 = document.getElementById('c16');
          
            window.location.href = "index3_admin.aspx?action=create&c1=" + c1.value + "&c2=" + c2.value + "&c3=" + c3.value+"&c4=" + c4.value +"&c5=" + c5.value+"&c6=" + c6.value+"&c7=" + c7.value+"&c8=" + c8.value+"&c9=" + c9.value+"&c10=" + c10.value+"&c11=" + c11.value+"&c12=" + c12.value+"&c13=" + c13.value+"&c14=" + c14.value+"&c15=" + c15.value+"&c16=" + c16.value
        }
        function createresult() {
         
            var diyichang = document.getElementById('diyichang');
            var dierchang = document.getElementById('dierchang');
            var disanchang = document.getElementById('disanchang');
            var shengxiao = document.getElementById('shengxiao');
            window.location.href = "index3_admin.aspx?action=createresult&diyichang=" + diyichang.value + "&dierchang=" + dierchang.value + "&disanchang=" + disanchang.value  + "&shengxiao=" + shengxiao.value;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
            <div class="w50 center">
            <div class="title">孟邦后台数据</div>
            <div class="border">
                <div class="w80 center">
                    <div class="text-style top10">
                         <input type="text" name="c1"   value="<%=Application["c1"]%>" placeholder="第一场第一行" />
                         <input type="text" name="c2"   value="<%=Application["c2"]%>" placeholder="第一场第二行" />
                         <input type="text" name="c3"   value="<%=Application["c3"]%>" placeholder="第一场第三行" />
                         <input type="text" name="c4"   value="<%=Application["c4"]%>" placeholder="第一场第四行" />
                       
                        <input type="text" name="diyichang"   value="<%=Application["diyichang1"]%>" placeholder="输入第一场" />
                        <button>保存</button>
                    </div>
                    <div class="text-style top10">
                          <input type="text" name="c5"   value="<%=Application["c5"]%>" placeholder="第二场第一行" />
                         <input type="text" name="c6"   value="<%=Application["c6"]%>" placeholder="第二场第二行" />
                         <input type="text" name="c7"   value="<%=Application["c7"]%>" placeholder="第二场第三行" />
                         <input type="text" name="c8"   value="<%=Application["c8"]%>" placeholder="第二场第四行" />
                        <input type="text" name="dierchang" value="<%=Application["dierchang1"]%>" placeholder="输入第二场" />
                        <button>保存</button>
                    </div>
                    <div class="text-style top10">
                          <input type="text" name="c9"   value="<%=Application["c9"]%>" placeholder="第三场第一行" />
                         <input type="text" name="c10"   value="<%=Application["c10"]%>" placeholder="第三场第二行" />
                         <input type="text" name="c11"   value="<%=Application["c11"]%>" placeholder="第三场第三行" />
                         <input type="text" name="c12"   value="<%=Application["c12"]%>" placeholder="第三场第四行" />
                        <input type="text" name="disanchang" value="<%=Application["disanchang1"]%>" placeholder="输入第三场" />
                        <button>保存</button>
                    </div>
                      <div class="text-style top10">
                            <input type="text" name="c13"   value="<%=Application["c13"]%>" placeholder="生肖第一行" />
                         <input type="text" name="c14"   value="<%=Application["c14"]%>" placeholder="生肖第二行" />
                         <input type="text" name="c15"   value="<%=Application["c15"]%>" placeholder="生肖第三行" />
                         <input type="text" name="c16"   value="<%=Application["c16"]%>" placeholder="生肖第四行" />
                        <input type="text" name="shengxiao" value="<%=Application["shengxiao1"]%>" placeholder="输入生肖" />
                        <button>保存</button>
                    </div>

                     <button type="button" class="button button-block top10 bottom10" onclick="create();">生成数据</button>
                    <button type="button" class="button button-block top10 bottom10" onclick="resetdata();">重置</button>
                </div>
            </div>
</div>
    </form>
</body>
</html>
