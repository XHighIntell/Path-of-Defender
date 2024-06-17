using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Graphics;

namespace Path_of_Defender {
    public partial class FrostNova : Skill {
        public FrostNova() : base(Images.Skill.Icon.GetImage("Frost Nova"), AbilityType.Spell) { }
        public override void DrawInfo() {
            float Value;
            PlayerStats o = e.Player.Stats; string[] Properties = new string[0];
            
            Value = (o.FrostNova_Chance_Freeze + o.Chance_Freeze) *100;
            if (Value != 0) { Extensions.Add(ref Properties, Math.Round(Value).ToString() + "% CHANGE TO FREEZE ENEMIES"); }
            Value = (o.FrostNova_Increased_Radius + o.Increased_AOE) * 100;
            if (Value != 0) { Extensions.Add(ref Properties, Math.Round(Value).ToString() + "% INCREASED AREA OF EFFECT RADIUS"); }

            Extensions.Add(ref Properties, Math.Round(o.FrostNova_Cold_Damage[0]).ToString() + "-" + Math.Round(o.FrostNova_Cold_Damage[1]).ToString() + " COLD DAMAGE");

            
            DrawInfo(Icon, "Frost Nova", new string[2] { "Cast Time", "Mana Cost" },
                new string[2] { Math.Round(Base_Cast_Time / (1 + o.Increased_Cast_Speed), 3).ToString(), 
                            Math.Round(GetManaRequired()).ToString() },
                    "A chilling circle of ice expands from the caster.", Properties);
        }
    }
    public partial class FrostNova : Skill {
        public static float Base_Radius = 200;
        public static float Base_Mana_Cost = 8, Base_Cast_Time = 0.935f;
        
        public override float GetCastTime() { return Base_Cast_Time / (1 + e.Player.Stats.Increased_Cast_Speed); }
        public override float GetManaRequired() { return (e.Player.Stats.FrostNova_Mana_Cost) * (1 - e.Player.Stats.Reduced_Mana_Cost); }
        public override void Cast() {
            FrostNovaObject.Create(e.Mouse_PositionF);
            base.Cast();
            
        }
    }

    public class FrostNovaObject : SkillObject {
        public struct Spike {
            public float Speed;
            public Vector2 Pos, Scale;
            public float Time;
            public float Alpha;
            public Spike(float x, float y, float alpha, float speed) {
                Pos = new Vector2(x, y);
                Scale = (new Vector2(0.6f, 1)) * 0.4f;
                Alpha = alpha; Time = 0;
                Speed = speed;
            }


            public void Draw(Color color) {
                e.SpriteBatch.Draw(Images.Skill.FrostNova.ImageNovaSpike, Pos, Images.Skill.FrostNova.RectNovaSpike,
                    color, Alpha, Images.Skill.FrostNova.OriginNovaSpike, Scale * Time / Base_Explosive_Time, SpriteEffects.None, 0);
            }
            public void Update() {
                Pos.X += (float)Math.Sin(Alpha) * Speed * Base_3D.X;
                Pos.Y -= (float)Math.Cos(Alpha) * Speed * Base_3D.Y;
                Time += GameSetting.SecondPerFrame;
            }
        }
        public static Vector2 Base_3D = new Vector2(1, 0.6f);
        public static float Base_Explosive_Time = 1.2f; // = include fade time
        public static float Base_Ice_Fade_At = 0.8f;
        public static float Base_Ice_Fade_Duration = 0.4f;
        public static float Base_Spikes_Fade_At = 0.7f;
        public static float Base_Spikes_Fade_Duration = 0.4f;

        public float Radius;
        public float Time;
        public Spike[] Spikes = { };
        public int Add_Count, Tick_Count;

        private void AddSpike(Spike item) {
            Array.Resize(ref Spikes, Spikes.Length + 1);
            Spikes[Spikes.Length - 1] = item;
        }
        
        public FrostNovaObject() {
            PlayerStats o = e.Player.Stats;
            //Physical = GameSetting.RND.NextFloat(o.FrostNova_Physical_Damage[0], o.FrostNova_Physical_Damage[1]);
            //Fire = GameSetting.RND.NextFloat(o.FrostNova_Fire_Damage[0], o.FrostNova_Fire_Damage[1]);
            Cold = GameSetting.RND.NextFloat(o.FrostNova_Cold_Damage[0], o.FrostNova_Cold_Damage[1]);
            //Lightning = GameSetting.RND.NextFloat(o.FrostNova_Lightning_Damage[0], o.FrostNova_Lightning_Damage[1]);
            //Chaos = GameSetting.RND.NextFloat(o.FrostNova_Chaos_Damage[0], o.FrostNova_Chaos_Damage[1]);

            Critical_Strike_Chance = 0.05f * (1 + o.Increased_Critical_Strike_Chance);
            IsCrit = GameSetting.ProcChance(Critical_Strike_Chance);
            if (IsCrit == true) {
                Increased_Critical_Strike_Multiplier = o.Increased_Critical_Strike_Multiplier;
                Cold *= 1.5f * (1 + Increased_Critical_Strike_Multiplier);
            }
            
            //Causes_Bleeding, Increased_Bleeding_Duration;
            //Chance_Ignite, Increased_Ignite_Duration;
            Chance_Freeze = o.Chance_Freeze + o.FrostNova_Chance_Freeze;
            Increased_Freeze_Duration = o.Increased_Freeze_Duration + o.FrostNova_Increased_Freeze_Duration;
            Increased_Chill_Duration = o.Increased_Chill_Duration + o.FrostNova_Increased_Chill_Duration;
            //Chance_Shock, Increased_Shock_Duration;

            Radius = FrostNova.Base_Radius * (1 + o.Increased_AOE + o.FrostNova_Increased_Radius);
            
        }
        public override void Draw() {
            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.Additive);
            if (Time < Base_Ice_Fade_At) {
                e.SpriteBatch.Draw(Images.Skill.FrostNova.ImageNova, Position, Images.Skill.FrostNova.RectNova, Color.White, 0, Images.Skill.FrostNova.OriginNova,
                Base_3D * Radius / 256 * Time / Base_Explosive_Time  /*<Important*/ , SpriteEffects.None, 0);
 
            } else if (Time < Base_Ice_Fade_At + Base_Ice_Fade_Duration ) {
                e.SpriteBatch.Draw(Images.Skill.FrostNova.ImageNova, Position, Images.Skill.FrostNova.RectNova,
                Color.White * (1 - (Time - Base_Ice_Fade_At) / Base_Ice_Fade_Duration), 0, Images.Skill.FrostNova.OriginNova,
                Base_3D * Radius / 256 * Time / Base_Explosive_Time, SpriteEffects.None, 0);

                e.SpriteBatch.Draw(Images.Skill.FrostNova.ImageNovaHalfOpacity, Position, Images.Skill.FrostNova.RectNovaHalfOpacity,
                Color.White * (1 - (Time - Base_Ice_Fade_At) / Base_Ice_Fade_Duration), 0, Images.Skill.FrostNova.OriginNovaHalfOpacity,
                Base_3D * Radius / 512 * Time / Base_Explosive_Time, SpriteEffects.None, 0);
            }

            if (Time < Base_Spikes_Fade_At) {
                for (int i = 0; i < Spikes.Length; i++) { Spikes[i].Draw(Color.White); }
            } else if(Time < Base_Spikes_Fade_At + Base_Spikes_Fade_Duration) {
                for (int i = 0; i < Spikes.Length; i++) {
                    Spikes[i].Draw(Color.White * (1 - (Time - Base_Spikes_Fade_At) / Base_Spikes_Fade_Duration));
                }
            }
            e.SpriteBatch.End();
        }
        public override void Update() {
            Time += GameSetting.SecondPerFrame;

            if (Time < Base_Spikes_Fade_At && Time < Base_Spikes_Fade_At + Base_Spikes_Fade_Duration) {
                for (int i = 0; i < Spikes.Length; i++) {
                    if (Spikes[i].Alpha < Math.PI) { Spikes[i].Alpha += 0.01f; } 
                    else { Spikes[i].Alpha -= 0.01f; }
                }
            } else if (Time > Base_Explosive_Time && Time > Base_Spikes_Fade_At + Base_Spikes_Fade_Duration) {
                Deletable = true;
                return;
            } 
            Tick_Count += 1;
            if (Tick_Count == 10) {
                Tick_Count = 0;
                Add_Count += 1;
                if (Time < Base_Explosive_Time) HitMonster();
                
            }

            if (Add_Count <= 2) {
                for (int i = 0; i < 10; i++) { AddSpike(new Spike(Position.X, Position.Y, GameSetting.RND.NextFloat(0, (float)Math.PI * 2), Radius / Base_Explosive_Time / 75)); }
            }
            for (int i = 0; i < Spikes.Length; i++) { Spikes[i].Update(); }
            
        }

        public MonsterCollection AttackedMonsters = new MonsterCollection();
        private void HitMonster() {
            Monster[] monsters = e.All.Monsters.GetMonstersInEllipse(Position.X, Position.Y, Radius * Time / Base_Explosive_Time, Radius * 0.6f * Time / Base_Explosive_Time, MonsterState.Alive);
            for (int i = 0; i < monsters.Length; i++) {
                if (Array.IndexOf(AttackedMonsters.Items, monsters[i]) == -1) {
                    AttackedMonsters.Add(monsters[i]);
                    monsters[i].TakeDamage(this, Position.X, Position.Y);
                }
            }
        }
        public static void Create(Vector2 position) {
            FrostNovaObject NEW = new FrostNovaObject();
            NEW.Position = position;
            e.All.AddSort(NEW); 
        }

    }
}
