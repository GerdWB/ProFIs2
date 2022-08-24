namespace ProFiS2.WordAddIn.Services
{
    internal interface IMessageService
    {
        void ShowCriticalMessage(string text);

        void ShowInfoMessage(string text);
    }
}