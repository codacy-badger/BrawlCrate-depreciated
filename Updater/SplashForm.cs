using System.Windows.Forms;

namespace Net
{
    public partial class SplashForm : Form
    {
        public SplashForm()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            InitializeComponent();
        }

        private void SplashForm_Load(object sender, System.EventArgs e)
        {

        }
    }
}
