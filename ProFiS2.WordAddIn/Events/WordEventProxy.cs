namespace ProFiS2.WordAddIn.Events
{
    using System;
    using Microsoft.Extensions.Logging;
    using Microsoft.Office.Interop.Word;
    using Services;

    internal class WordEventProxy : IWordEventProxy
    {
        private readonly ILogger _logger;
        private readonly IUploadService _uploadService;
        private Application _application;

        public WordEventProxy(
            Application application,
            IUploadService uploadService,
            ILogger<WordEventProxy> logger)
        {
            _application = application ?? throw new ArgumentNullException(nameof(application));
            _uploadService = uploadService;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            ((ApplicationEvents4_Event)_application).Quit += AppOnQuit;
            _application.DocumentBeforeSave += OnSave;
            _application.DocumentBeforeClose += BeforeClose;
        }


        private void AppOnQuit()
        {
            _logger.LogInformation("Application Quit");
            UnsubscribeAllEvents();
        }

        private void BeforeClose(Document document, ref bool cancel)
        {
            if (!Globals.IsProFis2Document(document))
            {
                _logger.LogInformation("BeforeClose: Not an Profis document");
            }

            _logger.LogInformation("BeforeClose: It is an Profis document");

            var profiS2WordData = Globals.GetProfiS2Data(document);
            _uploadService.Upload(profiS2WordData, null);
        }

        private void OnSave(Document document, ref bool ui, ref bool cancel)
        {
            if (!Globals.IsProFis2Document(document))
            {
                _logger.LogInformation("Application Save not an Profis document");
                return;
            }

            _logger.LogInformation("Application Save it is an Profis document");

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