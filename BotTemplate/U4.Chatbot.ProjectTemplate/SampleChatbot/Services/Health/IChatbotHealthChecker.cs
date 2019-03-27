using $ext_safeprojectname$.Models;
using System.Threading.Tasks;

namespace $ext_safeprojectname$.Services.Health
{
    public interface IChatbotHealthChecker
    {
        Task<HealthResult> CheckHealth();
    }
}
