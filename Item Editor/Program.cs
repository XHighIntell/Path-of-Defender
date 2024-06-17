using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Path_of_Defender {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Form1 x = new Form1();
            //Application.Run(x);

            Main_Form main = new Main_Form();
            main.Run();
            //Application.Run(new Form1());
        }
    }
}
