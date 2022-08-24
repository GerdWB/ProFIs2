namespace ProFiS2.WordAddIn.Services
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Model;

    internal interface IUploadService
    {
        bool Upload(ProfiS2WordData profiS2WordData, byte[] docx);
    }

    internal class UploadService : IUploadService
    {
        private readonly ILogger<UploadService> _logger;
        private readonly IMessageService _messageService;

        public UploadService(ILogger<UploadService> logger, IMessageService messageService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
        }


        public bool Upload(ProfiS2WordData profiS2WordData, byte[] docx)
        {
            try
            {
                var data = new ProFis2Data
                {
                    DocKey = profiS2WordData.Settings.DocKey,
                    InstId = profiS2WordData.Settings.InstId,
                    TpyeId = profiS2WordData.Settings.TypeId,
                    CatId = profiS2WordData.Settings.CatId,
                    DocumentName = profiS2WordData.Settings.DocumentName,
                    Date = DateTime.UtcNow.ToString("DD.MM.YYYY hh:mm:ss"),
                    Document = docx
                };

                var jsonString = JsonSerializer.Serialize(data);

                var client = new HttpClient
                {
                    BaseAddress = new Uri(profiS2WordData.Settings.RestUrl),
                    Timeout = default,
                    MaxResponseContentBufferSize = 0
                };
                //client.BaseAddress = new Uri("https://localhost:7250/ProFis2");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var task = Task.Run(() => client.PostAsJsonAsync("", data));
                task.Wait();
                var response = task.Result;

                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Could not upload document");
                _messageService.ShowCriticalMessage("Could not upload document");
                return false;
            }
        }
    }
}