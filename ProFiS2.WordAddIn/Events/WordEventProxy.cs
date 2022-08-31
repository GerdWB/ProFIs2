namespace ProFiS2.WordAddIn.Events
{
    using System;
    using Helper;
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
            if (document == null)
            {
                return;
            }

            if (!Globals.IsProFis2Document(document))
            {
                _logger.LogInformation("BeforeClose: Not an Profis document");
                return;
            }

            _logger.LogInformation("BeforeClose: It is an Profis document");
            Upload(document);
        }

        private bool DocumentStillLoaded(Document document)
        {
            try
            {
                var x = document.FullName;
                return true;
            }
            catch
            {
                return false;
            }
        }


        private void OnSave(Document document, ref bool ui, ref bool cancel)
        {
            if (document == null)
            {
                return;
            }

            if (ui == false)
            {
                return;
            }

            if (!Globals.IsProFis2Document(document))
            {
                _logger.LogInformation("Application Save not an Profis document");
                return;
            }

            _logger.LogInformation("Application Save it is an Profis document");

            ui = false;
            cancel = true;
            Upload(document);
        }

        private void UnsubscribeAllEvents()
        {
            ((ApplicationEvents4_Event)_application).Quit -= AppOnQuit;
            _application.DocumentBeforeSave -= OnSave;

            _application = null;
        }

        private bool Upload(Document document)
        {
            if (document == null)
            {
                return true;
            }

            var profiS2WordData = Globals.GetProfiS2Data(document);
            var result = WordHelper.GetDocxBinary(document);

            if (result.Success)
            {
                _uploadService.Upload(profiS2WordData, result.Docx);
                return true;
            }

            return false;
        }
    }
}