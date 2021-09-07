using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
namespace Updater
{
    class Program
    {
        static void Main(string[] args)
        {
            WebClient client = new WebClient();
            FileInfo f = new FileInfo(Process.GetCurrentProcess().MainModule.FileName);
            DirectoryInfo d = f.Directory;
            if (File.Exists(Process.GetCurrentProcess().MainModule.FileName + ".zip"))
            {
                File.Delete(Process.GetCurrentProcess().MainModule.FileName + ".zip");
            }
            if (Directory.Exists(d.FullName + "/temp"))
            {
                Directory.Delete(d.FullName + "/temp", true);
            }
            client.DownloadFile("https://github.com/WasifBoomz/Sonic-Creator-Ultimate/releases/latest/download/release.zip", Process.GetCurrentProcess().MainModule.FileName + ".zip");
            ZipFile.ExtractToDirectory(Process.GetCurrentProcess().MainModule.FileName + ".zip", d.FullName+"/temp");
            Console.WriteLine("Extracting Update");
            DirectoryInfo d2 = new DirectoryInfo(d.FullName + "/temp");
            Console.WriteLine("Moving Update");
            CopyAll(d2, d);
            Process.Start(d.FullName + "/Sonic Creator Ultimate.exe");
            Environment.Exit(0);
        }
        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            if (source.FullName.ToLower() == target.FullName.ToLower())
            {
                return;
            }

            // Check if the target directory exists, if not, create it.
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it's new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                if (!fi.Name.Contains("Updater"))
                {
                    fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
                }
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}
