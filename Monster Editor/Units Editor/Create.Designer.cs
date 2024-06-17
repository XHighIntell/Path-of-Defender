namespace Monster_Editor {
    partial class MonsterCreateForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MonsterCreateForm));
            this.Text_MonsterName = new System.Windows.Forms.TextBox();
            this.Button_Monster = new System.Windows.Forms.Button();
            this.Button_OK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Button_Cancel = new System.Windows.Forms.Button();
            this.Button_Monster1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Text_MonsterName
            // 
            this.Text_MonsterName.Location = new System.Drawing.Point(12, 12);
            this.Text_MonsterName.Name = "Text_MonsterName";
            this.Text_MonsterName.Size = new System.Drawing.Size(434, 20);
            this.Text_MonsterName.TabIndex = 0;
            this.Text_MonsterName.TextChanged += new System.EventHandler(this.Text_MonsterName_TextChanged);
            // 
            // Button_Monster
            // 
            this.Button_Monster.AutoSize = true;
            this.Button_Monster.Image = ((System.Drawing.Image)(resources.GetObject("Button_Monster.Image")));
            this.Button_Monster.Location = new System.Drawing.Point(12, 51);
            this.Button_Monster.Name = "Button_Monster";
            this.Button_Monster.Size = new System.Drawing.Size(46, 51);
            this.Button_Monster.TabIndex = 1;
            this.Button_Monster.Tag = "Bandit";
            this.Button_Monster.UseVisualStyleBackColor = false;
            this.Button_Monster.Click += new System.EventHandler(this.Button_Monster_Click);
            // 
            // Button_OK
            // 
            this.Button_OK.Location = new System.Drawing.Point(290, 156);
            this.Button_OK.Name = "Button_OK";
            this.Button_OK.Size = new System.Drawing.Size(75, 23);
            this.Button_OK.TabIndex = 2;
            this.Button_OK.Text = "OK";
            this.Button_OK.UseVisualStyleBackColor = true;
            this.Button_OK.Click += new System.EventHandler(this.Buttons_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Unit: Bandit";
            // 
            // Button_Cancel
            // 
            this.Button_Cancel.Location = new System.Drawing.Point(371, 156);
            this.Button_Cancel.Name = "Button_Cancel";
            this.Button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.Button_Cancel.TabIndex = 4;
            this.Button_Cancel.Text = "Cancel";
            this.Button_Cancel.UseVisualStyleBackColor = true;
            this.Button_Cancel.Click += new System.EventHandler(this.Buttons_Click);
            // 
            // Button_Monster1
            // 
            this.Button_Monster1.AutoSize = true;
            this.Button_Monster1.Image = ((System.Drawing.Image)(resources.GetObject("Button_Monster1.Image")));
            this.Button_Monster1.Location = new System.Drawing.Point(64, 51);
            this.Button_Monster1.Name = "Button_Monster1";
            this.Button_Monster1.Size = new System.Drawing.Size(46, 51);
            this.Button_Monster1.TabIndex = 5;
            this.Button_Monster1.Tag = "Firen";
            this.Button_Monster1.UseVisualStyleBackColor = false;
            this.Button_Monster1.Click += new System.EventHandler(this.Button_Monster_Click);
            // 
            // MonsterCreateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 191);
            this.Controls.Add(this.Button_Monster1);
            this.Controls.Add(this.Button_Cancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Button_OK);
            this.Controls.Add(this.Button_Monster);
            this.Controls.Add(this.Text_MonsterName);
            this.Name = "MonsterCreateForm";
            this.Text = "Create_New_Monster";
            this.Load += new System.EventHandler(this.MonsterCreateForm_Load);
            this.Shown += new System.EventHandler(this.This_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Text_MonsterName;
        private System.Windows.Forms.Button Button_Monster;
        private System.Windows.Forms.Button Button_OK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Button_Cancel;
        private System.Windows.Forms.Button Button_Monster1;
    }
}