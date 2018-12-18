using EasyRestClient.Tools;
using RestSharp;

namespace EasyRestClient.EndpointTemplate
{
    public abstract class EasyRestRequestProcessorBase : IEasyRestRequestProcessor
    {
        protected IEasyRestRequest CurrentRequest;

        public virtual void OnRestClientInitialized(IRestClient restClient)
        {
        }

        public virtual IRestRequest BuildRestRequest(IEasyRestRequest request)
        {
            CurrentRequest = request;

            IRestRequest restRequest = new RestRequest(request.Resource, request.Method)
            {
                RequestFormat = request.RequestFormat
            };

            return restRequest;
        }

        public virtual IRestRequest BuildRestRequest<T>(IEasyRestRequest<T> request) where T : new()
        {
            return BuildRestRequest((IEasyRestRequest)request);
        }

        public virtual void RestResponseHandler(IRestResponse response)
        {
        }
    }

    public abstract class EasyRestRequestProcessorBase<TRequest> : EasyRestRequestProcessorBase, IEasyRestRequestProcessor<TRequest>
        where TRequest : IEasyRestRequest
    {
        protected TRequest CurrentTRequest;
        private IResourceParameterInjector _resourceParameterInjector;

        public virtual IResourceParameterInjector ResourceParameterInjector
        {
            get => _resourceParameterInjector ?? (_resourceParameterInjector = new ResourceParameterInjector());
            set => _resourceParameterInjector = value;
        }

        public sealed override IRestRequest BuildRestRequest(IEasyRestRequest request)
        {
            var restRequest = base.BuildRestRequest(request);

            restRequest = AppendRequestDataToRestRequest(CurrentTRequest, restRequest);
            restRequest = ResourceParameterInjector.Inject(CurrentTRequest, restRequest);

            return restRequest;
        }

        public sealed override IRestRequest BuildRestRequest<T>(IEasyRestRequest<T> request)
        {
            CurrentTRequest = (TRequest)request;

            return base.BuildRestRequest(request);
        }

        public virtual IRestRequest AppendRequestDataToRestRequest(TRequest request, IRestRequest restRequest)
        {
            return restRequest;
        }
    }

    public abstract class EasyRestRequestProcessorBase<TRequest, TResponse> : EasyRestRequestProcessorBase<TRequest>, IEasyRestRequestProcessor<TRequest, TResponse>
        where TRequest : IEasyRestRequest<TResponse>
        where TResponse : new()
    {
        public sealed override void RestResponseHandler(IRestResponse response)
        {
            RestResponseHandler((IRestResponse<TResponse>)response, CurrentTRequest);
        }

        public virtual void RestResponseHandler(IRestResponse<TResponse> response, TRequest request)
        {
        }
    }
}
