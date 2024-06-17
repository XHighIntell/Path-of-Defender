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
    public partial class InventoryOne {
        public TypeItem AllowType;
        public Item Item;
        public InventoryOne(TypeItem allowType) {
            AllowType = allowType;
            InitializeComponent();
        }
        private void InitializeComponent() { 
            MouseDown += InventoryOne_MouseDown;
            MouseMove += InventoryOne_MouseMove;
            MouseEnter += InventoryOne_MouseEnter;
        }

    }
}
