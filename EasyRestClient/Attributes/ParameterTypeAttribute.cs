using System;
using RestSharp;

namespace EasyRestClient.Attributes
{
    public class ParameterTypeAttribute : Attribute
    {
        public ParameterType Value { get; set; }

        public bool Ignored { get; set; }

        public ParameterTypeAttribute(ParameterType parameterType, bool ignored = false)
        {
            Value = parameterType;
            Ignored = ignored;
        }
    }
}