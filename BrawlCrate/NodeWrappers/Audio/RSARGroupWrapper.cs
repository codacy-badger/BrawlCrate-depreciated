using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.RSARGroup)]
    internal class RSARGroupWrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;
        static RSARGroupWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Export Sawnd", null, ExportSawndzAction, Keys.Control | Keys.I));
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
        protected static void ExportSawndzAction(object sender, EventArgs e) { GetInstance<RSARGroupWrapper>().ExportSawndz(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[0].Enabled = _menu.Items[0].Visible = _menu.Items[1].Visible = _menu.Items[2].Enabled = _menu.Items[4].Enabled = _menu.Items[6].Enabled = _menu.Items[7].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            RSARGroupWrapper w = GetInstance<RSARGroupWrapper>();
            _menu.Items[0].Enabled = _menu.Items[0].Visible = _menu.Items[1].Visible = (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "sawndz.exe"));
            _menu.Items[2].Enabled = w.Parent != null;
            _menu.Items[4].Enabled = ((w._resource.IsDirty) || (w._resource.IsBranch));
            _menu.Items[6].Enabled = w.PrevNode != null;
            _menu.Items[7].Enabled = w.NextNode != null;
        }

        #endregion

        public RSARGroupWrapper() { ContextMenuStrip = _menu; }

        public void ExportSawndz()
        {
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "sawndz.exe"))
            {
                return;
            }

            string target = _resource.Name;
            int groupID = ((RSARGroupNode)_resource).StringId;
            bool stuffChanged = Program.Save(true);
            int index = Program.SaveFile("Brawl SFX files (*.sawnd)|*.sawnd", groupID.ToString(), out string outPath);
            if (index != 0)
            {
                string fileRoot = Program.RootPath;
                string openFile = Program.openTempFile;
                Program.Close(true);
                exportSawnd(outPath, openFile, groupID);
                MainForm.Instance.Reset();
                Program.Open(openFile, fileRoot, "group", target);
                if (stuffChanged && MainForm.Instance.RootNode != null && MainForm.Instance.RootNode.ResourceNode != null)
                {
                    MainForm.Instance.RootNode.ResourceNode.SignalPropertyChange();
                }
            }
        }

        public static void exportSawnd(string sawndfileName, string bRSARGroupfileName, int groupID)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "sawnd.sawnd"))
            {
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + '\\' + "sawnd.sawnd");
            }

            runSawndzWithArgs("sawndcreate " + groupID + " \"" + bRSARGroupfileName + "\"");
            if (File.Exists(sawndfileName))
            {
                File.Delete(sawndfileName);
            }

            File.Move("sawnd.sawnd", sawndfileName);
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + '\\' + "sawnd.sawnd"))
            {
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + '\\' + "sawnd.sawnd");
            }
        }

        public static Process p;

        private static void runSawndzWithArgs(string args)
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
                {
                    p.WaitForExit();
                }

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
