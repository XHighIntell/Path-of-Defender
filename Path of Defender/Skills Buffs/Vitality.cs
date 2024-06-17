using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Graphics;

namespace Path_of_Defender {
    public partial class Vitality : Skill {
        public Vitality() : base(Images.Skill.Icon.GetImage("Aura Regen"), AbilityType.Spell) { }
        public override void DrawInfo() {
            string[] Properties = new string[0];

            Extensions.Add(ref Properties, "You regenerate " + Math.Round(o.Vitality_Life_Regeneration_Percent * 100, 1).ToString() + "% Life per second");
            Extensions.Add(ref Properties, "You regenerate " + Math.Round(o.Vitality_Life_Regeneration).ToString() + " Life per second");
            Extensions.Add(ref Properties, Math.Round(GetDuration(), 1).ToString() + " Seconds Duration");
            

            DrawInfo(Icon, "Vitality", new string[2] { "Cast Time", "Mana Cost" },
                new string[2] { Math.Round(GetCastTime(), 3).ToString() + "s", Math.Round(GetManaRequired()).ToString() },
                "Casts an aura that grants life regeneration to you.", Properties);
        }
    }
    public partial class Vitality : Skill {
        public static float Vitality_Cast_Time = 0.8f;

        public override float GetCastTime() { return Vitality_Cast_Time / (1 + o.Increased_Cast_Speed); }
        public override float GetManaRequired() { return o.Vitality_Mana_Cost * (1 - o.Reduced_Mana_Cost); }
        public float GetDuration() { return o.Vitality_Duration * (1 + o.Vitality_Increased_Duration + o.Increased_Duration + o.Increased_Buff_Duration); }
        public override void Cast() { VitalityBuff.Cast(); base.Cast(); }
    }

    public class VitalityBuff : Buff {
        public float Life_Regeneration, Life_Regeneration_Percent;

        public VitalityBuff() : base(Images.Skill.Icon.GetImage("Aura Regen")) { }
        
        public override void Update() {
            Duration -= GameSetting.SecondPerFrame;
            if (Duration <= 0 && Deletable == false) { 
                o.Life_Regeneration -= Life_Regeneration; 
                o.Life_Regeneration_Percent -= Life_Regeneration_Percent;
                Deletable = true; 
            }
        }
        public override void Draw(int Index) { Draw(Index, Icon, BuffType.Buff, Math.Round(Duration).ToString() + "s"); }
        public static void Cast() {
            PlayerStats o = e.Player.Stats;
            int Index = e.Player.Buffs.IndexOf(typeof(VitalityBuff));

            if (Index == -1) {
                VitalityBuff NEW = new VitalityBuff();
                NEW.Duration = o.Vitality_Duration * (1 + o.Vitality_Increased_Duration + o.Increased_Duration + o.Increased_Buff_Duration);
                NEW.Life_Regeneration = o.Vitality_Life_Regeneration;
                NEW.Life_Regeneration_Percent = o.Vitality_Life_Regeneration_Percent;

                e.Player.Stats.Life_Regeneration += NEW.Life_Regeneration;
                e.Player.Stats.Life_Regeneration_Percent += NEW.Life_Regeneration_Percent;
                e.Player.Buffs.Add(NEW);
            }
            else { e.Player.Buffs.Items[Index].Duration = o.Vitality_Duration * (1 + o.Vitality_Increased_Duration + o.Increased_Duration); }

        }

    }
    
}
