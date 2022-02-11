using log4net;
using System;
using System.Threading.Tasks;

namespace Artikel_Import.src.Frontend
{
    public partial class MessagePopUp : System.Windows.Forms.Form
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// This Form is being used to display a <paramref name="message"/> on top of the <see cref="MainForm"/>
        /// </summary>
        /// <param name="message">the string being displayed</param>
        public MessagePopUp(string message)
        {
            log.Info($"MessagePopUp.MessagePopUp message: '{message}'");
            InitializeComponent();
            labelMessage.Text = message;
        }

        /// <summary>
        /// This Form is being used to display a <paramref name="message"/> on top of the the <see
        /// cref="MainForm"/>. It automatically closes itself after <paramref name="dispalyTime"/> seconds.
        /// </summary>
        /// <param name="message">text that will be displayed</param>
        /// <param name="dispalyTime">amount of seconds the forms should be displayed</param>
        public MessagePopUp(string message, int dispalyTime)
        {
            log.Info($"MessagePopUp.MessagePopUp message: '{message}'");
            InitializeComponent();
            labelMessage.Text = message + "\n\n" + Properties.Resources.AutoCloseMessagePopUp + $"{dispalyTime}s";
            AutoClose(dispalyTime);
        }

        private async void AutoClose(int seconds)
        {
            if(seconds < 1)
            {
                await Task.Delay(1);
                Close();
                return;
            }
            await Task.Delay(seconds * 1000); //convert from milliseconds
            log.Info("MessagePopUp.AutoClose");
            Close();
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            log.Info("MessagePopUp.ButtonOK_Click");
            Close();
        }
    }
}