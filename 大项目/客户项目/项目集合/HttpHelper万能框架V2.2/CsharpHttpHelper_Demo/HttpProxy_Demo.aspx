<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HttpProxy_Demo.aspx.cs"
    Inherits="CsharpHttpHelper_Demo.HttpProxy_Demo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        fdsfsfsfsf<br />
        fdsfsfsfsf<br />
        fdsfsfsfsf<br />
        fdsfsfsfsf<br />
        fdsfsfsfsf<br />
        fdsfsfsfsf<br />
    <script type="text/javascript">

        function strimgshwo() {
            document.getElementById('view').innerHTML = '<img src="http://www.sufeinet.com/dv/httphelper/banner2.gif" />';
        }
        function viewonclick() {
            setTimeout("document.getElementById('view').innerHTML = ''", "2000");
        }

        setTimeout("strimgshwo()", "1000");
        setTimeout("document.getElementById('view').innerHTML=''", "20000"); //在此修改播放时间，根据自己的需要修改
        </script>
        <a id="view" href="http://httphelper.sufeinet.com/" onclick="viewonclick()" target="_blank">
        </a>
    </div>
    fdsfsfsfsf<br />
    fdsfsfsfsf<br />
    fdsfsfsfsf<br />
    fdsfsfsfsf<br />
    fdsfsfsfsf<br />
    fdsfsfsfsf<br />
    fdsfsfsfsf<br />
    </form>
</body>
</html>
