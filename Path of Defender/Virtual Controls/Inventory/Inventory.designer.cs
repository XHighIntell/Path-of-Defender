using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpDX;
namespace Path_of_Defender {
    public partial class Inventory {
        public Inventory() { InitializeComponent(); }
        public Inventory(int slot_Width, int slot_Height) {
            InitializeComponent();
            Slot_Width = slot_Width; Slot_Height = slot_Height;
            Width = Slot_Width * 39;
            Height = slot_Height * 39;
        }
        private void InitializeComponent() {
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(Inventory_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(Inventory_MouseMove);
        }
    }

    public partial class Inventory { 
        #region Find & Count Functions: FindEmtyPosition(), Find(), CountItemsInRect()
        public bool FindEmtyPosition(int W, int H, ref int Slot_X, ref int Slot_Y) {
            //byte[,] bin = new byte[Slot_Width, Slot_Height];
            //for (int i = 0; i < Items.Length; i++) { }
            for (int y = 0; y <= Slot_Height - H; y++) {
                for (int x = 0; x <= Slot_Width - W; x++) {
                    if (IsEmpty(x, y, W, H) == true) {
                        Slot_X = x; Slot_Y = y;
                        return true;
                    }
                }
            }

            return false;
        }
        public Item[] Find(int slot_X, int slot_Y, int w, int h) {
            Item[] Returns = { };
            for (int i = 0; i < Items.Length; i++) {
                if (IsMixed(slot_X, slot_Y, w, h, Items[i].SlotX, Items[i].SlotY, Items[i].Width, Items[i].Height) == true) {
                    Extensions.Add<Item>(ref Returns, Items[i]);
                }
            }
            return Returns;
        }
        public int CountItemsInRect(int slot_X, int slot_Y, int w, int h) {
            int count = 0;
            for (int i = 0; i < Items.Length; i++) {
                if (IsMixed(slot_X, slot_Y, w, h, Items[i].SlotX, Items[i].SlotY, Items[i].Width, Items[i].Height) == true) { count += 1; }
            }
            return count;
        }
        public Item[] FindItems(string Name) {
            Item[] items = new Item[] { };
            for (int i = 0; i < Items.Length; i++) {
                if (Items[i].Data.Name == Name) { Extensions.Add<Item>(ref items, Items[i]); }
            }
            return items;
        }

        #endregion
        #region Check Slot Functions: IsEmpty(), IsMixed()
        public bool IsEmpty(int X, int Y) {
            if (X + 1 > Slot_Width || Y + 1 > Slot_Height) { return false; }

            for (int i = 0; i < Items.Length; i++) {
                if (Items[i].SlotX <= X && X <= Items[i].SlotX + Items[i].Width - 1 && Items[i].SlotY <= Y && Y <= Items[i].SlotY + Items[i].Height - 1) { return false; }
            }
            return true;
        }
        public bool IsEmpty(int X, int Y, int W, int H) {

            if (X + W > Slot_Width || Y + H > Slot_Width) { return false; }
            for (int i = 0; i < Items.Length; i++) {
                if (IsMixed(X, Y, W, H, Items[i].SlotX, Items[i].SlotY, Items[i].Width, Items[i].Height)) { return false; }
            }
            return true;
        }
        #endregion
        #region Hit Functions: HitIndex(), HitItem(), HitSlot()
        public int HitIndex(int x, int y) {
            for (int i = 0; i < Items.Length; i++) {
                if (39 * Items[i].SlotX < x && x < 39 * Items[i].SlotX + 39 * Items[i].Data.Width &&
                    39 * Items[i].SlotY < y && y < 39 * Items[i].SlotY + 39 * Items[i].Data.Height) {
                        return i;
                }
            }
            return -1;
        }
        public Item HitItem(int x, int y) {
            int Index = HitIndex(x, y);
            if (Index != -1) { return Items[Index]; } else { return null; }    
        }
        /// <summary> Find slot position from point. </summary>
        /// <returns> If found slot, return True, else False</returns>
        public bool HitSlot(float x, float y, ref int Slot_X, ref int Slot_Y) {
            float hitX = x / 39;
            float hitY = y / 39;
            if (hitX >= 0 && hitX < Slot_Width && hitY >= 0 && hitY < Slot_Height) {
                Slot_X = (int)hitX; Slot_Y = (int)hitY;
                return true;
            }
            return false;
        }
        public Point HitSlot(float x, float y, Item item) {
            float hitX = (x - (item.Width - 1) * 20) / 39;
            float hitY = (y - (item.Height - 1) * 20) / 39;

            Point point = new Point();

            point.X = Math.Max((int)hitX, 0);
            point.X = Math.Min(point.X, Slot_Width - item.Width);

            point.Y = Math.Max((int)hitY, 0);
            point.Y = Math.Min(point.Y, Slot_Height - item.Height);

            return point;
        }
        #endregion

        public bool IsMixed(int x1, int y1, int w1, int h1, int x2, int y2, int w2, int h2) {
            //1__2
            //|  |
            //3__4
            if (x2 < x1 && y2 < y1) { if (x2 + w2 > x1 & y2 + h2 > y1) { return true; } }
            else if (x2 < x1 + w1 && y2 < y1) { if (y2 + h2 > y1) { return true; } }
            else if (x2 < x1 && y2 < y1 + h1) { if (x2 + w2 > x1) { return true; } }
            else if (x2 < x1 + w1 && y2 < y1 + h1) { return true; }

            return false;
        }
    }
}
