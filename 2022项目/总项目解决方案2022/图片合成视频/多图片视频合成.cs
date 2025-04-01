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
    public partial class 多图片视频合成 : Form
    {
        public 多图片视频合成()
        {
            InitializeComponent();
        }
        public class FileRetriever
        {
            /// <summary>
            /// 获取文件夹前N个文件路径
            /// </summary>
            /// <param name="directoryPath">目录路径</param>
            /// <param name="n">需要获取的文件数量</param>
            /// <param name="searchPattern">搜索模式（默认*.*）</param>
            /// <param name="sortBy">排序方式（默认按文件名升序）</param>
            /// <returns>前N个文件路径列表</returns>
            public static List<string> GetTopNFiles(
                string directoryPath,
                int n,
                string searchPattern = "*.*",
                FileSortOrder sortBy = FileSortOrder.NameAscending)
            {
                if (!Directory.Exists(directoryPath))
                    throw new DirectoryNotFoundException($"目录不存在: {directoryPath}");

                var files = Directory.GetFiles(directoryPath, searchPattern);

                // 根据排序要求处理文件列表
                switch (sortBy)
                {
                    case FileSortOrder.NameAscending:
                        files = files.OrderBy(f => f).ToArray();
                        break;
                    case FileSortOrder.NameDescending:
                        files = files.OrderByDescending(f => f).ToArray();
                        break;
                    case FileSortOrder.CreationTimeAscending:
                        files = files.OrderBy(f => File.GetCreationTime(f)).ToArray();
                        break;
                    case FileSortOrder.CreationTimeDescending:
                        files = files.OrderByDescending(f => File.GetCreationTime(f)).ToArray();
                        break;
                    case FileSortOrder.LastModifiedAscending:
                        files = files.OrderBy(f => File.GetLastWriteTime(f)).ToArray();
                        break;
                    case FileSortOrder.LastModifiedDescending:
                        files = files.OrderByDescending(f => File.GetLastWriteTime(f)).ToArray();
                        break;
                }

                return files.Take(n).ToList();
            }

            /// <summary>
            /// 文件排序方式枚举
            /// </summary>
            public enum FileSortOrder
            {
                NameAscending,          // 文件名升序
                NameDescending,         // 文件名降序
                CreationTimeAscending,  // 创建时间升序
                CreationTimeDescending, // 创建时间降序
                LastModifiedAscending, // 修改时间升序
                LastModifiedDescending  // 修改时间降序
            }
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
        
        Thread thread;
        private void 多图片视频合成_Load(object sender, EventArgs e)
        {
            textBox1.Text = path + "图片\\";
            textBox2.Text = path + "素材\\";

            textBox3.Text = "D:\\视频文件处理器\\视频处理器.exe";
        }

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

        string path = AppDomain.CurrentDomain.BaseDirectory;
        string deskpath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

       
            
            void CreateVideoFromImage(List<string> imagePaths, string outputPath, double totalSeconds)
            {
                if (imagePaths.Count == 0)
                {
                    Console.WriteLine("错误：未找到输入图片");
                    return;
                }

                // FFmpeg路径（需自行配置）
                string ffmpegPath = path + "\\ffmpeg.exe";

                // 视频参数配置
                int width = 1080;       // 9:16比例
                int height = 1920;
                double durationPerImage = totalSeconds / imagePaths.Count;

                // 构建FFmpeg命令参数
                var argsBuilder = new StringBuilder();
                argsBuilder.Append("-y ");  // 覆盖输出文件

                // 添加输入文件参数
                foreach (var image in imagePaths)
                {
                    argsBuilder.Append($"-loop 1 -t {durationPerImage:0.##} -i \"{image}\" ");
                }

                // 构建滤镜链
                argsBuilder.Append("-filter_complex \"");

                // 拼接所有输入流
                for (int i = 0; i < imagePaths.Count; i++)
                {
                    argsBuilder.Append($"[{i}:v]");
                }
                argsBuilder.Append($"concat=n={imagePaths.Count}:v=1:a=0[concat_out];");

                // 缩放和填充处理
                argsBuilder.Append("[concat_out]scale=" +
                                  $"w={width}:h={height}:" +
                                  "force_original_aspect_ratio=decrease," +
                                  $"pad={width}:{height}:(ow-iw)/2:(oh-ih)/2," +
                                  "setsar=1[video_out]\" ");

                // 输出参数
                argsBuilder.Append("-map \"[video_out]\" " +
                                  "-c:v libx264 " +          // H.264编码
                                  "-r 30 " +                // 输出帧率
                                  "-pix_fmt yuv420p " +     // 兼容像素格式
                                  "\"" + outputPath + "\"");

                // 配置进程参数
                var processInfo = new ProcessStartInfo
                {
                    FileName = ffmpegPath,
                    Arguments = argsBuilder.ToString(),
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                // 执行FFmpeg命令
                // 启动进程
                using (Process process = new Process { StartInfo = processInfo })
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
          



            /// <summary>
            /// 多个图片
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
                  

                    string videoPath = textBox2.Text + "B.mp4";


                    if (File.Exists(textBox2.Text + "A.mp4"))
                    {
                        File.Delete(textBox2.Text + "A.mp4");
                    }

                    textBox4.Text += DateTime.Now.ToString() + "：正在获取素材时长..." + "\r\n";


                   
                    string video1 = GetFirstFilePath(textBox2.Text);

                    //第一个素材改为"B.mp4"
                    string directory = Path.GetDirectoryName(video1);
                    string newFilePath = Path.Combine(directory, "B.mp4");
                    if (!File.Exists(newFilePath))
                    {
                        File.Move(video1, newFilePath);
                    }





                    double seconds = 视频合成.VideoDurationHelper.GetVideoDuration(videoPath);

                    var images = FileRetriever.GetTopNFiles(
             textBox1.Text,
             Convert.ToInt32(numericUpDown1.Value),
             "*.png",
             FileRetriever.FileSortOrder.NameAscending
         );
                    try
                    {

                    

                        textBox4.Text += DateTime.Now.ToString() + "：正在合成视频..." + "\r\n";
                        CreateVideoFromImage(
                        imagePaths: images,
            outputPath: textBox2.Text + "A.mp4",
            totalSeconds: seconds);


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

                        if (time > 300)
                        {
                            textBox4.Text += DateTime.Now.ToString() + "未发现成品跳过....." + "\r\n";
                            break;
                        }

                        Thread.Sleep(1000);
                        time = time + 1;
                    }
                    try
                    {


                        CloseProcessByName("视频处理器");

                        textBox4.Text += DateTime.Now.ToString() + ("：关闭第三方软件") + "\r\n";


                        Thread.Sleep(5000);


                        for (int a = 0; a < images.Count; a++)
                        {
                            File.Delete(images[a]);
                        }
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

                textBox4.Text += ex.ToString();
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



    }
}
