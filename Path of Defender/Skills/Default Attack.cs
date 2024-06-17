using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Graphics;

namespace Path_of_Defender {
    
    public partial class DefaultAttack : Skill {
        public DefaultAttack() : base(Images.Skill.Icon.GetImage("Basic Attack"), AbilityType.Attack) { }
        public override void DrawInfo() {
            PlayerStats o = e.Player.Stats; string[] Properties = new string[0];

            Extensions.Add(ref Properties, Math.Round(o.Physical_Damage_Attack[0] * (1 + o.Increased_Physical_Damage)).ToString() + "-" +
                                           Math.Round(o.Physical_Damage_Attack[1] * (1 + o.Increased_Physical_Damage)).ToString() + " PHYSICAL DAMAGE");

            DrawInfo(Icon, "DEFAULT ATTACK", new string[1] { "Attack Time" },
                new string[1] { Math.Round(GetCastTime(), 3).ToString() },
                    "Strike your foes down with a powerfull blow.", Properties);
        }

        public override float GetCastTime() {
            //if (e.Player.Weapon == null) { return 1 / Player.Unarmed_Attack_per_Second; }
            if (e.Player.Weapon == null) { return float.NaN; }
            return 1 / e.Player.Weapon.Data.Attacks_per_Second / (1 + e.Player.Stats.Increased_Attack_Speed);
        }
        public override void Cast() { Basic_Arrow.Create(e.Mouse_PositionF); base.Cast(); }
    }

    public class Basic_Arrow : SkillObject {
        public static float Base_Speed = 30;
        public static Vector2 Base_Start = new Vector2(-10, 300);
        public float Time, Alpha;
        public Basic_Arrow() : base(AbilityType.Attack) {
            PlayerStats o = e.Player.Stats; Position = Base_Start;
            Initialize();
        }

        public override void Update() {
            Time += GameSetting.SecondPerFrame;
            if (Time >= 1) { Deletable = true; }
            else {
                Position.X += (float)Math.Cos(Alpha) * Base_Speed;
                Position.Y += (float)Math.Sin(Alpha) * Base_Speed;
                Monster AttackedMonster = e.All.Monsters.GetMonsterInPoint(Position.X, Position.Y, MonsterState.Alive);
                if (AttackedMonster != null) {
                    AttackedMonster.TakeDamage(this);
                    Deletable = true;
                };
            }
        }
        public override void DrawOnTop() {
            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.Additive);

            e.SpriteBatch.Draw(Images.Skill.Arrow.Trail, new Vector2((float)(Position.X - 40 * Math.Cos(Alpha)), (float)(Position.Y - 40 * Math.Sin(Alpha))),
                    Images.Skill.Arrow.TrailRect, Color.White, Alpha, Images.Skill.Arrow.TrailOrigin, new Vector2(2, 0.1f), SpriteEffects.None, 0);
            e.SpriteBatch.Draw(Images.Skill.Arrow.Image, Position, Images.Skill.Arrow.Rect, Color.White, Alpha, Images.Skill.Arrow.Origin, 1, SpriteEffects.None, 0);
            e.SpriteBatch.End();
        }

        public static float AlphaFromPoint(float x, float y) {
            return (float)Math.Atan((y - Base_Start.Y) / (x - Base_Start.X));
        }

        public static Basic_Arrow Create(Vector2 Position) {
            Basic_Arrow NEW = new Basic_Arrow();
            NEW.Alpha = AlphaFromPoint(Position.X, Position.Y);
            e.All.AddSort(NEW);
            return NEW;
        }

    }
}
