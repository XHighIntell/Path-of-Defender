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
    using PSS = Images.UI.PassiveSkillScreen;
    public partial class MainMenu {
        public bool IsLoadedTopPlayers;

        private void Controls_Click(object sender, EventArgs E) {
            if (sender == Button_NewGame) { e.Main.Start(); }
        }        
        
        
        public void Update() {

            if (IsLoadedTopPlayers == false) {
                //We was going to develop this game as mini game. but 
                //Client.Headers.Clear(); Client.Headers.Add("Action-Type", "GET_POD");
                //Client.DownloadStringAsync(new Uri("http://xhighintell.com/game/PathOfDefender.aspx"));
                IsLoadedTopPlayers = true;
            }
            Controls.Update();
        }
        public void Draw() {
            Controls.Draw();
            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied);
            e.SpriteBatch.DrawString(Fonts.FontinRegular13, (e.MouseEnter_Control == Button_NewGame).ToString(), new Vector2(200, 100), Color.White);
            e.SpriteBatch.End();
        }
        public void Client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e) {
            if (e.Error != null) { Status_Control.Status = StatusControl.WaitStatus.Error; return; }
            Status_Control.Visible = false; Main_ListView.Visible = true;

            Main_ListView.Columns.Clear();
            Main_ListView.Items.Clear();
            if (Strings.InStr(e.Result, "TOP!") == 1) { 
                string Result = Strings.Mid( e.Result, 5);
                string[] Rows = Strings.Split(Result, "?");
                string[] Items;

                
                Items = Strings.Split(Rows[0], "|");
                for (int j = 0; j < Items.Length; j++) {
                    if (j == 0) { Main_ListView.Columns.Add(Items[j], 50, HorizontalAlignment.Center, Color.White); }
                    else if (j == 1) { Main_ListView.Columns.Add(Items[j], 200, HorizontalAlignment.Center, new Color(0, 255, 0)); }
                    else if (j == 2) { Main_ListView.Columns.Add(Items[j], 50, HorizontalAlignment.Center, Color.Yellow); }
                    else { Main_ListView.Columns.Add(Items[j], 50, HorizontalAlignment.Center, Color.White); }
                    
                }

                for (int i = 1; i < Rows.Length; i++) {
                    Items = Strings.Split(Rows[i], "|");
                    if (i % 2 == 0) { Main_ListView.Items.Add(new VirtualListViewItem(Fonts.FontinRegular10, Color.White, Items)); }
                    else { Main_ListView.Items.Add(new VirtualListViewItem(Fonts.FontinRegular10, new Color(0, 255, 0), Items)); }
                    
                }
            }
        }
    }

    public partial class MainEndMenu {
        public static float Base_Fade_Time = 2;

        public float Time;
        public void Update() {
            Time += GameSetting.SecondPerFrame;
            Controls.Update();
        }

        Vector2 GameOver_Position = new Vector2(GameSetting.Width / 2, GameSetting.Height / 2);
        float Opacity;
        public void Draw() {
            e.SpriteBatch.Begin(SpriteSortMode.Deferred, e.GraphicsDevice.BlendStates.NonPremultiplied);
            
            if (Time <= Base_Fade_Time) {
                Opacity = Time / Base_Fade_Time;
                GameOver_Position.Y = GameSetting.Height / 2;
            } else if (2 < Time && Time <= 4) {
                Opacity = 1;
                GameOver_Position.Y = GameSetting.Height / 2 - 250 * (Time - 2) / 2;
            } else if (Text_Username.Visible == false) {
                Text_Username.Visible = true;
                Button_Post.Visible = true;
            }

            Vector2 Size_String = Fonts.FontinBold50.MeasureString("Game Over");
            e.SpriteBatch.DrawString(Fonts.FontinBold50, "Game Over", GameOver_Position, Color.White * Opacity, 0, Size_String / 2, 1, SpriteEffects.None, 1);
            e.SpriteBatch.DrawString(Fonts.FontinBold50, "Game Over", GameOver_Position + 4, Color.Black * Opacity, 0, Size_String / 2, 1f, SpriteEffects.None, 0);
            e.SpriteBatch.End();

            Controls.Draw();
        }

        private void Button_Post_Click(object sender, EventArgs E) {
            if (Status_Control.Status != StatusControl.WaitStatus.Wait) {
                Text_Username.Text = Strings.Replace(Strings.Replace(Text_Username.Text, "?", ""), "|", "");
                Client.Headers.Clear(); Client.Headers.Add("Action-Type", "POST_POD_1");
                Client.UploadStringAsync(new Uri("http://xhighintell.com/game/PathOfDefender.aspx"), "POST", 
                    Text_Username.Text + "|" + (e.Main.GameLevels.StageIndex + 1) + "|" + e.Player.KilledMonster);
                Status_Control.Visible = true;
                Status_Control.Status = StatusControl.WaitStatus.Wait;
                Button_Post.Visible = false;
            }
        }
        public void Client_UploadStringCompleted(object sender, UploadStringCompletedEventArgs E) {
            if (E.Error == null) {
                e.Menu.IsLoadedTopPlayers = false;
                Status_Control.Status = StatusControl.WaitStatus.Ok;
            } else{
                Status_Control.Status = StatusControl.WaitStatus.Error;
                Button_Post.Visible = true;
            }
        }
    }





}
