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
    using System.Drawing.Design;
    public partial class Item {
        public Item() { ImageName = "CurrencyGemQuality"; }
        public Item(ItemStructure source) {
            Source = source;
        }

        public ItemStructure Data;
        [Browsable(false)]
        public ItemStructure Source { get { return Data; } set { Data = value; Init(); } }
        public Inventory Inventory;
        public VirtualControl Parent;

        public int SlotX, SlotY;


        private Texture2D Image;
        private Rectangle Image_Rect;
        

        public void Init() { 
            int i = Images.Items.GetIndex(Data.ImageName);
            Image = Images.Items.Images[i]; Image_Rect = Images.Items.Rects[i];
        }
        public bool IsBelongShop;
        public bool StickedOnShop;
    }

    public partial class Item {
        [Category("Important")] public string Name { get { return Data.Name; } set { Data.Name = value; } }
        [Category("Important")]
        public string Caption {
            get { return Data.Caption; }
            set { Data.Caption = value.Replace("|", "\r\n"); }
        }
        [Category("Important")][Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor))]
        public string Description {
            get { return Data.Description; }
            set { Data.Description = value; }
        }
        [Category("Important")] public LevelItem Level { get { return Data.Level; } set { Data.Level = value; } }
        [Category("Important")] public TypeItem Type { get { return Data.Type; } set { Data.Type = value; } }
        [Category("Important"), Editor(typeof(ItemIamgeEdior), typeof(UITypeEditor))] 
        public string ImageName { 
            get { return Data.ImageName; }
            set {
                int i = Array.IndexOf<string>(Images.Items.Names, value);
                if (i != -1) {
                    if (Type == TypeItem.Flask) { Width = 1; Height = 2; }
                    else { Data.Width = Images.Items.Rects[i].Width / 78; Data.Height = Images.Items.Rects[i].Height / 78; }
                    Data.ImageName = value; 
                    Init(); 
                } else {    }
            } 
        }

        //{ get { ;} set { ;} }
        [Category("Armor")] public float Armor { get { return Data.Armor; } set { Data.Armor = value; } }
        [Category("Armor")] public float Evasion { get { return Data.Evasion; } set { Data.Evasion = value; } }
        [Category("Armor")] public float Energy_Shield { get { return Data.Energy_Shield; } set { Data.Energy_Shield = value; } }

        [Category("Weapon")] public float[] Physical_Damage { get { return Data.Physical_Damage; } set { Data.Physical_Damage = value; } }
        [Category("Weapon")] public float Critical_Strike_Chance { get { return Data.Critical_Strike_Chance; } set { Data.Critical_Strike_Chance = value; } }
        [Category("Weapon")] public float Attacks_per_Second { get { return Data.Attacks_per_Second; } set { Data.Attacks_per_Second = value; } }
        
        [Category("Flask")] public float Life { get { return Data.Life; } set { Data.Life = value; } }
        [Category("Flask")] public float Mana { get { return Data.Mana; } set { Data.Mana = value; } }
        [Category("Flask")] public float Time { get { return Data.Time; } set { Data.Time = value; } }
        [Category("Flask")] public float Capacity { get { return Data.Capacity; } set { Data.Capacity = value; } }
        [Category("Flask")] public float Usage { get { return Data.Usage; } set { Data.Usage = value; } }
        [Category("Flask")] public float Charges { get { return Data.Charges; } set { Data.Charges = value; } }

        [Category("Currency")] public int Stack { get { return Data.Stack; } set { Data.Stack = value; } }
        [Category("Currency")] public int Stack_Max { get { return Data.Stack_Max; } set { Data.Stack_Max = value; } }

        [Category("Requires")] public int Requires_Level { get { return Data.Requires_Level; } set { Data.Requires_Level = value; } }
        [Category("Requires")] public int Requires_Str { get { return Data.Requires_Str; } set { Data.Requires_Str = value; } }
        [Category("Requires")] public int Requires_Dex { get { return Data.Requires_Dex; } set { Data.Requires_Dex = value; } }
        [Category("Requires")] public int Requires_Int { get { return Data.Requires_Int; } set { Data.Requires_Int = value; } }

        [Category("Property")] public Mod Default_Mod { get { return Data.Default_Mod; } set { Data.Default_Mod = value; } }
        [Category("Property")] public Mod[] Mods { get { return Data.Mods; } set { Data.Mods = value; } }

        //[Category("Socket")] public int Socket { get { return Data.Socket; } set { Data.Socket = value; } }
        //[Category("Socket")] public int Socket_Max { get { return Data.Socket_Max; } set { Data.Socket_Max = value; } }
        //[Category("Socket")] public SocketStructure[] Sockets { get { return Data.Sockets; } set { Data.Sockets = value; } }
        
        [Category("Cost")] public ItemCost[] Cost { get { return Data.Cost; } set { Data.Cost = value; } }
        [Category("Cost")] public int Cost_Gold { get { return Data.Cost_Gold; } set { Data.Cost_Gold = value; } }

        [Category("Collision")] public int Width { get { return Data.Width; } set { Data.Width = value; } }
        [Category("Collision")] public int Height { get { return Data.Height; } set { Data.Height = value; } }
        
    }
}
