using RestSharp;

namespace EasyRestClient.EndpointTemplate
{
    public interface IEasyRestRequestProcessor
    {
        IRestRequest BuildRestRequest(IEasyRestRequest request);

        IRestRequest BuildRestRequest<T>(IEasyRestRequest<T> request) where T : new();

        void RestResponseHandler(IRestResponse response);
    }

    public interface IEasyRestRequestProcessor<in TRequest> : IEasyRestRequestProcessor
        where TRequest : IEasyRestRequest
    {
        IRestRequest AppendRequestDataToRestRequest(TRequest request, IRestRequest restRequest);
    }

    public interface IEasyRestRequestProcessor<in TRequest, TResponse> : IEasyRestRequestProcessor<TRequest>
        where TRequest : IEasyRestRequest<TResponse>
        where TResponse : new()
    {
        void RestResponseHandler(IRestResponse<TResponse> response, TRequest request);
    }
}
