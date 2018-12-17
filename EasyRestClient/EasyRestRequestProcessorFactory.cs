using System;
using System.Collections.Generic;
using System.Linq;
using EasyRestClient.EndpointTemplate;

namespace EasyRestClient
{
    public class EasyRestRequestProcessorFactory : IEasyRestRequestProcessorFactory
    {
        #region static Feilds and Methods
        public static readonly Dictionary<Type, Type> RequestProcessorTypesMapping =
            new Dictionary<Type, Type>();

        public static void Register<TRequest, TRequestProcesser>()
            where TRequestProcesser : IEasyRestRequestProcessor
        {
            var processerType = typeof(TRequestProcesser);
            var processerKey = typeof(TRequest);

            AddProcesserToMapping(processerType, processerKey);
        }

        public static void Register<TRequestProcesser>()
            where TRequestProcesser : IEasyRestRequestProcessor
        {
            var processorType = typeof (TRequestProcesser);
            var processorKey = FindRequestType(processorType);

            if (processorKey == null)
            {
                throw new ArgumentException("Registering request processor not implemented generic IEasyRestRequestProcessor<TRequest> interface!");
            }

            AddProcesserToMapping(processorType, processorKey);
        }

        private static Type FindRequestType(Type processorType)
        {
            var interfaces = processorType.GetInterfaces().Where(t => t.IsGenericType).ToArray();
            var processorKey = interfaces.SelectMany(t => t.GetGenericArguments())
                .FirstOrDefault(t => typeof (IEasyRestRequest).IsAssignableFrom(t));

            return processorKey;
        }

        private static void AddProcesserToMapping(Type processerType, Type processerKey)
        {
            if (processerType != null && RequestProcessorTypesMapping.ContainsKey(processerKey))
            {
                return;
            }

            RequestProcessorTypesMapping.Add(processerKey, processerType);
        }
        #endregion

        private readonly IEasyRestRequestProcessorResolver _easyRestRequestProcessorResolver;

        public EasyRestRequestProcessorFactory(IEasyRestRequestProcessorResolver easyRestRequestProcessorResolver)
        {
            _easyRestRequestProcessorResolver = easyRestRequestProcessorResolver;
        }

        public IEasyRestRequestProcessor Create(IEasyRestRequest request)
        {
            var requestType = request.GetType();
            Type responseType = null;
            IEasyRestRequestProcessor requestProcessor = null;

            if (!RequestProcessorTypesMapping.TryGetValue(requestType, out var requestProcessorType))
            {
                if (requestType.BaseType != null && requestType.BaseType.IsGenericType)
                {
                    responseType = requestType.BaseType.GetGenericArguments()[0];
                }

                requestProcessorType = responseType == null
                    ? typeof(IEasyRestRequestProcessor<>).MakeGenericType(requestType)
                    : typeof(IEasyRestRequestProcessor<,>).MakeGenericType(requestType, responseType);
            }

            requestProcessor = _easyRestRequestProcessorResolver.Resolve(requestProcessorType);

            if (requestProcessor == null)
            {
                throw new ApplicationException("Can't find request processor for request type: " + requestType.FullName);
            }

            return requestProcessor;
        }
    }
}
