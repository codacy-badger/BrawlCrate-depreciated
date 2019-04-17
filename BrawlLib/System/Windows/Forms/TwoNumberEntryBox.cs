namespace System.Windows.Forms
{
    public partial class TwoNumberEntryBox : Form
    {
        public string title = "Numeric Entry Box";
        public string lowerText = "Error; No arguments given";

        public TwoNumberEntryBox()
        {
            InitializeComponent();
        }

        public int Value1 { get { return (int)numericInputBox1.Value; } }
        public int Value2 { get { return (int)numericInputBox2.Value; } }

        public DialogResult ShowDialog()
        {
            this.Text = title;
            this.label1.Text = lowerText;
            this.label2.Text = lowerText;
            this.numericInputBox1.Value = 0;
            this.numericInputBox2.Value = 0;
            return base.ShowDialog();
        }

        public DialogResult ShowDialog(string newTitle, string lower1, string lower2)
        {
            this.Text = newTitle;
            this.label1.Text = lower1;
            this.label2.Text = lower2;
            this.numericInputBox1.Value = 0;
            this.numericInputBox2.Value = 0;
            return base.ShowDialog();
        }

        public DialogResult ShowDialog(string newTitle, string lower1, string lower2, int val1, int val2)
        {
            this.Text = newTitle;
            this.label1.Text = lower1;
            this.label2.Text = lower2;
            this.numericInputBox1.Value = val1;
            this.numericInputBox2.Value = val2;
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
