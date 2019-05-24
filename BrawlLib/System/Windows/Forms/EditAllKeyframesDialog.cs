﻿using BrawlLib.Wii.Animations;

namespace System.Windows.Forms
{
    public class EditAllKeyframesDialog : Form
    {
        private int _type;
        private IKeyframeSource _target;

        public EditAllKeyframesDialog() { InitializeComponent(); }

        public DialogResult ShowDialog(IWin32Window owner, int type, IKeyframeSource target)
        {
            _target = target;
            _type = type;
            comboBox1.Items.Add("Add");
            comboBox1.Items.Add("Subtract");
            comboBox1.SelectedIndex = 0;
            return base.ShowDialog(owner);
        }

        private unsafe void btnOkay_Click(object sender, EventArgs e)
        {
            KeyframeEntry kfe;
            if (amount.Text != null)
            {
                if (comboBox1.SelectedIndex == 0)
                {
                    for (int x = 0; x < _target.FrameCount; x++) //Loop thru each frame
                    {
                        if ((kfe = _target.KeyArrays[_type].GetKeyframe(x)) != null) //Check for a keyframe
                        {
                            kfe._value += Convert.ToSingle(amount.Text);
                        }
                    }
                }

                if (comboBox1.SelectedIndex == 1)
                {
                    for (int x = 0; x < _target.FrameCount; x++) //Loop thru each frame
                    {
                        if ((kfe = _target.KeyArrays[_type].GetKeyframe(x)) != null) //Check for a keyframe
                        {
                            kfe._value -= Convert.ToSingle(amount.Text);
                        }
                    }
                }
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) { DialogResult = DialogResult.Cancel; Close(); }

        #region Designer

        private TextBox amount;
        private Button btnCancel;
        private Label label1;
        private ComboBox comboBox1;
        private Button btnOkay;

        private void InitializeComponent()
        {
            amount = new System.Windows.Forms.TextBox();
            btnCancel = new System.Windows.Forms.Button();
            btnOkay = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            comboBox1 = new System.Windows.Forms.ComboBox();
            SuspendLayout();
            // 
            // amount
            // 
            amount.HideSelection = false;
            amount.Location = new System.Drawing.Point(116, 12);
            amount.Name = "amount";
            amount.Size = new System.Drawing.Size(107, 20);
            amount.TabIndex = 0;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(197, 38);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "&Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += new System.EventHandler(btnCancel_Click);
            // 
            // btnOkay
            // 
            btnOkay.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
            btnOkay.Location = new System.Drawing.Point(116, 38);
            btnOkay.Name = "btnOkay";
            btnOkay.Size = new System.Drawing.Size(75, 23);
            btnOkay.TabIndex = 1;
            btnOkay.Text = "&Okay";
            btnOkay.UseVisualStyleBackColor = true;
            btnOkay.Click += new System.EventHandler(btnOkay_Click);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(229, 15);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(43, 13);
            label1.TabIndex = 3;
            label1.Text = "from all.";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new System.Drawing.Point(12, 11);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new System.Drawing.Size(98, 21);
            comboBox1.TabIndex = 4;
            comboBox1.SelectedIndexChanged += new System.EventHandler(comboBox1_SelectedIndexChanged);
            // 
            // EditDialog
            // 
            AcceptButton = btnOkay;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(284, 69);
            Controls.Add(comboBox1);
            Controls.Add(label1);
            Controls.Add(btnOkay);
            Controls.Add(btnCancel);
            Controls.Add(amount);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Name = "EditDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Edit All Keyframes";
            ResumeLayout(false);
            PerformLayout();

        }
        #endregion

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                label1.Text = "to all.";
            }

            if (comboBox1.SelectedIndex == 1)
            {
                label1.Text = "from all.";
            }
        }
    }
}
