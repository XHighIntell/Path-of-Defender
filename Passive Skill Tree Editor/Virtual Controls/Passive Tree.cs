using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Graphics;
using Microsoft.VisualBasic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.ComponentModel;
using System.Windows.Forms;
using SWF = System.Windows.Forms;

namespace PassiveSkillScreen_Editor {
 using PSS = Images.UI.PassiveSkillScreen;
    using System.Diagnostics;
    public delegate void PassiveSkillClickedEventHandler(object sender, PassiveSkillClickedEventArgs e);
    public class PassiveSkillClickedEventArgs : EventArgs {
        public int Index;
        public PassiveSkillClickedEventArgs(int index) {
            Index = index;
        }
    }
    
    public struct _Rect  {
        public int X, Y, Width, Height;
        public _Rect(int x, int y, int w, int h) { X = x; Y = y; Width = w; Height = h; }
    }
    public enum SelectType { None = 0, Skill = 1, Line = 2, Background = 3, Label = 4 }
    public partial class PassiveTree : VirtualControl {
        public int PointsLeft = 10;
        public float Scale = 0.5f;
        public Vector2 Position = new Vector2(0, 0);
        public PassiveTreeData SelectedData;

        public PassiveTree() {
            InitializeComponent();
            //SelectedData = new PassiveTreeData(); AddSkill(new PassiveSkill()); SelectedData.Skills[0].Status = SkillStatus.Allocated;
            ViewAsCenter(Position);
        }
        public override void Draw() {
            if (SelectedData == null) { return; }
            //Draw background reapeat
            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.SpriteBatch.GraphicsDevice.BlendStates.NonPremultiplied, e.SpriteBatch.GraphicsDevice.SamplerStates.PointMirror);
            e.SpriteBatch.Draw(PSS.Background.Image, new Rectangle(0, 0, GameSetting.Width, GameSetting.Height),
                new Rectangle((int)(Position.X / Scale), (int)(Position.Y / Scale), (int)(GameSetting.Width / Scale), (int)(GameSetting.Height / Scale)), Color.White);
            for (int i = 0; i < SelectedData.Backgrounds.Length; i++) {
                SelectedData.Backgrounds[i].Draw(e.SpriteBatch, Scale, Position);
            }
            e.SpriteBatch.End();

            //Draw Line 
            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.SpriteBatch.GraphicsDevice.BlendStates.NonPremultiplied, e.SpriteBatch.GraphicsDevice.SamplerStates.LinearWrap);
            for (int i = 0; i < SelectedData.Lines.Length; i++) { SelectedData.Lines[i].Draw(e.SpriteBatch, Scale, Position); }
            e.SpriteBatch.End();

            //Draw background Skill
            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.SpriteBatch.GraphicsDevice.BlendStates.Default, e.SpriteBatch.GraphicsDevice.SamplerStates.PointWrap);
            for (int i = 1; i < SelectedData.Skills.Length; i++) { SelectedData.Skills[i].DrawBackground(e.SpriteBatch, Scale, Position); }
            e.SpriteBatch.End();

            //Draw foreground Skill
            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.SpriteBatch.GraphicsDevice.BlendStates.NonPremultiplied, e.SpriteBatch.GraphicsDevice.SamplerStates.PointWrap);
            for (int i = 1; i < SelectedData.Skills.Length; i++) { SelectedData.Skills[i].DrawCircle(e.SpriteBatch, Scale, Position); }
            e.SpriteBatch.End();

            //Draw Labels
            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.SpriteBatch.GraphicsDevice.BlendStates.NonPremultiplied, e.GraphicsDevice.SamplerStates.PointWrap);
            for (int i = 0; i < SelectedData.Labels.Length; i++) { SelectedData.Labels[i].Draw(Scale, Position); }
            e.SpriteBatch.End();

            //Draw Tootip
            if (Hover != -1 && IsHover == true) {
                Vector2 sizeCaption = Fonts.FontinRegular11.MeasureString(SelectedData.Skills[Hover].Caption);
                Vector2 sizeDescription = Fonts.FontinRegular11.MeasureString(SelectedData.Skills[Hover].Description);
                Vector2 sizeLegend = Fonts.FontinItalic11.MeasureString(SelectedData.Skills[Hover].Legend);
                
                int scrW, scrH;
                if (sizeCaption.X > sizeDescription.X) { scrW = (int)sizeCaption.X; } else { scrW = (int)sizeDescription.X; }
                if (sizeLegend.X > scrW) { scrW = (int)sizeLegend.X; }
                scrH = (int)(sizeCaption.Y + sizeDescription.Y + sizeLegend.Y );
                if (SelectedData.Skills[Hover].Legend != "") scrH += 5;

                e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.SpriteBatch.GraphicsDevice.BlendStates.NonPremultiplied, e.SpriteBatch.GraphicsDevice.SamplerStates.PointWrap);

                if (MouseMove_PointF.X + 20 + scrW > GameSetting.Width) { MouseMove_PointF.X = GameSetting.Width - 20 - scrW; }
                if (MouseMove_PointF.Y + 20 + scrH > GameSetting.Height) { MouseMove_PointF.Y = GameSetting.Height - 20 - scrH; }

                e.SpriteBatch.Draw(PSS.PopupBackground.Image, MouseMove_PointF + 10, new Rectangle(0, 0, scrW + 10, scrH + 10), Color.White * 0.8f);
                e.SpriteBatch.DrawString(Fonts.FontinRegular11, SelectedData.Skills[Hover].Caption, MouseMove_PointF + 15, Color_Caption);
                e.SpriteBatch.DrawString(Fonts.FontinRegular11, SelectedData.Skills[Hover].Description, MouseMove_PointF + 15 + new Vector2(0, sizeCaption.Y + 2), Color.White);
                e.SpriteBatch.DrawString(Fonts.FontinItalic11, SelectedData.Skills[Hover].Legend, MouseMove_PointF + 15 + new Vector2(0, sizeCaption.Y + sizeDescription.Y + 5), Color.DarkOrange);
                e.SpriteBatch.End();
            }
            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.SpriteBatch.GraphicsDevice.BlendStates.NonPremultiplied);
            e.SpriteBatch.Draw(PSS.PointsBackground.Image, new Vector2(GameSetting.Width / 2, 50), PSS.PointsBackground.Rect, Color.White, 0, PSS.PointsBackground.Origin, 0.5f, SpriteEffects.None, 0);
            Vector2 SizeString = Fonts.FontinRegular11.MeasureString(PointsLeft + " Points Left");
            e.SpriteBatch.DrawString(Fonts.FontinRegular13, PointsLeft + " Points Left", new Vector2(GameSetting.Width / 2 - SizeString.X / 2, 39), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            e.SpriteBatch.End();
            Controls.Draw();            
        }

        public void Refresh() {
            for (int i = 0; i < SelectedData.Lines.Length; i++) {
                if (SelectedData.Skills[SelectedData.Lines[i].SkillA].Status == SkillStatus.Allocated && SelectedData.Skills[SelectedData.Lines[i].SkillB].Status == SkillStatus.Allocated) {
                    SelectedData.Lines[i].Status = SkillStatus.Allocated;
                } else if (SelectedData.Skills[SelectedData.Lines[i].SkillA].Status == SkillStatus.Allocated || SelectedData.Skills[SelectedData.Lines[i].SkillB].Status == SkillStatus.Allocated) {
                    SelectedData.Lines[i].Status = SkillStatus.CanAllocated;
                } else {
                    SelectedData.Lines[i].Status = SkillStatus.Unallocated;
                }
            }

            for (int i = 1; i < SelectedData.Skills.Length; i++) {
                if (SelectedData.Skills[i].Status == SkillStatus.Unallocated || SelectedData.Skills[i].Status == SkillStatus.CanAllocated ) {
                    SelectedData.Skills[i].Status = SkillStatus.Unallocated;
   
                    for (int x = 0; x < SelectedData.Skills[i].ConnectedSkills.Length; x++) {
                        if (SelectedData.Skills[SelectedData.Skills[i].ConnectedSkills[x]].Status == SkillStatus.Allocated) {
                            SelectedData.Skills[i].Status = SkillStatus.CanAllocated;
                            break;
                        }
                    }


                }
            }
        }
        #region Backup & Restore
        public PassiveSkill[] BackupSkills;
        public Line[] BackupLines;
        public int BackupPointsLeft;
        public void CreateBackup() {
            BackupPointsLeft = PointsLeft;
            BackupSkills = new PassiveSkill[SelectedData.Skills.Length];
            BackupLines = new Line[SelectedData.Lines.Length];

            Array.Copy(SelectedData.Skills, BackupSkills, SelectedData.Skills.Length);
            Array.Copy(SelectedData.Lines, BackupLines, SelectedData.Lines.Length);
        }
        public void RestoreBackup() {
            PointsLeft = BackupPointsLeft;
            SelectedData.Skills = BackupSkills;
            SelectedData.Lines = BackupLines;
        }
        #endregion

        #region Add & Remove
        public void AddSkill(PassiveSkill item) {
            Array.Resize(ref SelectedData.Skills, SelectedData.Skills.Length + 1);
            SelectedData.Skills[SelectedData.Skills.Length - 1] = item;
        }
        public void AddLine(Line item) {
            Array.Resize(ref SelectedData.Lines, SelectedData.Lines.Length + 1);
            SelectedData.Lines[SelectedData.Lines.Length - 1] = item;
        }
        public void AddBackground(Background item) {
            Array.Resize(ref SelectedData.Backgrounds, SelectedData.Backgrounds.Length + 1);
            SelectedData.Backgrounds[SelectedData.Backgrounds.Length - 1] = item;
        }
        public void AddLabel(Label item) {
            Array.Resize(ref SelectedData.Labels, SelectedData.Labels.Length + 1);
            SelectedData.Labels[SelectedData.Labels.Length - 1] = item;
        }

        public void RemoveSkill(int index) {
            Array.Copy(SelectedData.Skills, index + 1, SelectedData.Skills, index, SelectedData.Skills.Length - index - 1);
            Array.Resize(ref SelectedData.Skills, SelectedData.Skills.Length - 1);
        }
        public void RemoveLine(int index) {
            Array.Copy(SelectedData.Lines, index + 1, SelectedData.Lines, index, SelectedData.Lines.Length - index - 1);
            Array.Resize(ref SelectedData.Lines, SelectedData.Lines.Length - 1);
        }
        public void RemoveBackground(int index) {
            Array.Copy(SelectedData.Backgrounds, index + 1, SelectedData.Backgrounds, index, SelectedData.Backgrounds.Length - index - 1);
            Array.Resize(ref SelectedData.Backgrounds, SelectedData.Backgrounds.Length - 1);
        }
        public void RemoveLabel(int index) {
            Array.Copy(SelectedData.Labels, index + 1, SelectedData.Labels, index, SelectedData.Labels.Length - index - 1);
            Array.Resize(ref SelectedData.Labels, SelectedData.Labels.Length - 1);
        }

        #endregion

        #region Hit Functions
        public int HitTest(float x, float y) {
            x = (x + Position.X) / Scale; y = (y + Position.Y) / Scale;
            for (int i = SelectedData.Skills.Length - 1; i > 0; i--) {
                if (SelectedData.Skills[i].Size == SkillSize.Small) {
                    if (SelectedData.Skills[i].Position.X - 51 < x && x < SelectedData.Skills[i].Position.X + 51 && SelectedData.Skills[i].Position.Y - 51 < y && y < SelectedData.Skills[i].Position.Y + 51) {
                        return i;
                    }
                }
                else if (SelectedData.Skills[i].Size == SkillSize.Medium) {
                    if (SelectedData.Skills[i].Position.X - 75 < x && x < SelectedData.Skills[i].Position.X + 75 && SelectedData.Skills[i].Position.Y - 75 < y && y < SelectedData.Skills[i].Position.Y + 75) {
                        return i;
                    }
                }
                else if (SelectedData.Skills[i].Size == SkillSize.Large) {
                    if (SelectedData.Skills[i].Position.X - 110 < x && x < SelectedData.Skills[i].Position.X + 110 && SelectedData.Skills[i].Position.Y - 110 < y && y < SelectedData.Skills[i].Position.Y + 110) {
                        return i;
                    }
                }
            }
            return -1;
        }
        public int HitTestBackground(float x, float y) {
            x = (x + Position.X) / Scale; y = (y + Position.Y) / Scale;
            int index;
            for (int i = SelectedData.Backgrounds.Length - 1; i >= 0; i--) {
                index = PSS.ScreenBackGround.GetIndex(SelectedData.Backgrounds[i].ImageName);
                if (SelectedData.Backgrounds[i].Position.X - PSS.ScreenBackGround.Origins[index].X < x && x < SelectedData.Backgrounds[i].Position.X - PSS.ScreenBackGround.Origins[index].X + PSS.ScreenBackGround.Rects[index].Width &&
                    SelectedData.Backgrounds[i].Position.Y - PSS.ScreenBackGround.Origins[index].Y < y && y < SelectedData.Backgrounds[i].Position.Y - PSS.ScreenBackGround.Origins[index].Y + PSS.ScreenBackGround.Rects[index].Height)
                {
                    return i;
                }
            }

            return -1;
        }
        public int HitTestLine(float x, float y) {
            float xx = (x + Position.X) / Scale; float yy = (y + Position.Y) / Scale;
            Vector2 point;
            PSS.Connect.ConnectBase connectBase;


            for (int i = SelectedData.Lines.Length - 1; i >= 0; i--) {
                if (SelectedData.Lines[i].Style == LineStyle.Line) {
                    point = PointRotate(new Vector2(xx, yy), -SelectedData.Lines[i].Alpha, SelectedData.Lines[i].Position);
                    x = point.X; y = point.Y; connectBase = PSS.Connect.Line;
                    if (SelectedData.Lines[i].Position.X - connectBase.Origin.X < x && x < SelectedData.Lines[i].Position.X - connectBase.Origin.X + SelectedData.Lines[i].Width && SelectedData.Lines[i].Position.Y - connectBase.Origin.Y < y && y < SelectedData.Lines[i].Position.Y - connectBase.Origin.Y + connectBase.Rect.Height) { return i; }
                }
            }

            for (int i = SelectedData.Lines.Length - 1; i >= 0; i--) {
                if (SelectedData.Lines[i].Style == LineStyle.Tiny_Circle) {
                    point = PointRotate(new Vector2(xx, yy), -SelectedData.Lines[i].Alpha, SelectedData.Lines[i].Position);
                    x = point.X; y = point.Y; connectBase = PSS.Connect.TinyCircle;
                    if (SelectedData.Lines[i].Position.X - connectBase.Origin.X < x && x < SelectedData.Lines[i].Position.X - connectBase.Origin.X + SelectedData.Lines[i].Width && SelectedData.Lines[i].Position.Y - connectBase.Origin.Y < y && y < SelectedData.Lines[i].Position.Y - connectBase.Origin.Y + connectBase.Rect.Height) { return i; }
                }
            }

            for (int i = SelectedData.Lines.Length - 1; i >= 0; i--) {
                if (SelectedData.Lines[i].Style == LineStyle.Small_Circle) {
                    point = PointRotate(new Vector2(xx, yy), -SelectedData.Lines[i].Alpha, SelectedData.Lines[i].Position);
                    x = point.X; y = point.Y; connectBase = PSS.Connect.SmallCircle;
                    if (SelectedData.Lines[i].Position.X - connectBase.Origin.X < x && x < SelectedData.Lines[i].Position.X - connectBase.Origin.X + SelectedData.Lines[i].Width && SelectedData.Lines[i].Position.Y - connectBase.Origin.Y < y && y < SelectedData.Lines[i].Position.Y - connectBase.Origin.Y + connectBase.Rect.Height) { return i; }
                }
            }

            for (int i = SelectedData.Lines.Length - 1; i >= 0; i--) {
                if (SelectedData.Lines[i].Style == LineStyle.Medium_Circle) {
                    point = PointRotate(new Vector2(xx, yy), -SelectedData.Lines[i].Alpha, SelectedData.Lines[i].Position);
                    x = point.X; y = point.Y; connectBase = PSS.Connect.MediumCircle;
                    if (SelectedData.Lines[i].Position.X - connectBase.Origin.X < x && x < SelectedData.Lines[i].Position.X - connectBase.Origin.X + SelectedData.Lines[i].Width && SelectedData.Lines[i].Position.Y - connectBase.Origin.Y < y && y < SelectedData.Lines[i].Position.Y - connectBase.Origin.Y + connectBase.Rect.Height) { return i; }
                }
            }

            for (int i = SelectedData.Lines.Length - 1; i >= 0; i--) {
                if (SelectedData.Lines[i].Style == LineStyle.Large_Circle) {
                    point = PointRotate(new Vector2(xx, yy), -SelectedData.Lines[i].Alpha, SelectedData.Lines[i].Position);
                    x = point.X; y = point.Y; connectBase = PSS.Connect.LargeCircle;
                    if (SelectedData.Lines[i].Position.X - connectBase.Origin.X < x && x < SelectedData.Lines[i].Position.X - connectBase.Origin.X + SelectedData.Lines[i].Width && SelectedData.Lines[i].Position.Y - connectBase.Origin.Y < y && y < SelectedData.Lines[i].Position.Y - connectBase.Origin.Y + connectBase.Rect.Height) { return i; }
                }
            }
            return -1;
        }
        public int HitLabels(float x, float y) {
            float xx = (x + Position.X) / Scale; float yy = (y + Position.Y) / Scale;

            Vector2 TMP_Label_Size;
            for (int i = SelectedData.Labels.Length - 1; i >= 0; i--) {
                TMP_Label_Size = Fonts.FontinRegular25.MeasureString(SelectedData.Labels[i].Text) * SelectedData.Labels[i].Scale;
                if (SelectedData.Labels[i].Position.X - TMP_Label_Size.X / 2 < xx && xx < SelectedData.Labels[i].Position.X + TMP_Label_Size.X / 2 &&
                    SelectedData.Labels[i].Position.Y - TMP_Label_Size.Y / 2 < yy && yy < SelectedData.Labels[i].Position.Y + TMP_Label_Size.Y / 2) {
                    return i;
                }
            }
            return -1;
        }
        private Vector2 PointRotate(Vector2 point,float Alpha, Vector2 origin) {
            point = point - origin;
            //x1 = Math.Cos(Alpha) * point.X;
            //y1 = Math.Sin(Alpha) * point.X;

            //x2 = -Math.Sin(Alpha) * point.Y;
            //y2 = Math.Cos(Alpha) * point.Y;

            float x = (float)(Math.Cos(Alpha) * point.X - Math.Sin(Alpha) * point.Y);
            float y = (float)(Math.Sin(Alpha) * point.X + Math.Cos(Alpha) * point.Y);

            Vector2 re = new Vector2(x, y);
            re += origin;
            return re;
        }
        #endregion

        #region View Functions
        public void ViewAsCenter(Vector2 position) {
            Position.X = position.X * Scale - GameSetting.Width / 2;
            Position.Y = position.Y * Scale - GameSetting.Height / 2;
        }
        public Vector2 GetCenterPoint() {
            return new Vector2((Position.X + GameSetting.Width / 2) / Scale, (Position.Y + GameSetting.Height / 2) / Scale);
        }

        private static float[] ZoomValues = { 0.25f, 0.5f, 0.75f, 1f };
        public void Zoom(float scale) {
            Vector2 center = GetCenterPoint();
            Scale = scale;
            ViewAsCenter(center);
        }
        public void ZoomIn() {
            float previous = Scale;
            for (int i = EditorZoomValues.Length - 1; i >= 0; i--) { if (previous > EditorZoomValues[i]) { previous = EditorZoomValues[i]; break; } }
            Zoom(previous);
        }
        public void ZoomOut() {
            float previous = Scale;
            for (int i = 0; i < EditorZoomValues.Length; i++) { if (previous < EditorZoomValues[i]) { previous = EditorZoomValues[i]; break; } }
            Zoom(previous);
        }
        #endregion

        Vector2 MouseMove_PointF; public int Hover; Vector2 Hit;
        bool CanMovePosition; //true WHEN MouseMove can move PassiveTree
        int Hit_Skill_Index;
        #region Events Passive Control
        private void Passive_MouseDown(object sender, MouseEventArgs e) {
            Hit.X = e.X; Hit.Y = e.Y; Hit_Skill_Index = HitTest(Hit.X, Hit.Y);
            if (Hit_Skill_Index == -1) { CanMovePosition = true; }
            else { CanMovePosition = false; }
        }
        private void Passive_MouseMove(object sender, MouseEventArgs E) {
            if (CanMovePosition == true && E.Button == MouseButtons.Left) {
                if (IsEditor == true) { if (e.Keyboard.IsKeyDown(SharpDX.Toolkit.Input.Keys.LeftControl) == false) { Position.X = Position.X + Hit.X - E.X; Position.Y = Position.Y + Hit.Y - E.Y; } } 
                else { Position.X = Position.X + Hit.X - E.X; Position.Y = Position.Y + Hit.Y - E.Y; }
                if (GameSetting.Width <= SelectedData.Rect.Width * Scale) {
                    if (Position.X / Scale < SelectedData.Rect.X) { Position.X = SelectedData.Rect.X * Scale; }
                    else if ((Position.X + GameSetting.Width) / Scale > SelectedData.Rect.X + SelectedData.Rect.Width) { Position.X = (SelectedData.Rect.X + SelectedData.Rect.Width) * Scale - GameSetting.Width; }
                    if (Position.Y / Scale < SelectedData.Rect.Y) { Position.Y = SelectedData.Rect.Y * Scale; }
                    else if ((Position.Y + GameSetting.Height) / Scale > SelectedData.Rect.Y + SelectedData.Rect.Height) { Position.Y = (SelectedData.Rect.Y + SelectedData.Rect.Height) * Scale - GameSetting.Height; }
                } 
                Hit.X = E.X; Hit.Y = E.Y;
            } 
            Hover = HitTest(E.X, E.Y);
            MouseMove_PointF.X = E.X; MouseMove_PointF.Y = E.Y;
        }
        private void Passive_MouseUp(object sender, MouseEventArgs e) {
            if (Hit_Skill_Index != -1 && Hit_Skill_Index == Hover) {
                if (IsEditor == true) { Passive_Editor_ItemMouseClick(e, new MouseEventArgs(e.Button, Hit_Skill_Index, e.X, e.Y, e.Delta)); }
                if (ItemMouseClick != null) { ItemMouseClick(this, new MouseEventArgs(e.Button, Hit_Skill_Index, e.X, e.Y, e.Delta)); }
            }
        }
        private void Passive_MouseWheel(object sender, MouseEventArgs e) {
            if (IsEditor == true) { Passive_Editor_MouseWheel(sender, e); return; }
            if (e.Delta > 0) { ZoomOut(); } else if (e.Delta < 0) { ZoomIn(); }
        }
        #endregion

        #region Editor Only
        private void Passive_Editor_ItemMouseClick(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                //SelectedData.Skills[e.Clicks].Status = SkillStatus.Allocated;
                RefreshEditor();
            } else if (e.Button == MouseButtons.Right) {
                if (SelectedData.Skills[e.Clicks].Status == SkillStatus.Allocated) { SelectedData.Skills[e.Clicks].Status = SkillStatus.Unallocated; } 
                else { SelectedData.Skills[e.Clicks].Status = SkillStatus.Allocated; }; 
                RefreshEditor();
            }
        }

        private static float[] EditorZoomValues = { 0.01f, 0.025f, 0.05f, 0.075f, 0.1f, 0.25f, 0.5f, 0.75f, 1, 1.25f, 1.5f, 1.75f, 2, 2.5f, 3, 3.5f, 4, 5, 6, 7, 8, 9, 10, 15, 20, 25, 30, 40, 50, 60, 70, 80, 90, 100 };
        private void Passive_Editor_MouseWheel(object sender, MouseEventArgs e) {
            float previous = Scale;

            if (e.Delta < 0) {
                for (int i = EditorZoomValues.Length - 1; i >= 0; i--) { if (previous > EditorZoomValues[i]) { previous = EditorZoomValues[i]; break; } }
                Zoom(previous);
            } else {
                for (int i = 0; i < EditorZoomValues.Length; i++) { if (previous < EditorZoomValues[i]) { previous = EditorZoomValues[i]; break; } }
                Zoom(previous);
            }
            //Scale = (float)(Math.Round(Scale * 100) / 100);
        }
        #endregion

        public SelectType HitType = SelectType.None;
        public int HitIndex = -1;
        public bool SelectLabel = true, SelectSkill = true, SelectLine = true, SelectBackground = true;

        #region Editor Functions & Properties
        bool sss = true;
        public void DrawEditor(bool drawSkillNumber, bool drawSkills, bool drawLines, bool drawLabes, bool drawBackground) {
            //if (e.Keyboard.IsKeyPressed(SharpDX.Toolkit.Input.Keys.F12) == true) {
            //    SaveAsPng(drawSkillNumber, drawSkills, drawLines, drawLabes, drawBackground);
            //}
            //if (e.Keyboard.IsKeyPressed(SharpDX.Toolkit.Input.Keys.O) == true) sss = !sss;
            //SamplerState mainSamplerState;
            //if (sss == true) {
            //    mainSamplerState = e.SpriteBatch.GraphicsDevice.SamplerStates.PointWrap; //
            //} else {
            //    mainSamplerState = e.SpriteBatch.GraphicsDevice.SamplerStates.LinearWrap; //PointWrap
            //}

            SamplerState mainSamplerState = e.SpriteBatch.GraphicsDevice.SamplerStates.PointWrap;

            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.SpriteBatch.GraphicsDevice.BlendStates.NonPremultiplied, e.SpriteBatch.GraphicsDevice.SamplerStates.PointMirror);
            //Draw background reapeat
            e.SpriteBatch.Draw(PSS.Background.Image, new Rectangle(0, 0, GameSetting.Width, GameSetting.Height), new Rectangle((int)(Position.X / Scale), (int)(Position.Y / Scale), (int)(GameSetting.Width / Scale), (int)(GameSetting.Height / Scale)), Color.White);
            //Draw background
            if (drawBackground == true) {
                for (int i = 0; i < SelectedData.Backgrounds.Length; i++) { SelectedData.Backgrounds[i].Draw(e.SpriteBatch, Scale, Position); }
                if (drawSkillNumber == true) { for (int i = 0; i < SelectedData.Backgrounds.Length; i++) { DrawNumber(SelectedData.Backgrounds[i].Position, Scale, i.ToString()); } }
            }
            e.SpriteBatch.End();

            //Draw Line 
            if (drawLines == true) {
                e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.SpriteBatch.GraphicsDevice.BlendStates.NonPremultiplied, e.SpriteBatch.GraphicsDevice.SamplerStates.LinearWrap);
                for (int i = 0; i < SelectedData.Lines.Length; i++) { SelectedData.Lines[i].Draw(e.SpriteBatch, Scale, Position); }
                if (drawSkillNumber == true) { for (int i = 0; i < SelectedData.Lines.Length; i++) { DrawNumber(SelectedData.Lines[i].Position, Scale, i.ToString()); } }
                e.SpriteBatch.End();
            }

            //Draw Labels
            if (drawLabes == true) {
                e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.SpriteBatch.GraphicsDevice.BlendStates.NonPremultiplied, mainSamplerState);
                for (int i = 0; i < SelectedData.Labels.Length; i++) { SelectedData.Labels[i].Draw(Scale, Position); }
                e.SpriteBatch.End();
            }

            //Draw background Skill
            if (drawSkills == true) {
                e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.SpriteBatch.GraphicsDevice.BlendStates.Default, mainSamplerState);
                for (int i = 1; i < SelectedData.Skills.Length; i++) { SelectedData.Skills[i].DrawBackground(e.SpriteBatch, Scale, Position); }
                e.SpriteBatch.End();

                //Draw foreground Skill
                e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.SpriteBatch.GraphicsDevice.BlendStates.NonPremultiplied, mainSamplerState);
                for (int i = 1; i < SelectedData.Skills.Length; i++) { SelectedData.Skills[i].DrawCircle(e.SpriteBatch, Scale, Position); }
                if (drawSkillNumber == true) { for (int i = 1; i < SelectedData.Skills.Length; i++) { DrawNumber(SelectedData.Skills[i].Position, Scale, i.ToString()); } }
                e.SpriteBatch.End();
                
                //Draw Tootip
                if (Hover != -1) {
                    Vector2 sizeCaption = Fonts.FontinRegular11.MeasureString(SelectedData.Skills[Hover].Caption);
                    Vector2 sizeDescription = Fonts.FontinRegular11.MeasureString(SelectedData.Skills[Hover].Description);
                    Vector2 sizeLegend = Fonts.FontinItalic11.MeasureString(SelectedData.Skills[Hover].Legend);

                    int scrW, scrH;
                    if (sizeCaption.X > sizeDescription.X) { scrW = (int)sizeCaption.X; } else { scrW = (int)sizeDescription.X; }
                    if (sizeLegend.X > scrW) { scrW = (int)sizeLegend.X; }
                    scrH = (int)(sizeCaption.Y + sizeDescription.Y + sizeLegend.Y);
                    if (SelectedData.Skills[Hover].Legend != "") scrH += 5;

                    e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.SpriteBatch.GraphicsDevice.BlendStates.NonPremultiplied, mainSamplerState);

                    if (MouseMove_PointF.X + 20 + scrW > GameSetting.Width) { MouseMove_PointF.X = GameSetting.Width - 20 - scrW; }
                    if (MouseMove_PointF.Y + 20 + scrH > GameSetting.Height) { MouseMove_PointF.Y = GameSetting.Height - 20 - scrH; }

                    e.SpriteBatch.Draw(PSS.PopupBackground.Image, MouseMove_PointF + 10, new Rectangle(0, 0, scrW + 10, scrH + 10), Color.White * 0.8f);
                    e.SpriteBatch.DrawString(Fonts.FontinRegular11, SelectedData.Skills[Hover].Caption, MouseMove_PointF + 15, Color_Caption);
                    e.SpriteBatch.DrawString(Fonts.FontinRegular11, SelectedData.Skills[Hover].Description, MouseMove_PointF + 15 + new Vector2(0, sizeCaption.Y + 2), Color.White);
                    e.SpriteBatch.DrawString(Fonts.FontinItalic11, SelectedData.Skills[Hover].Legend, MouseMove_PointF + 15 + new Vector2(0, sizeCaption.Y + sizeDescription.Y + 5), Color.DarkOrange);
                    e.SpriteBatch.End();
                }
            }



            //Draw Rect

            e.SpriteBatch.Begin(SpriteSortMode.Deferred, null, mainSamplerState);

            Vector2 Virtual1 = new Vector2(SelectedData.Rect.X, SelectedData.Rect.Y) * Scale - Position;
            Vector2 Virtual2 = new Vector2(SelectedData.Rect.X + SelectedData.Rect.Width, SelectedData.Rect.Y) * Scale - Position;
            Vector2 Virtual3 = new Vector2(SelectedData.Rect.X + SelectedData.Rect.Width, SelectedData.Rect.Y + SelectedData.Rect.Height) * Scale - Position;
            Vector2 Virtual4 = new Vector2(SelectedData.Rect.X, SelectedData.Rect.Y + SelectedData.Rect.Height) * Scale - Position;

            e.SpriteBatch.Draw(PSS.Select.Image, new Rectangle((int)Virtual1.X, (int)Virtual1.Y, (int)(SelectedData.Rect.Width * Scale), 1), Color.White); //1-2
            e.SpriteBatch.Draw(PSS.Select.Image, new Rectangle((int)Virtual2.X, (int)Virtual2.Y, 1, (int)(SelectedData.Rect.Height * Scale)), Color.White); //2-3
            e.SpriteBatch.Draw(PSS.Select.Image, new Rectangle((int)Virtual4.X, (int)Virtual4.Y, (int)(SelectedData.Rect.Width * Scale), 1), Color.White); //4-3
            e.SpriteBatch.Draw(PSS.Select.Image, new Rectangle((int)Virtual1.X, (int)Virtual1.Y, 1, (int)(SelectedData.Rect.Height * Scale)), Color.White);//1-4

            e.SpriteBatch.End();
        }
        public void DrawNumber(Vector2 position, float Scale, string number) {
            Vector2 size = Fonts.FontinRegular50.MeasureString(number);
            Vector2 origin = new Vector2(size.X / 2, size.Y / 2);
            e.SpriteBatch.DrawString(Fonts.FontinRegular50, number, position * Scale - Position, Color.Red, 0, origin, Scale, SpriteEffects.None, 0);
        }
        public int RefreshEditor(ref string detail) {
            string Detail = ""; int Errors = 0;

            for (int i = 0; i < SelectedData.Lines.Length; i++) {
                if (SelectedData.Lines[i].SkillA >= SelectedData.Skills.Length) {
                    Detail += "Line[" + i + "]: " + "Error in SkillA. Can't Find Skills[" + SelectedData.Lines[i].SkillA + "]" + System.Environment.NewLine; Errors++;
                }
                if (SelectedData.Lines[i].SkillB >= SelectedData.Skills.Length) {
                    Detail += "Line[" + i + "]: " + "Error in SkillB. Can't Find Skills[" + SelectedData.Lines[i].SkillB + "]" + System.Environment.NewLine; Errors++;
                }

                if (SelectedData.Lines[i].SkillA < SelectedData.Skills.Length && SelectedData.Lines[i].SkillB < SelectedData.Skills.Length) { 
                    if (SelectedData.Skills[SelectedData.Lines[i].SkillA].Status == SkillStatus.Allocated && SelectedData.Skills[SelectedData.Lines[i].SkillB].Status == SkillStatus.Allocated) {
                        SelectedData.Lines[i].Status = SkillStatus.Allocated;
                    } else if (SelectedData.Skills[SelectedData.Lines[i].SkillA].Status == SkillStatus.Allocated || SelectedData.Skills[SelectedData.Lines[i].SkillB].Status == SkillStatus.Allocated) {
                        SelectedData.Lines[i].Status = SkillStatus.CanAllocated;
                    } else {
                        SelectedData.Lines[i].Status = SkillStatus.Unallocated;
                    }
                }
            }

            for (int i = 1; i < SelectedData.Skills.Length; i++) {
                if (SelectedData.Skills[i].Status == SkillStatus.Unallocated || SelectedData.Skills[i].Status == SkillStatus.CanAllocated ) {
                    SelectedData.Skills[i].Status = SkillStatus.Unallocated;
   
                    for (int x = 0; x < SelectedData.Skills[i].ConnectedSkills.Length; x++) {

                        if (SelectedData.Skills[i].ConnectedSkills[x] >= SelectedData.Skills.Length) {
                            Detail += "Skills[" + i + "]: " + "Error in ConnectedSkills, Can't Find Skills[" + SelectedData.Skills[i].ConnectedSkills[x] + "]" + System.Environment.NewLine; Errors++;
                        }

                        if (SelectedData.Skills[i].ConnectedSkills[x] < SelectedData.Skills.Length) { 
                            if (SelectedData.Skills[SelectedData.Skills[i].ConnectedSkills[x]].Status == SkillStatus.Allocated) {
                                SelectedData.Skills[i].Status = SkillStatus.CanAllocated;
                                break;
                            }    
                        }
                    }


                }
            }
            detail = Detail;
            return Errors;
        }
        public void RefreshEditor() {
            for (int i = 0; i < SelectedData.Lines.Length; i++) {
                if (SelectedData.Lines[i].SkillA < SelectedData.Skills.Length && SelectedData.Lines[i].SkillB < SelectedData.Skills.Length) { 
                    if (SelectedData.Skills[SelectedData.Lines[i].SkillA].Status == SkillStatus.Allocated && SelectedData.Skills[SelectedData.Lines[i].SkillB].Status == SkillStatus.Allocated) {
                        SelectedData.Lines[i].Status = SkillStatus.Allocated;
                    } else if (SelectedData.Skills[SelectedData.Lines[i].SkillA].Status == SkillStatus.Allocated || SelectedData.Skills[SelectedData.Lines[i].SkillB].Status == SkillStatus.Allocated) {
                        SelectedData.Lines[i].Status = SkillStatus.CanAllocated;
                    } else {
                        SelectedData.Lines[i].Status = SkillStatus.Unallocated;
                    }
                }
            }

            for (int i = 1; i < SelectedData.Skills.Length; i++) {
                if (SelectedData.Skills[i].Status == SkillStatus.Unallocated || SelectedData.Skills[i].Status == SkillStatus.CanAllocated ) {
                    SelectedData.Skills[i].Status = SkillStatus.Unallocated;
   
                    for (int x = 0; x < SelectedData.Skills[i].ConnectedSkills.Length; x++) {

                        if (SelectedData.Skills[i].ConnectedSkills[x] < SelectedData.Skills.Length) { 
                            if (SelectedData.Skills[SelectedData.Skills[i].ConnectedSkills[x]].Status == SkillStatus.Allocated) {
                                SelectedData.Skills[i].Status = SkillStatus.CanAllocated;
                                break;
                            }    
                        }
                    }


                }
            }
        }

        public void SaveAsPng(bool drawSkillNumber, bool drawSkills, bool drawLines, bool drawLabes, bool drawBackground) {
            SamplerState mainSamplerState = e.SpriteBatch.GraphicsDevice.SamplerStates.PointWrap;
            
            RenderTarget2D s = RenderTarget2D.New(e.GraphicsDevice, 1000, 1000, PixelFormat.B8G8R8A8.UNorm, TextureFlags.ShaderResource | TextureFlags.RenderTarget);
            e.GraphicsDevice.Clear(Color.Transparent);

            e.GraphicsDevice.SetRenderTargets(s);

            Vector2 position = new Vector2(-600, -600);

            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.SpriteBatch.GraphicsDevice.BlendStates.AlphaBlend, e.SpriteBatch.GraphicsDevice.SamplerStates.PointMirror);
            //Draw background reapeat
            //e.SpriteBatch.Draw(PSS.Background.Image, new Rectangle(0, 0, GameSetting.Width, GameSetting.Height), 
                //new Rectangle(0, 0, (int)(GameSetting.Width / Scale), (int)(GameSetting.Height / Scale)), Color.White);
            //Draw background
            if (drawBackground == true) {
                for (int i = 0; i < SelectedData.Backgrounds.Length; i++) { SelectedData.Backgrounds[i].Draw(e.SpriteBatch, Scale, position); }
                if (drawSkillNumber == true) { for (int i = 0; i < SelectedData.Backgrounds.Length; i++) { DrawNumber(SelectedData.Backgrounds[i].Position, Scale, i.ToString()); } }
            }
            e.SpriteBatch.End();

            //Draw Line 
            if (drawLines == true) {
                e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.SpriteBatch.GraphicsDevice.BlendStates.AlphaBlend, e.SpriteBatch.GraphicsDevice.SamplerStates.LinearWrap);
                for (int i = 0; i < SelectedData.Lines.Length; i++) { SelectedData.Lines[i].Draw(e.SpriteBatch, Scale, position); }
                if (drawSkillNumber == true) { for (int i = 0; i < SelectedData.Lines.Length; i++) { DrawNumber(SelectedData.Lines[i].Position, Scale, i.ToString()); } }
                e.SpriteBatch.End();
            }

            //Draw Labels
            if (drawLabes == true) {
                e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.SpriteBatch.GraphicsDevice.BlendStates.NonPremultiplied, mainSamplerState);
                for (int i = 0; i < SelectedData.Labels.Length; i++) { SelectedData.Labels[i].Draw(Scale, position); }
                e.SpriteBatch.End();
            }

            //Draw background Skill
            if (drawSkills == true) {
                e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.SpriteBatch.GraphicsDevice.BlendStates.AlphaBlend, mainSamplerState);
                for (int i = 1; i < SelectedData.Skills.Length; i++) { SelectedData.Skills[i].DrawBackground(e.SpriteBatch, Scale, position); }
                e.SpriteBatch.End();

                //Draw foreground Skill
                e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.SpriteBatch.GraphicsDevice.BlendStates.AlphaBlend, mainSamplerState);
                for (int i = 1; i < SelectedData.Skills.Length; i++) { SelectedData.Skills[i].DrawCircle(e.SpriteBatch, Scale, position); }
                if (drawSkillNumber == true) { for (int i = 1; i < SelectedData.Skills.Length; i++) { DrawNumber(SelectedData.Skills[i].Position, Scale, i.ToString()); } }
                e.SpriteBatch.End();
            }
            
            s.Save(@"D:\123skills.png", ImageFileType.Png);
            s.Dispose();
        }

        //Property use for Editor
        [Category("Size")] public int Tree_X { get { return SelectedData.Rect.X; } set { SelectedData.Rect.X = value; } }
        [Category("Size")] public int Tree_Y { get { return SelectedData.Rect.Y; } set { SelectedData.Rect.Y = value; } }
        [Category("Size")] public int Tree_Width { get { return SelectedData.Rect.Width; } set { SelectedData.Rect.Width = value; } }
        [Category("Size")] public int Tree_Height { get { return SelectedData.Rect.Height; } set { SelectedData.Rect.Height = value; } }

        #endregion
    }
    
    public class PassiveTreeData {
        public int Version;
        public _Rect Rect = new _Rect(-2000, -2000, 4000, 4000);
        public Background[] Backgrounds = { };
        public Line[] Lines = { };
        public PassiveSkill[] Skills = { };
        public Label[] Labels = { };

        public static PassiveTreeData New(string file) {
            PassiveTreeData Return = new PassiveTreeData();
            Return.Load(file);
            return Return;
        }

        #region Add & Remove
        public void AddSkill(PassiveSkill item) {
            Array.Resize(ref Skills, Skills.Length + 1);
            Skills[Skills.Length - 1] = item;
        }
        public void AddLine(Line item) {
            Array.Resize(ref Lines, Lines.Length + 1);
            Lines[Lines.Length - 1] = item;
        }
        public void AddLabel(Label item) {
            Array.Resize(ref Labels, Labels.Length + 1);
            Labels[Labels.Length - 1] = item;
        }
        public void AddBackground(Background item) {
            Array.Resize(ref Backgrounds, Backgrounds.Length + 1);
            Backgrounds[Backgrounds.Length - 1] = item;
        }
        public void RemoveSkill(int index) {
            Array.Copy(Skills, index + 1, Skills, index, Skills.Length - index - 1);
            Array.Resize(ref Skills, Skills.Length - 1);
        }
        public void RemoveLine(int index) {
            Array.Copy(Lines, index + 1, Lines, index, Lines.Length - index - 1);
            Array.Resize(ref Lines, Lines.Length - 1);
        }
        public void RemoveLabel(int index) {
            Array.Copy(Labels, index + 1, Labels, index, Labels.Length - index - 1);
            Array.Resize(ref Labels, Labels.Length - 1);
        }
        public void RemoveBackground(int index) {
            Array.Copy(Backgrounds, index + 1, Backgrounds, index, Backgrounds.Length - index - 1);
            Array.Resize(ref Backgrounds, Backgrounds.Length - 1);
        }
        #endregion

        public void Load(string file) {
            ValueType TMP_ValueType = new PassiveTreeDataVersion1();
            FileSystem.FileOpen(3, file, OpenMode.Binary, OpenAccess.Default);
            FileSystem.FileGet(3, ref TMP_ValueType);
            FileSystem.FileClose(3);

            PassiveTreeDataVersion1 TMP = (PassiveTreeDataVersion1)TMP_ValueType;
            Rect = TMP.Rect; Version = TMP.Version;
            Skills = TMP.Skills; Lines = TMP.Lines; Labels = TMP.Labels; Backgrounds = TMP.Backgrounds;

            for (int i = 0; i < Backgrounds.Length; i++) { Backgrounds[i].Init(); }
            for (int i = 0; i < Lines.Length; i++) { Lines[i].Init(); }
            for (int i = 0; i < Skills.Length; i++) { Skills[i].Init(); }
        }
        public void Save(string file) {
            PassiveTreeDataVersion1 TMP = new PassiveTreeDataVersion1();
            TMP.Version = 1;
            TMP.Rect = Rect; TMP.Skills = Skills; TMP.Lines = Lines; TMP.Labels = Labels; TMP.Backgrounds = Backgrounds; 
            FileSystem.FileOpen(2, file, OpenMode.Binary, OpenAccess.Default);
            FileSystem.FilePut(2, TMP);
            FileSystem.FileClose(2);
            
        }

        private struct UseToSavePassiveTree {
            public _Rect Rect;
            public Background[] Backgrounds;
            public Line[] Lines;
            public PassiveSkill[] Skills;
        }
        private struct PassiveTreeDataVersion1 {
            public int Version;
            public _Rect Rect;
            public PassiveSkill[] Skills;
            public Line[] Lines;
            public Label[] Labels;
            public Background[] Backgrounds;
        }


    }



    public struct Background {
        public string ImageName;
        public Vector2 Position;
        public SpriteEffects Effect;


        private int Index;
        public Background(string imageName, Vector2 position, SpriteEffects effect) {
            ImageName = imageName; Position = position; Effect = effect;
            Index = PSS.ScreenBackGround.GetIndex(ImageName);    
        }
        public void Draw(SpriteBatch spriteBatch, float Scale, Vector2 position) {
            spriteBatch.Draw(PSS.ScreenBackGround.Images[Index],
                Position * Scale - position, PSS.ScreenBackGround.Rects[Index], Color.White, 0, PSS.ScreenBackGround.Origins[Index], Scale, Effect, 0);
        }
        public void Init() {
            Index = PSS.ScreenBackGround.GetIndex(ImageName);
        }
    }
    public struct Label {
        public Vector2 Position;
        public string Text;
        public Color Color;
        public float Alpha, Scale;        
        public Label(Vector2 position, string text, Color color, float alpha, float scale) {
            Position = position; Text = text; Color = color; Alpha = alpha; Scale = scale;
        }

        public void Draw(float scale, Vector2 position) {
            Vector2 Size_Text = Fonts.FontinRegular25.MeasureString(Text);
            e.SpriteBatch.DrawString(Fonts.FontinRegular25, Text, Position * scale - position, Color, Alpha, Size_Text / 2, Scale * scale, SpriteEffects.None, 0);
        }
    }
    public struct Line {
        public int SkillA;
        public int SkillB;
        public Vector2 Position;
        public int Width;
        public float Alpha;
        public SkillStatus Status;
        public LineStyle Style;


        private Rectangle SourceRect;
        public Line(int A, int B, Vector2 position, int width, float alpha, LineStyle style){
            SkillA = A; SkillB = B; Position = position; Width = width;
            Alpha = alpha; Style = style; Status = SkillStatus.Unallocated;
            if (Style == LineStyle.Line) { SourceRect = PSS.Connect.Line.Rect; }
            else if (Style == LineStyle.Tiny_Circle) { SourceRect = PSS.Connect.TinyCircle.Rect; }
            else if (Style == LineStyle.Small_Circle) { SourceRect = PSS.Connect.SmallCircle.Rect; }
            else if (Style == LineStyle.Medium_Circle) { SourceRect = PSS.Connect.MediumCircle.Rect; }
            else { SourceRect = PSS.Connect.LargeCircle.Rect; }
            SourceRect.Width = width ;
        }
        public void Init() {
            if (Style == LineStyle.Line) { SourceRect = PSS.Connect.Line.Rect; }
            else if (Style == LineStyle.Tiny_Circle) { SourceRect = PSS.Connect.TinyCircle.Rect; }
            else if (Style == LineStyle.Small_Circle) { SourceRect = PSS.Connect.SmallCircle.Rect; }
            else if (Style == LineStyle.Medium_Circle) { SourceRect = PSS.Connect.MediumCircle.Rect; }
            else { SourceRect = PSS.Connect.LargeCircle.Rect; }
            SourceRect.Width = Width;
        }
        public void Draw(SpriteBatch spriteBatch, float Scale, Vector2 position) {
            PSS.Connect.ConnectBase connect = null;
            RenderTarget2D image = null;

            if (Style == LineStyle.Line) {
                connect = PSS.Connect.Line;
            } else if( Style ==  LineStyle.Tiny_Circle) {
                connect = PSS.Connect.TinyCircle;
            } else if( Style ==  LineStyle.Small_Circle) {
                connect = PSS.Connect.SmallCircle;
            } else if( Style ==  LineStyle.Medium_Circle) {
                connect = PSS.Connect.MediumCircle;
            } else if( Style ==  LineStyle.Large_Circle) {
                connect = PSS.Connect.LargeCircle;
            }

            if (Status == SkillStatus.Unallocated) {
                image = connect.NormalImage;
            } else if (Status == SkillStatus.CanAllocated) {
                image = connect.CanAllocateImage;
            } else if (Status == SkillStatus.Allocated) {
                image = connect.ActiveImage;
            }

            spriteBatch.Draw(image, Position * Scale - position, SourceRect, Color.White, Alpha, connect.Origin, Scale, SpriteEffects.None, 0);
        }
    }
    public struct PassiveSkill {
        public Property[] Properties;
        public string Caption;
        public string Description;
        public string Legend;

        public int[] ConnectedSkills;
        public String ImageName;
        public Vector2 Position;
        public SkillStatus Status;
        public SkillSize Size;

        private Texture2D Image;
        public PassiveSkill(string imageName, Vector2 position, SkillStatus status, SkillSize size,params int[] connectedSkill) {
            Properties = new Property[0];
            ConnectedSkills = connectedSkill;
            ImageName = imageName;
            Image = PSS.Skills.GetImage(ImageName);
            Position = position;
            Status = status;
            Size = size;
            Caption = "";
            Description = "";
            Legend = "";
        }
        public void DrawBackground(SpriteBatch spriteBatch, float Scale, Vector2 position) {
            float color = 1, ScaleImage = 1;
            if (Status == SkillStatus.CanAllocated || Status == SkillStatus.Unallocated) { color = 0.7f; }

            if (Size == SkillSize.Small && Image.Width == 128) { ScaleImage = 0.5f; } 
            else if (Size == SkillSize.Medium && Image.Width == 128) { ScaleImage = 0.75f; }
            else if (Size == SkillSize.Medium && Image.Width == 64) { ScaleImage = 1.5f; }
            else if (Size == SkillSize.Large && Image.Width == 64) { ScaleImage = 2; }

            spriteBatch.Draw(Image, Position * Scale - position, new Rectangle(0, 0, Image.Width, Image.Height), Color.White * color, 0, new Vector2(Image.Width / 2, Image.Height / 2), Scale * ScaleImage, SpriteEffects.None, 0);
        }
        public void DrawCircle(SpriteBatch spriteBatch, float Scale, Vector2 position) { 
            if (Size == SkillSize.Small) {
                if (Status == SkillStatus.Unallocated) {
                    spriteBatch.Draw(PSS.PassiveFrame.Image, Position * Scale - position, PSS.PassiveFrame.NormalRect, Color.White, 0, PSS.PassiveFrame.Origin, Scale, SpriteEffects.None, 0);
                } else if (Status == SkillStatus.CanAllocated) {
                    spriteBatch.Draw(PSS.PassiveFrame.Image, Position * Scale - position, PSS.PassiveFrame.CanAllocateRect, Color.White, 0, PSS.PassiveFrame.Origin, Scale, SpriteEffects.None, 0);
                } else if (Status == SkillStatus.Allocated) {
                    spriteBatch.Draw(PSS.PassiveFrame.Image, Position * Scale - position, PSS.PassiveFrame.ActiveRect, Color.White, 0, PSS.PassiveFrame.Origin, Scale, SpriteEffects.None, 0);
                }
            }

            else if (Size == SkillSize.Medium) {
                if (Status == SkillStatus.Unallocated) {
                    spriteBatch.Draw(PSS.NotableFrame.NormalImage, Position * Scale - position, PSS.NotableFrame.NormalRect, Color.White, 0, PSS.NotableFrame.Origin, Scale, SpriteEffects.None, 0); 
                } else if (Status == SkillStatus.CanAllocated) {
                    spriteBatch.Draw(PSS.NotableFrame.CanAllocateImage, Position * Scale - position, PSS.NotableFrame.CanAllocateRect, Color.White, 0, PSS.NotableFrame.Origin, Scale, SpriteEffects.None, 0);
                } else if (Status == SkillStatus.Allocated) {
                    spriteBatch.Draw(PSS.NotableFrame.ActiveImage, Position * Scale - position, PSS.NotableFrame.ActiveRect, Color.White, 0, PSS.NotableFrame.Origin, Scale, SpriteEffects.None, 0);
                }
            }

            else if (Size == SkillSize.Large) {
                if (Status == SkillStatus.Unallocated) {
                    spriteBatch.Draw(PSS.Keystone.Image, Position * Scale - position, PSS.Keystone.NormalRect, Color.White, 0, PSS.Keystone.Origin, Scale, SpriteEffects.None, 0); 
                } else if (Status == SkillStatus.CanAllocated) {
                    spriteBatch.Draw(PSS.Keystone.Image, Position * Scale - position, PSS.Keystone.CanAllocateRect, Color.White, 0, PSS.Keystone.Origin, Scale, SpriteEffects.None, 0);
                } else if (Status == SkillStatus.Allocated) {
                    spriteBatch.Draw(PSS.Keystone.Image, Position * Scale - position, PSS.Keystone.ActiveRect, Color.White, 0, PSS.Keystone.Origin, Scale, SpriteEffects.None, 0);
                }
            }
        }
        
        public void Init() {
            Image = PSS.Skills.GetImage(ImageName);
            if (Caption == null) { Caption = ""; }
            if (Description == null) { Description = ""; }
            if (Legend == null) { Legend = ""; }
        }
    }

    public enum SkillStatus { Unallocated = 0, CanAllocated = 1, Allocated = 2 }
    public enum SkillSize { Small = 0, Medium = 1, Large = 2 }
    public enum LineStyle { Line = 0, Tiny_Circle = 1, Small_Circle = 2, Medium_Circle = 3, Large_Circle = 4 }

    public struct Property {
        public TypeProperty Type;
        public float[] Value;
    }

    public enum TypeProperty {
        None = 0, Strength = 1, Dexterity = 2, Intelligence = 3, Attributes = 4,

        Life = 10, Increased_Life = 11, Life_Regeneration = 12, Life_Regeneration_Percent = 14, Life_on_Kill = 16, Life_on_Hit = 18,
        Mana = 40, Increased_Mana = 41, Mana_Regeneration = 42, Mana_Regeneration_Percent = 44, Mana_on_Kill = 46, Mana_on_Hit = 48, Increased_Mana_Regeneration = 50, Reduced_Mana_Cost = 52, Reduced_Cooldown = 54,
        Energy = 70, Increased_Energy = 71, Faster_Start_of_Energy_Recharge = 73,
        //Energy_Shield_Maximum = 70,

        Chance_Block = 100,
        Armor = 110, Increased_Armor = 112,
        Fire_Resistance = 140, Cold_Resistance = 141, Lightning_Resistance = 142, Chaos_Resistance = 143,

        Increased_Attack_Speed = 150, Increased_Cast_Speed = 152, Increased_Duration = 154, Increased_Buff_Duration = 155 , Increased_AOE = 156,
        Increased_Critical_Strike_Chance = 158, Increased_Critical_Strike_Multiplier = 160,
        Increased_Critical_Strike_Chance_For_Spells = 162, Increased_Critical_Strike_Multiplier_For_Spells = 164,

        #region Physical Damage
        Increased_Physical_Damage = 200, 
        Reduced_Enemy_Stun_Threshold = 202, 
        Increased_Stun_Duration = 204,
        #endregion
        Increased_Fire_Damage = 300, Chance_Ignite = 302, Increased_Ignite_Duration = 304,
        Increased_Cold_Damage = 400, Chance_Freeze = 402, Increased_Freeze_Duration = 404, Increased_Chill_Duration = 406,
        Increased_Lightning_Damage = 500, Chance_Shock = 502, Increased_Shock_Duration = 504, 
        Increased_Spell_Damage = 590,
        Increased_Chaos_Damage = 600, 

        Chance_Knockback = 700, Knockback_Distance = 702,
        Increased_Knockback_Distance = 704, Knockback_on_Crit = 706, 
        
        Projectiles = 720,

        //Skills
        #region Split Arrow
        Split_Arrow = 1000,
        Split_Arrow_Multiplier_Weapon_Damage = 1001,
        Split_Arrow_Multiplier_Damage = 1002,
        Split_Arrow_Mana_Cost = 1010,
        Split_Arrow_Additional_Projectiles = 1020,
        Split_Arrow_Increased_Attack_Speed = 1030,
        #endregion
        #region Firestorm Ver 0.1 full damage
        Firestorm = 2000,
        Firestorm_Physical_Damage = 2002,
        Firestorm_Fire_Damage = 2003, Firestorm_Chance_Ignite = 2302, Firestorm_Increased_Ignite_Duration = 2304,
        Firestorm_Cold_Damage = 2004,
        Firestorm_Lightning_Damage = 2005,
        Firestorm_Chaos_Damage = 2006,

        Firestorm_Mana_Cost = 2010,
        Firestorm_Duration = 2038, Firestorm_Increased_Duration = 2040, Firestorm_Increased_AOE = 2050,
        Firestorm_Interval = 2060,
        Firestorm_Critical_Strike_Chance = 2062,
        #endregion

        #region Vitality
        Vitality = 3000,
        Vitality_Duration = 3030 , Vitality_Increased_Duration = 3040,
        Vitality_Life_Regeneration = 3100, Vitality_Life_Regeneration_Percent = 3110,
        Vitality_Mana_Cost = 3010,
        #endregion

        FrostNova = 4000,
        FrostNova_Cold_Damage = 4004, FrostNova_Chance_Freeze = 4402, FrostNova_Increased_Freeze_Duration = 4404, FrostNova_Increased_Chill_Duration = 4406,
        FrostNova_Increased_Radius = 4050,

        #region Arc
        Arc = 5000,
        Arc_Mana_Cost = 5001, Arc_Increased_Mana_Cost = 5002, 
        Arc_Lightning_Damage = 5005,
        Arc_Critical_Strike_Chance = 5062,
        Arc_Increased_Cast_Speed = 5152,
        Arc_Increased_Lightning_Damage = 5500, Arc_Chance_Shock = 5502, Arc_Increased_Shock_Duration = 5504,
        Arc_Chain = 5900,
        
        #endregion

        #region Mana Shield
        Mana_Shield = 6000,
        Mana_Shield_Absorbs_Percent = 6010,
        Mana_Shield_Damage_Absorbed_Per_Mana = 6020,
        #endregion

        #region Barrage
        Barrage = 7000,
        Barrage_Arrow_Multiplier_Weapon_Damage = 7001,
        Barrage_Arrow_Multiplier_Damage = 7002,
        Barrage_Arrow_Mana_Cost = 7010,
        Barrage_Additional_Projectiles = 7020,
        #endregion

        #region Spectral Throw
        Spectral_Throw = 8000,
        Spectral_Throw_Multiplier_Weapon_Damage = 8001,
        Spectral_Throw_Multiplier_Damage = 8002,
        Spectral_Throw_Mana_Cost = 8010,
        Spectral_Throw_Additional_Projectiles = 8020
        #endregion
    }
}
