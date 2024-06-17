using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Graphics;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using System.IO;
//Install-Package SharpDX.Toolkit.Game

namespace Path_of_Defender {
    public partial class Main_Form : Game  {
        protected override void LoadContent() {
            TMP_Item = SystemDropItem.CreateWeapon();
            //Main_Inventory.AddItem(TMP_Item);
            //Main_Property.SelectedObject = w;
            base.LoadContent();
        }
        protected override void Update(GameTime gameTime) {
             
            e.Update(); 
            Item.Hover_Item = null;
            Controls.Update();
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);
            //GraphicsDevice.Clear(Color.Black);
            Controls.Draw();
            Item.Static_Draw();

            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied, e.GraphicsDevice.SamplerStates.PointWrap);

            if (Selected == SelectedType.Item) { TMP_Item.DrawInfo(200, 25); }
            
            e.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }

    
    public partial class Main_Form : Game {
        public SystemItem SI = new SystemItem();
        public Item TMP_Item;
        public SelectedType Selected = SelectedType.None;

        public enum SelectedType { None = 0, Item = 1 }


        void Main_ToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs E) {
            if (E.ClickedItem == Button_New) { SI = new SystemItem(); Refresh_Outline(); }
            else if (E.ClickedItem == Buton_Open) { if (OpenDialog.ShowDialog(Me) == DialogResult.OK) { SI.Load(OpenDialog.FileName); Refresh_Outline(); } }
            else if (E.ClickedItem == Button_Save) { if (SaveDialog.ShowDialog(Me) == DialogResult.OK) { SI.Save(SaveDialog.FileName); Refresh_Outline(); } }
            else if (E.ClickedItem == Button_Create_Weapon_Bow || E.ClickedItem == Button_Create_Weapon_Wand || E.ClickedItem == Button_Create_Flask ||
                E.ClickedItem == Button_Create_Helmet || E.ClickedItem == Button_Create_Body_Armor || E.ClickedItem == Button_Create_Glove || E.ClickedItem == Button_Create_Boot) {
                if (E.ClickedItem == Button_Create_Weapon_Bow) { TMP_Item.Source = SystemDropItem.CreateWeapon(TypeItem.Bow); } 
                else if (E.ClickedItem == Button_Create_Weapon_Wand) { TMP_Item.Source = SystemDropItem.CreateWeapon(TypeItem.Wand); } 
                else if (E.ClickedItem == Button_Create_Flask) { TMP_Item.Source = SystemDropItem.CreateFlask(); }

                else if (E.ClickedItem == Button_Create_Helmet) { TMP_Item.Source = SystemDropItem.CreateHelmet(); }
                else if (E.ClickedItem == Button_Create_Body_Armor) { TMP_Item.Source = SystemDropItem.CreateBodyArmor(); }
                else if (E.ClickedItem == Button_Create_Glove) { TMP_Item.Source = SystemDropItem.CreateGlove(); }
                else if (E.ClickedItem == Button_Create_Boot) { TMP_Item.Source = SystemDropItem.CreateBoot(); }
                
                if (Outline_TreeView.SelectedNode == Node_White_Items) {
                    TMP_Item.Data.Level = LevelItem.Normal;
                    Extensions.Add<ItemStructure>(ref SI.data.White_Items, TMP_Item.Source);
                } else if (Outline_TreeView.SelectedNode == Node_Unique_Items) {
                    TMP_Item.Data.Level = LevelItem.Unique;
                    Extensions.Add<ItemStructure>(ref SI.data.Unique_Items, TMP_Item.Source);
                }

                Refresh_Outline();
            } else if (E.ClickedItem == Button_Create_Random_Magic) {
                TMP_Item.Source = SI.RandomMagicItem();
            } else if (E.ClickedItem == Button_Create_Random_Rare) {
                TMP_Item.Source = SI.RandomRareItem();
            } else if (E.ClickedItem == Button_Edit_Prefixes) {
                AffixesEditor Affixes_Editor = new AffixesEditor(SI.data.Prefixes);
                Affixes_Editor.ShowDialog(Me);
                SI.data.Prefixes = Affixes_Editor.Value;
            } else if (E.ClickedItem == Button_Edit_Suffixes) {
                AffixesEditor Affixes_Editor = new AffixesEditor(SI.data.Suffixes);
                Affixes_Editor.ShowDialog(Me);
                SI.data.Suffixes = Affixes_Editor.Value;
            }
        }
        private void Set_Enable(bool value) {
            Button_Create_Weapon_Bow.Enabled = value;
            Button_Create_Weapon_Dagger.Enabled = value;
            Button_Create_Weapon_Wand.Enabled = value;
            Button_Create_Flask.Enabled = value;

            Button_Create_Helmet.Enabled = value;
            Button_Create_Body_Armor.Enabled = value;
            Button_Create_Glove.Enabled = value;
            Button_Create_Boot.Enabled = value;
        }
        private void Outline_TreeView_AfterSelect(object sender, TreeViewEventArgs E) {
            if (E.Action == TreeViewAction.Unknown) { return; }
            if (E.Node == Node_White_Items || E.Node == Node_Unique_Items) { Set_Enable(true); }
            else { Set_Enable(false); }
            Refresh_Property();
        }
        private void Refresh_Property() { 
            if (Outline_TreeView.SelectedNode == Node_Others) {
                Main_Property.SelectedObject = SI;
            } else if (Outline_TreeView.SelectedNode.Parent == Node_White_Items) {
                TMP_Item.Source = SI.data.White_Items[Outline_TreeView.SelectedNode.Index];
                Main_Property.SelectedObject = TMP_Item;
                Main_Inventory.AddItem(TMP_Item);
                Selected = SelectedType.Item;
            } else if (Outline_TreeView.SelectedNode.Parent == Node_Unique_Items) {
                TMP_Item.Source = SI.data.Unique_Items[Outline_TreeView.SelectedNode.Index];
                Main_Property.SelectedObject = TMP_Item;
                Main_Inventory.AddItem(TMP_Item);
                Selected = SelectedType.Item;
            } else {
                Selected = SelectedType.None;
                Main_Property.SelectedObject = null;
                for (int i = 0; i < Main_Inventory.Items.Length; i++) { Main_Inventory.RemoveItemAt(i); }
            }
        }
        private void Refresh_Outline() { 
            Outline_TreeView.BeginUpdate();
            for (int i = 0; i < SI.data.White_Items.Length; i++) {
                if (i <Node_White_Items.Nodes.Count) { Node_White_Items.Nodes[i].Text = SI.data.White_Items[i].Caption; } 
                else { Node_White_Items.Nodes.Add(SI.data.White_Items[i].Caption); }
            }
            for (int i = 0; i < SI.data.Unique_Items.Length; i++) {
                if (i < Node_Unique_Items.Nodes.Count) { Node_Unique_Items.Nodes[i].Text = SI.data.Unique_Items[i].Caption; }
                else { Node_Unique_Items.Nodes.Add(SI.data.Unique_Items[i].Caption); }
            }

            for (int i = SI.data.White_Items.Length; i < Node_White_Items.Nodes.Count; i++) {
                Node_White_Items.Nodes.RemoveAt(i); i--;
            }
            for (int i = SI.data.Unique_Items.Length; i < Node_Unique_Items.Nodes.Count; i++) {
                Node_Unique_Items.Nodes.RemoveAt(i); i--;
            }
            Outline_TreeView.EndUpdate();
        }
        private void Copy() {
            if (Outline_TreeView.SelectedNode != null) {
                string file = Application.StartupPath + @"\ItemStructure.Copy";
                if (File.Exists(file) == true) { System.IO.File.Delete(file); }
                if (Outline_TreeView.SelectedNode.Parent == Node_White_Items) { ItemStructure.Save(SI.data.White_Items[Outline_TreeView.SelectedNode.Index], file); }
                else if (Outline_TreeView.SelectedNode.Parent == Node_Unique_Items) { ItemStructure.Save(SI.data.Unique_Items[Outline_TreeView.SelectedNode.Index], file); }
                
                Clipboard.SetData("ItemStructure", file);
            }
        }
        private void Paste() { 
            if (Clipboard.ContainsData("ItemStructure") == true) {
                string file = (string)Clipboard.GetData("ItemStructure");
                if (File.Exists(file) == true) {
                    ItemStructure item = ItemStructure.Load(file);
                    if (Outline_TreeView.SelectedNode != null) {
                        if (Outline_TreeView.SelectedNode == Node_White_Items) { Extensions.Add<ItemStructure>(ref SI.data.White_Items, item); }
                        else if (Outline_TreeView.SelectedNode.Parent == Node_White_Items) { Extensions.Add<ItemStructure>(ref SI.data.White_Items, item, Outline_TreeView.SelectedNode.Index); }

                        else if (Outline_TreeView.SelectedNode == Node_Unique_Items) { Extensions.Add<ItemStructure>(ref SI.data.Unique_Items, item); }
                        else if (Outline_TreeView.SelectedNode.Parent == Node_Unique_Items) { Extensions.Add<ItemStructure>(ref SI.data.Unique_Items, item, Outline_TreeView.SelectedNode.Index); }
                    }
                    Refresh_Outline();
                }
            }
        }
        private void Delete() { 
            if (Outline_TreeView.SelectedNode != null) {
                if (Outline_TreeView.SelectedNode.Parent == Node_White_Items) { Extensions.RemoveAt(ref SI.data.White_Items, Outline_TreeView.SelectedNode.Index); Refresh_Outline(); Refresh_Property(); }
                else if (Outline_TreeView.SelectedNode.Parent == Node_Unique_Items) { Extensions.RemoveAt(ref SI.data.Unique_Items, Outline_TreeView.SelectedNode.Index); Refresh_Outline(); Refresh_Property(); }
            }
        }
        private void Main_Property_PropertyValueChanged(object sender, PropertyValueChangedEventArgs E) {
            if (Outline_TreeView.SelectedNode.Parent == Node_White_Items) {
                SI.data.White_Items[Outline_TreeView.SelectedNode.Index] = TMP_Item.Source;
                Refresh_Outline();
            } else if (Outline_TreeView.SelectedNode.Parent == Node_Unique_Items) {
                SI.data.Unique_Items[Outline_TreeView.SelectedNode.Index] = TMP_Item.Source;
                Refresh_Outline();
            }
        }
    }
}

