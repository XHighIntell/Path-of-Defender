using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Graphics;
using Microsoft.VisualBasic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Path_of_Defender {
    public partial class InventoryBelts : CustomInventory {
        public Item[] Items = new Item[5];
        Item Accepted_Item;

        public override void Draw() {
            //e.SpriteBatch.Begin();
            //e.SpriteBatch.Draw(Images.VirtualControl.ImageWhitePoint, new Vector2(X, Y), new Rectangle(0, 0, Width, Height), Color.White);
            //e.SpriteBatch.End();
            
            Point point = ClientToScreen(Point.Zero);

            if (IsHover == true && Item.Selected_Item != null) {
                Color color = Color.White;
                if (Item.Selected_Item == Accepted_Item) { color = Color.Green; }
                else { color = Color.Red; }
                int Slot_X = HitSlot(e.X - point.X, e.Y - point.Y);

                e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied, e.GraphicsDevice.SamplerStates.PointWrap);
                e.SpriteBatch.Draw(Images.VirtualControl.ImageWhitePoint, new Point(point.X + Slot_X * 39, point.Y), new Rectangle(0, 0, 39, Height), color * 0.4f);
                e.SpriteBatch.End();
            }

            for (int i = 0; i < Items.Length; i++) {
                e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied, e.GraphicsDevice.SamplerStates.AnisotropicWrap);
                if (Items[i] != null) {
                    Items[i].DrawOnChest(new Rectangle(point.X + i * 39, point.Y, 39, 76));
                }
                e.SpriteBatch.End();
            }
            
        }

        
        private void Inventory_MouseMove(object sender, MouseEventArgs e) {
            
            int index = HitSlot(e.X, e.Y);
            if (index != -1) {
                Point point = ClientToScreen(Point.Zero);
                Item.Hover_Item = Items[index];
                
                Item.Rectangle_Item.X = point.X;
                Item.Rectangle_Item.Y = point.Y;

                Item.Rectangle_Item.Width = Width;
                Item.Rectangle_Item.Height = Height;
            }
        }
        private void Inventory_MouseDown(object sender, MouseEventArgs E) {
            if (E.Button == MouseButtons.Left) {
                int index = HitSlot(E.X, E.Y);
                if (index != -1) { 
                    if (Item.Selected_Item == null) {
                        if (Items[index] != null) {
                            Item.Selected_Item = Items[index];
                            Items[index] = null;
                            Inventory_MouseEnter(this, null);
                            //Proc_Changed();
                            Proc_ItemChanged(new CustomInventoryEventArgs(Item.Selected_Item, null));
                        }
                    } else {
                        if (Item.Selected_Item == Accepted_Item) {
                            if (Item.Selected_Item.IsBelongShop == false) {
                                Item TMP_Item = Item.Selected_Item;
                                Item.Selected_Item = Items[index];
                                Items[index] = TMP_Item;

                                Proc_ItemChanged(new CustomInventoryEventArgs(Item.Selected_Item, TMP_Item));
                                //Proc_ItemAdded(new CustomInventoryEventArgs(Item));
                                //Proc_Changed();
                            } else {
                                Item Buy_Item = Item.Selected_Item.Inventory.Buy(Item.Selected_Item);
                                if (Buy_Item != null) {
                                    Item.Selected_Item = Items[index];
                                    Items[index] = Buy_Item;
                                    Proc_ItemChanged(new CustomInventoryEventArgs(Item.Selected_Item, Buy_Item));
                                    //Proc_Changed();
                                }   
                            }
                            Inventory_MouseEnter(this, null);
                        }
                    }
                }
            }
        }
        private void Inventory_MouseEnter(object sender, EventArgs e) {
            if (Item.Selected_Item != null && OnAccept != null) {
                CustomInventoryEventArgs eventAgrs = new CustomInventoryEventArgs(Item.Selected_Item);
                OnAccept(this, eventAgrs);
                if (eventAgrs.Accept == true) { Accepted_Item = Item.Selected_Item; }
            }
        }
        


    }
}
