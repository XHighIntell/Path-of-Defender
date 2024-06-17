using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Graphics;

namespace Path_of_Defender {
    public class Flask_Buff : Buff {
        public float Time;
        public Item Flask;
        public Flask_Buff(Item item) {
            if (item.Data.Level == LevelItem.Normal) { Icon = Images.Skill.Buff.GetImage("flaskcharge"); }
            else if (item.Data.Level == LevelItem.Magic) { Icon = Images.Skill.Buff.GetImage("flaskcritstrike"); }
            else if (item.Data.Level == LevelItem.Rare) { Icon = Images.Skill.Buff.GetImage("UniqueSapphireFlaskBuff"); }
            else if (item.Data.Level == LevelItem.Unique) { Icon = Images.Skill.Buff.GetImage("Unique289Icon3"); }

            Time = item.Data.Time;
            e.Player.ReceiveItem(item);
            Flask = item;
        }

        public override void Update() {
            Time -= GameSetting.SecondPerFrame;
            e.Player.RegenLife(Flask.Data.Life / Flask.Data.Time * GameSetting.SecondPerFrame);
            e.Player.RegenMana(Flask.Data.Mana / Flask.Data.Time * GameSetting.SecondPerFrame);


            if (Time <= 0) {
                e.Player.RemoveItem(Flask);
                Deletable = true; 
            }
            
        }

        public override void Draw(int Index) {
            Draw(Index, Icon, BuffType.Buff, Math.Round(Time).ToString() + "s");
        }
    }
}
