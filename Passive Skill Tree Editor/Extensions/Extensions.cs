using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit.Graphics;

namespace PassiveSkillScreen_Editor {
    public static class Extensions {
        public static void DrawLineW(this SpriteBatch spriteBatch, float x, float y, float width, Color color) {
            spriteBatch.Draw(Images.VirtualControl.ImageWhitePoint, new RectangleF(x, y, width, 1), color);
        }
        public static void DrawLineH(this SpriteBatch spriteBatch, float x, float y, float height, Color color) {
            spriteBatch.Draw(Images.VirtualControl.ImageWhitePoint, new RectangleF(x, y, 1, height), color);
        }
    }
}
