using System;
using System.Windows.Forms;

namespace BrawlCrate
{
    public partial class AboutForm : Form
    {
        private static AboutForm _instance;
        public static AboutForm Instance => _instance == null ? _instance = new AboutForm() : _instance;

        public AboutForm()
        {
            InitializeComponent();
            lblName.Text = "BrawlCrate" + (MainForm.Instance.Canary ? " Canary" : "");
            lblVersion.Text = MainForm.Instance.Canary ? MainForm.Instance.commitIDlong.Substring(1) : "Version " + Program.AssemblyVersion;
#if DEBUG
            this.lblVersion.Text += " DEBUG";
#endif
            txtDescription.Text = Program.AssemblyDescription;
            lblCopyright.Text = Program.AssemblyCopyright;
            try
            {
                lblBrawlLib.Text = "Using " + (MainForm.Instance.Canary ? ("BrawlCrateLib Canary" + MainForm.Instance.commitIDlong) : (Program.BrawlLibTitle.Substring(0, Program.BrawlLibTitle.LastIndexOf(' ')) + " v" + Program.BrawlLibVersion));
            }
            catch
            {
                lblBrawlLib.Text = "Using " + (MainForm.Instance.Canary ? ("BrawlCrateLib Canary" + MainForm.Instance.commitIDlong) : Program.BrawlLibTitle);
            }
            if (Program.IsBirthday)
            {
                txtDescription.Location = new System.Drawing.Point(12, 81);
                txtDescription.Size = new System.Drawing.Size(454, 218);

                lblSpecialMessage.Text = "Happy ";
                lblSpecialMessage.Text += (DateTime.Now.Year - 2018);
                if (lblSpecialMessage.Text.Substring(0, lblSpecialMessage.Text.Length - 1).EndsWith("1"))
                {
                    lblSpecialMessage.Text += "th";
                }
                else if (lblSpecialMessage.Text.EndsWith("1"))
                {
                    lblSpecialMessage.Text += "st";
                }
                else if (lblSpecialMessage.Text.EndsWith("2"))
                {
                    lblSpecialMessage.Text += "nd";
                }
                else if (lblSpecialMessage.Text.EndsWith("3"))
                {
                    lblSpecialMessage.Text += "rd";
                }
                else
                {
                    lblSpecialMessage.Text += "th";
                }

                lblSpecialMessage.Text += " Birthday BrawlCrate!";
            }
            else if (DateTime.Now.Month == 7 && DateTime.Now.Day == 4)
            {
                txtDescription.Location = new System.Drawing.Point(12, 81);
                txtDescription.Size = new System.Drawing.Size(454, 218);

                lblSpecialMessage.Text = "Happy Birthday soopercool101!";
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
