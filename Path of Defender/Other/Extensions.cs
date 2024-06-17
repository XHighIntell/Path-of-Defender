using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit.Graphics;
using Microsoft.VisualBasic;

namespace Path_of_Defender {
    public static class Extensions {
        public static float GetPointRotate(this Vector2 e, SharpDX.Vector2 origin) {
            if (e.X >= origin.X) {
                return (float)Math.Atan((e.Y - origin.Y) / (e.X - origin.X));
            } else {
                return (float)(Math.Atan((e.Y - origin.Y) / (e.X - origin.X)) - Math.PI);
            }
        }

        public static Vector2 RandomPointEllipse(this Vector2 position, float rx, float ry) {
            Vector2 NEW = new Vector2();
            NEW.X = GameSetting.RND.NextFloat(-rx, rx);
            float Y = (float)Math.Sqrt(ry * ry - NEW.X * NEW.X * ry * ry / rx / rx);
            NEW.Y = GameSetting.RND.NextFloat(-Y, Y);
            return NEW + position;
        }
        public static Vector2 RandomPointCircle(this Vector2 position, float r) {
            Vector2 NEW = new Vector2();
            NEW.X = GameSetting.RND.NextFloat(-r, r);
            float  Y = (float)Math.Sqrt(r * r - NEW.X * NEW.X);
            NEW.Y = GameSetting.RND.NextFloat(-Y, Y);
            return NEW + position;
        }

        public static void DrawRectangle(this SpriteBatch spriteBatch, int x, int y, int width, int height, Color color) { 
            Vector2 pos1 = new Vector2(x, y);
            Vector2 pos2 = new Vector2(x + width, y);
            Vector2 pos3 = new Vector2(x + width, y + height);
            Vector2 pos4 = new Vector2(x, y + height);
            
            spriteBatch.Draw(Images.VirtualControl.ImageWhitePoint, new Rectangle((int)pos1.X, (int)pos1.Y, width, 1), color); //1-2
            spriteBatch.Draw(Images.VirtualControl.ImageWhitePoint, new Rectangle((int)pos2.X, (int)pos2.Y, 1, (int)height), color); //2-3
            spriteBatch.Draw(Images.VirtualControl.ImageWhitePoint, new Rectangle((int)pos4.X, (int)pos4.Y, (int)width, 1), color); //4-3
            spriteBatch.Draw(Images.VirtualControl.ImageWhitePoint, new Rectangle((int)pos1.X, (int)pos1.Y, 1, (int)height), color);//1-4
        }
        public static void DrawRectangle(this SpriteBatch spriteBatch, float x, float y, float width, float height, Color color) {
            Vector2 pos1 = new Vector2(x, y);
            Vector2 pos2 = new Vector2(x + width, y);
            Vector2 pos3 = new Vector2(x + width, y + height);
            Vector2 pos4 = new Vector2(x, y + height);

            spriteBatch.Draw(Images.VirtualControl.ImageWhitePoint, new RectangleF(pos1.X, pos1.Y, width, 1), color); //1-2
            spriteBatch.Draw(Images.VirtualControl.ImageWhitePoint, new RectangleF(pos2.X, pos2.Y, 1, height + 1), color); //2-3
            spriteBatch.Draw(Images.VirtualControl.ImageWhitePoint, new RectangleF(pos4.X, pos4.Y, width, 1), color); //4-3
            spriteBatch.Draw(Images.VirtualControl.ImageWhitePoint, new RectangleF(pos1.X, pos1.Y, 1, height), color);//1-4
        }
        public static void DrawRectangle(this SpriteBatch spriteBatch, int x, int y, int width, int height, Color color, int radius) { 
            Vector2 pos1 = new Vector2(x, y);
            Vector2 pos2 = new Vector2(x + width, y);
            Vector2 pos3 = new Vector2(x + width, y + height);
            Vector2 pos4 = new Vector2(x, y + height);

            spriteBatch.Draw(Images.VirtualControl.ImageWhitePoint, new Rectangle((int)pos1.X + radius, (int)pos1.Y, width - radius * 2, 1), color); //1-2
            spriteBatch.Draw(Images.VirtualControl.ImageWhitePoint, new Rectangle((int)pos2.X, (int)pos2.Y + radius, 1, (int)height - radius * 2), color); //2-3
            spriteBatch.Draw(Images.VirtualControl.ImageWhitePoint, new Rectangle((int)pos4.X + radius, (int)pos4.Y, (int)width - radius * 2, 1), color); //4-3
            spriteBatch.Draw(Images.VirtualControl.ImageWhitePoint, new Rectangle((int)pos1.X, (int)pos1.Y + radius, 1, (int)height - radius * 2), color);//1-4

            for (float i = 0; i <= radius ; i += 0.25f) {
                spriteBatch.Draw(Images.VirtualControl.ImageWhitePoint, new Vector2(x + radius - i, (float)(y + radius - Math.Sqrt(radius * radius - i * i))), color); //1
                spriteBatch.Draw(Images.VirtualControl.ImageWhitePoint, new Vector2(x + width - radius + i, (float)(y + radius - Math.Sqrt(radius * radius - i * i))), color);//2
                spriteBatch.Draw(Images.VirtualControl.ImageWhitePoint, new Vector2(x + width - radius + i, (float)(y + height - radius + Math.Sqrt(radius * radius - i * i))), color); //3
                spriteBatch.Draw(Images.VirtualControl.ImageWhitePoint, new Vector2(x + radius - i, (float)(y + height - radius + Math.Sqrt(radius * radius - i * i))), color); //4
            }

        }
        public static void DrawLineW(this SpriteBatch spriteBatch, float x, float y, float width, Color color) {
            spriteBatch.Draw(Images.VirtualControl.ImageWhitePoint, new RectangleF(x, y, width, 1), color);
        }
        public static void DrawLineH(this SpriteBatch spriteBatch, float x, float y, float height, Color color) {
            spriteBatch.Draw(Images.VirtualControl.ImageWhitePoint, new RectangleF(x, y, 1, height), color);
        }

        public static void FillRectangle(this SpriteBatch spriteBatch, int x, int y, int width, int height, Color color) {
            spriteBatch.Draw(Images.VirtualControl.ImageWhitePoint, new RectangleF(x, y, width, height), color);
        }


        public static double DistanceTo(this Vector2 position, Vector2 des) {
            return Math.Sqrt(Math.Pow(position.X - des.X, 2) + Math.Pow(position.Y - des.Y, 2));
        }
        public static void InsertMonsterToArray(ref Monster[] array, Monster item, int index) {
            Array.Resize(ref array, array.Length + 1);
            //if (array.Length != 0) { }
            Array.Copy(array, index, array, index + 1, array.Length - 1 - index);
            array[index] = item;
        }
        public static void AddSortMonsterToArray(ref Monster[] array, Monster item){
            int index = 0;
            for (int i = 0; i < array.Length; i++) {
                if (item.Position.Y > array[i].Position.Y) { index = i + 1; }
                else { break; }
            }
            InsertMonsterToArray(ref array, item, index);
        }
        public static void AddMonsterToArray(ref Monster[] array, Monster item) {
            Array.Resize(ref array, array.Length + 1);
            array[array.Length - 1] = item;
        }

        public static void Add(ref string[] array, string item) {
            Array.Resize(ref array, array.Length + 1);
            array[array.Length - 1] = item;
        }
        public static void Add<T>(ref T[] array, T item) {
            Array.Resize<T>(ref array, array.Length + 1);
            array[array.Length - 1] = item;
        }
        public static void RemoveAt<T>(ref T[] array, int index) {
            Array.Copy(array, index + 1, array, index, array.Length - index - 1);
            Array.Resize(ref array, array.Length - 1);
        }
        public static void Remove<T>(ref T[] array, T item) {
            int index = Array.IndexOf(array, item);
            if (index != -1) RemoveAt<T>(ref array, index);
        }



        public static void DrawStringCenter(this SpriteBatch spriteBatch,SpriteFont spriteFont, string[] text, RectangleF rect, float margin_top, Color color, bool ucase) {
            float Line_Height = spriteFont.MeasureString(" ").Y;

            if (ucase == false) { 
                for(int i=0;i<text.Length ;i++){
                    spriteBatch.DrawString(spriteFont, text[i], new Vector2(rect.X + rect.Width / 2, rect.Y + margin_top + Line_Height / 2 + Line_Height * i), color, 0, spriteFont.MeasureString(text[i]) / 2, 1, SpriteEffects.None, 0);
                }
            } else {
                for(int i=0;i<text.Length ;i++){
                    spriteBatch.DrawString(spriteFont, text[i].ToUpper(), new Vector2(rect.X + rect.Width / 2, rect.Y + margin_top + Line_Height / 2 + Line_Height * i), color, 0, spriteFont.MeasureString(text[i].ToUpper()) / 2, 1, SpriteEffects.None, 0);
                }
            }
            
            
        }
        public static void DrawStringCenter(this SpriteBatch spriteBatch, SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, float scale, SpriteEffects effects) {
            spriteBatch.DrawString(spriteFont, text, position, color, rotation, spriteFont.MeasureString(text) / 2, scale, effects, 0);
        }
        public static void DrawStringCenter(this SpriteBatch spriteBatch, SpriteFont spriteFont, string text, RectangleF desRect, Color color, float rotation, float scale, SpriteEffects effects) {
            string[] lines = Strings.Split(text, Constants.vbCrLf);

            Vector2 Total_Size = spriteFont.MeasureString(text);

            float x = desRect.X + desRect.Width / 2;
            float y = desRect.Y + (desRect.Height - Total_Size.Y) / 2;

            Vector2 String_Size;
            for (int i = 0; i < lines.Length; i++) {
                String_Size = spriteFont.MeasureString(lines[i]);
                spriteBatch.DrawString(spriteFont, lines[i], new Vector2(x, y + String_Size.Y / 2), color, 0, String_Size / 2, scale, effects, 0);
                y += String_Size.Y;
            }
        }

    }
}
