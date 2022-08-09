using Office = Microsoft.Office.Core;

namespace ProFiS2.WordAddIn
{
    using System;
    using Events;

    public partial class ThisAddIn
    {
        private void ConfigureServices()
        {
            var wordEventProxy = new WordEventProxy(Application);
        }

        #region VSTO generated code

        /// <summary>
        ///     Required method for Designer support - do not modify
        ///     the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            Startup += ThisAddIn_Startup;
            Shutdown += ThisAddIn_Shutdown;
        }

        #endregion

        private void ThisAddIn_Shutdown(object sender, EventArgs e)
        {
        }

        private void ThisAddIn_Startup(object sender, EventArgs e)
        {
            ConfigureServices();
        }
    }
}