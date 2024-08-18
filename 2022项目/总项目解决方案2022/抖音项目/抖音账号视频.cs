using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 抖音项目
{
    public partial class 抖音账号视频 : Form
    {
        public 抖音账号视频()
        {
            InitializeComponent();
        }
        string path = AppDomain.CurrentDomain.BaseDirectory;
		string ua = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36";
		
		#region GET请求
		public  string GetUrl(string Url, string charset)
		{
			
			string result;
			try
			{
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
				//request.Proxy = null;
				//request.AllowAutoRedirect = true;
				request.UserAgent = ua;
				request.Referer = "https://www.douyin.com/";
				request.Headers.Add("Cookie", COOKIE);
				request.Headers.Add("Accept-Encoding", "gzip");
				//request.KeepAlive = true;
				//request.Accept = "*/*";
				//request.Timeout = 5000;
				HttpWebResponse response = request.GetResponse() as HttpWebResponse;
				bool flag = response.Headers["Content-Encoding"] == "gzip";
				string html;
				if (flag)
				{
					GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
					StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
					html = reader.ReadToEnd();
					reader.Close();
				}
				else
				{
					StreamReader reader2 = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset));
					html = reader2.ReadToEnd();
					reader2.Close();
				}
				response.Close();
				result = html;
			}
			catch (Exception ex)
			{
				result = ex.ToString();
			}
			return result;
		}
		#endregion

		#region POST默认请求
		public static string PostUrlDefault(string url, string postData, string COOKIE)
		{
			string result;
			try
			{
				string charset = "utf-8";
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				request.Method = "Post";
				
				//request.ContentType = "application/x-www-form-urlencoded";
				 request.ContentType = "application/json";
				request.ContentLength = (long)Encoding.UTF8.GetBytes(postData).Length;
				request.Headers.Add("Accept-Encoding", "gzip");
				request.AllowAutoRedirect = false;
				request.KeepAlive = true;
				request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
				request.Headers.Add("Cookie", COOKIE);
				request.Referer = "http://39.107.101.62:8111/docs";
				StreamWriter sw = new StreamWriter(request.GetRequestStream());
				sw.Write(postData);
				sw.Flush();
				HttpWebResponse response = request.GetResponse() as HttpWebResponse;
				response.GetResponseHeader("Set-Cookie");
				bool flag = response.Headers["Content-Encoding"] == "gzip";
				string html;
				if (flag)
				{
					GZipStream gzip = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
					StreamReader reader = new StreamReader(gzip, Encoding.GetEncoding(charset));
					html = reader.ReadToEnd();
					reader.Close();
				}
				else
				{
					StreamReader reader2 = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset));
					html = reader2.ReadToEnd();
					reader2.Close();
				}
				response.Close();
				result = html;
			}
			catch (WebException ex)
			{
				//result = ex.ToString();
				//400错误也返回内容
				using (var reader = new StreamReader(ex.Response.GetResponseStream()))
				{
					result = reader.ReadToEnd();
				}
			}
			return result;
		}
		#endregion

		public string getxbogus(string postdata)
        {
			string url = "http://39.107.101.62:8111/dy/xbogus/";
			string data = "{\"params\":\""+postdata+ "\", \"ua\":\"" + ua+"\",\"enc_type\": 1}";
			string html = PostUrlDefault(url,data,"");
			
			string xbogus= Regex.Match(html, @"""x_bogus"":""([\s\S]*?)""").Groups[1].Value.Trim();
			return xbogus;
		}
		private void 抖音账号视频_Load(object sender, EventArgs e)
        {
            
			
			StreamReader sr = new StreamReader(path+"cookie.txt", Encoding.UTF8);
            //一次性读取完 
           COOKIE= sr.ReadToEnd().Trim();
        }

        public string COOKIE = "";
       
        public void run()
        {


			ArrayList lists = function .getusers();

			try
            {
                for (int a= 0; a < lists.Count; a++)
                {
					//string uid = "MS4wLjABAAAA4cMRZJWYin6Sgu4j1D8dMqFjBLDDJYSoMMm51fY_EXm8b7CvrNofA3ISkNpmbTFd";
					string uid=lists[a].ToString();
					textBox1.Text += DateTime.Now.ToString() +"正在读取："+uid + "\r\n";
					int page = 0;

					string data = "device_platform=webapp&aid=6383&sec_user_id=" + uid + "&max_cursor=0&offset=" + page + "&count=18";
					string xbogus = getxbogus(data);
					
					string url = "https://www.douyin.com/aweme/v1/web/aweme/favorite/?"+data+"&X-Bogus="+xbogus;
					
					
					
					string html = GetUrl(url, "utf-8");

					MatchCollection aweme_ids = Regex.Matches(html, @"""aweme_id"":""([\s\S]*?)""");
					MatchCollection photo_urls = Regex.Matches(html, @"""cover""([\s\S]*?)url_list"":\[""([\s\S]*?)""");
					MatchCollection durations = Regex.Matches(html, @"""video_duration"":([\s\S]*?),");

					MatchCollection authors = Regex.Matches(html, @"""author"":""([\s\S]*?)""");
					MatchCollection comment_counts = Regex.Matches(html, @"""comment_count"":([\s\S]*?),");
					MatchCollection share_counts = Regex.Matches(html, @"""share_count"":([\s\S]*?),");
					MatchCollection collect_counts = Regex.Matches(html, @"""collect_count"":([\s\S]*?),");

					MatchCollection digg_counts = Regex.Matches(html, @"""digg_count"":([\s\S]*?),");
					MatchCollection create_times = Regex.Matches(html, @"""create_time"":([\s\S]*?),");
					MatchCollection desc = Regex.Matches(html, @"""desc"":""([\s\S]*?)""");
					MatchCollection play_addrs = Regex.Matches(html, @"""bit_rate_audio""([\s\S]*?)url_list"":\[""([\s\S]*?)""");

					if(aweme_ids.Count==0)
                    {
						textBox1.Text += DateTime.Now.ToString() + uid+"喜欢列表加密" + "\r\n";
					}

					for (int i = 0; i < aweme_ids.Count; i++)
					{
						try
						{
							string aweme_url = "https://www.douyin.com/video/" + aweme_ids[i].Groups[1].Value.Trim();
							string photo_url = function.Unicode2String(photo_urls[i].Groups[2].Value);
							string duration = durations[i].Groups[1].Value;
							string author_nickname = authors[i].Groups[1].Value;

							string comment_count = comment_counts[i].Groups[1].Value.Replace("}", "");
							string share_count = share_counts[i].Groups[1].Value.Replace("}", "");
							string collect_count = collect_counts[i].Groups[1].Value.Replace("}", "");
							string digg_count = digg_counts[i].Groups[1].Value.Replace("}", "");
							string create_time = create_times[i].Groups[1].Value;
							string descs = desc[i].Groups[1].Value;
							string created_at = function.ConvertStringToDateTime(create_times[i].Groups[1].Value).ToString("yyyy-MM-dd");
							string updated_at = created_at;
							string whoslike = uid;
							string author_sec_uid = "";
							string author_douyin_id = "";
							int dz_speed = 1;
							string create_date_time = DateTime.Now.ToString("yyyy-MM-dd hh:MM:ss");
							string unique_sign = "";
							string play_addr1 = function.Unicode2String(play_addrs[i].Groups[2].Value);

							string info = function.adddata(aweme_url, photo_url, duration, author_nickname, comment_count, share_count, collect_count, digg_count, create_time, descs, created_at, updated_at, whoslike, author_sec_uid, author_douyin_id, dz_speed, create_date_time, unique_sign, play_addr1);

							textBox1.Text += DateTime.Now.ToString() + descs + info + "\r\n";
							if(textBox1.Text.Length>1000)
                            {
								textBox1.Text = "";
                            }
						}
						catch (Exception)
						{

							continue;
						}
					}



					Thread.Sleep(2000);
				}

			}
			catch (Exception ex)
            {

				textBox1.Text += ex.ToString()+"\r\n" ;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
			this.textBox1.SelectionStart = this.textBox1.Text.Length;
			this.textBox1.SelectionLength = 0;
			this.textBox1.ScrollToCaret();
		}
		Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {
			if (DateTime.Now > Convert.ToDateTime("2024-09-21"))
			{
				return;
			}
			if (thread == null || !thread.IsAlive)
			{
				thread = new Thread(run);
				thread.Start();
				Control.CheckForIllegalCrossThreadCalls = false;
			}
		}

        private void button2_Click(object sender, EventArgs e)
        {
			thread.Abort();
        }
    }
}
