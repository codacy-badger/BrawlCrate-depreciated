namespace System.Windows.Forms
{
    public partial class PAT0OffsetControl : Form
    {
        public string title = "PAT0 Offset";

        public PAT0OffsetControl()
        {
            InitializeComponent();
        }

        public int NewValue { get { return (int)numNewCount.Value; } }
        public bool IncreaseFrames { get { return chkIncreaseFrames.Checked; } }
        public bool OffsetOtherTextures { get { return chkOffsetOtherTextures.Checked; } }

        public new DialogResult ShowDialog()
        {
            this.Text = title;
            return base.ShowDialog();
        }

        public DialogResult ShowDialog(string newTitle)
        {
            this.Text = newTitle;
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

        private void chkOffsetOtherTextures_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkIncreaseFrames_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
