
var domainhost = 'localhost';

$(document).ready(function () {

    if (getCookie("username")) {
        var username = getCookie("username");
        var password = getCookie("password");
    var usertype= getCookie("usertype");

        if (document.querySelector("#nickname") != null) {
            document.querySelector("#nickname").innerHTML = username;
             document.querySelector("#user_vip_txt").innerHTML = usertype;
        }

          if(usertype=='商户')
        {
            $("#userdisplay").css("display", "none");
             $("#xmadddisplay").css("display", "none");
        }

    }
    else {
       location.href = 'login.html'

    }
})




function getQueryString(name) {
    var url = location.search; //获取url中"?"符后的字串
    var theRequest = new Object();
    if (url.indexOf("?") != -1) {
        var str = url.substr(1);
        strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {
            theRequest[strs[i].split("=")[0]] = decodeURI(strs[i].split("=")[1]);
        }
    }
    return theRequest[name];
}


function getCookie(c_name) {
    if (document.cookie.length > 0) {
        c_start = document.cookie.indexOf(c_name + "=")
        if (c_start != -1) {
            c_start = c_start + c_name.length + 1
            c_end = document.cookie.indexOf(";", c_start)
            if (c_end == -1) c_end = document.cookie.length
            return unescape(document.cookie.substring(c_start, c_end))
        }
    }
    return ""
}







function loginout() {
    var userid = getCookie("userid");
    var username = getCookie("username");
    var password = getCookie("password");
    setCookie('userid', userid, -3600)
    setCookie('username', username, -3600)
    setCookie('password', password, -3600)
     setCookie('usertype', '', -3600)
}

function timestampToTime(timestamp) {
    var date = new Date(timestamp * 1000);//时间戳为10位需*1000，时间戳为13位的话不需乘1000
    var Y = date.getFullYear() + '-';
    var M = (date.getMonth() + 1 <= 10 ? '0' + (date.getMonth() + 1) : date.getMonth() + 1) + '-';
    var D = (date.getDate() + 1 <= 10 ? '0' + (date.getDate()) : date.getDate()) + ' ';
    var h = (date.getHours() + 1 <= 10 ? '0' + (date.getHours()) : date.getHours()) + ':';
    var m = (date.getMinutes() + 1 <= 10 ? '0' + (date.getMinutes()) : date.getMinutes()) + ':';
    var s = (date.getSeconds() + 1 <= 10 ? '0' + (date.getSeconds()) : date.getSeconds());
    return Y + M + D + h + m + s;
}



function user_add(username, password, shanghuname,usertype) {
  
    $.ajaxSettings.async = false;//设置为同步
   var url = `api.aspx?username=${username}&usertype=${usertype}&password=${password}&shanghuname=${shanghuname}&method=adduser`
    $.post(url, function (data) {

        if (data.status == "1") {
 layer.msg("添加成功")
            var index = parent.layer.getFrameIndex(window.name);
            //关闭当前frame
            //parent.layer.close(index);

            
            //$("#refresh").click();


        }
        else {
            layer.msg("添加失败")


        }


    }, "json")
}









function getusers() {
   
    $("#usertable  tbody").html(""); //翻页清空表数据，保留表头
   var usertype = getCookie("usertype");
    $(function () {
        $.ajax({
            url: `api.aspx?usertype=${usertype}&method=getusers`,
            type: 'post',
            dataType: 'json',
            success: function (data) {

                console.log(data);
                //方法中传入的参数data为后台获取的数据
                for (i in data) //data.data指的是数组，数组里是8个对象，i为数组的索引
                {
                    var tr;
                    tr = '<td>' + data[i].userid + '</td>'+'<td>' + data[i].username+ '</td>' + '<td>' + data[i].password + '</td>' + '<td>' + data[i].usertype+ '</td>' + '<td>' + data[i].shanghuname + '</td>'+ '<td>' + data[i].time + '</td>' + '<td class="td-manage">' +

                       '<a title="删除账号"  onclick="user_del(this,' + data[i].userid + ')" href="javascript:;">' +
                        '<i class="layui-icon" style="font-size: 20px;">&#xe640;</i>&nbsp;&nbsp;&nbsp;&nbsp;' +
                        '</a>' +
                      
                        
                        '</td>'


                    $("#usertable").append('<tr>' + tr + '</tr>')
                }
            }
        })
    })


}




function code_del(obj,id) {
            layer.confirm('确认要删除吗？', function (index) {
                //发异步删除数据
                code_del_func(id);
                $(obj).parents("tr").remove();
                layer.msg('已删除!', { icon: 1, time: 1000 });
            });
        }



function code_del_func(code) {
   
var url = `api.aspx?code=${code}&method=delcode`
        $.post(url, function (data) {
            if (data.status == "1") {

layer.msg("删除成功")
            }
            else {
                layer.msg("删除失败")


            }

 }, "json")
}






function user_del_func(userid) {
   

var url = `api.aspx?userid=${userid}&method=deluser`
        $.post(url, function (data) {
            if (data.status == "1") {

layer.msg("删除成功")
            }
            else {
                layer.msg("删除失败")


            }

 }, "json")
}









function exportExcel(taskid, taskname) {
    var userid = getCookie("userid");
    var sign = getsign();
    var url = `http://${domainhost}:8080/api/exportExcel.html?userid=${userid}&taskid=${taskid}&taskname=${taskname}&sign=${sign}`
    $.post(url, function (data) {

        if (data.status == "1") {
            layer.msg("正在生成表格...，点击【下载列表】查看")



        }
        else {
            layer.msg("生成失败，请联系客服")
            return "";

        }


    })


}





function getcodes(username,code,xm_code) {
   
 var usertype = getCookie("usertype");
    $("#codetable  tbody").html(""); //翻页清空表数据，保留表头
   
    $(function () {
        $.ajax({
            url: `api.aspx?usertype=${usertype}&xm_code=${xm_code}&code=${code}&username=${username}&method=getcodes`,
            type: 'post',
            dataType: 'json',
            success: function (data) {

                console.log(data);
                //方法中传入的参数data为后台获取的数据
                for (i in data) //data.data指的是数组，数组里是8个对象，i为数组的索引
                {
                    var tr;

                   

                    tr = '<td>' + data[i].xm_code+ '</td>' + '<td>' + data[i].username+ '</td>' + '<td>' + data[i].xm_name+ '</td>' + '<td>' + data[i].code+ '</td>' + '<td>' + data[i].date + '</td>' +'<td>' + data[i].time + '</td>'+ '<td class="td-manage">' +

                       '<a title="删除券码"  onclick="code_del(this,\'' + data[i].code + '\')" href="javascript:;">' +
                        '<i class="layui-icon" style="font-size: 20px;">&#xe640;</i>&nbsp;&nbsp;&nbsp;&nbsp;' +
                        '</a>' +
                        '<a title="修改券码" onclick="x_admin_show(\'修改券码\',\'code-edite.html?code=' + data[i].code + '&xm_code=' + data[i].xm_code + '&username=' + data[i].username + '&xm_name=' + data[i].xm_name + '&date=' + data[i].date + '&time=' + data[i].time + '\',400,600)" href="javascript:;">' +
                        '<i class="layui-icon" style="font-size: 20px;">&#xe62d;</i>&nbsp;&nbsp;&nbsp;&nbsp;' +
                        '</a>' +
                        
                        '</td>'


                    $("#codetable").append('<tr>' + tr + '</tr>')
                }
            }
        })
    })


}


