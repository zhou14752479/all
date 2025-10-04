<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="视频号web端.index" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>微信扫码登录</title>
    <style>
        .container {
            width: 100%;
            margin: 50px auto;
            text-align: center;
        }
        #qrCodeImage {
            border: 1px solid #ccc;
            padding: 10px;
            margin: 20px 0;
        }
        #statusMessage {
            margin: 15px 0;
            padding: 10px;
            border-radius: 4px;
            background-color: #f5f5f5;
        }
        .btn-refresh {
            background-color: #007bff;
            color: white;
            border: none;
            padding: 8px 16px;
            border-radius: 4px;
            cursor: pointer;
        }
        .btn-refresh:hover {
            background-color: #0056b3;
        }

         #gonggaoLabel {
     color:red;
     font-size:30px;
 }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        
        <div class="container">
            <h2>微信扫码登录</h2>
            
            
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Image ID="qrCodeImage" runat="server" Width="250" Height="250" />
                    
                    <div id="statusMessage">
                        <asp:Label ID="statusLabel" runat="server" Text="请扫描二维码登录"></asp:Label>
                    </div>
                    
                    <asp:Button ID="refreshButton" runat="server" Text="生成二维码" 
                        CssClass="btn-refresh" OnClick="refreshButton_Click" />
                    
                    <asp:HiddenField ID="tokenField" runat="server" />
                    <asp:Timer ID="statusTimer" runat="server" Interval="2000" OnTick="statusTimer_Tick"></asp:Timer>
                </ContentTemplate>
            </asp:UpdatePanel>
            
            <pre><asp:Label ID="gonggaoLabel" runat="server" Text="暂无公告"></asp:Label></pre>
        </div>
    </form>
</body>
</html>
