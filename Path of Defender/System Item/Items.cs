using System;
using System.Collections.Generic;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Graphics;
using Microsoft.VisualBasic;
using System.ComponentModel;

namespace Path_of_Defender {
    using Drawing = Images.Items.Drawing;
    public partial class Item {        
        public Item(ItemStructure source) {
            Source = source;
        }

        public ItemStructure Data;
        public Inventory Inventory;
        public VirtualControl Parent;

        public int SlotX, SlotY;


        public Texture2D Image;
        private Rectangle Image_Rect;
        

        public void Init() { 
            int i = Images.Items.GetIndex(Data.ImageName);
            Image = Images.Items.Images[i]; Image_Rect = Images.Items.Rects[i];
        }
        public bool IsBelongShop;
        public bool StickedOnShop;
    }
    public partial class Item {
        public ItemStructure Source { get { return Data; } set { Data = value; Init(); } }
        public int Width { get { return Data.Width; } set { Data.Width = value; } }
        public int Height { get { return Data.Height; } set { Data.Height = value; } }
    }
}
