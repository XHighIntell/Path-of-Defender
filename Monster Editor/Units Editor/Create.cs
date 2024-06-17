using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Monster_Editor {
    public partial class MonsterCreateForm : Form {
        public MonsterCreateForm() {
            InitializeComponent();
        }

        public string Monster_Name = "";
        public MonsterType Monster_Type;

        private void MonsterCreateForm_Load(object sender, EventArgs e) {
            Monster_Type = MonsterType.Bandit;
        }

        private void Text_MonsterName_TextChanged(object sender, EventArgs e) {
            Monster_Name = Text_MonsterName.Text;
        }

        private void Button_Monster_Click(object sender, EventArgs e) {
            Monster_Type = (MonsterType)Enum.Parse(typeof(MonsterType), (string)((Control)(sender)).Tag);
        }

        private void Buttons_Click(object sender, EventArgs e) {
            if (sender == Button_OK) {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            } else if (sender == Button_Cancel) {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
            this.Close();
        }

        private void This_Shown(object sender, EventArgs e) {
            this.Left = this.Owner.Left + (this.Owner.Width - this.Width) / 2;
            this.Top = this.Owner.Top + (this.Owner.Height - this.Height) / 2;
        }
    }
}
