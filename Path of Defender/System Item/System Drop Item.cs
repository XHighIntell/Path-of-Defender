using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Graphics;

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
    public struct RangeValues {
        public float Minimum, Maximum;
        public RangeValues(float minimum, float maximum) { Minimum = minimum; Maximum = maximum; }
        public override string ToString() {
            return Minimum.ToString() + " - " + Maximum.ToString();
        }
    }
    public struct Affix {
        public string Caption;
        public int Level;
        public ModType Type;
        public RangeValues Value1, Value2;
        public bool IsPercent;


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
        public SystemItem(SystemItemStructure source) { data = source; }
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

        public LevelItem RandomRarity(float magic_precent, float rare_percent, float unique_percent) {
            float i = GameSetting.RND.NextFloat(0, 1);
            if (i <= magic_precent) { return LevelItem.Magic; }
            else if (magic_precent < i && i <= magic_precent + rare_percent) { return LevelItem.Rare; }
            else if (magic_precent + rare_percent < i && i <= magic_precent + rare_percent + unique_percent) { return LevelItem.Unique; }
            else { return LevelItem.Normal; }
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
            return RandomMagicItem(white_item, white_item.Requires_Level);
        }
        public ItemStructure RandomMagicItem(ItemStructure white_item, int max_Mod_Level) {
            Affix[] prefixes = GetPrefixes(max_Mod_Level);
            Affix[] suffixes = GetSuffixes(max_Mod_Level);

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
            return RandomRareItem(white_item, white_item.Requires_Level);
        }
        public ItemStructure RandomRareItem(ItemStructure white_item, int max_Mod_Level) {
            return RandomRareItem(white_item, max_Mod_Level, 3, 6);
        }
        public ItemStructure RandomRareItem(ItemStructure white_item, int max_Mod_Level, int min_Mods, int max_Mods) {
            int max_mods = GameSetting.RND.Next(min_Mods, max_Mods + 1);

            int number_prefixes = max_mods / 2;
            int number_suffixes = max_mods - number_prefixes;

            Affix[] prefixes = RandomPrefixes(max_Mod_Level, number_prefixes);
            Affix[] suffixes = RandomSuffixes(max_Mod_Level, number_suffixes);

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
    }
}
