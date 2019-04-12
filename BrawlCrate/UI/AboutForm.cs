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
#if DEBUG
            this.lblVersion.Text += " DEBUG";
#endif
            this.txtDescription.Text = Program.AssemblyDescription;
            this.lblCopyright.Text = Program.AssemblyCopyright;
            this.lblBrawlLib.Text = "Using " + (MainForm.Instance.Canary ? ("BrawlCrateLib Canary" + MainForm.Instance.commitIDlong) : Program.BrawlLibTitle);
            if(Program.IsBirthday)
            {
                this.txtDescription.Location = new System.Drawing.Point(12, 81);
                this.txtDescription.Size = new System.Drawing.Size(454, 218);

                this.lblSpecialMessage.Text = "Happy ";
                this.lblSpecialMessage.Text += (DateTime.Now.Year - 2018);
                if (this.lblSpecialMessage.Text.Substring(0, this.lblSpecialMessage.Text.Length - 1).Equals("1"))
                    this.lblSpecialMessage.Text += "th";
                else if (this.lblSpecialMessage.Text.EndsWith("1"))
                    this.lblSpecialMessage.Text += "st";
                else if (this.lblSpecialMessage.Text.EndsWith("2"))
                    this.lblSpecialMessage.Text += "nd";
                else if (this.lblSpecialMessage.Text.EndsWith("3"))
                    this.lblSpecialMessage.Text += "rd";
                else
                    this.lblSpecialMessage.Text += "th";
                this.lblSpecialMessage.Text += " Birthday BrawlCrate!";
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
