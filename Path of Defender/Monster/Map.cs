using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using System.Collections;
using Microsoft.VisualBasic;

namespace Path_of_Defender {
    public class GameLevels {
        public MapFile Map;
        public int StageIndex, StageRepeated, NextTriggerIndex;
        public float StageTime;

        public void Update() {
            StageTime += GameSetting.SecondPerFrame;
            if (StageTime >= Map.Stages[StageIndex].Triggers[NextTriggerIndex].TriggerTime) {
                for (int i = 0; i < Map.Stages[StageIndex].Triggers[NextTriggerIndex].Monsters.Length; i++) {
                    int index = Map.Stages[StageIndex].Triggers[NextTriggerIndex].Monsters[i].Index;
                    MonsterStats monsterStats = Map.MonstersInfo[index].Stats;
                    monsterStats.Life_Maximum *= (1 + StageRepeated * Map.Stages[StageIndex].Repeat_Increased_Life);
                    monsterStats.Mana_Maximum *= (1 + StageRepeated * Map.Stages[StageIndex].Repeat_Increased_Mana);

                    Trigger.Stuct_Monster struc_Monster = Map.Stages[StageIndex].Triggers[NextTriggerIndex].Monsters[i];

                    if (Map.MonstersInfo[index].Style == MonsterType.Bandit) {
                        if (struc_Monster.Random_Position == true) {
                            e.All.AddSort(new Bandit(GameSetting.Width + 50, GameSetting.RND.NextFloat(80, GameSetting.Height - 150), monsterStats, struc_Monster.Life_Percent, struc_Monster.Mana_Percent, struc_Monster.Animation_State));
                        } else {
                            e.All.AddSort(new Bandit(struc_Monster.X, struc_Monster.Y, monsterStats, struc_Monster.Life_Percent, struc_Monster.Mana_Percent, struc_Monster.Animation_State));
                        }
                    } else if (Map.MonstersInfo[index].Style == MonsterType.Hunter) {
                        if (struc_Monster.Random_Position == true) {
                            e.All.AddSort(new Hunter(GameSetting.Width + 50, GameSetting.RND.NextFloat(80, GameSetting.Height - 150), monsterStats, struc_Monster.Life_Percent, struc_Monster.Mana_Percent, struc_Monster.Animation_State));
                        } else {
                            e.All.AddSort(new Hunter(struc_Monster.X, struc_Monster.Y, monsterStats, struc_Monster.Life_Percent, struc_Monster.Mana_Percent, struc_Monster.Animation_State));
                        }
                    } else if (Map.MonstersInfo[index].Style == MonsterType.Firen) { 
                        if (struc_Monster.Random_Position == true) {
                            e.All.AddSort(new Firen(GameSetting.Width + 50, GameSetting.RND.NextFloat(80, GameSetting.Height - 150), monsterStats, struc_Monster.Life_Percent, struc_Monster.Mana_Percent, struc_Monster.Animation_State));
                        } else {
                            e.All.AddSort(new Firen(struc_Monster.X, struc_Monster.Y, monsterStats, struc_Monster.Life_Percent, struc_Monster.Mana_Percent, struc_Monster.Animation_State));
                        }
                    }
                    
                }

                NextTriggerIndex++;
                if (NextTriggerIndex == Map.Stages[StageIndex].Triggers.Length) { 
                    NextTriggerIndex = 0; StageTime = 0;
                    if (StageRepeated >= Map.Stages[StageIndex].Repeats) { StageIndex++; StageRepeated = 0; }
                    else { StageRepeated++; }
                }
                if (StageIndex == Map.Stages.Length) { StageIndex = Map.Stages.Length - 1; }
            }
        }
        public void Draw() {
            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied, e.GraphicsDevice.SamplerStates.PointMirror);
            e.SpriteBatch.DrawString(Fonts.FontinRegular25, "STAGE " + (StageIndex + 1), new Vector2(GameSetting.Width / 2, 0), Color.WhiteSmoke);

            //e.SpriteBatch.DrawString(Fonts.FontinRegular30, Map.Stages[StageIndex].Caption, new Vector2(GameSetting.Width / 2, 20) - Fonts.FontinRegular30.MeasureString(Map.Stages[StageIndex].Caption) / 2, Color.WhiteSmoke);
            e.SpriteBatch.End();
        }
        public void Clear() {
            StageIndex = 0;
            StageRepeated = 0;
            NextTriggerIndex = 0;
            StageTime = 0;
        }

    }
    public enum MonsterType { Bandit = 1, Hunter = 2, Firen = 10 }

    public struct MonsterInfo {
        public string Name;
        public MonsterType Style;
        public MonsterStats Stats;
    }

    public struct Stage {
        public string Caption;
        public int Repeats;
        public float Repeat_Increased_Life;
        public float Repeat_Increased_Mana;
        public Trigger[] Triggers;

        public static Stage Zero;
        static Stage() {
            Zero.Caption = "New Stage";
            Zero.Triggers = new Trigger[0];
        }
        public static Stage New() {
            return Zero;
        }
    }
    public struct Trigger {
        public float TriggerTime;
        public Stuct_Monster[] Monsters;

        public struct Stuct_Monster {
            public string Name; 
            public int Index;
            public float Life_Percent, Mana_Percent;
            public bool Random_Position;
            public float X, Y;
            public MoveState Animation_State;
            public static Stuct_Monster Zero;
            static Stuct_Monster() {
                Zero.Name = "Invaid Name";
                Zero.Life_Percent = 100;
                Zero.Mana_Percent = 100;
                Zero.Random_Position = true;
            }

            public static Stuct_Monster New() {
                return Zero;
            }
        }

        public static Trigger Zero;
        static Trigger() {
            Zero.TriggerTime = 0;
            Zero.Monsters = new Stuct_Monster[0];
        }
        public static Trigger New() { return Zero; }
    }
    public struct MapFile {
        public MonsterInfo[] MonstersInfo;
        public Stage[] Stages;
        public MapFile(MonsterInfo[] units, Stage[] stages) {
            MonstersInfo = units;
            Stages = stages;
        }
        public int GetIndex(string Name) {
            for (int i = 0; i < MonstersInfo.Length; i++) {
                if (MonstersInfo[i].Name == Name) {
                    return i;
                }
            }
            return -1;
        }
        public void Load(string file) {
            FileSystem.FileOpen(3, file, OpenMode.Binary, OpenAccess.ReadWrite, OpenShare.Default);
            ValueType tmp = this;
            FileSystem.FileGet(3, ref tmp);
            this = (MapFile)tmp;
            FileSystem.FileClose(3);
            for (int i = 0; i < this.Stages.Length; i++) {
                for (int j = 0; j < this.Stages[i].Triggers.Length; j++) { 
                    for (int k = 0; k < this.Stages[i].Triggers[j].Monsters.Length; k++) {
                        this.Stages[i].Triggers[j].Monsters[k].Index = this.GetIndex(this.Stages[i].Triggers[j].Monsters[k].Name);
                    }
                }
            }
        }
        public void Save(string file) {
            FileSystem.FileOpen(3, file, OpenMode.Binary, OpenAccess.ReadWrite, OpenShare.Default);           
            FileSystem.FilePut(3, this);
            FileSystem.FileClose(3);
        }
    }
}
