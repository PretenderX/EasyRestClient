using System.Reflection;
using EasyRestClient.Attributes;
using EasyRestClient.EndpointTemplate;
using RestSharp;

namespace EasyRestClient.Tools
{
    public class ResourceParameterInjector : IResourceParameterInjector
    {
        public ResourceParameterInjector()
        {
            IgnoreNull = true;
        }

        public bool IgnoreNull { get; set; }

        public virtual IRestRequest Inject<T>(T request, IRestRequest restRequest) where T : IEasyRestRequest
        {
            var requestType = typeof (T);
            var properties = requestType.GetProperties();

            foreach (var propertyInfo in properties)
            {
                var parameterTypeAttribute = propertyInfo.GetCustomAttribute<ParameterTypeAttribute>();

                if (parameterTypeAttribute != null && !parameterTypeAttribute.Ignored)
                {
                    var parameterNameAttribute = propertyInfo.GetCustomAttribute<ParameterNameAttribute>();
                    var parameterName = parameterNameAttribute != null
                        ? parameterNameAttribute.Value
                        : propertyInfo.Name;
                    var parameterValue = propertyInfo.GetValue(request, null);

                    if (IgnoreNull && parameterValue == null)
                    {
                        continue;
                    }

                    restRequest.AddParameter(parameterName, parameterValue, parameterTypeAttribute.Value);
                }
            }


            return restRequest;
        }
    }
}