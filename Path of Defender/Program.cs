using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Graphics;
using System;

namespace Path_of_Defender {
    static class Program {

        static void Main() {
            PathOfDefender x = new PathOfDefender();
            x.Run(); 
        }
    }
    public partial class PathOfDefender {
        private GraphicsDeviceManager deviceManager;
        public PathOfDefender() {
            deviceManager = new GraphicsDeviceManager(this);
            deviceManager.PreferredBackBufferWidth = GameSetting.Width;
            deviceManager.PreferredBackBufferHeight = GameSetting.Height;
        }
        protected override void Initialize() {
            base.Initialize();
            this.Window.IsMouseVisible = true;
            e.Create(this); e.Form.Left = 0; e.Form.Top = 0;
            e.Form.Icon = Properties.Resources.X;
            e.Form.Text = "Path of Defender";
            this.IsFixedTimeStep = true;
            e.Form.Resize += new EventHandler(Form_Resize);
        }
        private void Form_Resize(object sender, EventArgs E) {

        }
    }

    public partial class PathOfDefender : Game {
        protected override void LoadContent() {
            
            Images.Load(GraphicsDevice, e.SpriteBatch, System.Windows.Forms.Application.StartupPath + @"\Graphics");
            Fonts.Load(GraphicsDevice, System.Windows.Forms.Application.StartupPath + @"\Graphics");
            e.SI = new SystemItem(System.Windows.Forms.Application.StartupPath + @"\System\Items.SI");
            

            e.Main = new MainGame();
            e.Menu = new MainMenu();
            e.EndMenu = new MainEndMenu();

           
            //Fonts.CompileAndSave("Fontin", 20, 0, 0, Fonts.FontStyle.Bold);
            //Fonts.CompileAndSave("Fontin", 25, 0, 0, Fonts.FontStyle.Bold);
            //Fonts.CompileAndSave("Fontin", 30, 0, 0, Fonts.FontStyle.Bold);
            base.LoadContent();
            if (enableFPS == true) stopwatch.Start();
        }

        protected override void Update(GameTime gameTime) {
            e.Update();
            if (e.State == GameState.Menu) {
                e.Menu.Update();
            } else if (e.State == GameState.Gameplay) {
                e.Main.Update();
            } else if (e.State == GameState.Gameover) {
                e.EndMenu.Update();
            }
            
            base.Update(gameTime);
        }

        bool enableFPS = true;
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        int fps = 0;
        public static Color Blue = new Color(16, 32, 108);
        protected override void Draw(GameTime gameTime) {

            if (e.State == GameState.Menu) {
                e.GraphicsDevice.Clear(Blue);
                e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied);
                e.SpriteBatch.Draw(Images.Menu.Image, new Vector2(0, 0), Color.White);
                e.SpriteBatch.End();
                e.Menu.Draw();
            } else if (e.State== GameState.Gameover) {
                e.GraphicsDevice.Clear(Color.Black);
                e.Main.Draw();
                e.EndMenu.Draw();
            } else {
                e.GraphicsDevice.Clear(Color.Black);
                e.Main.Draw();
            }
            //Fonts.FontinBold12.MeasureString("
            if (enableFPS == true) {
                fps++;
                if (stopwatch.Elapsed.TotalSeconds >= 1) { stopwatch.Restart(); fps = 0; }

                if (stopwatch.Elapsed.TotalSeconds != 0) {
                    e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied, GraphicsDevice.SamplerStates.LinearMirror);
                    e.SpriteBatch.DrawString(Fonts.FontinRegular20, "FPS: " + Math.Round(fps / stopwatch.Elapsed.TotalSeconds, 1).ToString(), new Vector2(0, 0), Color.Red, 0f, Vector2.Zero, 1, SpriteEffects.None, 0);
                    
                    
                    //Fonts.FontinRegular20.gr
                    //e.SpriteBatch.DrawString(Fonts.FontinRegular20, Math.Round(count_Update / stopwatch.Elapsed.TotalSeconds, 1).ToString(), new Vector2(100, 150), Color.Red);
                    e.SpriteBatch.End();
                }
            }
            
            

            base.Draw(gameTime);
        }


    }
    
}
