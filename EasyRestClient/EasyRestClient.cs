﻿using System;
using System.Net;
using EasyRestClient.Configuration;
using EasyRestClient.EndpointTemplate;
using EasyRestClient.Exception;
using RestSharp;

namespace EasyRestClient
{
    public class EasyRestClient : IEasyRestClient
    {
        private readonly IEasyRestClientConfiguration _configuration;
        private readonly IEasyRestRequestProcessorFactory _requestProcessorFactory;
        private readonly IRestClient _restClient;

        public EasyRestClient(IEasyRestClientConfiguration configuration,
            IEasyRestRequestProcessorFactory requestProcessorFactory,
            IRestClient restClient)
        {
            _configuration = configuration;
            _requestProcessorFactory = requestProcessorFactory;
            _restClient = restClient;
        }

        public IRestResponse Execute(IEasyRestRequest request)
        {
            SetupRestClient();

            var requestProcessor = _requestProcessorFactory.Create(request);
            var restRequest = requestProcessor.BuildRestRequest(request);

            var response = _restClient.Execute(restRequest);

            DefualtResponseHandler(response);
            requestProcessor.RestResponseHandler(response);

            return response;
        }

        public T Execute<T>(IEasyRestRequest<T> request) where T : new()
        {
            SetupRestClient();

            var requestProcessor = _requestProcessorFactory.Create(request);
            var restRequest = requestProcessor.BuildRestRequest(request);

            var response = _restClient.Execute<T>(restRequest);

            DefualtResponseHandler(response);
            requestProcessor.RestResponseHandler(response);

            return response.Data;
        }

        private static void DefualtResponseHandler(IRestResponse response)
        {
            if (response == null)
            {
                throw new EasyRestException("Response is null for unknow reason.");
            }

            if (response.ErrorException != null)
            {
                throw new EasyRestException(response.ErrorMessage, response.ErrorException);
            }

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return;
                case HttpStatusCode.NotFound:
                    throw new EasyRestException(string.Format("Not found endpoint: {0}",
                        response.Request.Resource));
                case HttpStatusCode.Forbidden:
                    throw new EasyRestException(string.Format("Not authorized for endpoint: {0}",
                        response.Request.Resource));
            }
        }

        private void SetupRestClient()
        {
            _restClient.BaseUrl = new Uri(_configuration.BaseUrl);
            _restClient.Authenticator = _configuration.Authenticator;
        }
    }
}
