using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Graphics;
using Microsoft.VisualBasic;

namespace Path_of_Defender {
    using UI = Images.UI;
    using System.Reflection;
    public class PlayerStats {
        public float Strength, Dexterity, Intelligence;

        public float Life, Increased_Life, Life_Regeneration, Life_Regeneration_Percent, Life_on_Kill, Life_on_Hit;
        public float Mana, Increased_Mana, Mana_Regeneration, Mana_Regeneration_Percent, Mana_on_Kill, Mana_on_Hit, Increased_Mana_Regeneration;
        public float Energy, Increased_Energy, Faster_Start_of_Energy_Recharge;

        public float Reduced_Mana_Cost, Reduced_Cooldown;

        //public float Increased_Mana_Cost, Increased_Cooldown; <- impossible
        //public float Multiplier_Mana_Cost; <-impossible nếu có thì phải nằm ở skill

        public float Chance_Block;
        public float Armor, Increased_Armor;
        public float Fire_Resistance, Cold_Resistance, Lightning_Resistance, Chaos_Resistance;

        public float Increased_Attack_Speed, Increased_Cast_Speed, Increased_Duration, Increased_Buff_Duration, Increased_AOE;
        public float Increased_Critical_Strike_Chance, Increased_Critical_Strike_Multiplier;
        public float Increased_Critical_Strike_Chance_For_Spells, Increased_Critical_Strike_Multiplier_For_Spells;

        #region Attack Damage
        public float[] Physical_Damage_Attack = new float[2] { 1, 2 };
        public float[] Fire_Damage_Attack = new float[2] { 0, 0 };
        public float[] Cold_Damage_Attack = new float[2] { 0, 0 };
        public float[] Lightning_Damage_Attack = new float[2] { 0, 0 };
        public float[] Chaos_Damage_Attack = new float[2] { 0, 0 };
        #endregion
        #region Spell Damage
        public float[] Physical_Damage_Spell = new float[2] { 0, 0 };
        public float[] Fire_Damage_Spell = new float[2] { 0, 0 };
        public float[] Cold_Damage_Spell = new float[2] { 0, 0 };
        public float[] Lightning_Damage_Spell = new float[2] { 0, 0 };
        public float[] Chaos_Damage_Spell = new float[2] { 0, 0 };
        #endregion

        #region Physical Damage
        public float Increased_Physical_Damage;
        public float Reduced_Enemy_Stun_Threshold;
        public float Increased_Stun_Duration;
        #endregion
        #region Fire Damage
        public float Increased_Fire_Damage;
        public float Chance_Ignite, Increased_Ignite_Duration;
        #endregion
        #region Cold Damage
        public float Increased_Cold_Damage;
        public float Chance_Freeze, Increased_Freeze_Duration, Increased_Chill_Duration;
        #endregion
        #region Lightning Damage
        public float Increased_Lightning_Damage;
        public float Chance_Shock, Increased_Shock_Duration;
        #endregion
        #region Spell Damage
        public float Increased_Spell_Damage;
        #endregion
        #region Chaos Damage
        public float Increased_Chaos_Damage;
        #endregion


        #region Special - Knockback, Projectiles
        public float Chance_Knockback, Knockback_Distance = 100f, Increased_Knockback_Distance;
        public bool Knockback_on_Crit;
        public int Projectiles;
        #endregion

        #region Split Arrow
        public float Split_Arrow_Multiplier_Weapon_Damage = 0.8f;
        public float Split_Arrow_Multiplier_Damage = 1;
        public float Split_Arrow_Mana_Cost;
        public int Split_Arrow_Additional_Projectiles;
        public float Split_Arrow_Increased_Attack_Speed;

        #endregion

        #region Firestorm Ver 0.1 full damage
        public float[] Firestorm_Physical_Damage = new float[2];
        public float[] Firestorm_Fire_Damage = new float[2];
        public float[] Firestorm_Cold_Damage = new float[2];
        public float[] Firestorm_Lightning_Damage = new float[2];
        public float[] Firestorm_Chaos_Damage = new float[2];

        public float Firestorm_Chance_Ignite, Firestorm_Increased_Ignite_Duration;

        public float Firestorm_Mana_Cost;
        public float Firestorm_Duration, Firestorm_Increased_Duration, Firestorm_Increased_AOE;
        public float Firestorm_Interval;
        public float Firestorm_Critical_Strike_Chance;

        #endregion

        #region Vitality
        public float Vitality_Duration, Vitality_Increased_Duration;
        public float Vitality_Life_Regeneration, Vitality_Life_Regeneration_Percent;
        public float Vitality_Mana_Cost;
        #endregion

        #region FrostNova
        public float[] FrostNova_Cold_Damage = new float[2];
        public float FrostNova_Chance_Freeze, FrostNova_Increased_Freeze_Duration, FrostNova_Increased_Chill_Duration, FrostNova_Increased_Radius;
        public float FrostNova_Mana_Cost;
        #endregion

        #region Arc Ver 0.1
        public float[] Arc_Lightning_Damage = new float[2];
        public float Arc_Chance_Shock, Arc_Increased_Shock_Duration;
        public float Arc_Mana_Cost, Arc_Increased_Mana_Cost, Arc_Chain;
        public float Arc_Critical_Strike_Chance;
        public float Arc_Increased_Cast_Speed;
        #endregion

        #region Spectral Throw
        public float Spectral_Throw_Multiplier_Weapon_Damage;
        public float Spectral_Throw_Multiplier_Damage = 1;
        public float Spectral_Throw_Mana_Cost;
        public int Spectral_Throw_Additional_Projectiles;
        #endregion

        #region Mana Shield
        public float Mana_Shield_Absorbs_Percent;
        public float Mana_Shield_Damage_Absorbed_Per_Mana;
        #endregion

        #region Barrage
        public float Barrage_Arrow_Multiplier_Weapon_Damage;
        public float Barrage_Arrow_Multiplier_Damage = 1;
        public float Barrage_Arrow_Mana_Cost;
        public int Barrage_Additional_Projectiles;
        #endregion
    }
    public partial class Player {
        public const float Unarmed_Attack_per_Second = 1.2f;
        public float Life, Life_Maximum, Mana, Mana_Maximum, Energy_Shield, Energy_Shield_Maximum;
        
        public float MaxXP = 10, CurXP;
        public int Level = 1;
        public int Gold;
        public int PassivePointsLeft;
        public PlayerStats Stats = new PlayerStats();
        public BuffCollection Buffs = new BuffCollection();

        public PassiveTreeData PassiveTreeData = new PassiveTreeData();

        public Item[] Items = new Item[0];
        public Item Helmet, Amulet, Weapon, Offhand_Weapon, Ring_Left, Ring_Right, Body_Armor, Glove, Belt, Boots;
        public Item[] Flasks = new Item[5];

        public double TimeAttackRemain;
        //Skill
        public Skill[] LearnSkills = { };
        public int[] ShortSkills = new int[8] { 0, -1, -1, -1, -1, -1, -1, -1 };
        
        public void NewStats(float strength, float dexterity, float intelligence, float base_Life, float base_Energry, float base_Mana) {
            Stats = new PlayerStats();
            ReceiveStrength(strength);
            ReceiveDexterity(dexterity);
            ReceiveIntelligence(intelligence);

            ReceiveLife(base_Life);
            ReceiveEnergy(base_Energry);
            ReceiveMana(base_Mana);

            //Stats.Life = base_Life;
            //Stats.Energy_Shield = base_Energry_Shield;
            //Stats.Mana = base_Mana;
            
            //Life_Maximum = Stats.Strength * 0.5f + Stats.Life; Life = Life_Maximum;
            //Energy_Shield_Maximum = Stats.Energy_Shield; Energy_Shield = Energy_Shield_Maximum;
            //Mana_Maximum = Stats.Intelligence * 0.5f + Stats.Mana; Mana = Mana_Maximum;
            Life = Life_Maximum;
            Energy_Shield = Energy_Shield_Maximum;
            Mana = Mana_Maximum;
        }

        #region Passive
        public void RefreshAllValueFromPassive(PassiveSkill[] oldSkills, PassiveSkill[] newSkills) {
            for (int x = 0; x < newSkills.Length; x++) {
                if (newSkills[x].Status == SkillStatus.Allocated && (oldSkills[x].Status == SkillStatus.Unallocated || oldSkills[x].Status == SkillStatus.CanAllocated)) {
                    for (int i = 0; i < newSkills[x].Properties.Length; i++) {
                        ReceiveProperty(newSkills[x].Properties[i].Type, newSkills[x].Properties[i].Value);
                    } 
                }
            }


        }
        private void ReceiveProperty(TypeProperty e, float[] value) {
            if (e == TypeProperty.Split_Arrow || e == TypeProperty.Firestorm || e == TypeProperty.Vitality || e == TypeProperty.FrostNova || e == TypeProperty.Arc ||
                e == TypeProperty.Spectral_Throw || e == TypeProperty.Mana_Shield || e == TypeProperty.Barrage) {
                ReceiveSkill(e.ToString());
            }
            else if (e == TypeProperty.Strength) { ReceiveStrength(value[0]); }
            else if (e == TypeProperty.Dexterity) { ReceiveDexterity(value[0]); }
            else if (e == TypeProperty.Intelligence) { ReceiveIntelligence(value[0]); }
            else if (e == TypeProperty.Attributes) { ReceiveAttributes(value[0]); }

            else if (e == TypeProperty.Life) { ReceiveLife(value[0]); }
            else if (e == TypeProperty.Increased_Life) { ReceiveIncreasedLife(value[0]); }
            else if (e == TypeProperty.Mana) { ReceiveMana(value[0]); }
            else if (e == TypeProperty.Increased_Mana) { ReceiveIncreasedMana(value[0]); }
            else if (e == TypeProperty.Energy) { ReceiveEnergy(value[0]); }
            else if (e == TypeProperty.Increased_Energy) { ReceiveIncreasedEnergy(value[0]); }

            else if (e == TypeProperty.Elemental_Resistances) { ReceiveElementalResistances(value[0]); }
            else if (e == TypeProperty.Armor) { ReceiveArmor(value[0]); }
            else if (e == TypeProperty.Increased_Armor) { ReceiveIncreasedArmor(value[0]); }
            else if (e == TypeProperty.Increased_Armor_Energy) { ReceiveArmor(value[0]); ReceiveIncreasedEnergy(value[0]); }

            // Những trường hợp cố định.
            else if (e == TypeProperty.Split_Arrow_Multiplier_Weapon_Damage || e == TypeProperty.Barrage_Arrow_Multiplier_Weapon_Damage) {
                System.Reflection.FieldInfo FieldInfo = typeof(PlayerStats).GetField(e.ToString());
                if (FieldInfo == null) { return; }
                if (FieldInfo.FieldType.Name == "Int32") { FieldInfo.SetValue(Stats, value[0]); }
                else if (FieldInfo.FieldType.Name == "Single") { FieldInfo.SetValue(Stats, value[0]); }
            }

            //Những trường hợp tích
            else if (e == TypeProperty.Split_Arrow_Multiplier_Damage || e == TypeProperty.Barrage_Arrow_Multiplier_Damage) {
                System.Reflection.FieldInfo FieldInfo = typeof(PlayerStats).GetField(e.ToString());
                if (FieldInfo == null) { return; }
                if (FieldInfo.FieldType.Name == "Single") { FieldInfo.SetValue(Stats, (float)FieldInfo.GetValue(Stats) * value[0]); }
            }

            else {
                System.Reflection.FieldInfo FieldInfo = typeof(PlayerStats).GetField(e.ToString());

                if (FieldInfo == null) { return; }
                if (FieldInfo.FieldType.Name == "Int32") { FieldInfo.SetValue(Stats, (int)FieldInfo.GetValue(Stats) + (int)value[0]); }
                else if (FieldInfo.FieldType.Name == "Single") { FieldInfo.SetValue(Stats, (float)FieldInfo.GetValue(Stats) + (float)value[0]); }
                else if (FieldInfo.FieldType.Name == "Boolean") { FieldInfo.SetValue(Stats, true); }
                else if (FieldInfo.FieldType.Name == "Single[]") {
                    Array My_Values = (Array)FieldInfo.GetValue(Stats);
                    for (int i = 0; i < value.Length; i++) { if (i < My_Values.Length) { My_Values.SetValue(value[i] + (float)My_Values.GetValue(i), i); } }
                    FieldInfo.SetValue(Stats, My_Values);
                }
            }
        }
        #endregion

        public void ReceiveStrength(float strength) {
            ReceiveLife(strength * 0.5f);
            Stats.Increased_Physical_Damage += strength * 0.002f;
            Stats.Increased_Stun_Duration += strength * 0.002f;
            Stats.Strength += strength;
        }
        public void ReceiveDexterity(float dexterity) {
            ReceiveArmor(dexterity * 0.5f);
            Stats.Increased_Attack_Speed += dexterity * 0.002f;
            Stats.Increased_Cast_Speed += dexterity * 0.002f;
            Stats.Dexterity += dexterity;
        }
        public void ReceiveIntelligence(float intelligence) {
            ReceiveMana(intelligence * 0.5f);
            ReceiveEnergy(intelligence * 0.5f);
            Stats.Increased_Spell_Damage += intelligence * 0.002f;
            Stats.Intelligence += intelligence;
        }
        public void ReceiveAttributes(float value) {
            ReceiveStrength(value);
            ReceiveDexterity(value);
            ReceiveIntelligence(value);
        }

        public void ReceiveIncreasedLife(float increased) {
            float tmp = (1 + Stats.Increased_Life + increased) / (1 + Stats.Increased_Life);
            Life_Maximum = Life_Maximum * tmp;
            Life = Life * tmp;
            Stats.Increased_Life += increased;
        }
        public void ReceiveIncreasedEnergy(float increased) {
            float tmp = (1 + Stats.Increased_Energy + increased) / (1 + Stats.Increased_Energy);
            Energy_Shield_Maximum = Energy_Shield_Maximum * tmp;
            Energy_Shield = Energy_Shield * tmp;
            Stats.Increased_Energy += increased;
        }
        public void ReceiveIncreasedMana(float increased) {
            float tmp = (1 + Stats.Increased_Mana + increased) / (1 + Stats.Increased_Mana);
            Mana_Maximum = Mana_Maximum * tmp;
            Mana = Mana * tmp;
            Stats.Increased_Mana += increased;
        }

        public void ReceiveXP(float xp) {
            CurXP += xp;
            if (CurXP >= MaxXP) { CurXP = 0; PassivePointsLeft += 1; Level++; MaxXP += 5; } 
        }
        public void ReceiveLife(float life) {
            float PercentLife = Life / Life_Maximum;
            Stats.Life += life;
            Life_Maximum += life * (1 + Stats.Increased_Life);
            Life = Life_Maximum * PercentLife;
        }
        public void ReceiveEnergy(float energy) {
            float PercentEnergy = Energy_Shield / Energy_Shield_Maximum;
            Stats.Energy += energy;
            Energy_Shield_Maximum += energy * (1 + Stats.Increased_Life);
            Energy_Shield = Energy_Shield_Maximum * PercentEnergy;
        }
        public void ReceiveMana(float mana) {
            float PercentMana = Mana / Mana_Maximum;
            Stats.Mana += mana;
            Mana_Maximum += mana * (1 + Stats.Increased_Mana);
            Mana = Mana_Maximum * PercentMana;
        }

        public void ReceiveIncreasedArmor(float Increased){
            Stats.Armor = Stats.Armor * (1 + Stats.Increased_Armor + Increased) / (1 + Stats.Increased_Armor);
            Stats.Increased_Armor += Increased;
        }
        public void ReceiveArmor(float armor) { Stats.Armor += armor * (1 + Stats.Increased_Armor); }

        public void ReceiveElementalResistances(float all) { Stats.Fire_Resistance += all; Stats.Cold_Resistance += all; Stats.Lightning_Resistance += all; }


        public void ReceiveSkill(string class_Name) {
            Array.Resize(ref LearnSkills, LearnSkills.Length + 1);
            LearnSkills[LearnSkills.Length - 1] = (Skill)Activator.CreateInstance(Type.GetType("Path_of_Defender." + class_Name));
        }
        public void ReceiveSkill(Skill skill) {
            Array.Resize(ref LearnSkills, LearnSkills.Length + 1);
            LearnSkills[LearnSkills.Length - 1] = skill;
        }

        public void ReceiveItem(Item item) {
            ReceiveProperty(TypeProperty.Armor, new float[] { item.Data.Armor });
            ReceiveProperty(TypeProperty.Energy, new float[] { item.Data.Energy_Shield });
            Stats.Physical_Damage_Attack[0] += item.Data.Physical_Damage[0];
            Stats.Physical_Damage_Attack[1] += item.Data.Physical_Damage[1];
            ReceiveProperty((TypeProperty)item.Data.Default_Mod.Type, new float[] { item.Data.Default_Mod.Value1, item.Data.Default_Mod.Value2 });
            for (int i = 0; i < item.Data.Mods.Length; i++) {
                ReceiveProperty((TypeProperty)item.Data.Mods[i].Type, new float[] { item.Data.Mods[i].Value1, item.Data.Mods[i].Value2 });
            }
        }
        public void RemoveItem(Item item) { 
            ReceiveProperty(TypeProperty.Armor, new float[] { -item.Data.Armor });
            ReceiveProperty(TypeProperty.Energy, new float[] { -item.Data.Energy_Shield });
            Stats.Physical_Damage_Attack[0] -= item.Data.Physical_Damage[0];
            Stats.Physical_Damage_Attack[1] -= item.Data.Physical_Damage[1];
            ReceiveProperty((TypeProperty)item.Data.Default_Mod.Type, new float[] { -item.Data.Default_Mod.Value1, -item.Data.Default_Mod.Value2 });
            for (int i = 0; i < item.Data.Mods.Length; i++) {
                ReceiveProperty((TypeProperty)item.Data.Mods[i].Type, new float[] { -item.Data.Mods[i].Value1, -item.Data.Mods[i].Value2 });
            }
        }

    }
    public partial class Player {
        public void RegenLife(float life) { Life += life; if (Life >= Life_Maximum) { Life = Life_Maximum; } }
        public void RegenMana(float mana) { Mana += mana; if (Mana >= Mana_Maximum) { Mana = Mana_Maximum; } }
        public void RegenEnergy(float energy) { Energy_Shield += energy; if (Energy_Shield > Energy_Shield_Maximum) { Energy_Shield = Energy_Shield_Maximum; } }


        float NonTakeDamage;
        public void Update() {
            if (TimeAttackRemain > 0) { TimeAttackRemain -= GameSetting.SecondPerFrame; }
            if (TimeAttackRemain < 0) { TimeAttackRemain = 0; }
            Buffs.Update();
            RegenLife((Life_Maximum * Stats.Life_Regeneration_Percent + Stats.Life_Regeneration) /60);
            RegenMana((Mana_Maximum * Stats.Mana_Regeneration_Percent + Stats.Mana_Regeneration) * (1 + Stats.Increased_Mana_Regeneration) / 60);
            NonTakeDamage += GameSetting.SecondPerFrame;

            if (NonTakeDamage >= 6 / (1 + Stats.Faster_Start_of_Energy_Recharge)) {
                RegenEnergy(Energy_Shield_Maximum * 0.25f / 60);
            }

            UpdateInput();
            UpdateSkill();
        }
        public void UpdateInput() { 
            Skill skill = GetSkillFromInput();
            if (skill != null) {
                if (skill.Ability == AbilityType.Attack && Weapon == null) { 
                    
                } else {
                    if (skill.GetCastTime() == 0 && Mana >= skill.GetManaRequired()) { skill.Cast(); }
                    else if (TimeAttackRemain <= 0 && Mana >= skill.GetManaRequired()) { skill.Cast(); }
                }
            }
            Item flask = GetFlaskFromInput();
            if (flask != null) {
                if (flask.Data.Charges >= flask.Data.Usage) { 
                    Buffs.Add(new Flask_Buff(flask));
                    flask.Data.Charges -= flask.Data.Usage;
                }
            }
        }


        private void UpdateSkill() {
            for (int i = 0; i < LearnSkills.Length; i++) {
                LearnSkills[i].Update();
            }
        }

        private Skill GetSkillFromInput() {
            if (e.Keyboard.IsKeyDown(Keys.T) == true && ShortSkills[7] != -1) { return LearnSkills[ShortSkills[7]]; }
            else if (e.Keyboard.IsKeyDown(Keys.R) == true && ShortSkills[6] != -1) { return LearnSkills[ShortSkills[6]]; }
            else if (e.Keyboard.IsKeyDown(Keys.E) == true && ShortSkills[5] != -1) { return LearnSkills[ShortSkills[5]]; }
            else if (e.Keyboard.IsKeyDown(Keys.W) == true && ShortSkills[4] != -1) { return LearnSkills[ShortSkills[4]]; }
            else if (e.Keyboard.IsKeyDown(Keys.Q) == true && ShortSkills[3] != -1) { return LearnSkills[ShortSkills[3]]; }
            else if ((e.Mouse.RightButton.Down == true || e.Mouse.RightButton.Pressed == true) && ShortSkills[2] != -1) { return LearnSkills[ShortSkills[2]]; }
            else if ((e.Mouse.MiddleButton.Down == true || e.Mouse.MiddleButton.Pressed == true) && ShortSkills[1] != -1) { return LearnSkills[ShortSkills[1]]; }
            else if (e.Focus_Control == null && (e.Mouse.LeftButton.Down == true || e.Mouse.LeftButton.Pressed == true) && ShortSkills[0] != -1) { return LearnSkills[ShortSkills[0]]; }
            return null;
        }
        private Item GetFlaskFromInput() {
            if (Flasks[0] != null & e.Keyboard.IsKeyPressed(Keys.D1) == true) { return Flasks[0]; }
            if (Flasks[1] != null & e.Keyboard.IsKeyPressed(Keys.D2) == true) { return Flasks[1]; }
            if (Flasks[2] != null & e.Keyboard.IsKeyPressed(Keys.D3) == true) { return Flasks[2]; }
            if (Flasks[3] != null & e.Keyboard.IsKeyPressed(Keys.D4) == true) { return Flasks[3]; }
            if (Flasks[4] != null & e.Keyboard.IsKeyPressed(Keys.D5) == true) { return Flasks[4]; }
            return null;
        }

        public void TakeDamage(Monster sender, float Physical, float Fire, float Cold, float Lightning, float Chaos, float Crit_Chance = 0, float Crit_Multiplier = 1.5f) {
            float final;
            if (GameSetting.ProcChance(Crit_Chance) == false) { Crit_Multiplier = 1; }
            final = (float)(
                Physical * (1 - 0.06 * Stats.Armor * (1 + Stats.Increased_Armor) / (1 + 0.06 * Stats.Armor * (1 + Stats.Increased_Armor))) +
                Cold * (1 - Stats.Cold_Resistance) +
                Fire * (1 - Stats.Fire_Resistance) +
                Lightning * (1 - Stats.Lightning_Resistance)) * Crit_Multiplier;

            for (int i = 0; i < Buffs.Items.Length; i++) { final = Buffs.Items[i].TakeDamage(final); }

            if (final < Energy_Shield) {
                Energy_Shield -= final;
            } else{
                final -= Energy_Shield;
                Energy_Shield = 0;
                Life = Life - final;
            }

            NonTakeDamage = 0;
            if (Life < 0) { Life = 0; e.State = GameState.Gameover; }
        }
    }
    public partial class Player {
        //Statistical
        public int KilledMonster;
        public float AttackedHP;
        public void OnHitMonster(Monster sender, float DealHp) {
            RegenLife(Stats.Life_on_Hit);
            RegenMana(Stats.Mana_on_Hit);
            AttackedHP += DealHp;
        }
        public void OnKilledMonster(Monster sender) {
            RegenLife(Stats.Life_on_Kill);
            RegenMana(Stats.Mana_on_Kill);
            KilledMonster++;
            for (int i = 0; i <= 4; i++) {
                if (Flasks[i] != null) { 
                    Flasks[i].Data.Charges++;
                    if (Flasks[i].Data.Charges > Flasks[i].Data.Capacity) { Flasks[i].Data.Charges = Flasks[i].Data.Capacity; }
                }
            }
            
            Gold += (int)sender.Stats.Gold_Bounty;
        }

        public void ResetStatistical() {
            KilledMonster = 0;            
        }
    }



    public class PlayerSkill {
        public Type SkillType;
        public Texture2D Icon;
        public float Cooldown;


        MethodInfo MethodInfo_Cast;
        MethodInfo MethodInfo_GetRequiredMana;
        public PlayerSkill(string skillName) {
            SkillType = Type.GetType("Path_of_Defender." + skillName);

            Icon = Images.Skill.Icon.GetImage(skillName.Replace("_", " "));

            MethodInfo_Cast = SkillType.GetMethod("Cast");
            MethodInfo_GetRequiredMana = SkillType.GetMethod("GetRequiredMana");
        }
        public void Cast() {
            MethodInfo_Cast.Invoke(null, null);
        }
        public float GetRequiredMana() {
            return (float)MethodInfo_GetRequiredMana.Invoke(null, null);
        }
    }
    public class PlayerSkillColection {
        public PlayerSkill[] Items = { };
        public int Count { get { return Items.Length; } }
        public void Add(PlayerSkill item) {
            Array.Resize(ref Items, Items.Length + 1);
            Items[Items.Length - 1] = item;
        }
        public void RemoveAt(int index) {
            Array.Copy(Items, index + 1, Items, index, Items.Length - index - 1);
            Array.Resize(ref Items, Items.Length - 1);
        }
        public void Remove(PlayerSkill skill) {
            int index = Array.IndexOf(Items, skill);
            if (index != -1) RemoveAt(index);
        }
        public void Clear() { Array.Resize(ref  Items, 0); }
    }



    public static class MyExtensions { 
        public static string[] AddSkill(this string[] e, string item){
            int i = Array.IndexOf(e, item);
            Array.Resize(ref e, e.Length + 1);
            e[e.Length - 1] = item;
            return e;
        }
    }
}
