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
    public partial class InventoryFullSet : CustomInventory {
        public InventoryOne Helmet = new InventoryOne() { X = 202, Y = 7, Width = 78, Height = 78, Visible = true, Tag_TypeItem = TypeItem.Helmet };
        public InventoryOne Amulet = new InventoryOne() { X = 298, Y = 85, Width = 39, Height = 39, Visible = true, Tag_TypeItem = TypeItem.Amulet };

        public InventoryOne Weapon_Left = new InventoryOne() { X = 49, Y = 16, Width = 78, Height = 156, Visible = true};
        public InventoryOne Weapon_Right = new InventoryOne() { X = 355, Y = 16, Width = 78, Height = 156, Visible = true };

        public InventoryOne Ring_Left = new InventoryOne() { X = 145, Y = 133, Width = 39, Height = 39, Visible = true, Tag_TypeItem = TypeItem.Ring };
        public InventoryOne Ring_Right = new InventoryOne() { X = 298, Y = 133, Width = 39, Height = 39, Visible = true, Tag_TypeItem = TypeItem.Ring };

        public InventoryOne Body_Armor = new InventoryOne() { X = 202, Y = 94, Width = 78, Height = 117, Visible = true, Tag_TypeItem = TypeItem.Body_Armour };

        public InventoryOne Glove = new InventoryOne() { X = 106, Y = 182, Width = 78, Height = 78, Visible = true, Tag_TypeItem = TypeItem.Glove };
        public InventoryOne Belt = new InventoryOne() { X = 202, Y = 221, Width = 78, Height = 39, Visible = true, Tag_TypeItem = TypeItem.Belt };
        public InventoryOne Boots = new InventoryOne() { X = 298, Y = 182, Width = 78, Height = 78, Visible = true, Tag_TypeItem = TypeItem.Boot };

        public InventoryBelts Flasks = new InventoryBelts() { X = 147, Y = 270, Visible = true, Tag_TypeItem = TypeItem.Flask };

        public event EventHandler<CustomInventoryEventArgs> OnEquipped, OnUnEquipped;


        public InventoryFullSet() {
            Width = 484; Height = 350;

            Controls.Add(Helmet);
            Controls.Add(Amulet);

            Controls.Add(Weapon_Left);
            Controls.Add(Weapon_Right);

            Controls.Add(Ring_Left);
            Controls.Add(Ring_Right);

            Controls.Add(Body_Armor);

            Controls.Add(Glove);
            Controls.Add(Belt);
            Controls.Add(Boots);

            Controls.Add(Flasks);
            SetEvents();
        }

        private void SetEvents() {
            Helmet.OnAccept += Inventory_OnAccept;
            Amulet.OnAccept += Inventory_OnAccept;
            Weapon_Left.OnAccept += Inventory_OnAccept;
            Weapon_Right.OnAccept += Inventory_OnAccept;
            Ring_Left.OnAccept += Inventory_OnAccept;
            Ring_Right.OnAccept += Inventory_OnAccept;
            Body_Armor.OnAccept += Inventory_OnAccept;
            Glove.OnAccept += Inventory_OnAccept;
            Belt.OnAccept += Inventory_OnAccept;
            Boots.OnAccept += Inventory_OnAccept;
            Flasks.OnAccept += Inventory_OnAccept;

            Helmet.ItemChanged += Inventory_OnItemChanged;
            Amulet.ItemChanged += Inventory_OnItemChanged;
            Weapon_Left.ItemChanged += Inventory_OnItemChanged;
            Weapon_Right.ItemChanged += Inventory_OnItemChanged;
            Ring_Left.ItemChanged += Inventory_OnItemChanged;
            Ring_Right.ItemChanged += Inventory_OnItemChanged;
            Body_Armor.ItemChanged += Inventory_OnItemChanged;
            Glove.ItemChanged += Inventory_OnItemChanged;
            Belt.ItemChanged += Inventory_OnItemChanged;
            Boots.ItemChanged += Inventory_OnItemChanged;
            Flasks.ItemChanged += Inventory_OnItemChanged;
        }
        private void Inventory_OnAccept(object sender, CustomInventoryEventArgs E) {
            if (E.Item.Data.Requires_Level <= e.Player.Level && E.Item.Data.Requires_Str <= e.Player.Stats.Strength && E.Item.Data.Requires_Dex <= e.Player.Stats.Dexterity && E.Item.Data.Requires_Int <= e.Player.Stats.Intelligence) {
                if (sender == Weapon_Left) {
                    if (E.Item.Data.Type == TypeItem.Bow || E.Item.Data.Type == TypeItem.Wand || E.Item.Data.Type == TypeItem.Dagger) { E.Accept = true; }
                } else if (sender == Weapon_Right) {

                }
                else if (((CustomInventory)sender).Tag_TypeItem == E.Item.Data.Type) {
                    E.Accept = true;
                }
            }
        }

        private void Inventory_OnItemChanged(object sender, CustomInventoryEventArgs E) {
            if (E.OldItem == null && E.NewItem != null) {
                if (OnEquipped != null) { E.Item = E.NewItem; OnEquipped(this, E); } // Equipped Event
            } else if (E.OldItem != null && E.NewItem == null) {
                if (OnUnEquipped != null) { E.Item = E.OldItem; OnUnEquipped(this, E); } // Drop Event
            } else if (E.OldItem != null && E.NewItem != null) {
                if (OnUnEquipped != null) { E.Item = E.OldItem; OnUnEquipped(this, E); } // Drop Event
                if (OnEquipped != null) { E.Item = E.NewItem; OnEquipped(this, E); } // Equipped Event
            }
        }
        public void Clear() {
            Helmet.Item = null;
            Amulet.Item = null;
            Weapon_Left.Item = null;
            Weapon_Right.Item = null;
            Ring_Left.Item = null;
            Ring_Right.Item = null;
            Body_Armor.Item = null;
            Glove.Item = null;
            Belt.Item = null;
            Boots.Item = null;
            Flasks.Items = new Item[5];
        }

        public void SetWeapon_Left(Item weapon) {
            Weapon_Left.Item = weapon;
            if (OnEquipped != null) { OnEquipped(this, new CustomInventoryEventArgs(weapon)); }

        }
    }
}
