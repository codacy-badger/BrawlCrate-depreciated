using System;
using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;
using System.Windows.Forms;
using System.ComponentModel;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.STPM)]
    class STPMWrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;
        static STPMWrapper()
        {
            _menu = new ContextMenuStrip();

            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Add New Entry", null, NewEntryAction, Keys.Control | Keys.J));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Replace Camera", null, ReplaceCameraAction, null));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripMenuItem("&Duplicate", null, DuplicateAction, Keys.Control | Keys.D));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }
        protected static void NewEntryAction(object sender, EventArgs e) { GetInstance<STPMWrapper>().NewEntry(); }
        protected static void ReplaceCameraAction(object sender, EventArgs e) { GetInstance<STPMWrapper>().ReplaceCamera(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[5].Enabled = _menu.Items[6].Enabled = _menu.Items[7].Enabled = _menu.Items[9].Enabled = _menu.Items[10].Enabled = _menu.Items[13].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            STPMWrapper w = GetInstance<STPMWrapper>();
            _menu.Items[5].Enabled = _menu.Items[7].Enabled = _menu.Items[13].Enabled = w.Parent != null;
            _menu.Items[6].Enabled = ((w._resource.IsDirty) || (w._resource.IsBranch));
            _menu.Items[9].Enabled = w.PrevNode != null;
            _menu.Items[10].Enabled = w.NextNode != null;
        }
        #endregion
        
        public override string ExportFilter { get { return FileFilters.STPM; } }

        public override ResourceNode Duplicate()
        {
            if (_resource._parent == null)
            {
                return null;
            }
            _resource.Rebuild();
            STPMNode newNode = NodeFactory.FromAddress(null, _resource.WorkingUncompressed.Address, _resource.WorkingUncompressed.Length) as STPMNode;
            _resource._parent.InsertChild(newNode, true, _resource.Index + 1);
            newNode.Populate();
            newNode.FileType = ((STPMNode)_resource).FileType;
            newNode.FileIndex = ((STPMNode)_resource).FileIndex;
            newNode.GroupID = ((STPMNode)_resource).GroupID;
            newNode.RedirectIndex = ((STPMNode)_resource).RedirectIndex;
            newNode.Name = _resource.Name;
            return newNode;
        }

        public void NewEntry()
        {
            STPMEntryNode node = new STPMEntryNode() { Name = "NewEntry" };
            _resource.AddChild(node);
        }
        public void ReplaceCamera()
        {
            bool portPauseCam = true;
            STPMNode external = null;
            OpenFileDialog o = new OpenFileDialog();
            o.Filter = "Stage Parameters (*.stpm)|*.stpm";
            o.Title = "Please select an STPM file to port the camera from.";
            if (o.ShowDialog() == DialogResult.OK)
                if ((external = (STPMNode)NodeFactory.FromFile(null, o.FileName)) != null)
                {
                    if(_resource.Children.Count > 0 && external.Children.Count > 0)
                    {
                        // Port exclusively camera variables
                        ((STPMEntryNode)(_resource.Children[0])).CameraFOV = ((STPMEntryNode)(external.Children[0])).CameraFOV;

                        ((STPMEntryNode)(_resource.Children[0])).MinimumZ = ((STPMEntryNode)(external.Children[0])).MinimumZ;
                        ((STPMEntryNode)(_resource.Children[0])).MaximumZ = ((STPMEntryNode)(external.Children[0])).MaximumZ;

                        ((STPMEntryNode)(_resource.Children[0])).MinimumTilt = ((STPMEntryNode)(external.Children[0])).MinimumTilt;
                        ((STPMEntryNode)(_resource.Children[0])).MaximumTilt = ((STPMEntryNode)(external.Children[0])).MaximumTilt;

                        ((STPMEntryNode)(_resource.Children[0])).HorizontalRotationFactor = ((STPMEntryNode)(external.Children[0])).HorizontalRotationFactor;
                        ((STPMEntryNode)(_resource.Children[0])).VerticalRotationFactor = ((STPMEntryNode)(external.Children[0])).VerticalRotationFactor;

                        ((STPMEntryNode)(_resource.Children[0])).CharacterBubbleBufferMultiplier = ((STPMEntryNode)(external.Children[0])).CharacterBubbleBufferMultiplier;

                        ((STPMEntryNode)(_resource.Children[0])).CameraSpeed = ((STPMEntryNode)(external.Children[0])).CameraSpeed;

                        ((STPMEntryNode)(_resource.Children[0])).StarKOCamTilt = ((STPMEntryNode)(external.Children[0])).StarKOCamTilt;
                        ((STPMEntryNode)(_resource.Children[0])).FinalSmashCamTilt = ((STPMEntryNode)(external.Children[0])).FinalSmashCamTilt;

                        ((STPMEntryNode)(_resource.Children[0])).CameraRight = ((STPMEntryNode)(external.Children[0])).CameraRight;
                        ((STPMEntryNode)(_resource.Children[0])).CameraLeft = ((STPMEntryNode)(external.Children[0])).CameraLeft;

                        if (portPauseCam)
                        {
                            ((STPMEntryNode)(_resource.Children[0])).PauseCamX = ((STPMEntryNode)(external.Children[0])).PauseCamX;
                            ((STPMEntryNode)(_resource.Children[0])).PauseCamY = ((STPMEntryNode)(external.Children[0])).PauseCamY;
                            ((STPMEntryNode)(_resource.Children[0])).PauseCamZ = ((STPMEntryNode)(external.Children[0])).PauseCamZ;
                            ((STPMEntryNode)(_resource.Children[0])).PauseCamAngle = ((STPMEntryNode)(external.Children[0])).PauseCamAngle;
                            ((STPMEntryNode)(_resource.Children[0])).PauseCamZoomIn = ((STPMEntryNode)(external.Children[0])).PauseCamZoomIn;
                            ((STPMEntryNode)(_resource.Children[0])).PauseCamZoomDefault = ((STPMEntryNode)(external.Children[0])).PauseCamZoomDefault;
                            ((STPMEntryNode)(_resource.Children[0])).PauseCamZoomOut = ((STPMEntryNode)(external.Children[0])).PauseCamZoomOut;
                            ((STPMEntryNode)(_resource.Children[0])).PauseCamRotYMin = ((STPMEntryNode)(external.Children[0])).PauseCamRotYMin;
                            ((STPMEntryNode)(_resource.Children[0])).PauseCamRotYMax = ((STPMEntryNode)(external.Children[0])).PauseCamRotYMax;
                            ((STPMEntryNode)(_resource.Children[0])).PauseCamRotXMin = ((STPMEntryNode)(external.Children[0])).PauseCamRotXMin;
                            ((STPMEntryNode)(_resource.Children[0])).PauseCamRotXMax = ((STPMEntryNode)(external.Children[0])).PauseCamRotXMax;
                        }

                        ((STPMEntryNode)(_resource.Children[0])).FixedCamX = ((STPMEntryNode)(external.Children[0])).FixedCamX;
                        ((STPMEntryNode)(_resource.Children[0])).FixedCamY = ((STPMEntryNode)(external.Children[0])).FixedCamY;
                        ((STPMEntryNode)(_resource.Children[0])).FixedCamZ = ((STPMEntryNode)(external.Children[0])).FixedCamZ;
                        ((STPMEntryNode)(_resource.Children[0])).FixedCamAngle = ((STPMEntryNode)(external.Children[0])).FixedCamAngle;
                        ((STPMEntryNode)(_resource.Children[0])).FixedHorizontalAngle = ((STPMEntryNode)(external.Children[0])).FixedHorizontalAngle;
                        ((STPMEntryNode)(_resource.Children[0])).FixedVerticalAngle = ((STPMEntryNode)(external.Children[0])).FixedVerticalAngle;

                    }
                }
        }

        public STPMWrapper() { ContextMenuStrip = _menu; }
    }
}
