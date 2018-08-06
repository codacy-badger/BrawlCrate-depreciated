namespace System.Windows.Forms
{
    public partial class StageBoxHexEntry : Form
    {
        public string title = "StageBox Hex Entry Box";
        public string lowerText = "Error; No arguments given";

        public StageBoxHexEntry()
        {
            InitializeComponent();
        }

        public int NewValue
        {
            get
            {
                if (numNewCount.Text.Length == 0)
                    return -1;
                if (!char.IsDigit(numNewCount.Text[0]))
                    return -1;
                int fromBase = numNewCount.Text.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase)
                    ? 16
                    : 10;
                return (int)(Convert.ToByte(numNewCount.Text, fromBase));
            }
        }

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
