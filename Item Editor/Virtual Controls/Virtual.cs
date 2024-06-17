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
    using PSS = Images.UI.PassiveSkillScreen;
    using VC = Images.VirtualControl;


    public abstract class VirtualControl {
        public string Name;
        public bool Visible = true, Enable = true;
        public int X, Y, Width, Height, Index;

        public object Tag;
        public string Text = "";
        public Texture2D Image;
        public Form Form;
        public VirtualControl Parent;
        public VirtualControlCollection Controls;
        public DrawMode DrawMode = DrawMode.Normal;

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
    
    public class VirtualContainer : VirtualControl {
        public VirtualContainer() { }
        public override void Draw() {
            
        }
    }
}
