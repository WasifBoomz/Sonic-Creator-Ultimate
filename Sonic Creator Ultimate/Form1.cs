using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using IniParser;
using IniParser.Model;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
namespace Sonic_Creator_Ultimate
{
    public partial class Form1 : Form
    {
        List<Mod> Mods = new List<Mod>();
        List<Process> p = new List<Process>();
        public Form1()
        {
            if (new DirectoryInfo(Application.StartupPath).Name != "SonicColorsUltimate")
            {
                MessageBox.Show("Please place this in the Sonic Colors Ultimate directory!");
                Application.Exit();
            }
            InitializeComponent();
            LoadData();
            RefreshList();
        }
        void RefreshMods()
        {
            Mods.Clear();
            foreach(string dir in Directory.GetDirectories(Application.StartupPath + "/mods"))
            {
                if (File.Exists(dir + "/mod.ini"))
                {
                    List<Mod> mo = new List<Mod>();
                    var parser = new FileIniDataParser();
                    IniData ini = parser.ReadFile(dir + "/mod.ini");
                    Mod m = new Mod();

                    if (ini.TryGetKey("Description", out string re1))
                    {
                        if (re1.Length > 20)
                        {
                            re1 = re1.Substring(0, 20);
                        }
                        m.Description = re1;
                    }
                    if (ini.TryGetKey("Author", out string re2))
                    {
                        if (re2.Length > 15)
                        {
                            re2 = re2.Substring(0, 15);
                        }
                        m.Creator = re2;
                    }
                    if (ini.TryGetKey("Version", out string re3))
                    {
                        if (re3.Length > 15)
                        {
                            re3 = re3.Substring(0, 15);
                        }
                        m.Version = re3;
                    }
                    if (ini.TryGetKey("Name", out string re4))
                    {
                        if (re4.Length > 15)
                        {
                            re4 = re4.Substring(0, 15);
                        }
                        m.Name = re4;
                        m.Path = dir;
                        Mods.Add(m);
                    }
                }
            }
            SaveData();
            RefreshList();
        }
        void RefreshList()
        {
            checkedListBox1.Items.Clear();
            List<string> mod = new List<string>();
            for (int i = 0; i < Mods.Count; i++)
            {
                mod.Add(Mods[i].Name.PadRight(15) + "     " + Mods[i].Description.PadRight(20) + "     " + Mods[i].Version.PadRight(15) + "     " + Mods[i].Creator.PadRight(15));
                checkedListBox1.Items.Insert(i, mod[i]);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < Mods.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, Mods[i].Checked);
            }
            if (!Directory.Exists(Application.StartupPath + "/mods"))
            {
                MessageBox.Show("Mods folder does not exist! Creating folder!");
                Directory.CreateDirectory(Application.StartupPath + "/mods");
            }
            if (Directory.Exists(Application.StartupPath + "/content/acorn/Backup"))
            {
                pictureBox3.Image = Sonic_Creator_Ultimate.Properties.Resources.MenuRestore;
            }
            else
            {
                MessageBox.Show("Backup folder does not exist inside \"SonicColorsUltimate/content/acorn\" folder! Creating one now! Please note that the program will take a little while to show up.");
                pictureBox3.Image = Sonic_Creator_Ultimate.Properties.Resources.MenuBackup;
                Backup();
            }
            System.Threading.Thread.Sleep(1000);
            Bitmap c = new Bitmap(pictureBox6.Image);
            Bitmap n = new Bitmap(pictureBox6.Image);
            for (int i = 0; i < 1024; i++)
            {
                for (int k = 0; k < 1024; k++)
                {
                    n.SetPixel(i, k, c.GetPixel(i, ((k - 512) * -1) + 511));
                }
            }
            pictureBox6.Image = n;
        }
        void Backup()
        {
            if (Directory.Exists(Application.StartupPath + "/content/acorn/Backup"))
            {
                foreach (string f in Directory.GetFiles(Application.StartupPath + "/content/acorn/Backup"))
                {
                    FileInfo file = new FileInfo(f);
                    File.Delete(Application.StartupPath + "/content/acorn/" + file.Name);
                    file.MoveTo(Application.StartupPath + "/content/acorn/" + file.Name);
                }
                Directory.Delete(Application.StartupPath + "/content/acorn/Backup");
                pictureBox3.Image = Sonic_Creator_Ultimate.Properties.Resources.MenuBackup;
            }
            else
            {
                Directory.CreateDirectory(Application.StartupPath + "/content/acorn/Backup");
                for (int i = 0; i <= 7; i++)
                {
                    progressBar1.Value = i;
                    File.Copy(Application.StartupPath + "/content/acorn/sonic" + i + ".pck", Application.StartupPath + "/content/acorn/Backup/sonic" + i + ".pck");
                }
                progressBar1.Value = 0;
                pictureBox3.Image = Sonic_Creator_Ultimate.Properties.Resources.MenuRestore;
            }
        }

        private void progressBar3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Backup();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            RefreshMods();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(Application.StartupPath + "/content/acorn/Backup"))
            {
                Save(false);
            }
            else
            {
                MessageBox.Show("No extracted files!");
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(Application.StartupPath + "/content/acorn/Backup"))
            {
                Save(true);
            }
            else
            {
                MessageBox.Show("No extracted files!");
            }
        }
        void Save(bool play)
        {
            p.Clear();
            SaveData();
            for (int i= 0; i < Mods.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    for (int k = 0; k <= 7; k++)
                    {
                        if (Directory.Exists(Mods[i].Path + "/sonic" + k))
                        {
                            p.Add(Process.Start("godotpcktool", "\""+Application.StartupPath+"/content/acorn/sonic"+k+ ".pck\" -a a \"" + Mods[i].Path+"/sonic"+k+ "\" --remove-prefix \"" + Mods[i].Path + "/sonic" + k+ "\""));
                        }
                    }
                }
            }
            foreach (Process pro in p)
            {
                pro.WaitForExit();
            }

                    if (play)
            {
                Play();
            }
        }
        public static void Empty(System.IO.DirectoryInfo directory)
        {
            foreach (System.IO.FileInfo file in directory.GetFiles()) file.Delete();
            foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
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
                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
        void Play()
        {
            Process.Start(Application.StartupPath + "/rainbow Shipping/Sonic Colors - Ultimate.exe", "-epicportal");
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.SelectedIndex-1>-1 && checkedListBox1.SelectedIndex != -1)
            {
                Mod m = Mods[checkedListBox1.SelectedIndex - 1];
                Mods[checkedListBox1.SelectedIndex - 1] = Mods[checkedListBox1.SelectedIndex];
                Mods[checkedListBox1.SelectedIndex] = m;
                RefreshList();
                SaveData();
            }
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.SelectedIndex + 1 < checkedListBox1.Items.Count&&checkedListBox1.SelectedIndex!=-1)
            {
                Mod m = Mods[checkedListBox1.SelectedIndex + 1];
                Mods[checkedListBox1.SelectedIndex + 1] = Mods[checkedListBox1.SelectedIndex];
                Mods[checkedListBox1.SelectedIndex] = m;
                RefreshList();
                SaveData();
            }
        }
        void SaveData()
        {
            XmlSerializer x = new XmlSerializer(Mods.GetType());
            StreamWriter st = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Sonic Colors/Mods.xml");
            for(int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                    Mods[i].Checked = checkedListBox1.GetItemChecked(i);
            }
            x.Serialize(st, Mods);
            st.WriteLine();
            st.Close();
        }
        void LoadData()
        {
            if(File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Sonic Colors/Mods.xml"))
            {
                XmlSerializer x = new XmlSerializer(Mods.GetType());
                StreamReader st = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Sonic Colors/Mods.xml");
                Mods = (List<Mod>)x.Deserialize(st);
                RefreshList();
                st.Close();
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveData();
        }
    }
    public class Mod
    {
        public string Name;
        public string Description;
        public string Creator;
        public string Version;
        public string Path;
        public bool Checked;
    }
}
