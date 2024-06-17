//using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Graphics;
using Microsoft.VisualBasic;
using System;
using System.Runtime.InteropServices;

namespace Path_of_Defender {
    public partial class Main_Form {
        [DllImport("uxtheme", EntryPoint = "SetWindowTheme", CharSet = CharSet.Unicode)] private static extern int SetWindowTheme(int hWnd, string pszSubAppName, string pszSubIdList);
        [DllImport("user32", EntryPoint = "PostMessageW")] private static extern int PostMessageW(int hWnd, int wMsg, int wParam, int lParam);
        GraphicsDeviceManager deviceManager;

        public Main_Form() {
            deviceManager = new GraphicsDeviceManager(this);
            deviceManager.PreferredBackBufferWidth = GameSetting.Width;
            deviceManager.PreferredBackBufferHeight = GameSetting.Height;
            IsFixedTimeStep = true;
        }
        protected override void Initialize() {
            this.Window.IsMouseVisible = true;
            e.Create(this);
            //if (System.Diagnostics.Debugger.IsAttached == true) {
            //    Images.Load(this.GraphicsDevice, e.SpriteBatch, @"E:\My Life\Projects\C# - Path of Defender\Effects\Graphics\");
            //    Fonts.Load(this.GraphicsDevice, @"E:\My Life\Projects\C# - Path of Defender\Effects\Graphics\");
            //} else {
                Images.Load(GraphicsDevice, e.SpriteBatch, System.Windows.Forms.Application.StartupPath + @"\Graphics");
                Fonts.Load(GraphicsDevice, System.Windows.Forms.Application.StartupPath + @"\Graphics");
            //}
            

            InitializeControls();
            InitializeVirtualControls();
            base.Initialize();
        }
    }

    public partial class Main_Form {
        Inventory Main_Inventory;
        VirtualControlCollection Controls;

        ///<summary> Initialize Virtual Controls </summary>
        private void InitializeVirtualControls() {
            Main_Inventory = new Inventory(8, 15);
            Main_Inventory.X = 600; Main_Inventory.Y = 65;
            
            Controls = new VirtualControlCollection(e.Form);
            Controls.Add(Main_Inventory);
        }    
    }

    public partial class Main_Form { 
        #region Window Form Controls
        Form Me;
        #region ToolStrip
        ToolStrip Main_ToolStrip = new ToolStrip();
        ToolStripButton Button_New = new ToolStripButton("Create a new System Item", Properties.Resources.New) { DisplayStyle = ToolStripItemDisplayStyle.Image, ImageTransparentColor = System.Drawing.Color.Magenta };
        ToolStripButton Buton_Open = new ToolStripButton("Open a System Item file", Properties.Resources.Open) { DisplayStyle = ToolStripItemDisplayStyle.Image };
        ToolStripButton Button_Save = new ToolStripButton("Save", Properties.Resources.Save);
        ToolStripSeparator Separator1 = new ToolStripSeparator();
        ToolStripButton Button_Create_Weapon_Bow = new ToolStripButton("Create a random Bow", Properties.Resources.Bow) { DisplayStyle = ToolStripItemDisplayStyle.Image, Enabled = false };
        ToolStripButton Button_Create_Weapon_Dagger = new ToolStripButton("Create a random Dagger", Properties.Resources.Dagger) { DisplayStyle = ToolStripItemDisplayStyle.Image, Enabled = false };
        ToolStripButton Button_Create_Weapon_Wand = new ToolStripButton("Create a random Wand", Properties.Resources.Wand) { DisplayStyle = ToolStripItemDisplayStyle.Image, Enabled = false };

        ToolStripSeparator Separator2 = new ToolStripSeparator();
        ToolStripButton Button_Create_Helmet = new ToolStripButton("Create a simplest Helmet", Properties.Resources.Helmet) { DisplayStyle = ToolStripItemDisplayStyle.Image, Enabled = false };
        ToolStripButton Button_Create_Body_Armor = new ToolStripButton("Create a simplest Body Armor", Properties.Resources.Armor) { DisplayStyle = ToolStripItemDisplayStyle.Image, Enabled = false };
        ToolStripButton Button_Create_Glove = new ToolStripButton("Create a simplest Glove", Properties.Resources.Glove) { DisplayStyle = ToolStripItemDisplayStyle.Image, Enabled = false };
        ToolStripButton Button_Create_Boot = new ToolStripButton("Create a simplest Boot", Properties.Resources.Boots) { DisplayStyle = ToolStripItemDisplayStyle.Image, Enabled = false };

        ToolStripSeparator Separator3 = new ToolStripSeparator();
        ToolStripButton Button_Create_Flask = new ToolStripButton("Create a random Flask", Properties.Resources.Flask) { DisplayStyle = ToolStripItemDisplayStyle.Image };
        ToolStripSeparator Separator4 = new ToolStripSeparator();
        ToolStripButton Button_Create_Random_Magic = new ToolStripButton("Test Magic", Properties.Resources.Random) { DisplayStyle = ToolStripItemDisplayStyle.Image };
        ToolStripButton Button_Create_Random_Rare = new ToolStripButton("Test Rare", Properties.Resources.Random) { DisplayStyle = ToolStripItemDisplayStyle.Image };
        ToolStripSeparator Separator5 = new ToolStripSeparator();
        ToolStripButton Button_Edit_Prefixes = new ToolStripButton("Edit Prefixes") { DisplayStyle = ToolStripItemDisplayStyle.Text };
        ToolStripButton Button_Edit_Suffixes = new ToolStripButton("Edit Suffixes") { DisplayStyle = ToolStripItemDisplayStyle.Text };

        #endregion
        #region Outline
        TreeView Outline_TreeView = new TreeView();
        TreeNode Node_White_Items = new TreeNode("White Items");
        TreeNode Node_Unique_Items = new TreeNode("Unique Items");
        TreeNode Node_Others = new TreeNode("Others");

        //TreeNode Node_Prefixes = new TreeNode("Prefixes");
        //TreeNode Node_Suffixes = new TreeNode("Suffixes");
        //TreeNode Node_Prefix_Caption = new TreeNode("Prefix Caption");
        //TreeNode Node_Suffix_Caption = new TreeNode("Suffix Captions");
        #endregion
        PropertyGrid Main_Property = new PropertyGrid();
        OpenFileDialog OpenDialog = new OpenFileDialog();
        SaveFileDialog SaveDialog = new SaveFileDialog();
        #endregion
        ///<summary> Initialize Controls </summary>
        private void InitializeControls() {
            #region Main_ToolStrip
            Main_ToolStrip.SuspendLayout();
            Main_ToolStrip.Items.AddRange(new ToolStripItem[] { Button_New, Buton_Open, Button_Save, 
                Separator1, Button_Create_Weapon_Bow, Button_Create_Weapon_Dagger, Button_Create_Weapon_Wand, 
                Separator2, Button_Create_Helmet, Button_Create_Body_Armor, Button_Create_Glove, Button_Create_Boot,
                Separator3, Button_Create_Flask, 
                Separator4, Button_Create_Random_Magic, Button_Create_Random_Rare,
                Separator5, Button_Edit_Prefixes, Button_Edit_Suffixes
            });
            Main_ToolStrip.RenderMode = ToolStripRenderMode.System;
            Main_ToolStrip.ItemClicked +=new ToolStripItemClickedEventHandler(Main_ToolStrip_ItemClicked);
           


            Main_ToolStrip.ResumeLayout(false);
            Main_ToolStrip.PerformLayout();
            #endregion
            #region Outline_TreeView
            Outline_TreeView.Dock = DockStyle.Left;
            //Outline_TreeView.Nodes.AddRange(new TreeNode[] { Node_White_Items, Node_Unique_Items, Node_Prefixes, Node_Suffixes, Node_Prefix_Caption, Node_Suffix_Caption });
            Outline_TreeView.Nodes.AddRange(new TreeNode[] { Node_White_Items, Node_Unique_Items, Node_Others });
            Outline_TreeView.Size = new System.Drawing.Size(200, 424);
            Outline_TreeView.HideSelection = false;
            Outline_TreeView.ShowLines = false;
            Outline_TreeView.TabIndex = 1;
            SetWindowTheme(Outline_TreeView.Handle.ToInt32(), "Explorer", null);
            PostMessageW(Outline_TreeView.Handle.ToInt32(), 4396, 4, 4);
            Outline_TreeView.AfterSelect +=new TreeViewEventHandler(Outline_TreeView_AfterSelect);
            Outline_TreeView.KeyDown += (sender, E) => { 
                if (E.Control && E.KeyCode == System.Windows.Forms.Keys.C) { Copy(); }
                else if (E.Control && E.KeyCode == System.Windows.Forms.Keys.V) { Paste(); }
                else if (E.KeyCode == System.Windows.Forms.Keys.Delete) { Delete(); }
            };
            #endregion
            #region Main_Property
            Main_Property.Dock = DockStyle.Right;
            Main_Property.Size = new System.Drawing.Size(300, 424);
            Main_Property.PropertyValueChanged +=new PropertyValueChangedEventHandler(Main_Property_PropertyValueChanged);
            #endregion

            OpenDialog.Filter = "System Item|*.SI|All files|*.*";
            SaveDialog.Filter = "System Item|*.SI|All files|*.*";

            #region Me
            Me = e.Form;
            Me.SuspendLayout();
            Me.Left = 0; Me.Top = 0;
            Me.Controls.Add(this.Main_Property);
            Me.Controls.Add(this.Outline_TreeView);
            Me.Controls.Add(this.Main_ToolStrip);
            Me.Text = "System Item Editor";
            Me.Resize += (sender, E) => { GameSetting.Width = Me.ClientSize.Width; GameSetting.Height = Me.ClientSize.Height; };
            Me.ResumeLayout(false);
            Me.PerformLayout();
            #endregion
        }
    }
}





