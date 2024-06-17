using System.Runtime.InteropServices;
using System;
namespace Monster_Editor
{
    partial class Units_Editor {
        [DllImport("uxtheme", EntryPoint = "SetWindowTheme", CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(int hWnd, string pszSubAppName, string pszSubIdList);
        [DllImport("user32", EntryPoint = "PostMessageW")]
        private static extern int PostMessageW(int hWnd, int wMsg, int wParam, int lParam);

        /// <summary>
        /// Required designer variable.
        /// </summary>
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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Units_Editor));
            this.Properties = new System.Windows.Forms.ListView();
            this.ColumnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Objects_TreeView = new System.Windows.Forms.TreeView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.Button_New = new System.Windows.Forms.ToolStripButton();
            this.Main_Menu = new System.Windows.Forms.ContextMenu();
            this.Menu_Copy = new System.Windows.Forms.MenuItem();
            this.Menu_Paste = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.Menu_Select = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.Menu_New = new System.Windows.Forms.MenuItem();
            this.Menu_Delete = new System.Windows.Forms.MenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Properties
            // 
            this.Properties.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader1,
            this.ColumnHeader4});
            this.Properties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Properties.FullRowSelect = true;
            this.Properties.HideSelection = false;
            this.Properties.Location = new System.Drawing.Point(0, 0);
            this.Properties.MultiSelect = false;
            this.Properties.Name = "Properties";
            this.Properties.Size = new System.Drawing.Size(541, 466);
            this.Properties.TabIndex = 2;
            this.Properties.UseCompatibleStateImageBehavior = false;
            this.Properties.View = System.Windows.Forms.View.Details;
            this.Properties.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Properties_MouseDoubleClick);
            // 
            // ColumnHeader1
            // 
            this.ColumnHeader1.Text = "Name";
            this.ColumnHeader1.Width = 271;
            // 
            // ColumnHeader4
            // 
            this.ColumnHeader4.Text = "Value";
            this.ColumnHeader4.Width = 195;
            // 
            // Objects_TreeView
            // 
            this.Objects_TreeView.AllowDrop = true;
            this.Objects_TreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Objects_TreeView.HideSelection = false;
            this.Objects_TreeView.Location = new System.Drawing.Point(0, 0);
            this.Objects_TreeView.Name = "Objects_TreeView";
            this.Objects_TreeView.Size = new System.Drawing.Size(295, 466);
            this.Objects_TreeView.TabIndex = 3;
            this.Objects_TreeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.Objects_ItemDrag);
            this.Objects_TreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.Objects_TreeView_AfterSelect);
            this.Objects_TreeView.DragDrop += new System.Windows.Forms.DragEventHandler(this.Objects_DragDrop);
            this.Objects_TreeView.DragEnter += new System.Windows.Forms.DragEventHandler(this.Objects_DragEnter);
            this.Objects_TreeView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Objects_MouseUp);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_New});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(838, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // Button_New
            // 
            this.Button_New.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_New.Image = ((System.Drawing.Image)(resources.GetObject("Button_New.Image")));
            this.Button_New.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_New.Name = "Button_New";
            this.Button_New.Size = new System.Drawing.Size(23, 22);
            this.Button_New.Text = "Add New Monster";
            this.Button_New.Click += new System.EventHandler(this.Buttons_Click);
            // 
            // Main_Menu
            // 
            this.Main_Menu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.Menu_Copy,
            this.Menu_Paste,
            this.menuItem4,
            this.Menu_Select,
            this.menuItem7,
            this.Menu_New,
            this.Menu_Delete});
            // 
            // Menu_Copy
            // 
            this.Menu_Copy.Index = 0;
            this.Menu_Copy.Text = "Copy Unit";
            this.Menu_Copy.Click += new System.EventHandler(this.Menu_Click);
            // 
            // Menu_Paste
            // 
            this.Menu_Paste.Index = 1;
            this.Menu_Paste.Text = "Paste Unit";
            this.Menu_Paste.Click += new System.EventHandler(this.Menu_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.Text = "-";
            // 
            // Menu_Select
            // 
            this.Menu_Select.Index = 3;
            this.Menu_Select.Text = "Select In Tool Palette";
            this.Menu_Select.Click += new System.EventHandler(this.Menu_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 4;
            this.menuItem7.Text = "-";
            // 
            // Menu_New
            // 
            this.Menu_New.Index = 5;
            this.Menu_New.Text = "New Unit";
            this.Menu_New.Click += new System.EventHandler(this.Menu_Click);
            // 
            // Menu_Delete
            // 
            this.Menu_Delete.Index = 6;
            this.Menu_Delete.Text = "Delete Unit";
            this.Menu_Delete.Click += new System.EventHandler(this.Menu_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.Objects_TreeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.Properties);
            this.splitContainer1.Size = new System.Drawing.Size(838, 466);
            this.splitContainer1.SplitterDistance = 295;
            this.splitContainer1.SplitterWidth = 2;
            this.splitContainer1.TabIndex = 5;
            // 
            // Units_Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(838, 491);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Units_Editor";
            this.ShowIcon = false;
            this.Text = "Objects Editor";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ListView Properties;
        internal System.Windows.Forms.ColumnHeader ColumnHeader1;
        internal System.Windows.Forms.ColumnHeader ColumnHeader4;
        private System.Windows.Forms.TreeView Objects_TreeView;
        public System.Windows.Forms.TreeNode Node_Monsters;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton Button_New;
        private System.Windows.Forms.ContextMenu Main_Menu;
        private System.Windows.Forms.MenuItem Menu_Copy;
        private System.Windows.Forms.MenuItem Menu_Paste;
        private System.Windows.Forms.MenuItem Menu_Select;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem Menu_New;
        private System.Windows.Forms.MenuItem menuItem7;
        private System.Windows.Forms.MenuItem Menu_Delete;

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e) {
            PostMessageW(Properties.Handle.ToInt32(), 295, 65537, 0);
        }
        private void ListView1_GotFocus(object sender, EventArgs e) {
            PostMessageW(Properties.Handle.ToInt32(), 295, 65537, 0);
        }

        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}