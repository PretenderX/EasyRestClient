using EasyRestClient.EndpointTemplate;

namespace EasyRestClient
{
    public interface IEasyRestRequestProcessorFactory
    {
        IEasyRestRequestProcessor Create(IEasyRestRequest request);
    }
}
