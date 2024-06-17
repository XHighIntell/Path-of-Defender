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
    public class CustomInventoryEventArgs : EventArgs {
        public Item Item, OldItem, NewItem;
        public bool Accept;
        public CustomInventoryEventArgs(Item item) { Item = item; }
        public CustomInventoryEventArgs(Item oldItem, Item newItem) { OldItem = oldItem; NewItem = newItem; }
    }
    
    public partial class CustomInventory :VirtualControl {
        public event EventHandler<CustomInventoryEventArgs> ItemAdded;
        public event EventHandler<CustomInventoryEventArgs> ItemChanged;
        public event EventHandler Changed;

        protected void Proc_ItemAdded(CustomInventoryEventArgs e) { if (ItemAdded != null) { ItemAdded(this, e); } }
        protected void Proc_ItemChanged(CustomInventoryEventArgs e) { if (ItemChanged != null) { ItemChanged(this, e); } }
        protected void Proc_Changed() { if (Changed != null) { Changed(this, null); } }
    }

}
