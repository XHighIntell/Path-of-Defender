using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace Path_of_Defender {
    partial class Form1 {
        [DllImport("uxtheme", EntryPoint = "SetWindowTheme", CharSet = CharSet.Unicode)] private static extern int SetWindowTheme(int hWnd, string pszSubAppName, string pszSubIdList);
        [DllImport("user32", EntryPoint = "PostMessageW")] private static extern int PostMessageW(int hWnd, int wMsg, int wParam, int lParam);
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        //TreeNode Node_White_Items = new TreeNode("White Items");
        //System.Windows.Forms.TreeNode Node_Unique_Items = new System.Windows.Forms.TreeNode("Unique Items");
        //System.Windows.Forms.TreeNode Node_Prefixes = new System.Windows.Forms.TreeNode("Prefixes");
        //System.Windows.Forms.TreeNode Node_Suffixes = new System.Windows.Forms.TreeNode("Suffixes");
        //System.Windows.Forms.TreeNode Node_Prefix_Caption = new System.Windows.Forms.TreeNode("Prefix Caption");
        //System.Windows.Forms.TreeNode Node_Suffix_Caption = new System.Windows.Forms.TreeNode("Suffix Captions");

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("White Items");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Unique Items");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Prefixes");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Suffixes");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Prefix Caption");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Suffix Captions");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Main_ToolStrip = new System.Windows.Forms.ToolStrip();
            this.Separator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Separator2 = new System.Windows.Forms.ToolStripSeparator();
            this.Outline_TreeView = new System.Windows.Forms.TreeView();
            this.Main_Property = new System.Windows.Forms.PropertyGrid();
            this.Button_New = new System.Windows.Forms.ToolStripButton();
            this.Buton_Open = new System.Windows.Forms.ToolStripButton();
            this.Button_Save = new System.Windows.Forms.ToolStripButton();
            this.Button_Create_Weapon_Bow = new System.Windows.Forms.ToolStripButton();
            this.Button_Create_Weapon_Dagger = new System.Windows.Forms.ToolStripButton();
            this.Button_Create_Weapon_Wand = new System.Windows.Forms.ToolStripButton();
            this.Button_Create_Flask = new System.Windows.Forms.ToolStripButton();
            this.Separator3 = new System.Windows.Forms.ToolStripSeparator();
            this.Button_Edit_Prefixes = new System.Windows.Forms.ToolStripButton();
            this.Button_Edit_Suffixes = new System.Windows.Forms.ToolStripButton();
            this.Main_ToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // Main_ToolStrip
            // 
            this.Main_ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_New,
            this.Buton_Open,
            this.Button_Save,
            this.Separator1,
            this.Button_Create_Weapon_Bow,
            this.Button_Create_Weapon_Dagger,
            this.Button_Create_Weapon_Wand,
            this.Separator2,
            this.Button_Create_Flask,
            this.Separator3,
            this.Button_Edit_Prefixes,
            this.Button_Edit_Suffixes});
            this.Main_ToolStrip.Location = new System.Drawing.Point(0, 0);
            this.Main_ToolStrip.Name = "Main_ToolStrip";
            this.Main_ToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.Main_ToolStrip.Size = new System.Drawing.Size(821, 25);
            this.Main_ToolStrip.TabIndex = 0;
            // 
            // Separator1
            // 
            this.Separator1.Name = "Separator1";
            this.Separator1.Size = new System.Drawing.Size(6, 25);
            // 
            // Separator2
            // 
            this.Separator2.Name = "Separator2";
            this.Separator2.Size = new System.Drawing.Size(6, 25);
            // 
            // Outline_TreeView
            // 
            this.Outline_TreeView.Dock = System.Windows.Forms.DockStyle.Left;
            this.Outline_TreeView.Location = new System.Drawing.Point(0, 25);
            this.Outline_TreeView.Name = "Outline_TreeView";
            treeNode7.Name = "";
            treeNode7.Text = "White Items";
            treeNode8.Name = "";
            treeNode8.Text = "Unique Items";
            treeNode9.Name = "";
            treeNode9.Text = "Prefixes";
            treeNode10.Name = "";
            treeNode10.Text = "Suffixes";
            treeNode11.Name = "";
            treeNode11.Text = "Prefix Caption";
            treeNode12.Name = "";
            treeNode12.Text = "Suffix Captions";
            this.Outline_TreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode7,
            treeNode8,
            treeNode9,
            treeNode10,
            treeNode11,
            treeNode12});
            this.Outline_TreeView.Size = new System.Drawing.Size(203, 424);
            this.Outline_TreeView.TabIndex = 1;
            // 
            // Main_Property
            // 
            this.Main_Property.Dock = System.Windows.Forms.DockStyle.Right;
            this.Main_Property.Location = new System.Drawing.Point(611, 25);
            this.Main_Property.Name = "Main_Property";
            this.Main_Property.Size = new System.Drawing.Size(210, 424);
            this.Main_Property.TabIndex = 2;
            // 
            // Button_New
            // 
            this.Button_New.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_New.Image = global::Path_of_Defender.Properties.Resources.New;
            this.Button_New.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_New.Name = "Button_New";
            this.Button_New.Size = new System.Drawing.Size(23, 22);
            this.Button_New.ToolTipText = "Create a new System Item";
            // 
            // Buton_Open
            // 
            this.Buton_Open.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Buton_Open.Image = global::Path_of_Defender.Properties.Resources.Open;
            this.Buton_Open.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Buton_Open.Name = "Buton_Open";
            this.Buton_Open.Size = new System.Drawing.Size(23, 22);
            this.Buton_Open.ToolTipText = "Open a System Item file";
            // 
            // Button_Save
            // 
            this.Button_Save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_Save.Image = global::Path_of_Defender.Properties.Resources.Save;
            this.Button_Save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Save.Name = "Button_Save";
            this.Button_Save.Size = new System.Drawing.Size(23, 22);
            this.Button_Save.ToolTipText = "Save";
            // 
            // Button_Create_Weapon_Bow
            // 
            this.Button_Create_Weapon_Bow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_Create_Weapon_Bow.Image = ((System.Drawing.Image)(resources.GetObject("Button_Create_Weapon_Bow.Image")));
            this.Button_Create_Weapon_Bow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Create_Weapon_Bow.Name = "Button_Create_Weapon_Bow";
            this.Button_Create_Weapon_Bow.Size = new System.Drawing.Size(23, 22);
            this.Button_Create_Weapon_Bow.ToolTipText = "Create a random Bow";
            // 
            // Button_Create_Weapon_Dagger
            // 
            this.Button_Create_Weapon_Dagger.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_Create_Weapon_Dagger.Image = global::Path_of_Defender.Properties.Resources.Wand;
            this.Button_Create_Weapon_Dagger.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Create_Weapon_Dagger.Name = "Button_Create_Weapon_Dagger";
            this.Button_Create_Weapon_Dagger.Size = new System.Drawing.Size(23, 22);
            this.Button_Create_Weapon_Dagger.ToolTipText = "Create a random Wand";
            // 
            // Button_Create_Weapon_Wand
            // 
            this.Button_Create_Weapon_Wand.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_Create_Weapon_Wand.Image = global::Path_of_Defender.Properties.Resources.Dagger;
            this.Button_Create_Weapon_Wand.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Create_Weapon_Wand.Name = "Button_Create_Weapon_Wand";
            this.Button_Create_Weapon_Wand.Size = new System.Drawing.Size(23, 22);
            this.Button_Create_Weapon_Wand.ToolTipText = "Create a random Dagger";
            // 
            // Button_Create_Flask
            // 
            this.Button_Create_Flask.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_Create_Flask.Image = global::Path_of_Defender.Properties.Resources.Flask;
            this.Button_Create_Flask.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Create_Flask.Name = "Button_Create_Flask";
            this.Button_Create_Flask.Size = new System.Drawing.Size(23, 22);
            this.Button_Create_Flask.ToolTipText = "Create a random Flask";
            // 
            // Separator3
            // 
            this.Separator3.Name = "Separator3";
            this.Separator3.Size = new System.Drawing.Size(6, 25);
            // 
            // Button_Edit_Prefixes
            // 
            this.Button_Edit_Prefixes.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Button_Edit_Prefixes.Image = ((System.Drawing.Image)(resources.GetObject("Button_Edit_Prefixes.Image")));
            this.Button_Edit_Prefixes.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Edit_Prefixes.Name = "Button_Edit_Prefixes";
            this.Button_Edit_Prefixes.Size = new System.Drawing.Size(63, 22);
            this.Button_Edit_Prefixes.Text = "Edit Prefix";
            // 
            // Button_Edit_Suffixes
            // 
            this.Button_Edit_Suffixes.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Button_Edit_Suffixes.Image = ((System.Drawing.Image)(resources.GetObject("Button_Edit_Suffixes.Image")));
            this.Button_Edit_Suffixes.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Edit_Suffixes.Name = "Button_Edit_Suffixes";
            this.Button_Edit_Suffixes.Size = new System.Drawing.Size(74, 22);
            this.Button_Edit_Suffixes.Text = "Edit Prefixes";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(821, 449);
            this.Controls.Add(this.Main_Property);
            this.Controls.Add(this.Outline_TreeView);
            this.Controls.Add(this.Main_ToolStrip);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Main_ToolStrip.ResumeLayout(false);
            this.Main_ToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ToolStrip Main_ToolStrip;
        private ToolStripButton Button_New;
        private ToolStripButton Buton_Open;
        private ToolStripButton Button_Save;
        private ToolStripSeparator Separator1;
        private ToolStripButton Button_Create_Weapon_Bow;
        private ToolStripButton Button_Create_Weapon_Dagger;
        private ToolStripButton Button_Create_Weapon_Wand;
        private ToolStripSeparator Separator2;
        private ToolStripButton Button_Create_Flask;
        private TreeView Outline_TreeView;
        private PropertyGrid Main_Property;
        private ToolStripSeparator Separator3;
        private ToolStripButton Button_Edit_Prefixes;
        private ToolStripButton Button_Edit_Suffixes;
    }
}

