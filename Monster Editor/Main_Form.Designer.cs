using System.Runtime.InteropServices;
using System;
using System.Windows.Forms;
namespace Monster_Editor
{
    partial class Main_Form {
        [DllImport("uxtheme", EntryPoint = "SetWindowTheme", CharSet = CharSet.Unicode)] private static extern int SetWindowTheme(int hWnd, string pszSubAppName, string pszSubIdList);
        [DllImport("user32", EntryPoint = "PostMessageW")] private static extern int PostMessageW(int hWnd, int wMsg, int wParam, int lParam);
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) { components.Dispose(); }
            base.Dispose(disposing);
        }

        public TreeNode Node_Stages, Node_Monsters, Node_TriggersMonsters;

        public Main_Form() {
            InitializeComponent();
            SetWindowTheme(TreeView_Stages.Handle.ToInt32(), "Explorer", null);
            PostMessageW(TreeView_Stages.Handle.ToInt32(), 4396, 4, 4); //Set DOUBLEBUFFER for Treeview

            SetWindowTheme(TreeView_Monsters.Handle.ToInt32(), "Explorer", null);
            PostMessageW(TreeView_Monsters.Handle.ToInt32(), 4396, 4, 4); //Set DOUBLEBUFFER for Treeview

            SetWindowTheme(TreeView_TriggersMonsters.Handle.ToInt32(), "Explorer", null);
            PostMessageW(TreeView_TriggersMonsters.Handle.ToInt32(), 4396, 4, 4); //Set DOUBLEBUFFER for Treeview


            SetWindowTheme(ListView_Properties.Handle.ToInt32(), "Explorer", null);
            PostMessageW(ListView_Properties.Handle.ToInt32(), 4150, 65536, 65536); //ListView

            //Properties.GotFocus += delegate { PostMessageW(Properties.Handle.ToInt32(), 295, 65537, 0); };
            //Properties.SelectedIndexChanged += delegate { PostMessageW(Properties.Handle.ToInt32(), 295, 65537, 0); };

            Node_Stages = new TreeNode("Stages");
            TreeView_Stages.Nodes.Add(Node_Stages);

            Node_Monsters = new TreeNode("Monsters");
            TreeView_Monsters.Nodes.Add(Node_Monsters);

            Node_TriggersMonsters = new TreeNode("Monsters");
            TreeView_TriggersMonsters.Nodes.Add(Node_TriggersMonsters);
            Form_Load();
        }
        #region Windows Form Designer generated code

        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main_Form));
            this.Main_Tools = new System.Windows.Forms.ToolStrip();
            this.TreeView_Stages = new System.Windows.Forms.TreeView();
            this.TreeView_Monsters = new System.Windows.Forms.TreeView();
            this.Main_Menu = new System.Windows.Forms.ContextMenu();
            this.Menu_Copy = new System.Windows.Forms.MenuItem();
            this.Menu_Paste = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.Menu_New = new System.Windows.Forms.MenuItem();
            this.Menu_Delete = new System.Windows.Forms.MenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ListView_Properties = new System.Windows.Forms.ListView();
            this.ColumnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TreeView_TriggersMonsters = new System.Windows.Forms.TreeView();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.FileDialog_Open = new System.Windows.Forms.OpenFileDialog();
            this.FileDialog_Save = new System.Windows.Forms.SaveFileDialog();
            this.Button_New = new System.Windows.Forms.ToolStripButton();
            this.Button_Open = new System.Windows.Forms.ToolStripButton();
            this.Button_Save = new System.Windows.Forms.ToolStripButton();
            this.Button_Units = new System.Windows.Forms.ToolStripButton();
            this.Button_Check_Errors = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.Main_Tools.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Main_Tools
            // 
            this.Main_Tools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_New,
            this.Button_Open,
            this.Button_Save,
            this.toolStripSeparator1,
            this.Button_Units,
            this.toolStripSeparator2,
            this.Button_Check_Errors});
            this.Main_Tools.Location = new System.Drawing.Point(0, 0);
            this.Main_Tools.Name = "Main_Tools";
            this.Main_Tools.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.Main_Tools.Size = new System.Drawing.Size(1015, 25);
            this.Main_Tools.TabIndex = 5;
            this.Main_Tools.Text = "toolStrip1";
            // 
            // TreeView_Stages
            // 
            this.TreeView_Stages.AllowDrop = true;
            this.TreeView_Stages.Dock = System.Windows.Forms.DockStyle.Left;
            this.TreeView_Stages.HideSelection = false;
            this.TreeView_Stages.LabelEdit = true;
            this.TreeView_Stages.Location = new System.Drawing.Point(0, 25);
            this.TreeView_Stages.Name = "TreeView_Stages";
            this.TreeView_Stages.Size = new System.Drawing.Size(231, 426);
            this.TreeView_Stages.TabIndex = 6;
            this.TreeView_Stages.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.TreeView_Stages_BeforeLabelEdit);
            this.TreeView_Stages.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.TreeView_Stages_AfterLabelEdit);
            this.TreeView_Stages.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.TreeView_Stages_ItemDrag);
            this.TreeView_Stages.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_Stages_AfterSelect);
            this.TreeView_Stages.DragDrop += new System.Windows.Forms.DragEventHandler(this.TreeView_Stages_DragDrop);
            this.TreeView_Stages.DragEnter += new System.Windows.Forms.DragEventHandler(this.TreeView_Stages_DragEnter);
            this.TreeView_Stages.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TreeView_MouseUp);
            // 
            // TreeView_Monsters
            // 
            this.TreeView_Monsters.Dock = System.Windows.Forms.DockStyle.Right;
            this.TreeView_Monsters.HideSelection = false;
            this.TreeView_Monsters.Location = new System.Drawing.Point(795, 25);
            this.TreeView_Monsters.Name = "TreeView_Monsters";
            this.TreeView_Monsters.Size = new System.Drawing.Size(220, 426);
            this.TreeView_Monsters.TabIndex = 7;
            this.TreeView_Monsters.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TreeView_Monsters_MouseDoubleClick);
            // 
            // Main_Menu
            // 
            this.Main_Menu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.Menu_Copy,
            this.Menu_Paste,
            this.menuItem7,
            this.Menu_New,
            this.Menu_Delete});
            // 
            // Menu_Copy
            // 
            this.Menu_Copy.Index = 0;
            this.Menu_Copy.Text = "Copy";
            this.Menu_Copy.Click += new System.EventHandler(this.Menu_Click);
            // 
            // Menu_Paste
            // 
            this.Menu_Paste.Index = 1;
            this.Menu_Paste.Text = "Paste";
            this.Menu_Paste.Click += new System.EventHandler(this.Menu_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 2;
            this.menuItem7.Text = "-";
            // 
            // Menu_New
            // 
            this.Menu_New.Index = 3;
            this.Menu_New.Text = "New";
            this.Menu_New.Click += new System.EventHandler(this.Menu_Click);
            // 
            // Menu_Delete
            // 
            this.Menu_Delete.Index = 4;
            this.Menu_Delete.Text = "Delete";
            this.Menu_Delete.Click += new System.EventHandler(this.Menu_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.ListView_Properties);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(462, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(333, 426);
            this.panel1.TabIndex = 8;
            // 
            // ListView_Properties
            // 
            this.ListView_Properties.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader1,
            this.ColumnHeader4});
            this.ListView_Properties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListView_Properties.FullRowSelect = true;
            this.ListView_Properties.HideSelection = false;
            this.ListView_Properties.Location = new System.Drawing.Point(0, 0);
            this.ListView_Properties.MultiSelect = false;
            this.ListView_Properties.Name = "ListView_Properties";
            this.ListView_Properties.Size = new System.Drawing.Size(333, 426);
            this.ListView_Properties.TabIndex = 3;
            this.ListView_Properties.UseCompatibleStateImageBehavior = false;
            this.ListView_Properties.View = System.Windows.Forms.View.Details;
            this.ListView_Properties.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListView_Properties_MouseDoubleClick);
            // 
            // ColumnHeader1
            // 
            this.ColumnHeader1.Text = "Name";
            this.ColumnHeader1.Width = 152;
            // 
            // ColumnHeader4
            // 
            this.ColumnHeader4.Text = "Value";
            this.ColumnHeader4.Width = 195;
            // 
            // TreeView_TriggersMonsters
            // 
            this.TreeView_TriggersMonsters.Dock = System.Windows.Forms.DockStyle.Left;
            this.TreeView_TriggersMonsters.HideSelection = false;
            this.TreeView_TriggersMonsters.Location = new System.Drawing.Point(231, 25);
            this.TreeView_TriggersMonsters.Name = "TreeView_TriggersMonsters";
            this.TreeView_TriggersMonsters.Size = new System.Drawing.Size(231, 426);
            this.TreeView_TriggersMonsters.TabIndex = 7;
            this.TreeView_TriggersMonsters.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_TriggersMonsters_AfterSelect);
            this.TreeView_TriggersMonsters.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TreeView_MouseUp);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // FileDialog_Open
            // 
            this.FileDialog_Open.Filter = "Path of Defend Map|*.podmap|All files|*.*";
            // 
            // FileDialog_Save
            // 
            this.FileDialog_Save.Filter = "Path of Defend Map|*.podmap|All files|*.*";
            // 
            // Button_New
            // 
            this.Button_New.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_New.Image = global::Monster_Editor.Properties.Resources.New;
            this.Button_New.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_New.Name = "Button_New";
            this.Button_New.Size = new System.Drawing.Size(23, 22);
            this.Button_New.Text = "New";
            this.Button_New.ToolTipText = "Create New Map";
            this.Button_New.Click += new System.EventHandler(this.Buttons_Click);
            // 
            // Button_Open
            // 
            this.Button_Open.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_Open.Image = global::Monster_Editor.Properties.Resources.Open;
            this.Button_Open.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Open.Name = "Button_Open";
            this.Button_Open.Size = new System.Drawing.Size(23, 22);
            this.Button_Open.ToolTipText = "Open file *.podmap";
            this.Button_Open.Click += new System.EventHandler(this.Buttons_Click);
            // 
            // Button_Save
            // 
            this.Button_Save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_Save.Image = global::Monster_Editor.Properties.Resources.Save;
            this.Button_Save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Save.Name = "Button_Save";
            this.Button_Save.Size = new System.Drawing.Size(23, 22);
            this.Button_Save.ToolTipText = "Save";
            this.Button_Save.Click += new System.EventHandler(this.Buttons_Click);
            // 
            // Button_Units
            // 
            this.Button_Units.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_Units.Image = ((System.Drawing.Image)(resources.GetObject("Button_Units.Image")));
            this.Button_Units.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Units.Name = "Button_Units";
            this.Button_Units.Size = new System.Drawing.Size(23, 22);
            this.Button_Units.Text = "Monsters Editor";
            this.Button_Units.Click += new System.EventHandler(this.Buttons_Click);
            // 
            // Button_Check_Errors
            // 
            this.Button_Check_Errors.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_Check_Errors.Image = global::Monster_Editor.Properties.Resources.Warning;
            this.Button_Check_Errors.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Check_Errors.Name = "Button_Check_Errors";
            this.Button_Check_Errors.Size = new System.Drawing.Size(23, 22);
            this.Button_Check_Errors.ToolTipText = "Check errors";
            this.Button_Check_Errors.Click += new System.EventHandler(this.Buttons_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // Main_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1015, 451);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.TreeView_TriggersMonsters);
            this.Controls.Add(this.TreeView_Monsters);
            this.Controls.Add(this.TreeView_Stages);
            this.Controls.Add(this.Main_Tools);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main_Form";
            this.Text = "Map Editor";
            this.Main_Tools.ResumeLayout(false);
            this.Main_Tools.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip Main_Tools;
        private System.Windows.Forms.ToolStripButton Button_Units;
        private System.Windows.Forms.TreeView TreeView_Stages;
        private TreeView TreeView_Monsters;
        private ContextMenu Main_Menu;
        private MenuItem Menu_Copy;
        private MenuItem Menu_Paste;
        private MenuItem menuItem7;
        private MenuItem Menu_New;
        private MenuItem Menu_Delete;
        private Panel panel1;
        private TreeView TreeView_TriggersMonsters;
        public ListView ListView_Properties;
        internal ColumnHeader ColumnHeader1;
        internal ColumnHeader ColumnHeader4;
        private ToolStripButton Button_New;
        private ToolStripButton Button_Open;
        private ToolStripButton Button_Save;
        private ToolStripSeparator toolStripSeparator1;
        private OpenFileDialog FileDialog_Open;
        private SaveFileDialog FileDialog_Save;
        private ToolStripButton Button_Check_Errors;
        private ToolStripSeparator toolStripSeparator2;
    }
}

