using System;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib;
using System.Windows.Forms;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.RSAR)]
    class RSARWrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;
        static RSARWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Import Sawnd", null, ImportSawndzAction, Keys.Control | Keys.I));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }
        protected static void ImportSawndzAction(object sender, EventArgs e) { GetInstance<RSARWrapper>().ImportSawndz(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[0].Enabled = _menu.Items[0].Visible = _menu.Items[1].Visible = _menu.Items[2].Enabled = _menu.Items[4].Enabled = _menu.Items[6].Enabled = _menu.Items[7].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            RSARWrapper w = GetInstance<RSARWrapper>();
            RSARSoundNode n = w._resource as RSARSoundNode;
            _menu.Items[0].Enabled = _menu.Items[0].Visible = _menu.Items[1].Visible = (w._resource.Parent == null && File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "sawndz.exe"));
            _menu.Items[2].Enabled = w.Parent != null;
            _menu.Items[4].Enabled = ((w._resource.IsDirty) || (w._resource.IsBranch));
            _menu.Items[6].Enabled = w.PrevNode != null;
            _menu.Items[7].Enabled = w.NextNode != null;
        }

        #endregion

        public RSARWrapper() { ContextMenuStrip = _menu; }

        public override string ExportFilter { get { return FileFilters.RSAR; } }

        public void ImportSawndz()
        {
            if (Parent != null || !File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "sawndz.exe"))
                return;
            Program.Save(true);
            string inPath = "";
            int index = Program.OpenFile("Brawl SFX files (*.sawnd)|*.sawnd", out inPath);
            if (index != 0)
            {
                string fileRoot = Program.RootPath;
                string openFile = Program.openTempFile;
                Program.Close(true);
                insertSawnd(inPath, openFile);
                MainForm.Instance.Reset();
                Program.Open(openFile, fileRoot);
                MainForm.Instance.RootNode.ResourceNode.SignalPropertyChange();
            }
        }

        public static void insertSawnd(string sawndfileName, string brsarfileName)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "sawnd.sawnd"))
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + '\\' + "sawnd.sawnd");
            File.Copy(sawndfileName, AppDomain.CurrentDomain.BaseDirectory + '\\' + "sawnd.sawnd");
            runSawndzWithArgs("sawnd" + " \"" + brsarfileName + "\"");
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "sawnd.sawnd"))
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + '\\' + "sawnd.sawnd");
        }

        public static Process p;
        static void runSawndzWithArgs(string args)
        {
            try
            {
                p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.EnableRaisingEvents = true;
                p.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + '\\' + "sawndz.exe";
                p.StartInfo.Arguments = args;
                p.Start();
                while ((!p.HasExited || !p.StandardOutput.EndOfStream))
                {

                    char[] buffer = new char[1];
                    int count = p.StandardOutput.Read(buffer, 0, 1);
                    Console.Write(buffer);
                }
                if (!p.HasExited)
                    p.WaitForExit();
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                //If the process is still running kill it
                if (p != null && !p.HasExited)
                {
                    p.Kill();
                    p = null;
                }
            }
        }
    }
}
