namespace System.Windows.Forms
{
    public partial class PAT0OffsetControl : Form
    {
        public string title = "PAT0 Offset";

        public PAT0OffsetControl()
        {
            InitializeComponent();
        }

        public int NewValue => (int)numNewCount.Value;
        public bool IncreaseFrames => chkIncreaseFrames.Checked;
        public bool OffsetOtherTextures => chkOffsetOtherTextures.Checked;

        public new DialogResult ShowDialog()
        {
            Text = title;
            return base.ShowDialog();
        }

        public DialogResult ShowDialog(string newTitle)
        {
            Text = newTitle;
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
