using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序2025
{
    public partial class 删除文件 : Form
    {
        public 删除文件()
        {
            InitializeComponent();
        }

        private void 删除文件_Load(object sender, EventArgs e)
        {

            timer1.Start();
            timer1.Interval = 10000;

        }


        /// <summary>
        /// 删除文件夹中指定时间之前的文件
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        /// <param name="expireMinutes">过期分钟数</param>
        /// <param name="useCreationTime">是否使用创建时间判断</param>
        /// <returns>成功删除的文件数量</returns>
        public int DeleteExpiredFiles(string folderPath, int expireMinutes, bool useCreationTime = true)
        {
            int deletedCount = 0;

            // 验证文件夹是否存在
            if (!Directory.Exists(folderPath))
            {
               textBox3.Text+=($"文件夹不存在: {folderPath}")+"\r\n";
            }

            // 计算过期时间点
            DateTime expireTime = DateTime.Now.AddMinutes(-expireMinutes);

            try
            {
                // 获取文件夹中所有文件（不包含子目录）
                string[] files = Directory.GetFiles(folderPath);

                foreach (string file in files)
                {
                    try
                    {
                        if(file.Contains("ecloud"))
                        {
                            continue;
                        }



                        // 获取文件的时间戳（创建时间或最后修改时间）
                        DateTime fileTime = useCreationTime
                            ? File.GetCreationTime(file)
                            : File.GetLastWriteTime(file);

                        // 检查文件是否过期
                        if (fileTime < expireTime)
                        {
                            // 删除文件
                            File.Delete(file);
                            deletedCount++;
                            textBox3.Text += ($"已删除过期文件: {file}") + "\r\n";
                        }
                    }
                    catch (IOException ex)
                    {
                        textBox3.Text += ($"删除文件时发生I/O错误: {file}，错误信息: {ex.Message}") + "\r\n";
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        textBox3.Text += ($"没有权限删除文件: {file}，错误信息: {ex.Message}") + "\r\n";
                    }
                    catch (Exception ex)
                    {
                        textBox3.Text += ($"处理文件时发生错误: {file}，错误信息: {ex.Message}") + "\r\n";
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                textBox3.Text += ($"没有权限访问文件夹: {folderPath}，错误信息: {ex.Message}") + "\r\n";
            }
            catch (Exception ex)
            {
                textBox3.Text += ($"处理文件夹时发生错误: {folderPath}，错误信息: {ex.Message}") + "\r\n";
            }

            return deletedCount;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DeleteExpiredFiles(textBox1.Text,Convert.ToInt32(textBox2.Text));
            if (textBox3.Text.Length > 10000)
                textBox3.Text = "";
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

                textBox1.Text = dialog.SelectedPath;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void 删除文件_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定要关闭吗？", "关闭", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                // Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            else
            {
                e.Cancel = true;//点取消的代码 
            }
        }
    }
}
