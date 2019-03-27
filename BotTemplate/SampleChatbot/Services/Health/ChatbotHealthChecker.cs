using SampleChatbot.Models;
using System.Threading.Tasks;

namespace SampleChatbot.Services.Health
{
    internal class ChatbotHealthChecker : IChatbotHealthChecker
    {
        public Task<HealthResult> CheckHealth()
        {
            //Todo: verify health of chatbot
            return Task.FromResult(new HealthResult { Status = HealthStatus.Ok, Message = $"{Constants.ChatbotId} is healthy." });
        }
    }
}
