using System;

namespace BPSourceGen
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    sealed class BPCreationAttribute : Attribute
    {
        public string PropertyName { get; set; }

        public Type ReturnType { get; set; }

        public Type OwnerType { get; set; }

        public string DefaultValue { get; set; }

        public string DefaultBindingMode { get; set; }

        public string ValidadeValueMethodName { get; set; }

        public string PropertyChangedMethodName { get; set; }

        public string PropertyChangingMethodName { get; set; }

        public string CoerceValueMethodName { get; set; }

        public string DefaultValueCreatorMethodName { get; set; }

        public BPCreationAttribute()
        {
        }
    }
}


namespace BPSourceGen
{
    [BPCreation(DefaultValue = "2", OwnerType = typeof(B))]
    [BPCreation(DefaultValue = "2", OwnerType = typeof(B))]
    [BPCreation(DefaultValue = "2", OwnerType = typeof(B))]
    [BPCreation(DefaultValue = "2", OwnerType = typeof(B))]
    [BPCreation(DefaultValue = "2", OwnerType = typeof(B))]
    class B
    {

    }

    [BPCreation(DefaultValue = "2", OwnerType = typeof(B))]
    class C : B
    {

    }
}
