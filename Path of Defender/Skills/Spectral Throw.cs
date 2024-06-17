using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Graphics;

namespace Path_of_Defender {
    using ST = Images.Skill.SpectralThrow;
    public partial class Spectral_Throw : Skill {
        public static float Min_Alpha = (float)Math.PI / 20;
        public static float Max_Alpha = (float)Math.PI / 4;

        public Spectral_Throw() : base(Images.Skill.Icon.GetImage("Spectral Throw"), AbilityType.Attack) { }
        public override void DrawInfo() {
            PlayerStats o = e.Player.Stats; string[] Properties = new string[0];

            if (o.Spectral_Throw_Additional_Projectiles != 0) { Extensions.Add(ref Properties, o.Spectral_Throw_Additional_Projectiles + " additional Arrows"); }

            Extensions.Add(ref Properties, "Deals " + (int)(o.Spectral_Throw_Multiplier_Weapon_Damage * 100) + "% of Base Damage");

            if (o.Spectral_Throw_Multiplier_Damage < 1) {
                Extensions.Add(ref Properties, Math.Round((1 - o.Spectral_Throw_Multiplier_Damage) * 100).ToString() + "% less Damage");
            }
            else if (o.Spectral_Throw_Multiplier_Damage > 1) {
                Extensions.Add(ref Properties, Math.Round((o.Spectral_Throw_Multiplier_Damage - 1) * 100).ToString() + "% more Damage");
            }

            Extensions.Add(ref Properties, Math.Round(o.Physical_Damage_Attack[0] * o.Spectral_Throw_Multiplier_Weapon_Damage * o.Spectral_Throw_Multiplier_Damage * (1 + o.Increased_Physical_Damage)).ToString() + "-" +
                                           Math.Round(o.Physical_Damage_Attack[1] * o.Spectral_Throw_Multiplier_Weapon_Damage * o.Spectral_Throw_Multiplier_Damage * (1 + o.Increased_Physical_Damage)).ToString() + " PHYSICAL DAMAGE");

            DrawInfo(Icon, "Spectral Throw", new string[2] { "Attack Time", "Mana Cost" },
                new string[] { Math.Round(GetCastTime(), 3).ToString(), Math.Round(GetManaRequired()).ToString() },
                    "Throws a spectral copy of your melee weapon.\r\nIt flies out and then returns to you, in a spinning attack\r\nthat strikes enemies in its path.", Properties);
        }

        
        
    }
    public partial class Spectral_Throw : Skill {
        public override float GetManaRequired() { return o.Spectral_Throw_Mana_Cost * (1 - o.Reduced_Mana_Cost); }
        public override float GetCastTime() {
            if (e.Player.Weapon == null) { return float.NaN; }
            return 1 / e.Player.Weapon.Data.Attacks_per_Second / (1 + e.Player.Stats.Increased_Attack_Speed); 

        }
        public override void Cast() {
            int Additional_Projectiles = o.Spectral_Throw_Additional_Projectiles + e.Player.Stats.Projectiles;
            float Delta_Alpha = (Max_Alpha - (Max_Alpha - Min_Alpha) * e.X / 1200) / (Additional_Projectiles + 1);
            float Center_Alpha = SpectralThrow_Arrow.AlphaFromPoint(e.X, e.Y);

            SpectralThrow_Arrow.Create(Center_Alpha);

            int Added = 0;
            for (int i = 1; i <= Additional_Projectiles; i++) {
                SpectralThrow_Arrow.Create(-i * Delta_Alpha + Center_Alpha);
                Added += 1;
                if (Added >= Additional_Projectiles) { break; }
                SpectralThrow_Arrow.Create(i * Delta_Alpha + Center_Alpha);
                Added += 1;
                if (Added >= Additional_Projectiles) { break; }
            }
            e.Player.Mana -= GetManaRequired();
            e.Player.TimeAttackRemain = GetCastTime();
        }
        
    }

    public class SpectralThrow_Arrow : SkillObject { 
        public static Vector2 Base_Start = new Vector2(-10, 300);
        public static float Base_Start_Speed = 20;

        public float Time, Alpha, Speed, Rotation;
        public bool IsReturning;

        public SpectralThrow_Arrow() : base(AbilityType.Attack) {
            PlayerStats o = e.Player.Stats; Position = Base_Start;

            //DefalutValuesFromPlayer(e.Player.Weapon.Critical_Strike_Chance);
            Initialize();
            Physical *= o.Spectral_Throw_Multiplier_Weapon_Damage * o.Spectral_Throw_Multiplier_Damage;
            Fire *= o.Spectral_Throw_Multiplier_Weapon_Damage * o.Spectral_Throw_Multiplier_Damage;
            Cold *= o.Spectral_Throw_Multiplier_Weapon_Damage * o.Spectral_Throw_Multiplier_Damage;
            Lightning *= o.Spectral_Throw_Multiplier_Weapon_Damage * o.Spectral_Throw_Multiplier_Damage;
            Chaos *= o.Spectral_Throw_Multiplier_Weapon_Damage * o.Spectral_Throw_Multiplier_Damage;
            Speed = Base_Start_Speed;
        }

        public Monster[] AttackedMonsters = { };
        public override void Update() {
            Time += GameSetting.SecondPerFrame;
            if (Deletable == false) {
                Position.X += (float)Math.Cos(Alpha) * Speed;
                Position.Y += (float)Math.Sin(Alpha) * Speed;
                Speed = (float)Math.Cos(Time * 1.5f) * Base_Start_Speed;
                Rotation -= 0.3f;


                if (IsReturning == false && Speed < 0) { IsReturning = true; AttackedMonsters = new Monster[0]; }
                //Monster[] monsters = e.All.Monsters.GetMonstersInPoint(Position.X, Position.Y, MonsterState.Alive);
                Monster[] monsters = e.All.Monsters.GetMonstersInEllipse(Position.X, Position.Y, 30, 50, MonsterState.Alive);

                for (int i = 0; i < monsters.Length; i++) {
                    if (Array.IndexOf(AttackedMonsters, monsters[i]) == -1) {
                        monsters[i].TakeDamage(this, this.Position.X, this.Position.Y);
                        Extensions.AddMonsterToArray(ref AttackedMonsters, monsters[i]);                
                    }
                }

                if (Position.X < -100) { Deletable = true; }
            }
        }
        public override void DrawOnTop() {
            //SharpDX.Direct3D11.DepthStencilView Backup;
            //var renderTargets = e.GraphicsDevice.GetRenderTargets(out Backup);
            //e.GraphicsDevice.SetRenderTargets((RenderTarget2D)ST.TMP_Image);

            //e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied);
            //e.GraphicsDevice.Clear(Color.Transparent);
            //e.SpriteBatch.Draw(ST.Image, new Vector2(78, 78), ST.Rect, Color.White, Rotation, new Vector2(78, 78), 1, SpriteEffects.None, 0);
            //e.SpriteBatch.End();

            //e.GraphicsDevice.SetRenderTargets(Backup, renderTargets);

            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied);
            e.SpriteBatch.Draw(ST.Image, Position, ST.Rect, Color.White, Rotation, ST.Origin, new Vector2(0.5f, 0.25f), SpriteEffects.None, 0);
            e.SpriteBatch.End();
        }

        public static float AlphaFromPoint(float x, float y) {
            return (float)Math.Atan((y - Base_Start.Y) / (x - Base_Start.X));
        }
        public static SpectralThrow_Arrow Create(float Alpha) {
            SpectralThrow_Arrow New = new SpectralThrow_Arrow();
            New.Alpha = Alpha;
            e.All.AddSort(New);
            return New;
        }
    }
}
