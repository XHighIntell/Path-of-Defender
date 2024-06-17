using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Graphics;
using Microsoft.VisualBasic;

namespace Path_of_Defender {
    using PSS = Images.UI.PassiveSkillScreen; using VC = Images.VirtualControl;


    public abstract class VirtualControl {
        public string Name;
        public bool Visible, Enable = true;
        public int X, Y, Width, Height, Index;

        public object Tag;
        public Skill Skill;
        public string Text = "";
        public string TooltipText = "";
        public Texture2D Image;
        /// <summary> Là Form của cả chương trình </summary>
        public Form Form;
        public VirtualControl Parent;
        public VirtualControlCollection Controls;

        public VirtualControl() {
            Controls = new VirtualControlCollection(this);
            Form = e.Form;
        }

        public void AutoSize() {
            Width = 0; Height = 0;
            for (int i = 0; i < Controls.Count; i++) {
                if (Controls.Items[i].Visible == true) { 
                    if (Controls.Items[i].Width + Controls.Items[i].X >= Width) {
                        Width = Controls.Items[i].Width + Controls.Items[i].X;
                    }
                    if (Controls.Items[i].Height + Controls.Items[i].Y >= Height) {
                        Height = Controls.Items[i].Height + Controls.Items[i].Y;
                    }
                }
            }
        }
        public virtual void Draw() { }
        public virtual void Update() { }

        //runtime
        public bool IsHover = false;

        public Point ClientToScreen(Point point) {
            if (this.Parent != null) {
                Point TMP_Point = this.Parent.ClientToScreen(point);
                return new Point(TMP_Point.X + this.X, TMP_Point.Y + this.Y);
            } else {
                return new Point(point.X + this.X, point.Y + this.Y);
            }   
        }

        //Events
        public event MouseEventHandler MouseDown, MouseUp, MouseMove, MouseWheel;
        public event EventHandler Click, MouseEnter, MouseLeave, DrawControl;

        internal void Proc_MouseDown(MouseEventArgs e) {
            if (MouseDown != null) { MouseDown(this, e); }
        }
        internal void Proc_MouseUp(MouseEventArgs e) {
            if (MouseUp != null) { 
                MouseUp(this, e);
            } 

            if (e.Button == MouseButtons.Left) {
                if (0 <= e.X && e.X <= this.Width && 0 <= e.Y && e.Y <= this.Height) {
                    if (Click != null) { Click(this, null); }
                }
            }

        }
        internal void Proc_MouseMove(MouseEventArgs e) {
            if (MouseMove != null) { MouseMove(this, e); }
        }
        internal void Proc_MouseEnter(EventArgs e) {
            if (MouseEnter != null) { MouseEnter(this, e); }
            IsHover = true;
        }
        internal void Proc_MouseLeave(EventArgs e) {
            if (MouseLeave != null) { MouseLeave(this, e); }
            IsHover = false;
        }
        internal void Proc_DrawControl(EventArgs e) {
            if (DrawControl != null) { DrawControl(this, e); }
        }
        internal void Proc_MouseWheel(MouseEventArgs e) {
            if (MouseWheel != null) { MouseWheel(this, e); }
        }

        //Extra Function
        //public void Draw_Tooltip() {
        //   if (TooltipText != "") {
        //       Point point = ClientToScreen(Point.Zero);
        //       Vector2 String_Size = Font.MeasureString(TooltipText);
        //       e.SpriteBatch.Draw(Images.VirtualControl.ImageWhitePoint, new RectangleF(point.X, point.Y, String_Size.X, String_Size.Y), Color.White * 0.3f);
        //       e.SpriteBatch.DrawString(Font, TooltipText, new Vector2(point.X + Width - String_Size.X / 2, point.Y - String_Size.Y), Color.White);
        //   }
        //}
    }


    public partial class VirtualControlCollection {
        public VirtualControl[] Items = { };
        public int Count { get { return Items.Length; } }
        public void Add(VirtualControl item) {
            Array.Resize(ref Items, Items.Length + 1);
            Items[Items.Length - 1] = item;
            item.Parent = Owner;
        }
        public void RemoveAt(int index) {
            Array.Copy(Items, index + 1, Items, index, Items.Length - index - 1);
            Array.Resize(ref Items, Items.Length - 1);
        }
        public void Clear() {
            Array.Resize(ref  Items, 0);
        }
        
        public VirtualControl Hit(float x, float y) {
            VirtualControl Return;
            for (int i = Items.Length - 1; i >= 0; i--) {
                if (Items[i].Visible == true) {
                    if (Items[i].X < x && x < Items[i].X + Items[i].Width &&
                        Items[i].Y < y && y < Items[i].Y + Items[i].Height) {
                            Return = Items[i].Controls.Hit(x - Items[i].X, y - Items[i].Y);
                        if (Return != null) { return Return; }
                        return Items[i];
                    }
                }
            }
            return null;
        }
        public VirtualControl Hit(int x, int y) {
            VirtualControl Return;
            for (int i = Items.Length - 1; i >= 0; i--) {
                if (Items[i].Visible == true) {
                    if (Items[i].X <= x && x <= Items[i].X + Items[i].Width &&
                        Items[i].Y <= y && y <= Items[i].Y + Items[i].Height) {
                            Return = Items[i].Controls.Hit(x - Items[i].X, y - Items[i].Y);
                        if (Return != null) { return Return; }
                        return Items[i];
                    }
                }
            }
            return null;
        }
        public Point ScreenToClient(VirtualControl control, Point point) {

            if (control.Parent != null) {
                Point TMP_Point = ScreenToClient(control.Parent, point);
                return new Point(TMP_Point.X - control.X, TMP_Point.Y - control.Y);
            } else {
                return new Point(point.X - control.X, point.Y - control.Y);
            }
            
            
        }

        public VirtualControl Owner;
        public VirtualControlCollection(VirtualControl owner) { Owner = owner; }
        public VirtualControlCollection() { }
        /// <summary>
        /// Khi tạo Collection này với VirtualControlCollection(), thì cần gọi Update() để bắt các sự kiện từ Mouse.
        /// Những sự kiện từ Keyboard Update() không làm gì cả.
        /// </summary>
        public void Update() {
            MouseButtons TMP_MouseButtons = MouseButtons.None;
            VirtualControl TMP_Hit_Control = Hit(e.X, e.Y);
            Point TMP_Point;
            
            #region MouseLeave MouseEnter
            if (e.MouseDown_Control == null) {
                //MouseLeave
                if (e.MouseEnter_Control != null && TMP_Hit_Control != e.MouseEnter_Control) {
                    e.MouseEnter_Control.Proc_MouseLeave(null); e.MouseEnter_Control = null;
                }
                //MouseEnter
                if (e.MouseEnter_Control != TMP_Hit_Control && TMP_Hit_Control != null) {
                    e.MouseEnter_Control = TMP_Hit_Control;
                    e.MouseEnter_Control.Proc_MouseEnter(null);
                }
            }
            #endregion
            
            #region MouseDown
            TMP_MouseButtons = MouseButtons.None;
                 if (e.Mouse.LeftButton.Pressed)   { TMP_MouseButtons |= MouseButtons.Left; }
            else if (e.Mouse.MiddleButton.Pressed) { TMP_MouseButtons |= MouseButtons.Middle; }
            else if (e.Mouse.RightButton.Pressed)  { TMP_MouseButtons |= MouseButtons.Right; }
            else if (e.Mouse.XButton1.Pressed)     { TMP_MouseButtons |= MouseButtons.XButton1; }
            else if (e.Mouse.XButton2.Pressed)     { TMP_MouseButtons |= MouseButtons.XButton2; }

            //Proc events lên MouseDown_Control
            //Khi MouseDown_Control = null thì mới tìm thằng mới còn nếu != null thì proc event lên MouseDown_Control
            //Set value to global
            if (TMP_MouseButtons != MouseButtons.None) { 
                if (e.MouseDown_Control == null) { e.MouseDown_Control = TMP_Hit_Control; e.Focus_Control = e.MouseDown_Control; }
                if (e.MouseDown_Control != null) {
                    TMP_Point = ScreenToClient(e.MouseDown_Control, e.Mouse_Position);
                    e.MouseDown_Control.Proc_MouseDown(new MouseEventArgs(TMP_MouseButtons, 1, TMP_Point.X, TMP_Point.Y, 0)); 
                }
            }
            #endregion

            #region MouseMove
            TMP_MouseButtons = MouseButtons.None;
            if (e.Mouse.LeftButton.Down)   { TMP_MouseButtons |= MouseButtons.Left; }
            if (e.Mouse.MiddleButton.Down) { TMP_MouseButtons |= MouseButtons.Middle; }
            if (e.Mouse.RightButton.Down)  { TMP_MouseButtons |= MouseButtons.Right; }
            if (e.Mouse.XButton1.Down)     { TMP_MouseButtons |= MouseButtons.XButton1; }
            if (e.Mouse.XButton2.Down)     { TMP_MouseButtons |= MouseButtons.XButton2; }
            
            //Nếu MouseDown_Control != null thì cứ Proc MouseMove Event không cần phân biệt tọa độ và MouseMove_Control
            if (e.MouseDown_Control != null) {
                TMP_Point = ScreenToClient(e.MouseDown_Control, e.Mouse_Position);
                e.MouseDown_Control.Proc_MouseMove(new MouseEventArgs(TMP_MouseButtons, 0, TMP_Point.X, TMP_Point.Y, 0));
                
            } else {
            //New MouseDown_Control = null thì, dùng MouseMove_Control để Proc Events
                e.MouseMove_Control = TMP_Hit_Control;
                if (e.MouseMove_Control != null) {
                    TMP_Point = ScreenToClient(e.MouseMove_Control, e.Mouse_Position);
                    e.MouseMove_Control.Proc_MouseMove(new MouseEventArgs(TMP_MouseButtons, 0, TMP_Point.X, TMP_Point.Y, 0));
                }
            }
            #endregion

            #region MouseUp
            if (e.MouseDown_Control != null) {
                //Proc events lên MouseDown_Control
                TMP_MouseButtons = MouseButtons.None;
                     if (e.Mouse.LeftButton.Released)   { TMP_MouseButtons |= MouseButtons.Left; }
                else if (e.Mouse.MiddleButton.Released) { TMP_MouseButtons |= MouseButtons.Middle; }
                else if (e.Mouse.RightButton.Released)  { TMP_MouseButtons |= MouseButtons.Right; }
                else if (e.Mouse.XButton1.Released)     { TMP_MouseButtons |= MouseButtons.XButton1; }
                else if (e.Mouse.XButton2.Released)     { TMP_MouseButtons |= MouseButtons.XButton2; }

                if (TMP_MouseButtons != MouseButtons.None) {
                    TMP_Point = ScreenToClient(e.MouseDown_Control, e.Mouse_Position);
                    e.MouseDown_Control.Proc_MouseUp(new MouseEventArgs(TMP_MouseButtons, 1, TMP_Point.X, TMP_Point.Y, 0));
                    e.MouseDown_Control = null;
                } 
            }
            #endregion

            #region MouseWheel
            if (e.Mouse.WheelDelta != 0) {
                if (e.MouseDown_Control != null) {
                    TMP_Point = ScreenToClient(e.MouseDown_Control, e.Mouse_Position);
                    e.MouseDown_Control.Proc_MouseWheel(new MouseEventArgs(MouseButtons.None, 0, TMP_Point.X, TMP_Point.Y, e.Mouse.WheelDelta));
                } else if (TMP_Hit_Control != null) {
                    TMP_Point = ScreenToClient(TMP_Hit_Control, e.Mouse_Position);
                    TMP_Hit_Control.Proc_MouseWheel(new MouseEventArgs(MouseButtons.None, 0, TMP_Point.X, TMP_Point.Y, e.Mouse.WheelDelta));
                }
            }
            #endregion

            #region Call Update
            for (int i = 0; i < Items.Length; i++) { Items[i].Update(); }
            #endregion
        }
        public void Draw() {
            for (int i = 0; i < Items.Length; i++) {
                if (Items[i].Visible == true) {
                    Items[i].Draw();
                    Items[i].Controls.Draw();
                }
            }
        }
    }
    #region Test
    public partial class VirtualControlCollection { 
        public VirtualControlCollection(System.Windows.Forms.Form parent) { 
            parent.Click += new EventHandler(Parent_Click);
            parent.MouseDown +=new MouseEventHandler(Parent_MouseDown);
            parent.MouseUp += new MouseEventHandler(Parent_MouseUp);
        }

        private void Parent_Click(Object sender, EventArgs E) { 
            
        }
        private void Parent_MouseDown(Object sender, MouseEventArgs E) { 
            //MouseDown_Control = 
            MouseEventArgs s = new MouseEventArgs(MouseButtons.Left, 1, 2, 2, 120);
        }
        private void Parent_MouseUp(Object sender, MouseEventArgs E) { 
            
        }
    }
    #endregion

    public class VirtualButton : VirtualControl {
        public DrawMode DrawMode = DrawMode.Normal;
        public VirtualButton() { }

        public override void Draw() {
            if (DrawMode == DrawMode.Normal) {
                Point TMP_Point = ClientToScreen(Point.Zero);
                e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied, e.GraphicsDevice.SamplerStates.LinearWrap);
                if (Enable == true) {
                    if (IsHover == true) { PSS.Buttons.ActiveButton.Draw(Text, TMP_Point.X, TMP_Point.Y, Width, Height); }
                    else { PSS.Buttons.NormalButton.Draw(Text, TMP_Point.X, TMP_Point.Y, Width, Height); }
                }
                else { PSS.Buttons.DisableButton.Draw(Text, TMP_Point.X, TMP_Point.Y, Width, Height); }
                e.SpriteBatch.End();
            } else if (DrawMode == System.Windows.Forms.DrawMode.OwnerDrawVariable) {
                Proc_DrawControl(null);
            }
        }
        private void NormalDraw() {           
        }
    }

    public partial class VirtualEdit : VirtualControl {
        public static float Base_Cursor_Blink_Rate = 0.4f;
        public static int Base_Border_Radius = 2;
        public static Color Base_Border_Color = new Color(0xFFEFE9E3);
        public static Color Base_Border_3D_Color = new Color(0xFFB3ADAB);

        public static Color Base_Focus_Border_Color = new Color(0xFFE7CFB5);
        public static Color Base_Focus_Border_3D_Color = new Color(0xFFAD7B3D);

        //for runtime 
        private float Time_Cursor_Blink;
        public Rectangle Rect_Cursor = new Rectangle(0, 0, 1, 20);
        public Vector2 Origin_Cursor = new Vector2(0, 10);

        public int SelectionStart;

        public Padding Padding = new Padding(3, 2, 3, 2);
        public HorizontalAlignment TextAlign = HorizontalAlignment.Left;
        public SpriteFont _Font = Fonts.FontinRegular11;
        public SpriteFont Font {
            get { return _Font; }
            set { 
                _Font = value;
                Rect_Cursor.Height = (int)_Font.MeasureString("L").Y;
                Origin_Cursor.Y = Rect_Cursor.Height / 2;
            }
        }

        public int MaxLength = 10;
        //public 
        public VirtualEdit() {
            Form.KeyPress += new System.Windows.Forms.KeyPressEventHandler(Parent_KeyPress);
            Form.KeyDown += new System.Windows.Forms.KeyEventHandler(Parent_KeyDown);
            this.MouseDown +=new MouseEventHandler(This_MouseDown);
        }

    }
    public partial class VirtualEdit : VirtualControl { 
        private void Parent_KeyPress(Object sender, System.Windows.Forms.KeyPressEventArgs E) {
            if (e.Focus_Control == this) {
                if (Strings.Asc(E.KeyChar) == 8) { 
                    if (SelectionStart > 0) {
                        Text = Strings.Mid(Text, 1, SelectionStart - 1) + Strings.Mid(Text, SelectionStart + 1);
                        SelectionStart -= 1;
                    }
                }
                else if (Text.Length < MaxLength) {
                    Text = Strings.Mid(Text, 1, SelectionStart) + E.KeyChar + Strings.Mid(Text, SelectionStart + 1);
                    SelectionStart += 1;
                }

                if (SelectionStart < 0) { SelectionStart = 0; }
                if (SelectionStart > Text.Length) { SelectionStart = Text.Length; } 
            }
        }
        private void Parent_KeyDown(Object sender, System.Windows.Forms.KeyEventArgs E) {
            if (e.Focus_Control == this) {
                if (E.KeyCode == System.Windows.Forms.Keys.Left) { SelectionStart -= 1; }
                else if (E.KeyCode == System.Windows.Forms.Keys.Right) { SelectionStart += 1; }
                else if (E.KeyCode == System.Windows.Forms.Keys.Home) { SelectionStart = 0; }
                else if (E.KeyCode == System.Windows.Forms.Keys.End) { SelectionStart = Text.Length; }
                else if (E.KeyCode == System.Windows.Forms.Keys.Delete) { Text = Strings.Mid(Text, 1, SelectionStart) + Strings.Mid(Text, 2 + SelectionStart); }

                if (SelectionStart < 0) { SelectionStart = 0; }
                else if (SelectionStart > Text.Length) { SelectionStart = Text.Length; }
                Time_Cursor_Blink = 0;
            }
        }
        private void This_MouseDown(Object sender, System.Windows.Forms.MouseEventArgs E) {
            Vector2 TMP_Vector2 = new Vector2(); SelectionStart = 0;
            for (int i = 0; i < Text.Length; i++) {
                TMP_Vector2 = Font.MeasureString(Strings.Mid(Text, 1, i + 1));
                if (E.X > TMP_Vector2.X){
                    SelectionStart = i + 1;
                } else {
                    break;
                };
            }
            
        }

        public override void Draw() {
            Time_Cursor_Blink += GameSetting.SecondPerFrame;
            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied);

            //Base_Border_3D_Color
            //e.SpriteBatch.Draw(VC.ImageWhitePoint, new Vector2(X, Y), new Rectangle(X, Y, Width, 1), Color.Red, 0, Vector2.Zero, 1, SpriteEffects.None, 0); 


            DrawText();
            if (e.Focus_Control == this) {
                e.SpriteBatch.DrawRectangle(X, Y, Width, Height, Base_Focus_Border_Color, Base_Border_Radius);
                e.SpriteBatch.Draw(VC.ImageWhitePoint, new Rectangle(X + Base_Border_Radius, Y, Width - Base_Border_Radius * 2, 1), Base_Focus_Border_3D_Color);
                DrawCursor(); 
            }
            else {
                e.SpriteBatch.DrawRectangle(X, Y, Width, Height, Base_Border_Color, Base_Border_Radius);
                e.SpriteBatch.Draw(VC.ImageWhitePoint, new Rectangle(X + Base_Border_Radius, Y, Width - Base_Border_Radius * 2, 1), Base_Border_3D_Color);
            }
            e.SpriteBatch.End();
        }

        private void DrawText() {
            if (TextAlign == HorizontalAlignment.Left) { e.SpriteBatch.DrawString(Fonts.FontinRegular11, Text, new Vector2(X + Padding.Left, Y + Padding.Top), Color.White); }
            else if (TextAlign == HorizontalAlignment.Center) {
                Vector2 String_Size = Font.MeasureString(Text);
                e.SpriteBatch.DrawString(Fonts.FontinRegular11, Text,
                    new Vector2((X + X + Width) / 2, (Y + Y + Height) / 2), Color.White, 0, String_Size / 2, 1, SpriteEffects.None, 0);
            }
        }
        private void DrawCursor() { 
            if (Time_Cursor_Blink < Base_Cursor_Blink_Rate) {
                Vector2 Pos_Cursor = new Vector2();
                Vector2 SelectionStart_String_Size = Fonts.FontinRegular11.MeasureString(Strings.Mid(Text, 1, SelectionStart));
                
                

                if (TextAlign == HorizontalAlignment.Left) { Pos_Cursor.X = X + Padding.Left + SelectionStart_String_Size.X; Pos_Cursor.Y = Y + Padding.Top + Origin_Cursor.Y; }
                else if (TextAlign == HorizontalAlignment.Center) {
                    Vector2 String_Size = Fonts.FontinRegular11.MeasureString(Text);
                    Pos_Cursor.X = X + (Width - String_Size.X) / 2 + SelectionStart_String_Size.X; Pos_Cursor.Y = Y + Height / 2; 
                }


                e.SpriteBatch.Draw(PSS.PopupBackground.Image, Pos_Cursor, Rect_Cursor, Color.White, 0, Origin_Cursor, 1, SpriteEffects.None, 0);
            } else if (Time_Cursor_Blink < 2 * Base_Cursor_Blink_Rate) { } 
            else { Time_Cursor_Blink = 0; }
        }
    }
    
    public class VirtualContainer : VirtualControl {
        public VirtualContainer() { }
        public override void Draw() {
            
        }
    }
}
