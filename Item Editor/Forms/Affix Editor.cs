using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Path_of_Defender {
    public partial class Affix_Editor : Form {
        public Affix Value;
        private enum Enum_LastEdit { Caption, Level, Mod, Value1Min, Value1Max, Value2Min, Value2Max, Percent }
        private static Enum_LastEdit LastEdit = Enum_LastEdit.Caption;
        public Affix_Editor(Affix value) {
            InitializeComponent(); InitializeComponent1();
            Value = value;
            Refresh_Values();
        }
        private void Affix_Editor_Shown(object sender, EventArgs e) {
            if (LastEdit == Enum_LastEdit.Caption) { Value_Caption.Focus(); }
            else if (LastEdit == Enum_LastEdit.Level) { Value_Level.Focus(); }
            else if (LastEdit == Enum_LastEdit.Mod) { Value_ModType.Focus(); }
            else if (LastEdit == Enum_LastEdit.Value1Min) { Value_Value1_Min.Focus(); }
            else if (LastEdit == Enum_LastEdit.Value1Max) { Value_Value1_Max.Focus(); }
            else if (LastEdit == Enum_LastEdit.Value2Min) { Value_Value2_Min.Focus(); }
            else if (LastEdit == Enum_LastEdit.Value2Max) { Value_Value2_Max.Focus(); }
            else if (LastEdit == Enum_LastEdit.Percent) { Value_Percent.Focus(); }
        }

        public void Refresh_Values() {
            Is_Getting = true;
            Value_Caption.Text = Value.Caption;
            Value_Level.Value = Value.Level;
            Value_ModType.Text = Value.Type.ToString();
            Value_Value1_Min.Value = (decimal)Value.Value1.Minimum;
            Value_Value1_Max.Value = (decimal)Value.Value1.Maximum;

            Value_Value2_Min.Value = (decimal)Value.Value2.Minimum;
            Value_Value2_Max.Value = (decimal)Value.Value2.Maximum;

            Value_Percent.Checked = Value.IsPercent;
            Label_Description.Text = Value.ToString();
            Is_Getting = false;
        }
        bool Is_Getting = false;
        private void Value_Changed(object sender, EventArgs e) {
            if (Is_Getting == true) { return; }
            if (sender == Value_Level) { Value.Level = (int)Value_Level.Value; LastEdit = Enum_LastEdit.Level; }
            else if (sender == Value_Value1_Min) { Value.Value1.Minimum = (float)Value_Value1_Min.Value; LastEdit = Enum_LastEdit.Value1Min; }
            else if (sender == Value_Value1_Max) { Value.Value1.Maximum = (float)Value_Value1_Max.Value; LastEdit = Enum_LastEdit.Value1Max; }
            else if (sender == Value_Value2_Min) { Value.Value2.Minimum = (float)Value_Value2_Min.Value; LastEdit = Enum_LastEdit.Value2Min; }
            else if (sender == Value_Value2_Max) { Value.Value2.Maximum = (float)Value_Value2_Max.Value; LastEdit = Enum_LastEdit.Value2Max; }
            Refresh_Values();
        }
        private void Value_ModType_SelectedIndexChanged(object sender, EventArgs e) {
            if (Is_Getting == true) { return; }
            Value.Type = (ModType)Enum.Parse(typeof(ModType), Value_ModType.Text); LastEdit = Enum_LastEdit.Mod;
            Refresh_Values();
        }
        private void Value_Caption_TextChanged(object sender, EventArgs e) {
            if (Is_Getting == true) { return; }
            Value.Caption = Value_Caption.Text; LastEdit = Enum_LastEdit.Caption;
            Refresh_Values();
        }
        private void Value_Percent_CheckedChanged(object sender, EventArgs e) {
            if (Is_Getting == true) { return; }
            Value.IsPercent = Value_Percent.Checked; LastEdit = Enum_LastEdit.Percent; 
        }

        private void Controls_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) { this.DialogResult = System.Windows.Forms.DialogResult.OK; this.Close(); } 
            else if (e.KeyCode == Keys.Escape) { this.DialogResult = System.Windows.Forms.DialogResult.Cancel; this.Close(); }
        }

        
    }
}
