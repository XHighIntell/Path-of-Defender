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
    public partial class InventoryOne : CustomInventory {
        //Events
        public event EventHandler<CustomInventoryEventArgs> OnAccept;

        //Handle Events
        private Item Accepted_Item;
        private void InventoryOne_MouseDown(object sender, MouseEventArgs E) {            
            if (E.Button == MouseButtons.Left) {
                if (Item.Selected_Item == null) {
                    if (Item != null) {
                        Item.Selected_Item = Item;
                        Item = null;
                        InventoryOne_MouseEnter(this, null);
                        Proc_ItemChanged(new CustomInventoryEventArgs(Item.Selected_Item, null));
                    }
                } else {
                    if (Item.Selected_Item == Accepted_Item) {
                        if (Item.Selected_Item.IsBelongShop == false) {
                            Item oldItem = Item;
                            Item = Item.Selected_Item;
                            Item.Selected_Item = oldItem;
                            Proc_ItemChanged(new CustomInventoryEventArgs(oldItem, Item));
                            Proc_ItemAdded(new CustomInventoryEventArgs(Item));
                        } else {
                            Item Buy_Item = Item.Selected_Item.Inventory.Buy(Item.Selected_Item);
                            if (Buy_Item != null) {
                                Item.Selected_Item = Item;
                                Item = Buy_Item;
                                Proc_ItemChanged(new CustomInventoryEventArgs(Item.Selected_Item, Buy_Item));
                                Proc_ItemAdded(new CustomInventoryEventArgs(Buy_Item));
                            }   
                        }
                        InventoryOne_MouseEnter(this, null);
                    }
                }
            }

        }
        private void InventoryOne_MouseMove(object sender, MouseEventArgs e) {
            if (Item != null) {
                Item.Hover_Item = Item;
          
                Point point = ClientToScreen(Point.Zero);
                Item.Rectangle_Item.X = point.X;
                Item.Rectangle_Item.Y = point.Y;

                Item.Rectangle_Item.Width = Width;
                Item.Rectangle_Item.Height = Height;
            }
        }
        private void InventoryOne_MouseEnter(object sender, EventArgs e) {
            if (Item.Selected_Item != null && Item.Selected_Item.Data.Type == AllowType && OnAccept != null) {
                CustomInventoryEventArgs eventAgrs = new CustomInventoryEventArgs(Item.Selected_Item);
                OnAccept(this, eventAgrs);
                if (eventAgrs.Accept == true) { Accepted_Item = Item.Selected_Item; }
            }
        }

        public override void Draw() {
            Point point = ClientToScreen(Point.Zero);

            if (IsHover == true && Item.Selected_Item != null) {
                Color color = Color.White;
                if (Item.Selected_Item.Data.Type == AllowType && Item.Selected_Item == Accepted_Item) { color = Color.Green; }
                else { color = Color.Red; }
                
                e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied, e.GraphicsDevice.SamplerStates.PointWrap);
                e.SpriteBatch.Draw(Images.VirtualControl.ImageWhitePoint, point, new Rectangle(0, 0, Width, Height), color * 0.4f);
                e.SpriteBatch.End();
            }

            
            //e.SpriteBatch.Draw(Images.VirtualControl.ImageWhitePoint, new RectangleF(point.X, point.Y, Width, Height), Color.White);
            if (Item != null) {
                e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied, e.GraphicsDevice.SamplerStates.AnisotropicWrap);
                Item.DrawOnChest(new Rectangle(point.X, point.Y, Width, Height));
                e.SpriteBatch.End();
                
            }
            
        }
    }
}
