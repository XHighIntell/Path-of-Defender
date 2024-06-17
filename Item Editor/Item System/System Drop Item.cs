using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Graphics;
using System.ComponentModel;

namespace Path_of_Defender {
    public struct SystemItemStructure {
        public static SystemItemStructure Create() {
            SystemItemStructure item = new SystemItemStructure();
            item.White_Items = new ItemStructure[0];
            item.Unique_Items = new ItemStructure[0];

            item.Prefixes = new Affix[0];
            item.Suffixes = new Affix[0];

            item.Rare_Prefixes_Caption = new string[0];
            item.Rare_Suffixes_Caption = new string[0];
            return item;
        }
        public ItemStructure[] White_Items, Unique_Items;
        public Affix[] Prefixes, Suffixes;
        public string[] Rare_Prefixes_Caption, Rare_Suffixes_Caption;
    }

    [TypeConverter(typeof(RangeValuesConverter))]
    public struct RangeValues {
        public float Minimum, Maximum;
        public RangeValues(float minimum, float maximum) { Minimum = minimum; Maximum = maximum; }

        //public float min { get { return Min; } set { Min = value; } }
        //public float max { get { return Max; } set { Max = value; } }


        public override string ToString() {
            return Minimum.ToString() + " - " + Maximum.ToString();
        }
    }

    [TypeConverter(typeof(ObjectConverter))]
    public struct Affix {
        public string Caption;
        public int Level;
        public ModType Type;
        public RangeValues Value1, Value2;
        
        public RangeValues value1 { get { return Value1; } set { Value1 = value; } }
        public RangeValues value2 { get { return Value2; } set { Value2 = value; } }

        public bool IsPercent;

        public string caption { get { return Caption; } set { Caption = value; } }
        public int level { get { return Level; } set { Level = value; } }
        public ModType type { get { return Type; } set { Type = value; } }
        public bool percent { get { return IsPercent; } set { IsPercent = value; } }


        public override string ToString() {
            if (Type == ModType.None) { return "None"; }
            String Return = Mod.BaseModString(Type);

            int i = Strings.InStr(Return, "?");
            int ii = Strings.InStr(i + 1, Return, "?");

            if (i != 0) {
                if (Strings.Mid(Return, i + 1, 1) == "%")
                    Return = Strings.Replace(Return, "?", "(" + Math.Round(Value1.Minimum * 100).ToString() + " to " + Math.Round(Value1.Maximum * 100).ToString() + ")", 1, 1);
                else { Return = Strings.Replace(Return, "?", "(" + Value1.Minimum.ToString() + " to " + Value1.Maximum.ToString() + ")", 1, 1); }
            }
            if (ii != 0) {
                if (Strings.Mid(Return, ii + 1, 1) == "%")
                    Return = Strings.Replace(Return, "?", "(" + Math.Round(Value2.Minimum * 100).ToString() + " to " + Math.Round(Value2.Maximum * 100).ToString() + ")", 1, 1);
                else { Return = Strings.Replace(Return, "?", "(" + Value2.Minimum.ToString() + " to " + Value2.Maximum.ToString() + ")", 1, 1); }
            }

            return Return;
        }
        public Mod CreateMod() {
            if (IsPercent == true) {
                return new Mod(Type, (float)Math.Round(GameSetting.RND.NextFloat(Value1.Minimum, Value1.Maximum), 2),
                                     (float)Math.Round(GameSetting.RND.NextFloat(Value2.Minimum, Value2.Maximum), 2));
            } else {
                return new Mod(Type, (float)Math.Round(GameSetting.RND.NextFloat(Value1.Minimum, Value1.Maximum)),
                                     (float)Math.Round(GameSetting.RND.NextFloat(Value2.Minimum, Value2.Maximum)));
            }
            
        }
        public static Affix[] DistinctType(Affix[] values) {
            for (int x = 0; x < values.Length; x++) {
                for (int y = x + 1; y < values.Length; y++) {
                    if (values[x].Type == values[y].Type) { Extensions.RemoveAt<Affix>(ref values, y); y--; }
                }
            }
            return values;
        }
    }
    public partial class SystemItem {
        public SystemItemStructure data;
        public SystemItem() { data = SystemItemStructure.Create(); }
        public SystemItem(SystemItemStructure source) { Source = source; }
        public SystemItem(string file) { Load(file); }

        public void Load(string file) {
            FileSystem.FileOpen(3, file, OpenMode.Binary);
            ValueType TMP = data;
            FileSystem.FileGet(3, ref TMP);
            data = (SystemItemStructure)TMP;
            FileSystem.FileClose(3);
        }
        public void Save(string file) {
            FileSystem.FileOpen(3, file, OpenMode.Binary);
            FileSystem.FilePut(3, data);
            FileSystem.FileClose(3);        
        }

        public ItemStructure Random(float magic_precent, float rare_percent, float unique_percent) {
            float i = GameSetting.RND.NextFloat(0, 1);
            if (i <= magic_precent) { return RandomMagicItem(RandomWhiteItem()); }
            else if (magic_precent < i && i <= magic_precent + rare_percent) { return RandomRareItem(RandomWhiteItem()); }
            else if (magic_precent + rare_percent < i && i <= magic_precent + rare_percent + unique_percent) { return RandomUniqueItem(); }
            else { return RandomWhiteItem(); }
        }
        public ItemStructure Random(int max_level, float magic_precent, float rare_percent, float unique_percent) {
            float i = GameSetting.RND.NextFloat(0, 1);
            if (i <= magic_precent) { return RandomMagicItem(RandomWhiteItem(max_level)); }
            else if (magic_precent < i && i <= magic_precent + rare_percent) { return RandomRareItem(RandomWhiteItem(max_level)); }
            else if (magic_precent + rare_percent < i && i <= magic_precent + rare_percent + unique_percent) { return RandomUniqueItem(max_level); }
            else { return RandomWhiteItem(max_level); }
        }

        //
        public ItemStructure RandomWhiteItem() {
            return data.White_Items[GameSetting.RND.Next(data.White_Items.Length)];
        }
        public ItemStructure RandomWhiteItem(TypeItem type_item) {
            ItemStructure[] items = new ItemStructure[0];
            for (int i = 0; i < data.White_Items.Length; i++) {
                if (data.White_Items[i].Type == type_item) Extensions.Add<ItemStructure>(ref items, data.White_Items[i]);
            }
            return items[GameSetting.RND.Next(items.Length)];
        }
        public ItemStructure RandomWhiteItem(TypeItem type_item, int maxLevel) {
            ItemStructure[] items = new ItemStructure[0];
            for (int i = 0; i < data.White_Items.Length; i++) {
                if (data.White_Items[i].Requires_Level <= maxLevel && data.White_Items[i].Type == type_item) { Extensions.Add<ItemStructure>(ref items, data.White_Items[i]); }
            }
            return items[GameSetting.RND.Next(items.Length)];
        }
        public ItemStructure RandomWhiteItem(int maxLevel) {
            ItemStructure[] items = new ItemStructure[0];
            for (int i = 0; i < data.White_Items.Length; i++) {
                if (data.White_Items[i].Requires_Level <= maxLevel) { Extensions.Add<ItemStructure>(ref items, data.White_Items[i]); }
            }
            return items[GameSetting.RND.Next(items.Length)];
        }
        

        public bool RandomWhiteItem(int maxLevel, ref ItemStructure item) {
            ItemStructure[] items = new ItemStructure[0];

            for (int i = 0; i < data.White_Items.Length; i++) {
                if (data.White_Items[i].Requires_Level <= maxLevel) { Extensions.Add<ItemStructure>(ref items, data.White_Items[i]); }
            }

            if (items.Length == 0) { return false; }
            else { item = items[GameSetting.RND.Next(items.Length)]; return true; }
        }

        public ItemStructure RandomMagicItem() {
            return RandomMagicItem(RandomWhiteItem());
        }
        public ItemStructure RandomMagicItem(ItemStructure white_item) {
            Affix[] prefixes = GetPrefixes(white_item.Requires_Level);
            Affix[] suffixes = GetSuffixes(white_item.Requires_Level);

            if (prefixes.Length > 0) {
                Affix prefix = prefixes[GameSetting.RND.Next(prefixes.Length)];
                white_item.Caption = prefix.Caption + " " + white_item.Caption;
                Extensions.Add<Mod>(ref white_item.Mods, prefix.CreateMod());
                white_item.Level = LevelItem.Magic;
            }
            
            if (suffixes.Length > 0) {
                Affix suffix = suffixes[GameSetting.RND.Next(suffixes.Length)];
                white_item.Caption = white_item.Caption + " of " + suffix.Caption;
                Extensions.Add<Mod>(ref white_item.Mods, suffix.CreateMod());
                white_item.Level = LevelItem.Magic;
            }

            white_item.Cost_Gold *= 2;
            return white_item;
        }

        public ItemStructure RandomRareItem() {
            return RandomRareItem(RandomWhiteItem());
        }
        public ItemStructure RandomRareItem(ItemStructure white_item) {
            int max_mods = GameSetting.RND.Next(3, 7);

            int number_prefixes = max_mods / 2;
            int number_suffixes = max_mods - number_prefixes;

            Affix[] prefixes = RandomPrefixes(white_item.Requires_Level, number_prefixes);
            Affix[] suffixes = RandomSuffixes(white_item.Requires_Level, number_suffixes);

            for (int i = 0; i < prefixes.Length; i++) { Extensions.Add<Mod>(ref white_item.Mods, prefixes[i].CreateMod()); }
            for (int i = 0; i < suffixes.Length; i++) { Extensions.Add<Mod>(ref white_item.Mods, suffixes[i].CreateMod()); }
            white_item.Level = LevelItem.Rare;
            white_item.Caption = RandomPrefixCaption() + " " + RandomSuffixCaption() + Constants.vbCrLf + white_item.Caption;

            white_item.Cost_Gold *= 3;
            return white_item;
        }

        public ItemStructure RandomUniqueItem() {
            return data.Unique_Items[GameSetting.RND.Next(0, data.Unique_Items.Length)];
        }
        public ItemStructure RandomUniqueItem(int max_level) {
            ItemStructure[] items = new ItemStructure[0];
            for (int i = 0; i < data.Unique_Items.Length; i++) {
                if (data.Unique_Items[i].Requires_Level <= max_level) { Extensions.Add<ItemStructure>(ref items, data.Unique_Items[i]); }
            }
            return items[GameSetting.RND.Next(items.Length)];
        }

        public Affix[] GetPrefixes(int max_level) {
            Affix[] prefixes = new Affix[0];
            for (int i = 0; i < data.Prefixes.Length; i++) {
                if (data.Prefixes[i].Level <= max_level) { Extensions.Add<Affix>(ref prefixes, data.Prefixes[i]); }
            }
            return prefixes;
        }
        public Affix[] RandomPrefixes(int max_level, int length) {
            Affix[] prefixes = GetPrefixes(max_level);
            Affix[] Return = new Affix[0];

            if (prefixes.Length <= length) {
                Return = prefixes;
                Return = Affix.DistinctType(Return);
            } else {
                int index;

                do {
                    //for (int i = 0; i < prefixes.Length; i++) {
                        index = GameSetting.RND.Next(prefixes.Length);
                        Extensions.Add<Affix>(ref Return, prefixes[index]);
                        Extensions.RemoveAt<Affix>(ref prefixes, index);
                    //}
                    Return = Affix.DistinctType(Return);
                } while (Return.Length < length && prefixes.Length > 0);
            }

            return Return;
        }

        public Affix[] GetSuffixes(int max_level) {
            Affix[] suffixes = new Affix[0];
            for (int i = 0; i < data.Suffixes.Length; i++) {
                if (data.Suffixes[i].Level <= max_level) { Extensions.Add<Affix>(ref suffixes, data.Suffixes[i]); }
            }
            return suffixes;
        }
        public Affix[] RandomSuffixes(int max_level, int length) {
            Affix[] suffixes = GetSuffixes(max_level);
            Affix[] Return = new Affix[0];

            if (suffixes.Length <= length) {
                Return = suffixes;
                Return = Affix.DistinctType(Return);
            } else {
                int index;

                do {
                    //for (int i = 0; i < suffixes.Length; i++) {
                        index = GameSetting.RND.Next(suffixes.Length);
                        Extensions.Add<Affix>(ref Return, suffixes[index]);
                        Extensions.RemoveAt<Affix>(ref suffixes, index);
                    //}
                    Return = Affix.DistinctType(Return);
                } while (Return.Length < length && suffixes.Length > 0);
            }

            return Return;
        }

        public string RandomPrefixCaption() { return data.Rare_Prefixes_Caption[GameSetting.RND.Next(data.Rare_Prefixes_Caption.Length)]; }
        public string RandomSuffixCaption() { return data.Rare_Suffixes_Caption[GameSetting.RND.Next(data.Rare_Suffixes_Caption.Length)]; }
        
        [Browsable(false)]
        public SystemItemStructure Source {
            get { return data; }
            set { data = value; }
        }
        [DisplayName("Prefixes Caption")]
        public string[] Prefixes_Caption {
            get { return data.Rare_Prefixes_Caption; }
            set { data.Rare_Prefixes_Caption = value; }
        }
        public string[] Suffixes_Caption {
            get { return data.Rare_Suffixes_Caption; }
            set { data.Rare_Suffixes_Caption = value; }
        }
        public Affix[] Prefixes {
            get { return data.Prefixes; }
            set { data.Prefixes = value; }
        }
        public Affix[] Suffixes {
            get { return data.Suffixes; }
            set { data.Suffixes = value; }
        }
    }




    public static class SystemDropItem {
        public static string[] Caption_Dagger = new string[] { "Glass Shank", "Demon Dagger", "Skinning Knife", "Carving Knife", "Stiletto", "Boot Knife", "Copper Kris", "Skean", "Imp Dagger", "Prong Dagger", "Flaying Knife", "Butcher Knife", "Poignard", "Boot Blade", "Golden Kris", "Royal Skean", "Fiend Dagger", "Trisula", "Gutting Knife", "Slaughter Knife", "Ambusher", "Ezomyte Dagger", "Platinum Kris", "Imperial Skean", "Demon Dagger" };
        public static string[] Caption_Wand = new string[] { "Driftwood Wand", "Goat's Horn", "Carved Wand", "Quartz Wand", "Spiraled Wand", "Sage Wand", "Pagan Wand", "Faun's Horn", "Engraved Wand", "Crystal Wand", "Serpent Wand", "Omen Wand", "Heathen Wand", "Demon's Horn", "Imbued Wand", "Opal Wand", "Tornado Wand", "Prophecy Wand", "Profane Wand" };
        public static string[] Caption_Bow = new string[] { "Crude Bow", "Short Bow", "Long Bow", "Composite Bow", "Recurve Bow", "Bone Bow", "Royal Bow", "Death Bow", "Grove Bow", "Decurve Bow", "Compound Bow", "Sniper Bow", "Ivory Bow", "Highborn Bow", "Decimation Bow", "Thicket Bow", "Citadel Bow", "Ranger Bow", "Assassin Bow", "Spine Bow", "Imperial Bow", "Harbinger Bow" };

        public static string[] Caption_Flask_Life = new string[] { "Small Life Flask", "Medium Life Flask", "Large Life Flask", "Greater Life Flask", "Grand Life Flask", "Giant Life Flask", "Colossal Life Flask", "Sacred Life Flask", "Hallowed Life Flask", "Sanctified Life Flask", "Divine Life Flask", "Eternal Life Flask" };
        public static string[] Caption_Flask_Mana = new string[] { "Small Mana Flask", "Medium Mana Flask", "Large Mana Flask", "Greater Mana Flask", "Grand Mana Flask", "Giant Mana Flask", "Colossal Mana Flask", "Sacred Mana Flask", "Hallowed Mana Flask", "Sanctified Mana Flask", "Divine Mana Flask", "Eternal Mana Flask" };
        public static string[] Caption_Flask_Hybrid = new string[] { "Small Hybrid Flask", "Medium Hybrid Flask", "Large Hybrid Flask", "Colossal Hybrid Flask", "Sacred Hybrid Flask", "Hallowed Hybrid Flask" };
        public static string[] Caption_Flask_Utility = new string[] { "Quicksilver Flask", "Ruby Flask", "Sapphire Flask", "Topaz Flask", "Amethyst Flask", "Granite Flask", "Diamond Flask", "Jade Flask", "Quartz Flask" };

        public static Item Random() {
            ItemStructure itemStructure;
            itemStructure.Caption = "New";

            itemStructure.Armor =  GameSetting.RND.Next(100);
            itemStructure.Energy_Shield = GameSetting.RND.Next(100);
            itemStructure.ImageName = Images.Items.Names[GameSetting.RND.Next(Images.Items.Images.Length)];
            itemStructure.Type = (TypeItem)Enum.GetValues(typeof(TypeItem)).GetValue(GameSetting.RND.Next(13));


            if (itemStructure.Type == TypeItem.Bow || itemStructure.Type == TypeItem.Wand) {
                itemStructure.Physical_Damage = new float[] { 13, 18 };
                itemStructure.Critical_Strike_Chance = 0.05f;
                itemStructure.Attacks_per_Second = 1.4f;
            }

            //itemStructure.Default_Mod.ToString(

            return null;
        }
        public static Item CreateWeapon() {
            ItemStructure itemStructure = ItemStructure.Create();
            itemStructure.Caption = "Long Bow";
            itemStructure.Level = LevelItem.Rare;
            itemStructure.Type = TypeItem.Bow;
            itemStructure.ImageName = Images.Items.Names_Bows[GameSetting.RND.Next(Images.Items.Names_Bows.Length)];

            itemStructure.Physical_Damage = new float[] { GameSetting.RND.Next(10), GameSetting.RND.Next(10) + 10 };
            itemStructure.Critical_Strike_Chance = 0.05f;
            itemStructure.Attacks_per_Second = (float)Math.Round(GameSetting.RND.NextFloat(0.8f, 2), 2);

            itemStructure.Requires_Level = (int)(itemStructure.Attacks_per_Second * 10);
            itemStructure.Requires_Str = (int)(itemStructure.Physical_Damage[0] * 2);
            itemStructure.Requires_Dex = itemStructure.Requires_Level * 2;

            int i = Array.IndexOf<string>(Images.Items.Names, itemStructure.ImageName);
            itemStructure.Width = Images.Items.Images[i].Width / 78;
            itemStructure.Height = Images.Items.Images[i].Height / 78;

            Item item = new Item();
            item.Data = itemStructure;
            item.Init();

            return item;
        }
        public static ItemStructure CreateWeapon(TypeItem type) {
            ItemStructure item = ItemStructure.Create();
            if (type == TypeItem.Dagger) {
                item.Type = type;
                item.Caption = Caption_Dagger[GameSetting.RND.Next(Caption_Dagger.Length)];
                item.ImageName = Images.Items.Names_Dagger[GameSetting.RND.Next(Images.Items.Names_Dagger.Length)];
            } else if (type == TypeItem.Wand) {
                item.Type = type;
                item.Caption = Caption_Wand[GameSetting.RND.Next(Caption_Wand.Length)];
                item.ImageName = Images.Items.Names_Wand[GameSetting.RND.Next(Images.Items.Names_Wand.Length)];
            } else {
                item.Type = TypeItem.Bow;
                item.Caption = Caption_Bow[GameSetting.RND.Next(Caption_Bow.Length)];
                item.ImageName = Images.Items.Names_Bows[GameSetting.RND.Next(Images.Items.Names_Bows.Length)];
            }

            
            item.Physical_Damage = new float[] { GameSetting.RND.Next(10), GameSetting.RND.Next(10) + 10 };
            item.Critical_Strike_Chance = 0.05f;
            item.Attacks_per_Second = (float)Math.Round(GameSetting.RND.NextFloat(0.8f, 2), 2);

            item.Requires_Level = (int)(item.Attacks_per_Second * 10);
            item.Requires_Str = (int)(item.Physical_Damage[0] * 2) + 10;
            item.Requires_Dex = item.Requires_Level * 2 + 10;

            int i = Array.IndexOf<string>(Images.Items.Names, item.ImageName);
            item.Width = Images.Items.Images[i].Width / 78;
            item.Height = Images.Items.Images[i].Height / 78;

            return item;
        }
        public static ItemStructure CreateWeapon(TypeItem type, LevelItem level) {
            ItemStructure item = ItemStructure.Create();
            if (type == TypeItem.Dagger) {
                item.Type = type;
                item.Caption = Caption_Dagger[GameSetting.RND.Next(Caption_Dagger.Length)];
                item.ImageName = Images.Items.Names_Dagger[GameSetting.RND.Next(Images.Items.Names_Dagger.Length)];
            } else if (type == TypeItem.Wand) {
                item.Type = type;
                item.Caption = Caption_Wand[GameSetting.RND.Next(Caption_Wand.Length)];
                item.ImageName = Images.Items.Names_Wand[GameSetting.RND.Next(Images.Items.Names_Wand.Length)];
            } else {
                item.Type = TypeItem.Bow;
                item.Caption = Caption_Bow[GameSetting.RND.Next(Caption_Bow.Length)];
                item.ImageName = Images.Items.Names_Bows[GameSetting.RND.Next(Images.Items.Names_Bows.Length)];
            }
            item.Level = level;
            
            if (level == LevelItem.Magic) { 
                
            }
            else if (level == LevelItem.Rare) { }
            else if (level == LevelItem.Unique) { }

            
            item.Physical_Damage = new float[] { GameSetting.RND.Next(10), GameSetting.RND.Next(10) + 10 };
            item.Critical_Strike_Chance = 0.05f;
            item.Attacks_per_Second = (float)Math.Round(GameSetting.RND.NextFloat(0.8f, 2), 2);

            item.Requires_Level = (int)(item.Attacks_per_Second * 10);
            item.Requires_Str = (int)(item.Physical_Damage[0] * 2) + 10;
            item.Requires_Dex = item.Requires_Level * 2 + 10;

            int i = Array.IndexOf<string>(Images.Items.Names, item.ImageName);
            item.Width = Images.Items.Images[i].Width / 78;
            item.Height = Images.Items.Images[i].Height / 78;

            return item;
        }

        public enum FlaskType { Life = 0, Mana = 1, Hybrid = 2, Utility = 3}
        public static ItemStructure CreateFlask() {
            Array array = Enum.GetValues(typeof(FlaskType));
            return CreateFlask((FlaskType)array.GetValue(GameSetting.RND.Next(array.Length)));
        }
        public static ItemStructure CreateFlask(FlaskType flaskType) {
            ItemStructure item = ItemStructure.Create();
            if (flaskType == FlaskType.Life) { item.Caption = Caption_Flask_Life[GameSetting.RND.Next(Caption_Flask_Life.Length)]; item.Life = 1000; }
            else if (flaskType == FlaskType.Mana) { item.Caption = Caption_Flask_Mana[GameSetting.RND.Next(Caption_Flask_Mana.Length)]; item.Mana = 300; }
            else if (flaskType == FlaskType.Hybrid) { item.Caption = Caption_Flask_Hybrid[GameSetting.RND.Next(Caption_Flask_Hybrid.Length)]; item.Life = 1000; item.Mana = 300; }
            else if (flaskType == FlaskType.Utility) { item.Caption = Caption_Flask_Utility[GameSetting.RND.Next(Caption_Flask_Utility.Length)]; }
            item.Type = TypeItem.Flask;
            item.ImageName = Images.Items.Names_Flask[GameSetting.RND.Next(Images.Items.Names_Flask.Length)];

            
            item.Time = 2;
            item.Usage = 10;
            item.Capacity = 30;
            item.Charges = 0;


            item.Description = "Right click to drink. Can only hold charges" + Constants.vbCrLf + "while in belt. Refills as you kill monsters.";

            item.Width = 1;
            item.Height = 2;

            return item;
        }

        public static ItemStructure CreateBodyArmor() {
            ItemStructure item = ItemStructure.Create();
            item.Type = TypeItem.Body_Armour;
            item.ImageName = "BodyDex1A";
            item.Caption = "Plate Vest";
            item.Armor = 4;
            item.Width = 2; item.Height = 3;
            return item;
        }
        public static ItemStructure CreateBoot() {
            ItemStructure item = ItemStructure.Create();
            item.Type = TypeItem.Boot;
            item.Caption = "Iron Greaves";
            item.ImageName = "BootsDex1";
            item.Armor = 4;
            item.Width = 2; item.Height = 2;
            return item;
        }
        public static ItemStructure CreateGlove() {
            ItemStructure item = ItemStructure.Create();
            item.Type = TypeItem.Glove;
            item.ImageName = "GlovesDex1";
            item.Caption = "Iron Gauntlets";
            item.Armor = 1;
            item.Width = 2; item.Height = 2;
            return item;
        }
        public static ItemStructure CreateHelmet() {
            ItemStructure item = ItemStructure.Create();
            item.Type = TypeItem.Helmet;
            item.ImageName = "HelmetDex1";
            item.Caption = "Iron Hat";
            item.Armor = 1;
            item.Width = 2; item.Height = 2;
            return item;
        }

        public static Mod CreateMod() {
            string[] Strings_Enum = Enum.GetNames(typeof(ModType));
            ModType modType = (ModType)Enum.Parse(typeof(ModType), Strings_Enum[GameSetting.RND.Next(Strings_Enum.Length)]);

            Mod mod = new Mod();
            mod.Type = modType;
            //if (mod.Type == ModType.Life) { mod.Values = new float[1] { GameSetting.RND.Next(1, 40) }; }

            return mod;
        }
        public static Mod[] CreateMods(int len, int level) {
            Mod[] s = new Mod[] { };

            return s;
        }
    }
}
