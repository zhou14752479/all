using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using myDLL;

namespace 常用软件非客户
{
    public partial class 图片批量下载 : Form
    {
        public 图片批量下载()
        {
            InitializeComponent();
        }

        #region 下载文件  【好用】
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="URLAddress">图片地址</param>
        /// <param name="subPath">图片所在文件夹</param>
        /// <param name="name">图片名称</param>
        public static void downloadFile(string URLAddress, string subPath, string name, string COOKIE)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //在GetUrl()函数前加上这一句就可以
                string path = System.IO.Directory.GetCurrentDirectory();

                WebClient client = new WebClient();
                //WebProxy proxy = new WebProxy();
                //proxy.UseDefaultCredentials = false;
                //proxy.Address = new Uri("http://tps115.kdlapi.com:15818"); 
                //client.Proxy = proxy;

                client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");
                client.Headers.Add("Cookie", COOKIE);
                client.Headers.Add("Referer", "");
                if (false == System.IO.Directory.Exists(subPath))
                {
                    //创建pic文件夹
                    System.IO.Directory.CreateDirectory(subPath);
                }

                client.DownloadFile(URLAddress, subPath + "\\" + name);
            }
            catch (WebException ex)
            {

               ex.ToString();
            }
        }



        #endregion
        string path = AppDomain.CurrentDomain.BaseDirectory;


        ArrayList finishes = new ArrayList();
        public void download()
        {
            string cookie = "RES_TRACKINGID=553328881033867; _gcl_au=1.1.951625230.1624617847; dtm_token=AQEIxp9X_iBRVgE5PlHBAQHyNAE; s_ecid=MCMID%7C71665817602425959411962606342967286693; s_fid=0B287B7D70BFE6E9-25055AD3952EA38A; TLTSID=7772d917d3f015e1950100e0ed6a79aa; TLTUID=7772d917d3f015e1950100e0ed6a79aa; bm_sz=F2D946A6585938F88ED47D035333B736~YAAQpFw6FwLfl0V6AQAAy0/MTwy77yftJXoAeNCKDJjP+vYpbNlvu0kg7ud8FieKCuG3F3IiJiKEB9QIPipnL2FkOkARYgl/ovwn0O43kiYbNbIuV1Knn2CGUIAau9X72CjIZDzfXVY+xn6RMEWfqm9UGk3bSOG7P7uPbthSe/pFk+zC8QE2ftUHct6rmjgPVoQ=; _scid=06ecf98f-7596-4e5e-add0-ea2c76b29ccd; AMCVS_0E3334FA53DA8C980A490D44%40AdobeOrg=1; s_cc=true; __idcontext=eyJjb29raWVJRCI6IkUyVFFIRlFQQldHTjZCQzYyQVhQWEFLRjRDR0lEWENVWEpVNjRCRFlGRFhRPT09PSIsImRldmljZUlEIjoiRTJUUUhGUVBBRzYyRUgyRVVZNjYzSkRSM1QyMkxRM1JUUklOUUpaQkpHMkE9PT09IiwiaXYiOiJOMzdMTkhERU1IR1VURk1JRUJaRExXVlhZQT09PT09PSIsInYiOjF9; _sctr=1|1624809600000; bm_mi=4A3D3FF5D0E670692E60808F75378D2F~/217TNexsv+zJ9kYxgnvqBGz+4aLA+xlEdYwumod1H6uhjPPizBfhXx+HpFQOqIueYU90F50PiEMnAChTtF/UPKGns/Qtpx0eB8tAXBOyiaGvlO7YAWdZdC22/i0l8fUS1+Xh8kb0E5JbVuAua29+Yl4N/iWtSNOUPx/mqRkI6hxDXerPBK1ZL1OpUypDSg/RFoDrrSpDlZd6OSN/fuc9xYRN+yqBiGLHRxMLHsADJv9VEc19xBSKGKtezaQRoCvJeHSSDnSY8nTSPbPATHgDlrle2uqW7AmExNjpHh6Xn3Mveb441Iit1dwLX6zDdQYEyz+XUsexOOII864NYHNBHIihN47S5ZH3ktP4eXD4FuTHuc7xO8+MS2UmoxqVDi2znyZhcRZst69ezhtY7xxtg==; ak_bmsc=26E39B022B64B6128F4C8E04345AE08C~000000000000000000000000000000~YAAQrzJ8aEqH3Ex6AQAAbSQ/UAyhEdZJmrONQ3pzZU9yRO2lXq8IC0OeDZSco5az1xDFSvhHt4r7KuPj6KcSbxNPFKdJo5BKnMduRhKHFw8CGS6VTlV2rysvmsOMZckgIUfek8ECHgbw1y4wNsEh3e2hsmQ+7pDNTi/5ihOn1eRea5cgzezIEJ9fEuy4b0YjYypYvrcsxA4EVfpHLejzswTrmww49e7CaqoUhpQQOLFRol4a2KZ0hjSfgPk20YDswKVFR0bR8xGzrCqteHgCisteLRRH8alzEJRVEdbW3xYX37bcsj+zWDjxPWycmLJyYW0fhObQDoL7nMKtUVfOAqivhMU06/qi8pjFmsF4Tf9kjPOqEzrOoZNRC0+AkiMfkewOgJMUIlMTU6B/li6JS2dLDYYozRR/VfLlwbGhzqJwGp2iYd4Sz+uItotjSbvFqrujJG3sLqMSKXSK4STLUuHzMSLg+tv8u13CIopxrKhe5uYMMiG7vw+3DQkUW9AEVQXU6zoLJydNDx3kJimKL6TJsrmrtCRZfrkXQQ==; _abck=F5AADFA7CE18F018EB3A77FE5B7853F2~0~YAAQh1w6Fz6fBUJ6AQAA4VyXUAbL4nqd9LPwG0fiSgjTl4I72HRmM9bf6nt+3hWJYZyLVBnK60BYxeVqdyTMksRBLShqL5B9WBcNxJEatC0TzHDKGFp3IVeyNQ10obDcPCArkgnwM5d3J7T+aSQYheu1PTzu8kKxC/gvW8FlKrTWLGVD8Snn72ybu5blmtnddqJTd21LPz91w0/mPApjqDOEeIDvFlviN2RCBYDylze+62q6HhbKNpa0Wi7BULuMq8GKkbUhTHR0o0Uh+Cw3581bYPQ3YA1OQSUot6tXLvUGvTQ2ep48Si8dfjlFscs+StGxHsc4ibYqA1Ox0+mS1vm+2+QCLwao4fTzk346Lp9zFN4mPET5AOIS/SOnnLyILijocU3OF5FSZYz2rTTCElpOOWZbY20zpqE=~-1~-1~-1; _uetsid=319e8e60d79f11ebb418eb5ff48d8548; _uetvid=49a5b400d5a211ebaf32035e455a6bb8; AMCV_0E3334FA53DA8C980A490D44%40AdobeOrg=1406116232%7CMCIDTS%7C18806%7CMCMID%7C71665817602425959411962606342967286693%7CMCAAMLH-1625454535%7C9%7CMCAAMB-1625454535%7CRKhpRz8krg2tLO6pguXWp5olkAcUniQYPHaMWWgdJ3xzPWQmdj0y%7CMCOPTOUT-1624856935s%7CNONE%7CMCAID%7CNONE%7CvVersion%7C2.5.0; s_pn=az%3Ahome; sc_nrv=1624849735289-Repeat; sc_lv=1624849735292; sc_lv_s=Less%20than%201%20day; s_vnum=1625068800401%26vn%3D5; s_invisit=true; s_p24=https%3A%2F%2Fwww.autozone.com%2F; bounceClientVisit3804v=N4IgNgDiBcIBYBcEQM4FIDMBBNAmAYnvgO6kB0AhgK4ID2AXrQHYCmZAxrQLZEgA0IAE4wQ-EChYBzGAG0AugF8gA; AKA_A2=A; s_sq=autozmobilefirstdefdev%3D%2526c.%2526a.%2526activitymap.%2526page%253Dhttps%25253A%25252F%25252Fwww.autozone.com%25252Fsearchresult%25253FsearchText%25253DFA1634%2526link%253DAir%252520Filter%2526region%253Dsearch-result-list%2526.activitymap%2526.a%2526.c; RT=\"z=1&dm=autozone.com&si=67f390a8-c24b-4701-b56c-b79a82b3c97a&ss=kqftnm2d & sl=1o&tt=18ly&obo=19 & rl=1\"; utag_main=v_id:017a42c5062e00997bc05b5e396003072001906a00bd0$_sn:4$_se:40$_ss:0$_st:1624851954260$vapi_domain:autozone.com$ses_id:1624849733015%3Bexp-session$_pn:39%3Bexp-session; bm_sv=4261B95BE5FCDCFE580F4A5263E1DE86~/JgyIdYuK/bR5923ToOX6W+AydkZDXZv3mnu2qJCxfzHuIbssDRxt7SiwAtJqQluCZG1GzggNLhqGxZAY4Qo6kv+LMp2oPOeYVTfR3DNmDTP4OAP3DyZQx8f0w5eiIEdaryl1LyVIJfsBkB9M4YBwx96kCB/A0AjLTrXK/16uOs=";
           
                for (int i = 0; i < richTextBox1.Lines.Length; i++)
                {
                try
                {
                   // string filename = Regex.Match(richTextBox1.Lines[i], @"ksm\/([\s\S]*?)\/").Groups[1].Value+".jpg";
                    // string filename = Path.GetFileName(richTextBox1.Lines[i]).Replace(" ", "");
                    string filename = richTextBox2.Lines[i] + ".jpg";
                    downloadFile(richTextBox1.Lines[i], path + "image//", filename, "");
                    textBox2.Text = "正在下载第：" + (i + 1);

                }
                catch (Exception ex)
                {
                    textBox2.Text += richTextBox1.Lines[i] + "\r\n";
                    continue;
                }
                   

                }
          

            //for (int i = 0; i < richTextBox1.Lines.Length; i++)
            //{
            //    string[] text = richTextBox1.Lines[i].Split(new string[] { " " }, StringSplitOptions.None);

            //    string newTxt = "";
            //    for (int j = 0; j < text.Length-1; j++)
            //    {
            //        newTxt += text[j] + " ";
            //    }

            //    richTextBox2.Text += newTxt + "\r\n";

            //    button1.Text = richTextBox2.Lines.Length.ToString();
            //}
        }


        private void button1_Click(object sender, EventArgs e)
        {

            Thread thread = new Thread(download);
            thread.Start();
            Control.CheckForIllegalCrossThreadCalls = false;


        }

        private void button2_Click(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader(path+"1.txt", method.EncodingType.GetTxtType(path + "1.txt"));
            //一次性读取完 
            string texts = sr.ReadToEnd();
            MatchCollection names = Regex.Matches(texts, @"\/category\/([\s\S]*?)\.");
            for (int i = 0; i < names.Count; i++)
            {
                textBox2.Text += names[i].Groups[1].Value+"\r\n";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog f = new FolderBrowserDialog();
            if (f.ShowDialog() == DialogResult.OK)
            {
                DirectoryInfo folder = new DirectoryInfo(f.SelectedPath);
                for (int i = 0; i < folder.GetFiles().Count(); i++)
                {
                    richTextBox1.Text += folder.GetFiles()[i].Name+"\r\n";
                }
            }

            
           
        }

        private void 图片批量下载_Load(object sender, EventArgs e)
        {

        }
    }
}
