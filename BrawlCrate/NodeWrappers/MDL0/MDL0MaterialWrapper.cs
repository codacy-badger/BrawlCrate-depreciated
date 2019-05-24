﻿using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Graphics;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.MDL0Material)]
    internal class MDL0MaterialWrapper : GenericWrapper
    {
        #region Menu

        private static readonly ContextMenuStrip _menu;
        static MDL0MaterialWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Add New Reference", null, CreateAction, Keys.Control | Keys.Alt | Keys.N));
            _menu.Items.Add(new ToolStripMenuItem("Export GLSL Shader", null, ExportShaderAction));
            //_menu.Items.Add(new ToolStripMenuItem("Add shadow diffuse", null, AddShadowDiffuseAction));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }
        protected static void CreateAction(object sender, EventArgs e) { GetInstance<MDL0MaterialWrapper>().CreateRef(); }
        protected static void ExportShaderAction(object sender, EventArgs e) { GetInstance<MDL0MaterialWrapper>().ExportShader(); }
        protected static void AddShadowDiffuseAction(object sender, EventArgs e) { GetInstance<MDL0MaterialWrapper>().AddShadowDiffuse(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[4].Enabled = _menu.Items[5].Enabled = _menu.Items[7].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            MDL0MaterialWrapper w = GetInstance<MDL0MaterialWrapper>();
            _menu.Items[4].Enabled = w.PrevNode != null;
            _menu.Items[5].Enabled = w.NextNode != null;
            _menu.Items[7].Enabled = w._resource.Children.Count < 8; //8 mat refs max!
        }
        private void CreateRef()
        {
            if (_resource.Children.Count < 8)
            {
                MDL0MaterialRefNode node = new MDL0MaterialRefNode();
                _resource.AddChild(node);
                node.Default();
                _resource.SignalPropertyChange();

                //if (node.Model.AutoMetalMaterials && ((MDL0MaterialNode)node.Parent).MetalMaterial != null)
                //    ((MDL0MaterialNode)node.Parent).MetalMaterial.UpdateAsMetal();

                Nodes[Nodes.Count - 1].EnsureVisible();
                //TreeView.SelectedNode = Nodes[Nodes.Count - 1];
            }
        }

        private void AddShadowDiffuse()
        {
            Expand();
            ((MDL0MaterialNode)_resource).AddShadowDiffuse(false, false);
        }

        private void ExportShader()
        {
            MDL0MaterialNode mat = _resource as MDL0MaterialNode;

            ShaderGenerator.SetTarget(mat);

            ShaderGenerator.UsePixelLighting = MessageBox.Show(MainForm.Instance, "Use per-pixel lighting?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

            SaveFileDialog s = new SaveFileDialog
            {
                Filter = "Text File (*.txt)|*.txt",
                FileName = _resource.Name + " vertex shader",
                Title = "Choose a place to save the vertex shader"
            };
            if (s.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(s.FileName, ShaderGenerator.GenVertexShader().Replace("\n", Environment.NewLine));
            }

            s.Filter = "Text File (*.txt)|*.txt";
            s.FileName = _resource.Name + " fragment shader";
            s.Title = "Choose a place to save the fragment shader";
            if (s.ShowDialog() == DialogResult.OK)
            {
                string m = ShaderGenerator.GenMaterialFragShader();
                string[] t = ShaderGenerator.GenTEVFragShader();
                System.IO.File.WriteAllText(s.FileName, ShaderGenerator.CombineFragShader(m, t, mat.ActiveShaderStages).Replace("\n", Environment.NewLine));
            }

            ShaderGenerator.ClearTarget();
            ShaderGenerator._forceRecompile = false;
        }
        #endregion

        public override string ExportFilter => FileFilters.MDL0Material;

        public MDL0MaterialWrapper() { ContextMenuStrip = _menu; }
    }
}
