namespace ProFiS2.WordAddIn.Services
{
    using System.Windows.Forms;

    internal class MessageService : IMessageService
    {
        public void ShowCriticalMessage(string text)
        {
            MessageBox.Show(text, "AddIn", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ShowInfoMessage(string text)
        {
            MessageBox.Show(text, "AddIn", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}