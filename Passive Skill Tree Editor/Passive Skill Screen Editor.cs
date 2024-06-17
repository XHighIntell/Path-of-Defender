using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Graphics;
using SWF = System.Windows.Forms;
namespace PassiveSkillScreen_Editor {
    using PSS = Images.UI.PassiveSkillScreen;
    public static class GameSetting {
        public static float SecondPerFrame = (float)1 / 60;
        public static Random RND = new Random();
        public static int Width = 1250;
        public static int Height = 690;
        public static bool ProcChance(float chance) {
            if (chance > RND.NextDouble()) { return true; }
            else { return false; }
        }
    }

    partial class Main_Edit :Game {
        private GraphicsDeviceManager deviceManager;
        

        //1st
        public Main_Edit() {
            deviceManager = new GraphicsDeviceManager(this);
            deviceManager.PreferredBackBufferWidth = GameSetting.Width;
            deviceManager.PreferredBackBufferHeight = GameSetting.Height;
        }

        //2nd
        protected override void Initialize() {
            base.Initialize(); this.Window.IsMouseVisible = true;
            e.Create(this);
            //PassiveTree
            //if (System.Diagnostics.Debugger.IsAttached == true) {
            //    Images.Load(GraphicsDevice, e.SpriteBatch, @"E:\My Life\Projects\C# - Path of Defender\Effects\Graphics");
            //    Fonts.Load(GraphicsDevice, @"E:\My Life\Projects\C# - Path of Defender\Effects\Graphics");
            //} else {
                Images.Load(GraphicsDevice, e.SpriteBatch, System.Windows.Forms.Application.StartupPath + "\\Graphics");
                Fonts.Load(GraphicsDevice, System.Windows.Forms.Application.StartupPath + "\\Graphics");
            //}

            PassiveTree = new PassiveTree();
            PassiveTree.Visible = true;
            PassiveTree.Width = GameSetting.Width;
            PassiveTree.Height = GameSetting.Height;
            PassiveTree.Button_OK.Visible = false;
            PassiveTree.Button_Cancel.Visible = false;
            PassiveTree.IsEditor = true;
            PassiveTree.MouseWheel +=new SWF.MouseEventHandler(PassiveTree_MouseWheel);

            Controls = new VirtualControlCollection();
            Controls.Add(PassiveTree);
        }


        protected override void LoadContent() {
            PassiveTree.SelectedData = new PassiveTreeData();

            PassiveTree.SelectedData.AddBackground(new Background("Start", new Vector2(0, 0), SpriteEffects.None));

            PassiveTree.SelectedData.AddSkill(new PassiveSkill()); PassiveTree.SelectedData.Skills[0].Status = SkillStatus.Allocated;
            PassiveTree.SelectedData.AddSkill(new PassiveSkill("Plus Intelligence", new Vector2(164, -290), SkillStatus.Unallocated, SkillSize.Small, 0)); //1
            PassiveTree.SelectedData.AddSkill(new PassiveSkill("Plus Intelligence", new Vector2(480, -430), SkillStatus.Unallocated, SkillSize.Small, 1)); //2

            PassiveTree.AddLabel(new Label(new Vector2(0, -131), "0", new Color(0, 156, 255), 0, 1));
            PassiveTree.AddLabel(new Label(new Vector2(-112, 60), "0", new Color(255, 74, 0), 0, 1));
            PassiveTree.AddLabel(new Label(new Vector2(112, 60), "0", new Color(0, 255, 87), 0, 1));

            PassiveTree.SelectedData.AddLine(new Line(1, 2, new Vector2(164, -290), 300, (float)-0.40f, LineStyle.Line));

            PassiveTree.RefreshEditor();
           
            base.LoadContent();
        }

        Vector2 Old_Postion, Hit, Delta_Vector;

        protected override void Update(GameTime gameTime) {
            e.Update(); Controls.Update();
            


            Update2();
            float plusX = 0, plusY = 0; ;
            if (e.Keyboard.IsKeyDown(Keys.Left) == true) { plusX -= 1 / PassiveTree.Scale; }
            if (e.Keyboard.IsKeyDown(Keys.Right) == true) { plusX += 1 / PassiveTree.Scale; }
            if (e.Keyboard.IsKeyDown(Keys.Up) == true) { plusY -= 1 / PassiveTree.Scale; }
            if (e.Keyboard.IsKeyDown(Keys.Down) == true) { plusY += 1 / PassiveTree.Scale; }
            if (Type == SelectType.Skill) { PassiveTree.SelectedData.Skills[Index].Position.X += plusX; PassiveTree.SelectedData.Skills[Index].Position.Y += plusY; }
            else if (Type == SelectType.Line) { PassiveTree.SelectedData.Lines[Index].Position.X += plusX; PassiveTree.SelectedData.Lines[Index].Position.Y += plusY; }
            else if (Type == SelectType.Background) { PassiveTree.SelectedData.Backgrounds[Index].Position.X += plusX; PassiveTree.SelectedData.Backgrounds[Index].Position.Y += plusY; }
            else if (Type == SelectType.Label) { PassiveTree.SelectedData.Labels[Index].Position.X += plusX; PassiveTree.SelectedData.Labels[Index].Position.Y += plusY; }
            base.Update(gameTime);
        }



        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);
            PassiveTree.DrawEditor(Check_ShowNumber.Checked, Check_Skill.Checked, Check_Line.Checked, Check_Label.Checked, Check_Background.Checked);
            Draw2();
            base.Draw(gameTime);
        }

        private void Draw2() {
            Vector2 TMP_Vector2 = new Vector2();
            Vector2 TMP_Vector1 = new Vector2();
            if (e.Keyboard.IsKeyDown(Keys.LeftControl) == true && Type != SelectType.None) {
                e.SpriteBatch.Begin(SpriteSortMode.Deferred, GraphicsDevice.BlendStates.NonPremultiplied);

                

                if (Index_Equal_X != -1) {
                    if (Type == SelectType.Skill) { TMP_Vector2 = PassiveTree.SelectedData.Skills[Index_Equal_X].Position; }
                    else if (Type == SelectType.Background) { TMP_Vector2 = PassiveTree.SelectedData.Backgrounds[Index_Equal_X].Position; }
                    else if (Type == SelectType.Label) { TMP_Vector2 = PassiveTree.SelectedData.Labels[Index_Equal_X].Position; }
                    else if (Type == SelectType.Line) { TMP_Vector2 = PassiveTree.SelectedData.Lines[Index_Equal_X].Position; }

                    TMP_Vector2 = TMP_Vector2 * PassiveTree.Scale - PassiveTree.Position;
                    e.SpriteBatch.DrawLineH(TMP_Vector2.X, TMP_Vector2.Y - 1000, 2000, Color.White);
                }

                if (Index_Equal_Y != -1) {
                    if (Type == SelectType.Skill) { TMP_Vector2 = PassiveTree.SelectedData.Skills[Index_Equal_Y].Position; }
                    else if (Type == SelectType.Background) { TMP_Vector2 = PassiveTree.SelectedData.Backgrounds[Index_Equal_Y].Position; }
                    else if (Type == SelectType.Label) { TMP_Vector2 = PassiveTree.SelectedData.Labels[Index_Equal_Y].Position; }
                    else if (Type == SelectType.Line) { TMP_Vector2 = PassiveTree.SelectedData.Lines[Index_Equal_Y].Position; }

                    TMP_Vector2 = TMP_Vector2 * PassiveTree.Scale - PassiveTree.Position;
                    e.SpriteBatch.DrawLineW(TMP_Vector2.X - 1000, TMP_Vector2.Y, 2000, Color.White);
                }

                if (Index_Symmetry_X != -1) {
                    if (Type == SelectType.Skill) { TMP_Vector1 = PassiveTree.SelectedData.Skills[Index].Position; TMP_Vector2 = PassiveTree.SelectedData.Skills[Index_Symmetry_X].Position; }
                    else if (Type == SelectType.Background) { TMP_Vector1 = PassiveTree.SelectedData.Backgrounds[Index].Position; TMP_Vector2 = PassiveTree.SelectedData.Backgrounds[Index_Symmetry_X].Position; }
                    else if (Type == SelectType.Label) { TMP_Vector1 = PassiveTree.SelectedData.Labels[Index].Position; TMP_Vector2 = PassiveTree.SelectedData.Labels[Index_Symmetry_X].Position; }
                    else if (Type == SelectType.Line) { TMP_Vector1 = PassiveTree.SelectedData.Lines[Index].Position; TMP_Vector2 = PassiveTree.SelectedData.Lines[Index_Symmetry_X].Position; }

                    TMP_Vector1 = TMP_Vector1 * PassiveTree.Scale - PassiveTree.Position;
                    TMP_Vector2 = TMP_Vector2 * PassiveTree.Scale - PassiveTree.Position;

                    e.SpriteBatch.DrawLineH((TMP_Vector1.X + TMP_Vector2.X) / 2, TMP_Vector1.Y - 1000, 2000, Color.White);
                    //Draw plus sign at:
                    e.SpriteBatch.DrawLineH(TMP_Vector1.X, TMP_Vector1.Y - 50, 100, Color.White);
                    e.SpriteBatch.DrawLineH(TMP_Vector2.X, TMP_Vector1.Y - 50, 100, Color.White);

                    e.SpriteBatch.DrawLineW(TMP_Vector1.X - 50, TMP_Vector1.Y, 100, Color.White);
                    e.SpriteBatch.DrawLineW(TMP_Vector2.X - 50, TMP_Vector1.Y, 100, Color.White);
                }

                if (Index_Symmetry_Y != -1) {
                    if (Type == SelectType.Skill) { TMP_Vector1 = PassiveTree.SelectedData.Skills[Index].Position; TMP_Vector2 = PassiveTree.SelectedData.Skills[Index_Symmetry_Y].Position; }
                    else if (Type == SelectType.Background) { TMP_Vector1 = PassiveTree.SelectedData.Backgrounds[Index].Position; TMP_Vector2 = PassiveTree.SelectedData.Backgrounds[Index_Symmetry_Y].Position; }
                    else if (Type == SelectType.Label) { TMP_Vector1 = PassiveTree.SelectedData.Labels[Index].Position; TMP_Vector2 = PassiveTree.SelectedData.Labels[Index_Symmetry_Y].Position; }
                    else if (Type == SelectType.Line) { TMP_Vector1 = PassiveTree.SelectedData.Lines[Index].Position; TMP_Vector2 = PassiveTree.SelectedData.Lines[Index_Symmetry_Y].Position; }

                    TMP_Vector1 = TMP_Vector1 * PassiveTree.Scale - PassiveTree.Position;
                    TMP_Vector2 = TMP_Vector2 * PassiveTree.Scale - PassiveTree.Position;
                    
                    e.SpriteBatch.DrawLineW(TMP_Vector1.X - 1000, (TMP_Vector1.Y + TMP_Vector2.Y) / 2, 2000, Color.White);
                    //Draw plus sign at:
                    e.SpriteBatch.DrawLineH(TMP_Vector1.X, TMP_Vector1.Y - 50, 100, Color.White);
                    e.SpriteBatch.DrawLineH(TMP_Vector1.X, TMP_Vector2.Y - 50, 100, Color.White);

                    e.SpriteBatch.DrawLineW(TMP_Vector1.X - 50, TMP_Vector1.Y, 100, Color.White);
                    e.SpriteBatch.DrawLineW(TMP_Vector1.X - 50, TMP_Vector2.Y, 100, Color.White);
                }

                e.SpriteBatch.End();
            }
        }
    }



}
