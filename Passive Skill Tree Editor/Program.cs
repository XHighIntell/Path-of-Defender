using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace PassiveSkillScreen_Editor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Main_Edit x = new Main_Edit();
            x.Run();
        }
    }

    public static class Images {
        internal static GraphicsDevice Device;
        internal static SpriteBatch SpriteBatch;
        public static Texture2D Image1, Image2, Image3, Image4, Image5, Image6, Image7;
        internal static string MainPath;
        public static void Load(GraphicsDevice device, SpriteBatch spriteBatch, string mainPath) { //Attention Needed
            Device = device; SpriteBatch = spriteBatch; MainPath = mainPath;
            Image1 = Texture2D.Load(Device, MainPath + @"\Interface\2DArt_UIImages_1.dds");
            Image2 = Texture2D.Load(Device, MainPath + @"\Interface\2DArt_UIImages_2.dds");
            Image3 = Texture2D.Load(Device, MainPath + @"\Interface\2DArt_UIImages_3.dds");
            Image4 = Texture2D.Load(Device, MainPath + @"\Interface\2DArt_UIImages_4.dds");
            Image5 = Texture2D.Load(Device, MainPath + @"\Interface\2DArt_UIImages_5.dds");
            Image6 = Texture2D.Load(Device, MainPath + @"\Interface\2DArt_UIImages_6.dds");
            Image7 = Texture2D.Load(Device, MainPath + @"\Interface\2DArt_UIImages_7.dds");

            UI.Load();
            VirtualControl.Load();
        }

        public static class UI {
            public static void Load() {
                PassiveSkillScreen.Load();
            }
            public static class PassiveSkillScreen {
                public static void Load() {
                    Background.Load();
                    ScreenBackGround.Load();
                    Skills.Load();
                    Connect.Load();
                    PopupBackground.Load();
                    Select.Load();
                    Buttons.Load();
                }
                public static class Background {
                    public static RenderTarget2D Image;
                    public static Rectangle Rect = new Rectangle(1473, 89, 272, 272);
                    static Background() {
                        Image = RenderTarget2D.New(Device, 272, 272, PixelFormat.B8G8R8A8.UNorm);
                        Device.SetRenderTargets(Image);
                        Device.Clear(Color.Transparent);

                        SpriteBatch.Begin(SpriteSortMode.Deferred, Device.BlendStates.NonPremultiplied);
                        SpriteBatch.Draw(Image6, Vector2.Zero, new Rectangle(1473, 89, 272, 272), Color.White);
                        SpriteBatch.End();
                    }
                    public static void Load() { }
                }
                public static class ScreenBackGround {
                    public static string[] Names = { };
                    public static Texture2D[] Images = { };
                    public static Rectangle[] Rects = { };
                    public static Vector2[] Origins = { };
                    private static void ResizeAll(int size) {
                        Array.Resize(ref Names, size);
                        Array.Resize(ref Images, size); Array.Resize(ref Rects, size); Array.Resize(ref Origins, size);
                    }

                    public static void Load() {
                        ResizeAll(5);

                        Names[0] = "Error";
                        Images[0] = Image7;
                        Rects[0] = new Rectangle(14, 1218, 38, 38);
                        Origins[0] = new Vector2(18, 18);

                        Names[1] = "Start";
                        Images[1] = Image2;
                        Rects[1] = new Rectangle(1276, 1158, 628, 578);
                        Origins[1] = new Vector2(314, 289);

                        Names[2] = "Group Small";
                        Images[2] = Image3;
                        Rects[2] = new Rectangle(1346, 997, 359, 359);
                        Origins[2] = new Vector2(179, 179);

                        Names[3] = "Group Medium";
                        Images[3] = Image4;
                        Rects[3] = new Rectangle(1060, 1063, 465, 465);
                        Origins[3] = new Vector2(232, 232);

                        Names[4] = "Group Large Half";
                        Images[4] = Image1;
                        Rects[4] = new Rectangle(979, 732, 807, 403);
                        Origins[4] = new Vector2(403, 201);

                        string[] files = Directory.GetFiles(MainPath + @"\Background");
                        for (int i = 0; i < files.Length; i++) {
                            ResizeAll(Names.Length + 1);
                            Names[Names.Length - 1] = Path.GetFileNameWithoutExtension(files[i]);
                            Images[Images.Length - 1] = Texture2D.Load(Device, files[i]);
                            Rects[Rects.Length - 1] = new Rectangle(0, 0, Images[Images.Length - 1].Width, Images[Images.Length - 1].Height);
                            Origins[Origins.Length - 1] = new Vector2(Images[Images.Length - 1].Width / 2, Images[Images.Length - 1].Height / 2);
                        }

                    }

                    public static Texture2D GetImage(string name)
                    {
                        int i = Array.IndexOf(Names, name);
                        if (i != -1) { return Images[i]; }
                        else { return null; }
                    }
                    public static int GetIndex(string name){
                        int i = Array.IndexOf(Names, name);
                        if (i == -1) { return 0; }
                        return i;
                    }
                }

                //biggest 217x221
                public static class Keystone {
                    public static Texture2D Image;
                    public static Rectangle NormalRect = new Rectangle(1551, 468, 217, 221);
                    public static Rectangle CanAllocateRect = new Rectangle(1812, 323, 217, 221);
                    public static Rectangle ActiveRect = new Rectangle(1754, 89, 217, 221);
                    public static Vector2 Origin = new Vector2(108, 110);
                    static Keystone() { Image = Image6; }
                }

                //medium 151x152
                public static class NotableFrame { 
                    public static Texture2D NormalImage;
                    public static Texture2D CanAllocateImage;
                    public static Texture2D ActiveImage;

                    public static Rectangle NormalRect = new Rectangle(648, 1456, 151, 152);
                    public static Rectangle CanAllocateRect = new Rectangle(1882, 1716, 151, 152);
                    public static Rectangle ActiveRect = new Rectangle(1856, 1781, 151, 152);
                    public static Vector2 Origin = new Vector2(75, 75);

                    static NotableFrame() { NormalImage = Image5; CanAllocateImage = Image4; ActiveImage = Image5; }
                }

                //small 102x102
                public static class PassiveFrame { 
                    public static Texture2D Image;

                    public static Rectangle NormalRect = new Rectangle(1914, 222, 102, 102);
                    public static Rectangle CanAllocateRect = new Rectangle(1914, 333, 102, 102);
                    public static Rectangle ActiveRect = new Rectangle(1914, 444, 102, 102);
                    public static Vector2 Origin = new Vector2(50, 50);

                    static PassiveFrame() { Image = Image3; }
                }

                //small 102x102
                public static class PlusFrame { 
                    public static Texture2D NormalImage;
                    public static Texture2D CanAllocateImage;
                    public static Texture2D ActiveImage;

                    public static Rectangle NormalRect = new Rectangle(1915, 0, 102, 102);
                    public static Rectangle CanAllocateRect = new Rectangle(1914, 111, 102, 102);
                    public static Rectangle ActiveRect = new Rectangle(1914, 1624, 102, 102);
                    public static Vector2 Origin = new Vector2(50, 50);
                    
                    static PlusFrame() { NormalImage = Image3; CanAllocateImage = Image3; ActiveImage = Image2; }
                }

                public static class Connect {
                    public class ConnectBase {
                        public RenderTarget2D NormalImage, CanAllocateImage, ActiveImage;
                        public Rectangle Rect;
                        public Vector2 Origin;
                    }
                    public static ConnectBase Line = new ConnectBase();
                    public static ConnectBase TinyCircle = new ConnectBase();
                    public static ConnectBase SmallCircle = new ConnectBase();
                    public static ConnectBase MediumCircle = new ConnectBase();
                    public static ConnectBase LargeCircle = new ConnectBase();

                    public static void Load() {
                        LoadLine();
                        LoadTinyCircle(); LoadSmallCircle(); LoadMediumCircle(); LoadLargeCircle();
                    }
                    private static void LoadLine() { 
                        Line.Rect = new Rectangle(0, 0, 64, 43);
                        Line.Origin = new Vector2(0, 21);

                        //Create 64x43
                        Line.NormalImage = RenderTarget2D.New(Device, 64, 43, PixelFormat.B8G8R8A8.UNorm);
                        Line.CanAllocateImage = RenderTarget2D.New(Device, 64, 43, PixelFormat.B8G8R8A8.UNorm);
                        Line.ActiveImage = RenderTarget2D.New(Device, 64, 43, PixelFormat.B8G8R8A8.UNorm);

                        //Draw
                        Device.SetRenderTargets(Line.NormalImage); Device.Clear(Color.Transparent);
                        SpriteBatch.Begin(SpriteSortMode.Deferred, Device.BlendStates.NonPremultiplied);
                        SpriteBatch.Draw(Image4, Vector2.Zero, new Rectangle(1954, 1411, 64, 43), Color.White);
                        SpriteBatch.End();

                        Device.SetRenderTargets(Line.CanAllocateImage); Device.Clear(Color.Transparent);
                        SpriteBatch.Begin(SpriteSortMode.Deferred, Device.BlendStates.NonPremultiplied);
                        SpriteBatch.Draw(Image5, Vector2.Zero, new Rectangle(1952, 464, 64, 43), Color.White);
                        SpriteBatch.End();

                        Device.SetRenderTargets(Line.ActiveImage); Device.Clear(Color.Transparent);
                        SpriteBatch.Begin(SpriteSortMode.Deferred, Device.BlendStates.NonPremultiplied);
                        SpriteBatch.Draw(Image7, Vector2.Zero, new Rectangle(1030, 1252, 64, 43), Color.White);
                        SpriteBatch.End();
                    }
                    private static void LoadTinyCircle() {
                        //Image4
                        //Normal  new Rectangle(1321+409, 535+410, 111, 110)
                        //Can  new Rectangle(1+409, 1020+410     , 111, 110)
                        //Active  new Rectangle(531+409, 1020+410, 111, 110)
                        TinyCircle.Rect = new Rectangle(0, 0, 111, 110);
                        TinyCircle.Origin = new Vector2(20, 110);

                        //Create 111x110
                        TinyCircle.NormalImage = RenderTarget2D.New(Device, 111, 110, PixelFormat.B8G8R8A8.UNorm);
                        TinyCircle.CanAllocateImage = RenderTarget2D.New(Device, 111, 110, PixelFormat.B8G8R8A8.UNorm);
                        TinyCircle.ActiveImage = RenderTarget2D.New(Device, 111, 110, PixelFormat.B8G8R8A8.UNorm);

                        //Draw
                        Device.SetRenderTargets(TinyCircle.NormalImage); Device.Clear(Color.Transparent);
                        SpriteBatch.Begin(SpriteSortMode.Deferred, Device.BlendStates.NonPremultiplied, Device.SamplerStates.PointWrap);
                        SpriteBatch.Draw(Image4, new Vector2(0, 9), new Rectangle(1321 + 409, 535 + 410 + 9, 111, 110), Color.White);
                        SpriteBatch.Draw(Image4, new Vector2(9, 0), new Rectangle(1321 + 409 + 9, 535 + 410, 111, 110), Color.White);
                        SpriteBatch.End();

                        Device.SetRenderTargets(TinyCircle.CanAllocateImage); Device.Clear(Color.Transparent);
                        SpriteBatch.Begin(SpriteSortMode.Deferred, Device.BlendStates.NonPremultiplied, Device.SamplerStates.PointWrap);
                        SpriteBatch.Draw(Image4, new Vector2(0, 9), new Rectangle(1 + 409, 1020 + 410 + 9, 111, 110), Color.White);
                        SpriteBatch.Draw(Image4, new Vector2(9, 0), new Rectangle(1 + 409 + 9, 1020 + 410, 111, 110), Color.White);
                        SpriteBatch.End();

                        Device.SetRenderTargets(TinyCircle.ActiveImage); Device.Clear(Color.Transparent);
                        SpriteBatch.Begin(SpriteSortMode.Deferred, Device.BlendStates.NonPremultiplied, Device.SamplerStates.PointWrap);
                        SpriteBatch.Draw(Image4, new Vector2(0, 9), new Rectangle(531 + 409, 1020 + 410 + 9, 111, 110), Color.White);
                        SpriteBatch.Draw(Image4, new Vector2(9, 0), new Rectangle(531 + 409 + 9, 1020 + 410, 111, 110), Color.White);
                        SpriteBatch.End();
                    }
                    private static void LoadSmallCircle() {
                        //Image4
                        //Normal  new Rectangle(1321+329, 535+329   , 191, 191)
                        //Can  new Rectangle(      1+329, 1020+329  , 191, 191)
                        //Active  new Rectangle( 531+329,  1020+329 , 191, 191)
                        SmallCircle.Rect = new Rectangle(0, 0, 191, 191);
                        SmallCircle.Origin = new Vector2(20, 191);

                        //Create 191x191
                        SmallCircle.NormalImage = RenderTarget2D.New(Device, 191, 191, PixelFormat.B8G8R8A8.UNorm);
                        SmallCircle.CanAllocateImage = RenderTarget2D.New(Device, 191, 191, PixelFormat.B8G8R8A8.UNorm);
                        SmallCircle.ActiveImage = RenderTarget2D.New(Device, 191, 191, PixelFormat.B8G8R8A8.UNorm);

                        //Draw
                        Device.SetRenderTargets(SmallCircle.NormalImage); Device.Clear(Color.Transparent);
                        SpriteBatch.Begin(SpriteSortMode.Deferred, Device.BlendStates.NonPremultiplied, Device.SamplerStates.PointWrap);
                        SpriteBatch.Draw(Image4, Vector2.Zero, new Rectangle(1321 + 329, 535 + 329, 191, 80), Color.White);
                        SpriteBatch.Draw(Image4, Vector2.Zero, new Rectangle(1321 + 329, 535 + 329, 80, 191), Color.White);
                        SpriteBatch.Draw(Image4, Vector2.Zero, new Rectangle(1321 + 329, 535 + 329, 90, 90), Color.White);
                        SpriteBatch.End();

                        Device.SetRenderTargets(SmallCircle.CanAllocateImage); Device.Clear(Color.Transparent);
                        SpriteBatch.Begin(SpriteSortMode.Deferred, Device.BlendStates.NonPremultiplied, Device.SamplerStates.PointWrap);
                        SpriteBatch.Draw(Image4, Vector2.Zero, new Rectangle(1 + 329, 1020 + 329, 191, 80), Color.White);
                        SpriteBatch.Draw(Image4, Vector2.Zero, new Rectangle(1 + 329, 1020 + 329, 80, 191), Color.White);
                        SpriteBatch.Draw(Image4, Vector2.Zero, new Rectangle(1 + 329, 1020 + 329, 90, 90), Color.White);
                        SpriteBatch.End();

                        Device.SetRenderTargets(SmallCircle.ActiveImage); Device.Clear(Color.Transparent);
                        SpriteBatch.Begin(SpriteSortMode.Deferred, Device.BlendStates.NonPremultiplied, Device.SamplerStates.PointWrap);
                        SpriteBatch.Draw(Image4, Vector2.Zero, new Rectangle(531 + 329, 1020 + 329, 191, 80), Color.White);
                        SpriteBatch.Draw(Image4, Vector2.Zero, new Rectangle(531 + 329, 1020 + 329, 80, 191), Color.White);
                        SpriteBatch.Draw(Image4, Vector2.Zero, new Rectangle(531 + 329, 1020 + 329, 90, 90), Color.White);
                        SpriteBatch.End();
                    }
                    private static void LoadMediumCircle() {
                        //Image4
                        //Normal  new Rectangle(1321+158, 535+156   , 191, 191)
                        //Can  new Rectangle(      1+158, 1020+156  , 191, 191)
                        //Active  new Rectangle( 531+158,  1020+156 , 191, 191)
                        MediumCircle.Rect = new Rectangle(0, 0, 362, 364);
                        MediumCircle.Origin = new Vector2(20, 364);

                        //Create 362x364
                        MediumCircle.NormalImage = RenderTarget2D.New(Device, 362, 364, PixelFormat.B8G8R8A8.UNorm);
                        MediumCircle.CanAllocateImage = RenderTarget2D.New(Device, 362, 364, PixelFormat.B8G8R8A8.UNorm);
                        MediumCircle.ActiveImage = RenderTarget2D.New(Device, 362, 364, PixelFormat.B8G8R8A8.UNorm);

                        //Draw
                        Device.SetRenderTargets(MediumCircle.NormalImage); Device.Clear(Color.Transparent);
                        SpriteBatch.Begin(SpriteSortMode.Deferred, Device.BlendStates.NonPremultiplied, Device.SamplerStates.PointWrap);
                        SpriteBatch.Draw(Image4, new Vector2(55,0), new Rectangle(1321 + 158 + 55, 535 + 156, 362, 173), Color.White);
                        SpriteBatch.Draw(Image4, new Vector2(0, 100), new Rectangle(1321 + 158, 535 + 156 + 100, 120, 364), Color.White);
                        SpriteBatch.End();

                        Device.SetRenderTargets(MediumCircle.CanAllocateImage); Device.Clear(Color.Transparent);
                        SpriteBatch.Begin(SpriteSortMode.Deferred, Device.BlendStates.NonPremultiplied, Device.SamplerStates.PointWrap);
                        SpriteBatch.Draw(Image4, new Vector2(55, 0), new Rectangle(1 + 158 + 55, 1020 + 156, 362, 173), Color.White);
                        SpriteBatch.Draw(Image4, new Vector2(0, 100), new Rectangle(1 + 158, 1020 + 156 + 100, 120, 364), Color.White);
                        SpriteBatch.End();

                        Device.SetRenderTargets(MediumCircle.ActiveImage); Device.Clear(Color.Transparent);
                        SpriteBatch.Begin(SpriteSortMode.Deferred, Device.BlendStates.NonPremultiplied, Device.SamplerStates.PointWrap);
                        SpriteBatch.Draw(Image4, new Vector2(55, 0), new Rectangle(531 + 158 + 55, 1020 + 156, 362, 173), Color.White);
                        SpriteBatch.Draw(Image4, new Vector2(0, 100), new Rectangle(531 + 158, 1020 + 156 + 100, 120, 364), Color.White);
                        SpriteBatch.End();
                    }
                    private static void LoadLargeCircle() { 
                        //Image4
                        //Normal  new Rectangle(1321, 535, 520, 520)
                        //Can  new Rectangle(1, 1020, 520, 520)
                        //Active  new Rectangle(531, 1020, 520, 520)
                        LargeCircle.Rect = new Rectangle(0, 0, 520, 520);
                        LargeCircle.Origin = new Vector2(21, 520);

                        //Create 520x520
                        LargeCircle.NormalImage = RenderTarget2D.New(Device, 520, 520, PixelFormat.B8G8R8A8.UNorm);
                        LargeCircle.CanAllocateImage = RenderTarget2D.New(Device, 520, 520, PixelFormat.B8G8R8A8.UNorm);
                        LargeCircle.ActiveImage = RenderTarget2D.New(Device, 520, 520, PixelFormat.B8G8R8A8.UNorm);

                        //Draw
                        Device.SetRenderTargets(LargeCircle.NormalImage); Device.Clear(Color.Transparent);
                        SpriteBatch.Begin(SpriteSortMode.Deferred, Device.BlendStates.NonPremultiplied, Device.SamplerStates.PointWrap);
                        SpriteBatch.Draw(Image4, Vector2.Zero, new Rectangle(1321, 535, 520, 150), Color.White);
                        SpriteBatch.Draw(Image4, Vector2.Zero, new Rectangle(1321, 535, 150, 520), Color.White);
                        SpriteBatch.Draw(Image4, Vector2.Zero, new Rectangle(1321, 535, 250, 250), Color.White);
                        SpriteBatch.End();

                        Device.SetRenderTargets(LargeCircle.CanAllocateImage); Device.Clear(Color.Transparent);
                        SpriteBatch.Begin(SpriteSortMode.Deferred, Device.BlendStates.NonPremultiplied, Device.SamplerStates.PointWrap);
                        SpriteBatch.Draw(Image4, Vector2.Zero, new Rectangle(1, 1020, 520, 150), Color.White);
                        SpriteBatch.Draw(Image4, Vector2.Zero, new Rectangle(1, 1020, 150, 520), Color.White);
                        SpriteBatch.Draw(Image4, Vector2.Zero, new Rectangle(1, 1020, 250, 250), Color.White);
                        SpriteBatch.End();

                        Device.SetRenderTargets(LargeCircle.ActiveImage); Device.Clear(Color.Transparent);
                        SpriteBatch.Begin(SpriteSortMode.Deferred, Device.BlendStates.NonPremultiplied, Device.SamplerStates.PointWrap);
                        SpriteBatch.Draw(Image4, Vector2.Zero, new Rectangle(531, 1020, 520, 150), Color.White);
                        SpriteBatch.Draw(Image4, Vector2.Zero, new Rectangle(531, 1020, 150, 520), Color.White);
                        SpriteBatch.Draw(Image4, Vector2.Zero, new Rectangle(531, 1020, 250, 250), Color.White);
                        SpriteBatch.End();
                    }


                }
                public static class Skills {
                    public static string[] Names = { };
                    public static Texture2D[] Images = { };
                    //public static Vector2 Origin = new Vector2(64, 64);
                    public static Rectangle Rect = new Rectangle(0, 0, 128, 128);
                    private static void ResizeAll(int size) {
                        Array.Resize(ref Names, size); Array.Resize(ref Images, size);
                    }
                    public static void Load() {
                        ResizeAll(Names.Length + 1);
                        Names[Names.Length - 1] = Path.GetFileNameWithoutExtension(MainPath + @"\Other\Error.dds");
                        Images[Images.Length - 1] = Texture2D.Load(Device, MainPath + @"\Other\Error.dds");

                        string[] files = Directory.GetFiles(MainPath + @"\Passive");
                        for (int i = 0; i < files.Length; i++) {
                            ResizeAll(Names.Length + 1);
                            Names[Names.Length - 1] = Path.GetFileNameWithoutExtension(files[i]);
                            Images[Images.Length - 1] = Texture2D.Load(Device, files[i]);
                        }

                        files = Directory.GetFiles(MainPath + @"\Passive\Skills");
                        for (int i = 0; i < files.Length; i++) {
                            ResizeAll(Names.Length + 1);
                            Names[Names.Length - 1] = Path.GetFileNameWithoutExtension(files[i]);
                            Images[Images.Length - 1] = Texture2D.Load(Device, files[i]);
                        }

                    }
                    public static Texture2D GetImage(string name) {
                        int i = Array.IndexOf(Names, name);
                        if (i != -1) { return Images[i]; }
                        else { return GetImage("Error"); }
                    }

                }

                public static class PopupBackground {
                    public static RenderTarget2D Image;
                    public static Rectangle Rect = new Rectangle(0, 0, 10, 10);
                    public static void Load() {
                        Image = RenderTarget2D.New(Device, 10, 10, PixelFormat.B8G8R8A8.UNorm);
                        Device.SetRenderTargets(Image);
                        Device.Clear(Color.Black);
                    }
                }
                public static class PointsBackground {
                    public static Texture2D Image = Image3;
                    public static Rectangle Rect = new Rectangle(1678, 1936, 313, 48);
                    public static Vector2 Origin = new Vector2(156, 24);
                }
                public static class Buttons {
                    public static Button DisableButton = new Button(Image7, new Rectangle(913, 1459, 40, 56), new Rectangle(158, 1437, 48, 56), new Rectangle(1856, 1469, 40, 56));
                    public static Button NormalButton = new Button(Image7, new Rectangle(1591, 1462, 40, 56), new Rectangle(764, 1442, 48, 56), new Rectangle(1664, 1469, 40, 56));
                    public static Button ActiveButton = new Button(Image7, new Rectangle(1431, 1436, 40, 56), new Rectangle(1964, 1448, 48, 56), new Rectangle(1712, 1469, 40, 56));

                    public static int Width = 176;
                    public static int Height = 56;

                    public static void Load() { 
                        
                    }

                    public class Button {
                        public Texture2D BaseImage;
                        public Rectangle RectLeft;
                        public Rectangle RectMid;
                        public Rectangle RectRight;
                        public RenderTarget2D Image;
                        public Rectangle Rect = new Rectangle(0, 0, 176, 56);
                        public void Draw(string text ,int x, int y, float scale) {
                            SpriteBatch.Draw(Image, new Vector2(x, y), Rect, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                            Vector2 stringSize = Fonts.FontinRegular13.MeasureString(text);
                            SpriteBatch.DrawString(Fonts.FontinRegular13, text, new Vector2((176 * scale - stringSize.X) / 2 + x, (56 * scale - stringSize.Y) / 2 + y), Color.White);
                        }
                        public void Draw(string text ,int x, int y, int width, int height) {
                            Vector2 scale = new Vector2((float)width / Width, (float)height / Height);
                            SpriteBatch.Draw(Image, new Vector2(x, y), Rect, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                            Vector2 stringSize = Fonts.FontinRegular13.MeasureString(text);
                            SpriteBatch.DrawString(Fonts.FontinRegular13, text, new Vector2((176 * scale.X - stringSize.X) / 2 + x, (56 * scale.Y - stringSize.Y) / 2 + y), Color.White);
                        }
                        public Button(Texture2D image, Rectangle rectLeft, Rectangle rectMid, Rectangle rectRight) {
                            BaseImage = image; RectLeft = rectLeft; RectMid = rectMid; RectRight = rectRight;
                            Image = RenderTarget2D.New(Device, RectLeft.Width * 2 + RectMid.Width * 2, RectLeft.Height, PixelFormat.R8G8B8A8.UNorm);
                            Device.SetRenderTargets(Image); Device.Clear(Color.Transparent);
                            SpriteBatch.Begin(SpriteSortMode.Deferred, Device.BlendStates.NonPremultiplied);
                            SpriteBatch.Draw(BaseImage, new Vector2(0, 0), RectLeft, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                            SpriteBatch.Draw(BaseImage, new Vector2(RectLeft.Width, 0), RectMid, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                            SpriteBatch.Draw(BaseImage, new Vector2(RectLeft.Width + RectMid.Width, 0), RectMid, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                            SpriteBatch.Draw(BaseImage, new Vector2((float)(int)(RectLeft.Width + RectMid.Width * 2), 0), RectRight, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                            SpriteBatch.End();
                        }
                    }
                }
                public static class Select {
                    public static RenderTarget2D Image;
                    public static Rectangle Rect = new Rectangle(0, 0, 1, 1);
                    public static void Load()
                    {
                        Image = RenderTarget2D.New(Device, 1, 1, PixelFormat.B8G8R8A8.UNorm);
                        Device.SetRenderTargets(Image);
                        Device.Clear(Color.Red);
                    }
                }
            } //Call Load();
        }
        public static class VirtualControl {
            public static RenderTarget2D ImageWhitePoint;

            public static void Load() {
                ImageWhitePoint = RenderTarget2D.New(Device, 1, 1, PixelFormat.R8G8B8A8.UNorm);
                Device.SetRenderTargets(ImageWhitePoint);
                Device.Clear(Color.White);
            }
        }
    }

    public static class Fonts {
        #region Fonts
        public static SpriteFont FontinRegular9;
        public static SpriteFont FontinRegular10;
        public static SpriteFont FontinRegular11;
        public static SpriteFont FontinRegular12;
        public static SpriteFont FontinRegular13;
        public static SpriteFont FontinRegular14;
        public static SpriteFont FontinRegular15;
        public static SpriteFont FontinRegular20;
        public static SpriteFont FontinRegular25;
        public static SpriteFont FontinRegular30;
        public static SpriteFont FontinRegular50;

        public static SpriteFont FontinItalic9;
        public static SpriteFont FontinItalic10;
        public static SpriteFont FontinItalic11;
        public static SpriteFont FontinItalic12;
        public static SpriteFont FontinItalic13;
        public static SpriteFont FontinItalic14;
        public static SpriteFont FontinItalic15;
        public static SpriteFont FontinItalic20;
        public static SpriteFont FontinItalic25;
        public static SpriteFont FontinItalic30;
        public static SpriteFont FontinItalic50;

        public static SpriteFont FontinBold9;
        public static SpriteFont FontinBold10;
        public static SpriteFont FontinBold11;
        public static SpriteFont FontinBold12;
        public static SpriteFont FontinBold13;
        public static SpriteFont FontinBold14;
        public static SpriteFont FontinBold15;
        public static SpriteFont FontinBold20;
        public static SpriteFont FontinBold25;
        public static SpriteFont FontinBold30;
        public static SpriteFont FontinBold50;
        #endregion

        public static string MainPath;
        public static void Load(GraphicsDevice device, string mainPath) {
            MainPath = mainPath;
            FontinRegular9 = SpriteFont.Load(device, MainPath + @"\Fonts\Fontin Regular 9.font");
            FontinRegular10 = SpriteFont.Load(device, MainPath + @"\Fonts\Fontin Regular 10.font");
            FontinRegular11 = SpriteFont.Load(device, MainPath + @"\Fonts\Fontin Regular 11.font");
            FontinRegular12 = SpriteFont.Load(device, MainPath + @"\Fonts\Fontin Regular 12.font");
            FontinRegular13 = SpriteFont.Load(device, MainPath + @"\Fonts\Fontin Regular 13.font");
            FontinRegular14 = SpriteFont.Load(device, MainPath + @"\Fonts\Fontin Regular 14.font");
            FontinRegular15 = SpriteFont.Load(device, MainPath + @"\Fonts\Fontin Regular 15.font");
            FontinRegular20 = SpriteFont.Load(device, MainPath + @"\Fonts\Fontin Regular 20.font");
            FontinRegular25 = SpriteFont.Load(device, MainPath + @"\Fonts\Fontin Regular 25.font");
            FontinRegular30 = SpriteFont.Load(device, MainPath + @"\Fonts\Fontin Regular 30.font");
            FontinRegular50 = SpriteFont.Load(device, MainPath + @"\Fonts\Fontin Regular 50.font");

            FontinItalic9 = SpriteFont.Load(device, MainPath + @"\Fonts\Fontin Italic 9.font");
            FontinItalic10 = SpriteFont.Load(device, MainPath + @"\Fonts\Fontin Italic 10.font");
            FontinItalic11 = SpriteFont.Load(device, MainPath + @"\Fonts\Fontin Italic 11.font");
            FontinItalic12 = SpriteFont.Load(device, MainPath + @"\Fonts\Fontin Italic 12.font");
            FontinItalic13 = SpriteFont.Load(device, MainPath + @"\Fonts\Fontin Italic 13.font");
            FontinItalic14 = SpriteFont.Load(device, MainPath + @"\Fonts\Fontin Italic 14.font");
            FontinItalic15 = SpriteFont.Load(device, MainPath + @"\Fonts\Fontin Italic 15.font");
            FontinItalic20 = SpriteFont.Load(device, MainPath + @"\Fonts\Fontin Italic 20.font");
            FontinItalic25 = SpriteFont.Load(device, MainPath + @"\Fonts\Fontin Italic 25.font");
            FontinItalic30 = SpriteFont.Load(device, MainPath + @"\Fonts\Fontin Italic 30.font");
            FontinItalic50 = SpriteFont.Load(device, MainPath + @"\Fonts\Fontin Italic 50.font");

            FontinBold9 = SpriteFont.Load(device, MainPath + @"\Fonts\Fontin Bold 9.font");
            FontinBold10 = SpriteFont.Load(device, MainPath + @"\Fonts\Fontin Bold 10.font");
            FontinBold11 = SpriteFont.Load(device, MainPath + @"\Fonts\Fontin Bold 11.font");
            FontinBold12 = SpriteFont.Load(device, MainPath + @"\Fonts\Fontin Bold 12.font");
            FontinBold13 = SpriteFont.Load(device, MainPath + @"\Fonts\Fontin Bold 13.font");
            FontinBold14 = SpriteFont.Load(device, MainPath + @"\Fonts\Fontin Bold 14.font");
            FontinBold15 = SpriteFont.Load(device, MainPath + @"\Fonts\Fontin Bold 15.font");
            FontinBold20 = SpriteFont.Load(device, MainPath + @"\Fonts\Fontin Bold 20.font");
            FontinBold25 = SpriteFont.Load(device, MainPath + @"\Fonts\Fontin Bold 25.font");
            FontinBold30 = SpriteFont.Load(device, MainPath + @"\Fonts\Fontin Bold 30.font");
            FontinBold50 = SpriteFont.Load(device, MainPath + @"\Fonts\Fontin Bold 50.font");
        }
    }

    public struct Frame {
        public string name;//walking/running
        public int pic;
        public int wait;
        public int next;
        public bool IsAttack;

        public Frame(string Name, int Pic, int Wait, int Next) {
            name = Name;
            pic = Pic;
            wait = Wait;
            next = Next;
            IsAttack = false;
        }
        public Frame(string Name, int Pic, int Wait, int Next, bool isAttackFarm) {
            name = Name;
            pic = Pic;
            wait = Wait;
            next = Next;
            IsAttack = isAttackFarm;
        }
    }
}
