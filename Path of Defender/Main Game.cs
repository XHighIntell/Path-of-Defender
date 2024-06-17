using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
//using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Graphics;
using System.Windows.Forms;

namespace Path_of_Defender {
    public partial class MainGame {
        public MainGame() { InitializeComponent(); }

        public bool Paused = false;
        public void Update() {
            Item.Hover_Item = null;
            if (e.Keyboard.IsKeyPressed(SharpDX.Toolkit.Input.Keys.Escape) == true || e.Keyboard.IsKeyPressed(SharpDX.Toolkit.Input.Keys.F1) == true) { Paused = !Paused; }
            if (e.Keyboard.IsKeyPressed(SharpDX.Toolkit.Input.Keys.S) == true) { Button_Shop_Click(null, null); }
            
            Controls.Update();

            if (Paused == false) {
                GameLevels.Update(); e.Player.Update(); e.All.Update(); Update_Shop();
            } else {
                if (e.Keyboard.IsKeyPressed(SharpDX.Toolkit.Input.Keys.X) == true) { GameLevels.Update(); e.Player.Update(); e.All.Update(); }
            }    
        }

        #region Passive Tree's Events
        private void Passive_Buttons_Click(object sender, EventArgs E) {
            if (((VirtualControl)sender).Name == "Cancel") {
                PassiveTree.RestoreBackup();
                PassiveTree.Visible = false;
            } else if (((VirtualControl)sender).Name == "OK") {
                e.Player.RefreshAllValueFromPassive(PassiveTree.BackupSkills, e.Player.PassiveTreeData.Skills);
                e.Player.PassivePointsLeft = PassiveTree.PointsLeft;
                PassiveTree.CreateBackup();
                PassiveTree.Button_OK.Enable = false;
            }
        }
        private void Passive_ItemMouseClick(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                if (PassiveTree.PointsLeft > 0 && PassiveTree.SelectedData.Skills[e.Clicks].Status == SkillStatus.CanAllocated) {
                    PassiveTree.SelectedData.Skills[e.Clicks].Status = SkillStatus.Allocated;
                    PassiveTree.PointsLeft -= 1;
                    PassiveTree.Refresh();

                    for (int i = 0; i < PassiveTree.SelectedData.Skills[e.Clicks].Properties.Length; i++) { 
                        if (PassiveTree.SelectedData.Skills[e.Clicks].Properties[i].Type == TypeProperty.Intelligence) {
                            PassiveTree.SelectedData.Labels[0].Text = (Convert.ToInt32(PassiveTree.SelectedData.Labels[0].Text) + PassiveTree.SelectedData.Skills[e.Clicks].Properties[i].Value[0]).ToString();
                        } else if (PassiveTree.SelectedData.Skills[e.Clicks].Properties[i].Type == TypeProperty.Strength) {
                            PassiveTree.SelectedData.Labels[1].Text = (Convert.ToInt32(PassiveTree.SelectedData.Labels[1].Text) + PassiveTree.SelectedData.Skills[e.Clicks].Properties[i].Value[0]).ToString();
                        } else if (PassiveTree.SelectedData.Skills[e.Clicks].Properties[i].Type == TypeProperty.Dexterity) {
                            PassiveTree.SelectedData.Labels[2].Text = (Convert.ToInt32(PassiveTree.SelectedData.Labels[2].Text) + PassiveTree.SelectedData.Skills[e.Clicks].Properties[i].Value[0]).ToString();
                        }
                    }

                    if (PassiveTree.SelectedData.Skills != PassiveTree.BackupSkills) { PassiveTree.Button_OK.Enable = true; }
                }
            }
            /*
            else if (e.Button == MouseButtons.Right) {
                if (PassiveTree.BackupSkills[e.Clicks].Status == SkillStatus.CanAllocated || PassiveTree.BackupSkills[e.Clicks].Status == SkillStatus.Unallocated) { 
                    if (PassiveTree.SelectedData.CanUnallocated(e.Clicks) == true) {
                        PassiveTree.SelectedData.Skills[e.Clicks].Status = SkillStatus.CanAllocated;
                        PassiveTree.Refresh();
                        PassiveTree.PointsLeft += 1;

                        for (int i = 0; i < PassiveTree.SelectedData.Skills[e.Clicks].Properties.Length; i++) { 
                            if (PassiveTree.SelectedData.Skills[e.Clicks].Properties[i].Type == TypeProperty.Add_Int) {
                                PassiveTree.SelectedData.Labels[0].Text = (Convert.ToInt32(PassiveTree.SelectedData.Labels[0].Text) + PassiveTree.SelectedData.Skills[e.Clicks].Properties[i].Value[0]).ToString();
                            } else if (PassiveTree.SelectedData.Skills[e.Clicks].Properties[i].Type == TypeProperty.Add_Str) {
                                PassiveTree.SelectedData.Labels[1].Text = (Convert.ToInt32(PassiveTree.SelectedData.Labels[1].Text) + PassiveTree.SelectedData.Skills[e.Clicks].Properties[i].Value[0]).ToString();
                            } else if (PassiveTree.SelectedData.Skills[e.Clicks].Properties[i].Type == TypeProperty.Add_Dex) {
                                PassiveTree.SelectedData.Labels[2].Text = (Convert.ToInt32(PassiveTree.SelectedData.Labels[2].Text) + PassiveTree.SelectedData.Skills[e.Clicks].Properties[i].Value[0]).ToString();
                            }
                        }
                    }
                }
            }
            */
        }
        #endregion
        #region Some Controls's Events
        private int Last_Pick_Index;
        private void Controls_Click(object sender, EventArgs E) {
            VirtualControl Sender = (VirtualControl)sender;
            if (Sender == ButtonPlusPassive){
                if (PassiveTree.Visible == true) { PassiveTree.RestoreBackup(); PassiveTree.Visible = false; }
                else { 
                    PassiveTree.CreateBackup(); PassiveTree.Button_OK.Enable = false; PassiveTree.Visible = true;
                    PassiveTree.PointsLeft = e.Player.PassivePointsLeft;
                    e.Player.PassiveTreeData.Labels[0].Text = e.Player.Stats.Intelligence.ToString();
                    e.Player.PassiveTreeData.Labels[1].Text = e.Player.Stats.Strength.ToString();
                    e.Player.PassiveTreeData.Labels[2].Text = e.Player.Stats.Dexterity.ToString();
                }

            } else if (Sender.Name == "Skill") {
                Last_Pick_Index = Sender.Index;
                for (int i = 0; i < SkillPicker.Controls.Count; i++) { SkillPicker.Controls.Items[i].Visible = false; }
                for (int i = 0; i < e.Player.LearnSkills.Length; i++) {
                    Buttons_Pick[i].Index = i;
                    Buttons_Pick[i].Visible = true;
                    Buttons_Pick[i].Skill = e.Player.LearnSkills[i];
                }
                Buttons_Pick[e.Player.LearnSkills.Length].Visible = true;
                Buttons_Pick[e.Player.LearnSkills.Length].Skill = null;
                SkillPicker.AutoSize();
                SkillPicker.Y = GameSetting.Height - 120 - SkillPicker.Height;
                SkillPicker.Visible = true;

            } else if (Sender.Name == "Pick") {
                if (Sender.Skill != null) {
                    e.Player.ShortSkills[Last_Pick_Index] = Sender.Index;
                    Buttons_Skill[Last_Pick_Index].Skill = e.Player.LearnSkills[Sender.Index];
                } else {
                    e.Player.ShortSkills[Last_Pick_Index] = -1;
                    Buttons_Skill[Last_Pick_Index].Skill = null;
                }
                SkillPicker.Visible = false;
            }
        }
        #endregion

        private void Button_Shop_Click(object sender, EventArgs E) {
            Character_Inventory_Invisible = !Character_Inventory_Invisible;
            Main_Inventory.Visible = Character_Inventory_Invisible;
            FullSet_Inventory.Visible = Character_Inventory_Invisible;
            Main_Shop.Visible = Character_Inventory_Invisible;
        }

        private void FullSet_Inventory_OnEquipped(object sender, CustomInventoryEventArgs E) {
            if (E.Item.Data.Type != TypeItem.Flask) { e.Player.ReceiveItem(E.Item); RefreshItems(); }
            Flaks_Drawer.Items = FullSet_Inventory.Flasks.Items;
        }
        private void FullSet_Inventory_OnUnEquipped(object sender, CustomInventoryEventArgs E) {
            if (E.Item.Data.Type != TypeItem.Flask) { e.Player.RemoveItem(E.Item); RefreshItems(); }
            Flaks_Drawer.Items = FullSet_Inventory.Flasks.Items;
        }
        private void RefreshItems() {
            e.Player.Helmet = FullSet_Inventory.Helmet.Item;
            e.Player.Amulet = FullSet_Inventory.Amulet.Item;
            e.Player.Weapon = FullSet_Inventory.Weapon_Left.Item;
            e.Player.Offhand_Weapon = FullSet_Inventory.Weapon_Right.Item;
            e.Player.Ring_Left = FullSet_Inventory.Ring_Left.Item;
            e.Player.Ring_Right = FullSet_Inventory.Ring_Right.Item;

            e.Player.Body_Armor = FullSet_Inventory.Body_Armor.Item;
            e.Player.Glove = FullSet_Inventory.Glove.Item;
            e.Player.Belt = FullSet_Inventory.Belt.Item;
            e.Player.Boots = FullSet_Inventory.Boots.Item;
            e.Player.Flasks = FullSet_Inventory.Flasks.Items;
        }

        private void OnBuy(object sender, InventoryEventArgs E) {
            if (e.Player.Gold >= E.Item.Data.Cost_Gold) { E.IsBought = true; e.Player.Gold -= E.Item.Data.Cost_Gold; }
        }
    }

    public partial class MainGame {
        public int Index_Image_Environment;
        public float Refresh_Shop_Time_Remaining;
        public void Start() {
            Index_Image_Environment = GameSetting.RND.Next(Images.Environments.Ens.Length);
            e.State = GameState.Gameplay;

            GameLevels = new GameLevels();
            
            GameLevels.Map.Load(System.Windows.Forms.Application.StartupPath + @"\Graphics\Maps\Normal.podmap");

            e.Player = new Player();
            e.Player.PassiveTreeData.Load(System.Windows.Forms.Application.StartupPath + @"\Graphics\Enough for Testing.pst");
            e.Player.ReceiveSkill(new DefaultAttack());
            

            e.All.Skills.Clear();
            e.All.Monsters.Clear();
            e.All.Objects.Clear();
            e.Main.GameLevels.Clear();            
            e.Main.SkillPicker.Visible = false;

            for (int i = 0; i < e.Main.Buttons_Skill.Length; i++) {
                e.Main.Buttons_Skill[i].Image = null;
                e.Main.Buttons_Skill[i].Skill = null;
            }

            for (int i = 0; i < e.Player.ShortSkills.Length; i++) {
                if (e.Player.ShortSkills[i] != -1) {
                    e.Main.Buttons_Skill[i].Image = e.Player.LearnSkills[e.Player.ShortSkills[i]].Icon;
                    e.Main.Buttons_Skill[i].Skill = e.Player.LearnSkills[e.Player.ShortSkills[i]];
                }
            }

            e.Player.NewStats(10, 10, 10, 80, 0, 70);
            e.Player.Stats.Life_Regeneration_Percent = 0.01f;
            e.Player.Stats.Mana_Regeneration_Percent = 0.01f;
            //Extensions.Add<Item>(ref e.Player.Items, new Item(e.SI.data.White_Items[0]));
            FullSet_Inventory.Clear(); e.Main.Main_Inventory.Clear();
            FullSet_Inventory.SetWeapon_Left(new Item(e.SI.data.White_Items[0]));
            Refresh_Shop();

            
#if DEBUG
            e.Player.PassivePointsLeft = 100;
#else
            e.Player.PassivePointsLeft = 0;
#endif
            //e.Player.PassivePointsLeft = 100;

            e.Main.PassiveTree.SelectedData = e.Player.PassiveTreeData;
        }
        public void Refresh_Shop() {
            Main_Shop.Clear();
            for (int i = 0; i < 6; i++) {
                LevelItem Rarity = e.SI.RandomRarity(0.20f, 0.10f, 0.01f);
                if (Rarity == LevelItem.Normal) { Main_Shop.AddItem(new Item(e.SI.RandomWhiteItem(e.Player.Level))); }
                else if (Rarity == LevelItem.Magic) { Main_Shop.AddItem(new Item(e.SI.RandomMagicItem(e.SI.RandomWhiteItem(e.Player.Level), e.Player.Level))); }
                else if (Rarity == LevelItem.Rare) { Main_Shop.AddItem(new Item(e.SI.RandomRareItem(e.SI.RandomWhiteItem(e.Player.Level), e.Player.Level))); }
            }

            Main_Shop.AddItem(new Item(e.SI.Random(0.20f, 0.10f, 0.05f)));
            Refresh_Shop_Time_Remaining = 30;
        }
        public void Update_Shop() {
            Refresh_Shop_Time_Remaining -= GameSetting.SecondPerFrame;
            if (Refresh_Shop_Time_Remaining <= 0) { Refresh_Shop(); }
        }
    }
}
