using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DaS_BRush_Console
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new DaS_BRush.ConsoleWindow());
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show("Dark Souls Scripting Console Requires \"" + e.FileName + "\" to be in the same directory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
