using System;

namespace EasyRestClient.Attributes
{
    public class ParameterNameAttribute : Attribute
    {
        public string Value { get; set; }

        public ParameterNameAttribute(string parameterName)
        {
            Value = parameterName;
        }
    }
}