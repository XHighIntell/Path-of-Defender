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
        public event EventHandler<CustomInventoryEventArgs> OnAccept;


        public InventoryBelts() { InitializeComponent(); }
        private void InitializeComponent() {
            MouseDown += Inventory_MouseDown;
            MouseMove += Inventory_MouseMove;
            MouseEnter += Inventory_MouseEnter;
            Width = 195; Height = 76;
        }
    
        public int HitSlot(int x, int y) { return Math.Min(x / 39, 4); }
    }
}
