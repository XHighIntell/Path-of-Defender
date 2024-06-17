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
    public delegate void InventoryEventHandler(object sender, InventoryEventArgs e);
    public class InventoryEventArgs {
        public Item Item;
        public bool IsBought;
        public InventoryEventArgs(Item item) {
            Item = item;
        }
    }

    public enum InventoryType { Chest = 0, Shop = 1 }
    public partial class Inventory : VirtualControl {
        public int Slot_Width, Slot_Height;
        public Item[] Items = { };
        public bool AddItem(Item item) {
            if (item.Inventory != null) { item.Inventory.RemoveItem(item); }
            int x = 0, y = 0;
            if (FindEmtyPosition(item.Width, item.Height, ref x, ref y) == true) {
                item.SlotX = x; item.SlotY = y;
                Extensions.Add<Item>(ref Items, item); 
                item.Inventory = this;
                item.Parent = this;
                if (Type == InventoryType.Shop) { item.IsBelongShop = true; }
                else { item.IsBelongShop = false; }
                return true;
            };
            return false;
        }
        public bool AddItemStack(Item item) { 
            if (item.Data.Type == TypeItem.Currency) {
                Item[] items = FindItems(item.Data.Name);
                int stack_get;
                for (int i = 0; i < items.Length; i++) {
                    stack_get = Math.Min(items[i].Data.Stack_Max - items[i].Data.Stack, item.Data.Stack);
                    items[i].Data.Stack += stack_get;
                    item.Data.Stack -= stack_get;
                    if (item.Data.Stack == 0) { return true; }
                }
            }
            return AddItem(item);
        }

        public void RemoveItemAt(int index) {
            Items[index].Inventory = null;
            Array.Copy(Items, index + 1, Items, index, Items.Length - index - 1);
            Array.Resize(ref Items, Items.Length - 1);
        }
        public void RemoveItem(Item item) {
            int index = Array.IndexOf(Items, item);
            if (index == -1) { return; }
            Items[index].Inventory = null;
            Array.Copy(Items, index + 1, Items, index, Items.Length - index - 1);
            Array.Resize(ref Items, Items.Length - 1);
        }
        public void Clear() { Items = new Item[0]; }
    }


    public partial class Inventory : VirtualControl {
        public int Hover_Index = -1;
        public void Inventory_MouseDown(object sender, MouseEventArgs E) {            
            if (e.Mouse.LeftButton.Pressed == true) {
                if (Item.Selected_Item == null) {
                    Mouse_PickItem(E);
                } else if (Item.Selected_Item != null) {
                    Mouse_DropItem(E);
                }
            }
        }
        private void Mouse_PickItem(MouseEventArgs E) {
            if (Hover_Index != -1) {
                Item.Selected_Item = Items[Hover_Index]; 
                if (Type == InventoryType.Chest) { 
                    RemoveItemAt(Hover_Index); } 
                else if (Type == InventoryType.Shop) {
                }
                Hover_Index = -1;
            }



            
        }

        private void Mouse_DropItem(MouseEventArgs E) {
            if (Type == InventoryType.Chest) {
                Point Slot = HitSlot(E.X, E.Y, Item.Selected_Item);
                Item[] items = Find(Slot.X, Slot.Y, Item.Selected_Item.Width, Item.Selected_Item.Height);

                if (Item.Selected_Item.IsBelongShop == true) {
                    Item Buy_Item;
                    if (items.Length == 0 || items.Length == 1) {
                        Buy_Item = Item.Selected_Item.Inventory.Buy(Item.Selected_Item);
                        if (Buy_Item != null) { Item.Selected_Item = Buy_Item; } else { return; }
                    }
                }

                if (items.Length == 0) {
                    AddItem(Item.Selected_Item);
                    Item.Selected_Item.SlotX = Slot.X; Item.Selected_Item.SlotY = Slot.Y;
                    Item.Selected_Item = null;
                } else if (items.Length == 1) {
                    if (Item.Selected_Item.Data.Type == TypeItem.Currency && Item.Selected_Item.Data.Name == items[0].Data.Name) {
                        int stack_get = Math.Min(items[0].Data.Stack_Max - items[0].Data.Stack, Item.Selected_Item.Data.Stack);
                        items[0].Data.Stack += stack_get;
                        Item.Selected_Item.Data.Stack -= stack_get;
                        if (Item.Selected_Item.Data.Stack == 0) { Item.Selected_Item = null; }
                    } else {
                        RemoveItem(items[0]);
                        AddItem(Item.Selected_Item);
                        Item.Selected_Item.SlotX = Slot.X; Item.Selected_Item.SlotY = Slot.Y;
                        Item.Selected_Item = items[0];
                    }
                }

            } else if (Type == InventoryType.Shop) {
                if (Item.Selected_Item.IsBelongShop == true) {
                    //Item.Selected_Item.Inventory.AddItem(Item.Selected_Item);   
                } else {
                    if (OnSell != null) { OnSell(this, new InventoryEventArgs(Item.Selected_Item)); }
                }
                Item.Selected_Item = null;
            }

            

        }







        public void Inventory_MouseMove(object sender, MouseEventArgs e) { 
            Hover_Index = HitIndex(e.X, e.Y); 
            if (Hover_Index == -1) { Item.Hover_Item = null; } else {
                Item.Hover_Item = Items[Hover_Index]; 

                Point point = ClientToScreen(Point.Zero) ;
                Item.Rectangle_Item.X = point.X + Item.Hover_Item.SlotX * 39;
                Item.Rectangle_Item.Y = point.Y + Item.Hover_Item.SlotY * 39;

                Item.Rectangle_Item.Width = Item.Hover_Item.Width * 39;
                Item.Rectangle_Item.Height = Item.Hover_Item.Height * 39;
            }
            
        }
        public override void Draw() {
            Point point = ClientToScreen(Point.Zero);

            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied, e.GraphicsDevice.SamplerStates.AnisotropicWrap);
            e.SpriteBatch.Draw(Images.Inventory.Image_Slots, new Vector2(point.X, point.Y), new Rectangle(0, 0, 78 * Slot_Width, 78 * Slot_Height), Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
            e.SpriteBatch.End();
            
            if (IsHover == true && Item.Selected_Item != null) {
                Point Slot = HitSlot(e.X - point.X, e.Y - point.Y, Item.Selected_Item);
                Color color = Color.White;
                if (Type == InventoryType.Chest) { 
                    if (CountItemsInRect(Slot.X, Slot.Y, Item.Selected_Item.Width, Item.Selected_Item.Height) <= 1) { color = Color.Green; } 
                    else { color = Color.Red; }
                } else if (Type == InventoryType.Shop) {
                    if (Item.Selected_Item.IsBelongShop) { color = Color.Red; } 
                    else { color = Color.Green; }
                }
                e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied, e.GraphicsDevice.SamplerStates.PointWrap);
                e.SpriteBatch.Draw(Images.VirtualControl.ImageWhitePoint, new Vector2(point.X + Slot.X * 39, point.Y + Slot.Y * 39), new Rectangle(0, 0, Item.Selected_Item.Width * 39, Item.Selected_Item.Height * 39), color * 0.4f);
                e.SpriteBatch.End();
            }

            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied, e.GraphicsDevice.SamplerStates.AnisotropicWrap);
            for (int i = 0; i < Items.Length; i++) { Items[i].DrawOnChest(point); }
            
            e.SpriteBatch.End();
        }
    }
    public partial class Inventory : VirtualControl {
        public InventoryType Type;
        public event InventoryEventHandler OnBuy;
        public event InventoryEventHandler OnSell;

        //For Shop only
        public Item Buy(Item item) {
            if (OnBuy != null) {
                InventoryEventArgs eventAgrs = new InventoryEventArgs(item);
                OnBuy(this, eventAgrs);
                if (eventAgrs.IsBought == true) {
                    if (item.StickedOnShop == true) {
                        return new Item(item.Data);
                    } else {
                        item.IsBelongShop = false;
                        RemoveItem(item);
                        return item;
                    }
                    
                }
            }
            return null;
        }
    }
    
}
