using Microsoft.Identity.Client;
using System.Threading.Tasks;

namespace dotnet_azure_fhir_web_api.IServices
{
    public interface IAccessTokenService
    {
        Task<AuthenticationResult> GetAuthenticationResult();
    }
}
