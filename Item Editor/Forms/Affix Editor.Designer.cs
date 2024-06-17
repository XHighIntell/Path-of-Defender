using System;
namespace Path_of_Defender
{
    partial class Affix_Editor {
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
        private void InitializeComponent()
        {
            this.Value_Caption = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Value_Level = new System.Windows.Forms.NumericUpDown();
            this.Value_ModType = new System.Windows.Forms.ComboBox();
            this.Value_Value1_Min = new System.Windows.Forms.NumericUpDown();
            this.Value_Value1_Max = new System.Windows.Forms.NumericUpDown();
            this.Value_Value2_Min = new System.Windows.Forms.NumericUpDown();
            this.Value_Value2_Max = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Button_Cancel = new System.Windows.Forms.Button();
            this.Button_OK = new System.Windows.Forms.Button();
            this.Label_Description = new System.Windows.Forms.Label();
            this.Value_Percent = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.Value_Level)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Value_Value1_Min)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Value_Value1_Max)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Value_Value2_Min)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Value_Value2_Max)).BeginInit();
            this.SuspendLayout();
            // 
            // Value_Caption
            // 
            this.Value_Caption.Location = new System.Drawing.Point(63, 24);
            this.Value_Caption.Name = "Value_Caption";
            this.Value_Caption.Size = new System.Drawing.Size(100, 20);
            this.Value_Caption.TabIndex = 0;
            this.Value_Caption.Text = "Caption";
            this.Value_Caption.TextChanged += new System.EventHandler(this.Value_Caption_TextChanged);
            this.Value_Caption.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Controls_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Caption";
            // 
            // Value_Level
            // 
            this.Value_Level.Location = new System.Drawing.Point(169, 24);
            this.Value_Level.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.Value_Level.Name = "Value_Level";
            this.Value_Level.Size = new System.Drawing.Size(71, 20);
            this.Value_Level.TabIndex = 2;
            this.Value_Level.ValueChanged += new System.EventHandler(this.Value_Changed);
            this.Value_Level.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Controls_KeyDown);
            // 
            // Value_ModType
            // 
            this.Value_ModType.FormattingEnabled = true;
            this.Value_ModType.Location = new System.Drawing.Point(63, 50);
            this.Value_ModType.Name = "Value_ModType";
            this.Value_ModType.Size = new System.Drawing.Size(177, 21);
            this.Value_ModType.TabIndex = 3;
            this.Value_ModType.SelectedIndexChanged += new System.EventHandler(this.Value_ModType_SelectedIndexChanged);
            this.Value_ModType.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Controls_KeyDown);
            // 
            // Value_Value1_Min
            // 
            this.Value_Value1_Min.DecimalPlaces = 2;
            this.Value_Value1_Min.Location = new System.Drawing.Point(63, 77);
            this.Value_Value1_Min.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.Value_Value1_Min.Name = "Value_Value1_Min";
            this.Value_Value1_Min.Size = new System.Drawing.Size(71, 20);
            this.Value_Value1_Min.TabIndex = 4;
            this.Value_Value1_Min.ValueChanged += new System.EventHandler(this.Value_Changed);
            this.Value_Value1_Min.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Controls_KeyDown);
            // 
            // Value_Value1_Max
            // 
            this.Value_Value1_Max.DecimalPlaces = 2;
            this.Value_Value1_Max.Location = new System.Drawing.Point(169, 77);
            this.Value_Value1_Max.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.Value_Value1_Max.Name = "Value_Value1_Max";
            this.Value_Value1_Max.Size = new System.Drawing.Size(71, 20);
            this.Value_Value1_Max.TabIndex = 5;
            this.Value_Value1_Max.ValueChanged += new System.EventHandler(this.Value_Changed);
            this.Value_Value1_Max.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Controls_KeyDown);
            // 
            // Value_Value2_Min
            // 
            this.Value_Value2_Min.DecimalPlaces = 2;
            this.Value_Value2_Min.Location = new System.Drawing.Point(63, 103);
            this.Value_Value2_Min.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.Value_Value2_Min.Name = "Value_Value2_Min";
            this.Value_Value2_Min.Size = new System.Drawing.Size(71, 20);
            this.Value_Value2_Min.TabIndex = 6;
            this.Value_Value2_Min.ValueChanged += new System.EventHandler(this.Value_Changed);
            this.Value_Value2_Min.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Controls_KeyDown);
            // 
            // Value_Value2_Max
            // 
            this.Value_Value2_Max.DecimalPlaces = 2;
            this.Value_Value2_Max.Location = new System.Drawing.Point(169, 103);
            this.Value_Value2_Max.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.Value_Value2_Max.Name = "Value_Value2_Max";
            this.Value_Value2_Max.Size = new System.Drawing.Size(71, 20);
            this.Value_Value2_Max.TabIndex = 7;
            this.Value_Value2_Max.ValueChanged += new System.EventHandler(this.Value_Changed);
            this.Value_Value2_Max.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Controls_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Mod";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Value 1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Value 2";
            // 
            // Button_Cancel
            // 
            this.Button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Button_Cancel.Location = new System.Drawing.Point(165, 155);
            this.Button_Cancel.Name = "Button_Cancel";
            this.Button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.Button_Cancel.TabIndex = 11;
            this.Button_Cancel.Text = "Cancel";
            this.Button_Cancel.UseVisualStyleBackColor = true;
            // 
            // Button_OK
            // 
            this.Button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Button_OK.Location = new System.Drawing.Point(84, 155);
            this.Button_OK.Name = "Button_OK";
            this.Button_OK.Size = new System.Drawing.Size(75, 23);
            this.Button_OK.TabIndex = 12;
            this.Button_OK.Text = "OK";
            this.Button_OK.UseVisualStyleBackColor = true;
            // 
            // Label_Description
            // 
            this.Label_Description.Location = new System.Drawing.Point(0, 0);
            this.Label_Description.Name = "Label_Description";
            this.Label_Description.Size = new System.Drawing.Size(253, 22);
            this.Label_Description.TabIndex = 13;
            this.Label_Description.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Value_Percent
            // 
            this.Value_Percent.AutoSize = true;
            this.Value_Percent.Location = new System.Drawing.Point(63, 129);
            this.Value_Percent.Name = "Value_Percent";
            this.Value_Percent.Size = new System.Drawing.Size(125, 17);
            this.Value_Percent.TabIndex = 14;
            this.Value_Percent.Text = "This value is Percent";
            this.Value_Percent.UseVisualStyleBackColor = true;
            this.Value_Percent.CheckedChanged += new System.EventHandler(this.Value_Percent_CheckedChanged);
            // 
            // Affix_Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(254, 190);
            this.Controls.Add(this.Value_Percent);
            this.Controls.Add(this.Label_Description);
            this.Controls.Add(this.Button_OK);
            this.Controls.Add(this.Button_Cancel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Value_Value2_Max);
            this.Controls.Add(this.Value_Value2_Min);
            this.Controls.Add(this.Value_Value1_Max);
            this.Controls.Add(this.Value_Value1_Min);
            this.Controls.Add(this.Value_ModType);
            this.Controls.Add(this.Value_Level);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Value_Caption);
            this.Name = "Affix_Editor";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Affix Editor";
            this.Shown += new System.EventHandler(this.Affix_Editor_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.Value_Level)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Value_Value1_Min)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Value_Value1_Max)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Value_Value2_Min)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Value_Value2_Max)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private void InitializeComponent1() {
            Value_ModType.Items.AddRange(Enum.GetNames(typeof(ModType)));
        }

        private System.Windows.Forms.TextBox Value_Caption;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown Value_Level;
        private System.Windows.Forms.ComboBox Value_ModType;
        private System.Windows.Forms.NumericUpDown Value_Value1_Min;
        private System.Windows.Forms.NumericUpDown Value_Value1_Max;
        private System.Windows.Forms.NumericUpDown Value_Value2_Min;
        private System.Windows.Forms.NumericUpDown Value_Value2_Max;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button Button_Cancel;
        private System.Windows.Forms.Button Button_OK;
        private System.Windows.Forms.Label Label_Description;
        private System.Windows.Forms.CheckBox Value_Percent;
    }
}