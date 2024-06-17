using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Graphics;

namespace Path_of_Defender {
    using FS = Images.Skill.FireStorm;
    public partial class Firestorm : Skill {
        public Firestorm() : base(Images.Skill.Icon.GetImage("Firestorm"), AbilityType.Spell) { }
        public override void DrawInfo() {
 	        PlayerStats o = e.Player.Stats; string[] Properties = new string[0];

            Extensions.Add(ref Properties, "Base duration is " + Math.Round(GetDuration(), 2) + " seconds");
            Extensions.Add(ref Properties, "One fireball falls every " + o.Firestorm_Interval + " seconds");

            if (o.Firestorm_Physical_Damage[1] != 0) {
                Extensions.Add(ref Properties, Math.Round(o.Firestorm_Physical_Damage[0] * (1 + o.Increased_Physical_Damage)) + "-" +
                    Math.Round(o.Firestorm_Physical_Damage[1] * (1 + o.Increased_Physical_Damage)) + " PHYSICAL DAMAGE");
            }

            if (o.Firestorm_Fire_Damage[1] != 0) {
                Extensions.Add(ref Properties, Math.Round(o.Firestorm_Fire_Damage[0] * (1 + o.Increased_Fire_Damage)) + "-" +
                        Math.Round(o.Firestorm_Fire_Damage[1] * (1 + o.Increased_Fire_Damage)) + " Fire DAMAGE");
            }
            
            if (o.Firestorm_Cold_Damage[1] != 0) {
                Extensions.Add(ref Properties, Math.Round(o.Firestorm_Cold_Damage[0] * (1 + o.Increased_Cold_Damage)) + "-" +
                        Math.Round(o.Firestorm_Cold_Damage[1] * (1 + o.Increased_Cold_Damage)) + " Cold DAMAGE");
            }

            if (o.Firestorm_Lightning_Damage[1] != 0) {
                Extensions.Add(ref Properties, Math.Round(o.Firestorm_Lightning_Damage[0] * (1 + o.Increased_Lightning_Damage)) + "-" +
                        Math.Round(o.Firestorm_Lightning_Damage[1] * (1 + o.Increased_Lightning_Damage)) + " Lightning DAMAGE");
            }

            if (o.Firestorm_Chaos_Damage[1] != 0) {
                Extensions.Add(ref Properties, Math.Round(o.Firestorm_Chaos_Damage[0] * (1 + o.Increased_Chaos_Damage)) + "-" +
                        Math.Round(o.Firestorm_Chaos_Damage[1] * (1 + o.Increased_Chaos_Damage)) + " Chaos DAMAGE");
            }

            DrawInfo(Icon, "Firestorm", new string[2] { "Cast Time", "Mana Cost" },
                new string[2] { Math.Round(GetCastTime(), 3).ToString() + "s",
                Math.Round(GetManaRequired()).ToString() }, 
                "Flaming bolts rain down over the targeted area.\r\nThey explode when landing, dealing damage to nearby enemies.", Properties);
        }
    }
    public partial class Firestorm : Skill {
        public static float Base_CastTime = 1.2f;

        public float GetDuration() { return  o.Firestorm_Duration * (1 + o.Increased_Duration + o.Firestorm_Increased_Duration); }
        public override float GetCastTime() { return Base_CastTime / (1 + e.Player.Stats.Increased_Cast_Speed); }
        public override float GetManaRequired() {
            return o.Firestorm_Mana_Cost * (1 - o.Reduced_Mana_Cost); }
        public override void Cast() { FirestormObject.Create(e.Mouse_PositionF); base.Cast(); }
    }


    public class FirestormObject : SkillObject  {
        public static float Base_Radius = 120;
        public float Radius, Interval, Duration;
        public float Time, TimeCallBall;

        public FirestormObject() : base(AbilityType.Spell) {
            PlayerStats o = e.Player.Stats;
            Radius = Base_Radius * (1 + o.Increased_AOE + o.Firestorm_Increased_AOE);
            Interval = o.Firestorm_Interval;
            Duration = o.Firestorm_Duration * (1 + o.Increased_Duration);

            Initialize(
            Physical1: o.Firestorm_Physical_Damage[0], Physical2: o.Firestorm_Physical_Damage[1],
            Fire1: o.Firestorm_Fire_Damage[0], Fire2: o.Firestorm_Fire_Damage[1],
            Cold1: o.Firestorm_Cold_Damage[0], Cold2: o.Firestorm_Cold_Damage[1],
            Lightning1: o.Firestorm_Lightning_Damage[0], Lightning2: o.Firestorm_Lightning_Damage[1],
            Chaos1: o.Firestorm_Chaos_Damage[0], Chaos2: o.Firestorm_Chaos_Damage[1],

            Critical_Chance: o.Firestorm_Critical_Strike_Chance,
            Chance_Ignite: o.Firestorm_Chance_Ignite,
            Increased_Ignite_Duration: o.Firestorm_Increased_Ignite_Duration
            );

            
            
        }
        public override void Update() {
            Time += InfoSystemGame.SecondPerFrame;
            TimeCallBall += InfoSystemGame.SecondPerFrame;

            if (Time <= this.Duration) {
                if (TimeCallBall >= this.Interval) {
                    TimeCallBall -= this.Interval;
                    FireStormBall.Create(Position.RandomPointEllipse(Radius, Radius * 0.75f), this);
                }
            } else if (Time > Duration) { Deletable = true; }

        }
        public static void Create(Vector2 Position){
            FirestormObject NEW = new FirestormObject();
            NEW.Position = Position;
            e.All.AddSort(NEW);
        }
    }
    

    public class FireStormBall : GameObject {
        public static float FallTime = 0.4f;
        public static float StartAt = 500;
        public static float ExplosiveFrameTime = GameSetting.SecondPerFrame * 2;
        public static float Base_Explosive_Radius = 60;
        public static Vector2 Point3D = new Vector2(600, 1600);

        public float Radius;
            
        //track
        public FirestormObject Parent;
        public float Time;
        
        public int Frame; 

        //Graphics
        public Vector2 PositionStart;
        public float Increase_AOE, Angle, Fall_Progress;

        public FireStormBall(Vector2 position, FirestormObject parent) {
            Position = position; Parent = parent;
            Angle = (float)Math.Atan((Position.X - Point3D.X) / (Point3D.Y - Position.Y));
            PositionStart.X = Position.X + FireStormBall.StartAt * (Position.X - Point3D.X) / (Point3D.Y - Position.Y);
            PositionStart.Y = Position.Y - StartAt;
            
            PlayerStats o = e.Player.Stats;
            Increase_AOE = o.Increased_AOE + o.Firestorm_Increased_AOE;
            Radius = Base_Explosive_Radius * (1 + Increase_AOE);
        }


        

        public override void Update() {
            Time += GameSetting.SecondPerFrame;
            if (FallTime + 3 * ExplosiveFrameTime < Time && Time < FallTime + 8 * ExplosiveFrameTime) {  HitMonster(); }
            else if (Time > FallTime + 8 * ExplosiveFrameTime) { Deletable = true; }
        }

        public Monster[] Attacked_Monsters = { };
        public void HitMonster() {
            Monster[] monsters = e.All.Monsters.GetMonstersInEllipse(Position.X, Position.Y, Radius, Radius * 0.75f, MonsterState.Alive);
            for (int i = 0; i < monsters.Length; i++) {
                if (Array.IndexOf(Attacked_Monsters, monsters[i]) == -1) {
                    Extensions.AddMonsterToArray(ref Attacked_Monsters, monsters[i]);
                    monsters[i].TakeDamage(Parent, Position.X, Position.Y);
                }
            }
        }

        public override void Draw() {
            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.Additive);
            if (Time <= FallTime) {
                Fall_Progress = Time / FireStormBall.FallTime;
                e.SpriteBatch.Draw(Images.Skill.FireStorm.ImageTrail, (PositionStart - Position) * (1 - Fall_Progress) + Position, Images.Skill.FireStorm.RectTrail, Color.White,
                Angle, Images.Skill.FireStorm.OriginTrail, (float)(1F - 0.6 * Fall_Progress) * (1 + Increase_AOE), SpriteEffects.None, 1); 
            } else if (Time <= FallTime + 7 * ExplosiveFrameTime) {
                Frame = (int)((Time - FallTime) / ExplosiveFrameTime);
                if (Frame < 2) {
                    e.SpriteBatch.Draw(Images.Skill.FireStorm.ImageTrail, Position,
                        Images.Skill.FireStorm.RectTrail, Color.White, Angle, Images.Skill.FireStorm.OriginTrail, 0.4f * (1 + Increase_AOE), SpriteEffects.None, 1);
                }
                e.SpriteBatch.Draw(Images.Skill.FireStorm.ImageExplode, Position, Images.Skill.FireStorm.FramesRectExplode[Frame], Color.White, Angle, Images.Skill.FireStorm.OriginExplode,
                (float)(0.4 + Position.Y / Point3D.Y) * (1 + Increase_AOE), SpriteEffects.None, 0);
            }
            e.SpriteBatch.End();
        }

        public static void Create(Vector2 Position, FirestormObject parent) {
            FireStormBall NEW = new FireStormBall(Position, parent);
            e.All.AddSort(NEW);
        }
    }
}
