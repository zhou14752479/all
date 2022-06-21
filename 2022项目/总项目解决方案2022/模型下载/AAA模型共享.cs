using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DownloadSystemClient;

namespace 模型下载
{
    public partial class AAA模型共享 : Form
    {
        public AAA模型共享()
        {
            InitializeComponent();
        }

		public class AliOss
		{
			// Token: 0x17000053 RID: 83
			// (get) Token: 0x060001B2 RID: 434 RVA: 0x00002DA4 File Offset: 0x00000FA4
			// (set) Token: 0x060001B3 RID: 435 RVA: 0x00002DAC File Offset: 0x00000FAC
			public string Ossid { get; set; }

			// Token: 0x17000054 RID: 84
			// (get) Token: 0x060001B4 RID: 436 RVA: 0x00002DB5 File Offset: 0x00000FB5
			// (set) Token: 0x060001B5 RID: 437 RVA: 0x00002DBD File Offset: 0x00000FBD
			public string BucketName { get; set; }

			// Token: 0x17000055 RID: 85
			// (get) Token: 0x060001B6 RID: 438 RVA: 0x00002DC6 File Offset: 0x00000FC6
			// (set) Token: 0x060001B7 RID: 439 RVA: 0x00002DCE File Offset: 0x00000FCE
			public string Endpoint { get; set; }

			// Token: 0x17000056 RID: 86
			// (get) Token: 0x060001B8 RID: 440 RVA: 0x00002DD7 File Offset: 0x00000FD7
			// (set) Token: 0x060001B9 RID: 441 RVA: 0x00002DDF File Offset: 0x00000FDF
			public string BucketDomain { get; set; }

			// Token: 0x17000057 RID: 87
			// (get) Token: 0x060001BA RID: 442 RVA: 0x00002DE8 File Offset: 0x00000FE8
			// (set) Token: 0x060001BB RID: 443 RVA: 0x00002DF0 File Offset: 0x00000FF0
			public string AccessKeyId { get; set; }

			// Token: 0x17000058 RID: 88
			// (get) Token: 0x060001BC RID: 444 RVA: 0x00002DF9 File Offset: 0x00000FF9
			// (set) Token: 0x060001BD RID: 445 RVA: 0x00002E01 File Offset: 0x00001001
			public string AccessKeySecret { get; set; }
		}


		private void AAA模型共享_Load(object sender, EventArgs e)
        {

        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            DownloadSystemClient.MainWindow a = new DownloadSystemClient.MainWindow();

			//DownloadSystemClient.MainWindow.GetObjectProgress("", "", "", "", "", "", true);

		}
    }
}
