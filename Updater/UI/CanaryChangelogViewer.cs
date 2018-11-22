using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Windows.Forms
{
    public partial class CanaryChangelogViewer : Form
    {
        public bool Finished = false;
        public CanaryChangelogViewer(string commitID, string changelog)
        {
            Finished = false;
            InitializeComponent();
            Text = "BrawlCrate Canary Changelog #" + commitID;
            richTextBox1.Text = changelog;
            richTextBox1.Enabled = false;
        }

        private void CanaryChangelogViewer_Load(object sender, EventArgs e)
        {

        }

        protected override void OnClosed(EventArgs e)
        {
            Finished = true;
            base.OnClosed(e);
        }
    }
}
