using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.VisualBasic;

namespace Path_of_Defender {
    public enum LevelItem { Normal = 0, Magic = 1, Rare = 2, Unique = 3 }
    public enum TypeItem { Other = 0, Amulet = 1, Belt = 2, Ring = 3, Quiver = 4, Body_Armour = 5, Boot = 6, Glove = 7, Helmet = 8, Shield = 9, Bow = 10, Dagger = 11, Wand = 12, Flask = 13, Currency = 14 }
    public enum ModType {
        None = 0, Strength = 1, Dexterity = 2, Intelligence = 3, Attributes = 4,

        Life = 10, Increased_Life = 11, Life_Regeneration = 12, Life_Regeneration_Percent = 14, Life_on_Kill = 16, Life_on_Hit = 18,
        Mana = 40, Increased_Mana = 41, Mana_Regeneration = 42, Mana_Regeneration_Percent = 44, Mana_on_Kill = 46, Mana_on_Hit = 48, Increased_Mana_Regeneration = 50, Reduced_Mana_Cost = 52, Reduced_Cooldown = 54,

        Energy = 70, Increased_Energy = 71, Faster_Start_of_Energy_Recharge = 73,

        Chance_Block = 100,
        Armor = 110, Increased_Armor = 112, Increased_Armor_Energy = 114,
        Fire_Resistance = 140, Cold_Resistance = 141, Lightning_Resistance = 142, Chaos_Resistance = 143, Elemental_Resistances = 144,

        Increased_Attack_Speed = 150, Increased_Cast_Speed = 152, Increased_Duration = 154, Increased_Buff_Duration = 155, Increased_AOE = 156,
        Increased_Critical_Strike_Chance = 158, Increased_Critical_Strike_Multiplier = 160,
        Increased_Critical_Strike_Chance_For_Spells = 162, Increased_Critical_Strike_Multiplier_For_Spells = 164,

        #region Physical Damage
        Increased_Physical_Damage = 200, Reduced_Enemy_Stun_Threshold = 202, Increased_Stun_Duration = 204,
        Physical_Damage_Attack = 210, Physical_Damage_Spell = 220,
        #endregion

        #region Fire Damage
        Increased_Fire_Damage = 300, Chance_Ignite = 302, Increased_Ignite_Duration = 304,
        Fire_Damage_Attack = 310, Fire_Damage_Spell = 320,
        #endregion
        #region Cold Damage
        Increased_Cold_Damage = 400, Chance_Freeze = 402, Increased_Freeze_Duration = 404, Increased_Chill_Duration = 406,
        Cold_Damage_Attack = 410, Cold_Damage_Spell = 420,
        #endregion
        #region Lightning Damage
        Increased_Lightning_Damage = 500, Chance_Shock = 502, Increased_Shock_Duration = 504,
        Lightning_Damage_Attack = 510, Lightning_Damage_Spell = 520,
        #endregion

        Increased_Spell_Damage = 590,
        Increased_Chaos_Damage = 600,
        Chaos_Damage_Attack = 610, Chaos_Damage_Spell = 620,

        Chance_Knockback = 700, Knockback_Distance = 702,
        Increased_Knockback_Distance = 704, Knockback_on_Crit = 706,

        Projectiles = 720,
        
        Identifies_an_item = 1000000
    }

    public struct Mod {
        public ModType Type;
        public float Value1, Value2;
        public Mod(ModType type, float value1) { Type = type; Value1 = value1; Value2 = 0; }
        public Mod(ModType type, float value1, float value2) { Type = type; Value1 = value1; Value2 = value2; }

        public static string BaseModString(ModType type) {
            if (type == ModType.Strength) { return "+? to Strength"; }
            else if (type == ModType.Dexterity) { return "+? to Dexterity"; }
            else if (type == ModType.Intelligence) { return "+? to Intelligence"; }
            else if (type == ModType.Attributes) { return "+? to all Attributes"; }

            else if (type == ModType.Life) { return "+? to maximum Life"; }
            else if (type == ModType.Increased_Life) { return "?% increased maximum Life"; }
            else if (type == ModType.Life_Regeneration) { return "? Life Regenerated per second"; }
            else if (type == ModType.Life_Regeneration_Percent) { return "?% of Life Regenerated per second"; }
            else if (type == ModType.Life_on_Kill) { return "+? Life Gained on Kill"; }
            else if (type == ModType.Life_on_Hit) { return "+? Life gained for each enemy hit by your Attacks"; }

            else if (type == ModType.Mana) { return "+? to maximum Mana"; }
            else if (type == ModType.Increased_Mana) { return "?% increased maximum Mana"; }

            else if (type == ModType.Mana_Regeneration) { return "? Mana Regenerated per second"; }
            else if (type == ModType.Mana_Regeneration_Percent) { return "?% of Mana Regenerated per Second"; }
            else if (type == ModType.Mana_on_Kill) { return "+? Mana Gained on Kill"; }
            else if (type == ModType.Mana_on_Hit) { return "? Mana gained for each enemy hit by your Attacks"; }
            else if (type == ModType.Increased_Mana_Regeneration) { return "?% increased Mana Regeneration Rate"; }
            else if (type == ModType.Reduced_Mana_Cost) { return "?% reduced Mana Cost"; }
            else if (type == ModType.Reduced_Cooldown) { return "?% reduced Cooldown"; }

            else if (type == ModType.Energy) { return "+? to maximum Energy Shield"; }
            else if (type == ModType.Increased_Energy) { return "?% increased Energy Shield"; }
            else if (type == ModType.Faster_Start_of_Energy_Recharge) { return "?% faster start of Energy Shield Recharge"; }

            else if (type == ModType.Chance_Block) { return "?% Chance to Block"; }
            else if (type == ModType.Armor) { return "+? to Armor"; }
            else if (type == ModType.Increased_Armor) { return "?% increased Armor"; }
            else if (type == ModType.Increased_Armor_Energy) { return "?% increased Armor and Energy Shield"; }

            else if (type == ModType.Fire_Resistance) { return "+?% to Fire Resistance"; }
            else if (type == ModType.Cold_Resistance) { return "+?% to Cold Resistance"; }
            else if (type == ModType.Lightning_Resistance) { return "+?% to Lightning Resistance"; }
            else if (type == ModType.Chaos_Resistance) { return "+?% to Chaos Resistance"; }
            else if (type == ModType.Elemental_Resistances) { return "+?% to all Elemental Resistances"; }

            else if (type == ModType.Increased_Attack_Speed) { return "?% increased Attack Speed"; }
            else if (type == ModType.Increased_Cast_Speed) { return "?% increased Cast Speed"; }
            else if (type == ModType.Increased_Duration) { return "?% increased Duration"; }
            else if (type == ModType.Increased_Buff_Duration) { return "?% increased Buff Duration"; }
            else if (type == ModType.Increased_AOE) { return "?% increased Area of Effect Radius"; }

            else if (type == ModType.Increased_Critical_Strike_Chance) { return "?% increased Critical Strike Chance"; }
            else if (type == ModType.Increased_Critical_Strike_Multiplier) { return "?% increased Critical Strike Multiplier"; }
            else if (type == ModType.Increased_Critical_Strike_Chance_For_Spells) { return "?% increased Critical Strike Chance for Spell"; }
            else if (type == ModType.Increased_Critical_Strike_Multiplier_For_Spells) { return "?% increased Critical Strike Multiplier for Spell"; }

            else if (type == ModType.Increased_Physical_Damage) { return "?% increased Physical Damage"; }
            else if (type == ModType.Reduced_Enemy_Stun_Threshold) { return "?% reduced Enemy Stun Threshold"; }
            else if (type == ModType.Increased_Stun_Duration) { return "?% increased Stun Duration on enemies"; }
            else if (type == ModType.Physical_Damage_Attack) { return "Adds ? - ? Physical Damage to Attack"; }
            else if (type == ModType.Physical_Damage_Spell) { return "Adds ? - ? Physical Damage to Spell"; }

            else if (type == ModType.Increased_Fire_Damage) { return "?% increased Fire Damage"; }
            else if (type == ModType.Chance_Ignite) { return "?% chance to Ignite"; }
            else if (type == ModType.Increased_Ignite_Duration) { return "?% Increased Ignite Duration on Enemies"; }
            else if (type == ModType.Fire_Damage_Attack) { return "Adds ? - ? Fire Damage to Attack"; }
            else if (type == ModType.Fire_Damage_Spell) { return "Adds ? - ? Fire Damage to Spell"; }

            else if (type == ModType.Increased_Cold_Damage) { return "?% increased Cold Damage"; }
            else if (type == ModType.Chance_Freeze) { return "?% chance to Freeze"; }
            else if (type == ModType.Increased_Freeze_Duration) { return "?% Increased Freeze Duration on Enemies"; }
            else if (type == ModType.Increased_Chill_Duration) { return "?% Increased Chill Duration on Enemies"; }
            else if (type == ModType.Cold_Damage_Attack) { return "Adds ? - ? Cold Damage to Attack"; }
            else if (type == ModType.Cold_Damage_Spell) { return "Adds ? - ? Cold Damage to Spell"; }

            else if (type == ModType.Increased_Lightning_Damage) { return "?% increased Lightning Damage"; }
            else if (type == ModType.Chance_Shock) { return "?% chance to Shock"; }
            else if (type == ModType.Increased_Shock_Duration) { return "?% Increased Shock Duration on Enemies"; }
            else if (type == ModType.Lightning_Damage_Attack) { return "Adds ? - ? Lightning Damage to Attack"; }
            else if (type == ModType.Lightning_Damage_Spell) { return "Adds ? - ? Lightning Damage to Spell"; }

            else if (type == ModType.Increased_Spell_Damage) { return "?% increased Spell Damage"; }
            else if (type == ModType.Increased_Chaos_Damage) { return "?% increased Chaos Damage"; }
            else if (type == ModType.Chaos_Damage_Attack) { return "Adds ? - ? Chaos Damage to Attack"; }
            else if (type == ModType.Chaos_Damage_Spell) { return "Adds ? - ? Chaos Damage to Spell"; }

            else if (type == ModType.Chance_Knockback) { return "?% Chance to Knockback"; }
            else if (type == ModType.Knockback_Distance) { return "+?m to Knockback Distance"; }
            else if (type == ModType.Increased_Knockback_Distance) { return "?% Increased Knockback Distance"; }
            else if (type == ModType.Knockback_on_Crit) { return "Knocks Back enemies if you get a Critical Strike"; }
            else if (type == ModType.Projectiles) { return "? additional Projectiles"; }

            else if (type == ModType.Identifies_an_item) { return "Identifies an item"; }
            else { return ""; }
        }
        public override string ToString() {
            return ToString(Type, Value1, Value2);
        }
        public static string ToString(ModType type, float value1) {
            String Return = BaseModString(type);
            int i = Strings.InStr(Return, "?");
            if (i != 0) {
                if (Strings.Mid(Return, i + 1, 1) == "%") { Return = Strings.Replace(Return, "?", Math.Round(value1 * 100).ToString(), 1, 1); }
                else { Return = Strings.Replace(Return, "?", value1.ToString(), 1, 1); }
            }
            return Return;
        }
        public static string ToString(ModType type, float value1, float value2) {
            String Return = BaseModString(type);
            int i, ii = 0;
            i = Strings.InStr(Return, "?");
            if (i != 0) { ii = Strings.InStr(i + 1, Return, "?"); }

            if (i != 0) {
                if (Strings.Mid(Return, i + 1, 1) == "%") { Return = Strings.Replace(Return, "?", Math.Round(value1 * 100).ToString(), 1, 1); }
                else { Return = Strings.Replace(Return, "?", value1.ToString(), 1, 1); }
            }
            if (ii != 0) {
                if (Strings.Mid(Return, ii + 1, 1) == "%") { Return = Strings.Replace(Return, "?", Math.Round(value2 * 100).ToString(), 1, 1); }
                else { Return = Strings.Replace(Return, "?", value2.ToString(), 1, 1); }
            }
            return Return;
        }
    }


    public struct ItemStructure {
        /// <summary> <para> Set default value for fields. </para>
        /// Width = 1, Height = 1, Stack_Max = 10, Attacks_per_Second = 1
        /// </summary>
        public static ItemStructure Create() {
            ItemStructure item = new ItemStructure();
            item.Name = "";
            item.Caption = "";
            item.Description = "";
            item.Level = LevelItem.Normal;
            item.Type = TypeItem.Other;
            item.ImageName = "CurrencyGemQuality";

            item.Physical_Damage = new float[2];
            item.Attacks_per_Second = 1;            

            item.Time = 3;
            item.Capacity = 30;
            item.Usage = 10;

            item.Stack = 0;
            item.Stack_Max = 10;

            item.Mods = new Mod[0];
            item.Cost = new ItemCost[0];

            item.Width = 1;
            item.Height = 1;

            return item;
            //return 
        }
        #region Properties  
        [Category("Important")] public string Name;
        [Category("Important")] public string Caption;
        [Category("Important")] public string Description;
        [Category("Important")] public LevelItem Level;
        [Category("Important")] public TypeItem Type;
        [Category("Important")] public string ImageName;

        public int Quality;
        #region Weapon Basic Info
        [Category("Weapon")] public float[] Physical_Damage;
        [Category("Weapon")] public float Critical_Strike_Chance;
        [Category("Weapon")] public float Attacks_per_Second;
        #endregion

        #region Armor Basic Info
        [Category("Armor")] public float Armor;
        [Category("Armor")] public float Evasion;
        [Category("Armor")] public float Energy_Shield;
        #endregion

        #region Flask Basic Info
        [Category("Flask")] public float Life;
        [Category("Flask")] public float Mana;
        [Category("Flask")] public float Time;
        [Category("Flask")] public float Capacity;
        [Category("Flask")] public float Usage;
        [Category("Flask")] public float Charges;
        #endregion

        #region Currency Basic Info
        [Category("Currency")] public int Stack;
        [Category("Currency")] public int Stack_Max;
        #endregion


        [Category("Requires")] public int Requires_Level;
        [Category("Requires")] public int Requires_Str;
        [Category("Requires")] public int Requires_Dex;
        [Category("Requires")] public int Requires_Int;

        [Category("Property")] public Mod Default_Mod;
        [Category("Property")] public Mod[] Mods;

        [Category("Socket")] public int Socket;
        [Category("Socket")] public int Socket_Max;
        [Category("Socket")] public SocketStructure[] Sockets;

        [Category("Cost")] public ItemCost[] Cost;
        [Category("Cost")] public int Cost_Gold;

        [Category("Collision")] public int Width;
        [Category("Collision")] public int Height;
        #endregion

        public string GetText(bool getCost = false) { 
            string Total_Text = "";
            
            Total_Text += Caption;
            Total_Text += GetBasicInfo();
            Total_Text += GetRequires();
            Total_Text += GetProperties();
            if (Description != "") { Total_Text += Constants.vbCrLf + Description; }
            if (getCost == true) {
                Total_Text += GetCost();
            }

            return Total_Text;
        }

        public string GetBasicInfo() {
            string Text = "";
            if (Type == TypeItem.Bow || Type == TypeItem.Dagger || Type == TypeItem.Wand) {
                Text += Constants.vbCrLf + Type.ToString();
                Text += Constants.vbCrLf + "Physical Damage: " + Physical_Damage[0].ToString() + " - " + Physical_Damage[1].ToString();
                Text += Constants.vbCrLf + "Critical Strike Chance: " + (int)(Critical_Strike_Chance * 100) + "%";
                Text += Constants.vbCrLf + "Attacks per Second: " + Math.Round(Attacks_per_Second, 2);
            } else if (Type == TypeItem.Belt || Type == TypeItem.Body_Armour || Type == TypeItem.Boot || Type == TypeItem.Glove || Type == TypeItem.Helmet || Type == TypeItem.Shield) {
                if (Armor > 0) { Text += Constants.vbCrLf + "Armor: " + Armor; }
                if (Evasion > 0) { Text += Constants.vbCrLf + "Evasion: " + Armor; }
                if (Energy_Shield > 0) { Text += Constants.vbCrLf + "Energy Shield: " + Energy_Shield; }
            } else if (Type == TypeItem.Currency) {
                Text += Constants.vbCrLf + "Stack Size: " + Stack + "/" + Stack_Max;
            } else if (Type == TypeItem.Flask) {
                //if (Life > 0) { Text += Constants.vbCrLf + "Recovers " + Life.ToString() + " Life Over " + Time.ToString() + " Seconds"; }
                //if (Mana > 0) { Text += Constants.vbCrLf + "Recovers " + Mana.ToString() + " Mana Over " + Time.ToString() + " Seconds"; }
                Text += Constants.vbCrLf + "Lasts " + Time.ToString() + " Seconds";
                if (Life > 0) { Text += Constants.vbCrLf + "Recovers " + Life.ToString() + " Life"; }
                if (Mana > 0) { Text += Constants.vbCrLf + "Recovers " + Mana.ToString() + " Mana"; }
                Text += Constants.vbCrLf + "Consumes " + Usage.ToString() + " of " + Capacity.ToString() + " Charges on use";
                Text += Constants.vbCrLf + "Currently Has " + Charges.ToString() + " Charges";
            }

            return Strings.Mid(Text, 3);
        }
        public string GetRequires() {
            string Text = "";
            #region Requires
            //Requires Level 6, 21 Str, 7 Dex, 37 Int
            if (Type == TypeItem.Currency) { 

            } else if (Requires_Level > 0 || Requires_Str > 0 || Requires_Dex > 0 || Requires_Int > 0) {
                Text += "Requires";
                if (Requires_Level > 0) { Text += " Level " + Requires_Level; }
                if (Requires_Str > 0) { Text += ", " + Requires_Str + " Str"; }
                if (Requires_Dex > 0) { Text += ", " + Requires_Dex + " Dex"; }
                if (Requires_Int > 0) { Text += ", " + Requires_Int + " Int"; }
            }
            #endregion
            return Text;
        }
        public string GetProperties() {
            string Text = "";
            for (int i = 0; i < Mods.Length; i++) { Text += Constants.vbCrLf + Mods[i].ToString(); }
            return Strings.Mid(Text, 3);
        }
        public string GetCost() {
            string Text = "";
            for (int i = 0; i < Cost.Length; i++) {
                Text += Constants.vbCrLf + Cost[i].Quantity.ToString() + " x " + Cost[i].Item_Base_Name;
            }
            if (Cost_Gold != 0) { Text += Constants.vbCrLf + Cost_Gold.ToString() + " Gold"; }

            if (Text == "") { return Text; }

            return "Cost:" + Text;
        }
    }
    public struct SocketStructure {
        public ItemStructure ItemStructure;
        public bool Used;
    }
    public struct ItemCost {
        public ItemCost(string item_Base_Name, int quantity) {
            Item_Base_Name = item_Base_Name;
            Quantity = quantity;
        }
        public string Item_Base_Name;
        public int Quantity;
        public override string ToString() {
            return Quantity + " x " + Item_Base_Name;
        }
    }
}
