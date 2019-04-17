namespace System.Windows.Forms
{
    public partial class NumericEntryBox : Form
    {
        public string title = "Numeric Entry Box";
        public string lowerText = "Error; No arguments given";

        public NumericEntryBox()
        {
            InitializeComponent();
        }

        public int NewValue { get { return (int)numNewCount.Value; } }

        public DialogResult ShowDialog()
        {
            this.Text = title;
            this.label2.Text = lowerText;
            return base.ShowDialog();
        }

        public DialogResult ShowDialog(string newTitle, string newLower)
        {
            this.Text = newTitle;
            this.label2.Text = newLower;
            return base.ShowDialog();
        }
        public DialogResult ShowDialog(string newTitle, string newLower, int val)
        {
            this.Text = newTitle;
            this.label2.Text = newLower;
            this.numNewCount.Value = val;
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
    }
}
