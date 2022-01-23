using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 短信群发
{
    public partial class 短信群发 : Form
    {
        public 短信群发()
        {
            InitializeComponent();
        }
        public void sendMsg(string mobile)
        {

            //IClientProfile profile = DefaultProfile.GetProfile("default", "LTAIKq0r0wXeCOO7", "U2IDaGVEH3cl9Xsv9RKnodi5BA0ZML");
            //DefaultAcsClient client = new DefaultAcsClient(profile);
            //CommonRequest request = new CommonRequest();
            //request.Method = MethodType.POST;
            //request.Domain = "dysmsapi.aliyuncs.com";
            //request.Version = "2017-05-25";
            //request.Action = "SendSms";
            //// request.Protocol = ProtocolType.HTTP;
            //request.AddQueryParameters("SignName", "舆情宝");
            //request.AddQueryParameters("PhoneNumbers", mobile);
            //request.AddQueryParameters("TemplateCode", "SMS_167963792");
            //request.AddQueryParameters("TemplateParam", "{'code':'8888',}");
            //try
            //{
            //    CommonResponse response = client.GetCommonResponse(request);
            //    Console.WriteLine(System.Text.Encoding.Default.GetString(response.HttpResponse.Content));

            //}
            //catch (ServerException ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
            //label7.Text = "已成功发送消息！";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            sendMsg("测试");
        }
    }
}
