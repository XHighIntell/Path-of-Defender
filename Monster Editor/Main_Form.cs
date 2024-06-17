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
    public partial class Main_Form : Form {
        Units_Editor Form_Editor; bool Copied; object CopiedObject;
        private void Form_Load() { E.Main_Form = this; }

        private void Buttons_Click(object sender, EventArgs e) {
            if (sender == Button_New) {
                E.Map.MonstersInfo = new MonsterInfo[0]; E.Map.Stages = new Stage[0];
                Refresh_AllControl();
            } else if (sender == Button_Open) {
                FileDialog_Open.ShowDialog(this);
                if (FileDialog_Open.FileName != "") {
                    E.Map.Load(FileDialog_Open.FileName);
                    Refresh_AllControl();
                }
            } else if (sender == Button_Save) {
                FileDialog_Save.ShowDialog(this);
                if (FileDialog_Save.FileName != "") {
                    E.Map.Save(FileDialog_Save.FileName);
                }
            } else if (sender == Button_Units) {
                if (Form_Editor == null || Form_Editor.IsDisposed == true) { 
                    Form_Editor= new Units_Editor();
                    Form_Editor.Show();
                } else {
                    Form_Editor.Focus();
                }
            } else if (sender == Button_Check_Errors) {
                int Errors = 0, Warning = 0;

                string TMP_String_Errors = "", TMP_String_Warning = "";
                //1 Errors:
                //Line[1]: Error in SkillB. Can't Find Skills[100]
                for (int i = 0; i < E.Map.Stages.Length; i++) {
                    for (int j = 0; j < E.Map.Stages[i].Triggers.Length; j++) { 
                        for (int k = 0; k < E.Map.Stages[i].Triggers[j].Monsters.Length; k++) {
                            if (E.Map.GetIndex(E.Map.Stages[i].Triggers[j].Monsters[k].Name) == -1) {
                                Errors++;
                                TMP_String_Errors += "Stage[" + i + "].Triggers[" + j + "].Monsters[" + k + "]: Error in Name. Can't find " + E.Map.Stages[i].Triggers[j].Monsters[k].Name + Constants.vbCrLf;
                            };
                        }
                    }
                }

                for (int i = 0; i < E.Map.Stages.Length; i++) {
                    for (int j = 0; j < E.Map.Stages[i].Triggers.Length; j++) {
                        if (j > 0 && E.Map.Stages[i].Triggers[j].TriggerTime < E.Map.Stages[i].Triggers[j - 1].TriggerTime) {
                            Warning++;
                            TMP_String_Warning += "Stage[" + i + "].Trigger[" + j + "].TriggerTime shouldn't smaller Stage[" + i + "].Trigger[" + (j - 1) + "].TriggerTime";
                        }
                    }
                }

                string TMP = Errors + " Error(s):" + Constants.vbCrLf + TMP_String_Errors;
                TMP += Warning + " Warning(s):" + Constants.vbCrLf + TMP_String_Warning;

                MessageBox.Show(this, TMP, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }
        private void TreeView_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button != MouseButtons.Right) return;

        #region Menu TreeView_Stages
            if (sender == TreeView_Stages) {
                TreeNode node = TreeView_Stages.HitTest(e.Location).Node;
                if (node == Node_Stages) {
                    Menu_Copy.Text = "Copy Stage"; Menu_Copy.Visible = false;
                    Menu_Paste.Text = "Paste Stage"; Menu_Paste.Visible = true; Menu_Paste.Enabled = false;
                    Menu_New.Text = "New Stage"; Menu_New.Visible = true; Menu_New.Enabled = true;
                    Menu_Delete.Text = "Delete Stage"; Menu_Delete.Visible = false; 

                    if (Copied == true && CopiedObject is Stage) { Menu_Paste.Enabled = true; }
                    TreeView_Stages.SelectedNode = node; 
                    Main_Menu.Show((Control)sender, e.Location);
                }

                else if (node != null && node.Parent == Node_Stages) {
                    Menu_Copy.Text = "Copy Stage"; Menu_Copy.Visible = true; Menu_Copy.Enabled = true;
                    Menu_Paste.Text = "Paste Trigger"; Menu_Paste.Visible = true; Menu_Paste.Enabled = false;
                    Menu_New.Text = "New Trigger"; Menu_New.Visible = true; Menu_New.Enabled = true;
                    Menu_Delete.Text = "Delete Stage"; Menu_Delete.Visible = true; Menu_Delete.Enabled = true;

                    if (Copied == true && CopiedObject is Trigger) { Menu_Paste.Enabled = true; }
                    TreeView_Stages.SelectedNode = node;
                    Main_Menu.Show((Control)sender, e.Location);
                }

                else if (node != null && node.Parent != null && node.Parent.Parent == Node_Stages) {
                    Menu_Copy.Text = "Copy Trigger"; Menu_Copy.Visible = true; Menu_Copy.Enabled = true;
                    Menu_Paste.Text = "Paste Trigger"; Menu_Paste.Visible = false;
                    Menu_New.Text = "New Trigger"; Menu_New.Visible = false;
                    Menu_Delete.Text = "Delete Trigger"; Menu_Delete.Visible = true; Menu_Delete.Enabled = true;

                    TreeView_Stages.SelectedNode = node;
                    Main_Menu.Show((Control)sender, e.Location);
                }
            }
        #endregion

        #region Menu TriggersMonsters
            else if (sender == TreeView_TriggersMonsters && TreeView_Stages.SelectedNode != null && TreeView_Stages.SelectedNode.Level == 2) {
                TreeNode node = TreeView_TriggersMonsters.HitTest(e.Location).Node;

                if (node == Node_TriggersMonsters) {
                    Menu_Copy.Text = "Copy Monster"; Menu_Copy.Visible = false;
                    Menu_Paste.Text = "Paste Monster"; Menu_Paste.Visible = true; Menu_Paste.Enabled = false;
                    Menu_New.Text = "New Monster"; Menu_New.Visible = true; Menu_New.Enabled = true;
                    Menu_Delete.Text = "Delete Monster"; Menu_Delete.Visible = false; 

                    if (Copied == true && CopiedObject is Trigger.Stuct_Monster) { Menu_Paste.Enabled = true; }
                    TreeView_TriggersMonsters.SelectedNode = node; 
                    Main_Menu.Show((Control)sender, e.Location);
                }

                else if (node != null && node.Level == 1) {
                    Menu_Copy.Text = "Copy Monster"; Menu_Copy.Visible = true; Menu_Copy.Enabled = true;
                    Menu_Paste.Text = "Paste Monster"; Menu_Paste.Visible = true; Menu_Paste.Enabled = false;
                    Menu_New.Text = "New Monster"; Menu_New.Visible = true; Menu_New.Enabled = true;
                    Menu_Delete.Text = "Delete Monster"; Menu_Delete.Visible = true; Menu_Delete.Enabled = true;

                    if (Copied == true && CopiedObject is Trigger.Stuct_Monster) { Menu_Paste.Enabled = true; }
                    TreeView_TriggersMonsters.SelectedNode = node;
                    Main_Menu.Show((Control)sender, e.Location);
                }
            }
        #endregion
        }
        private void Menu_Click(object sender, EventArgs e) {
            #region TreeView_Stages

            if (Main_Menu.SourceControl == TreeView_Stages) {
                if (TreeView_Stages.SelectedNode == Node_Stages) { 
                    if (sender == Menu_Paste) { E.Map.Stages = (Stage[])E.Map.Stages.Add(CopiedObject); }
                    else if (sender == Menu_New) { E.Map.Stages = (Stage[])E.Map.Stages.Add(Stage.New()); }
                }

                else if (TreeView_Stages.SelectedNode.Parent == Node_Stages) {
                    if (sender == Menu_Copy) {
                        CopiedObject = E.Map.Stages[TreeView_Stages.SelectedNode.Index]; Copied = true;
                    } else if (sender == Menu_Paste) {
                        E.Map.Stages[TreeView_Stages.SelectedNode.Index].Triggers = (Trigger[])E.Map.Stages[TreeView_Stages.SelectedNode.Index].Triggers.Add(CopiedObject);
                    } else if (sender == Menu_New) {
                        E.Map.Stages[TreeView_Stages.SelectedNode.Index].Triggers = (Trigger[])E.Map.Stages[TreeView_Stages.SelectedNode.Index].Triggers.Add(Trigger.New());
                    } else if (sender == Menu_Delete) {
                        E.Map.Stages = (Stage[])E.Map.Stages.RemoveAt(TreeView_Stages.SelectedNode.Index);
                    }
                }
                else if (TreeView_Stages.SelectedNode.Parent != null && TreeView_Stages.SelectedNode.Parent.Parent == Node_Stages) {
                    if (sender == Menu_Copy) {
                        //Copy Trigger
                        TreeNode node = TreeView_Stages.SelectedNode;
                        CopiedObject = E.Map.Stages[node.Parent.Index].Triggers[node.Index]; Copied = true;
                    } else if (sender == Menu_Delete) {
                        TreeNode node = TreeView_Stages.SelectedNode;
                        E.Map.Stages[node.Parent.Index].Triggers = (Trigger[])E.Map.Stages[node.Parent.Index].Triggers.RemoveAt(node.Index);
                    }
                }
                
                Refresh_TreeView_Stages();
            }
            #endregion

            else if (Main_Menu.SourceControl == TreeView_TriggersMonsters) {
                int StageIndex = TreeView_Stages.SelectedNode.Parent.Index;
                int TriggerIndex = TreeView_Stages.SelectedNode.Index;
                

                if (TreeView_TriggersMonsters.SelectedNode == Node_TriggersMonsters) {
                    if (sender == Menu_Paste) { E.Map.Stages[StageIndex].Triggers[TriggerIndex].Monsters = (Trigger.Stuct_Monster[])E.Map.Stages[StageIndex].Triggers[TriggerIndex].Monsters.Add(CopiedObject); }
                    else if (sender == Menu_New) { E.Map.Stages[StageIndex].Triggers[TriggerIndex].Monsters = (Trigger.Stuct_Monster[])E.Map.Stages[StageIndex].Triggers[TriggerIndex].Monsters.Add(Trigger.Stuct_Monster.New()); }
                }

                else if (TreeView_TriggersMonsters.SelectedNode.Parent == Node_TriggersMonsters) {
                    int MonsterIndex = TreeView_TriggersMonsters.SelectedNode.Index;
                    if (sender == Menu_Copy) {
                        CopiedObject = E.Map.Stages[StageIndex].Triggers[TriggerIndex].Monsters[MonsterIndex]; Copied = true;
                    } else if (sender == Menu_Paste) {
                        E.Map.Stages[StageIndex].Triggers[TriggerIndex].Monsters = (Trigger.Stuct_Monster[])E.Map.Stages[StageIndex].Triggers[TriggerIndex].Monsters.Add(CopiedObject);
                    } else if (sender == Menu_New) {
                        E.Map.Stages[StageIndex].Triggers[TriggerIndex].Monsters = (Trigger.Stuct_Monster[])E.Map.Stages[StageIndex].Triggers[TriggerIndex].Monsters.Insert(MonsterIndex, Trigger.Stuct_Monster.New());
                    } else if (sender == Menu_Delete) {
                        E.Map.Stages[StageIndex].Triggers[TriggerIndex].Monsters = (Trigger.Stuct_Monster[])E.Map.Stages[StageIndex].Triggers[TriggerIndex].Monsters.RemoveAt(MonsterIndex);
                    }
                }
                Refresh_TreeView_TriggersMonsters();
            }
        }

        private void TreeView_Stages_ItemDrag(object sender, ItemDragEventArgs e) {
            TreeNode node = (TreeNode)e.Item;
            if (node != Node_Stages) {
                TreeView_Stages.SelectedNode = node;
                TreeView_Stages.DoDragDrop(node, DragDropEffects.Move);
            }
        }
        private void TreeView_Stages_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetData(typeof(TreeNode)) != null) { e.Effect = DragDropEffects.Move; }
            else { e.Effect = DragDropEffects.None; }
            
        }
        private void TreeView_Stages_DragDrop(object sender, DragEventArgs e) {
            TreeNode node = TreeView_Stages.HitTest(TreeView_Stages.PointToClient(new Point(e.X, e.Y))).Node;
            TreeNode Move_node = (TreeNode)e.Data.GetData(typeof(TreeNode));
            if (node != null && node != Move_node && node.Level != 2 && Move_node.Parent == Node_Stages) {
                int index = Move_node.Index;
                Move_node.Remove();
                Stage tmp = E.Map.Stages[index];
                E.Map.Stages = (Stage[])E.Map.Stages.RemoveAt(index);

                if (node == Node_Stages) {
                    E.Map.Stages = (Stage[])E.Map.Stages.Insert(0, tmp);
                    Node_Stages.Nodes.Insert(0, Move_node);
                }
                else if (node.Parent == Node_Stages){
                    E.Map.Stages = (Stage[])E.Map.Stages.Insert(node.Index + 1, tmp);
                    Node_Stages.Nodes.Insert(node.Index + 1, Move_node);            
                }
                TreeView_Stages.SelectedNode = Move_node;
            }

            else if (node != null && node != Move_node && node != Node_Stages && Move_node.Parent != null && Move_node.Parent.Parent == Node_Stages) {
                int StageIndex = Move_node.Parent.Index;
                int TriggerIndex = Move_node.Index;

                Move_node.Remove();

                Trigger tmp = E.Map.Stages[StageIndex].Triggers[TriggerIndex];
                E.Map.Stages[StageIndex].Triggers = (Trigger[])E.Map.Stages[StageIndex].Triggers.RemoveAt(TriggerIndex);

                if (node.Parent == Node_Stages) {
                    E.Map.Stages[node.Index].Triggers = (Trigger[])E.Map.Stages[node.Index].Triggers.Insert(0, tmp);
                    node.Nodes.Insert(0, Move_node);
                }

                else if (node.Parent != null && node.Parent.Parent == Node_Stages){
                    E.Map.Stages[node.Parent.Index].Triggers = (Trigger[])E.Map.Stages[node.Parent.Index].Triggers.Insert(node.Index + 1, tmp);
                    node.Parent.Nodes.Insert(node.Index + 1, Move_node);
                }
                TreeView_Stages.SelectedNode = Move_node;
            }
        }
        private void ListView_Properties_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (Editing == EditingObjects.Stage) {
                Values_Editor edit = new Values_Editor(
                    typeof(Stage).GetField(ListView_Properties.SelectedItems[0].Text).GetValue(E.Map.Stages[Index]), ListView_Properties.SelectedItems[0].Text + ":");
                DialogResult result = edit.ShowDialog(this);
                if (result == DialogResult.OK) {
                    typeof(Stage).GetField(ListView_Properties.SelectedItems[0].Text).SetValueDirect(
                        __makeref(E.Map.Stages[Index]), edit.Value) ;
                    ListView_Properties.SelectedItems[0].SubItems[1].Text = edit.Value.ToString();
                    Node_Stages.Nodes[Index].Text = E.Map.Stages[Index].Caption;
                }
            }

            else if (Editing == EditingObjects.Trigger) {
                Values_Editor edit = new Values_Editor(
                    typeof(Trigger).GetField(ListView_Properties.SelectedItems[0].Text).GetValue(E.Map.Stages[TreeView_Stages.SelectedNode.Parent.Index].Triggers[Index]), 
                    ListView_Properties.SelectedItems[0].Text + ":");

                DialogResult result = edit.ShowDialog(this);
                if (result == DialogResult.OK) {
                    typeof(Trigger).GetField(ListView_Properties.SelectedItems[0].Text).SetValueDirect(
                        __makeref(E.Map.Stages[TreeView_Stages.SelectedNode.Parent.Index].Triggers[Index]), 
                        edit.Value);
                    ListView_Properties.SelectedItems[0].SubItems[1].Text = edit.Value.ToString();
                    TreeView_Stages.SelectedNode.Text = E.Map.Stages[TreeView_Stages.SelectedNode.Parent.Index].Triggers[Index].TriggerTime.ToString();
                }
            }

            else if (Editing == EditingObjects.Monster) {
                Values_Editor edit = new Values_Editor(
                    typeof(Trigger.Stuct_Monster).GetField(ListView_Properties.SelectedItems[0].Text).GetValue(E.Map.Stages[TreeView_Stages.SelectedNode.Parent.Index].Triggers[TreeView_Stages.SelectedNode.Index].Monsters[Index]), 
                    ListView_Properties.SelectedItems[0].Text + ":");

                DialogResult result = edit.ShowDialog(this);
                if (result == DialogResult.OK) {
                    typeof(Trigger.Stuct_Monster).GetField(ListView_Properties.SelectedItems[0].Text).SetValueDirect(
                        __makeref(E.Map.Stages[TreeView_Stages.SelectedNode.Parent.Index].Triggers[TreeView_Stages.SelectedNode.Index].Monsters[Index]), 
                        edit.Value);
                    ListView_Properties.SelectedItems[0].SubItems[1].Text = edit.Value.ToString();
                    TreeView_TriggersMonsters.SelectedNode.Text = E.Map.Stages[TreeView_Stages.SelectedNode.Parent.Index].Triggers[TreeView_Stages.SelectedNode.Index].Monsters[Index].Name;
                }
            }
        }

        private enum EditingObjects { None = 0, Stage = 1, Trigger = 2, Monster = 3 }
        
        EditingObjects Editing = EditingObjects.None;
        int Index = 0;
        private void TreeView_Stages_AfterSelect(object sender, TreeViewEventArgs e) {
            TreeView_TriggersMonsters.Enabled = false;
            TreeView_Monsters.Enabled = false;
            if (e.Node.Level == 0) {
                Editing = EditingObjects.None; Index = -1;
                ListView_Properties.Items.Clear();
            } else if (e.Node.Level == 1) {
                Editing = EditingObjects.Stage; Index = e.Node.Index;
                GetProperties(E.Map.Stages[e.Node.Index]);
            } else if (e.Node.Level == 2) {
                Editing = EditingObjects.Trigger; Index = e.Node.Index;
                GetProperties(E.Map.Stages[e.Node.Parent.Index].Triggers[e.Node.Index]);
                TreeView_TriggersMonsters.Enabled =true;
                TreeView_Monsters.Enabled = true;
            }

            Refresh_TreeView_TriggersMonsters();
        }
        private void TreeView_Stages_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e) { if (e.Node.Level != 1) { e.CancelEdit = true; } }
        private void TreeView_Stages_AfterLabelEdit(object sender, NodeLabelEditEventArgs e) {
            if (e.Label != null) { E.Map.Stages[e.Node.Index].Caption = e.Label; RefreshProperties(); }
        }

        private void TreeView_TriggersMonsters_AfterSelect(object sender, TreeViewEventArgs e) {
            if (e.Node.Level == 0) {
                Editing = EditingObjects.None; Index = -1;
            } else if (e.Node.Level == 1) {
                Editing = EditingObjects.Monster; Index = e.Node.Index;
            }
            RefreshProperties();
        }
        private void TreeView_Monsters_MouseDoubleClick(object sender, MouseEventArgs e) {
            TreeNode node = TreeView_Monsters.HitTest(e.Location).Node;
            if (node != null && node.Level == 1) {
                int StageIndex = TreeView_Stages.SelectedNode.Parent.Index;
                int TriggerIndex = TreeView_Stages.SelectedNode.Index;
                Trigger.Stuct_Monster New = Trigger.Stuct_Monster.New();
                New.Name = E.Map.MonstersInfo[node.Index].Name;
                E.Map.Stages[StageIndex].Triggers[TriggerIndex].Monsters = (Trigger.Stuct_Monster[])E.Map.Stages[StageIndex].Triggers[TriggerIndex].Monsters.Add(New);
                Refresh_TreeView_TriggersMonsters();
            }
        }
        //Node_Stages, Node_Monsters
        
        public void Refresh_TreeView_Stages() { 
            for (int i = 0; i < E.Map.Stages.Length; i++) {
                if (i <= Node_Stages.Nodes.Count - 1) {
                    Node_Stages.Nodes[i].Text = E.Map.Stages[i].Caption;
                } else {
                    Node_Stages.Nodes.Add(E.Map.Stages[i].Caption);
                }

                for (int j = 0; j < E.Map.Stages[i].Triggers.Length; j++) {
                    if (j <= Node_Stages.Nodes[i].Nodes.Count - 1) {
                        Node_Stages.Nodes[i].Nodes[j].Text = E.Map.Stages[i].Triggers[j].TriggerTime.ToString();
                    } else {
                        Node_Stages.Nodes[i].Nodes.Add(E.Map.Stages[i].Triggers[j].TriggerTime.ToString());
                    }
                }

                for (int j = E.Map.Stages[i].Triggers.Length; j < Node_Stages.Nodes[i].Nodes.Count; j++) {
                    Node_Stages.Nodes[i].Nodes.RemoveAt(j); j--;
                }
            }

            for (int i = E.Map.Stages.Length; i < Node_Stages.Nodes.Count; i++) {
                Node_Stages.Nodes.RemoveAt(i); i--;
            }


        }
        public void Refresh_TreeView_TriggersMonsters() { 
            Node_TriggersMonsters.Nodes.Clear();
            if (TreeView_Stages.SelectedNode != null && TreeView_Stages.SelectedNode.Level == 2) {
                int StageIndex = TreeView_Stages.SelectedNode.Parent.Index;
                int TriggerIndex = TreeView_Stages.SelectedNode.Index;
                for (int i = 0; i < E.Map.Stages[StageIndex].Triggers[TriggerIndex].Monsters.Length; i++ ) {
                    Node_TriggersMonsters.Nodes.Add(E.Map.Stages[StageIndex].Triggers[TriggerIndex].Monsters[i].Name);
                }
            }
        }
        public void Refresh_TreeView_Monsters() {
            for (int i = 0; i < E.Map.MonstersInfo.Length; i++) {
                if (i <= Node_Monsters.Nodes.Count - 1) {
                    Node_Monsters.Nodes[i].Text = E.Map.MonstersInfo[i].Name;
                } else {
                    Node_Monsters.Nodes.Add(E.Map.MonstersInfo[i].Name);
                }
            }

            for (int i = E.Map.MonstersInfo.Length; i < Node_Monsters.Nodes.Count; i++) {
                Node_Monsters.Nodes.RemoveAt(i); i--;
            }

        }
        void Refresh_AllControl() {
            Refresh_TreeView_Stages();
            Refresh_TreeView_TriggersMonsters();
            Refresh_TreeView_Monsters();
        }

        private void GetProperties(object TargetObject) {
            ListViewItem item;
            FieldInfo[] fields = TargetObject.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            ListView_Properties.Items.Clear();
            for (int i = 0; i < fields.Length; i++) {
                if (fields[i].FieldType.IsArray == false) { 
                    item = new ListViewItem(fields[i].Name);
                    item.SubItems.Add(fields[i].GetValue(TargetObject).ToString());
                    ListView_Properties.Items.Add(item);
                }
            }
        }
        private void RefreshProperties() {
            if (Editing == EditingObjects.Stage) {
                GetProperties(E.Map.Stages[Index]);
            } else if (Editing == EditingObjects.Trigger) {
                GetProperties(E.Map.Stages[TreeView_Stages.SelectedNode.Parent.Index].Triggers[Index]);
            } else if (Editing == EditingObjects.Monster) {
                GetProperties(E.Map.Stages[TreeView_Stages.SelectedNode.Parent.Index].Triggers[TreeView_Stages.SelectedNode.Index].Monsters[Index]);
            } else if (Editing == EditingObjects.None){
                ListView_Properties.Items.Clear();
            }

        }







        


    }
}

