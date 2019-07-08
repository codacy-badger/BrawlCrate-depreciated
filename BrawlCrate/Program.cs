using BrawlLib.BrawlCrate;
using BrawlLib.IO;
using BrawlLib.SSBB.ResourceNodes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace BrawlCrate
{
    internal static class Program
    {
        //Make sure this matches the tag name of the release on github exactly
        public static readonly string TagName = "BrawlCrate_v0.26Hotfix2";
        public static readonly string UpdateMessage = @"Updated to BrawlCrate v0.26! This release:
- Fixes issue in which Shaders could be initialized with null names
- Fixes crashes caused by VIS0 syncing
- Fixes crashes when moving vertices
- Fixes issue where an exported DAE could not be reimported
- Fixes bug in which vertex edits could not be saved
- Fixes bug in which Final Smash flags for Fighter Configs switched the None/Single flag
- Fixes bug in which generating metal materials for a model that doesn't currently have them would result in the metal texture being set to null
- Fixes bug in which exporting a model with null textures would crash the program

Full changelog can be found in the installation folder: " + '\n' + AppDomain.CurrentDomain.BaseDirectory + "Changelog.txt";

        public static string AssemblyTitle;
        public static readonly string AssemblyVersion;
        public static readonly string AssemblyDescription;
        public static readonly string AssemblyCopyright;
        public static readonly string FullPath;
        public static readonly string BrawlLibTitle;
        public static readonly string BrawlLibVersion;

        private static readonly OpenFileDialog _openDlg;
        private static readonly SaveFileDialog _saveDlg;
        private static readonly FolderBrowserDialog _folderDlg;

        internal static ResourceNode _rootNode;
        public static ResourceNode RootNode { get => _rootNode; set { _rootNode = value; MainForm.Instance.Reset(); } }
        internal static string _rootPath;
        public static string RootPath => _rootPath;
        public static string FileName => _rootPath == null ? null : _rootPath.Substring(_rootPath.LastIndexOf('\\') + 1);

        internal static bool _birthday;
        public static bool IsBirthday => _birthday;

        static Program()
        {
            Application.EnableVisualStyles();
            _birthday = BrawlLib.BrawlCrate.PerSessionSettings.Birthday = (DateTime.Now.Month == 4 && DateTime.Now.Day == 8 && DateTime.Now.Year > 2018);
            AssemblyTitle = ((AssemblyTitleAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0]).Title;
            try
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Canary\\Active"))
                {
                    AssemblyTitle = "BrawlCrate Canary" + MainForm.getCommitId(false);
                }
            }
            catch
            {
                AssemblyTitle = ((AssemblyTitleAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0]).Title;
            }
            if (_birthday)
            {
                AssemblyTitle = "PartyBrawl" + AssemblyTitle.Substring(AssemblyTitle.IndexOf(' '));
            }

            AssemblyDescription = ((AssemblyDescriptionAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0]).Description;
            AssemblyVersion = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileVersion;
            AssemblyCopyright = ((AssemblyCopyrightAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0]).Copyright;
            FullPath = Process.GetCurrentProcess().MainModule.FileName;
            BrawlLibTitle = ((AssemblyTitleAttribute)Assembly.GetAssembly(typeof(ResourceNode)).GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0]).Title;
            BrawlLibVersion = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetAssembly(typeof(ResourceNode)).Location).FileVersion;

            _openDlg = new OpenFileDialog();
            _saveDlg = new SaveFileDialog();
            _folderDlg = new FolderBrowserDialog();

            StageNameGenerators.GenerateList();
            FighterNameGenerators.GenerateLists();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.ThreadException += Application_ThreadException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            List<ResourceNode> dirty = GetDirtyFiles();
            Exception ex = e.Exception;
            IssueDialog d = new IssueDialog(ex, dirty);
            d.ShowDialog();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception)
            {
                List<ResourceNode> dirty = GetDirtyFiles();
                Exception ex = e.ExceptionObject as Exception;
                IssueDialog d = new IssueDialog(ex, dirty);
                d.ShowDialog();
            }
        }

        private static List<ResourceNode> GetDirtyFiles()
        {
            List<ResourceNode> dirty = new List<ResourceNode>();

            foreach (ModelEditControl control in ModelEditControl.Instances)
            {
                foreach (ResourceNode r in control.rightPanel.pnlOpenedFiles.OpenedFiles)
                {
                    if (r.IsDirty && !dirty.Contains(r))
                    {
                        dirty.Add(r);
                    }
                }
            }

            if (_rootNode != null && _rootNode.IsDirty && !dirty.Contains(_rootNode))
            {
                dirty.Add(_rootNode);
            }

            return dirty;
        }

        public static bool firstBoot = false;
        [STAThread]
        public static void Main(string[] args)
        {
            SplashForm s = new SplashForm();
            s.Show();
            s.Focus();
            firstBoot = false;
#if !DEBUG
            if (BrawlCrate.Properties.Settings.Default.UpdateSettings)
            {
                foreach (Assembly _Assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (Type _Type in _Assembly.GetTypes())
                    {
                        if (_Type.Name == "Settings" && typeof(SettingsBase).IsAssignableFrom(_Type))
                        {
                            ApplicationSettingsBase settings = (ApplicationSettingsBase)_Type.GetProperty("Default").GetValue(null, null);
                            if (settings != null)
                            {
                                settings.Upgrade();
                                settings.Reload();
                                settings.Save();
                            }
                        }
                    }
                }
                // This is the first time booting this update
                firstBoot = true;
                // Canary setting should be initialized depending on if canary is active
                if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary") && File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Active"))
                {
                    BrawlCrate.Properties.Settings.Default.DownloadCanaryBuilds = true;
                    BrawlCrate.Properties.Settings.Default.CheckUpdatesAtStartup = true;
                    BrawlCrate.Properties.Settings.Default.UpdateAutomatically = true;
                }
                else
                {
                    BrawlCrate.Properties.Settings.Default.DownloadCanaryBuilds = false;
                }
                // Ensure settings only get updated once
                BrawlCrate.Properties.Settings.Default.UpdateSettings = false;
                BrawlCrate.Properties.Settings.Default.Save();
            }
            // Ensure canary status updated correctly
            if ((Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary") && File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Active")) != BrawlCrate.Properties.Settings.Default.DownloadCanaryBuilds)
            {
                MessageBox.Show("Canary Status Changed. Updating accordingly.");
                // Canary setting should be initialized depending on if canary is active
                if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary") && File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Active"))
                {
                    BrawlCrate.Properties.Settings.Default.DownloadCanaryBuilds = true;
                    BrawlCrate.Properties.Settings.Default.CheckUpdatesAtStartup = true;
                    BrawlCrate.Properties.Settings.Default.UpdateAutomatically = true;
                    BrawlCrate.Properties.Settings.Default.Save();
                }
                else
                {
                    BrawlCrate.Properties.Settings.Default.DownloadCanaryBuilds = false;
                    BrawlCrate.Properties.Settings.Default.Save();
                }
                if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary") && File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Active"))
                {
                    ForceDownloadCanary();
                }
                else
                {
                    ForceDownloadStable();
                }
            }
            try
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Update.exe"))
                {
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Update.exe");
                }

                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "temp.exe"))
                {
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + '\\' + "temp.exe");
                }

                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Update.bat"))
                {
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Update.bat");
                }

                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "StageBox.exe"))
                {
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + '\\' + "StageBox.exe");
                }

                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "BrawlBox.exe"))
                {
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + '\\' + "BrawlBox.exe");
                }
            }
            catch
            {

            }
#endif
            if (args.Length >= 1)
            {
                if (args[0].Equals("/gct", StringComparison.InvariantCultureIgnoreCase))
                {
                    GCTEditor editor = new GCTEditor();
                    if (args.Length >= 2)
                    {
                        editor.TargetNode = GCTEditor.LoadGCT(args[1]);
                    }

                    s.Close();
                    Application.Run(editor);
                    return;
                }
                else if (args[0].EndsWith(".gct", StringComparison.InvariantCultureIgnoreCase))
                {
                    GCTEditor editor = new GCTEditor
                    {
                        TargetNode = GCTEditor.LoadGCT(args[0])
                    };
                    s.Close();
                    Application.Run(editor);
                    return;
                }
            }

            try
            {
                if (args.Length >= 1)
                {
                    if (args[0] != "null" && args[0] != "-Canary" && args[0] != "-Stable")
                    {
                        Open(args[0]);
                    }
                }
                if (args.Length >= 2 && args[1] != "-Canary" && args[1] != "-Stable")
                {
                    ResourceNode target = ResourceNode.FindNode(RootNode, args[1], true);
                    if (target != null)
                    {
                        MainForm.Instance.TargetResource(target);
                    }
                    else
                    {
                        Say(string.Format("Error: Unable to find node or path '{0}'!", args[1]));
                    }
                }

#if !DEBUG //Don't need to see this every time a debug build is compiled
                if (MainForm.Instance.CheckUpdatesOnStartup)
                {
                    MainForm.Instance.CheckUpdates(false);
                }
                // Show changelog if this is the first time opening this release, and the message wasn't seen 
                if (BrawlCrate.Properties.Settings.Default.DownloadCanaryBuilds)
                {
                    if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary") && File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Old"))
                    {
                        MainForm.Instance.ShowCanaryChangelog();
                    }
                }
#endif
                s.Close();
                Application.Run(MainForm.Instance);
            }
            catch (FileNotFoundException x)
            {
                if (x.Message.Contains("Could not load file or assembly"))
                {
                    MessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    throw x;
                }
            }
            finally
            {
                //if (CanRunDiscordRPC())
                //{
                Discord.DiscordRpc.ClearPresence();
                Discord.DiscordRpc.Shutdown();
                //}
                try
                {
                    Generate1to1Stages.clearTmpDir(Generate1to1Stages.tmpDirectory);
                }
                catch
                {

                }
                Close(true);
            }
        }

        public static void ForceDownloadCanary(bool manual = true)
        {
            try
            {
                if (!MainForm.CheckForInternetConnection())
                {
                    if (manual)
                    {
                        MessageBox.Show("Could not connect to internet");
                    }

                    return;
                }
                if (Program.CanRunGithubApp(true, out string path))
                {
                    Process git = Process.Start(new ProcessStartInfo()
                    {
                        FileName = path,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Arguments = string.Format("-dlCanary \"{0}\"", Program.RootPath == null ? "<null>" : Program.RootPath),
                    });
                    git.WaitForExit();
                }
            }
            catch (Exception e)
            {
                if (manual)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        public static void ForceDownloadStable(bool manual = true)
        {
            try
            {
                if (!MainForm.CheckForInternetConnection())
                {
                    if (manual)
                    {
                        MessageBox.Show("Could not connect to internet");
                    }

                    return;
                }
                if (Program.CanRunGithubApp(true, out string path))
                {
                    Process git = Process.Start(new ProcessStartInfo()
                    {
                        FileName = path,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Arguments = string.Format("-dlStable \"{0}\"", Program.RootPath == null ? "<null>" : Program.RootPath),
                    });
                    git.WaitForExit();
                }
            }
            catch (Exception e)
            {
                if (manual)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        public static void Say(string msg)
        {
            MessageBox.Show(msg);
        }

        public static bool New<T>() where T : ResourceNode
        {
            if (!Close())
            {
                return false;
            }

            _rootNode = Activator.CreateInstance<T>();
            _rootNode.Name = "NewTree";
            MainForm.Instance.Reset();

            return true;
        }

        public static bool Close() { return Close(false); }
        public static bool Close(bool force)
        {
            //Have to close external files before the root file
            while (ModelEditControl.Instances.Count > 0)
            {
                ModelEditControl control = ModelEditControl.Instances[0];
                if (control.ParentForm != null)
                {
                    control.ParentForm.Close();
                    if (!control.IsDisposed)
                    {
                        return false;
                    }
                }
                else if (!control.Close())
                {
                    return false;
                }
            }

            if (_rootNode != null)
            {
                if (_rootNode.IsDirty && !force)
                {
                    DialogResult res = MessageBox.Show("Save changes?", "Closing", MessageBoxButtons.YesNoCancel);
                    if ((res == DialogResult.Yes && !Save()) || res == DialogResult.Cancel)
                    {
                        return false;
                    }
                }

                _rootNode.Dispose();
                _rootNode = null;

                MainForm.Instance.Reset();
            }

            _rootPath = null;
            //HardcodedFiles.DeleteHardcodedFiles();
            MainForm.Instance.UpdateName();
            return true;
        }

        public static bool Open(string path)
        {
            return Open(path, true);
        }

        public static string openTempFile = null;
        public static bool Open(string path, bool setRoot)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            if (!File.Exists(path))
            {
                Say("File does not exist.");
                return false;
            }

            if (path.EndsWith(".gct", StringComparison.InvariantCultureIgnoreCase))
            {
                GCTEditor editor = new GCTEditor
                {
                    TargetNode = GCTEditor.LoadGCT(path)
                };
                editor.Show();
                MainForm.Instance.recentFileHandler.AddFile(path);
                return true;
            }

            if (!Close())
            {
                return false;
            }

        REGEN:
            DirectoryInfo tmp = Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + '\\' + "tmp");
            tmp.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            Random rand = new Random();
            byte[] buf = new byte[8];
            rand.NextBytes(buf);
            ulong randnumgen = BitConverter.ToUInt64(buf, 0);
            string newTempFile = AppDomain.CurrentDomain.BaseDirectory + '\\' + "tmp" + '\\' + randnumgen.ToString("X16");
            if (Directory.Exists(newTempFile))
            {
                goto REGEN;
            }

            Directory.CreateDirectory(newTempFile);
            newTempFile += '\\' + Path.GetFileName(path);
            File.Copy(path, newTempFile);
#if !DEBUG
            try
            {
#endif
                if ((_rootNode = NodeFactory.FromFile(null, openTempFile = newTempFile)) != null)
                {
                    _rootPath = path;
                    if (!setRoot)
                    {
                        _rootPath = null;
                    }
                    else if (_rootNode is ARCNode && ((ARCNode)_rootNode).IsCharacter)
                    {
                        if (Program.RootPath.EndsWith(".pcs", StringComparison.OrdinalIgnoreCase) && (_rootNode.Compression.Equals("LZ77", StringComparison.OrdinalIgnoreCase) || _rootNode.Compression.Equals("None", StringComparison.OrdinalIgnoreCase)) && BrawlLib.Properties.Settings.Default.AutoCompressFighterPCS)
                        {
                            _rootNode.Compression = "ExtendedLZ77";
                        }
                        else if (Program.RootPath.EndsWith(".pac", StringComparison.OrdinalIgnoreCase) && !_rootNode.Compression.Equals("None", StringComparison.OrdinalIgnoreCase) && BrawlLib.Properties.Settings.Default.AutoDecompressFighterPAC)
                        {
                            _rootNode.Compression = "None";
                        }
                    }
                    MainForm.Instance.Reset();
                    MainForm.Instance.recentFileHandler.AddFile(path);
                    return true;
                }
                else
                {
                    openTempFile = null;
                    _rootPath = null;
                    Say("Unable to recognize input file.");
                    MainForm.Instance.Reset();
                }
#if !DEBUG
            }
            catch (Exception x) { Say(x.ToString()); }
#endif

            Close();

            return false;
        }

        public static bool Open(string path, string root, string folder = null, string openNode = null)
        {
            bool returnVal = Open(path, false);
            if (returnVal)
            {
                _rootPath = root;
                MainForm.Instance.Reset();
                if (openNode != null && folder != null)
                {
                    ResourceNode target = ResourceNode.FindNode(RootNode, folder, true);
                    if (target != null)
                    {
                        ResourceNode target2 = ResourceNode.FindNode(target, openNode, true);
                        if (target2 != null)
                        {
                            MainForm.Instance.TargetResource(target2);
                        }
                        else
                        {
                            MainForm.Instance.TargetResource(target);
                        }
                    }
                }
                else if (folder != null)
                {
                    ResourceNode target = ResourceNode.FindNode(RootNode, folder, true);
                    if (target != null)
                    {
                        MainForm.Instance.TargetResource(target);
                    }
                }
            }
            return returnVal;
        }

        public static unsafe void Scan(FileMap map, FileScanNode node)
        {
            using (ProgressWindow progress = new ProgressWindow(MainForm.Instance, "File Scanner", "Scanning for known file types, please wait...", true))
            {
                progress.TopMost = true;
                progress.Begin(0, map.Length, 0);

                byte* data = (byte*)map.Address;
                uint i = 0;
                do
                {
                    ResourceNode n = null;
                    DataSource d = new DataSource(&data[i], 0);
                    if ((n = NodeFactory.GetRaw(d)) != null)
                    {
                        if (!(n is MSBinNode))
                        {
                            n.Initialize(node, d);
                            try
                            {
                                i += (uint)n.WorkingSource.Length;
                            }
                            catch
                            {

                            }
                        }
                    }
                    progress.Update(i + 1);
                }
                while (++i + 4 < map.Length);

                progress.Finish();
            }
        }

        public static bool Save()
        {
            return Save(false);
        }

        public static bool Save(bool saveTemp)
        {
            bool restoreHex = false;
            if (_rootNode != null)
            {
#if !DEBUG
                try
                {
#endif

                    if (_rootPath == null && (!saveTemp || openTempFile == null))
                    {
                        return SaveAs();
                    }

                    bool force = Control.ModifierKeys == (Keys.Control | Keys.Shift);
                    if (!force && !_rootNode.IsDirty)
                    {
                        if (!saveTemp)
                        {
                            MessageBox.Show("No changes have been made.");
                        }

                        return false;
                    }

                    if (MainForm.Instance.ShowHex == true)
                    {
                        MainForm.Instance.ShowHex = false;
                        MainForm.Instance.Invalidate();
                        MainForm.Instance.resourceTree_SelectionChanged(MainForm.Instance, EventArgs.Empty);
                        restoreHex = true;
                    }

                    _rootNode.Merge(force);
                    _rootNode.IsDirty = false;
                    _rootNode.Export(saveTemp ? openTempFile : _rootPath, saveTemp);

                    if (restoreHex)
                    {
                        MainForm.Instance.ShowHex = true;
                        MainForm.Instance.Invalidate();
                        MainForm.Instance.resourceTree_SelectionChanged(MainForm.Instance, EventArgs.Empty);
                    }

                    return true;

#if !DEBUG
                }
                catch (Exception x) { Say(x.Message); _rootNode.SignalPropertyChange(); }
#endif
            }
            return false;
        }

        public static string ChooseFolder()
        {
            if (_folderDlg.ShowDialog() == DialogResult.OK)
            {
                return _folderDlg.SelectedPath;
            }

            return null;
        }

        public static int OpenFile(string filter, out string fileName) { return OpenFile(filter, out fileName, true); }
        public static int OpenFile(string filter, out string fileName, bool categorize)
        {
            _openDlg.Filter = filter;
            //_openDlg.AutoUpgradeEnabled = false;
#if !DEBUG
            try
            {
#endif
                if (_openDlg.ShowDialog() == DialogResult.OK)
                {
                    fileName = _openDlg.FileName;
                    if ((categorize) && (_openDlg.FilterIndex == 1))
                    {
                        return CategorizeFilter(_openDlg.FileName, filter);
                    }
                    else
                    {
                        return _openDlg.FilterIndex;
                    }
                }
#if !DEBUG
            }
            catch (Exception ex) { Say(ex.ToString()); }
#endif
            fileName = null;
            return 0;
        }
        public static int SaveFile(string filter, string name, out string fileName) { return SaveFile(filter, name, out fileName, true); }
        public static int SaveFile(string filter, string name, out string fileName, bool categorize)
        {
            int fIndex = 0;
            fileName = null;

            _saveDlg.Filter = filter;
            _saveDlg.FileName = name;
            _saveDlg.FilterIndex = 1;
            if (_saveDlg.ShowDialog() == DialogResult.OK)
            {
                if ((categorize) && (_saveDlg.FilterIndex == 1) && (Path.HasExtension(_saveDlg.FileName)))
                {
                    fIndex = CategorizeFilter(_saveDlg.FileName, filter);
                }
                else
                {
                    fIndex = _saveDlg.FilterIndex;
                }

                //Fix extension
                fileName = ApplyExtension(_saveDlg.FileName, filter, fIndex - 1);
            }

            return fIndex;
        }
        public static int CategorizeFilter(string path, string filter)
        {
            string ext = "*" + Path.GetExtension(path);

            string[] split = filter.Split('|');
            for (int i = 3; i < split.Length; i += 2)
            {
                foreach (string s in split[i].Split(';'))
                {
                    if (s.Equals(ext, StringComparison.OrdinalIgnoreCase))
                    {
                        return (i + 1) / 2;
                    }
                }
            }

            return 1;
        }
        public static string ApplyExtension(string path, string filter, int filterIndex)
        {
            if ((Path.HasExtension(path)) && (!int.TryParse(Path.GetExtension(path), out int tmp)))
            {
                return path;
            }

            int index = filter.IndexOfOccurance('|', filterIndex * 2);
            if (index == -1)
            {
                return path;
            }

            index = filter.IndexOf('.', index);
            int len = Math.Max(filter.Length, filter.IndexOfAny(new char[] { ';', '|' })) - index;

            string ext = filter.Substring(index, len);

            if (ext.IndexOf('*') >= 0)
            {
                return path;
            }

            return path + ext;
        }

        internal static bool SaveAs()
        {
            bool restoreHex = false;
            if (MainForm.Instance.RootNode is GenericWrapper)
            {
#if !DEBUG
                try
                {
#endif
                    if (MainForm.Instance.ShowHex == true)
                    {
                        MainForm.Instance.ShowHex = false;
                        MainForm.Instance.Invalidate();
                        MainForm.Instance.resourceTree_SelectionChanged(MainForm.Instance, EventArgs.Empty);
                        restoreHex = true;
                    }

                    GenericWrapper w = MainForm.Instance.RootNode as GenericWrapper;
                    string path = w.Export();
                    if (path != null)
                    {
                        _rootPath = path;
                        MainForm.Instance.UpdateName();

                        if (restoreHex)
                        {
                            MainForm.Instance.ShowHex = true;
                            MainForm.Instance.Invalidate();
                            MainForm.Instance.resourceTree_SelectionChanged(MainForm.Instance, EventArgs.Empty);
                        }
                        w.ResourceNode.IsDirty = false;
                        return true;
                    }
                    else
                    {
                        w.ResourceNode.IsDirty = true;
                        return false;
                    }
#if !DEBUG
                }
                catch (Exception x) { Say(x.Message); _rootNode.SignalPropertyChange(); }
                //finally { }
#endif
            }

            if (restoreHex)
            {
                MainForm.Instance.ShowHex = true;
                MainForm.Instance.Invalidate();
                MainForm.Instance.resourceTree_SelectionChanged(MainForm.Instance, EventArgs.Empty);
            }
            return false;
        }

        public static bool CanRunGithubApp(bool showMessages, out string path)
        {
            path = System.Windows.Forms.Application.StartupPath + "\\Updater.exe";
            if (!File.Exists(path))
            {
                if (showMessages)
                {
                    MessageBox.Show("Could not find " + path);
                }

                return false;
            }

            using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).
                OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\"))
            {
                object o = ndpKey.GetValue("Release");
                if (o == null)
                {
                    return false;
                }

                int releaseKey = Convert.ToInt32(o);
                if (releaseKey < 378389)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool CanRunDiscordRPC()
        {
            string path = System.Windows.Forms.Application.StartupPath + "\\discord-rpc.dll";
            return File.Exists(path);
        }
    }
}
