﻿using BrawlLib.SSBB.ResourceNodes;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.NoEditEntry)]
    internal class NoEditEntryWrapper : GenericWrapper
    {
        private static readonly ContextMenuStrip _menu;
        static NoEditEntryWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            //_menu.Items.Add(new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {

        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {

        }
        public NoEditEntryWrapper() { ContextMenuStrip = _menu; }
    }

    [NodeWrapper(ResourceType.NoEditFolder)]
    internal class NoEditFolderWrapper : GenericWrapper
    {
        private static readonly ContextMenuStrip _menu;
        static NoEditFolderWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            //_menu.Items.Add(new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {

        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {

        }
        public NoEditFolderWrapper() { ContextMenuStrip = _menu; }
    }
}
