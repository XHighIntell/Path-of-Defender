using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Graphics;

namespace Path_of_Defender {
    public partial class VirtualListView : VirtualControl {
        public VirtualColumnHeaderCollection Columns = new VirtualColumnHeaderCollection();
        //public VirtualListViewItemCollection Items = new VirtualListViewItemCollection();
        public List<VirtualListViewItem> Items = new List<VirtualListViewItem> { };
        public SpriteFont Font {
            get { return _Font; }
            set { 
                _Font = value;
            }
        }

        SpriteFont _Font = Fonts.FontinRegular10;


        public override void Draw() {
            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied, e.GraphicsDevice.SamplerStates.PointMirror);
            DrawHeaders();
            DrawRows();
            e.SpriteBatch.End();
        }
        
        Vector2 Current_Position;
        Vector2 Size_String;

        void DrawHeaders() {
            float TMP_Height = _Font.MeasureString(" ").Y;
            Current_Position = new Vector2(X, Y);

            Current_Position.Y += TMP_Height / 2;

            for (int i = 0; i < Columns.Count; i++) {
                Size_String = Font.MeasureString(Columns.Items[i].Text); 
                Current_Position.X += Columns.Items[i].Width / 2;
                e.SpriteBatch.DrawString(Font, Columns.Items[i].Text, Current_Position, Color.White, 0, Size_String / 2, 1, SpriteEffects.None, 0);
                Current_Position.X += Columns.Items[i].Width / 2;
            }
            Current_Position.X = X;
            Current_Position.Y += TMP_Height / 2;
        }
        public void DrawRows() {
            float TMP_Height;
            for (int i = 0; i < Items.Count; i++) {
                //Trước khi bắt đầu vẻ Row
                TMP_Height = Items[i].Font.MeasureString(" ").Y;

                Current_Position.X = X;
                Current_Position.Y += TMP_Height / 2;

                for (int j = 0; j < Items[i].Texts.Count; j++) {
                    Size_String = Items[i].Font.MeasureString(Items[i].Texts[j]);
                    

                    if (j < Columns.Count) {
                        Current_Position.X += Columns.Items[j].Width / 2;
                        e.SpriteBatch.DrawString(Items[i].Font, Items[i].Texts[j], Current_Position, Items[i].ForeColor, 0, Size_String / 2, 1, SpriteEffects.None, 0);
                        Current_Position.X += Columns.Items[j].Width / 2;
                    } else {
                        //Nếu vượt số lượng Row thì bỏ qua
                        break;
                    }
                }
                //Sao khi Draw xong 1 Row
                Current_Position.Y += TMP_Height / 2;
            }
        }


    }

    public class VirtualListViewItem {
        public SpriteFont Font;
        public Color ForeColor;

        public List<string> Texts = new List<string> { };
        public VirtualListViewItem(params string[] text) {
            Font = Fonts.FontinRegular10;
            ForeColor = Color.White;
            for (int i = 0; i < text.Length; i++) {
                Texts.Add(text[i]);
            }
        }
        public VirtualListViewItem(SpriteFont font, Color foreColor, params string[] text) {
            Font = font;
            ForeColor = foreColor;
            Texts.AddRange(text);
        }
    }
    public class VirtualListViewItemCollection {
        public VirtualListViewItem[] Items = { };
        public int Count { get { return Items.Length; } }
        public void Add(VirtualListViewItem item) {
            Array.Resize(ref Items, Items.Length + 1);
            Items[Items.Length - 1] = item;
        }
        public void RemoveAt(int index) {
            Array.Copy(Items, index + 1, Items, index, Items.Length - index - 1);
            Array.Resize(ref Items, Items.Length - 1);
        }
        public void Clear() {
            Array.Resize(ref  Items, 0);
        }
    }

    public class VirtualColumnHeader {
        public string Text;
        public int Width;
        public HorizontalAlignment TextAlign;
        public Color Color;

        public VirtualColumnHeader(string text, int width, HorizontalAlignment textAlign, Color color) {
            Text = text;
            Width = width;
            TextAlign = textAlign;
            Color = color;
        }
    }
    public class VirtualColumnHeaderCollection {
        public VirtualColumnHeader[] Items = { };
        public int Count { get { return Items.Length; } }
        public void Add(VirtualColumnHeader item) {
            Array.Resize(ref Items, Items.Length + 1);
            Items[Items.Length - 1] = item;
        }
        public void RemoveAt(int index) {
            Array.Copy(Items, index + 1, Items, index, Items.Length - index - 1);
            Array.Resize(ref Items, Items.Length - 1);
        }
        public void Clear() {
            Array.Resize(ref  Items, 0);
        }

        public void Add(string text, int width, HorizontalAlignment textAlign, Color color) {
            Add(new VirtualColumnHeader(text, width, textAlign, color));
        }
    }
}
