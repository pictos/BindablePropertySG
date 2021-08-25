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
    }
}
