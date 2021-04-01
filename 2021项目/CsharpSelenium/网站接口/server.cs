using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 网站接口
{
    class server
    {
        string requesturl = "";
        //string parames = "";

        public bool running = false; // Is it running?

        private int timeout = 8; // Time limit for data transfers.
        private Encoding charEncoder = Encoding.UTF8; // To encode string
        private Socket serverSocket; // Our server socket
        private string contentPath; // Root path of our contents

        // Content types that are supported by our server
        // You can add more...
        // To see other types: 
        private Dictionary<string, string> extensions = new Dictionary<string, string>()
{ 
    //{ "extension", "content type" }
    { "htm", "text/html" },
    { "html", "text/html" },
    { "xml", "text/xml" },
    { "txt", "text/plain" },
    { "css", "text/css" },
    { "png", "image/png" },
    { "gif", "image/gif" },
    { "jpg", "image/jpg" },
    { "jpeg", "image/jpeg" },
    { "zip", "application/zip"}
};

        public bool start(IPAddress ipAddress, int port, int maxNOfCon, string contentPath)
        {
            if (running) return false; // If it is already running, exit.

            try
            {
                // A tcp/ip socket (ipv4)
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                               ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(ipAddress, port));
                serverSocket.Listen(maxNOfCon);
                serverSocket.ReceiveTimeout = timeout;
                serverSocket.SendTimeout = timeout;
              
                running = true;
                this.contentPath = contentPath;
            }
            catch { return false; }

            // Our thread that will listen connection requests
            // and create new threads to handle them.
            Thread requestListenerT = new Thread(() =>
            {
                while (running)
                {
                    Socket clientSocket;
                    try
                    {
                        clientSocket = serverSocket.Accept();
                        // Create new thread to handle the request and continue to listen the socket.
                        Thread requestHandler = new Thread(() =>
                        {
                            clientSocket.ReceiveTimeout = timeout;
                            clientSocket.SendTimeout = timeout;
                            try { handleTheRequest(clientSocket); }
                            catch
                            {
                                try { clientSocket.Close(); } catch { }
                            }
                        });
                        requestHandler.Start();
                    }
                    catch { }
                }
            });
            requestListenerT.Start();

            return true;
        }


        public void stop()
        {
            if (running)
            {
                running = false;
                try { serverSocket.Close(); }
                catch { }
                serverSocket = null;
            }
        }

        /// <summary>
        /// strReceived//所有request参数包括网址
        /// </summary>
        /// <param name="clientSocket"></param>
        private void handleTheRequest(Socket clientSocket)
        {
            byte[] buffer = new byte[10240]; // 10 kb, just in case
            int receivedBCount = clientSocket.Receive(buffer); // Receive the request
            string strReceived = charEncoder.GetString(buffer, 0, receivedBCount);

            // Parse method of the request
            string httpMethod = strReceived.Substring(0, strReceived.IndexOf(" "));

            int start = strReceived.IndexOf(httpMethod) + httpMethod.Length + 1;
            int length = strReceived.LastIndexOf("HTTP") - start - 1;
            string requestedUrl = strReceived.Substring(start, length);

            //获取请求的网址
            requesturl = requestedUrl;
            //获取请求参数 strReceived请求参数列表

            // parames = Regex.Match(strReceived, @"parames=.*").Groups[0].Value.Replace("parames=","");
         

            string requestedFile;
            if (httpMethod.Equals("GET") || httpMethod.Equals("POST"))
                requestedFile = requestedUrl.Split('?')[0];
            else // You can implement other methods...
            {
                notImplemented(clientSocket);
                return;
            }

            requestedFile = requestedFile.Replace("/", @"\").Replace("\\..", "");

         
            start = requestedFile.LastIndexOf('.') + 1;
            if (start > 0)
            {
              
               // MessageBox.Show(contentPath + requestedFile);
                length = requestedFile.Length - start;
                string extension = requestedFile.Substring(start, length);
                if (extensions.ContainsKey(extension)) // Do we support this extension?
                    if (File.Exists(contentPath + requestedFile)) //If yes check existence of the file
                                                                  // Everything is OK, send requested file with correct content type:
                        sendOkResponse(clientSocket,
                          File.ReadAllBytes(contentPath + requestedFile), extensions[extension]);
                    else
                        notFound(clientSocket); // We don't support this extension.
                                                // We are assuming that it doesn't exist.
            }
            else
            {
                // If file is not specified try to send index.htm or index.html
                // You can add more (default.htm, default.html)
                if (requestedFile.Substring(length - 1, 1) != @"\")
                    requestedFile += @"\";
                if (File.Exists(contentPath + requestedFile + "index.htm"))
                    sendOkResponse(clientSocket,
                      File.ReadAllBytes(contentPath + requestedFile + "\\index.htm"), "text/html");
                else if (File.Exists(contentPath + requestedFile + "index.html"))
                    sendOkResponse(clientSocket,
                      File.ReadAllBytes(contentPath + requestedFile + "\\index.html"), "text/html");
                else
                    notFound(clientSocket);
            }
        }



        private void notImplemented(Socket clientSocket)
        {
            sendResponse(clientSocket, "<html><head><meta " +
                "http -equiv =\"Content-Type\" content=\"text/html; " +
                "charset = utf - 8\">" +
                "</ head >< body >< h2 > Atasoy Simple Web" +
                "Server </ h2 >< div > 501 - Method Not" +
                "Implemented </div ></body></html > ",
                "501 Not Implemented", "text/html");

        }

        private void notFound(Socket clientSocket)
        {

            sendResponse(clientSocket, @"<html><head><meta" +

                "http-equiv =\"Content-Type\" content=\"text/html;" +

                "charset = utf-8\"></head><body><h2>Atasoy Simple Web " +

                "Server </ h2 >< div > 404 - Not" +

                "Found </div></body></html> ",

                "404 Not Found", "text/html");
        }


        method md = new method();



        Dictionary<string, string> uservipDic = new Dictionary<string, string>();
        private void sendOkResponse(Socket clientSocket, byte[] bContent, string contentType)
        {
            #region  地图网站
            //   sendResponse(clientSocket, bContent, "200 OK", contentType); //展示本地html页面
            if (requesturl.Contains("creatHtml"))
            {

                string neirong = md.createHtml();

                sendResponse(clientSocket, neirong, "200 OK", "application/json;charset=utf-8");
            }

            else if (requesturl.Contains("api/task_add"))
            {
                string userid = Regex.Match(requesturl, @"userid=([\s\S]*?)&").Groups[1].Value;
                string taskname = Regex.Match(requesturl, @"taskname=([\s\S]*?)&").Groups[1].Value;
                string city = Regex.Match(requesturl, @"city=([\s\S]*?)&").Groups[1].Value;
                string keyword = Regex.Match(requesturl, @"keyword=([\s\S]*?)&").Groups[1].Value;
                string neirong = "";

                if (userid == "" || taskname == "" || city == "" || keyword == "")
                {
                    neirong = "{\"status\":0,\"msg\":\"参数错误\"}";
                }
                else
                {
                    string o = userid + "," + taskname + "," + city + "," + keyword;
                    neirong = md.task_add(o);
                }
                sendResponse(clientSocket, neirong, "200 OK", "application/json;charset=utf-8");
            }


            else if (requesturl.Contains("api/task_get"))
            {
                string userid = Regex.Match(requesturl, @"userid=([\s\S]*?)&").Groups[1].Value;

                string neirong = "";

                if (userid == "")
                {
                    neirong = "{\"status\":0,\"msg\":\"参数错误\"}";
                }
                else
                {
                    string o = userid;
                    neirong = md.task_get(o);
                }
                sendResponse(clientSocket, neirong, "200 OK", "application/json;charset=utf-8");
            }


            else if (requesturl.Contains("api/task_start"))
            {
                string userid = Regex.Match(requesturl, @"userid=([\s\S]*?)&").Groups[1].Value;
                string taskid = Regex.Match(requesturl, @"taskid=([\s\S]*?)&").Groups[1].Value;
                string city = Regex.Match(requesturl, @"city=([\s\S]*?)&").Groups[1].Value;
                string keyword = Regex.Match(requesturl, @"keyword=([\s\S]*?)&").Groups[1].Value;
                string neirong = "";

                if (userid == "" || taskid == "" || city == "" || keyword == "")
                {
                    neirong = "{\"status\":0,\"msg\":\"参数错误\"}";

                }

                else
                {
                    Thread thread = new Thread(new ParameterizedThreadStart(md.Amap));
                    string o = userid + "," + taskid + "," + city + "," + keyword;
                    thread.Start((object)o);
                    neirong = "{\"status\":1,\"msg\":\"启动成功\"}";

                }
                //string body = @"<html><head><meta " +

                //    "http-equiv =\"Content-Type\" content=\"text/html;" +

                //    "charset = utf-8\"></head><body>" + neirong + "</body></html> ";

                sendResponse(clientSocket, neirong, "200 OK", "application/json;charset=utf-8");
            }

            else if (requesturl.Contains("api/task_del"))
            {
                string userid = Regex.Match(requesturl, @"userid=([\s\S]*?)&").Groups[1].Value;
                string taskid = Regex.Match(requesturl, @"taskid=([\s\S]*?)&").Groups[1].Value;

                string neirong = "";

                if (userid == "" || taskid == "")
                {
                    neirong = "{\"status\":0,\"msg\":\"参数错误\"}";

                }
                else
                {
                    string o = userid + "#" + taskid;
                    neirong = md.task_del(o);
                }
                sendResponse(clientSocket, neirong, "200 OK", "application/json;charset=utf-8");
            }

            else if (requesturl.Contains("api/filedata_del"))
            {
                string userid = Regex.Match(requesturl, @"userid=([\s\S]*?)&").Groups[1].Value;
            

                string neirong = "";

                if (userid == "" )
                {
                    neirong = "{\"status\":0,\"msg\":\"参数错误\"}";

                }
                else
                {
                    string o = userid + "," +"";
                    neirong = md.filedata_del(o);
                }
                sendResponse(clientSocket, neirong, "200 OK", "application/json;charset=utf-8");
            }

            else if (requesturl.Contains("api/data_get"))
            {
                string userid = Regex.Match(requesturl, @"userid=([\s\S]*?)&").Groups[1].Value;
                string taskid = Regex.Match(requesturl, @"taskid=([\s\S]*?)&").Groups[1].Value;
                string page = Regex.Match(requesturl, @"page=([\s\S]*?)&").Groups[1].Value;
                string neirong = "";

                if (userid == "" || page == "")
                {
                    neirong = "{\"status\":0,\"msg\":\"参数错误\"}";

                }
                else
                {
                    string o = userid + "," + taskid + "," + page;
                    neirong = md.getdata(o);
                }
                sendResponse(clientSocket, neirong, "200 OK", "application/json;charset=utf-8");
            }
            else if (requesturl.Contains("api/register"))
            {
                string username = Regex.Match(requesturl, @"username=([\s\S]*?)&").Groups[1].Value;
                string password = Regex.Match(requesturl, @"password=([\s\S]*?)&").Groups[1].Value;

                string neirong = "";

                if (username == "" || password == "")
                {
                    neirong = "{\"status\":0,\"msg\":\"参数错误\"}";

                }
                else
                {
                    string o = username + "," + password;
                    neirong = md.register(o);
                }
                sendResponse(clientSocket, neirong, "200 OK", "application/json;charset=utf-8");
            }
            else if (requesturl.Contains("api/login"))
            {
                string username = Regex.Match(requesturl, @"username=([\s\S]*?)&").Groups[1].Value;
                string password = Regex.Match(requesturl, @"password=([\s\S]*?)&").Groups[1].Value;

                string neirong = "";

                if (username == "" || password == "")
                {
                    neirong = "{\"status\":0,\"msg\":\"参数错误\"}";

                }
                else
                {
                    string o = username + "," + password;
                    neirong = md.login(o);
                }
                sendResponse(clientSocket, neirong, "200 OK", "application/json;charset=utf-8");
            }


            else if (requesturl.Contains("api/vip"))
            {

                string neirong = md.vip().Replace(" ", "");

                sendResponse(clientSocket, neirong, "200 OK", "text/html;charset=utf-8");
            }
            else if (requesturl.Contains("api/exportExcel"))
            {
                string neirong = "";
                string userid = Regex.Match(requesturl, @"userid=([\s\S]*?)&").Groups[1].Value;
                string taskid = Regex.Match(requesturl, @"taskid=([\s\S]*?)&").Groups[1].Value;
                string taskname = System.Web.HttpUtility.UrlDecode(Regex.Match(requesturl, @"taskname=([\s\S]*?)&").Groups[1].Value);
                string fileName = "C:/phpStudy/PHPTutorial/WWW/excel/" + taskname + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".xlsx";
                if (taskid == "")
                {
                    neirong = "{\"status\":0,\"msg\":\"参数错误\"}";

                }
                else
                {
                    string o = userid + "," + taskid + "," + taskname + "," + fileName;
                    Thread thread = new Thread(new ParameterizedThreadStart(md.exportExcel));
                    thread.Start((object)o);
                    neirong = "{\"status\":1,\"msg\":\"正在生成表格...\",\"filename\":\"" + fileName.Replace("C:/phpStudy/PHPTutorial/WWW", "") + "\"}";
                }
                sendResponse(clientSocket, neirong, "200 OK", "application/json;charset=utf-8");
            }

            else if (requesturl.Contains("api/downloadfiles_get"))
            {
                string neirong = "";
                string userid = Regex.Match(requesturl, @"userid=([\s\S]*?)&").Groups[1].Value;
                if (userid == "")
                {
                    neirong = "{\"status\":0,\"msg\":\"参数错误\"}";

                }
                else
                {
                    string o = userid + ",11";
                    neirong = md.downloadfiles_get(o);
                }
                sendResponse(clientSocket, neirong, "200 OK", "application/json;charset=utf-8");
            }


            else if (requesturl.Contains("api/articleslist_get"))
            {
                string neirong = "";
                string page = Regex.Match(requesturl, @"page=([\s\S]*?)&").Groups[1].Value;
                // string articleid = Regex.Match(requesturl, @"articleid=([\s\S]*?)&").Groups[1].Value;
                if (page == "")
                {
                    neirong = "{\"status\":0,\"msg\":\"参数错误\"}";

                }
                else
                {
                    string o = page + "," + "articleid";
                    neirong = md.articleslist_get(o);
                }

                sendResponse(clientSocket, neirong, "200 OK", "application/json;charset=utf-8");
            }

            #endregion



            #region  美团软件

            else if (requesturl.Contains("api/mt/register"))
            {
                string neirong = "";
                string username = Regex.Match(requesturl, @"username=([\s\S]*?)&").Groups[1].Value;
                string password = Regex.Match(requesturl, @"password=([\s\S]*?)&").Groups[1].Value;
                string code = Regex.Match(requesturl, @"code=([\s\S]*?)&").Groups[1].Value;
             
                if (code == "" || username == "" || password== "" )
                {
                    neirong = "{\"status\":siyifalse,\"msg\":\"参数缺失\"}";

                }
              

                else
                {
                    string o = username + "," + password + "," + code;
                    neirong = md.mtregister(o);
                }

                sendResponse(clientSocket, neirong, "200 OK", "application/json;charset=utf-8");
            }


            else if (requesturl.Contains("api/mt/login"))
            {
                string neirong = "";
                string username = Regex.Match(requesturl, @"username=([\s\S]*?)&").Groups[1].Value;
                string password = Regex.Match(requesturl, @"password=([\s\S]*?)&").Groups[1].Value;
                string code = Regex.Match(requesturl, @"code=([\s\S]*?)&").Groups[1].Value;
                if (code == "" || username == "" || password == "")
                {
                    neirong = "{\"status\":siyifalse,\"msg\":\"参数缺失\"}";

                }
               
                else
                {
                    string o = username + "," + password + "," + code;
                    neirong = md.mtlogin(o);
                }

                sendResponse(clientSocket, neirong, "200 OK", "application/json;charset=utf-8");
            }

           
            else if (requesturl.Contains("api/mt/mt_getdata"))
            {
               
                string neirong = "";
                string userid = Regex.Match(requesturl, @"userid=([\s\S]*?)&").Groups[1].Value;
                string cityid = Regex.Match(requesturl, @"cityid=([\s\S]*?)&").Groups[1].Value;
                string cateid = Regex.Match(requesturl, @"cateid=([\s\S]*?)&").Groups[1].Value;
                string page = Regex.Match(requesturl, @"page=([\s\S]*?)&").Groups[1].Value;

                string code = Regex.Match(requesturl, @"code=([\s\S]*?)&").Groups[1].Value;
                string timestamp = Regex.Match(requesturl, @"timestamp=([\s\S]*?)&").Groups[1].Value;
                string sign = Regex.Match(requesturl, @"sign=.*").Groups[0].Value.Replace("sign=","");
                if (userid == "" || cityid == "" || cateid == "" || page == "" || code == "" || timestamp == "" || sign == "")
                {
                    neirong = "{\"status\":siyifalse,\"msg\":\"参数缺失\"}";
                    sendResponse(clientSocket, neirong, "200 OK", "application/json;charset=utf-8");
                    return;
                }
                long intime = Convert.ToInt64(timestamp) + 60;
                if (intime < Convert.ToInt64(md.GetTimeStamp()))
                {
                    neirong = "{\"status\":siyifalse,\"msg\":\"timestamp参数错误\"}";
                    sendResponse(clientSocket, neirong, "200 OK", "application/json;charset=utf-8");
                    return;
                }
                if (method.GetMD5(code + timestamp).ToUpper() != sign)
                {
                    neirong = "{\"status\":siyifalse,\"msg\":\"sign参数错误\"}";
                    sendResponse(clientSocket, neirong, "200 OK", "application/json;charset=utf-8");
                   
                }

                else
                {
                    string isvip = "";
                    if (uservipDic.ContainsKey(code))
                    {
                        isvip = uservipDic[code];
                    }
                    else
                    {
                        isvip = md.mtjiance(code);
                        uservipDic.Add(code, isvip);
                    }


                    string o = userid + "," + cityid + "," + cateid + "," + page + "," + timestamp + "," + sign;
                  
                    if (isvip == "普通会员" || isvip == "高级会员")
                    {
                        neirong = md.mt_getdata(o);
                    }
                    else
                    {
                        neirong = md.mt_getdata(o);
                        //  neirong = Regex.Replace(neirong, @"phone"":""\d{4}", "phone\":\"****");
                        neirong = Regex.Replace(neirong, @"\d{4}"",""areaName", "****\",\"areaName");
                    }
                    sendResponse(clientSocket, neirong, "200 OK", "application/json;charset=utf-8");
                }

               
            }
            #endregion



            else
            {
                string neirong = "{\"status\":0,\"msg\":\"API不存在\"}";
                sendResponse(clientSocket, neirong, "200 OK", "application/json;charset=utf-8");
            }

        }









        // For strings
        private void sendResponse(Socket clientSocket, string strContent, string responseCode,
                                  string contentType)
        {
            byte[] bContent = charEncoder.GetBytes(strContent);
            sendResponse(clientSocket, bContent, responseCode, contentType);

        }

        // For byte arrays
        private void sendResponse(Socket clientSocket, byte[] bContent, string responseCode,
                                  string contentType)
        {
            try
            {
                byte[] bHeader = charEncoder.GetBytes(
                                    "HTTP/1.1 " + responseCode + "\r\n"
                                  + "Server: Atasoy Simple Web Server\r\n"
                                  + "Content-Length: " + bContent.Length.ToString() + "\r\n"
                                  + "Connection: close\r\n"
                                   + "Access-Control-Allow-Origin: * \r\n"
                                    + "Access-Control-Allow-Credentials:true \r\n"
                                  + "Content-Type: " + contentType + "\r\n\r\n");
                clientSocket.Send(bHeader);
                clientSocket.Send(bContent);
                //MessageBox.Show(System.Text.Encoding.UTF8.GetString(bContent)); //弹出网页内容


                clientSocket.Close();
            }
            catch { }
        }






    }
}
