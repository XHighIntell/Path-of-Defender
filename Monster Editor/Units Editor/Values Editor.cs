using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Monster_Editor {
    public partial class Values_Editor : Form {
        public Values_Editor() { InitializeComponent(); Form_Load(); }
        public Values_Editor(object value, string text) {
            InitializeComponent(); Form_Load();
            Value = value;
            ValueType = value.GetType();

            Label_Text.Text = text;
            if (ValueType.FullName == "System.Single") {
                //Graphics
                Value1.Left = Label_Text.Left + Label_Text.Width + 6;                
                this.ClientSize = new Size(Value1.Left + Value1.Width + 6, Button_OK.Top + Button_OK.Height + 6);

                Value1.Minimum = -1000000;
                Value1.Maximum = 1000000;

                //Set Value
                Value1.Value = Convert.ToDecimal(value);
                Value1.Visible = true;
            } else if (ValueType.FullName == "System.Single[]") {
                Value1.Left = Label_Text.Left + Label_Text.Width + 6;
                Value2.Left = Value1.Left + Value1.Width + 6;
                this.ClientSize = new Size(Value2.Left + Value2.Width + 6, Button_OK.Top + Button_OK.Height + 6);

                Value1.Minimum = -1000000; Value1.Maximum = 1000000;
                Value2.Minimum = -1000000; Value2.Maximum = 1000000;
                //
                Value1.Value = Convert.ToDecimal(((float[])value)[0]);
                Value2.Value = Convert.ToDecimal(((float[])value)[1]);

                Value1.Visible = true; Value2.Visible = true;
            } else if (ValueType.FullName == "System.String") {
                Value_String.Left = Label_Text.Left + Label_Text.Width + 6;
                this.ClientSize = new Size(Value_String.Left + Value_String.Width + 6, Button_OK.Top + Button_OK.Height + 6);
                Value_String.Text = (string)value;
                Value_String.Visible = true;
            } else if (ValueType.FullName == "System.Int32") {
                Value1.Left = Label_Text.Left + Label_Text.Width + 6;
                this.ClientSize = new Size(Value1.Left + Value1.Width + 6, Button_OK.Top + Button_OK.Height + 6);

                Value1.DecimalPlaces = 0; Value1.Increment = 1;
                Value1.Minimum = System.Int32.MinValue;
                Value1.Maximum = System.Int32.MaxValue;
                //Set Value
                Value1.Value = Convert.ToDecimal(value);
                Value1.Visible = true;
            } else if (ValueType.FullName == "System.Boolean") {
                Value_Bool.Left = Label_Text.Left + Label_Text.Width + 6;
                
                this.ClientSize = new Size(Value_Bool.Left + Value_Bool.Width + 6, Button_OK.Top + Button_OK.Height + 6);
                //Set Value
                Value_Bool.Checked = (bool)Value;
                Value_Bool.Visible = true;
            } else if (ValueType.IsEnum == true) {
                Value_Enum.Left = Label_Text.Left + Label_Text.Width + 6;
                this.ClientSize = new Size(Value_Enum.Left + Value_Enum.Width + 6, Button_OK.Top + Button_OK.Height + 6);

                string[] enums = ValueType.GetEnumNames();
                for (int i = 0; i < enums.Length; i++) {
                    Value_Enum.Items.Add(enums[i]);
                }
                Value_Enum.Text = value.ToString();
                Value_Enum.Visible = true;
            } else if (ValueType.IsArray == true){
                Array arr = (Array)Value;
                for (int i = 0; i < arr.Length; i++) {
                    Value_Array.Items.Add(arr.GetValue(i).ToString());
                }
                this.ClientSize = new Size(Value_Array.Left + Value_Array.Width + 6, Button_Add.Top + Button_Add.Height + 6 + Button_Cancel.Height + 6);

                Button_Cancel.Top = Button_Add.Top + Button_Add.Height + 6;
                Button_OK.Top = Button_Add.Top + Button_Add.Height + 6;
                Value_Array.Visible = true;
            }

            if (this.ClientSize.Width < 160) { this.ClientSize = new Size(160, this.ClientSize.Height); }

            Button_Cancel.Left = this.ClientSize.Width - Button_Cancel.Width - 6;
            Button_OK.Left = Button_Cancel.Left - Button_Cancel.Width - 6;
            

            //OwnedForms 
        }

        private void Form_Load() {
            SetWindowTheme(Value_Array.Handle.ToInt32(), "Explorer", null);

        }

        public Type ValueType;
        public object Value;



        private void Button_OK_Click(object sender, EventArgs e) {
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void Value_ValueChanged(object sender, EventArgs e) {
            if (ValueType.FullName == "System.Single") {
                Value = Convert.ToSingle(((NumericUpDown)sender).Value);
            } else if (ValueType.FullName == "System.Single[]") {
                int i = Convert.ToInt32(((Control)sender).Tag);
                ((float[])Value)[i] = Convert.ToSingle(((NumericUpDown)sender).Value);
            } else if (ValueType.FullName == "System.Int32") {
                Value = Convert.ToInt32(((NumericUpDown)sender).Value);
            }
        }
        private void Value_String_TextChanged(object sender, EventArgs e) {
            Value = Value_String.Text;
        }
        private void Value_Bool_CheckedChanged(object sender, EventArgs e) {
            Value = Value_Bool.Checked;
        }
        private void This_Shown(object sender, EventArgs e) {
            this.Left = this.Owner.Left + (this.Owner.Width - this.Width) / 2;
            this.Top = this.Owner.Top + (this.Owner.Height - this.Height) / 2;
        }
        private void Value_Enum_SelectedIndexChanged(object sender, EventArgs e) {
            Value = Enum.Parse(ValueType, Value_Enum.Text);
        }


        //Array Enum
        private void Value_Array_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (Value_Array.SelectedIndex != -1) {

                Values_Editor edit = new Values_Editor(((Array)Value).GetValue(0), Label_Text.Text);
                DialogResult result = edit.ShowDialog(this);
                if (result == DialogResult.OK) {
                    ((Array)Value).SetValue(edit.Value, Value_Array.SelectedIndex);
                    Value_Array.Items[Value_Array.SelectedIndex] = edit.Value.ToString();
                }
            }
        }
        private void Button_Add_Click(object sender, EventArgs e) {
            Values_Editor edit;
            if (ValueType.GetElementType().FullName == "System.String")
            {
                edit = new Values_Editor("", Label_Text.Text);
            } else {
                edit = new Values_Editor(Activator.CreateInstance(ValueType.GetElementType()), Label_Text.Text);
            }
            
            DialogResult result = edit.ShowDialog(this);
            if (result == DialogResult.OK) {
                Value = ((Array)Value).Add(edit.Value);
                Value_Array.Items.Add(edit.Value.ToString());
            }
        }
        private void Button_Delete_Click(object sender, EventArgs e) {
            if (Value_Array.SelectedIndex != -1) {
                Value = ((Array)Value).RemoveAt(Value_Array.SelectedIndex);
                Value_Array.Items.RemoveAt(Value_Array.SelectedIndex);
            }
        }


    }
}
