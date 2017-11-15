using EasyRestClient.EndpointTemplate;
using RestSharp;

namespace EasyRestClient
{
    public interface IEsayRestClient
    {
        IRestResponse Execute(IEasyRestRequest request);

        T Execute<T>(IEasyRestRequest<T> request) where T : new();
    }
}
