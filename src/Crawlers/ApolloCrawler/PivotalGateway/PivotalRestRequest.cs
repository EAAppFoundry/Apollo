﻿using RestSharp;

namespace Apollo.PivotalGateway
{
    public class PivotalRestRequest : RestRequest
    {
        private const string TOKEN = "c98d6a3d817c0fd41119ea20f4dadae3"; //"fa48813f1a201b56b55871cdb7448cb4";
        public PivotalRestRequest()
        {
            //DateFormat = @"yyyy/MM/dd HH:mm:ss EDT";
            AddParameter("X-TrackerToken", TOKEN, ParameterType.HttpHeader);
        }

        public PivotalRestRequest(string resource)
            : base(resource)
        {
            //DateFormat = @"yyyy/MM/dd HH:mm:ss EDT";
            AddParameter("X-TrackerToken", TOKEN, ParameterType.HttpHeader);
        }

        public PivotalRestRequest(string resource, Method method)
            : base(resource, method)
        {
            //DateFormat = @"yyyy/MM/dd HH:mm:ss EDT";
            AddParameter("X-TrackerToken", TOKEN, ParameterType.HttpHeader);
        }
    }
}