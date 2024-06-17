using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Graphics;
using System.Windows.Forms;

namespace Path_of_Defender{
    public class BeltFlasks : VirtualControl {
        public BeltFlasks() { 
            MouseMove +=new System.Windows.Forms.MouseEventHandler(This_MouseMove);
            Width = 170;
            Height = 78;
        }

        public Item[] Items = new Item[5];

        public static Vector2 Scale_Vector2 = new Vector2(0.45f, 0.5f);
        public override void Draw() {
            Point point = ClientToScreen(Point.Zero);
            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied);
            for (int i = 0; i < Items.Length; i++) {
                if (Items[i] != null) {
                    float percent = Items[i].Data.Charges / Items[i].Data.Capacity;

                    //e.SpriteBatch.Draw(Items[i].Image, new Vector2(point.X + 34 * i, point.Y), new Rectangle(78, 0, 78, 156), Color.White, 0, Vector2.Zero, Scale_Vector2, SpriteEffects.None, 0);
                    
                    e.SpriteBatch.Draw(Items[i].Image, new Vector2(point.X + 34 * i, point.Y + 23 + 50 * (1 - percent)),
                        new Rectangle(156, (int)Math.Round(46 + 100 * (1 - percent), 0),
                            78, (int)(102 * percent)), Color.White, 0, Vector2.Zero, Scale_Vector2, SpriteEffects.None, 0);
                    e.SpriteBatch.Draw(Items[i].Image, new Vector2(point.X + 34 * i, point.Y), new Rectangle(0, 0, 78, 156), Color.White, 0, Vector2.Zero, Scale_Vector2, SpriteEffects.None, 0);
                    e.SpriteBatch.DrawString(Fonts.FontinRegular10, (i + 1).ToString(), new Vector2(point.X + 34 * i + 4, point.Y + 64), Color.White);
                }
            }
            e.SpriteBatch.End();
        }


        private void This_MouseMove(object sender, MouseEventArgs E) {
            int index = HitSlot(E.X, E.Y);
            if (index != -1) {
                Point point = ClientToScreen(Point.Zero);

                Item.Hover_Item = Items[index];
                Item.Rectangle_Item = new Rectangle(point.X + index * 43, point.Y, 34, 78);
            }
        }
        public int HitSlot(int X, int Y) {
            if (X < 170) { return X / 34; }
            return -1;
        }
    }
}
