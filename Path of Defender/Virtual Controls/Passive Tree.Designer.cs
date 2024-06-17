using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;

namespace Path_of_Defender {
    using PSS = Images.UI.PassiveSkillScreen;

    partial class PassiveTree {
        public event MouseEventHandler ItemMouseClick;

        public Color Color_Caption = new Color(27, 162, 155);
        public bool IsEditor;

        public VirtualControl Button_OK;
        public VirtualControl Button_Cancel;

        private void InitializeComponent() {
            //Button_OK
            this.Button_OK = new VirtualButton();
            this.Button_OK.Name = "OK";
            this.Button_OK.Visible = true;
            this.Button_OK.X = GameSetting.Width / 2 - PSS.Buttons.Width / 2;
            this.Button_OK.Y = 70;
            this.Button_OK.Width = PSS.Buttons.Width / 2;
            this.Button_OK.Height = PSS.Buttons.Height / 2;
            this.Button_OK.Text = "OK";

            //Button_Cancel
            this.Button_Cancel = new VirtualButton();
            this.Button_Cancel.Name = "Cancel";
            this.Button_Cancel.Visible = true;
            this.Button_Cancel.X = GameSetting.Width / 2;
            this.Button_Cancel.Y = 70;
            this.Button_Cancel.Width = PSS.Buttons.Width / 2;
            this.Button_Cancel.Height = PSS.Buttons.Height / 2;
            this.Button_Cancel.Text = "Cancel";

            //Form
            this.Controls.Add(Button_OK);
            this.Controls.Add(Button_Cancel);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(Passive_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(Passive_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(Passive_MouseUp);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(Passive_MouseWheel);
        }
    }
}
