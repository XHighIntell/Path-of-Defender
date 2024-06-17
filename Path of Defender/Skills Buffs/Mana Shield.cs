using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Path_of_Defender {
    public partial class Mana_Shield : Skill {
        public Mana_Shield() : base(Images.Skill.Icon.GetImage("Mana Shield"), AbilityType.Toggle) { }
        public override void DrawInfo() {
            string[] Properties = new string[0];

            Extensions.Add(ref Properties, "Absorbs " + Math.Round(o.Mana_Shield_Absorbs_Percent * 100) + "% of the incoming damage in exchange for mana");
            Extensions.Add(ref Properties, o.Mana_Shield_Damage_Absorbed_Per_Mana + " Damage Absorbed per Mana");

            DrawInfo(Icon, "Mana Shield", new string[] { "Cooldown" }, new string[] { "1s" }, 
                "Creates a shield that absorbs of the incoming damage in\r\nexchange for mana.", Properties);
        }
    }
    public partial class Mana_Shield : Skill {
        public override float GetCooldown() { return 1f; }
        public override void Cast() {
            if (Cooldown > 0) { return; }
            if (Active == true) { 
                int Index = e.Player.Buffs.IndexOf(typeof(ManaShieldBuff));
                if (Index != -1) { e.Player.Buffs.Items[Index].Deletable = true; }
            }
            else { ManaShieldBuff.Cast(); }
            Active = !Active;
            base.Cast();
        }
    }

    public class ManaShieldBuff : Buff {
        public ManaShieldBuff() : base(Images.Skill.Buff.GetImage("cannotbeignited")) { }
        
        public override void Draw(int Index) { Draw(Index, Icon, BuffType.Buff); }
        public override float TakeDamage(float Final) {
            float Need_Mana = Final * o.Mana_Shield_Absorbs_Percent / o.Mana_Shield_Damage_Absorbed_Per_Mana;

            if (e.Player.Mana > Need_Mana) {
                e.Player.Mana -= Need_Mana; Final = Final * (1 - o.Mana_Shield_Absorbs_Percent);
            } else {
                Final = Final - e.Player.Mana * o.Mana_Shield_Damage_Absorbed_Per_Mana / o.Mana_Shield_Absorbs_Percent;
                e.Player.Mana = 0;
            }

            return Final;
        }

        public static void Cast() {
            PlayerStats o = e.Player.Stats;
            int Index = e.Player.Buffs.IndexOf(typeof(ManaShieldBuff));
            if (Index == -1) {
                ManaShieldBuff NEW = new ManaShieldBuff();
                e.Player.Buffs.Add(NEW);
            }
        }

    }
}
