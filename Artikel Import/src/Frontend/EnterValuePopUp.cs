using log4net;
using System;

namespace Artikel_Import.src.Frontend
{
    /// <summary>
    /// Form used to enter a value
    /// </summary>
    public partial class EnterValuePopUp : System.Windows.Forms.Form
    {
        /// <summary>
        /// the value the user choose
        /// </summary>
        public string value;

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// This Form is being displayed, so the user can enter a value
        /// </summary>
        public EnterValuePopUp(string message)
        {
            InitializeComponent();
            label1.Text = message;
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            log.Info($"EnterNamePopUp.ButtonCancel_Click");
            value = "";
            Close();
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            log.Info($"EnterNamePopUp.ButtonOk_Click text: '{textBox1.Text}'");
            value = textBox1.Text;
            Close();
        }
    }
}