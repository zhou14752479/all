<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="data-list.aspx.cs" Inherits="药价中台.data_list" %>

<!DOCTYPE html>
<html class="fly-html-layui fly-html-store">
<head>
    <title>后台管理</title>
    <meta charset="utf-8">
    <meta name="renderer" content="webkit">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <meta name="referrer" content="never">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
  
    <link rel="shortcut icon" href="/favicon.ico" type="image/x-icon" />
    <link rel="stylesheet" href="./css/font.css">
    <link rel="stylesheet" href="./css/xadmin.css">
    <script src="./js/md5.min_1.js"></script>
    <script type="text/javascript" src="https://cdn.bootcss.com/jquery/3.2.1/jquery.min.js"></script>
    <script src="./lib/layui/layui.js" charset="utf-8"></script>
    <script type="text/javascript" src="./js/xadmin.js"></script>
    <script type="text/javascript" src="./js/function.js"></script>
</head>

<body>
    <div class="x-nav">
        <span class="layui-breadcrumb">
            <a href="">首页</a>
            <a href="">数据管理</a>
          
        </span>
        <a class="layui-btn layui-btn-small" style="line-height:1.6em;margin-top:3px;float:right" href="javascript:location.replace(location.href);" title="刷新">
            <i class="layui-icon" style="line-height:30px">ဂ</i>
        </a>
    </div>
    <div class="x-body">
        
     
        <xblock style="text-align:center">
         <input id="username" />
           <span style="font-size:20px">导入表格</span>： <input class="layui-btn"  type="file" id="export" >
      
             <button class="layui-btn" id="getprice" lay-submit="" lay-filter="sreach">查询价格</button>
             <button class="layui-btn" id="deleteprice" lay-submit="" lay-filter="sreach">清空记录</button>
        </xblock>

        <table class="layui-table" id="datatable">
            <thead>
                <tr>
                
                
                    <th>批准文号</th>
                    <th>药品名称</th>
                    <th>规格</th> 
                    <th>价格</th> 
                    <th>药房价格</th>
                     <th>查询时间</th> 
                </tr>
            </thead>
            <tbody></tbody>
        </table>
            
        <div id="page" style="text-align: center;"></div>
    </div>

     <script src="https://cdn.bootcss.com/jquery/3.2.1/jquery.js"></script>
    <script src="https://cdn.bootcss.com/xlsx/0.11.5/xlsx.core.min.js"></script>
    <script>

    //给input标签绑定change事件，一上传选中的.xls文件就会触发该函数
            $('#export').change(function(e) {
        var files = e.target.files;
            var fileReader = new FileReader();
            fileReader.onload = function(ev) {
            try {
                var data = ev.target.result
            var workbook = XLSX.read(data, {
                type: 'binary'
                }) // 以二进制流方式读取得到整份excel表格对象
            var persons = []; // 存储获取到的数据
            } catch (e) {
                console.log('文件类型不正确');
            return;
            }
            // 表格的表格范围，可用于判断表头是否数量是否正确
            var fromTo = '';
            // 遍历每张表读取
            for (var sheet in workbook.Sheets) {
                if (workbook.Sheets.hasOwnProperty(sheet)) {
                fromTo = workbook.Sheets[sheet]['!ref'];
            console.log(fromTo);
            persons = persons.concat(XLSX.utils.sheet_to_json(workbook.Sheets[sheet]));
                    //break; // 如果只取第一张表，就取消注释这行
                }
            }
            //在控制台打印出来表格中的数据
            console.log(persons);
               // $("#area").val(JSON.stringify(persons));

                var data = persons;
                for (i in data) //data.data指的是数组，数组里是8个对象，i为数组的索引
                {
                    var tr;
                    tr = '<td>' + data[i].wenhao + '</td>' + '<td>' + data[i].name + '</td>' + '<td>' + data[i].guige + '</td>' + '<td>' + data[i].price + '</td>' + '<td>...</td>' + '<td>正在查价</td>' 
                       
                    $("#datatable").append('<tr>' + tr + '</tr>')
                }
                    $.ajax({
                        url: "api.aspx?method=uploadprice&username=<%=Application["username"]%>",
                        async: true,
                        type: 'post',
                        dataType: 'JSON',
                        contentType: 'application/json',
                        data: JSON.stringify(persons),
                        success: function (data) {
                            layer.alert("比价完成");
                           
                        }
                    });
              
                layer.alert("比价完成，点击查询获取比价");
        };
            // 以二进制方式打开文件
            fileReader.readAsBinaryString(files[0]);
            });




        $("#getprice").click(function () {

            $("#datatable  tbody").html(""); //翻页清空表数据，保留表头
            var username = $("#username").val().trim()
        
            $.ajax({
                url: `api.aspx?method=getprice&username=${username}`,
               async: true,
               type: 'post',
               data: '',
                success: function (data) {

                    var data = JSON.parse(data);
                    console.log(data);
                    for (i in data) //data.data指的是数组，数组里是8个对象，i为数组的索引
                    {
                        var tr;
                        tr = '<td>' + data[i].wenhao + '</td>' + '<td>' + data[i].name + '</td>' + '<td>' + data[i].guige + '</td>' + '<td>' + data[i].price + '</td>' + '<td>' + data[i].yfprice + '</td>' + '<td>' + data[i].time + '</td>' 

                        $("#datatable").append('<tr>' + tr + '</tr>')
                    }
                    layer.alert("查询完成");


               }
           });
        })

        $("#deleteprice").click(function () {

            $("#datatable  tbody").html(""); //翻页清空表数据，保留表头
            var username = $("#username").val().trim()
  
           $.ajax({
               url: `api.aspx?method=deleteprice&username=${username}`,
               async: true,
               type: 'post',
               data: '',
               success: function (data) {

                   console.log(data);
                   layer.alert(data);


               }
           });
       })
  


    </script>

</body>

</html>
