using Office = Microsoft.Office.Core;

namespace ProFiS2.WordAddIn
{
    using System;
    using Configuration;
    using Events;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;

    public partial class ThisAddIn
    {
        private static void CreateLogger(IConfigurationRoot configurationRoot)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configurationRoot)
                .CreateLogger();
        }

        private static IConfigurationRoot GetConfiguration()
        {
            var configuration = new ConfigurationBuilder();
            configuration.AddJsonFile("profis2.json", false, false);
            var configurationRoot = configuration.Build();
            return configurationRoot;
        }

        public IServiceProvider ServiceProvider { get; set; }

        private void ConfigureServices(IConfigurationRoot configurationRoot)
        {
            // ReSharper disable once ObjectCreationAsStatement
            new GlobalExceptionHandling(Log.Logger, true);

            var proFiS2Configuration = configurationRoot.Get<ProFiS2Configuration>();

            var services = new ServiceCollection();
            services.AddSingleton(Application);
            services.AddSingleton<IWordEventProxy, WordEventProxy>();
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
            ServiceProvider = services.BuildServiceProvider();
            ServiceProvider.GetService<IWordEventProxy>();

            Log.Logger.Information("AddIn started and configured");
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
            var configurationRoot = GetConfiguration();

            CreateLogger(configurationRoot);
            ConfigureServices(configurationRoot);
        }
    }
}