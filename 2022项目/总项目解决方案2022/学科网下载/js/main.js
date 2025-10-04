
document.addEventListener('DOMContentLoaded', function() {
        document.getElementById('search-btn').addEventListener('click', function() {
            
            document.getElementById('loading').style.display = 'block';
            document.getElementById('error').style.display = 'none';
            document.getElementById('result-container').style.display = 'none';
             

            const key = $('input[name="key"]').val();
            const link = $('input[name="link"]').val();


            if(key=='' || link=='')
            {
                 showError('秘钥为空 或 资料网址格式不正确');
                 document.getElementById('loading').style.display = 'none';
                 return;
              }
            // 模拟API请求延迟
            setTimeout(() => {
                try {
                    

                   $.ajax({
                url:"http://113.250.184.46:8888/api.aspx?method=getfile" , // 请求路径
                type:"GET" , //请求方式
                data: "key="+key+"&link="+link,//请求参数
                
                contentType:"application/x-www-form-urlencoded",
                dataType: 'json',
                success:function (data) {
                  showError(data.msg);
                 document.getElementById('filename').textContent = data.filename;
				 document.getElementById('extime').textContent = data.extime;
				 document.getElementById('cishu').textContent = data.cishu;
                 document.getElementById("mylink").href = data.fileurl;
                 
                 var nowtime=getFormattedDateTime();
                 
				
                 if(data.fileurl !=null && data.fileurl !='')
                 {
                    addDetailItem(data.fileurl, data.filename,nowtime );
                 window.open(data.fileurl)
                  }
                
                },//响应成功后的回调函数
                error:function () {
                    alert
                    showError('下载出错1，请联系客服');
                },//表示如果请求响应出现错误，会执行的回调函数     
                  });
  




                   
                    displayResult();

                } catch (error) {
                    showError('下载出错2，请联系客服'+error);
                } finally {
                    document.getElementById('loading').style.display = 'none';
                }
            }, 100);
        });
 });


        function displayResult() {
           
            //document.getElementById('qq-nickname').textContent = ``;
            // 动画效果
            setTimeout(() => {
                document.getElementById('value-bar').style.width = `100%`;
            }, 100);
             
            document.getElementById('result-container').style.display = 'block';
        }
 
      
 
        function showError(message) {
            const errorEl = document.getElementById('error');
            errorEl.textContent = message;
            errorEl.style.display = 'block';

        }
        function openPopup() {
           window.open('soft_Secure.rar', '_blank'); // 打开新窗口
       }


       document.addEventListener('DOMContentLoaded', function() {
    const params = new URLSearchParams(window.location.search);
    const KeyValue = params.get('key');
    const inputElement = document.getElementById('qq-input');

    if (KeyValue) {
        inputElement.value = KeyValue;
    }

    });



function getFormattedDateTime() {
  const now = new Date();
  
  // 获取月、日、时、分、秒，并补零（如个位数前加0）
  const month = now.getMonth() + 1; // 月份从0开始，所以+1
  const day = now.getDate();
  const hours = String(now.getHours()).padStart(2, '0');
  const minutes = String(now.getMinutes()).padStart(2, '0');
  const seconds = String(now.getSeconds()).padStart(2, '0');
  
  // 拼接格式：月-日 时:分:秒
  return `${month}-${day} ${hours}:${minutes}:${seconds}`;
}




   function addDetailItem(aaa, bbb, ccc) {
    // 创建最外层的div元素
    const detailItem = document.createElement('div');
    detailItem.className = 'detail-item';

    // 创建左侧带有链接的span元素
    const detailLabel = document.createElement('span');
    detailLabel.className = 'detail-label';

    // 创建a标签
    const link = document.createElement('a');
    link.href = aaa;
    link.textContent = bbb;
    link.target = '_blank'; // 在新窗口打开
    link.rel = 'noopener noreferrer'; // 安全防护
    detailLabel.appendChild(link);

    // 创建右侧的分数span元素
    const detailScore = document.createElement('span');
    detailScore.className = 'detail-score';
    detailScore.textContent = ccc;

    // 把两个span添加到最外层的div中
    detailItem.appendChild(detailLabel);
    detailItem.appendChild(detailScore);

    // 把创建好的元素添加到页面中id为container的元素里
    document.getElementById('container').appendChild(detailItem);
}

// 使用示例


