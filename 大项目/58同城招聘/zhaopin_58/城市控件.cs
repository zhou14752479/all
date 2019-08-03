using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zhaopin_58
{
    public partial class 城市控件 : Form
    {
        public 城市控件()
        {
            InitializeComponent();
        }
   
        private void 城市控件_Load(object sender, EventArgs e)
        {
            getp();
            TreeNode anime = new TreeNode("所有城市");
            foreach (string plist in plists)
            {

                var sheng = anime.Nodes.Add(plist);
                ArrayList clists = getc(plist.ToString());

                foreach (string city in clists)
                {
                    sheng.Nodes.Add(city);
                }
            }
            this.treeView1.Nodes.Add(anime);
            treeView1.Nodes[0].Expand();//展开父节点
        }

        public static ArrayList checklists = new ArrayList();


        ArrayList plists = new ArrayList();
        #region 获取省份集合
        public void getp()
        {
           
            try
            {
                string constr = "Host =47.99.68.92;Database=citys;Username=root;Password=zhoukaige00.@*.";
                string str = "SELECT name from province where cid=1";
                MySqlDataAdapter da = new MySqlDataAdapter(str, constr);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    plists.Add(dr[0].ToString().Trim());
                }
            }
            catch (MySqlException ee)
            {
                MessageBox.Show(ee.Message.ToString());
            }
           

        }
        #endregion


        #region 获取省份下的城市集合
        public ArrayList getc(string sheng)
        {
            ArrayList clists = new ArrayList();
            try
            {
                string constr = "Host =47.99.68.92;Database=citys;Username=root;Password=zhoukaige00.@*.";
                string str = "SELECT name from province where sheng='" + sheng + "' ";
                MySqlDataAdapter da = new MySqlDataAdapter(str, constr);
                DataSet ds = new DataSet();
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    clists.Add(dr[0].ToString().Trim());
                }
            }
            catch (MySqlException ee)
            {
                MessageBox.Show(ee.Message.ToString());
            }

            return clists;
        }
        #endregion
      



        //取消节点选中状态之后，取消所有父节点的选中状态
        private void setParentNodeCheckedState(TreeNode currNode, bool state)
        {
            TreeNode parentNode = currNode.Parent;
            parentNode.Checked = state;
            if (currNode.Parent.Parent != null)
            {
                setParentNodeCheckedState(currNode.Parent, state);
            }
        }
        //选中节点之后，选中节点的所有子节点
        private void setChildNodeCheckedState(TreeNode currNode, bool state)
        {
            TreeNodeCollection nodes = currNode.Nodes;
            if (nodes.Count > 0)
            {
                foreach (TreeNode tn in nodes)
                {
                    tn.Checked = state;
                    setChildNodeCheckedState(tn, state);
                    checklists.Add(tn.Text);


                }
            }
        }
        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse)
            {
               
                if (e.Node.Checked == true)
                {
                    //选中节点之后，选中该节点所有的子节点
                    setChildNodeCheckedState(e.Node, true);
                }
                else if (e.Node.Checked == false)
                {
                    //取消节点选中状态之后，取消该节点所有子节点选中状态
                    setChildNodeCheckedState(e.Node, false);
                    //如果节点存在父节点，取消父节点的选中状态
                    if (e.Node.Parent != null)
                    {
                        setParentNodeCheckedState(e.Node, false);
                    }
                }
            }
           
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
        }
    }
}
