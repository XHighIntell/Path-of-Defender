using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpDX;

namespace PassiveSkillScreen_Editor
{
    public partial class Dialog_Editor : Form
    {
        public enum DiablogType { New = 0, Edit = 1 }
        public Dialog_Editor() { InitializeComponent(); }
        public Dialog_Editor(DiablogType Type, ref PassiveTreeData Data) { InitializeComponent(); PassiveTreeData = Data; type = Type; }


        PassiveTreeData PassiveTreeData;
        DiablogType type;

        SharpDX.Rectangle[] Rects = {new SharpDX.Rectangle (-2000,-2000,4000,4000),new SharpDX.Rectangle (-4000,-4000,8000,8000)
                                    ,new SharpDX.Rectangle (-8000,-8000,16000,16000),new SharpDX.Rectangle (-16000,-16000,32000,32000)};

        RadioButton[] RadioButtons;
        
        private void Dialog_Editor_Load(object sender, EventArgs e) {
            RadioButtons = new RadioButton[4] { Radio_Tiny, Radio_Small, Radio_Medium, Radio_Large };
            Radio_Tiny.Text = "Tiny " + Rects[0].ToString();
            Radio_Small.Text = "Small " + Rects[1].ToString();
            Radio_Medium.Text = "Medium " + Rects[2].ToString();
            Radio_Large.Text = "Large " + Rects[3].ToString();
        }

        private void Buttons_Click(object sender, EventArgs e) {
            if (sender == Button_Ok) {
                if (type == DiablogType.New) {
                    int[] ints = StringToInts(Text_Size.Text);

                    //PassiveTreeData = new PassiveTreeData();

                    for (int i = 1; i < PassiveTreeData.Skills.Length; i++) {
                        PassiveTreeData.RemoveSkill(i); i--;
                    }
                    PassiveTreeData.Lines = new Line[0];
                    PassiveTreeData.Labels = new Label[0];
                    PassiveTreeData.Backgrounds = new Background[0];                    

                    PassiveTreeData.AddBackground(new Background("Start", new SharpDX.Vector2(0, 0), SharpDX.Toolkit.Graphics.SpriteEffects.None));

                    PassiveTreeData.AddLabel(new Label(new Vector2(0, -131), "0", new SharpDX.Color(0, 156, 255), 0, 1));
                    PassiveTreeData.AddLabel(new Label(new Vector2(-112, 60), "0", new SharpDX.Color(255, 74, 0), 0, 1));
                    PassiveTreeData.AddLabel(new Label(new Vector2(112, 60), "0", new SharpDX.Color(0, 255, 87), 0, 1));

                    PassiveTreeData.Rect.X = ints[0];
                    PassiveTreeData.Rect.Y = ints[1];
                    PassiveTreeData.Rect.Width = ints[2];
                    PassiveTreeData.Rect.Height = ints[3];
                    for (int i = 0; i < RadioButtons.Length; i++) {
                        if (RadioButtons[i].Checked == true) {
                            PassiveTreeData.Rect.X = Rects[i].X; PassiveTreeData.Rect.Y = Rects[i].Y;
                            PassiveTreeData.Rect.Width = Rects[i].Width; PassiveTreeData.Rect.Height = Rects[i].Height;
                            this.DialogResult = System.Windows.Forms.DialogResult.OK;
                            this.Close(); return;
                        }
                    }
                }
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close(); return;
            } else if (sender == Button_Cancel) {
                this.Close();
            }
        }

        private int[] StringToInts(string e) {
            string[] s = Microsoft.VisualBasic.Strings.Split(e, ",");
            int[] r = new int[s.Length];

            for (int i = 0; i < r.Length; i++) {
                if (s[i] != "") {
                    r[i] = Convert.ToInt32(s[i]);   
                }
             
            }

            return r;

        }
    }
}
