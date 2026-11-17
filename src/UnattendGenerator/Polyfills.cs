// Polyfills for C# 9-13 / .NET 5+ compiler-recognized attributes on .NET Framework 4.8

namespace System.Runtime.CompilerServices
{
  internal static class IsExternalInit { }

  [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
  internal sealed class CompilerFeatureRequiredAttribute(string featureName) : Attribute
  {
    public string FeatureName { get; } = featureName;
    public bool IsOptional { get; init; }
  }

  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
  internal sealed class RequiredMemberAttribute : Attribute { }

  [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
  internal sealed class CallerArgumentExpressionAttribute(string parameterName) : Attribute
  {
    public string ParameterName { get; } = parameterName;
  }
}

namespace System.Diagnostics.CodeAnalysis
{
  [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, Inherited = false)]
  internal sealed class NotNullAttribute : Attribute { }

  [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
  internal sealed class NotNullWhenAttribute(bool returnValue) : Attribute
  {
    public bool ReturnValue { get; } = returnValue;
  }

  [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
  internal sealed class MaybeNullWhenAttribute(bool returnValue) : Attribute
  {
    public bool ReturnValue { get; } = returnValue;
  }

  [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, Inherited = false)]
  internal sealed class MaybeNullAttribute : Attribute { }

  [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property, Inherited = false)]
  internal sealed class AllowNullAttribute : Attribute { }

  [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property, Inherited = false)]
  internal sealed class DisallowNullAttribute : Attribute { }

  [AttributeUsage(AttributeTargets.Method, Inherited = false)]
  internal sealed class DoesNotReturnAttribute : Attribute { }
}
