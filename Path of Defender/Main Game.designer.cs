using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Graphics;
using System.Reflection;
namespace Path_of_Defender {

    using UI = Images.UI;
    public partial class MainGame {
        static int Base_Maximum_Skills_In_Game = 10;

        public VirtualControlCollection Controls;
        public PassiveTree PassiveTree;

        public VirtualContainer SkillPicker;
        public VirtualButton ButtonPlusPassive;
        public GameLevels GameLevels;
        public VirtualButton[] Buttons_Skill = new VirtualButton[8];
        public VirtualButton[] Buttons_Pick = new VirtualButton[Base_Maximum_Skills_In_Game];
        public VirtualButton Button_Shop = new VirtualButton() { X = 874, Y = 617, Width = 45, Height = 23, Visible = true};
        


        private void InitializeComponent() {
            //PassiveTree Control
            PassiveTree = new PassiveTree();
            PassiveTree.Width = GameSetting.Width;
            PassiveTree.Height = GameSetting.Height;
            PassiveTree.Button_OK.Click += new EventHandler(Passive_Buttons_Click);
            PassiveTree.Button_Cancel.Click += new EventHandler(Passive_Buttons_Click);
            PassiveTree.ItemMouseClick += new System.Windows.Forms.MouseEventHandler(Passive_ItemMouseClick);

            //ButtonPlusPassive 
            ButtonPlusPassive = new VirtualButton();
            ButtonPlusPassive.Name = "Passive";
            ButtonPlusPassive.Visible = true;
            ButtonPlusPassive.X = 420; ButtonPlusPassive.Y = 620;
            ButtonPlusPassive.Width =42; ButtonPlusPassive.Height = 40;
            ButtonPlusPassive.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            ButtonPlusPassive.Click += new EventHandler(Controls_Click);

            //Buttons_Skill 
            for (int i = 0; i <= 7; i++) {
                Buttons_Skill[i] = new VirtualButton();
                Buttons_Skill[i].Name = "Skill"; Buttons_Skill[i].Index = i;
                Buttons_Skill[i].Visible = true;
                if (i < 3) {
                    Buttons_Skill[i].X = GameSetting.Width - 292 + i * 41;
                    Buttons_Skill[i].Y = GameSetting.Height - 92;
                } else {
                    Buttons_Skill[i].X = GameSetting.Width - 374 + (i - 3) * 41;
                    Buttons_Skill[i].Y = GameSetting.Height - 42;
                }
                Buttons_Skill[i].Width = 36;
                Buttons_Skill[i].Height = 36;
                Buttons_Skill[i].DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
                Buttons_Skill[i].Click +=new EventHandler(Controls_Click);
            }

            int TMP_Int;
            //Buttons_Pick 
            for (int i = 0; i < Base_Maximum_Skills_In_Game; i++) {
                Buttons_Pick[i] = new VirtualButton(); Buttons_Pick[i].Visible = true; Buttons_Pick[i].Name = "Pick";
                TMP_Int = i / 4;
                Buttons_Pick[i].Y = TMP_Int * 40; Buttons_Pick[i].X = (i - TMP_Int * 4) * 40; Buttons_Pick[i].Width = 40; Buttons_Pick[i].Height = 40;
                Buttons_Pick[i].DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
                Buttons_Pick[i].Click += new EventHandler(Controls_Click);
            }

            //SkillPicker 
            SkillPicker = new VirtualContainer();
            SkillPicker.X = GameSetting.Width - 374;
            SkillPicker.Y = GameSetting.Height - 100;
            SkillPicker.Width = 400;
            SkillPicker.Height = 10;
            for (int i = 0; i < Base_Maximum_Skills_In_Game; i++) {
                SkillPicker.Controls.Add(Buttons_Pick[i]);
            }
            
            //Form             
            this.Controls = new VirtualControlCollection();
            this.Controls.Add(PassiveTree);
            this.Controls.Add(Flaks_Drawer);
            this.Controls.Add(Main_Inventory);
            this.Controls.Add(FullSet_Inventory);
            this.Controls.Add(Main_Shop);
            this.Controls.Add(Button_Shop); 
            this.Controls.Add(SkillPicker); this.Controls.Add(ButtonPlusPassive);
            for (int i = 0; i <= 7; i++) { this.Controls.Add(Buttons_Skill[i]); }
            InitializeComponent2();
        }

        /// <summary> Inventory HUD </summary>
        public Inventory Main_Inventory = new Inventory(12, 5);
        public InventoryFullSet FullSet_Inventory = new InventoryFullSet(); public bool Character_Inventory_Invisible;
        public BeltFlasks Flaks_Drawer = new BeltFlasks() { X = 226, Y = 610, Visible = true };

        public Inventory Main_Shop = new Inventory(10, 10) { X = 50, Y = 150, Type = InventoryType.Shop }; //public bool Shop_Inventory_Invisible;
        private void InitializeComponent2() {
            Main_Inventory.X = GameSetting.Width - 474;  Main_Inventory.Y = 359;
            
            FullSet_Inventory.X = 768;
            FullSet_Inventory.OnEquipped += FullSet_Inventory_OnEquipped;
            FullSet_Inventory.OnUnEquipped += FullSet_Inventory_OnUnEquipped;
            Button_Shop.Click += Button_Shop_Click;
            Main_Shop.OnBuy += OnBuy;
        }

        
        



        private float ButtonPlusOpacity = 0; bool IsIncreaseOpacity = true;
        #region Draw
        public void Draw() {
            if (PassiveTree.Visible == true) {
                PassiveTree.Draw();
            } else {
                Images.Environments.Ens[Index_Image_Environment].Draw();
                Images.UI.StrongHold.Draw();
                e.All.Draw();
                GameLevels.Draw(); // Draw Stage Name Index...
                if (Paused == true) { //Draw Pause state
                    e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied);
                    e.SpriteBatch.DrawStringCenter(Fonts.FontinBold50, "PAUSED", new Vector2(GameSetting.Width / 2, GameSetting.Height / 2), Color.Red, 0, 1, SpriteEffects.None);
                    e.SpriteBatch.End();
                }
            }
            e.Player.Buffs.Draw();
            DrawInventory();
            DrawHUD();
        }
        public void DrawHUD() {
            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied, e.GraphicsDevice.SamplerStates.PointClamp);
            //Orb Life //Orb Mana
            DrawOrbLife(e.SpriteBatch, e.GraphicsDevice);
            DrawOrbMana(e.SpriteBatch, e.GraphicsDevice);
            
            //ExperienceBarBackground
            for (int i = 0; i < 5; i++) {
                e.SpriteBatch.Draw(Images.UI.HUD.ExperienceBarBackground.Image, new Vector2(419 + 86 * i, GameSetting.Height), Images.UI.HUD.ExperienceBarBackground.Rect, Color.White, 0, Images.UI.HUD.ExperienceBarBackground.Origin, 0.5f, SpriteEffects.None, 0);
            }
            
            //Draw MainLeft
            e.SpriteBatch.Draw(Images.UI.HUD.MainLeft.Image, new Vector2(0, GameSetting.Height), Images.UI.HUD.MainLeft.Rect, Color.White, 0, Images.UI.HUD.MainLeft.Origin, 0.5f, SpriteEffects.None, 0);
            DrawOrbEnergyShield();

            //Draw MainRight
            e.SpriteBatch.Draw(Images.UI.HUD.MainRight.Image, new Vector2(GameSetting.Width, GameSetting.Height), Images.UI.HUD.MainRight.Rect, Color.White, 0, Images.UI.HUD.MainRight.Origin, 0.5f, SpriteEffects.None, 0);
            e.SpriteBatch.End();


            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.AlphaBlend, e.GraphicsDevice.SamplerStates.AnisotropicWrap);
            //ExperienceBarFill
            e.SpriteBatch.Draw(Images.UI.HUD.ExperienceBarFill.Image, new Vector2(410, GameSetting.Height),
                Images.UI.HUD.ExperienceBarFill.Rect, Color.White, 0, Images.UI.HUD.ExperienceBarFill.Origin, new Vector2(23.8333f * e.Player.CurXP / e.Player.MaxXP, 1f), SpriteEffects.None, 0);
            DrawButtons();
            e.SpriteBatch.End();
            Flaks_Drawer.Draw();
            Draw_Button_Shop();
            Item.Static_Draw();
            DrawSkillPicker_ShortcutSkills(e.SpriteBatch, e.GraphicsDevice);
        }
        private void DrawOrbLife(SpriteBatch spriteBatch, GraphicsDevice devide) {
            float Percent = e.Player.Life / e.Player.Life_Maximum;
            float H = 314 * (1 - Percent);

            Vector2 Pos_OrbLife = new Vector2(8, GameSetting.Height - 157);
            spriteBatch.Draw(Images.UI.HUD.OrbLifeBackground.Image, Pos_OrbLife,
                Images.UI.HUD.OrbLifeBackground.Rect, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);


            Rectangle d = new Rectangle(Images.UI.HUD.OrbLifeContent.Rect.X, (int)(Images.UI.HUD.OrbLifeContent.Rect.Y + H),
                Images.UI.HUD.OrbLifeContent.Rect.Width, (int)(Images.UI.HUD.OrbManaContent.Rect.Height - H));

            spriteBatch.Draw(Images.UI.HUD.OrbLifeContent.Image, new Vector2(8,GameSetting.Height - 157 * Percent),
            d, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);

            spriteBatch.Draw(Images.UI.HUD.OrbLifeForeground.Image, Pos_OrbLife,
                Images.UI.HUD.OrbLifeForeground.Rect, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);

            Vector2 SizeString = Fonts.FontinRegular11.MeasureString("Life: " + (int)e.Player.Life + "/" + (int)e.Player.Life_Maximum);
            spriteBatch.DrawString(Fonts.FontinRegular9, "Life: " + (int)e.Player.Life + "/" + (int)e.Player.Life_Maximum, new Vector2(90 - SizeString.X / 2, GameSetting.Height - 176 - SizeString.Y), Color.White);
            
        }
        private void DrawOrbEnergyShield() {
            float Percent = e.Player.Energy_Shield / e.Player.Energy_Shield_Maximum;
            int Height = (int)(314 * (1 - Percent));
            Vector2 Pos_Orb = new Vector2(38, GameSetting.Height - 157 *Percent);
            Rectangle Rect;

            if (e.Player.Energy_Shield_Maximum >= 800) {
                Rect = new Rectangle(Images.UI.HUD.OrbEnergyShieldContent.Rect_Shield_3.X, Images.UI.HUD.OrbEnergyShieldContent.Rect_Shield_3.Y + Height,
                Images.UI.HUD.OrbEnergyShieldContent.Rect_Shield_3.Width, Images.UI.HUD.OrbEnergyShieldContent.Rect_Shield_3.Height - Height);

                e.SpriteBatch.Draw(Images.UI.HUD.OrbEnergyShieldContent.Image_Shield_3, Pos_Orb,
                    Rect, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
            } else if (e.Player.Energy_Shield_Maximum >= 350) {
                Rect = new Rectangle(Images.UI.HUD.OrbEnergyShieldContent.Rect_Shield_2.X, Images.UI.HUD.OrbEnergyShieldContent.Rect_Shield_2.Y + Height,
                Images.UI.HUD.OrbEnergyShieldContent.Rect_Shield_2.Width, Images.UI.HUD.OrbEnergyShieldContent.Rect_Shield_2.Height - Height);

                e.SpriteBatch.Draw(Images.UI.HUD.OrbEnergyShieldContent.Image_Shield_2, Pos_Orb,
                    Rect, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
            } else if (e.Player.Energy_Shield_Maximum >= 150) {
                Rect = new Rectangle(Images.UI.HUD.OrbEnergyShieldContent.Rect_Shield_1.X, Images.UI.HUD.OrbEnergyShieldContent.Rect_Shield_1.Y + Height,
                    Images.UI.HUD.OrbEnergyShieldContent.Rect_Shield_1.Width, Images.UI.HUD.OrbEnergyShieldContent.Rect_Shield_1.Height - Height);

                e.SpriteBatch.Draw(Images.UI.HUD.OrbEnergyShieldContent.Image_Shield_1, Pos_Orb,
                    Rect, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
            } else {
                Rect = new Rectangle(Images.UI.HUD.OrbEnergyShieldContent.Rect_Shield_0.X, Images.UI.HUD.OrbEnergyShieldContent.Rect_Shield_0.Y + Height,
                        Images.UI.HUD.OrbEnergyShieldContent.Rect_Shield_0.Width, Images.UI.HUD.OrbEnergyShieldContent.Rect_Shield_0.Height - Height);

                e.SpriteBatch.Draw(Images.UI.HUD.OrbEnergyShieldContent.Image_Shield_0, Pos_Orb, Rect, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
            }

            string Text = "Shield: " + (int)e.Player.Energy_Shield + "/" + (int)e.Player.Energy_Shield_Maximum;
            Vector2 SizeString = Fonts.FontinRegular11.MeasureString(Text);
            e.SpriteBatch.DrawString(Fonts.FontinRegular9, Text, new Vector2(90 - SizeString.X / 2, GameSetting.Height - 173 - SizeString.Y / 2), Color.White);
        }

        private void DrawOrbMana(SpriteBatch spriteBatch, GraphicsDevice devide) {
            Vector2 Pos_OrbMana = new Vector2(GameSetting.Width - 164, GameSetting.Height - 156);
            spriteBatch.Draw(Images.UI.HUD.OrbManaBackground.Image, Pos_OrbMana,
                Images.UI.HUD.OrbManaBackground.Rect, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);

            float PercentMana = e.Player.Mana / e.Player.Mana_Maximum;
            float H = 314 * (1 - PercentMana);

            Rectangle d = new Rectangle(Images.UI.HUD.OrbManaContent.Rect.X, (int)(Images.UI.HUD.OrbManaContent.Rect.Y + H),
                Images.UI.HUD.OrbManaContent.Rect.Width, (int)(Images.UI.HUD.OrbManaContent.Rect.Height - H));

            spriteBatch.Draw(Images.UI.HUD.OrbManaContent.Image, new Vector2(GameSetting.Width - 164, GameSetting.Height - 157 * PercentMana),
                d, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);

            spriteBatch.Draw(Images.UI.HUD.OrbManaForeground.Image, Pos_OrbMana,
                Images.UI.HUD.OrbManaForeground.Rect, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);

            Vector2 SizeString = Fonts.FontinRegular9.MeasureString("Mana: " + (int)e.Player.Mana + "/" + (int)e.Player.Mana_Maximum);
            spriteBatch.DrawString(Fonts.FontinRegular9, "Mana: " + (int)e.Player.Mana + "/" + (int)e.Player.Mana_Maximum, new Vector2(GameSetting.Width - 84 - SizeString.X / 2, GameSetting.Height - 173 - SizeString.Y / 2), Color.White);

        }
        private void DrawButtons() { 
            if (IsIncreaseOpacity == true) {
                ButtonPlusOpacity += 0.04f;
                if (ButtonPlusOpacity >= 1) { ButtonPlusOpacity = 1; IsIncreaseOpacity = false; }
            } else {
                ButtonPlusOpacity -= 0.04f;
                if (ButtonPlusOpacity <= 0) { ButtonPlusOpacity = 0; IsIncreaseOpacity = true; }
            }

            e.SpriteBatch.Draw(UI.HUD.Buttons.NormalButton.Image, new Vector2(ButtonPlusPassive.X, ButtonPlusPassive.Y), UI.HUD.Buttons.NormalButton.Rect, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);

            if (ButtonPlusPassive.IsHover == true) {
                e.SpriteBatch.Draw(UI.HUD.Buttons.ActiveButton.Image, new Vector2(ButtonPlusPassive.X, ButtonPlusPassive.Y), UI.HUD.Buttons.ActiveButton.Rect, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
            } else {
                e.SpriteBatch.Draw(UI.HUD.Buttons.ActiveButton.Image, new Vector2(ButtonPlusPassive.X, ButtonPlusPassive.Y), UI.HUD.Buttons.ActiveButton.Rect, Color.White * ButtonPlusOpacity, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
            }
        }
        private void Draw_Button_Shop() {
            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied, e.GraphicsDevice.SamplerStates.AnisotropicClamp);
            if (Button_Shop == e.MouseDown_Control ) {
                e.SpriteBatch.Draw(UI.HUD.Button_Shop.Image, new Vector2(Button_Shop.X, Button_Shop.Y), UI.HUD.Button_Shop.Rect_Down, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
                e.SpriteBatch.DrawString(Fonts.FontinRegular10, "SHOP", new Vector2(Button_Shop.X + 5, Button_Shop.Y + 3), Color.White * 0.4f);
            } else if (Button_Shop.IsHover == true) {
                e.SpriteBatch.Draw(UI.HUD.Button_Shop.Image, new Vector2(Button_Shop.X, Button_Shop.Y), UI.HUD.Button_Shop.Rect_Hover, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
                e.SpriteBatch.DrawString(Fonts.FontinRegular10, "SHOP", new Vector2(Button_Shop.X + 5, Button_Shop.Y + 3), Color.White);
            } else {
                e.SpriteBatch.Draw(UI.HUD.Button_Shop.Image, new Vector2(Button_Shop.X, Button_Shop.Y), UI.HUD.Button_Shop.Rect_Normal, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
                e.SpriteBatch.DrawString(Fonts.FontinRegular10, "SHOP", new Vector2(Button_Shop.X + 5, Button_Shop.Y + 3), Color.White * 0.8f);
            }
            e.SpriteBatch.End();
        }
        public static Color DisabledColor = new Color(100, 100, 100);
        private void DrawSkillPicker_ShortcutSkills(SpriteBatch spriteBatch, GraphicsDevice devide) { 
            //Draw Skill picker
            spriteBatch.Begin(SpriteSortMode.Deferred, devide.BlendStates.NonPremultiplied);
            if (SkillPicker.Visible == true) {
                for (int i = 0; i < Buttons_Pick.Length; i++) {
                    if (Buttons_Pick[i].Visible == true) {
                        if (Buttons_Pick[i].Skill != null) {
                            Buttons_Pick[i].Skill.DrawIcon(Buttons_Pick[i].X + SkillPicker.X, Buttons_Pick[i].Y + SkillPicker.Y, Buttons_Pick[i].Width, Buttons_Pick[i].Height);
                        } else {
                            spriteBatch.Draw(Images.Skill.Icon.GetImage("X"), new Rectangle(Buttons_Pick[i].X + SkillPicker.X, Buttons_Pick[i].Y + SkillPicker.Y, Buttons_Pick[i].Width, Buttons_Pick[i].Height), Color.White);
                        }
                    }
                }
            }

            //Đây là những buttons skill đả chọn 0 - 7
            for (int i = 0; i < Buttons_Skill.Length ; i++) {
                if (Buttons_Skill[i].Skill != null) {
                    Buttons_Skill[i].Skill.DrawIcon(Buttons_Skill[i].X, Buttons_Skill[i].Y, 36, 36);
                }
            }

            spriteBatch.Draw(UI.HUD.VirtualKeyIcons.Image, new Vector2(Buttons_Skill[0].X, Buttons_Skill[0].Y + 20), UI.HUD.VirtualKeyIcons.MouseLeft, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
            spriteBatch.Draw(UI.HUD.VirtualKeyIcons.Image, new Vector2(Buttons_Skill[1].X, Buttons_Skill[1].Y + 20), UI.HUD.VirtualKeyIcons.MouseMid, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
            spriteBatch.Draw(UI.HUD.VirtualKeyIcons.Image, new Vector2(Buttons_Skill[2].X, Buttons_Skill[2].Y + 20), UI.HUD.VirtualKeyIcons.MouseRight, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);

            spriteBatch.DrawString(Fonts.FontinRegular9, "Q", new Vector2(Buttons_Skill[3].X + 2, Buttons_Skill[3].Y + 22), Color.White);
            spriteBatch.DrawString(Fonts.FontinRegular9, "W", new Vector2(Buttons_Skill[4].X + 2, Buttons_Skill[4].Y + 22), Color.White);
            spriteBatch.DrawString(Fonts.FontinRegular9, "E", new Vector2(Buttons_Skill[5].X + 2, Buttons_Skill[5].Y + 22), Color.White);
            spriteBatch.DrawString(Fonts.FontinRegular9, "R", new Vector2(Buttons_Skill[6].X + 2, Buttons_Skill[6].Y + 22), Color.White);
            spriteBatch.DrawString(Fonts.FontinRegular9, "T", new Vector2(Buttons_Skill[7].X + 2, Buttons_Skill[7].Y + 22), Color.White);
            spriteBatch.End();
            DrawInfoSkill();
        }
        void DrawInfoSkill() {
            Skill.Rotation += 0.1f;
            if (e.MouseMove_Control != null && e.MouseMove_Control.Skill != null) {
                e.MouseMove_Control.Skill.DrawInfo();
            }
        }
        private void DrawInventory() {
            if (Character_Inventory_Invisible == true) { 
                e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied);
                e.SpriteBatch.Draw(Images.Player.Image_UpperBackground, new Vector2(GameSetting.Width - 482, -8), Images.Player.Rect_UpperBackground, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
                Images.Player.DrawInventoryBorder(GameSetting.Width - 482, 352);
                e.SpriteBatch.DrawString(Fonts.FontinRegular12, "Gold: " + e.Player.Gold.ToString(), new Vector2(780, 320), Color.White);
                e.SpriteBatch.End();
                Main_Inventory.Draw();
                FullSet_Inventory.Controls.Draw();
            }

            if (Main_Shop.Visible == true) {
                e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied, e.GraphicsDevice.SamplerStates.PointWrap);
                e.SpriteBatch.Draw(Images.Player.Image_PanelShop, new Vector2(0, 61), Images.Player.Rect_PanelShop, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
                e.SpriteBatch.Draw(Images.Player.Image_PanelTitleBar, Vector2.Zero, Images.Player.Rect_PanelTitleBar, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
                e.SpriteBatch.DrawString(Fonts.FontinBold15, "SHOP", new Vector2(216, 34), Color.White);
                e.SpriteBatch.DrawString(Fonts.FontinRegular12, "Refresh in " + ((int)Refresh_Shop_Time_Remaining).ToString() + "s", new Vector2(190, 64), Color.White);
                e.SpriteBatch.End();
                Main_Shop.Draw();
            }

            
        }
        #endregion
    }
}
