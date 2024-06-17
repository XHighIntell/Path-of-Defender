using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;
namespace Monster_Editor {
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
                Zero.X = 1280;
                Zero.Animation_State = MoveState.Walking;
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

    public static class Extensions {
        public static MonsterInfo[] Delete(this MonsterInfo[] e, int index) {
            Array.Copy(e, index + 1, e, index, e.Length - index - 1);
            Array.Resize(ref e, e.Length - 1);
            return e;
        }
        public static MonsterInfo[] Insert(this MonsterInfo[] e, int index, MonsterInfo monster) {
            Array.Resize(ref e, e.Length + 1);
            Array.Copy(e, index, e, index + 1, e.Length - 1 - index);
            e[index] = monster;
            return e;
        }

        public static Array RemoveAt(this Array e, int index) {
            Array.Copy(e, index + 1, e, index, e.Length - index - 1);
            Array New_Array = Array.CreateInstance(e.GetType().GetElementType(), e.Length - 1);
            Array.Copy(e, New_Array, e.Length - 1);
            return New_Array;
        }
        public static Array Insert(this Array e, int index, object item) {
            Array New_Array = Array.CreateInstance(e.GetType().GetElementType(), e.Length + 1);

            Array.Copy(e, 0, New_Array, 0, index);
            Array.Copy(e, index, New_Array, index + 1, e.Length  - index);

            New_Array.SetValue(item, index);
            return New_Array;
        }
        public static Array Add(this Array e, object item) {
            Array New_Array = Array.CreateInstance(e.GetType().GetElementType(), e.Length + 1);
            Array.Copy(e, 0, New_Array, 0, e.Length);
            New_Array.SetValue(item, e.Length);
            return New_Array;
        }

        
    }
}
