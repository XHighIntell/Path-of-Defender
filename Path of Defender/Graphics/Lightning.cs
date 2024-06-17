using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Graphics;

namespace Path_of_Defender.Graphics {
    using arc = Images.Skill.Arc;
    using UI = Images.UI;

    public class Spark : GameObject {
        public static float Base_Start_Spark;
        public float Time_Spark;
        public Monster Target_Monster;

        public Spark(Vector2 pos) {
            Position = pos;
        }
        public Spark(Monster monster) {
            Target_Monster = monster;
        }

        public override void DrawOnTop2() {
            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.Additive);

            if (Time_Spark < 0.5) {
                e.SpriteBatch.Draw(arc.Image_Sparks, Target_Monster.Position, arc.Rect_Sparks, Color.White * 0.8f, 0, arc.Origin_Sparks, 0.7f, SpriteEffects.None, 0);
            } else if (Time_Spark >= 0.5f && Time_Spark < 0.7f) {
                e.SpriteBatch.Draw(arc.Image_Sparks, Target_Monster.Position, arc.Rect_Sparks, Color.White * 0.8f * (1 - (Time_Spark - 0.5f) / 0.2f), 0, arc.Origin_Sparks, (1 - (Time_Spark - 0.5f) / 0.2f) * 0.7f, SpriteEffects.None, 0);
            }
            
            e.SpriteBatch.End();
        }
        public override void Update() {
            Time_Spark += GameSetting.SecondPerFrame;
            if (Time_Spark >= 1) { Deletable = true; } 
        }
    }
}





