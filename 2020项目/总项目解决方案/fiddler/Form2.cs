using Fiddler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fiddler
{
    public partial class Form2 : Form
    {
        static Proxy oSecureEndpoint;
        static string sSecureEndpointHostname = "localhost";
        static int iSecureEndpointPort = 8888;

        public Form2()
        {
            InitializeComponent();
        }

        private void btnstart_Click(object sender, EventArgs e)
        {


            btnstart.Enabled = false;
            Control.CheckForIllegalCrossThreadCalls = false;
            //设置别名
            Fiddler.FiddlerApplication.SetAppDisplayName("FiddlerCoreDemoApp");

            //启动方式
            FiddlerCoreStartupFlags oFCSF = FiddlerCoreStartupFlags.Default;

            //定义http代理端口
            int iPort = 8877;
            //启动代理程序，开始监听http请求
            //端口,是否使用windows系统代理（如果为true，系统所有的http访问都会使用该代理）我使用的是
            Fiddler.FiddlerApplication.Startup(iPort, true, true, true);

            // 我们还将创建一个HTTPS监听器，当FiddlerCore被伪装成HTTPS服务器有用
            // 而不是作为一个正常的CERN样式代理服务器。
            oSecureEndpoint = FiddlerApplication.CreateProxyEndpoint(iSecureEndpointPort, true, sSecureEndpointHostname);


            List<Fiddler.Session> oAllSessions = new List<Fiddler.Session>();

            //请求出错时处理
            //Fiddler.FiddlerApplication.BeforeReturningError += FiddlerApplication_BeforeReturningError;

            //在发送请求之前执行的操作
            Fiddler.FiddlerApplication.BeforeRequest += delegate (Fiddler.Session oS)
            {
                //请求的全路径
                Console.WriteLine("Before request for:\t" + oS.fullUrl);
                // 为了使反应篡改，必须使用缓冲模式
                // 被启用。这允许FiddlerCore以允许修改
                // 在BeforeResponse处理程序中的反应，而不是流
                // 响应给客户机作为响应进来。

                oS.bBufferResponse = true;
                Monitor.Enter(oAllSessions);
                oAllSessions.Add(oS);
                Monitor.Exit(oAllSessions);
                oS["X-AutoAuth"] = "(default)";

                /* 如果请求是要我们的安全的端点，我们将回显应答。

                注：此BeforeRequest是越来越要求我们两个主隧道代理和我们的安全的端点，
                让我们来看看它的Fiddler端口连接到（pipeClient.LocalPort）客户端，以确定是否该请求
                被发送到安全端点，或为了达到**安全端点被仅仅发送到主代理隧道（例如，一个CONNECT）。
                因此，如果你运行演示和参观的https：//本地主机：7777在浏览器中，你会看到
                Session list contains...

                    1 CONNECT http://localhost:7777
                    200                                         <-- CONNECT tunnel sent to the main proxy tunnel, port 8877
                    2 GET https://localhost:7777/
                    200 text/html                               <-- GET request decrypted on the main proxy tunnel, port 8877
                    3 GET https://localhost:7777/               
                    200 text/html                               <-- GET request received by the secure endpoint, port 7777
                */

                //oS.utilCreateResponseAndBypassServer();
                //oS.oResponse.headers.SetStatus(200, "Ok");
                //string str = oS.GetResponseBodyAsString();
                //oS.utilSetResponseBody(str + "aaaaaaaaaaaaaaaaaaaaa");

                if ((oS.oRequest.pipeClient.LocalPort == iSecureEndpointPort) && (oS.hostname == sSecureEndpointHostname))
                {
                    oS.utilCreateResponseAndBypassServer();
                    oS.oResponse.headers.SetStatus(200, "Ok");
                    oS.oResponse["Content-Type"] = "text/html; charset=UTF-8";
                    oS.oResponse["Cache-Control"] = "private, max-age=0";
                    oS.utilSetResponseBody("<html><body>Request for httpS://" + sSecureEndpointHostname + ":" + iSecureEndpointPort.ToString() + " received. Your request was:<br /><plaintext>" + oS.oRequest.headers.ToString());
                }
                //if ((oS.oRequest.pipeClient.LocalPort == 8877) && (oS.hostname == "www.baidu.com"))
                //{
                //    string url = oS.fullUrl;
                //    oS.utilCreateResponseAndBypassServer();
                //    oS.oResponse.headers.SetStatus(200, "Ok");
                //    oS.oResponse["Content-Type"] = "text/html; charset=UTF-8";
                //    oS.oResponse["Cache-Control"] = "private, max-age=0";
                //    oS.utilSetResponseBody("<html><body>Request for httpS://" + sSecureEndpointHostname + ":" + iSecureEndpointPort.ToString() + " received. Your request was:<br /><plaintext>" + oS.oRequest.headers.ToString());
                //}
            };

            /*
                // 下面的事件，您可以检查由Fiddler阅读每一响应缓冲区。  
             *     请注意，这不是为绝大多数应用非常有用，因为原始缓冲区几乎是无用的;它没有解压，它包括标题和正文字节数等。
                //
                // 本次仅适用于极少数的应用程序这就需要一个原始的，未经处理的字节流获取有用
                Fiddler.FiddlerApplication.OnReadResponseBuffer += new EventHandler<RawReadEventArgs>(FiddlerApplication_OnReadResponseBuffer);
            */


            Fiddler.FiddlerApplication.BeforeResponse += delegate (Fiddler.Session oS) {


                if (oS.fullUrl.IndexOf("https://www.baidu.com") >= 0)
                {
                    // 取消以下两条语句解压缩/ unchunk的
                    //HTTP响应，并随后修改任何HTTP响应，以取代
                    //单词“微软”和“Bayden”的实例。 你必须如此设置：
                    // set bBufferResponse = true inside the beforeREQUEST method above.
                    //
                    oS.utilDecodeResponse();
                    txtlog.AppendText("已抓到数据" + oS.fullUrl + Environment.NewLine);
                    txtlog.AppendText("已抓到数据" + oS.GetResponseBodyAsString() + Environment.NewLine);
                    if (txtlog.Lines.Count() > 100)
                    {
                        txtlog.Text = "";
                        txtlog.AppendText("清空缓存" + Environment.NewLine);
                    }
                   
                }
                //bool r = oS.utilReplaceInResponse("1.欢迎使用！", "aaaaaaaaaaaaaaaaaaaaaa");
                //Console.WriteLine(r);

            };

            //Fiddler.FiddlerApplication.AfterSessionComplete += delegate (Fiddler.Session oS)
            //{
            //    //Console.WriteLine("Finished session:\t" + oS.fullUrl); 
            //    Console.Title = ("Session list contains: " + oAllSessions.Count.ToString() + " sessions");
            //};
        }



        private void btnout_Click(object sender, EventArgs e)
        {
            btnstart.Enabled = true;
            if (null != oSecureEndpoint) oSecureEndpoint.Dispose();
            Fiddler.FiddlerApplication.Shutdown();
            Thread.Sleep(500);

        }

        private void txtview_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
