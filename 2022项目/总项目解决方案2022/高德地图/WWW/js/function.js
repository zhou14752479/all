
var domainhost = '8.155.4.205';

$(document).ready(function () {

    if (getCookie("username")) {
        var username = getCookie("username");
        var password = getCookie("password");
        getuserinfo(username, password);

        if (document.querySelector("#nickname") != null) {
            document.querySelector("#nickname").innerHTML = username;
        }

    }
    else {
        var pathurl = window.location.pathname;
       
        if (pathurl == '/admin/login.html' || pathurl == '/admin/register.html') {

        }
        else {

            location.href = 'login.html'
        }

    }
})


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

function getsign() {

    var timestamp = (new Date()).getTime();
    return md5(timestamp);
}


function setCookie(c_name, value, expireSeconds) {
    date = new Date();
    date.setTime(date.getTime() + expireSeconds * 1000); //设置date为当前时间+expireSeconds秒
    document.cookie = c_name + "=" + value + "; expires=" + date.toGMTString(); //将date赋值给expires
}

function loginout() {
    var userid = getCookie("userid");
    var username = getCookie("username");
    var password = getCookie("password");
    setCookie('userid', userid, -3600)
    setCookie('username', username, -3600)
    setCookie('password', password, -3600)
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


function register(username, passwordMd5) {
    var sign = getsign();
    var url = `http://${domainhost}:8080/api/register.html?username=${username}&password=${passwordMd5}&sign=${sign}`
    $.get(url, function (data) {

        if (data.status == "1") {
            layer.msg("注册成功，即将跳转登录页面......")
            setTimeout("location.href='login.html'", 2000);



        }
        else {
            layer.msg("注册失败请联系客服")

        }
    })
}


function login(username, password) {
   
    $.ajax({
    url: `http://${domainhost}/main.aspx?method=login&username=${username}&password=${password}`,
    method: 'POST',
    dataType: 'json', // 明确指定返回数据类型为JSON
    success: function (data) {
       if (data.status == "1") {
            setCookie('userid', data.userid, 3600 * 24 * 3)
            setCookie('username', data.username, 3600 * 24 * 3)
            layer.msg("登录成功，即将跳转后台......")
            setTimeout(" location.href='admin.html'", 2000);

            document.querySelector("#nickname").innerHTML = usernmae;
        }
        else {
            layer.msg("密码错误")

        }
    },
    error: function (error) {
        console.error('请求出错:', error);
    }
});
}


function getuserinfo(username, passwordMd5) {
    var sign = getsign();
    var url = `http://${domainhost}:8080/api/login.html?username=${username}&password=${passwordMd5}&sign=${sign}`
    $.get(url, function (data) {
        console.log(data.status)
        if (data.status == "0") {
            var userid = getCookie("userid");
            var username = getCookie("username");
            var password = getCookie("password");

            setCookie('userid', '', -3600)
            setCookie('username', '', -3600)
            setCookie('password', '', -3600)
        }

        if (data.status == "1") {
            if (document.querySelector("#nickname") != null) {
                if (data.isvip == "0") {
                    document.querySelector("#user_vip_txt").innerHTML = "普通用户";
                }
                if (data.isvip == "1") {
                    document.querySelector("#user_vip_txt").innerHTML = "基础会员";
                }
                if (data.isvip == "2") {
                    document.querySelector("#user_vip_txt").innerHTML = "高级会员";
                }
            }

        }

    })
}

function getdata(page, taskid) {
   
    $("#datatable  tbody").html(""); //翻页清空表数据，保留表头

    var userid = getCookie("userid");
    $(function () {
        $.ajax({
            url: `http://${domainhost}/main.aspx?method=getdata&taskid=${taskid}&page=${page}`,
            type: 'get',
            dataType: 'json',
            success: function (data) {
                console.log(data);
                //方法中传入的参数data为后台获取的数据
                for (i in data) //data.data指的是数组，数组里是8个对象，i为数组的索引
                {
                    var tr;
                    tr = '<td><div class="layui-unselect layui-form-checkbox" lay-skin="primary" data-id="2"><i class="layui-icon">&#xe605;</i></div></td><td>' + i + '</td>' + '<td>' + data[i].name + '</td>' + '<td>' + data[i].addr + '</td>' + '<td>' + data[i].tel + '</td>' + '<td>' + data[i].area + '</td>' 

                    $("#datatable").append('<tr>' + tr + '</tr>')
                }
            }
        })
    })


}




function gettask() {
    
    $("#tasktable  tbody").html(""); //翻页清空表数据，保留表头
    var userid = getCookie("userid");
    $(function () {
        $.ajax({
            url: `http://${domainhost}/main.aspx?method=gettask&userid=${userid}`,
            type: 'get',
            dataType: 'json',
            success: function (data) {

                console.log(data);
                //方法中传入的参数data为后台获取的数据
                for (i in data) //data.data指的是数组，数组里是8个对象，i为数组的索引
                {
                    var tr;
                    tr = '<td><div class="layui-unselect layui-form-checkbox" lay-skin="primary" data-id=' + data[i].taskid + '><i class="layui-icon">&#xe605;</i></div></td><td>' + data[i].taskid + '</td>' + '<td>' + data[i].taskname + '</td>' + '<td>' + data[i].city + '</td>' + '<td>' + data[i].keyword + '</td>' + '<td>' + data[i].time + '</td><td ' +
                        'class="td-status">' +
                        '<span class="layui-btn layui-btn-normal layui-btn-mini">' + data[i].status + '</span></td><td class="td-manage">' +
                        '<a title="启动任务"  onclick=\'task_start(' + data[i].taskid + ',"' + data[i].city + '","' + data[i].keyword + '")\' href="javascript:;">' +
                        '<i class="layui-icon" style="font-size: 20px;">&#xe623;</i>&nbsp;&nbsp;&nbsp;&nbsp;' +
                        '</a>' +
                        '<a title="删除任务" onclick="task_del(this,' + data[i].taskid + ')" href="javascript:;">' +
                        '<i class="layui-icon" style="font-size: 20px;">&#xe640;</i>&nbsp;&nbsp;&nbsp;&nbsp;' +
                        '</a>' +
                        '<a title="导出数据" onclick="x_admin_show(\'数据管理\',\'data-list.html?taskid=' + data[i].taskid + '&taskname=' + data[i].taskname + '\')" href="javascript:;">' +
                        '<i class="layui-icon" style="font-size: 20px;">&#xe62d;</i>' +
                        '</a>' +
                        '</td>'


                    $("#tasktable").append('<tr>' + tr + '</tr>')
                }
            }
        })
    })


}


function task_start(taskid, city, keyword) {
    layer.confirm('确认要启动该任务吗？', function (index) {


       
        var userid = getCookie("userid");
        var url = `http://${domainhost}/main.aspx?method=starttask&taskid=${taskid}&city=${city}&keyword=${keyword}`
        $.get(url, function (data) {

           
                layer.msg("启动成功，任务完成后状态将更新为已完成")
                setTimeout(" location.href='javascript:location.replace(location.href);'", 2000);

        

        },"json")

    })

}




function task_del_func(taskid) {
   
        
        var userid = getCookie("userid");
        var url = `http://${domainhost}:8080/api/task_del.html?userid=${userid}&taskid=${taskid}`
        $.get(url, function (data) {
            if (data.status == "1") {

              
            }
            else {
                layer.msg("删除失败")


            }


        })

}


function filedata_del_func() {
   
        var sign = getsign();
        var userid = getCookie("userid");
        var url = `http://${domainhost}:8080/api/filedata_del.html?userid=${userid}&sign=${sign}`
        $.get(url, function (data) {
            if (data.status == "1") {

              
            }
            else {
                layer.msg("删除失败")


            }


        })

}



function task_add(taskname, city, keyword) {
   
    var userid = getCookie("userid");
    $.ajaxSettings.async = false;//设置为同步
   var url= `http://${domainhost}/main.aspx?method=addtask&userid=${userid}&taskname=${taskname}&keyword=${keyword}&city=${city}`;
    $.get(url, function (data) {

        if (data.status == "1") {

            var index = parent.layer.getFrameIndex(window.name);
            //关闭当前frame
            parent.layer.close(index);

            alert("添加成功")
           // $("#refresh").click();


        }
        else {
            layer.msg("添加失败")


        }


    }, 'json')
}











function exportExcel(taskid) {
  
    var url = `http://${domainhost}/main.aspx?method=createExcel&taskid=${taskid}`
    $.get(url, function (data) {

        layer.msg("正在生成表格...，")
       
         setTimeout(`location.href='/admin/excel/${taskid}.xlsx'`, 5000);

    })


}








function getdownloadfiles() {
    var sign = getsign();
    $("#getdownloadfilestable  tbody").html(""); //翻页清空表数据，保留表头
    var userid = getCookie("userid");
    $(function () {
        $.ajax({
            url: `http://${domainhost}:8080/api/downloadfiles_get.html?userid=${userid}&sign=${sign}`,
            type: 'get',
            dataType: 'json',
            success: function (data) {

                console.log(data);
                //方法中传入的参数data为后台获取的数据
                for (i in data.data) //data.data指的是数组，数组里是8个对象，i为数组的索引
                {
                    var tr;
                    tr = '<td>' + data.data[i].id + '</td>' + '<td>' + data.data[i].filename + '</td><td ' +
                        '<td>' + data.data[i].createtime + '</td><td ' +
                        'class="td-status">' +
                        '<a title="下载"  href="http://www.acaiji.com/excel/' + data.data[i].filename + '">' +
                        '<span class="layui-btn layui-btn-normal layui-btn-' + data.data[i].btnmsg + '">' + data.data[i].msg + '</span>' +
                        '</a>' +
                        '</td>'


                    $("#getdownloadfilestable").append('<tr>' + tr + '</tr>')



                }
            }
        })
    })


}