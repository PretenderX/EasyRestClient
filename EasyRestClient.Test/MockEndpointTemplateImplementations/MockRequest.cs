using EasyRestClient.EndpointTemplate;
using RestSharp;

namespace EasyRestClient.Test.MockEndpointTemplateImplementations
{
    public class MockRequest : IEasyRestRequest<MockResponse>
    {
        public string Resource => "";
        public Method Method => Method.GET;
        public DataFormat RequestFormat => DataFormat.Json;
        public string Accept => "application/josn";
    }
}