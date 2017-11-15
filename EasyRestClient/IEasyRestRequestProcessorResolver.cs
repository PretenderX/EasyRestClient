using System;
using EasyRestClient.EndpointTemplate;

namespace EasyRestClient
{
    public interface IEasyRestRequestProcessorResolver
    {
        IEasyRestRequestProcessor Resolve(Type processorType);
    }
}