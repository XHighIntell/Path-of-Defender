using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Graphics;

namespace Path_of_Defender {
    public static class e {
        public static Game This;
        public static GraphicsDevice GraphicsDevice;
        public static SpriteBatch SpriteBatch;
        public static System.Windows.Forms.Form Form;

        public static ObjectsManager All = new ObjectsManager();

        public static Player Player;

        public static MainMenu Menu;
        public static MainGame Main;
        public static MainEndMenu EndMenu;
        public static GameState State = GameState.Menu;
        public static SystemItem SI;

        public static MouseManager MouseManager;
        public static KeyboardManager KeyboardManager;


        public static MouseState Mouse;
        public static KeyboardState Keyboard;
        public static float XF, YF; public static int X, Y;
        public static Point Mouse_Position; public static Vector2 Mouse_PositionF;

        public static VirtualControl MouseDown_Control, MouseMove_Control, MouseEnter_Control, Focus_Control;

        //public static double TimeRemain;
        //public static int ElapsedFrames;

        public static void Create(Game game) {
            This = game;
            e.Form = (System.Windows.Forms.Form)game.Window.NativeWindow;
            e.GraphicsDevice = This.GraphicsDevice;
            e.SpriteBatch = new SpriteBatch(This.GraphicsDevice);
            MouseManager = new MouseManager(This);
            KeyboardManager = new KeyboardManager(This);
        }
        public static void Update() {
            //GameTime gameTime
            e.Mouse = MouseManager.GetState(); e.Keyboard = KeyboardManager.GetState();

            e.XF = e.Mouse.X * GameSetting.Width;  e.YF = e.Mouse.Y * GameSetting.Height;
            e.X = (int)Math.Round(e.XF); e.Y = (int)Math.Round(e.YF);

            e.Mouse_PositionF.X = e.X; e.Mouse_PositionF.Y = e.Y;
            e.Mouse_Position.X = e.X; e.Mouse_Position.Y = e.Y;

            //ElapsedFrames = (int)(gameTime.ElapsedGameTime.TotalMilliseconds + TimeRemain) / (1000 / 60);
            //TimeRemain = gameTime.ElapsedGameTime.TotalMilliseconds + TimeRemain - ElapsedFrames * 1000 / 60;
        }
    }

    public enum GameState { Menu = 0, Gameplay = 1, Gameover = 3 }

    public static class GameSetting { 
        //public static 
        public static float SecondPerFrame = (float)1 / 60;
        public static Random RND = new Random();
        public static int Width = 1250;
        public static int Height = 690;
        public static bool ProcChance(float chance) {
            if (chance > RND.NextDouble()) { return true; }
            else { return false; }
        }
        //static GameSetting() { RND = new Random(); }
    }
}
