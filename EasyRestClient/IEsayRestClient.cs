using EasyRestClient.EndpointTemplate;
using RestSharp;

namespace EasyRestClient
{
    public interface IEasyRestClient
    {
        IRestResponse Execute(IEasyRestRequest request);

        T Execute<T>(IEasyRestRequest<T> request) where T : new();
    }
}
