using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Microsoft.VisualBasic;

namespace Monster_Editor {
    public partial class Units_Editor : Form {
        public Units_Editor() {
            InitializeComponent();
            SetWindowTheme(Objects_TreeView.Handle.ToInt32(), "Explorer", null);
            PostMessageW(Objects_TreeView.Handle.ToInt32(), 4396, 4, 4); //Set DOUBLEBUFFER for Treeview

            SetWindowTheme(Properties.Handle.ToInt32(), "Explorer", null);
            PostMessageW(Properties.Handle.ToInt32(), 4150, 65536, 65536); //ListView

            Properties.GotFocus += delegate { PostMessageW(Properties.Handle.ToInt32(), 295, 65537, 0); };
            Properties.SelectedIndexChanged += delegate { PostMessageW(Properties.Handle.ToInt32(), 295, 65537, 0); };
            Node_Monsters = new TreeNode("Monsters");
            Objects_TreeView.Nodes.Add(Node_Monsters);
            Form_Load();
        }

        FieldInfo[] MonsterStatsFields = typeof(MonsterStats).GetFields(BindingFlags.Public | BindingFlags.Instance);
        void Form_Load() { GetObjects(); }
        #region Menu Events Click
        private void Menu_Click(object sender, EventArgs e) {
            if (sender == Menu_Copy) {
                CopiedMonster = E.Map.MonstersInfo[SelectedIndex];
                IsCopied = true;
            }
            else if (sender == Menu_Paste) {
                E.Map.MonstersInfo = E.Map.MonstersInfo.Insert(SelectedIndex + 1, CopiedMonster);
                Node_Monsters.Nodes.Insert(SelectedIndex + 1, CopiedMonster.Name);
                E.Main_Form.Refresh_TreeView_Monsters();
            }
            else if (sender == Menu_New) { CreateNewMonster(); }
            else if (sender == Menu_Delete) {
                E.Map.MonstersInfo = E.Map.MonstersInfo.Delete(SelectedIndex);
                Objects_TreeView.SelectedNode.Remove();
                E.Main_Form.Refresh_TreeView_Monsters();
            }
        }

        private void Objects_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button == System.Windows.Forms.MouseButtons.Right) {
                TreeNode HitNode = Objects_TreeView.HitTest(e.X, e.Y).Node;
                if (HitNode != null) {
                    Objects_TreeView.SelectedNode = HitNode;
                    if (HitNode == Node_Monsters) {
                        Menu_Copy.Enabled = false;
                        Menu_Select.Enabled = false;
                        Menu_Delete.Enabled = false;
                        Menu_Paste.Enabled = IsCopied;
                    } else {
                        Menu_Copy.Enabled = true;
                        Menu_Select.Enabled = true;
                        Menu_Delete.Enabled = true;
                        Menu_Paste.Enabled = IsCopied;
                    }

                    Main_Menu.Show((Control)sender, new Point(e.X, e.Y));
                }
            }
        }

        int SelectedIndex; bool IsCopied; MonsterInfo CopiedMonster;
        #endregion

        #region ToolStrip
        private void Buttons_Click(object sender, EventArgs e) {
            if (sender == Button_New) {
                CreateNewMonster();
            }
        }
        #endregion
        private void Objects_TreeView_AfterSelect(object sender, TreeViewEventArgs e) {
            SelectedIndex = -1;
            if (e.Node != Node_Monsters) { 
                SelectedIndex = E.Map.GetIndex(e.Node.Text);
                GetProperties();
            } else {
                Properties.Items.Clear();
            }
        }        
        private void Properties_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (Properties.SelectedItems[0].Text == "Name") {
                Values_Editor edit = new Values_Editor(E.Map.MonstersInfo[SelectedIndex].Name, "Name:");
                DialogResult result = edit.ShowDialog(this);
                if (result == DialogResult.OK) {
                    E.Map.MonstersInfo[SelectedIndex].Name = (string)edit.Value;
                    Objects_TreeView.SelectedNode.Text = (string)edit.Value;
                    RefreshProperties();
                    E.Main_Form.Refresh_TreeView_Monsters();
                }

            } else if (Properties.SelectedItems[0].Text == "Style") {
                Values_Editor edit = new Values_Editor(E.Map.MonstersInfo[SelectedIndex].Style, "Style:");
                DialogResult result = edit.ShowDialog(this);
                if (result == DialogResult.OK) {
                    E.Map.MonstersInfo[SelectedIndex].Style = (MonsterType)edit.Value;
                    RefreshProperties();
                }
            } else {

                Values_Editor edit = new Values_Editor(
                    typeof(MonsterStats).GetField(Properties.SelectedItems[0].Text).GetValue(E.Map.MonstersInfo[SelectedIndex].Stats),
                    Properties.SelectedItems[0].Text + ":");
                DialogResult result = edit.ShowDialog(this);
                if (result == DialogResult.OK) {

                    typeof(MonsterStats).GetField(Properties.SelectedItems[0].Text).SetValueDirect(
                        __makeref(E.Map.MonstersInfo[SelectedIndex].Stats), edit.Value);
                    RefreshProperties();
                    //GetProperties();
                }
            }
        }
        /// <summary> Clear and Get Values of E.Map.MonstersInfo[SelectedIndex] and Show them on Properties ListView</summary>
        private void GetProperties() {
            ListViewItem item;
            Properties.Items.Clear();
            //Add Name và Style và trước
            item = new ListViewItem("Name"); item.SubItems.Add(E.Map.MonstersInfo[SelectedIndex].Name); Properties.Items.Add(item);
            item = new ListViewItem("Style"); item.SubItems.Add(E.Map.MonstersInfo[SelectedIndex].Style.ToString()); Properties.Items.Add(item);
            
            for (int i = 0; i < MonsterStatsFields.Length; i++) {
                item = new ListViewItem(MonsterStatsFields[i].Name);
                if (MonsterStatsFields[i].FieldType.FullName == "System.Single[]") {
                    Single[] Values = (Single[])MonsterStatsFields[i].GetValue(E.Map.MonstersInfo[SelectedIndex].Stats);
                    item.SubItems.Add(Values[0].ToString() + " - " + Values[1].ToString());
                    Properties.Items.Add(item);
                } else if (MonsterStatsFields[i].FieldType.IsArray == true) {
                    Type element = MonsterStatsFields[i].FieldType.GetElementType();
                    if (element.IsArray == false) {
                        Array arr = (Array)MonsterStatsFields[i].GetValue(E.Map.MonstersInfo[SelectedIndex].Stats);
                        item.SubItems.Add("");
                        for (int j = 0; j < arr.Length; j++) {
                            if (j == arr.Length - 1) {
                                item.SubItems[1].Text += arr.GetValue(j).ToString();
                            } else {
                                item.SubItems[1].Text += arr.GetValue(j).ToString() + ", ";
                            }
                        }
                        Properties.Items.Add(item);
                    } 
                } else {
                    item.SubItems.Add(MonsterStatsFields[i].GetValue(E.Map.MonstersInfo[SelectedIndex].Stats).ToString());
                    Properties.Items.Add(item);
                }
                
            };
        }
        /// <summary> Don't Clear </summary>
        private void RefreshProperties() {
            for (int i = 0; i < Properties.Items.Count; i++) {
                if (Properties.Items[i].Text == "Name") {
                    Properties.Items[i].SubItems[1].Text = E.Map.MonstersInfo[SelectedIndex].Name;
                } else if (Properties.Items[i].Text == "Style") {
                    Properties.Items[i].SubItems[1].Text = E.Map.MonstersInfo[SelectedIndex].Style.ToString();
                } else {
                    Object Value = typeof(MonsterStats).GetField(Properties.Items[i].Text).GetValue(E.Map.MonstersInfo[SelectedIndex].Stats);
                    if (Value.GetType().FullName == "System.Single[]") {
                        float[] Value_Float = (float[])Value;
                        Properties.Items[i].SubItems[1].Text = Value_Float[0].ToString() + " - " + Value_Float[1].ToString();
                    } else if (Value.GetType().FullName == "System.Single") {
                        Properties.Items[i].SubItems[1].Text = Value.ToString();

                    } else if (Value.GetType().IsArray == true) {
                        Type element = Value.GetType().GetElementType();
                        if (element.IsArray == false) {
                            Array arr = (Array)Value;
                            Properties.Items[i].SubItems[1].Text = "";
                            for (int j = 0; j < arr.Length; j++) {
                                if (j == arr.Length - 1) {
                                    Properties.Items[i].SubItems[1].Text += arr.GetValue(j).ToString();
                                } else {
                                    Properties.Items[i].SubItems[1].Text += arr.GetValue(j).ToString() + ", ";
                                }
                            }
                        } 
                    }
                }
            }
        }
        private void GetObjects() { 
            Node_Monsters.Nodes.Clear();
            for (int i = 0; i < E.Map.MonstersInfo.Length; i++) {
                Node_Monsters.Nodes.Add(E.Map.MonstersInfo[i].Name);
            }
        }
        private void RefreshObjects() {
            for (int i = 0; i < Node_Monsters.Nodes.Count; i++) {
                Node_Monsters.Nodes[i].Text = E.Map.MonstersInfo[i].Name;
            }
        }

        private void CreateNewMonster() { 
            MonsterCreateForm dialog = new MonsterCreateForm();
            DialogResult result = dialog.ShowDialog(this);
            if (result == System.Windows.Forms.DialogResult.OK) {
                Array.Resize(ref E.Map.MonstersInfo, E.Map.MonstersInfo.Length + 1);
                E.Map.MonstersInfo[E.Map.MonstersInfo.Length - 1] = new MonsterInfo();
                E.Map.MonstersInfo[E.Map.MonstersInfo.Length - 1].Name = dialog.Monster_Name;
                E.Map.MonstersInfo[E.Map.MonstersInfo.Length - 1].Style = dialog.Monster_Type;
                E.Map.MonstersInfo[E.Map.MonstersInfo.Length - 1].Stats = MonsterStats.Zero;
                Node_Monsters.Nodes.Add(dialog.Monster_Name);
                E.Main_Form.Refresh_TreeView_Monsters();
            }
        }

        

        #region DrapDrop
        private void Objects_ItemDrag(object sender, ItemDragEventArgs e) {
            if (e.Item != Node_Monsters) {
                Objects_TreeView.SelectedNode = (TreeNode)e.Item;
                Objects_TreeView.DoDragDrop(e.Item, DragDropEffects.Move);
            }
        }
        private void Objects_DragEnter(object sender, DragEventArgs e) {
            e.Effect = DragDropEffects.Move;
        }
        private void Objects_DragDrop(object sender, DragEventArgs e) {
            TreeNode node = Objects_TreeView.HitTest(Objects_TreeView.PointToClient(new Point(e.X, e.Y))).Node;
            TreeNode Move_node = (TreeNode)e.Data.GetData(typeof(TreeNode));
            if (node != null && node != Move_node) {
                MonsterInfo tmp = E.Map.MonstersInfo[SelectedIndex];
                E.Map.MonstersInfo = E.Map.MonstersInfo.Delete(SelectedIndex);
                

                Move_node.Remove();
                if (node == Node_Monsters) {
                    E.Map.MonstersInfo = E.Map.MonstersInfo.Insert(0, tmp);
                    Node_Monsters.Nodes.Insert(0, Move_node);
                } else {
                    E.Map.MonstersInfo = E.Map.MonstersInfo.Insert(node.Index + 1, tmp);
                    Node_Monsters.Nodes.Insert(node.Index + 1, Move_node);
                }
                Objects_TreeView.SelectedNode = Move_node;
                E.Main_Form.Refresh_TreeView_Monsters();
            }
        }
        #endregion



        



    }
}

