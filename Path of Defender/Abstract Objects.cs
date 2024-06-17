using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
namespace Path_of_Defender {
    //A collection for all and manage all
    public class ObjectsManager {
        public GameObjectCollection Objects = new GameObjectCollection();
        public SkillCollection Skills = new SkillCollection();
        public MonsterCollection Monsters = new MonsterCollection();
        public void AddSort(GameObject item) {
            if (item is Monster) { Monsters.Add((Monster)item); }
            else if (item is SkillObject) { Skills.Add((SkillObject)item); }
            Objects.AddSort(item);
        }
        public void Add(GameObject item) {
            if (item is Monster) { Monsters.Add((Monster)item); }
            else if (item is SkillObject) { Skills.Add((SkillObject)item); }
            Objects.Add(item);
        }
        public void Remove(GameObject gameObject) {
            Objects.Remove(gameObject);
            if (gameObject is Monster) { Monsters.Remove((Monster)gameObject); }
            else if (gameObject is SkillObject) { Skills.Remove((SkillObject)gameObject); }
        }
        public void Draw() {
            for (int i = 0; i < Objects.Items.Length; i++) { Objects.Items[i].DrawOnGround(); }
            for (int i = 0; i < Objects.Items.Length; i++) { Objects.Items[i].Draw(); }
            for (int i = 0; i < Objects.Items.Length; i++) { Objects.Items[i].DrawOnTop(); }
            for (int i = 0; i < Objects.Items.Length; i++) { Objects.Items[i].DrawOnTop2(); }
        }
        private GameObject[] TMP_Objects;
        public void Update() {
            TMP_Objects = new GameObject[Objects.Items.Length];
            Array.Copy(Objects.Items, TMP_Objects, Objects.Items.Length);
            for (int i = 0; i < TMP_Objects.Length; i++) {
                TMP_Objects[i].Update();
                if (TMP_Objects[i].Deletable == true) {
                    Remove(TMP_Objects[i]);
                }
            }
        }
    }

    //GameObject
    public abstract class GameObject {
        public Vector2 Position; 
        public bool Deletable = false;
        public virtual void DrawOnGround() { }
        public virtual void Draw() { }
        public virtual void DrawOnTop() { }
        public virtual void DrawOnTop2() { }
        public virtual void Update() { }
    }
    public class GameObjectCollection {
        public GameObject[] Items = { };

        public int Count { get { return Items.Length; } }
        public void Add(GameObject item) {
            Array.Resize(ref Items, Items.Length + 1);
            Items[Items.Length - 1] = item;
        }
        public void Insert(GameObject item, int index) {
            Array.Resize(ref Items, Items.Length + 1);
            if (Items.Length != 1) {
                Array.Copy(Items, index, Items, index + 1, Items.Length - 1 - index);
            }
            Items[index] = item;
        }
        public void AddSort(GameObject item) {
            int index = 0;
            for (int i = 0; i < Items.Length; i++) {
                if (item.Position.Y > Items[i].Position.Y) { index = i + 1; }
                else { break; }
            }
            Insert(item, index);
        }

        public void RemoveAt(int index) {
            Array.Copy(Items, index + 1, Items, index, Items.Length - index - 1);
            Array.Resize(ref Items, Items.Length - 1); }
        public void Remove(GameObject item) {
            int index = Array.IndexOf(Items, item);
            if (index != -1) RemoveAt(index);
        }
        public void Clear() { Array.Resize(ref  Items, 0); }

        GameObjectIComparer Comparer = new GameObjectIComparer();
        public class GameObjectIComparer : System.Collections.IComparer {
            int System.Collections.IComparer.Compare(Object x, Object y) {
                GameObject GameObject1 = (GameObject)x;
                GameObject GameObject2 = (GameObject)y;
                return (int)(GameObject1.Position.Y - GameObject2.Position.Y);
            }
        }
        public void Sort() { Array.Sort(Items, Comparer); }
    }

    //Monster
    public class MonsterCollection {
        public Monster[] Items = { };

        public int Count { get { return Items.Length; } }
        public void Add(Monster item) {
            Array.Resize(ref Items, Items.Length + 1);
            Items[Items.Length - 1] = item;
        }
        public void RemoveAt(int index) {
            Array.Copy(Items, index + 1, Items, index, Items.Length - index - 1);
            Array.Resize(ref Items, Items.Length - 1);
        }
        public void Remove(Monster monster) {
            int index = Array.IndexOf(Items, monster);
            if (index != -1) RemoveAt(index);
        }
        public void Clear() { Array.Resize(ref  Items, 0); }

        public Monster[] GetMonstersInEllipse(float x, float y, float rx, float ry, MonsterState state) {
            Monster[] Re = { };
            for (int i = 0; i < Items.Length; i++) {
                if (Items[i].State == state && Math.Pow((Items[i].Position.X - x) / rx, 2) + Math.Pow((Items[i].Position.Y - y) / ry, 2) <= 1) {
                    Array.Resize(ref Re, Re.Length + 1);
                    Re[Re.Length - 1] = Items[i];
                }

            }
            return Re;
        }
        public Monster GetMonsterInPoint(float x, float y, MonsterState state) {
            for (int i = 0; i < Items.Length; i++) {
                if (state == Items[i].State && Items[i].Position.X + Items[i].Collision_Rect.X <= x && x <= Items[i].Position.X + Items[i].Collision_Rect.X + Items[i].Collision_Rect.Width && 
                    Items[i].Position.Y + Items[i].Collision_Rect.Y <= y && y <= Items[i].Position.Y + Items[i].Collision_Rect.Y + Items[i].Collision_Rect.Height) {
                        return Items[i];
                }
            }
            return null;
        }
        public Monster[] GetMonstersInPoint(float x, float y, MonsterState state) {
            Monster[] monsters = new Monster[0];
            for (int i = 0; i < Items.Length; i++) {
                if (state == Items[i].State && Items[i].Position.X + Items[i].Collision_Rect.X <= x && x <= Items[i].Position.X + Items[i].Collision_Rect.X + Items[i].Collision_Rect.Width && 
                    Items[i].Position.Y + Items[i].Collision_Rect.Y <= y && y <= Items[i].Position.Y + Items[i].Collision_Rect.Y + Items[i].Collision_Rect.Height) {
                        Extensions.AddMonsterToArray(ref monsters, Items[i]);
                }
            }
            return monsters;
        }

        /// <summary> Tìm 1 monster gần nhất </summary>
        public Monster FindClosestMonsterInCircle(Vector2 position, float radius) {
            Monster Return;
            double Distance = 0, TMP_Distance;
            if (Items.Length >= 1) { Distance = Items[0].Position.DistanceTo(position); Return = Items[0]; }
            else { return null; }

            for (int i = 1; i < Items.Length; i++) {
                TMP_Distance = Items[i].Position.DistanceTo(position);
                if (TMP_Distance < Distance) { Distance = TMP_Distance; Return = Items[i]; }
            }
            if (Distance <= radius) { return Return; }
            return null;
        }

        /// <summary> Tìm nhiều monster trong circle và sắp xếp theo khoản cách. Index = 0 là gần nhất </summary>
        public Monster[] FindClosestMonstersInCircle(Vector2 position, float radius, MonsterState state) {
            Monster[] Re = { };
            for (int i = 0; i < Items.Length; i++) {
                if (Items[i].State == state) { 
                Items[i].Float = (float)Items[i].Position.DistanceTo(position);
                    if (Items[i].Float < radius) {
                        int index = 0;
                        for (int j = 0; j < Re.Length; j++) {
                            if (Items[i].Float > Re[j].Float) { index = j + 1; }
                            else { break; }
                        }
                        Extensions.InsertMonsterToArray(ref Re, Items[i], index);
                    }
                }
            }
            return Re;
        }


    }


    //Skills

    public class SkillCollection {
        public SkillObject[] Items = { };
        public int Count { get { return Items.Length; } }
        public void Add(SkillObject item) {
            Array.Resize(ref Items, Items.Length + 1);
            Items[Items.Length - 1] = item;
        }
        public void RemoveAt(int index) {
            Array.Copy(Items, index + 1, Items, index, Items.Length - index - 1);
            Array.Resize(ref Items, Items.Length - 1);
        }
        public void Remove(SkillObject skill) {
            int index = Array.IndexOf(Items, skill);
            if (index != -1) RemoveAt(index);
        }
        public void Clear() { Array.Resize(ref  Items, 0); }
    }
}
