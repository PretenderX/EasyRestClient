using EasyRestClient.EndpointTemplate;
using RestSharp;

namespace EasyRestClient.Tools
{
    public interface IResourceParameterInjector
    {
        bool IgnoreNull { get; set; }

        IRestRequest Inject<T>(T request, IRestRequest restRequest) where T : IEasyRestRequest;
    }
}