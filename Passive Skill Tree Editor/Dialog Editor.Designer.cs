namespace PassiveSkillScreen_Editor
{
    partial class Dialog_Editor
    {
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Text_Size = new System.Windows.Forms.TextBox();
            this.Radio_Custom = new System.Windows.Forms.RadioButton();
            this.Radio_Large = new System.Windows.Forms.RadioButton();
            this.Radio_Medium = new System.Windows.Forms.RadioButton();
            this.Radio_Small = new System.Windows.Forms.RadioButton();
            this.Radio_Tiny = new System.Windows.Forms.RadioButton();
            this.Button_Ok = new System.Windows.Forms.Button();
            this.Button_Cancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Text_Size);
            this.groupBox1.Controls.Add(this.Radio_Custom);
            this.groupBox1.Controls.Add(this.Radio_Large);
            this.groupBox1.Controls.Add(this.Radio_Medium);
            this.groupBox1.Controls.Add(this.Radio_Small);
            this.groupBox1.Controls.Add(this.Radio_Tiny);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(305, 158);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tree Size";
            // 
            // Text_Size
            // 
            this.Text_Size.Location = new System.Drawing.Point(17, 132);
            this.Text_Size.Name = "Text_Size";
            this.Text_Size.Size = new System.Drawing.Size(282, 20);
            this.Text_Size.TabIndex = 3;
            this.Text_Size.Text = "-10000,-10000,10000,10000";
            this.Text_Size.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Radio_Custom
            // 
            this.Radio_Custom.AutoSize = true;
            this.Radio_Custom.Location = new System.Drawing.Point(17, 111);
            this.Radio_Custom.Name = "Radio_Custom";
            this.Radio_Custom.Size = new System.Drawing.Size(60, 17);
            this.Radio_Custom.TabIndex = 4;
            this.Radio_Custom.TabStop = true;
            this.Radio_Custom.Text = "Custom";
            this.Radio_Custom.UseVisualStyleBackColor = true;
            // 
            // Radio_Large
            // 
            this.Radio_Large.AutoSize = true;
            this.Radio_Large.Location = new System.Drawing.Point(17, 88);
            this.Radio_Large.Name = "Radio_Large";
            this.Radio_Large.Size = new System.Drawing.Size(193, 17);
            this.Radio_Large.TabIndex = 3;
            this.Radio_Large.TabStop = true;
            this.Radio_Large.Text = "Large (-6000, -6000, 12000, 12000)";
            this.Radio_Large.UseVisualStyleBackColor = true;
            // 
            // Radio_Medium
            // 
            this.Radio_Medium.AutoSize = true;
            this.Radio_Medium.Location = new System.Drawing.Point(17, 65);
            this.Radio_Medium.Name = "Radio_Medium";
            this.Radio_Medium.Size = new System.Drawing.Size(191, 17);
            this.Radio_Medium.TabIndex = 2;
            this.Radio_Medium.TabStop = true;
            this.Radio_Medium.Text = "Medium (-4500, -4500, 9000, 9000)";
            this.Radio_Medium.UseVisualStyleBackColor = true;
            // 
            // Radio_Small
            // 
            this.Radio_Small.AutoSize = true;
            this.Radio_Small.Location = new System.Drawing.Point(17, 42);
            this.Radio_Small.Name = "Radio_Small";
            this.Radio_Small.Size = new System.Drawing.Size(179, 17);
            this.Radio_Small.TabIndex = 1;
            this.Radio_Small.TabStop = true;
            this.Radio_Small.Text = "Small (-3000, -3000, 6000, 6000)";
            this.Radio_Small.UseVisualStyleBackColor = true;
            // 
            // Radio_Tiny
            // 
            this.Radio_Tiny.AutoSize = true;
            this.Radio_Tiny.Location = new System.Drawing.Point(17, 19);
            this.Radio_Tiny.Name = "Radio_Tiny";
            this.Radio_Tiny.Size = new System.Drawing.Size(174, 17);
            this.Radio_Tiny.TabIndex = 0;
            this.Radio_Tiny.TabStop = true;
            this.Radio_Tiny.Text = "Tiny (-1500, -1500, 3000, 3000)";
            this.Radio_Tiny.UseVisualStyleBackColor = true;
            // 
            // Button_Ok
            // 
            this.Button_Ok.Location = new System.Drawing.Point(323, 19);
            this.Button_Ok.Name = "Button_Ok";
            this.Button_Ok.Size = new System.Drawing.Size(75, 23);
            this.Button_Ok.TabIndex = 1;
            this.Button_Ok.Text = "OK";
            this.Button_Ok.UseVisualStyleBackColor = true;
            this.Button_Ok.Click += new System.EventHandler(this.Buttons_Click);
            // 
            // Button_Cancel
            // 
            this.Button_Cancel.Location = new System.Drawing.Point(323, 48);
            this.Button_Cancel.Name = "Button_Cancel";
            this.Button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.Button_Cancel.TabIndex = 2;
            this.Button_Cancel.Text = "Cancel";
            this.Button_Cancel.UseVisualStyleBackColor = true;
            this.Button_Cancel.Click += new System.EventHandler(this.Buttons_Click);
            // 
            // Dialog_Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 181);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Button_Cancel);
            this.Controls.Add(this.Button_Ok);
            this.Name = "Dialog_Editor";
            this.ShowIcon = false;
            this.Text = "New Passive Skill Tree";
            this.Load += new System.EventHandler(this.Dialog_Editor_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton Radio_Large;
        private System.Windows.Forms.RadioButton Radio_Medium;
        private System.Windows.Forms.RadioButton Radio_Small;
        private System.Windows.Forms.RadioButton Radio_Tiny;
        private System.Windows.Forms.Button Button_Ok;
        private System.Windows.Forms.Button Button_Cancel;
        private System.Windows.Forms.RadioButton Radio_Custom;
        private System.Windows.Forms.TextBox Text_Size;
    }
}