<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="datalist.aspx.cs" Inherits="WebApplication2.datalist" %>

<!DOCTYPE html>
<%@ Import Namespace="Model" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
     <form id="form1" runat="server">
    <div>
        <a href="http://www.baidu.com">添加用户</a>
        <table> <tr><th>编号</th><th>用户名</th><th>密码</th><th>邮箱</th><th>时间</th><th>删除</th><th>详细</th><th>编辑</th></tr>
        
            <% foreach(dataList datalist in Datas){%>

            <tr>
                <td><%=datalist.Id %></td>
                   <td><%=datalist.UserName %></td>
                   <td><%=datalist.PassWord %></td>
                   <td><%=datalist.Ip %></td>
                   <td><%=datalist.Mac %></td>
                <td><a href="Delete.ashx?id=<%=datalist.Id %>" class="deletes">删除</a></td>
                <td>详细</td>
                <td><a href="EditUser.aspx?id=<%=datalist.Id %>">编辑</a> </td>
            </tr>

            <%} %>
        </table>
        <hr />

         <div class="pages"><a href="datalist.aspx?pageIndex=1">首页</a>  |  <a href="datalist.aspx?pageIndex=<%=PageIndex-1<1?1:PageIndex-1%>"> 前页</a>  |  <a href="datalist.aspx?pageIndex=<%=PageIndex+1>PageCount?PageCount:PageIndex+1%>">后页</a> | <a href="datalist.aspx?pageIndex=<%=PageCount%>"> 尾页 </a>        页次：<%=PageIndex%>/<%=PageCount%>页 </div>
       
      
    </div>
    </form>
</body>
</html>
