using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using SharpDX;
using Microsoft.VisualBasic;
using SharpDX.Toolkit.Graphics;


namespace Path_of_Defender {
    using Drawing = Images.Items.Drawing;

    public partial class Item {
        public static Color Background_Color = new Color(0, 0, 0, 200);
        public static Color Color_Chest_Background = Color.Blue * 0.25f;
        public static float Scale = 0.5f;

        public Vector2 GetHeaderSize() {
            Vector2 Size = Vector2.Zero;
            // phần header có margin left right = 23 => tất cả là 46
            if (Data.Caption == null) { Size = Fonts.FontinRegular12.MeasureString(""); Size.X += 46; }
            else { Size = Fonts.FontinRegular12.MeasureString(Data.Caption); Size.X += 46; }
            
            if (Strings.InStr(Data.Caption, Constants.vbCrLf) == 0) { Size.Y = 27; }
            else { Size.Y = 44; }

            return Size;
        }
        public Vector2 GetBodySize() {
            Vector2 Size = Vector2.Zero;

            string Total_Text = ""; 
            string TMP_String;
            bool Need_Separator = false; ;

            TMP_String = Data.GetBasicInfo();
            if (TMP_String != "") { Total_Text = Constants.vbCrLf + TMP_String; Need_Separator = true; }

            TMP_String = Data.GetRequires();
            if (TMP_String != "") {
                if (Need_Separator == true) { Size.Y += 6; } Need_Separator = true;
                Total_Text += Constants.vbCrLf + TMP_String;
            }

            if (Data.Default_Mod.Type != ModType.None) {
                if (Need_Separator == true) { Size.Y += 6; } Need_Separator = true;
                Total_Text += Constants.vbCrLf + Data.Default_Mod.ToString(); 
            }

            TMP_String = Data.GetProperties();
            if (TMP_String != "") {
                if (Need_Separator == true) { Size.Y += 6; } Need_Separator = true;
                Total_Text += Constants.vbCrLf + TMP_String;            
            }
            if (Data.Description != "" && Data.Description != null) {
                if (Need_Separator == true) { Size.Y += 6; } Need_Separator = true;
                Total_Text += Constants.vbCrLf + Data.Description; 
            }

            if (IsBelongShop == true) {
                TMP_String = Data.GetCost();
                if (TMP_String != "") {
                    if (Need_Separator == true) { Size.Y += 6; } Need_Separator = true;
                    Total_Text += Constants.vbCrLf + TMP_String; 
                }
            }
            Total_Text = Strings.Mid(Total_Text, 3);
            Size = Vector2.Add(Size, Fonts.FontinRegular11.MeasureString(Total_Text));
            Size.Y += 2; Size.X += 20;
            return Size;
        }


        public void DrawInfo(int X, int Y) { 
            Vector2 Header_Size = GetHeaderSize();
            Vector2 Body_Size = GetBodySize();
            Vector2 Total_Size = new Vector2(Math.Max(Header_Size.X, Body_Size.X), Header_Size.Y + Body_Size.Y);
            RectangleF Total_Rect = new RectangleF(X, Y, (int)Total_Size.X, (int)Total_Size.Y);
            Vector2 Current_Position = new Vector2(Total_Rect.X + Total_Rect.Width / 2, Total_Rect.Y);
            //Draw Backround
            e.SpriteBatch.Draw(Images.VirtualControl.ImageWhitePoint, new RectangleF(Total_Rect.X, Total_Rect.Y + Header_Size.Y, Total_Rect.Width, Body_Size.Y), Background_Color);
            //Draw Header
            DrawHeader(Header_Size.Y, Total_Rect, ref Current_Position);
            //Draw Body
            DrawBody(Total_Rect, Current_Position);
            return;
        }  
        public void DrawInfo(Rectangle rectangle_Item) {
            Vector2 Header_Size = GetHeaderSize();
            Vector2 Body_Size = GetBodySize();
            Point Total_Size = new Point((int)Math.Max(Header_Size.X, Body_Size.X), (int)(Header_Size.Y + Body_Size.Y));
            RectangleF Total_Rect = new RectangleF(0, 0, Total_Size.X, Total_Size.Y);

            if (rectangle_Item.Y >= Total_Size.Y) {
                Total_Rect.X = (int)(rectangle_Item.X + (rectangle_Item.Width - Total_Rect.Width) / 2);
                Total_Rect.Y = (int)(rectangle_Item.Y - Total_Size.Y);
            } else {
                Total_Rect.Y = 0;
                if (rectangle_Item.X >= Total_Size.X) { Total_Rect.X = rectangle_Item.X - Total_Size.X; }
                else { Total_Rect.X = rectangle_Item.X + rectangle_Item.Width; }
            }
            
            if (Total_Rect.X < 0) { Total_Rect.X = 0; }
            if (Total_Rect.X + Total_Rect.Width > GameSetting.Width) { Total_Rect.X = GameSetting.Width - Total_Rect.Width; }
            if (Total_Rect.Y < 25) { Total_Rect.Y = 25; }

            Vector2 Current_Position = new Vector2((int)(Total_Rect.X + Total_Rect.Width / 2), Total_Rect.Y);

            //Draw Backround
            e.SpriteBatch.Draw(Images.VirtualControl.ImageWhitePoint, new RectangleF(Total_Rect.X, Total_Rect.Y + Header_Size.Y, Total_Rect.Width, Body_Size.Y), Background_Color);
            //Draw Header
            DrawHeader(Header_Size.Y, Total_Rect, ref Current_Position);

            DrawBody(Total_Rect, Current_Position);
            return;
        }
        public void Draw(int X, int Y) { 
            if (Data.Type == TypeItem.Flask) {
                e.SpriteBatch.Draw(Image,
                    new Vector2(X, Y + 78 - 55 + 50 * (1 - Data.Charges / Data.Capacity)),
                    new Rectangle(156, (int)(46 + 100 * (1 - Data.Charges / Data.Capacity)), 78, (int)(102 * (Data.Charges / Data.Capacity))), Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
                e.SpriteBatch.Draw(Image, new RectangleF(X, Y, Data.Width * 39, Data.Height * 39), new Rectangle(0, 0, 78, 156), Color.White);
            } else {
                e.SpriteBatch.Draw(Image, new RectangleF(X, Y, Data.Width * 39, Data.Height * 39), Color.White);
            }

            if (Data.Type == TypeItem.Currency) {
                e.SpriteBatch.DrawString(Fonts.FontinRegular10, Data.Stack.ToString(), new Vector2(X + 4, Y), Color.White);
            }
        }
        public void Draw(int X, int Y, Color colorBackground) {
            e.SpriteBatch.Draw(Images.VirtualControl.ImageWhitePoint, new Vector2(X, Y), new Rectangle(0, 0, Data.Width * 39, Data.Height * 39), colorBackground);
            Draw(X, Y);
        }

        /// <summary> Draw item in center of Rectangle. </summary>
        public void DrawOnChest(Rectangle destinationRectangle) {
            e.SpriteBatch.Draw(Images.VirtualControl.ImageWhitePoint, destinationRectangle, destinationRectangle, Color_Chest_Background);
            Point pos = new Point(destinationRectangle.X + (destinationRectangle.Width - Data.Width * 39) / 2, destinationRectangle.Y + (destinationRectangle.Height - Data.Height * 39) / 2);
            Draw(pos.X, pos.Y);
        }
        /// <summary> Start to draw item at X, Y. </summary>
        public void DrawOnChest(int X, int Y) { Draw(X, Y, Color_Chest_Background); }
        /// <summary> Start to draw item at (Point.X + 39 * SlotX, Point.Y + 39 * SlotY). </summary>
        public void DrawOnChest(Point point) {
            e.SpriteBatch.Draw(Images.VirtualControl.ImageWhitePoint, new Vector2(point.X + 39 * SlotX, point.Y + 39 * SlotY),
                new Rectangle(0, 0, Data.Width * 39, Data.Height * 39), Color.Blue *0.25f);
            Draw(point.X + 39 * SlotX, point.Y + 39 * SlotY);
        }

        public void DrawOnCursor() {
            Draw(e.X - Data.Width * 20, e.Y - Data.Height * 20);
        }

        #region Draw Header, Body, Separator and Description
        public void DrawHeader(float Header_Height, RectangleF Total_Rect, ref Vector2 Current_Position) { 
            Color color = Color.White;
            Rectangle Left_Rect = Rectangle.Empty, Mid_Rect = Rectangle.Empty, Right_Rect = Rectangle.Empty;

            if (Data.Type == TypeItem.Currency) {
                color = Drawing.Color_Currency;
                Left_Rect = Drawing.HeaderCurrencyLeft_Rect;
                Mid_Rect = Drawing.HeaderCurrencyMid_Rect;
                Right_Rect = Drawing.HeaderCurrencyRight_Rect;
            } else if (Data.Level == LevelItem.Normal) {
                color = Drawing.Color_White;
                Left_Rect = Drawing.HeaderWhiteLeft_Rect;
                Mid_Rect = Drawing.HeaderWhiteMid_Rect;
                Right_Rect = Drawing.HeaderWhiteRight_Rect;
            } else if (Data.Level == LevelItem.Magic) {
                color = Drawing.Color_Magic;
                Left_Rect = Drawing.HeaderMagicLeft_Rect;
                Mid_Rect = Drawing.HeaderMagicMid_Rect;
                Right_Rect = Drawing.HeaderMagicRight_Rect;
            } else if (Data.Level == LevelItem.Rare) {
                color = Drawing.Color_Rare;
                if (Strings.InStr(Data.Caption, Constants.vbCrLf) == 0) {
                    Left_Rect = Drawing.HeaderRareLeft_Rect;
                    Mid_Rect = Drawing.HeaderRareMid_Rect;
                    Right_Rect = Drawing.HeaderRareRight_Rect;
                } else {
                    Left_Rect = Drawing.HeaderRareLeft2_Rect;
                    Mid_Rect = Drawing.HeaderRareMid2_Rect;
                    Right_Rect = Drawing.HeaderRareRight2_Rect;
                }
            } else if (Data.Level == LevelItem.Unique) {
                color = Drawing.Color_Unique;
                if (Strings.InStr(Data.Caption, Constants.vbCrLf) == 0) {
                    Left_Rect = Drawing.HeaderUniqueLeft_Rect;
                    Mid_Rect = Drawing.HeaderUniqueMid_Rect;
                    Right_Rect = Drawing.HeaderUniqueRight_Rect;
                } else {
                    Left_Rect = Drawing.HeaderUniqueLeft2_Rect;
                    Mid_Rect = Drawing.HeaderUniqueMid2_Rect;
                    Right_Rect = Drawing.HeaderUniqueRight2_Rect;
                }
            }
            Total_Rect.X = (int)Total_Rect.X;
            Total_Rect.Y = (int)Total_Rect.Y;
            Total_Rect.Width = (int)Total_Rect.Width;
            Total_Rect.Height = (int)Total_Rect.Height;

            e.SpriteBatch.Draw(Images.Image7, new Vector2(Total_Rect.X, Total_Rect.Y), Left_Rect, Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
            e.SpriteBatch.Draw(Images.Image7, new RectangleF(Total_Rect.X + Left_Rect.Width / 2, Total_Rect.Y, Total_Rect.Width - Left_Rect.Width, Mid_Rect.Height / 2), Mid_Rect, Color.White);
            e.SpriteBatch.Draw(Images.Image7, new Vector2(Total_Rect.X + Total_Rect.Width - Right_Rect.Width /2, Total_Rect.Y), Right_Rect, Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);

            if (Data.Caption != null) {
                e.SpriteBatch.DrawStringCenter(Fonts.FontinRegular12, Data.Caption, new RectangleF(Total_Rect.X, Total_Rect.Y, Total_Rect.Width, Header_Height), color, 0, 1, SpriteEffects.None);
            }

            Current_Position.Y += Header_Height;
        }
        private void DrawBody(RectangleF Total_Rect, Vector2 Current_Position) {
            bool Need_Separator = false; // Cái này dùng đế nhửng hàm vẻ sao biết cần để vẻ DrawSeparator()

            if (Data.Type == TypeItem.Currency) {
                DrawNextLine(Fonts.FontinRegular11, "Stack Size: ", Drawing.Color_Dark_White, Data.Stack.ToString() + "/" + Data.Stack_Max.ToString(), Color.White, Total_Rect, ref Current_Position);
                Need_Separator = true;
            } else if (Data.Type == TypeItem.Dagger || Data.Type == TypeItem.Bow || Data.Type == TypeItem.Wand) {
                DrawNextLine(Fonts.FontinRegular11, Data.Type.ToString(), Drawing.Color_Dark_White, ref Current_Position);
                DrawNextLine(Fonts.FontinRegular11, "Physical Damage: ", Drawing.Color_Dark_White, Data.Physical_Damage[0].ToString() + " - " + Data.Physical_Damage[1].ToString(), Color.White, Total_Rect, ref Current_Position);
                DrawNextLine(Fonts.FontinRegular11, "Critical Strike Chance: ", Drawing.Color_Dark_White, Math.Round(Data.Critical_Strike_Chance * 100).ToString() + "%", Color.White, Total_Rect, ref Current_Position);
                DrawNextLine(Fonts.FontinRegular11, "Attacks per Second: ", Drawing.Color_Dark_White, Math.Round(Data.Attacks_per_Second, 2).ToString(), Color.White, Total_Rect, ref Current_Position);
                Need_Separator = true;
            } else if (Data.Type == TypeItem.Belt || Data.Type == TypeItem.Body_Armour || Data.Type == TypeItem.Boot || Data.Type == TypeItem.Glove || Data.Type == TypeItem.Helmet || Data.Type == TypeItem.Shield) {
                if (Data.Armor > 0) { DrawNextLine(Fonts.FontinRegular11, "Armor: ", Drawing.Color_Dark_White, Data.Armor.ToString(), Color.White, Total_Rect, ref Current_Position); Need_Separator = true; }
                if (Data.Evasion > 0) { DrawNextLine(Fonts.FontinRegular11, "Evasion: ", Drawing.Color_Dark_White, Data.Evasion.ToString(), Color.White, Total_Rect, ref Current_Position); Need_Separator = true; }
                if (Data.Energy_Shield > 0) { DrawNextLine(Fonts.FontinRegular11, "Energy Shield: ", Drawing.Color_Dark_White, Data.Energy_Shield.ToString(), Color.White, Total_Rect, ref Current_Position); Need_Separator = true; }
            } else if (Data.Type == TypeItem.Flask) {
                DrawNextLine(Fonts.FontinRegular11, new string[] { "Lasts ", Data.Time.ToString(), " Seconds" }, new Color[] { Drawing.Color_Dark_White, Color.White, Drawing.Color_Dark_White }, Total_Rect, ref Current_Position);
                if (Data.Life > 0) { DrawNextLine(Fonts.FontinRegular11, new string[] { "Recovers ", Data.Life.ToString(), " Life"}, new Color[] { Drawing.Color_Dark_White, Color.White, Drawing.Color_Dark_White }, Total_Rect, ref Current_Position); }
                if (Data.Mana > 0) { DrawNextLine(Fonts.FontinRegular11, new string[] { "Recovers ", Data.Mana.ToString(), " Mana"}, new Color[] { Drawing.Color_Dark_White, Color.White, Drawing.Color_Dark_White }, Total_Rect, ref Current_Position); }

                //if (Data.Life > 0) {
                //    DrawNextLine(Fonts.FontinRegular11, new string[] { "Recovers ", Data.Life.ToString(), " Life Over ", Data.Time.ToString(), " Seconds" }, new Color[] { Drawing.Color_Dark_White, Color.White, Drawing.Color_Dark_White, Color.White, Drawing.Color_Dark_White }, Total_Rect, ref Current_Position);
                //}
                //if (Data.Mana > 0) {
                //    DrawNextLine(Fonts.FontinRegular11, new string[] { "Recovers ", Data.Mana.ToString(), " Mana Over ", Data.Time.ToString(), " Seconds" }, new Color[] { Drawing.Color_Dark_White, Color.White, Drawing.Color_Dark_White, Color.White, Drawing.Color_Dark_White }, Total_Rect, ref Current_Position);
                //}
                DrawNextLine(Fonts.FontinRegular11, new string[] { "Consumes ", Data.Usage.ToString(), " of ", Data.Capacity.ToString(), " Charges on use" }, new Color[] { Drawing.Color_Dark_White, Color.White, Drawing.Color_Dark_White, Color.White, Drawing.Color_Dark_White }, Total_Rect, ref Current_Position);
                DrawNextLine(Fonts.FontinRegular11, new string[] { "Currently Has ", Data.Charges.ToString(), " Charges" }, new Color[] { Drawing.Color_Dark_White, Color.White, Drawing.Color_Dark_White }, Total_Rect, ref Current_Position);
                Need_Separator = true;
            }
           

            if (Data.Type == TypeItem.Currency) { } 
            else if (Data.Requires_Level > 0 || Data.Requires_Str > 0 || Data.Requires_Dex > 0 || Data.Requires_Int > 0) {
                if (Need_Separator == true) { DrawSeparator(ref Current_Position); } Need_Separator = true;
                DrawNextLine(Fonts.FontinRegular11, Data.GetRequires(), Drawing.Color_Dark_White, ref Current_Position);
            }

            if (Data.Default_Mod.Type != ModType.None) {
                if (Need_Separator == true) { DrawSeparator(ref Current_Position); } Need_Separator = true;
                DrawNextLine(Fonts.FontinRegular11, Data.Default_Mod.ToString(), Drawing.Color_Magic, ref Current_Position);
            }

            if (Data.Mods.Length != 0) {
                if (Need_Separator == true) { DrawSeparator(ref Current_Position); } Need_Separator = true;
                for (int i = 0; i < Data.Mods.Length; i++) {
                    DrawNextLine(Fonts.FontinRegular11, Data.Mods[i].ToString(), Drawing.Color_Magic, ref Current_Position);
                }
            }

            DrawDescription(ref Current_Position, Need_Separator);

            if (IsBelongShop == true) {
                string TMP_String = Data.GetCost();
                if (TMP_String != "") {
                    DrawSeparator(ref Current_Position);
                    DrawNextMultiLine(Fonts.FontinRegular11, TMP_String, Drawing.Color_Currency, ref Current_Position);
                }
            }
        }
        public void DrawSeparator(ref Vector2 Current_Position) {
            Current_Position.Y += 3;
            Texture2D Separator = Drawing.SeparatorWhite; Rectangle Separator_Rect = Rectangle.Empty;
            
            if (Data.Type == TypeItem.Currency) { 
                Separator = Drawing.SeparatorCurrency; Separator_Rect = Drawing.SeparatorCurrency_Rect; 
            } else {
                if (Data.Level == LevelItem.Normal) { Separator = Drawing.SeparatorWhite; Separator_Rect = Drawing.SeparatorWhite_Rect; }
                else if (Data.Level == LevelItem.Magic) { Separator = Drawing.SeparatorMagic; Separator_Rect = Drawing.SeparatorMagic_Rect; }
                else if (Data.Level == LevelItem.Rare) { Separator = Drawing.SeparatorRare; Separator_Rect = Drawing.SeparatorRare_Rect; }
                else if (Data.Level == LevelItem.Unique) { Separator = Drawing.SeparatorUnique; Separator_Rect = Drawing.SeparatorUnique_Rect; }
            }
            
            e.SpriteBatch.Draw(Separator, Current_Position, Separator_Rect, Color.White, 0, Drawing.Separator_Origin, 0.5f, SpriteEffects.None, 0);

            Current_Position.Y += 3;
            //
        }
        public void DrawDescription(ref Vector2 Current_Position, bool Need_Separator) {
            Color color;
            if (Data.Description != "" && Data.Description != null) {
                if (Need_Separator == true) { DrawSeparator(ref Current_Position); } Need_Separator = true;

                if (Data.Type == TypeItem.Currency || Data.Type == TypeItem.Flask) { color = Drawing.Color_Dark_White; }
                else { color = Drawing.Color_Unique; }
                DrawNextMultiLine(Fonts.FontinItalic11, Data.Description, color, ref Current_Position); 
            }
        }

        #endregion
        #region Functions to Draw
        /// <summary> Vẻ phần Header của Item </summary>
        /// <param name="Total_Rect"> </param>
        /// <param name="Current_Position"> Là vị trí chính giữa. </param>
        public void DrawNextLine(SpriteFont spriteFont, string text, Color color, ref Vector2 Current_Position) {
            Vector2 Vector2_String = spriteFont.MeasureString(text);
            e.SpriteBatch.DrawString(spriteFont, text, new Vector2(Current_Position.X, Current_Position.Y + Vector2_String.Y / 2), color, 0, Vector2_String / 2, 1, SpriteEffects.None, 0);
            Current_Position.Y += Vector2_String.Y;
        }
        public void DrawNextLine(SpriteFont spriteFont, string text1, Color color1, string text2, Color color2, RectangleF Total_Rect, ref Vector2 Current_Position) {
            Vector2 Vector2_String1 = spriteFont.MeasureString(text1);
            Vector2 Vector2_String2 = spriteFont.MeasureString(text2);

            Vector2 now = new Vector2(Total_Rect.X, Current_Position.Y);

            now.X += (Total_Rect.Width - Vector2_String1.X - Vector2_String2.X) / 2;
            e.SpriteBatch.DrawString(spriteFont, text1, new Vector2(now.X, now.Y + Vector2_String1.Y / 2), color1, 0, new Vector2(0, Vector2_String1.Y / 2), 1, SpriteEffects.None, 0);

            now.X += Vector2_String1.X;
            e.SpriteBatch.DrawString(spriteFont, text2, new Vector2(now.X, now.Y + Vector2_String2.Y / 2), color2, 0, new Vector2(0, Vector2_String2.Y / 2), 1, SpriteEffects.None, 0);

            Current_Position.Y += Vector2_String1.Y;
        }
        public void DrawNextLine(SpriteFont spriteFont, string[] text, Color[] color, RectangleF Total_Rect, ref Vector2 Current_Position) {
            Vector2[] Vector2_String = new Vector2[text.Length];
            float Margin_Left = Total_Rect.Width;
            for (int i = 0; i < text.Length; i++) {
                Vector2_String[i] = spriteFont.MeasureString(text[i]);
                Margin_Left -= Vector2_String[i].X;
            }

            Vector2 now = new Vector2(Total_Rect.X + Margin_Left / 2, Current_Position.Y);
            e.SpriteBatch.DrawString(spriteFont, text[0], new Vector2(now.X, now.Y + Vector2_String[0].Y / 2), color[0], 0, new Vector2(0, Vector2_String[0].Y / 2), 1, SpriteEffects.None, 0);

            for (int i = 1; i < text.Length; i++) {
                now.X += Vector2_String[i - 1].X;
                e.SpriteBatch.DrawString(spriteFont, text[i], new Vector2(now.X, now.Y + Vector2_String[i].Y / 2), color[i], 0, new Vector2(0, Vector2_String[i].Y / 2), 1, SpriteEffects.None, 0);
            }
            Current_Position.Y += Vector2_String[0].Y;
        }
        public void DrawNextMultiLine(SpriteFont spriteFont, string text, Color color, ref Vector2 Current_Position) {
            string[] lines = Strings.Split(text, Constants.vbCrLf);
            for (int i = 0; i < lines.Length; i++) {
                DrawNextLine(spriteFont, lines[i], color, ref Current_Position);
            }
            
        }
        #endregion
    }
    public partial class Item {
        public static Item Selected_Item;
        public static Item Hover_Item;
        public static Rectangle Rectangle_Item = Rectangle.Empty;

        public static void Static_Draw() {
            if (Selected_Item != null) {
                e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied, e.GraphicsDevice.SamplerStates.AnisotropicWrap);
                Selected_Item.DrawOnCursor();
                e.SpriteBatch.End();
            }
            //&& Hover_Item.Parent.IsHover == true
            if (Hover_Item != null) {
                e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied, e.GraphicsDevice.SamplerStates.PointWrap);
                Hover_Item.DrawInfo(Rectangle_Item);
                e.SpriteBatch.End();
            }

        }
    }
}
