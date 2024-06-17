using System.Runtime.InteropServices;
namespace Path_of_Defender {
    public partial class AffixesEditor {
        [DllImport("uxtheme", EntryPoint = "SetWindowTheme", CharSet = CharSet.Unicode)] private static extern int SetWindowTheme(int hWnd, string pszSubAppName, string pszSubIdList);
        [DllImport("user32", EntryPoint = "PostMessageW")] private static extern int PostMessageW(int hWnd, int wMsg, int wParam, int lParam);
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) { components.Dispose(); }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.Main_Listview = new System.Windows.Forms.ListView();
            this.Column_Type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Column_Level = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Column_Caption = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Column_Value1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Column_Value2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Column_Percent = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Column_Text = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Main_ToolStrip = new System.Windows.Forms.ToolStrip();
            this.Button_Import = new System.Windows.Forms.ToolStripButton();
            this.Button_Export = new System.Windows.Forms.ToolStripButton();
            this.Separator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Button_Add = new System.Windows.Forms.ToolStripButton();
            this.Button_Delete = new System.Windows.Forms.ToolStripButton();
            this.OpenDialog = new System.Windows.Forms.OpenFileDialog();
            this.SaveDialog = new System.Windows.Forms.SaveFileDialog();
            this.Main_ToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // Main_Listview
            // 
            this.Main_Listview.AllowDrop = true;
            this.Main_Listview.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Column_Type,
            this.Column_Level,
            this.Column_Caption,
            this.Column_Value1,
            this.Column_Value2,
            this.Column_Percent,
            this.Column_Text});
            this.Main_Listview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Main_Listview.FullRowSelect = true;
            this.Main_Listview.Location = new System.Drawing.Point(0, 25);
            this.Main_Listview.Name = "Main_Listview";
            this.Main_Listview.Size = new System.Drawing.Size(968, 437);
            this.Main_Listview.TabIndex = 5;
            this.Main_Listview.UseCompatibleStateImageBehavior = false;
            this.Main_Listview.View = System.Windows.Forms.View.Details;
            this.Main_Listview.DoubleClick += new System.EventHandler(this.Main_Listview_DoubleClick);
            this.Main_Listview.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Main_Listview_KeyDown);
            // 
            // Column_Type
            // 
            this.Column_Type.Text = "Type";
            this.Column_Type.Width = 176;
            // 
            // Column_Level
            // 
            this.Column_Level.Text = "Level";
            this.Column_Level.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Column_Level.Width = 86;
            // 
            // Column_Caption
            // 
            this.Column_Caption.Text = "Caption";
            this.Column_Caption.Width = 84;
            // 
            // Column_Value1
            // 
            this.Column_Value1.Text = "Value 1";
            this.Column_Value1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Column_Value1.Width = 84;
            // 
            // Column_Value2
            // 
            this.Column_Value2.Text = "Value 2";
            this.Column_Value2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Column_Value2.Width = 164;
            // 
            // Column_Percent
            // 
            this.Column_Percent.Text = "Percent";
            this.Column_Percent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Column_Text
            // 
            this.Column_Text.Text = "Description";
            this.Column_Text.Width = 213;
            // 
            // Main_ToolStrip
            // 
            this.Main_ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Button_Import,
            this.Button_Export,
            this.Separator1,
            this.Button_Add,
            this.Button_Delete});
            this.Main_ToolStrip.Location = new System.Drawing.Point(0, 0);
            this.Main_ToolStrip.Name = "Main_ToolStrip";
            this.Main_ToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.Main_ToolStrip.Size = new System.Drawing.Size(968, 25);
            this.Main_ToolStrip.TabIndex = 6;
            this.Main_ToolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.Main_ToolStrip_ItemClicked);
            // 
            // Button_Import
            // 
            this.Button_Import.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_Import.Image = global::Path_of_Defender.Properties.Resources.Import;
            this.Button_Import.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Import.Name = "Button_Import";
            this.Button_Import.Size = new System.Drawing.Size(23, 22);
            this.Button_Import.Text = "Import/Add from a file";
            // 
            // Button_Export
            // 
            this.Button_Export.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_Export.Image = global::Path_of_Defender.Properties.Resources.Export;
            this.Button_Export.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Export.Name = "Button_Export";
            this.Button_Export.Size = new System.Drawing.Size(23, 22);
            this.Button_Export.Text = "Export/Save to file";
            // 
            // Separator1
            // 
            this.Separator1.Name = "Separator1";
            this.Separator1.Size = new System.Drawing.Size(6, 25);
            // 
            // Button_Add
            // 
            this.Button_Add.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_Add.Image = global::Path_of_Defender.Properties.Resources.Add;
            this.Button_Add.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Add.Name = "Button_Add";
            this.Button_Add.Size = new System.Drawing.Size(23, 22);
            this.Button_Add.Text = "Add a new Affix";
            // 
            // Button_Delete
            // 
            this.Button_Delete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_Delete.Image = global::Path_of_Defender.Properties.Resources.Delete;
            this.Button_Delete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Button_Delete.Name = "Button_Delete";
            this.Button_Delete.Size = new System.Drawing.Size(23, 22);
            this.Button_Delete.ToolTipText = "Delete selected items";
            // 
            // OpenDialog
            // 
            this.OpenDialog.Filter = "Affixes|*.Affix[]|All files|*.*";
            // 
            // SaveDialog
            // 
            this.SaveDialog.Filter = "Affixes|*.Affix[]|All files|*.*";
            // 
            // AffixesEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(968, 462);
            this.Controls.Add(this.Main_Listview);
            this.Controls.Add(this.Main_ToolStrip);
            this.Name = "AffixesEditor";
            this.Text = "Affixes Editor";
            this.Main_ToolStrip.ResumeLayout(false);
            this.Main_ToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void InitializeComponent2() {
            SetWindowTheme(Main_Listview.Handle.ToInt32(), "Explorer", null);
            PostMessageW(Main_Listview.Handle.ToInt32(), 4150, 65536, 65536);
        }

        private System.Windows.Forms.ListView Main_Listview;
        private System.Windows.Forms.ColumnHeader Column_Caption;
        private System.Windows.Forms.ColumnHeader Column_Level;
        private System.Windows.Forms.ColumnHeader Column_Type;
        private System.Windows.Forms.ColumnHeader Column_Value1;
        private System.Windows.Forms.ColumnHeader Column_Value2;
        private System.Windows.Forms.ToolStrip Main_ToolStrip;
        private System.Windows.Forms.ToolStripButton Button_Import;
        private System.Windows.Forms.ToolStripButton Button_Export;
        private System.Windows.Forms.ToolStripSeparator Separator1;
        private System.Windows.Forms.ColumnHeader Column_Text;
        private System.Windows.Forms.ToolStripButton Button_Add;
        private System.Windows.Forms.ToolStripButton Button_Delete;
        private System.Windows.Forms.OpenFileDialog OpenDialog;
        private System.Windows.Forms.SaveFileDialog SaveDialog;
        private System.Windows.Forms.ColumnHeader Column_Percent;
    }
}