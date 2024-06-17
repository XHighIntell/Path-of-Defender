using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Monster_Editor {
    public struct MonsterStats {
        public float Life_Maximum, Life_Regeneration, Life_Regeneration_Percent;
        public float Mana_Maximum, Mana_Regeneration, Mana_Regeneration_Percent;
        public float[] Attack_Physical_Damage, Attack_Fire_Damage, Attack_Cold_Damage, Attack_Lightning_Damage, Attack_Chaos_Damage;
        public string[] Abilities;
        public float Armor, Fire_Resistance, Cold_Resistance, Lightning_Resistance, Chaos_Resistance;
        public float Block_Damage, Block_Chance;
        public float Critical_Strike_Chance, Critical_Strike_Multiplier;
        public float Movement_Speed_Walking, Movement_Speed_Running;
        public float Attack_Cooldown_Time, Attack_Range;
        //Bounty
        public float Gold_Bounty, XP_Bounty;

        public static MonsterStats Zero;
        static MonsterStats() {
            Array.Resize(ref Zero.Attack_Physical_Damage, 2);
            Array.Resize(ref Zero.Attack_Fire_Damage, 2);
            Array.Resize(ref Zero.Attack_Cold_Damage, 2);
            Array.Resize(ref Zero.Attack_Lightning_Damage, 2);
            Array.Resize(ref Zero.Attack_Chaos_Damage, 2);

            Zero.Life_Maximum = 10;
            Zero.Mana_Maximum = 10;
            Zero.Attack_Physical_Damage[0] = 1;
            Zero.Attack_Physical_Damage[1] = 1;
            Zero.Abilities = new string[0];
            //Zero.Abilities = new string[0];
            Zero.Armor = 1;
            Zero.Movement_Speed_Walking = 1.2f;
            Zero.Movement_Speed_Running = 1.8f;
            Zero.Attack_Cooldown_Time = 1;
            Zero.Attack_Range = 200;
        }
    }
    public enum MonsterAbility {  None = 0, EnergySheild = 1, ManaShield = 2  }

    public enum MoveState { Walking, Running }

    
}
