﻿using BrawlLib.SSBB.ResourceNodes;

namespace System.Windows.Forms
{
    public class RenameDialog : Form
    {
        private ResourceNode _node;
        private string name;
        public string NewName => name;

        public RenameDialog()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(IWin32Window owner, ResourceNode node)
        {
            _node = node;
            Text = "Rename Node";

            if (_node is ARCNode)
            {
                txtName.MaxLength = 47;
            }
            else
            {
                txtName.MaxLength = 255;
            }

            name = node.Name;
            txtName.Text = node.Name;

            try
            {
                return base.ShowDialog(owner);
            }
            finally
            {
                _node = null;
            }
        }

        public DialogResult ShowDialog(IWin32Window owner, string title, string defaultText)
        {
            _node = null;
            Text = title;

            txtName.MaxLength = 255;

            name = defaultText;
            txtName.Text = defaultText;

            try
            {
                return base.ShowDialog(owner);
            }
            finally
            {
                _node = null;
            }
        }

        private unsafe void btnOkay_Click(object sender, EventArgs e)
        {
            name = txtName.Text;

            if (_node == null)
            {
                DialogResult = DialogResult.OK;
                Close();
                return;
            }

            if (name.Length == 0)
            {
                name = "<null>";
            }

            if (name.Equals("<null>", StringComparison.OrdinalIgnoreCase))
            {
                if (!_node.AllowNullNames)
                {
                    MessageBox.Show(this, "Name cannot be null!", "What the...");
                    return;
                }
            }
            else if (!_node.AllowDuplicateNames && _node.Parent != null)
            {
                //No duplicates
                foreach (ResourceNode c in _node.Parent.Children)
                {
                    if (c.Name == name && c.GetType() == _node.GetType() && c != _node)
                    {
                        MessageBox.Show(this, "A resource with that name already exists!", "What the...");
                        return;
                    }
                }
            }

            //Also change palette node
            if (_node is TEX0Node)
            {
                PLT0Node plt = ((TEX0Node) _node).GetPaletteNode();
                if (plt != null)
                {
                    plt.Name = name;
                }
            }

            _node.Name = name;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }


        #region Designer

        private TextBox txtName;
        private Button btnCancel;
        private Button btnOkay;

        private void InitializeComponent()
        {
            txtName = new TextBox();
            btnCancel = new Button();
            btnOkay = new Button();
            SuspendLayout();
            // 
            // txtName
            // 
            txtName.HideSelection = false;
            txtName.Location = new System.Drawing.Point(12, 12);
            txtName.Name = "txtName";
            txtName.Size = new System.Drawing.Size(260, 20);
            txtName.TabIndex = 0;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(197, 38);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "&Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += new EventHandler(btnCancel_Click);
            // 
            // btnOkay
            // 
            btnOkay.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnOkay.Location = new System.Drawing.Point(116, 38);
            btnOkay.Name = "btnOkay";
            btnOkay.Size = new System.Drawing.Size(75, 23);
            btnOkay.TabIndex = 1;
            btnOkay.Text = "&Okay";
            btnOkay.UseVisualStyleBackColor = true;
            btnOkay.Click += new EventHandler(btnOkay_Click);
            // 
            // RenameDialog
            // 
            AcceptButton = btnOkay;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(284, 69);
            Controls.Add(btnOkay);
            Controls.Add(btnCancel);
            Controls.Add(txtName);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "RenameDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Rename Node";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}