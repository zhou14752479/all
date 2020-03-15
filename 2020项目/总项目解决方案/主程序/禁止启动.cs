using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 主程序
{
    public partial class 禁止启动 : Form
    {
        public 禁止启动()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 复制文件夹中的所有内容
        /// </summary>
        /// <param name="sourceDirPath">源文件夹目录</param>
        /// <param name="saveDirPath">指定文件夹目录</param>
        public static void CopyDirectory(string sourceDirPath, string saveDirPath)
        {
            try
            {
                //如果指定的存储路径不存在，则创建该存储路径
                if (!Directory.Exists(saveDirPath))
                {
                    //创建
                    Directory.CreateDirectory(saveDirPath);
                }

                //获取源路径文件的名称
                string[] files = Directory.GetFiles(sourceDirPath);

                //遍历子文件夹的所有文件。
                foreach (string file in files)
                {
                    string pFilePath = saveDirPath + "\\" + Path.GetFileName(file);
                    if (System.IO.File.Exists(pFilePath))
                        continue;
                    System.IO.File.Copy(file, pFilePath, true);
                }

                string[] dirs = Directory.GetDirectories(sourceDirPath);

                //递归，遍历文件夹
                foreach (string dir in dirs)
                {
                    CopyDirectory(dir, saveDirPath + "\\" + Path.GetFileName(dir));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }  


        private void 禁止启动_Load(object sender, EventArgs e)
        {

        }

        [ComImport]
        [Guid("00021401-0000-0000-C000-000000000046")]
        internal class ShellLink
        {
        }
        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("000214F9-0000-0000-C000-000000000046")]
        internal interface IShellLink
        {
            void GetPath([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxPath, out IntPtr pfd, int fFlags);
            void GetIDList(out IntPtr ppidl);
            void SetIDList(IntPtr pidl);
            void GetDescription([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, int cchMaxName);
            void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
            void GetWorkingDirectory([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);
            void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
            void GetArguments([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);
            void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
            void GetHotkey(out short pwHotkey);
            void SetHotkey(short wHotkey);
            void GetShowCmd(out int piShowCmd);
            void SetShowCmd(int iShowCmd);
            void GetIconLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath, int cchIconPath, out int piIcon);
            void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
            void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);
            void Resolve(IntPtr hwnd, int fFlags);
            void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);


        }

        private void creatShortcut(string path,string name)
        {
            IShellLink link = (IShellLink)new ShellLink();
            link.SetDescription("快捷方式描述");
            link.SetPath(path); //指定文件路径

            IPersistFile file = (IPersistFile)link;
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string sfile = Path.Combine(desktopPath, name+".lnk");
            if (System.IO.File.Exists(sfile))
                System.IO.File.Delete(sfile);
            file.Save(sfile, false);  //快捷方式保存到桌面
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                string pLocalFilePath = path + "\\幸运飞艇\\";//要复制的文件路径
                string pSaveFilePath = @"C:\幸运飞艇";//指定存储的路径
                CopyDirectory(pLocalFilePath, pSaveFilePath);


                creatShortcut(@"C:\幸运飞艇\幸运飞艇.exe","幸运飞艇");

                
                MessageBox.Show("成功拷贝幸运飞艇");
            }
            if (radioButton2.Checked == true)
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                string pLocalFilePath = path + "\\澳洲幸运10\\";//要复制的文件路径
                string pSaveFilePath = @"C:\澳洲幸运10";//指定存储的路径
                CopyDirectory(pLocalFilePath, pSaveFilePath);

                creatShortcut(@"C:\澳洲幸运10\澳洲幸运10.exe","澳洲幸运10");
                MessageBox.Show("成功拷贝澳洲幸运10");
            }

        }
    }
}
