using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Graphics;

namespace Path_of_Defender {
    public partial class Barrage : Skill {
        public Barrage() : base(Images.Skill.Icon.GetImage("Barrage"), AbilityType.Attack) { }
        public override void DrawInfo() {
            string[] Properties = new string[0];            
            Extensions.Add(ref Properties, "Deals " + Math.Round(o.Barrage_Arrow_Multiplier_Weapon_Damage * 100) + "% of Base Damage");
            Extensions.Add(ref Properties, o.Barrage_Additional_Projectiles + " Additional Projectiles");
            if (o.Barrage_Arrow_Multiplier_Damage < 1) {
                Extensions.Add(ref Properties, Math.Round((1 - o.Barrage_Arrow_Multiplier_Damage) * 100).ToString() + "% less Damage");
            } else if (o.Barrage_Arrow_Multiplier_Damage > 1) {
                Extensions.Add(ref Properties, Math.Round((o.Barrage_Arrow_Multiplier_Damage - 1) * 100).ToString() + "% more Damage");
            }
            Extensions.Add(ref Properties, "Last 0.2 seconds");

            Skill.DrawInfo(Icon, "Barrage", new string[2] { "Attack Time", "Mana Cost" },
                new string[2] { Math.Round(GetCastTime(), 3).ToString() + "s", Math.Round(GetManaRequired()).ToString() },
                "After a short preparation time, you attack repeatedly.\r\nThese attacks have a small randomized spread.", Properties);
        }
    }

    public partial class Barrage : Skill {
        public override float GetCastTime() {
            if (e.Player.Weapon == null) return float.NaN;
            return 1 / e.Player.Weapon.Data.Attacks_per_Second / (1 + o.Increased_Attack_Speed); }
        public override float GetManaRequired() { return o.Barrage_Arrow_Mana_Cost * (1 - o.Reduced_Mana_Cost); }
        public override void Cast() { BarrageObject.Create(e.Mouse_PositionF); base.Cast(); }
    }


    public partial class BarrageObject : GameObject {
        public float Time;
        public int Arrows, Max_Arrows;
        public BarrageObject() {
            PlayerStats o = e.Player.Stats;
            Max_Arrows = 1 + o.Projectiles + o.Barrage_Additional_Projectiles;
        }

        public override void Update() {
            Time += GameSetting.SecondPerFrame;
            if (Time >= 0.2f / Max_Arrows) { 
                Time -= 0.2f / Max_Arrows; 
                Barrage_Arrow.Create(Position); Arrows++;
            }
            if (Arrows >= Max_Arrows) { Deletable = true; }
        }
        public static void Create(Vector2 Position) {
            BarrageObject NEW = new BarrageObject();
            NEW.Position = e.Mouse_PositionF;
            e.All.Add(NEW);
        }
    } 

    public partial class Barrage_Arrow : SkillObject  { 
        public static float Min_Alpha = (float)Math.PI / 20;
        public static float Max_Alpha = (float)Math.PI / 4;
        public static float Base_Speed = 30;
        public static Vector2 Base_Start = new Vector2(-10, 300);
        public static float Random_Radius = 35;
    }

    public partial class Barrage_Arrow :SkillObject  { 
        public float Alpha;
        public Barrage_Arrow() : base(AbilityType.Attack) {
            PlayerStats o = e.Player.Stats;
            Position = Base_Start;
            Position.Y += GameSetting.RND.NextFloat(-Random_Radius, Random_Radius);

            Initialize();
            Physical *= o.Barrage_Arrow_Multiplier_Damage * o.Barrage_Arrow_Multiplier_Weapon_Damage;
            Fire *= o.Barrage_Arrow_Multiplier_Damage * o.Barrage_Arrow_Multiplier_Weapon_Damage;
            Cold *= o.Barrage_Arrow_Multiplier_Damage * o.Barrage_Arrow_Multiplier_Weapon_Damage;
            Lightning *= o.Barrage_Arrow_Multiplier_Damage * o.Barrage_Arrow_Multiplier_Weapon_Damage;
            Chaos *= o.Barrage_Arrow_Multiplier_Damage * o.Barrage_Arrow_Multiplier_Weapon_Damage;
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
                    Images.Skill.Arrow.TrailRect, Color.Green, Alpha, Images.Skill.Arrow.TrailOrigin, new Vector2(2, 0.1f), SpriteEffects.None, 0);
            e.SpriteBatch.Draw(Images.Skill.Arrow.Image, Position, Images.Skill.Arrow.Rect, new Color(200, 255, 200, 255), Alpha, Images.Skill.Arrow.Origin, 1, SpriteEffects.None, 0);
            e.SpriteBatch.End();
        }

        public static void Create(Vector2 Position) {
            Barrage_Arrow NEW = new Barrage_Arrow();
            NEW.Alpha = (float)Math.Atan((Position.Y + GameSetting.RND.NextFloat(-Random_Radius, Random_Radius) - Base_Start.Y) / (Position.X - Base_Start.X));
            e.All.AddSort(NEW);
        }   
    }

}
