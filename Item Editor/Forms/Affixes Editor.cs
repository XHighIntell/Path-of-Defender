using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.IO;
namespace Path_of_Defender {
    public partial class AffixesEditor : Form {
        public Affix[] Value;

        public AffixesEditor(Affix[] value) {
            InitializeComponent();
            InitializeComponent2();
            Value = value;
            Refresh_Listview();
        }

        private void Main_Listview_DoubleClick(object sender, EventArgs e) {
            if (Main_Listview.SelectedItems.Count == 1) {
                Affix_Editor Editor = new Affix_Editor(Value[Main_Listview.SelectedItems[0].Index]);
                Editor.ShowDialog(this);
                if (Editor.DialogResult == System.Windows.Forms.DialogResult.OK) {
                    Value[Main_Listview.SelectedItems[0].Index] = Editor.Value;
                    Refresh_Listview();
                }
            }
        }
        public void Refresh_Listview() {
            Main_Listview.BeginUpdate();
            for (int i = 0; i < Value.Length; i++) {
                if (i < Main_Listview.Items.Count) {
                    ListViewItem item = Main_Listview.Items[i];
                    item.Text = (int)Value[i].Type + ". " + Value[i].Type.ToString();
                    item.SubItems[1].Text = Value[i].Level.ToString();
                    item.SubItems[2].Text = Value[i].Caption;
                    item.SubItems[3].Text = Value[i].Value1.ToString();
                    item.SubItems[4].Text = Value[i].Value2.ToString();
                    if (Value[i].IsPercent == true) { item.SubItems[5].Text = "√"; }
                    else { item.SubItems[5].Text = ""; }
                    item.SubItems[6].Text = Value[i].ToString();
                } else {
                    ListViewItem item = new ListViewItem();
                    item.Text = (int)Value[i].Type + ". " + Value[i].Type.ToString();
                    item.SubItems.Add(Value[i].Level.ToString());
                    item.SubItems.Add(Value[i].Caption);
                    item.SubItems.Add(Value[i].Value1.ToString());
                    item.SubItems.Add(Value[i].Value2.ToString());

                    if (Value[i].IsPercent == true) { item.SubItems.Add("√"); }
                    else { item.SubItems.Add(""); }
                    

                    item.SubItems.Add(Value[i].ToString());
                    Main_Listview.Items.Add(item);   
                }
            }

            //clear
            for (int i = Value.Length; i < Main_Listview.Items.Count; i++) { Main_Listview.Items.RemoveAt(i); i--; }

            Main_Listview.EndUpdate();
        }

        private void Main_ToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {
            if (e.ClickedItem == Button_Add) {
                Extensions.Add<Affix>(ref Value, new Affix() { Caption = "" });
                Refresh_Listview(); } 
            else if (e.ClickedItem == Button_Delete) { Delete(); }
            else if (e.ClickedItem == Button_Import) {
                if (OpenDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) {
                    Affix[] affixes = LoadAffixes(OpenDialog.FileName);
                    if (Main_Listview.SelectedItems.Count == 0) { Extensions.Add<Affix>(ref Value, affixes); }
                    else { Extensions.Add<Affix>(ref Value, affixes, Main_Listview.SelectedItems[0].Index); }
                    Refresh_Listview();
                };
            } else if (e.ClickedItem == Button_Export) {
                if (SaveDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) { SaveAffixes(Value, SaveDialog.FileName); };
            }
        }


        private void Main_Listview_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.C && e.Control) { Copy(); }
            else if (e.KeyCode == Keys.V && e.Control){ Paste(); }
            else if (e.KeyCode == Keys.Delete) { Delete(); }
        }

        private struct Save_Affixies {
            public Save_Affixies(Affix[] affixies) { Affixies = affixies; }
            public Affix[] Affixies;
        }

        public void Copy() {
            string file = Application.StartupPath + @"\Affix[].Copy";
            if (File.Exists(file) == true) { File.Delete(file); }

            if (Main_Listview.SelectedItems.Count > 0) {
                Affix[] affixes = new Affix[0];
                for (int i = 0; i < Main_Listview.SelectedItems.Count; i++) {
                    Extensions.Add<Affix>(ref affixes, Value[Main_Listview.SelectedItems[i].Index]);
                }
                SaveAffixes(affixes, file);
                Clipboard.SetData("Affix[]", file);
            }
        }
        public void Paste() {
            if (Clipboard.ContainsData("Affix[]") == true) {
                string file = (string)Clipboard.GetData("Affix[]");
                if (File.Exists(file) == true) {
                    Affix[] affixes = LoadAffixes(file);

                    if (Main_Listview.SelectedItems.Count == 0) {
                        Extensions.Add<Affix>(ref Value, affixes);
                    } else {
                        Extensions.Add<Affix>(ref Value, affixes, Main_Listview.SelectedItems[0].Index);
                    }
                    
                    Refresh_Listview();
                }
            }
        }
        public void Delete() {
            for (int i = Main_Listview.SelectedItems.Count -1 ; i >= 0 ; i--) {
                Extensions.RemoveAt<Affix>(ref Value, Main_Listview.SelectedItems[i].Index);
            }
            Refresh_Listview();
        }
        
        public Affix[] LoadAffixes(string file) {
            Save_Affixies save = new Save_Affixies(new Affix[0]);
            ValueType TMP = save;

            FileSystem.FileOpen(4, file, OpenMode.Binary);
            FileSystem.FileGet(4, ref TMP);
            FileSystem.FileClose(4);

            save = (Save_Affixies)TMP;
            return save.Affixies;
        }
        public void SaveAffixes(Affix[] affixes, string file) {
            FileSystem.FileOpen(4, file, OpenMode.Binary);
            FileSystem.FilePut(4, new Save_Affixies(affixes));
            FileSystem.FileClose(4);
        }
    }
}
