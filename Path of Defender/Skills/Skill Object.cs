using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Graphics;

namespace Path_of_Defender {
    using UI = Images.UI;
    using buff = Images.Skill.Buff;

    public abstract partial class SkillObject : GameObject {
        //Đây là những giá trị damages đả bao gồm tất cả source và đả đc random.
        public float Physical, Fire, Cold, Lightning, Chaos;
        

        //đây là những giá đả bao gồm source của skill + stats player.
        public AbilityType Ability;
        public bool IsCrit;
        public float Critical_Strike_Chance, Increased_Critical_Strike_Multiplier;

        public float Chance_Knockback, Knockback_Distance, Increased_Knockback_Distance;
        public bool Knockback_on_Crit;

        //public bool Causes_Bleeding, Increased_Bleeding_Duration;
        public float Reduced_Enemy_Stun_Threshold, Increased_Stun_Duration, Recalculation_Reduced_Enemy_Stun_Threshold;
        public float Chance_Ignite, Increased_Ignite_Duration;
        public float Chance_Freeze, Increased_Freeze_Duration, Increased_Chill_Duration;
        public float Chance_Shock, Increased_Shock_Duration;
    }
    public abstract partial class SkillObject : GameObject {
        public SkillObject() { }
        public SkillObject(AbilityType Type) { Ability = Type; }
        /// <summary> Create properties from player and customed properties for Skill. </summary>
        public void Initialize(
            float Physical1 = 0, float Fire1 = 0, float Cold1 = 0, float Lightning1 = 0, float Chaos1 = 0,
            float Physical2 = 0, float Fire2 = 0, float Cold2 = 0, float Lightning2 = 0, float Chaos2 = 0,
            float Critical_Chance = 0, float Increased_Critical_Chance = 0, float Increased_Critical_Multiplier = 0,
            float Increased_Physical = 0, float Reduced_Enemy_Stun_Threshold = 0, float Increased_Stun_Duration = 0,
            float Increased_Fire = 0, float Chance_Ignite = 0, float Increased_Ignite_Duration = 0,
            float Increased_Cold = 0, float Chance_Freeze = 0, float Increased_Freeze_Duration = 0, float Increased_Chill_Duration = 0,
            float Increased_Lightning = 0, float Chance_Shock = 0, float Increased_Shock_Duration = 0,
            float Increased_Chaos = 0,

            float Chance_Knockback = 0, float Increased_Knockback_Distance = 0, bool Knockback_on_Crit = false) {
            PlayerStats o = e.Player.Stats;
            
            if (Ability == AbilityType.Spell) {
                this.Critical_Strike_Chance = Critical_Chance * (1 + o.Increased_Critical_Strike_Chance + Increased_Critical_Chance + o.Increased_Critical_Strike_Chance_For_Spells);
                this.Increased_Critical_Strike_Multiplier = o.Increased_Critical_Strike_Multiplier + Increased_Critical_Multiplier +  o.Increased_Critical_Strike_Multiplier_For_Spells;
            } else if (Ability == AbilityType.Attack) {
                this.Critical_Strike_Chance = e.Player.Weapon.Data.Critical_Strike_Chance * (1 + o.Increased_Critical_Strike_Chance + Increased_Critical_Chance);
                this.Increased_Critical_Strike_Multiplier = o.Increased_Critical_Strike_Multiplier + Increased_Critical_Multiplier;
            }



if (Ability == AbilityType.Spell) {
    this.Physical = GameSetting.RND.NextFloat(o.Physical_Damage_Spell[0] + Physical1, o.Physical_Damage_Spell[1] + Physical2) * (1 + o.Increased_Physical_Damage + Increased_Physical);
    this.Fire = GameSetting.RND.NextFloat(o.Fire_Damage_Spell[0] + Fire1, o.Fire_Damage_Spell[1] + Fire2) * (1 + o.Increased_Fire_Damage + Increased_Fire + o.Increased_Spell_Damage);
    this.Cold = GameSetting.RND.NextFloat(o.Cold_Damage_Spell[0] + Cold1, o.Cold_Damage_Spell[1] + Cold2) * (1 + o.Increased_Cold_Damage + Increased_Cold + o.Increased_Spell_Damage);
    this.Lightning = GameSetting.RND.NextFloat(o.Lightning_Damage_Spell[0] + Lightning1, o.Lightning_Damage_Spell[1] + Lightning2) * (1 + o.Increased_Lightning_Damage + Increased_Lightning + o.Increased_Spell_Damage);

    //this.Fire      = Fire      * (1 + o.Increased_Fire_Damage      + Increased_Fire      + o.Increased_Spell_Damage);
    //this.Cold      = Cold      * (1 + o.Increased_Cold_Damage      + Increased_Cold      + o.Increased_Spell_Damage);
    //this.Lightning = Lightning * (1 + o.Increased_Lightning_Damage + Increased_Lightning + o.Increased_Spell_Damage);
} else if (Ability== AbilityType.Attack) {
    this.Physical = GameSetting.RND.NextFloat(o.Physical_Damage_Attack[0] + Physical1, o.Physical_Damage_Attack[1] + Physical2) * (1 + o.Increased_Physical_Damage + Increased_Physical);
    this.Fire = GameSetting.RND.NextFloat(o.Fire_Damage_Attack[0] + Fire1, o.Fire_Damage_Attack[1] + Fire2) * (1 + o.Increased_Fire_Damage + Increased_Fire);
    this.Cold = GameSetting.RND.NextFloat(o.Cold_Damage_Attack[0] + Cold1, o.Cold_Damage_Attack[1] + Cold2) * (1 + o.Increased_Cold_Damage + Increased_Cold);
    this.Lightning = GameSetting.RND.NextFloat(o.Lightning_Damage_Attack[0] + Lightning1, o.Lightning_Damage_Attack[1] + Lightning2) * (1 + o.Increased_Lightning_Damage + Increased_Lightning);
}
            
            //this.Chaos = Chaos * (1 + o.Increased_Chaos_Damage + Increased_Chaos);

            this.Reduced_Enemy_Stun_Threshold = o.Reduced_Enemy_Stun_Threshold + Reduced_Enemy_Stun_Threshold;
            this.Increased_Stun_Duration = o.Increased_Stun_Duration + Increased_Stun_Duration;
            if (this.Reduced_Enemy_Stun_Threshold <= 0.75f) { this.Recalculation_Reduced_Enemy_Stun_Threshold = this.Reduced_Enemy_Stun_Threshold; } 
            else if (this.Reduced_Enemy_Stun_Threshold > 0.75f) { this.Recalculation_Reduced_Enemy_Stun_Threshold = 0.75f + (this.Reduced_Enemy_Stun_Threshold - 0.75f) * 0.25f / (this.Reduced_Enemy_Stun_Threshold - 0.5f); }


            this.Chance_Ignite = o.Chance_Ignite + Chance_Ignite;
            this.Increased_Ignite_Duration = o.Increased_Ignite_Duration + Increased_Ignite_Duration;

            this.Chance_Freeze = o.Chance_Freeze + Chance_Freeze;
            this.Increased_Freeze_Duration = o.Increased_Freeze_Duration + Increased_Freeze_Duration;
            this.Increased_Chill_Duration = o.Increased_Chill_Duration + Increased_Chill_Duration;

            this.Chance_Shock = o.Chance_Shock + Chance_Shock;
            this.Increased_Shock_Duration = o.Increased_Shock_Duration + Increased_Shock_Duration;
            
            if (Ability == AbilityType.Attack) {
                this.Chance_Knockback = o.Chance_Knockback + Chance_Knockback;
                this.Knockback_Distance = o.Knockback_Distance * (1 + o.Increased_Knockback_Distance + Increased_Knockback_Distance);
                this.Knockback_on_Crit = o.Knockback_on_Crit || Knockback_on_Crit;
            }

            IsCrit = GameSetting.ProcChance(Critical_Strike_Chance);
            if (IsCrit == true) {
                this.Physical  *= 1.5f * (1 + this.Increased_Critical_Strike_Multiplier + Increased_Critical_Multiplier);
                this.Fire      *= 1.5f * (1 + this.Increased_Critical_Strike_Multiplier + Increased_Critical_Multiplier);
                this.Cold      *= 1.5f * (1 + this.Increased_Critical_Strike_Multiplier + Increased_Critical_Multiplier);
                this.Lightning *= 1.5f * (1 + this.Increased_Critical_Strike_Multiplier + Increased_Critical_Multiplier);
                this.Chaos     *= 1.5f * (1 + this.Increased_Critical_Strike_Multiplier + Increased_Critical_Multiplier);
            }
        }
    }
}
