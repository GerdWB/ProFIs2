namespace ProFiS2.WordAddIn
{
    using System;
    using System.Runtime.ExceptionServices;
    using Serilog;

    public class GlobalExceptionHandling
    {
        private readonly ILogger _logger;

        public GlobalExceptionHandling(ILogger logger, bool logChanceException)
        {
            _logger = logger;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            if (logChanceException)
            {
                AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
            }
        }

        private void CurrentDomain_FirstChanceException(object sender, FirstChanceExceptionEventArgs e)
        {
            _logger.Error(e.Exception, "First Chance Exception");
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _logger.Error((Exception)e.ExceptionObject, "Unhandled Exception");
        }
    }
}