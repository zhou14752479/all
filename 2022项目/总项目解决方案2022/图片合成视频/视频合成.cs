using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 图片合成视频
{
    public partial class 视频合成 : Form
    {
        public 视频合成()
        {
            InitializeComponent();
        }


        string GetFirstFilePath(string folderPath)
        {
            // 使用EnumerateFiles方法遍历文件夹下的所有文件，包括子目录
            foreach (string file in Directory.EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories))
            {
                // 找到第一个文件后直接返回其路径
                return file;
            }
            // 如果没有找到任何文件，返回null
            return null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择所在文件夹";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    MessageBox.Show(this, "文件夹路径不能为空", "提示");
                    return;
                }

                textBox1.Text = dialog.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择所在文件夹";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    MessageBox.Show(this, "文件夹路径不能为空", "提示");
                    return;
                }

                textBox2.Text = dialog.SelectedPath;
            }
        }
        string path = AppDomain.CurrentDomain.BaseDirectory;
        string deskpath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        private void 视频合成_Load(object sender, EventArgs e)
        {
            textBox1.Text = path + "图片\\";
            textBox2.Text = path + "素材\\";
           
            textBox3.Text =  "D:\\视频文件处理器\\视频处理器.exe";

        }


        public class VideoGenerator
        {
            public void CreateVideoFromImage(
                string ffmpegPath,
                string inputImagePath,
                string outputVideoPath,
                TimeSpan duration)
            {
                // 构建FFmpeg参数
                //string arguments = $"-y -loop 1 -i \"{inputImagePath}\" " +
                //                   $"-c:v libx264 -t {duration.TotalSeconds} " +
                //                   $"-pix_fmt yuv420p -vf \"fps=30,scale=1920:1080\" " +
                //                   $"\"{outputVideoPath}\"";


                string arguments = $"-loop 1 -i \"{inputImagePath}\" " +
                           "-vf \"scale=1080:1920:force_original_aspect_ratio=decrease," +
                           "pad=1080:1920:(ow-iw)/2:(oh-ih)/2\" " +
                           $"-t {duration.TotalSeconds} -r 30 -c:v libx264 -pix_fmt yuv420p " +
                           $"\"{outputVideoPath}\"";
               
                // 配置进程启动参数
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = ffmpegPath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardError = true
                };

                // 启动进程
                using (Process process = new Process { StartInfo = startInfo })
                {
                    process.Start();
                    string errorOutput = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        throw new InvalidOperationException($"FFmpeg error: {errorOutput}");
                    }
                }
            }
        }


      public  class VideoDurationHelper
        {
            public static double GetVideoDuration(string videoPath)
            {
               
                // 构建FFmpeg命令
                string arguments = $"-i \"{videoPath}\" -hide_banner";

                // 启动FFmpeg进程
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "ffmpeg",
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardError = true, // FFmpeg的输出在StandardError中
                    RedirectStandardOutput = true
                };

                using (Process process = new Process { StartInfo = startInfo })
                {
                    process.Start();
                    string output = process.StandardError.ReadToEnd(); // 读取FFmpeg的输出
                    process.WaitForExit();

                    // 解析输出，提取时长
                    string durationLine = output.Split('\n')
                                                .FirstOrDefault(line => line.Contains("Duration"));
                    if (durationLine != null)
                    {
                        // 提取时长部分（格式：HH:MM:SS.mm）
                        string durationString = durationLine.Split(',')[0].Replace("Duration:", "").Trim();
                        
                        TimeSpan duration = TimeSpan.Parse(durationString);
                        return duration.TotalSeconds;
                    }
                    else
                    {
                        
                       throw new Exception("无法解析视频时长");
                    }
                }
            }
        }

      
        Thread thread;
        private void button3_Click(object sender, EventArgs e)
        {
            
            
            if (DateTime.Now > Convert.ToDateTime("2025-05-11"))
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

        /// <summary>
        /// 单个图片
        /// </summary>
        public void run()
        {

            try
            {
                DirectoryInfo directory2 = new DirectoryInfo(textBox2.Text);
                // 获取文件夹内的文件数组
                FileInfo[] files = directory2.GetFiles();
                // 获取文件数量
                int fileCount = files.Length;

                for (int i = 0; i < fileCount; i++)
                {
                    var creator = new VideoGenerator();

                    string videoPath = textBox2.Text + "B.mp4";


                    if (File.Exists(textBox2.Text + "A.mp4"))
                    {
                        File.Delete(textBox2.Text + "A.mp4");
                    }

                    textBox4.Text += DateTime.Now.ToString() + "：正在获取素材时长..." + "\r\n";


                    string pic1 = GetFirstFilePath(textBox1.Text);
                    string video1 = GetFirstFilePath(textBox2.Text);

                    //第一个素材改为"B.mp4"
                    string directory = Path.GetDirectoryName(video1);
                    string newFilePath = Path.Combine(directory, "B.mp4");
                    if (!File.Exists(newFilePath))
                    {
                        File.Move(video1, newFilePath);
                    }





                    double seconds = VideoDurationHelper.GetVideoDuration(videoPath);


                    try
                    {
                        textBox4.Text += DateTime.Now.ToString() + "：正在合成视频..." + "\r\n";
                        creator.CreateVideoFromImage(
                            ffmpegPath: path + "\\ffmpeg.exe",


                        inputImagePath: pic1,
                        outputVideoPath: textBox2.Text + "A.mp4",
                        duration: TimeSpan.FromSeconds(seconds));




                        textBox4.Text += DateTime.Now.ToString() + "视频生成成功！" + "\r\n";
                    }
                    catch (Exception ex)
                    {
                        textBox4.Text += DateTime.Now.ToString() + ($"错误: {ex.ToString()}");
                    }

                    //剪切视频到第三方软件

                    string Bvideo = textBox2.Text + "B.mp4";
                    string Avideo = textBox2.Text + "A.mp4";
                    if (File.Exists("D:\\视频文件处理器\\B.mp4"))
                    {
                        File.Delete("D:\\视频文件处理器\\B.mp4");
                    }

                    if (File.Exists("D:\\视频文件处理器\\A.mp4"))
                    {
                        File.Delete("D:\\视频文件处理器\\A.mp4");
                    }
                    File.Move(Bvideo, "D:\\视频文件处理器\\B.mp4");
                    File.Move(Avideo, "D:\\视频文件处理器\\A.mp4");

                    //视频生成完成开始启动第三方软件

                    string programPath = textBox3.Text;

                    textBox4.Text += DateTime.Now.ToString() + "：正在启动第三方软件..." + "\r\n";

                    //Process process = StartProgram(programPath);

                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = @"D:\视频文件处理器\视频处理器.exe", // 程序 B 的实际路径
                        WorkingDirectory = @"D:\视频文件处理器\",       // 显式设置程序 B 的工作目录
                        UseShellExecute = false                       // 避免 Shell 启动干扰路径
                    };


                    Process.Start(startInfo);


                    int time = 1;
                    while (true)
                    {

                        if (File.Exists(@"D:\视频文件处理器\flyjhe.mp4"))
                        {
                           
                            break;
                        }
                       

                        //300秒未出现成品跳过
                       
                        if(time>300)
                        {
                            textBox4.Text += DateTime.Now.ToString() +"未发现成品跳过....."+ "\r\n";
                            break;
                        }

                        Thread.Sleep(1000);
                        time = time + 1 ;
                    }
                    try
                    {


                        CloseProcessByName("视频处理器");

                        textBox4.Text += DateTime.Now.ToString() + ("：关闭第三方软件") + "\r\n";


                        Thread.Sleep(5000);

                        File.Delete(pic1); //删除图片
                        textBox4.Text += DateTime.Now.ToString() + ("：删除处理好的图片") + "\r\n";

                        File.Move(@"D:\视频文件处理器\flyjhe.mp4", @"D:\视频文件处理器\成品\" + DateTime.Now.ToString("HHmmss") + ".mp4");
                    }
                    catch (Exception ex)
                    {
                        textBox4.Text += DateTime.Now.ToString() + "删除图片失败跳过....." + "\r\n";
                        textBox4.Text += DateTime.Now.ToString() + ex.ToString() + "\r\n";
                        continue; 
                    }
                }
            }
            catch (Exception ex)
            {

                textBox4.Text +=ex.ToString();
            }

        }


        // 启动指定路径的程序
        Process StartProgram(string path)
        {
            try
            {
               
                Process process = new Process();
                process.StartInfo.FileName = path;
                process.Start();
                return process;
            }
            catch (Exception ex)
            {
                textBox4.Text += DateTime.Now.ToString() + ("启动程序时出错: " + ex.Message) + "\r\n";
                return null;
            }
        }

        // 关闭指定的程序
        public static void CloseProcessByName(string processName)
        {
            try
            {
                // 获取指定名称的所有进程
                Process[] processes = Process.GetProcessesByName(processName);

                if (processes.Length > 0)
                {
                    foreach (Process process in processes)
                    {
                        try
                        {
                            // 尝试正常关闭进程
                            if (process.CloseMainWindow())
                            {
                                // 等待进程退出
                                process.WaitForExit();
                                Console.WriteLine($"进程 {processName}（ID: {process.Id}）已正常关闭。");
                            }
                            else
                            {
                                // 正常关闭失败，强制终止进程
                                process.Kill();
                                process.WaitForExit();
                                Console.WriteLine($"进程 {processName}（ID: {process.Id}）已被强制终止。");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"关闭进程 {processName}（ID: {process.Id}）时发生错误: {ex.Message}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"未找到名为 {processName} 的进程。");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生错误: {ex.Message}");
            }
        
    }

    private void textBox4_TextChanged(object sender, EventArgs e)
        {
            this.textBox4.SelectionStart = this.textBox4.Text.Length;
            this.textBox4.SelectionLength = 0;
            this.textBox4.ScrollToCaret();
        }

        private void button4_Click(object sender, EventArgs e)
        {


            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开文件";
            openFileDialog1.InitialDirectory = @"C:\Users\Administrator\Desktop";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框选择的文件
                textBox3.Text = openFileDialog1.FileName;



            }
        }

      
    }
}
