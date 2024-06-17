using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Graphics;

namespace Path_of_Defender {
    using arc = Images.Skill.Arc;
    public partial class Arc : Skill {
        public Arc() : base(Images.Skill.Icon.GetImage("Arc"), AbilityType.Spell) { }
        public override void DrawInfo() {
            string[] Properties = new string[0];

            Extensions.Add(ref Properties, "CHAIN " + o.Arc_Chain + " TIMES");
            if (o.Arc_Chance_Shock != 0) {
                Extensions.Add(ref Properties, (int)(o.Arc_Chance_Shock * 100) + "% chance to Shock enemies");
            }
            Extensions.Add(ref Properties, Math.Round(o.Arc_Lightning_Damage[0] * (1 + o.Increased_Lightning_Damage)) + "-" + Math.Round(o.Arc_Lightning_Damage[1] * (1 + o.Increased_Lightning_Damage)) + " Lightning DAMAGE");

            DrawInfo(Icon, "Arc", new string[2] { "Cast Time", "Mana Cost" },
                new string[2] { Math.Round(GetCastTime(), 3).ToString() + "s", Math.Round(GetManaRequired()).ToString() },
                "An arc of lightning stretches from the caster to a targeted\r\nnearby enemy and chains on to additional targets.", Properties);
        }
    }
    public partial class Arc : Skill {
        public static float Base_Cast_Time = 0.8f;

        public override float GetCastTime() { return Base_Cast_Time / (1 + o.Increased_Cast_Speed + o.Arc_Increased_Cast_Speed); }
        public override float GetManaRequired() { return o.Arc_Mana_Cost * (1 - o.Reduced_Mana_Cost + o.Arc_Increased_Mana_Cost); }
        public override void Cast() { ArcObject.Create(e.Mouse_PositionF); base.Cast(); }
    }


    public partial class ArcObject : SkillObject {
        public static float Base_Max_Find_Radius = 300;
        public static float Base_Time_Each_Target = 0.1f;

        public int Max_Chain, Chain;
        public float Time;

        public ArcObject() :base(AbilityType.Spell) {
            PlayerStats o = e.Player.Stats;

            Initialize(
            Lightning1: o.Arc_Lightning_Damage[0], Lightning2: o.Arc_Lightning_Damage[1],
            Increased_Shock_Duration: o.Arc_Increased_Shock_Duration,
            Critical_Chance: o.Arc_Critical_Strike_Chance, 
            Chance_Shock: o.Arc_Chance_Shock);
            
            Max_Chain = (int)o.Arc_Chain;
        }

        public Monster[] AttackedMonsters = { };
        public override void Update() {    
            Time += GameSetting.SecondPerFrame;
            if (Chain < Max_Chain && Time >= (Chain + 1) * Base_Time_Each_Target) {
                Monster[] monsters = e.All.Monsters.FindClosestMonstersInCircle(Position, Base_Max_Find_Radius, MonsterState.Alive);
                for (int i = 0; i < monsters.Length; i++) {
                    if (Array.IndexOf(AttackedMonsters, monsters[i]) == -1) {
                        if (Chain == 0) { monsters[i].TakeDamage(this, -10, 300); }
                        else { monsters[i].TakeDamage(this, Position.X, Position.Y); }
                        
                        Extensions.AddMonsterToArray(ref AttackedMonsters, monsters[i]);
                        Position = monsters[i].Position;
                        if (Chain == 0) { 
                            e.All.Add(new ArcGraphics(null, AttackedMonsters[0]));
                               e.All.Add(new Graphics.Spark(AttackedMonsters[0]));
                        }
                        else { 
                            e.All.Add(new ArcGraphics(AttackedMonsters[Chain - 1], AttackedMonsters[Chain]));
                            e.All.Add(new Graphics.Spark(AttackedMonsters[Chain]));
                        }
                        Chain++; if (Chain >= Max_Chain) { Deletable = true; }
                        return;
                    }

                }
                Deletable = true; return;
            } 
        }
        public static void Create(Vector2 position) {
            ArcObject o = new ArcObject();  o.Position = position;  e.All.AddSort(o); 
        }
    }
    public class ArcGraphics : GameObject {
        public static float Base_Fade_At = 0.4f;
        public static float Base_Fade_Time = 0.1f;

        public float Time;
        public int Time_Ticks;

        Monster MonsterA, MonsterB;
        int Rnd_1, Rnd_2; SpriteEffects Rnd_SpriteEffects;

        public ArcGraphics(Monster monsterA, Monster monsterB) {
            MonsterA = monsterA; MonsterB = monsterB;
            Rnd_SpriteEffects = (SpriteEffects)GameSetting.RND.Next(4);
        }

        public override void Update() {
            Time += GameSetting.SecondPerFrame; Time_Ticks++;

            if (Time_Ticks % 6 == 0) { Rnd_1 = GameSetting.RND.Next(8); Rnd_2 = GameSetting.RND.Next(2); }
            if (Time_Ticks % 15 == 0) { Rnd_SpriteEffects = (SpriteEffects)GameSetting.RND.Next(4); }

            if (Time >= 0.5f) { Deletable = true; }
        }
        public override void DrawOnTop() {
            Vector2 PositionA, PositionB;
            float Rotation, Distance, Opacity = 0;

            if (MonsterA == null) { PositionA = new Vector2(-10, 300); }
            else { PositionA = MonsterA.Position; }
            PositionB = MonsterB.Position;
            Distance = (float)PositionA.DistanceTo(PositionB);
            Rotation = PositionB.GetPointRotate(PositionA);

            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.Additive, e.GraphicsDevice.SamplerStates.LinearWrap);
                
   
            int Width_Image;

            if (Distance <= 50) { Width_Image = 50; }
            else if (Distance <= 290) { Width_Image = 290; }
            else if (Distance <= 400) { Width_Image = 400; }
            else if (Distance <= 510) { Width_Image = 510; }
            else if (Distance <= 577) { Width_Image = 577; }
            else if (Distance <= 690) { Width_Image = 690; }
            else if (Distance <= 1020) { Width_Image = 1020; }
            else if (Distance <= 1050) { Width_Image = 1050; }
            else if (Distance <= 1540) { Width_Image = 1540; }
            else if (Distance <= 1765) { Width_Image = 1765; }
            else { Width_Image = (int)Distance; }

                        

            if (Time < 0.3f) { Opacity = 1; } 
            else if (Time >= 0.3f && Time <= 0.5f) { Opacity = (1 - (Time - 0.3f) / 0.2f); }

            e.SpriteBatch.Draw(arc.Image_Lightning_Long, PositionA, new Rectangle(0, 0, Width_Image, 128),
                Color.White * Opacity, Rotation, new Vector2(0, 64), Distance / Width_Image, Rnd_SpriteEffects, 0);

            e.SpriteBatch.Draw(arc.Image_Swirly_Alpha, PositionA, new Rectangle((int)(-2000 * Time), 0, (int)Distance, 512), Color.White * Opacity, Rotation, arc.Origin_Swirly_Alpha, new Vector2(1, 0.2f), SpriteEffects.None, 0);
            e.SpriteBatch.Draw(arc.Image_Projectile, PositionB, arc.Rects_Projectile[Rnd_1], Color.White * Opacity, Rotation + 1.57f, arc.Origin_Projectile, 0.5f, SpriteEffects.None, 0);
                


            e.SpriteBatch.End();
        }

    }
}

