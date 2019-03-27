using SampleChatbot.Models;
using System.Threading.Tasks;

namespace SampleChatbot.Services.Health
{
    public interface IChatbotHealthChecker
    {
        Task<HealthResult> CheckHealth();
    }
}
