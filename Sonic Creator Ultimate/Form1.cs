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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sonic_Creator_Ultimate
{
    public partial class Form1 : Form
    {
        List<Mod> Mods = new List<Mod>();
        public Form1()
        {
            if (new DirectoryInfo(Application.StartupPath).Name != "SonicColorsUltimate")
            {
                MessageBox.Show("Please place this in the Sonic Colors Ultimate directory!");
                Application.Exit();
            }
            InitializeComponent();
            RefreshMods();
        }
        void RefreshMods()
        {
            Mods.Clear();
            foreach(string dir in Directory.GetDirectories(Application.StartupPath + "/mods"))
            {
                if (File.Exists(dir + "/mod.ini"))
                {
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

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.SelectedIndex-1>-1 && checkedListBox1.SelectedIndex != -1)
            {
                Mod m = Mods[checkedListBox1.SelectedIndex - 1];
                Mods[checkedListBox1.SelectedIndex - 1] = Mods[checkedListBox1.SelectedIndex];
                Mods[checkedListBox1.SelectedIndex] = m;
                RefreshList();
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
            }
        }
    }

    class Mod
    {
        public string Name;
        public string Description;
        public string Creator;
        public string Version;
        public string Path;
    }
}
