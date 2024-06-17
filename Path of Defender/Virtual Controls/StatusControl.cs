using System;
using System.Net;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Input;
using SharpDX.Toolkit.Graphics;
using Microsoft.VisualBasic;

namespace Path_of_Defender {
    public class StatusControl : VirtualControl {
        public WaitStatus Status;
        public enum WaitStatus { None = 0, Wait = 1, Ok = 2, Error = 3 }
        public StatusControl() { 
            
        }
        float rotation;
        public override void Draw() {
            if (Status == WaitStatus.Wait) {
                e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied);
                rotation += 0.1f;
                e.SpriteBatch.Draw(Images.Menu.Image_Wait, new Vector2(X, Y), Images.Menu.Rect_Wait, Color.White, rotation, Images.Menu.Origin_Wait, 0.5f, SpriteEffects.None, 0);
                e.SpriteBatch.End();
            } else if (Status == WaitStatus.Error ) {
                e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied);
                e.SpriteBatch.DrawString(Fonts.FontinItalic11, "No Internet.", new Vector2(X, Y), Color.White, 0, Fonts.FontinItalic11.MeasureString("No Internet.") / 2, 1, SpriteEffects.None, 0);
                e.SpriteBatch.End();
            } else if (Status == WaitStatus.Ok) {
                e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied);
                e.SpriteBatch.DrawString(Fonts.FontinItalic15, "Done", new Vector2(X, Y), Color.White, 0, Fonts.FontinRegular15.MeasureString("Done") / 2, 1, SpriteEffects.None, 0);
                e.SpriteBatch.End();
            }
        }
    }
}
