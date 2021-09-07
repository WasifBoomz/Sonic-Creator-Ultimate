using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Diagnostics;
namespace Sonic_Creator_Ultimate
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://github.com/WasifBoomz/Sonic-Creator-Ultimate/releases/latest/download/release.zip");
            request.Method = "HEAD";
            HttpWebResponse res = (HttpWebResponse)request.GetResponse();
            DateTime localProgram = File.GetLastWriteTime(Application.ExecutablePath);
            DateTime onlineProgram = res.LastModified;
            if (localProgram < onlineProgram)
            {
                DialogResult dialogResult = MessageBox.Show("Your version is from: " + localProgram.ToString() + "\nThe new version is from: " + onlineProgram.ToString(), "New Update Available!", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Process.Start(Application.StartupPath + "/Updater.exe");
                    Application.Exit();
                    return;
                }
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
