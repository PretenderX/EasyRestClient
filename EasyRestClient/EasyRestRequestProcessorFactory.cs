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

        public static bool AutoRegistered;

        /// <summary>
        /// Calling this method will find all generic IEasyRestRequestProcessor<TRequest> implementations and register them automatically.
        /// </summary>
        public static void AutoRegisterAllProcessors()
        {
            if (AutoRegistered)
            {
                return;
            }

            var processorTypes = Utility.FindImplementationClasses<IEasyRestRequestProcessor>();

            foreach (var processorType in processorTypes)
            {
                var processorKey = FindRequestType(processorType);

                if (processorKey == null)
                {
                    continue;
                }

                AddProcesserToMapping(processorType, processorKey);
            }

            AutoRegistered = true;
        }

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

            AutoRegisterAllProcessors();
        }

        public IEasyRestRequestProcessor Create(IEasyRestRequest request)
        {
            var requestType = request.GetType();
            var processorKey = RequestProcessorTypesMapping.Keys
                .FirstOrDefault(type => type.IsAssignableFrom(requestType));

            if (processorKey != null)
            {
                var processorType = RequestProcessorTypesMapping[processorKey];
                return _easyRestRequestProcessorResolver.Resolve(processorType);
            }

            throw new ApplicationException("Can't find request processor for request type: " + requestType.FullName);
        }
    }
}
