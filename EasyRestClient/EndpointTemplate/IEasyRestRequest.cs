using RestSharp;

namespace EasyRestClient.EndpointTemplate
{
    public interface IEasyRestRequest
    {
        string Resource { get; }

        Method Method { get; }

        DataFormat RequestFormat { get; }
    }

    public interface IEasyRestRequest<T> : IEasyRestRequest where T : new()
    {
    }
}
