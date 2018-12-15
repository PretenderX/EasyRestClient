using EasyRestClient.Attributes;
using EasyRestClient.EndpointTemplate;
using EasyRestClient.Tools;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;

namespace EasyRestClient.Test.Tools
{
    public class TestRequest : IEasyRestRequest
    {
        [ParameterType(ParameterType.UrlSegment)]
        public string ProjectId { get; set; }

        [ParameterType(ParameterType.QueryString)]
        [ParameterName("api-version")]
        public string ApiVersion { get; set; }

        [ParameterType(ParameterType.UrlSegment)]
        public int Id { get; set; }

        [ParameterType(ParameterType.HttpHeader)]
        [ParameterName("Accept")]
        public string AcceptHeader { get; set; }

        [ParameterType(ParameterType.QueryString)]
        public string NullValue { get; set; }

        [ParameterType(ParameterType.UrlSegment, true)]
        public string IgnoredValue { get; set; }

        [ParameterType(ParameterType.Cookie)]
        public string MyCookie { get; set; }

        [ParameterType(ParameterType.GetOrPost)]
        public string GetOrPost { get; set; }

        [ParameterType(ParameterType.RequestBody)]
        public object Body { get; set; }

        public string Resource => "somewhere/something";

        public Method Method => Method.GET;

        public DataFormat RequestFormat => DataFormat.Json;
    }

    [TestClass]
    public class ResourceParameterInjectorTest
    {
        private ResourceParameterInjector resourceParameterInjector;

        [TestInitialize]
        public void Setup()
        {
            resourceParameterInjector = new ResourceParameterInjector();
        }

        [TestMethod]
        public void Inject_Should_BeAbleToAddPamameterToRestRequestByParameterTypeAttributeAndParameterNameAttribute()
        {
            // Arrange
            var mockRequest = new TestRequest
            {
                ProjectId = "LDS",
                ApiVersion = "2.0",
                Id = 3516,
                AcceptHeader = "application/json",
                NullValue = null,
                IgnoredValue = "YouShouldNotFoundMe",
                MyCookie = "PretenderX",
                GetOrPost = "LOL",
                Body = new {FirstName = "Lionel", LastName = "Wu"}
            };
            var mockRestRequest = new RestRequest("{ProjectId}/_apis/tfvc/changesets/{Id}?api-version={apiVersion}");

            // Act
            var result = resourceParameterInjector.Inject(mockRequest, mockRestRequest);

            // Assert
            result.Parameters.Should().Contain(p => p.Type == ParameterType.UrlSegment &&
                                                    p.Name == nameof(mockRequest.ProjectId) &&
                                                    p.Value.Equals(mockRequest.ProjectId));
            result.Parameters.Should().Contain(p => p.Type == ParameterType.QueryString &&
                                                    p.Name == "api-version" &&
                                                    p.Value.Equals(mockRequest.ApiVersion));
            result.Parameters.Should().Contain(p => p.Type == ParameterType.UrlSegment &&
                                                    p.Name == nameof(mockRequest.Id) &&
                                                    p.Value.Equals(mockRequest.Id));
            result.Parameters.Should().Contain(p => p.Type == ParameterType.HttpHeader &&
                                                    p.Name == "Accept" &&
                                                    p.Value.Equals(mockRequest.AcceptHeader));
            result.Parameters.Should().NotContain(p => p.Type == ParameterType.QueryString &&
                                                       p.Name == nameof(mockRequest.NullValue));
            result.Parameters.Should().NotContain(p => p.Type == ParameterType.UrlSegment &&
                                                       p.Name == nameof(mockRequest.IgnoredValue));
            result.Parameters.Should().Contain(p => p.Type == ParameterType.Cookie &&
                                                    p.Name == nameof(mockRequest.MyCookie) &&
                                                    p.Value.Equals(mockRequest.MyCookie));
            result.Parameters.Should().Contain(p => p.Type == ParameterType.GetOrPost &&
                                                    p.Name == nameof(mockRequest.GetOrPost) &&
                                                    p.Value.Equals(mockRequest.GetOrPost));
            result.Parameters.Should().Contain(p => p.Type == ParameterType.RequestBody &&
                                                    p.Name == "application/json" &&
                                                    p.Value.Equals("{\"FirstName\":\"Lionel\",\"LastName\":\"Wu\"}"));
        }
    }
}
