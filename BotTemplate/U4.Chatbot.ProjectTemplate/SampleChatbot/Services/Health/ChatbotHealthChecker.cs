using $ext_safeprojectname$.Models;
using System.Threading.Tasks;

namespace $ext_safeprojectname$.Services.Health
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
