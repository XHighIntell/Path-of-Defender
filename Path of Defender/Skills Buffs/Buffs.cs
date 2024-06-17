using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Graphics;

namespace Path_of_Defender {
    using buff = Images.Skill.Buff;
    public class BuffCollection { 
        public Buff[] Items = { };
        public int Count { get { return Items.Length; } }
        public void Add(Buff item) {
            Array.Resize(ref Items, Items.Length + 1);
            Items[Items.Length - 1] = item;
        }
        public void RemoveAt(int index) {
            Array.Copy(Items, index + 1, Items, index, Items.Length - index - 1);
            Array.Resize(ref Items, Items.Length - 1);
        }
        public void Clear() { Array.Resize(ref  Items, 0); }

        public void Update() {
            for (int i = 0; i < Items.Length; i++) {
                Items[i].Update();
                if (Items[i].Deletable == true) { RemoveAt(i); i--; }
            }
        }
        public void Draw() {
            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied, e.GraphicsDevice.SamplerStates.PointWrap);
            for (int i = 0; i < Items.Length; i++) {
                Items[i].Draw(i);
            }
            e.SpriteBatch.End();
        }
        
        public int IndexOf(Type type) {
            for (int i = 0; i < Items.Length; i++) {
                if (Items[i].GetType() == type) { return i; }
            }
            return -1;
        }
        public int CountIndexOf(Type type) {
            int count = 0;
            for (int i = 0; i < Items.Length; i++) { if (Items[i].GetType() == type) { count ++; } }
            return count;
        }
    }

    public enum BuffType { Buff = 0, Debuff = 1, Charge = 2 }
    public abstract class Buff {
        public Buff() { o = e.Player.Stats; }
        public Buff(Texture2D icon) { Icon = icon; o = e.Player.Stats; UseIcon = true; }

        public Texture2D Icon;
        public bool UseIcon;
        public PlayerStats o;
        public float Duration, Value;


        public bool Deletable;
        public virtual void Draw(int Index) { }
        public virtual void Update() { }
        public virtual float TakeDamage(float Final) { return Final; }

        public static void Draw(int Index, Texture2D Icon, BuffType Type) {
            Vector2 Position = new Vector2(30 + (4 + 48) * Index, 30);
            Rectangle Rect = new Rectangle(5 + (4 + 48) * Index, 5, 48, 47);

            e.SpriteBatch.Draw(Icon, Position, new Rectangle(0, 0, 64, 64), Color.White, 0, buff.OriginSkill, 0.5f, SpriteEffects.None, 0);

            if (Type == BuffType.Buff) { e.SpriteBatch.Draw(buff.ImageBuff, Position, buff.RectBuff, Color.White, 0, buff.OriginBuff, 0.5f, SpriteEffects.None, 0); }
            else if (Type == BuffType.Debuff) { e.SpriteBatch.Draw(buff.ImageDebuff, Position, buff.RectDebuff, Color.White, 0, buff.OriginBuff, 0.5f, SpriteEffects.None, 0); }
            else if (Type == BuffType.Charge) { e.SpriteBatch.Draw(buff.ImageCharge, Position, buff.RectCharge, Color.White, 0, buff.OriginBuff, 0.5f, SpriteEffects.None, 0); }
        }
        public static void Draw(int Index, Texture2D Icon, BuffType Type, string Duration) {
            Vector2 Position = new Vector2(30 + (4 + 48) * Index, 30);
            Rectangle Rect = new Rectangle(5 + (4 + 48) * Index, 5, 48, 47);

            e.SpriteBatch.Draw(Icon, Position, new Rectangle(0, 0, 64, 64), Color.White, 0, buff.OriginSkill, 0.5f, SpriteEffects.None, 0);

            if (Type == BuffType.Buff) { e.SpriteBatch.Draw(buff.ImageBuff, Position, buff.RectBuff, Color.White, 0, buff.OriginBuff, 0.5f, SpriteEffects.None, 0); }
            else if (Type == BuffType.Debuff) { e.SpriteBatch.Draw(buff.ImageDebuff, Position, buff.RectDebuff, Color.White, 0, buff.OriginBuff, 0.5f, SpriteEffects.None, 0); }
            else if (Type == BuffType.Charge) { e.SpriteBatch.Draw(buff.ImageCharge, Position, buff.RectCharge, Color.White, 0, buff.OriginBuff, 0.5f, SpriteEffects.None, 0); }
            Vector2 Size_String;

            Position.Y += 31;
            e.SpriteBatch.Draw(buff.ImageTimer, Position, buff.RectTimer, Color.White, 0, buff.OriginTimer, 0.5f, SpriteEffects.None, 0);
            Size_String = Fonts.FontinRegular9.MeasureString(Duration);
            Position.Y -= 2;
            e.SpriteBatch.DrawString(Fonts.FontinRegular9, Duration, Position - Size_String / 2, Color.White);
        }
        public static void Draw(int Index, Texture2D Icon, BuffType Type, string Duration, string Value) {
            Vector2 Position = new Vector2(30 + (4 + 48) * Index, 30);
            Rectangle Rect = new Rectangle(5 + (4 + 48) * Index, 5, 48, 47);

            e.SpriteBatch.Draw(Icon, Position, new Rectangle(0, 0, 64, 64), Color.White, 0, buff.OriginSkill, 0.5f, SpriteEffects.None, 0);

            if (Type == BuffType.Buff) { e.SpriteBatch.Draw(buff.ImageBuff, Position, buff.RectBuff, Color.White, 0, buff.OriginBuff, 0.5f, SpriteEffects.None, 0); }
            else if (Type == BuffType.Debuff) { e.SpriteBatch.Draw(buff.ImageDebuff, Position, buff.RectDebuff, Color.White, 0, buff.OriginBuff, 0.5f, SpriteEffects.None, 0); }
            else if (Type == BuffType.Charge) { e.SpriteBatch.Draw(buff.ImageCharge, Position, buff.RectCharge, Color.White, 0, buff.OriginBuff, 0.5f, SpriteEffects.None, 0); }
            Vector2 Size_String;

            Position.Y += 31;
            e.SpriteBatch.Draw(buff.ImageTimer, Position, buff.RectTimer, Color.White, 0, buff.OriginTimer, 0.5f, SpriteEffects.None, 0);
            Size_String = Fonts.FontinRegular9.MeasureString(Duration);
            Position.Y -= 2;
            e.SpriteBatch.DrawString(Fonts.FontinRegular9, Duration, Position - Size_String / 2, Color.White);

            Position.Y += 16;
            e.SpriteBatch.Draw(buff.ImageNumber, Position, buff.RectNumber, Color.White, 0, buff.OriginNumber, 0.5f, SpriteEffects.None, 0);
            Size_String = Fonts.FontinRegular9.MeasureString(Value);
            Position.Y -= 2;
            e.SpriteBatch.DrawString(Fonts.FontinRegular9, Value, Position - Size_String / 2, Color.White);
        }
    }
}
