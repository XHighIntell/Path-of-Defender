using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Graphics;

namespace PassiveSkillScreen_Editor {
    class e {
        public static Game This;
        public static GraphicsDevice GraphicsDevice;
        public static SpriteBatch SpriteBatch;
        public static System.Windows.Forms.Form Form;

        public static MouseManager MouseManager;
        public static KeyboardManager KeyboardManager;

        public static MouseState Mouse;
        public static KeyboardState Keyboard;
        public static float XF, YF; public static int X, Y;
        public static Point Mouse_Position; public static Vector2 Mouse_PositionF;

        public static VirtualControl MouseDown_Control, MouseMove_Control, MouseEnter_Control, Focus_Control;




        public static void Create(Game game) {
            This = game;
            e.Form = (System.Windows.Forms.Form)game.Window.NativeWindow;
            e.GraphicsDevice = This.GraphicsDevice;
            e.SpriteBatch = new SpriteBatch(This.GraphicsDevice);
            MouseManager = new MouseManager(This);
            KeyboardManager = new KeyboardManager(This);
        }
        public static void Update() {
            e.Mouse = MouseManager.GetState(); e.Keyboard = KeyboardManager.GetState();

            e.XF = e.Mouse.X * GameSetting.Width; e.YF = e.Mouse.Y * GameSetting.Height;
            e.X = (int)Math.Round(e.XF); e.Y = (int)Math.Round(e.YF);

            e.Mouse_PositionF.X = e.X; e.Mouse_PositionF.Y = e.Y;
            e.Mouse_Position.X = e.X; e.Mouse_Position.Y = e.Y;
        }
    }
}
