using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit.Graphics;
using Microsoft.VisualBasic;

namespace Path_of_Defender {
    public static class Extensions {
        public static void DrawLineW(this SpriteBatch spriteBatch, float x, float y, float width, Color color) {
            spriteBatch.Draw(Images.VirtualControl.ImageWhitePoint, new RectangleF(x, y, width, 1), color);
        }
        public static void DrawLineH(this SpriteBatch spriteBatch, float x, float y, float height, Color color) {
            spriteBatch.Draw(Images.VirtualControl.ImageWhitePoint, new RectangleF(x, y, 1, height), color);
        }
        public static void DrawRepeatW(this SpriteBatch spriteBatch, SharpDX.Direct3D11.ShaderResourceView texture, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float scale) {
            Vector2 Position = new Vector2(destinationRectangle.X, destinationRectangle.Y);

            while (Position.X < destinationRectangle.X + destinationRectangle.Width) {
                spriteBatch.Draw(texture, Position, sourceRectangle, color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                Position.X += sourceRectangle.Width * scale;
            } 
        }
        public static void DrawStringCenter(this SpriteBatch spriteBatch, SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, float scale, SpriteEffects effects)
        {
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
        

        public static void Add<T>(ref T[] array, T item) {
            Array.Resize<T>(ref array, array.Length + 1);
            array[array.Length - 1] = item;
        }
        public static void Add<T>(ref T[] array, T item, int index) {
            Array.Resize<T>(ref array, array.Length + 1);
            Array.Copy(array, index, array, index + 1, array.Length - 1 - index);
            array[index] = item;
        }

        public static void Add<T>(ref T[] array, T[] items) {
            int i = array.Length;
            Array.Resize<T>(ref array, array.Length + items.Length);
            Array.Copy(items, 0, array, i, items.Length);            
        }
        public static void Add<T>(ref T[] array, T[] items, int index) {
            Array.Resize<T>(ref array, array.Length + items.Length);
            Array.Copy(array, index, array, index + items.Length, array.Length - items.Length -index);
            Array.Copy(items, 0, array, index, items.Length);
        }

        public static void RemoveAt<T>(ref T[] array, int index) {
            Array.Copy(array, index + 1, array, index, array.Length - index - 1);
            Array.Resize(ref array, array.Length - 1);
        }
        public static void Remove<T>(ref T[] array, T item) {
            int index = Array.IndexOf(array, item);
            if (index != -1) RemoveAt<T>(ref array, index);
        }
        public static void Swap<T>(ref T[] array, int from, int to) { 
        
        }

        
    }
}
