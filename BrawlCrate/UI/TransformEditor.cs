using System;
using System.Collections.Generic;
using System.ComponentModel;
using BrawlLib.Wii.Models;
using BrawlLib.SSBBTypes;
using BrawlLib.Modeling;
using BrawlLib.OpenGL;
using System.Drawing;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using System.Linq;

namespace System.Windows.Forms
{
    public partial class TransformEditor : Form
    {
        public TransformEditor()
        {
            InitializeComponent();
        }
        
        public xyTransform _transform = new xyTransform();

        public DialogResult ShowDialog()
        {
            _transform = new xyTransform();
            propertyGrid1.SelectedObject = _transform;
            return base.ShowDialog();
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            DialogResult = Forms.DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = Forms.DialogResult.Cancel;
            Close();
        }

        private void propertyGrid1_Click(object sender, EventArgs e)
        {

        }
    }

    public class xyTransform
    {
        private Vector2 _trans = new Vector2(0, 0);
        [Category("Transform"), TypeConverter(typeof(Vector2StringConverter))]
        [DisplayName("\t\t\tTranslation")]
        public Vector2 Translation
        {
            get { return _trans; }
            set { _trans = value; }
        }

        private int _rot = 0;
        [Category("Transform")]
        [DisplayName("\t\tRotation")]
        public int Rotation
        {
            get { return _rot; }
            set { _rot = value; }
        }

        public enum ScaleType
        {
            FromOrigin,
            FromCenterOfCollisions
        }
        /*
        ScaleType _scaleType = ScaleType.FromOrigin;
        [Category("Transform"), Description("Changes the way in which values are scaled")]
        [DisplayName("\tScaling Type")]
        public ScaleType ScalingType { get { return _scaleType; } set { _scaleType = value; } }
        */
        private Vector2 _scale = new Vector2(1, 1);
        [Category("Transform"), TypeConverter(typeof(Vector2StringConverter))]
        [DisplayName("Scale")]
        public Vector2 Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }
    }
}
