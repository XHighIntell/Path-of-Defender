using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Graphics;

namespace Path_of_Defender {
    public partial class Split_Arrow : Skill {
        public Split_Arrow() : base(Images.Skill.Icon.GetImage("Split Arrow"), AbilityType.Attack) { }
        public override void DrawInfo() {
            string[] Properties = new string[0];

            Extensions.Add(ref Properties, o.Split_Arrow_Additional_Projectiles + " additional Arrows");
            Extensions.Add(ref Properties, "Deals " + (int)(o.Split_Arrow_Multiplier_Weapon_Damage * 100) + "% of Base Damage");
            if (o.Split_Arrow_Multiplier_Damage < 1) {
                Extensions.Add(ref Properties, Math.Round((1 - o.Split_Arrow_Multiplier_Damage) * 100).ToString() + "% less Damage");
            }
            else if (o.Split_Arrow_Multiplier_Damage > 1) {
                Extensions.Add(ref Properties, Math.Round((o.Split_Arrow_Multiplier_Damage - 1) * 100).ToString() + "% more Damage");
            }

            Extensions.Add(ref Properties,
                Math.Round(o.Physical_Damage_Attack[0] * o.Split_Arrow_Multiplier_Weapon_Damage * o.Split_Arrow_Multiplier_Damage * (1 + o.Increased_Physical_Damage)) + "-" +
                Math.Round(o.Physical_Damage_Attack[1] * o.Split_Arrow_Multiplier_Weapon_Damage * o.Split_Arrow_Multiplier_Damage * (1 + o.Increased_Physical_Damage)) + " Physical DAMAGE");

            DrawInfo(Icon, "Split Arrow", new string[2] { "Attack Time", "Mana Cost" },
                new string[2] { Math.Round(GetCastTime(), 3).ToString() + "s", Math.Round(GetManaRequired()).ToString() },
                "Fires multiple arrows at different targets.", Properties);
        }
    }


    public partial class Split_Arrow : Skill  {
        public static float Min_Alpha = (float)Math.PI / 20;
        public static float Max_Alpha = (float)Math.PI / 4;

        public override float GetCastTime() {
            if (e.Player.Weapon == null) { return float.NaN; }
            
            return 1 / e.Player.Weapon.Data.Attacks_per_Second / (1 + o.Increased_Attack_Speed + o.Split_Arrow_Increased_Attack_Speed); 
        
        }
        public override float GetManaRequired() { return o.Split_Arrow_Mana_Cost * (1 - o.Reduced_Mana_Cost); }
        public override void Cast() {
            int Additional_Projectiles = e.Player.Stats.Split_Arrow_Additional_Projectiles + e.Player.Stats.Projectiles;
            float Delta_Alpha = (Max_Alpha - (Max_Alpha - Min_Alpha) * e.X / 1200) / (Additional_Projectiles + 1);
            float Center_Alpha = Basic_Arrow.AlphaFromPoint(e.X, e.Y);

            Split_Arrow_Arrow Center_Arrow = Split_Arrow_Arrow.Create(new Vector2(e.X, e.Y));
            Split_Arrow_Arrow Arrow;

            int Added = 0;
            for (int i = 1; i <= Additional_Projectiles; i++) {
                Arrow = new Split_Arrow_Arrow(); Arrow.Alpha = -i * Delta_Alpha + Center_Alpha; e.All.Add(Arrow);
                Added += 1;
                if (Added >= Additional_Projectiles) { break; }
                Arrow = new Split_Arrow_Arrow(); Arrow.Alpha = i * Delta_Alpha + Center_Alpha; e.All.Add(Arrow);
                Added += 1;
                if (Added >= Additional_Projectiles) { break; }
            }
            base.Cast();
        }
    }

    public class Split_Arrow_Arrow : SkillObject { 
        public static float Base_Speed = 30;
        public static Vector2 Base_Start = new Vector2(-10, 300);
        public float Time;
        public float Alpha;
        public Split_Arrow_Arrow() : base(AbilityType.Attack) {
            PlayerStats o = e.Player.Stats;
            Position = Base_Start;
            
            Initialize();
            Physical *= o.Split_Arrow_Multiplier_Weapon_Damage * o.Split_Arrow_Multiplier_Damage;
            Fire *= o.Split_Arrow_Multiplier_Weapon_Damage * o.Split_Arrow_Multiplier_Damage;
            Cold *= o.Split_Arrow_Multiplier_Weapon_Damage * o.Split_Arrow_Multiplier_Damage;
            Lightning *= o.Split_Arrow_Multiplier_Weapon_Damage * o.Split_Arrow_Multiplier_Damage;
        }

        public override void Update() {
            if (Deletable == false) {
                Position.X += (float)Math.Cos(Alpha) * Base_Speed;
                Position.Y += (float)Math.Sin(Alpha) * Base_Speed;
                Monster AttackedMonster = e.All.Monsters.GetMonsterInPoint(Position.X, Position.Y, MonsterState.Alive);
                if (AttackedMonster != null) {
                    AttackedMonster.TakeDamage(this);
                    Deletable = true;
                };
                if (Position.X > 1500 || Position.Y > 1600 || Position.Y < -600) {
                    Deletable = true;
                }
            }
        }
        public override void DrawOnTop() {
            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.Additive);

            e.SpriteBatch.Draw(Images.Skill.Arrow.Trail, new Vector2((float)(Position.X - 40 * Math.Cos(Alpha)), (float)(Position.Y - 40 * Math.Sin(Alpha))),
                    Images.Skill.Arrow.TrailRect, Color.White, Alpha, Images.Skill.Arrow.TrailOrigin, new Vector2(2, 0.1f), SpriteEffects.None, 0);
            e.SpriteBatch.Draw(Images.Skill.Arrow.Image, Position, Images.Skill.Arrow.Rect, Color.White, Alpha, Images.Skill.Arrow.Origin, 1, SpriteEffects.None, 0);
            e.SpriteBatch.End();
        }

        public static Split_Arrow_Arrow Create(Vector2 Position) {
            Split_Arrow_Arrow NEW = new Split_Arrow_Arrow();
            NEW.Alpha = (float)Math.Atan((Position.Y - Base_Start.Y) / (Position.X - Base_Start.X));
            e.All.AddSort(NEW);
            return NEW;
        }    
    }

}
