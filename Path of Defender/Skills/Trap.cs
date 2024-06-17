using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Graphics;

namespace Path_of_Defender {
    public class Trap : SkillObject {
        public static float Base_Activation_Delay = 0.5f; //là thời gian it nhất để nổ
        public static float Base_Detonation_Delay = 0.3f;
        public string Skill_Name;
        public float Radius = 50;
        public float Time;
        public Trap(string skillName) {
            Position = new Vector2(e.X, e.Y);
            e.Player.TimeAttackRemain = 0.1f;
            Skill_Name = "Path_of_Defender." + skillName;
        }
        
        public override void Draw() {
            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied);
            e.SpriteBatch.Draw(Images.Skill.Trap.ImageMine, Position, Images.Skill.Trap.RectMine, Color.White, 0, Images.Skill.Trap.OriginMine, 0.2f, SpriteEffects.None, 0);
            e.SpriteBatch.End();
        }

        public int Add_Count, Tick_Count;
        public bool Active;
        public override void Update() {
            Time += GameSetting.SecondPerFrame;
            if (Active == false && Time >= Base_Activation_Delay) {
                if (e.All.Monsters.GetMonstersInEllipse(Position.X, Position.Y, Radius, Radius * 0.6f, MonsterState.Alive).Length > 0) {
                    Active = true; Time = 0;
                }; 
            } else if (Active == true && Time >= Base_Detonation_Delay) {
                Type.GetType(Skill_Name).GetMethod("Create").Invoke(null, new object[] { this.Position });
                Deletable = true;
            }
        }
    }
}
