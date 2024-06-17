using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpDX.Toolkit;
using System.Drawing;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
namespace PassiveSkillScreen_Editor {
    using PSS = Images.UI.PassiveSkillScreen;
    
    partial class Main_Edit : Game {
        [DllImport("uxtheme", EntryPoint = "SetWindowTheme", CharSet = CharSet.Unicode)] private static extern int SetWindowTheme(int hWnd, string pszSubAppName, string pszSubIdList);
        [DllImport("user32", EntryPoint = "PostMessageW")] private static extern int PostMessageW(int hWnd, int wMsg, int wParam, int lParam);

        public VirtualControlCollection Controls;
        public PassiveTree PassiveTree;

        #region Controls
        #region Menu ToolStrip
        private ToolStripButton Button_New = new ToolStripButton("Create New Passive Skill Tree", Properties.Resources.New) { DisplayStyle = ToolStripItemDisplayStyle.Image };
        private ToolStripButton Button_Open = new ToolStripButton("Open file Passive skill tree", Properties.Resources.Open) { DisplayStyle = ToolStripItemDisplayStyle.Image };
        private ToolStripButton Button_Save = new ToolStripButton("Save", Properties.Resources.Save){ DisplayStyle = ToolStripItemDisplayStyle.Image };
        
        private ToolStripButton Button_AddSkill = new ToolStripButton("Add Skill", Properties.Resources.Add);
        private ToolStripButton Button_AddLine = new ToolStripButton("Add Line", Properties.Resources.Add);
        private ToolStripButton Button_AddBackground = new ToolStripButton("Add Background", Properties.Resources.Add);
        private ToolStripButton Button_AddLabel = new ToolStripButton("Add Label", Properties.Resources.Add);
        
        private ToolStripButton Check_Property = new ToolStripButton("Show Property", Properties.Resources.Property) { Checked = true, Margin = new Padding(0, 1, 1, 2), DisplayStyle = ToolStripItemDisplayStyle.Image };
        private ToolStripButton Check_ShowNumber = new ToolStripButton("Show Index", Properties.Resources.Number) { Checked = true, DisplayStyle = ToolStripItemDisplayStyle.Image };
        private ToolStripButton Check_Skill = new ToolStripButton("Show Skills", Properties.Resources.Skill) { Checked = true, Margin = new Padding(1, 1, 1, 2), DisplayStyle = ToolStripItemDisplayStyle.Image };
        private ToolStripButton Check_Line = new ToolStripButton("Show Lines", Properties.Resources.Line) { Checked = true, DisplayStyle = ToolStripItemDisplayStyle.Image };
        private ToolStripButton Check_Background = new ToolStripButton("Show Backgrounds", Properties.Resources.Background) { Checked = true, DisplayStyle = ToolStripItemDisplayStyle.Image, Margin = new Padding(1, 1, 1, 2) };
        private ToolStripButton Check_Label = new ToolStripButton("Show Labels", Properties.Resources.Label) { Checked = true, DisplayStyle = ToolStripItemDisplayStyle.Image };
        
        private ToolStripButton Check_Outline = new ToolStripButton("Document Outline", Properties.Resources.Outline) { DisplayStyle = ToolStripItemDisplayStyle.Image };
        private ToolStripButton Check_Errors = new ToolStripButton("Error List", Properties.Resources.Warning) { Margin = new Padding(1, 1, 0, 2), DisplayStyle = ToolStripItemDisplayStyle.Image };

        private ToolStripButton Check_Symmetry_X = new ToolStripButton("When move objects, it will find and set location equal to -X of a another object.", Properties.Resources.Symmetry_X) { DisplayStyle = ToolStripItemDisplayStyle.Image };
        private ToolStripButton Check_Symmetry_Y = new ToolStripButton("When move objects, it will find and set location equal to -Y of a another object.", Properties.Resources.Symmetry_Y) { Margin = new Padding(1, 1, 1, 2), DisplayStyle = ToolStripItemDisplayStyle.Image };
        private ToolStripButton Check_Equal_X = new ToolStripButton("When move objects, it will find and set location equal to X of a another object.", Properties.Resources.Equal_X) { DisplayStyle = ToolStripItemDisplayStyle.Image };
        private ToolStripButton Check_Equal_Y = new ToolStripButton("When move objects, it will find and set location equal to Y of a another object.", Properties.Resources.Equal_Y) { Margin = new Padding(1, 1, 1, 2), DisplayStyle = ToolStripItemDisplayStyle.Image };
        private ToolStrip Main_ToolStrip = new ToolStrip() { RenderMode = System.Windows.Forms.ToolStripRenderMode.System };
        #endregion

        #region Skill Editor
        private GroupBox Skill_Property = new System.Windows.Forms.GroupBox() { Size = new Size(192, 414), Dock = DockStyle.Right, Text = "Property (Skill)", Visible = false };
        private ComboBox Skill_ImageName = new ComboBox() { FormattingEnabled = true, Location = new Point(15, 32), Size = new Size(165, 21) };
        private ComboBox Skill_Size = new ComboBox() { FormattingEnabled = true, Location = new Point(15, 79), Size = new Size(165, 21) };
        private TextBox Skill_ConnectedSkill = new TextBox() { Location = new Point(9, 119), Size = new Size(171, 20) };
        private TextBox Skill_Caption = new TextBox() { Location = new Point(12, 165), Size = new Size(171, 20) };
        private TextBox Skill_Description = new TextBox() {Multiline = true, Location = new Point(9, 209), Size = new Size(171, 100) };
        private TextBox Skill_Legend = new TextBox() { Multiline = true, ForeColor = Color.FromArgb(192, 64, 0), Location = new Point(9, 315), Size = new Size(171, 43) };
        private Button Skill_AddProperty = new Button() { Text = "+", Location = new Point(9, 360), Size = new Size(24, 21) };
        private Button Skill_DeleteProperty = new Button() { Text = "-", Location = new Point(35, 360), Size = new Size(24, 21) };
        private ComboBox Skill_PropertyIndex = new ComboBox() { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(1, 382), Size = new Size(190, 21) };
        private ComboBox Skill_PropertyType = new ComboBox() { Location = new Point(1, 409), Size = new Size(190, 21) };
        private TextBox Skill_PropertyValue = new TextBox() { Location = new Point(9, 436), Size = new Size(171, 20) };

        private System.Windows.Forms.Label Label_Skill_ImageName = new System.Windows.Forms.Label() { Text = "Image Name", Location = new Point(6, 16), AutoSize = true };
        private System.Windows.Forms.Label Label_Skill_Size = new System.Windows.Forms.Label() { AutoSize = true, Text = "Size", Location = new Point(6, 63) };
        private System.Windows.Forms.Label Label_ConnectedSkill = new System.Windows.Forms.Label() { Text = "Connected Skill", AutoSize = true, Location = new Point(6, 104) };
        private System.Windows.Forms.Label Label_Skill_Caption = new System.Windows.Forms.Label() { Text = "Caption", AutoSize = true, Location = new Point(6, 149) };
        private System.Windows.Forms.Label Label_Skill_Description = new System.Windows.Forms.Label() { Text = "Description", AutoSize = true, Location = new Point(6, 193), Size = new Size(60, 13) };
        #endregion

        #region Line Editor
        private GroupBox Line_Property = new GroupBox() { Text = "Property (Line)", Visible = false, Dock = DockStyle.Right, Size = new Size(192, 414) };
        private NumericUpDown Line_SkillA = new NumericUpDown() { Maximum = 999999, Location = new Point(49, 21), Size = new Size(120, 20) };
        private NumericUpDown Line_SkillB = new NumericUpDown() { Maximum = 999999, Location = new Point(49, 61), Size = new Size(120, 20) };
        private ComboBox Line_Style = new ComboBox() { Text = "Line", FormattingEnabled = true, Location = new Point(46, 100), Size = new Size(123, 21) };
        private NumericUpDown Line_Width = new NumericUpDown() { Maximum = 10000, Location = new Point(49, 133), Size = new Size(120, 20) };
        private NumericUpDown Line_Alpha = new NumericUpDown() { DecimalPlaces = 8, Increment = 0.1m, Minimum = -100, Location = new Point(49, 166), Size = new Size(120, 20) };

        private System.Windows.Forms.Label Label_Line_SkillA = new System.Windows.Forms.Label() { Text = "Skill A", AutoSize = true, Location = new Point(10, 23) };
        private System.Windows.Forms.Label Label_Line_SkillB = new System.Windows.Forms.Label() { Text = "Skill B", AutoSize = true, Location = new Point(10, 63) };
        private System.Windows.Forms.Label Label_Line_Style = new System.Windows.Forms.Label() { Text = "Style", AutoSize = true, Location = new Point(10, 103) };
        private System.Windows.Forms.Label Label_Line_Width = new System.Windows.Forms.Label() { Text = "Width", AutoSize = true, Location = new Point(10, 135) };
        private System.Windows.Forms.Label Label_Line_Alpha = new System.Windows.Forms.Label() { Text = "Alpha", AutoSize = true, Location = new Point(10, 168) };
        #endregion

        #region Label Editor
        private GroupBox Label_Property = new GroupBox() { Text = "Property (Label)", Visible = false, Dock = DockStyle.Right, Size = new Size(196, 549) };
        private TextBox Label_X = new TextBox() { Text = "0", Location = new Point(60, 32), Size = new Size(44, 20) };
        private TextBox Label_Y = new TextBox() { Text = "0", Location = new Point(112, 32), Size = new Size(44, 20) };
        private TextBox Label_Text = new TextBox() { Multiline = true, Location = new Point(6, 175), Size = new Size(184, 119) };
        private NumericUpDown Label_Red = new NumericUpDown() { Maximum = 255, Location = new Point(11, 78), Size = new Size(45, 20) };
        private NumericUpDown Label_Green = new NumericUpDown() { Maximum = 255, Location = new Point(56, 78), Size = new Size(45, 20) };
        private NumericUpDown Label_Blue = new NumericUpDown() { Maximum = 255, Location = new Point(100, 78), Size = new Size(45, 20) };
        private NumericUpDown Label_Opacity = new NumericUpDown() { Maximum = 255, Location = new Point(144, 78), Size = new Size(45, 20) };
        private NumericUpDown Label_Alpha = new NumericUpDown() { DecimalPlaces = 2, Increment = 0.01m, Location = new Point(56, 106), Size = new Size(94, 20) };
        private NumericUpDown Label_Scale = new NumericUpDown() { DecimalPlaces = 2, Increment = 0.01m, Location = new Point(56, 131), Size = new Size(94, 20) };

        private System.Windows.Forms.Label Label_Label_Location = new System.Windows.Forms.Label() { Text = "Location", AutoSize = true, Location = new Point(8, 35) };
        private System.Windows.Forms.Label Label_Label_Color = new System.Windows.Forms.Label() { Text = "   Red        Green     Blue      Opacity", AutoSize = true, Location = new Point(8, 61) };
        private System.Windows.Forms.Label Label_Label_Alpha = new System.Windows.Forms.Label() { Text = "Alpha", AutoSize = true, Location = new Point(8, 108) };
        private System.Windows.Forms.Label Label_Label_Scale = new System.Windows.Forms.Label() { Text = "Scale", AutoSize = true, Location = new System.Drawing.Point(8, 133) };
        private System.Windows.Forms.Label Label_Label_Text = new System.Windows.Forms.Label() { Text = "Text", AutoSize = true, Location = new System.Drawing.Point(8, 159) };
        #endregion

        #region Background Editor
        private GroupBox Background_Property = new GroupBox() { Text = "Property (Background)", Visible = false, Dock = DockStyle.Right, Size = new Size(192, 414) };
        private ComboBox Background_ImageName = new ComboBox() { FormattingEnabled = true, Location = new Point(9, 32), Size = new Size(177, 21) };
        private ComboBox Background_Effect = new ComboBox() { FormattingEnabled = true, Location = new Point(9, 79), Size = new Size(177, 21) };

        private System.Windows.Forms.Label Label_Background_ImageName = new System.Windows.Forms.Label() { Text = "Image Name", AutoSize = true, Location = new Point(6, 16) };
        private System.Windows.Forms.Label Label_Background_Effect = new System.Windows.Forms.Label() { Text = "Background's Effect", AutoSize = true, Location = new Point(6, 63) };
        #endregion

        #region Dialogs + Others
        private OpenFileDialog FileDialog_Open = new OpenFileDialog() { Filter = "Passive skill tree|*.pst|All files|*.*" };
        private SaveFileDialog FileDialog_Save = new SaveFileDialog() { Filter = "Passive skill tree|*.pst|All files|*.*" };

        private PropertyGrid Main_Property = new PropertyGrid() { Dock = DockStyle.Right, Size = new Size(192, 414), PropertySort = PropertySort.NoSort };

        private Panel Outline_Panel = new Panel() { Visible = false, Dock = DockStyle.Left, Size = new Size(207, 436) };
        private Panel Outline_PanelButtons = new Panel() { Dock = DockStyle.Top, Size = new Size(207, 33) };
        private Button Button_Delete = new Button() { Enabled = false, Image = Properties.Resources.Delete, Location = new Point(75, 2), Size = new Size(30, 30) };
        private Button Button_Down = new Button() { Enabled = false, Image = Properties.Resources.Down, Location = new Point(2, 1), Size = new Size(30, 30) };
        private Button Button_Up = new Button() { Enabled = false, Image = Properties.Resources.Up, Location = new Point(39, 2), Size = new Size(30, 30) };
        private TreeView Outline_TreeView = new TreeView() { HideSelection = false, ShowLines = false, Dock = DockStyle.Fill };

        private Panel Errors_Panel = new Panel() { Visible = false, Dock = DockStyle.Bottom, Size = new Size(372, 232) };
        private TextBox Errors_TextBox = new TextBox() { Multiline = true, Dock = DockStyle.Fill };
        #endregion
        #endregion
        
        Form me;
        protected override void OnWindowCreated() {
            base.OnWindowCreated(); me = (Form)this.Window.NativeWindow;
            Control[] controls;
            #region Main Menu
            ToolStripItem[] menu_controls = new ToolStripItem[] { Button_New, Button_Open, Button_Save, new ToolStripSeparator(), Button_AddSkill, Button_AddLine, Button_AddBackground, Button_AddLabel, new ToolStripSeparator(), Check_Property, Check_ShowNumber, Check_Skill, Check_Line, Check_Background, Check_Label, new ToolStripSeparator(), Check_Outline, Check_Errors, new ToolStripSeparator(), Check_Symmetry_X, Check_Symmetry_Y, Check_Equal_X, Check_Equal_Y };
            Main_ToolStrip.Items.AddRange(menu_controls);
            #endregion
            
            #region Skill
            controls = new Control[] { Skill_ImageName, Skill_Size, Skill_ConnectedSkill, Skill_Caption, Skill_Description, Skill_Legend, Skill_AddProperty, Skill_DeleteProperty, Skill_PropertyIndex, Skill_PropertyType, Skill_PropertyValue, Label_Skill_ImageName, Label_ConnectedSkill, Label_Skill_Size, Label_Skill_Caption, Label_Skill_Description };
            Skill_Property.Controls.AddRange(controls);
            Skill_Size.Items.AddRange(new object[] { "Small", "Medium", "Large"});
            #endregion

            #region Line
            Line_Style.Items.AddRange(new object[] { "Line", "Tiny_Circle", "Small_Circle", "Medium_Circle", "Large_Circle" });
            controls = new Control[] { Line_SkillA, Line_SkillB, Line_Style, Line_Width, Line_Alpha, Label_Line_SkillA, Label_Line_SkillB, Label_Line_Style, Label_Line_Width, Label_Line_Alpha };
            Line_Property.Controls.AddRange(controls);
            #endregion

            #region Background
            Background_Effect.Items.AddRange(new object[] { "None", "FlipVertically", "FlipHorizontally", "FlipBoth" });
            controls = new Control[] { Background_ImageName, Background_Effect, Label_Background_ImageName, Label_Background_Effect };
            Background_Property.Controls.AddRange(controls);
            #endregion

            #region Label
            controls = new Control[] { Label_X, Label_Y, Label_Text, Label_Red, Label_Green, Label_Blue, Label_Opacity, Label_Alpha, Label_Scale, Label_Label_Location, Label_Label_Color, Label_Label_Alpha, Label_Label_Scale, Label_Label_Text };
            Label_Property.Controls.AddRange(controls);
            #endregion

            #region Document Outline & Errors
            controls = new Control[] { Outline_TreeView, Outline_PanelButtons };
            Outline_Panel.Controls.AddRange(controls);

            controls = new Control[] { Button_Delete, Button_Up, Button_Down };
            Outline_PanelButtons.Controls.AddRange(controls);

            SetWindowTheme(Outline_TreeView.Handle.ToInt32(), "Explorer", null);
            PostMessageW(Outline_TreeView.Handle.ToInt32(), 4396, 4, 4);

            Errors_Panel.Controls.Add(Errors_TextBox);
            #endregion

            #region Form
            me.SuspendLayout();
            controls = new Control[] { Main_Property, Skill_Property, Line_Property, Background_Property, Label_Property, Outline_Panel, Errors_Panel, Main_ToolStrip };
            me.Controls.AddRange(controls);
            me.Text = "Passive Skill Screen Editor";
            me.Icon = Properties.Resources.X;
            me.ResumeLayout(false);
            #endregion
        }

        protected override void BeginRun() {
            SetEvent();
            ClearAndAddOutline();
            base.BeginRun();
        }
        private void SetEvent() {
            //Menu
            Main_ToolStrip.ItemClicked += new ToolStripItemClickedEventHandler(Menu_Click);
            PassiveTree.ItemMouseClick +=new MouseEventHandler(PassiveTree_ItemMouseClick);

            //Form
            me.Left = 0; me.Top = 0;
            me.SizeChanged +=new EventHandler(Me_SizeChanged);

            //Outline
            Node_Skills = Outline_TreeView.Nodes.Add("Skills", "Skills");
            Node_Lines = Outline_TreeView.Nodes.Add("Lines", "Lines");
            Node_Backgrounds = Outline_TreeView.Nodes.Add("Backgrounds", "Backgrounds");
            Node_Labels = Outline_TreeView.Nodes.Add("Labes", "Labes");

            Outline_TreeView.AfterSelect += new TreeViewEventHandler(Outline_AfterSelect);
            Button_Up.Click += new EventHandler(Outline_Buttons);
            Button_Down.Click += new EventHandler(Outline_Buttons);
            Button_Delete.Click += new EventHandler(Outline_Buttons);

            //Skill Property
            Skill_ImageName.TextChanged += new System.EventHandler(this.Skill_Changed);
            Skill_Size.SelectedIndexChanged += new System.EventHandler(this.Skill_Changed);
            Skill_ConnectedSkill.TextChanged += new EventHandler(Skill_Changed);
            Skill_Caption.TextChanged += new EventHandler(Skill_Changed);
            Skill_Description.TextChanged += new EventHandler(Skill_Changed);
            Skill_Legend.TextChanged += new EventHandler(Skill_Changed);
            for (int i = 0; i < PSS.Skills.Names.Length; i++) {
                if (PSS.Skills.Names[i] != null) {
                    Skill_ImageName.Items.Add(PSS.Skills.Names[i]);
                }
            }
            Skill_PropertyIndex.SelectedIndexChanged += new EventHandler(Skill_PropertyIndex_Changed);
            Skill_PropertyType.TextChanged += new EventHandler(Skill_Changed);
            Skill_PropertyValue.TextChanged += new EventHandler(Skill_Changed);

            string[] enums =typeof(TypeProperty).GetEnumNames();
            for (int i = 0; i < enums.Length; i++) {
                Skill_PropertyType.Items.Add(enums[i]);
            }
            Skill_AddProperty.Click += new EventHandler(Skill_Changed);
            Skill_DeleteProperty.Click += new EventHandler(Skill_Changed);
            


            //Line
            Line_Alpha.ValueChanged += new EventHandler(Skill_Changed);
            Line_SkillA.ValueChanged += new EventHandler(Skill_Changed);
            Line_SkillB.ValueChanged += new EventHandler(Skill_Changed);
            Line_Style.SelectedValueChanged += new EventHandler(Skill_Changed);
            Line_Width.ValueChanged += new EventHandler(Skill_Changed);

            //Background
            Background_Effect.SelectedValueChanged += new EventHandler(Skill_Changed);
            Background_ImageName.TextChanged += new EventHandler(Skill_Changed);
            for (int i = 0; i < PSS.ScreenBackGround.Names.Length; i++) {
                Background_ImageName.Items.Add(PSS.ScreenBackGround.Names[i]);
            }

            #region Events on Label_Property
            Label_X.TextChanged += new EventHandler(Skill_Changed);
            Label_Y.TextChanged += new EventHandler(Skill_Changed);
            Label_Red.ValueChanged += new EventHandler(Skill_Changed);
            Label_Green.ValueChanged += new EventHandler(Skill_Changed);
            Label_Blue.ValueChanged += new EventHandler(Skill_Changed);
            Label_Opacity.ValueChanged += new EventHandler(Skill_Changed);
            Label_Alpha.ValueChanged += new EventHandler(Skill_Changed);
            Label_Scale.ValueChanged += new EventHandler(Skill_Changed);
            Label_Text.TextChanged += new EventHandler(Skill_Changed);
            #endregion
        }
        //Menu
        private void Menu_Click(object sender, ToolStripItemClickedEventArgs  e) {
            if (e.ClickedItem == Button_New) {
                Dialog_Editor s = new Dialog_Editor(Dialog_Editor.DiablogType.New, ref PassiveTree.SelectedData);
                if (s.ShowDialog() == DialogResult.OK) {
                    Type = SelectType.None; Index = -1; PropertiesGet();
                };
                
            } else if (e.ClickedItem == Button_Open) {
                FileDialog_Open.ShowDialog();
                if (FileDialog_Open.FileName != "") {
                    PassiveTree.SelectedData.Load(FileDialog_Open.FileName);
                    ClearAndAddOutline();
                }
            } else if (e.ClickedItem == Button_Save) {
                FileDialog_Save.ShowDialog(me);
                if (FileDialog_Save.FileName != "") { PassiveTree.SelectedData.Save(FileDialog_Save.FileName); }
            } else if (e.ClickedItem == Button_AddSkill) {
                PassiveTree.AddSkill(new PassiveSkill("Spell Damage",
                    new SharpDX.Vector2((PassiveTree.Position.X + GameSetting.Width / 2) / PassiveTree.Scale, (PassiveTree.Position.Y + GameSetting.Height / 2) / PassiveTree.Scale)
                    ,SkillStatus.Unallocated, SkillSize.Small));
                RefreshOutline();
            } else if (e.ClickedItem == Button_AddLine) {
                PassiveTree.AddLine(new Line(0, 0,
                    new SharpDX.Vector2((PassiveTree.Position.X + GameSetting.Width / 2) / PassiveTree.Scale, (PassiveTree.Position.Y + GameSetting.Height / 2) / PassiveTree.Scale), 110, 0, LineStyle.Line));
                RefreshOutline();
            } else if (e.ClickedItem == Button_AddBackground) {
                PassiveTree.AddBackground(new Background(PSS.ScreenBackGround.Names[0],
                    new SharpDX.Vector2((PassiveTree.Position.X + GameSetting.Width / 2) / PassiveTree.Scale, (PassiveTree.Position.Y + GameSetting.Height / 2) / PassiveTree.Scale), SharpDX.Toolkit.Graphics.SpriteEffects.None));
                RefreshOutline();
            } else if (e.ClickedItem == Button_AddLabel) {
                PassiveTree.AddLabel(new Label(new SharpDX.Vector2((PassiveTree.Position.X + GameSetting.Width / 2) / PassiveTree.Scale, (PassiveTree.Position.Y + GameSetting.Height / 2) / PassiveTree.Scale), "New Label", SharpDX.Color.White, 0, 1));
                RefreshOutline();
            } else if (e.ClickedItem == Check_Property) {
                Check_Property.Checked = !Check_Property.Checked;
                PropertiesGet();
            } else if (e.ClickedItem == Check_ShowNumber || e.ClickedItem == Check_Skill || e.ClickedItem == Check_Line || e.ClickedItem == Check_Background || e.ClickedItem == Check_Label ||
                e.ClickedItem == Check_Symmetry_X || e.ClickedItem == Check_Symmetry_Y || e.ClickedItem == Check_Equal_X || e.ClickedItem == Check_Equal_Y) {
                ((ToolStripButton)e.ClickedItem).Checked = !((ToolStripButton)e.ClickedItem).Checked;
            } else if (e.ClickedItem == Check_Outline) {
                Check_Outline.Checked = !Check_Outline.Checked;
                if (Check_Outline.Checked == true) { RefreshOutline(); }
                Outline_Panel.Visible = Check_Outline.Checked;
            } else if (e.ClickedItem == Check_Errors) {
                Check_Errors.Checked = !Check_Errors.Checked;
                Errors_Panel.Visible = Check_Errors.Checked;
            } 
        }
        private void PassiveTree_MouseWheel(object sender, MouseEventArgs e) {
            me.Text = "Passive Skill Screen Editor - Zoom " + PassiveTree.Scale;
        }
        private void Me_SizeChanged(object sender, EventArgs e) {
            GameSetting.Width = me.ClientRectangle.Width;
            GameSetting.Height = me.ClientRectangle.Height;
            PassiveTree.Width = GameSetting.Width;
            PassiveTree.Height = GameSetting.Height;
        }

        //Skill
        private void Skill_PropertyIndex_Changed(object sender, EventArgs e) {
            if (IsSettingProperties == true) { return; }
            IsGettingProperties = true;
            Skill_PropertyType.Text = PassiveTree.SelectedData.Skills[Index].Properties[Skill_PropertyIndex.SelectedIndex].Type.ToString();
            Skill_PropertyValue.Text = String.Join(",", PassiveTree.SelectedData.Skills[Index].Properties[Skill_PropertyIndex.SelectedIndex].Value);
            IsGettingProperties = false;
        }

        //Property
        bool IsGettingProperties, IsSettingProperties;
        
        private void PropertiesGet() {
            Skill_Property.Visible = false;
            Line_Property.Visible = false;
            Background_Property.Visible = false;
            Main_Property.Visible = false;
            Label_Property.Visible = false;
            if (Check_Property.Checked == false) { return; }

            me.Focus();

            IsGettingProperties = true;
            if (Type == SelectType.Label ) {
                Label_X.Text = PassiveTree.SelectedData.Labels[Index].Position.X.ToString();
                Label_Y.Text = PassiveTree.SelectedData.Labels[Index].Position.Y.ToString();
                Label_Red.Value = PassiveTree.SelectedData.Labels[Index].Color.R;
                Label_Green.Value = PassiveTree.SelectedData.Labels[Index].Color.G;
                Label_Blue.Value = PassiveTree.SelectedData.Labels[Index].Color.B;
                Label_Opacity.Value = PassiveTree.SelectedData.Labels[Index].Color.A;
                Label_Alpha.Value = (decimal)PassiveTree.SelectedData.Labels[Index].Alpha;
                Label_Scale.Value = (decimal)PassiveTree.SelectedData.Labels[Index].Scale;
                Label_Text.Text = PassiveTree.SelectedData.Labels[Index].Text;
                Label_Property.Visible = true;
            } else if (Type == SelectType.Skill) {
                Skill_ImageName.Text = PassiveTree.SelectedData.Skills[Index].ImageName;
                Skill_Size.Text = PassiveTree.SelectedData.Skills[Index].Size.ToString();
                Skill_ConnectedSkill.Text = String.Join(",", PassiveTree.SelectedData.Skills[Index].ConnectedSkills); 
                Skill_Caption.Text = PassiveTree.SelectedData.Skills[Index].Caption;
                Skill_Description.Text = PassiveTree.SelectedData.Skills[Index].Description;
                Skill_Legend.Text = PassiveTree.SelectedData.Skills[Index].Legend;

                Label_ConnectedSkill.Text = Index.ToString() + " Connected Skills";
                //Property
                Skill_PropertyIndex.Items.Clear();
                for (int i = 0; i < PassiveTree.SelectedData.Skills[Index].Properties.Length; i++) {
                    Skill_PropertyIndex.Items.Add(PassiveTree.SelectedData.Skills[Index].Properties[i].Type.ToString()); 
                }
                if (Skill_PropertyIndex.Items.Count > 0) { 
                    Skill_PropertyIndex.SelectedIndex = 0;
                    Skill_PropertyType.Visible = true;
                    Skill_PropertyValue.Visible = true;
                } else {
                    Skill_PropertyType.Visible = false;
                    Skill_PropertyValue.Visible = false;
                }

                Skill_Property.Visible = true;
            } else if (Type == SelectType.Line) {
                Line_SkillA.Value = PassiveTree.SelectedData.Lines[Index].SkillA;
                Line_SkillB.Value = PassiveTree.SelectedData.Lines[Index].SkillB;
                Line_Style.Text = PassiveTree.SelectedData.Lines[Index].Style.ToString() ;
                Line_Width.Value = PassiveTree.SelectedData.Lines[Index].Width;
                Line_Alpha.Value = (decimal)PassiveTree.SelectedData.Lines[Index].Alpha;
                Line_Property.Visible = true;
            } else if (Type == SelectType.Background) {
                Background_ImageName.Text = PassiveTree.SelectedData.Backgrounds[Index].ImageName;
                Background_Effect.Text = PassiveTree.SelectedData.Backgrounds[Index].Effect.ToString();
                Background_Property.Visible = true;
            } else {
                this.Main_Property.SelectedObject = PassiveTree;
                Main_Property.Visible = true;
            }
            
            IsGettingProperties = false;
        }
        private void PropertiesSet() {
            IsSettingProperties = true;
            if (Type == SelectType.Label) {
                PassiveTree.SelectedData.Labels[Index].Position.X = (int)Conversion.Val(Label_X.Text);
                PassiveTree.SelectedData.Labels[Index].Position.Y = (int)Conversion.Val(Label_Y.Text);
                PassiveTree.SelectedData.Labels[Index].Color.R = Convert.ToByte(Label_Red.Value);
                PassiveTree.SelectedData.Labels[Index].Color.G = Convert.ToByte(Label_Green.Value);
                PassiveTree.SelectedData.Labels[Index].Color.B = Convert.ToByte(Label_Blue.Value);
                PassiveTree.SelectedData.Labels[Index].Color.A = Convert.ToByte(Label_Opacity.Value);

                PassiveTree.SelectedData.Labels[Index].Alpha = (float)Label_Alpha.Value;
                PassiveTree.SelectedData.Labels[Index].Scale = (float)Label_Scale.Value;
                PassiveTree.SelectedData.Labels[Index].Text = Label_Text.Text;

            } else if (Type == SelectType.Skill) {
                PassiveTree.SelectedData.Skills[Index].ImageName = Skill_ImageName.Text;
                PassiveTree.SelectedData.Skills[Index].Size = (SkillSize)Enum.Parse(typeof(SkillSize), Skill_Size.Text);
                PassiveTree.SelectedData.Skills[Index].Caption = Skill_Caption.Text;
                try {
                    PassiveTree.SelectedData.Skills[Index].ConnectedSkills = StringToInts(Skill_ConnectedSkill.Text);
                    Skill_ConnectedSkill.ForeColor = System.Drawing.Color.Black;
                } catch {
                    Skill_ConnectedSkill.ForeColor = System.Drawing.Color.Red; 
                }
                PassiveTree.SelectedData.Skills[Index].Description = Skill_Description.Text;
                PassiveTree.SelectedData.Skills[Index].Legend = Skill_Legend.Text;
                //Property
                if (Skill_PropertyIndex.SelectedIndex != -1) {
                    if (Enum.IsDefined(typeof(TypeProperty), Skill_PropertyType.Text)) {
                        Skill_PropertyType.ForeColor = System.Drawing.Color.Black;
                        PassiveTree.SelectedData.Skills[Index].Properties[Skill_PropertyIndex.SelectedIndex].Type = (TypeProperty)Enum.Parse(typeof(TypeProperty), Skill_PropertyType.Text);
                        Skill_PropertyIndex.Items[Skill_PropertyIndex.SelectedIndex] = PassiveTree.SelectedData.Skills[Index].Properties[Skill_PropertyIndex.SelectedIndex].Type.ToString();
                    } else {
                        Skill_PropertyType.ForeColor = System.Drawing.Color.Red;
                    }
                    try {
                        PassiveTree.SelectedData.Skills[Index].Properties[Skill_PropertyIndex.SelectedIndex].Value = StringToFloats(Skill_PropertyValue.Text);
                    } catch { }

                    
                }
                PassiveTree.SelectedData.Skills[Index].Init(); CheckErrors();
            } else if(Type == SelectType.Line ){
                PassiveTree.SelectedData.Lines[Index].SkillA = (int)Line_SkillA.Value;
                PassiveTree.SelectedData.Lines[Index].SkillB = (int)Line_SkillB.Value;
                PassiveTree.SelectedData.Lines[Index].Style = (LineStyle)Enum.Parse(typeof(LineStyle), Line_Style.Text);
                PassiveTree.SelectedData.Lines[Index].Width = (int)Line_Width.Value;
                PassiveTree.SelectedData.Lines[Index].Alpha = (float)Line_Alpha.Value;
                PassiveTree.SelectedData.Lines[Index].Init(); CheckErrors();
            } else if (Type == SelectType.Background) {
                PassiveTree.SelectedData.Backgrounds[Index].ImageName = Background_ImageName.Text;
                PassiveTree.SelectedData.Backgrounds[Index].Effect = (SharpDX.Toolkit.Graphics.SpriteEffects)Enum.Parse(typeof(SharpDX.Toolkit.Graphics.SpriteEffects), Background_Effect.Text);
                PassiveTree.SelectedData.Backgrounds[Index].Init(); CheckErrors();
            }
            IsSettingProperties = false;
        }

        //Property Change
        private void Skill_Changed(object sender, EventArgs e) {
            if (sender == Skill_AddProperty) {
                Array.Resize(ref PassiveTree.SelectedData.Skills[Index].Properties, PassiveTree.SelectedData.Skills[Index].Properties.Length + 1);
                PassiveTree.SelectedData.Skills[Index].Properties[PassiveTree.SelectedData.Skills[Index].Properties.Length - 1].Value = new float[1];
                PropertiesGet();
            } else if (sender == Skill_DeleteProperty){
                if (PassiveTree.SelectedData.Skills[Index].Properties.Length > 0) {
                    Array.Resize(ref PassiveTree.SelectedData.Skills[Index].Properties, PassiveTree.SelectedData.Skills[Index].Properties.Length - 1);
                }
                PropertiesGet();
            }

            if (IsGettingProperties == false) { PropertiesSet(); }
        }
        private int[] StringToInts(string e) {
            if (e == "" || e == null) { return new int[0]; }
            e = Strings.Replace(e.Trim(), " ", ",");
            string[] s = Strings.Split(e, ",");
            int[] r = new int[s.Length];

            for (int i = 0; i < r.Length; i++) {
                if (s[i] != "") {
                    r[i] = Convert.ToInt32(s[i]);   
                }
             
            }

            return r;

        }
        private float[] StringToFloats(string e) {
            e = Strings.Replace(e, " ", ",");
            string[] s = Strings.Split(e, ",");
            float[] r = new float[s.Length];

            for (int i = 0; i < r.Length; i++) {
                if (s[i] != "") {
                    r[i] = Convert.ToSingle(s[i]);
                }
             
            }

            return r;

        }
        private float GetPointRotate(float pointX, float pointY, SharpDX.Vector2 origin) {
            if (pointX >= origin.X) {
                return (float)Math.Atan((pointY - origin.Y) / (pointX - origin.X));
            } else {
                return (float)(Math.Atan((pointY - origin.Y) / (pointX - origin.X)) - Math.PI);
            }
        }

        SelectType Type; int Index; object o;

        private void Update2() {
            if (Type == SelectType.Skill && e.Keyboard.IsKeyDown(SharpDX.Toolkit.Input.Keys.LeftShift) == true) {
                e.Form.Cursor = System.Windows.Forms.Cursors.Cross;
            } else {
                e.Form.Cursor = System.Windows.Forms.Cursors.Default;
            }

            if (e.Mouse.LeftButton.Pressed == true) {
                if (e.Keyboard.IsKeyDown(SharpDX.Toolkit.Input.Keys.LeftShift) == true && Type == SelectType.Skill) {
                    int i = PassiveTree.HitTest(e.X, e.Y);
                    if (i != -1) {
                        int pos_skill = Array.IndexOf<int>(PassiveTree.SelectedData.Skills[Index].ConnectedSkills, i);
                        if (pos_skill == -1) { Skill_ConnectedSkill.Text += " " + i.ToString(); }
                    }
                } else {
                    SelectElement();
                    if (o != null) {
                        Delta_Vector = (SharpDX.Vector2)(o.GetType().GetField("Position").GetValue(o)) * PassiveTree.Scale - e.Mouse_PositionF;
                        Old_Postion = (SharpDX.Vector2)(o.GetType().GetField("Position").GetValue(o));
                    }
                }
            }

            if (e.Mouse.LeftButton.Down == true) {
                if (e.Keyboard.IsKeyDown(SharpDX.Toolkit.Input.Keys.LeftControl) == true && Type != SelectType.None) {
                    if (Type == SelectType.Skill) {
                        PassiveTree.SelectedData.Skills[Index].Position = (e.Mouse_PositionF + Delta_Vector) / PassiveTree.Scale;
                    } else if (Type == SelectType.Line) {
                        PassiveTree.SelectedData.Lines[Index].Position = (e.Mouse_PositionF + Delta_Vector) / PassiveTree.Scale;
                    } else if (Type == SelectType.Background) {
                        PassiveTree.SelectedData.Backgrounds[Index].Position = (e.Mouse_PositionF + Delta_Vector) / PassiveTree.Scale;
                    } else if(Type == SelectType.Label ) {
                        PassiveTree.SelectedData.Labels[Index].Position = (e.Mouse_PositionF + Delta_Vector) / PassiveTree.Scale;
                    }
                    Index_Equal_X = -1; Index_Equal_Y = -1; Index_Symmetry_X = -1; Index_Symmetry_Y = -1;
                    SetClosePosition(Type);
                    SetSymmetryPosition(Type);
                } else {
                    Index_Equal_X = -1; Index_Equal_Y = -1; Index_Symmetry_X = -1; Index_Symmetry_Y = -1;
                }
            }

            #region Line.Alpha
            if (e.Mouse.RightButton.Down == true) {
                if (e.Keyboard.IsKeyDown(SharpDX.Toolkit.Input.Keys.LeftControl) == true) { 
                    if (Type == SelectType.Line) {
                        if (PassiveTree.SelectedData.Lines[Index].Style == LineStyle.Line) {
                            PassiveTree.SelectedData.Lines[Index].Alpha = GetPointRotate(e.X, e.Y, (PassiveTree.SelectedData.Lines[Index].Position * PassiveTree.Scale - PassiveTree.Position));
                        } else {
                            PassiveTree.SelectedData.Lines[Index].Alpha = GetPointRotate(e.X, e.Y, (PassiveTree.SelectedData.Lines[Index].Position * PassiveTree.Scale - PassiveTree.Position)) + (float)(Math.PI / 4);
                        }

                        PropertiesGet();
                    } 
                }
            }
            #endregion
            #region Line A, B
            if (Type == SelectType.Line) {
                if (e.Keyboard.IsKeyPressed(SharpDX.Toolkit.Input.Keys.A) == true) {
                    int i = PassiveTree.HitTest(e.X, e.Y);
                    Line_SkillA.Value = (i != -1) ? i : 0;
                    //PassiveTree.SelectedData.Lines[Index].SkillA = (i != -1) ? i : 0;
                } else if (e.Keyboard.IsKeyPressed(SharpDX.Toolkit.Input.Keys.B) == true) {
                    int i = PassiveTree.HitTest(e.X, e.Y); Line_SkillB.Value = (i != -1) ? i : 0;
                    //PassiveTree.SelectedData.Lines[Index].SkillB = (i != -1) ? i : 0;
                }
            }
            #endregion

            #region Copy
            if (Type != SelectType.None && e.Keyboard.IsKeyDown(SharpDX.Toolkit.Input.Keys.LeftControl) && e.Keyboard.IsKeyPressed(SharpDX.Toolkit.Input.Keys.C)) {
                if (System.IO.File.Exists(Application.StartupPath + @"\file.tmp") == true) { System.IO.File.Delete(Application.StartupPath + @"\file.tmp"); }
                FileSystem.FileOpen(33, Application.StartupPath + @"\file.tmp", OpenMode.Binary);
                if (Type == SelectType.Background) {
                    FileSystem.FilePut(33, PassiveTree.SelectedData.Backgrounds[Index]);
                    Clipboard.SetData("Editor/Background", Application.StartupPath + @"\file.tmp");
                } else if (Type == SelectType.Line) {
                    FileSystem.FilePut(33, PassiveTree.SelectedData.Lines[Index]);
                    Clipboard.SetData("Editor/Line", Application.StartupPath + @"\file.tmp");
                } else if (Type == SelectType.Label) {
                    FileSystem.FilePut(33, PassiveTree.SelectedData.Labels[Index]);
                    Clipboard.SetData("Editor/Label", Application.StartupPath + @"\file.tmp");
                } else if (Type == SelectType.Skill) { 
                    FileSystem.FilePut(33, PassiveTree.SelectedData.Skills[Index]); 
                    Clipboard.SetData("Editor/Skill", Application.StartupPath + @"\file.tmp");
                }
                FileSystem.FileClose(33);
            }
            #endregion
            #region Paste
            if (e.Keyboard.IsKeyDown(SharpDX.Toolkit.Input.Keys.LeftControl) && e.Keyboard.IsKeyPressed(SharpDX.Toolkit.Input.Keys.V)) {
                string FileCopy = ""; SelectType TypeCopy = SelectType.None; ValueType TMP = null;

                if (Clipboard.ContainsData("Editor/Background") == true) { FileCopy = (string)Clipboard.GetData("Editor/Background"); TypeCopy = SelectType.Background; }
                else if (Clipboard.ContainsData("Editor/Line") == true) { FileCopy = (string)Clipboard.GetData("Editor/Line"); TypeCopy = SelectType.Line; }
                else if (Clipboard.ContainsData("Editor/Label") == true) { FileCopy = (string)Clipboard.GetData("Editor/Label"); TypeCopy = SelectType.Label; }
                else if (Clipboard.ContainsData("Editor/Skill") == true) { FileCopy = (string)Clipboard.GetData("Editor/Skill"); TypeCopy = SelectType.Skill; }

                if (FileCopy != "") { 
                    FileSystem.FileOpen(3, FileCopy, OpenMode.Binary);
                    if (TypeCopy == SelectType.Background) { 
                        TMP = new Background();
                        FileSystem.FileGet(3, ref TMP);
                        Background background = (Background)TMP;
                        background.Position = PassiveTree.GetCenterPoint();
                        background.Init();
                        PassiveTree.AddBackground(background);
                    } else if (TypeCopy == SelectType.Line) { 
                        TMP = new Line();
                        FileSystem.FileGet(3, ref TMP);
                        Line line = (Line)TMP;
                        line.Position = PassiveTree.GetCenterPoint();
                        line.Init();
                        PassiveTree.AddLine(line);
                    } else if (TypeCopy == SelectType.Label) { 
                        TMP = new Label();
                        FileSystem.FileGet(3, ref TMP);
                        Label label = (Label)TMP;
                        label.Position = PassiveTree.GetCenterPoint();
                        PassiveTree.AddLabel(label);
                    } else if (TypeCopy == SelectType.Skill) { 
                        TMP = new PassiveSkill();
                        FileSystem.FileGet(3, ref TMP);
                        PassiveSkill skill = (PassiveSkill)TMP;
                        skill.Position = PassiveTree.GetCenterPoint();
                        skill.Init();
                        PassiveTree.AddSkill(skill);
                    }
                    FileSystem.FileClose(3);
                }
            }
            #endregion
            #region Delete 
            if (e.Keyboard.IsKeyPressed(SharpDX.Toolkit.Input.Keys.Delete)) { Outline_Buttons(Button_Delete, null); }
            #endregion
        }
        private static float Size_Fize_Position = 10;

        int Index_Equal_X = -1, Index_Equal_Y = -1;
        int Index_Symmetry_X = -1, Index_Symmetry_Y = -1;
        private void SetClosePosition(SelectType select) { 
            int Length = 0;
            float Width, Height, oX, oY;
            SharpDX.Vector2 Current_Position = new SharpDX.Vector2();
            SharpDX.Vector2 New_Position = new SharpDX.Vector2();

            if (select == SelectType.Skill) { Length = PassiveTree.SelectedData.Skills.Length; Current_Position = PassiveTree.SelectedData.Skills[Index].Position; }
            else if (select == SelectType.Background) { Length = PassiveTree.SelectedData.Backgrounds.Length; Current_Position = PassiveTree.SelectedData.Backgrounds[Index].Position; }
            else if (select == SelectType.Line) { Length = PassiveTree.SelectedData.Lines.Length; Current_Position = PassiveTree.SelectedData.Lines[Index].Position; }
            else if (select == SelectType.Label) { Length = PassiveTree.SelectedData.Labels.Length; Current_Position = PassiveTree.SelectedData.Labels[Index].Position; }

            if (Check_Equal_X.Checked == true) {
                Width = Size_Fize_Position / PassiveTree.Scale; 
                Height = Size_Fize_Position / PassiveTree.Scale;
                //Index_Equal_X = -1;

                for (int i = 0; i < Length; i++) {
                    if (select == SelectType.Skill) { New_Position = PassiveTree.SelectedData.Skills[i].Position; }
                    else if (select == SelectType.Background) { New_Position = PassiveTree.SelectedData.Backgrounds[i].Position; }
                    else if (select == SelectType.Line) { New_Position = PassiveTree.SelectedData.Lines[i].Position; }
                    else if (select == SelectType.Label) { New_Position = PassiveTree.SelectedData.Labels[i].Position; }
                    oX = Math.Abs(New_Position.X - Current_Position.X);

                    if (oX <= Width && i != Index) { Width = oX; Index_Equal_X = i; }
                }

                if (Index_Equal_X != -1) {
                    if (select == SelectType.Skill) { PassiveTree.SelectedData.Skills[Index].Position.X = PassiveTree.SelectedData.Skills[Index_Equal_X].Position.X; }
                    else if (select == SelectType.Background) { PassiveTree.SelectedData.Backgrounds[Index].Position.X = PassiveTree.SelectedData.Backgrounds[Index_Equal_X].Position.X; }
                    else if (select == SelectType.Line) { PassiveTree.SelectedData.Lines[Index].Position.X = PassiveTree.SelectedData.Lines[Index_Equal_X].Position.X; }
                    else if (select == SelectType.Label) { PassiveTree.SelectedData.Labels[Index].Position.X = PassiveTree.SelectedData.Labels[Index_Equal_X].Position.X; }
                }
            }
    
            if (Check_Equal_Y.Checked == true) {
                Width = Size_Fize_Position / PassiveTree.Scale; 
                Height = Size_Fize_Position / PassiveTree.Scale;                

                for (int i = 0; i < Length; i++) {
                    if (select == SelectType.Skill) { New_Position = PassiveTree.SelectedData.Skills[i].Position; }
                    else if (select == SelectType.Background) { New_Position = PassiveTree.SelectedData.Backgrounds[i].Position; }
                    else if (select == SelectType.Line) { New_Position = PassiveTree.SelectedData.Lines[i].Position; }
                    else if (select == SelectType.Label) { New_Position = PassiveTree.SelectedData.Labels[i].Position; }

                    oY = Math.Abs(New_Position.Y - Current_Position.Y);

                    if (oY <= Height && i != Index) { Height = oY; Index_Equal_Y = i; }
                }

                if (Index_Equal_Y != -1) {
                    if (select == SelectType.Skill) { PassiveTree.SelectedData.Skills[Index].Position.Y = PassiveTree.SelectedData.Skills[Index_Equal_Y].Position.Y; }
                    else if (select == SelectType.Background) { PassiveTree.SelectedData.Backgrounds[Index].Position.Y = PassiveTree.SelectedData.Backgrounds[Index_Equal_Y].Position.Y; }
                    else if (select == SelectType.Line) { PassiveTree.SelectedData.Lines[Index].Position.Y = PassiveTree.SelectedData.Lines[Index_Equal_Y].Position.Y; }
                    else if (select == SelectType.Label) { PassiveTree.SelectedData.Labels[Index].Position.Y = PassiveTree.SelectedData.Labels[Index_Equal_Y].Position.Y; }
                }
            }
        }
        private void SetSymmetryPosition(SelectType select) {
            int Length = 0;
            float Width, Height, oX, oY;
            SharpDX.Vector2 Current_Position = new SharpDX.Vector2();
            SharpDX.Vector2 New_Position = new SharpDX.Vector2();

            if (select == SelectType.Skill) { Length = PassiveTree.SelectedData.Skills.Length; Current_Position = PassiveTree.SelectedData.Skills[Index].Position; }
            else if (select == SelectType.Background) { Length = PassiveTree.SelectedData.Backgrounds.Length; Current_Position = PassiveTree.SelectedData.Backgrounds[Index].Position; }
            else if (select == SelectType.Line) { Length = PassiveTree.SelectedData.Lines.Length; Current_Position = PassiveTree.SelectedData.Lines[Index].Position; }
            else if (select == SelectType.Label) { Length = PassiveTree.SelectedData.Labels.Length; Current_Position = PassiveTree.SelectedData.Labels[Index].Position; }

            if (Check_Symmetry_X.Checked == true) {
                Width = Size_Fize_Position / PassiveTree.Scale; 
                Height = Size_Fize_Position / PassiveTree.Scale;

                for (int i = 0; i < Length; i++) {
                    if (select == SelectType.Skill) { New_Position = PassiveTree.SelectedData.Skills[i].Position; }
                    else if (select == SelectType.Background) { New_Position = PassiveTree.SelectedData.Backgrounds[i].Position; }
                    else if (select == SelectType.Line) { New_Position = PassiveTree.SelectedData.Lines[i].Position; }
                    else if (select == SelectType.Label) { New_Position = PassiveTree.SelectedData.Labels[i].Position; }

                    oX = Math.Abs(New_Position.X + Current_Position.X);
                    oY = Math.Abs(New_Position.Y - Current_Position.Y);

                    if (oX <= Width && oY <= Height && i != Index) { Width = oX; Height = oY; Index_Symmetry_X = i; }
                }

                if (Index_Symmetry_X != -1) {
                    if (select == SelectType.Skill) { PassiveTree.SelectedData.Skills[Index].Position.X = -PassiveTree.SelectedData.Skills[Index_Symmetry_X].Position.X; }
                    else if (select == SelectType.Background) { PassiveTree.SelectedData.Backgrounds[Index].Position.X = -PassiveTree.SelectedData.Backgrounds[Index_Symmetry_X].Position.X; }
                    else if (select == SelectType.Line) { PassiveTree.SelectedData.Lines[Index].Position.X = -PassiveTree.SelectedData.Lines[Index_Symmetry_X].Position.X; }
                    else if (select == SelectType.Label) { PassiveTree.SelectedData.Labels[Index].Position.X = -PassiveTree.SelectedData.Labels[Index_Symmetry_X].Position.X; }
                }
            }
    
            if (Check_Symmetry_Y.Checked == true) {
                Width = Size_Fize_Position / PassiveTree.Scale; 
                Height = Size_Fize_Position / PassiveTree.Scale;

                for (int i = 0; i < Length; i++) {
                    if (select == SelectType.Skill) { New_Position = PassiveTree.SelectedData.Skills[i].Position; }
                    else if (select == SelectType.Background) { New_Position = PassiveTree.SelectedData.Backgrounds[i].Position; }
                    else if (select == SelectType.Line) { New_Position = PassiveTree.SelectedData.Lines[i].Position; }
                    else if (select == SelectType.Label) { New_Position = PassiveTree.SelectedData.Labels[i].Position; }

                    oX = Math.Abs(New_Position.X - Current_Position.X);
                    oY = Math.Abs(New_Position.Y + Current_Position.Y);

                    if (oX <= Width && oY <= Height && i != Index) { Width = oX; Height = oY; Index_Symmetry_Y = i; }
                }

                if (Index_Symmetry_Y != -1) {
                    if (select == SelectType.Skill) { PassiveTree.SelectedData.Skills[Index].Position.Y = -PassiveTree.SelectedData.Skills[Index_Symmetry_Y].Position.Y; }
                    else if (select == SelectType.Background) { PassiveTree.SelectedData.Backgrounds[Index].Position.Y = -PassiveTree.SelectedData.Backgrounds[Index_Symmetry_Y].Position.Y; }
                    else if (select == SelectType.Line) { PassiveTree.SelectedData.Lines[Index].Position.Y = -PassiveTree.SelectedData.Lines[Index_Symmetry_Y].Position.Y; }
                    else if (select == SelectType.Label) { PassiveTree.SelectedData.Labels[Index].Position.Y = -PassiveTree.SelectedData.Labels[Index_Symmetry_Y].Position.Y; }
                }
            }
        }

        private void DoPaste() {
            FileSystem.FileOpen(3, Application.StartupPath + @"\file.tmp", OpenMode.Binary);
            //FileSystem.FileGet(3,
            FileSystem.FileClose(3);
        }
        private void SelectElement() {
            me.Focus();

            if (Check_Label.Checked == true) {
                Index = PassiveTree.HitLabels(e.X, e.Y);
                if (Index != -1) { Type = SelectType.Label; o = PassiveTree.SelectedData.Labels[Index]; PropertiesGet(); RefreshOutline(); return; }
            }
            if (Check_Skill.Checked == true) { 
                Index = PassiveTree.HitTest(e.X, e.Y);
                if (Index != -1) { Type = SelectType.Skill; o = PassiveTree.SelectedData.Skills[Index]; PropertiesGet(); RefreshOutline(); return; }
            }
            if (Check_Line.Checked == true) {
                Index = PassiveTree.HitTestLine(e.X, e.Y);
                if (Index != -1) { Type = SelectType.Line; o = PassiveTree.SelectedData.Lines[Index]; PropertiesGet(); RefreshOutline(); return; } 
            }
            if (Check_Background.Checked == true) {
                Index = PassiveTree.HitTestBackground(e.X, e.Y);
                if (Index != -1) { Type = SelectType.Background; o = PassiveTree.SelectedData.Backgrounds[Index]; PropertiesGet(); RefreshOutline(); return; }
            }

            Type = SelectType.None; o = null; PropertiesGet(); RefreshOutline(); me.Focus();
        }

        private PassiveTreeItems SelectElements(SharpDX.Rectangle Rect) {
            PassiveTreeItems Return = new PassiveTreeItems();
            for (int i = 0; i < PassiveTree.SelectedData.Skills.Length; i++) { 

                

            }


            return new PassiveTreeItems();
        }

        private struct PassiveTreeItems {
            PassiveSkill[] Skills;
            Background[] Backgrounds;
            Line[] Lines;
            Label[] Labels;
            public PassiveTreeItems(int skills, int backgrounds, int lines, int labels) {
                Skills = new PassiveSkill[skills];
                Backgrounds = new Background[backgrounds];
                Lines = new Line[lines];
                Labels = new Label[labels];
            }
        }

        private void PassiveTree_ItemMouseClick(object sender, MouseEventArgs e) {
            float Int = 0, Str = 0, Agi = 0;
            
            for (int i = 1; i < PassiveTree.SelectedData.Skills.Length; i++) {
                if (PassiveTree.SelectedData.Skills[i].Status == SkillStatus.Allocated) {
                    for (int j = 0; j < PassiveTree.SelectedData.Skills[i].Properties.Length; j++) {
                        if (PassiveTree.SelectedData.Skills[i].Properties[j].Type == TypeProperty.Intelligence) {
                            Int += PassiveTree.SelectedData.Skills[i].Properties[j].Value[0];
                        } else if (PassiveTree.SelectedData.Skills[i].Properties[j].Type == TypeProperty.Strength) {
                            Str += PassiveTree.SelectedData.Skills[i].Properties[j].Value[0];
                        } else if (PassiveTree.SelectedData.Skills[i].Properties[j].Type == TypeProperty.Dexterity) {
                            Agi += PassiveTree.SelectedData.Skills[i].Properties[j].Value[0];
                        } else if (PassiveTree.SelectedData.Skills[i].Properties[j].Type == TypeProperty.Attributes) {
                            Int += PassiveTree.SelectedData.Skills[i].Properties[j].Value[0];
                            Str += PassiveTree.SelectedData.Skills[i].Properties[j].Value[0];
                            Agi += PassiveTree.SelectedData.Skills[i].Properties[j].Value[0];
                        }
                    }
                }
            }
     
            if (PassiveTree.SelectedData.Labels.Length > 0) { PassiveTree.SelectedData.Labels[0].Text = Math.Round(Int).ToString(); }
            if (PassiveTree.SelectedData.Labels.Length > 1) { PassiveTree.SelectedData.Labels[1].Text = Math.Round(Str).ToString(); }
            if (PassiveTree.SelectedData.Labels.Length > 2) { PassiveTree.SelectedData.Labels[2].Text = Math.Round(Agi).ToString(); }
        }


        //Outline
        TreeNode Node_Skills; TreeNode Node_Lines; TreeNode Node_Backgrounds; TreeNode Node_Labels;
        private void ClearAndAddOutline() {
            Node_Skills.Nodes.Clear(); Node_Lines.Nodes.Clear(); Node_Backgrounds.Nodes.Clear(); Node_Labels.Nodes.Clear();
            for (int i = 0; i < PassiveTree.SelectedData.Skills.Length; i++) {
                Node_Skills.Nodes.Add(i.ToString() + " " + PassiveTree.SelectedData.Skills[i].Caption);
            }
            for (int i = 0; i < PassiveTree.SelectedData.Lines.Length; i++) {
                Node_Lines.Nodes.Add(i.ToString());
            }
            for (int i = 0; i < PassiveTree.SelectedData.Backgrounds.Length; i++) {
                Node_Backgrounds.Nodes.Add(i.ToString() + " " + PassiveTree.SelectedData.Backgrounds[i].ImageName);
            }
            for (int i = 0; i < PassiveTree.SelectedData.Labels.Length; i++) {
                Node_Labels.Nodes.Add(i.ToString() + " " + PassiveTree.SelectedData.Labels[i].Text);
            }
        }
        private void RefreshOutline() {
            if (Outline_Panel.Visible == false) { return; }
            Outline_TreeView.BeginUpdate();
            
            //Refresh and Add
            for (int i = 0; i < PassiveTree.SelectedData.Skills.Length; i++) {
                if (i <Node_Skills.Nodes.Count) {
                    Node_Skills.Nodes[i].Text = i.ToString() + " " + PassiveTree.SelectedData.Skills[i].Caption;
                } else {
                    Node_Skills.Nodes.Add(i.ToString() + " " + PassiveTree.SelectedData.Skills[i].Caption);
                }
            }

            for (int i = 0; i < PassiveTree.SelectedData.Lines.Length; i++) {
                if (i < Node_Lines.Nodes.Count) {
                    Node_Lines.Nodes[i].Text = i.ToString();
                } else {
                    Node_Lines.Nodes.Add(i.ToString() );
                }
            }
            for (int i = 0; i < PassiveTree.SelectedData.Backgrounds.Length; i++) {
                if (i < Node_Backgrounds.Nodes.Count) {
                    Node_Backgrounds.Nodes[i].Text = i.ToString() + " " + PassiveTree.SelectedData.Backgrounds[i].ImageName;
                } else {
                    Node_Backgrounds.Nodes.Add(i.ToString() + " " + PassiveTree.SelectedData.Backgrounds[i].ImageName);
                }
            }
            for (int i = 0; i < PassiveTree.SelectedData.Labels.Length; i++) {
                if (i < Node_Labels.Nodes.Count) {
                    Node_Labels.Nodes[i].Text = i.ToString() + " " + PassiveTree.SelectedData.Labels[i].Text;
                } else {
                    Node_Labels.Nodes.Add(i.ToString() + " " + PassiveTree.SelectedData.Labels[i].Text);
                }
            }

            //Delete if need
            for (int i = PassiveTree.SelectedData.Skills.Length; i < Node_Skills.Nodes.Count; i++) {
                Node_Skills.Nodes.RemoveAt(i); i--;
            }
            for (int i = PassiveTree.SelectedData.Lines.Length; i < Node_Lines.Nodes.Count; i++) {
                Node_Lines.Nodes.RemoveAt(i); i--;
            }
            for (int i = PassiveTree.SelectedData.Backgrounds.Length; i < Node_Backgrounds.Nodes.Count; i++) {
                Node_Backgrounds.Nodes.RemoveAt(i); i--;
            }
            for (int i = PassiveTree.SelectedData.Labels.Length; i < Node_Labels.Nodes.Count; i++) {
                Node_Labels.Nodes.RemoveAt(i); i--;
            }

            //Select
            if (Index > 0) { Button_Up.Enabled = true; } else { Button_Up.Enabled = false; }
            Button_Delete.Enabled = true;
            if (Type == SelectType.Skill) {
                Node_Skills.Nodes[Index].EnsureVisible();
                Outline_TreeView.SelectedNode = Node_Skills.Nodes[Index];
                if (Index > 1) { Button_Up.Enabled = true; } else { Button_Up.Enabled = false; }
                if (Index < PassiveTree.SelectedData.Skills.Length - 1) { Button_Down.Enabled = true; } else { Button_Down.Enabled = false; }
            } else if( Type == SelectType.Line) {
                Node_Lines.Nodes[Index].EnsureVisible();
                Outline_TreeView.SelectedNode = Node_Lines.Nodes[Index];
                if (Index < PassiveTree.SelectedData.Lines.Length - 1) { Button_Down.Enabled = true; } else { Button_Down.Enabled = false; }
            } else if (Type == SelectType.Background) {
                Node_Backgrounds.Nodes[Index].EnsureVisible();
                Outline_TreeView.SelectedNode = Node_Backgrounds.Nodes[Index];
                if (Index < PassiveTree.SelectedData.Backgrounds.Length - 1) { Button_Down.Enabled = true; } else { Button_Down.Enabled = false; }
            } else if (Type == SelectType.Label) {
                Node_Labels.Nodes[Index].EnsureVisible();
                Outline_TreeView.SelectedNode = Node_Labels.Nodes[Index];
                if (Index < PassiveTree.SelectedData.Labels.Length - 1) { Button_Down.Enabled = true; } else { Button_Down.Enabled = false; }
            } else {
                Button_Up.Enabled = false; Button_Down.Enabled = false; Button_Delete.Enabled = false;
                Outline_TreeView.SelectedNode = null;
            }
            CheckErrors();
            Outline_TreeView.EndUpdate();
        }
        private void Outline_AfterSelect(object sender, TreeViewEventArgs e) {
            if (e.Action ==  TreeViewAction.ByMouse && e.Node != Node_Skills && e.Node != Node_Lines && e.Node != Node_Backgrounds) {
                if (e.Node.Parent == Node_Skills) {
                    if (e.Node.Index != 0) {
                        Type = SelectType.Skill; Index = e.Node.Index; PropertiesGet();
                        PassiveTree.ViewAsCenter(PassiveTree.SelectedData.Skills[Index].Position);
                    }
                } else if (e.Node.Parent == Node_Lines) {
                    Type = SelectType.Line ; Index = e.Node.Index; PropertiesGet();
                    PassiveTree.ViewAsCenter(PassiveTree.SelectedData.Lines[Index].Position);
                } else if (e.Node.Parent == Node_Backgrounds) {
                    Type = SelectType.Background; Index = e.Node.Index; PropertiesGet();
                    PassiveTree.ViewAsCenter(PassiveTree.SelectedData.Backgrounds[Index].Position);
                } else if (e.Node.Parent == Node_Labels) {
                    Type = SelectType.Label; Index = e.Node.Index; PropertiesGet();
                    PassiveTree.ViewAsCenter(PassiveTree.SelectedData.Labels[Index].Position);
                }
                RefreshOutline();
            }
        }
        private void Outline_Buttons(object sender, EventArgs e) {
            if (sender == Button_Up) {
                if (Type == SelectType.Skill) {
                    PassiveSkill tmp = PassiveTree.SelectedData.Skills[Index];
                    PassiveTree.SelectedData.Skills[Index] = PassiveTree.SelectedData.Skills[Index - 1];
                    PassiveTree.SelectedData.Skills[Index - 1] = tmp;
                } else if (Type == SelectType.Line) {
                    Line tmp = PassiveTree.SelectedData.Lines[Index];
                    PassiveTree.SelectedData.Lines[Index] = PassiveTree.SelectedData.Lines[Index - 1];
                    PassiveTree.SelectedData.Lines[Index - 1] = tmp;
                } else if (Type == SelectType.Label) {
                    Label tmp = PassiveTree.SelectedData.Labels[Index];
                    PassiveTree.SelectedData.Labels[Index] = PassiveTree.SelectedData.Labels[Index - 1];
                    PassiveTree.SelectedData.Labels[Index - 1] = tmp;
                } else if (Type == SelectType.Background) {
                    Background tmp = PassiveTree.SelectedData.Backgrounds[Index];
                    PassiveTree.SelectedData.Backgrounds[Index] = PassiveTree.SelectedData.Backgrounds[Index - 1];
                    PassiveTree.SelectedData.Backgrounds[Index - 1] = tmp;
                }
                Outline_TreeView.SelectedNode = Outline_TreeView.SelectedNode.Parent.Nodes[Outline_TreeView.SelectedNode.Index - 1];
                Index = Index - 1;
                RefreshOutline();
            } else if (sender == Button_Down) { 
                if (Type == SelectType.Skill) {
                    PassiveSkill tmp = PassiveTree.SelectedData.Skills[Index];
                    PassiveTree.SelectedData.Skills[Index] = PassiveTree.SelectedData.Skills[Index + 1];
                    PassiveTree.SelectedData.Skills[Index + 1] = tmp;
                } else if (Type == SelectType.Line) {
                    Line tmp = PassiveTree.SelectedData.Lines[Index];
                    PassiveTree.SelectedData.Lines[Index] = PassiveTree.SelectedData.Lines[Index + 1];
                    PassiveTree.SelectedData.Lines[Index + 1] = tmp;
                } else if (Type == SelectType.Label) {
                    Label tmp = PassiveTree.SelectedData.Labels[Index];
                    PassiveTree.SelectedData.Labels[Index] = PassiveTree.SelectedData.Labels[Index + 1];
                    PassiveTree.SelectedData.Labels[Index + 1] = tmp;
                } else if (Type == SelectType.Background) {
                    Background tmp = PassiveTree.SelectedData.Backgrounds[Index];
                    PassiveTree.SelectedData.Backgrounds[Index] = PassiveTree.SelectedData.Backgrounds[Index + 1];
                    PassiveTree.SelectedData.Backgrounds[Index + 1] = tmp;
                }
                Outline_TreeView.SelectedNode = Outline_TreeView.SelectedNode.Parent.Nodes[Outline_TreeView.SelectedNode.Index + 1];
                Index = Index + 1;
                RefreshOutline();
            } else if (sender == Button_Delete) { 
                if (Type == SelectType.Skill) {
                    Array.Copy(PassiveTree.SelectedData.Skills, Index + 1, PassiveTree.SelectedData.Skills, Index, PassiveTree.SelectedData.Skills.Length - Index - 1);
                    Array.Resize(ref PassiveTree.SelectedData.Skills, PassiveTree.SelectedData.Skills.Length - 1);
                } else if (Type == SelectType.Line) {
                    Array.Copy(PassiveTree.SelectedData.Lines, Index + 1, PassiveTree.SelectedData.Lines, Index, PassiveTree.SelectedData.Lines.Length - Index - 1);
                    Array.Resize(ref PassiveTree.SelectedData.Lines, PassiveTree.SelectedData.Lines.Length - 1);
                } else if (Type == SelectType.Label) {
                    Array.Copy(PassiveTree.SelectedData.Labels, Index + 1, PassiveTree.SelectedData.Labels, Index, PassiveTree.SelectedData.Labels.Length - Index - 1);
                    Array.Resize(ref PassiveTree.SelectedData.Labels, PassiveTree.SelectedData.Labels.Length - 1);
                } else if (Type == SelectType.Background) {
                    Array.Copy(PassiveTree.SelectedData.Backgrounds, Index + 1, PassiveTree.SelectedData.Backgrounds, Index, PassiveTree.SelectedData.Backgrounds.Length - Index - 1);
                    Array.Resize(ref PassiveTree.SelectedData.Backgrounds, PassiveTree.SelectedData.Backgrounds.Length - 1);
                }
                PassiveTree.Hover = -1;
                Type = SelectType.None; Index = -1; RefreshOutline(); PropertiesGet();
            }
            CheckErrors();
        }
        //Errors
        private void CheckErrors() {
            string s = "";
            int Errors = PassiveTree.RefreshEditor(ref s);

            Errors_TextBox.Text = Errors + " Errors:" + System.Environment.NewLine + s;

        }
    }
}

