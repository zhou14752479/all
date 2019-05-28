using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main._2019_5
{
    public partial class 医药网 : Form
    {
        public 医药网()
        {
            InitializeComponent();
        }

        private void 医药网_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        string[] keys ={


            "%e7%9b%90%e9%85%b8%e5%a4%9a%e5%b7%b4%e9%85%9a%e4%b8%81%e8%83%ba%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e5%8e%bb%e4%b9%99%e9%85%b0%e6%af%9b%e8%8a%b1%e8%8b%b7%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e7%9b%90%e9%85%b8%e6%99%ae%e7%bd%97%e5%b8%95%e9%85%ae%e7%89%87",
"%e5%8d%95%e7%a1%9d%e9%85%b8%e5%bc%82%e5%b1%b1%e6%a2%a8%e9%85%af%e8%91%a1%e8%90%84%e7%b3%96%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e7%a1%9d%e9%85%b8%e5%bc%82%e5%b1%b1%e6%a2%a8%e9%85%af%e8%91%a1%e8%90%84%e7%b3%96%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e7%9b%90%e9%85%b8%e7%be%8e%e8%a5%bf%e5%be%8b%e7%89%87",
"%e5%ae%81%e5%bf%83%e5%ae%9d%e8%83%b6%e5%9b%8a",
"%e7%9b%90%e9%85%b8%e5%88%a9%e5%a4%9a%e5%8d%a1%e5%9b%a0%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e7%9b%90%e9%85%b8%e6%99%ae%e7%bd%97%e5%b8%95%e9%85%ae%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e7%9b%90%e9%85%b8%e8%83%ba%e7%a2%98%e9%85%ae%e7%89%87",
"%e7%9b%90%e9%85%b8%e7%bb%b4%e6%8b%89%e5%b8%95%e7%b1%b3%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e8%be%9b%e4%bc%90%e4%bb%96%e6%b1%80%e7%89%87",
"%e6%9b%b2%e5%85%8b%e8%8a%a6%e4%b8%81%e6%b0%af%e5%8c%96%e9%92%a0%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e6%b3%a8%e5%b0%84%e7%94%a8%e9%98%bf%e9%ad%8f%e9%85%b8%e9%92%a0",
"%e6%b4%bb%e8%a1%80%e9%80%9a%e8%84%89%e7%89%87",
"%e5%a4%8d%e6%96%b9%e4%b8%89%e7%bb%b4%e4%ba%9a%e6%b2%b9%e9%85%b8%e8%83%b6%e4%b8%b8I",
"%e7%83%9f%e9%85%b8%e8%82%8c%e9%86%87%e9%85%af%e7%89%87",
"%e5%86%a0%e5%bf%83%e8%8b%8f%e5%90%88%e8%83%b6%e5%9b%8a",
"%e6%b3%a8%e5%b0%84%e7%94%a8%e5%8d%95%e7%a1%9d%e9%85%b8%e5%bc%82%e5%b1%b1%e6%a2%a8%e9%85%af",
"%e5%a4%8d%e6%96%b9%e4%b8%b9%e5%8f%82%e6%bb%b4%e4%b8%b8",
"%e9%80%9f%e6%95%88%e6%95%91%e5%bf%83%e4%b8%b8",
"%e5%bf%83%e5%ae%9d%e4%b8%b8",
"%e9%ba%9d%e9%a6%99%e4%bf%9d%e5%bf%83%e4%b8%b8",
"%e7%a1%ab%e9%85%b8%e9%95%81%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e6%9b%bf%e7%b1%b3%e6%b2%99%e5%9d%a6%e7%89%87",
"%e7%8f%8d%e8%8f%8a%e9%99%8d%e5%8e%8b%e7%89%87",
"%e6%b8%85%e8%84%91%e9%99%8d%e5%8e%8b%e7%89%87",
"%e5%90%b2%e8%be%be%e5%b8%95%e8%83%ba%e7%bc%93%e9%87%8a%e7%89%87",
"%e9%a9%ac%e6%9d%a5%e9%85%b8%e4%be%9d%e9%82%a3%e6%99%ae%e5%88%a9%e7%89%87",
"%e5%a4%a7%e6%b4%bb%e7%bb%9c%e8%83%b6%e5%9b%8a",
"%e6%b6%88%e6%a0%93%e5%86%8d%e9%80%a0%e4%b8%b8",
"%e8%88%92%e5%bf%83%e5%ae%81%e7%89%87",
"%e8%84%89%e7%ae%a1%e5%a4%8d%e5%ba%b7%e7%89%87",
"%e7%a1%9d%e9%85%b8%e5%bc%82%e5%b1%b1%e6%a2%a8%e9%85%af%e7%89%87",
"%e9%86%92%e8%84%91%e5%86%8d%e9%80%a0%e8%83%b6%e5%9b%8a",
"%e8%a1%80%e5%a1%9e%e9%80%9a%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e5%b0%bc%e9%ba%a6%e8%a7%92%e6%9e%97%e8%83%b6%e5%9b%8a",
"%e8%84%91%e5%ae%89%e8%83%b6%e5%9b%8a",
"%e5%a4%a9%e4%b8%b9%e9%80%9a%e7%bb%9c%e8%83%b6%e5%9b%8a",
"%e5%a4%8d%e6%96%b9%e5%b0%8f%e6%b4%bb%e7%bb%9c%e4%b8%b8",
"%e7%a8%b3%e5%bf%83%e9%a2%97%e7%b2%92",
"%e5%bf%83%e8%be%be%e5%ba%b7%e7%89%87",
"%e9%9d%9e%e8%af%ba%e8%b4%9d%e7%89%b9%e8%83%b6%e5%9b%8a",
"%e9%98%bf%e6%89%98%e4%bc%90%e4%bb%96%e6%b1%80%e9%92%99%e7%89%87",
"%e9%87%91%e6%b3%bd%e5%86%a0%e5%bf%83%e8%83%b6%e5%9b%8a",
"%e8%be%9b%e4%bc%90%e4%bb%96%e6%b1%80%e8%83%b6%e5%9b%8a",
"%e6%b4%9b%e4%bc%90%e4%bb%96%e6%b1%80%e8%83%b6%e5%9b%8a",
"%e6%b6%88%e6%a0%93%e9%80%9a%e7%bb%9c%e7%89%87",
"%e8%a1%80%e5%a1%9e%e9%80%9a%e7%89%87",
"%e5%8d%95%e7%a1%9d%e9%85%b8%e5%bc%82%e5%b1%b1%e6%a2%a8%e9%85%af%e7%bc%93%e9%87%8a%e7%89%87",
"%e6%b3%a8%e5%b0%84%e7%94%a8%e8%91%9b%e6%a0%b9%e7%b4%a0",
"%e5%a4%8d%e6%96%b9%e5%b7%9d%e8%8a%8e%e8%83%b6%e5%9b%8a",
"%e5%86%a0%e5%bf%83%e7%94%9f%e8%84%89%e4%b8%b8",
"%e5%bf%83%e5%ae%81%e7%89%87",
"%e6%b3%a8%e5%b0%84%e7%94%a8%e7%8e%af%e7%a3%b7%e8%85%ba%e8%8b%b7%e8%91%a1%e8%83%ba",
"%e5%a4%8d%e6%96%b9%e5%8f%b3%e6%97%8b%e7%b3%96%e9%85%9040%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e8%82%9d%e7%b4%a0%e9%92%a0%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e9%a6%99%e4%b8%b9%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e9%85%92%e7%9f%b3%e9%85%b8%e7%be%8e%e6%89%98%e6%b4%9b%e5%b0%94%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e6%b3%a8%e5%b0%84%e7%94%a8%e5%b0%bf%e6%bf%80%e9%85%b6",
"%e5%8d%8e%e6%b3%95%e6%9e%97%e9%92%a0%e7%89%87",
"%e9%98%bf%e5%8f%b8%e5%8c%b9%e6%9e%97%e5%8f%8c%e5%98%a7%e8%be%be%e8%8e%ab%e7%89%87",
"%e7%9b%90%e9%85%b8%e5%9c%b0%e5%b0%94%e7%a1%ab%e5%8d%93%e7%89%87",
"%e6%b3%a8%e5%b0%84%e7%94%a8%e5%a5%a5%e6%89%8e%e6%a0%bc%e9%9b%b7%e9%92%a0",
"%e8%9a%93%e6%bf%80%e9%85%b6%e8%82%a0%e6%ba%b6%e8%83%b6%e5%9b%8a",
"%e6%9b%b2%e5%85%8b%e8%8a%a6%e4%b8%81%e7%89%87",
"%e9%86%8b%e9%85%b8%e7%94%b2%e7%be%9f%e5%ad%95%e9%85%ae%e7%89%87",
"%e6%b3%a8%e5%b0%84%e7%94%a8%e5%b8%95%e7%b1%b3%e8%86%a6%e9%85%b8%e4%ba%8c%e9%92%a0",
"%e6%9e%b8%e6%a9%bc%e9%85%b8%e4%bb%96%e8%8e%ab%e6%98%94%e8%8a%ac%e7%89%87",
"%e6%9d%a5%e6%9b%b2%e5%94%91%e7%89%87",
"%e8%89%be%e8%bf%aa%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e6%b3%a8%e5%b0%84%e7%94%a8%e4%ba%9a%e5%8f%b6%e9%85%b8%e9%92%99",
"%e5%8f%82%e9%ba%a6%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e5%8f%82%e8%8a%aa%e5%8d%81%e4%b8%80%e5%91%b3%e9%a2%97%e7%b2%92",
"%e6%b3%a8%e5%b0%84%e7%94%a8%e5%94%91%e6%9d%a5%e8%86%a6%e9%85%b8",
"%e8%a5%bf%e9%bb%84%e4%b8%b8",
"%e6%8a%97%e7%99%8c%e5%b9%b3%e4%b8%b8",
"%e5%8e%bb%e6%b0%a7%e6%b0%9f%e5%b0%bf%e8%8b%b7%e8%83%b6%e5%9b%8a",
"%e6%b3%a8%e5%b0%84%e7%94%a8%e9%a1%ba%e9%93%82",
"%e7%94%b2%e6%b0%a8%e8%9d%b6%e5%91%a4%e7%89%87",
"%e5%af%b9%e4%b9%99%e9%85%b0%e6%b0%a8%e5%9f%ba%e9%85%9a%e7%89%87",
"%e6%b4%bb%e8%a1%80%e6%ad%a2%e7%97%9b%e8%83%b6%e5%9b%8a",
"%e5%8f%8c%e6%b0%af%e8%8a%ac%e9%85%b8%e9%92%be%e7%89%87",
"%e5%ae%89%e4%b9%83%e8%bf%91%e7%89%87",
"%e8%99%8e%e5%8a%9b%e6%95%a3%e8%83%b6%e5%9b%8a",
"%e5%85%b3%e8%8a%82%e6%ad%a2%e7%97%9b%e8%86%8f",
"%e9%aa%a8%e8%82%bd%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e4%bc%a4%e6%b9%bf%e7%a5%9b%e7%97%9b%e8%86%8f",
"%e5%ae%89%e7%bb%9c%e7%97%9b%e7%89%87",
"%e5%a4%a9%e5%92%8c%e8%bf%bd%e9%a3%8e%e8%86%8f",
"%e8%90%98%e4%b8%81%e7%be%8e%e9%85%ae%e8%83%b6%e5%9b%8a",
"%e8%82%be%e9%aa%a8%e8%83%b6%e5%9b%8a",
"%e8%97%bf%e9%a6%99%e6%ad%a3%e6%b0%94%e7%89%87",
"%e5%b9%bf%e4%b8%9c%e5%87%89%e8%8c%b6%e9%a2%97%e7%b2%92",
"%e4%bb%81%e4%b8%b9",
"%e8%8b%8f%e5%90%88%e9%a6%99%e4%b8%b8",
"%e9%be%99%e8%99%8e%e4%ba%ba%e4%b8%b9",
"%e5%8d%81%e6%bb%b4%e6%b0%b4%e8%83%b6%e4%b8%b8",
"%e5%a4%8d%e6%96%b9%e6%b0%a8%e9%85%9a%e7%a9%bf%e5%bf%83%e8%8e%b2%e7%89%87",
"%e5%a6%82%e6%84%8f%e7%8f%8d%e5%ae%9d%e4%b8%b8",
"%e5%b0%8f%e5%84%bf%e6%b0%a8%e5%92%96%e9%bb%84%e6%95%8f%e9%a2%97%e7%b2%92",
"%e5%b0%8f%e5%84%bf%e9%87%91%e4%b8%b9%e7%89%87",
"%e8%b5%96%e6%b0%a8%e5%8c%b9%e6%9e%97%e6%95%a3",
"%e9%98%bf%e5%8f%b8%e5%8c%b9%e6%9e%97%e7%bc%93%e9%87%8a%e7%89%87",
"%e4%ba%ba%e5%b7%a5%e7%89%9b%e9%bb%84%e7%94%b2%e7%a1%9d%e5%94%91%e8%83%b6%e5%9b%8a",
"%e9%bb%84%e8%bf%9e%e4%b8%8a%e6%b8%85%e4%b8%b8",
"%e5%a4%8d%e6%96%b9%e6%b0%af%e5%b7%b2%e5%ae%9a%e5%90%ab%e6%bc%b1%e6%b6%b2",
"%e7%94%b2%e7%a1%9d%e5%94%91%e8%8a%ac%e5%b8%83%e8%8a%ac%e8%83%b6%e5%9b%8a",
"%e6%b8%85%e8%83%83%e9%bb%84%e8%bf%9e%e4%b8%b8",
"%e4%ba%94%e5%91%b3%e9%ba%9d%e9%a6%99%e4%b8%b8",
"%e8%97%bf%e9%a6%99%e6%ad%a3%e6%b0%94%e8%bd%af%e8%83%b6%e5%9b%8a",
"%e6%b8%85%e5%87%89%e6%b2%b9",
"%e7%99%bd%e8%8a%b1%e6%b2%b9",
"%e7%9b%90%e9%85%b8%e5%90%97%e5%95%89%e8%83%8d%e7%89%87",
"%e9%bb%84%e7%9f%b3%e6%84%9f%e5%86%92%e7%89%87",
"%e6%8a%97%e7%97%85%e6%af%92%e9%a2%97%e7%b2%92",
"%e7%be%8e%e6%b4%9b%e6%98%94%e5%ba%b7%e7%89%87",
"%e5%8d%a1%e9%a9%ac%e8%a5%bf%e5%b9%b3%e7%89%87",
"%e5%9d%8e%e7%a6%bb%e7%a0%82",
"%e7%9b%90%e9%85%b8%e4%b9%99%e5%93%8c%e7%ab%8b%e6%9d%be%e7%89%87",
"%e4%b8%87%e9%80%9a%e7%ad%8b%e9%aa%a8%e7%89%87",
"%e5%a3%ae%e9%aa%a8%e5%85%b3%e8%8a%82%e4%b8%b8",
"%e9%9d%9e%e6%99%ae%e6%8b%89%e5%ae%97%e7%89%87",
"%e5%af%b9%e4%b9%99%e9%85%b0%e6%b0%a8%e5%9f%ba%e9%85%9a%e7%bc%93%e9%87%8a%e7%89%87",
"%e9%85%ae%e6%b4%9b%e8%8a%ac%e7%bc%93%e9%87%8a%e8%83%b6%e5%9b%8a",
"%e7%94%b2%e8%8a%ac%e9%82%a3%e9%85%b8%e7%89%87",
"%e7%9b%90%e9%85%b8%e5%b8%83%e6%af%94%e5%8d%a1%e5%9b%a0%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e5%90%b2%e5%93%9a%e7%be%8e%e8%be%9b%e7%bc%93%e9%87%8a%e8%83%b6%e5%9b%8a",
"%e5%a6%87%e7%a7%91%e5%8d%81%e5%91%b3%e7%89%87",
"%e5%85%ad%e5%91%b3%e5%ae%89%e6%b6%88%e8%83%b6%e5%9b%8a",
"%e4%b9%9d%e6%b0%94%e6%8b%88%e7%97%9b%e4%b8%b8",
"%e7%8b%ac%e4%b8%80%e5%91%b3%e8%bd%af%e8%83%b6%e5%9b%8a",
"%e5%b0%91%e8%85%b9%e9%80%90%e7%98%80%e9%a2%97%e7%b2%92",
"%e5%bd%93%e5%bd%92%e7%89%87",
"%e6%b0%af%e5%8c%96%e7%90%a5%e7%8f%80%e8%83%86%e7%a2%b1%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e5%bf%ab%e8%83%83%e7%89%87",
"%e8%8b%af%e7%94%b2%e9%86%87%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e5%9c%9f%e9%9c%89%e7%b4%a0%e7%89%87",
"%e5%b7%a6%e9%87%91%e4%b8%b8",
"%e6%a0%b9%e7%97%9b%e5%b9%b3%e8%83%b6%e5%9b%8a",
"%e9%a6%99%e8%8d%af%e8%83%83%e5%ae%89%e7%89%87",
"%e9%80%8d%e9%81%a5%e4%b8%b8",
"%e9%be%99%e8%83%86%e6%b3%bb%e8%82%9d%e4%b8%b8",
"%e5%8f%8c%e5%a7%9c%e8%83%83%e7%97%9b%e4%b8%b8",
"%e8%83%83%e5%ba%b7%e7%81%b5%e8%83%b6%e5%9b%8a",
"%e8%88%92%e8%82%9d%e5%92%8c%e8%83%83%e4%b8%b8",
"%e6%b3%a8%e5%b0%84%e7%94%a8%e7%94%b2%e7%a3%ba%e9%85%b8%e5%8a%a0%e8%b4%9d%e9%85%af",
"%e7%9b%90%e9%85%b8%e7%b1%b3%e8%af%ba%e7%8e%af%e7%b4%a0%e8%83%b6%e5%9b%8a",
"%e9%86%8b%e9%85%b8%e5%a5%a5%e6%9b%b2%e8%82%bd%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e6%9b%b2%e5%8c%b9%e5%b8%83%e9%80%9a%e7%89%87",
"%e5%a4%8d%e6%96%b9%e8%83%86%e9%80%9a%e7%89%87",
"%e8%83%86%e5%ae%81%e7%89%87",
"%e8%83%86%e7%9f%b3%e5%88%a9%e9%80%9a%e7%89%87",
"%e9%9d%9e%e5%b8%83%e4%b8%99%e9%86%87%e8%83%b6%e4%b8%b8",
"%e8%8c%b4%e4%b8%89%e7%a1%ab%e7%89%87",
"%e5%91%8b%e5%a1%9e%e7%b1%b3%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e7%94%98%e9%9c%b2%e9%86%87%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e5%b8%83%e7%be%8e%e4%bb%96%e5%b0%bc%e7%89%87",
"%e7%a1%ab%e5%94%91%e5%98%8c%e5%91%a4%e7%89%87",
"%e8%9e%ba%e5%86%85%e9%85%af%e7%89%87",
"%e6%89%98%e6%8b%89%e5%a1%9e%e7%b1%b3%e7%89%87",
"%e9%86%8b%e9%85%b8%e7%94%b2%e8%90%98%e6%b0%a2%e9%86%8c%e7%89%87",
"%e8%bf%9e%e8%92%b2%e5%8f%8c%e6%b8%85%e7%89%87",
"%e8%97%8f%e8%8c%b5%e9%99%88%e7%89%87",
"%e7%be%9f%e7%94%b2%e7%83%9f%e8%83%ba%e7%89%87",
"%e8%83%86%e9%85%b8%e9%92%a0%e7%89%87",
"%e4%ba%ae%e8%8f%8c%e7%94%b2%e7%b4%a0%e7%89%87",
"%e9%b8%a1%e9%aa%a8%e8%8d%89%e8%83%b6%e5%9b%8a",
"%e5%a4%8d%e6%96%b9%e7%94%98%e8%8d%89%e9%85%b8%e5%8d%95%e9%93%b5%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e8%be%85%e9%85%b6Q10%e8%83%b6%e5%9b%8a",
"%e6%9b%bf%e5%8a%a0%e6%b0%9f%e7%89%87",
"%e8%81%94%e8%8b%af%e5%8f%8c%e9%85%af%e6%bb%b4%e4%b8%b8",
"%e6%b3%a8%e5%b0%84%e7%94%a8%e7%9b%90%e9%85%b8%e5%b9%b3%e9%98%b3%e9%9c%89%e7%b4%a0",
"%e6%b6%88%e7%99%8c%e5%b9%b3%e7%b3%96%e6%b5%86",
"%e6%b3%a8%e5%b0%84%e7%94%a8%e7%8e%af%e7%a3%b7%e8%85%ba%e8%8b%b7",
"%e6%b3%a8%e5%b0%84%e7%94%a8%e8%bf%98%e5%8e%9f%e5%9e%8b%e8%b0%b7%e8%83%b1%e7%94%98%e8%82%bd",
"%e6%8a%a4%e8%82%9d%e7%89%87",
"%e8%8e%aa%e6%9c%af%e6%b2%b9%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e5%86%b3%e6%98%8e%e9%99%8d%e8%84%82%e7%89%87",
"%e6%8a%a4%e8%82%9d%e5%ae%81%e7%89%87",
"%e9%be%99%e6%b3%bd%e7%86%8a%e8%83%86%e8%83%b6%e5%9b%8a",
"%e8%a5%bf%e5%92%aa%e6%9b%bf%e4%b8%81%e7%89%87",
"%e8%88%92%e8%82%9d%e6%ad%a2%e7%97%9b%e4%b8%b8",
"%e7%9b%90%e9%85%b8%e7%b2%be%e6%b0%a8%e9%85%b8%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e7%9b%8a%e8%82%9d%e7%81%b5%e7%89%87",
"%e9%a6%96%e4%b9%8c%e5%bb%b6%e5%af%bf%e7%89%87",
"%e9%98%bf%e8%8e%ab%e8%a5%bf%e6%9e%97%e7%89%87",
"%e6%b3%a8%e5%b0%84%e7%94%a8%e5%a4%b4%e5%ad%a2%e5%94%91%e6%9e%97%e9%92%a0",
"%e5%a4%b4%e5%ad%a2%e7%be%9f%e6%b0%a8%e8%8b%84%e8%83%b6%e5%9b%8a",
"%e5%b0%8f%e5%84%bf%e5%92%b3%e5%96%98%e7%81%b5%e5%8f%a3%e6%9c%8d%e6%b6%b2",
"%e6%b0%a7%e6%b0%9f%e6%b2%99%e6%98%9f%e7%89%87",
"%e6%b3%a8%e5%b0%84%e7%94%a8%e9%98%bf%e5%a5%87%e9%9c%89%e7%b4%a0",
"%e6%b0%a8%e8%8c%b6%e7%a2%b1%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e6%a0%b8%e9%85%aa%e5%8f%a3%e6%9c%8d%e6%ba%b6%e6%b6%b2",
"%e6%9b%b2%e5%ae%89%e5%a5%88%e5%be%b7%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e7%a1%ab%e9%85%b8%e6%b2%99%e4%b8%81%e8%83%ba%e9%86%87%e6%b0%94%e9%9b%be%e5%89%82",
"%e7%bb%86%e8%be%9b%e8%84%91%e7%89%87",
"%e5%af%8c%e9%a9%ac%e9%85%b8%e6%b0%af%e9%a9%ac%e6%96%af%e6%b1%80%e7%89%87",
"%e5%ae%9d%e5%92%b3%e5%ae%81%e9%a2%97%e7%b2%92",
"%e5%a4%8d%e6%96%b9%e6%b0%a8%e9%85%9a%e7%83%b7%e8%83%ba%e7%89%87",
"%e6%84%9f%e5%86%92%e6%b8%85%e7%83%ad%e9%a2%97%e7%b2%92",
"%e5%8c%96%e7%97%b0%e5%b9%b3%e5%96%98%e7%89%87",
"%e9%80%9a%e5%ae%a3%e7%90%86%e8%82%ba%e4%b8%b8",
"%e6%b3%a8%e5%b0%84%e7%94%a8%e7%9b%90%e9%85%b8%e6%b0%a8%e6%ba%b4%e7%b4%a2",
"%e6%b6%88%e7%99%8c%e5%b9%b3%e7%89%87",
"%e6%b3%a8%e5%b0%84%e7%94%a8%e8%bd%ac%e7%a7%bb%e5%9b%a0%e5%ad%90",
"%e5%a4%9a%e8%a5%bf%e4%bb%96%e8%b5%9b%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e6%b3%a8%e5%b0%84%e7%94%a8%e7%8e%af%e7%a3%b7%e9%85%b0%e8%83%ba",
"%e6%b3%a8%e5%b0%84%e7%94%a8%e7%9b%90%e9%85%b8%e5%90%89%e8%a5%bf%e4%bb%96%e6%bb%a8",
"%e6%b0%9f%e5%b0%bf%e5%98%a7%e5%95%b6%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e6%9d%bf%e8%93%9d%e6%a0%b9%e9%a2%97%e7%b2%92",
"%e5%a4%8d%e6%96%b9%e6%b0%a8%e9%85%9a%e7%83%b7%e8%83%ba%e8%83%b6%e5%9b%8a",
"%e6%b8%85%e7%81%ab%e7%89%87",
"%e5%af%bc%e8%b5%a4%e4%b8%b8",
"%e6%84%9f%e5%86%92%e5%ba%b7%e8%83%b6%e5%9b%8a",
"%e5%a4%8d%e6%96%b9%e5%8d%97%e6%9d%bf%e8%93%9d%e6%a0%b9%e9%a2%97%e7%b2%92",
"%e7%94%b2%e6%b3%bc%e5%b0%bc%e9%be%99%e7%89%87",
"%e5%ba%b7%e5%a4%8d%e6%96%b0%e6%b6%b2",
"%e5%88%a9%e7%a6%8f%e5%96%b7%e4%b8%81%e8%83%b6%e5%9b%8a",
"%e7%9b%90%e9%85%b8%e4%b9%99%e8%83%ba%e4%b8%81%e9%86%87%e7%89%87",
"%e8%a1%a5%e9%87%91%e7%89%87",
"%e5%9b%9e%e7%94%9f%e7%94%98%e9%9c%b2%e4%b8%b8",
"%e7%a1%ac%e8%84%82%e9%85%b8%e7%ba%a2%e9%9c%89%e7%b4%a0%e7%89%87",
"%e5%9e%82%e4%bd%93%e5%90%8e%e5%8f%b6%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e6%b3%a8%e5%b0%84%e7%94%a8%e5%a4%b4%e5%ad%a2%e4%bb%96%e5%95%b6",
"%e7%9b%90%e9%85%b8%e6%b4%9b%e8%b4%9d%e6%9e%97%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e5%85%8b%e6%8b%89%e9%9c%89%e7%b4%a0%e8%83%b6%e5%9b%8a",
"%e7%9b%90%e9%85%b8%e5%a4%b4%e5%ad%a2%e4%bb%96%e7%be%8e%e9%85%af%e7%89%87",
"%e6%9f%b4%e8%bf%9e%e5%8f%a3%e6%9c%8d%e6%b6%b2",
"%e5%b0%bc%e5%8f%af%e5%88%b9%e7%b1%b3%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e5%b0%8f%e5%84%bf%e6%b6%88%e7%a7%af%e6%ad%a2%e5%92%b3%e5%8f%a3%e6%9c%8d%e6%b6%b2",
"%e9%b1%bc%e8%85%a5%e8%8d%89%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e5%a4%8f%e6%a1%91%e8%8f%8a%e9%a2%97%e7%b2%92",
"%e7%be%9a%e7%be%8a%e6%84%9f%e5%86%92%e8%83%b6%e5%9b%8a",
"%e5%b0%8f%e5%84%bf%e8%82%ba%e7%83%ad%e5%b9%b3%e8%83%b6%e5%9b%8a",
"%e9%9c%8d%e9%a6%99%e6%ad%a3%e6%b0%94%e7%89%87",
"%e5%b7%a6%e6%97%8b%e5%a4%9a%e5%b7%b4%e7%89%87",
"%e5%8d%95%e5%94%be%e6%b6%b2%e9%85%b8%e5%9b%9b%e5%b7%b1%e7%b3%96%e7%a5%9e%e7%bb%8f%e8%8a%82%e8%8b%b7%e8%84%82%e9%92%a0%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e8%84%91%e5%ba%b7%e6%b3%b0%e8%83%b6%e5%9b%8a",
"%e5%a4%9a%e5%b7%b4%e4%b8%9d%e8%82%bc%e8%83%b6%e5%9b%8a",
"%e7%9b%90%e9%85%b8%e8%8b%af%e6%b5%b7%e7%b4%a2%e7%89%87",
"%e7%94%b2%e7%a1%ab%e9%85%b8%e6%96%b0%e6%96%af%e7%9a%84%e6%98%8e%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e6%b0%a2%e6%ba%b4%e9%85%b8%e5%8a%a0%e5%85%b0%e4%bb%96%e6%95%8f%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e5%91%8b%e5%96%83%e7%a1%ab%e8%83%ba%e7%89%87",
"%e5%a4%8d%e6%96%b9%e5%90%a1%e6%8b%89%e8%a5%bf%e5%9d%a6%e8%84%91%e8%9b%8b%e7%99%bd%e6%b0%b4%e8%a7%a3%e7%89%a9%e7%89%87",
"%e4%b8%99%e6%88%8a%e9%85%b8%e9%92%a0%e7%89%87",
"%e5%a4%8d%e6%96%b9%e8%84%91%e8%9b%8b%e7%99%bd%e6%b0%b4%e8%a7%a3%e7%89%a9%e7%89%87",
"%e6%b0%9f%e5%93%8c%e5%95%b6%e9%86%87%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e7%9b%90%e9%85%b8%e5%a4%9a%e5%a5%88%e5%93%8c%e9%bd%90%e7%89%87",
"%e5%90%a1%e6%8b%89%e8%a5%bf%e5%9d%a6%e8%83%b6%e5%9b%8a",
"%e4%b8%b9%e5%8f%82%e8%88%92%e5%bf%83%e8%83%b6%e5%9b%8a",
"%e5%bf%83%e8%84%91%e5%81%a5%e8%83%b6%e5%9b%8a",
"%e5%a4%8d%e6%96%b9%e5%a4%a9%e9%ba%bb%e9%a2%97%e7%b2%92",
"%e7%9b%90%e9%85%b8%e5%a4%9a%e5%a1%9e%e5%b9%b3%e7%89%87",
"%e9%95%87%e8%84%91%e5%ae%81%e8%83%b6%e5%9b%8a",
"%e4%ba%94%e5%91%b3%e5%ad%90%e7%b3%96%e6%b5%86",
"%e5%b0%bc%e8%8e%ab%e5%9c%b0%e5%b9%b3%e8%83%b6%e5%9b%8a",
"%e4%ba%8c%e5%8d%81%e4%ba%94%e5%91%b3%e7%8f%8a%e7%91%9a%e4%b8%b8",
"%e8%82%8c%e9%86%87%e7%83%9f%e9%85%b8%e9%85%af%e7%89%87",
"%e5%a4%a9%e9%ba%bb%e7%b4%a0%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e7%96%8f%e8%a1%80%e9%80%9a%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e8%b1%86%e8%85%90%e6%9e%9c%e8%8b%b7%e7%89%87",
"%e7%9b%90%e9%85%b8%e6%b0%af%e7%b1%b3%e5%b8%95%e6%98%8e%e7%89%87",
"%e8%84%91%e8%9b%8b%e7%99%bd%e6%b0%b4%e8%a7%a3%e7%89%a9%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e7%99%b8%e6%b0%9f%e5%a5%8b%e4%b9%83%e9%9d%99%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e5%a5%8b%e4%b9%83%e9%9d%99%e7%89%87",
"%e7%9b%90%e9%85%b8%e6%b0%af%e4%b8%99%e5%97%aa%e7%89%87",
"%e5%88%a9%e5%9f%b9%e9%85%ae%e7%89%87",
"%e4%ba%94%e6%b0%9f%e5%88%a9%e5%a4%9a%e7%89%87",
"%e8%8c%b6%e8%8b%af%e6%b5%b7%e6%98%8e%e7%89%87",
"%e5%a4%8d%e6%96%b9%e6%b0%a2%e6%ba%b4%e9%85%b8%e4%b8%9c%e8%8e%a8%e8%8f%aa%e7%a2%b1%e8%b4%b4%e8%86%8f",
"%e9%a3%9e%e9%b9%b0%e6%b4%bb%e7%bb%9c%e6%b2%b9",
"%e6%b0%a2%e6%ba%b4%e9%85%b8%e4%b8%9c%e8%8e%a8%e8%8f%aa%e7%a2%b1%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e7%9b%90%e9%85%b8%e5%80%8d%e4%bb%96%e5%8f%b8%e6%b1%80%e7%89%87",
"%e5%b0%bc%e9%ba%a6%e8%a7%92%e6%9e%97%e7%89%87",
"%e8%84%91%e5%be%97%e7%94%9f%e7%89%87",
"%e5%bf%83%e8%84%91%e5%ba%b7%e8%83%b6%e5%9b%8a",
"%e9%95%bf%e6%98%a5%e8%83%ba%e7%bc%93%e9%87%8a%e8%83%b6%e5%9b%8a",
"%e5%88%ba%e4%ba%94%e5%8a%a0%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e5%85%bb%e8%a1%80%e6%b8%85%e8%84%91%e9%a2%97%e7%b2%92",
"%e9%bb%84%e8%b1%86%e8%8b%b7%e5%85%83%e7%89%87",
"%e8%82%89%e8%94%bb%e4%ba%94%e5%91%b3%e4%b8%b8",
"%e8%88%92%e5%bf%83%e9%99%8d%e8%84%82%e7%89%87",
"%e5%a4%a9%e9%ba%bb%e9%92%a9%e8%97%a4%e9%a2%97%e7%b2%92",
"%e6%b3%a8%e5%b0%84%e7%94%a8%e8%bf%98%e5%8e%9f%e5%9e%8b%e8%b0%b7%e8%83%b1%e7%94%98%e8%82%bd%e9%92%a0",
"%e7%9b%90%e9%85%b8%e6%96%87%e6%8b%89%e6%b3%95%e8%be%9b%e8%83%b6%e5%9b%8a",
"%e7%9b%90%e9%85%b8%e8%88%8d%e6%9b%b2%e6%9e%97%e7%89%87",
"%e7%9b%90%e9%85%b8%e4%b8%99%e7%b1%b3%e5%97%aa%e7%89%87",
"%e7%9b%90%e9%85%b8%e5%b8%95%e7%bd%97%e8%a5%bf%e6%b1%80%e7%89%87",
"%e6%b0%a2%e6%ba%b4%e9%85%b8%e8%a5%bf%e9%85%9e%e6%99%ae%e5%85%b0%e7%89%87",
"%e7%b1%b3%e6%b0%ae%e5%b9%b3%e7%89%87",
"%e7%9b%90%e9%85%b8%e5%80%8d%e4%bb%96%e5%8f%b8%e6%b1%80%e5%8f%a3%e6%9c%8d%e6%b6%b2",
"%e7%9b%90%e9%85%b8%e6%9b%b2%e7%be%8e%e4%bb%96%e5%97%aa%e7%89%87",
"%e6%b3%a8%e5%b0%84%e7%94%a8%e7%9b%90%e9%85%b8%e5%80%8d%e4%bb%96%e5%8f%b8%e6%b1%80",
"%e5%bc%ba%e5%8a%9b%e5%ae%9a%e7%9c%a9%e7%89%87",
"%e6%b4%bb%e8%a1%80%e9%80%9a%e8%84%89%e8%83%b6%e5%9b%8a",
"%e8%84%91%e7%ab%8b%e6%b8%85%e4%b8%b8",
"%e4%b8%99%e6%88%8a%e9%85%b8%e9%95%81%e7%89%87",
"%e7%9b%90%e9%85%b8%e6%b0%9f%e6%a1%82%e5%88%a9%e5%97%aa%e7%89%87",
"%e8%96%84%e8%8a%9d%e7%b3%96%e8%82%bd%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e4%ba%8c%e5%8d%81%e4%ba%94%e5%91%b3%e7%8f%8d%e7%8f%a0%e4%b8%b8",
"%e8%b0%b7%e6%b0%a8%e9%85%b8%e7%89%87",
"%e7%9b%90%e9%85%b8%e6%b0%9f%e6%a1%82%e5%88%a9%e5%97%aa%e8%83%b6%e5%9b%8a",
"%e6%b3%a8%e5%b0%84%e7%94%a8%e4%b8%83%e5%8f%b6%e7%9a%82%e8%8b%b7%e9%92%a0",
"%e7%94%b2%e9%92%b4%e8%83%ba%e7%89%87",
"%e7%a3%b7%e9%85%b8%e5%b7%9d%e8%8a%8e%e5%97%aa%e7%89%87",
"%e7%9b%90%e9%85%b8%e7%bd%82%e7%b2%9f%e7%a2%b1%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e7%83%9f%e9%85%b8%e5%8d%a0%e6%9b%bf%e8%af%ba%e7%89%87",
"%e7%99%83%e9%97%ad%e8%88%92%e7%89%87",
"%e5%89%8d%e5%88%97%e9%80%9a%e7%98%80%e8%83%b6%e5%9b%8a",
"%e7%9b%90%e9%85%b8%e7%89%b9%e6%8b%89%e5%94%91%e5%97%aa%e7%89%87",
"%e7%9b%90%e9%85%b8%e5%9d%a6%e7%b4%a2%e7%bd%97%e8%be%9b%e7%bc%93%e9%87%8a%e8%83%b6%e5%9b%8a",
"%e9%87%91%e7%a0%82%e4%ba%94%e6%b7%8b%e4%b8%b8",
"%e8%82%be%e5%ba%b7%e5%ae%81%e7%89%87",
"%e8%82%be%e8%a1%b0%e5%ae%81%e8%83%b6%e5%9b%8a",
"%e5%a4%a9%e9%ba%bb%e8%9c%9c%e7%8e%af%e8%8f%8c%e7%89%87",
"%e5%a6%87%e7%a7%91%e7%99%bd%e5%b8%a6%e7%89%87",
"%e7%94%b7%e5%ae%9d%e8%83%b6%e5%9b%8a",
"%e4%bb%99%e7%81%b5%e9%aa%a8%e8%91%86%e8%83%b6%e5%9b%8a",
"%e8%85%b0%e8%82%be%e8%86%8f",
"%e7%a2%b3%e9%85%b8%e6%b0%a2%e9%92%a0%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e9%87%91%e5%8c%ae%e8%82%be%e6%b0%94%e7%89%87",
"%e5%b0%bf%e6%af%92%e6%b8%85%e9%a2%97%e7%b2%92",
"%e9%98%bf%e9%ad%8f%e9%85%b8%e5%93%8c%e5%97%aa%e7%89%87",
"%e8%97%bb%e9%85%b8%e5%8f%8c%e9%85%af%e9%92%a0%e7%89%87",
"%e5%a4%8d%e6%96%b9%e8%8a%a6%e4%b8%81%e7%89%87",
"%e8%82%be%e7%82%8e%e5%9b%9b%e5%91%b3%e7%89%87",
"%e8%97%bb%e9%85%b8%e5%8f%8c%e9%85%af%e9%92%a0%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e8%82%be%e7%82%8e%e7%81%b5%e8%83%b6%e5%9b%8a",
"%e9%bb%84%e8%91%b5%e8%83%b6%e5%9b%8a",
"%e4%ba%ba%e8%a1%80%e7%99%bd%e8%9b%8b%e7%99%bd",
"%e6%89%98%e6%8b%89%e5%a1%9e%e7%b1%b3%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e7%bb%93%e7%9f%b3%e9%80%9a%e7%89%87",
"%e6%b6%88%e7%9f%b3%e7%89%87",
"%e7%9f%b3%e6%b7%8b%e9%80%9a%e9%a2%97%e7%b2%92",
"%e6%8e%92%e7%9f%b3%e9%a2%97%e7%b2%92",
"%e5%87%80%e7%9f%b3%e7%81%b5%e8%83%b6%e5%9b%8a",
"%e9%87%91%e9%92%b1%e8%8d%89%e9%a2%97%e7%b2%92",
"%e5%a3%ae%e9%98%b3%e6%98%a5%e8%83%b6%e5%9b%8a",
"%e5%89%8d%e5%88%97%e9%80%9a%e7%89%87",
"%e7%83%ad%e6%b7%8b%e6%b8%85%e8%83%b6%e5%9b%8a",
"%e6%ba%b4%e5%90%a1%e6%96%af%e7%9a%84%e6%98%8e%e7%89%87",
"%e7%bc%a9%e6%b3%89%e4%b8%b8",
"%e7%9b%90%e9%85%b8%e7%94%b2%e6%b0%af%e8%8a%ac%e9%85%af%e8%83%b6%e5%9b%8a",
"%e7%a1%ab%e9%85%b8%e9%98%bf%e7%b1%b3%e5%8d%a1%e6%98%9f%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e6%b3%a8%e5%b0%84%e7%94%a8%e7%a1%ab%e9%85%b8%e5%a5%88%e6%9b%bf%e7%b1%b3%e6%98%9f",
"%e7%a1%ab%e9%85%b8%e5%8d%a1%e9%82%a3%e9%9c%89%e7%b4%a0%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e6%b3%a8%e5%b0%84%e7%94%a8%e5%88%a9%e5%b7%b4%e9%9f%a6%e6%9e%97",
"%e5%8a%a0%e6%9b%bf%e6%b2%99%e6%98%9f%e7%89%87",
"%e7%9b%90%e9%85%b8%e5%b7%a6%e6%b0%a7%e6%b0%9f%e6%b2%99%e6%98%9f%e8%83%b6%e5%9b%8a",
"%e6%b3%a8%e5%b0%84%e7%94%a8%e7%9b%90%e9%85%b8%e5%a4%b4%e5%ad%a2%e5%90%a1%e8%82%9f",
"%e6%8a%97%e5%ae%ab%e7%82%8e%e8%83%b6%e5%9b%8a",
"%e6%b3%a8%e5%b0%84%e7%94%a8%e7%9b%90%e9%85%b8%e6%b4%9b%e7%be%8e%e6%b2%99%e6%98%9f",
"%e7%9b%90%e9%85%b8%e9%bb%84%e9%85%ae%e5%93%8c%e9%85%af%e7%89%87",
"%e4%b9%8c%e6%b4%9b%e6%89%98%e5%93%81%e7%89%87",
"%e8%81%9a%e7%bb%b4%e9%85%ae%e7%a2%98%e6%a0%93",
"%e6%9b%b2%e5%ae%89%e5%a5%88%e5%be%b7%e7%9b%8a%e5%ba%b7%e5%94%91%e4%b9%b3%e8%86%8f",
"%e6%9b%b2%e5%92%aa%e6%96%b0%e4%b9%b3%e8%86%8f",
"%e5%a4%8d%e6%96%b9%e6%b0%a8%e8%82%bd%e7%b4%a0%e7%89%87",
"%e9%85%9e%e4%b8%81%e5%ae%89%e4%b9%b3%e8%86%8f",
"%e5%a4%8d%e6%96%b9%e9%85%ae%e5%ba%b7%e5%94%91%e8%bd%af%e8%86%8f",
"%e5%86%b0%e9%bb%84%e8%82%a4%e4%b9%90%e8%bd%af%e8%86%8f",
"%e7%81%b0%e9%bb%84%e9%9c%89%e7%b4%a0%e7%89%87",
"%e7%9b%90%e9%85%b8%e5%a4%9a%e5%a1%9e%e5%b9%b3%e4%b9%b3%e8%86%8f",
"%e7%9b%90%e9%85%b8%e7%89%b9%e6%af%94%e8%90%98%e8%8a%ac%e4%b9%b3%e8%86%8f",
"%e5%a4%8d%e6%96%b9%e6%b0%b4%e6%9d%a8%e9%85%b8%e6%90%bd%e5%89%82",
"%e7%8e%af%e5%90%a1%e9%85%ae%e8%83%ba%e4%b9%b3%e8%86%8f",
"%e5%85%8b%e9%9c%89%e5%94%91%e6%ba%b6%e6%b6%b2",
"%e5%92%aa%e5%96%b9%e8%8e%ab%e7%89%b9%e4%b9%b3%e8%86%8f",
"%e9%ac%bc%e8%87%bc%e6%af%92%e7%b4%a0%e9%85%8a",
"%e6%9b%b2%e5%ae%89%e5%a5%88%e5%be%b7%e6%96%b0%e9%9c%89%e7%b4%a0%e8%b4%b4%e8%86%8f",
"%e4%b8%83%e5%8f%82%e8%bf%9e%e8%bd%af%e8%86%8f",
"%e9%98%bf%e6%98%94%e6%b4%9b%e9%9f%a6%e4%b9%b3%e8%86%8f",
"%e6%b3%9b%e6%98%94%e6%b4%9b%e9%9f%a6%e8%83%b6%e5%9b%8a",
"%e9%85%9e%e4%b8%81%e5%ae%89%e6%90%bd%e5%89%82",
"%e8%bd%ac%e7%a7%bb%e5%9b%a0%e5%ad%90%e5%8f%a3%e6%9c%8d%e6%ba%b6%e6%b6%b2",
"%e5%a4%8d%e6%96%b9%e6%b0%b4%e6%9d%a8%e9%85%b8%e6%ba%b6%e6%b6%b2",
"%e6%ba%b6%e8%8f%8c%e9%85%b6%e8%82%a0%e6%ba%b6%e7%89%87",
"%e7%97%a2%e7%89%b9%e6%95%8f%e9%a2%97%e7%b2%92",
"%e6%b0%af%e9%9c%89%e7%b4%a0%e7%89%87",
"%e9%87%91%e8%83%86%e7%89%87",
"%e6%b3%a8%e5%b0%84%e7%94%a8%e7%9b%90%e9%85%b8%e5%a4%b4%e5%ad%a2%e6%9b%bf%e5%ae%89",
"%e6%b3%a8%e5%b0%84%e7%94%a8%e9%98%bf%e6%b4%9b%e8%a5%bf%e6%9e%97%e9%92%a0",
"%e7%9b%90%e9%85%b8%e6%b4%9b%e7%be%8e%e6%b2%99%e6%98%9f%e7%89%87",
"%e8%83%86%e4%b9%90%e8%83%b6%e5%9b%8a",
"%e5%9c%b0%e5%96%b9%e6%b0%af%e9%93%b5%e7%9f%ad%e6%9d%86%e8%8f%8c%e7%b4%a0%e5%90%ab%e7%89%87",
"%e5%96%89%e8%88%92%e5%ae%81%e7%89%87",
"%e4%ba%a4%e6%b2%99%e9%9c%89%e7%b4%a0%e7%89%87",
"%e7%9f%b3%e9%bb%84%e6%8a%97%e8%8f%8c%e7%89%87",
"%e6%b3%a8%e5%b0%84%e7%94%a8%e5%a4%b4%e5%ad%a2%e5%8c%b9%e8%83%ba%e9%92%a0",
"%e4%b9%99%e9%85%b0%e8%9e%ba%e6%97%8b%e9%9c%89%e7%b4%a0%e7%89%87",
"%e5%8f%b8%e5%b8%95%e6%b2%99%e6%98%9f%e7%89%87",
"%e5%96%b7%e6%98%94%e6%b4%9b%e9%9f%a6%e4%b9%b3%e8%86%8f",
"%e5%88%a9%e7%a6%8f%e6%98%94%e6%98%8e%e5%b9%b2%e6%b7%b7%e6%82%ac%e5%89%82",
"%e5%85%bb%e8%83%83%e8%88%92%e9%a2%97%e7%b2%92",
"%e8%83%b6%e4%bd%93%e9%85%92%e7%9f%b3%e9%85%b8%e9%93%8b%e8%83%b6%e5%9b%8a",
"%e7%9b%90%e9%85%b8%e5%b0%8f%e6%aa%97%e7%a2%b1%e7%89%87",
"%e9%93%9d%e7%a2%b3%e9%85%b8%e9%95%81%e7%89%87",
"%e5%a4%8d%e6%96%b9%e6%b0%a2%e6%b0%a7%e5%8c%96%e9%93%9d%e7%89%87",
"%e7%9b%96%e8%83%83%e5%b9%b3%e7%89%87",
"%e6%ad%a2%e7%97%a2%e5%ae%81%e7%89%87",
"%e7%a1%ab%e9%85%b8%e5%ba%86%e5%a4%a7%e9%9c%89%e7%b4%a0%e7%89%87",
"%e5%9b%ba%e8%82%a0%e6%ad%a2%e6%b3%bb%e4%b8%b8",
"%e8%82%a0%e7%82%8e%e5%ae%81%e7%89%87",
"%e8%8b%a6%e5%8f%82%e7%89%87",
"%e8%82%a0%e8%83%83%e9%80%82%e8%83%b6%e5%9b%8a",
"%e6%9f%8f%e6%a0%80%e7%a5%9b%e6%b9%bf%e6%b4%97%e6%b6%b2",
"%e6%b4%81%e5%b0%94%e9%98%b4%e6%b4%97%e6%b6%b2",
"%e5%85%8b%e7%97%92%e8%88%92%e6%b4%97%e6%b6%b2",
"%e4%b8%89%e7%bb%b4%e5%88%b6%e9%9c%89%e7%b4%a0%e6%a0%93",
"%e5%85%8b%e9%9c%89%e5%94%91%e9%98%b4%e9%81%93%e6%b3%a1%e8%85%be%e7%89%87",
"%e5%a6%87%e7%82%8e%e5%ba%b7%e8%83%b6%e5%9b%8a",
"%e7%9b%90%e9%85%b8%e5%90%97%e5%95%89%e8%83%8d%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e5%88%a9%e5%b7%b4%e9%9f%a6%e6%9e%97%e6%bb%b4%e9%bc%bb%e6%b6%b2",
"%e8%81%9a%e6%98%8e%e8%83%b6%e8%82%bd%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e7%83%ad%e6%b7%8b%e6%b8%85%e7%89%87",
"%e5%a4%8d%e6%96%b9%e8%8f%a0%e8%90%9d%e8%9b%8b%e7%99%bd%e9%85%b6%e8%82%a0%e6%ba%b6%e7%89%87",
"%e4%b8%89%e9%87%91%e7%89%87",
"%e4%b8%99%e9%85%b8%e5%80%8d%e6%b0%af%e7%b1%b3%e6%9d%be%e4%b9%b3%e8%86%8f",
"%e7%9a%ae%e8%82%a4%e7%97%85%e8%a1%80%e6%af%92%e4%b8%b8",
"%e4%ba%ba%e8%83%8e%e7%9b%98%e7%bb%84%e7%bb%87%e6%b6%b2",
"%e9%bb%91%e8%b1%86%e9%a6%8f%e6%b2%b9%e8%bd%af%e8%86%8f",
"%e6%8a%97%e5%ae%ab%e7%82%8e%e7%89%87",
"%e5%a4%b4%e5%ad%a2%e5%9c%b0%e5%b0%bc%e8%83%b6%e5%9b%8a",
"%e7%a1%ab%e8%bd%af%e8%86%8f",
"%e6%b0%a7%e5%8c%96%e9%94%8c%e8%bd%af%e8%86%8f",
"%e5%bc%ba%e5%8a%9b%e7%a2%98%e6%ba%b6%e6%b6%b2",
"%e6%a7%90%e8%a7%92%e4%b8%b8",
"%e8%82%9b%e6%b3%b0",
"%e8%82%be%e4%b8%8a%e8%85%ba%e8%89%b2%e8%85%99%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e7%b4%ab%e6%9d%89%e9%86%87%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e4%b9%9d%e5%8d%8e%e8%86%8f",
"%e6%b6%88%e7%97%94%e8%bd%af%e8%86%8f",
"%e6%b6%88%e7%97%94%e7%81%b5%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e5%a4%8d%e6%96%b9%e6%b6%88%e7%97%94%e6%a0%93",
"%e9%99%84%e5%ad%90%e7%90%86%e4%b8%ad%e4%b8%b8",
"%e8%85%b9%e5%8f%af%e5%ae%89%e7%89%87",
"%e7%9b%90%e9%85%b8%e4%bc%8a%e6%89%98%e5%bf%85%e5%88%a9%e7%89%87",
"%e5%a4%9a%e6%bd%98%e7%ab%8b%e9%85%ae%e7%89%87",
"%e9%a6%99%e7%a0%82%e5%85%bb%e8%83%83%e4%b8%b8",
"%e6%b0%af%e6%b3%a2%e5%bf%85%e5%88%a9%e7%89%87",
"%e6%b3%a8%e5%b0%84%e7%94%a8%e5%a5%a5%e7%be%8e%e6%8b%89%e5%94%91%e9%92%a0",
"%e5%85%b0%e7%b4%a2%e6%8b%89%e5%94%91%e7%89%87",
"%e8%83%83%e8%86%9c%e7%b4%a0%e8%83%b6%e5%9b%8a",
"%e6%ba%83%e7%96%a1%e7%81%b5%e8%83%b6%e5%9b%8a",
"%e6%b3%ae%e6%89%98%e6%8b%89%e5%94%91%e9%92%a0%e8%82%a0%e6%ba%b6%e7%89%87",
"%e7%97%9b%e8%a1%80%e5%ba%b7%e8%83%b6%e5%9b%8a",
"%e8%92%99%e8%84%b1%e7%9f%b3%e6%95%a3",
"%e5%bc%80%e8%83%b8%e9%a1%ba%e6%b0%94%e4%b8%b8",
"%e5%a4%8d%e6%96%b9%e7%94%b0%e4%b8%83%e8%83%83%e7%97%9b%e8%83%b6%e5%9b%8a",
"%e6%b3%bb%e7%97%a2%e5%9b%ba%e8%82%a0%e4%b8%b8",
"%e4%b8%81%e6%a1%82%e5%84%bf%e8%84%90%e8%b4%b4",
"%e4%b9%b3%e9%85%b8%e8%8f%8c%e7%b4%a0%e7%89%87",
"%e5%a4%8d%e6%96%b9%e8%83%b0%e9%85%b6%e6%95%a3",
"%e8%82%a5%e5%84%bf%e5%ae%9d%e9%a2%97%e7%b2%92",
"%e5%90%ab%e7%b3%96%e8%83%83%e8%9b%8b%e7%99%bd%e9%85%b6",
"%e8%83%b0%e9%85%b6%e8%82%a0%e6%ba%b6%e8%83%b6%e5%9b%8a",
"%e9%a6%99%e6%9e%9c%e5%81%a5%e6%b6%88%e7%89%87",
"%e5%8a%a0%e5%91%b3%e4%bf%9d%e5%92%8c%e4%b8%b8",
"%e5%bc%80%e5%a1%9e%e9%9c%b2",
"%e9%a9%ac%e6%9d%a5%e9%85%b8%e6%9b%b2%e7%be%8e%e5%b8%83%e6%b1%80%e7%89%87",
"%e9%ba%bb%e4%bb%81%e6%bb%8b%e8%84%be%e4%b8%b8",
"%e8%83%83%e8%82%a0%e7%81%b5%e8%83%b6%e5%9b%8a",
"%e8%a5%bf%e6%b2%99%e5%bf%85%e5%88%a9%e7%89%87",
"%e5%b0%8f%e5%84%bf%e6%b6%88%e9%a3%9f%e7%89%87",
"%e9%be%99%e8%83%86%e7%a2%b3%e9%85%b8%e6%b0%a2%e9%92%a0%e7%89%87",
"%e5%9f%83%e7%b4%a2%e7%be%8e%e6%8b%89%e5%94%91%e9%95%81%e8%82%a0%e6%ba%b6%e7%89%87",
"%e5%a5%a5%e7%be%8e%e6%8b%89%e5%94%91%e8%82%a0%e6%ba%b6%e8%83%b6%e5%9b%8a",
"%e7%9b%90%e9%85%b8%e9%9b%b7%e5%b0%bc%e6%9b%bf%e4%b8%81%e7%89%87",
"%e9%93%9d%e9%95%81%e5%8a%a0%e6%b7%b7%e6%82%ac%e6%b6%b2",
"%e4%bf%9d%e5%92%8c%e4%b8%b8",
"%e5%a5%a5%e7%be%8e%e6%8b%89%e5%94%91%e8%82%a0%e6%ba%b6%e7%89%87",
"%e4%b8%99%e8%b0%b7%e8%83%ba%e7%89%87",
"%e6%8b%89%e5%91%8b%e6%9b%bf%e4%b8%81%e7%89%87",
"%e6%9b%b2%e6%98%94%e5%8c%b9%e7%89%b9%e7%89%87",
"%e5%ae%89%e4%b8%ad%e7%89%87",
"%e6%b3%95%e8%8e%ab%e6%9b%bf%e4%b8%81%e8%83%b6%e5%9b%8a",
"%e5%a4%9a%e9%85%b6%e7%89%87",
"%e5%90%af%e8%84%be%e4%b8%b8",
"%e5%a4%a7%e5%b1%b1%e6%a5%82%e4%b8%b8",
"%e5%84%bf%e5%ba%b7%e5%ae%81%e7%b3%96%e6%b5%86",
"%e5%b0%8f%e5%84%bf%e5%81%a5%e8%83%83%e7%b3%96%e6%b5%86",
"%e8%b6%8a%e9%9e%a0%e4%bf%9d%e5%92%8c%e4%b8%b8",
"%e8%83%b6%e4%bd%93%e6%9e%9c%e8%83%b6%e9%93%8b%e8%83%b6%e5%9b%8a",
"%e6%b6%88%e6%97%8b%e5%b1%b1%e8%8e%a8%e8%8f%aa%e7%a2%b1%e7%89%87",
"%e7%82%8e%e7%97%a2%e5%87%80%e7%89%87",
"%e8%83%83%e7%97%9b%e5%ae%9a%e8%83%b6%e5%9b%8a",
"%e7%a1%ab%e7%b3%96%e9%93%9d%e8%83%b6%e5%9b%8a",
"%e9%98%bf%e6%9b%bf%e6%b4%9b%e5%b0%94%e7%89%87",
"%e9%85%92%e7%9f%b3%e9%85%b8%e7%be%8e%e6%89%98%e6%b4%9b%e5%b0%94%e8%83%b6%e5%9b%8a",
"%e4%b8%99%e7%a1%ab%e6%b0%a7%e5%98%a7%e5%95%b6%e7%89%87",
"%e7%8f%8d%e5%ae%9d%e4%b8%b8",
"%e7%94%b2%e5%b7%af%e5%92%aa%e5%94%91%e7%89%87",
"%e7%94%b2%e7%8a%b6%e8%85%ba%e7%89%87",
"%e5%b7%a6%e7%94%b2%e7%8a%b6%e8%85%ba%e7%b4%a0%e9%92%a0%e7%89%87",
"%e9%98%bf%e6%b3%95%e9%aa%a8%e5%8c%96%e9%86%87%e8%bd%af%e8%83%b6%e5%9b%8a",
"%e5%a4%8d%e6%96%b9%e5%b7%a6%e7%82%94%e8%af%ba%e5%ad%95%e9%85%ae%e6%bb%b4%e4%b8%b8",
"%e7%bb%b4%e5%8f%82%e9%94%8c%e8%83%b6%e5%9b%8a",
"%e5%b7%a6%e7%82%94%e8%af%ba%e5%ad%95%e9%85%ae%e7%82%94%e9%9b%8c%e9%86%9a%e7%89%87",
"%e7%b1%b3%e7%b4%a2%e5%89%8d%e5%88%97%e9%86%87%e7%89%87",
"%e7%94%b2%e7%a3%ba%e9%85%b8%e9%85%9a%e5%a6%a5%e6%8b%89%e6%98%8e%e7%89%87",
"%e7%9b%90%e9%85%b8%e4%ba%8c%e7%94%b2%e5%8f%8c%e8%83%8d%e7%89%87",
"%e6%a0%bc%e5%88%97%e9%bd%90%e7%89%b9%e7%89%87",
"%e7%9b%90%e9%85%b8%e8%8b%af%e4%b9%99%e5%8f%8c%e8%83%8d%e7%89%87",
"%e6%9c%a8%e7%b3%96%e9%86%87%e6%b3%a8%e5%b0%84%e6%b6%b2",
"%e6%a0%bc%e5%88%97%e9%bd%90%e7%89%b9%e7%bc%93%e9%87%8a%e7%89%87",
"%e7%94%b2%e8%8b%af%e7%a3%ba%e4%b8%81%e8%84%b2%e7%89%87",
"%e9%98%bf%e5%8d%a1%e6%b3%a2%e7%b3%96%e7%89%87",
"%e6%a0%bc%e5%88%97%e5%90%a1%e5%97%aa%e8%83%b6%e5%9b%8a",
"%e6%b6%88%e6%b8%b4%e9%99%8d%e7%b3%96%e7%89%87",
"%e8%b0%b7%e7%bb%b4%e7%b4%a0%e7%89%87",
"%e5%a4%a7%e9%bb%84%e8%9b%b0%e8%99%ab%e4%b8%b8"

};
        bool zanting = true;
        public static string COOKIE = "COOKIE_mode=0; ASP.NET_SessionId=5lt4qecmtxf1fgxmutukosag; COOKIE_NAME=thcgn0213; COOKIE_PWD=hy4881";
        public void run()
        {


            try
            {
                foreach (string key in keys)
                {
                    for (int i = 1; i < 51; i = i++)
                    {

                        string url = "http://www.hyey.cn/ashx/GetList2.ashx?pageIndex=" + i + "&keys=" + key + "&FGSID=&Lx=3&jg=&isck=&xqid=0&_=1557055172737";
                        string strhtml = method.GetUrlWithCookie(url, COOKIE,"utf-8");

                        
                        


                            MatchCollection  a1 = Regex.Matches(strhtml, @"""YPMC\\"":\\""([\s\S]*?)\\");
                        MatchCollection a2 = Regex.Matches(strhtml, @"""GG\\"":\\""([\s\S]*?)\\");

                        MatchCollection a3 = Regex.Matches(strhtml, @"""CDMC\\"":\\""([\s\S]*?)\\");
                        MatchCollection a4 = Regex.Matches(strhtml, @"""DJ\\"":\\""([\s\S]*?)\\");
                        MatchCollection a5 = Regex.Matches(strhtml, @"""INTEGRAL\\"":\\""([\s\S]*?)\\");
                        MatchCollection a6 = Regex.Matches(strhtml, @"""PH\\"":\\""([\s\S]*?)\\");

                        MatchCollection a7 = Regex.Matches(strhtml, @"""DW\\"":\\""([\s\S]*?)\\");
                        MatchCollection a8 = Regex.Matches(strhtml, @"""ZBZ\\"":\\""([\s\S]*?)\\");
                        MatchCollection a9 = Regex.Matches(strhtml, @"""ISRETAIL\\"":\\""([\s\S]*?)\\");
                        MatchCollection a10 = Regex.Matches(strhtml, @"YPMC\\"":\\""([\s\S]*?)\\");

                        MatchCollection a12 = Regex.Matches(strhtml, @"""PZWH\\"":\\""([\s\S]*?)\\");
                        MatchCollection a13 = Regex.Matches(strhtml, @"""YXQ\\"":\\""([\s\S]*?)\\");
                        MatchCollection a14 = Regex.Matches(strhtml, @"""BZ\\"":\\""([\s\S]*?)\\");
                        MatchCollection a15 = Regex.Matches(strhtml, @"""LSJ\\"":\\""([\s\S]*?)\\");


                        if (a1.Count == 0)  //当前页没有网址数据跳过之后的网址采集，进行下个foreach采集

                            break;

                        for (int j = 0;  j< a1.Count; j++)
                        {

    
                                ListViewItem lv1 = this.listView1.Items.Add((listView1.Items.Count + 1).ToString());
                                lv1.SubItems.Add(a1[j].Groups[1].Value);
                            lv1.SubItems.Add(a2[j].Groups[1].Value);
                            lv1.SubItems.Add(a3[j].Groups[1].Value);
                            lv1.SubItems.Add(a4[j].Groups[1].Value);
                            lv1.SubItems.Add(a5[j].Groups[1].Value);
                            lv1.SubItems.Add(a6[j].Groups[1].Value);
                            lv1.SubItems.Add(a7[j].Groups[1].Value);
                            lv1.SubItems.Add(a8[j].Groups[1].Value);
                            lv1.SubItems.Add(a9[j].Groups[1].Value);
                            lv1.SubItems.Add(a10[j].Groups[1].Value);
                            lv1.SubItems.Add(a12[j].Groups[1].Value);
                            lv1.SubItems.Add(a13[j].Groups[1].Value);
                            lv1.SubItems.Add(a14[j].Groups[1].Value);
                            lv1.SubItems.Add(a15[j].Groups[1].Value);
                           


                        }


                        while (this.zanting == false)
                            {
                                Application.DoEvents();//如果loader是false表明正在加载,,则Application.DoEvents()意思就是处理其他消息。阻止当前的队列继续执行。
                            }
                            if (this.listView1.Items.Count > 2)
                            {
                                this.listView1.EnsureVisible(this.listView1.Items.Count - 1);
                            }

                        }




                    }

                }
            
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zanting = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            zanting = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            method.DataTableToExcel(method.listViewToDataTable(this.listView1), "Sheet1", true);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void SplitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
