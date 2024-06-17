using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Monster_Editor {
    static class Program {
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main_Form());
            //Application.Run(new Units_Editor());
        }
    }

    public static class E {
        public static MapFile Map = new MapFile(new MonsterInfo[0], new Stage[0]);
        public static Main_Form Main_Form;
    }
}
