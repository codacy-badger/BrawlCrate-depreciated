namespace System.Windows.Forms
{
    public class TwoInputStringDialog : Form
    {
        private string txt1;
        private string txt2;
        public string InputText1 => txt1;
        public string InputText2 => txt2;

        public TwoInputStringDialog() { InitializeComponent(); }

        public DialogResult ShowDialog(IWin32Window owner, string title, string label_1, string default_1, string label_2, string default_2)
        {
            Text = title;

            label1.Text = label_1;
            textBox1.MaxLength = 255;
            textBox1.Text = txt1 = default_1;
            label2.Text = label_2;
            textBox2.MaxLength = 255;
            textBox2.Text = txt2 = default_2;


            return base.ShowDialog(owner);
        }
        private unsafe void btnOkay_Click(object sender, EventArgs e)
        {
            txt1 = textBox1.Text;
            txt2 = textBox2.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) { DialogResult = DialogResult.Cancel; Close(); }


        #region Designer

        private TextBox textBox1;
        private TextBox textBox2;
        private Label label1;
        private Label label2;
        private Button btnCancel;
        private Button btnOkay;

        private void InitializeComponent()
        {
            textBox1 = new System.Windows.Forms.TextBox();
            btnCancel = new System.Windows.Forms.Button();
            btnOkay = new System.Windows.Forms.Button();
            textBox2 = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.HideSelection = false;
            textBox1.Location = new System.Drawing.Point(62, 12);
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(210, 20);
            textBox1.TabIndex = 0;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(197, 66);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "&Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += new System.EventHandler(btnCancel_Click);
            // 
            // btnOkay
            // 
            btnOkay.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
            btnOkay.Location = new System.Drawing.Point(116, 66);
            btnOkay.Name = "btnOkay";
            btnOkay.Size = new System.Drawing.Size(75, 23);
            btnOkay.TabIndex = 2;
            btnOkay.Text = "&Okay";
            btnOkay.UseVisualStyleBackColor = true;
            btnOkay.Click += new System.EventHandler(btnOkay_Click);
            // 
            // textBox2
            // 
            textBox2.HideSelection = false;
            textBox2.Location = new System.Drawing.Point(62, 38);
            textBox2.Name = "textBox2";
            textBox2.Size = new System.Drawing.Size(210, 20);
            textBox2.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(20, 15);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(36, 13);
            label1.TabIndex = 4;
            label1.Text = "Repo:";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 41);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(44, 13);
            label2.TabIndex = 5;
            label2.Text = "Branch:";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TwoInputStringDialog
            // 
            AcceptButton = btnOkay;
            CancelButton = btnCancel;
            ClientSize = new System.Drawing.Size(284, 97);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBox2);
            Controls.Add(btnOkay);
            Controls.Add(btnCancel);
            Controls.Add(textBox1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Icon = global::BrawlLib.Properties.Resources.BrawlCrateIcon;
            Name = "TwoInputStringDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Change Canary Branch";
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

    }
}
