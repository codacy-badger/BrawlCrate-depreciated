using BrawlCrate.Properties;
using BrawlLib.Imaging;
using BrawlLib.Modeling;
using BrawlLib.OpenGL;
using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Audio;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Configuration;
using System.Net;
using System.Text.RegularExpressions;
using System.Reflection;

namespace BrawlCrate
{
    public partial class MainForm : Form
    {
        public bool renderPreviews = true;

        private static MainForm _instance;
        public static MainForm Instance { get { return _instance == null ? _instance = new MainForm() : _instance; } }

        private BaseWrapper _root;
        public BaseWrapper RootNode { get { return _root; } }

        private SettingsDialog _settings;
        private SettingsDialog Settings { get { return _settings == null ? _settings = new SettingsDialog() : _settings; } }

        public RecentFileHandler recentFileHandler;

        private InterpolationForm _interpolationForm = null;
        public InterpolationForm InterpolationForm
        {
            get
            {
                if (_interpolationForm == null)
                {
                    _interpolationForm = new InterpolationForm(null);
                    _interpolationForm.FormClosed += _interpolationForm_FormClosed;
                    _interpolationForm.Show();
                }
                return _interpolationForm;
            }
        }

        void _interpolationForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _interpolationForm = null;
        }

        // Canary stuff
        public string commitIDshort = "";
        public string commitIDlong = "";
        public static string getCommitId(bool longID = false)
        {
            return (" #" + File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "new")[longID ? 2 : 1]);
        }
        public static readonly string mainRepo = "soopercool101/BrawlCrate";
        public static readonly string mainBranch = "brawlcrate-master";
        public static string currentBranch { get { return GetCurrentBranch(); } set { SetCurrentBranch(value); } }
        private static string GetCurrentBranch()
        {
            try
            {
                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Branch"))
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Branch");
                return mainBranch;
                string temp = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Branch")[0];
                if (temp == null || temp == "")
                    throw (new ArgumentNullException());
                return temp;
            }
            catch
            {
                return mainBranch;
            }
        }
        private static void SetCurrentBranch(string newBranch)
        {
            if (newBranch == null || newBranch == "")
                newBranch = mainBranch;
            if (currentBranch.Equals(mainBranch, StringComparison.OrdinalIgnoreCase))
                return;
            string cRepo = currentRepo;
            // Check if Branch is valid
            ServicePointManager.SecurityProtocol |= (SecurityProtocolType)3072;
            try
            {
                string url = "https://github.com/" + cRepo + "/blob/" + newBranch + "/CanaryBuild/CanaryREADME.md";
                using (WebClient x = new WebClient())
                {
                    string source = x.DownloadString(url);
                    string title = Regex.Match(source, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value;
                    if (title.Contains("not found", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show(newBranch + " was not found as a valid branch of the " + cRepo + " repository, or does not properly support Canary builds.");
                        return;
                    }
                }
            }
            catch
            {
                MessageBox.Show(newBranch + " was not found as a valid branch of the " + cRepo + " repository, or does not properly support Canary builds.");
                return;
            }
            DirectoryInfo CanaryDir = Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary");
            CanaryDir.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Branch"))
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Branch");
            using (var sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Branch"))
            {
                sw.WriteLine(newBranch);
                sw.Write(cRepo);
                sw.Close();
            }
            MessageBox.Show("The canary updater will now track the " + newBranch + " branch, starting with the next canary update.", "Branch Changed");
        }
        public static string currentRepo { get { return GetCurrentRepo(); } set { SetCurrentRepo(value); } }
        private static string GetCurrentRepo()
        {
            try
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Branch"))
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Branch");
                return mainBranch;
                string temp = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Branch")[1];
                if (temp == null || temp == "")
                    throw (new ArgumentNullException());
                return temp;
            }
            catch
            {
                return mainRepo;
            }
        }
        private static void SetCurrentRepo(string newRepo)
        {
            if (newRepo == null || newRepo == "")
                newRepo = mainRepo;
            if (currentRepo.Equals(mainRepo, StringComparison.OrdinalIgnoreCase))
                return;
            string cBranch = currentBranch;
            // Check if Repo is valid
            ServicePointManager.SecurityProtocol |= (SecurityProtocolType)3072;
            try
            {
                string url = "https://github.com/" + newRepo + "/blob/" + cBranch + "/CanaryBuild/CanaryREADME.md";
                using (WebClient x = new WebClient())
                {
                    string source = x.DownloadString(url);
                    string title = Regex.Match(source, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value;
                    if (title.Contains("not found", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show(cBranch + " was not found as a valid branch of the " + newRepo + " repository, or does not properly support Canary builds.");
                        return;
                    }
                }
            }
            catch
            {
                MessageBox.Show(cBranch + " was not found as a valid branch of the " + newRepo + " repository, or does not properly support Canary builds.");
                return;
            }
            DirectoryInfo CanaryDir = Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary");
            CanaryDir.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Branch"))
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Branch");
            using (var sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Branch"))
            {
                sw.WriteLine(cBranch);
                sw.Write(newRepo);
                sw.Close();
            }
            MessageBox.Show("The canary updater will now track the " + newRepo + " repo, starting with the next canary update.", "Repo Changed");
        }

        // Sets branch and repo
        public static bool SetCanaryTracking(string newRepo, string newBranch)
        {
            if (newRepo == null || newRepo == "")
                newRepo = mainRepo;
            if (newBranch == null || newBranch == "")
                newBranch = mainBranch;
            if (currentRepo.Equals(newRepo, StringComparison.OrdinalIgnoreCase) && currentBranch.Equals(newBranch, StringComparison.OrdinalIgnoreCase))
                return false;
            // Check if Repo/Branch combo is valid
            ServicePointManager.SecurityProtocol |= (SecurityProtocolType)3072;
            try
            {
                string url = "https://github.com/" + newRepo + "/blob/" + newBranch + "/CanaryBuild/CanaryREADME.md";
                using (WebClient x = new WebClient())
                {
                    string source = x.DownloadString(url);
                    string title = Regex.Match(source, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value;
                    if (title.Contains("not found", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show(newBranch + " was not found as a valid branch of the " + newRepo + " repository, or does not properly support Canary builds.");
                        return false;
                    }
                }
            }
            catch
            {
                MessageBox.Show(newBranch + " was not found as a valid branch of the " + newRepo + " repository, or does not properly support Canary builds.");
                return false;
            }
            if (!newRepo.StartsWith("soopercool101/", StringComparison.OrdinalIgnoreCase) && !newRepo.StartsWith("libertyernie/", StringComparison.OrdinalIgnoreCase))
                if (MessageBox.Show("In future Canary updates you will track the " + newRepo + " repository. Please note that this repository is not affiliated with the BrawlBox or BrawlCrate teams. Neither team holds responsibility for malicious, illegal, or otherwise problematic code or content of this repository. Would you like to continue?", "Switching Repositories", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return false;
            DirectoryInfo CanaryDir = Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary");
            CanaryDir.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Branch"))
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Branch");
            using (var sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Branch"))
            {
                sw.WriteLine(newBranch);
                sw.Write(newRepo);
                sw.Close();
            }
            if(MessageBox.Show("The canary updater will now track the " + newBranch + " branch of the " + newRepo + " repository, starting with the next canary update. Would you like to " + (BrawlCrate.Properties.Settings.Default.DownloadCanaryBuilds ? "" : "enable Canary and ") + "begin the update now?", "Canary Target Changed", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Instance.Canary = true;
                Program.ForceDownloadCanary(true);
            }
            return true;
        }


        public MainForm()
        {
            InitializeComponent();
            _autoUpdate = BrawlCrate.Properties.Settings.Default.UpdateAutomatically;
            _displayPropertyDescription = BrawlCrate.Properties.Settings.Default.DisplayPropertyDescriptionWhenAvailable;
            _updatesOnStartup = BrawlCrate.Properties.Settings.Default.CheckUpdatesAtStartup;
            _docUpdates = BrawlCrate.Properties.Settings.Default.GetDocumentationUpdates;
            _showHex = BrawlCrate.Properties.Settings.Default.ShowHex;
            _canary = BrawlCrate.Properties.Settings.Default.DownloadCanaryBuilds;
            _autoCompressModules = BrawlLib.Properties.Settings.Default.AutoCompressModules;
            _autoCompressPCS = BrawlLib.Properties.Settings.Default.AutoCompressFighterPCS;
            _autoDecompressPAC = BrawlLib.Properties.Settings.Default.AutoDecompressFighterPAC;
            _autoCompressStages = BrawlLib.Properties.Settings.Default.AutoCompressStages;
            _autoPlayAudio = BrawlCrate.Properties.Settings.Default.AutoPlayAudio;
            if (_canary)
            {
                if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary"))
                {
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "new"))
                    {
                        commitIDshort = "#" + File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "new")[1];
                        commitIDlong = " #" + File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "new")[2];
                    }
                }
            }
            _showFullPath = BrawlCrate.Properties.Settings.Default.ShowFullPath;
            UpdateName();            
            // Slight space saving by deleting unused/unnecessary branch identifier
            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary"))
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Branch"))
                    if (currentRepo.Equals(mainRepo, StringComparison.OrdinalIgnoreCase) && currentBranch.Equals(mainBranch, StringComparison.OrdinalIgnoreCase))
                        File.Delete(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Branch");
                /*if(Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary").GetFiles().Length == 0)
                    Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary");*/
            }
            _compatibilityMode = BrawlLib.Properties.Settings.Default.CompatibilityMode;

            // Currently depreciated settings
            _importPNGwPalette = BrawlLib.Properties.Settings.Default.ImportPNGsWithPalettes;

            soundPackControl1._grid = propertyGrid1;
            soundPackControl1.lstSets.SmallImageList = ResourceTree.Images;
            foreach (Control c in splitContainer2.Panel2.Controls)
            {
                c.Visible = false;
                c.Dock = DockStyle.Fill;
            }
            m_DelegateOpenFile = new DelegateOpenFile(Program.Open);
            _instance = this;

            _currentControl = modelPanel1;

            modelPanel1.CurrentViewport._allowSelection = false;

            recentFileHandler = new RecentFileHandler(this.components);
            recentFileHandler.RecentFileToolStripItem = this.recentFilesToolStripMenuItem;

            if (Program.CanRunDiscordRPC() && !_updatesOnStartup)
            {
                Process[] px = Process.GetProcessesByName("BrawlCrate");
                if(px.Length == 1)
                    BrawlCrate.Discord.DiscordRpc.ClearPresence();
                BrawlCrate.Discord.DiscordSettings.startTime = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                BrawlCrate.Discord.DiscordSettings.DiscordController = new BrawlCrate.Discord.DiscordController();
                BrawlCrate.Discord.DiscordSettings.DiscordController.Initialize();
                BrawlCrate.Discord.DiscordSettings.LoadSettings(true);
            }
        }

        private delegate bool DelegateOpenFile(String s);
        private DelegateOpenFile m_DelegateOpenFile;
        
        protected override void OnShown(EventArgs e)
        {
            Focus();
            base.OnShown(e);
#if !DEBUG
            if (!Canary && BrawlCrate.Properties.Settings.Default.UpdateAutomatically && Program.firstBoot)
                MessageBox.Show(Program.UpdateMessage);
#endif
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                try
                {
                    using (var client = new System.Net.WebClient())
                    using (client.OpenRead("http://clients3.google.com/generate_204"))
                    {
                        using (System.Net.NetworkInformation.Ping s = new System.Net.NetworkInformation.Ping())
                            return s.Send("www.github.com").Status == System.Net.NetworkInformation.IPStatus.Success;
                    }
                }
                catch
                {
                    return false;
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }

        public void CheckUpdates(bool manual = true)
        {
            try
            {
                string path;
                if(!CheckForInternetConnection())
                {
                    if (manual)
                        MessageBox.Show("Could not connect to internet");
                    return;
                }
                bool automatic = (!manual && _autoUpdate) || (!manual && _canary);
                if (Program.CanRunGithubApp(manual, out path))
                {
                    if (!_canary)
                    {
                        Process git = Process.Start(new ProcessStartInfo()
                        {
                            FileName = path,
                            WindowStyle = ProcessWindowStyle.Hidden,
                            Arguments = String.Format("-bu {0} {1} {2} {3} \"{4}\"",
                            Program.TagName, manual ? "1" : "0", _docUpdates ? "1" : "0", automatic ? "1" : "0", Program.RootPath == null ? "<null>" : Program.RootPath),
                        });
                        if (automatic)
                            git.WaitForExit();
                    }
                    else
                    {
                        Process git = Process.Start(new ProcessStartInfo()
                        {
                            FileName = path,
                            WindowStyle = ProcessWindowStyle.Hidden,
                            Arguments = String.Format("-buc {0} \"{1}\"", manual ? "1" : "0", (Program.RootPath == null ? "<null>" : Program.RootPath)),
                        });
                        if (automatic)
                            git.WaitForExit();
                    }
                }
                else
                {
                    if (manual)
                        MessageBox.Show(".NET version 4.5 is required to run the updater.");
                    checkForUpdatesToolStripMenuItem.Enabled =
                    checkForUpdatesToolStripMenuItem.Visible = false;
                }
            }
            catch (Exception e)
            {
                if (manual)
                    MessageBox.Show(e.Message);
            }
            if (!manual && Program.CanRunDiscordRPC())
            {
                Process[] px = Process.GetProcessesByName("BrawlCrate");
                if (px.Length == 1)
                    BrawlCrate.Discord.DiscordRpc.ClearPresence();
                BrawlCrate.Discord.DiscordSettings.startTime = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                BrawlCrate.Discord.DiscordSettings.DiscordController = new BrawlCrate.Discord.DiscordController();
                BrawlCrate.Discord.DiscordSettings.DiscordController.Initialize();
                BrawlCrate.Discord.DiscordSettings.LoadSettings(true);
            }
        }

        public void ShowCanaryChangelog()
        {
            // Call canary changelog generation functionality
            try
            {
                string path;
                if (!CheckForInternetConnection())
                {
                    MessageBox.Show("Could not connect to internet");
                    return;
                }
                if (Program.CanRunGithubApp(true, out path))
                {
                    Process git = Process.Start(new ProcessStartInfo()
                    {
                        FileName = path,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Arguments = String.Format("-canarylog"),
                    });
                    git.WaitForExit();
                }
                else
                {
                    MessageBox.Show(".NET version 4.5 is required to run the updater.");
                    checkForUpdatesToolStripMenuItem.Enabled =
                    checkForUpdatesToolStripMenuItem.Visible = false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public bool DisplayPropertyDescriptionsWhenAvailable
        {
            get { return _displayPropertyDescription; }
            set
            {
                _displayPropertyDescription = value;

                BrawlCrate.Properties.Settings.Default.DisplayPropertyDescriptionWhenAvailable = _displayPropertyDescription;
                BrawlCrate.Properties.Settings.Default.Save();
                UpdatePropertyDescriptionBox(propertyGrid1.SelectedGridItem);
            }
        }
        bool _displayPropertyDescription;

        public bool CheckUpdatesOnStartup
        {
            get { return _updatesOnStartup; }
            set
            {
                _updatesOnStartup = value;

                BrawlCrate.Properties.Settings.Default.CheckUpdatesAtStartup = _updatesOnStartup;
                BrawlCrate.Properties.Settings.Default.Save();
            }
        }
        bool _updatesOnStartup;

        public bool GetDocumentationUpdates
        {
            get { return _docUpdates; }
            set
            {
                _docUpdates = value;

                BrawlCrate.Properties.Settings.Default.GetDocumentationUpdates = _docUpdates;
                BrawlCrate.Properties.Settings.Default.Save();
            }
        }
        bool _docUpdates;

        public bool AutoCompressPCS
        {
            get { return _autoCompressPCS; }
            set
            {
                _autoCompressPCS = value;

                BrawlLib.Properties.Settings.Default.AutoCompressFighterPCS = _autoCompressPCS;
                BrawlLib.Properties.Settings.Default.Save();
            }
        }
        bool _autoCompressPCS;
        
        public bool AutoDecompressFighterPAC
        {
            get { return _autoDecompressPAC; }
            set
            {
                _autoDecompressPAC = value;

                BrawlLib.Properties.Settings.Default.AutoDecompressFighterPAC = _autoDecompressPAC;
                BrawlLib.Properties.Settings.Default.Save();
            }
        }
        bool _autoDecompressPAC;

        public bool AutoCompressStages
        {
            get { return _autoCompressStages; }
            set
            {
                _autoCompressStages = value;

                BrawlLib.Properties.Settings.Default.AutoCompressStages = _autoCompressStages;
                BrawlLib.Properties.Settings.Default.Save();
            }
        }
        bool _autoCompressStages;
        
        public bool AutoCompressModules
        {
            get { return _autoCompressModules; }
            set
            {
                _autoCompressModules = value;

                BrawlLib.Properties.Settings.Default.AutoCompressStages = _autoCompressModules;
                BrawlLib.Properties.Settings.Default.Save();
            }
        }
        bool _autoCompressModules;
        
        public bool AutoPlayAudio
        {
            get { return _autoPlayAudio; }
            set
            {
                _autoPlayAudio = value;

                BrawlCrate.Properties.Settings.Default.AutoPlayAudio = _autoPlayAudio;
                BrawlCrate.Properties.Settings.Default.Save();
            }
        }
        bool _autoPlayAudio;

        public bool UpdateAutomatically

        {
            get { return _autoUpdate; }
            set
            {
                _autoUpdate = value;

                BrawlCrate.Properties.Settings.Default.UpdateAutomatically = _autoUpdate;
                BrawlCrate.Properties.Settings.Default.Save();
            }
        }
        bool _autoUpdate;

        public bool ShowHex
        {
            get { return _showHex; }
            set
            {
                _showHex = value;

                BrawlCrate.Properties.Settings.Default.ShowHex = _showHex;
                BrawlCrate.Properties.Settings.Default.Save();
            }
        }
        bool _showHex;

        public bool CompatibilityMode
        {
            get { return _compatibilityMode; }
            set
            {
                _compatibilityMode = value;

                BrawlLib.Properties.Settings.Default.HideMDL0Errors = BrawlLib.Properties.Settings.Default.CompatibilityMode = _compatibilityMode;
                BrawlLib.Properties.Settings.Default.Save();
            }
        }
        bool _compatibilityMode;

        public bool Canary
        {
            get { return _canary; }
            set
            {
                _canary = value;

                BrawlCrate.Properties.Settings.Default.DownloadCanaryBuilds = _canary;
                BrawlCrate.Properties.Settings.Default.Save();
            }
        }
        bool _canary;
        
        public bool ImportPNGsWithPalettes
        {
            get { return _importPNGwPalette; }
            set
            {
                _importPNGwPalette = value;

                BrawlLib.Properties.Settings.Default.ImportPNGsWithPalettes = _importPNGwPalette;
                BrawlLib.Properties.Settings.Default.Save();
            }
        }
        bool _importPNGwPalette;
        
        public bool ShowFullPath
        {
            get { return _showFullPath; }
            set
            {
                _showFullPath = value;

                BrawlCrate.Properties.Settings.Default.ShowFullPath = _showFullPath;
                BrawlCrate.Properties.Settings.Default.Save();
                UpdateName();
            }
        }
        bool _showFullPath;

        private void UpdatePropertyDescriptionBox(GridItem item)
        {
            if (!DisplayPropertyDescriptionsWhenAvailable)
            {
                if (propertyGrid1.HelpVisible != false)
                    propertyGrid1.HelpVisible = false;
            }
            else
                propertyGrid1.HelpVisible = item != null && item.PropertyDescriptor != null && !String.IsNullOrEmpty(item.PropertyDescriptor.Description);
        }

        private void propertyGrid1_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            if (DisplayPropertyDescriptionsWhenAvailable)
                UpdatePropertyDescriptionBox(e.NewSelection);
        }

        public void Reset()
        {
            _root = null;
            resourceTree.SelectedNode = null;
            resourceTree.Clear();

            if (Program.RootNode != null)
            {
                _root = BaseWrapper.Wrap(this, Program.RootNode);
                resourceTree.BeginUpdate();
                resourceTree.Nodes.Add(_root);
                resourceTree.SelectedNode = _root;
                _root.Expand();
                resourceTree.EndUpdate();

                closeToolStripMenuItem.Enabled = true;
                saveAsToolStripMenuItem.Enabled = true;
                saveToolStripMenuItem.Enabled = true;

                Program.RootNode._mainForm = this;
            }
            else
            {
                closeToolStripMenuItem.Enabled = false;
                saveAsToolStripMenuItem.Enabled = false;
                saveToolStripMenuItem.Enabled = false;
            }
            resourceTree_SelectionChanged(null, null);

            UpdateName();
            if(Program.CanRunDiscordRPC())
                BrawlCrate.Discord.DiscordSettings.LoadSettings(true);
        }

        public void UpdateName()
        {
            try
            {
                if (Canary && !Program.AssemblyTitle.Contains("canary", StringComparison.OrdinalIgnoreCase))
                    throw new InvalidEnumArgumentException();
                if (Program.RootPath != null)
                    Text = String.Format("{0} - {1}", Program.AssemblyTitle, ShowFullPath ? Program.RootPath : Program.FileName);
                else if (Canary)
                    Text = Program.AssemblyTitle.Substring(0, Program.AssemblyTitle.LastIndexOf(" #")) + commitIDlong;
                else
                    Text = Program.AssemblyTitle;
            }
            catch (InvalidEnumArgumentException)
            {
                try
                {
                    if (Program.RootPath != null)
                        Text = String.Format("{0} - {1}", _canary ? "BrawlCrate Canary" + commitIDlong + Program.RootPath : Program.AssemblyTitle);
                    else
                        Text = Program.AssemblyTitle;
                    if (Program.IsBirthday)
                        Text = "PartyBrawl" + Text.Substring(Text.IndexOf(' '));
                    Program.AssemblyTitle = ((AssemblyTitleAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0]).Title;
                    try
                    {
                        if (BrawlCrate.Properties.Settings.Default.DownloadCanaryBuilds)
                            Program.AssemblyTitle = "BrawlCrate Canary" + (MainForm.currentRepo.Equals(MainForm.mainRepo, StringComparison.OrdinalIgnoreCase) ? (MainForm.currentBranch.Equals(MainForm.mainBranch, StringComparison.OrdinalIgnoreCase) ? "" : ("@" + MainForm.currentBranch)) : "@" + MainForm.currentRepo + "@" + MainForm.currentBranch) + MainForm.getCommitId(false);
                    }
                    catch
                    {
                        Program.AssemblyTitle = ((AssemblyTitleAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0]).Title;
                    }
                    if (Program.IsBirthday)
                        Program.AssemblyTitle = "PartyBrawl" + Program.AssemblyTitle.Substring(Program.AssemblyTitle.IndexOf(' '));
                }
                catch
                {
                    Text = Program.AssemblyTitle;
                }
            }
            catch
            {
                Text = Program.AssemblyTitle;
            }
        }

        public void TargetResource(ResourceNode n)
        {
            if (_root != null)
                resourceTree.SelectedNode = _root.FindResource(n, true);
        }

        public Control _currentControl = null;
        private Control _secondaryControl = null;
        public unsafe void resourceTree_SelectionChanged(object sender, EventArgs e)
        {
            audioPlaybackPanel1.TargetSource = null;
            animEditControl.TargetSequence = null;
            texAnimEditControl.TargetSequence = null;
            shpAnimEditControl.TargetSequence = null;
            msBinEditor1.CurrentNode = null;
            //soundPackControl1.TargetNode = null;
            clrControl.ColorSource = null;
            visEditor.TargetNode = null;
            scN0CameraEditControl1.TargetSequence = null;
            scN0LightEditControl1.TargetSequence = null;
            scN0FogEditControl1.TargetSequence = null;
            texCoordControl1.TargetNode = null;
            ppcDisassembler1.SetTarget(null, 0, null);
            modelPanel1.ClearAll();
            mdL0ObjectControl1.SetTarget(null);
            if (hexBox1.ByteProvider != null)
                ((Be.Windows.Forms.DynamicFileByteProvider)hexBox1.ByteProvider).Dispose();
            
            Control newControl = null;
            Control newControl2 = null;

            BaseWrapper w;
            ResourceNode node = null;
            bool disable2nd = false;
            if ((resourceTree.SelectedNode is BaseWrapper) && ((node = (w = resourceTree.SelectedNode as BaseWrapper).ResourceNode) != null))
            {
                propertyGrid1.SelectedObject = node;
                if (node is IBufferNode && MainForm.Instance.ShowHex)
                {
                    IBufferNode d = node as IBufferNode;
                    if (d.IsValid())
                    {
                        hexBox1.ByteProvider = new Be.Windows.Forms.DynamicFileByteProvider(new UnmanagedMemoryStream(
                                (byte*)d.GetAddress(),
                                d.GetLength(),
                                d.GetLength(),
                                FileAccess.ReadWrite)) { _supportsInsDel = false };
                        newControl = hexBox1;
                    }
                }
                else if (node is RSARGroupNode)
                {
                    rsarGroupEditor.LoadGroup(node as RSARGroupNode);
                    newControl = rsarGroupEditor;
                }
                else if (node is RELMethodNode)
                {
                    ppcDisassembler1.SetTarget((RELMethodNode)node);
                    newControl = ppcDisassembler1;
                }
                else if (node is IVideo)
                {
                    videoPlaybackPanel1.TargetSource = node as IVideo;
                    if (node is PAT0TextureNode)
                        videoPlaybackPanel1.chkLoop.Checked = ((PAT0Node)node.Parent.Parent).Loop;
                    newControl = videoPlaybackPanel1;
                }
                else if (node is MDL0MaterialRefNode)
                {
                    newControl = texCoordControl1;
                }
                else if (node is MDL0ObjectNode)
                {
                    newControl = mdL0ObjectControl1;
                }
                else if (node is MSBinNode)
                {
                    msBinEditor1.CurrentNode = node as MSBinNode;
                    newControl = msBinEditor1;
                }
                else if (node is CHR0EntryNode)
                {
                    animEditControl.TargetSequence = node as CHR0EntryNode;
                    newControl = animEditControl;
                }
                else if (node is SRT0TextureNode)
                {
                    texAnimEditControl.TargetSequence = node as SRT0TextureNode;
                    newControl = texAnimEditControl;
                }
                else if (node is SHP0VertexSetNode)
                {
                    shpAnimEditControl.TargetSequence = node as SHP0VertexSetNode;
                    newControl = shpAnimEditControl;
                }
                else if (node is RSARNode)
                {
                    soundPackControl1.TargetNode = node as RSARNode;
                    newControl = soundPackControl1;
                }
                else if (node is VIS0EntryNode)
                {
                    visEditor.TargetNode = node as VIS0EntryNode;
                    newControl = visEditor;
                }
                else if (node is SCN0CameraNode)
                {
                    scN0CameraEditControl1.TargetSequence = node as SCN0CameraNode;
                    newControl = scN0CameraEditControl1;
                }
                else if (node is SCN0LightNode)
                {
                    scN0LightEditControl1.TargetSequence = node as SCN0LightNode;
                    newControl = scN0LightEditControl1;
                    disable2nd = true;
                }
                else if (node is SCN0FogNode)
                {
                    scN0FogEditControl1.TargetSequence = node as SCN0FogNode;
                    newControl = scN0FogEditControl1;
                    disable2nd = true;
                }
                else if (node is IAudioSource)
                {
                    audioPlaybackPanel1.TargetSource = node as IAudioSource;
                    IAudioStream[] sources = audioPlaybackPanel1.TargetSource.CreateStreams();
                    if (sources != null && sources.Length > 0 && sources[0] != null)
                    {
                        newControl = audioPlaybackPanel1;
                        if (node is RSTMNode)
                            audioPlaybackPanel1.chkLoop.Checked = ((RSTMNode)node).IsLooped;
                        if (AutoPlayAudio)
                            audioPlaybackPanel1.Play();
                    }
                }
                else if (node is IImageSource)
                {
                    previewPanel2.RenderingTarget = ((IImageSource)node);
                    newControl = previewPanel2;
                }
                else if (node is IRenderedObject)
				{
                    newControl = modelPanel1;
				}
				else if (node is STDTNode)
				{
					STDTNode stdt = (STDTNode)node;

					attributeGrid1.Clear();
					attributeGrid1.AddRange(stdt.GetPossibleInterpretations());
					attributeGrid1.TargetNode = stdt;
					newControl = attributeGrid1;
				} else if (node is TBCLNode)
				{
					TBCLNode tbcl = (TBCLNode)node;

					attributeGrid1.Clear();
					attributeGrid1.AddRange(tbcl.GetPossibleInterpretations());
					attributeGrid1.TargetNode = tbcl;
					newControl = attributeGrid1;
				} else if (node is TBGCNode)
                {
                    TBGCNode tbgc = (TBGCNode)node;

                    attributeGrid1.Clear();
                    attributeGrid1.AddRange(tbgc.GetPossibleInterpretations());
                    attributeGrid1.TargetNode = tbgc;
                    newControl = attributeGrid1;
                }  else if (node is TBGDNode)
                {
                    TBGDNode tbgd = (TBGDNode)node;

                    attributeGrid1.Clear();
                    attributeGrid1.AddRange(tbgd.GetPossibleInterpretations());
                    attributeGrid1.TargetNode = tbgd;
                    newControl = attributeGrid1;
                } else if (node is TBGMNode)
                {
                    TBGMNode tbgm = (TBGMNode)node;

                    attributeGrid1.Clear();
                    attributeGrid1.AddRange(tbgm.GetPossibleInterpretations());
                    attributeGrid1.TargetNode = tbgm;
                    newControl = attributeGrid1;
                } else if (node is TBLVNode)
                {
                    TBLVNode tblv = (TBLVNode)node;

                    attributeGrid1.Clear();
                    attributeGrid1.AddRange(tblv.GetPossibleInterpretations());
                    attributeGrid1.TargetNode = tblv;
                    newControl = attributeGrid1;
                } else if (node is TBRMNode)
                {
                    TBRMNode tbrm = (TBRMNode)node;

                    attributeGrid1.Clear();
                    attributeGrid1.AddRange(tbrm.GetPossibleInterpretations());
                    attributeGrid1.TargetNode = tbrm;
                    newControl = attributeGrid1;
                }  else if (node is TBSTNode)
                {
                    TBSTNode tbst = (TBSTNode)node;

                    attributeGrid1.Clear();
                    attributeGrid1.AddRange(tbst.GetPossibleInterpretations());
                    attributeGrid1.TargetNode = tbst;
                    newControl = attributeGrid1;
                } else if (node is IColorSource && !disable2nd)
                {
                    clrControl.ColorSource = node as IColorSource;
                    if (((IColorSource)node).ColorCount(0) > 0)
                        if (newControl != null)
                            newControl2 = clrControl;
                        else
                            newControl = clrControl;
                }
                else if (MainForm.Instance.ShowHex && !(node is RELEntryNode || node is RELNode))
                {
                    if (node.WorkingUncompressed.Length > 0)
                    {
                        hexBox1.ByteProvider = new Be.Windows.Forms.DynamicFileByteProvider(new UnmanagedMemoryStream(
                                (byte*)node.WorkingUncompressed.Address,
                                node.WorkingUncompressed.Length,
                                node.WorkingUncompressed.Length,
                                FileAccess.ReadWrite)) { _supportsInsDel = false };
                        newControl = hexBox1;
                    }
                }

                if ((editToolStripMenuItem.DropDown = w.ContextMenuStrip) != null)
                    editToolStripMenuItem.Enabled = true;
                else
                    editToolStripMenuItem.Enabled = false;
            }
            else
            {
                propertyGrid1.SelectedObject = null;
                try
                {
                    editToolStripMenuItem.DropDown = null;
                }
                catch
                {

                }
                editToolStripMenuItem.Enabled = false;
            }

            if (_secondaryControl != newControl2)
            {
                if (_secondaryControl != null)
                {
                    _secondaryControl.Dock = DockStyle.Fill;
                    _secondaryControl.Visible = false;
                }
                _secondaryControl = newControl2;
                if (_secondaryControl != null)
                {
                    _secondaryControl.Dock = DockStyle.Right;
                    _secondaryControl.Visible = true;
                    _secondaryControl.Width = 340;
                }
            }
            if (_currentControl != newControl)
            {
                if (_currentControl != null)
                    _currentControl.Visible = false;
                _currentControl = newControl;
                if (_currentControl != null)
                    _currentControl.Visible = true;
            }
            else if (_currentControl != null && !_currentControl.Visible)
                _currentControl.Visible = true;
            if (_currentControl != null)
            {
                if (_secondaryControl != null)
                    _currentControl.Width = splitContainer2.Panel2.Width - _secondaryControl.Width;
                _currentControl.Dock = DockStyle.Fill;
            }

            //Model panel has to be loaded first to display model correctly
            if (_currentControl is ModelPanel && renderPreviews)
            {
                if (node._children == null)
                    node.Populate(0);
                
                if (node is IModel && ModelEditControl.Instances.Count == 0)
                {
                    IModel m = node as IModel;
                    m.ResetToBindState();
                }

                IRenderedObject o = node as IRenderedObject;
                modelPanel1.AddTarget(o);
                modelPanel1.SetCamWithBox(o.GetBox());
            }
            else if (_currentControl is MDL0ObjectControl)
            {
                if(!mdL0ObjectControl1.SetTarget(node as MDL0ObjectNode))
                    _currentControl = _secondaryControl = null;
            }
            else if (_currentControl is TexCoordControl)
                texCoordControl1.TargetNode = ((MDL0MaterialRefNode)node);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!Program.Close()) 
                e.Cancel = true;

            base.OnClosing(e);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string inFile;
            int i = Program.OpenFile(SupportedFilesHandler.CompleteFilterEditableOnly, out inFile);
            if(i != 0)
                Program.Open(inFile);
        }

        #region File Menu
        private void aRCArchiveToolStripMenuItem_Click(object sender, EventArgs e) { Program.New<ARCNode>(); }
        private void u8FileArchiveToolStripMenuItem_Click(object sender, EventArgs e) { Program.New<U8Node>(); }
        private void brresPackToolStripMenuItem_Click(object sender, EventArgs e) { Program.New<BRRESNode>(); }
        private void tPLTextureArchiveToolStripMenuItem_Click(object sender, EventArgs e) { Program.New<TPLNode>(); }
        private void eFLSEffectListToolStripMenuItem_Click(object sender, EventArgs e) { Program.New<EFLSNode>(); }
        private void rEFFParticlesToolStripMenuItem_Click(object sender, EventArgs e) { Program.New<REFFNode>(); }
        private void rEFTParticleTexturesToolStripMenuItem_Click(object sender, EventArgs e) { Program.New<REFTNode>(); }
        // PM 3.6 1:1
        private void pm36STGBATTLEFIELD_ToolStripMenuItem_Click(object sender, EventArgs e) { if (BrawlLib.BrawlCrate.Generate1to1Stages.PM36STGBATTLEFIELD()) { Program.Close(); Program.Open(BrawlLib.BrawlCrate.Generate1to1Stages.pm36stgbattlefieldDirectory, false); } }
        private void pm36STGDOLPIC_ToolStripMenuItem_Click(object sender, EventArgs e) { if (BrawlLib.BrawlCrate.Generate1to1Stages.PM36STGDOLPIC()) { Program.Close(); Program.Open(BrawlLib.BrawlCrate.Generate1to1Stages.pm36stgdolpicDirectory, false); } }
        private void pm36STGDXGREENS_ToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void pm36STGFINAL_ToolStripMenuItem_Click(object sender, EventArgs e) { if (BrawlLib.BrawlCrate.Generate1to1Stages.PM36STGFINAL()) { Program.Close(); Program.Open(BrawlLib.BrawlCrate.Generate1to1Stages.pm36stgfinalDirectory, false); } }
        private void pm36STGFAMICOM_ToolStripMenuItem_Click(object sender, EventArgs e) { if (BrawlLib.BrawlCrate.Generate1to1Stages.PM36STGFAMICOM()) { Program.Close(); Program.Open(BrawlLib.BrawlCrate.Generate1to1Stages.pm36stgfamicomDirectory, false); } }
        private void pm36STGGREENHILL_ToolStripMenuItem_Click(object sender, EventArgs e) { if (BrawlLib.BrawlCrate.Generate1to1Stages.PM36STGGREENHILL()) { Program.Close(); Program.Open(BrawlLib.BrawlCrate.Generate1to1Stages.pm36stggreenhillDirectory, false); } }
        private void pm36STGDXPSTADIUM_ToolStripMenuItem_Click(object sender, EventArgs e) { if (BrawlLib.BrawlCrate.Generate1to1Stages.PM36STGDXPSTADIUM()) { Program.Close(); Program.Open(BrawlLib.BrawlCrate.Generate1to1Stages.pm36stgdxpstadiumDirectory, false); } }
        private void pm36STGVILLAGE_ToolStripMenuItem_Click(object sender, EventArgs e) { if (BrawlLib.BrawlCrate.Generate1to1Stages.PM36STGVILLAGE()) { Program.Close(); Program.Open(BrawlLib.BrawlCrate.Generate1to1Stages.pm36stgvillageDirectory, false); } }
        private void pm36STGVILLAGE_nv_ToolStripMenuItem_Click(object sender, EventArgs e) { if (BrawlLib.BrawlCrate.Generate1to1Stages.PM36STGVILLAGE_NV()) { Program.Close(); Program.Open(BrawlLib.BrawlCrate.Generate1to1Stages.pm36stgvillagenvDirectory, false); } }
        private void pm36STGMADEIN_ToolStripMenuItem_Click(object sender, EventArgs e) { if (BrawlLib.BrawlCrate.Generate1to1Stages.PM36STGMADEIN()) { Program.Close(); Program.Open(BrawlLib.BrawlCrate.Generate1to1Stages.pm36stgmadeinDirectory, false); } }
        // vBrawl 1:1
        private void vBrawlSTGBATTLEFIELD_ToolStripMenuItem_Click(object sender, EventArgs e) { if (BrawlLib.BrawlCrate.Generate1to1Stages.vBrawlSTGBATTLEFIELD()) { Program.Close(); Program.Open(BrawlLib.BrawlCrate.Generate1to1Stages.vbrawlstgbattlefieldDirectory, false); } }
        private void vBrawlSTGFINAL_ToolStripMenuItem_Click(object sender, EventArgs e) { if (BrawlLib.BrawlCrate.Generate1to1Stages.vBrawlSTGFINAL()) { Program.Close(); Program.Open(BrawlLib.BrawlCrate.Generate1to1Stages.vbrawlstgfinalDirectory, false); } }
        private void vBrawlSTGVILLAGE_ToolStripMenuItem_Click(object sender, EventArgs e) { if (BrawlLib.BrawlCrate.Generate1to1Stages.vBrawlSTGVILLAGE()) { Program.Close(); Program.Open(BrawlLib.BrawlCrate.Generate1to1Stages.vbrawlstgvillageDirectory, false); } }
        private void vBrawlSTGVILLAGE_nv_ToolStripMenuItem_Click(object sender, EventArgs e) { if (BrawlLib.BrawlCrate.Generate1to1Stages.vBrawlSTGVILLAGE_NV()) { Program.Close(); Program.Open(BrawlLib.BrawlCrate.Generate1to1Stages.vbrawlstgvillagenvDirectory, false); } }
        // Custom 1:1
        private void customSkySanctuary_ToolStripMenuItem_Click(object sender, EventArgs e) { if (BrawlLib.BrawlCrate.Generate1to1Stages.STGSKYSANCTUARY()) { Program.Close(); Program.Open(BrawlLib.BrawlCrate.Generate1to1Stages.skySanctDirectory, false); } }


        private void saveToolStripMenuItem_Click(object sender, EventArgs e) { Program.Save(); }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) { Program.SaveAs(); }
        private void closeToolStripMenuItem_Click(object sender, EventArgs e) { Program.Close(); }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) { this.Close(); }
        #endregion

        private void fileResizerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //using (FileResizer res = new FileResizer())
            //    res.ShowDialog();
        }
        private void settingsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Settings.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) { AboutForm.Instance.ShowDialog(this); }

        private void bRStmAudioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path;
            if (Program.OpenFile("PCM Audio (*.wav)|*.wav", out path) > 0)
            {
                if (Program.New<RSTMNode>())
                {
                    using (BrstmConverterDialog dlg = new BrstmConverterDialog())
                    {
                        dlg.AudioSource = path;
                        if (dlg.ShowDialog(this) == DialogResult.OK)
                        {
                            Program.RootNode.Name = Path.GetFileNameWithoutExtension(dlg.AudioSource);
                            Program.RootNode.ReplaceRaw(dlg.AudioData);
                        }
                        else
                            Program.Close(true);
                    }
                }
            }
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            Array a = (Array)e.Data.GetData(DataFormats.FileDrop);
            if (a != null)
            {
                string s = null;
                for (int i = 0; i < a.Length; i++)
                {
                    s = a.GetValue(i).ToString();
                    this.BeginInvoke(m_DelegateOpenFile, new Object[] { s });
                }
            }
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void gCTEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new GCTEditor().Show();
        }

        private void recentFilesToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (File.Exists(((RecentFileHandler.FileMenuItem)e.ClickedItem).FileName))
            {
                Program.Open(((RecentFileHandler.FileMenuItem)e.ClickedItem).FileName);
            }
            else
            {
                MessageBox.Show("File does not exist.");
            }
        }

        private void checkForUpdatesToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            CheckUpdates();
        }

        private void splitContainer_MouseDown(object sender, MouseEventArgs e)
        {
            ((SplitContainer)sender).IsSplitterFixed = true;
        }
        private void splitContainer_MouseUp(object sender, MouseEventArgs e)
        {
            ((SplitContainer)sender).IsSplitterFixed = false;
        }
        private void splitContainer_MouseMove(object sender, MouseEventArgs e)
        {
            SplitContainer splitter = (SplitContainer)sender;
            if (splitter.IsSplitterFixed)
            {
                if (e.Button.Equals(MouseButtons.Left))
                {
                    if (splitter.Orientation.Equals(Orientation.Vertical))
                    {
                        if (e.X > 0 && e.X < splitter.Width)
                        {
                            splitter.SplitterDistance = e.X;
                            splitter.Refresh();
                        }
                    }
                    else
                    {
                        if (e.Y > 0 && e.Y < splitter.Height)
                        {
                            splitter.SplitterDistance = e.Y;
                            splitter.Refresh();
                        }
                    }
                }
                else
                    splitter.IsSplitterFixed = false;
            }
        }
    }

    public class RecentFileHandler : Component
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
        }
        public class FileMenuItem : ToolStripMenuItem
        {
            string fileName;

            public string FileName
            {
                get { return fileName; }
                set { fileName = value; }
            }

            public FileMenuItem(string fileName)
            {
                this.fileName = fileName;
            }

            public override string Text
            {
                get
                {
                    ToolStripMenuItem parent = (ToolStripMenuItem)this.OwnerItem;
                    int index = parent.DropDownItems.IndexOf(this);
                    return string.Format("{0} {1}", index + 1, fileName);
                }
                set
                {
                }
            }
        }

        public RecentFileHandler()
        {
            InitializeComponent();

            Init();
        }

        public RecentFileHandler(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

            Init();
        }

        void Init()
        {
            Settings.Default.PropertyChanged += new PropertyChangedEventHandler(Default_PropertyChanged);
        }

        public void AddFile(string fileName)
        {
            try
            {
                if (this.recentFileToolStripItem == null)
                    throw new OperationCanceledException("recentFileToolStripItem can not be null!");

                // check if the file is already in the collection
                int alreadyIn = GetIndexOfRecentFile(fileName);
                if (alreadyIn != -1) // remove it
                {
                    Settings.Default.RecentFiles.RemoveAt(alreadyIn);
                    if (recentFileToolStripItem.DropDownItems.Count > alreadyIn)
                        recentFileToolStripItem.DropDownItems.RemoveAt(alreadyIn);
                }
                else if (alreadyIn == 0) // it´s the latest file so return
                    return;

                // insert the file on top of the list
                Settings.Default.RecentFiles.Insert(0, fileName);
                recentFileToolStripItem.DropDownItems.Insert(0, new FileMenuItem(fileName));

                // remove any beyond the max size, if max size is reached
                while (Settings.Default.RecentFiles.Count > Settings.Default.RecentFilesMax)
                    Settings.Default.RecentFiles.RemoveAt(Settings.Default.RecentFilesMax);
                while (recentFileToolStripItem.DropDownItems.Count > Settings.Default.RecentFilesMax)
                    recentFileToolStripItem.DropDownItems.RemoveAt(Settings.Default.RecentFilesMax);

                // enable the menu item if it´s disabled
                if (!recentFileToolStripItem.Enabled)
                    recentFileToolStripItem.Enabled = true;

                // save the changes
                Settings.Default.Save();
            }
            catch { }
        }

        int GetIndexOfRecentFile(string filename)
        {
            for (int i = 0; i < Settings.Default.RecentFiles.Count; i++)
            {
                string currentFile = Settings.Default.RecentFiles[i];
                if (string.Equals(currentFile, filename, StringComparison.OrdinalIgnoreCase))
                    return i;
            }
            return -1;
        }

        ToolStripMenuItem recentFileToolStripItem;

        public ToolStripMenuItem RecentFileToolStripItem
        {
            get { return recentFileToolStripItem; }
            set
            {
                if (recentFileToolStripItem == value)
                    return;

                recentFileToolStripItem = value;

                ReCreateItems();
            }
        }

        void Default_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "RecentFilesMax")
            {
                ReCreateItems();
            }
        }

        void ReCreateItems()
        {
            if (recentFileToolStripItem == null)
                return;

            if (Settings.Default.RecentFiles == null)
                Settings.Default.RecentFiles = new StringCollection();

            recentFileToolStripItem.DropDownItems.Clear();
            recentFileToolStripItem.Enabled = (Settings.Default.RecentFiles.Count > 0);

            int fileItemCount = Math.Min(Settings.Default.RecentFilesMax, Settings.Default.RecentFiles.Count);
            for (int i = 0; i < fileItemCount; i++)
            {
                string file = Settings.Default.RecentFiles[i];
                recentFileToolStripItem.DropDownItems.Add(new FileMenuItem(file));
            }
        }

        public void Clear()
        {
            Settings.Default.RecentFiles.Clear();
            ReCreateItems();
        }
    }
}
