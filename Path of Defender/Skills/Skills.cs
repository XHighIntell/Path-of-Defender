using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Graphics;

namespace Path_of_Defender {
    using UI = Images.UI;
    using buff = Images.Skill.Buff;
    
    //Graphics
    public abstract partial class Skill {
        public static float Margin_Draw_Desc = 4;
        public static float Margin_Draw_Sub = 4;
        public static Color Base_Border_Color = new Color(136, 98, 59);
        public static Color Base_Background_Color = new Color(0, 0, 0, 200);
        public static Color Base_Properties_Color = new Color(135, 135, 254);
        public static Color DisabledColor = new Color(100, 100, 100);
        public static float Rotation; 
        internal PlayerStats o;
        public static void DrawInfo(Texture2D Icon, string Name, string[] Properties, string[] Values, string Desc, string[] SubProperties) {
            RectangleF Rect = new RectangleF(); Rect.Width = 400;
            Vector2 Size_String;
            

            // ICON = 36 + 
            Rect.Height = 36 + Margin_Draw_Desc * 2 + Fonts.FontinRegular11.MeasureString(Desc).Y + Margin_Draw_Sub * 2 + SubProperties.Length * Fonts.FontinRegular9.MeasureString(" ").Y;
            Rect.X = e.Mouse_Position.X - Rect.Width / 2;
            Rect.Y = e.Mouse_Position.Y - Rect.Height;

            if (Rect.X + Rect.Width + 1 > GameSetting.Width) { Rect.X = GameSetting.Width - Rect.Width - 1; }
                        
            //e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied);
            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied, e.GraphicsDevice.SamplerStates.PointMirror);
            //Draw Background & Drawborder
            e.SpriteBatch.Draw(Images.VirtualControl.ImageWhitePoint, new RectangleF(Rect.X, Rect.Y, Rect.Width, Rect.Height), Base_Background_Color);
            e.SpriteBatch.DrawRectangle(Rect.X, Rect.Y, Rect.Width, Rect.Height, Base_Border_Color);

            //e.SpriteBatch.End();
            //e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied);
            e.SpriteBatch.Draw(Icon, new RectangleF(Rect.X + 1, Rect.Y + 1, 36, 36), Color.White);

            Size_String = Fonts.FontinRegular15.MeasureString(Name.ToUpper()); Size_String.X = 0;
            e.SpriteBatch.DrawString(Fonts.FontinRegular15, Name.ToUpper(), new Vector2(Rect.X + 32 + 16, Rect.Y + 17), Color.White, 0, Size_String / 2, 1, SpriteEffects.None, 0);

            RectangleF FocusedRect = new RectangleF(Rect.X + Rect.Width, Rect.Y, 0, 37);

            for (int i = 0; i < Properties.Length; i++) {
                //Properties[i] Values[i]
                Size_String = Fonts.FontinRegular10.MeasureString(Properties[i]);
                FocusedRect.X -= Size_String.X + 10; FocusedRect.Width = Size_String.X + 10;

                e.SpriteBatch.DrawString(Fonts.FontinRegular10, Properties[i], new Vector2(FocusedRect.X + FocusedRect.Width / 2, FocusedRect.Y + Size_String.Y / 2 + 2),
                    Color.White, 0, Size_String / 2, 1, SpriteEffects.None, 0);

                Size_String = Fonts.FontinRegular10.MeasureString(Values[i]);
                e.SpriteBatch.DrawString(Fonts.FontinRegular10, Values[i], new Vector2(FocusedRect.X + FocusedRect.Width / 2, FocusedRect.Y + 25), 
                    Color.White, 0, Size_String / 2, 1, SpriteEffects.None, 0);

                e.SpriteBatch.DrawLineH(FocusedRect.X, FocusedRect.Y, 37, Base_Border_Color);
            }
            e.SpriteBatch.DrawLineW(Rect.X, Rect.Y + 37, Rect.Width, Base_Border_Color);

            //Draw Desc of Skill
            string[] Lines = Microsoft.VisualBasic.Strings.Split(Desc, "\r\n");
            Size_String = Fonts.FontinRegular11.MeasureString(Desc);
            FocusedRect = new RectangleF(Rect.X, Rect.Y + 37, Rect.Width, Size_String.Y + Margin_Draw_Desc * 2);

            e.SpriteBatch.DrawStringCenter(Fonts.FontinItalic11, Lines, FocusedRect, Margin_Draw_Desc, Color.DarkOrange, false);
            e.SpriteBatch.DrawLineW(FocusedRect.X, FocusedRect.Y + FocusedRect.Height, FocusedRect.Width, Base_Border_Color);

            //Sub Properties
            FocusedRect.Y = FocusedRect.Y + FocusedRect.Height;
            e.SpriteBatch.DrawStringCenter(Fonts.FontinRegular9, SubProperties, FocusedRect, Margin_Draw_Sub, Base_Properties_Color, true);
            e.SpriteBatch.End();
        }
        ///<summary>Draw Icon on shortcut skills bar.</summary>
        public virtual void DrawIcon(int x, int y, int w, int h) {
            if (Icon == null) { return; }

            if (e.Player.Mana >= GetManaRequired()) {
                e.SpriteBatch.Draw(Icon, new Rectangle(x, y, w, h), Color.White); 
            } else { 
                e.SpriteBatch.Draw(Icon, new Rectangle(x, y, w, h), DisabledColor); 
            }

            if (Cooldown > 0) { e.SpriteBatch.FillRectangle(x, y, w, (int)(h * Cooldown / GetCooldown()), Color.Red * 0.5f); }

            if (Active == true) {
                e.SpriteBatch.Draw(buff.ImageActive, new Vector2(x + w / 2, y + h / 2), buff.RectActive, Color.White, Rotation, buff.OriginActive, 0.5f, SpriteEffects.None, 0);
            }
        }
        public abstract void DrawInfo();
    }


    public enum AbilityType { Attack = 0, Spell = 1, Toggle = 2 }
    public abstract partial class Skill {
        public Texture2D Icon;
        public AbilityType Ability;
        public Skill(Texture2D icon, AbilityType type) { Icon = icon; Ability = type; o = e.Player.Stats; }

        public float Cooldown;
        public bool Active;

        public virtual void Update() {
            if (Cooldown > 0) { Cooldown -= GameSetting.SecondPerFrame; }
            if (Cooldown < 0) { Cooldown = 0; }
        }
        public virtual float GetCooldown() { return 0; }
        public virtual float GetManaRequired() { return 0; }
        public virtual float GetCastTime() { return 0; }
        public virtual void Cast() {
            e.Player.Mana -= GetManaRequired();
            e.Player.TimeAttackRemain += GetCastTime();
            Cooldown = GetCooldown();
        }
    }
}

