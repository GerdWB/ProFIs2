namespace ProFiS2.WordAddIn.Events
{
    using System;
    using Microsoft.Office.Interop.Word;

    internal class WordEventProxy : IWordEventProxy
    {
        private Application _application;

        public WordEventProxy(Application application)
        {
            _application = application ?? throw new ArgumentNullException(nameof(application));

            ((ApplicationEvents4_Event)_application).Quit += AppOnQuit;
            _application.DocumentBeforeSave += OnSave;
        }


        private void AppOnQuit()
        {
            UnsubscribeAllEvents();
        }

        private void OnSave(Document document, ref bool ui, ref bool cancel)
        {
            if (!Globals.IsProFis2Document(document))
            {
                return;
            }

            var a = Globals.GetProfiS2Data(document);
            ui = false;
            cancel = true;
        }

        private void UnsubscribeAllEvents()
        {
            ((ApplicationEvents4_Event)_application).Quit -= AppOnQuit;
            _application.DocumentBeforeSave -= OnSave;

            _application = null;
        }
    }
}