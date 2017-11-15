using System;
using EasyRestClient.EndpointTemplate;

namespace EasyRestClient
{
    public class DefaultRequestProcessorResolver : IEasyRestRequestProcessorResolver
    {
        public virtual IEasyRestRequestProcessor Resolve(Type processorType)
        {
            if (processorType == null)
            {
                throw new ArgumentNullException(nameof(processorType));
            }

            if (processorType.GetConstructor(Type.EmptyTypes) == null)
            {
                throw new ArgumentException("processorType has no parameterless constructor! Consider to override Resolve method or implement your own IEasyRestRequestProcessorResolver.");
            }

            var returnType = typeof(IEasyRestRequestProcessor);
            if (!returnType.IsAssignableFrom(processorType))
            {
                throw new ArgumentException($"{returnType.FullName} is not assignable from {processorType.FullName}!");
            }

            return Activator.CreateInstance(processorType) as IEasyRestRequestProcessor;
        }
    }
}