using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;

namespace Path_of_Defender {
    using PSS = Images.UI.PassiveSkillScreen;
    using System.Windows.Forms;
    public partial class MainMenu {
        public VirtualControlCollection Controls = new VirtualControlCollection();
        VirtualButton Button_NewGame;
        VirtualEdit Edit_A, Edit_B;
        VirtualListView Main_ListView;
        public WebClient Client;
        public StatusControl Status_Control;

        public MainMenu() {
            InitializeComponent();
        }
        private void InitializeComponent() {
            //Button_NewGame
            Button_NewGame = new VirtualButton();
            Button_NewGame.Name = "Button_NewGame";
            Button_NewGame.Visible = true;
            Button_NewGame.Width = PSS.Buttons.Width;
            Button_NewGame.Height = PSS.Buttons.Height;
            Button_NewGame.X = GameSetting.Width / 2 - PSS.Buttons.Width / 2;
            Button_NewGame.Y = 70;
            Button_NewGame.Text = "New Game";
            Button_NewGame.Click +=new EventHandler(Controls_Click);

            //Edit_A
            Edit_A = new VirtualEdit();
            Edit_A.X = 100;
            Edit_A.Y = 100;
            Edit_A.Width = 100;
            Edit_A.Height = 50;
            Edit_A.Visible = true;
            
            //Edit_B
            Edit_B = new VirtualEdit();
            Edit_B.X = 100;
            Edit_B.Y = 200;
            Edit_B.Width = 100;
            Edit_B.Height = 50;
            Edit_B.Visible = true;
            Edit_B.MouseUp += delegate { this.Edit_B.Text += this.Edit_B.Text; };

            Main_ListView = new VirtualListView();
            Main_ListView.X = 600;
            Main_ListView.Y = 400;
            Main_ListView.Width = 500;
            Main_ListView.Height = 250;
            Main_ListView.Visible = false;

            Client = new WebClient();
            Client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(Client_DownloadStringCompleted);

            Status_Control = new StatusControl();
            Status_Control.X = Main_ListView.X + Main_ListView.Width / 2;
            Status_Control.Y = Main_ListView.Y;// +Main_ListView.Height / 2;
            Status_Control.Visible = true;
            Status_Control.Status = StatusControl.WaitStatus.Wait;

            //Form
            Controls.Add(Button_NewGame);
            Controls.Add(Main_ListView);
            Controls.Add(Edit_A); 
            Controls.Add(Edit_B);
            Controls.Add(Status_Control);
        }

    }

    public partial class MainEndMenu {
        public VirtualControlCollection Controls = new VirtualControlCollection();
        VirtualButton Button_NewGame, Button_Post;
        VirtualEdit Text_Username; StatusControl Status_Control;
        WebClient Client;

        public MainEndMenu() {
            InitializeComponent();
        }
        private void InitializeComponent() {
            Client = new WebClient();
            Client.UploadStringCompleted += new UploadStringCompletedEventHandler(Client_UploadStringCompleted);

            Button_NewGame = new VirtualButton();
            Button_NewGame.Name = "Button_NewGame";
            Button_NewGame.Visible = true;
            Button_NewGame.Width = PSS.Buttons.Width;
            Button_NewGame.Height = PSS.Buttons.Height;
            Button_NewGame.X = GameSetting.Width / 2 - PSS.Buttons.Width / 2;
            Button_NewGame.Y = 500;
            Button_NewGame.Text = "Return to Menu";
            Button_NewGame.Click +=new EventHandler(Button_NewGame_Click);


            Text_Username = new VirtualEdit();
            Text_Username.Width = 200;
            Text_Username.Height = 50;
            Text_Username.X = GameSetting.Width / 2 - Text_Username.Width / 2;
            Text_Username.Y = 250;
            Text_Username.MaxLength = 20;
            Text_Username.TextAlign = HorizontalAlignment.Center;

            Button_Post = new VirtualButton();
            Button_Post.Width = PSS.Buttons.Width;
            Button_Post.Height = PSS.Buttons.Height;
            Button_Post.X = GameSetting.Width / 2 - PSS.Buttons.Width / 2;
            Button_Post.Y = 325;
            Button_Post.Text = "Upload Your Score";
            Button_Post.Click += new EventHandler(Button_Post_Click);

            Status_Control = new StatusControl();
            Status_Control.X = GameSetting.Width / 2;
            Status_Control.Y = 350;

            Controls.Add(Button_NewGame);
            Controls.Add(Text_Username);
            Controls.Add(Button_Post);
            Controls.Add(Status_Control);
        }
        private void Button_NewGame_Click(object sender, EventArgs E) {
            e.State = GameState.Menu;
            Time = 0;
            Status_Control.Visible = false;
            Button_Post.Visible = false;
            Text_Username.Visible = false;
        }

        

    }
}
