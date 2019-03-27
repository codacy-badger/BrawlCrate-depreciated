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
    public unsafe class AdvancedCollisionEditor : CollisionEditor
    {
        #region designer
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        // Advanced collision type selectors
        private GroupBox groupBoxType;
        private CheckBox chkTypeFloor;
        private CheckBox chkTypeCeiling;
        private CheckBox chkTypeLeftWall;
        private CheckBox chkTypeRightWall;

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            // Advanced floor editor
            this.groupBoxType = new System.Windows.Forms.GroupBox();
            this.chkTypeFloor = new System.Windows.Forms.CheckBox();
            this.chkTypeCeiling = new System.Windows.Forms.CheckBox();
            this.chkTypeLeftWall = new System.Windows.Forms.CheckBox();
            this.chkTypeRightWall = new System.Windows.Forms.CheckBox();
            
            this.pnlPlaneProps.SuspendLayout();
            this.groupBoxType.SuspendLayout();

            // 
            // panel3
            // 
            this.panel3.Controls.Clear();
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
            this.pnlPlaneProps.Controls.Clear();
            this.pnlPlaneProps.Controls.Add(this.groupBoxFlags2);
            this.pnlPlaneProps.Controls.Add(this.groupBoxFlags1);
            this.pnlPlaneProps.Controls.Add(this.groupBoxTargets);
            this.pnlPlaneProps.Controls.Add(this.cboMaterial);
            this.pnlPlaneProps.Controls.Add(this.label5);
            this.pnlPlaneProps.Controls.Add(this.groupBoxType);
            this.pnlPlaneProps.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPlaneProps.Location = new System.Drawing.Point(0, -199);
            this.pnlPlaneProps.Name = "pnlPlaneProps";
            this.pnlPlaneProps.Size = new System.Drawing.Size(209, 514);
            this.pnlPlaneProps.TabIndex = 0;
            this.pnlPlaneProps.Visible = false;
            // 
            // groupBoxType
            // 
            this.groupBoxType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxType.Controls.Add(this.chkTypeFloor);
            this.groupBoxType.Controls.Add(this.chkTypeCeiling);
            this.groupBoxType.Controls.Add(this.chkTypeLeftWall);
            this.groupBoxType.Controls.Add(this.chkTypeRightWall);
            this.groupBoxType.Location = new System.Drawing.Point(0, 25);
            this.groupBoxType.Margin = new System.Windows.Forms.Padding(0);
            this.groupBoxType.Name = "groupBoxType";
            this.groupBoxType.Padding = new System.Windows.Forms.Padding(0);
            this.groupBoxType.Size = new System.Drawing.Size(208, 59);
            this.groupBoxType.TabIndex = 14;
            this.groupBoxType.TabStop = false;
            this.groupBoxType.Text = "Type";
            // 
            // chkTypeFloor
            // 
            this.chkTypeFloor.Location = new System.Drawing.Point(8, 17);
            this.chkTypeFloor.Margin = new System.Windows.Forms.Padding(0);
            this.chkTypeFloor.Name = "chkTypeFloor";
            this.chkTypeFloor.Size = new System.Drawing.Size(86, 18);
            this.chkTypeFloor.TabIndex = 3;
            this.chkTypeFloor.Text = "Floor";
            this.chkTypeFloor.UseVisualStyleBackColor = true;
            this.chkTypeFloor.CheckedChanged += new System.EventHandler(this.chkTypeFloor_CheckedChanged);
            // 
            // chkTypeCeiling
            // 
            this.chkTypeCeiling.Location = new System.Drawing.Point(112, 17);
            this.chkTypeCeiling.Margin = new System.Windows.Forms.Padding(0);
            this.chkTypeCeiling.Name = "chkTypeCeiling";
            this.chkTypeCeiling.Size = new System.Drawing.Size(86, 18);
            this.chkTypeCeiling.TabIndex = 3;
            this.chkTypeCeiling.Text = "Ceiling";
            this.chkTypeCeiling.UseVisualStyleBackColor = true;
            this.chkTypeCeiling.CheckedChanged += new System.EventHandler(this.chkTypeCeiling_CheckedChanged);
            // 
            // chkTypeLeftWall
            // 
            this.chkTypeLeftWall.Location = new System.Drawing.Point(8, 33);
            this.chkTypeLeftWall.Margin = new System.Windows.Forms.Padding(0);
            this.chkTypeLeftWall.Name = "chkTypeLeftWall";
            this.chkTypeLeftWall.Size = new System.Drawing.Size(86, 18);
            this.chkTypeLeftWall.TabIndex = 3;
            this.chkTypeLeftWall.Text = "Left Wall";
            this.chkTypeLeftWall.UseVisualStyleBackColor = true;
            this.chkTypeLeftWall.CheckedChanged += new System.EventHandler(this.chkTypeLeftWall_CheckedChanged);
            // 
            // chkTypeRightWall
            // 
            this.chkTypeRightWall.Location = new System.Drawing.Point(112, 33);
            this.chkTypeRightWall.Margin = new System.Windows.Forms.Padding(0);
            this.chkTypeRightWall.Name = "chkTypeRightWall";
            this.chkTypeRightWall.Size = new System.Drawing.Size(86, 18);
            this.chkTypeRightWall.TabIndex = 3;
            this.chkTypeRightWall.Text = "Right Wall";
            this.chkTypeRightWall.UseVisualStyleBackColor = true;
            this.chkTypeRightWall.CheckedChanged += new System.EventHandler(this.chkTypeRightWall_CheckedChanged);
            // 
            // groupBoxFlags1
            // 
            this.groupBoxFlags1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxFlags1.Location = new System.Drawing.Point(0, 128);
            this.groupBoxFlags1.Name = "groupBoxFlags1";
            // 
            // groupBoxFlags2
            // 
            this.groupBoxFlags2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxFlags2.Location = new System.Drawing.Point(104, 128);
            this.groupBoxFlags2.Name = "groupBoxFlags2";
            // 
            // cboMaterial
            // 
            this.cboMaterial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMaterial.Location = new System.Drawing.Point(59, 4);
            this.cboMaterial.Name = "cboMaterial";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(0, 4);
            this.label5.Name = "label5";
            //
            // groupBoxTargets
            // 
            this.groupBoxTargets.Location = new System.Drawing.Point(0, 76);
            this.groupBoxTargets.Name = "groupBoxTargets";

            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        }

        private void chkTypeFloor_CheckedChanged(object sender, EventArgs e) { if (_updating) return; TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes) p.IsFloor = chkTypeFloor.Checked; }
        private void chkTypeCeiling_CheckedChanged(object sender, EventArgs e) { if (_updating) return; TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes) p.IsCeiling = chkTypeCeiling.Checked; }
        private void chkTypeLeftWall_CheckedChanged(object sender, EventArgs e) { if (_updating) return; TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes) p.IsLeftWall = chkTypeLeftWall.Checked; }
        private void chkTypeRightWall_CheckedChanged(object sender, EventArgs e) { if (_updating) return; TargetNode.SignalPropertyChange(); foreach (CollisionPlane p in _selectedPlanes) p.IsRightWall = chkTypeRightWall.Checked; }

        #endregion
        #endregion

        public AdvancedCollisionEditor()
        {
            InitializeComponent();
            _errorChecking = false;
            cboMaterial.DataSource = CollisionTerrain.Terrains.ToList();
        }

        protected override void SelectionModified()
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
                panel3.Height = 230;
            }
            else if (_selectedLinks.Count == 1)
            {
                pnlPointProps.Visible = true;
                panel3.Height = 70;
            }

            UpdatePropPanels();
        }

        protected override void UpdatePropPanels()
        {
            _updating = true;

            if (pnlPlaneProps.Visible)
            {
                if (_selectedPlanes.Count <= 0)
                {
                    pnlPlaneProps.Visible = false;
                    return;
                }
                CollisionPlane p = _selectedPlanes[0];

                //Material
                cboMaterial.SelectedItem = cboMaterial.Items[p._material];
                //Type
                chkTypeFloor.Checked = p.IsFloor;
                chkTypeCeiling.Checked = p.IsCeiling;
                chkTypeLeftWall.Checked = p.IsLeftWall;
                chkTypeRightWall.Checked = p.IsRightWall;
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
                if (_selectedObject == null)
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
    }
}
