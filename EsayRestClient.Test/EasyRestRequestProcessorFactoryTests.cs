using EasyRestClient;
using EsayRestClient.Test.MockEndpointTemplateImplementations;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EsayRestClient.Test
{
    [TestClass]
    public class EasyRestRequestProcessorFactoryTests
    {
        [TestMethod]
        public void AutoRegisterAllProcessors_Should_RegisterAllGenericIEasyRestRequestProcessorImplementations()
        {
            // Arrange
            var expectedKey = typeof (MockRequest);
            var expectedValue = typeof (MockRequestProcessor);

            // Act
            EasyRestRequestProcessorFactory.AutoRegisterAllProcessors();

            // Assert
            EasyRestRequestProcessorFactory.RequestProcessorTypesMapping.Should().ContainKey(expectedKey);
            EasyRestRequestProcessorFactory.RequestProcessorTypesMapping[expectedKey].Should().Be(expectedValue);
        }
    }
}
