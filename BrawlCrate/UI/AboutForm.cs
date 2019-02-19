using System;
using System.Windows.Forms;

namespace BrawlCrate
{
    public partial class AboutForm : Form
    {
        private static AboutForm _instance;
        public static AboutForm Instance { get { return _instance == null ? _instance = new AboutForm() : _instance; } }

        public AboutForm()
        {
            InitializeComponent();
            this.lblName.Text = "BrawlCrate" + (MainForm.Instance.Canary ? " Canary" : "");
            this.lblVersion.Text = MainForm.Instance.Canary ? MainForm.currentRepo + "@" + MainForm.currentBranch + MainForm.Instance.commitIDshort : "Version " + Program.AssemblyVersion;
            if (MainForm.Instance.Canary)
                this.lblVersion.Font = new System.Drawing.Font(this.lblVersion.Font.FontFamily.ToString(), 10.0f);
#if DEBUG
            this.lblVersion.Text += " DEBUG";
#endif
            this.txtDescription.Text = Program.AssemblyDescription;
            this.lblCopyright.Text = Program.AssemblyCopyright;
            this.lblBrawlLib.Text = "Using " + (MainForm.Instance.Canary ? ("BrawlCrateLib Canary" + MainForm.Instance.commitIDlong) : Program.BrawlLibTitle);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
