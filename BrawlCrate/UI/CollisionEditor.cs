using System.Linq;
using BrawlLib.SSBB.ResourceNodes;
using System.ComponentModel;
using BrawlLib.SSBBTypes;
using BrawlLib.OpenGL;
using System.Collections.Generic;
using System.Drawing;
using BrawlLib.Modeling;
using OpenTK.Graphics.OpenGL;

namespace System.Windows.Forms
{
    public unsafe class CollisionEditor : UserControl
    {
        #region Designer

        public ModelPanel _modelPanel;
        protected bool _errorChecking = true;
        protected SplitContainer undoToolStrip;
        protected SplitContainer redoToolStrip;
        protected CheckBox chkAllModels;
        protected Panel pnlPlaneProps;
        protected Label label5;
        protected Label labelType;
        protected ComboBox cboMaterial;
        protected Panel pnlObjProps;
        protected ToolStrip toolStrip1;
        protected ToolStripButton btnSplit;
        protected ToolStripButton btnMerge;
        protected ToolStripButton btnDelete;
        protected ContextMenuStrip contextMenuStrip1;
        protected ContextMenuStrip contextMenuStrip3;
        private IContainer components;
        protected ToolStripMenuItem snapToolStripMenuItem;
        protected Panel panel1;
        protected TrackBar trackBar1;
        protected Button btnResetRot;
        protected ToolStripButton btnResetCam;
        protected GroupBox groupBoxFlags1;
        protected CheckBox chkFallThrough;
        protected GroupBox groupBoxFlags2;
        protected CheckBox chkNoWalljump;
        protected CheckBox chkRightLedge;
        protected CheckBox chkTypeCharacters;
        protected CheckBox chkTypeItems;
        protected CheckBox chkTypePokemonTrainer;
        protected CheckBox chkTypeRotating;
        
        // Advanced unknown flags
        protected GroupBox groupBoxTargets;
        protected CheckBox chkFlagUnknown1;
        protected CheckBox chkFlagUnknown2;
        protected CheckBox chkFlagUnknown3;
        protected CheckBox chkFlagUnknown4;

        protected Panel pnlPointProps;
        protected NumericInputBox numX;
        protected Label label2;
        protected NumericInputBox numY;
        protected Label label1;
        protected ToolStripSeparator toolStripSeparator1;
        protected ToolStripButton btnSameX;
        protected ToolStripButton btnSameY;
        protected ToolStripMenuItem newObjectToolStripMenuItem;
        protected ToolStripSeparator toolStripMenuItem2;
        protected ToolStripSeparator toolStripMenuItem3;
        protected ToolStripSeparator assignSeperatorToolStripMenuItem;
        protected ToolStripSeparator toolStripMenuItem1;
        protected ToolStripMenuItem deleteToolStripMenuItem;
        protected TextBox txtModel;
        protected Label label3;
        protected Panel panel2;
        protected CheckBox chkPoly;
        protected Button btnRelink;
        protected TextBox txtBone;
        protected Label label4;
        protected CheckBox chkBones;
        protected CheckBox chkLeftLedge;
        protected ComboBox cboType;
        protected TreeView modelTree;
        protected Button btnUnlink;
        protected ContextMenuStrip contextMenuStrip2;
        protected ToolStripMenuItem assignToolStripMenuItem;
        protected ToolStripMenuItem assignNoMoveToolStripMenuItem;
        protected ToolStripMenuItem unlinkToolStripMenuItem;
        protected ToolStripMenuItem unlinkNoMoveToolStripMenuItem;
        protected ToolStripMenuItem snapToolStripMenuItem1;
        protected ToolStripSeparator toolStripSeparator2;
        protected ToolStripButton btnResetSnap;
        protected ToolStripButton btnUndo;
        protected ToolStripButton btnRedo;
        protected ToolStripSeparator toolStripSeparator3;
        protected CheckBox chkObjModule;
        protected CheckBox chkObjUnk;
        protected CheckBox chkObjSSEUnk;
        protected Button btnPlayAnims;
        protected Panel panel4;
        protected Panel panel3;
        protected Button btnPrevFrame;
        protected Button btnNextFrame;
        protected ToolStripButton btnHelp;
        protected CheckedListBox lstObjects;
        
        // BrawlCrate buttons
        protected ToolStripSeparator toolStripSeparatorCamera;    // Seperator for Camera controls
        protected ToolStripButton btnPerspectiveCam;              // Goes into perspective mode
        protected ToolStripButton btnFlipColl;
        protected ToolStripButton btnOrthographicCam;             // Goes into orthographic mode
        protected ToolStripButton btnBoundaries;
        protected ToolStripButton btnSpawns;
        protected ToolStripButton btnItems;
        private ToolStripMenuItem moveToNewObjectToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem splitToolStripMenuItem;
        private ToolStripMenuItem mergeToolStripMenuItem;
        private ToolStripMenuItem flipToolStripMenuItem;
        private ToolStripMenuItem deleteToolStripMenuItem1;
        private ToolStripMenuItem transformToolStripMenuItem;
        private ToolStripMenuItem alignXToolStripMenuItem;
        private ToolStripMenuItem alignYToolStripMenuItem;
        protected ToolStripSeparator toolStripSeparatorOverlays;    // Seperator for Overlay controls

        protected void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CollisionEditor));
            this.undoToolStrip = new System.Windows.Forms.SplitContainer();
            this.redoToolStrip = new System.Windows.Forms.SplitContainer();
            this.modelTree = new System.Windows.Forms.TreeView();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.assignToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.assignNoMoveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.assignSeperatorToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.snapToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkBones = new System.Windows.Forms.CheckBox();
            this.chkPoly = new System.Windows.Forms.CheckBox();
            this.chkAllModels = new System.Windows.Forms.CheckBox();
            this.lstObjects = new System.Windows.Forms.CheckedListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newObjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.unlinkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unlinkNoMoveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.snapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pnlPlaneProps = new System.Windows.Forms.Panel();
            this.groupBoxFlags2 = new System.Windows.Forms.GroupBox();
            this.chkFlagUnknown1 = new System.Windows.Forms.CheckBox();
            this.chkFlagUnknown2 = new System.Windows.Forms.CheckBox();
            this.chkFlagUnknown3 = new System.Windows.Forms.CheckBox();
            this.chkFlagUnknown4 = new System.Windows.Forms.CheckBox();
            this.groupBoxFlags1 = new System.Windows.Forms.GroupBox();
            this.chkLeftLedge = new System.Windows.Forms.CheckBox();
            this.chkNoWalljump = new System.Windows.Forms.CheckBox();
            this.chkRightLedge = new System.Windows.Forms.CheckBox();
            this.chkTypeRotating = new System.Windows.Forms.CheckBox();
            this.chkFallThrough = new System.Windows.Forms.CheckBox();
            this.groupBoxTargets = new System.Windows.Forms.GroupBox();
            this.chkTypePokemonTrainer = new System.Windows.Forms.CheckBox();
            this.chkTypeItems = new System.Windows.Forms.CheckBox();
            this.chkTypeCharacters = new System.Windows.Forms.CheckBox();
            this.cboMaterial = new System.Windows.Forms.ComboBox();
            this.cboType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.labelType = new System.Windows.Forms.Label();
            this.pnlPointProps = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.numY = new System.Windows.Forms.NumericInputBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numX = new System.Windows.Forms.NumericInputBox();
            this.pnlObjProps = new System.Windows.Forms.Panel();
            this.chkObjSSEUnk = new System.Windows.Forms.CheckBox();
            this.chkObjModule = new System.Windows.Forms.CheckBox();
            this.chkObjUnk = new System.Windows.Forms.CheckBox();
            this.btnUnlink = new System.Windows.Forms.Button();
            this.btnRelink = new System.Windows.Forms.Button();
            this.txtBone = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtModel = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnPlayAnims = new System.Windows.Forms.Button();
            this.btnPrevFrame = new System.Windows.Forms.Button();
            this.btnNextFrame = new System.Windows.Forms.Button();
            this._modelPanel = new System.Windows.Forms.ModelPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnUndo = new System.Windows.Forms.ToolStripButton();
            this.btnRedo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSplit = new System.Windows.Forms.ToolStripButton();
            this.btnMerge = new System.Windows.Forms.ToolStripButton();
            this.btnFlipColl = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSameX = new System.Windows.Forms.ToolStripButton();
            this.btnSameY = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnPerspectiveCam = new System.Windows.Forms.ToolStripButton();
            this.btnOrthographicCam = new System.Windows.Forms.ToolStripButton();
            this.btnResetCam = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorCamera = new System.Windows.Forms.ToolStripSeparator();
            this.btnSpawns = new System.Windows.Forms.ToolStripButton();
            this.btnItems = new System.Windows.Forms.ToolStripButton();
            this.btnBoundaries = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorOverlays = new System.Windows.Forms.ToolStripSeparator();
            this.btnResetSnap = new System.Windows.Forms.ToolStripButton();
            this.btnHelp = new System.Windows.Forms.ToolStripButton();
            this.btnResetRot = new System.Windows.Forms.Button();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.contextMenuStrip3 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.moveToNewObjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.splitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mergeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flipToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.transformToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alignXToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alignYToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.undoToolStrip)).BeginInit();
            this.undoToolStrip.Panel1.SuspendLayout();
            this.undoToolStrip.Panel2.SuspendLayout();
            this.undoToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.redoToolStrip)).BeginInit();
            this.redoToolStrip.Panel1.SuspendLayout();
            this.redoToolStrip.Panel2.SuspendLayout();
            this.redoToolStrip.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.pnlPlaneProps.SuspendLayout();
            this.groupBoxFlags2.SuspendLayout();
            this.groupBoxFlags1.SuspendLayout();
            this.groupBoxTargets.SuspendLayout();
            this.pnlPointProps.SuspendLayout();
            this.pnlObjProps.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.contextMenuStrip3.SuspendLayout();
            this.SuspendLayout();
            // 
            // undoToolStrip
            // 
            this.undoToolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.undoToolStrip.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.undoToolStrip.Location = new System.Drawing.Point(0, 0);
            this.undoToolStrip.Name = "undoToolStrip";
            // 
            // undoToolStrip.Panel1
            // 
            this.undoToolStrip.Panel1.Controls.Add(this.redoToolStrip);
            // 
            // undoToolStrip.Panel2
            // 
            this.undoToolStrip.Panel2.Controls.Add(this._modelPanel);
            this.undoToolStrip.Panel2.Controls.Add(this.panel1);
            this.undoToolStrip.Size = new System.Drawing.Size(694, 467);
            this.undoToolStrip.SplitterDistance = 209;
            this.undoToolStrip.TabIndex = 1;
            // 
            // redoToolStrip
            // 
            this.redoToolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.redoToolStrip.Location = new System.Drawing.Point(0, 0);
            this.redoToolStrip.Name = "redoToolStrip";
            this.redoToolStrip.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // redoToolStrip.Panel1
            // 
            this.redoToolStrip.Panel1.Controls.Add(this.modelTree);
            this.redoToolStrip.Panel1.Controls.Add(this.panel2);
            // 
            // redoToolStrip.Panel2
            // 
            this.redoToolStrip.Panel2.Controls.Add(this.lstObjects);
            this.redoToolStrip.Panel2.Controls.Add(this.panel3);
            this.redoToolStrip.Panel2.Controls.Add(this.panel4);
            this.redoToolStrip.Size = new System.Drawing.Size(209, 467);
            this.redoToolStrip.SplitterDistance = 242;
            this.redoToolStrip.TabIndex = 2;
            // 
            // modelTree
            // 
            this.modelTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.modelTree.CheckBoxes = true;
            this.modelTree.ContextMenuStrip = this.contextMenuStrip2;
            this.modelTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelTree.HideSelection = false;
            this.modelTree.Location = new System.Drawing.Point(0, 17);
            this.modelTree.Name = "modelTree";
            this.modelTree.Size = new System.Drawing.Size(209, 225);
            this.modelTree.TabIndex = 4;
            this.modelTree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.modelTree_AfterCheck);
            this.modelTree.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.modelTree_BeforeSelect);
            this.modelTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.modelTree_AfterSelect);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.assignToolStripMenuItem,
            this.assignNoMoveToolStripMenuItem,
            this.assignSeperatorToolStripMenuItem,
            this.snapToolStripMenuItem1});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(239, 76);
            this.contextMenuStrip2.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip2_Opening);
            // 
            // assignToolStripMenuItem
            // 
            this.assignToolStripMenuItem.Name = "assignToolStripMenuItem";
            this.assignToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.assignToolStripMenuItem.Text = "Assign";
            this.assignToolStripMenuItem.Click += new System.EventHandler(this.btnRelink_Click);
            // 
            // assignNoMoveToolStripMenuItem
            // 
            this.assignNoMoveToolStripMenuItem.Name = "assignNoMoveToolStripMenuItem";
            this.assignNoMoveToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.assignNoMoveToolStripMenuItem.Text = "Assign (No relative movement)";
            this.assignNoMoveToolStripMenuItem.Click += new System.EventHandler(this.btnRelinkNoMove_Click);
            // 
            // assignSeperatorToolStripMenuItem
            // 
            this.assignSeperatorToolStripMenuItem.Name = "assignSeperatorToolStripMenuItem";
            this.assignSeperatorToolStripMenuItem.Size = new System.Drawing.Size(235, 6);
            // 
            // snapToolStripMenuItem1
            // 
            this.snapToolStripMenuItem1.Name = "snapToolStripMenuItem1";
            this.snapToolStripMenuItem1.Size = new System.Drawing.Size(238, 22);
            this.snapToolStripMenuItem1.Text = "Snap";
            this.snapToolStripMenuItem1.Click += new System.EventHandler(this.snapToolStripMenuItem1_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.chkBones);
            this.panel2.Controls.Add(this.chkPoly);
            this.panel2.Controls.Add(this.chkAllModels);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(209, 17);
            this.panel2.TabIndex = 3;
            // 
            // chkBones
            // 
            this.chkBones.Location = new System.Drawing.Point(100, 0);
            this.chkBones.Name = "chkBones";
            this.chkBones.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.chkBones.Size = new System.Drawing.Size(67, 17);
            this.chkBones.TabIndex = 4;
            this.chkBones.Text = "Bones";
            this.chkBones.UseVisualStyleBackColor = true;
            this.chkBones.CheckedChanged += new System.EventHandler(this.chkBones_CheckedChanged);
            // 
            // chkPoly
            // 
            this.chkPoly.Checked = true;
            this.chkPoly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPoly.Location = new System.Drawing.Point(44, 0);
            this.chkPoly.Name = "chkPoly";
            this.chkPoly.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.chkPoly.Size = new System.Drawing.Size(54, 17);
            this.chkPoly.TabIndex = 3;
            this.chkPoly.Text = "Poly";
            this.chkPoly.ThreeState = true;
            this.chkPoly.UseVisualStyleBackColor = true;
            this.chkPoly.CheckStateChanged += new System.EventHandler(this.chkPoly_CheckStateChanged);
            // 
            // chkAllModels
            // 
            this.chkAllModels.Checked = true;
            this.chkAllModels.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllModels.Location = new System.Drawing.Point(0, 0);
            this.chkAllModels.Name = "chkAllModels";
            this.chkAllModels.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.chkAllModels.Size = new System.Drawing.Size(41, 17);
            this.chkAllModels.TabIndex = 2;
            this.chkAllModels.Text = "All";
            this.chkAllModels.UseVisualStyleBackColor = true;
            this.chkAllModels.CheckedChanged += new System.EventHandler(this.chkAllModels_CheckedChanged);
            // 
            // lstObjects
            // 
            this.lstObjects.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstObjects.ContextMenuStrip = this.contextMenuStrip1;
            this.lstObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstObjects.FormattingEnabled = true;
            this.lstObjects.IntegralHeight = false;
            this.lstObjects.Location = new System.Drawing.Point(0, 0);
            this.lstObjects.Name = "lstObjects";
            this.lstObjects.Size = new System.Drawing.Size(209, 82);
            this.lstObjects.TabIndex = 1;
            this.lstObjects.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstObjects_ItemCheck);
            this.lstObjects.SelectedValueChanged += new System.EventHandler(this.lstObjects_SelectedValueChanged);
            this.lstObjects.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstObjects_MouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newObjectToolStripMenuItem,
            this.toolStripMenuItem3,
            this.unlinkToolStripMenuItem,
            this.unlinkNoMoveToolStripMenuItem,
            this.toolStripMenuItem2,
            this.snapToolStripMenuItem,
            this.toolStripMenuItem1,
            this.deleteToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(238, 132);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // newObjectToolStripMenuItem
            // 
            this.newObjectToolStripMenuItem.Name = "newObjectToolStripMenuItem";
            this.newObjectToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.newObjectToolStripMenuItem.Text = "New Object";
            this.newObjectToolStripMenuItem.Click += new System.EventHandler(this.newObjectToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(234, 6);
            // 
            // unlinkToolStripMenuItem
            // 
            this.unlinkToolStripMenuItem.Name = "unlinkToolStripMenuItem";
            this.unlinkToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.unlinkToolStripMenuItem.Text = "Unlink";
            this.unlinkToolStripMenuItem.Click += new System.EventHandler(this.btnUnlink_Click);
            // 
            // unlinkNoMoveToolStripMenuItem
            // 
            this.unlinkNoMoveToolStripMenuItem.Name = "unlinkNoMoveToolStripMenuItem";
            this.unlinkNoMoveToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.unlinkNoMoveToolStripMenuItem.Text = "Unlink (No relative movement)";
            this.unlinkNoMoveToolStripMenuItem.Click += new System.EventHandler(this.btnUnlinkNoMove_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(234, 6);
            // 
            // snapToolStripMenuItem
            // 
            this.snapToolStripMenuItem.Name = "snapToolStripMenuItem";
            this.snapToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.snapToolStripMenuItem.Text = "Snap";
            this.snapToolStripMenuItem.Click += new System.EventHandler(this.snapToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(234, 6);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete)));
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(237, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.pnlPlaneProps);
            this.panel3.Controls.Add(this.pnlPointProps);
            this.panel3.Controls.Add(this.pnlObjProps);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 82);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(209, 115);
            this.panel3.TabIndex = 16;
            // 
            // pnlPlaneProps
            // 
            this.pnlPlaneProps.Controls.Add(this.groupBoxFlags2);
            this.pnlPlaneProps.Controls.Add(this.groupBoxFlags1);
            this.pnlPlaneProps.Controls.Add(this.groupBoxTargets);
            this.pnlPlaneProps.Controls.Add(this.cboMaterial);
            this.pnlPlaneProps.Controls.Add(this.cboType);
            this.pnlPlaneProps.Controls.Add(this.label5);
            this.pnlPlaneProps.Controls.Add(this.labelType);
            this.pnlPlaneProps.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlPlaneProps.Location = new System.Drawing.Point(0, -273);
            this.pnlPlaneProps.Name = "pnlPlaneProps";
            this.pnlPlaneProps.Size = new System.Drawing.Size(209, 188);
            this.pnlPlaneProps.TabIndex = 0;
            this.pnlPlaneProps.Visible = false;
            // 
            // groupBoxFlags2
            // 
            this.groupBoxFlags2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxFlags2.Controls.Add(this.chkFlagUnknown1);
            this.groupBoxFlags2.Controls.Add(this.chkFlagUnknown2);
            this.groupBoxFlags2.Controls.Add(this.chkFlagUnknown3);
            this.groupBoxFlags2.Controls.Add(this.chkFlagUnknown4);
            this.groupBoxFlags2.Location = new System.Drawing.Point(104, 102);
            this.groupBoxFlags2.Margin = new System.Windows.Forms.Padding(0);
            this.groupBoxFlags2.Name = "groupBoxFlags2";
            this.groupBoxFlags2.Padding = new System.Windows.Forms.Padding(0);
            this.groupBoxFlags2.Size = new System.Drawing.Size(105, 160);
            this.groupBoxFlags2.TabIndex = 14;
            this.groupBoxFlags2.TabStop = false;
            // 
            // chkFlagUnknown1
            // 
            this.chkFlagUnknown1.Location = new System.Drawing.Point(8, 17);
            this.chkFlagUnknown1.Margin = new System.Windows.Forms.Padding(0);
            this.chkFlagUnknown1.Name = "chkFlagUnknown1";
            this.chkFlagUnknown1.Size = new System.Drawing.Size(86, 18);
            this.chkFlagUnknown1.TabIndex = 3;
            this.chkFlagUnknown1.Text = "Unknown 1";
            this.chkFlagUnknown1.UseVisualStyleBackColor = true;
            this.chkFlagUnknown1.CheckedChanged += new System.EventHandler(this.chkFlagUnknown1_CheckedChanged);
            // 
            // chkFlagUnknown2
            // 
            this.chkFlagUnknown2.Location = new System.Drawing.Point(8, 33);
            this.chkFlagUnknown2.Margin = new System.Windows.Forms.Padding(0);
            this.chkFlagUnknown2.Name = "chkFlagUnknown2";
            this.chkFlagUnknown2.Size = new System.Drawing.Size(86, 18);
            this.chkFlagUnknown2.TabIndex = 3;
            this.chkFlagUnknown2.Text = "Unknown 2";
            this.chkFlagUnknown2.UseVisualStyleBackColor = true;
            this.chkFlagUnknown2.CheckedChanged += new System.EventHandler(this.chkFlagUnknown2_CheckedChanged);
            // 
            // chkFlagUnknown3
            // 
            this.chkFlagUnknown3.Location = new System.Drawing.Point(8, 49);
            this.chkFlagUnknown3.Margin = new System.Windows.Forms.Padding(0);
            this.chkFlagUnknown3.Name = "chkFlagUnknown3";
            this.chkFlagUnknown3.Size = new System.Drawing.Size(86, 18);
            this.chkFlagUnknown3.TabIndex = 3;
            this.chkFlagUnknown3.Text = "Unknown 3";
            this.chkFlagUnknown3.UseVisualStyleBackColor = true;
            this.chkFlagUnknown3.CheckedChanged += new System.EventHandler(this.chkFlagUnknown3_CheckedChanged);
            // 
            // chkFlagUnknown4
            // 
            this.chkFlagUnknown4.Location = new System.Drawing.Point(8, 65);
            this.chkFlagUnknown4.Margin = new System.Windows.Forms.Padding(0);
            this.chkFlagUnknown4.Name = "chkFlagUnknown4";
            this.chkFlagUnknown4.Size = new System.Drawing.Size(86, 18);
            this.chkFlagUnknown4.TabIndex = 3;
            this.chkFlagUnknown4.Text = "Unknown 4";
            this.chkFlagUnknown4.UseVisualStyleBackColor = true;
            this.chkFlagUnknown4.CheckedChanged += new System.EventHandler(this.chkFlagUnknown4_CheckedChanged);
            // 
            // groupBoxFlags1
            // 
            this.groupBoxFlags1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxFlags1.Controls.Add(this.chkLeftLedge);
            this.groupBoxFlags1.Controls.Add(this.chkNoWalljump);
            this.groupBoxFlags1.Controls.Add(this.chkRightLedge);
            this.groupBoxFlags1.Controls.Add(this.chkTypeRotating);
            this.groupBoxFlags1.Controls.Add(this.chkFallThrough);
            this.groupBoxFlags1.Location = new System.Drawing.Point(0, 102);
            this.groupBoxFlags1.Margin = new System.Windows.Forms.Padding(0);
            this.groupBoxFlags1.Name = "groupBoxFlags1";
            this.groupBoxFlags1.Padding = new System.Windows.Forms.Padding(0);
            this.groupBoxFlags1.Size = new System.Drawing.Size(104, 160);
            this.groupBoxFlags1.TabIndex = 13;
            this.groupBoxFlags1.TabStop = false;
            this.groupBoxFlags1.Text = "Flags";
            // 
            // chkLeftLedge
            // 
            this.chkLeftLedge.Location = new System.Drawing.Point(8, 33);
            this.chkLeftLedge.Margin = new System.Windows.Forms.Padding(0);
            this.chkLeftLedge.Name = "chkLeftLedge";
            this.chkLeftLedge.Size = new System.Drawing.Size(86, 18);
            this.chkLeftLedge.TabIndex = 4;
            this.chkLeftLedge.Text = "Left Ledge";
            this.chkLeftLedge.UseVisualStyleBackColor = true;
            this.chkLeftLedge.CheckedChanged += new System.EventHandler(this.chkLeftLedge_CheckedChanged);
            // 
            // chkNoWalljump
            // 
            this.chkNoWalljump.Location = new System.Drawing.Point(8, 65);
            this.chkNoWalljump.Margin = new System.Windows.Forms.Padding(0);
            this.chkNoWalljump.Name = "chkNoWalljump";
            this.chkNoWalljump.Size = new System.Drawing.Size(90, 18);
            this.chkNoWalljump.TabIndex = 2;
            this.chkNoWalljump.Text = "No Walljump";
            this.chkNoWalljump.UseVisualStyleBackColor = true;
            this.chkNoWalljump.CheckedChanged += new System.EventHandler(this.chkNoWalljump_CheckedChanged);
            // 
            // chkRightLedge
            // 
            this.chkRightLedge.Location = new System.Drawing.Point(8, 49);
            this.chkRightLedge.Margin = new System.Windows.Forms.Padding(0);
            this.chkRightLedge.Name = "chkRightLedge";
            this.chkRightLedge.Size = new System.Drawing.Size(86, 18);
            this.chkRightLedge.TabIndex = 1;
            this.chkRightLedge.Text = "Right Ledge";
            this.chkRightLedge.UseVisualStyleBackColor = true;
            this.chkRightLedge.CheckedChanged += new System.EventHandler(this.chkRightLedge_CheckedChanged);
            // 
            // chkTypeRotating
            // 
            this.chkTypeRotating.Location = new System.Drawing.Point(8, 81);
            this.chkTypeRotating.Margin = new System.Windows.Forms.Padding(0);
            this.chkTypeRotating.Name = "chkTypeRotating";
            this.chkTypeRotating.Size = new System.Drawing.Size(86, 18);
            this.chkTypeRotating.TabIndex = 4;
            this.chkTypeRotating.Text = "Rotating";
            this.chkTypeRotating.UseVisualStyleBackColor = true;
            this.chkTypeRotating.CheckedChanged += new System.EventHandler(this.chkTypeRotating_CheckedChanged);
            // 
            // chkFallThrough
            // 
            this.chkFallThrough.Location = new System.Drawing.Point(8, 17);
            this.chkFallThrough.Margin = new System.Windows.Forms.Padding(0);
            this.chkFallThrough.Name = "chkFallThrough";
            this.chkFallThrough.Size = new System.Drawing.Size(90, 18);
            this.chkFallThrough.TabIndex = 0;
            this.chkFallThrough.Text = "Fall-Through";
            this.chkFallThrough.UseVisualStyleBackColor = true;
            this.chkFallThrough.CheckedChanged += new System.EventHandler(this.chkFallThrough_CheckedChanged);
            // 
            // groupBoxTargets
            // 
            this.groupBoxTargets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxTargets.Controls.Add(this.chkTypePokemonTrainer);
            this.groupBoxTargets.Controls.Add(this.chkTypeItems);
            this.groupBoxTargets.Controls.Add(this.chkTypeCharacters);
            this.groupBoxTargets.Location = new System.Drawing.Point(0, 50);
            this.groupBoxTargets.Margin = new System.Windows.Forms.Padding(0);
            this.groupBoxTargets.Name = "groupBoxTargets";
            this.groupBoxTargets.Padding = new System.Windows.Forms.Padding(0);
            this.groupBoxTargets.Size = new System.Drawing.Size(208, 71);
            this.groupBoxTargets.TabIndex = 14;
            this.groupBoxTargets.TabStop = false;
            this.groupBoxTargets.Text = "Collision Targets";
            // 
            // chkTypePokemonTrainer
            // 
            this.chkTypePokemonTrainer.Location = new System.Drawing.Point(82, 33);
            this.chkTypePokemonTrainer.Margin = new System.Windows.Forms.Padding(0);
            this.chkTypePokemonTrainer.Name = "chkTypePokemonTrainer";
            this.chkTypePokemonTrainer.Size = new System.Drawing.Size(116, 18);
            this.chkTypePokemonTrainer.TabIndex = 3;
            this.chkTypePokemonTrainer.Text = "Pokémon Trainer";
            this.chkTypePokemonTrainer.UseVisualStyleBackColor = true;
            this.chkTypePokemonTrainer.CheckedChanged += new System.EventHandler(this.chkTypePokemonTrainer_CheckedChanged);
            // 
            // chkTypeItems
            // 
            this.chkTypeItems.Location = new System.Drawing.Point(8, 33);
            this.chkTypeItems.Margin = new System.Windows.Forms.Padding(0);
            this.chkTypeItems.Name = "chkTypeItems";
            this.chkTypeItems.Size = new System.Drawing.Size(86, 18);
            this.chkTypeItems.TabIndex = 3;
            this.chkTypeItems.Text = "Items";
            this.chkTypeItems.UseVisualStyleBackColor = true;
            this.chkTypeItems.CheckedChanged += new System.EventHandler(this.chkTypeItems_CheckedChanged);
            // 
            // chkTypeCharacters
            // 
            this.chkTypeCharacters.Location = new System.Drawing.Point(8, 17);
            this.chkTypeCharacters.Margin = new System.Windows.Forms.Padding(0);
            this.chkTypeCharacters.Name = "chkTypeCharacters";
            this.chkTypeCharacters.Size = new System.Drawing.Size(194, 18);
            this.chkTypeCharacters.TabIndex = 4;
            this.chkTypeCharacters.Text = "Everything";
            this.chkTypeCharacters.UseVisualStyleBackColor = true;
            this.chkTypeCharacters.CheckedChanged += new System.EventHandler(this.chkTypeCharacters_CheckedChanged);
            // 
            // cboMaterial
            // 
            this.cboMaterial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMaterial.FormattingEnabled = true;
            this.cboMaterial.Location = new System.Drawing.Point(66, 25);
            this.cboMaterial.Name = "cboMaterial";
            this.cboMaterial.Size = new System.Drawing.Size(139, 21);
            this.cboMaterial.TabIndex = 12;
            this.cboMaterial.SelectedIndexChanged += new System.EventHandler(this.cboMaterial_SelectedIndexChanged);
            // 
            // cboType
            // 
            this.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboType.FormattingEnabled = true;
            this.cboType.Location = new System.Drawing.Point(66, 4);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(139, 21);
            this.cboType.TabIndex = 5;
            this.cboType.SelectedIndexChanged += new System.EventHandler(this.cboType_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(7, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 21);
            this.label5.TabIndex = 8;
            this.label5.Text = "Material:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelType
            // 
            this.labelType.Location = new System.Drawing.Point(7, 4);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(53, 21);
            this.labelType.TabIndex = 8;
            this.labelType.Text = "Type:";
            this.labelType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlPointProps
            // 
            this.pnlPointProps.Controls.Add(this.label2);
            this.pnlPointProps.Controls.Add(this.numY);
            this.pnlPointProps.Controls.Add(this.label1);
            this.pnlPointProps.Controls.Add(this.numX);
            this.pnlPointProps.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlPointProps.Location = new System.Drawing.Point(0, -85);
            this.pnlPointProps.Name = "pnlPointProps";
            this.pnlPointProps.Size = new System.Drawing.Size(209, 70);
            this.pnlPointProps.TabIndex = 15;
            this.pnlPointProps.Visible = false;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(18, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Y";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numY
            // 
            this.numY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numY.Integral = false;
            this.numY.Location = new System.Drawing.Point(59, 32);
            this.numY.MaximumValue = 3.402823E+38F;
            this.numY.MinimumValue = -3.402823E+38F;
            this.numY.Name = "numY";
            this.numY.Size = new System.Drawing.Size(100, 20);
            this.numY.TabIndex = 2;
            this.numY.Text = "0";
            this.numY.ValueChanged += new System.EventHandler(this.numY_ValueChanged);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(18, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "X";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numX
            // 
            this.numX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numX.Integral = false;
            this.numX.Location = new System.Drawing.Point(59, 13);
            this.numX.MaximumValue = 3.402823E+38F;
            this.numX.MinimumValue = -3.402823E+38F;
            this.numX.Name = "numX";
            this.numX.Size = new System.Drawing.Size(100, 20);
            this.numX.TabIndex = 0;
            this.numX.Text = "0";
            this.numX.ValueChanged += new System.EventHandler(this.numX_ValueChanged);
            // 
            // pnlObjProps
            // 
            this.pnlObjProps.Controls.Add(this.chkObjSSEUnk);
            this.pnlObjProps.Controls.Add(this.chkObjModule);
            this.pnlObjProps.Controls.Add(this.chkObjUnk);
            this.pnlObjProps.Controls.Add(this.btnUnlink);
            this.pnlObjProps.Controls.Add(this.btnRelink);
            this.pnlObjProps.Controls.Add(this.txtBone);
            this.pnlObjProps.Controls.Add(this.label4);
            this.pnlObjProps.Controls.Add(this.txtModel);
            this.pnlObjProps.Controls.Add(this.label3);
            this.pnlObjProps.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlObjProps.Location = new System.Drawing.Point(0, -15);
            this.pnlObjProps.Name = "pnlObjProps";
            this.pnlObjProps.Size = new System.Drawing.Size(209, 130);
            this.pnlObjProps.TabIndex = 1;
            this.pnlObjProps.Visible = false;
            // 
            // chkObjSSEUnk
            // 
            this.chkObjSSEUnk.AutoSize = true;
            this.chkObjSSEUnk.Location = new System.Drawing.Point(10, 102);
            this.chkObjSSEUnk.Name = "chkObjSSEUnk";
            this.chkObjSSEUnk.Size = new System.Drawing.Size(96, 17);
            this.chkObjSSEUnk.TabIndex = 15;
            this.chkObjSSEUnk.Text = "SSE Unknown";
            this.chkObjSSEUnk.UseVisualStyleBackColor = true;
            this.chkObjSSEUnk.CheckedChanged += new System.EventHandler(this.chkObjSSEUnk_CheckedChanged);
            // 
            // chkObjModule
            // 
            this.chkObjModule.AutoSize = true;
            this.chkObjModule.Location = new System.Drawing.Point(10, 79);
            this.chkObjModule.Name = "chkObjModule";
            this.chkObjModule.Size = new System.Drawing.Size(111, 17);
            this.chkObjModule.TabIndex = 14;
            this.chkObjModule.Text = "Module Controlled";
            this.chkObjModule.UseVisualStyleBackColor = true;
            this.chkObjModule.CheckedChanged += new System.EventHandler(this.chkObjModule_CheckedChanged);
            // 
            // chkObjUnk
            // 
            this.chkObjUnk.AutoSize = true;
            this.chkObjUnk.Location = new System.Drawing.Point(10, 56);
            this.chkObjUnk.Name = "chkObjUnk";
            this.chkObjUnk.Size = new System.Drawing.Size(72, 17);
            this.chkObjUnk.TabIndex = 13;
            this.chkObjUnk.Text = "Unknown";
            this.chkObjUnk.UseVisualStyleBackColor = true;
            this.chkObjUnk.CheckedChanged += new System.EventHandler(this.chkObjUnk_CheckedChanged);
            // 
            // btnUnlink
            // 
            this.btnUnlink.Location = new System.Drawing.Point(177, 22);
            this.btnUnlink.Name = "btnUnlink";
            this.btnUnlink.Size = new System.Drawing.Size(28, 21);
            this.btnUnlink.TabIndex = 12;
            this.btnUnlink.Text = "-";
            this.btnUnlink.UseVisualStyleBackColor = true;
            this.btnUnlink.Click += new System.EventHandler(this.btnUnlink_Click);
            // 
            // btnRelink
            // 
            this.btnRelink.Location = new System.Drawing.Point(177, 2);
            this.btnRelink.Name = "btnRelink";
            this.btnRelink.Size = new System.Drawing.Size(28, 21);
            this.btnRelink.TabIndex = 4;
            this.btnRelink.Text = "+";
            this.btnRelink.UseVisualStyleBackColor = true;
            this.btnRelink.Click += new System.EventHandler(this.btnRelink_Click);
            // 
            // txtBone
            // 
            this.txtBone.Location = new System.Drawing.Point(49, 23);
            this.txtBone.Name = "txtBone";
            this.txtBone.ReadOnly = true;
            this.txtBone.Size = new System.Drawing.Size(126, 20);
            this.txtBone.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(4, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 20);
            this.label4.TabIndex = 2;
            this.label4.Text = "Bone:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtModel
            // 
            this.txtModel.Location = new System.Drawing.Point(49, 3);
            this.txtModel.Name = "txtModel";
            this.txtModel.ReadOnly = true;
            this.txtModel.Size = new System.Drawing.Size(126, 20);
            this.txtModel.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(4, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "Model:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnPlayAnims);
            this.panel4.Controls.Add(this.btnPrevFrame);
            this.panel4.Controls.Add(this.btnNextFrame);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Enabled = false;
            this.panel4.Location = new System.Drawing.Point(0, 197);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(209, 24);
            this.panel4.TabIndex = 17;
            this.panel4.Visible = false;
            // 
            // btnPlayAnims
            // 
            this.btnPlayAnims.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPlayAnims.Location = new System.Drawing.Point(35, 0);
            this.btnPlayAnims.Name = "btnPlayAnims";
            this.btnPlayAnims.Size = new System.Drawing.Size(139, 24);
            this.btnPlayAnims.TabIndex = 16;
            this.btnPlayAnims.Text = "Play Animations";
            this.btnPlayAnims.UseVisualStyleBackColor = true;
            this.btnPlayAnims.Click += new System.EventHandler(this.btnPlayAnims_Click);
            // 
            // btnPrevFrame
            // 
            this.btnPrevFrame.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnPrevFrame.Location = new System.Drawing.Point(0, 0);
            this.btnPrevFrame.Name = "btnPrevFrame";
            this.btnPrevFrame.Size = new System.Drawing.Size(35, 24);
            this.btnPrevFrame.TabIndex = 18;
            this.btnPrevFrame.Text = "<";
            this.btnPrevFrame.UseVisualStyleBackColor = true;
            this.btnPrevFrame.Click += new System.EventHandler(this.btnPrevFrame_Click);
            // 
            // btnNextFrame
            // 
            this.btnNextFrame.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnNextFrame.Location = new System.Drawing.Point(174, 0);
            this.btnNextFrame.Name = "btnNextFrame";
            this.btnNextFrame.Size = new System.Drawing.Size(35, 24);
            this.btnNextFrame.TabIndex = 17;
            this.btnNextFrame.Text = ">";
            this.btnNextFrame.UseVisualStyleBackColor = true;
            this.btnNextFrame.Click += new System.EventHandler(this.btnNextFrame_Click);
            // 
            // _modelPanel
            // 
            this._modelPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._modelPanel.Location = new System.Drawing.Point(0, 25);
            this._modelPanel.Name = "_modelPanel";
            this._modelPanel.Size = new System.Drawing.Size(481, 442);
            this._modelPanel.TabIndex = 0;
            this._modelPanel.PreRender += new System.Windows.Forms.GLRenderEventHandler(this._modelPanel_PreRender);
            this._modelPanel.PostRender += new System.Windows.Forms.GLRenderEventHandler(this._modelPanel_PostRender);
            this._modelPanel.KeyDown += new System.Windows.Forms.KeyEventHandler(this._modelPanel_KeyDown);
            this._modelPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this._modelPanel_MouseDown);
            this._modelPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this._modelPanel_MouseMove);
            this._modelPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this._modelPanel_MouseUp);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Controls.Add(this.btnResetRot);
            this.panel1.Controls.Add(this.trackBar1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(481, 25);
            this.panel1.TabIndex = 2;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnUndo,
            this.btnRedo,
            this.toolStripSeparator3,
            this.btnSplit,
            this.btnMerge,
            this.btnFlipColl,
            this.btnDelete,
            this.toolStripSeparator2,
            this.btnSameX,
            this.btnSameY,
            this.toolStripSeparator1,
            this.btnPerspectiveCam,
            this.btnOrthographicCam,
            this.btnResetCam,
            this.toolStripSeparatorCamera,
            this.btnSpawns,
            this.btnItems,
            this.btnBoundaries,
            this.toolStripSeparatorOverlays,
            this.btnResetSnap,
            this.btnHelp});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(335, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnUndo
            // 
            this.btnUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnUndo.Enabled = false;
            this.btnUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(40, 22);
            this.btnUndo.Text = "Undo";
            this.btnUndo.Click += new System.EventHandler(this.Undo);
            // 
            // btnRedo
            // 
            this.btnRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnRedo.Enabled = false;
            this.btnRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRedo.Name = "btnRedo";
            this.btnRedo.Size = new System.Drawing.Size(38, 22);
            this.btnRedo.Text = "Redo";
            this.btnRedo.Click += new System.EventHandler(this.Redo);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnSplit
            // 
            this.btnSplit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSplit.Enabled = false;
            this.btnSplit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSplit.Name = "btnSplit";
            this.btnSplit.Size = new System.Drawing.Size(34, 22);
            this.btnSplit.Text = "Split";
            this.btnSplit.Click += new System.EventHandler(this.btnSplit_Click);
            // 
            // btnMerge
            // 
            this.btnMerge.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnMerge.Enabled = false;
            this.btnMerge.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(45, 22);
            this.btnMerge.Text = "Merge";
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // btnFlipColl
            // 
            this.btnFlipColl.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnFlipColl.Enabled = false;
            this.btnFlipColl.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFlipColl.Name = "btnFlipColl";
            this.btnFlipColl.Size = new System.Drawing.Size(30, 22);
            this.btnFlipColl.Text = "Flip";
            this.btnFlipColl.Click += new System.EventHandler(this.btnFlipColl_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDelete.Enabled = false;
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(44, 22);
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnSameX
            // 
            this.btnSameX.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSameX.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSameX.Name = "btnSameX";
            this.btnSameX.Size = new System.Drawing.Size(49, 22);
            this.btnSameX.Text = "Align X";
            this.btnSameX.Click += new System.EventHandler(this.btnSameX_Click);
            // 
            // btnSameY
            // 
            this.btnSameY.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSameY.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSameY.Name = "btnSameY";
            this.btnSameY.Size = new System.Drawing.Size(49, 19);
            this.btnSameY.Text = "Align Y";
            this.btnSameY.Click += new System.EventHandler(this.btnSameY_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnPerspectiveCam
            // 
            this.btnPerspectiveCam.Checked = true;
            this.btnPerspectiveCam.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnPerspectiveCam.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnPerspectiveCam.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPerspectiveCam.Name = "btnPerspectiveCam";
            this.btnPerspectiveCam.Size = new System.Drawing.Size(71, 19);
            this.btnPerspectiveCam.Text = "Perspective";
            this.btnPerspectiveCam.Click += new System.EventHandler(this.btnPerspectiveCam_Click);
            // 
            // btnOrthographicCam
            // 
            this.btnOrthographicCam.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnOrthographicCam.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOrthographicCam.Name = "btnOrthographicCam";
            this.btnOrthographicCam.Size = new System.Drawing.Size(82, 19);
            this.btnOrthographicCam.Text = "Orthographic";
            this.btnOrthographicCam.Click += new System.EventHandler(this.btnOrthographicCam_Click);
            // 
            // btnResetCam
            // 
            this.btnResetCam.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnResetCam.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnResetCam.Name = "btnResetCam";
            this.btnResetCam.Size = new System.Drawing.Size(67, 19);
            this.btnResetCam.Text = "Reset Cam";
            this.btnResetCam.Click += new System.EventHandler(this.btnResetCam_Click);
            // 
            // toolStripSeparatorCamera
            // 
            this.toolStripSeparatorCamera.Name = "toolStripSeparatorCamera";
            this.toolStripSeparatorCamera.Size = new System.Drawing.Size(6, 25);
            // 
            // btnSpawns
            // 
            this.btnSpawns.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSpawns.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSpawns.Name = "btnSpawns";
            this.btnSpawns.Size = new System.Drawing.Size(51, 19);
            this.btnSpawns.Text = "Spawns";
            this.btnSpawns.Click += new System.EventHandler(this.btnSpawns_Click);
            // 
            // btnItems
            // 
            this.btnItems.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnItems.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnItems.Name = "btnItems";
            this.btnItems.Size = new System.Drawing.Size(40, 19);
            this.btnItems.Text = "Items";
            this.btnItems.Click += new System.EventHandler(this.btnItems_Click);
            // 
            // btnBoundaries
            // 
            this.btnBoundaries.Checked = true;
            this.btnBoundaries.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnBoundaries.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnBoundaries.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnBoundaries.Name = "btnBoundaries";
            this.btnBoundaries.Size = new System.Drawing.Size(70, 19);
            this.btnBoundaries.Text = "Boundaries";
            this.btnBoundaries.Click += new System.EventHandler(this.btnBoundaries_Click);
            // 
            // toolStripSeparatorOverlays
            // 
            this.toolStripSeparatorOverlays.Name = "toolStripSeparatorOverlays";
            this.toolStripSeparatorOverlays.Size = new System.Drawing.Size(6, 6);
            // 
            // btnResetSnap
            // 
            this.btnResetSnap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnResetSnap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnResetSnap.Name = "btnResetSnap";
            this.btnResetSnap.Size = new System.Drawing.Size(57, 19);
            this.btnResetSnap.Text = "Un-Snap";
            this.btnResetSnap.Click += new System.EventHandler(this.btnResetSnap_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnHelp.Image = ((System.Drawing.Image)(resources.GetObject("btnHelp.Image")));
            this.btnHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(36, 19);
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // btnResetRot
            // 
            this.btnResetRot.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnResetRot.Enabled = false;
            this.btnResetRot.FlatAppearance.BorderSize = 0;
            this.btnResetRot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetRot.Location = new System.Drawing.Point(335, 0);
            this.btnResetRot.Name = "btnResetRot";
            this.btnResetRot.Size = new System.Drawing.Size(16, 25);
            this.btnResetRot.TabIndex = 4;
            this.btnResetRot.Text = "*";
            this.btnResetRot.UseVisualStyleBackColor = true;
            this.btnResetRot.Visible = false;
            this.btnResetRot.Click += new System.EventHandler(this.btnResetRot_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.trackBar1.Enabled = false;
            this.trackBar1.Location = new System.Drawing.Point(351, 0);
            this.trackBar1.Maximum = 180;
            this.trackBar1.Minimum = -180;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(130, 25);
            this.trackBar1.TabIndex = 3;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar1.Visible = false;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // contextMenuStrip3
            // 
            this.contextMenuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.moveToNewObjectToolStripMenuItem,
            this.transformToolStripMenuItem,
            this.alignXToolStripMenuItem,
            this.alignYToolStripMenuItem,
            this.toolStripSeparator4,
            this.splitToolStripMenuItem,
            this.mergeToolStripMenuItem,
            this.flipToolStripMenuItem,
            this.deleteToolStripMenuItem1});
            this.contextMenuStrip3.Name = "contextMenuStrip3";
            this.contextMenuStrip3.Size = new System.Drawing.Size(184, 208);
            this.contextMenuStrip3.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip3_Opening);
            // 
            // moveToNewObjectToolStripMenuItem
            // 
            this.moveToNewObjectToolStripMenuItem.Name = "moveToNewObjectToolStripMenuItem";
            this.moveToNewObjectToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.moveToNewObjectToolStripMenuItem.Text = "Move to New Object";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(180, 6);
            // 
            // splitToolStripMenuItem
            // 
            this.splitToolStripMenuItem.Name = "splitToolStripMenuItem";
            this.splitToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.splitToolStripMenuItem.Text = "Split";
            this.splitToolStripMenuItem.Click += new System.EventHandler(this.btnSplit_Click);
            // 
            // mergeToolStripMenuItem
            // 
            this.mergeToolStripMenuItem.Name = "mergeToolStripMenuItem";
            this.mergeToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.mergeToolStripMenuItem.Text = "Merge";
            this.mergeToolStripMenuItem.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // flipToolStripMenuItem
            // 
            this.flipToolStripMenuItem.Name = "flipToolStripMenuItem";
            this.flipToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.flipToolStripMenuItem.Text = "Flip";
            this.flipToolStripMenuItem.Click += new System.EventHandler(this.btnFlipColl_Click);
            // 
            // deleteToolStripMenuItem1
            // 
            this.deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
            this.deleteToolStripMenuItem1.Size = new System.Drawing.Size(183, 22);
            this.deleteToolStripMenuItem1.Text = "Delete";
            this.deleteToolStripMenuItem1.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // transformToolStripMenuItem
            // 
            this.transformToolStripMenuItem.Name = "transformToolStripMenuItem";
            this.transformToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.transformToolStripMenuItem.Text = "Transform";
            this.transformToolStripMenuItem.Click += new System.EventHandler(this.transformToolStripMenuItem_Click);
            // 
            // alignXToolStripMenuItem
            // 
            this.alignXToolStripMenuItem.Name = "alignXToolStripMenuItem";
            this.alignXToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.alignXToolStripMenuItem.Text = "Align X";
            this.alignXToolStripMenuItem.Click += new System.EventHandler(this.btnSameX_Click);
            // 
            // alignYToolStripMenuItem
            // 
            this.alignYToolStripMenuItem.Name = "alignYToolStripMenuItem";
            this.alignYToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.alignYToolStripMenuItem.Text = "Align Y";
            this.alignYToolStripMenuItem.Click += new System.EventHandler(this.btnSameY_Click);
            // 
            // CollisionEditor
            // 
            this.BackColor = System.Drawing.Color.Lavender;
            this.Controls.Add(this.undoToolStrip);
            this.Name = "CollisionEditor";
            this.Size = new System.Drawing.Size(694, 467);
            this.undoToolStrip.Panel1.ResumeLayout(false);
            this.undoToolStrip.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.undoToolStrip)).EndInit();
            this.undoToolStrip.ResumeLayout(false);
            this.redoToolStrip.Panel1.ResumeLayout(false);
            this.redoToolStrip.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.redoToolStrip)).EndInit();
            this.redoToolStrip.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.pnlPlaneProps.ResumeLayout(false);
            this.groupBoxFlags2.ResumeLayout(false);
            this.groupBoxFlags1.ResumeLayout(false);
            this.groupBoxTargets.ResumeLayout(false);
            this.pnlPointProps.ResumeLayout(false);
            this.pnlPointProps.PerformLayout();
            this.pnlObjProps.ResumeLayout(false);
            this.pnlObjProps.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.contextMenuStrip3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected const float SelectWidth = 7.0f;
        protected const float PointSelectRadius = 1.5f;
        protected const float SmallIncrement = 0.5f;
        protected const float LargeIncrement = 3.0f;

        protected CollisionNode _targetNode;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CollisionNode TargetNode
        {
            get { return _targetNode; }
            set { TargetChanged(value); }
        }

        protected bool _updating;
        protected CollisionObject _selectedObject;
        protected Matrix _snapMatrix;

        protected bool _hovering;
        protected List<CollisionLink> _selectedLinks = new List<CollisionLink>();
        protected List<CollisionPlane> _selectedPlanes = new List<CollisionPlane>();

        protected bool _selecting, _selectInverse;
        protected Vector3 _selectStart, _selectLast, _selectEnd;
        protected bool _creating;

        protected CollisionState save;
        protected List<CollisionState> undoSaves = new List<CollisionState>();
        protected List<CollisionState> redoSaves = new List<CollisionState>();
        protected int saveIndex = 0;
        protected bool hasMoved = false;

        public CollisionEditor()
        {
            InitializeComponent();

            _modelPanel.AddViewport(ModelPanelViewport.DefaultPerspective);

            _modelPanel.CurrentViewport.DefaultTranslate = new Vector3(0.0f, 10.0f, 250.0f);
            _modelPanel.CurrentViewport.AllowSelection = false;
            _modelPanel.CurrentViewport.BackgroundColor = Color.Black;

            pnlObjProps.Dock = DockStyle.Fill;
            pnlPlaneProps.Dock = DockStyle.Fill;
            pnlPointProps.Dock = DockStyle.Fill;

            _updating = true;
            cboMaterial.DataSource = Enum.GetValues(typeof(CollisionPlaneMaterialUnexpanded));
            cboType.DataSource = Enum.GetValues(typeof(CollisionPlaneType));
            _updating = false;
        }

        protected void TargetChanged(CollisionNode node)
        {
            ClearSelection();
            trackBar1.Value = 0;
            _snapMatrix = Matrix.Identity;
            _selectedObject = null;

            _modelPanel.ClearAll();

            _targetNode = node;

            PopulateModelList();
            PopulateObjectList();

            if (lstObjects.Items.Count > 0)
            {
                lstObjects.SelectedIndex = 0;
                _selectedObject = lstObjects.Items[0] as CollisionObject;
                //SnapObject();
            }
            ObjectSelected();

            _modelPanel.ResetCamera();
        }

        protected virtual void SelectionModified()
        {
            _selectedPlanes.Clear();
            foreach (CollisionLink l in _selectedLinks)
                foreach (CollisionPlane p in l._members)
                    if (_selectedLinks.Contains(p._linkLeft) &&
                        _selectedLinks.Contains(p._linkRight) &&
                        !_selectedPlanes.Contains(p))
                        _selectedPlanes.Add(p);

            pnlPlaneProps.Visible = false;
            pnlObjProps.Visible = false;
            pnlPointProps.Visible = false;
            panel3.Height = 0;

            if (_selectedPlanes.Count > 0)
            {
                pnlPlaneProps.Visible = true;
                panel3.Height = 205;
            }
            else if (_selectedLinks.Count == 1)
            {
                pnlPointProps.Visible = true;
                panel3.Height = 70;
            }

            UpdatePropPanels();
        }

        protected virtual void UpdatePropPanels()
        {
            _updating = true;
            
            if (pnlPlaneProps.Visible)
            {
                if(_selectedPlanes.Count <= 0)
                {
                    pnlPlaneProps.Visible = false;
                    return;
                }
                CollisionPlane p = _selectedPlanes[0];

                //Material
                if((byte)p._material >= 32)
                {
                    // Select basic by default (currently cannot display expanded collisions in default previewer)
                    cboMaterial.SelectedItem = (CollisionPlaneMaterialUnexpanded)(0x0);
                }
                else
                {
                    // Otherwise convert to the proper place in the unexpanded list
                    cboMaterial.SelectedItem = (CollisionPlaneMaterialUnexpanded)((byte)p._material);
                }
                //Type
                cboType.SelectedItem = p.Type;
                //Flags
                chkFallThrough.Checked = p.IsFallThrough;
                chkLeftLedge.Checked = p.IsLeftLedge;
                chkRightLedge.Checked = p.IsRightLedge;
                chkNoWalljump.Checked = p.IsNoWalljump;
                chkTypeCharacters.Checked = p.IsCharacters;
                chkTypeItems.Checked = p.IsItems;
                chkTypePokemonTrainer.Checked = p.IsPokemonTrainer;
                chkTypeRotating.Checked = p.IsRotating;
                //UnknownFlags
                chkFlagUnknown1.Checked = p.IsUnknownStageBox;
                chkFlagUnknown2.Checked = p.IsUnknownFlag1;
                chkFlagUnknown3.Checked = p.IsUnknownFlag3;
                chkFlagUnknown4.Checked = p.IsUnknownFlag4;
            }
            else if (pnlPointProps.Visible)
            {
                if (_selectedLinks.Count <= 0)
                {
                    pnlPointProps.Visible = false;
                    return;
                }
                numX.Value = _selectedLinks[0].Value._x;
                numY.Value = _selectedLinks[0].Value._y;
            }
            else if (pnlObjProps.Visible)
            {
                if(_selectedObject == null)
                {
                    pnlObjProps.Visible = false;
                    return;
                }
                txtModel.Text = _selectedObject._modelName;
                txtBone.Text = _selectedObject._boneName;
                chkObjUnk.Checked = _selectedObject._flags[0];
                chkObjModule.Checked = _selectedObject._flags[2];
                chkObjSSEUnk.Checked = _selectedObject._flags[3];
            }
            
            _updating = false;
        }

        protected List<IModel> _models = new List<IModel>();
        protected void PopulateModelList()
        {
            modelTree.BeginUpdate();
            modelTree.Nodes.Clear();
            _models.Clear();

            if ((_targetNode != null) && (_targetNode._parent != null))
                foreach (MDL0Node n in _targetNode._parent.FindChildrenByType(null, ResourceType.MDL0))
                {
                    TreeNode modelNode = new TreeNode(n._name) { Tag = n, Checked = true };
                    modelTree.Nodes.Add(modelNode);
                    _models.Add(n);

                    foreach (MDL0BoneNode bone in n._linker.BoneCache)
                        modelNode.Nodes.Add(new TreeNode(bone._name) { Tag = bone, Checked = true });

                    _modelPanel.AddTarget(n);
                    n.ResetToBindState();
                }

            modelTree.EndUpdate();
        }

        #region Object List

        protected void PopulateObjectList()
        {
            lstObjects.BeginUpdate();
            lstObjects.Items.Clear();

            if (_targetNode != null)
                foreach (CollisionObject obj in _targetNode._objects)
                {
                    obj._render = true;
                    lstObjects.Items.Add(obj, true);

                    MDL0Node model = _models.Where(m => m is MDL0Node && ((ResourceNode)m).Name == obj._modelName).FirstOrDefault() as MDL0Node;

                    if (model != null)
                    {
                        MDL0BoneNode bone = model._linker.BoneCache.Where(b => b.Name == obj._boneName).FirstOrDefault() as MDL0BoneNode;
                        if (bone != null)
                            obj._linkedBone = bone;
                    }

                    /*if (!obj._flags[1])
                        foreach (TreeNode n in modelTree.Nodes)
                            foreach (TreeNode b in n.Nodes)
                            {
                                MDL0BoneNode bone = b.Tag as MDL0BoneNode;
                                if (bone != null && bone.Name == obj._boneName && bone.BoneIndex == obj._boneIndex)
                                    obj._linkedBone = bone;
                            }*/
                }

            lstObjects.EndUpdate();
        }
        protected void lstObjects_MouseDown(object sender, MouseEventArgs e)
        {
            int index = lstObjects.IndexFromPoint(e.Location);
            lstObjects.SelectedIndex = index;
        }
        protected void lstObjects_SelectedValueChanged(object sender, EventArgs e)
        {
            _selectedObject = lstObjects.SelectedItem as CollisionObject;
            ObjectSelected();
        }
        protected void snapToolStripMenuItem_Click(object sender, EventArgs e) { SnapObject(); }

        protected void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedObject == null)
                return;

            int index = lstObjects.SelectedIndex;

            _targetNode._objects.Remove(_selectedObject);
            lstObjects.Items.Remove(_selectedObject);
            _selectedObject = null;
            ClearSelection();
            if (lstObjects.Items.Count > 0)
            {
                if (lstObjects.Items.Count > index)
                    lstObjects.SelectedIndex = index;
                else if (index > 0)
                    lstObjects.SelectedIndex = index - 1;
                ObjectSelected();
            }
            _modelPanel.Invalidate();
            TargetNode.SignalPropertyChange();
        }

        protected void newObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _selectedObject = new CollisionObject();
            _targetNode._objects.Add(_selectedObject);
            lstObjects.Items.Add(_selectedObject, true);
            lstObjects.SelectedItem = _selectedObject;
            //TargetNode.SignalPropertyChange();
        }

        protected void ObjectSelected()
        {
            pnlPlaneProps.Visible = false;
            pnlPointProps.Visible = false;
            pnlObjProps.Visible = false;
            panel3.Height = 0;
            if (_selectedObject != null)
            {
                pnlObjProps.Visible = true;
                panel3.Height = 130;
                UpdatePropPanels();
            }
        }

        protected void SnapObject()
        {
            if (_selectedObject == null)
                return;

            _updating = true;

            _snapMatrix = Matrix.Identity;

            for (int i = 0; i < lstObjects.Items.Count; i++)
                lstObjects.SetItemChecked(i, false);

            //Set snap matrix
            if (!String.IsNullOrEmpty(_selectedObject._modelName))
                foreach (TreeNode node in modelTree.Nodes)
                    if (node.Text == _selectedObject._modelName)
                    {
                        foreach (TreeNode bNode in node.Nodes)
                            if (bNode.Text == _selectedObject._boneName)
                            {
                                _snapMatrix = ((MDL0BoneNode)bNode.Tag)._inverseBindMatrix;
                                break;
                            }
                        break;
                    }

            //Show objects with similar bones
            for (int i = lstObjects.Items.Count; i-- > 0; )
            {
                CollisionObject obj = lstObjects.Items[i] as CollisionObject;
                if ((obj._modelName == _selectedObject._modelName) && (obj._boneName == _selectedObject._boneName))
                    lstObjects.SetItemChecked(i, true);
            }

            _updating = false;
            _modelPanel.Invalidate();
        }

        protected void lstObjects_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CollisionObject obj = lstObjects.Items[e.Index] as CollisionObject;
            obj._render = e.NewValue == CheckState.Checked;

            ClearSelection();

            if (!_updating)
                _modelPanel.Invalidate();
        }


        #endregion

        protected void ClearSelection()
        {
            foreach (CollisionLink l in _selectedLinks)
                l._highlight = false;
            _selectedLinks.Clear();
            _selectedPlanes.Clear();
        }

        protected void UpdateSelection(bool finish)
        {
            foreach (CollisionObject obj in _targetNode._objects)
                foreach (CollisionLink link in obj._points)
                {
                    link._highlight = false;
                    if (!obj._render)
                        continue;

                    Vector3 point = (Vector3)link.Value;

                    if (_selectInverse && point.Contained(_selectStart, _selectEnd, 0.0f))
                    {
                        if (finish)
                            _selectedLinks.Remove(link);
                        continue;
                    }

                    if (_selectedLinks.Contains(link))
                        link._highlight = true;
                    else if (!_selectInverse && point.Contained(_selectStart, _selectEnd, 0.0f))
                    {
                        link._highlight = true;
                        if (finish)
                            _selectedLinks.Add(link);
                    }
                }
        }
        public void UpdateTools()
        {
            if (_selecting || _hovering || (_selectedLinks.Count == 0))
                btnDelete.Enabled = btnFlipColl.Enabled = btnMerge.Enabled = btnSplit.Enabled = btnSameX.Enabled = btnSameY.Enabled = false;
            else
            {
                btnMerge.Enabled = btnSameX.Enabled = btnSameY.Enabled = _selectedLinks.Count > 1;
                btnDelete.Enabled = btnSplit.Enabled = true;
                btnFlipColl.Enabled = _selectedPlanes.Count > 0;
            }
        }

        protected void _treeObjects_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is CollisionObject)
                (e.Node.Tag as CollisionObject)._render = e.Node.Checked;
            if (e.Node.Tag is CollisionPlane)
                (e.Node.Tag as CollisionPlane)._render = e.Node.Checked;

            _modelPanel.Invalidate();
        }

        protected void chkAllModels_CheckedChanged(object sender, EventArgs e)
        {
            foreach (TreeNode node in modelTree.Nodes)
                node.Checked = chkAllModels.Checked;
        }

        protected void BeginHover(Vector3 point)
        {
            if (_hovering)
                return;

            if (!hasMoved) //Create undo for first move
            {
                CreateUndo();
                hasMoved = true;
                TargetNode.SignalPropertyChange();
            }
            
            _selectStart = _selectLast = point;
            _hovering = true;
            UpdateTools();
        }
        protected void UpdateHover(int x, int y)
        {
            if (!_hovering)
                return;

            _selectEnd = Vector3.IntersectZ(_modelPanel.CurrentViewport.UnProject(x, y, 0.0f), _modelPanel.CurrentViewport.UnProject(x, y, 1.0f), _selectLast._z);
            
            //Apply difference in start/end
            Vector3 diff = _selectEnd - _selectLast;
            _selectLast = _selectEnd;

            //Move points
            foreach (CollisionLink p in _selectedLinks)
                p.Value += diff;
            
            _modelPanel.Invalidate();

            UpdatePropPanels();
        }
        protected void CancelHover()
        {
            if (!_hovering)
                return;

            if (hasMoved)
            {
                undoSaves.RemoveAt(undoSaves.Count - 1);
                saveIndex--;
                hasMoved = false;
                if (saveIndex == 0)
                    btnUndo.Enabled = false;
            }

            _hovering = false;

            if (_creating)
            {
                _creating = false;
                //Delete points/plane
                _selectedLinks[0].Pop();
                ClearSelection();
                SelectionModified();
            }
            else
            {
                Vector3 diff = _selectStart - _selectLast;
                foreach (CollisionLink l in _selectedLinks)
                    l.Value += diff;
            }
            _modelPanel.Invalidate();
            UpdatePropPanels();
        }
        protected void FinishHover() { _hovering = false; }
        protected void BeginSelection(Vector3 point, bool inverse)
        {
            if (_selecting)
                return;

            _selectStart = _selectEnd = point;

            _selectEnd._z += SelectWidth;
            _selectStart._z -= SelectWidth;

            _selecting = true;
            _selectInverse = inverse;

            UpdateTools();
        }
        protected void CancelSelection()
        {
            if (!_selecting)
                return;

            _selecting = false;
            _selectStart = _selectEnd = new Vector3(float.MaxValue);
            UpdateSelection(false);
            _modelPanel.Invalidate();
        }
        protected void FinishSelection()
        {
            if (!_selecting)
                return;

            _selecting = false;
            UpdateSelection(true);
            _modelPanel.Invalidate();

            SelectionModified();

            //Selection Area Selected.
        }

        System.Drawing.Point _RCstart, _RCend;
        protected void _modelPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                bool create = Control.ModifierKeys == Keys.Alt;
                bool add = Control.ModifierKeys == Keys.Shift;
                bool subtract = Control.ModifierKeys == Keys.Control;
                bool move = Control.ModifierKeys == (Keys.Control | Keys.Shift);

                float depth = _modelPanel.GetDepth(e.X, e.Y);
                Vector3 target = _modelPanel.CurrentViewport.UnProject(e.X, e.Y, depth);
                Vector2 point;

                if (!move && (depth < 1.0f))
                {
                    point = (Vector2)target;
                    
                    //Hit-detect points first
                    foreach (CollisionObject obj in _targetNode._objects)
                        if (obj._render)
                            foreach (CollisionLink p in obj._points)
                                if (p.Value.Contained(point, point, PointSelectRadius))
                                {
                                    if (create)
                                    {
                                        //Connect all selected links to point
                                        foreach (CollisionLink l in _selectedLinks)
                                            l.Connect(p);

                                        //Select point
                                        ClearSelection();
                                        p._highlight = true;
                                        _selectedLinks.Add(p);
                                        SelectionModified();

                                        _modelPanel.Invalidate();
                                        return;
                                    }

                                    if (subtract)
                                    {
                                        p._highlight = false;
                                        _selectedLinks.Remove(p);
                                        _modelPanel.Invalidate();
                                        SelectionModified();
                                    }
                                    else if (!_selectedLinks.Contains(p))
                                    {
                                        if (!add)
                                            ClearSelection();

                                        _selectedLinks.Add(p);
                                        p._highlight = true;
                                        _modelPanel.Invalidate();
                                        SelectionModified();
                                    }

                                    if ((!add) && (!subtract))
                                        BeginHover(target);
                                    //Single Link Selected
                                    return;
                                }

                    float dist;
                    float bestDist = float.MaxValue;
                    CollisionPlane bestMatch = null;

                    //Hit-detect planes finding best match
                    foreach (CollisionObject obj in _targetNode._objects)
                        if (obj._render)
                            foreach (CollisionPlane p in obj._planes)
                                if (point.Contained(p.PointLeft, p.PointRight, PointSelectRadius))
                                {
                                    dist = point.TrueDistance(p.PointLeft) + point.TrueDistance(p.PointRight) - p.PointLeft.TrueDistance(p.PointRight);
                                    if (dist < bestDist)
                                    { bestDist = dist; bestMatch = p; }
                                }

                    if (bestMatch != null)
                    {
                        if (create)
                        {
                            ClearSelection();

                            _selectedLinks.Add(bestMatch.Split(point));
                            _selectedLinks[0]._highlight = true;
                            SelectionModified();
                            _modelPanel.Invalidate();

                            _creating = true;
                            BeginHover(target);

                            return;
                        }

                        if (subtract)
                        {
                            _selectedLinks.Remove(bestMatch._linkLeft);
                            _selectedLinks.Remove(bestMatch._linkRight);
                            bestMatch._linkLeft._highlight = bestMatch._linkRight._highlight = false;
                            _modelPanel.Invalidate();

                            SelectionModified();
                            return;
                        }

                        //Select both points
                        if (!_selectedLinks.Contains(bestMatch._linkLeft) || !_selectedLinks.Contains(bestMatch._linkRight))
                        {
                            if (!add)
                                ClearSelection();

                            _selectedLinks.Add(bestMatch._linkLeft);
                            _selectedLinks.Add(bestMatch._linkRight);
                            bestMatch._linkLeft._highlight = bestMatch._linkRight._highlight = true;
                            _modelPanel.Invalidate();

                            SelectionModified();
                        }

                        if (!add)
                            BeginHover(target);
                        //Single Platform Selected;
                        return;
                    }
                }

                //Nothing found :(

                //Trace ray to Z axis
                target = Vector3.IntersectZ(target, _modelPanel.CurrentViewport.UnProject(e.X, e.Y, 0.0f), 0.0f);
                point = (Vector2)target;

                if (create)
                {
                    if (_selectedLinks.Count == 0)
                    {
                        if (_selectedObject == null)
                            return;

                        _creating = true;

                        //Create two points and hover
                        CollisionLink point1 = new CollisionLink(_selectedObject, point).Branch(point);

                        _selectedLinks.Add(point1);
                        point1._highlight = true;

                        SelectionModified();
                        BeginHover(target);
                        _modelPanel.Invalidate();
                        return;
                    }
                    else if (_selectedLinks.Count == 1)
                    {
                        //Create new plane extending to point
                        CollisionLink link = _selectedLinks[0];
                        _selectedLinks[0] = link.Branch((Vector2)target);
                        _selectedLinks[0]._highlight = true;
                        link._highlight = false;
                        SelectionModified();
                        _modelPanel.Invalidate();

                        //Hover new point so it can be moved
                        BeginHover(target);
                        return;
                    }
                    else if (_selectedPlanes.Count > 0)
                    {
                        //Find two closest points and insert between
                        CollisionPlane bestMatch = null;
                        if (_selectedPlanes.Count == 1)
                            bestMatch = _selectedPlanes[0];
                        else
                        {
                            float dist;
                            float bestDist = float.MaxValue;

                            foreach (CollisionPlane p in _selectedPlanes)
                            {
                                dist = point.TrueDistance(p.PointLeft) + point.TrueDistance(p.PointRight) - p.PointLeft.TrueDistance(p.PointRight);
                                if (dist < bestDist)
                                { bestDist = dist; bestMatch = p; }
                            }
                        }

                        if (bestMatch == null)
                            bestMatch = _selectedPlanes[0];
                        ClearSelection();
                        if (bestMatch != null)
                        {
                            _selectedLinks.Add(bestMatch.Split(point));
                            _selectedLinks[0]._highlight = true;
                            SelectionModified();
                            _modelPanel.Invalidate();

                            _creating = true;
                            BeginHover(target);
                        }
                        
                        return;
                    }
                    else
                    {
                        //Create new planes extending to point
                        CollisionLink link = null;
                        List<CollisionLink> links = new List<CollisionLink>();
                        _creating = true;
                        foreach (CollisionLink l in _selectedLinks)
                        {
                            links.Add(l.Branch((Vector2)target));
                            l._highlight = false;
                        }
                        link = links[0];
                        links.RemoveAt(0);
                        for (int x = 0; x < links.Count;)
                        {
                            if (link.Merge(links[x]))
                            {
                                links.RemoveAt(x);
                            }
                            else
                                x++;
                        }
                        _selectedLinks.Clear();
                        _selectedLinks.Add(link);
                        link._highlight = true;
                        SelectionModified();
                        _modelPanel.Invalidate();

                        //Hover new point so it can be moved
                        BeginHover(target);

                        return;
                    }
                }

                if (move)
                {
                    if (_selectedLinks.Count > 0)
                        BeginHover(target);
                    return;
                }

                if (!add && !subtract)
                    ClearSelection();

                BeginSelection(target, subtract);
            }
            else if(e.Button == MouseButtons.Right)
            {
                _RCstart = Cursor.Position;
            }
        }
        protected void _modelPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (saveIndex - 1 > 0 && saveIndex - 1 < undoSaves.Count)
                    if (undoSaves[saveIndex - 1]._collisionLinks[0].Value.ToString() == undoSaves[saveIndex - 1]._linkVectors[0].ToString())//If equal to starting point, remove.
                    {
                        undoSaves.RemoveAt(saveIndex - 1);
                        saveIndex--;
                        if (saveIndex == 0)
                            btnUndo.Enabled = false;
                    }
                
                hasMoved = false;
                FinishSelection();
                FinishHover();
                UpdateTools();
            }
            else if(e.Button == MouseButtons.Right)
            {
                _RCend = Cursor.Position;
                if ((((_RCstart.X - _RCend.X) * (_RCstart.X - _RCend.X) + (_RCstart.Y - _RCend.Y) * (_RCstart.Y - _RCend.Y)) < 2 * 2) && _selectedLinks != null && _selectedLinks.Count > 0)
                {
                    this.contextMenuStrip3.Show(System.Windows.Forms.Cursor.Position);
                }
            }
        }

        protected void _modelPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (_selecting) //Selection Box
            {
                Vector3 ray1 = _modelPanel.CurrentViewport.UnProject(new Vector3(e.X, e.Y, 0.0f));
                Vector3 ray2 = _modelPanel.CurrentViewport.UnProject(new Vector3(e.X, e.Y, 1.0f));

                _selectEnd = Vector3.IntersectZ(ray1, ray2, 0.0f);
                _selectEnd._z += SelectWidth;

                //Update selection
                UpdateSelection(false);

                _modelPanel.Invalidate();
            }

            UpdateHover(e.X, e.Y);
        }

        protected bool PointCollides(Vector3 point)
        {
            float f;
            return PointCollides(point, out f);
        }
        protected bool PointCollides(Vector3 point, out float y_result)
        {
            y_result = float.MaxValue;
            Vector2 v2 = new Vector2(point._x, point._y);
            foreach (CollisionObject obj in _targetNode._objects)
            {
                if (obj._render || true)
                {
                    foreach (CollisionPlane plane in obj._planes)
                    {
                        if (plane._type == BrawlLib.SSBBTypes.CollisionPlaneType.Floor && plane.IsCharacters)
                        {
                            if (plane.PointLeft._x <= v2._x && plane.PointRight._x >= v2._x)
                            {
                                float x = v2._x;
                                float m = (plane.PointLeft._y - plane.PointRight._y)
                                    / (plane.PointLeft._x - plane.PointRight._x);
                                float b = plane.PointRight._y - m * plane.PointRight._x;
                                float y_target = m * x + b;
                                //Console.WriteLine(y_target);
                                if (Math.Abs(y_target - v2._y) <= Math.Abs(y_result - v2._y))
                                {
                                    y_result = y_target;
                                }
                            }
                        }
                    }
                }
            }
            return (Math.Abs(y_result - v2._y) <= 5);
        }

        protected void _modelPanel_PreRender(object sender)
        {

        }

        protected unsafe void _modelPanel_PostRender(object sender)
        {
            //Clear depth buffer so we can hit-detect
            GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            //Render objects
            if (_targetNode != null)
                _targetNode.Render();

            if (_modelPanel.RenderBones)
                foreach (IRenderedObject o in _modelPanel._renderList)
                    if (o is IModel)
                        ((IModel)o).RenderBones(_modelPanel.CurrentViewport);

            #region RenderOverlays
            GL.Disable(EnableCap.DepthTest);
            List<MDL0BoneNode> ItemBones = new List<MDL0BoneNode>();

            MDL0Node stgPos = null;

            MDL0BoneNode CamBone0 = null, CamBone1 = null,
                         DeathBone0 = null, DeathBone1 = null;

            foreach (MDL0Node m in _models)
            {
                if ((m.Name.Contains("StgPosition", StringComparison.OrdinalIgnoreCase)) || m.Name.Contains("stagePosition", StringComparison.OrdinalIgnoreCase))
                {
                    stgPos = m;
                    break;
                }
            }

            if (stgPos != null)
                foreach (MDL0BoneNode bone in stgPos._linker.BoneCache)
                {
                    if (bone._name == "CamLimit0N") { CamBone0 = bone; }
                    else if (bone.Name == "CamLimit1N") { CamBone1 = bone; }
                    else if (bone.Name == "Dead0N") { DeathBone0 = bone; }
                    else if (bone.Name == "Dead1N") { DeathBone1 = bone; }
                    else if (bone._name.Contains("Player") && btnSpawns.Checked)
                    {
                        Vector3 position = bone._frameMatrix.GetPoint();

                        if (PointCollides(position))
                            GL.Color4(0.0f, 1.0f, 0.0f, 0.5f);
                        else
                            GL.Color4(1.0f, 0.0f, 0.0f, 0.5f);

                        TKContext.DrawSphere(position, 5.0f, 32);
                    }
                    else if (bone._name.Contains("Rebirth") && btnSpawns.Checked)
                    {
                        GL.Color4(1.0f, 1.0f, 1.0f, 0.1f);
                        TKContext.DrawSphere(bone._frameMatrix.GetPoint(), 5.0f, 32);
                    }
                    else if (bone._name.Contains("Item"))
                        ItemBones.Add(bone);
                }

            //Render item fields if checked
            if (ItemBones != null && btnItems.Checked)
            {
                GL.Color4(0.5f, 0.0f, 1.0f, 0.4f);
                for (int i = 0; i < ItemBones.Count; i += 2)
                {
                    Vector3 pos1, pos2;
                    if (ItemBones[i]._frameMatrix.GetPoint()._y == ItemBones[i + 1]._frameMatrix.GetPoint()._y)
                    {
                        pos1 = new Vector3(ItemBones[i]._frameMatrix.GetPoint()._x, ItemBones[i]._frameMatrix.GetPoint()._y + 1.5f, 1.0f);
                        pos2 = new Vector3(ItemBones[i + 1]._frameMatrix.GetPoint()._x, ItemBones[i + 1]._frameMatrix.GetPoint()._y - 1.5f, 1.0f);
                    }
                    else
                    {
                        pos1 = new Vector3(ItemBones[i]._frameMatrix.GetPoint()._x, ItemBones[i]._frameMatrix.GetPoint()._y, 1.0f);
                        pos2 = new Vector3(ItemBones[i + 1]._frameMatrix.GetPoint()._x, ItemBones[i + 1]._frameMatrix.GetPoint()._y, 1.0f);
                    }


                    if (pos1._x != pos2._x)
                        TKContext.DrawBox(pos1, pos2);
                    else
                        TKContext.DrawSphere(new Vector3(ItemBones[i]._frameMatrix.GetPoint()._x, ItemBones[i]._frameMatrix.GetPoint()._y, pos1._z), 3.0f, 32);
                }
            }

            //Render boundaries if checked
            if (CamBone0 != null && CamBone1 != null && btnBoundaries.Checked)
            {
                //GL.Clear(ClearBufferMask.DepthBufferBit);
                GL.Disable(EnableCap.DepthTest);
                GL.Disable(EnableCap.Lighting);
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                GL.Enable(EnableCap.CullFace);
                GL.CullFace(CullFaceMode.Front);

                GL.Color4(Color.Blue);
                GL.Begin(BeginMode.LineLoop);
                GL.LineWidth(15.0f);

                Vector3
                    camBone0 = CamBone0._frameMatrix.GetPoint(),
                    camBone1 = CamBone1._frameMatrix.GetPoint(),
                    deathBone0 = DeathBone0._frameMatrix.GetPoint(),
                    deathBone1 = DeathBone1._frameMatrix.GetPoint();

                GL.Vertex2(camBone0._x, camBone0._y);
                GL.Vertex2(camBone1._x, camBone0._y);
                GL.Vertex2(camBone1._x, camBone1._y);
                GL.Vertex2(camBone0._x, camBone1._y);
                GL.End();
                GL.Begin(BeginMode.LineLoop);
                GL.Color4(Color.Red);
                GL.Vertex2(deathBone0._x, deathBone0._y);
                GL.Vertex2(deathBone1._x, deathBone0._y);
                GL.Vertex2(deathBone1._x, deathBone1._y);
                GL.Vertex2(deathBone0._x, deathBone1._y);
                GL.End();
                GL.Color4(0.0f, 0.5f, 1.0f, 0.3f);
                GL.Begin(BeginMode.TriangleFan);
                GL.Vertex2(camBone0._x, camBone0._y);
                GL.Vertex2(deathBone0._x, deathBone0._y);
                GL.Vertex2(deathBone1._x, deathBone0._y);
                GL.Vertex2(camBone1._x, camBone0._y);
                GL.End();
                GL.Begin(BeginMode.TriangleFan);
                GL.Vertex2(camBone1._x, camBone1._y);
                GL.Vertex2(deathBone1._x, deathBone1._y);
                GL.Vertex2(deathBone0._x, deathBone1._y);
                GL.Vertex2(camBone0._x, camBone1._y);
                GL.End();
                GL.Begin(BeginMode.TriangleFan);
                GL.Vertex2(camBone1._x, camBone0._y);
                GL.Vertex2(deathBone1._x, deathBone0._y);
                GL.Vertex2(deathBone1._x, deathBone1._y);
                GL.Vertex2(camBone1._x, camBone1._y);
                GL.End();
                GL.Begin(BeginMode.TriangleFan);
                GL.Vertex2(camBone0._x, camBone1._y);
                GL.Vertex2(deathBone0._x, deathBone1._y);
                GL.Vertex2(deathBone0._x, deathBone0._y);
                GL.Vertex2(camBone0._x, camBone0._y);
                GL.End();
            }

            #endregion

            //Render selection box
            if (!_selecting)
                return;

            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.CullFace);

            //Draw lines
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.Color4(0.0f, 0.0f, 1.0f, 0.5f);
            TKContext.DrawBox(_selectStart, _selectEnd);

            //Draw box
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Color4(1.0f, 1.0f, 0.0f, 0.2f);
            TKContext.DrawBox(_selectStart, _selectEnd);
        }

        protected void btnSplit_Click(object sender, EventArgs e)
        {
            ClearUndoBuffer();
            for (int i = _selectedLinks.Count; --i >= 0; )
                _selectedLinks[i].Split();
            ClearSelection();
            SelectionModified();
            _modelPanel.Invalidate();
            TargetNode.SignalPropertyChange();
        }

        protected void btnMerge_Click(object sender, EventArgs e)
        {
            ClearUndoBuffer();

            for (int i = 0; i < _selectedLinks.Count - 1; )
            {
                CollisionLink link = _selectedLinks[i++];
                Vector2 pos = link.Value;
                int count = 1;
                for (int x = i; x < _selectedLinks.Count;)
                {
                    if (link.Merge(_selectedLinks[x]))
                    {
                        pos += _selectedLinks[x].Value;
                        count++;
                        _selectedLinks.RemoveAt(x);
                    }
                    else
                        x++;
                }
                link.Value = pos / count;
                TargetNode.SignalPropertyChange();
            }
            _modelPanel.Invalidate();
        }

        protected void trackBar1_Scroll(object sender, EventArgs e) { _modelPanel.Invalidate(); }
        protected void btnResetRot_Click(object sender, EventArgs e) { trackBar1.Value = 0; _modelPanel.Invalidate(); }
        protected void btnResetCam_Click(object sender, EventArgs e) { _modelPanel.ResetCamera(); }
        
        // StageBox Perspective viewer
        protected void btnPerspectiveCam_Click(object sender, EventArgs e) {
            if (_updating)
                return;

            btnPerspectiveCam.Checked = true;
            btnOrthographicCam.Checked = false;
            if (_modelPanel.CurrentViewport.ViewType != ViewportProjection.Perspective)
            {
                _modelPanel.ResetCamera();
                _modelPanel.CurrentViewport.ViewType = ViewportProjection.Perspective;
            }
        }
        
        // StageBox Orthographic viewer
        protected void btnOrthographicCam_Click(object sender, EventArgs e)
        {
            if (_updating)
                return;

            btnPerspectiveCam.Checked = false;
            btnOrthographicCam.Checked = true;
            if(_modelPanel.CurrentViewport.ViewType != ViewportProjection.Orthographic)
            {
                _modelPanel.ResetCamera();
                _modelPanel.CurrentViewport.ViewType = ViewportProjection.Orthographic;
            }
        }

        protected void btnSpawns_Click(object sender, EventArgs e)
        {
            if (_updating)
                return;

            btnSpawns.Checked = !btnSpawns.Checked;
            _modelPanel.Invalidate();
        }

        protected void btnItems_Click(object sender, EventArgs e)
        {
            if (_updating)
                return;

            btnItems.Checked = !btnItems.Checked;
            _modelPanel.Invalidate();
        }

        protected void btnBoundaries_Click(object sender, EventArgs e)
        {
            if (_updating)
                return;

            btnBoundaries.Checked = !btnBoundaries.Checked;
            _modelPanel.Invalidate();
        }

        protected void _modelPanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (_hovering)
                    CancelHover();
                else if (_selecting)
                    CancelSelection();
                else
                {
                    ClearSelection();
                    _modelPanel.Invalidate();
                }
            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (_selectedPlanes.Count > 0)
                {
                    foreach (CollisionPlane plane in _selectedPlanes)
                        plane.Delete();
                    TargetNode.SignalPropertyChange();
                }
                else if (_selectedLinks.Count > 0)
                {
                    for(int i = 0; i < _selectedLinks.Count; i++)
                        _selectedLinks[i].Pop();
                    TargetNode.SignalPropertyChange();
                }

                ClearSelection();
                SelectionModified();
                _modelPanel.Invalidate();
            }
            else if (Control.ModifierKeys == Keys.Control)
            {
                if (e.KeyCode == Keys.Z)
                {
                    if (_hovering)
                        CancelHover();
                    else if (btnUndo.Enabled)
                        Undo(this, null);
                }
                else if (e.KeyCode == Keys.Y)
                {
                    if (_hovering)
                        CancelHover();
                    else if (btnRedo.Enabled)
                        Redo(this, null);
                }
            }
            else if (e.KeyCode == Keys.OemOpenBrackets)
            {
                CollisionLink link = null;
                bool two = false;

                if (_selectedPlanes.Count == 1)
                {
                    link = _selectedPlanes[0]._linkLeft;
                    two = true;
                }
                else if (_selectedLinks.Count == 1)
                    link = _selectedLinks[0];

                if (link != null)
                    foreach (CollisionPlane p in link._members)
                        if (p._linkRight == link)
                        {
                            ClearSelection();

                            _selectedLinks.Add(p._linkLeft);
                            p._linkLeft._highlight = true;
                            if (two)
                            {
                                _selectedLinks.Add(p._linkRight);
                                p._linkRight._highlight = true;
                            }

                            SelectionModified();
                            _modelPanel.Invalidate();
                            break;
                        }
            }
            else if (e.KeyCode == Keys.OemCloseBrackets)
            {
                CollisionLink link = null;
                bool two = false;

                if (_selectedPlanes.Count == 1)
                {
                    link = _selectedPlanes[0]._linkRight;
                    two = true;
                }
                else if (_selectedLinks.Count == 1)
                    link = _selectedLinks[0];

                if (link != null)
                    foreach (CollisionPlane p in link._members)
                        if (p._linkLeft == link)
                        {
                            ClearSelection();

                            _selectedLinks.Add(p._linkRight);
                            p._linkRight._highlight = true;
                            if (two)
                            {
                                _selectedLinks.Add(p._linkLeft);
                                p._linkLeft._highlight = true;
                            }
                            SelectionModified();

                            _modelPanel.Invalidate();
                            break;
                        }
            }
            else if (e.KeyCode == Keys.W)
            {
                CreateUndo();
                float amount = Control.ModifierKeys == Keys.Shift ? LargeIncrement : SmallIncrement;
                foreach (CollisionLink link in _selectedLinks)
                    link._rawValue._y += amount;
                UpdatePropPanels();
                _modelPanel.Invalidate();
                TargetNode.SignalPropertyChange();
            }
            else if (e.KeyCode == Keys.S)
            {
                CreateUndo();
                float amount = Control.ModifierKeys == Keys.Shift ? LargeIncrement : SmallIncrement;
                foreach (CollisionLink link in _selectedLinks)
                    link._rawValue._y -= amount;
                UpdatePropPanels();
                _modelPanel.Invalidate();
                TargetNode.SignalPropertyChange();
            }
            else if (e.KeyCode == Keys.A)
            {
                CreateUndo();
                float amount = Control.ModifierKeys == Keys.Shift ? LargeIncrement : SmallIncrement;
                foreach (CollisionLink link in _selectedLinks)
                    link._rawValue._x -= amount;
                UpdatePropPanels();
                _modelPanel.Invalidate();
                TargetNode.SignalPropertyChange();
            }
            else if (e.KeyCode == Keys.D)
            {
                CreateUndo();
                float amount = Control.ModifierKeys == Keys.Shift ? LargeIncrement : SmallIncrement;
                foreach (CollisionLink link in _selectedLinks)
                    link._rawValue._x += amount;
                UpdatePropPanels();
                _modelPanel.Invalidate();
                TargetNode.SignalPropertyChange();
            }
        }

        #region Plane Properties

        protected void cboMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating) return;
            foreach (CollisionPlane plane in _selectedPlanes)
                plane._material = (CollisionPlaneMaterial)((byte)cboMaterial.SelectedItem);
            TargetNode.SignalPropertyChange();
        }
        protected void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating) return;
            foreach (CollisionPlane plane in _selectedPlanes)
            {
                plane.Type = (CollisionPlaneType)cboType.SelectedItem;
                if (!plane.IsRotating)
                {
                    if (!plane.IsFloor)
                    {
                        plane.IsFallThrough = false;
                        chkFallThrough.Checked = false;
                        plane.IsRightLedge = false;
                        chkRightLedge.Checked = false;
                        plane.IsLeftLedge = false;
                        chkLeftLedge.Checked = false;
                    }
                    if(!plane.IsWall)
                    {
                        plane.IsNoWalljump = false;
                        chkNoWalljump.Checked = false;
                    }
                }
            }
            _modelPanel.Invalidate();
            TargetNode.SignalPropertyChange();
        }

        protected void chkTypeCharacters_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;
            if (!_errorChecking)
            {
                chkTypeCharacters_CheckedChanged_NoErrorHandling(sender, e);
                return;
            }
            TargetNode.SignalPropertyChange();
            bool selection = chkTypeCharacters.Checked;
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsCharacters = selection;
                if (p.IsCharacters)
                {
                    p.IsItems = false;
                    chkTypeItems.Checked = false;
                    p.IsPokemonTrainer = false;
                    chkTypePokemonTrainer.Checked = false;
                } else
                {
                    p.IsFallThrough = false;
                    chkFallThrough.Checked = false;
                    p.IsNoWalljump = false;
                    chkNoWalljump.Checked = false;
                    p.IsRightLedge = false;
                    chkRightLedge.Checked = false;
                    p.IsLeftLedge = false;
                    chkLeftLedge.Checked = false;
                }
            }
            _modelPanel.Invalidate();
        }

        protected void chkTypeItems_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;
            if (!_errorChecking)
            {
                chkTypeItems_CheckedChanged_NoErrorHandling(sender, e);
                return;
            }
            TargetNode.SignalPropertyChange();
            bool selection = chkTypeItems.Checked;
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsItems = selection;
                if (p.IsItems)
                {
                    p.IsCharacters = false;
                    chkTypeCharacters.Checked = false;
                    p.IsFallThrough = false;
                    chkFallThrough.Checked = false;
                    p.IsNoWalljump = false;
                    chkNoWalljump.Checked = false;
                    p.IsRightLedge = false;
                    chkRightLedge.Checked = false;
                    p.IsLeftLedge = false;
                    chkLeftLedge.Checked = false;
                }
            }
            _modelPanel.Invalidate();
        }

        protected void chkTypePokemonTrainer_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;
            if (!_errorChecking)
            {
                chkTypePokemonTrainer_CheckedChanged_NoErrorHandling(sender, e);
                return;
            }
            TargetNode.SignalPropertyChange();
            bool selection = chkTypePokemonTrainer.Checked;
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsPokemonTrainer = selection;
                if (p.IsPokemonTrainer)
                {
                    p.IsCharacters = false;
                    chkTypeCharacters.Checked = false;
                    p.IsFallThrough = false;
                    chkFallThrough.Checked = false;
                    p.IsNoWalljump = false;
                    chkNoWalljump.Checked = false;
                    p.IsRightLedge = false;
                    chkRightLedge.Checked = false;
                    p.IsLeftLedge = false;
                    chkLeftLedge.Checked = false;
                }
            }
            _modelPanel.Invalidate();
        }

        protected void chkTypeRotating_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;
            if (!_errorChecking)
            {
                chkTypeRotating_CheckedChanged_NoErrorHandling(sender, e);
                return;
            }
            TargetNode.SignalPropertyChange();
            bool selection = chkTypeRotating.Checked;
            CollisionPlaneType firstType = _selectedPlanes[0].Type;
            bool allSameType = true;
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsRotating = selection;
                if (!p.IsRotating)
                {
                    if (!p.IsFloor)
                    {
                        p.IsFallThrough = false;
                        p.IsRightLedge = false;
                        p.IsLeftLedge = false;
                    }
                    if (!p.IsWall)
                    {
                        p.IsNoWalljump = false;
                    }
                }
                if (allSameType)
                {
                    if (p.IsWall && (firstType == CollisionPlaneType.LeftWall || firstType == CollisionPlaneType.RightWall))
                    {
                        // This is fine as far as types are concerned
                    }
                    else if (p.Type != firstType)
                    {
                        allSameType = false;
                    }
                }
            }
            if ((_selectedPlanes.Count == 1 || allSameType) && _selectedPlanes.Count > 0)
            {
                chkTypeRotating.Checked = _selectedPlanes[0].IsRotating;
                if (!_selectedPlanes[0].IsRotating)
                {
                    if (!_selectedPlanes[0].IsFloor)
                    {
                        chkFallThrough.Checked = false;
                        chkRightLedge.Checked = false;
                        chkLeftLedge.Checked = false;
                    }
                    if (!_selectedPlanes[0].IsWall)
                    {
                        chkNoWalljump.Checked = false;
                    }
                }
            }
            _modelPanel.Invalidate();
        }

        protected void chkFallThrough_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;
            if (!_errorChecking)
            {
                chkFallThrough_CheckedChanged_NoErrorHandling(sender, e);
                return;
            }
            TargetNode.SignalPropertyChange();
            bool selection = chkFallThrough.Checked;
            CollisionPlaneType firstType = _selectedPlanes[0].Type;
            bool firstIsRotating = _selectedPlanes[0].IsRotating;
            bool allSameType = true;
            bool allNonCharacters = !_selectedPlanes[0].IsCharacters;
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsFallThrough = selection;
                if ((!p.IsFloor && !p.IsRotating) || !p.IsCharacters)
                {
                    p.IsFallThrough = false;
                }

                if (allSameType)
                {
                    if (p.IsRotating != firstIsRotating)
                    {
                        allSameType = false;
                    }
                    if (p.IsWall && (firstType == CollisionPlaneType.LeftWall || firstType == CollisionPlaneType.RightWall))
                    {
                        // This is fine as far as types are concerned
                    }
                    else if (p.Type != firstType)
                    {
                        allSameType = false;
                    }
                }

                if (allNonCharacters)
                {
                    allNonCharacters = !p.IsCharacters;
                }
            }
            if ((_selectedPlanes.Count == 1 || allSameType) && _selectedPlanes.Count > 0)
            {
                chkFallThrough.Checked = _selectedPlanes[0].IsFallThrough;
                if ((!_selectedPlanes[0].IsFloor && !_selectedPlanes[0].IsRotating) || !_selectedPlanes[0].IsCharacters)
                {
                    chkFallThrough.Checked = false;
                }
            }
            if(allNonCharacters)
            {
                chkFallThrough.Checked = false;
            }
        }

        protected void chkLeftLedge_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;
            if (!_errorChecking)
            {
                chkLeftLedge_CheckedChanged_NoErrorHandling(sender, e);
                return;
            }
            TargetNode.SignalPropertyChange();
            bool selection = chkLeftLedge.Checked;
            CollisionPlaneType firstType = _selectedPlanes[0].Type;
            bool firstIsRotating = _selectedPlanes[0].IsRotating;
            bool allSameType = true;
            bool allNonCharacters = !_selectedPlanes[0].IsCharacters;
            bool anyNoLedgeFloors = false;
            bool allNoLedge = true;
            foreach (CollisionPlane p in _selectedPlanes)
            {
                bool noLedge = false;
                if(!p.IsRotating)
                    foreach (CollisionPlane x in p._linkLeft._members)
                    {
                        if (x != p)
                        {
                            if ((x.Type == CollisionPlaneType.Floor || x.Type == CollisionPlaneType.RightWall) && x.IsCharacters)
                            {
                                noLedge = true;
                                if(x.Type == CollisionPlaneType.Floor)
                                    anyNoLedgeFloors = true;
                            }
                        }
                    }
                if ((!p.IsFloor && !p.IsRotating) || !p.IsCharacters)
                {
                    noLedge = true;
                }

                if (!noLedge)
                {
                    allNoLedge = false;
                    p.IsLeftLedge = selection;
                }
                else
                {
                    p.IsLeftLedge = false;
                    continue;
                }
                
                if (p.IsLeftLedge)
                {
                    p.IsRightLedge = false;
                }
                if(allSameType)
                {
                    if(p.IsRotating != firstIsRotating)
                    {
                        allSameType = false;
                    }
                    if(p.IsWall && (firstType == CollisionPlaneType.LeftWall || firstType == CollisionPlaneType.RightWall))
                    {
                        // This is fine as far as types are concerned
                    }
                    else if(p.Type != firstType)
                    {
                        allSameType = false;
                    }
                }
                if (allNonCharacters)
                {
                    allNonCharacters = !p.IsCharacters;
                }
            }

            if (allNonCharacters)
            {
                chkLeftLedge.Checked = false;
            }
            else if (allNoLedge)
            {
                chkLeftLedge.Checked = false;
            }
            else if (anyNoLedgeFloors)
            {
                if (chkLeftLedge.Checked != selection)
                    chkLeftLedge.Checked = selection;
            }
            else if (!anyNoLedgeFloors)
            {
                chkRightLedge.Checked = false;
                if (chkLeftLedge.Checked != selection)
                    chkLeftLedge.Checked = selection;
            }
            if ((_selectedPlanes.Count == 1 || allSameType) && _selectedPlanes.Count > 0 && !anyNoLedgeFloors && !allNonCharacters)
            {
                chkLeftLedge.Checked = _selectedPlanes[0].IsLeftLedge;
                if (_selectedPlanes[0].IsLeftLedge)
                {
                    chkRightLedge.Checked = false;
                }
                if (!_selectedPlanes[0].IsFloor && !_selectedPlanes[0].IsRotating)
                {
                    chkRightLedge.Checked = false;
                    chkLeftLedge.Checked = false;
                }
            }
            _modelPanel.Invalidate();
        }

        protected void chkRightLedge_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;
            if (!_errorChecking)
            {
                chkRightLedge_CheckedChanged_NoErrorHandling(sender, e);
                return;
            }
            TargetNode.SignalPropertyChange();
            bool selection = chkRightLedge.Checked;
            CollisionPlaneType firstType = _selectedPlanes[0].Type;
            bool firstIsRotating = _selectedPlanes[0].IsRotating;
            bool allSameType = true;
            bool allNonCharacters = !_selectedPlanes[0].IsCharacters;
            bool anyNoLedgeFloors = false;
            bool allNoLedge = true;

            foreach (CollisionPlane p in _selectedPlanes)
            {
                bool noLedge = false;

                if (!p.IsRotating)
                    foreach (CollisionPlane x in p._linkRight._members)
                    {
                        if (x != p)
                        {
                            if ((x.Type == CollisionPlaneType.Floor || x.Type == CollisionPlaneType.LeftWall) && x.IsCharacters)
                            {
                                noLedge = true;
                                if (x.Type == CollisionPlaneType.Floor)
                                    anyNoLedgeFloors = true;
                            }
                        }
                    }
                if ((!p.IsFloor && !p.IsRotating) || !p.IsCharacters)
                {
                    noLedge = true;
                }

                if (!noLedge)
                {
                    allNoLedge = false;
                    p.IsRightLedge = selection;
                }
                else
                {
                    p.IsRightLedge = false;
                    continue;
                }

                if (p.IsRightLedge)
                {
                    p.IsLeftLedge = false;
                }
                if (allSameType)
                {
                    if (p.IsRotating != firstIsRotating)
                    {
                        allSameType = false;
                    }
                    if (p.IsWall && (firstType == CollisionPlaneType.RightWall || firstType == CollisionPlaneType.LeftWall))
                    {
                        // This is fine as far as types are concerned
                    }
                    else if (p.Type != firstType)
                    {
                        allSameType = false;
                    }
                }
                if (allNonCharacters)
                {
                    allNonCharacters = !p.IsCharacters;
                }
            }
            if (allNonCharacters)
            {
                chkRightLedge.Checked = false;
            }
            else if (allNoLedge)
            {
                if (chkRightLedge.Checked == true)
                    chkRightLedge.Checked = false;
            }
            else if (anyNoLedgeFloors)
            {
                if (chkRightLedge.Checked != selection)
                    chkRightLedge.Checked = selection;
            }
            else if (!anyNoLedgeFloors)
            {
                chkLeftLedge.Checked = false;
                if (chkRightLedge.Checked != selection)
                    chkRightLedge.Checked = selection;
            }
            if ((_selectedPlanes.Count == 1 || allSameType) && _selectedPlanes.Count > 0 && !anyNoLedgeFloors && !allNonCharacters)
            {
                chkRightLedge.Checked = _selectedPlanes[0].IsRightLedge;
                if (_selectedPlanes[0].IsRightLedge)
                {
                    chkLeftLedge.Checked = false;
                }
                if (!_selectedPlanes[0].IsFloor && !_selectedPlanes[0].IsRotating)
                {
                    chkLeftLedge.Checked = false;
                    chkRightLedge.Checked = false;
                }
            }
            _modelPanel.Invalidate();
        }

        protected void chkNoWalljump_CheckedChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;
            if (!_errorChecking)
            {
                chkNoWalljump_CheckedChanged_NoErrorHandling(sender, e);
                return;
            }
            TargetNode.SignalPropertyChange();
            bool selection = chkNoWalljump.Checked;
            CollisionPlaneType firstType = _selectedPlanes[0].Type;
            bool firstIsRotating = _selectedPlanes[0].IsRotating;
            bool allSameType = true;
            bool allNonCharacters = !_selectedPlanes[0].IsCharacters;
            foreach (CollisionPlane p in _selectedPlanes)
            {
                p.IsNoWalljump = selection;
                if ((!p.IsWall && !p.IsRotating) || !p.IsCharacters)
                {
                    p.IsNoWalljump = false;
                }

                if (allSameType)
                {
                    if (p.IsRotating != firstIsRotating)
                    {
                        allSameType = false;
                    }
                    if (p.IsWall && (firstType == CollisionPlaneType.LeftWall || firstType == CollisionPlaneType.RightWall))
                    {
                        // This is fine as far as types are concerned
                    }
                    else if (p.Type != firstType)
                    {
                        allSameType = false;
                    }
                }
                if (allNonCharacters)
                {
                    allNonCharacters = !p.IsCharacters;
                }
            }

            if ((_selectedPlanes.Count == 1 || allSameType) && _selectedPlanes.Count > 0)
            {
                chkNoWalljump.Checked = _selectedPlanes[0].IsNoWalljump;
                if (!_selectedPlanes[0].IsWall && !_selectedPlanes[0].IsRotating)
                {
                    chkNoWalljump.Checked = false;
                }
            }
            if (allNonCharacters)
            {
                chkNoWalljump.Checked = false;
            }
        }

        protected void chkTypeCharacters_CheckedChanged_NoErrorHandling(object sender, EventArgs e) { if (_updating) return; TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes) p.IsCharacters = chkTypeCharacters.Checked; _modelPanel.Invalidate(); }
        protected void chkTypeItems_CheckedChanged_NoErrorHandling(object sender, EventArgs e) { if (_updating) return; TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes) p.IsItems = chkTypeItems.Checked; }
        protected void chkTypePokemonTrainer_CheckedChanged_NoErrorHandling(object sender, EventArgs e) { if (_updating) return; TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes) p.IsPokemonTrainer = chkTypePokemonTrainer.Checked; }
        protected void chkTypeRotating_CheckedChanged_NoErrorHandling(object sender, EventArgs e) { if (_updating) return; TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes) p.IsRotating = chkTypeRotating.Checked; }
        
        protected void chkFallThrough_CheckedChanged_NoErrorHandling(object sender, EventArgs e) { if (_updating) return; TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes) p.IsFallThrough = chkFallThrough.Checked; }
        protected void chkLeftLedge_CheckedChanged_NoErrorHandling(object sender, EventArgs e) { if (_updating) return; TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes) p.IsLeftLedge = chkLeftLedge.Checked; _modelPanel.Invalidate(); }
        protected void chkRightLedge_CheckedChanged_NoErrorHandling(object sender, EventArgs e) { if (_updating) return; TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes) p.IsRightLedge = chkRightLedge.Checked; _modelPanel.Invalidate(); }
        protected void chkNoWalljump_CheckedChanged_NoErrorHandling(object sender, EventArgs e) { if (_updating) return; TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes) p.IsNoWalljump = chkNoWalljump.Checked; }
        
        protected void chkFlagUnknown1_CheckedChanged(object sender, EventArgs e) { if (_updating) return; TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes) p.IsUnknownStageBox = chkFlagUnknown1.Checked; }
        protected void chkFlagUnknown2_CheckedChanged(object sender, EventArgs e) { if (_updating) return; TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes) p.IsUnknownFlag1 = chkFlagUnknown2.Checked; }
        protected void chkFlagUnknown3_CheckedChanged(object sender, EventArgs e) { if (_updating) return; TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes) p.IsUnknownFlag3 = chkFlagUnknown3.Checked; }
        protected void chkFlagUnknown4_CheckedChanged(object sender, EventArgs e) { if (_updating) return; TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes) p.IsUnknownFlag4 = chkFlagUnknown4.Checked; }

        #endregion

        #region Point Properties

        protected void numX_ValueChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;
            if (numX.Text == "" && _errorChecking)
                return;
            foreach (CollisionLink link in _selectedLinks)
            {
                if (link._parent == null || link._parent.LinkedBone == null)
                {
                    link._rawValue._x = numX.Value;
                } else
                {
                    Vector2 oldValue = link.Value;
                    link.Value = new Vector2(numX.Value, oldValue._y);
                }
            }
            _modelPanel.Invalidate();
            TargetNode.SignalPropertyChange();
        }

        protected void numY_ValueChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;
            if (numY.Text == "" && _errorChecking)
                return;
            foreach (CollisionLink link in _selectedLinks)
            {
                if (link._parent == null || link._parent.LinkedBone == null)
                {
                    link._rawValue._y = numY.Value;
                } else
                {
                    Vector2 oldValue = link.Value;
                    link.Value = new Vector2(oldValue._x, numY.Value);
                }
            }
            _modelPanel.Invalidate();
            TargetNode.SignalPropertyChange();
        }

        #endregion

        protected void transformToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TransformEditor transform = new TransformEditor();
            if (transform.ShowDialog() == DialogResult.OK)
            {
                CreateUndo();

                if (_selectedPlanes.Count > 0)
                {
                    FrameState _centerState = new FrameState(new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector3(0, 0, 0));
                    /*if(transform._transform.ScalingType == xyTransform.ScaleType.FromCenterOfCollisions)
                    {
                        Vector2 v2avg = new Vector2(0, 0);
                        int i = 0;
                        foreach(CollisionLink l in _selectedLinks)
                        {
                            v2avg += l._rawValue;
                            i++;
                        }
                        float newX = (v2avg._x / i) * -1;// * (v2avg._x >= 1 ? -1 : 1);
                        float newY = (v2avg._y / i) * -1;// * (v2avg._y >= 1 ? -1 : 1);
                        Console.WriteLine(new Vector2(newX, newY));
                        _centerState = new FrameState(new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector3(newX, newY, 0));
                    }*/
                    Vector3 v3trans = new Vector3(transform._transform.Translation._x, transform._transform.Translation._y, 0);
                    Vector3 v3rot = new Vector3(0, 0, transform._transform.Rotation);
                    Vector3 v3scale = new Vector3(transform._transform.Scale._x, transform._transform.Scale._y, 1);
                    FrameState _frameState = new FrameState(v3scale, v3rot, v3trans);
                    foreach (CollisionLink l in _selectedLinks)
                    {
                        l._rawValue = (_centerState._transform * _frameState._transform) * l._rawValue;
                    }
                } else
                {
                    foreach(CollisionLink l in _selectedLinks)
                    {
                        l._rawValue += transform._transform.Translation;
                    }
                }
                _modelPanel.Invalidate();
            }
        }

        protected void btnSameX_Click(object sender, EventArgs e)
        {
            CreateUndo();

            for (int i = 1; i < _selectedLinks.Count; i++)
                _selectedLinks[i]._rawValue._x = _selectedLinks[0]._rawValue._x;
            _modelPanel.Invalidate();
            TargetNode.SignalPropertyChange();
        }

        protected void btnSameY_Click(object sender, EventArgs e)
        {
            CreateUndo();

            for (int i = 1; i < _selectedLinks.Count; i++)
                _selectedLinks[i]._rawValue._y = _selectedLinks[0]._rawValue._y;
            _modelPanel.Invalidate();
            TargetNode.SignalPropertyChange();
        }

        protected void chkPoly_CheckStateChanged(object sender, EventArgs e)
        {
            _modelPanel.BeginUpdate();
            _modelPanel.RenderPolygons = chkPoly.CheckState == CheckState.Checked;
            _modelPanel.RenderWireframe = chkPoly.CheckState == CheckState.Indeterminate;
            _modelPanel.EndUpdate();
        }

        protected void chkBones_CheckedChanged(object sender, EventArgs e)
        {
            _modelPanel.RenderBones = chkBones.Checked;
        }

        protected void modelTree_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is MDL0Node)
            {
                ((MDL0Node)e.Node.Tag).IsRendering = e.Node.Checked;
                if (!_updating)
                {
                    _updating = true;
                    foreach (TreeNode n in e.Node.Nodes)
                        n.Checked = e.Node.Checked;
                    _updating = false;
                }
            }
            else if (e.Node.Tag is MDL0BoneNode)
                ((MDL0BoneNode)e.Node.Tag)._render = e.Node.Checked;

            if (!_updating)
                _modelPanel.Invalidate();
        }

        protected void modelTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                if (e.Node.Tag is MDL0BoneNode)
                {
                    MDL0BoneNode bone = e.Node.Tag as MDL0BoneNode;
                    bone._boneColor = Color.FromArgb(255, 0, 0);
                    bone._nodeColor = Color.FromArgb(255, 128, 0);
                    _modelPanel.Invalidate();
                }
            }
        }

        protected void modelTree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            TreeNode node = modelTree.SelectedNode;
            if (node != null)
            {
                if (node.Tag is MDL0BoneNode)
                {
                    MDL0BoneNode bone = node.Tag as MDL0BoneNode;
                    bone._nodeColor = bone._boneColor = Color.Transparent;
                    _modelPanel.Invalidate();
                }
            }
        }

        protected void btnRelink_Click(object sender, EventArgs e)
        {
            TreeNode node = modelTree.SelectedNode;
            if ((_selectedObject == null) || (node == null) || !(node.Tag is MDL0BoneNode))
                return;

            txtBone.Text = _selectedObject._boneName = node.Text;
            _selectedObject.LinkedBone = ((MDL0BoneNode)node.Tag);
            txtModel.Text = _selectedObject._modelName = node.Parent.Text;
            TargetNode.SignalPropertyChange();
            _modelPanel.Invalidate();
        }

        protected void btnRelinkNoMove_Click(object sender, EventArgs e)
        {
            TreeNode node = modelTree.SelectedNode;
            if ((_selectedObject == null) || (node == null) || !(node.Tag is MDL0BoneNode))
                return;

            txtBone.Text = _selectedObject._boneName = node.Text;
            _selectedObject.LinkedBone = ((MDL0BoneNode)node.Tag);
            txtModel.Text = _selectedObject._modelName = node.Parent.Text;
            if(_selectedObject._points != null)
                foreach(CollisionLink l in _selectedObject._points)
                    l.Value = l._rawValue;
            TargetNode.SignalPropertyChange();
            _modelPanel.Invalidate();
        }

        protected void btnUnlink_Click(object sender, EventArgs e)
        {
            if (_selectedObject == null)
                return;
            txtBone.Text = "";
            txtModel.Text = "";
            _selectedObject.LinkedBone = null;
            TargetNode.SignalPropertyChange();
            _modelPanel.Invalidate();
        }

        protected void btnUnlinkNoMove_Click(object sender, EventArgs e)
        {
            if (_selectedObject == null)
                return;
            if (_selectedObject._points != null)
                foreach (CollisionLink l in _selectedObject._points)
                    l._rawValue = l.Value;
            txtBone.Text = "";
            txtModel.Text = "";
            _selectedObject.LinkedBone = null;
            TargetNode.SignalPropertyChange();
            _modelPanel.Invalidate();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedPlanes.Count > 0)
            {
                foreach (CollisionPlane plane in _selectedPlanes)
                    plane.Delete();
                TargetNode.SignalPropertyChange();
            }
            else if (_selectedLinks.Count > 0)
            {
                for (int i = 0; i < _selectedLinks.Count; i++)
                    _selectedLinks[i].Pop();
                TargetNode.SignalPropertyChange();
            }

            ClearSelection();
            SelectionModified();
            _modelPanel.Invalidate();
        }

        protected void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {
            if ((modelTree.SelectedNode == null) || !(modelTree.SelectedNode.Tag is MDL0BoneNode))
                e.Cancel = true;
        }

        protected void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (_selectedObject == null)
                contextMenuStrip1.Items[1].Visible = contextMenuStrip1.Items[2].Visible = contextMenuStrip1.Items[3].Visible = contextMenuStrip1.Items[4].Visible = contextMenuStrip1.Items[5].Visible = contextMenuStrip1.Items[6].Visible = contextMenuStrip1.Items[7].Visible = false;
            else
                contextMenuStrip1.Items[1].Visible = contextMenuStrip1.Items[2].Visible = contextMenuStrip1.Items[3].Visible = contextMenuStrip1.Items[4].Visible = contextMenuStrip1.Items[5].Visible = contextMenuStrip1.Items[6].Visible = contextMenuStrip1.Items[7].Visible = true;
        }

        protected void contextMenuStrip3_Opening(object sender, CancelEventArgs e)
        {
            mergeToolStripMenuItem.Visible = alignXToolStripMenuItem.Visible = alignYToolStripMenuItem.Visible = (_selectedLinks != null && _selectedLinks.Count > 1);
            moveToNewObjectToolStripMenuItem.Visible = flipToolStripMenuItem.Visible = (_selectedPlanes != null && _selectedPlanes.Count > 0);
            moveToNewObjectToolStripMenuItem.Visible = false;
            //contextMenuStrip3.Items[0].Visible = contextMenuStrip3.Items[1].Visible = contextMenuStrip3.Items[2].Visible = contextMenuStrip3.Items[3].Visible = contextMenuStrip3.Items[4].Visible = contextMenuStrip3.Items[6].Visible = contextMenuStrip3.Items[7].Visible = (_selectedPlanes != null && _selectedPlanes.Count > 0);
        }

        protected void snapToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TreeNode node = modelTree.SelectedNode;
            if ((node == null) || !(node.Tag is MDL0BoneNode))
                return;

            _snapMatrix = ((MDL0BoneNode)node.Tag)._inverseBindMatrix;
            _modelPanel.Invalidate();
        }

        protected void btnResetSnap_Click(object sender, EventArgs e)
        {
            _snapMatrix = Matrix.Identity;
            _modelPanel.Invalidate();
        }
        
        protected void btnFlipColl_Click(object sender, EventArgs e)
        {
            foreach(CollisionPlane p in _selectedPlanes)
            {
                p.SwapLinks();
            }
            TargetNode.SignalPropertyChange();
            _modelPanel.Invalidate();
        }

        protected void CreateUndo()
        {
            CheckSaveIndex();
            if (undoSaves.Count > saveIndex)
            {
                undoSaves.RemoveRange(saveIndex, undoSaves.Count - saveIndex);
                redoSaves.Clear();
            }

            save = new CollisionState();
            save._collisionLinks = new List<CollisionLink>();
            save._linkVectors = new List<Vector2>();

            foreach (CollisionLink l in _selectedLinks)
            { save._collisionLinks.Add(l); save._linkVectors.Add(l.Value); }

            undoSaves.Add(save);
            btnUndo.Enabled = true;
            saveIndex++;
            save = null; 
        }

        protected void CheckSaveIndex()
        {
            if (saveIndex < 0)
            { saveIndex = 0; }

            if (undoSaves.Count > 25)
            { undoSaves.RemoveAt(0); saveIndex--; }
        }

        protected void ClearUndoBuffer()
        {
            saveIndex = 0;
            undoSaves.Clear();
            redoSaves.Clear();
            btnUndo.Enabled = btnRedo.Enabled = false;
        }

        protected void Undo(object sender, EventArgs e)
        {
            _selectedLinks.Clear();

            save = new CollisionState();

            if (undoSaves[saveIndex - 1]._linkVectors != null)     //XY Positions changed.
            {
                save._collisionLinks = new List<CollisionLink>();
                save._linkVectors = new List<Vector2>();

                for (int i = 0; i < undoSaves[saveIndex - 1]._collisionLinks.Count; i++)
                {
                    _selectedLinks.Add(undoSaves[saveIndex - 1]._collisionLinks[i]);
                    save._collisionLinks.Add(undoSaves[saveIndex - 1]._collisionLinks[i]);
                    save._linkVectors.Add(undoSaves[saveIndex - 1]._collisionLinks[i].Value);
                    _selectedLinks[i].Value = undoSaves[saveIndex - 1]._linkVectors[i];
                }
            }

            saveIndex--;
            CheckSaveIndex();

            if (saveIndex == 0)
            { btnUndo.Enabled = false; }
            btnRedo.Enabled = true;

            redoSaves.Add(save);
            save = null;
            
            _modelPanel.Invalidate();
            UpdatePropPanels();
        }

        protected void Redo(object sender, EventArgs e)
        {
            _selectedLinks.Clear();

            for (int i = 0; i < redoSaves[undoSaves.Count - saveIndex - 1]._collisionLinks.Count; i++)
            {
                _selectedLinks.Add(redoSaves[undoSaves.Count - saveIndex - 1]._collisionLinks[i]);
                _selectedLinks[i].Value = redoSaves[undoSaves.Count - saveIndex - 1]._linkVectors[i];
            }

            redoSaves.RemoveAt(undoSaves.Count - saveIndex - 1);
            saveIndex++;

            if (redoSaves.Count == 0)
            { btnRedo.Enabled = false; }
            btnUndo.Enabled = true;

            _modelPanel.Invalidate();
            UpdatePropPanels();
        }

        protected void chkObjUnk_CheckedChanged(object sender, EventArgs e)
        {
            if (_selectedObject == null || _updating) return;
            _selectedObject._flags[0] = chkObjUnk.Checked;
            TargetNode.SignalPropertyChange();
        }

        protected void chkObjIndep_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void chkObjModule_CheckedChanged(object sender, EventArgs e)
        {
            if (_selectedObject == null || _updating) return;
            _selectedObject._flags[2] = chkObjModule.Checked;
            TargetNode.SignalPropertyChange();
        }

        protected void chkObjSSEUnk_CheckedChanged(object sender, EventArgs e)
        {
            if (_selectedObject == null || _updating) return;
            _selectedObject._flags[3] = chkObjSSEUnk.Checked;
            TargetNode.SignalPropertyChange();
        }

        protected void btnPlayAnims_Click(object sender, EventArgs e)
        {

        }

        protected void btnPrevFrame_Click(object sender, EventArgs e)
        {

        }

        protected void btnNextFrame_Click(object sender, EventArgs e)
        {

        }

        protected void btnHelp_Click(object sender, EventArgs e)
        {
            new ModelViewerHelp().Show(this, true);
        }

        protected void btnTranslateAll_Click(object sender, EventArgs e) {
            if (_selectedLinks.Count == 0) {
                MessageBox.Show("You must select at least one collision link.");
                return;
            }
            using (TransformAttributesForm f = new TransformAttributesForm()) {
                f.TwoDimensional = true;
                if (f.ShowDialog() == DialogResult.OK) {
                    Matrix m = f.GetMatrix();
                    foreach (var link in _selectedLinks) {
                        link.Value = m * link.Value;
                    }
                    TargetNode.SignalPropertyChange();
                    _modelPanel.Invalidate();
                }
            }
        }
    }
}
