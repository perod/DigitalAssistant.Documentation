using U4.Bot.Builder.Interfaces;

namespace SampleChatbot.Services.BusinessLogic
{
    internal class BusinessLogicService : IBusinessLogicService
    {
        private readonly IBotLogger _botLogger;
        private readonly int _luckyNumber;

        public BusinessLogicService(IBotLogger botLogger)
        {
            _botLogger = botLogger; //Include core logging service.
            _luckyNumber = 4; //As in Unit4.
        }

        public bool IsLuckyNumber(int number)
        {
            var isLucky = number == _luckyNumber;

            var logMsg = isLucky
                ? "The user guessed the correct number."
                : "The user guessed the wrong number.";

            _botLogger.Information(logMsg);

            return isLucky;
        }
    }
}
