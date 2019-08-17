using BrawlLib.SSBB.ResourceNodes;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate
{
    [NodeWrapper(ResourceType.BLOC)]
    internal class BLOCWrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;

        static BLOCWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Ne&w", null,
                new ToolStripMenuItem("GSND Archive", null, NewGSNDAction),
                new ToolStripMenuItem("ADSJ Stepjump File", null, NewADSJAction),
                new ToolStripMenuItem("GDOR Adventure Door File", null, NewGDORAction),
                new ToolStripMenuItem("GDBF Factory Door File", null, NewGDBFAction),
                new ToolStripMenuItem("GWAT Swimmable Water File", null, NewGWATAction),
                new ToolStripMenuItem("GEG1 Enemy File", null, NewGEG1Action),
                new ToolStripMenuItem("GCAM Animated Camera File", null, NewGCAMAction),
                new ToolStripMenuItem("GITM Fighter Trophy File", null, NewGITMAction)
            ));

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

        protected static void NewGSNDAction(object sender, EventArgs e)
        {
            GetInstance<BLOCWrapper>().NewGSND();
        }

        protected static void NewADSJAction(object sender, EventArgs e)
        {
            GetInstance<BLOCWrapper>().NewADSJ();
        }

        protected static void NewGDORAction(object sender, EventArgs e)
        {
            GetInstance<BLOCWrapper>().NewGDOR();
        }

        protected static void NewGDBFAction(object sender, EventArgs e)
        {
            GetInstance<BLOCWrapper>().NewGDBF();
        }

        protected static void NewGWATAction(object sender, EventArgs e)
        {
            GetInstance<BLOCWrapper>().NewGWAT();
        }

        protected static void NewGEG1Action(object sender, EventArgs e)
        {
            GetInstance<BLOCWrapper>().NewGEG1();
        }

        protected static void NewGCAMAction(object sender, EventArgs e)
        {
            GetInstance<BLOCWrapper>().NewGCAM();
        }

        protected static void NewGITMAction(object sender, EventArgs e)
        {
            GetInstance<BLOCWrapper>().NewGITM();
        }

        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[8].Enabled = true;
        }

        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            BLOCWrapper w = GetInstance<BLOCWrapper>();

            _menu.Items[8].Enabled = w.Parent != null;
        }

        #endregion

        public override string ExportFilter => "BLOC Adventure Archive (*.BLOC)|*.bloc";

        public BLOCWrapper()
        {
            ContextMenuStrip = _menu;
        }

        public GSNDNode NewGSND()
        {
            GSNDNode node = new GSNDNode() {Name = _resource.FindName("NewGSND")};
            _resource.AddChild(node);

            BaseWrapper w = FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }

        public ADSJNode NewADSJ()
        {
            ADSJNode node = new ADSJNode() {Name = _resource.FindName("NewADSJ")};
            _resource.AddChild(node);

            BaseWrapper w = FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }

        public GDORNode NewGDOR()
        {
            GDORNode node = new GDORNode() {Name = _resource.FindName("NewGDOR")};
            _resource.AddChild(node);

            BaseWrapper w = FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }

        public GDBFNode NewGDBF()
        {
            GDBFNode node = new GDBFNode() {Name = _resource.FindName("NewGDBF")};
            _resource.AddChild(node);

            BaseWrapper w = FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }

        public GWATNode NewGWAT()
        {
            GWATNode node = new GWATNode() {Name = _resource.FindName("NewGWAT")};
            _resource.AddChild(node);

            BaseWrapper w = FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }

        public GEG1Node NewGEG1()
        {
            GEG1Node node = new GEG1Node() {Name = _resource.FindName("NewGEG1")};
            _resource.AddChild(node);

            BaseWrapper w = FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }

        public GCAMNode NewGCAM()
        {
            GCAMNode node = new GCAMNode() {Name = _resource.FindName("NewGCAM")};
            _resource.AddChild(node);

            BaseWrapper w = FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }

        public GITMNode NewGITM()
        {
            GITMNode node = new GITMNode() {Name = _resource.FindName("NewGITM")};
            _resource.AddChild(node);

            BaseWrapper w = FindResource(node, false);
            w.EnsureVisible();
            w.TreeView.SelectedNode = w;
            return node;
        }

        public override void OnExport(string outPath, int filterIndex)
        {
            ((BLOCNode) _resource).Export(outPath);
        }
    }
}