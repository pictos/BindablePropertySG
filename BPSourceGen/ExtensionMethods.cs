using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BPSourceGen
{
    static class ExtensionMethods
    {
        public static TypedConstant GetAttributeValueByName(this AttributeData attribute, string name)
        {
            return attribute.NamedArguments.SingleOrDefault(kvp => kvp.Key == name).Value;
        }

        public static string GetAttributeValueByNameAsString(this AttributeData attribute, string name, string placeholder = "null")
        {
            var data = attribute.NamedArguments.SingleOrDefault(kvp => kvp.Key == name).Value;

            return data.Value is null ? placeholder : (string)data.Value;
        }
    }
}
