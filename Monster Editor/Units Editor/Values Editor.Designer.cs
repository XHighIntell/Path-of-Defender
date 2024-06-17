using System.Runtime.InteropServices;
namespace Monster_Editor
{
    partial class Values_Editor {
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
            this.Label_Text = new System.Windows.Forms.Label();
            this.Value1 = new System.Windows.Forms.NumericUpDown();
            this.Value2 = new System.Windows.Forms.NumericUpDown();
            this.Button_OK = new System.Windows.Forms.Button();
            this.Button_Cancel = new System.Windows.Forms.Button();
            this.Value_String = new System.Windows.Forms.TextBox();
            this.Value_Enum = new System.Windows.Forms.ComboBox();
            this.Value_Bool = new System.Windows.Forms.CheckBox();
            this.Value_Array = new System.Windows.Forms.ListBox();
            this.Button_Add = new System.Windows.Forms.Button();
            this.Button_Delete = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Value1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Value2)).BeginInit();
            this.SuspendLayout();
            // 
            // Label_Text
            // 
            this.Label_Text.AutoSize = true;
            this.Label_Text.Location = new System.Drawing.Point(12, 9);
            this.Label_Text.MinimumSize = new System.Drawing.Size(50, 0);
            this.Label_Text.Name = "Label_Text";
            this.Label_Text.Size = new System.Drawing.Size(50, 13);
            this.Label_Text.TabIndex = 0;
            this.Label_Text.Text = "X:";
            this.Label_Text.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Value1
            // 
            this.Value1.DecimalPlaces = 3;
            this.Value1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.Value1.Location = new System.Drawing.Point(123, 7);
            this.Value1.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.Value1.Name = "Value1";
            this.Value1.Size = new System.Drawing.Size(80, 20);
            this.Value1.TabIndex = 1;
            this.Value1.Tag = "0";
            this.Value1.Value = new decimal(new int[] {
            3,
            0,
            0,
            131072});
            this.Value1.Visible = false;
            this.Value1.ValueChanged += new System.EventHandler(this.Value_ValueChanged);
            // 
            // Value2
            // 
            this.Value2.DecimalPlaces = 3;
            this.Value2.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.Value2.Location = new System.Drawing.Point(209, 7);
            this.Value2.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.Value2.Name = "Value2";
            this.Value2.Size = new System.Drawing.Size(80, 20);
            this.Value2.TabIndex = 2;
            this.Value2.Tag = "1";
            this.Value2.Value = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.Value2.Visible = false;
            this.Value2.ValueChanged += new System.EventHandler(this.Value_ValueChanged);
            // 
            // Button_OK
            // 
            this.Button_OK.Location = new System.Drawing.Point(466, 33);
            this.Button_OK.Name = "Button_OK";
            this.Button_OK.Size = new System.Drawing.Size(70, 20);
            this.Button_OK.TabIndex = 3;
            this.Button_OK.Text = "OK";
            this.Button_OK.UseVisualStyleBackColor = true;
            this.Button_OK.Click += new System.EventHandler(this.Button_OK_Click);
            // 
            // Button_Cancel
            // 
            this.Button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Button_Cancel.Location = new System.Drawing.Point(542, 33);
            this.Button_Cancel.Name = "Button_Cancel";
            this.Button_Cancel.Size = new System.Drawing.Size(70, 20);
            this.Button_Cancel.TabIndex = 4;
            this.Button_Cancel.Text = "Cancel";
            this.Button_Cancel.UseVisualStyleBackColor = true;
            // 
            // Value_String
            // 
            this.Value_String.Location = new System.Drawing.Point(305, 7);
            this.Value_String.Name = "Value_String";
            this.Value_String.Size = new System.Drawing.Size(166, 20);
            this.Value_String.TabIndex = 5;
            this.Value_String.Visible = false;
            this.Value_String.TextChanged += new System.EventHandler(this.Value_String_TextChanged);
            // 
            // Value_Enum
            // 
            this.Value_Enum.FormattingEnabled = true;
            this.Value_Enum.Location = new System.Drawing.Point(490, 7);
            this.Value_Enum.Name = "Value_Enum";
            this.Value_Enum.Size = new System.Drawing.Size(166, 21);
            this.Value_Enum.TabIndex = 6;
            this.Value_Enum.SelectedIndexChanged += new System.EventHandler(this.Value_Enum_SelectedIndexChanged);
            // 
            // Value_Bool
            // 
            this.Value_Bool.AutoSize = true;
            this.Value_Bool.Location = new System.Drawing.Point(665, 10);
            this.Value_Bool.Name = "Value_Bool";
            this.Value_Bool.Size = new System.Drawing.Size(15, 14);
            this.Value_Bool.TabIndex = 7;
            this.Value_Bool.UseVisualStyleBackColor = true;
            this.Value_Bool.Visible = false;
            this.Value_Bool.CheckedChanged += new System.EventHandler(this.Value_Bool_CheckedChanged);
            // 
            // Value_Array
            // 
            this.Value_Array.FormattingEnabled = true;
            this.Value_Array.Location = new System.Drawing.Point(8, 33);
            this.Value_Array.Name = "Value_Array";
            this.Value_Array.Size = new System.Drawing.Size(191, 199);
            this.Value_Array.TabIndex = 8;
            this.Value_Array.Visible = false;
            this.Value_Array.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Value_Array_MouseDoubleClick);
            // 
            // Button_Add
            // 
            this.Button_Add.Location = new System.Drawing.Point(8, 238);
            this.Button_Add.Name = "Button_Add";
            this.Button_Add.Size = new System.Drawing.Size(91, 20);
            this.Button_Add.TabIndex = 9;
            this.Button_Add.Text = "Add Value";
            this.Button_Add.UseVisualStyleBackColor = true;
            this.Button_Add.Click += new System.EventHandler(this.Button_Add_Click);
            // 
            // Button_Delete
            // 
            this.Button_Delete.Location = new System.Drawing.Point(108, 238);
            this.Button_Delete.Name = "Button_Delete";
            this.Button_Delete.Size = new System.Drawing.Size(91, 20);
            this.Button_Delete.TabIndex = 10;
            this.Button_Delete.Text = "Delete Value";
            this.Button_Delete.UseVisualStyleBackColor = true;
            this.Button_Delete.Click += new System.EventHandler(this.Button_Delete_Click);
            // 
            // Values_Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.Button_Cancel;
            this.ClientSize = new System.Drawing.Size(701, 289);
            this.ControlBox = false;
            this.Controls.Add(this.Button_Delete);
            this.Controls.Add(this.Button_Add);
            this.Controls.Add(this.Value_Array);
            this.Controls.Add(this.Value_Bool);
            this.Controls.Add(this.Value_Enum);
            this.Controls.Add(this.Value_String);
            this.Controls.Add(this.Button_Cancel);
            this.Controls.Add(this.Button_OK);
            this.Controls.Add(this.Value2);
            this.Controls.Add(this.Value1);
            this.Controls.Add(this.Label_Text);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(177, 38);
            this.Name = "Values_Editor";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Edit Value";
            this.Shown += new System.EventHandler(this.This_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.Value1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Value2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Label_Text;
        private System.Windows.Forms.NumericUpDown Value1;
        private System.Windows.Forms.NumericUpDown Value2;
        private System.Windows.Forms.Button Button_OK;
        private System.Windows.Forms.Button Button_Cancel;
        private System.Windows.Forms.TextBox Value_String;
        private System.Windows.Forms.ComboBox Value_Enum;
        private System.Windows.Forms.CheckBox Value_Bool;
        private System.Windows.Forms.ListBox Value_Array;
        private System.Windows.Forms.Button Button_Add;
        private System.Windows.Forms.Button Button_Delete;
    }
}