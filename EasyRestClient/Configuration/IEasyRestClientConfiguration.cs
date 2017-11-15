using RestSharp.Authenticators;

namespace EasyRestClient.Configuration
{
    public interface IEasyRestClientConfiguration
    {
        string BaseUrl { get; }

        IAuthenticator Authenticator { get; }
    }
}
