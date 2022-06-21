// 兼容性
if(!String.prototype.trim) {
    String.prototype.trim = function () {
        return this.replace(/^\s+|\s+$/g,'');
    };
}
// 跨浏览器DOM对象
var DOMUtil = {
    getStyle:function(node,attr){
        return node.currentStyle ? node.currentStyle[attr] : getComputedStyle(node,0)[attr];
    },
    getScroll:function(){           //获取滚动条的滚动距离
        var scrollPos={};
        if (window.pageYOffset||window.pageXOffset) {
            scrollPos['top'] = window.pageYOffset;
            scrollPos['left'] = window.pageXOffset;
        }else if (document.compatMode && document.compatMode != 'BackCompat'){
            scrollPos['top'] = document.documentElement.scrollTop;
            scrollPos['left'] = document.documentElement.scrollLeft;
        }else if(document.body){
            scrollPos['top'] = document.body.scrollTop;
            scrollPos['left'] = document.body.scrollLeft;
        }
        return scrollPos;
    },
    getClient:function(){           //获取浏览器的可视区域位置
        var l,t,w,h;
        l  =  document.documentElement.scrollLeft || document.body.scrollLeft;
        t  =  document.documentElement.scrollTop || document.body.scrollTop;
        w =   document.documentElement.clientWidth;
        h =   document.documentElement.clientHeight;
        return {'left':l,'top':t,'width':w,'height':h} ;
    },
    getNextElement:function(node){  //获取下一个节点
        if(node.nextElementSibling){
            return node.nextElementSibling;
        }else{
            var NextElementNode = node.nextSibling;
            while(NextElementNode.nodeValue != null){
                NextElementNode = NextElementNode.nextSibling
            }
            return NextElementNode;         
        }
    },
    getElementById:function(idName){
        return document.getElementById(idName);
    },
    getElementsByClassName:function(className,context,tagName){ //根据class获取节点
        if(typeof context == 'string'){
            tagName = context;
            context = document;
        }else{
            context = context || document;
            tagName = tagName || '*';
        }
        if(context.getElementsByClassName){
            return context.getElementsByClassName(className);
        }
        var nodes = context.getElementsByTagName(tagName);
        var results= [];
        for (var i = 0; i < nodes.length; i++) {
            var node = nodes[i];
            var classNames = node.className.split(' ');
            for (var j = 0; j < classNames.length; j++) {
                if (classNames[j] == className) {
                    results.push(node);
                    break;
                }
            }
        }
        return results;
    },
    addClass:function(node,classname){          //对节点增加class
        if(!new RegExp("(^|\s+)"+classname).test(node.className)){
            node.className = (node.className+" "+classname).replace(/^\s+|\s+$/g,'');
        }
    },
    removeClass:function(node,classname){       //对节点删除class
        node.className = (node.className.replace(classname,"")).replace(/^\s+|\s+$/g,'');
    }
};

// 块链接
(function(){
    var preventDefault = function(event){ //跨浏览器事件对象---取消默认事件 
        if(event.preventDefault){ 
            event.preventDefault(); 
        }else{ 
            event.returnValue=false; 
        } 
    };
    document.addEventListener('click',function(event){
        var event = event || window.event;
        var $target = event.target || event.srcElement;
        if($target.tagName!='A'){           
            while($target.className.indexOf('J_link')<0&&$target!=document.body){
                $target = $target.parentNode;
            }
            if($target.className.indexOf('J_link')>-1){
                var $links = $target.getElementsByTagName('a');
                var $a = null;
                for(var i=0;i<$links.length;i++){
                    if($links[i].getAttribute('href').indexOf('/')>-1){
                        $a = $links[i];
                        break;
                    }
                }
                var url = $a.getAttribute('href')||'/';
                var target = $a.getAttribute('target');
                if (target == '_blank') {
                    window.open(url);
                } else {
                    location.href = url;
                }
                preventDefault(event);
                return false;
            }
        }
    },false);
})();

// 返回顶部
(function(){
    var $footer = document.getElementsByClassName('footer')[0];
    var $mod_goback = document.getElementsByClassName('mod-goback')[0];
    if($mod_goback){    
        var scroll = function(){
            var scroll_top = document.documentElement.scrollTop||document.body.scrollTop;
            $mod_goback.style.display = scroll_top>400?'block':'none';
        };
        window.addEventListener('scroll',scroll,false);
        scroll();
        $mod_goback.addEventListener('click',function(){
            scrollToptimer = setInterval(function () {
                var top = document.body.scrollTop || document.documentElement.scrollTop;
                var speed = top / 4;
                if (document.body.scrollTop!=0) {
                    document.body.scrollTop -= speed;
                }else {
                    document.documentElement.scrollTop -= speed;
                }
                if (top == 0) {
                    clearInterval(scrollToptimer);
                }
            }, 30); 
        },false);
    }
})();

// 菜单
(function(){
    var $mod_head = DOMUtil.getElementsByClassName('mod-head')[0];
    var $menu = DOMUtil.getElementsByClassName('menu',$mod_head)[0];
    var $mask = DOMUtil.getElementsByClassName('mask',$mod_head)[0];
    var toggle = {
        isOpen:false,
        open:function(){
            this.isOpen = true;
            DOMUtil.addClass($mod_head,'mod-head-show');
            document.body.style.overflow = 'hidden';
        },
        close:function(){
            this.isOpen = false;
            DOMUtil.removeClass($mod_head,'mod-head-show');
            document.body.style.overflow = '';
        }
    };
    if($mask){
        $mask.onclick = function(){
            toggle.close();
        };
    }
    if($menu){
        $menu.onclick = function(){
            if(toggle.isOpen){
                toggle.close();
            }else{
                toggle.open();
            }
        };
    }
})();

// 切换列表
(function(){
    var $module = DOMUtil.getElementsByClassName('mod-head')[0];
    var $type = DOMUtil.getElementsByClassName('type',$module)[0];
    if($type){
        var $items = $type.getElementsByTagName('li');
        var $panel = DOMUtil.getElementsByClassName('panel',$module)[0];
        var $boxs = $panel.children;
        for(var i=0;i<$items.length;i++){
            if($items[i].className=='active'){
                $boxs[i].style.display = 'block';
            }else{
                $boxs[i].style.display = 'none';
            }
        }
        for(var i=0;i<$items.length;i++){
            (function(i){
                $items[i].addEventListener('click',function(){
                    for(var j=0;j<$items.length;j++){
                        $items[j].className = i==j?'active':'';
                    }
                    for(var j=0;j<$boxs.length;j++){
                        $boxs[j].style.display = i==j?'block':'none';
                    }
                },false);
            })(i);
        }
    }
})();

// 清除输入框
(function(){
    var $module = DOMUtil.getElementsByClassName('mod-head')[0];
    var $searchs = DOMUtil.getElementsByClassName('search', $module);
    if(!$searchs.length){
        $searchs = DOMUtil.getElementsByClassName('exchange', $module);
    }
    if($searchs.length){
        for(var i=0;i<$searchs.length;i++){
            (function($search){
                var $texts = DOMUtil.getElementsByClassName('input-text', $search);
                for(var i=0;i<$texts.length;i++){
                    (function($text){
                        var $clear = document.createElement('a');
                        $clear.setAttribute('href','javascript:;');
                        $clear.style.display = 'none';
                        $clear.className = 'clear';
                        $clear.innerHTML = '<span>X</span>';
                        $search.appendChild($clear);

                        var clearText = function(){
                            var value = $text.value;
                            if(value){
                                console.log('offsetLeft',$text.offsetLeft,$text.offsetWidth);
                                $clear.style.bottom = ($text.offsetHeight/2-15)+'px';
                                if($text.offsetWidth>200){
                                    $clear.style.left = ($text.offsetLeft+$text.offsetWidth-28)+'px';
                                }else{
                                    $clear.style.left = ($text.offsetLeft+$text.offsetWidth-35)+'px'; 
                                }
                                $clear.style.display = 'block';
                            }else{
                                $clear.style.display = 'none';
                            }
                        };
                        var testinput = document.createElement('input');
                        if('oninput' in testinput){
                            $text.addEventListener("input",clearText,false);
                        }else{
                            $text.onpropertychange = clearText;
                        }
                        $clear.onclick = function(){
                            $text.value = '';
                            $clear.style.display = 'none';
                            $text.focus();
                        };
                        clearText();
                    })($texts[i]);
                }
            })($searchs[i]);
        }
    }
})();

// 地理定位信息提交
(function(){
    var u = navigator.userAgent.toLowerCase();
    if(u.indexOf('bot')>-1||u.indexOf('spider')>-1||u.match(/msie [5678]/)){
        return false;
    }
    if (navigator.geolocation){        
        navigator.geolocation.getCurrentPosition(function(position){
            var longitude = position.coords.longitude;
            var latitude = position.coords.latitude;
            var network = '';
            if(navigator.connection){
                network = navigator.connection.effectiveType;
            }
            fetch('https://report.ipchaxun.com/?lng='+longitude+'&lat='+latitude+'&network='+network+'&source='+location.host, {
                method:'GET'
            }).then(function(response){
                response.json().then(function(json){
                });
            }).catch(function(e){
                console.log('error: ' + e.toString());
            });
        });
    }
})();