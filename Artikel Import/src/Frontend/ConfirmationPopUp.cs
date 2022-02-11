using log4net;
using System;

namespace Artikel_Import.src.Frontend
{
    /// <summary>
    /// This Form is being displayed before executing actions with big impact
    /// </summary>
    public partial class ConfirmationPopUp : System.Windows.Forms.Form
    {
        /// <summary>
        /// Does the user want to continue the action
        /// </summary>
        public bool confirmed = false;

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// This Form is being displayed before executing actions with big impact
        /// </summary>
        public ConfirmationPopUp(string message)
        {
            InitializeComponent();
            label1.Text = message;
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            log.Info("ConfirmationPopUp.ButtonCancel_Click");
            confirmed = false;
            Close();
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            log.Info("ConfirmationPopUp.ButtonOk_Click");
            confirmed = true;
            Close();
        }
    }
}