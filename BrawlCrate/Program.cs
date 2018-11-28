using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using BrawlLib.IO;
using BrawlLib.SSBB.ResourceNodes;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using BrawlLib.BrawlCrate;
using System.Threading.Tasks;
using System.Configuration;

namespace BrawlCrate
{
    static class Program
    {
        //Make sure this matches the tag name of the release on github exactly
        public static readonly string TagName = "BrawlCrate_v0.17Hotfix3";
        public static readonly string UpdateMessage = "Updated to BrawlCrate v0.17 Hotfix 2! This release:\n" +
			"\n- Adds a fancy new splash screen on boot" +
            "\n- Allows switching to the BrawlCrate Canary update track (check the settings if interested)" +
            "\n- Updates various aspects of the Model Viewer backend, improving performance" +
            "\n- Fixes various bugs and improves performance with the updater" +
            "\n- (Hotfix 1) Fixes crashes when viewing hex on older versions of Windows" +
            "\n- (Hotfix 2) Fixes hex viewer crash on BRSTM creation" +
			"\n- (Hotfix 3) Fixes DPI resize when viewing models" +
            "\n\nFull changelog can be found in the installation folder:\n" + AppDomain.CurrentDomain.BaseDirectory + "Changelog.txt";

        public static readonly string AssemblyTitle;
        public static readonly string AssemblyDescription;
        public static readonly string AssemblyCopyright;
        public static readonly string FullPath;
        public static readonly string BrawlLibTitle;

        private static OpenFileDialog _openDlg;
        private static SaveFileDialog _saveDlg;
        private static FolderBrowserDialog _folderDlg;

        internal static ResourceNode _rootNode;
        public static ResourceNode RootNode { get { return _rootNode; } set { _rootNode = value; MainForm.Instance.Reset(); } }
        internal static string _rootPath;
        public static string RootPath { get { return _rootPath; } }

        static Program()
        {
            Application.EnableVisualStyles();

            AssemblyTitle = ((AssemblyTitleAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0]).Title;
            AssemblyDescription = ((AssemblyDescriptionAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0]).Description;
            AssemblyCopyright = ((AssemblyCopyrightAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0]).Copyright;
            FullPath = Process.GetCurrentProcess().MainModule.FileName;
            BrawlLibTitle = ((AssemblyTitleAttribute)Assembly.GetAssembly(typeof(ResourceNode)).GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0]).Title;

            _openDlg = new OpenFileDialog();
            _saveDlg = new SaveFileDialog();
            _folderDlg = new FolderBrowserDialog();
            
            StageNameGenerators.GenerateList();
            FighterNameGenerators.GenerateLists();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.ThreadException += Application_ThreadException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            List<ResourceNode> dirty = GetDirtyFiles();
            Exception ex = e.Exception;
            IssueDialog d = new IssueDialog(ex, dirty);
            d.ShowDialog();
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
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

            foreach (var control in ModelEditControl.Instances)
                foreach (ResourceNode r in control.rightPanel.pnlOpenedFiles.OpenedFiles)
                    if (r.IsDirty && !dirty.Contains(r))
                        dirty.Add(r);

            if (_rootNode != null && _rootNode.IsDirty && !dirty.Contains(_rootNode))
                dirty.Add(_rootNode);

            return dirty;
        }

        [STAThread]
        public static void Main(string[] args)
        {
            SplashForm s = new SplashForm();
            s.Show();
            s.Focus();
            bool firstBoot = false;
            if (BrawlCrate.Properties.Settings.Default.UpdateSettings)
            {
                foreach (var _Assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (var _Type in _Assembly.GetTypes())
                    {
                        if (_Type.Name == "Settings" && typeof(SettingsBase).IsAssignableFrom(_Type))
                        {
                            var settings = (ApplicationSettingsBase)_Type.GetProperty("Default").GetValue(null, null);
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
                // Canary setting should be initialized as false
                BrawlCrate.Properties.Settings.Default.DownloadCanaryBuilds = false;
                // Ensure settings only get updated once
                BrawlCrate.Properties.Settings.Default.UpdateSettings = false;
                BrawlCrate.Properties.Settings.Default.Save();
            }
            if (!BrawlCrate.Properties.Settings.Default.DownloadCanaryBuilds)
            {
                try
                {
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Update.exe"))
                        File.Delete(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Update.exe");
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "temp.exe"))
                        File.Delete(AppDomain.CurrentDomain.BaseDirectory + '\\' + "temp.exe");
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Update.bat"))
                        File.Delete(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Update.bat");
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "StageBox.exe"))
                        File.Delete(AppDomain.CurrentDomain.BaseDirectory + '\\' + "StageBox.exe");
                }
                catch
                {

                }
            }

            if (args.Length >= 1)
            {
                if (args[0].Equals("/gct", StringComparison.InvariantCultureIgnoreCase))
                {
                    GCTEditor editor = new GCTEditor();
                    if (args.Length >= 2)
                        editor.TargetNode = GCTEditor.LoadGCT(args[1]);
                    s.Close();
                    Application.Run(editor);
                    return;
                }
                else if (args[0].EndsWith(".gct", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (args.Length >= 2)
                    {
                        if (args[1] == "-Canary")
                        {
                            // Set Canary build active
                            BrawlCrate.Properties.Settings.Default.DownloadCanaryBuilds = true;
                            BrawlCrate.Properties.Settings.Default.CheckUpdatesAtStartup = true;
                            BrawlCrate.Properties.Settings.Default.UpdateAutomatically = true;
                            BrawlCrate.Properties.Settings.Default.Save();
                        }
                        else if (args[1] == "-Stable")
                        {
                            // Set Stable build active
                            BrawlCrate.Properties.Settings.Default.DownloadCanaryBuilds = false;
                            BrawlCrate.Properties.Settings.Default.Save();
                        }
                    }
                    GCTEditor editor = new GCTEditor();
                    editor.TargetNode = GCTEditor.LoadGCT(args[0]);
                    s.Close();
                    Application.Run(editor);
                    return;
                }
            }

            try
            {
                if (args.Length >= 2)
                {
                    if (args[1] == "-Canary")
                    {
                        // Set Canary build active
                        BrawlCrate.Properties.Settings.Default.DownloadCanaryBuilds = true;
                        BrawlCrate.Properties.Settings.Default.CheckUpdatesAtStartup = true;
                        BrawlCrate.Properties.Settings.Default.UpdateAutomatically = true;
                        BrawlCrate.Properties.Settings.Default.Save();
                    }
                    else if (args[1] == "-Stable")
                    {
                        // Set Stable build active
                        BrawlCrate.Properties.Settings.Default.DownloadCanaryBuilds = false;
                        BrawlCrate.Properties.Settings.Default.Save();
                    }
                }
                if (args.Length >= 1)
                {
                    if (args[0] == "-Canary")
                    {
                        // Set Canary build active
                        BrawlCrate.Properties.Settings.Default.DownloadCanaryBuilds = true;
                        BrawlCrate.Properties.Settings.Default.CheckUpdatesAtStartup = true;
                        BrawlCrate.Properties.Settings.Default.UpdateAutomatically = true;
                        BrawlCrate.Properties.Settings.Default.Save();
                    }
                    else if (args[0] == "-Stable")
                    {
                        // Set Stable build active
                        BrawlCrate.Properties.Settings.Default.DownloadCanaryBuilds = false;
                        BrawlCrate.Properties.Settings.Default.Save();
                    }
                    else if (args[0] != "null")
                        Open(args[0]);
                }
                if(args.Length >= 2 && args[1] != "-Canary" && args[1] != "-Stable")
                {
                    ResourceNode target = ResourceNode.FindNode(RootNode, args[1], true);
                    if (target != null)
                        MainForm.Instance.TargetResource(target);
                    else
                        Say(String.Format("Error: Unable to find node or path '{0}'!", args[1]));
                }

#if !DEBUG //Don't need to see this every time a debug build is compiled
                if (MainForm.Instance.CheckUpdatesOnStartup)
                    MainForm.Instance.CheckUpdates(false);
                // Show changelog if this is the first time opening this release, and the message wasn't seen 
                if (BrawlCrate.Properties.Settings.Default.DownloadCanaryBuilds)
                {
                    if(Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary") && File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "Canary" + '\\' + "Old"))
                        MainForm.Instance.ShowCanaryChangelog();
                }
                else if (BrawlCrate.Properties.Settings.Default.UpdateAutomatically && firstBoot)
                {
                    Task.Factory.StartNew(() =>
                    {
                        MessageBox.Show(Program.UpdateMessage);
                    });
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
            finally {
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

        public static void Say(string msg)
        {
            MessageBox.Show(msg);
        }

        public static bool New<T>() where T : ResourceNode
        {
            if (!Close())
                return false;

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
                var control = ModelEditControl.Instances[0];
                if (control.ParentForm != null)
                {
                    control.ParentForm.Close();
                    if (!control.IsDisposed)
                        return false;
                }
                else if (!control.Close())
                    return false;
            }

            if (_rootNode != null)
            {
                if (_rootNode.IsDirty && !force)
                {
                    DialogResult res = MessageBox.Show("Save changes?", "Closing", MessageBoxButtons.YesNoCancel);
                    if ((res == DialogResult.Yes && !Save()) || res == DialogResult.Cancel)
                        return false;
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
            if (String.IsNullOrEmpty(path))
                return false;
            
            if (!File.Exists(path))
            {
                Say("File does not exist.");
                return false;
            }

            if (path.EndsWith(".gct", StringComparison.InvariantCultureIgnoreCase))
            {
                GCTEditor editor = new GCTEditor();
                editor.TargetNode = GCTEditor.LoadGCT(path);
                editor.Show();
                return true;
            }

            if (!Close())
                return false;
REGEN:
            DirectoryInfo tmp = Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + '\\' + "tmp");
            tmp.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            Random rand = new Random();
            byte[] buf = new byte[8];
            rand.NextBytes(buf);
            ulong randnumgen = BitConverter.ToUInt64(buf, 0);
            string newTempFile = AppDomain.CurrentDomain.BaseDirectory + '\\' + "tmp" + '\\' + randnumgen.ToString("X16");
            if (Directory.Exists(newTempFile))
                goto REGEN;
            Directory.CreateDirectory(newTempFile);
            newTempFile += '\\' + Path.GetFileNameWithoutExtension(path);
            File.Copy(path, newTempFile);
#if !DEBUG
            try
            {
            #endif
                if ((_rootNode = NodeFactory.FromFile(null, openTempFile = newTempFile)) != null)
                {
                    _rootPath = path;
                    if(!setRoot)
                        _rootPath = null;
                    MainForm.Instance.Reset();
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

        public static bool Open(string path, string root)
        {
            bool returnVal = Open(path, false);
            if (returnVal)
            {
                _rootPath = root;
                MainForm.Instance.Reset();
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
                    return SaveAs();

                bool force = Control.ModifierKeys == (Keys.Control | Keys.Shift);
                if (!force && !_rootNode.IsDirty)
                {
                    MessageBox.Show("No changes have been made.");
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
                _rootNode.Export(saveTemp ? openTempFile : _rootPath);
                
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
                return _folderDlg.SelectedPath;
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
                        return CategorizeFilter(_openDlg.FileName, filter);
                    else
                        return _openDlg.FilterIndex;
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
                    fIndex = CategorizeFilter(_saveDlg.FileName, filter);
                else
                    fIndex = _saveDlg.FilterIndex;

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
                foreach (string s in split[i].Split(';'))
                    if (s.Equals(ext, StringComparison.OrdinalIgnoreCase))
                        return (i + 1) / 2;
            return 1;
        }
        public static string ApplyExtension(string path, string filter, int filterIndex)
        {
            int tmp;
            if ((Path.HasExtension(path)) && (!int.TryParse(Path.GetExtension(path), out tmp)))
                return path;

            int index = filter.IndexOfOccurance('|', filterIndex * 2);
            if (index == -1)
                return path;

            index = filter.IndexOf('.', index);
            int len = Math.Max(filter.Length, filter.IndexOfAny(new char[] { ';', '|' })) - index;

            string ext = filter.Substring(index, len);

            if (ext.IndexOf('*') >= 0)
                return path;

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
                    w.ResourceNode.IsDirty = false;
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
                    MessageBox.Show("Could not find " + path);
                return false;
            }

            using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).
                OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\"))
            {
                object o = ndpKey.GetValue("Release");
                if (o == null)
                    return false;

                int releaseKey = Convert.ToInt32(o);
                if (releaseKey < 378389)
                    return false;
            }
            return true;
        }
    }
}
